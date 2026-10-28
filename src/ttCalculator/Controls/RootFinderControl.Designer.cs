using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class RootFinderControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;

        private Panel pnlInputsCard;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Label lblMethodDesc;

        private Label lblEquation;
        private TextBox txtEquation;
        private Label lblVariable;
        private TextBox txtVariable;

        private Label lblInitialPoint;
        private TextBox txtInitialPoint;
        private Label lblMinX;
        private TextBox txtMinX;
        private Label lblMaxX;
        private TextBox txtMaxX;

        private Label lblTolerance;
        private TextBox txtTolerance;
        private Label lblIterations;
        private TextBox txtIterations;
        private Button btnFindRoot;

        // Vurgulu Sonuç Kartı (KPI Card)
        private Panel pnlSummary;
        private Label lblRootSummaryTitle;
        private Label lblRootValue;
        private Label lblRootDetails;
        private FlowLayoutPanel pnlBadges;
        private Label lblBadgeIter;
        private Label lblBadgeError;
        private Label lblBadgeStatus;
        private Button btnPlotOnGraph;
        private Button btnCopyResult;

        // Split / Tabbed View for Results
        private TabControl tabResults;
        private TabPage tabGrid;
        private TabPage tabPlot;
        private TabPage tabText;

        private DataGridView dgvIterations;
        private Panel pnlConvergencePlot;
        private TextBox txtResult;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Denklem Kökü Bulucu (Root Finder)",
                "Newton-Raphson, Bisection, Secant, Regula Falsi ve Çoklu Kök Taraması ile yüksek hassasiyetli sayısal analiz.");

            this.pnlContent = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("Örnek 1: x³ - x - 2 = 0");
            this.btnEx2 = ThemeHelper.CreateExampleChip("Örnek 2: cos(x) - x = 0");
            this.btnEx3 = ThemeHelper.CreateExampleChip("Örnek 3: e^x - 3x = 0");

            this.pnlInputsCard = new Panel();
            this.lblMethod = new Label();
            this.cmbMethod = new ComboBox();
            this.lblMethodDesc = new Label();

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblVariable = new Label();
            this.txtVariable = new TextBox();

            this.lblInitialPoint = new Label();
            this.txtInitialPoint = new TextBox();
            this.lblMinX = new Label();
            this.txtMinX = new TextBox();
            this.lblMaxX = new Label();
            this.txtMaxX = new TextBox();

            this.lblTolerance = new Label();
            this.txtTolerance = new TextBox();
            this.lblIterations = new Label();
            this.txtIterations = new TextBox();
            this.btnFindRoot = ThemeHelper.CreateActionButton("🔍 Kökü Hesapla", 180, 34, true);

            // KPI Result Card
            this.pnlSummary = new Panel();
            this.lblRootSummaryTitle = new Label();
            this.lblRootValue = new Label();
            this.lblRootDetails = new Label();
            this.pnlBadges = new FlowLayoutPanel();
            this.lblBadgeIter = new Label();
            this.lblBadgeError = new Label();
            this.lblBadgeStatus = new Label();
            this.btnPlotOnGraph = new Button();
            this.btnCopyResult = ThemeHelper.CreateCopyButton("📋 Kopyala");

            // Tabbed Area
            this.tabResults = new TabControl();
            this.tabGrid = new TabPage();
            this.tabPlot = new TabPage();
            this.tabText = new TabPage();

            this.dgvIterations = new DataGridView();
            this.pnlConvergencePlot = new Panel();
            this.txtResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlInputsCard.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.pnlBadges.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabGrid.SuspendLayout();
            this.tabPlot.SuspendLayout();
            this.tabText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIterations)).BeginInit();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(20, 10, 20, 16);
            this.pnlContent.AutoScroll = true;

            // Presets row
            this.lblPresets.Text = "⚡ Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(20, 8);
            this.pnlPresets.Size = new Size(870, 32);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);

            // Inputs Card
            this.pnlInputsCard.Location = new Point(20, 44);
            this.pnlInputsCard.Size = new Size(870, 175);
            this.pnlInputsCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlInputsCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputsCard.Padding = new Padding(14, 10, 14, 10);

            void StyleLabel(Label l, string text, Point loc)
            {
                l.Text = text;
                l.Location = loc;
                l.AutoSize = true;
                l.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
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

            // Method Selector
            StyleLabel(this.lblMethod, "Nümerik Yöntem:", new Point(14, 14));
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
                "Newton-Raphson (Türev Tabanlı — Kuadratik Yakınsama)",
                "Bisection (İkiye Bölme — Garantili Aralık Kısıtlama)",
                "Secant (Kiriş Yöntemi — Türevsiz İki Noktalı Yaklaşım)",
                "Regula Falsi (Doğrusal İnterpolasyon ile Kök Bulma)",
                "🎯 Tüm Kökleri Bul (Otomatik Çoklu Kök Taraması [a, b])"
            });
            this.cmbMethod.Location = new Point(140, 11);
            this.cmbMethod.Size = new Size(420, 25);
            this.cmbMethod.Font = new Font("Segoe UI", 9F);
            this.cmbMethod.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblMethodDesc.Location = new Point(140, 39);
            this.lblMethodDesc.Size = new Size(710, 18);
            this.lblMethodDesc.Font = new Font("Segoe UI", 8.25F);
            this.lblMethodDesc.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblMethodDesc.Text = "💡 Fonksiyonun analitik türevi ile teğet çizerek hızlıca köke ulaşır. Tek bir başlangıç (x₀) yeterlidir.";

            // Equation & Variable
            StyleLabel(this.lblEquation, "Denklem f(x) = 0:", new Point(14, 66));
            StyleInput(this.txtEquation, "x^3 - x - 2", new Point(140, 63), new Size(320, 25));

            StyleLabel(this.lblVariable, "Değişken:", new Point(475, 66));
            StyleInput(this.txtVariable, "x", new Point(545, 63), new Size(50, 25));

            // Numerical points
            StyleLabel(this.lblInitialPoint, "Başlangıç (x₀):", new Point(14, 98));
            StyleInput(this.txtInitialPoint, "1.5", new Point(140, 95), new Size(100, 25));

            StyleLabel(this.lblMinX, "Alt Sınır (a):", new Point(14, 98));
            StyleInput(this.txtMinX, "1", new Point(140, 95), new Size(100, 25));

            StyleLabel(this.lblMaxX, "Üst Sınır (b):", new Point(255, 98));
            StyleInput(this.txtMaxX, "2", new Point(340, 95), new Size(100, 25));

            StyleLabel(this.lblTolerance, "Tolerans (ε):", new Point(455, 98));
            StyleInput(this.txtTolerance, "1e-7", new Point(545, 95), new Size(90, 25));

            StyleLabel(this.lblIterations, "Maks. Adım:", new Point(650, 98));
            StyleInput(this.txtIterations, "50", new Point(735, 95), new Size(55, 25));

            // Solve Button
            this.btnFindRoot.Location = new Point(140, 128);
            this.btnFindRoot.Size = new Size(200, 34);

            this.pnlInputsCard.Controls.Add(this.lblMethod);
            this.pnlInputsCard.Controls.Add(this.cmbMethod);
            this.pnlInputsCard.Controls.Add(this.lblMethodDesc);
            this.pnlInputsCard.Controls.Add(this.lblEquation);
            this.pnlInputsCard.Controls.Add(this.txtEquation);
            this.pnlInputsCard.Controls.Add(this.lblVariable);
            this.pnlInputsCard.Controls.Add(this.txtVariable);
            this.pnlInputsCard.Controls.Add(this.lblInitialPoint);
            this.pnlInputsCard.Controls.Add(this.txtInitialPoint);
            this.pnlInputsCard.Controls.Add(this.lblMinX);
            this.pnlInputsCard.Controls.Add(this.txtMinX);
            this.pnlInputsCard.Controls.Add(this.lblMaxX);
            this.pnlInputsCard.Controls.Add(this.txtMaxX);
            this.pnlInputsCard.Controls.Add(this.lblTolerance);
            this.pnlInputsCard.Controls.Add(this.txtTolerance);
            this.pnlInputsCard.Controls.Add(this.lblIterations);
            this.pnlInputsCard.Controls.Add(this.txtIterations);
            this.pnlInputsCard.Controls.Add(this.btnFindRoot);

            // ==========================================
            // Vurgulu Sonuç Kartı (KPI Card)
            // ==========================================
            this.pnlSummary.Location = new Point(20, 226);
            this.pnlSummary.Size = new Size(870, 96);
            this.pnlSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlSummary.BackColor = Color.FromArgb(22, 32, 50);
            this.pnlSummary.Padding = new Padding(14, 8, 14, 8);

            this.lblRootSummaryTitle.Text = "HESAPLANAN KÖK ($x^*$)";
            this.lblRootSummaryTitle.Location = new Point(14, 8);
            this.lblRootSummaryTitle.AutoSize = true;
            this.lblRootSummaryTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblRootSummaryTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblRootValue.Text = "x* ≈ —";
            this.lblRootValue.Location = new Point(12, 24);
            this.lblRootValue.AutoSize = true;
            this.lblRootValue.Font = new Font("Consolas", 18F, FontStyle.Bold);
            this.lblRootValue.ForeColor = Color.FromArgb(52, 211, 153); // Emerald Green

            this.lblRootDetails.Text = "Henüz hesaplama yapılmadı. Parametreleri belirleyip 'Kökü Hesapla' butonuna basınız.";
            this.lblRootDetails.Location = new Point(14, 66);
            this.lblRootDetails.AutoSize = true;
            this.lblRootDetails.Font = new Font("Segoe UI", 8.25F);
            this.lblRootDetails.ForeColor = Color.FromArgb(203, 213, 225);

            // Badges FlowPanel
            this.pnlBadges.Location = new Point(380, 16);
            this.pnlBadges.Size = new Size(260, 44);
            this.pnlBadges.BackColor = Color.Transparent;

            void StyleBadge(Label b, string text, Color fg, Color bg)
            {
                b.Text = text;
                b.AutoSize = true;
                b.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
                b.ForeColor = fg;
                b.BackColor = bg;
                b.Padding = new Padding(5, 3, 5, 3);
                b.Margin = new Padding(3, 4, 3, 4);
            }

            StyleBadge(this.lblBadgeIter, "İterasyon: —", Color.FromArgb(56, 189, 248), Color.FromArgb(12, 74, 110));
            StyleBadge(this.lblBadgeError, "Hata: —", Color.FromArgb(245, 158, 11), Color.FromArgb(120, 53, 15));
            StyleBadge(this.lblBadgeStatus, "Durum: Bekliyor", Color.FromArgb(148, 163, 184), Color.FromArgb(30, 41, 59));

            this.pnlBadges.Controls.Add(this.lblBadgeIter);
            this.pnlBadges.Controls.Add(this.lblBadgeError);
            this.pnlBadges.Controls.Add(this.lblBadgeStatus);

            // Actions in Card
            this.btnPlotOnGraph.Text = "📈 Grafikte Göster";
            this.btnPlotOnGraph.Location = new Point(650, 14);
            this.btnPlotOnGraph.Size = new Size(120, 32);
            this.btnPlotOnGraph.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnPlotOnGraph.FlatStyle = FlatStyle.Flat;
            this.btnPlotOnGraph.BackColor = Color.FromArgb(14, 165, 233);
            this.btnPlotOnGraph.ForeColor = Color.White;
            this.btnPlotOnGraph.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnPlotOnGraph.Cursor = Cursors.Hand;
            this.btnPlotOnGraph.FlatAppearance.BorderSize = 0;

            this.btnCopyResult.Location = new Point(780, 14);
            this.btnCopyResult.Size = new Size(80, 32);
            this.btnCopyResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlSummary.Controls.Add(this.lblRootSummaryTitle);
            this.pnlSummary.Controls.Add(this.lblRootValue);
            this.pnlSummary.Controls.Add(this.lblRootDetails);
            this.pnlSummary.Controls.Add(this.pnlBadges);
            this.pnlSummary.Controls.Add(this.btnPlotOnGraph);
            this.pnlSummary.Controls.Add(this.btnCopyResult);

            // ==========================================
            // Tabbed Results Area
            // ==========================================
            this.tabResults.Location = new Point(20, 330);
            this.tabResults.Size = new Size(870, 310);
            this.tabResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.tabResults.ItemSize = new Size(160, 26);
            this.tabResults.SizeMode = TabSizeMode.Fixed;

            this.tabGrid.Text = "  İterasyon Tablosu  ";
            this.tabGrid.BackColor = Color.FromArgb(15, 23, 42);
            this.tabGrid.Padding = new Padding(6);

            this.tabPlot.Text = "  Yakınsama Hızı Grafiği  ";
            this.tabPlot.BackColor = Color.FromArgb(15, 23, 42);
            this.tabPlot.Padding = new Padding(6);

            this.tabText.Text = "  Analitik Rapor  ";
            this.tabText.BackColor = Color.FromArgb(15, 23, 42);
            this.tabText.Padding = new Padding(6);

            // Tab 1: DataGridView
            this.dgvIterations.Dock = DockStyle.Fill;
            this.dgvIterations.BackgroundColor = Color.FromArgb(15, 23, 42);
            this.dgvIterations.BorderStyle = BorderStyle.None;
            this.dgvIterations.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvIterations.GridColor = Color.FromArgb(30, 41, 59);
            this.dgvIterations.RowHeadersVisible = false;
            this.dgvIterations.AllowUserToAddRows = false;
            this.dgvIterations.AllowUserToDeleteRows = false;
            this.dgvIterations.ReadOnly = true;
            this.dgvIterations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvIterations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIterations.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            this.dgvIterations.DefaultCellStyle.ForeColor = Color.White;
            this.dgvIterations.DefaultCellStyle.Font = new Font("Consolas", 9F);
            this.dgvIterations.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(22, 32, 50);
            this.dgvIterations.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            this.dgvIterations.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            this.dgvIterations.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.dgvIterations.EnableHeadersVisualStyles = false;

            this.tabGrid.Controls.Add(this.dgvIterations);

            // Tab 2: Canvas
            this.pnlConvergencePlot.Dock = DockStyle.Fill;
            this.pnlConvergencePlot.BackColor = Color.FromArgb(15, 23, 42);
            this.tabPlot.Controls.Add(this.pnlConvergencePlot);

            // Tab 3: Text Report
            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(15, 23, 42);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);
            this.tabText.Controls.Add(this.txtResult);

            this.tabResults.TabPages.Add(this.tabGrid);
            this.tabResults.TabPages.Add(this.tabPlot);
            this.tabResults.TabPages.Add(this.tabText);

            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.pnlInputsCard);
            this.pnlContent.Controls.Add(this.pnlSummary);
            this.pnlContent.Controls.Add(this.tabResults);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "RootFinderControl";
            this.Size = new Size(910, 680);

            this.pnlBadges.ResumeLayout(false);
            this.pnlBadges.PerformLayout();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.pnlInputsCard.ResumeLayout(false);
            this.pnlInputsCard.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.tabResults.ResumeLayout(false);
            this.tabGrid.ResumeLayout(false);
            this.tabPlot.ResumeLayout(false);
            this.tabText.ResumeLayout(false);
            this.tabText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIterations)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
