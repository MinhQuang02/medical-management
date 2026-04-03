namespace MedicalDataManagement.MedicalModule;

using MedicalDataManagement.Common;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MedicalLoginForm());
    }
}
