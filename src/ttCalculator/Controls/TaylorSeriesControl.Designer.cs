using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class TaylorSeriesControl
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
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Taylor Serisi Hesaplayıcı (Tek Değişkenli)",
                "Bir f(x) fonksiyonunun verilen a merkez noktasındaki n. dereceden Taylor / Maclaurin polinom açılımı, katsayıları ve hata analizi.");

            this.pnlContent = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExSin = ThemeHelper.CreateExampleChip("Örnek 1: sin(x) (a=0, n=5)");
            this.btnExCos = ThemeHelper.CreateExampleChip("Örnek 2: cos(x) (a=0, n=6)");
            this.btnExExp = ThemeHelper.CreateExampleChip("Örnek 3: exp(x) (a=0, n=5)");
            this.btnExLn = ThemeHelper.CreateExampleChip("Örnek 4: ln(1+x) (a=0, n=4)");
            this.btnExGeom = ThemeHelper.CreateExampleChip("Örnek 5: 1/(1-x) (a=0, n=5)");
            this.btnExAtan = ThemeHelper.CreateExampleChip("Örnek 6: atan(x) (a=0, n=5)");

            // Unified Input & Hero Card
            this.pnlInputCard = new Panel();
            this.lblFunction = new Label();
            this.txtFunction = new TextBox();
            this.lblVariable = new Label();
            this.txtVariable = new TextBox();
            this.lblCenterPoint = new Label();
            this.txtCenterPoint = new TextBox();
            this.btnMaclaurin = new Button();
            this.lblOrder = new Label();
            this.numOrder = new NumericUpDown();
            this.btnCalculate = ThemeHelper.CreateActionButton("📐 Taylor Polinomunu Hesapla", 200, 28, true);

            // Hero Polynomial row inside card
            this.lblHeroPolyTitle = new Label();
            this.lblHeroPolyValue = new Label();
            this.pnlHeroButtons = new FlowLayoutPanel();
            this.btnNavToGraph = new Button();
            this.btnCopyPoly = ThemeHelper.CreateCopyButton("📋 Kopyala");
            this.btnCopyLatex = ThemeHelper.CreateCopyButton("LaTeX");

            // Split Container for Canvas + Tabs
            this.splitVisual = new SplitContainer();
            this.pnlCanvasHeader = new Panel();
            this.lblCanvasTitle = new Label();
            this.lblCanvasLegend = new Label();
            this.pnlMiniCanvas = new Panel();

            // Tabs for Table, Error Analysis, Full Text
            this.tabResults = new TabControl();
            this.tabTable = new TabPage();
            this.dgvTerms = new DataGridView();
            this.tabError = new TabPage();
            this.lblLagrangeTheory = new Label();
            this.dgvError = new DataGridView();
            this.tabReport = new TabPage();
            this.txtResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlPresets.SuspendLayout();
            this.pnlInputCard.SuspendLayout();
            this.pnlHeroButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitVisual)).BeginInit();
            this.splitVisual.Panel1.SuspendLayout();
            this.splitVisual.Panel2.SuspendLayout();
            this.splitVisual.SuspendLayout();
            this.pnlCanvasHeader.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerms)).BeginInit();
            this.tabError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvError)).BeginInit();
            this.tabReport.SuspendLayout();
            this.SuspendLayout();

            // Header Panel
            this.pnlHeader.Dock = DockStyle.Top;

            // Content Panel (Hosts Presets, Card and SplitContainer)
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(16, 4, 16, 10);
            this.pnlContent.AutoScroll = false;

            // Presets Bar (Dock Top)
            this.pnlPresets.Dock = DockStyle.Top;
            this.pnlPresets.Height = 32;
            this.pnlPresets.WrapContents = false;
            this.pnlPresets.AutoScroll = false;
            this.pnlPresets.Margin = new Padding(0, 0, 0, 4);

            this.lblPresets.Text = "⚡ Hızlı:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 6, 6, 0);

            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExSin);
            this.pnlPresets.Controls.Add(this.btnExCos);
            this.pnlPresets.Controls.Add(this.btnExExp);
            this.pnlPresets.Controls.Add(this.btnExLn);
            this.pnlPresets.Controls.Add(this.btnExGeom);
            this.pnlPresets.Controls.Add(this.btnExAtan);

            // Unified Input & Hero Card (Dock Top)
            this.pnlInputCard.Dock = DockStyle.Top;
            this.pnlInputCard.Height = 88;
            this.pnlInputCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputCard.Padding = new Padding(12, 6, 12, 6);
            this.pnlInputCard.Margin = new Padding(0, 0, 0, 6);

            void StyleLabel(Label l, string text, Point loc)
            {
                l.Text = text;
                l.Location = loc;
                l.AutoSize = true;
                l.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
                l.ForeColor = ThemeHelper.LightText;
            }

            void StyleInput(TextBox t, string def, Point loc, Size sz)
            {
                t.Location = loc;
                t.Size = sz;
                t.BackColor = Color.FromArgb(15, 23, 42);
                t.BorderStyle = BorderStyle.FixedSingle;
                t.ForeColor = Color.White;
                t.Font = new Font("Consolas", 10F);
                t.Text = def;
            }

            // Row 1: Inputs
            StyleLabel(this.lblFunction, "f(x):", new Point(12, 11));
            StyleInput(this.txtFunction, "sin(x)", new Point(48, 8), new Size(220, 24));

            StyleLabel(this.lblVariable, "Değişken:", new Point(276, 11));
            StyleInput(this.txtVariable, "x", new Point(340, 8), new Size(32, 24));

            StyleLabel(this.lblCenterPoint, "Merkez a:", new Point(382, 11));
            StyleInput(this.txtCenterPoint, "0", new Point(448, 8), new Size(42, 24));

            this.btnMaclaurin.Text = "a=0 (Macl.)";
            this.btnMaclaurin.Location = new Point(496, 7);
            this.btnMaclaurin.Size = new Size(88, 26);
            this.btnMaclaurin.BackColor = Color.FromArgb(33, 44, 70);
            this.btnMaclaurin.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnMaclaurin.FlatStyle = FlatStyle.Flat;
            this.btnMaclaurin.FlatAppearance.BorderSize = 0;
            this.btnMaclaurin.Font = new Font("Segoe UI", 8F);
            this.btnMaclaurin.Cursor = Cursors.Hand;

            StyleLabel(this.lblOrder, "Derece n:", new Point(594, 11));
            this.numOrder.Location = new Point(656, 8);
            this.numOrder.Size = new Size(44, 24);
            this.numOrder.Minimum = 1;
            this.numOrder.Maximum = 20;
            this.numOrder.Value = 5;
            this.numOrder.BackColor = Color.FromArgb(15, 23, 42);
            this.numOrder.ForeColor = Color.White;
            this.numOrder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            this.btnCalculate.Location = new Point(708, 6);
            this.btnCalculate.Size = new Size(185, 28);
            this.btnCalculate.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);

            // Row 2: Polynomial Output & Action Buttons
            this.lblHeroPolyTitle.Text = "Pn(x) =";
            this.lblHeroPolyTitle.Location = new Point(12, 48);
            this.lblHeroPolyTitle.AutoSize = true;
            this.lblHeroPolyTitle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblHeroPolyTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblHeroPolyValue.Text = "P_n(x) = —";
            this.lblHeroPolyValue.Location = new Point(68, 46);
            this.lblHeroPolyValue.AutoSize = false;
            this.lblHeroPolyValue.Size = new Size(540, 34);
            this.lblHeroPolyValue.Font = new Font("Consolas", 10.5F, FontStyle.Bold);
            this.lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblHeroPolyValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.pnlHeroButtons.FlowDirection = FlowDirection.RightToLeft;
            this.pnlHeroButtons.Location = new Point(610, 44);
            this.pnlHeroButtons.Size = new Size(285, 34);
            this.pnlHeroButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnNavToGraph.Text = "📈 Grafikte Göster";
            this.btnNavToGraph.Size = new Size(125, 28);
            this.btnNavToGraph.BackColor = Color.FromArgb(16, 185, 129);
            this.btnNavToGraph.ForeColor = Color.White;
            this.btnNavToGraph.FlatStyle = FlatStyle.Flat;
            this.btnNavToGraph.FlatAppearance.BorderSize = 0;
            this.btnNavToGraph.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnNavToGraph.Cursor = Cursors.Hand;

            this.btnCopyPoly.Height = 28;
            this.btnCopyPoly.Width = 75;
            this.btnCopyLatex.Height = 28;
            this.btnCopyLatex.Width = 65;

            this.pnlHeroButtons.Controls.Add(this.btnNavToGraph);
            this.pnlHeroButtons.Controls.Add(this.btnCopyLatex);
            this.pnlHeroButtons.Controls.Add(this.btnCopyPoly);

            this.pnlInputCard.Controls.Add(this.lblFunction);
            this.pnlInputCard.Controls.Add(this.txtFunction);
            this.pnlInputCard.Controls.Add(this.lblVariable);
            this.pnlInputCard.Controls.Add(this.txtVariable);
            this.pnlInputCard.Controls.Add(this.lblCenterPoint);
            this.pnlInputCard.Controls.Add(this.txtCenterPoint);
            this.pnlInputCard.Controls.Add(this.btnMaclaurin);
            this.pnlInputCard.Controls.Add(this.lblOrder);
            this.pnlInputCard.Controls.Add(this.numOrder);
            this.pnlInputCard.Controls.Add(this.btnCalculate);
            this.pnlInputCard.Controls.Add(this.lblHeroPolyTitle);
            this.pnlInputCard.Controls.Add(this.lblHeroPolyValue);
            this.pnlInputCard.Controls.Add(this.pnlHeroButtons);

            // Split Container for Mini Canvas and Tabbed Details (Dock Fill!)
            this.splitVisual.Dock = DockStyle.Fill;
            this.splitVisual.Orientation = Orientation.Vertical;
            this.splitVisual.SplitterDistance = 380;
            this.splitVisual.SplitterWidth = 6;
            this.splitVisual.BackColor = ThemeHelper.DarkBackground;

            // Panel 1: Mini 2D Convergence Canvas
            this.splitVisual.Panel1.BackColor = Color.FromArgb(18, 26, 43);
            this.splitVisual.Panel1.Padding = new Padding(0);

            this.pnlCanvasHeader.Dock = DockStyle.Top;
            this.pnlCanvasHeader.Height = 26;
            this.pnlCanvasHeader.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlCanvasHeader.Padding = new Padding(6, 4, 6, 4);

            this.lblCanvasTitle.Text = "📐 2D Yakınsama Önizleme";
            this.lblCanvasTitle.Dock = DockStyle.Left;
            this.lblCanvasTitle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblCanvasTitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblCanvasTitle.AutoSize = true;

            this.lblCanvasLegend.Text = "— f(x) (Cyan)   — P_n(x) (Yeşil)   ● a (Turuncu)";
            this.lblCanvasLegend.Dock = DockStyle.Right;
            this.lblCanvasLegend.Font = new Font("Segoe UI", 7.5F);
            this.lblCanvasLegend.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblCanvasLegend.AutoSize = true;

            this.pnlCanvasHeader.Controls.Add(this.lblCanvasTitle);
            this.pnlCanvasHeader.Controls.Add(this.lblCanvasLegend);

            this.pnlMiniCanvas.Dock = DockStyle.Fill;
            this.pnlMiniCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlMiniCanvas.DoubleBuffered(true);

            this.splitVisual.Panel1.Controls.Add(this.pnlMiniCanvas);
            this.splitVisual.Panel1.Controls.Add(this.pnlCanvasHeader);

            // Panel 2: Tabbed Results
            this.splitVisual.Panel2.Controls.Add(this.tabResults);
            this.tabResults.Dock = DockStyle.Fill;
            this.tabResults.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // Tab 1: Katsayılar & Türevler Tablosu
            this.tabTable.Text = "Katsayılar & Türev Tablosu";
            this.tabTable.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTable.Padding = new Padding(4);
            SetupGrid(this.dgvTerms);
            this.tabTable.Controls.Add(this.dgvTerms);

            // Tab 2: Hata Analizi
            this.tabError.Text = "Hata & Lagrange Kalanı";
            this.tabError.BackColor = Color.FromArgb(24, 33, 52);
            this.tabError.Padding = new Padding(4);

            this.lblLagrangeTheory.Text = "📜 Lagrange Kalan Terimi: R_n(x) = f^(n+1)(ξ) / (n+1)! · (x - a)^(n+1)   (ξ, a ile x arasındadır)";
            this.lblLagrangeTheory.Dock = DockStyle.Top;
            this.lblLagrangeTheory.Height = 24;
            this.lblLagrangeTheory.Font = new Font("Consolas", 8.5F, FontStyle.Bold);
            this.lblLagrangeTheory.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblLagrangeTheory.TextAlign = ContentAlignment.MiddleLeft;

            SetupGrid(this.dgvError);
            this.tabError.Controls.Add(this.dgvError);
            this.tabError.Controls.Add(this.lblLagrangeTheory);

            // Tab 3: Detaylı Analitik Rapor
            this.tabReport.Text = "Detaylı Rapor";
            this.tabReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabReport.Padding = new Padding(4);

            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(11, 15, 28);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);
            this.tabReport.Controls.Add(this.txtResult);

            this.tabResults.TabPages.Add(this.tabTable);
            this.tabResults.TabPages.Add(this.tabError);
            this.tabResults.TabPages.Add(this.tabReport);

            // Add to pnlContent with proper Z-Order (Presets & Card at Top, SplitContainer Fills remainder)
            this.pnlContent.Controls.Add(this.splitVisual);
            this.pnlContent.Controls.Add(this.pnlInputCard);
            this.pnlContent.Controls.Add(this.pnlPresets);

            this.pnlPresets.SendToBack();
            this.pnlInputCard.SendToBack();
            this.splitVisual.BringToFront();

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "TaylorSeriesControl";
            this.Size = new Size(920, 680);

            this.pnlInputCard.ResumeLayout(false);
            this.pnlInputCard.PerformLayout();
            this.pnlHeroButtons.ResumeLayout(false);
            this.splitVisual.Panel1.ResumeLayout(false);
            this.splitVisual.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitVisual)).EndInit();
            this.splitVisual.ResumeLayout(false);
            this.pnlCanvasHeader.ResumeLayout(false);
            this.pnlCanvasHeader.PerformLayout();
            this.tabTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerms)).EndInit();
            this.tabError.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvError)).EndInit();
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.tabResults.ResumeLayout(false);
            this.pnlPresets.ResumeLayout(false);
            this.pnlPresets.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
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
            dgv.ColumnHeadersHeight = 26;
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
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnExSin;
        private Button btnExCos;
        private Button btnExExp;
        private Button btnExLn;
        private Button btnExGeom;
        private Button btnExAtan;

        private Panel pnlInputCard;
        private Label lblFunction;
        public TextBox txtFunction;
        private Label lblVariable;
        public TextBox txtVariable;
        private Label lblCenterPoint;
        public TextBox txtCenterPoint;
        private Button btnMaclaurin;
        private Label lblOrder;
        public NumericUpDown numOrder;
        public Button btnCalculate;

        private Label lblHeroPolyTitle;
        private Label lblHeroPolyValue;
        private FlowLayoutPanel pnlHeroButtons;
        private Button btnNavToGraph;
        private Button btnCopyPoly;
        private Button btnCopyLatex;

        private SplitContainer splitVisual;
        private Panel pnlCanvasHeader;
        private Label lblCanvasTitle;
        private Label lblCanvasLegend;
        private Panel pnlMiniCanvas;

        private TabControl tabResults;
        private TabPage tabTable;
        private DataGridView dgvTerms;
        private TabPage tabError;
        private Label lblLagrangeTheory;
        private DataGridView dgvError;
        private TabPage tabReport;
        public TextBox txtResult;
    }

    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            prop?.SetValue(control, enable, null);
        }
    }
}
