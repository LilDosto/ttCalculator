using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class OdeSolverControl
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
                "Diferansiyel Denklem Çözücü (1 Değişkenli ODE / Başlangıç Değer Problemi)",
                "dy/dx = f(x, y) başlangıç değer problemini Runge-Kutta (RK4, RK8), Heun (RK2) ve Euler ile çözer; dinamik çözüm eğrisi ve adım tablosu sunar.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("y' = x + y  [y(0)=1]");
            this.btnEx2 = ThemeHelper.CreateExampleChip("y' = -2*x*y  [y(0)=1]");
            this.btnEx3 = ThemeHelper.CreateExampleChip("y' = cos(x) - y  [y(0)=0]");
            this.btnEx4 = ThemeHelper.CreateExampleChip("y' = y*(1 - y/10) [Lojistik]");

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblMethod = new Label();
            this.cmbMethod = new ComboBox();

            this.lblX0 = new Label();
            this.txtX0 = new TextBox();
            this.lblY0 = new Label();
            this.txtY0 = new TextBox();
            this.lblXEnd = new Label();
            this.txtXEnd = new TextBox();
            this.lblSteps = new Label();
            this.numSteps = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 Denklemi Çöz", 150, 32, true);

            // Split Container for Interactive Canvas (Left) + Results/Table (Right)
            this.splitOde = new SplitContainer();

            // Left: Canvas Area
            this.pnlCanvasHeader = new Panel();
            this.lblCanvasTitle = new Label();
            this.btnExportPng = new Button();
            this.btnNavToGraph = new Button();
            this.pnlOdeCanvas = new Panel();

            // Right: Tabbed Output
            this.tabOdeResults = new TabControl();
            this.tabSummary = new TabPage();
            this.pnlKpiCard = new Panel();
            this.lblKpiFinalVal = new Label();
            this.lblKpiSub = new Label();
            this.pnlMetricsContainer = new Panel();
            this.lblMetricSteps = new Label();
            this.lblMetricH = new Label();
            this.lblMetricElapsed = new Label();
            this.lblMetricMethod = new Label();

            this.tabStepsTable = new TabPage();
            this.pnlTableToolbar = new Panel();
            this.btnCopyTsv = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala (TSV)");
            this.dgvSteps = new DataGridView();

            this.tabTextReport = new TabPage();
            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitOde)).BeginInit();
            this.splitOde.Panel1.SuspendLayout();
            this.splitOde.Panel2.SuspendLayout();
            this.splitOde.SuspendLayout();
            this.pnlCanvasHeader.SuspendLayout();
            this.tabOdeResults.SuspendLayout();
            this.tabSummary.SuspendLayout();
            this.pnlKpiCard.SuspendLayout();
            this.pnlMetricsContainer.SuspendLayout();
            this.tabStepsTable.SuspendLayout();
            this.pnlTableToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).BeginInit();
            this.tabTextReport.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 125;
            this.pnlControls.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlControls.Padding = new Padding(20, 8, 20, 8);

            this.lblPresets.Text = "⚡ Hızlı Şablonlar:";
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

            int yRow1 = 44;
            this.lblEquation.Text = "dy/dx = f(x, y):";
            this.lblEquation.Location = new Point(20, yRow1 + 3);
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblEquation.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.txtEquation.Location = new Point(125, yRow1);
            this.txtEquation.Size = new Size(240, 25);
            this.txtEquation.Text = "x + y";
            this.txtEquation.Font = new Font("Consolas", 10.5F);
            this.txtEquation.BackColor = Color.FromArgb(15, 23, 42);
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;

            this.lblMethod.Text = "Yöntem:";
            this.lblMethod.Location = new Point(380, yRow1 + 3);
            this.lblMethod.AutoSize = true;
            this.lblMethod.ForeColor = ThemeHelper.LightText;
            this.lblMethod.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.cmbMethod.Location = new Point(445, yRow1);
            this.cmbMethod.Size = new Size(230, 25);
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.Font = new Font("Segoe UI", 9F);
            this.cmbMethod.Items.AddRange(new object[] {
                "Runge-Kutta 4 (RK4 - Standart)",
                "Runge-Kutta 8 (RK8 - Ultra Hassas)",
                "Heun / Geliştirilmiş Euler (RK2)",
                "Euler Yöntemi (1. Derece)"
            });
            this.cmbMethod.SelectedIndex = 0;

            this.btnSolve.Location = new Point(690, yRow1 - 3);

            int yRow2 = 82;
            this.lblX0.Text = "x₀ (Başlangıç):";
            this.lblX0.Location = new Point(20, yRow2 + 4);
            this.lblX0.AutoSize = true;
            this.lblX0.ForeColor = ThemeHelper.LightText;
            this.lblX0.Font = new Font("Segoe UI", 9F);

            this.txtX0.Location = new Point(110, yRow2);
            this.txtX0.Size = new Size(55, 24);
            this.txtX0.Text = "0";
            this.txtX0.Font = new Font("Consolas", 10F);
            this.txtX0.BackColor = Color.FromArgb(15, 23, 42);
            this.txtX0.ForeColor = Color.White;
            this.txtX0.BorderStyle = BorderStyle.FixedSingle;
            this.txtX0.TextAlign = HorizontalAlignment.Center;

            this.lblY0.Text = "y(x₀):";
            this.lblY0.Location = new Point(180, yRow2 + 4);
            this.lblY0.AutoSize = true;
            this.lblY0.ForeColor = ThemeHelper.LightText;
            this.lblY0.Font = new Font("Segoe UI", 9F);

            this.txtY0.Location = new Point(220, yRow2);
            this.txtY0.Size = new Size(55, 24);
            this.txtY0.Text = "1";
            this.txtY0.Font = new Font("Consolas", 10F);
            this.txtY0.BackColor = Color.FromArgb(15, 23, 42);
            this.txtY0.ForeColor = Color.White;
            this.txtY0.BorderStyle = BorderStyle.FixedSingle;
            this.txtY0.TextAlign = HorizontalAlignment.Center;

            this.lblXEnd.Text = "x_son (Hedef):";
            this.lblXEnd.Location = new Point(290, yRow2 + 4);
            this.lblXEnd.AutoSize = true;
            this.lblXEnd.ForeColor = ThemeHelper.LightText;
            this.lblXEnd.Font = new Font("Segoe UI", 9F);

            this.txtXEnd.Location = new Point(380, yRow2);
            this.txtXEnd.Size = new Size(60, 24);
            this.txtXEnd.Text = "2";
            this.txtXEnd.Font = new Font("Consolas", 10F);
            this.txtXEnd.BackColor = Color.FromArgb(15, 23, 42);
            this.txtXEnd.ForeColor = Color.White;
            this.txtXEnd.BorderStyle = BorderStyle.FixedSingle;
            this.txtXEnd.TextAlign = HorizontalAlignment.Center;

            this.lblSteps.Text = "Adım Sayısı (N):";
            this.lblSteps.Location = new Point(455, yRow2 + 4);
            this.lblSteps.AutoSize = true;
            this.lblSteps.ForeColor = ThemeHelper.LightText;
            this.lblSteps.Font = new Font("Segoe UI", 9F);

            this.numSteps.Location = new Point(555, yRow2);
            this.numSteps.Size = new Size(65, 24);
            this.numSteps.Minimum = 2;
            this.numSteps.Maximum = 5000;
            this.numSteps.Value = 50;
            this.numSteps.BackColor = Color.FromArgb(15, 23, 42);
            this.numSteps.ForeColor = Color.White;

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEquation);
            this.pnlControls.Controls.Add(this.txtEquation);
            this.pnlControls.Controls.Add(this.lblMethod);
            this.pnlControls.Controls.Add(this.cmbMethod);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.lblX0);
            this.pnlControls.Controls.Add(this.txtX0);
            this.pnlControls.Controls.Add(this.lblY0);
            this.pnlControls.Controls.Add(this.txtY0);
            this.pnlControls.Controls.Add(this.lblXEnd);
            this.pnlControls.Controls.Add(this.txtXEnd);
            this.pnlControls.Controls.Add(this.lblSteps);
            this.pnlControls.Controls.Add(this.numSteps);

            // ================== SPLIT ODE CONTAINER ==================
            this.splitOde.Dock = DockStyle.Fill;
            this.splitOde.Orientation = Orientation.Vertical;
            this.splitOde.SplitterDistance = 490;
            this.splitOde.SplitterWidth = 8;
            this.splitOde.BackColor = Color.FromArgb(15, 23, 42);
            this.splitOde.Padding = new Padding(12);

            // Left: Canvas
            this.splitOde.Panel1.BackColor = Color.FromArgb(20, 28, 48);
            this.splitOde.Panel1.Padding = new Padding(8);

            this.pnlCanvasHeader.Dock = DockStyle.Top;
            this.pnlCanvasHeader.Height = 32;
            this.pnlCanvasHeader.BackColor = Color.FromArgb(24, 33, 52);

            this.lblCanvasTitle.Text = "📈 İnteraktif Çözüm Eğrisi y(x)";
            this.lblCanvasTitle.Dock = DockStyle.Left;
            this.lblCanvasTitle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblCanvasTitle.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblCanvasTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblCanvasTitle.Padding = new Padding(8, 0, 0, 0);

            this.btnNavToGraph.Text = "📈 Grafik Çiziciye Aktar";
            this.btnNavToGraph.Dock = DockStyle.Right;
            this.btnNavToGraph.Width = 145;
            this.btnNavToGraph.FlatStyle = FlatStyle.Flat;
            this.btnNavToGraph.BackColor = Color.FromArgb(16, 185, 129);
            this.btnNavToGraph.ForeColor = Color.White;
            this.btnNavToGraph.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnNavToGraph.FlatAppearance.BorderSize = 0;
            this.btnNavToGraph.Cursor = Cursors.Hand;

            this.btnExportPng.Text = "📷 PNG Kaydet";
            this.btnExportPng.Dock = DockStyle.Right;
            this.btnExportPng.Width = 95;
            this.btnExportPng.FlatStyle = FlatStyle.Flat;
            this.btnExportPng.BackColor = Color.FromArgb(33, 44, 70);
            this.btnExportPng.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnExportPng.Font = new Font("Segoe UI", 8F);
            this.btnExportPng.FlatAppearance.BorderSize = 0;
            this.btnExportPng.Cursor = Cursors.Hand;

            this.pnlCanvasHeader.Controls.Add(this.lblCanvasTitle);
            this.pnlCanvasHeader.Controls.Add(this.btnExportPng);
            this.pnlCanvasHeader.Controls.Add(this.btnNavToGraph);

            this.pnlOdeCanvas.Dock = DockStyle.Fill;
            this.pnlOdeCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlOdeCanvas.DoubleBuffered(true);

            this.splitOde.Panel1.Controls.Add(this.pnlOdeCanvas);
            this.splitOde.Panel1.Controls.Add(this.pnlCanvasHeader);

            // Right: Tabbed Output
            this.splitOde.Panel2.BackColor = Color.FromArgb(20, 28, 48);
            this.splitOde.Panel2.Padding = new Padding(4);

            this.tabOdeResults.Dock = DockStyle.Fill;
            this.tabOdeResults.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // Tab 1: Summary KPI
            this.tabSummary.Text = "Çözüm Özeti";
            this.tabSummary.BackColor = Color.FromArgb(24, 33, 52);
            this.tabSummary.Padding = new Padding(12);

            this.pnlKpiCard.Dock = DockStyle.Top;
            this.pnlKpiCard.Height = 90;
            this.pnlKpiCard.BackColor = Color.FromArgb(18, 26, 44);
            this.pnlKpiCard.Padding = new Padding(14);

            this.lblKpiFinalVal.Dock = DockStyle.Top;
            this.lblKpiFinalVal.Height = 36;
            this.lblKpiFinalVal.Font = new Font("Consolas", 15F, FontStyle.Bold);
            this.lblKpiFinalVal.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblKpiFinalVal.Text = "y(x_son) ≈ —";

            this.lblKpiSub.Dock = DockStyle.Bottom;
            this.lblKpiSub.Height = 22;
            this.lblKpiSub.Font = new Font("Segoe UI", 8.5F);
            this.lblKpiSub.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblKpiSub.Text = "Hesaplama bekleniyor.";

            this.pnlKpiCard.Controls.Add(this.lblKpiFinalVal);
            this.pnlKpiCard.Controls.Add(this.lblKpiSub);

            this.pnlMetricsContainer.Dock = DockStyle.Fill;
            this.pnlMetricsContainer.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlMetricsContainer.Padding = new Padding(12);

            this.lblMetricSteps.Text = "• Toplam Adım (N) : —";
            this.lblMetricSteps.Location = new Point(14, 20);
            this.lblMetricSteps.AutoSize = true;
            this.lblMetricSteps.Font = new Font("Consolas", 10F);
            this.lblMetricSteps.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblMetricH.Text = "• Adım Boyutu (h) : —";
            this.lblMetricH.Location = new Point(14, 52);
            this.lblMetricH.AutoSize = true;
            this.lblMetricH.Font = new Font("Consolas", 10F);
            this.lblMetricH.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblMetricElapsed.Text = "• Çözüm Süresi    : —";
            this.lblMetricElapsed.Location = new Point(14, 84);
            this.lblMetricElapsed.AutoSize = true;
            this.lblMetricElapsed.Font = new Font("Consolas", 10F);
            this.lblMetricElapsed.ForeColor = Color.FromArgb(226, 232, 240);

            this.lblMetricMethod.Text = "• Yöntem          : —";
            this.lblMetricMethod.Location = new Point(14, 116);
            this.lblMetricMethod.AutoSize = true;
            this.lblMetricMethod.Font = new Font("Consolas", 10F);
            this.lblMetricMethod.ForeColor = Color.FromArgb(56, 189, 248);

            this.pnlMetricsContainer.Controls.Add(this.lblMetricSteps);
            this.pnlMetricsContainer.Controls.Add(this.lblMetricH);
            this.pnlMetricsContainer.Controls.Add(this.lblMetricElapsed);
            this.pnlMetricsContainer.Controls.Add(this.lblMetricMethod);

            this.tabSummary.Controls.Add(this.pnlMetricsContainer);
            this.tabSummary.Controls.Add(this.pnlKpiCard);

            // Tab 2: Steps Table
            this.tabStepsTable.Text = "Adım Tablosu";
            this.tabStepsTable.BackColor = Color.FromArgb(24, 33, 52);
            this.tabStepsTable.Padding = new Padding(6);

            this.pnlTableToolbar.Dock = DockStyle.Top;
            this.pnlTableToolbar.Height = 32;
            this.pnlTableToolbar.BackColor = Color.FromArgb(24, 33, 52);

            this.btnCopyTsv.Dock = DockStyle.Right;
            this.btnCopyTsv.Height = 28;
            this.pnlTableToolbar.Controls.Add(this.btnCopyTsv);

            SetupGrid(this.dgvSteps);
            this.tabStepsTable.Controls.Add(this.dgvSteps);
            this.tabStepsTable.Controls.Add(this.pnlTableToolbar);

            // Tab 3: Text Report
            this.tabTextReport.Text = "Metin Raporu";
            this.tabTextReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTextReport.Padding = new Padding(6);

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 15, 28);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            this.tabTextReport.Controls.Add(this.txtOutput);

            this.tabOdeResults.TabPages.Add(this.tabSummary);
            this.tabOdeResults.TabPages.Add(this.tabStepsTable);
            this.tabOdeResults.TabPages.Add(this.tabTextReport);

            this.splitOde.Panel2.Controls.Add(this.tabOdeResults);

            // Assemble OdeSolverControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.splitOde);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "OdeSolverControl";
            this.Size = new Size(940, 680);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
            this.splitOde.Panel1.ResumeLayout(false);
            this.splitOde.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitOde)).EndInit();
            this.splitOde.ResumeLayout(false);
            this.pnlCanvasHeader.ResumeLayout(false);
            this.tabOdeResults.ResumeLayout(false);
            this.tabSummary.ResumeLayout(false);
            this.pnlKpiCard.ResumeLayout(false);
            this.pnlMetricsContainer.ResumeLayout(false);
            this.pnlMetricsContainer.PerformLayout();
            this.tabStepsTable.ResumeLayout(false);
            this.pnlTableToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSteps)).EndInit();
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
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;
        private Button btnEx4;
        private Label lblEquation;
        private TextBox txtEquation;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Button btnSolve;
        private Label lblX0;
        private TextBox txtX0;
        private Label lblY0;
        private TextBox txtY0;
        private Label lblXEnd;
        private TextBox txtXEnd;
        private Label lblSteps;
        private NumericUpDown numSteps;

        private SplitContainer splitOde;
        private Panel pnlCanvasHeader;
        private Label lblCanvasTitle;
        private Button btnExportPng;
        private Button btnNavToGraph;
        private Panel pnlOdeCanvas;

        private TabControl tabOdeResults;
        private TabPage tabSummary;
        private Panel pnlKpiCard;
        private Label lblKpiFinalVal;
        private Label lblKpiSub;
        private Panel pnlMetricsContainer;
        private Label lblMetricSteps;
        private Label lblMetricH;
        private Label lblMetricElapsed;
        private Label lblMetricMethod;

        private TabPage tabStepsTable;
        private Panel pnlTableToolbar;
        private Button btnCopyTsv;
        private DataGridView dgvSteps;

        private TabPage tabTextReport;
        private RichTextBox txtOutput;
    }
}
