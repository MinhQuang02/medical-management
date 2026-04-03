namespace MedicalDataManagement.MedicalModule;

public partial class MedicalLoginForm : Form
{
    public MedicalLoginForm()
    {
        InitializeComponent();
        btnLogin.Click += BtnLogin_Click;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            MessageBox.Show("Vui lòng nhập Username để xác định Role giao diện.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            // Bỏ qua kiểm tra kết nối DB ở Login theo yêu cầu
            var dbService = new DatabaseService(username, password);
            this.Hide();
            var mainForm = new MedicalMainForm(dbService, username);
            mainForm.Closed += (s, args) => this.Close();
            mainForm.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi hiển thị UI: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
