using System.Data;

namespace MedicalDataManagement.MedicalModule;

public partial class MedicalMainForm : Form
{
    private readonly DatabaseService _dbService;
    private readonly string _username;
    private List<string> _roles = new();
    private bool isCoord, isDoc, isTech, isPatient, isAdmin;

    public MedicalMainForm(DatabaseService dbService, string username)
    {
        InitializeComponent();
        _dbService = dbService;
        _username = username.ToUpper();

        // Common config
        ConfigureGrid(dgvNotifications);
        ConfigureGrid(dgvPatientInfo);
        ConfigureGrid(dgvMyServices);
        ConfigureGrid(dgvPatients);
        ConfigureGrid(dgvRecordsCoord);
        ConfigureGrid(dgvServicesCoord);
        ConfigureGrid(dgvRecordsDoctor);
        ConfigureGrid(dgvServicesDoctor);
        ConfigureGrid(dgvHistoryDoctor);
        ConfigureGrid(dgvPrescriptionsDoctor);

        this.Load += MedicalMainForm_Load;
        
        // Navigation clicks
        btnNavProfile.Click += (s, e) => { SwitchTab(tabProfile, btnNavProfile); LoadPatientProfile(); };
        btnNavTechServices.Click += (s, e) => { SwitchTab(tabTechServices, btnNavTechServices); LoadTechServices(); };
        btnNavCoordPatients.Click += (s, e) => { 
            if (isCoord && !isAdmin)
            {
                OpenChildForm(new CoordinatorForm(_dbService, _username));
            }
            
            else
            {
                SwitchTab(tabCoordPatients, btnNavCoordPatients);
                LoadCoordPatients();
            )
        };
        btnNavCoordRecords.Click += (s, e) => {
            if (isCoord && !isAdmin)
            {
                OpenChildForm(new CoordinatorForm(_dbService, _username));
            }
            
            else
            {
                SwitchTab(tabCoordRecords, btnNavCoordRecords);
                LoadCoordRecords();
            }
        };
        btnNavDocRecords.Click += (s, e) => { 
            if (isDoc && !isAdmin)
            {
                OpenChildForm(new DoctorForm(_dbService, _username));
            }
            
            else
            {
                SwitchTab(tabDocRecords, btnNavDocRecords);
                LoadDocRecords();
            }
        };
        btnNavNotifications.Click += (s, e) => { SwitchTab(tabNotifications, btnNavNotifications); LoadNotifications(); };

        // Attach CRUD events for each role
        AttachEvents();
    }

    private void MedicalMainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _roles = _dbService.GetCurrentUserRoles();
        }
        catch (Exception)
        {
            // If checking roles fails, we use username heuristic
        }

        isPatient = _roles.Any(r => r.Contains("PATIENT") || r.Contains("BENHNHAN")) || _username.StartsWith("BN");
        isTech = _roles.Any(r => r.Contains("TECHNICIAN") || r.Contains("KTV")) || _username.StartsWith("KTV");
        isCoord = _roles.Any(r => r.Contains("COORDINATOR") || r.Contains("DIEUPHOI")) || _username.StartsWith("DP");
        isDoc = _roles.Any(r => r.Contains("DOCTOR") || r.Contains("BACSI")) || _username.StartsWith("BS");

        if (!isPatient && !isTech && !isCoord && !isDoc)
        {
            // DEBUG MODE: Nếu tên đăng nhập bằng tài khoản chứa database, hiện tất cả để xem UI
            isAdmin = true;
        }

        btnNavProfile.Visible = isPatient;
        btnNavTechServices.Visible = isTech;
        btnNavCoordPatients.Visible = isCoord;
        btnNavCoordRecords.Visible = isCoord; // Also includes service assignment
        btnNavDocRecords.Visible = isDoc; // Contains all doctor functionalities
        btnNavNotifications.Visible = true; // All users

        if (isAdmin)
        {
            btnNavProfile.Visible = true;
            btnNavTechServices.Visible = true;
            btnNavCoordPatients.Visible = true;
            btnNavCoordRecords.Visible = true;
            btnNavDocRecords.Visible = true;
        }
        
        LayoutSidebar();

        // Show the first available tab
        if (isPatient || isAdmin) { SwitchTab(tabProfile, btnNavProfile); LoadPatientProfile(); }
        else if (isTech) { SwitchTab(tabTechServices, btnNavTechServices); LoadTechServices(); }
        else if (isCoord && !isAdmin) { 
            OpenChildForm(new CoordinatorForm(_dbService, _username));
        }
        else if (isDoc) {
            OpenChildForm(new DoctorForm(_dbService, _username));
        }
        else { SwitchTab(tabNotifications, btnNavNotifications);  LoadNotifications(); }
    }

    private void LayoutSidebar()
    {
        int y = 90;
        if (btnNavProfile.Visible) { btnNavProfile.Top = y; y += 50; }
        if (btnNavTechServices.Visible) { btnNavTechServices.Top = y; y += 50; }
        if (btnNavCoordPatients.Visible) { btnNavCoordPatients.Top = y; y += 50; }
        if (btnNavCoordRecords.Visible) { btnNavCoordRecords.Top = y; y += 50; }
        if (btnNavDocRecords.Visible) { btnNavDocRecords.Top = y; y += 50; }
        if (btnNavNotifications.Visible) { btnNavNotifications.Top = y; y += 50; }
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
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
    }

    private void SwitchTab(TabPage tab, Button activeBtn)
    {
        tabControlMain.SelectedTab = tab;
        foreach (Control c in panelSidebar.Controls)
        {
            if (c is Button b) b.BackColor = Color.Transparent;
        }
        activeBtn.BackColor = Color.FromArgb(45, 55, 75);
    }

    //Ham de mo them form moi theo role
    private void OpenChildForm(Form child)
    {
        panelSidebar.Visible = false;
        tabControlMain.Visible = false;

        child.TopLevel = false;
        child.FormBorderStyle = FormBorderStyle.None;
        child.Dock = DockStyle.Fill;

        this.Controls.Add(child);
        child.BringToFront();
        child.Show();
    }

    private void ShowError(string msg) => MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    private void ShowInfo(string msg) => MessageBox.Show(msg, "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);

    // ==========================================
    // NOTIFICATIONS
    // ==========================================
    private void LoadNotifications()
    {
        try { dgvNotifications.DataSource = _dbService.ExecuteQuery("SELECT * FROM NOTIFICATION"); }
        catch { /* Bỏ qua lỗi DB để hiển thị UI */ }
    }

    // ==========================================
    // PATIENT: Views and updates their own record
    // ==========================================
    private void LoadPatientProfile()
    {
        try {
            // Virtual Private Database (VPD) will automatically restrict the row to the logged-in patient.
            // Query only fetch patient's info matching their username
            var dt = _dbService.ExecuteQuery("SELECT * FROM BENHNHAN WHERE MABN = :u", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("u", _username) });
            if (dt.Rows.Count > 0) {
                var r = dt.Rows[0];
                txtPat_ID.Text = r["MABN"].ToString();
                txtPat_Name.Text = r["TENBN"].ToString();
                txtPat_Gender.Text = r["PHAI"].ToString();
                txtPat_DOB.Text = r["NGAYSINH"].ToString()?.Split(' ')[0];
                txtPat_CCCD.Text = r["CCCD"].ToString();
                txtPat_SoNha.Text = r["SONHA"].ToString();
                txtPat_Duong.Text = r["TENDUONG"].ToString();
                txtPat_Quan.Text = r["QUANHUYEN"].ToString();
                txtPat_Tinh.Text = r["TINHTP"].ToString();
            }
        } catch { /* Bỏ qua lỗi DB */ }
    }

    private void btnSaveProfile_Click(object? sender, EventArgs e)
    {
        try {
            string sql = "UPDATE BENHNHAN SET SONHA = :sn, TENDUONG = :td, QUANHUYEN = :qh, TINHTP = :tt WHERE MABN = :u";
            _dbService.ExecuteNonQuery(sql, new Oracle.ManagedDataAccess.Client.OracleParameter[] {
                new("sn", txtPat_SoNha.Text), new("td", txtPat_Duong.Text), new("qh", txtPat_Quan.Text), new("tt", txtPat_Tinh.Text), new("u", _username)
            });
            ShowInfo("Cập nhật thành công!");
        } catch (Exception ex) { ShowError("Cập nhật thất bại: " + ex.Message); }
    }

    // ==========================================
    // TECHNICIAN: Views assigned services & adds results
    // ==========================================
    private void LoadTechServices()
    {
        try { 
            // Gets only the MEDICAL_SERVICE records assigned to this Technician via MAKTV.
            dgvMyServices.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA_DV WHERE MAKTV = :u", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("u", _username) }); 
        }
        catch { }
    }

    private void btnTechSaveResult_Click(object? sender, EventArgs e)
    {
        if (dgvMyServices.SelectedRows.Count == 0) return;
        string mahsba = dgvMyServices.SelectedRows[0].Cells["MAHSBA"].Value.ToString()!;
        string loai = dgvMyServices.SelectedRows[0].Cells["LOAIDV"].Value.ToString()!;
        string result = txtTechResult.Text;
        try {
            // Updates the result string for the specific service on the exact record.
            // Can be replaced with executing Stored Procedure SP_UPDATE_KETQUA_HSBA_DV
            _dbService.ExecuteNonQuery("UPDATE HSBA_DV SET KETQUA = :kq WHERE MAHSBA = :m AND LOAIDV = :l AND MAKTV = :u",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("kq", result), new("m", mahsba), new("l", loai), new("u", _username) });
            ShowInfo("Lưu kết quả thành công");
            LoadTechServices();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    // ==========================================
    // COORDINATOR
    // ==========================================
    private void LoadCoordPatients()
    {
        try { dgvPatients.DataSource = _dbService.ExecuteQuery("SELECT * FROM BENHNHAN"); } catch {}
    }

    private void btnCoordAddPatient_Click(object? sender, EventArgs e)
    {
        try {
            _dbService.ExecuteNonQuery("INSERT INTO BENHNHAN(MABN, TENBN, PHAI, NGAYSINH, CCCD) VALUES(:m, :t, :p, TO_DATE(:d, 'YYYY-MM-DD'), :c)",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] {
                    new("m", txtCp_ID.Text), new("t", txtCp_Name.Text), new("p", cbCp_Gender.Text), new("d", txtCp_DOB.Text), new("c", txtCp_CCCD.Text)
                });
            ShowInfo("Thêm bệnh nhân thành công"); LoadCoordPatients();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void LoadCoordRecords()
    {
        try { 
            dgvRecordsCoord.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA"); 
            dgvServicesCoord.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA_DV"); 
        } catch {}
    }

    private void btnCoordAddRecord_Click(object? sender, EventArgs e)
    {
        try {
            _dbService.ExecuteNonQuery("INSERT INTO HSBA(MAHSBA, MABN, NGAY, MAKHOA, MABS) VALUES(:mh, :mb, SYSDATE, :mk, :ms)",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] {
                    new("mh", txtCr_RecordID.Text), new("mb", txtCr_PatientID.Text), new("mk", txtCr_DeptID.Text), new("ms", txtCr_DocID.Text)
                });
            ShowInfo("Tạo HSBA thành công"); LoadCoordRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void btnCoordAssignTech_Click(object? sender, EventArgs e)
    {
        if (dgvServicesCoord.SelectedRows.Count == 0) return;
        string mahsba = dgvServicesCoord.SelectedRows[0].Cells["MAHSBA"].Value.ToString()!;
        string loai = dgvServicesCoord.SelectedRows[0].Cells["LOAIDV"].Value.ToString()!;
        try {
            _dbService.ExecuteNonQuery("UPDATE HSBA_DV SET MAKTV = :k WHERE MAHSBA = :m AND LOAIDV = :l",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("k", txtCs_TechID.Text), new("m", mahsba), new("l", loai) });
            ShowInfo("Phân công KTV thành công"); LoadCoordRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    // ==========================================
    // DOCTOR: Access assigned Records, Prescriptions, Service results
    // ==========================================
    private void LoadDocRecords()
    {
        try {
            // Get records where MABS = Current Doctor
            dgvRecordsDoctor.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA WHERE MABS = :u", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("u", _username) });
            // View active patients' history (joined table to only show their patients)
            dgvHistoryDoctor.DataSource = _dbService.ExecuteQuery("SELECT B.MABN, B.TENBN, B.TIENSUBENH, B.TIENSUBENHGD, B.DIUNGTHUOC FROM BENHNHAN B JOIN HSBA H ON B.MABN = H.MABN WHERE H.MABS = :u", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("u", _username) });
            
            if (dgvRecordsDoctor.Rows.Count > 0)
            {
                string rID = dgvRecordsDoctor.Rows[0].Cells["MAHSBA"].Value.ToString()!;
                dgvServicesDoctor.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA_DV WHERE MAHSBA = :r", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("r", rID) });
                dgvPrescriptionsDoctor.DataSource = _dbService.ExecuteQuery("SELECT * FROM DONTHUOC WHERE MAHSBA = :r", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("r", rID) });
            }
        } catch {}
    }

    private void btnDocUpdateRecord_Click(object? sender, EventArgs e)
    {
        if (dgvRecordsDoctor.SelectedRows.Count == 0) return;
        string m = dgvRecordsDoctor.SelectedRows[0].Cells["MAHSBA"].Value.ToString()!;
        try {
            // DB-Processing: Doctor is only allowed to update DIAGNOSIS, TREATMENT, CONCLUSION.
            // Could call an Oracle Stored Procedure instead, e.g., SP_UPDATE_CHANDOAN
            _dbService.ExecuteNonQuery("UPDATE HSBA SET CHANDOAN = :cd, DIEUTRI = :dt, KETLUAN = :kl WHERE MAHSBA = :m AND MABS = :u",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("cd", txtDr_ChanDoan.Text), new("dt", txtDr_DieuTri.Text), new("kl", txtDr_KetLuan.Text), new("m", m), new("u", _username) });
            ShowInfo("Cập nhật HSBA thành công"); LoadDocRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void btnDocAddService_Click(object? sender, EventArgs e)
    {
        if (dgvRecordsDoctor.SelectedRows.Count == 0) return;
        string m = dgvRecordsDoctor.SelectedRows[0].Cells["MAHSBA"].Value.ToString()!;
        try {
            _dbService.ExecuteNonQuery("INSERT INTO HSBA_DV(MAHSBA, LOAIDV, NGAYDV) VALUES(:m, :l, SYSDATE)",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("m", m), new("l", txtDs_Loai.Text) });
            ShowInfo("Thêm dịch vụ thành công"); LoadDocRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void btnDocAddPrescript_Click(object? sender, EventArgs e)
    {
        if (dgvRecordsDoctor.SelectedRows.Count == 0) return;
        string m = dgvRecordsDoctor.SelectedRows[0].Cells["MAHSBA"].Value.ToString()!;
        try {
            _dbService.ExecuteNonQuery("INSERT INTO DONTHUOC(MAHSBA, NGAYDT, TENTHUOC, LIEUDUNG) VALUES(:m, SYSDATE, :t, :l)",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("m", m), new("t", txtDp_Ten.Text), new("l", txtDp_Lieu.Text) });
            ShowInfo("Thêm đơn thuốc thành công"); LoadDocRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void btnDocUpdateHistory_Click(object? sender, EventArgs e)
    {
        if (dgvHistoryDoctor.SelectedRows.Count == 0) return;
        string m = dgvHistoryDoctor.SelectedRows[0].Cells["MABN"].Value.ToString()!;
        try {
            _dbService.ExecuteNonQuery("UPDATE BENHNHAN SET TIENSUBENH = :t, TIENSUBENHGD = :tg, DIUNGTHUOC = :d WHERE MABN = :m",
                new Oracle.ManagedDataAccess.Client.OracleParameter[] { new("t", txtDh_TienSu.Text), new("tg", txtDh_GiaDinh.Text), new("d", txtDh_DiUng.Text), new("m", m) });
            ShowInfo("Cập nhật tiền sử thành công"); LoadDocRecords();
        } catch (Exception ex) { ShowError("Lỗi: " + ex.Message); }
    }

    private void AttachEvents()
    {
        btnSaveProfile.Click += btnSaveProfile_Click;
        btnTechSaveResult.Click += btnTechSaveResult_Click;
        btnCoordAddPatient.Click += btnCoordAddPatient_Click;
        btnCoordAddRecord.Click += btnCoordAddRecord_Click;
        btnCoordAssignTech.Click += btnCoordAssignTech_Click;
        btnDocUpdateRecord.Click += btnDocUpdateRecord_Click;
        btnDocAddService.Click += btnDocAddService_Click;
        btnDocAddPrescript.Click += btnDocAddPrescript_Click;
        btnDocUpdateHistory.Click += btnDocUpdateHistory_Click;

        dgvRecordsDoctor.SelectionChanged += (s, e) => {
            if (dgvRecordsDoctor.SelectedRows.Count > 0)
            {
                var r = dgvRecordsDoctor.SelectedRows[0];
                txtDr_ChanDoan.Text = r.Cells["CHANDOAN"].Value?.ToString();
                txtDr_DieuTri.Text = r.Cells["DIEUTRI"].Value?.ToString();
                txtDr_KetLuan.Text = r.Cells["KETLUAN"].Value?.ToString();
                try {
                    string rID = r.Cells["MAHSBA"].Value.ToString()!;
                    dgvServicesDoctor.DataSource = _dbService.ExecuteQuery("SELECT * FROM HSBA_DV WHERE MAHSBA = :r", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("r", rID) });
                    dgvPrescriptionsDoctor.DataSource = _dbService.ExecuteQuery("SELECT * FROM DONTHUOC WHERE MAHSBA = :r", new[] { new Oracle.ManagedDataAccess.Client.OracleParameter("r", rID) });
                } catch { }
            }
        };

        dgvHistoryDoctor.SelectionChanged += (s, e) => {
            if (dgvHistoryDoctor.SelectedRows.Count > 0)
            {
                var r = dgvHistoryDoctor.SelectedRows[0];
                txtDh_TienSu.Text = r.Cells["TIENSUBENH"].Value?.ToString();
                txtDh_GiaDinh.Text = r.Cells["TIENSUBENHGD"].Value?.ToString();
                txtDh_DiUng.Text = r.Cells["DIUNGTHUOC"].Value?.ToString();
            }
        };
    }
}
