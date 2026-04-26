-- ================================================================
-- YÊU CẦU 3 - MỤC 1: KÍCH HOẠT KIỂM TOÁN HỆ THỐNG
-- ================================================================
--Kết nối vào XEPDB1 với quyền SYSDBA
--sqlplus sys/1234567890@localhost:1521/xepdb1 as sysdba
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
AUDIT ALTER SESSION;
/

-- ============================================================
-- BƯỚC 4: KÍCH HOẠT AUDIT CHO DDL (Data Definition Language)
-- ============================================================

-- Kiểm toán các thao tác tạo/xóa/thay đổi cơ sở dữ liệu
AUDIT CREATE TABLE;
AUDIT DROP TABLE;
AUDIT ALTER TABLE;
AUDIT CREATE PROCEDURE;
AUDIT DROP PROCEDURE;
AUDIT CREATE VIEW;
AUDIT DROP VIEW;
AUDIT CREATE FUNCTION;
AUDIT DROP FUNCTION;
AUDIT CREATE TRIGGER;
AUDIT DROP TRIGGER;
/

-- ============================================================
-- BƯỚC 5: KÍCH HOẠT AUDIT CHO SYSTEM PRIVILEGES
-- ============================================================

-- Kiểm toán việc cấp/thu hồi quyền
AUDIT GRANT SYSTEM;
/

-- ============================================================
-- BƯỚC 6: KIỂM TRA AUDIT SETTINGS ĐÃ ĐƯỢC KÍCH HOẠT
-- ============================================================

COLUMN AUDIT_OPTION FORMAT A30
COLUMN BY_DEFAULT FORMAT A10

SELECT 
    AUDIT_OPTION,
    BY_DEFAULT
FROM DBA_STMT_AUDIT_OPTS
WHERE BY_DEFAULT IN ('BY ACCESS', 'BY SESSION')
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
-- Hệ thống kiểm toán đã được kích hoạt
-- Tất cả hoạt động sẽ được ghi lại trong DBA_AUDIT_TRAIL
COMMIT;
/

-- Xác nhận
SHOW PARAMETER AUDIT_TRAIL;
