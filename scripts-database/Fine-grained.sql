-- ================================================================
-- YÊU CẦU 3
-- ================================================================
-- Kết nối vào PDB (Pluggable Database) với quyền DBA
ALTER SESSION SET CONTAINER = XEPDB1;

-- ============================================================
-- PHẦN A: TẠO CÁC CHÍNH SÁCH KIỂM TOÁN (POLICIES)
-- ============================================================

-- Xóa các policy cũ nếu có (để tránh lỗi khi chạy lại script)
BEGIN
  DBMS_FGA.DROP_POLICY(object_schema => 'QLBENHVIEN', object_name => 'DONTHUOC', policy_name => 'FGA_AUDIT_DONTHUOC_COLS');
  DBMS_FGA.DROP_POLICY(object_schema => 'QLBENHVIEN', object_name => 'HSBA', policy_name => 'FGA_AUDIT_HSBA_COLS');
EXCEPTION
  WHEN OTHERS THEN NULL;
END;
/

-- ------------------------------------------------------------
-- Tình huống 3a: Hành vi cập nhật trên các cột MAHSBA, NGAYDT, TENTHUOC, LIEUDUNG của DONTHUOC
-- ------------------------------------------------------------
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema   => 'QLBENHVIEN',
    object_name     => 'DONTHUOC',
    policy_name     => 'FGA_AUDIT_DONTHUOC_COLS',
    audit_column    => 'MAHSBA, NGAYDT, TENTHUOC, LIEUDUNG',
    statement_types => 'UPDATE',
    audit_trail     => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/

-- ------------------------------------------------------------
-- Tình huống 3b & 3c: Kiểm toán trên các trường CHANDOAN, DIEUTRI, KETLUAN của HSBA
-- ------------------------------------------------------------
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema   => 'QLBENHVIEN',
    object_name     => 'HSBA',
    policy_name     => 'FGA_AUDIT_HSBA_COLS',
    audit_column    => 'CHANDOAN, DIEUTRI, KETLUAN',
    statement_types => 'UPDATE',
    audit_trail     => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/

-- ------------------------------------------------------------
-- Tình huống 3c: Hành vi cập nhật bất hợp pháp trên CHANDOAN, DIEUTRI, KETLUAN
-- ------------------------------------------------------------
CREATE UNIFIED AUDIT POLICY POL_HSBA_UPDATE_FAIL 
ACTIONS UPDATE ON QLBENHVIEN.HSBA;

AUDIT POLICY POL_HSBA_UPDATE_FAIL WHENEVER NOT SUCCESSFUL;

-- ------------------------------------------------------------
-- Tình huống 3d: Hành vi thêm, xóa, sửa bất hợp pháp trên HSBA_DV
-- ------------------------------------------------------------
CREATE UNIFIED AUDIT POLICY POL_HSBADV_DML_FAIL 
ACTIONS INSERT ON QLBENHVIEN.HSBA_DV, 
        UPDATE ON QLBENHVIEN.HSBA_DV, 
        DELETE ON QLBENHVIEN.HSBA_DV;

AUDIT POLICY POL_HSBADV_DML_FAIL WHENEVER NOT SUCCESSFUL;


-- ============================================================
-- PHẦN B: TẠO NGỮ CẢNH ĐỂ SINH DỮ LIỆU NHẬT KÝ KIỂM TOÁN
-- ============================================================

-- [TEST 3a]: Bác sĩ cập nhật thông tin liều dùng trong ĐƠN THUỐC (Hợp lệ)
CONN BS001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.DONTHUOC 
SET LIEUDUNG = 'Sang 1 vien, Toi 1 vien' 
WHERE MAHSBA = 'HSBA001' AND TENTHUOC = 'Paracetamol';
COMMIT;

-- [TEST 3b]: Bác sĩ cập nhật thành công CĐ, ĐT, KL trên HSBA (Hợp lệ)
CONN BS001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.HSBA 
SET CHANDOAN = 'Viem hong hat', DIEUTRI = 'Uong thuoc', KETLUAN = 'Theo doi them' 
WHERE MAHSBA = 'HSBA001';
COMMIT;

-- [TEST 3c]: Bệnh nhân cố tình cập nhật CHANDOAN trên HSBA (Bất hợp pháp - Lỗi thiếu quyền)
CONN BN00001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.HSBA 
SET CHANDOAN = 'Khong co benh' 
WHERE MAHSBA = 'HSBA001';

-- [TEST 3d]: Bệnh nhân cố tình INSERT/DELETE vào HSBA_DV (Bất hợp pháp)
CONN BN00001/123@localhost:1521/XEPDB1
INSERT INTO QLBENHVIEN.HSBA_DV (MAHSBA, LOAIDV, NGAYDV, MAKTV, KETQUA) 
VALUES ('HSBA001', 'X-Quang', SYSDATE, 'KTV001', 'Binh Thuong');

DELETE FROM QLBENHVIEN.HSBA_DV WHERE MAHSBA = 'HSBA001';
