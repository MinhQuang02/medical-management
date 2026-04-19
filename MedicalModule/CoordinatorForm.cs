using Oracle.ManagedDataAccess.Client;

namespace MedicalDataManagement.MedicalModule;

public partial class CoordinatorForm : Form
{
    private DatabaseService _db;
    private string _username;

    //Doi ten thanh user dang co schema du lieu
    public string Schema { get; set; } = "QLBENHVIEN";

    public CoordinatorForm(DatabaseService db, string username)
    {
        InitializeComponent();
        _db = db;
        _username = username;

        LoadAll();
    }

    //Load toan bo du lieu
    private void LoadAll()
    {
        LoadNhanVien();
        LoadBenhNhan();
        LoadHSBA();
        LoadDichVu();
    }

    //Cho load thong tin ca nhan
    private void LoadNhanVien()
    {
        dgvNhanVien.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.NHANVIEN");
    }

    //Cho nut cap nhat thong tin ca nhan
    private void btnUpdateNV_Click(object sender, EventArgs e)
    {
        var r = dgvNhanVien.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.NHANVIEN
              SET QUEQUAN = :qq,
                  SODT = :sdt
              WHERE MANV = :id",
        new OracleParameter[]
        {
            new OracleParameter("qq", r.Cells["QUEQUAN"].Value),
            new OracleParameter("sdt", r.Cells["SODT"].Value),
            new OracleParameter("id", r.Cells["MANV"].Value)
        });

        LoadNhanVien();
        MessageBox.Show("Update personal info successfully!");
    }

    //Cho load benh nhan
    private void LoadBenhNhan()
    {
        dgvBenhNhan.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.BENHNHAN");
    }

    //Cho nut them benh nhan
    private void btnAddBN_Click(object sender, EventArgs e)
    {
        _db.ExecuteNonQuery(
            $@"INSERT INTO {Schema}.BENHNHAN
            (MABN, TENBN, PHAI, NGAYSINH, CCCD, SONHA, TENDUONG, QUANHUYEN, TINHTP, TIENSUBENH, TIENSUBENHGD, DIUNGTHUOC)
            VALUES (:ma, :ten, :phai, SYSDATE, :cccd, :sonha, :duong, :qh, :tp, NULL, NULL, NULL)",
        new OracleParameter[]
        {
            new OracleParameter("ma", txtMaBN.Text),
            new OracleParameter("ten", txtTenBN.Text),
            new OracleParameter("phai", txtPhai.Text),
            new OracleParameter("cccd", txtCCCD.Text),
            new OracleParameter("sonha", txtSoNha.Text),
            new OracleParameter("duong", txtDuong.Text),
            new OracleParameter("qh", txtQuan.Text),
            new OracleParameter("tp", txtTP.Text)
        });

        LoadBenhNhan();
    }

    //Cho nut cap nhat benh nhan
    private void btnUpdateBN_Click(object sender, EventArgs e)
    {
        var r = dgvBenhNhan.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.BENHNHAN
              SET SONHA = :sn,
                  TENDUONG = :td,
                  QUANHUYEN = :qh,
                  TINHTP = :tp
              WHERE MABN = :id",
        new OracleParameter[]
        {
            new OracleParameter("sn", r.Cells["SONHA"].Value),
            new OracleParameter("td", r.Cells["TENDUONG"].Value),
            new OracleParameter("qh", r.Cells["QUANHUYEN"].Value),
            new OracleParameter("tp", r.Cells["TINHTP"].Value),
            new OracleParameter("id", r.Cells["MABN"].Value)
        });

        LoadBenhNhan();

        MessageBox.Show("Update patient successfully!");
    }

    //Cho load HSBA
    private void LoadHSBA()
    {
        // Fixed incorrect schema-qualified table name (removed duplicated schema segment)
        dgvHSBA.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.HSBA");
    }

    //Cho tao moi HSBA
    private void btnAddHSBA_Click(object sender, EventArgs e)
    {
        _db.ExecuteNonQuery(
            $@"INSERT INTO {Schema}.HSBA
            (MAHSBA, MABN, NGAY, CHANDOAN, DIEUTRI, MABS, MAKHOA, KETLUAN)
              VALUES (:ma, :mabn, SYSDATE, NULL, NULL, :mabs, :makhoa, NULL)",
        new OracleParameter[]
        {
            new OracleParameter("ma", txtMaHSBA.Text),
            new OracleParameter("mabn", txtMaBN.Text),
            new OracleParameter("mabs", txtMaBS.Text),
            new OracleParameter("makhoa", txtMaKhoa.Text)
        });

        LoadHSBA();
    }

    //Cho update cua hsba
    private void btnUpdateHSBA_Click(object sender, EventArgs e)
    {
        var r = dgvHSBA.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.HSBA
              SET MAKHOA = :mk,
                  MABS = :bs
              WHERE MAHSBA = :id",
        new OracleParameter[]
        {
            new OracleParameter("mk", r.Cells["MAKHOA"].Value),
            new OracleParameter("bs", r.Cells["MABS"].Value),
            new OracleParameter("id", r.Cells["MAHSBA"].Value)
        });

        LoadHSBA();
    }

    //Cho load phan dich vu
    private void LoadDichVu()
    {
        dgvDichVu.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.HSBA_DV");
    }

    //Cho update cac dich vu
    private void btnUpdateDV_Click(object sender, EventArgs e)
    {
        var r = dgvDichVu.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.HSBA_DV
              SET MAKTV = :ktv
              WHERE MAHSBA = :ma
              AND LOAIDV = :loai
              AND NGAYDV = :ngay",
        new OracleParameter[]
        {
            new OracleParameter("ktv", r.Cells["MAKTV"].Value),
            new OracleParameter("ma", r.Cells["MAHSBA"].Value),
            new OracleParameter("loai", r.Cells["LOAIDV"].Value),
            new OracleParameter("ngay", r.Cells["NGAYDV"].Value)
        });

        LoadDichVu();
    }
}