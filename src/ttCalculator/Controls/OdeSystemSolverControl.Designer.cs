using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class OdeSystemSolverControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Diferansiyel Denklem Sistemi Çözücü (System of ODEs / Faz Portresi & Zaman Serisi)",
                "dY/dt = F(t, Y) vektörel diferansiyel denklem sistemlerini RK4, RK8 ve Euler ile çözer; 2B Faz Uzayı (Phase Portrait) ve Zaman Serisi grafikleri sunar.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("Harmonik Osilatör [y₁'=y₂, y₂'=-y₁]");
            this.btnEx2 = ThemeHelper.CreateExampleChip("Av-Avcı [Lotka-Volterra]");
            this.btnEx3 = ThemeHelper.CreateExampleChip("Lorenz Kaotik Çekicisi [3D]");
            this.btnEx4 = ThemeHelper.CreateExampleChip("Sönümlü Sarkaç");

            this.lblEqCount = new Label();
            this.numEqCount = new NumericUpDown();
            this.lblMethod = new Label();
            this.cmbMethod = new ComboBox();

            this.lblT0 = new Label();
            this.txtT0 = new TextBox();
            this.lblTEnd = new Label();
            this.txtTEnd = new TextBox();
            this.lblSteps = new Label();
            this.numSteps = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 Sistemi Çöz", 150, 32, true);

            this.pnlEquations = new Panel();

            // Split Container: Visual Graphs (Left) + Stats & Vector Inspector (Right)
            this.splitSystem = new SplitContainer();

            // Left: Tabbed Visualizations
            this.tabSystemVisual = new TabControl();
            this.tabPhase = new TabPage();
            this.pnlPhaseHeader = new Panel();
            this.lblPhaseTitle = new Label();
            this.lblAxisX = new Label();
            this.cmbAxisX = new ComboBox();
            this.lblAxisY = new Label();
            this.cmbAxisY = new ComboBox();
            this.btnExportPhasePng = new Button();
            this.pnlPhaseCanvas = new Panel();

            this.tabTimeSeries = new TabPage();
            this.pnlTimeHeader = new Panel();
            this.lblTimeTitle = new Label();
            this.btnExportTimePng = new Button();
            this.pnlTimeCanvas = new Panel();

            this.tabTable = new TabPage();
            this.pnlTableToolbar = new Panel();
            this.btnCopySystemTsv = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala (TSV)");
            this.dgvSystemSteps = new DataGridView();

            // Right: Vector & Stats Panel
            this.pnlRightSidebar = new Panel();
            this.pnlVectorCard = new Panel();
            this.lblVectorTitle = new Label();
            this.lblVectorValue = new Label();
            this.pnlSystemMetrics = new Panel();
            this.lblSysMetricSteps = new Label();
            this.lblSysMetricH = new Label();
            this.lblSysMetricTime = new Label();
            this.lblSysMetricMethod = new Label();
            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSystem)).BeginInit();
            this.splitSystem.Panel1.SuspendLayout();
            this.splitSystem.Panel2.SuspendLayout();
            this.splitSystem.SuspendLayout();
            this.tabSystemVisual.SuspendLayout();
            this.tabPhase.SuspendLayout();
            this.pnlPhaseHeader.SuspendLayout();
            this.tabTimeSeries.SuspendLayout();
            this.pnlTimeHeader.SuspendLayout();
            this.tabTable.SuspendLayout();
            this.pnlTableToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystemSteps)).BeginInit();
            this.pnlRightSidebar.SuspendLayout();
            this.pnlVectorCard.SuspendLayout();
            this.pnlSystemMetrics.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 175;
            this.pnlControls.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlControls.Padding = new Padding(20, 6, 20, 6);

            this.lblPresets.Text = "⚡ Hazır Sistemler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(20, 6);
            this.pnlPresets.Size = new Size(880, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);

            int yRow = 42;
            this.lblEqCount.Text = "Denklem Sayısı (n):";
            this.lblEqCount.Location = new Point(20, yRow + 3);
            this.lblEqCount.AutoSize = true;
            this.lblEqCount.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblEqCount.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.numEqCount.Location = new Point(140, yRow);
            this.numEqCount.Size = new Size(42, 24);
            this.numEqCount.Minimum = 1;
            this.numEqCount.Maximum = 6;
            this.numEqCount.Value = 2;
            this.numEqCount.BackColor = Color.FromArgb(15, 23, 42);
            this.numEqCount.ForeColor = Color.White;

            this.lblMethod.Text = "Yöntem:";
            this.lblMethod.Location = new Point(195, yRow + 3);
            this.lblMethod.AutoSize = true;
            this.lblMethod.ForeColor = ThemeHelper.LightText;
            this.lblMethod.Font = new Font("Segoe UI", 9F);

            this.cmbMethod.Location = new Point(250, yRow);
            this.cmbMethod.Size = new Size(180, 24);
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.Font = new Font("Segoe UI", 9F);
            this.cmbMethod.Items.AddRange(new object[] {
                "Runge-Kutta 4 (RK4)",
                "Runge-Kutta 8 (RK8 - Ultra)",
                "Euler Yöntemi"
            });
            this.cmbMethod.SelectedIndex = 0;

            this.lblT0.Text = "t₀:";
            this.lblT0.Location = new Point(440, yRow + 3);
            this.lblT0.AutoSize = true;
            this.lblT0.ForeColor = ThemeHelper.LightText;

            this.txtT0.Location = new Point(465, yRow);
            this.txtT0.Size = new Size(45, 24);
            this.txtT0.Text = "0";
            this.txtT0.BackColor = Color.FromArgb(15, 23, 42);
            this.txtT0.ForeColor = Color.White;
            this.txtT0.BorderStyle = BorderStyle.FixedSingle;
            this.txtT0.TextAlign = HorizontalAlignment.Center;

            this.lblTEnd.Text = "t_son:";
            this.lblTEnd.Location = new Point(518, yRow + 3);
            this.lblTEnd.AutoSize = true;
            this.lblTEnd.ForeColor = ThemeHelper.LightText;

            this.txtTEnd.Location = new Point(560, yRow);
            this.txtTEnd.Size = new Size(50, 24);
            this.txtTEnd.Text = "15";
            this.txtTEnd.BackColor = Color.FromArgb(15, 23, 42);
            this.txtTEnd.ForeColor = Color.White;
            this.txtTEnd.BorderStyle = BorderStyle.FixedSingle;
            this.txtTEnd.TextAlign = HorizontalAlignment.Center;

            this.lblSteps.Text = "Adım (N):";
            this.lblSteps.Location = new Point(620, yRow + 3);
            this.lblSteps.AutoSize = true;
            this.lblSteps.ForeColor = ThemeHelper.LightText;

            this.numSteps.Location = new Point(685, yRow);
            this.numSteps.Size = new Size(60, 24);
            this.numSteps.Minimum = 10;
            this.numSteps.Maximum = 10000;
            this.numSteps.Value = 300;
            this.numSteps.BackColor = Color.FromArgb(15, 23, 42);
            this.numSteps.ForeColor = Color.White;

            this.btnSolve.Location = new Point(760, yRow - 3);

            // Dynamic Equations Panel
            this.pnlEquations.Location = new Point(20, 72);
            this.pnlEquations.Size = new Size(880, 95);
            this.pnlEquations.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlEquations.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlEquations.AutoScroll = true;
            this.pnlEquations.Padding = new Padding(6);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEqCount);
            this.pnlControls.Controls.Add(this.numEqCount);
            this.pnlControls.Controls.Add(this.lblMethod);
            this.pnlControls.Controls.Add(this.cmbMethod);
            this.pnlControls.Controls.Add(this.lblT0);
            this.pnlControls.Controls.Add(this.txtT0);
            this.pnlControls.Controls.Add(this.lblTEnd);
            this.pnlControls.Controls.Add(this.txtTEnd);
            this.pnlControls.Controls.Add(this.lblSteps);
            this.pnlControls.Controls.Add(this.numSteps);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.pnlEquations);

            // ================== SPLIT SYSTEM CONTAINER ==================
            this.splitSystem.Dock = DockStyle.Fill;
            this.splitSystem.Orientation = Orientation.Vertical;
            this.splitSystem.SplitterDistance = 600;
            this.splitSystem.SplitterWidth = 8;
            this.splitSystem.BackColor = Color.FromArgb(15, 23, 42);
            this.splitSystem.Padding = new Padding(12);

            // Left: Tabbed Visualizations
            this.splitSystem.Panel1.Controls.Add(this.tabSystemVisual);
            this.tabSystemVisual.Dock = DockStyle.Fill;
            this.tabSystemVisual.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // Tab 1: Faz Portresi
            this.tabPhase.Text = "Faz Portresi (Phase Portrait)";
            this.tabPhase.BackColor = Color.FromArgb(20, 28, 48);
            this.tabPhase.Padding = new Padding(4);

            this.pnlPhaseHeader.Dock = DockStyle.Top;
            this.pnlPhaseHeader.Height = 32;
            this.pnlPhaseHeader.BackColor = Color.FromArgb(24, 33, 52);

            this.lblPhaseTitle.Text = "🌀 Faz Uzayı:";
            this.lblPhaseTitle.Location = new Point(6, 6);
            this.lblPhaseTitle.AutoSize = true;
            this.lblPhaseTitle.ForeColor = Color.FromArgb(56, 189, 248);

            this.lblAxisX.Text = "X:";
            this.lblAxisX.Location = new Point(90, 6);
            this.lblAxisX.AutoSize = true;
            this.lblAxisX.ForeColor = ThemeHelper.LightText;

            this.cmbAxisX.Location = new Point(110, 4);
            this.cmbAxisX.Size = new Size(55, 23);
            this.cmbAxisX.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAxisX.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbAxisX.ForeColor = Color.White;

            this.lblAxisY.Text = "Y:";
            this.lblAxisY.Location = new Point(175, 6);
            this.lblAxisY.AutoSize = true;
            this.lblAxisY.ForeColor = ThemeHelper.LightText;

            this.cmbAxisY.Location = new Point(195, 4);
            this.cmbAxisY.Size = new Size(55, 23);
            this.cmbAxisY.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAxisY.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbAxisY.ForeColor = Color.White;

            this.btnExportPhasePng.Text = "📷 PNG Kaydet";
            this.btnExportPhasePng.Dock = DockStyle.Right;
            this.btnExportPhasePng.Width = 100;
            this.btnExportPhasePng.FlatStyle = FlatStyle.Flat;
            this.btnExportPhasePng.BackColor = Color.FromArgb(33, 44, 70);
            this.btnExportPhasePng.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnExportPhasePng.Font = new Font("Segoe UI", 8F);
            this.btnExportPhasePng.FlatAppearance.BorderSize = 0;
            this.btnExportPhasePng.Cursor = Cursors.Hand;

            this.pnlPhaseHeader.Controls.Add(this.lblPhaseTitle);
            this.pnlPhaseHeader.Controls.Add(this.lblAxisX);
            this.pnlPhaseHeader.Controls.Add(this.cmbAxisX);
            this.pnlPhaseHeader.Controls.Add(this.lblAxisY);
            this.pnlPhaseHeader.Controls.Add(this.cmbAxisY);
            this.pnlPhaseHeader.Controls.Add(this.btnExportPhasePng);

            this.pnlPhaseCanvas.Dock = DockStyle.Fill;
            this.pnlPhaseCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlPhaseCanvas.DoubleBuffered(true);

            this.tabPhase.Controls.Add(this.pnlPhaseCanvas);
            this.tabPhase.Controls.Add(this.pnlPhaseHeader);

            // Tab 2: Zaman Serisi
            this.tabTimeSeries.Text = "Zaman Serisi (Time Series)";
            this.tabTimeSeries.BackColor = Color.FromArgb(20, 28, 48);
            this.tabTimeSeries.Padding = new Padding(4);

            this.pnlTimeHeader.Dock = DockStyle.Top;
            this.pnlTimeHeader.Height = 32;
            this.pnlTimeHeader.BackColor = Color.FromArgb(24, 33, 52);

            this.lblTimeTitle.Text = "⏱️ Zaman Boyutu (t vs y₁, y₂, ...)";
            this.lblTimeTitle.Dock = DockStyle.Left;
            this.lblTimeTitle.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblTimeTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblTimeTitle.Padding = new Padding(8, 0, 0, 0);

            this.btnExportTimePng.Text = "📷 PNG Kaydet";
            this.btnExportTimePng.Dock = DockStyle.Right;
            this.btnExportTimePng.Width = 100;
            this.btnExportTimePng.FlatStyle = FlatStyle.Flat;
            this.btnExportTimePng.BackColor = Color.FromArgb(33, 44, 70);
            this.btnExportTimePng.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnExportTimePng.Font = new Font("Segoe UI", 8F);
            this.btnExportTimePng.FlatAppearance.BorderSize = 0;
            this.btnExportTimePng.Cursor = Cursors.Hand;

            this.pnlTimeHeader.Controls.Add(this.lblTimeTitle);
            this.pnlTimeHeader.Controls.Add(this.btnExportTimePng);

            this.pnlTimeCanvas.Dock = DockStyle.Fill;
            this.pnlTimeCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlTimeCanvas.DoubleBuffered(true);

            this.tabTimeSeries.Controls.Add(this.pnlTimeCanvas);
            this.tabTimeSeries.Controls.Add(this.pnlTimeHeader);

            // Tab 3: Table
            this.tabTable.Text = "Sayısal Veri Tablosu";
            this.tabTable.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTable.Padding = new Padding(6);

            this.pnlTableToolbar.Dock = DockStyle.Top;
            this.pnlTableToolbar.Height = 32;
            this.pnlTableToolbar.BackColor = Color.FromArgb(24, 33, 52);

            this.btnCopySystemTsv.Dock = DockStyle.Right;
            this.btnCopySystemTsv.Height = 28;
            this.pnlTableToolbar.Controls.Add(this.btnCopySystemTsv);

            SetupGrid(this.dgvSystemSteps);
            this.tabTable.Controls.Add(this.dgvSystemSteps);
            this.tabTable.Controls.Add(this.pnlTableToolbar);

            this.tabSystemVisual.TabPages.Add(this.tabPhase);
            this.tabSystemVisual.TabPages.Add(this.tabTimeSeries);
            this.tabSystemVisual.TabPages.Add(this.tabTable);

            // Right: Sidebar Panel
            this.splitSystem.Panel2.Controls.Add(this.pnlRightSidebar);
            this.pnlRightSidebar.Dock = DockStyle.Fill;
            this.pnlRightSidebar.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlRightSidebar.Padding = new Padding(6);

            // Vector Result Card
            this.pnlVectorCard.Dock = DockStyle.Top;
            this.pnlVectorCard.Height = 90;
            this.pnlVectorCard.BackColor = Color.FromArgb(18, 26, 44);
            this.pnlVectorCard.Padding = new Padding(10);

            this.lblVectorTitle.Text = "SONUÇ VEKTÖRÜ Y(t_son):";
            this.lblVectorTitle.Dock = DockStyle.Top;
            this.lblVectorTitle.Height = 18;
            this.lblVectorTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblVectorTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblVectorValue.Dock = DockStyle.Fill;
            this.lblVectorValue.Font = new Font("Consolas", 11.5F, FontStyle.Bold);
            this.lblVectorValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblVectorValue.Text = "Y = [ — ]";

            this.pnlVectorCard.Controls.Add(this.lblVectorValue);
            this.pnlVectorCard.Controls.Add(this.lblVectorTitle);

            // Metrics Container
            this.pnlSystemMetrics.Dock = DockStyle.Top;
            this.pnlSystemMetrics.Height = 110;
            this.pnlSystemMetrics.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlSystemMetrics.Padding = new Padding(10);

            this.lblSysMetricSteps.Text = "• Adım Sayısı (N) : —";
            this.lblSysMetricSteps.Location = new Point(10, 10);
            this.lblSysMetricSteps.AutoSize = true;
            this.lblSysMetricSteps.Font = new Font("Consolas", 9.5F);
            this.lblSysMetricSteps.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblSysMetricH.Text = "• Adım Boyutu (h) : —";
            this.lblSysMetricH.Location = new Point(10, 34);
            this.lblSysMetricH.AutoSize = true;
            this.lblSysMetricH.Font = new Font("Consolas", 9.5F);
            this.lblSysMetricH.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblSysMetricTime.Text = "• Süre            : —";
            this.lblSysMetricTime.Location = new Point(10, 58);
            this.lblSysMetricTime.AutoSize = true;
            this.lblSysMetricTime.Font = new Font("Consolas", 9.5F);
            this.lblSysMetricTime.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblSysMetricMethod.Text = "• Yöntem          : —";
            this.lblSysMetricMethod.Location = new Point(10, 82);
            this.lblSysMetricMethod.AutoSize = true;
            this.lblSysMetricMethod.Font = new Font("Consolas", 9.5F);
            this.lblSysMetricMethod.ForeColor = Color.FromArgb(56, 189, 248);

            this.pnlSystemMetrics.Controls.Add(this.lblSysMetricSteps);
            this.pnlSystemMetrics.Controls.Add(this.lblSysMetricH);
            this.pnlSystemMetrics.Controls.Add(this.lblSysMetricTime);
            this.pnlSystemMetrics.Controls.Add(this.lblSysMetricMethod);

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 15, 28);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            this.pnlRightSidebar.Controls.Add(this.txtOutput);
            this.pnlRightSidebar.Controls.Add(this.pnlSystemMetrics);
            this.pnlRightSidebar.Controls.Add(this.pnlVectorCard);

            // Assemble
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.splitSystem);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "OdeSystemSolverControl";
            this.Size = new Size(950, 700);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
            this.splitSystem.Panel1.ResumeLayout(false);
            this.splitSystem.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSystem)).EndInit();
            this.splitSystem.ResumeLayout(false);
            this.tabSystemVisual.ResumeLayout(false);
            this.tabPhase.ResumeLayout(false);
            this.pnlPhaseHeader.ResumeLayout(false);
            this.pnlPhaseHeader.PerformLayout();
            this.tabTimeSeries.ResumeLayout(false);
            this.pnlTimeHeader.ResumeLayout(false);
            this.tabTable.ResumeLayout(false);
            this.pnlTableToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystemSteps)).EndInit();
            this.pnlRightSidebar.ResumeLayout(false);
            this.pnlVectorCard.ResumeLayout(false);
            this.pnlSystemMetrics.ResumeLayout(false);
            this.pnlSystemMetrics.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.FromArgb(15, 23, 42);
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 33, 52);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 24;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(20, 28, 48);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Consolas", 9.5F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.GridColor = Color.FromArgb(40, 50, 75);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
        }

        private Panel pnlHeader;
        private Panel pnlControls;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;
        private Button btnEx4;

        private Label lblEqCount;
        private NumericUpDown numEqCount;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Label lblT0;
        private TextBox txtT0;
        private Label lblTEnd;
        private TextBox txtTEnd;
        private Label lblSteps;
        private NumericUpDown numSteps;
        private Button btnSolve;

        private Panel pnlEquations;

        private SplitContainer splitSystem;
        private TabControl tabSystemVisual;
        private TabPage tabPhase;
        private Panel pnlPhaseHeader;
        private Label lblPhaseTitle;
        private Label lblAxisX;
        private ComboBox cmbAxisX;
        private Label lblAxisY;
        private ComboBox cmbAxisY;
        private Button btnExportPhasePng;
        private Panel pnlPhaseCanvas;

        private TabPage tabTimeSeries;
        private Panel pnlTimeHeader;
        private Label lblTimeTitle;
        private Button btnExportTimePng;
        private Panel pnlTimeCanvas;

        private TabPage tabTable;
        private Panel pnlTableToolbar;
        private Button btnCopySystemTsv;
        private DataGridView dgvSystemSteps;

        private Panel pnlRightSidebar;
        private Panel pnlVectorCard;
        private Label lblVectorTitle;
        private Label lblVectorValue;
        private Panel pnlSystemMetrics;
        private Label lblSysMetricSteps;
        private Label lblSysMetricH;
        private Label lblSysMetricTime;
        private Label lblSysMetricMethod;
        private RichTextBox txtOutput;
    }
}
