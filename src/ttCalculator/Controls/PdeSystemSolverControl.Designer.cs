using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class PdeSystemSolverControl
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
                "Kısmi Diferansiyel Denklem Sistemi Çözücü (Coupled PDE System / Reaksiyon-Difüzyon)",
                "Bağlaşımlı Reaksiyon-Difüzyon sistemlerini (Gray-Scott, Schnakenberg, FitzHugh-Nagumo) çözer; canlı çift konsantrasyon dalgaları ve 2B Turing deseni haritası sunar.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExGrayScott = ThemeHelper.CreateExampleChip("Gray-Scott Deseni");
            this.btnExSchnakenberg = ThemeHelper.CreateExampleChip("Schnakenberg Modeli");
            this.btnExFitzHugh = ThemeHelper.CreateExampleChip("FitzHugh-Nagumo");
            this.btnExDiffusion = ThemeHelper.CreateExampleChip("Çift Difüzyon");

            this.lblReactionF = new Label();
            this.txtReactionF = new TextBox();

            this.lblReactionG = new Label();
            this.txtReactionG = new TextBox();

            this.lblInitU = new Label();
            this.txtInitU = new TextBox();

            this.lblInitV = new Label();
            this.txtInitV = new TextBox();

            this.lblDu = new Label();
            this.txtDu = new TextBox();

            this.lblDv = new Label();
            this.txtDv = new TextBox();

            this.lblL = new Label();
            this.txtL = new TextBox();

            this.lblT = new Label();
            this.txtT = new TextBox();

            this.lblNx = new Label();
            this.numNx = new NumericUpDown();

            this.lblNt = new Label();
            this.numNt = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🌀 Sistemi Çöz & Deseni Çiz", 200, 32, true);

            // TabControl for Visualizations
            this.tabSystemVisual = new TabControl();

            // Tab 1: Dual Concentration Profile & Animation
            this.tabDualProfile = new TabPage();
            this.pnlAnimToolbar = new Panel();
            this.btnPlayPause = new Button();
            this.lblCurrentTime = new Label();
            this.trkTimeStep = new TrackBar();
            this.lblAnimSpeed = new Label();
            this.cmbAnimSpeed = new ComboBox();
            this.pnlDualCanvas = new Panel();

            // Tab 2: Turing Pattern Heatmap
            this.tabTuringHeatmap = new TabPage();
            this.pnlHeatmapHeader = new Panel();
            this.lblHeatmapTitle = new Label();
            this.lblHeatmapLegend = new Label();
            this.btnExportHeatmapPng = new Button();
            this.pnlTuringCanvas = new Panel();

            // Tab 3: Phase Space (u vs v)
            this.tabPhaseSpace = new TabPage();
            this.pnlPhaseCanvas = new Panel();

            // Tab 4: Text Report
            this.tabReport = new TabPage();
            this.pnlReportToolbar = new Panel();
            this.btnExportCsv = ThemeHelper.CreateCopyButton("💾 CSV Olarak Kaydet");
            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).BeginInit();
            this.tabSystemVisual.SuspendLayout();
            this.tabDualProfile.SuspendLayout();
            this.pnlAnimToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeStep)).BeginInit();
            this.tabTuringHeatmap.SuspendLayout();
            this.pnlHeatmapHeader.SuspendLayout();
            this.tabPhaseSpace.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.pnlReportToolbar.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 168;
            this.pnlControls.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlControls.Padding = new Padding(20, 6, 20, 6);

            this.lblPresets.Text = "⚡ Reaksiyon Modelleri:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(20, 6);
            this.pnlPresets.Size = new Size(900, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExGrayScott);
            this.pnlPresets.Controls.Add(this.btnExSchnakenberg);
            this.pnlPresets.Controls.Add(this.btnExFitzHugh);
            this.pnlPresets.Controls.Add(this.btnExDiffusion);

            int yRow1 = 44;
            this.lblReactionF.Text = "F(u, v):";
            this.lblReactionF.Location = new Point(20, yRow1 + 3);
            this.lblReactionF.AutoSize = true;
            this.lblReactionF.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblReactionF.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.txtReactionF.Location = new Point(70, yRow1);
            this.txtReactionF.Size = new Size(220, 24);
            this.txtReactionF.Text = "-u*v^2 + 0.03*(1 - u)";
            this.txtReactionF.Font = new Font("Consolas", 9.5F);
            this.txtReactionF.BackColor = Color.FromArgb(15, 23, 42);
            this.txtReactionF.ForeColor = Color.White;
            this.txtReactionF.BorderStyle = BorderStyle.FixedSingle;

            this.lblReactionG.Text = "G(u, v):";
            this.lblReactionG.Location = new Point(300, yRow1 + 3);
            this.lblReactionG.AutoSize = true;
            this.lblReactionG.ForeColor = Color.FromArgb(244, 63, 94);
            this.lblReactionG.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.txtReactionG.Location = new Point(355, yRow1);
            this.txtReactionG.Size = new Size(220, 24);
            this.txtReactionG.Text = "u*v^2 - (0.03 + 0.062)*v";
            this.txtReactionG.Font = new Font("Consolas", 9.5F);
            this.txtReactionG.BackColor = Color.FromArgb(15, 23, 42);
            this.txtReactionG.ForeColor = Color.White;
            this.txtReactionG.BorderStyle = BorderStyle.FixedSingle;

            this.lblDu.Text = "Du:";
            this.lblDu.Location = new Point(590, yRow1 + 3);
            this.lblDu.AutoSize = true;
            this.lblDu.ForeColor = ThemeHelper.LightText;

            this.txtDu.Location = new Point(620, yRow1);
            this.txtDu.Size = new Size(60, 24);
            this.txtDu.Text = "0.00002";
            this.txtDu.Font = new Font("Consolas", 9F);
            this.txtDu.BackColor = Color.FromArgb(15, 23, 42);
            this.txtDu.ForeColor = Color.White;
            this.txtDu.BorderStyle = BorderStyle.FixedSingle;

            this.lblDv.Text = "Dv:";
            this.lblDv.Location = new Point(690, yRow1 + 3);
            this.lblDv.AutoSize = true;
            this.lblDv.ForeColor = ThemeHelper.LightText;

            this.txtDv.Location = new Point(720, yRow1);
            this.txtDv.Size = new Size(60, 24);
            this.txtDv.Text = "0.00001";
            this.txtDv.Font = new Font("Consolas", 9F);
            this.txtDv.BackColor = Color.FromArgb(15, 23, 42);
            this.txtDv.ForeColor = Color.White;
            this.txtDv.BorderStyle = BorderStyle.FixedSingle;

            int yRow2 = 80;
            this.lblInitU.Text = "u(x, 0):";
            this.lblInitU.Location = new Point(20, yRow2 + 3);
            this.lblInitU.AutoSize = true;
            this.lblInitU.ForeColor = ThemeHelper.LightText;

            this.txtInitU.Location = new Point(70, yRow2);
            this.txtInitU.Size = new Size(130, 24);
            this.txtInitU.Text = "1 - exp(-100*(x-0.5)^2)";
            this.txtInitU.Font = new Font("Consolas", 9F);
            this.txtInitU.BackColor = Color.FromArgb(15, 23, 42);
            this.txtInitU.ForeColor = Color.White;
            this.txtInitU.BorderStyle = BorderStyle.FixedSingle;

            this.lblInitV.Text = "v(x, 0):";
            this.lblInitV.Location = new Point(210, yRow2 + 3);
            this.lblInitV.AutoSize = true;
            this.lblInitV.ForeColor = ThemeHelper.LightText;

            this.txtInitV.Location = new Point(260, yRow2);
            this.txtInitV.Size = new Size(130, 24);
            this.txtInitV.Text = "exp(-100*(x-0.5)^2)";
            this.txtInitV.Font = new Font("Consolas", 9F);
            this.txtInitV.BackColor = Color.FromArgb(15, 23, 42);
            this.txtInitV.ForeColor = Color.White;
            this.txtInitV.BorderStyle = BorderStyle.FixedSingle;

            this.lblL.Text = "L:";
            this.lblL.Location = new Point(400, yRow2 + 3);
            this.lblL.AutoSize = true;
            this.lblL.ForeColor = ThemeHelper.LightText;

            this.txtL.Location = new Point(420, yRow2);
            this.txtL.Size = new Size(40, 24);
            this.txtL.Text = "1";
            this.txtL.BackColor = Color.FromArgb(15, 23, 42);
            this.txtL.ForeColor = Color.White;
            this.txtL.BorderStyle = BorderStyle.FixedSingle;
            this.txtL.TextAlign = HorizontalAlignment.Center;

            this.lblT.Text = "T:";
            this.lblT.Location = new Point(468, yRow2 + 3);
            this.lblT.AutoSize = true;
            this.lblT.ForeColor = ThemeHelper.LightText;

            this.txtT.Location = new Point(488, yRow2);
            this.txtT.Size = new Size(45, 24);
            this.txtT.Text = "50";
            this.txtT.BackColor = Color.FromArgb(15, 23, 42);
            this.txtT.ForeColor = Color.White;
            this.txtT.BorderStyle = BorderStyle.FixedSingle;
            this.txtT.TextAlign = HorizontalAlignment.Center;

            this.lblNx.Text = "Nx:";
            this.lblNx.Location = new Point(540, yRow2 + 3);
            this.lblNx.AutoSize = true;
            this.lblNx.ForeColor = ThemeHelper.LightText;

            this.numNx.Location = new Point(568, yRow2);
            this.numNx.Size = new Size(48, 24);
            this.numNx.Minimum = 10;
            this.numNx.Maximum = 300;
            this.numNx.Value = 60;
            this.numNx.BackColor = Color.FromArgb(15, 23, 42);
            this.numNx.ForeColor = Color.White;

            this.lblNt.Text = "Nt:";
            this.lblNt.Location = new Point(622, yRow2 + 3);
            this.lblNt.AutoSize = true;
            this.lblNt.ForeColor = ThemeHelper.LightText;

            this.numNt.Location = new Point(650, yRow2);
            this.numNt.Size = new Size(52, 24);
            this.numNt.Minimum = 10;
            this.numNt.Maximum = 2000;
            this.numNt.Value = 250;
            this.numNt.BackColor = Color.FromArgb(15, 23, 42);
            this.numNt.ForeColor = Color.White;

            this.btnSolve.Location = new Point(715, yRow2 - 2);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblReactionF);
            this.pnlControls.Controls.Add(this.txtReactionF);
            this.pnlControls.Controls.Add(this.lblReactionG);
            this.pnlControls.Controls.Add(this.txtReactionG);
            this.pnlControls.Controls.Add(this.lblDu);
            this.pnlControls.Controls.Add(this.txtDu);
            this.pnlControls.Controls.Add(this.lblDv);
            this.pnlControls.Controls.Add(this.txtDv);
            this.pnlControls.Controls.Add(this.lblInitU);
            this.pnlControls.Controls.Add(this.txtInitU);
            this.pnlControls.Controls.Add(this.lblInitV);
            this.pnlControls.Controls.Add(this.txtInitV);
            this.pnlControls.Controls.Add(this.lblL);
            this.pnlControls.Controls.Add(this.txtL);
            this.pnlControls.Controls.Add(this.lblT);
            this.pnlControls.Controls.Add(this.txtT);
            this.pnlControls.Controls.Add(this.lblNx);
            this.pnlControls.Controls.Add(this.numNx);
            this.pnlControls.Controls.Add(this.lblNt);
            this.pnlControls.Controls.Add(this.numNt);
            this.pnlControls.Controls.Add(this.btnSolve);

            // ================== TABBED VISUALS ==================
            this.tabSystemVisual.Dock = DockStyle.Fill;
            this.tabSystemVisual.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.tabSystemVisual.Padding = new Point(12, 4);

            // Tab 1: Dual Concentration Profile
            this.tabDualProfile.Text = "Çift Konsantrasyon Profili u(x) & v(x)";
            this.tabDualProfile.BackColor = Color.FromArgb(15, 23, 42);
            this.tabDualProfile.Padding = new Padding(8);

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

            this.pnlDualCanvas.Dock = DockStyle.Fill;
            this.pnlDualCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlDualCanvas.DoubleBuffered(true);

            this.tabDualProfile.Controls.Add(this.pnlDualCanvas);
            this.tabDualProfile.Controls.Add(this.pnlAnimToolbar);

            // Tab 2: Turing Pattern Heatmap
            this.tabTuringHeatmap.Text = "Mekan-Zaman Turing Deseni";
            this.tabTuringHeatmap.BackColor = Color.FromArgb(15, 23, 42);
            this.tabTuringHeatmap.Padding = new Padding(8);

            this.pnlHeatmapHeader.Dock = DockStyle.Top;
            this.pnlHeatmapHeader.Height = 32;
            this.pnlHeatmapHeader.BackColor = Color.FromArgb(24, 33, 52);

            this.lblHeatmapTitle.Text = "🗺️ Desen Morfolojisi u(x, t):";
            this.lblHeatmapTitle.Dock = DockStyle.Left;
            this.lblHeatmapTitle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblHeatmapTitle.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblHeatmapTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblHeatmapTitle.Padding = new Padding(8, 0, 0, 0);

            this.lblHeatmapLegend.Text = "Düşük (Mavi) ── Orta (Yeşil/Sarı) ── Yüksek (Kırmızı)";
            this.lblHeatmapLegend.Dock = DockStyle.Left;
            this.lblHeatmapLegend.Font = new Font("Segoe UI", 8F);
            this.lblHeatmapLegend.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblHeatmapLegend.TextAlign = ContentAlignment.MiddleLeft;
            this.lblHeatmapLegend.Padding = new Padding(20, 0, 0, 0);

            this.btnExportHeatmapPng.Text = "📷 PNG Kaydet";
            this.btnExportHeatmapPng.Dock = DockStyle.Right;
            this.btnExportHeatmapPng.Width = 100;
            this.btnExportHeatmapPng.FlatStyle = FlatStyle.Flat;
            this.btnExportHeatmapPng.BackColor = Color.FromArgb(33, 44, 70);
            this.btnExportHeatmapPng.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnExportHeatmapPng.Font = new Font("Segoe UI", 8F);
            this.btnExportHeatmapPng.FlatAppearance.BorderSize = 0;
            this.btnExportHeatmapPng.Cursor = Cursors.Hand;

            this.pnlHeatmapHeader.Controls.Add(this.lblHeatmapLegend);
            this.pnlHeatmapHeader.Controls.Add(this.lblHeatmapTitle);
            this.pnlHeatmapHeader.Controls.Add(this.btnExportHeatmapPng);

            this.pnlTuringCanvas.Dock = DockStyle.Fill;
            this.pnlTuringCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlTuringCanvas.DoubleBuffered(true);

            this.tabTuringHeatmap.Controls.Add(this.pnlTuringCanvas);
            this.tabTuringHeatmap.Controls.Add(this.pnlHeatmapHeader);

            // Tab 3: Phase Space (u vs v)
            this.tabPhaseSpace.Text = "Faz Uzayı (u vs v)";
            this.tabPhaseSpace.BackColor = Color.FromArgb(15, 23, 42);
            this.tabPhaseSpace.Padding = new Padding(8);

            this.pnlPhaseCanvas.Dock = DockStyle.Fill;
            this.pnlPhaseCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlPhaseCanvas.DoubleBuffered(true);

            this.tabPhaseSpace.Controls.Add(this.pnlPhaseCanvas);

            // Tab 4: Report
            this.tabReport.Text = "Rapor & Dışa Aktar";
            this.tabReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabReport.Padding = new Padding(6);

            this.pnlReportToolbar.Dock = DockStyle.Top;
            this.pnlReportToolbar.Height = 32;
            this.pnlReportToolbar.BackColor = Color.FromArgb(24, 33, 52);

            this.btnExportCsv.Dock = DockStyle.Right;
            this.btnExportCsv.Height = 28;
            this.pnlReportToolbar.Controls.Add(this.btnExportCsv);

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 15, 28);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            this.tabReport.Controls.Add(this.txtOutput);
            this.tabReport.Controls.Add(this.pnlReportToolbar);

            this.tabSystemVisual.TabPages.Add(this.tabDualProfile);
            this.tabSystemVisual.TabPages.Add(this.tabTuringHeatmap);
            this.tabSystemVisual.TabPages.Add(this.tabPhaseSpace);
            this.tabSystemVisual.TabPages.Add(this.tabReport);

            // Assemble
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.tabSystemVisual);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "PdeSystemSolverControl";
            this.Size = new Size(950, 700);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).EndInit();
            this.tabSystemVisual.ResumeLayout(false);
            this.tabDualProfile.ResumeLayout(false);
            this.pnlAnimToolbar.ResumeLayout(false);
            this.pnlAnimToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkTimeStep)).EndInit();
            this.tabTuringHeatmap.ResumeLayout(false);
            this.pnlHeatmapHeader.ResumeLayout(false);
            this.tabPhaseSpace.ResumeLayout(false);
            this.tabReport.ResumeLayout(false);
            this.pnlReportToolbar.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlControls;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnExGrayScott;
        private Button btnExSchnakenberg;
        private Button btnExFitzHugh;
        private Button btnExDiffusion;

        private Label lblReactionF;
        private TextBox txtReactionF;
        private Label lblReactionG;
        private TextBox txtReactionG;
        private Label lblInitU;
        private TextBox txtInitU;
        private Label lblInitV;
        private TextBox txtInitV;
        private Label lblDu;
        private TextBox txtDu;
        private Label lblDv;
        private TextBox txtDv;
        private Label lblL;
        private TextBox txtL;
        private Label lblT;
        private TextBox txtT;
        private Label lblNx;
        private NumericUpDown numNx;
        private Label lblNt;
        private NumericUpDown numNt;
        private Button btnSolve;

        private TabControl tabSystemVisual;
        private TabPage tabDualProfile;
        private Panel pnlAnimToolbar;
        private Button btnPlayPause;
        private Label lblCurrentTime;
        private TrackBar trkTimeStep;
        private Label lblAnimSpeed;
        private ComboBox cmbAnimSpeed;
        private Panel pnlDualCanvas;

        private TabPage tabTuringHeatmap;
        private Panel pnlHeatmapHeader;
        private Label lblHeatmapTitle;
        private Label lblHeatmapLegend;
        private Button btnExportHeatmapPng;
        private Panel pnlTuringCanvas;

        private TabPage tabPhaseSpace;
        private Panel pnlPhaseCanvas;

        private TabPage tabReport;
        private Panel pnlReportToolbar;
        private Button btnExportCsv;
        private RichTextBox txtOutput;
    }
}
