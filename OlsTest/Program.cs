using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

/// <summary>
/// Comprehensive OLS Diagnostic, Enable, Setup, and Verification Tool.
/// Handles all steps from enabling OLS to inserting test data.
/// </summary>
class OlsSetupTool
{
    // Connection strings
    static readonly string CDB_CONN = "User Id=sys;Password=1234567890;Data Source=localhost:1521/xe;DBA Privilege=SYSDBA;Connection Timeout=10;";
    static readonly string PDB_CONN = "User Id=sys;Password=1234567890;Data Source=localhost:1521/xepdb1;DBA Privilege=SYSDBA;Connection Timeout=10;";

    static int successCount = 0;
    static int skipCount = 0;
    static int errorCount = 0;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║    OLS SETUP TOOL - Oracle Label Security for THONGBAO  ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝\n");

        // ═══════════════ PHASE 1: DIAGNOSTIC ═══════════════
        Console.WriteLine("═══ PHASE 1: DIAGNOSTIC ═══");
        bool olsEnabled = RunDiagnostic();

        if (!olsEnabled)
        {
            // ═══════════════ PHASE 2: ENABLE OLS ═══════════════
            Console.WriteLine("\n═══ PHASE 2: ENABLE OLS ═══");
            olsEnabled = TryEnableOls();
        }

        if (!olsEnabled)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  ⚠️  OLS IS NOT ENABLED - MANUAL STEPS REQUIRED         ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  Run these commands in SQL*Plus as SYSDBA:               ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  -- Connect to PDB:                                      ║");
            Console.WriteLine("║  sqlplus sys/1234567890@localhost:1521/xepdb1 as sysdba   ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  -- Step 1: Configure OLS                                ║");
            Console.WriteLine("║  EXEC LBACSYS.CONFIGURE_OLS;                             ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  -- Step 2: Enable OLS                                   ║");
            Console.WriteLine("║  EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;                ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  -- Step 3: Restart PDB                                  ║");
            Console.WriteLine("║  ALTER PLUGGABLE DATABASE xepdb1 CLOSE IMMEDIATE;        ║");
            Console.WriteLine("║  ALTER PLUGGABLE DATABASE xepdb1 OPEN;                   ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("║  -- Step 4: Re-run this tool                             ║");
            Console.WriteLine("║  dotnet run --project OlsTest                            ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            return;
        }

        // ═══════════════ PHASE 3: CLEANUP ═══════════════
        Console.WriteLine("\n═══ PHASE 3: CLEANUP OLD OBJECTS ═══");
        CleanupOldObjects();

        // ═══════════════ PHASE 4: CREATE POLICY ═══════════════
        Console.WriteLine("\n═══ PHASE 4: CREATE OLS POLICY ═══");
        CreatePolicy();

        // ═══════════════ PHASE 5: CREATE TABLE ═══════════════
        Console.WriteLine("\n═══ PHASE 5: CREATE TABLE & APPLY POLICY ═══");
        CreateTableAndApplyPolicy();

        // ═══════════════ PHASE 6: CREATE USERS ═══════════════
        Console.WriteLine("\n═══ PHASE 6: CREATE USERS (u1-u8) ═══");
        CreateUsers();

        // ═══════════════ PHASE 7: SET USER LABELS ═══════════════
        Console.WriteLine("\n═══ PHASE 7: SET USER LABELS ═══");
        SetUserLabels();

        // ═══════════════ PHASE 8: INSERT DATA ═══════════════
        Console.WriteLine("\n═══ PHASE 8: INSERT NOTIFICATION DATA (t1-t7) ═══");
        InsertData();

        // ═══════════════ PHASE 9: VERIFY ═══════════════
        Console.WriteLine("\n═══ PHASE 9: VERIFICATION ═══");
        VerifyAccess();

        // ═══════════════ SUMMARY ═══════════════
        Console.WriteLine($"\n═══ SUMMARY ═══");
        Console.WriteLine($"  ✅ Succeeded: {successCount}");
        Console.WriteLine($"  ⏩ Skipped:   {skipCount}");
        Console.WriteLine($"  ❌ Errors:    {errorCount}");
        Console.WriteLine(errorCount == 0 ? "\n🎉 OLS SETUP COMPLETED SUCCESSFULLY!" : "\n⚠️ Some steps had errors. Review output above.");
    }

    // ══════════════════════════════════════════════════
    // PHASE 1: DIAGNOSTIC
    // ══════════════════════════════════════════════════
    static bool RunDiagnostic()
    {
        bool olsEnabled = false;

        // Check Oracle version
        try
        {
            string ver = QueryScalar(PDB_CONN, "SELECT BANNER FROM V$VERSION WHERE ROWNUM = 1") ?? "Unknown";
            Console.WriteLine($"  Oracle: {ver}");
        }
        catch (Exception ex) { Console.WriteLine($"  Oracle version: ERROR - {Short(ex)}"); }

        // Check OLS in DBA_REGISTRY
        try
        {
            string status = QueryScalar(PDB_CONN, "SELECT STATUS FROM DBA_REGISTRY WHERE COMP_ID = 'OLS'") ?? "NOT FOUND";
            Console.WriteLine($"  DBA_REGISTRY OLS: {status}");
        }
        catch (Exception ex) { Console.WriteLine($"  DBA_REGISTRY: {Short(ex)}"); }

        // Check V$OPTION
        try
        {
            string val = QueryScalar(PDB_CONN, "SELECT VALUE FROM V$OPTION WHERE PARAMETER = 'Oracle Label Security'") ?? "NOT FOUND";
            Console.WriteLine($"  V$OPTION OLS: {val}");
            olsEnabled = val.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception ex) { Console.WriteLine($"  V$OPTION: {Short(ex)}"); }

        // Check DBA_OLS_STATUS
        try
        {
            using var conn = new OracleConnection(PDB_CONN);
            conn.Open();
            using var cmd = new OracleCommand("SELECT NAME, STATUS FROM DBA_OLS_STATUS", conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string name = rdr.GetString(0);
                string status = rdr.GetString(1);
                Console.WriteLine($"  {name}: {status}");
                if (name.Contains("ENABLE") && status.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                    olsEnabled = true;
            }
        }
        catch (Exception ex) { Console.WriteLine($"  DBA_OLS_STATUS: {Short(ex)}"); }

        // Check LBACSYS account
        try
        {
            string status = QueryScalar(PDB_CONN, "SELECT ACCOUNT_STATUS FROM DBA_USERS WHERE USERNAME = 'LBACSYS'") ?? "NOT FOUND";
            Console.WriteLine($"  LBACSYS account: {status}");
        }
        catch (Exception ex) { Console.WriteLine($"  LBACSYS: {Short(ex)}"); }

        Console.WriteLine($"\n  >>> OLS Enabled: {(olsEnabled ? "YES ✅" : "NO ❌")}");
        return olsEnabled;
    }

    // ══════════════════════════════════════════════════
    // PHASE 2: TRY TO ENABLE OLS
    // ══════════════════════════════════════════════════
    static bool TryEnableOls()
    {
        Console.WriteLine("  Unlocking LBACSYS...");
        
        // Try to unlock LBACSYS at CDB level
        ExecSafe(CDB_CONN, "ALTER USER LBACSYS IDENTIFIED BY lbacsys ACCOUNT UNLOCK", "Unlock LBACSYS (CDB)");
        
        // Try at PDB level too  
        ExecSafe(PDB_CONN, "ALTER USER LBACSYS IDENTIFIED BY lbacsys ACCOUNT UNLOCK", "Unlock LBACSYS (PDB)");

        // Grant necessary privileges
        ExecSafe(PDB_CONN, "GRANT INHERIT PRIVILEGES ON USER SYS TO LBACSYS", "Grant INHERIT PRIVS");
        
        // Try CONFIGURE_OLS
        Console.WriteLine("  Attempting CONFIGURE_OLS...");
        bool configured = ExecSafe(PDB_CONN, "BEGIN LBACSYS.CONFIGURE_OLS; END;", "CONFIGURE_OLS");
        
        if (!configured)
        {
            // Try connecting as LBACSYS directly
            Console.WriteLine("  Trying as LBACSYS user...");
            string lbacsysConn = "User Id=LBACSYS;Password=lbacsys;Data Source=localhost:1521/xepdb1;Connection Timeout=10;";
            configured = ExecSafe(lbacsysConn, "BEGIN LBACSYS.CONFIGURE_OLS; END;", "CONFIGURE_OLS as LBACSYS");
        }

        if (configured)
        {
            // Try ENABLE_OLS
            Console.WriteLine("  Attempting ENABLE_OLS...");
            bool enabled = ExecSafe(PDB_CONN, "BEGIN LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS; END;", "ENABLE_OLS");

            if (enabled)
            {
                // Restart PDB
                Console.WriteLine("  Restarting PDB...");
                ExecSafe(CDB_CONN, "ALTER PLUGGABLE DATABASE xepdb1 CLOSE IMMEDIATE", "Close PDB");
                ExecSafe(CDB_CONN, "ALTER PLUGGABLE DATABASE xepdb1 OPEN", "Open PDB");
                
                // Verify
                System.Threading.Thread.Sleep(2000);
                try
                {
                    string val = QueryScalar(PDB_CONN, "SELECT VALUE FROM V$OPTION WHERE PARAMETER = 'Oracle Label Security'") ?? "FALSE";
                    if (val.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("  ✅ OLS is now ENABLED!");
                        return true;
                    }
                }
                catch { }
                
                // Check DBA_OLS_STATUS as fallback
                try
                {
                    string val = QueryScalar(PDB_CONN, "SELECT STATUS FROM DBA_OLS_STATUS WHERE NAME = 'OLS_ENABLE_STATUS'") ?? "FALSE";
                    if (val.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("  ✅ OLS is now ENABLED (confirmed via DBA_OLS_STATUS)!");
                        return true;
                    }
                }
                catch { }
            }
        }

        Console.WriteLine("  ❌ Could not enable OLS automatically.");
        return false;
    }

    // ══════════════════════════════════════════════════
    // PHASE 3: CLEANUP
    // ══════════════════════════════════════════════════
    static void CleanupOldObjects()
    {
        // Remove policy from table
        ExecSafe(PDB_CONN, @"BEGIN
            LBACSYS.SA_POLICY_ADMIN.REMOVE_TABLE_POLICY(
                policy_name => 'THONGBAO_POL',
                schema_name => 'QLBENHVIEN',
                table_name  => 'THONGBAO',
                drop_column => TRUE);
        END;", "Remove policy from table");

        // Drop policy
        ExecSafe(PDB_CONN, @"BEGIN
            LBACSYS.SA_SYSDBA.DROP_POLICY(
                policy_name => 'THONGBAO_POL',
                drop_column => TRUE);
        END;", "Drop policy");

        // Drop table
        ExecSafe(PDB_CONN, "DROP TABLE QLBENHVIEN.THONGBAO PURGE", "Drop THONGBAO table");

        // Drop users u1-u8
        for (int i = 1; i <= 8; i++)
        {
            ExecSafe(PDB_CONN, $"DROP USER u{i} CASCADE", $"Drop user u{i}");
        }

        Console.WriteLine("  Cleanup complete.");
    }

    // ══════════════════════════════════════════════════
    // PHASE 4: CREATE POLICY
    // ══════════════════════════════════════════════════
    static void CreatePolicy()
    {
        // Create policy with default options
        Exec(PDB_CONN, @"BEGIN
            LBACSYS.SA_SYSDBA.CREATE_POLICY(
                policy_name     => 'THONGBAO_POL',
                column_name     => 'OLS_LABEL',
                default_options => 'READ_CONTROL,WRITE_CONTROL,CHECK_CONTROL');
        END;", "Create policy THONGBAO_POL");

        // Levels: Ban Giám Đốc (300) > Lãnh Đạo Khoa (200) > Nhân Viên (100)
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POL', 300, 'GD', 'Ban Giam Doc'); END;", "Level: GD (300)");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POL', 200, 'LD', 'Lanh Dao Khoa'); END;", "Level: LD (200)");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POL', 100, 'NV', 'Nhan Vien'); END;", "Level: NV (100)");


        // Compartments: 3 Khoa
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POL', 10, 'TH', 'Khoa Tieu Hoa'); END;", "Compartment: TH");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POL', 20, 'TK', 'Khoa Than Kinh'); END;", "Compartment: TK");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POL', 30, 'TM', 'Khoa Tim Mach'); END;", "Compartment: TM");

        // Groups: 3 Cơ sở
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_GROUP('THONGBAO_POL', 10, 'HCM', 'Ho Chi Minh'); END;", "Group: HCM");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_GROUP('THONGBAO_POL', 20, 'HP', 'Hai Phong'); END;", "Group: HP");
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_COMPONENTS.CREATE_GROUP('THONGBAO_POL', 30, 'HN', 'Ha Noi'); END;", "Group: HN");
    }

    // ══════════════════════════════════════════════════
    // PHASE 5: CREATE TABLE & APPLY POLICY
    // ══════════════════════════════════════════════════
    static string LBACSYS_CONN = "User Id=LBACSYS;Password=lbacsys;Data Source=localhost:1521/xepdb1;Connection Timeout=10;";

    static void CreateTableAndApplyPolicy()
    {
        // Create table (3 columns as per requirement: NOIDUNG, NGAYGIO, DIADIEM)
        Exec(PDB_CONN, @"CREATE TABLE QLBENHVIEN.THONGBAO (
            NOIDUNG   NVARCHAR2(500),
            NGAYGIO   TIMESTAMP DEFAULT SYSTIMESTAMP,
            DIADIEM   NVARCHAR2(200)
        )", "Create THONGBAO table");

        // Grant EXECUTE on SA_POLICY_ADMIN to SYS and LBACSYS
        ExecSafe(PDB_CONN, "GRANT EXECUTE ON LBACSYS.SA_POLICY_ADMIN TO SYS", "Grant SA_POLICY_ADMIN to SYS");
        ExecSafe(PDB_CONN, "GRANT EXECUTE ON LBACSYS.SA_USER_ADMIN TO SYS", "Grant SA_USER_ADMIN to SYS");
        ExecSafe(PDB_CONN, "GRANT SELECT ON QLBENHVIEN.THONGBAO TO LBACSYS", "Grant SELECT to LBACSYS");
        ExecSafe(PDB_CONN, "GRANT INSERT ON QLBENHVIEN.THONGBAO TO LBACSYS", "Grant INSERT to LBACSYS");

        // Apply policy - try as SYS first, then as LBACSYS
        string applySQL = @"BEGIN
            SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
                policy_name   => 'THONGBAO_POL',
                schema_name   => 'QLBENHVIEN',
                table_name    => 'THONGBAO',
                table_options => 'READ_CONTROL');
        END;";

        if (!ExecSafe(PDB_CONN, applySQL, "Apply policy (as SYS, no prefix)"))
        {
            // Fallback: try with LBACSYS prefix
            string applySQL2 = @"BEGIN
                LBACSYS.SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
                    policy_name   => 'THONGBAO_POL',
                    schema_name   => 'QLBENHVIEN',
                    table_name    => 'THONGBAO',
                    table_options => 'READ_CONTROL');
            END;";
            if (!ExecSafe(PDB_CONN, applySQL2, "Apply policy (as SYS, with prefix)"))
            {
                // Fallback: try as LBACSYS user
                ExecSafe(PDB_CONN, "GRANT ALL ON QLBENHVIEN.THONGBAO TO LBACSYS", "Grant ALL to LBACSYS");
                if (!ExecSafe(LBACSYS_CONN, applySQL, "Apply policy (as LBACSYS)"))
                {
                    Exec(LBACSYS_CONN, applySQL2, "Apply policy (as LBACSYS, with prefix)");
                }
            }
        }

        // Verify OLS_LABEL column was created
        try
        {
            string colCount = QueryScalar(PDB_CONN, "SELECT COUNT(*) FROM ALL_TAB_COLUMNS WHERE TABLE_NAME='THONGBAO' AND COLUMN_NAME='OLS_LABEL' AND OWNER='QLBENHVIEN'") ?? "0";
            if (colCount == "1")
                Console.WriteLine("  ✅ OLS_LABEL column verified!");
            else
                Console.WriteLine("  ❌ OLS_LABEL column NOT found! Policy apply may have failed.");
        }
        catch (Exception ex) { Console.WriteLine($"  ⚠️ Column check: {Short(ex)}"); }
    }

    // PHASE 6: CREATE USERS
    // ══════════════════════════════════════════════════
    static void CreateUsers()
    {
        for (int i = 1; i <= 8; i++)
        {
            string uname = $"U{i}"; // Use uppercase for consistency
            ExecSafe(PDB_CONN, $"DROP USER {uname} CASCADE", $"Cleanup user {uname}");
            Exec(PDB_CONN, $"CREATE USER {uname} IDENTIFIED BY 123", $"Create user {uname}");
            Exec(PDB_CONN, $"GRANT CREATE SESSION TO {uname}", $"Grant session to {uname}");
            Exec(PDB_CONN, $"GRANT SELECT ON QLBENHVIEN.THONGBAO TO {uname}", $"Grant select to {uname}");
        }
    }

    // ══════════════════════════════════════════════════
    // PHASE 7: SET USER LABELS (per the image requirements)
    // ══════════════════════════════════════════════════
    static void SetUserLabels()
    {
        // u1: Giám đốc – đọc toàn bộ
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U1','GD:TH,TK,TM:HCM,HP,HN'); END;", "u1: GD full access");
        // u2: Lãnh đạo Khoa tim mạch tại HCM
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U2','LD:TM:HCM'); END;", "u2: LD TM HCM");
        // u3: Lãnh đạo Khoa thần kinh tại Hà Nội
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U3','LD:TK:HN'); END;", "u3: LD TK HN");
        // u4: Nhân viên Khoa thần kinh tại HCM
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U4','NV:TK:HCM'); END;", "u4: NV TK HCM");
        // u5: Nhân viên Khoa tim mạch tại HCM
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U5','NV:TM:HCM'); END;", "u5: NV TM HCM");
        // u6: Lãnh đạo phòng - đọc TM HCM
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U6','LD:TM:HCM'); END;", "u6: LD TM HCM");
        // u7: Lãnh đạo phòng - đọc toàn bộ cấp LD
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U7','LD:TH,TK,TM:HCM,HP,HN'); END;", "u7: LD full");
        // u8: Nhân viên Khoa tiêu hóa tại Hà Nội
        Exec(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_LABELS('THONGBAO_POL','U8','NV:TH:HN'); END;", "u8: NV TH HN");
    }

    // ══════════════════════════════════════════════════
    // PHASE 8: INSERT DATA (t1-t7)
    // ══════════════════════════════════════════════════
    static void InsertData()
    {
        // Grant FULL OLS to LBACSYS (it's the OLS admin, not a common user)
        ExecSafe(PDB_CONN, "BEGIN SA_USER_ADMIN.SET_USER_PRIVS('THONGBAO_POL','LBACSYS','FULL'); END;", "Grant FULL to LBACSYS");
        ExecSafe(PDB_CONN, "BEGIN LBACSYS.SA_USER_ADMIN.SET_USER_PRIVS('THONGBAO_POL','LBACSYS','FULL'); END;", "Grant FULL to LBACSYS (prefix)");

        // Try to grant FULL to SYS as well
        ExecSafe(PDB_CONN, "BEGIN SA_USER_ADMIN.SET_USER_PRIVS('THONGBAO_POL','SYS','FULL'); END;", "Grant FULL to SYS");

        // Data rows per requirement image
        var data = new (string noidung, string diadiem, string label)[]
        {
            ("t1: Thong bao chung toan vien",           "Hoi truong",      "NV"),         
            ("t2: Hop kin Ban Giam Doc",                 "Phong VIP",       "GD:TH,TK,TM:HCM,HP,HN"), // Make GD label explicit 
            ("t3: Giao ban Lanh Dao Khoa",               "P. Hop 2",        "LD:TH,TK,TM:HCM,HP,HN"), // Make LD label explicit
            ("t4: Hop noi bo Tieu Hoa",                  "Khoa TH",         "LD:TH"),      
            ("t5: Thong bao NV Tieu Hoa HCM",            "CS HCM",          "NV:TH:HCM"), 
            ("t6: Thong bao NV Tieu Hoa HN",             "CS HN",           "NV:TH:HN"),  
            ("t7: Hop lien khoa TH-TK Hai Phong",        "CS HP",           "LD:TH,TK:HP"), 
        };

        // Standardize: Insert using LBACSYS which is the policy owner
        Console.WriteLine("  Inserting data as LBACSYS...");
        foreach (var (noidung, diadiem, label) in data)
        {
            // Use Label String explicitly
            string sql = $@"INSERT INTO QLBENHVIEN.THONGBAO (NOIDUNG, NGAYGIO, DIADIEM, OLS_LABEL) 
                           VALUES ('{noidung}', SYSTIMESTAMP, '{diadiem}', CHAR_TO_LABEL('THONGBAO_POL', '{label}'))";
            Exec(LBACSYS_CONN, sql, $"Insert: {noidung.Substring(0, Math.Min(20, noidung.Length))}...");
        }
        Exec(LBACSYS_CONN, "COMMIT", "Commit");

        // DIAGNOSTIC Phase: Print actual labels
        Console.WriteLine("\n  --- DIAGNOSTIC: ROW LABELS ---");
        try
        {
            using var conn = new OracleConnection(LBACSYS_CONN);
            conn.Open();
            using var cmd = new OracleCommand("SELECT NOIDUNG, LABEL_TO_CHAR(OLS_LABEL) as LBL FROM QLBENHVIEN.THONGBAO", conn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine($"    Row: {rdr.GetString(0)} -> Label: {rdr.GetString(1)}");
            }
        }
        catch (Exception ex) { Console.WriteLine($"    ⚠️ Diagnostic failed: {Short(ex)}"); }
    }

    // ══════════════════════════════════════════════════
    // PHASE 9: VERIFICATION
    // ══════════════════════════════════════════════════
    static void VerifyAccess()
    {
        // Expected access matrix (from image)
        var expected = new (string user, int rows, string desc)[]
        {
            ("U1", 7, "GD: xem tat ca"),
            ("U2", 2, "LD:TM:HCM -> t1,t3"),
            ("U3", 2, "LD:TK:HN -> t1,t3"),
            ("U4", 1, "NV:TK:HCM -> t1"),
            ("U5", 1, "NV:TM:HCM -> t1"),
            ("U6", 2, "LD:TM:HCM -> t1,t3"),
            ("U7", 6, "LD:all -> t1,t3,t4,t5,t6,t7"),
            ("U8", 2, "NV:TH:HN -> t1,t6"),
        };

        bool allPass = true;
        foreach (var (user, exp, desc) in expected)
        {
            try
            {
                string userConn = $"User Id={user};Password=123;Data Source=localhost:1521/xepdb1;Connection Timeout=10;";
                string countStr = QueryScalar(userConn, "SELECT COUNT(*) FROM QLBENHVIEN.THONGBAO") ?? "0";
                int actual = int.Parse(countStr);
                string status = actual == exp ? "PASS ✅" : $"FAIL ❌ (got {actual})";
                if (actual != exp) allPass = false;
                Console.WriteLine($"  {user} ({desc}): expected={exp} -> {status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  {user}: ERROR - {Short(ex)}");
                allPass = false;
            }
        }

        if (allPass)
            Console.WriteLine("\n  🎉 ALL VERIFICATION TESTS PASSED!");
        else
            Console.WriteLine("\n  ⚠️ Some tests did not pass.");
    }

    // ══════════════════════════════════════════════════
    // HELPER METHODS
    // ══════════════════════════════════════════════════
    static string? QueryScalar(string connStr, string sql)
    {
        using var conn = new OracleConnection(connStr);
        conn.Open();
        using var cmd = new OracleCommand(sql, conn);
        var result = cmd.ExecuteScalar();
        return result?.ToString();
    }

    static void Exec(string connStr, string sql, string description)
    {
        try
        {
            using var conn = new OracleConnection(connStr);
            conn.Open();
            using var cmd = new OracleCommand(sql, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"  ✅ {description}");
            successCount++;
        }
        catch (OracleException ex)
        {
            Console.WriteLine($"  ❌ {description}: {Short(ex)}");
            errorCount++;
        }
    }

    static bool ExecSafe(string connStr, string sql, string description)
    {
        try
        {
            using var conn = new OracleConnection(connStr);
            conn.Open();
            using var cmd = new OracleCommand(sql, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"  ✅ {description}");
            successCount++;
            return true;
        }
        catch (OracleException ex)
        {
            int num = ex.Number;
            // Expected errors when re-running
            if (num == 955 || num == 1920 || num == 942 || num == 1918 ||  // object exists / doesn't exist
                num == 12474 || num == 12461 || num == 12462 || num == 12460 || // OLS dup
                num == 1927 || num == 65066)  // already granted / container
            {
                Console.WriteLine($"  ⏩ {description} (skipped: already exists/done)");
                skipCount++;
            }
            else
            {
                Console.WriteLine($"  ⚠️ {description}: {Short(ex)}");
                errorCount++;
            }
            return false;
        }
    }

    static string Short(Exception ex) => ex.Message.Split('\n')[0].Trim();
}
