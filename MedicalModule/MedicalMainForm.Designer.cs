namespace MedicalDataManagement.MedicalModule;

partial class MedicalMainForm
{
    private System.ComponentModel.IContainer components = null;

    // Layout Panels
    private Panel panelSidebar;
    private Panel panelHeader;
    private Panel panelContent;

    // Sidebar Navigation Buttons
    private Button btnNavProfile;
    private Button btnNavTechServices;
    private Button btnNavCoordPatients;
    private Button btnNavCoordRecords;
    private Button btnNavDocRecords;
    private Button btnNavNotifications;

    // Main TabControl
    private TabControl tabControlMain;
    
    // TabPages for different roles
    private TabPage tabProfile;
    private TabPage tabTechServices;
    private TabPage tabCoordPatients;
    private TabPage tabCoordRecords;
    private TabPage tabDocRecords;
    private TabPage tabNotifications;

    // Notifications Controls
    private DataGridView dgvNotifications;

    // Patient Controls
    private DataGridView dgvPatientInfo;
    private TextBox txtPat_ID;
    private TextBox txtPat_Name;
    private TextBox txtPat_Gender;
    private TextBox txtPat_DOB;
    private TextBox txtPat_CCCD;
    private TextBox txtPat_SoNha;
    private TextBox txtPat_Duong;
    private TextBox txtPat_Quan;
    private TextBox txtPat_Tinh;
    private Button btnSaveProfile;

    // Technician Controls
    private DataGridView dgvMyServices;
    private TextBox txtTechResult;
    private Button btnTechSaveResult;

    // Coordinator Controls - Patients
    private DataGridView dgvPatients;
    private TextBox txtCp_ID;
    private TextBox txtCp_Name;
    private ComboBox cbCp_Gender;
    private TextBox txtCp_DOB;
    private TextBox txtCp_CCCD;
    private Button btnCoordAddPatient;

    // Coordinator Controls - Records & Services
    private DataGridView dgvRecordsCoord;
    private DataGridView dgvServicesCoord;
    private TextBox txtCr_RecordID;
    private TextBox txtCr_PatientID;
    private TextBox txtCr_DeptID;
    private TextBox txtCr_DocID;
    private Button btnCoordAddRecord;
    private TextBox txtCs_TechID;
    private Button btnCoordAssignTech;

    // Doctor Controls
    private DataGridView dgvRecordsDoctor;
    private TextBox txtDr_ChanDoan;
    private TextBox txtDr_DieuTri;
    private TextBox txtDr_KetLuan;
    private Button btnDocUpdateRecord;
    
    private DataGridView dgvServicesDoctor;
    private TextBox txtDs_Loai;
    private Button btnDocAddService;

    private DataGridView dgvPrescriptionsDoctor;
    private TextBox txtDp_Ten;
    private TextBox txtDp_Lieu;
    private Button btnDocAddPrescript;

    private DataGridView dgvHistoryDoctor;
    private TextBox txtDh_TienSu;
    private TextBox txtDh_GiaDinh;
    private TextBox txtDh_DiUng;
    private Button btnDocUpdateHistory;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.panelSidebar = new Panel();
        this.panelHeader = new Panel();
        this.panelContent = new Panel();
        
        this.tabControlMain = new TabControl();
        this.tabProfile = new TabPage();
        this.tabTechServices = new TabPage();
        this.tabCoordPatients = new TabPage();
        this.tabCoordRecords = new TabPage();
        this.tabDocRecords = new TabPage();
        this.tabNotifications = new TabPage();

        this.btnNavProfile = new Button();
        this.btnNavTechServices = new Button();
        this.btnNavCoordPatients = new Button();
        this.btnNavCoordRecords = new Button();
        this.btnNavDocRecords = new Button();
        this.btnNavNotifications = new Button();

        this.SuspendLayout();

        // 
        // panelSidebar
        // 
        this.panelSidebar.BackColor = Color.FromArgb(28, 35, 49);
        this.panelSidebar.Dock = DockStyle.Left;
        this.panelSidebar.Width = 240;
        
        Label lblBrand = new Label() { Text = "MEDICAL APP", ForeColor = Color.FromArgb(0, 162, 232), Font = new Font("Segoe UI Semibold", 18), Location = new Point(20, 25), AutoSize = true };
        this.panelSidebar.Controls.Add(lblBrand);

        Font navFont = new Font("Segoe UI Semibold", 11);
        int sy = 90;
        
        StyleNavButton(btnNavProfile, "  Hồ Sơ Của Tôi", sy, navFont); sy += 50;
        StyleNavButton(btnNavTechServices, "  Dịch Vụ Của Tôi", sy, navFont); sy += 50;
        StyleNavButton(btnNavCoordPatients, "  Quản Lý Bệnh Nhân", sy, navFont); sy += 50;
        StyleNavButton(btnNavCoordRecords, "  Quản Lý HSBA & DV", sy, navFont); sy += 50;
        StyleNavButton(btnNavDocRecords, "  Khám Bệnh & HSBA", sy, navFont); sy += 50;
        StyleNavButton(btnNavNotifications, "  Thông Báo", sy, navFont);

        this.panelSidebar.Controls.Add(btnNavProfile);
        this.panelSidebar.Controls.Add(btnNavTechServices);
        this.panelSidebar.Controls.Add(btnNavCoordPatients);
        this.panelSidebar.Controls.Add(btnNavCoordRecords);
        this.panelSidebar.Controls.Add(btnNavDocRecords);
        this.panelSidebar.Controls.Add(btnNavNotifications);

        // 
        // panelHeader
        // 
        this.panelHeader.BackColor = Color.White;
        this.panelHeader.Dock = DockStyle.Top;
        this.panelHeader.Height = 70;
        Label lbTitle = new Label() { Text = "Medical System - Medical Data Management Dashboard", Font = new Font("Segoe UI", 13), ForeColor = Color.FromArgb(64, 64, 64), Location = new Point(20, 22), AutoSize = true };
        this.panelHeader.Controls.Add(lbTitle);

        // 
        // panelContent
        // 
        this.panelContent.Dock = DockStyle.Fill;
        this.panelContent.Controls.Add(this.tabControlMain);
        this.panelContent.BackColor = Color.FromArgb(244, 246, 249);

        // 
        // tabControlMain
        // 
        this.tabControlMain.Controls.Add(this.tabProfile);
        this.tabControlMain.Controls.Add(this.tabTechServices);
        this.tabControlMain.Controls.Add(this.tabCoordPatients);
        this.tabControlMain.Controls.Add(this.tabCoordRecords);
        this.tabControlMain.Controls.Add(this.tabDocRecords);
        this.tabControlMain.Controls.Add(this.tabNotifications);
        this.tabControlMain.Dock = DockStyle.Fill;
        this.tabControlMain.Appearance = TabAppearance.FlatButtons;
        this.tabControlMain.ItemSize = new Size(0, 1);
        this.tabControlMain.SizeMode = TabSizeMode.Fixed;

        // ===================================
        // TAB PROFILE (PATIENT)
        // ===================================
        this.tabProfile.BackColor = Color.FromArgb(244, 246, 249);
        this.tabProfile.Controls.Add(CreateSectionLabel("HỒ SƠ BỆNH NHÂN", new Point(25, 25), 15));
        
        int px = 25, py = 70;
        txtPat_ID = CreateTextBoxRO(px, py, 200, "Mã BN");
        txtPat_Name = CreateTextBoxRO(px + 220, py, 200, "Họ Tên");
        txtPat_Gender = CreateTextBoxRO(px + 440, py, 100, "Phái");
        txtPat_DOB = CreateTextBoxRO(px + 560, py, 150, "Ngày Sinh");
        txtPat_CCCD = CreateTextBoxRO(px, py + 40, 200, "CCCD");
        
        txtPat_SoNha = CreateTextBox(px, py + 100, 200, "Số Nhà");
        txtPat_Duong = CreateTextBox(px + 220, py + 100, 200, "Đường");
        txtPat_Quan = CreateTextBox(px + 440, py + 100, 150, "Quận Huyện");
        txtPat_Tinh = CreateTextBox(px + 610, py + 100, 150, "Tỉnh TP");

        btnSaveProfile = new Button();
        StyleActionButton(btnSaveProfile, "Lưu Hồ Sơ", new Point(px, py + 160), Color.FromArgb(0, 120, 215));

        this.tabProfile.Controls.AddRange(new Control[] { txtPat_ID, txtPat_Name, txtPat_Gender, txtPat_DOB, txtPat_CCCD, txtPat_SoNha, txtPat_Duong, txtPat_Quan, txtPat_Tinh, btnSaveProfile });

        // ===================================
        // TAB TECH SERVICES
        // ===================================
        this.tabTechServices.BackColor = Color.FromArgb(244, 246, 249);
        this.tabTechServices.Controls.Add(CreateSectionLabel("DỊCH VỤ ĐƯỢC PHÂN CÔNG", new Point(25, 25), 15));
        dgvMyServices = new DataGridView();
        dgvMyServices.Location = new Point(25, 70);
        dgvMyServices.Size = new Size(800, 300);
        
        txtTechResult = CreateTextBox(25, 390, 400, "Nhập Kết Quả");
        btnTechSaveResult = new Button();
        StyleActionButton(btnTechSaveResult, "Lưu Kết Quả", new Point(440, 389), Color.FromArgb(16, 124, 16));
        
        this.tabTechServices.Controls.AddRange(new Control[] { dgvMyServices, txtTechResult, btnTechSaveResult });

        // ===================================
        // TAB COORD PATIENTS
        // ===================================
        this.tabCoordPatients.BackColor = Color.FromArgb(244, 246, 249);
        this.tabCoordPatients.Controls.Add(CreateSectionLabel("QUẢN LÝ BỆNH NHÂN", new Point(25, 25), 15));
        dgvPatients = new DataGridView();
        dgvPatients.Location = new Point(25, 70);
        dgvPatients.Size = new Size(800, 300);
        
        txtCp_ID = CreateTextBox(25, 390, 100, "Mã BN");
        txtCp_Name = CreateTextBox(135, 390, 200, "Họ Tên");
        cbCp_Gender = new ComboBox() { Location = new Point(345, 390), Size = new Size(80, 25), DropDownStyle = ComboBoxStyle.DropDownList };
        cbCp_Gender.Items.AddRange(new string[] { "Nam", "Nữ" }); if (cbCp_Gender.Items.Count > 0) cbCp_Gender.SelectedIndex = 0;
        txtCp_DOB = CreateTextBox(435, 390, 100, "YYYY-MM-DD");
        txtCp_CCCD = CreateTextBox(545, 390, 100, "CCCD");
        
        btnCoordAddPatient = new Button();
        StyleActionButton(btnCoordAddPatient, "Thêm Mới", new Point(660, 389), Color.FromArgb(0, 120, 215));

        this.tabCoordPatients.Controls.AddRange(new Control[] { dgvPatients, txtCp_ID, txtCp_Name, cbCp_Gender, txtCp_DOB, txtCp_CCCD, btnCoordAddPatient });

        // ===================================
        // TAB COORD RECORDS
        // ===================================
        this.tabCoordRecords.BackColor = Color.FromArgb(244, 246, 249);
        this.tabCoordRecords.Controls.Add(CreateSectionLabel("QUẢN LÝ HỒ SƠ & DỊCH VỤ", new Point(25, 25), 15));
        
        dgvRecordsCoord = new DataGridView(); dgvRecordsCoord.Location = new Point(25, 60); dgvRecordsCoord.Size = new Size(400, 250);
        txtCr_RecordID = CreateTextBox(25, 320, 80, "Mã HS");
        txtCr_PatientID = CreateTextBox(115, 320, 80, "Mã BN");
        txtCr_DeptID = CreateTextBox(205, 320, 80, "Mã Khoa");
        txtCr_DocID = CreateTextBox(295, 320, 80, "Mã BS");
        btnCoordAddRecord = new Button(); StyleActionButton(btnCoordAddRecord, "Thêm", new Point(385, 319), Color.FromArgb(0, 120, 215));

        dgvServicesCoord = new DataGridView(); dgvServicesCoord.Location = new Point(480, 60); dgvServicesCoord.Size = new Size(400, 250);
        txtCs_TechID = CreateTextBox(480, 320, 150, "Mã KTV");
        btnCoordAssignTech = new Button(); StyleActionButton(btnCoordAssignTech, "Phân Công", new Point(640, 319), Color.FromArgb(255, 140, 0));

        this.tabCoordRecords.Controls.AddRange(new Control[] { dgvRecordsCoord, txtCr_RecordID, txtCr_PatientID, txtCr_DeptID, txtCr_DocID, btnCoordAddRecord, dgvServicesCoord, txtCs_TechID, btnCoordAssignTech });

        // ===================================
        // TAB DOCTOR RECORDS
        // ===================================
        this.tabDocRecords.BackColor = Color.FromArgb(244, 246, 249);
        this.tabDocRecords.Controls.Add(CreateSectionLabel("KHÁM BỆNH & CHỈ ĐỊNH", new Point(25, 20), 15));
        
        dgvRecordsDoctor = new DataGridView(); dgvRecordsDoctor.Location = new Point(25, 50); dgvRecordsDoctor.Size = new Size(450, 200);
        txtDr_ChanDoan = CreateTextBox(25, 260, 140, "Chẩn Đoán");
        txtDr_DieuTri = CreateTextBox(175, 260, 140, "Điều Trị");
        txtDr_KetLuan = CreateTextBox(325, 260, 140, "Kết Luận");
        btnDocUpdateRecord = new Button(); StyleActionButton(btnDocUpdateRecord, "Cập Nhật HS", new Point(25, 295), Color.FromArgb(0, 120, 215));

        dgvHistoryDoctor = new DataGridView(); dgvHistoryDoctor.Location = new Point(500, 50); dgvHistoryDoctor.Size = new Size(450, 200);
        txtDh_TienSu = CreateTextBox(500, 260, 140, "Tiền sử bệnh");
        txtDh_GiaDinh = CreateTextBox(650, 260, 140, "Bệnh gia đình");
        txtDh_DiUng = CreateTextBox(800, 260, 140, "Dị ứng thuốc");
        btnDocUpdateHistory = new Button(); StyleActionButton(btnDocUpdateHistory, "Cập Nhật TS", new Point(500, 295), Color.FromArgb(255, 140, 0));

        dgvServicesDoctor = new DataGridView(); dgvServicesDoctor.Location = new Point(25, 360); dgvServicesDoctor.Size = new Size(450, 200);
        txtDs_Loai = CreateTextBox(25, 570, 200, "Loại DV");
        btnDocAddService = new Button(); StyleActionButton(btnDocAddService, "Thêm Dịch Vụ", new Point(235, 569), Color.FromArgb(0, 120, 215));

        dgvPrescriptionsDoctor = new DataGridView(); dgvPrescriptionsDoctor.Location = new Point(500, 360); dgvPrescriptionsDoctor.Size = new Size(450, 200);
        txtDp_Ten = CreateTextBox(500, 570, 150, "Tên Thuốc");
        txtDp_Lieu = CreateTextBox(660, 570, 150, "Liều Dùng");
        btnDocAddPrescript = new Button(); StyleActionButton(btnDocAddPrescript, "Thêm Đơn", new Point(820, 569), Color.FromArgb(0, 120, 215));

        this.tabDocRecords.Controls.AddRange(new Control[] { 
            dgvRecordsDoctor, txtDr_ChanDoan, txtDr_DieuTri, txtDr_KetLuan, btnDocUpdateRecord,
            dgvHistoryDoctor, txtDh_TienSu, txtDh_GiaDinh, txtDh_DiUng, btnDocUpdateHistory,
            dgvServicesDoctor, txtDs_Loai, btnDocAddService,
            dgvPrescriptionsDoctor, txtDp_Ten, txtDp_Lieu, btnDocAddPrescript
        });

        // ===================================
        // TAB NOTIFICATIONS
        // ===================================
        this.tabNotifications.BackColor = Color.FromArgb(244, 246, 249);
        this.tabNotifications.Controls.Add(CreateSectionLabel("THÔNG BÁO HỆ THỐNG", new Point(25, 25), 15));
        dgvNotifications = new DataGridView();
        dgvNotifications.Location = new Point(25, 70);
        dgvNotifications.Size = new Size(950, 500);
        this.tabNotifications.Controls.Add(dgvNotifications);
        dgvPatientInfo = new DataGridView(); // unused graphic

        // Form Setup
        this.ClientSize = new Size(1250, 750);
        this.Controls.Add(this.panelContent);
        this.Controls.Add(this.panelHeader);
        this.Controls.Add(this.panelSidebar);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Text = "Medical Data Management";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ResumeLayout(false);
    }

    private void StyleNavButton(Button btn, string text, int top, Font f)
    {
        btn.Text = text;
        btn.Location = new Point(0, top);
        btn.Size = new Size(240, 50);
        btn.ForeColor = Color.FromArgb(180, 190, 210);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.Font = f;
        btn.Cursor = Cursors.Hand;
        btn.Visible = false; // Hide by default
    }

    private void StyleActionButton(Button btn, string text, Point loc, Color backColor)
    {
        btn.Text = text;
        btn.Location = loc;
        btn.Size = new Size(110, 30);
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = backColor;
        btn.ForeColor = Color.White;
        btn.FlatAppearance.BorderSize = 0;
        btn.Cursor = Cursors.Hand;
    }

    private Label CreateSectionLabel(string text, Point loc, int size = 11)
    {
        return new Label() { Text = text, Location = loc, AutoSize = true, Font = new Font("Segoe UI Bold", size) };
    }

    private TextBox CreateTextBoxRO(int x, int y, int w, string placeholder)
    {
        return new TextBox() { Location = new Point(x, y), Size = new Size(w, 30), ReadOnly = true, PlaceholderText = placeholder, Font = new Font("Segoe UI", 10) };
    }

    private TextBox CreateTextBox(int x, int y, int w, string placeholder)
    {
        return new TextBox() { Location = new Point(x, y), Size = new Size(w, 30), PlaceholderText = placeholder, Font = new Font("Segoe UI", 10) };
    }
}
