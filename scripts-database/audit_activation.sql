-- ================================================================
-- YÊU CẦU 3 - MỤC 1: KÍCH HOẠT KIỂM TOÁN HỆ THỐNG 
-- ================================================================
ALTER SESSION SET CONTAINER = XEPDB1;

-- ============================================================
-- BƯỚC 1: KIỂM TRA TRẠNG THÁI AUDIT TRAIL HIỆN TẠI
-- ============================================================
SHOW PARAMETER AUDIT_TRAIL;

-- ============================================================
-- BƯỚC 2: XÓA CÁC AUDIT SETTINGS CŨ (Nếu cần reset)
-- ============================================================
NOAUDIT ALL;
/
-- ============================================================
-- BƯỚC 3: KÍCH HOẠT AUDIT CHO TOÀN BỘ HỆ THỐNG
-- ============================================================
-- Kích hoạt audit cho tất cả hoạt động kết nối
AUDIT CREATE SESSION;
/

-- ============================================================
-- BƯỚC 4: KÍCH HOẠT AUDIT CHO DDL (Data Definition Language)
-- ============================================================
-- Oracle gom nhóm các quyền CREATE/DROP/TRUNCATE theo từng loại Object
AUDIT TABLE;       -- Bao trùm CREATE, DROP, TRUNCATE Table
AUDIT PROCEDURE;   -- Bao trùm CREATE, DROP Procedure/Function/Package
AUDIT VIEW;        -- Bao trùm CREATE, DROP View
AUDIT TRIGGER;     -- Bao trùm CREATE, ALTER, DROP Trigger
/

-- ============================================================
-- BƯỚC 5: KÍCH HOẠT AUDIT CHO SYSTEM PRIVILEGES
-- ============================================================
-- Kiểm toán việc cấp/thu hồi quyền hệ thống và Role
AUDIT SYSTEM GRANT;
/

-- ============================================================
-- BƯỚC 6: KIỂM TRA AUDIT SETTINGS ĐÃ ĐƯỢC KÍCH HOẠT
-- ============================================================
COLUMN AUDIT_OPTION FORMAT A30
COLUMN SUCCESS FORMAT A15
COLUMN FAILURE FORMAT A15

SELECT 
    USER_NAME,
    AUDIT_OPTION,
    SUCCESS,
    FAILURE
FROM DBA_STMT_AUDIT_OPTS
ORDER BY AUDIT_OPTION;

-- ============================================================
-- BƯỚC 7: XEM TỔNG QUÁT TRẠNG THÁI AUDIT
-- ============================================================
SELECT 
    'Audit Trail Status' AS CHECK_ITEM,
    CASE 
        WHEN (SELECT VALUE FROM V$PARAMETER WHERE NAME = 'audit_trail') IS NOT NULL 
        THEN 'ENABLED' 
        ELSE 'DISABLED' 
    END AS STATUS
FROM DUAL;

-- ============================================================
-- KẾT THÚC: KÍCH HOẠT AUDIT THÀNH CÔNG
-- ============================================================
COMMIT;
/