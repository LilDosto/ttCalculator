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

            // Input Card
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
            this.btnCalculate = ThemeHelper.CreateActionButton("📐 Taylor Polinomunu Hesapla", 230, 36, true);

            // Hero Polynomial Card
            this.pnlHeroPoly = new Panel();
            this.lblHeroPolyTitle = new Label();
            this.lblHeroPolyValue = new Label();
            this.pnlHeroButtons = new FlowLayoutPanel();
            this.btnNavToGraph = new Button();
            this.btnCopyPoly = ThemeHelper.CreateCopyButton("📋 Kopyala");
            this.btnCopyLatex = ThemeHelper.CreateCopyButton("LaTeX");

            // Split Container for Canvas + Tabs
            this.splitVisual = new SplitContainer();
            this.pnlMiniCanvas = new Panel();
            this.lblCanvasTitle = new Label();

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
            this.pnlInputCard.SuspendLayout();
            this.pnlHeroPoly.SuspendLayout();
            this.pnlHeroButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitVisual)).BeginInit();
            this.splitVisual.Panel1.SuspendLayout();
            this.splitVisual.Panel2.SuspendLayout();
            this.splitVisual.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerms)).BeginInit();
            this.tabError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvError)).BeginInit();
            this.tabReport.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(20, 10, 20, 16);
            this.pnlContent.AutoScroll = true;

            // Presets
            this.lblPresets.Text = "⚡ Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(20, 8);
            this.pnlPresets.Size = new Size(880, 32);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExSin);
            this.pnlPresets.Controls.Add(this.btnExCos);
            this.pnlPresets.Controls.Add(this.btnExExp);
            this.pnlPresets.Controls.Add(this.btnExLn);
            this.pnlPresets.Controls.Add(this.btnExGeom);
            this.pnlPresets.Controls.Add(this.btnExAtan);

            // Input Card
            this.pnlInputCard.Location = new Point(20, 44);
            this.pnlInputCard.Size = new Size(880, 88);
            this.pnlInputCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlInputCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputCard.Padding = new Padding(14, 10, 14, 10);

            void StyleLabel(Label l, string text, Point loc)
            {
                l.Text = text;
                l.Location = loc;
                l.AutoSize = true;
                l.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                l.ForeColor = ThemeHelper.LightText;
            }

            void StyleInput(TextBox t, string def, Point loc, Size sz)
            {
                t.Location = loc;
                t.Size = sz;
                t.BackColor = Color.FromArgb(15, 23, 42);
                t.BorderStyle = BorderStyle.FixedSingle;
                t.ForeColor = Color.White;
                t.Font = new Font("Consolas", 10.5F);
                t.Text = def;
            }

            StyleLabel(this.lblFunction, "Fonksiyon f(x):", new Point(14, 14));
            StyleInput(this.txtFunction, "sin(x)", new Point(110, 11), new Size(290, 26));

            StyleLabel(this.lblVariable, "Değişken:", new Point(410, 14));
            StyleInput(this.txtVariable, "x", new Point(475, 11), new Size(42, 26));

            StyleLabel(this.lblCenterPoint, "Merkez (a):", new Point(530, 14));
            StyleInput(this.txtCenterPoint, "0", new Point(605, 11), new Size(50, 26));

            this.btnMaclaurin.Text = "a=0 (Maclaurin)";
            this.btnMaclaurin.Location = new Point(660, 11);
            this.btnMaclaurin.Size = new Size(110, 26);
            this.btnMaclaurin.BackColor = Color.FromArgb(33, 44, 70);
            this.btnMaclaurin.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnMaclaurin.FlatStyle = FlatStyle.Flat;
            this.btnMaclaurin.FlatAppearance.BorderSize = 0;
            this.btnMaclaurin.Font = new Font("Segoe UI", 8F);
            this.btnMaclaurin.Cursor = Cursors.Hand;

            StyleLabel(this.lblOrder, "Derece (n):", new Point(14, 52));
            this.numOrder.Location = new Point(110, 50);
            this.numOrder.Size = new Size(55, 26);
            this.numOrder.Minimum = 1;
            this.numOrder.Maximum = 20;
            this.numOrder.Value = 5;
            this.numOrder.BackColor = Color.FromArgb(15, 23, 42);
            this.numOrder.ForeColor = Color.White;

            this.btnCalculate.Location = new Point(180, 46);

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

            // Hero Polynomial Card
            this.pnlHeroPoly.Location = new Point(20, 138);
            this.pnlHeroPoly.Size = new Size(880, 78);
            this.pnlHeroPoly.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlHeroPoly.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlHeroPoly.Padding = new Padding(16, 8, 16, 8);

            this.lblHeroPolyTitle.Text = "HESAPLANAN TAYLOR POLİNOMU (Pn(x))";
            this.lblHeroPolyTitle.Location = new Point(14, 8);
            this.lblHeroPolyTitle.AutoSize = true;
            this.lblHeroPolyTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblHeroPolyTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblHeroPolyValue.Text = "P_n(x) = —";
            this.lblHeroPolyValue.Location = new Point(12, 28);
            this.lblHeroPolyValue.AutoSize = false;
            this.lblHeroPolyValue.Size = new Size(550, 42);
            this.lblHeroPolyValue.Font = new Font("Consolas", 11.5F, FontStyle.Bold);
            this.lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

            this.pnlHeroButtons.FlowDirection = FlowDirection.RightToLeft;
            this.pnlHeroButtons.Location = new Point(570, 14);
            this.pnlHeroButtons.Size = new Size(300, 52);
            this.pnlHeroButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnNavToGraph.Text = "📈 Grafikte Göster";
            this.btnNavToGraph.Size = new Size(130, 32);
            this.btnNavToGraph.BackColor = Color.FromArgb(16, 185, 129);
            this.btnNavToGraph.ForeColor = Color.White;
            this.btnNavToGraph.FlatStyle = FlatStyle.Flat;
            this.btnNavToGraph.FlatAppearance.BorderSize = 0;
            this.btnNavToGraph.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnNavToGraph.Cursor = Cursors.Hand;

            this.btnCopyPoly.Height = 32;
            this.btnCopyLatex.Height = 32;

            this.pnlHeroButtons.Controls.Add(this.btnNavToGraph);
            this.pnlHeroButtons.Controls.Add(this.btnCopyLatex);
            this.pnlHeroButtons.Controls.Add(this.btnCopyPoly);

            this.pnlHeroPoly.Controls.Add(this.lblHeroPolyTitle);
            this.pnlHeroPoly.Controls.Add(this.lblHeroPolyValue);
            this.pnlHeroPoly.Controls.Add(this.pnlHeroButtons);

            // Split Container for Mini Canvas and Tabbed Details
            this.splitVisual.Location = new Point(20, 222);
            this.splitVisual.Size = new Size(880, 420);
            this.splitVisual.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.splitVisual.Orientation = Orientation.Vertical;
            this.splitVisual.SplitterDistance = 300;
            this.splitVisual.SplitterWidth = 8;
            this.splitVisual.BackColor = ThemeHelper.DarkBackground;

            // Panel 1: Mini 2D Convergence Canvas
            this.splitVisual.Panel1.BackColor = Color.FromArgb(18, 26, 43);
            this.splitVisual.Panel1.Padding = new Padding(4);

            this.lblCanvasTitle.Text = "📐 2D Yakınsama Önizleme";
            this.lblCanvasTitle.Dock = DockStyle.Top;
            this.lblCanvasTitle.Height = 24;
            this.lblCanvasTitle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblCanvasTitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblCanvasTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlMiniCanvas.Dock = DockStyle.Fill;
            this.pnlMiniCanvas.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlMiniCanvas.DoubleBuffered(true);

            this.splitVisual.Panel1.Controls.Add(this.pnlMiniCanvas);
            this.splitVisual.Panel1.Controls.Add(this.lblCanvasTitle);

            // Panel 2: Tabbed Results
            this.splitVisual.Panel2.Controls.Add(this.tabResults);
            this.tabResults.Dock = DockStyle.Fill;
            this.tabResults.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // Tab 1: Katsayılar & Türevler Tablosu
            this.tabTable.Text = "Katsayılar & Türev Tablosu";
            this.tabTable.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTable.Padding = new Padding(6);
            SetupGrid(this.dgvTerms);
            this.tabTable.Controls.Add(this.dgvTerms);

            // Tab 2: Hata Analizi
            this.tabError.Text = "Hata & Lagrange Kalanı";
            this.tabError.BackColor = Color.FromArgb(24, 33, 52);
            this.tabError.Padding = new Padding(6);

            this.lblLagrangeTheory.Text = "📜 Lagrange Kalan Terimi: R_n(x) = f^(n+1)(ξ) / (n+1)! · (x - a)^(n+1)   (Burada ξ, a ile x arasındadır)";
            this.lblLagrangeTheory.Dock = DockStyle.Top;
            this.lblLagrangeTheory.Height = 28;
            this.lblLagrangeTheory.Font = new Font("Consolas", 9F, FontStyle.Bold);
            this.lblLagrangeTheory.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblLagrangeTheory.TextAlign = ContentAlignment.MiddleLeft;

            SetupGrid(this.dgvError);
            this.tabError.Controls.Add(this.dgvError);
            this.tabError.Controls.Add(this.lblLagrangeTheory);

            // Tab 3: Detaylı Analitik Rapor
            this.tabReport.Text = "Detaylı Rapor";
            this.tabReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabReport.Padding = new Padding(6);

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

            this.pnlContent.Controls.Add(this.splitVisual);
            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.pnlInputCard);
            this.pnlContent.Controls.Add(this.pnlHeroPoly);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "TaylorSeriesControl";
            this.Size = new Size(920, 720);

            this.pnlInputCard.ResumeLayout(false);
            this.pnlInputCard.PerformLayout();
            this.pnlHeroButtons.ResumeLayout(false);
            this.pnlHeroPoly.ResumeLayout(false);
            this.pnlHeroPoly.PerformLayout();
            this.splitVisual.Panel1.ResumeLayout(false);
            this.splitVisual.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitVisual)).EndInit();
            this.splitVisual.ResumeLayout(false);
            this.tabTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTerms)).EndInit();
            this.tabError.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvError)).EndInit();
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.tabResults.ResumeLayout(false);
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

        private Panel pnlHeroPoly;
        private Label lblHeroPolyTitle;
        private Label lblHeroPolyValue;
        private FlowLayoutPanel pnlHeroButtons;
        private Button btnNavToGraph;
        private Button btnCopyPoly;
        private Button btnCopyLatex;

        private SplitContainer splitVisual;
        private Panel pnlMiniCanvas;
        private Label lblCanvasTitle;

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
