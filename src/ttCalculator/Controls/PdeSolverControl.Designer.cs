using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class PdeSolverControl
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
                "Kısmi Diferansiyel Denklem Çözücü (1D PDE Solver / Isı Haritası & Animasyon)",
                "1 Boyutlu Isı (FTCS / Crank-Nicolson), Dalga (Leapfrog) ve Taşınım (Upwind) denklemlerini sonlu farklar ile çözer; 2B Mekan-Zaman Isı Haritası ve canlı dalga animasyonu sunar.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExHeat = ThemeHelper.CreateExampleChip("Isı Yayılımı [sin(πx)]");
            this.btnExWave = ThemeHelper.CreateExampleChip("Titreşen Dalga [c=2, sin(πx)]");
            this.btnExAdvect = ThemeHelper.CreateExampleChip("Taşınım Dalgası [Advection]");
            this.btnExGauss = ThemeHelper.CreateExampleChip("Gauss Sıcaklık Paketi");

            this.lblPdeType = new Label();
            this.cmbPdeType = new ComboBox();

            this.lblInitU = new Label();
            this.txtInitU = new TextBox();

            this.lblInitVel = new Label();
            this.txtInitVel = new TextBox();

            this.lblParam = new Label();
            this.txtParam = new TextBox();

            this.lblL = new Label();
            this.txtL = new TextBox();

            this.lblT = new Label();
            this.txtT = new TextBox();

            this.lblNx = new Label();
            this.numNx = new NumericUpDown();

            this.lblNt = new Label();
            this.numNt = new NumericUpDown();

            this.lblBc = new Label();
            this.txtBcLeft = new TextBox();
            this.txtBcRight = new TextBox();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 PDE'yi Çöz & Simüle Et", 200, 32, true);

            // Live CFL Stability Indicator Bar
            this.pnlCflBar = new Panel();
            this.lblCflIcon = new Label();
            this.lblCflText = new Label();

            // TabControl for Visualizations
            this.tabPdeVisual = new TabControl();
            this.tabHeatmap = new TabPage();
            this.pnlHeatmapHeader = new Panel();
            this.lblHeatmapTitle = new Label();
            this.lblColorbarLegend = new Label();
            this.btnExportHeatmapPng = new Button();
            this.pnlHeatmapCanvas = new Panel();

            this.tabAnimation = new TabPage();
            this.pnlAnimToolbar = new Panel();
            this.btnPlayPause = new Button();
            this.lblCurrentTime = new Label();
            this.trkTimeStep = new TrackBar();
            this.lblAnimSpeed = new Label();
            this.cmbAnimSpeed = new ComboBox();
            this.pnlAnimCanvas = new Panel();

            this.tabSnapshots = new TabPage();
            this.pnlSnapToolbar = new Panel();
            this.btnCopyPdeTsv = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala (TSV)");
            this.dgvSnapshots = new DataGridView();

            this.tabTextReport = new TabPage();
            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).BeginInit();
            this.pnlCflBar.SuspendLayout();
            this.tabPdeVisual.SuspendLayout();
            this.tabHeatmap.SuspendLayout();
            this.pnlHeatmapHeader.SuspendLayout();
            this.tabAnimation.SuspendLayout();
            this.pnlAnimToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeStep)).BeginInit();
            this.tabSnapshots.SuspendLayout();
            this.pnlSnapToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSnapshots)).BeginInit();
            this.tabTextReport.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 158;
            this.pnlControls.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlControls.Padding = new Padding(20, 6, 20, 6);

            this.lblPresets.Text = "⚡ Senaryolar:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(20, 6);
            this.pnlPresets.Size = new Size(900, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExHeat);
            this.pnlPresets.Controls.Add(this.btnExWave);
            this.pnlPresets.Controls.Add(this.btnExAdvect);
            this.pnlPresets.Controls.Add(this.btnExGauss);

            int yRow1 = 42;
            this.lblPdeType.Text = "PDE Tipi:";
            this.lblPdeType.Location = new Point(20, yRow1 + 3);
            this.lblPdeType.AutoSize = true;
            this.lblPdeType.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblPdeType.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.cmbPdeType.Location = new Point(85, yRow1);
            this.cmbPdeType.Size = new Size(230, 24);
            this.cmbPdeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPdeType.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbPdeType.ForeColor = Color.White;
            this.cmbPdeType.Font = new Font("Segoe UI", 9F);
            this.cmbPdeType.Items.AddRange(new object[] {
                "Isı Denklemi (FTCS Açık Şema)",
                "Isı Denklemi (Crank-Nicolson)",
                "Dalga Denklemi (Leapfrog)",
                "Taşınım Denklemi (Upwind)"
            });
            this.cmbPdeType.SelectedIndex = 0;

            this.lblInitU.Text = "u(x, 0):";
            this.lblInitU.Location = new Point(325, yRow1 + 3);
            this.lblInitU.AutoSize = true;
            this.lblInitU.ForeColor = ThemeHelper.LightText;

            this.txtInitU.Location = new Point(375, yRow1);
            this.txtInitU.Size = new Size(130, 24);
            this.txtInitU.Text = "sin(pi*x)";
            this.txtInitU.Font = new Font("Consolas", 9.5F);
            this.txtInitU.BackColor = Color.FromArgb(15, 23, 42);
            this.txtInitU.ForeColor = Color.White;
            this.txtInitU.BorderStyle = BorderStyle.FixedSingle;

            this.lblInitVel.Text = "u_t(x, 0):";
            this.lblInitVel.Location = new Point(515, yRow1 + 3);
            this.lblInitVel.AutoSize = true;
            this.lblInitVel.ForeColor = ThemeHelper.LightText;

            this.txtInitVel.Location = new Point(570, yRow1);
            this.txtInitVel.Size = new Size(70, 24);
            this.txtInitVel.Text = "0";
            this.txtInitVel.Font = new Font("Consolas", 9.5F);
            this.txtInitVel.BackColor = Color.FromArgb(15, 23, 42);
            this.txtInitVel.ForeColor = Color.White;
            this.txtInitVel.BorderStyle = BorderStyle.FixedSingle;

            this.lblParam.Text = "α / c / v:";
            this.lblParam.Location = new Point(650, yRow1 + 3);
            this.lblParam.AutoSize = true;
            this.lblParam.ForeColor = ThemeHelper.LightText;

            this.txtParam.Location = new Point(705, yRow1);
            this.txtParam.Size = new Size(45, 24);
            this.txtParam.Text = "1";
            this.txtParam.Font = new Font("Consolas", 9.5F);
            this.txtParam.BackColor = Color.FromArgb(15, 23, 42);
            this.txtParam.ForeColor = Color.White;
            this.txtParam.BorderStyle = BorderStyle.FixedSingle;
            this.txtParam.TextAlign = HorizontalAlignment.Center;

            int yRow2 = 78;
            this.lblL.Text = "Uzunluk L:";
            this.lblL.Location = new Point(20, yRow2 + 3);
            this.lblL.AutoSize = true;
            this.lblL.ForeColor = ThemeHelper.LightText;

            this.txtL.Location = new Point(85, yRow2);
            this.txtL.Size = new Size(45, 24);
            this.txtL.Text = "1";
            this.txtL.BackColor = Color.FromArgb(15, 23, 42);
            this.txtL.ForeColor = Color.White;
            this.txtL.BorderStyle = BorderStyle.FixedSingle;
            this.txtL.TextAlign = HorizontalAlignment.Center;

            this.lblT.Text = "Süre T:";
            this.lblT.Location = new Point(140, yRow2 + 3);
            this.lblT.AutoSize = true;
            this.lblT.ForeColor = ThemeHelper.LightText;

            this.txtT.Location = new Point(185, yRow2);
            this.txtT.Size = new Size(45, 24);
            this.txtT.Text = "0.2";
            this.txtT.BackColor = Color.FromArgb(15, 23, 42);
            this.txtT.ForeColor = Color.White;
            this.txtT.BorderStyle = BorderStyle.FixedSingle;
            this.txtT.TextAlign = HorizontalAlignment.Center;

            this.lblNx.Text = "Nx:";
            this.lblNx.Location = new Point(240, yRow2 + 3);
            this.lblNx.AutoSize = true;
            this.lblNx.ForeColor = ThemeHelper.LightText;

            this.numNx.Location = new Point(268, yRow2);
            this.numNx.Size = new Size(50, 24);
            this.numNx.Minimum = 10;
            this.numNx.Maximum = 500;
            this.numNx.Value = 50;
            this.numNx.BackColor = Color.FromArgb(15, 23, 42);
            this.numNx.ForeColor = Color.White;

            this.lblNt.Text = "Nt:";
            this.lblNt.Location = new Point(328, yRow2 + 3);
            this.lblNt.AutoSize = true;
            this.lblNt.ForeColor = ThemeHelper.LightText;

            this.numNt.Location = new Point(355, yRow2);
            this.numNt.Size = new Size(55, 24);
            this.numNt.Minimum = 10;
            this.numNt.Maximum = 2000;
            this.numNt.Value = 200;
            this.numNt.BackColor = Color.FromArgb(15, 23, 42);
            this.numNt.ForeColor = Color.White;

            this.lblBc.Text = "Sınırlar [Sol, Sağ]:";
            this.lblBc.Location = new Point(420, yRow2 + 3);
            this.lblBc.AutoSize = true;
            this.lblBc.ForeColor = ThemeHelper.LightText;

            this.txtBcLeft.Location = new Point(520, yRow2);
            this.txtBcLeft.Size = new Size(40, 24);
            this.txtBcLeft.Text = "0";
            this.txtBcLeft.BackColor = Color.FromArgb(15, 23, 42);
            this.txtBcLeft.ForeColor = Color.White;
            this.txtBcLeft.BorderStyle = BorderStyle.FixedSingle;
            this.txtBcLeft.TextAlign = HorizontalAlignment.Center;

            this.txtBcRight.Location = new Point(568, yRow2);
            this.txtBcRight.Size = new Size(40, 24);
            this.txtBcRight.Text = "0";
            this.txtBcRight.BackColor = Color.FromArgb(15, 23, 42);
            this.txtBcRight.ForeColor = Color.White;
            this.txtBcRight.BorderStyle = BorderStyle.FixedSingle;
            this.txtBcRight.TextAlign = HorizontalAlignment.Center;

            this.btnSolve.Location = new Point(640, yRow2 - 2);

            // CFL Indicator Bar
            this.pnlCflBar.Location = new Point(20, 114);
            this.pnlCflBar.Size = new Size(900, 34);
            this.pnlCflBar.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlCflBar.Padding = new Padding(10, 6, 10, 6);

            this.lblCflIcon.Text = "⚖️";
            this.lblCflIcon.Dock = DockStyle.Left;
            this.lblCflIcon.Width = 26;
            this.lblCflIcon.Font = new Font("Segoe UI", 10F);

            this.lblCflText.Text = "CFL Kararlılık Durumu Hesaplanıyor...";
            this.lblCflText.Dock = DockStyle.Fill;
            this.lblCflText.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            this.lblCflText.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblCflText.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlCflBar.Controls.Add(this.lblCflText);
            this.pnlCflBar.Controls.Add(this.lblCflIcon);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblPdeType);
            this.pnlControls.Controls.Add(this.cmbPdeType);
            this.pnlControls.Controls.Add(this.lblInitU);
            this.pnlControls.Controls.Add(this.txtInitU);
            this.pnlControls.Controls.Add(this.lblInitVel);
            this.pnlControls.Controls.Add(this.txtInitVel);
            this.pnlControls.Controls.Add(this.lblParam);
            this.pnlControls.Controls.Add(this.txtParam);
            this.pnlControls.Controls.Add(this.lblL);
            this.pnlControls.Controls.Add(this.txtL);
            this.pnlControls.Controls.Add(this.lblT);
            this.pnlControls.Controls.Add(this.txtT);
            this.pnlControls.Controls.Add(this.lblNx);
            this.pnlControls.Controls.Add(this.numNx);
            this.pnlControls.Controls.Add(this.lblNt);
            this.pnlControls.Controls.Add(this.numNt);
            this.pnlControls.Controls.Add(this.lblBc);
            this.pnlControls.Controls.Add(this.txtBcLeft);
            this.pnlControls.Controls.Add(this.txtBcRight);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.pnlCflBar);

            // ================== TABBED VISUALS ==================
            this.tabPdeVisual.Dock = DockStyle.Fill;
            this.tabPdeVisual.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.tabPdeVisual.Padding = new Point(12, 4);

            // Tab 1: Heatmap
            this.tabHeatmap.Text = "Mekan-Zaman Isı Haritası (Heatmap)";
            this.tabHeatmap.BackColor = Color.FromArgb(15, 23, 42);
            this.tabHeatmap.Padding = new Padding(8);

            this.pnlHeatmapHeader.Dock = DockStyle.Top;
            this.pnlHeatmapHeader.Height = 32;
            this.pnlHeatmapHeader.BackColor = Color.FromArgb(24, 33, 52);

            this.lblHeatmapTitle.Text = "🗺️ 2B Mekan-Zaman Dağılımı u(x, t):";
            this.lblHeatmapTitle.Dock = DockStyle.Left;
            this.lblHeatmapTitle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblHeatmapTitle.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblHeatmapTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblHeatmapTitle.Padding = new Padding(8, 0, 0, 0);

            this.lblColorbarLegend.Text = "Mavi (Min) ── Yeşil ── Sarı ── Kırmızı (Maks)";
            this.lblColorbarLegend.Dock = DockStyle.Left;
            this.lblColorbarLegend.Font = new Font("Segoe UI", 8F);
            this.lblColorbarLegend.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblColorbarLegend.TextAlign = ContentAlignment.MiddleLeft;
            this.lblColorbarLegend.Padding = new Padding(20, 0, 0, 0);

            this.btnExportHeatmapPng.Text = "📷 PNG Kaydet";
            this.btnExportHeatmapPng.Dock = DockStyle.Right;
            this.btnExportHeatmapPng.Width = 100;
            this.btnExportHeatmapPng.FlatStyle = FlatStyle.Flat;
            this.btnExportHeatmapPng.BackColor = Color.FromArgb(33, 44, 70);
            this.btnExportHeatmapPng.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnExportHeatmapPng.Font = new Font("Segoe UI", 8F);
            this.btnExportHeatmapPng.FlatAppearance.BorderSize = 0;
            this.btnExportHeatmapPng.Cursor = Cursors.Hand;

            this.pnlHeatmapHeader.Controls.Add(this.lblColorbarLegend);
            this.pnlHeatmapHeader.Controls.Add(this.lblHeatmapTitle);
            this.pnlHeatmapHeader.Controls.Add(this.btnExportHeatmapPng);

            this.pnlHeatmapCanvas.Dock = DockStyle.Fill;
            this.pnlHeatmapCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlHeatmapCanvas.DoubleBuffered(true);

            this.tabHeatmap.Controls.Add(this.pnlHeatmapCanvas);
            this.tabHeatmap.Controls.Add(this.pnlHeatmapHeader);

            // Tab 2: Animation & Time Slider
            this.tabAnimation.Text = "Canlı Dalga & Isı Animasyonu";
            this.tabAnimation.BackColor = Color.FromArgb(15, 23, 42);
            this.tabAnimation.Padding = new Padding(8);

            this.pnlAnimToolbar.Dock = DockStyle.Top;
            this.pnlAnimToolbar.Height = 44;
            this.pnlAnimToolbar.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlAnimToolbar.Padding = new Padding(8, 6, 8, 6);

            this.btnPlayPause.Text = "▶ Oynat";
            this.btnPlayPause.Dock = DockStyle.Left;
            this.btnPlayPause.Width = 80;
            this.btnPlayPause.FlatStyle = FlatStyle.Flat;
            this.btnPlayPause.BackColor = Color.FromArgb(16, 185, 129);
            this.btnPlayPause.ForeColor = Color.White;
            this.btnPlayPause.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnPlayPause.FlatAppearance.BorderSize = 0;
            this.btnPlayPause.Cursor = Cursors.Hand;

            this.lblCurrentTime.Text = "t = 0.000 s (Adım: 0 / 0)";
            this.lblCurrentTime.Dock = DockStyle.Left;
            this.lblCurrentTime.Width = 190;
            this.lblCurrentTime.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            this.lblCurrentTime.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblCurrentTime.TextAlign = ContentAlignment.MiddleCenter;

            this.lblAnimSpeed.Text = "Hız:";
            this.lblAnimSpeed.Dock = DockStyle.Right;
            this.lblAnimSpeed.Width = 32;
            this.lblAnimSpeed.Font = new Font("Segoe UI", 8.5F);
            this.lblAnimSpeed.ForeColor = ThemeHelper.LightText;
            this.lblAnimSpeed.TextAlign = ContentAlignment.MiddleRight;

            this.cmbAnimSpeed.Dock = DockStyle.Right;
            this.cmbAnimSpeed.Width = 65;
            this.cmbAnimSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAnimSpeed.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbAnimSpeed.ForeColor = Color.White;
            this.cmbAnimSpeed.Items.AddRange(new object[] { "1x", "2x", "5x" });
            this.cmbAnimSpeed.SelectedIndex = 0;

            this.trkTimeStep.Dock = DockStyle.Fill;
            this.trkTimeStep.Minimum = 0;
            this.trkTimeStep.Maximum = 100;
            this.trkTimeStep.Value = 0;
            this.trkTimeStep.TickStyle = TickStyle.None;

            this.pnlAnimToolbar.Controls.Add(this.trkTimeStep);
            this.pnlAnimToolbar.Controls.Add(this.lblAnimSpeed);
            this.pnlAnimToolbar.Controls.Add(this.cmbAnimSpeed);
            this.pnlAnimToolbar.Controls.Add(this.lblCurrentTime);
            this.pnlAnimToolbar.Controls.Add(this.btnPlayPause);

            this.pnlAnimCanvas.Dock = DockStyle.Fill;
            this.pnlAnimCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlAnimCanvas.DoubleBuffered(true);

            this.tabAnimation.Controls.Add(this.pnlAnimCanvas);
            this.tabAnimation.Controls.Add(this.pnlAnimToolbar);

            // Tab 3: Snapshots
            this.tabSnapshots.Text = "Zaman Kesitleri Tablosu";
            this.tabSnapshots.BackColor = Color.FromArgb(24, 33, 52);
            this.tabSnapshots.Padding = new Padding(6);

            this.pnlSnapToolbar.Dock = DockStyle.Top;
            this.pnlSnapToolbar.Height = 32;
            this.pnlSnapToolbar.BackColor = Color.FromArgb(24, 33, 52);

            this.btnCopyPdeTsv.Dock = DockStyle.Right;
            this.btnCopyPdeTsv.Height = 28;
            this.pnlSnapToolbar.Controls.Add(this.btnCopyPdeTsv);

            SetupGrid(this.dgvSnapshots);
            this.tabSnapshots.Controls.Add(this.dgvSnapshots);
            this.tabSnapshots.Controls.Add(this.pnlSnapToolbar);

            // Tab 4: Text Report
            this.tabTextReport.Text = "Rapor & Konsol";
            this.tabTextReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTextReport.Padding = new Padding(6);

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 15, 28);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            this.tabTextReport.Controls.Add(this.txtOutput);

            this.tabPdeVisual.TabPages.Add(this.tabHeatmap);
            this.tabPdeVisual.TabPages.Add(this.tabAnimation);
            this.tabPdeVisual.TabPages.Add(this.tabSnapshots);
            this.tabPdeVisual.TabPages.Add(this.tabTextReport);

            // Assemble
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.tabPdeVisual);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "PdeSolverControl";
            this.Size = new Size(950, 700);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).EndInit();
            this.pnlCflBar.ResumeLayout(false);
            this.tabPdeVisual.ResumeLayout(false);
            this.tabHeatmap.ResumeLayout(false);
            this.pnlHeatmapHeader.ResumeLayout(false);
            this.tabAnimation.ResumeLayout(false);
            this.pnlAnimToolbar.ResumeLayout(false);
            this.pnlAnimToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeStep)).EndInit();
            this.tabSnapshots.ResumeLayout(false);
            this.pnlSnapToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSnapshots)).EndInit();
            this.tabTextReport.ResumeLayout(false);
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
        private Button btnExHeat;
        private Button btnExWave;
        private Button btnExAdvect;
        private Button btnExGauss;

        private Label lblPdeType;
        private ComboBox cmbPdeType;
        private Label lblInitU;
        private TextBox txtInitU;
        private Label lblInitVel;
        private TextBox txtInitVel;
        private Label lblParam;
        private TextBox txtParam;
        private Label lblL;
        private TextBox txtL;
        private Label lblT;
        private TextBox txtT;
        private Label lblNx;
        private NumericUpDown numNx;
        private Label lblNt;
        private NumericUpDown numNt;
        private Label lblBc;
        private TextBox txtBcLeft;
        private TextBox txtBcRight;
        private Button btnSolve;

        private Panel pnlCflBar;
        private Label lblCflIcon;
        private Label lblCflText;

        private TabControl tabPdeVisual;
        private TabPage tabHeatmap;
        private Panel pnlHeatmapHeader;
        private Label lblHeatmapTitle;
        private Label lblColorbarLegend;
        private Button btnExportHeatmapPng;
        private Panel pnlHeatmapCanvas;

        private TabPage tabAnimation;
        private Panel pnlAnimToolbar;
        private Button btnPlayPause;
        private Label lblCurrentTime;
        private TrackBar trkTimeStep;
        private Label lblAnimSpeed;
        private ComboBox cmbAnimSpeed;
        private Panel pnlAnimCanvas;

        private TabPage tabSnapshots;
        private Panel pnlSnapToolbar;
        private Button btnCopyPdeTsv;
        private DataGridView dgvSnapshots;

        private TabPage tabTextReport;
        private RichTextBox txtOutput;
    }
}
