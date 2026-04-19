using Oracle.ManagedDataAccess.Client;

namespace MedicalDataManagement.MedicalModule;

public partial class DoctorForm : Form
{
    private DatabaseService _db;
    private string _username;

    public string Schema { get; set; } = "QLBENHVIEN";

    public DoctorForm(DatabaseService db, string username)
    {
        InitializeComponent();
        _db = db;
        _username = username;

        LoadAll();
    }

    //Chuc nang cho tai het cac thong tin
    private void LoadAll()
    {
        LoadNhanVien();
        LoadHSBA();
        LoadBenhNhan();
        LoadDonThuoc();
        LoadDichVu();
    }

    //Cho nhan vien xem thong tin ca nhan
    private void LoadNhanVien()
    {
        dgvNhanVien.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.NHANVIEN");
    }

    //Cho nhan vien update thong tin chinh minh
    private void btnUpdateNV_Click(object sender, EventArgs e)
    {
        if (dgvNhanVien.CurrentRow == null) return;

        var row = dgvNhanVien.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.NHANVIEN
            SET QUEQUAN = :qq,
            SODT = :sdt
            WHERE MANV = :id",

        new OracleParameter[]
        {
            new OracleParameter ("qq", row.Cells["QUEQUAN"].Value),
            new OracleParameter ("sdt", row.Cells["SODT"].Value),
            new OracleParameter("id", row.Cells["MANV"].Value)
        });

        LoadNhanVien();
        MessageBox.Show("Update personal info successfully!");
    }

    //Cho load du lieu HSBA
    private void LoadHSBA()
    {
        dgvHSBA.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.HSBA");
    }

    //Cho nut de cap nhat HSBA
    private void btnUpdateHSBA_Click(object sender, EventArgs e)
    {
        if (dgvHSBA.CurrentRow == null) return;

        var r = dgvHSBA.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.HSBA
            SET CHANDOAN = :cd,
            DIEUTRI = :dt,
            KETLUAN = :kl
            WHERE MAHSBA = :id",
        new OracleParameter[]
        {
            new OracleParameter("cd", r.Cells["CHANDOAN"].Value),
            new OracleParameter("dt", r.Cells["DIEUTRI"].Value),
            new OracleParameter("kl", r.Cells["KETLUAN"].Value),
            new OracleParameter("id", r.Cells["MAHSBA"].Value)
        }
        );

        LoadHSBA();
        MessageBox.Show("Update medical record successfully!");
    }

    //Cho phan load benh nhan
    private void LoadBenhNhan()
    {
        dgvBenhNhan.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.BENHNHAN");
    }

    //Cho nut cap nhat benh nhan
    private void btnUpdateBN_Click(object sender, EventArgs e)
    {
        if (dgvBenhNhan.CurrentRow == null) return;

        var r = dgvBenhNhan.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.BENHNHAN
            SET TIENSUBENH = :ts,
            TIENSUBENHGD = :gd,
            DIUNGTHUOC = :du
            WHERE MABN = :id",
        new OracleParameter[]
        {
            new OracleParameter("ts", r.Cells["TIENSUBENH"].Value),
            new OracleParameter("gd", r.Cells["TIENSUBENHGD"].Value),
            new OracleParameter("du", r.Cells["DIUNGTHUOC"].Value),
            new OracleParameter("id", r.Cells["MABN"].Value)
        });

        LoadBenhNhan();
        MessageBox.Show("Update patient successfully!");
    }

    //Cho load don thuoc
    private void LoadDonThuoc()
    {
        dgvDonThuoc.DataSource = _db.ExecuteQuery($"SELECT * FROM {Schema}.DONTHUOC");
    }

    //Cho viec them don thuoc
    private void btnAddDT_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaHSBA.Text)) return;

        _db.ExecuteNonQuery(
            $@"INSERT INTO {Schema}.DONTHUOC
            VALUES (:ma, SYSDATE, :ten, :lieu)",
        new OracleParameter[]
        {
            new OracleParameter("ma", txtMaHSBA.Text),
            new OracleParameter("ten", txtTenThuoc.Text),
            new OracleParameter("lieu", txtLieuDung.Text)
        });

        LoadDonThuoc();
    }

    //Cho cap nhat don thuoc
    private void btnUpdateDT_Click(Object sender, EventArgs e)
    {
        if (dgvDonThuoc.CurrentRow == null) return;

        var r = dgvDonThuoc.CurrentRow;

        _db.ExecuteNonQuery(
            $@"UPDATE {Schema}.DONTHUOC
            SET TENTHUOC = :tt, LIEUDUNG = :lieu
            WHERE MAHSBA = :ma AND NGAYDT = :ng AND TENTHUOC = :old",

        new OracleParameter[]
        {
            new OracleParameter("tt", txtTenThuoc.Text),
            new OracleParameter("lieu", txtLieuDung.Text),
            new OracleParameter("ma", r.Cells["MAHSBA"].Value),
            new OracleParameter("ng", r.Cells["NGAYDT"].Value),
            new OracleParameter("old", r.Cells["TENTHUOC"].Value)
        });

        LoadDonThuoc();

        MessageBox.Show("Update prescription successfully!");
    }

    //Tranh cap nhat don thuoc bi rong
    private void dgvDonThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dgvDonThuoc.CurrentRow != null)
        {
            var r = dgvDonThuoc.CurrentRow;

            txtMaHSBA.Text = r.Cells["MAHSBA"].Value?.ToString();
            txtTenThuoc.Text = r.Cells["TENTHUOC"].Value?.ToString();
            txtLieuDung.Text = r.Cells["LIEUDUNG"].Value?.ToString();
        }
    }

    //Cho viec xoa don thuoc
    private void btnDeleteDT_Click(object sender, EventArgs e)
    {
        if (dgvDonThuoc.CurrentRow == null) return;

        var r = dgvDonThuoc.CurrentRow;

        _db.ExecuteNonQuery(
            $@"DELETE FROM {Schema}.DONTHUOC
            WHERE MAHSBA = :ma AND NGAYDT = :ng",
        new OracleParameter[]
        {
            new OracleParameter("ma", r.Cells["MAHSBA"].Value),
            new OracleParameter("ng", r.Cells["NGAYDT"].Value),
        });

        LoadDonThuoc();
    }

    //Cho load dich vu cua hsba
    private void LoadDichVu()
    {
        dgvDichVu.DataSource = _db.ExecuteQuery("SELECT * FROM HSBA_DV");
    }

    //Cho them dich vu hsba
    private void btnAddDichVu(object sender, EventArgs e)
    {
        if (dgvHSBA.CurrentRow == null) return;

        var r = dgvHSBA.CurrentRow;

        _db.ExecuteNonQuery(
            $@"INSERT INTO {Schema}.HSBA_DV VALUES 
            (:ma, :loai, SYSDATE, :maktv, :kq)",

        new OracleParameter[]
        {
            new OracleParameter("ma", r.Cells["MAHSBA"].Value),
            new OracleParameter("loai", txtLoaiDV.Text),
            new OracleParameter("maktv", _username),
            new OracleParameter("kq", txtKetQua.Text)
        });

        LoadDichVu();
    }

    //Cho xoa dich vu hsba
    private void btnDeleteDV_Click(object sender, EventArgs e)
    {
        if (dgvDichVu.CurrentRow == null) return;

        var r = dgvDichVu.CurrentRow;

        _db.ExecuteNonQuery(
            $@"DELETE FROM {Schema}.HSBA_DV
            WHERE MAHSBA = :ma AND LOAIDV = :loai",

            new OracleParameter[]
            {
                new OracleParameter("ma", r.Cells["MAHSBA"].Value),
                new OracleParameter("loai", r.Cells["LOAIDV"].Value)
            });

        LoadDichVu();
    }
}