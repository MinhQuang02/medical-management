using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MedicalDataManagement.AdminModule;

public class DatabaseService
{
    private const string ConnectionString = "User Id=sys;Password=1234567890;Data Source=localhost:1521/xe;DBA Privilege=SYSDBA;";

    private OracleConnection GetConnection()
    {
        OracleConnection conn = new OracleConnection(ConnectionString);
        conn.Open();
        
        using (var cmd = new OracleCommand("ALTER SESSION SET \"_ORACLE_SCRIPT\" = true", conn))
        {
            cmd.ExecuteNonQuery();
        }
        
        return conn;
    }

    public DataTable ExecuteQuery(string query, OracleParameter[]? parameters = null)
    {
        using (var conn = GetConnection())
        {
            using (var cmd = new OracleCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }

    public void ExecuteNonQuery(string query, OracleParameter[]? parameters = null)
    {
        using (var conn = GetConnection())
        {
            using (var cmd = new OracleCommand(query, conn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // --- User & Role Management with Search ---

    public DataTable GetAllUsers(string keyword = "")
    {
        string filter = string.IsNullOrEmpty(keyword) ? "" : "WHERE UPPER(USERNAME) LIKE :keyword";
        string query = $@"SELECT USERNAME, 
                               ACCOUNT_STATUS AS STATUS, 
                               CREATED, 
                               DECODE(ORACLE_MAINTAINED, 'Y', 'System', 'User') AS TYPE 
                        FROM DBA_USERS 
                        {filter} 
                        ORDER BY TYPE DESC, USERNAME ASC";
        
        OracleParameter[]? p = string.IsNullOrEmpty(keyword) ? null : new[] { new OracleParameter("keyword", $"%{keyword.ToUpper()}%") };
        return ExecuteQuery(query, p);
    }

    public DataTable GetAllRoles(string keyword = "")
    {
        string filter = string.IsNullOrEmpty(keyword) ? "" : "WHERE UPPER(ROLE) LIKE :keyword";
        string query = $@"SELECT ROLE, 
                               ROLE_ID, 
                               DECODE(ORACLE_MAINTAINED, 'Y', 'System', 'User') AS TYPE 
                        FROM DBA_ROLES 
                        {filter} 
                        ORDER BY TYPE DESC, ROLE ASC";
        
        OracleParameter[]? p = string.IsNullOrEmpty(keyword) ? null : new[] { new OracleParameter("keyword", $"%{keyword.ToUpper()}%") };
        return ExecuteQuery(query, p);
    }

    public void CreateUser(string username, string password) => ExecuteNonQuery($"CREATE USER {username} IDENTIFIED BY \"{password}\"");
    public void DropUser(string username) => ExecuteNonQuery($"DROP USER {username} CASCADE");
    public void AlterUserPassword(string username, string newPassword) => ExecuteNonQuery($"ALTER USER {username} IDENTIFIED BY \"{newPassword}\"");
    public void CreateRole(string roleName) => ExecuteNonQuery($"CREATE ROLE {roleName}");
    public void DropRole(string roleName) => ExecuteNonQuery($"DROP ROLE {roleName}");

    // --- Privilege Management ---

    public DataTable GetObjects(string objectType, bool hideSystem = true)
    {
        string filter = hideSystem ? "AND ORACLE_MAINTAINED = 'N'" : "";
        OracleParameter[] parameters = { new OracleParameter("objType", objectType) };
        
        return ExecuteQuery($@"SELECT OBJECT_NAME, OWNER, DECODE(ORACLE_MAINTAINED, 'Y', 'System', 'User') AS TYPE
                             FROM ALL_OBJECTS 
                             WHERE OBJECT_TYPE = :objType 
                             {filter}
                             AND OWNER NOT IN ('XDB', 'OUTLN', 'MDSYS')
                             ORDER BY TYPE DESC, OBJECT_NAME", parameters);
    }

    public DataTable GetTableColumns(string tableName)
    {
        OracleParameter[] parameters = { new OracleParameter("tableName", tableName) };
        return ExecuteQuery("SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = :tableName ORDER BY COLUMN_ID", parameters);
    }

    public void GrantPrivilege(string grantee, string privilege, string? objectName, bool withGrantOption, List<string>? columns = null)
    {
        string sql = "";
        string grantOpt = withGrantOption ? (IsRole(privilege) ? " WITH ADMIN OPTION" : " WITH GRANT OPTION") : "";
        bool isObjectPriv = !string.IsNullOrEmpty(objectName) && IsObjectPrivilege(privilege);

        if (isObjectPriv)
        {
            if (columns != null && columns.Count > 0 && privilege.ToUpper() == "UPDATE")
            {
                string cols = string.Join(", ", columns);
                sql = $"GRANT {privilege}({cols}) ON {objectName} TO {grantee}{grantOpt}";
            }
            else sql = $"GRANT {privilege} ON {objectName} TO {grantee}{grantOpt}";
        }
        else sql = $"GRANT {privilege} TO {grantee}{grantOpt}";
        
        ExecuteNonQuery(sql);
    }

    private bool IsObjectPrivilege(string priv)
    {
        string p = priv.ToUpper();
        return p == "SELECT" || p == "INSERT" || p == "UPDATE" || p == "DELETE" || p == "EXECUTE" || p == "ALL";
    }

    private bool IsRole(string name)
    {
        DataTable dt = ExecuteQuery("SELECT 1 FROM DBA_ROLES WHERE ROLE = UPPER(:name)", new[] { new OracleParameter("name", name) });
        return dt.Rows.Count > 0;
    }

    public void RevokePrivilege(string grantee, string privilege, string? objectName)
    {
        string sql = string.IsNullOrEmpty(objectName) || !IsObjectPrivilege(privilege)
            ? $"REVOKE {privilege} FROM {grantee}"
            : $"REVOKE {privilege} ON {objectName} FROM {grantee}";
        ExecuteNonQuery(sql);
    }

    public DataTable GetUserOrRolePrivileges(string name)
    {
        string sql = @"
            SELECT 'System' AS TYPE, PRIVILEGE AS PRIVILEGE, NULL AS OBJECT_NAME, ADMIN_OPTION AS WITH_GRANT 
            FROM DBA_SYS_PRIVS WHERE GRANTEE = UPPER(:name)
            UNION ALL
            SELECT 'Role' AS TYPE, GRANTED_ROLE AS PRIVILEGE, NULL AS OBJECT_NAME, ADMIN_OPTION AS WITH_GRANT 
            FROM DBA_ROLE_PRIVS WHERE GRANTEE = UPPER(:name)
            UNION ALL
            SELECT 'Object' AS TYPE, PRIVILEGE AS PRIVILEGE, TABLE_NAME AS OBJECT_NAME, GRANTABLE AS WITH_GRANT 
            FROM DBA_TAB_PRIVS WHERE GRANTEE = UPPER(:name)
            UNION ALL
            SELECT 'Column' AS TYPE, PRIVILEGE || '(' || COLUMN_NAME || ')' AS PRIVILEGE, TABLE_NAME AS OBJECT_NAME, GRANTABLE AS WITH_GRANT 
            FROM DBA_COL_PRIVS WHERE GRANTEE = UPPER(:name)";
        
        return ExecuteQuery(sql, new[] { new OracleParameter("name", name) });
    }
}
