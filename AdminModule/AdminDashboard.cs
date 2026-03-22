namespace MedicalDataManagement.AdminModule;

public partial class AdminDashboard : Form
{
    public AdminDashboard()
    {
        InitializeComponent();
        this.Load += AdminDashboard_Load;
    }

    private void AdminDashboard_Load(object? sender, EventArgs e)
    {
        // Kiểm tra kết nối Oracle khi màn hình vừa bật lên
        string connectionStatus = DatabaseConnection.TestConnection();
        MessageBox.Show(connectionStatus, "Kết quả kiểm tra Database Oracle", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
