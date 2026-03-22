namespace MedicalDataManagement.MedicalModule;

partial class MedicalMainForm
{
    private System.ComponentModel.IContainer components = null;

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
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Phân Hệ Ứng Dụng Chuyên Môn Y Tế";
        this.StartPosition = FormStartPosition.CenterScreen;
        
        Label lblWelcome = new Label() { Text = "Chào mừng đến với hệ thống quản lý dữ liệu y tế", AutoSize = true, Location = new System.Drawing.Point(50, 50), Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold) };
        this.Controls.Add(lblWelcome);
    }
}
