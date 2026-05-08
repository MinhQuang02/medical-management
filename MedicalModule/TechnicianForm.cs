using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace MedicalDataManagement.MedicalModule;

public class TechnicianForm : Form
{
    private DatabaseService _db;
    private string _username;
    public string Schema { get; set; } = "QLBENHVIEN";

    private DataGridView dgvServices = null!;
    private TextBox txtResult = null!;
    private Button btnSave = null!;

    private TextBox txtMaNV = null!;
    private TextBox txtHoTen = null!;
    private TextBox txtPhai = null!;
    private TextBox txtNgaySinh = null!;
    private TextBox txtCMND = null!;
    private TextBox txtQueQuan = null!;
    private TextBox txtSoDT = null!;
    private Button btnSavePers = null!;

    public TechnicianForm(DatabaseService db, string username)
    {
        _db = db;
        _username = username;

        this.Text = "Technician Form";
        this.Size = new Size(850, 700);

        InitializeUI();
        LoadData();
    }

    private void InitializeUI()
    {
        Label lblTitle = new Label()
        {
            Text = "Các Dịch Vụ Được Phân Công",
            Location = new Point(20, 20),
            AutoSize = true,
            Font = new Font("Arial", 14, FontStyle.Bold)
        };
        this.Controls.Add(lblTitle);

        dgvServices = new DataGridView()
        {
            Location = new Point(20, 60),
            Size = new Size(780, 260),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AllowUserToAddRows = false
        };
        this.Controls.Add(dgvServices);

        Label lblResult = new Label()
        {
            Text = "Kết quả:",
            Location = new Point(20, 340),
            AutoSize = true
        };
        this.Controls.Add(lblResult);

        txtResult = new TextBox()
        {
            Location = new Point(100, 335),
            Size = new Size(400, 30)
        };
        this.Controls.Add(txtResult);

        btnSave = new Button()
        {
            Text = "Lưu Kết Quả",
            Location = new Point(520, 333),
            Size = new Size(140, 35)
        };
        btnSave.Click += BtnSave_Click;
        this.Controls.Add(btnSave);

        Label lblPers = new Label()
        {
            Text = "Thông Tin Cá Nhân",
            Location = new Point(20, 390),
            AutoSize = true,
            Font = new Font("Arial", 14, FontStyle.Bold)
        };
        this.Controls.Add(lblPers);

        Label lblMa = new Label()
        {
            Text = "Mã NV:",
            Location = new Point(20, 440),
            AutoSize = true
        };
        this.Controls.Add(lblMa);

        txtMaNV = new TextBox()
        {
            Location = new Point(120, 435),
            Size = new Size(180, 30),
            ReadOnly = true
        };
        this.Controls.Add(txtMaNV);

        Label lblTen = new Label()
        {
            Text = "Họ Tên:",
            Location = new Point(330, 440),
            AutoSize = true
        };
        this.Controls.Add(lblTen);

        txtHoTen = new TextBox()
        {
            Location = new Point(420, 435),
            Size = new Size(200, 30),
            ReadOnly = true
        };
        this.Controls.Add(txtHoTen);

        Label lblPhai = new Label()
        {
            Text = "Phái:",
            Location = new Point(20, 480),
            AutoSize = true
        };
        this.Controls.Add(lblPhai);

        txtPhai = new TextBox()
        {
            Location = new Point(120, 475),
            Size = new Size(180, 30),
            ReadOnly = true
        };
        this.Controls.Add(txtPhai);

        Label lblNgaySinh = new Label()
        {
            Text = "Ngày Sinh:",
            Location = new Point(330, 480),
            AutoSize = true
        };
        this.Controls.Add(lblNgaySinh);

        txtNgaySinh = new TextBox()
        {
            Location = new Point(420, 475),
            Size = new Size(200, 30),
            ReadOnly = true
        };
        this.Controls.Add(txtNgaySinh);

        Label lblCCCD = new Label()
        {
            Text = "CCCD:",
            Location = new Point(20, 520),
            AutoSize = true
        };
        this.Controls.Add(lblCCCD);

        txtCMND = new TextBox()
        {
            Location = new Point(120, 515),
            Size = new Size(180, 30),
            ReadOnly = true
        };
        this.Controls.Add(txtCMND);

        Label lblQQ = new Label()
        {
            Text = "Quê Quán:",
            Location = new Point(330, 520),
            AutoSize = true
        };
        this.Controls.Add(lblQQ);

        txtQueQuan = new TextBox()
        {
            Location = new Point(420, 515),
            Size = new Size(200, 30)
        };
        this.Controls.Add(txtQueQuan);

        Label lblSDT = new Label()
        {
            Text = "SĐT:",
            Location = new Point(20, 560),
            AutoSize = true
        };
        this.Controls.Add(lblSDT);

        txtSoDT = new TextBox()
        {
            Location = new Point(120, 555),
            Size = new Size(180, 30)
        };
        this.Controls.Add(txtSoDT);

        btnSavePers = new Button()
        {
            Text = "Cập Nhật",
            Location = new Point(420, 555),
            Size = new Size(120, 35)
        };
        btnSavePers.Click += BtnSavePers_Click;
        this.Controls.Add(btnSavePers);
    }

    private void LoadData()
    {
        try
        {
            dgvServices.DataSource = _db.ExecuteQuery(
                $"SELECT * FROM {Schema}.KTV_XEMHSBA_DV"
            );

            System.Data.DataTable dt = _db.ExecuteQuery(
                $@"SELECT MANV, HOTEN, PHAI, NGAYSINH, CMND, QUEQUAN, SODT
                   FROM {Schema}.KTV_XEMTTCANHAN
                   WHERE MANV = :manv",
                new OracleParameter[]
                {
                    new OracleParameter("manv", _username)
                });

            if (dt.Rows.Count > 0)
            {
                txtMaNV.Text = dt.Rows[0]["MANV"].ToString();
                txtHoTen.Text = dt.Rows[0]["HOTEN"].ToString();
                txtPhai.Text = dt.Rows[0]["PHAI"].ToString();
                txtNgaySinh.Text = Convert.ToDateTime(dt.Rows[0]["NGAYSINH"]).ToString("dd/MM/yyyy");
                txtCMND.Text = dt.Rows[0]["CMND"].ToString();
                txtQueQuan.Text = dt.Rows[0]["QUEQUAN"].ToString();
                txtSoDT.Text = dt.Rows[0]["SODT"].ToString();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void BtnSavePers_Click(object? sender, EventArgs e)
    {
        try
        {
            _db.ExecuteNonQuery(
                $@"UPDATE {Schema}.NHANVIEN
                   SET QUEQUAN = :qq,
                       SODT = :sdt
                   WHERE MANV = :manv",
                new OracleParameter[]
                {
                    new OracleParameter("qq", txtQueQuan.Text),
                    new OracleParameter("sdt", txtSoDT.Text),
                    new OracleParameter("manv", _username)
                });

            MessageBox.Show("Cập nhật thành công!");
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (dgvServices.SelectedRows.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn dịch vụ.");
            return;
        }

        var row = dgvServices.SelectedRows[0];

        string ma = row.Cells["MAHSBA"].Value.ToString()!;
        string loai = row.Cells["LOAIDV"].Value.ToString()!;

        try
        {
            _db.ExecuteNonQuery(
                $@"UPDATE {Schema}.KTV_XEMHSBA_DV
                   SET KETQUA = :kq
                   WHERE MAHSBA = :ma
                   AND LOAIDV = :loai",
                new OracleParameter[]
                {
                    new OracleParameter("kq", txtResult.Text),
                    new OracleParameter("ma", ma),
                    new OracleParameter("loai", loai)
                });

            MessageBox.Show("Lưu kết quả thành công!");
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}