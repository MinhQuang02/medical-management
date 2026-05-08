-- ============================================================
-- 0. CONNECT AS DBA / QLBENHVIEN
-- ============================================================
ALTER SESSION SET CONTAINER = XEPDB1;

-- ============================================================
-- 1. CHECK AUDIT TRAIL
-- ============================================================
SHOW PARAMETER audit_trail;

-- ============================================================
-- 2. RESET OLD AUDIT
-- ============================================================
NOAUDIT ALL;
/

-- ============================================================
-- 3. SCENARIO 1 :Audit DELETE on HSBA_DV
-- ============================================================
AUDIT DELETE ON QLBENHVIEN.HSBA_DV BY ACCESS;
/

-- ============================================================
-- 4. SCENARIO 2: AUDIT UPDATE ON HSBA
-- ============================================================
AUDIT UPDATE ON QLBENHVIEN.HSBA BY ACCESS;
/

-- ============================================================
-- 5. SCENARIO 3 : AUDIT UPDATE ON DONTHUOC
-- ============================================================
AUDIT UPDATE ON QLBENHVIEN.DONTHUOC BY ACCESS;
/

-- ============================================================
-- 6. SCENARIO 4: AUDIT UPDATE ON VIEW (KTV)
-- ============================================================
AUDIT UPDATE ON QLBENHVIEN.KTV_XEMHSBA_DV BY ACCESS;
/

-- ============================================================
-- 7. SCENARIO 5: AUDIT DELETE ON DONTHUOC
-- ============================================================
AUDIT DELETE ON QLBENHVIEN.DONTHUOC BY ACCESS;
/

-- ============================================================
-- 8. GENERATE TEST DATA (SUCCESS + FAILURE)
-- ============================================================

-- ===== SCENARIO 1 =====
-- SUCCESS (Doctor)
CONN BS001/123@localhost:1521/XEPDB1
DELETE FROM QLBENHVIEN.HSBA_DV
WHERE ROWNUM = 1;

-- FAILURE (Patient)
CONN BN00001/123@localhost:1521/XEPDB1
DELETE FROM QLBENHVIEN.HSBA_DV;
-- ===== SCENARIO 2 =====
-- SUCCESS (Doctor)
CONN BS001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.HSBA
SET CHANDOAN = 'Test Audit'
WHERE ROWNUM = 1;

-- FAILURE (Technician)
CONN KTV001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.HSBA
SET CHANDOAN = 'Hack';

-- ===== SCENARIO 3  =====
-- SUCCESS (Doctor)
CONN BS001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.DONTHUOC
SET LIEUDUNG = 'UPDATED'
WHERE ROWNUM = 1;

-- FAILURE (Patient - no privilege)
CONN BN00001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.DONTHUOC
SET LIEUDUNG = 'HACK';

-- ===== SCENARIO 4 =====
-- SUCCESS (Technician correct row)
CONN KTV001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.KTV_XEMHSBA_DV
SET KETQUA = 'DONE';

-- FAILURE (Technician wrong object)
CONN KTV001/123@localhost:1521/XEPDB1
UPDATE QLBENHVIEN.HSBA_DV
SET KETQUA = 'HACK';

-- ===== SCENARIO 5 =====
-- SUCCESS (Doctor)
CONN BS001/123@localhost:1521/XEPDB1
DELETE FROM QLBENHVIEN.DONTHUOC
WHERE ROWNUM = 1;

-- FAILURE (Technician)
CONN KTV001/123@localhost:1521/XEPDB1
DELETE FROM QLBENHVIEN.DONTHUOC;

-- ============================================================
-- 9. VIEW AUDIT LOG
-- ============================================================
CONN QLBENHVIEN/123@localhost:1521/XEPDB1

COLUMN USERNAME FORMAT A15
COLUMN ACTION_NAME FORMAT A20
COLUMN OBJ_NAME FORMAT A25

SELECT 
    USERNAME,
    ACTION_NAME,
    OBJ_NAME,
    RETURNCODE,
    TO_CHAR(TIMESTAMP, 'DD-MM-YYYY HH24:MI:SS') AS TIME
FROM DBA_AUDIT_TRAIL
WHERE OBJ_NAME IN (
    'HSBA',
    'HSBA_DV',
    'KTV_XEMHSBA_DV',
    'DONTHUOC'
)
ORDER BY TIMESTAMP DESC;

-- ============================================================
-- 10. INTERPRETATION NOTE
-- ============================================================
-- RETURNCODE = 0  → SUCCESS
-- RETURNCODE ≠ 0 → FAILURE

COMMIT;