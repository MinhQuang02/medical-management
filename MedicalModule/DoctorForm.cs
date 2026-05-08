using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MedicalDataManagement.MedicalModule;

public partial class DoctorForm : Form
{
    private DatabaseService _db;
    private string _username;
    public string Schema { get; set; } = "QLBENHVIEN";

    private Label lblStatus = null!;

    public DoctorForm(DatabaseService db, string username)
    {
        InitializeComponent();

        _db = db;
        _username = username;

        lblStatus = new Label
        {
            Text = "⏳ Đang tải dữ liệu...",
            Dock = DockStyle.Bottom,
            Height = 24,
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.DimGray
        };

        Controls.Add(lblStatus);

        Shown += async (s, e) => await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        lblStatus.Text = "⏳ Đang tải dữ liệu...";

        await SafeLoadAsync(
            () => _db.ExecuteQuery($"SELECT * FROM {Schema}.NHANVIEN"),
            dgvNhanVien,
            "Nhân Viên"
        );

        await SafeLoadAsync(
            () => _db.ExecuteQuery($"SELECT * FROM {Schema}.HSBA"),
            dgvHSBA,
            "Hồ Sơ Bệnh Án"
        );

        await SafeLoadAsync(
            () => _db.ExecuteQuery($"SELECT * FROM {Schema}.BENHNHAN"),
            dgvBenhNhan,
            "Bệnh Nhân"
        );

        await SafeLoadAsync(
            () => _db.ExecuteQuery($"SELECT * FROM {Schema}.DONTHUOC"),
            dgvDonThuoc,
            "Đơn Thuốc"
        );

        lblStatus.Text = "✅ Tải xong.";
    }

    private async Task SafeLoadAsync(
        Func<DataTable> query,
        DataGridView dgv,
        string section)
    {
        try
        {
            DataTable dt = await Task.Run(query);

            dgv.Invoke(() =>
            {
                dgv.DataSource = dt;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Lỗi tải [{section}]: {ex.Message.Split('\n')[0]}",
                "Cảnh báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    private async void btnUpdateNV_Click(object sender, EventArgs e)
    {
        if (dgvNhanVien.CurrentRow == null) return;

        var row = dgvNhanVien.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"UPDATE {Schema}.NHANVIEN
                       SET QUEQUAN = :qq,
                           SODT = :sdt
                       WHERE MANV = :id",
                    new OracleParameter[]
                    {
                        new("qq", row.Cells["QUEQUAN"].Value?.ToString()),
                        new("sdt", row.Cells["SODT"].Value?.ToString()),
                        new("id", row.Cells["MANV"].Value?.ToString())
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.NHANVIEN"),
                dgvNhanVien,
                "Nhân Viên"
            );

            MessageBox.Show("Cập nhật thông tin thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnUpdateHSBA_Click(object sender, EventArgs e)
    {
        if (dgvHSBA.CurrentRow == null) return;

        var r = dgvHSBA.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"UPDATE {Schema}.HSBA
                       SET CHANDOAN = :cd,
                           DIEUTRI = :dt,
                           KETLUAN = :kl
                       WHERE MAHSBA = :id",
                    new OracleParameter[]
                    {
                        new("cd", r.Cells["CHANDOAN"].Value?.ToString()),
                        new("dt", r.Cells["DIEUTRI"].Value?.ToString()),
                        new("kl", r.Cells["KETLUAN"].Value?.ToString()),
                        new("id", r.Cells["MAHSBA"].Value?.ToString())
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.HSBA"),
                dgvHSBA,
                "HSBA"
            );

            MessageBox.Show("Cập nhật HSBA thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnUpdateBN_Click(object sender, EventArgs e)
    {
        if (dgvBenhNhan.CurrentRow == null) return;

        var r = dgvBenhNhan.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"UPDATE {Schema}.BENHNHAN
                       SET TIENSUBENH = :ts,
                           TIENSUBENHGD = :gd,
                           DIUNGTHUOC = :du
                       WHERE MABN = :id",
                    new OracleParameter[]
                    {
                        new("ts", r.Cells["TIENSUBENH"].Value?.ToString()),
                        new("gd", r.Cells["TIENSUBENHGD"].Value?.ToString()),
                        new("du", r.Cells["DIUNGTHUOC"].Value?.ToString()),
                        new("id", r.Cells["MABN"].Value?.ToString())
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.BENHNHAN"),
                dgvBenhNhan,
                "Bệnh Nhân"
            );

            MessageBox.Show("Cập nhật bệnh nhân thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnAddDT_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaHSBA.Text)) return;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"INSERT INTO {Schema}.DONTHUOC
                       (MAHSBA, NGAYDT, TENTHUOC, LIEUDUNG)
                       VALUES (:ma, SYSDATE, :ten, :lieu)",
                    new OracleParameter[]
                    {
                        new("ma", txtMaHSBA.Text),
                        new("ten", txtTenThuoc.Text),
                        new("lieu", txtLieuDung.Text)
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.DONTHUOC"),
                dgvDonThuoc,
                "Đơn Thuốc"
            );

            MessageBox.Show("Thêm đơn thuốc thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnUpdateDT_Click(object sender, EventArgs e)
    {
        if (dgvDonThuoc.CurrentRow == null) return;

        var r = dgvDonThuoc.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"UPDATE {Schema}.DONTHUOC
                       SET LIEUDUNG = :lieu
                       WHERE MAHSBA = :ma
                       AND NGAYDT = :ng
                       AND TENTHUOC = :tt",
                    new OracleParameter[]
                    {
                        new("lieu", txtLieuDung.Text),
                        new("ma", r.Cells["MAHSBA"].Value?.ToString()),
                        new OracleParameter("ng", OracleDbType.Date)
                        {
                            Value = Convert.ToDateTime(r.Cells["NGAYDT"].Value)
                        },
                        new("tt", r.Cells["TENTHUOC"].Value?.ToString())
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.DONTHUOC"),
                dgvDonThuoc,
                "Đơn Thuốc"
            );

            MessageBox.Show("Cập nhật đơn thuốc thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private void dgvDonThuoc_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dgvDonThuoc.CurrentRow == null) return;

        var r = dgvDonThuoc.CurrentRow;

        txtMaHSBA.Text = r.Cells["MAHSBA"].Value?.ToString();
        txtTenThuoc.Text = r.Cells["TENTHUOC"].Value?.ToString();
        txtLieuDung.Text = r.Cells["LIEUDUNG"].Value?.ToString();
    }

    private async void btnDeleteDT_Click(object sender, EventArgs e)
    {
        if (dgvDonThuoc.CurrentRow == null) return;

        var r = dgvDonThuoc.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"DELETE FROM {Schema}.DONTHUOC
                       WHERE MAHSBA = :ma
                       AND NGAYDT = :ng
                       AND TENTHUOC = :tt",
                    new OracleParameter[]
                    {
                        new("ma", r.Cells["MAHSBA"].Value?.ToString()),
                        new OracleParameter("ng", OracleDbType.Date)
                        {
                            Value = Convert.ToDateTime(r.Cells["NGAYDT"].Value)
                        },
                        new("tt", r.Cells["TENTHUOC"].Value?.ToString())
                    }
                )
            );

            await SafeLoadAsync(
                () => _db.ExecuteQuery($"SELECT * FROM {Schema}.DONTHUOC"),
                dgvDonThuoc,
                "Đơn Thuốc"
            );

            MessageBox.Show("Xóa đơn thuốc thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnAddDichVu_Click(object sender, EventArgs e)
    {
        if (dgvHSBA.CurrentRow == null)
        {
            MessageBox.Show("Chọn hồ sơ bệnh án.");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtLoaiDV.Text))
        {
            MessageBox.Show("Nhập loại dịch vụ.");
            return;
        }

        var r = dgvHSBA.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"INSERT INTO {Schema}.HSBA_DV
                       (MAHSBA, LOAIDV, NGAYDV, MAKTV, KETQUA)
                       VALUES (:ma, :loai, SYSDATE, NULL, NULL)",
                    new OracleParameter[]
                    {
                        new("ma", r.Cells["MAHSBA"].Value?.ToString()),
                        new("loai", txtLoaiDV.Text)
                    }
                )
            );

            MessageBox.Show("Thêm dịch vụ thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }

    private async void btnDeleteDV_Click(object sender, EventArgs e)
    {
        if (dgvDichVu.CurrentRow == null) return;

        var r = dgvDichVu.CurrentRow;

        try
        {
            await Task.Run(() =>
                _db.ExecuteNonQuery(
                    $@"DELETE FROM {Schema}.HSBA_DV
                       WHERE MAHSBA = :ma
                       AND LOAIDV = :loai
                       AND NGAYDV = :ngay",
                    new OracleParameter[]
                    {
                        new("ma", r.Cells["MAHSBA"].Value?.ToString()),
                        new("loai", r.Cells["LOAIDV"].Value?.ToString()),
                        new OracleParameter("ngay", OracleDbType.Date)
                        {
                            Value = Convert.ToDateTime(r.Cells["NGAYDV"].Value)
                        }
                    }
                )
            );

            MessageBox.Show("Xóa dịch vụ thành công!");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message.Split('\n')[0]);
        }
    }
}