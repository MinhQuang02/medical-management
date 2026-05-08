namespace MedicalDataManagement.MedicalModule;

partial class MedicalLoginForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private TextBox txtAdminUsername;
    private TextBox txtAdminPassword;
    private Button btnAdminLogin;
    private Label lblTitle;
    private Label lblUser;
    private Label lblPass;
    private Label lblMedicalTitle;
    private Label lblAdminTitle;
    private Label lblAdminUser;
    private Label lblAdminPass;
    private Panel panelLogin;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.txtUsername = new TextBox();
        this.txtPassword = new TextBox();
        this.btnLogin = new Button();
        this.txtAdminUsername = new TextBox();
        this.txtAdminPassword = new TextBox();
        this.btnAdminLogin = new Button();
        this.lblTitle = new Label();
        this.lblUser = new Label();
        this.lblPass = new Label();
        this.lblMedicalTitle = new Label();
        this.lblAdminTitle = new Label();
        this.lblAdminUser = new Label();
        this.lblAdminPass = new Label();
        this.panelLogin = new Panel();
        
        this.SuspendLayout();

        this.panelLogin.BackColor = Color.White;
        this.panelLogin.Controls.Add(lblTitle);
        this.panelLogin.Controls.Add(lblMedicalTitle);
        this.panelLogin.Controls.Add(lblUser);
        this.panelLogin.Controls.Add(txtUsername);
        this.panelLogin.Controls.Add(lblPass);
        this.panelLogin.Controls.Add(txtPassword);
        this.panelLogin.Controls.Add(btnLogin);
        this.panelLogin.Controls.Add(lblAdminTitle);
        this.panelLogin.Controls.Add(lblAdminUser);
        this.panelLogin.Controls.Add(txtAdminUsername);
        this.panelLogin.Controls.Add(lblAdminPass);
        this.panelLogin.Controls.Add(txtAdminPassword);
        this.panelLogin.Controls.Add(btnAdminLogin);
        this.panelLogin.Location = new Point(100, 50);
        this.panelLogin.Size = new Size(400, 520);

        this.lblTitle.Text = "HỆ THỐNG Y TẾ";
        this.lblTitle.Font = new Font("Segoe UI Bold", 16, FontStyle.Bold);
        this.lblTitle.ForeColor = Color.FromArgb(0, 120, 215);
        this.lblTitle.Location = new Point(0, 30);
        this.lblTitle.Size = new Size(400, 40);
        this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

        this.lblMedicalTitle.Text = "Medical account";
        this.lblMedicalTitle.Location = new Point(50, 82);
        this.lblMedicalTitle.Font = new Font("Segoe UI Bold", 10, FontStyle.Bold);
        this.lblMedicalTitle.ForeColor = Color.FromArgb(64, 64, 64);
        this.lblMedicalTitle.Size = new Size(300, 25);

        this.lblUser.Text = "Username:";
        this.lblUser.Location = new Point(50, 115);
        this.lblUser.Font = new Font("Segoe UI Semibold", 10);
        this.lblUser.Size = new Size(120, 25);

        this.txtUsername.Location = new Point(50, 140);
        this.txtUsername.Size = new Size(300, 25);
        this.txtUsername.Font = new Font("Segoe UI", 10);

        this.lblPass.Text = "Password:";
        this.lblPass.Location = new Point(50, 180);
        this.lblPass.Font = new Font("Segoe UI Semibold", 10);
        this.lblPass.Size = new Size(120, 25);

        this.txtPassword.Location = new Point(50, 205);
        this.txtPassword.Size = new Size(300, 25);
        this.txtPassword.Font = new Font("Segoe UI", 10);
        this.txtPassword.PasswordChar = '*';

        this.btnLogin.Text = "ĐĂNG NHẬP";
        this.btnLogin.Location = new Point(50, 255);
        this.btnLogin.Size = new Size(300, 40);
        this.btnLogin.BackColor = Color.FromArgb(0, 120, 215);
        this.btnLogin.ForeColor = Color.White;
        this.btnLogin.FlatStyle = FlatStyle.Flat;
        this.btnLogin.FlatAppearance.BorderSize = 0;
        this.btnLogin.Font = new Font("Segoe UI Bold", 10);
        this.btnLogin.Cursor = Cursors.Hand;

        this.lblAdminTitle.Text = "Admin account";
        this.lblAdminTitle.Location = new Point(50, 320);
        this.lblAdminTitle.Font = new Font("Segoe UI Bold", 10, FontStyle.Bold);
        this.lblAdminTitle.ForeColor = Color.FromArgb(64, 64, 64);
        this.lblAdminTitle.Size = new Size(300, 25);

        this.lblAdminUser.Text = "Admin username:";
        this.lblAdminUser.Location = new Point(50, 350);
        this.lblAdminUser.Font = new Font("Segoe UI Semibold", 10);
        this.lblAdminUser.Size = new Size(160, 25);

        this.txtAdminUsername.Location = new Point(50, 375);
        this.txtAdminUsername.Size = new Size(300, 25);
        this.txtAdminUsername.Font = new Font("Segoe UI", 10);
        this.txtAdminUsername.Text = "sys";

        this.lblAdminPass.Text = "Admin password:";
        this.lblAdminPass.Location = new Point(50, 415);
        this.lblAdminPass.Font = new Font("Segoe UI Semibold", 10);
        this.lblAdminPass.Size = new Size(160, 25);

        this.txtAdminPassword.Location = new Point(50, 440);
        this.txtAdminPassword.Size = new Size(300, 25);
        this.txtAdminPassword.Font = new Font("Segoe UI", 10);
        this.txtAdminPassword.PasswordChar = '*';

        this.btnAdminLogin.Text = "DANG NHAP ADMIN";
        this.btnAdminLogin.Location = new Point(50, 475);
        this.btnAdminLogin.Size = new Size(300, 35);
        this.btnAdminLogin.BackColor = Color.FromArgb(28, 35, 49);
        this.btnAdminLogin.ForeColor = Color.White;
        this.btnAdminLogin.FlatStyle = FlatStyle.Flat;
        this.btnAdminLogin.FlatAppearance.BorderSize = 0;
        this.btnAdminLogin.Font = new Font("Segoe UI Bold", 10);
        this.btnAdminLogin.Cursor = Cursors.Hand;

        this.ClientSize = new Size(600, 620);
        this.Controls.Add(panelLogin);
        this.BackColor = Color.FromArgb(244, 246, 249);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Medical Data Management - Login";

        this.ResumeLayout(false);
    }
}
