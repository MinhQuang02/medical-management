namespace MedicalDataManagement.AdminModule;

partial class AdminDashboard
{
    private System.ComponentModel.IContainer components = null;
    private TabControl tabControlMain;
    private TabPage tabUsersRoles;
    private TabPage tabPrivileges;
    private TabPage tabViewPrivileges;

    // Tab 1: Users & Roles
    private DataGridView dgvUsers;
    private DataGridView dgvRoles;
    private Button btnCreateUser;
    private Button btnDropUser;
    private Button btnEditUser;
    private Button btnCreateRole;
    private Button btnDropRole;
    private Label lblUsers;
    private Label lblRoles;

    // Tab 2: Privileges
    private ComboBox cbGrantee;
    private ComboBox cbPrivilegeType;
    private ComboBox cbObjectType;
    private ComboBox cbObjectName;
    private CheckedListBox clbColumns;
    private CheckBox chkWithGrantOption;
    private Button btnGrant;
    private Button btnRevoke;
    private Label lblGrantee;
    private Label lblPrivilegeType;

    // Tab 3: View Privileges
    private TextBox txtSearchUserRole;
    private Button btnSearchPrivileges;
    private DataGridView dgvPrivileges;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.tabControlMain = new TabControl();
        this.tabUsersRoles = new TabPage();
        this.tabPrivileges = new TabPage();
        this.tabViewPrivileges = new TabPage();

        this.dgvUsers = new DataGridView();
        this.dgvRoles = new DataGridView();
        this.btnCreateUser = new Button();
        this.btnDropUser = new Button();
        this.btnEditUser = new Button();
        this.btnCreateRole = new Button();
        this.btnDropRole = new Button();
        this.lblUsers = new Label();
        this.lblRoles = new Label();

        this.cbGrantee = new ComboBox();
        this.cbPrivilegeType = new ComboBox();
        this.cbObjectType = new ComboBox();
        this.cbObjectName = new ComboBox();
        this.clbColumns = new CheckedListBox();
        this.chkWithGrantOption = new CheckBox();
        this.btnGrant = new Button();
        this.btnRevoke = new Button();
        this.lblGrantee = new Label();
        this.lblPrivilegeType = new Label();

        this.txtSearchUserRole = new TextBox();
        this.btnSearchPrivileges = new Button();
        this.dgvPrivileges = new DataGridView();

        this.tabControlMain.SuspendLayout();
        this.tabUsersRoles.SuspendLayout();
        this.tabPrivileges.SuspendLayout();
        this.tabViewPrivileges.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.dgvUsers).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.dgvRoles).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.dgvPrivileges).BeginInit();
        this.SuspendLayout();

        // 
        // tabControlMain
        // 
        this.tabControlMain.Controls.Add(this.tabUsersRoles);
        this.tabControlMain.Controls.Add(this.tabPrivileges);
        this.tabControlMain.Controls.Add(this.tabViewPrivileges);
        this.tabControlMain.Dock = DockStyle.Fill;
        this.tabControlMain.Location = new System.Drawing.Point(0, 0);
        this.tabControlMain.Name = "tabControlMain";
        this.tabControlMain.SelectedIndex = 0;
        this.tabControlMain.Size = new System.Drawing.Size(900, 600);

        // 
        // tabUsersRoles
        // 
        this.tabUsersRoles.Controls.Add(this.lblUsers);
        this.tabUsersRoles.Controls.Add(this.dgvUsers);
        this.tabUsersRoles.Controls.Add(this.btnCreateUser);
        this.tabUsersRoles.Controls.Add(this.btnEditUser);
        this.tabUsersRoles.Controls.Add(this.btnDropUser);
        this.tabUsersRoles.Controls.Add(this.lblRoles);
        this.tabUsersRoles.Controls.Add(this.dgvRoles);
        this.tabUsersRoles.Controls.Add(this.btnCreateRole);
        this.tabUsersRoles.Controls.Add(this.btnDropRole);
        this.tabUsersRoles.Location = new System.Drawing.Point(4, 24);
        this.tabUsersRoles.Name = "tabUsersRoles";
        this.tabUsersRoles.Padding = new Padding(3);
        this.tabUsersRoles.Size = new System.Drawing.Size(892, 572);
        this.tabUsersRoles.Text = "Quản lý User & Role";
        this.tabUsersRoles.UseVisualStyleBackColor = true;

        // lblUsers
        this.lblUsers.AutoSize = true;
        this.lblUsers.Location = new System.Drawing.Point(20, 20);
        this.lblUsers.Text = "Danh sách User:";

        // dgvUsers
        this.dgvUsers.Location = new System.Drawing.Point(20, 45);
        this.dgvUsers.Size = new System.Drawing.Size(400, 450);

        // Buttons User
        this.btnCreateUser.Location = new System.Drawing.Point(20, 510);
        this.btnCreateUser.Text = "Tạo User";
        this.btnEditUser.Location = new System.Drawing.Point(120, 510);
        this.btnEditUser.Text = "Sửa User";
        this.btnDropUser.Location = new System.Drawing.Point(220, 510);
        this.btnDropUser.Text = "Xóa User";

        // lblRoles
        this.lblRoles.AutoSize = true;
        this.lblRoles.Location = new System.Drawing.Point(450, 20);
        this.lblRoles.Text = "Danh sách Role:";

        // dgvRoles
        this.dgvRoles.Location = new System.Drawing.Point(450, 45);
        this.dgvRoles.Size = new System.Drawing.Size(400, 450);

        // Buttons Role
        this.btnCreateRole.Location = new System.Drawing.Point(450, 510);
        this.btnCreateRole.Text = "Tạo Role";
        this.btnDropRole.Location = new System.Drawing.Point(550, 510);
        this.btnDropRole.Text = "Xóa Role";

        // 
        // tabPrivileges
        // 
        this.tabPrivileges.Controls.Add(this.lblGrantee);
        this.tabPrivileges.Controls.Add(this.cbGrantee);
        this.tabPrivileges.Controls.Add(this.lblPrivilegeType);
        this.tabPrivileges.Controls.Add(this.cbPrivilegeType);
        this.tabPrivileges.Controls.Add(this.cbObjectType);
        this.tabPrivileges.Controls.Add(this.cbObjectName);
        this.tabPrivileges.Controls.Add(this.clbColumns);
        this.tabPrivileges.Controls.Add(this.chkWithGrantOption);
        this.tabPrivileges.Controls.Add(this.btnGrant);
        this.tabPrivileges.Controls.Add(this.btnRevoke);
        this.tabPrivileges.Location = new System.Drawing.Point(4, 24);
        this.tabPrivileges.Name = "tabPrivileges";
        this.tabPrivileges.Padding = new Padding(3);
        this.tabPrivileges.Size = new System.Drawing.Size(892, 572);
        this.tabPrivileges.Text = "Cấp quyền & Thu hồi";
        this.tabPrivileges.UseVisualStyleBackColor = true;

        // lblGrantee
        this.lblGrantee.AutoSize = true;
        this.lblGrantee.Location = new System.Drawing.Point(20, 30);
        this.lblGrantee.Text = "User / Role (Người nhận quyền):";

        this.cbGrantee.Location = new System.Drawing.Point(220, 27);
        this.cbGrantee.Size = new System.Drawing.Size(200, 23);

        this.lblPrivilegeType.AutoSize = true;
        this.lblPrivilegeType.Location = new System.Drawing.Point(20, 70);
        this.lblPrivilegeType.Text = "Loại quyền (Select, Update...):";

        this.cbPrivilegeType.Location = new System.Drawing.Point(220, 67);
        this.cbPrivilegeType.Size = new System.Drawing.Size(200, 23);
        this.cbPrivilegeType.Items.AddRange(new object[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE", "ALL PRIVILEGES" });

        this.cbObjectType.Location = new System.Drawing.Point(220, 107);
        this.cbObjectType.Size = new System.Drawing.Size(100, 23);
        this.cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE" });
        this.cbObjectType.Text = "TABLE";

        this.cbObjectName.Location = new System.Drawing.Point(330, 107);
        this.cbObjectName.Size = new System.Drawing.Size(200, 23);
        
        Label lblObject = new Label() { Text = "Đối tượng (Table/View...):", Location = new System.Drawing.Point(20, 110), AutoSize = true };
        this.tabPrivileges.Controls.Add(lblObject);

        Label lblCols = new Label() { Text = "Cột (chỉ dành cho Select/Update):", Location = new System.Drawing.Point(20, 150), AutoSize = true };
        this.tabPrivileges.Controls.Add(lblCols);

        this.clbColumns.Location = new System.Drawing.Point(220, 150);
        this.clbColumns.Size = new System.Drawing.Size(200, 100);

        this.chkWithGrantOption.Location = new System.Drawing.Point(220, 270);
        this.chkWithGrantOption.Text = "WITH GRANT OPTION";
        this.chkWithGrantOption.AutoSize = true;

        this.btnGrant.Location = new System.Drawing.Point(220, 310);
        this.btnGrant.Size = new System.Drawing.Size(100, 40);
        this.btnGrant.Text = "Grant (Cấp)";
        
        this.btnRevoke.Location = new System.Drawing.Point(330, 310);
        this.btnRevoke.Size = new System.Drawing.Size(100, 40);
        this.btnRevoke.Text = "Revoke (Thu hồi)";

        // 
        // tabViewPrivileges
        // 
        this.tabViewPrivileges.Controls.Add(this.txtSearchUserRole);
        this.tabViewPrivileges.Controls.Add(this.btnSearchPrivileges);
        this.tabViewPrivileges.Controls.Add(this.dgvPrivileges);
        this.tabViewPrivileges.Location = new System.Drawing.Point(4, 24);
        this.tabViewPrivileges.Name = "tabViewPrivileges";
        this.tabViewPrivileges.Padding = new Padding(3);
        this.tabViewPrivileges.Size = new System.Drawing.Size(892, 572);
        this.tabViewPrivileges.Text = "Tra cứu Quyền";
        this.tabViewPrivileges.UseVisualStyleBackColor = true;

        Label lblSearch = new Label() { Text = "Nhập tên User / Role:", Location = new System.Drawing.Point(20, 25), AutoSize = true };
        this.tabViewPrivileges.Controls.Add(lblSearch);

        this.txtSearchUserRole.Location = new System.Drawing.Point(150, 22);
        this.txtSearchUserRole.Size = new System.Drawing.Size(200, 23);

        this.btnSearchPrivileges.Location = new System.Drawing.Point(370, 22);
        this.btnSearchPrivileges.Text = "Tra cứu";

        this.dgvPrivileges.Location = new System.Drawing.Point(20, 60);
        this.dgvPrivileges.Size = new System.Drawing.Size(850, 490);

        // 
        // AdminDashboard
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(900, 600);
        this.Controls.Add(this.tabControlMain);
        this.Name = "AdminDashboard";
        this.Text = "Phân Hệ Quản Trị Hệ Thống Cơ Sở Dữ Liệu Oracle";
        this.StartPosition = FormStartPosition.CenterScreen;

        this.tabControlMain.ResumeLayout(false);
        this.tabUsersRoles.ResumeLayout(false);
        this.tabUsersRoles.PerformLayout();
        this.tabPrivileges.ResumeLayout(false);
        this.tabPrivileges.PerformLayout();
        this.tabViewPrivileges.ResumeLayout(false);
        this.tabViewPrivileges.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.dgvUsers).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.dgvRoles).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.dgvPrivileges).EndInit();
        this.ResumeLayout(false);
    }
}
