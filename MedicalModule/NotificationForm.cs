using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalDataManagement.MedicalModule;

public class NotificationForm : Form
{
    private readonly DatabaseService _db;
    private readonly string _username;
    private readonly string _schema = "QLBENHVIEN";

    // UI Components
    private Panel headerPanel = null!;
    private Panel infoCard = null!;
    private Label lblUserRole = null!;
    private Label lblClearance = null!;
    private DataGridView dgvNoti = null!;
    private Label lblStats = null!;
    private Button btnRefresh = null!;

    // Descriptions based on image requirement
    private static readonly Dictionary<string, string> UserRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        ["u1"] = "Ban Giám Đốc - Có thể đọc được toàn bộ thông báo",
        ["u2"] = "Lãnh đạo Khoa tim mạch tại Hồ Chí Minh",
        ["u3"] = "Lãnh đạo Khoa thần kinh tại Hà Nội",
        ["u4"] = "Nhân viên thuộc Khoa thần kinh tại Hồ Chí Minh",
        ["u5"] = "Nhân viên thuộc Khoa tim mạch tại Hồ Chí Minh",
        ["u6"] = "Lãnh đạo phòng - Đọc thông báo Khoa tim mạch tại HCM",
        ["u7"] = "Lãnh đạo phòng - Đọc toàn bộ thông báo cấp Lãnh đạo",
        ["u8"] = "Nhân viên thuộc Khoa Tiêu hóa tại Hà Nội"
    };

    public NotificationForm(DatabaseService db, string username)
    {
        _db = db;
        _username = username;

        this.Text = "Hệ thống Thông báo OLS Security";
        this.Size = new Size(1100, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(244, 246, 249);

        SetupUI();
        this.Shown += async (s, e) => await LoadDataAsync();
    }

    private void SetupUI()
    {
        // 1. Header
        headerPanel = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(0, 120, 215) };
        var lblTitle = new Label {
            Text = "📋 CƠ CHẾ PHÁT TÁN THÔNG BÁO (OLS)",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Bold", 16),
            Location = new Point(20, 20),
            AutoSize = true
        };
        headerPanel.Controls.Add(lblTitle);
        this.Controls.Add(headerPanel);

        // 2. Info Card (Clearance Info)
        infoCard = new Panel {
            Location = new Point(20, 90),
            Size = new Size(1040, 100),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        
        lblUserRole = new Label {
            Text = $"👤 Người dùng: {_username.ToUpper()}",
            Font = new Font("Segoe UI Bold", 12),
            Location = new Point(15, 15),
            AutoSize = true
        };
        
        string roleDesc = UserRoles.TryGetValue(_username, out var desc) ? desc : "Không xác định";
        lblClearance = new Label {
            Text = $"ℹ️ Quyền hạn: {roleDesc}",
            Font = new Font("Segoe UI", 11),
            ForeColor = Color.FromArgb(64, 64, 64),
            Location = new Point(15, 45),
            AutoSize = true,
            MaximumSize = new Size(1000, 0)
        };

        infoCard.Controls.Add(lblUserRole);
        infoCard.Controls.Add(lblClearance);
        this.Controls.Add(infoCard);

        // 3. Grid
        dgvNoti = new DataGridView {
            Location = new Point(20, 210),
            Size = new Size(1040, 380),
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false
        };
        dgvNoti.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
        dgvNoti.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvNoti.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Bold", 10);
        dgvNoti.ColumnHeadersHeight = 40;
        dgvNoti.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
        this.Controls.Add(dgvNoti);

        // 4. Stats & Refresh
        lblStats = new Label {
            Text = "Đang tải dữ liệu...",
            Location = new Point(20, 610),
            Font = new Font("Segoe UI Italic", 10),
            AutoSize = true
        };
        this.Controls.Add(lblStats);

        btnRefresh = new Button {
            Text = "🔄 Refresh Data",
            Location = new Point(940, 605),
            Size = new Size(120, 35),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnRefresh.FlatAppearance.BorderSize = 0;
        btnRefresh.Click += async (s, e) => await LoadDataAsync();
        this.Controls.Add(btnRefresh);

        var lblExplain = new Label {
            Text = "* Ghi chú: Kết quả hiển thị là kết quả của lệnh 'SELECT * FROM THONGBAO'. Oracle tự động lọc dòng dựa trên OLS.",
            Location = new Point(20, 635),
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Gray,
            AutoSize = true
        };
        this.Controls.Add(lblExplain);
    }

    private async Task LoadDataAsync()
    {
        btnRefresh.Enabled = false;
        lblStats.Text = "⏳ Đang truy vấn từ Database (SELECT * FROM THONGBAO)...";

        try
        {
            // OLS enforced at Kernel level - no WHERE clause needed in code!
            DataTable dt = await Task.Run(() => _db.ExecuteQuery($"SELECT NOIDUNG, NGAYGIO, DIADIEM FROM {_schema}.THONGBAO"));
            
            dgvNoti.DataSource = dt;
            
            // Format columns
            if (dgvNoti.Columns.Contains("NOIDUNG")) dgvNoti.Columns["NOIDUNG"].HeaderText = "Nội Dung Thông Báo";
            if (dgvNoti.Columns.Contains("NGAYGIO")) dgvNoti.Columns["NGAYGIO"].HeaderText = "Thời Gian";
            if (dgvNoti.Columns.Contains("DIADIEM")) dgvNoti.Columns["DIADIEM"].HeaderText = "Địa Điểm";

            lblStats.Text = $"✅ Thành công: Tìm thấy {dt.Rows.Count} thông báo phù hợp với nhãn của user {_username.ToUpper()}.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi truy vấn: {ex.Message}", "Lỗi OLS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStats.Text = "❌ Lỗi truy vấn.";
        }
        finally
        {
            btnRefresh.Enabled = true;
        }
    }
}
