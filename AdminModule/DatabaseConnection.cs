using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MedicalDataManagement.AdminModule;

public static class DatabaseConnection
{
    // Cú pháp kết nối C# với Oracle thông qua thông tin bạn vừa cung cấp (kết nối dạng SYSDBA)
    private static readonly string ConnectionString = "User Id=sys;Password=1234567890;Data Source=localhost:1521/XE;DBA Privilege=SYSDBA;";

    public static string TestConnection()
    {
        try
        {
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                // Mở kết nối
                conn.Open();
                
                if (conn.State == ConnectionState.Open)
                {
                    // Lấy thông tin phiên bản từ CSDL như một câu lệnh test
                    string sqlTest = "SELECT banner FROM v$version WHERE rownum = 1";
                    using (OracleCommand cmd = new OracleCommand(sqlTest, conn))
                    {
                        object versionInfo = cmd.ExecuteScalar();
                        return $"[Kết nối thành công]\nĐã kết nối tài khoản 'sys' quyền sysdba!\nThông tin DB: {versionInfo}";
                    }
                }
                return "[Lỗi] Không thể mở kết nối tới database.";
            }
        }
        catch (Exception ex)
        {
            return $"[Lỗi kết nối CSDL]\n{ex.Message}";
        }
    }
}
