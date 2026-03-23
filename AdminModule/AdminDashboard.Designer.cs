namespace MedicalDataManagement.AdminModule;

partial class AdminDashboard
{
    private System.ComponentModel.IContainer components = null;
    
    // UI Panels for Layout
    private Panel panelSidebar;
    private Panel panelHeader;
    private Panel panelContent;
    
    // Sidebar Buttons (Navigation)
    private Button btnNavUsers;
    private Button btnNavPrivileges;
    private Button btnNavView;
    
    // TabControl for Sections
    private TabControl tabControlMain;
    private TabPage tabUsersRoles;
    private TabPage tabPrivileges;
    private TabPage tabViewPrivileges;

    // Filters & Search
    private TextBox txtQuickSearchUR;
    private CheckBox chkOnlySystem_Priv;

    // Tab 1: Users & Roles
    private DataGridView dgvUsers;
    private DataGridView dgvRoles;
    private Button btnCreateUser;
    private Button btnDropUser;
    private Button btnEditUser;
    private Button btnCreateRole;
    private Button btnDropRole;

    // Tab 2: Privileges
    private ComboBox cbGrantee;
    private ComboBox cbPrivilegeType;
    private ComboBox cbObjectType;
    private ComboBox cbObjectName;
    private CheckedListBox clbColumns;
    private CheckBox chkWithGrantOption;
    private Button btnGrant;
    private Button btnRevoke;

    // Tab 3: View Privileges
    private TextBox txtSearchUserRole;
    private Button btnSearchPrivileges;
    private DataGridView dgvPrivileges;

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
        
        this.btnNavUsers = new Button();
        this.btnNavPrivileges = new Button();
        this.btnNavView = new Button();
        
        this.tabControlMain = new TabControl();
        this.tabUsersRoles = new TabPage();
        this.tabPrivileges = new TabPage();
        this.tabViewPrivileges = new TabPage();

        this.txtQuickSearchUR = new TextBox();
        this.dgvUsers = new DataGridView();
        this.dgvRoles = new DataGridView();
        this.btnCreateUser = new Button();
        this.btnDropUser = new Button();
        this.btnEditUser = new Button();
        this.btnCreateRole = new Button();
        this.btnDropRole = new Button();

        this.cbGrantee = new ComboBox();
        this.cbPrivilegeType = new ComboBox();
        this.cbObjectType = new ComboBox();
        this.cbObjectName = new ComboBox();
        this.clbColumns = new CheckedListBox();
        this.chkWithGrantOption = new CheckBox();
        this.btnGrant = new Button();
        this.btnRevoke = new Button();
        this.chkOnlySystem_Priv = new CheckBox();

        this.txtSearchUserRole = new TextBox();
        this.btnSearchPrivileges = new Button();
        this.dgvPrivileges = new DataGridView();

        this.SuspendLayout();

        // 
        // panelSidebar
        // 
        this.panelSidebar.BackColor = Color.FromArgb(28, 35, 49); 
        this.panelSidebar.Dock = DockStyle.Left;
        this.panelSidebar.Width = 240;
        this.panelSidebar.Controls.Add(this.btnNavView);
        this.panelSidebar.Controls.Add(this.btnNavPrivileges);
        this.panelSidebar.Controls.Add(this.btnNavUsers);
        
        Label lblBrand = new Label() { 
            Text = "DBA COMMANDER", ForeColor = Color.FromArgb(0, 162, 232), 
            Font = new Font("Segoe UI Semibold", 18), 
            Location = new Point(20, 25), AutoSize = true 
        };
        this.panelSidebar.Controls.Add(lblBrand);

        Font navFont = new Font("Segoe UI Semibold", 11);
        StyleNavButton(btnNavUsers, "  Quản lý Người dùng", 100, navFont);
        StyleNavButton(btnNavPrivileges, "  Cấp và Thu hồi Quyền", 160, navFont);
        StyleNavButton(btnNavView, "  Tra cứu Đặc quyền", 220, navFont);

        // Sidebar Navigation Logic
        this.btnNavUsers.Click += (s, e) => { tabControlMain.SelectedIndex = 0; SetNavActive(btnNavUsers); };
        this.btnNavPrivileges.Click += (s, e) => { tabControlMain.SelectedIndex = 1; SetNavActive(btnNavPrivileges); };
        this.btnNavView.Click += (s, e) => { tabControlMain.SelectedIndex = 2; SetNavActive(btnNavView); };

        // 
        // panelHeader
        // 
        this.panelHeader.BackColor = Color.White;
        this.panelHeader.Dock = DockStyle.Top;
        this.panelHeader.Height = 70;
        Label lbTitle = new Label() { 
            Text = "Medical System - Database Administration Dashboard", 
            Font = new Font("Segoe UI", 13), ForeColor = Color.FromArgb(64, 64, 64),
            Location = new Point(20, 22), AutoSize = true 
        };
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
        this.tabControlMain.Controls.Add(this.tabUsersRoles);
        this.tabControlMain.Controls.Add(this.tabPrivileges);
        this.tabControlMain.Controls.Add(this.tabViewPrivileges);
        this.tabControlMain.Dock = DockStyle.Fill;
        this.tabControlMain.Appearance = TabAppearance.FlatButtons;
        this.tabControlMain.ItemSize = new Size(0, 1);
        this.tabControlMain.SizeMode = TabSizeMode.Fixed;

        // --- TAB 1: USERS & ROLES ---
        this.tabUsersRoles.BackColor = Color.FromArgb(244, 246, 249);
        
        Label lblSearchTitle = new Label() { Text = "Tìm kiếm nhanh User / Role:", Location = new Point(25, 20), AutoSize = true, Font = new Font("Segoe UI Semibold", 10) };
        this.txtQuickSearchUR.Location = new Point(25, 45);
        this.txtQuickSearchUR.Size = new Size(350, 30);
        this.txtQuickSearchUR.PlaceholderText = "Nhập tên cần tìm...";
        this.txtQuickSearchUR.Font = new Font("Segoe UI", 11);

        Label lblU = CreateSectionLabel("DANH SÁCH TÀI KHOẢN (USERS)", new Point(25, 90));
        this.dgvUsers.Location = new Point(25, 120);
        this.dgvUsers.Size = new Size(430, 385);

        Label lblR = CreateSectionLabel("DANH SÁCH VAI TRÒ (ROLES)", new Point(485, 90));
        this.dgvRoles.Location = new Point(485, 120);
        this.dgvRoles.Size = new Size(430, 385);

        StyleActionButton(btnCreateUser, "Thêm User", new Point(25, 520), Color.FromArgb(0, 120, 215));
        StyleActionButton(btnEditUser, "Đổi Mật khẩu", new Point(140, 520), Color.FromArgb(255, 140, 0));
        StyleActionButton(btnDropUser, "Xóa User", new Point(255, 520), Color.FromArgb(209, 52, 56));
        StyleActionButton(btnCreateRole, "Thêm Role", new Point(485, 520), Color.FromArgb(0, 120, 215));
        StyleActionButton(btnDropRole, "Xóa Role", new Point(600, 520), Color.FromArgb(209, 52, 56));

        this.tabUsersRoles.Controls.AddRange(new Control[] { lblSearchTitle, txtQuickSearchUR, lblU, lblR, dgvUsers, dgvRoles, btnCreateUser, btnEditUser, btnDropUser, btnCreateRole, btnDropRole });

        // --- TAB 2: PRIVILEGES ---
        this.tabPrivileges.BackColor = Color.FromArgb(244, 246, 249);
        Label lblPTitle = CreateSectionLabel("PHÂN QUYỀN HỆ THỐNG", new Point(25, 25), 15);
        
        int sY = 85; 
        int labelX = 25; int inputX = 300; int inputW = 350;

        AddLabelToTabWithFont(tabPrivileges, "Tên User hoặc Role hưởng quyền:", new Point(labelX, sY), new Font("Segoe UI Semibold", 10));
        SetupSearchableCombo(cbGrantee, new Point(inputX, sY - 4), inputW);

        AddLabelToTabWithFont(tabPrivileges, "Lựa chọn Đặc quyền (Privilege):", new Point(labelX, sY + 50), new Font("Segoe UI Semibold", 10));
        SetupSearchableCombo(cbPrivilegeType, new Point(inputX, sY + 46), inputW);

        AddLabelToTabWithFont(tabPrivileges, "Đối tượng CSDL ứng dụng:", new Point(labelX, sY + 100), new Font("Segoe UI Semibold", 10));
        SetupSearchableCombo(cbObjectType, new Point(inputX, sY + 96), 120);
        SetupSearchableCombo(cbObjectName, new Point(inputX + 130, sY + 96), 220);

        this.chkOnlySystem_Priv.Text = "Ẩn các bảng mặc định của Oracle Engine";
        this.chkOnlySystem_Priv.Checked = true;
        this.chkOnlySystem_Priv.Location = new Point(inputX, sY + 130);
        this.chkOnlySystem_Priv.AutoSize = true;

        AddLabelToTabWithFont(tabPrivileges, "Cột áp dụng (Update):", new Point(labelX, sY + 170), new Font("Segoe UI Semibold", 10));
        this.clbColumns.Location = new Point(inputX, sY + 170);
        this.clbColumns.Size = new Size(inputW, 140);
        this.clbColumns.BorderStyle = BorderStyle.FixedSingle;

        this.chkWithGrantOption.Text = "Cho phép cấp tiếp (WITH GRANT OPTION)";
        this.chkWithGrantOption.Location = new Point(inputX, sY + 325);
        this.chkWithGrantOption.AutoSize = true;

        StyleActionButton(btnGrant, "CẤP QUYỀN", new Point(inputX, sY + 375), Color.FromArgb(16, 124, 16));
        btnGrant.Size = new Size(160, 45);
        StyleActionButton(btnRevoke, "THU HỒI", new Point(inputX + 190, sY + 375), Color.FromArgb(209, 52, 56));
        btnRevoke.Size = new Size(160, 45);

        this.tabPrivileges.Controls.AddRange(new Control[] { lblPTitle, cbGrantee, cbPrivilegeType, cbObjectType, cbObjectName, clbColumns, chkWithGrantOption, btnGrant, btnRevoke, chkOnlySystem_Priv });

        // --- TAB 3: VIEW PRIVILEGES ---
        this.tabViewPrivileges.BackColor = Color.White;
        Label lblVTitle = CreateSectionLabel("TRA CỨU ĐẶC QUYỀN", new Point(25, 25));
        this.txtSearchUserRole.Location = new Point(25, 65);
        this.txtSearchUserRole.Size = new Size(300, 35);
        this.txtSearchUserRole.PlaceholderText = "Nhập tên User/Role...";
        StyleActionButton(btnSearchPrivileges, "KIỂM TRA", new Point(340, 64), Color.FromArgb(0, 120, 215));
        this.dgvPrivileges.Location = new Point(25, 120);
        this.dgvPrivileges.Size = new Size(930, 480);
        this.tabViewPrivileges.Controls.AddRange(new Control[] { lblVTitle, txtSearchUserRole, btnSearchPrivileges, dgvPrivileges });

        // Setup Main Form
        this.ClientSize = new Size(1250, 750);
        this.Controls.Add(this.panelContent);
        this.Controls.Add(this.panelHeader);
        this.Controls.Add(this.panelSidebar);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Text = "Database Management";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ResumeLayout(false);
    }

    private void StyleNavButton(Button btn, string text, int top, Font f)
    {
        btn.Text = text;
        btn.ForeColor = Color.FromArgb(180, 190, 210);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.Dock = DockStyle.Top;
        btn.Height = 60;
        btn.TextAlign = ContentAlignment.MiddleLeft;
        btn.Font = f;
        btn.Cursor = Cursors.Hand;
    }

    private void SetNavActive(Button activeBtn)
    {
        foreach (Control c in panelSidebar.Controls) if (c is Button b) b.BackColor = Color.Transparent;
        activeBtn.BackColor = Color.FromArgb(45, 55, 75);
    }

    private void StyleActionButton(Button btn, string text, Point loc, Color backColor)
    {
        btn.Text = text;
        btn.Location = loc;
        btn.Size = new Size(110, 40);
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

    private void AddLabelToTabWithFont(TabPage tab, string text, Point loc, Font f)
    {
        tab.Controls.Add(new Label() { Text = text, Location = loc, AutoSize = true, Font = f });
    }

    private void SetupSearchableCombo(ComboBox cb, Point loc, int width)
    {
        cb.Location = loc;
        cb.Size = new Size(width, 30);
        cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cb.AutoCompleteSource = AutoCompleteSource.ListItems;
        cb.DropDownStyle = ComboBoxStyle.DropDown;
    }
}
