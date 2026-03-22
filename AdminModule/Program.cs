namespace MedicalDataManagement.AdminModule;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new AdminDashboard());
    }
}
