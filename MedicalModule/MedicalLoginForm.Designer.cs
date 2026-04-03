namespace MedicalDataManagement.MedicalModule;

partial class MedicalLoginForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblTitle;
    private Label lblUser;
    private Label lblPass;
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
        this.lblTitle = new Label();
        this.lblUser = new Label();
        this.lblPass = new Label();
        this.panelLogin = new Panel();
        
        this.SuspendLayout();

        this.panelLogin.BackColor = Color.White;
        this.panelLogin.Controls.Add(lblTitle);
        this.panelLogin.Controls.Add(lblUser);
        this.panelLogin.Controls.Add(txtUsername);
        this.panelLogin.Controls.Add(lblPass);
        this.panelLogin.Controls.Add(txtPassword);
        this.panelLogin.Controls.Add(btnLogin);
        this.panelLogin.Location = new Point(100, 50);
        this.panelLogin.Size = new Size(400, 300);

        this.lblTitle.Text = "HỆ THỐNG Y TẾ";
        this.lblTitle.Font = new Font("Segoe UI Bold", 16);
        this.lblTitle.ForeColor = Color.FromArgb(0, 120, 215);
        this.lblTitle.Location = new Point(0, 30);
        this.lblTitle.Size = new Size(400, 30);
        this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

        this.lblUser.Text = "Username:";
        this.lblUser.Location = new Point(50, 90);
        this.lblUser.Font = new Font("Segoe UI Semibold", 10);

        this.txtUsername.Location = new Point(50, 115);
        this.txtUsername.Size = new Size(300, 25);
        this.txtUsername.Font = new Font("Segoe UI", 10);

        this.lblPass.Text = "Password:";
        this.lblPass.Location = new Point(50, 155);
        this.lblPass.Font = new Font("Segoe UI Semibold", 10);

        this.txtPassword.Location = new Point(50, 180);
        this.txtPassword.Size = new Size(300, 25);
        this.txtPassword.Font = new Font("Segoe UI", 10);
        this.txtPassword.PasswordChar = '*';

        this.btnLogin.Text = "ĐĂNG NHẬP";
        this.btnLogin.Location = new Point(50, 230);
        this.btnLogin.Size = new Size(300, 40);
        this.btnLogin.BackColor = Color.FromArgb(0, 120, 215);
        this.btnLogin.ForeColor = Color.White;
        this.btnLogin.FlatStyle = FlatStyle.Flat;
        this.btnLogin.FlatAppearance.BorderSize = 0;
        this.btnLogin.Font = new Font("Segoe UI Bold", 10);
        this.btnLogin.Cursor = Cursors.Hand;

        this.ClientSize = new Size(600, 400);
        this.Controls.Add(panelLogin);
        this.BackColor = Color.FromArgb(244, 246, 249);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Medical Data Management - Login";

        this.ResumeLayout(false);
    }
}
