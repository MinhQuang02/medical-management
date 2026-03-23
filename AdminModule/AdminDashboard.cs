using System.Data;
using MedicalDataManagement.Common;
using Oracle.ManagedDataAccess.Client;

namespace MedicalDataManagement.AdminModule;

public partial class AdminDashboard : Form
{
    private readonly DatabaseService _dbService;

    public AdminDashboard()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        
        ConfigureGrid(dgvUsers);
        ConfigureGrid(dgvRoles);
        ConfigureGrid(dgvPrivileges);
        dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPrivileges.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        this.Load += AdminDashboard_Load;
        SetNavActive(btnNavUsers);
        
        // --- Search Events ---
        txtQuickSearchUR.TextChanged += (s, e) => LoadUsersAndRoles();
        chkOnlySystem_Priv.CheckedChanged += (s, e) => { LoadObjects(); LoadGrantees(); };

        // --- CRUD Handlers ---
        btnCreateUser.Click += btnCreateUser_Click;
        btnEditUser.Click += btnEditUser_Click;
        btnDropUser.Click += btnDropUser_Click;
        btnCreateRole.Click += btnCreateRole_Click;
        btnDropRole.Click += btnDropRole_Click;
        btnGrant.Click += btnGrant_Click;
        btnRevoke.Click += btnRevoke_Click;
        btnSearchPrivileges.Click += btnSearchPrivileges_Click;
        
        cbObjectType.SelectedIndexChanged += cbObjectType_SelectedIndexChanged;
        cbObjectName.SelectedIndexChanged += cbObjectName_SelectedIndexChanged;

        // ComboBox setup
        cbObjectType.Items.Clear();
        cbObjectType.Items.AddRange(new object[] { "TABLE", "VIEW", "PROCEDURE", "FUNCTION" });
        cbObjectType.SelectedIndexChanged += (s, ev) => {
             cbPrivilegeType.Items.Clear();
             string type = cbObjectType.SelectedItem?.ToString() ?? "TABLE";
             if (type == "TABLE" || type == "VIEW") cbPrivilegeType.Items.AddRange(new object[] { "SELECT", "INSERT", "UPDATE", "DELETE", "ALL" });
             else cbPrivilegeType.Items.AddRange(new object[] { "EXECUTE" });
             if (cbPrivilegeType.Items.Count > 0) cbPrivilegeType.SelectedIndex = 0;
        };

        if (cbObjectType.Items.Count > 0) cbObjectType.SelectedIndex = 0;
    }

    private void ConfigureGrid(DataGridView dgv)
    {
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.MultiSelect = false;
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.RowHeadersVisible = false;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215);
        dgv.DefaultCellStyle.SelectionForeColor = Color.White;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 246, 249);
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Bold", 9);
        dgv.ColumnHeadersHeight = 40;
        dgv.GridColor = Color.FromArgb(238, 241, 245);
    }

    private void AdminDashboard_Load(object? sender, EventArgs e)
    {
        LoadUsersAndRoles();
        LoadObjects();
        LoadGrantees();
    }

    private void LoadUsersAndRoles()
    {
        try
        {
            string keyword = txtQuickSearchUR.Text.Trim();
            dgvUsers.DataSource = _dbService.GetAllUsers(keyword);
            dgvRoles.DataSource = _dbService.GetAllRoles(keyword);
        }
        catch (Exception ex) { ShowError("Lỗi danh sách: " + ex.Message); }
    }

    private void LoadGrantees()
    {
        try
        {
            DataTable users = _dbService.GetAllUsers("");
            DataTable roles = _dbService.GetAllRoles("");
            cbGrantee.Items.Clear();
            foreach (DataRow row in users.Rows) cbGrantee.Items.Add(row["USERNAME"].ToString()!);
            foreach (DataRow row in roles.Rows) cbGrantee.Items.Add(row["ROLE"].ToString()!);
            if (cbGrantee.Items.Count > 0) cbGrantee.SelectedIndex = 0;
        }
        catch (Exception ex) { ShowError("Lỗi Grantee: " + ex.Message); }
    }

    private void LoadObjects()
    {
        try
        {
            string type = cbObjectType.SelectedItem?.ToString() ?? "TABLE";
            bool hideSystem = chkOnlySystem_Priv.Checked;
            DataTable objects = _dbService.GetObjects(type, hideSystem);
            cbObjectName.DataSource = objects;
            cbObjectName.DisplayMember = "OBJECT_NAME";
        }
        catch (Exception ex) { ShowError("Lỗi đối tượng: " + ex.Message); }
    }

    private void cbObjectType_SelectedIndexChanged(object? sender, EventArgs e) => LoadObjects();

    private void cbObjectName_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            if (cbObjectType.SelectedItem?.ToString() == "TABLE")
            {
                string tableName = cbObjectName.Text;
                if (!string.IsNullOrEmpty(tableName))
                {
                    DataTable cols = _dbService.GetTableColumns(tableName);
                    clbColumns.Items.Clear();
                    foreach (DataRow row in cols.Rows) clbColumns.Items.Add(row["COLUMN_NAME"].ToString()!);
                }
            } else clbColumns.Items.Clear();
        }
        catch (Exception ex) { ShowError("Lỗi cột: " + ex.Message); }
    }

    private void btnCreateUser_Click(object? sender, EventArgs e)
    {
        string username = Microsoft.VisualBasic.Interaction.InputBox("Tên user:", "Thêm User", "");
        string password = Microsoft.VisualBasic.Interaction.InputBox("Mật khẩu:", "Thêm User", "123");
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            try { _dbService.CreateUser(username, password); LoadUsersAndRoles(); LoadGrantees(); }
            catch (Exception ex) { ShowError(ex.Message); }
        }
    }

    private void btnEditUser_Click(object? sender, EventArgs e)
    {
        if (dgvUsers.SelectedRows.Count > 0)
        {
            string username = dgvUsers.SelectedRows[0].Cells["USERNAME"].Value.ToString()!;
            string pass = Microsoft.VisualBasic.Interaction.InputBox($"Mật khẩu mới cho {username}:", "Đổi mật khẩu", "");
            if (!string.IsNullOrEmpty(pass))
            {
                try { _dbService.AlterUserPassword(username, pass); MessageBox.Show("Đã đổi password."); }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }
    }

    private void btnDropUser_Click(object? sender, EventArgs e)
    {
        if (dgvUsers.SelectedRows.Count > 0)
        {
            string username = dgvUsers.SelectedRows[0].Cells["USERNAME"].Value.ToString()!;
            if (MessageBox.Show($"Xóa user {username}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try { _dbService.DropUser(username); LoadUsersAndRoles(); LoadGrantees(); }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }
    }

    private void btnCreateRole_Click(object? sender, EventArgs e)
    {
        string role = Microsoft.VisualBasic.Interaction.InputBox("Tên role:", "Thêm Role", "");
        if (!string.IsNullOrEmpty(role))
        {
            try { _dbService.CreateRole(role); LoadUsersAndRoles(); LoadGrantees(); }
            catch (Exception ex) { ShowError(ex.Message); }
        }
    }

    private void btnDropRole_Click(object? sender, EventArgs e)
    {
        if (dgvRoles.SelectedRows.Count > 0)
        {
            string role = dgvRoles.SelectedRows[0].Cells["ROLE"].Value.ToString()!;
            if (MessageBox.Show($"Xóa role {role}?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try { _dbService.DropRole(role); LoadUsersAndRoles(); LoadGrantees(); }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }
    }

    private void btnGrant_Click(object? sender, EventArgs e)
    {
        try
        {
            string grantee = cbGrantee.Text;
            string priv = cbPrivilegeType.Text;
            string obj = cbObjectName.Text;
            bool withGrant = chkWithGrantOption.Checked;
            List<string> selectedCols = new List<string>();
            foreach (var item in clbColumns.CheckedItems) selectedCols.Add(item.ToString()!);
            _dbService.GrantPrivilege(grantee, priv, obj, withGrant, selectedCols);
            MessageBox.Show("Thành công!");
        }
        catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void btnRevoke_Click(object? sender, EventArgs e)
    {
        try {
            _dbService.RevokePrivilege(cbGrantee.Text, cbPrivilegeType.Text, cbObjectName.Text);
            MessageBox.Show("Đã thu hồi.");
        } catch (Exception ex) { ShowError(ex.Message); }
    }

    private void btnSearchPrivileges_Click(object? sender, EventArgs e)
    {
        try {
            string name = txtSearchUserRole.Text.Trim();
            if (!string.IsNullOrEmpty(name)) dgvPrivileges.DataSource = _dbService.GetUserOrRolePrivileges(name);
        } catch (Exception ex) { ShowError(ex.Message); }
    }

    private void ShowError(string msg) => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
