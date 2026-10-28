using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class CombinatoricsControl
    {
        private System.ComponentModel.IContainer components = null;

        // Main Layout
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlHeader;
        private Label lblMainTitle;
        private Label lblMainSubtitle;

        // Card 1: Combinatorics
        private Panel cardComb;
        private Label lblCard1Title;
        private Label lblN;
        private NumericUpDown numN;
        private Label lblR;
        private NumericUpDown numR;
        private Button btnCalcComb;
        private Button btnPascal;
        private Label lblFormulaPreview;

        // Card 2: String Generator
        private Panel cardStrings;
        private Label lblCard2Title;
        private Label lblAlphabet;
        private TextBox txtAlphabet;
        private Label lblPrefix;
        private TextBox txtPrefix;
        private Label lblLength;
        private NumericUpDown numLength;
        private Button btnGenStrings;
        private ProgressBar pbStrings;
        private Label lblStringsStatus;

        // Card 3: Polar & Vector
        private Panel cardPolar;
        private Label lblCard3Title;
        private Label lblCoordX;
        private TextBox txtCoordX;
        private Label lblCoordY;
        private TextBox txtCoordY;
        private Button btnPolarConvert;
        private Panel pnlVectorCanvas;

        // Card 4: Special Functions
        private Panel cardSpecial;
        private Label lblCard4Title;
        private Label lblSpecialX;
        private TextBox txtSpecialX;
        private Label lblBase;
        private TextBox txtBase;
        private Button btnSpecialCalc;

        // Right Panel: Output & Reporting
        private Panel pnlToolbar;
        private Label lblOutputTitle;
        private Button btnCopyOutput;
        private Button btnExportJson;
        private Button btnClearOutput;

        private TabControl tabOutput;
        private TabPage tabSummary;
        private TabPage tabPascal;
        private TabPage tabStrings;

        // Summary Tab Controls
        private Panel pnlKpiCard;
        private Label lblKpiTitle;
        private Label lblKpiValue;
        private Label lblKpiSubtitle;
        private RichTextBox txtSummaryDetails;

        // Pascal Tab Controls
        private RichTextBox txtPascalView;

        // Strings Tab Controls
        private Panel pnlPagination;
        private Button btnPrevPage;
        private Button btnNextPage;
        private Label lblPageInfo;
        private RichTextBox txtStringsView;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLeft = new Panel();
            this.pnlRight = new Panel();
            this.pnlHeader = new Panel();
            this.lblMainTitle = new Label();
            this.lblMainSubtitle = new Label();

            // Cards
            this.cardComb = new Panel();
            this.lblCard1Title = new Label();
            this.lblN = new Label();
            this.numN = new NumericUpDown();
            this.lblR = new Label();
            this.numR = new NumericUpDown();
            this.btnCalcComb = new Button();
            this.btnPascal = new Button();
            this.lblFormulaPreview = new Label();

            this.cardStrings = new Panel();
            this.lblCard2Title = new Label();
            this.lblAlphabet = new Label();
            this.txtAlphabet = new TextBox();
            this.lblPrefix = new Label();
            this.txtPrefix = new TextBox();
            this.lblLength = new Label();
            this.numLength = new NumericUpDown();
            this.btnGenStrings = new Button();
            this.pbStrings = new ProgressBar();
            this.lblStringsStatus = new Label();

            this.cardPolar = new Panel();
            this.lblCard3Title = new Label();
            this.lblCoordX = new Label();
            this.txtCoordX = new TextBox();
            this.lblCoordY = new Label();
            this.txtCoordY = new TextBox();
            this.btnPolarConvert = new Button();
            this.pnlVectorCanvas = new Panel();

            this.cardSpecial = new Panel();
            this.lblCard4Title = new Label();
            this.lblSpecialX = new Label();
            this.txtSpecialX = new TextBox();
            this.lblBase = new Label();
            this.txtBase = new TextBox();
            this.btnSpecialCalc = new Button();

            // Right
            this.pnlToolbar = new Panel();
            this.lblOutputTitle = new Label();
            this.btnCopyOutput = new Button();
            this.btnExportJson = new Button();
            this.btnClearOutput = new Button();

            this.tabOutput = new TabControl();
            this.tabSummary = new TabPage();
            this.tabPascal = new TabPage();
            this.tabStrings = new TabPage();

            this.pnlKpiCard = new Panel();
            this.lblKpiTitle = new Label();
            this.lblKpiValue = new Label();
            this.lblKpiSubtitle = new Label();
            this.txtSummaryDetails = new RichTextBox();

            this.txtPascalView = new RichTextBox();

            this.pnlPagination = new Panel();
            this.btnPrevPage = new Button();
            this.btnNextPage = new Button();
            this.lblPageInfo = new Label();
            this.txtStringsView = new RichTextBox();

            this.SuspendLayout();

            // Base control
            this.BackColor = Color.FromArgb(15, 23, 42); // Modern Dark Slate
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(10);

            // ==========================================
            // Left Panel (Cards)
            // ==========================================
            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 430;
            this.pnlLeft.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlLeft.Padding = new Padding(0, 0, 10, 0);
            this.pnlLeft.AutoScroll = true;

            // Header in Left Panel
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 65;
            this.pnlHeader.BackColor = Color.Transparent;

            this.lblMainTitle.Text = "Kombinatorik & Özel Fonksiyonlar";
            this.lblMainTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblMainTitle.ForeColor = Color.White;
            this.lblMainTitle.Location = new Point(4, 4);
            this.lblMainTitle.AutoSize = true;

            this.lblMainSubtitle.Text = "Ayrık matematik, dizi permütasyonları, vektör polar dönüşümleri ve ileri analitik laboratuvarı.";
            this.lblMainSubtitle.Font = new Font("Segoe UI", 8.25F);
            this.lblMainSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblMainSubtitle.Location = new Point(5, 28);
            this.lblMainSubtitle.Size = new Size(405, 30);

            this.pnlHeader.Controls.Add(this.lblMainTitle);
            this.pnlHeader.Controls.Add(this.lblMainSubtitle);

            // Helper for Styling Cards
            void SetupCard(Panel p, int height)
            {
                p.Dock = DockStyle.Top;
                p.Height = height;
                p.BackColor = Color.FromArgb(22, 32, 50);
                p.Padding = new Padding(12);
                p.Margin = new Padding(0, 0, 0, 10);
            }

            // --- Card 1: Combinatorics ---
            SetupCard(this.cardComb, 160);
            this.lblCard1Title.Text = "📐 1. Ayrık Matematik (Kombinatorik & Binom)";
            this.lblCard1Title.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblCard1Title.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblCard1Title.Location = new Point(10, 10);
            this.lblCard1Title.AutoSize = true;

            this.lblN.Text = "Eleman (n):";
            this.lblN.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblN.Location = new Point(12, 38);
            this.lblN.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numN)).BeginInit();
            this.numN.Location = new Point(90, 36);
            this.numN.Size = new Size(65, 23);
            this.numN.Minimum = 0;
            this.numN.Maximum = 60;
            this.numN.Value = 10;
            this.numN.BackColor = Color.FromArgb(30, 41, 59);
            this.numN.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numN)).EndInit();

            this.lblR.Text = "Seçim (r / k):";
            this.lblR.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblR.Location = new Point(175, 38);
            this.lblR.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numR)).BeginInit();
            this.numR.Location = new Point(255, 36);
            this.numR.Size = new Size(65, 23);
            this.numR.Minimum = 0;
            this.numR.Maximum = 60;
            this.numR.Value = 3;
            this.numR.BackColor = Color.FromArgb(30, 41, 59);
            this.numR.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numR)).EndInit();

            this.btnCalcComb.Text = "n!, P(n,r), C(n,r) Hesapla";
            this.btnCalcComb.Location = new Point(12, 70);
            this.btnCalcComb.Size = new Size(190, 32);
            this.btnCalcComb.FlatStyle = FlatStyle.Flat;
            this.btnCalcComb.BackColor = Color.FromArgb(14, 165, 233);
            this.btnCalcComb.ForeColor = Color.White;
            this.btnCalcComb.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnCalcComb.Cursor = Cursors.Hand;
            this.btnCalcComb.FlatAppearance.BorderSize = 0;

            this.btnPascal.Text = "Pascal Üçgeni Görüntüle";
            this.btnPascal.Location = new Point(208, 70);
            this.btnPascal.Size = new Size(185, 32);
            this.btnPascal.FlatStyle = FlatStyle.Flat;
            this.btnPascal.BackColor = Color.FromArgb(51, 65, 85);
            this.btnPascal.ForeColor = Color.FromArgb(226, 232, 240);
            this.btnPascal.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnPascal.Cursor = Cursors.Hand;
            this.btnPascal.FlatAppearance.BorderSize = 0;

            this.lblFormulaPreview.Text = "Formüller: C(n,r) = n! / [r!(n-r)!]  |  P(n,r) = n! / (n-r)!";
            this.lblFormulaPreview.Font = new Font("Segoe UI", 8F);
            this.lblFormulaPreview.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblFormulaPreview.Location = new Point(12, 115);
            this.lblFormulaPreview.AutoSize = true;

            this.cardComb.Controls.Add(this.lblCard1Title);
            this.cardComb.Controls.Add(this.lblN);
            this.cardComb.Controls.Add(this.numN);
            this.cardComb.Controls.Add(this.lblR);
            this.cardComb.Controls.Add(this.numR);
            this.cardComb.Controls.Add(this.btnCalcComb);
            this.cardComb.Controls.Add(this.btnPascal);
            this.cardComb.Controls.Add(this.lblFormulaPreview);

            // Spacer
            Panel sp1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // --- Card 2: Strings ---
            SetupCard(this.cardStrings, 180);
            this.lblCard2Title.Text = "🔤 2. Dizi & Permütasyon Dizilim Üreteci";
            this.lblCard2Title.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblCard2Title.ForeColor = Color.FromArgb(245, 158, 11);
            this.lblCard2Title.Location = new Point(10, 10);
            this.lblCard2Title.AutoSize = true;

            this.lblAlphabet.Text = "Alfabe:";
            this.lblAlphabet.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblAlphabet.Location = new Point(12, 38);
            this.lblAlphabet.AutoSize = true;

            this.txtAlphabet.Text = "ABC";
            this.txtAlphabet.Location = new Point(62, 35);
            this.txtAlphabet.Size = new Size(110, 23);
            this.txtAlphabet.BackColor = Color.FromArgb(30, 41, 59);
            this.txtAlphabet.ForeColor = Color.White;

            this.lblPrefix.Text = "Önek:";
            this.lblPrefix.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblPrefix.Location = new Point(185, 38);
            this.lblPrefix.AutoSize = true;

            this.txtPrefix.Text = "";
            this.txtPrefix.Location = new Point(225, 35);
            this.txtPrefix.Size = new Size(70, 23);
            this.txtPrefix.BackColor = Color.FromArgb(30, 41, 59);
            this.txtPrefix.ForeColor = Color.White;

            this.lblLength.Text = "Boyut:";
            this.lblLength.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblLength.Location = new Point(305, 38);
            this.lblLength.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.numLength.Location = new Point(350, 35);
            this.numLength.Size = new Size(45, 23);
            this.numLength.Minimum = 1;
            this.numLength.Maximum = 12;
            this.numLength.Value = 3;
            this.numLength.BackColor = Color.FromArgb(30, 41, 59);
            this.numLength.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();

            this.btnGenStrings.Text = "⚡ Dizilimleri Üret (Asenkron)";
            this.btnGenStrings.Location = new Point(12, 70);
            this.btnGenStrings.Size = new Size(382, 32);
            this.btnGenStrings.FlatStyle = FlatStyle.Flat;
            this.btnGenStrings.BackColor = Color.FromArgb(217, 119, 6);
            this.btnGenStrings.ForeColor = Color.White;
            this.btnGenStrings.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnGenStrings.Cursor = Cursors.Hand;
            this.btnGenStrings.FlatAppearance.BorderSize = 0;

            this.pbStrings.Location = new Point(12, 112);
            this.pbStrings.Size = new Size(382, 10);
            this.pbStrings.Style = ProgressBarStyle.Continuous;
            this.pbStrings.Visible = false;

            this.lblStringsStatus.Text = "Kapasite: N^k kombinasyon. N > 8 veya k > 6 durumunda güvenlik uyarısı verilir.";
            this.lblStringsStatus.Font = new Font("Segoe UI", 7.5F);
            this.lblStringsStatus.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblStringsStatus.Location = new Point(12, 130);
            this.lblStringsStatus.Size = new Size(385, 32);

            this.cardStrings.Controls.Add(this.lblCard2Title);
            this.cardStrings.Controls.Add(this.lblAlphabet);
            this.cardStrings.Controls.Add(this.txtAlphabet);
            this.cardStrings.Controls.Add(this.lblPrefix);
            this.cardStrings.Controls.Add(this.txtPrefix);
            this.cardStrings.Controls.Add(this.lblLength);
            this.cardStrings.Controls.Add(this.numLength);
            this.cardStrings.Controls.Add(this.btnGenStrings);
            this.cardStrings.Controls.Add(this.pbStrings);
            this.cardStrings.Controls.Add(this.lblStringsStatus);

            // Spacer
            Panel sp2 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // --- Card 3: Polar & Vector ---
            SetupCard(this.cardPolar, 230);
            this.lblCard3Title.Text = "🧭 3. Vektörel & Polar Koordinat Dönüşümü";
            this.lblCard3Title.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblCard3Title.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblCard3Title.Location = new Point(10, 10);
            this.lblCard3Title.AutoSize = true;

            this.lblCoordX.Text = "X (Reel):";
            this.lblCoordX.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblCoordX.Location = new Point(12, 38);
            this.lblCoordX.AutoSize = true;

            this.txtCoordX.Text = "4";
            this.txtCoordX.Location = new Point(70, 35);
            this.txtCoordX.Size = new Size(80, 23);
            this.txtCoordX.BackColor = Color.FromArgb(30, 41, 59);
            this.txtCoordX.ForeColor = Color.White;

            this.lblCoordY.Text = "Y (İmaj):";
            this.lblCoordY.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblCoordY.Location = new Point(165, 38);
            this.lblCoordY.AutoSize = true;

            this.txtCoordY.Text = "3";
            this.txtCoordY.Location = new Point(220, 35);
            this.txtCoordY.Size = new Size(80, 23);
            this.txtCoordY.BackColor = Color.FromArgb(30, 41, 59);
            this.txtCoordY.ForeColor = Color.White;

            this.btnPolarConvert.Text = "Dönüştür (r, θ)";
            this.btnPolarConvert.Location = new Point(310, 33);
            this.btnPolarConvert.Size = new Size(85, 26);
            this.btnPolarConvert.FlatStyle = FlatStyle.Flat;
            this.btnPolarConvert.BackColor = Color.FromArgb(16, 185, 129);
            this.btnPolarConvert.ForeColor = Color.White;
            this.btnPolarConvert.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnPolarConvert.Cursor = Cursors.Hand;
            this.btnPolarConvert.FlatAppearance.BorderSize = 0;

            // Mini Vector Canvas
            this.pnlVectorCanvas.Location = new Point(12, 68);
            this.pnlVectorCanvas.Size = new Size(382, 145);
            this.pnlVectorCanvas.BackColor = Color.FromArgb(15, 23, 42);

            this.cardPolar.Controls.Add(this.lblCard3Title);
            this.cardPolar.Controls.Add(this.lblCoordX);
            this.cardPolar.Controls.Add(this.txtCoordX);
            this.cardPolar.Controls.Add(this.lblCoordY);
            this.cardPolar.Controls.Add(this.txtCoordY);
            this.cardPolar.Controls.Add(this.btnPolarConvert);
            this.cardPolar.Controls.Add(this.pnlVectorCanvas);

            // Spacer
            Panel sp3 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // --- Card 4: Special Functions ---
            SetupCard(this.cardSpecial, 125);
            this.lblCard4Title.Text = "⚛️ 4. İleri Analitik & Özel Fonksiyonlar";
            this.lblCard4Title.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblCard4Title.ForeColor = Color.FromArgb(168, 85, 247);
            this.lblCard4Title.Location = new Point(10, 10);
            this.lblCard4Title.AutoSize = true;

            this.lblSpecialX.Text = "Değer (x):";
            this.lblSpecialX.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblSpecialX.Location = new Point(12, 38);
            this.lblSpecialX.AutoSize = true;

            this.txtSpecialX.Text = "1.5";
            this.txtSpecialX.Location = new Point(75, 35);
            this.txtSpecialX.Size = new Size(80, 23);
            this.txtSpecialX.BackColor = Color.FromArgb(30, 41, 59);
            this.txtSpecialX.ForeColor = Color.White;

            this.lblBase.Text = "Log Tabanı (b):";
            this.lblBase.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblBase.Location = new Point(170, 38);
            this.lblBase.AutoSize = true;

            this.txtBase.Text = "2";
            this.txtBase.Location = new Point(265, 35);
            this.txtBase.Size = new Size(50, 23);
            this.txtBase.BackColor = Color.FromArgb(30, 41, 59);
            this.txtBase.ForeColor = Color.White;

            this.btnSpecialCalc.Text = "sinh, cosh, tanh, log_b Hesapla";
            this.btnSpecialCalc.Location = new Point(12, 70);
            this.btnSpecialCalc.Size = new Size(382, 32);
            this.btnSpecialCalc.FlatStyle = FlatStyle.Flat;
            this.btnSpecialCalc.BackColor = Color.FromArgb(147, 51, 234);
            this.btnSpecialCalc.ForeColor = Color.White;
            this.btnSpecialCalc.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnSpecialCalc.Cursor = Cursors.Hand;
            this.btnSpecialCalc.FlatAppearance.BorderSize = 0;

            this.cardSpecial.Controls.Add(this.lblCard4Title);
            this.cardSpecial.Controls.Add(this.lblSpecialX);
            this.cardSpecial.Controls.Add(this.txtSpecialX);
            this.cardSpecial.Controls.Add(this.lblBase);
            this.cardSpecial.Controls.Add(this.txtBase);
            this.cardSpecial.Controls.Add(this.btnSpecialCalc);

            // Add cards in reverse order to pnlLeft for DockStyle.Top stacking
            this.pnlLeft.Controls.Add(this.cardSpecial);
            this.pnlLeft.Controls.Add(sp3);
            this.pnlLeft.Controls.Add(this.cardPolar);
            this.pnlLeft.Controls.Add(sp2);
            this.pnlLeft.Controls.Add(this.cardStrings);
            this.pnlLeft.Controls.Add(sp1);
            this.pnlLeft.Controls.Add(this.cardComb);
            this.pnlLeft.Controls.Add(this.pnlHeader);

            // ==========================================
            // Right Panel (Rich Tabbed Output Area)
            // ==========================================
            this.pnlRight.Dock = DockStyle.Fill;
            this.pnlRight.BackColor = Color.FromArgb(22, 32, 50);
            this.pnlRight.Padding = new Padding(12);

            // Toolbar
            this.pnlToolbar.Dock = DockStyle.Top;
            this.pnlToolbar.Height = 44;
            this.pnlToolbar.BackColor = Color.Transparent;

            this.lblOutputTitle.Text = "📊 Hesaplama ve Analiz Rapor Alanı";
            this.lblOutputTitle.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            this.lblOutputTitle.ForeColor = Color.White;
            this.lblOutputTitle.Location = new Point(4, 10);
            this.lblOutputTitle.AutoSize = true;

            this.btnClearOutput.Text = "🧹 Temizle";
            this.btnClearOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnClearOutput.Location = new Point(this.pnlRight.Width - 95, 6);
            this.btnClearOutput.Size = new Size(80, 30);
            this.btnClearOutput.FlatStyle = FlatStyle.Flat;
            this.btnClearOutput.BackColor = Color.FromArgb(51, 65, 85);
            this.btnClearOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.btnClearOutput.Cursor = Cursors.Hand;
            this.btnClearOutput.FlatAppearance.BorderSize = 0;

            this.btnExportJson.Text = "💾 JSON İndir";
            this.btnExportJson.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnExportJson.Location = new Point(this.pnlRight.Width - 195, 6);
            this.btnExportJson.Size = new Size(95, 30);
            this.btnExportJson.FlatStyle = FlatStyle.Flat;
            this.btnExportJson.BackColor = Color.FromArgb(51, 65, 85);
            this.btnExportJson.ForeColor = Color.FromArgb(226, 232, 240);
            this.btnExportJson.Cursor = Cursors.Hand;
            this.btnExportJson.FlatAppearance.BorderSize = 0;

            this.btnCopyOutput.Text = "📋 Kopyala";
            this.btnCopyOutput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnCopyOutput.Location = new Point(this.pnlRight.Width - 285, 6);
            this.btnCopyOutput.Size = new Size(85, 30);
            this.btnCopyOutput.FlatStyle = FlatStyle.Flat;
            this.btnCopyOutput.BackColor = Color.FromArgb(30, 41, 59);
            this.btnCopyOutput.ForeColor = Color.FromArgb(56, 189, 248);
            this.btnCopyOutput.Cursor = Cursors.Hand;
            this.btnCopyOutput.FlatAppearance.BorderSize = 1;
            this.btnCopyOutput.FlatAppearance.BorderColor = Color.FromArgb(56, 189, 248);

            this.pnlToolbar.Controls.Add(this.lblOutputTitle);
            this.pnlToolbar.Controls.Add(this.btnCopyOutput);
            this.pnlToolbar.Controls.Add(this.btnExportJson);
            this.pnlToolbar.Controls.Add(this.btnClearOutput);

            // TabControl
            this.tabOutput.Dock = DockStyle.Fill;
            this.tabOutput.ItemSize = new Size(130, 28);
            this.tabOutput.SizeMode = TabSizeMode.Fixed;

            this.tabSummary.Text = "  Özet Kartı  ";
            this.tabSummary.BackColor = Color.FromArgb(15, 23, 42);
            this.tabSummary.Padding = new Padding(10);

            this.tabPascal.Text = "  Pascal Üçgeni  ";
            this.tabPascal.BackColor = Color.FromArgb(15, 23, 42);
            this.tabPascal.Padding = new Padding(10);

            this.tabStrings.Text = "  Kombinasyon Listesi  ";
            this.tabStrings.BackColor = Color.FromArgb(15, 23, 42);
            this.tabStrings.Padding = new Padding(10);

            // Tab 1 (Summary)
            this.pnlKpiCard.Dock = DockStyle.Top;
            this.pnlKpiCard.Height = 100;
            this.pnlKpiCard.BackColor = Color.FromArgb(22, 32, 50);
            this.pnlKpiCard.Padding = new Padding(12);

            this.lblKpiTitle.Text = "SONUÇ DEĞERİ";
            this.lblKpiTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblKpiTitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblKpiTitle.Location = new Point(12, 10);
            this.lblKpiTitle.AutoSize = true;

            this.lblKpiValue.Text = "—";
            this.lblKpiValue.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblKpiValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblKpiValue.Location = new Point(10, 26);
            this.lblKpiValue.AutoSize = true;

            this.lblKpiSubtitle.Text = "Hesaplama yapmak için sol paneldeki modüllerden birini çalıştırın.";
            this.lblKpiSubtitle.Font = new Font("Segoe UI", 8.5F);
            this.lblKpiSubtitle.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblKpiSubtitle.Location = new Point(12, 70);
            this.lblKpiSubtitle.AutoSize = true;

            this.pnlKpiCard.Controls.Add(this.lblKpiTitle);
            this.pnlKpiCard.Controls.Add(this.lblKpiValue);
            this.pnlKpiCard.Controls.Add(this.lblKpiSubtitle);

            this.txtSummaryDetails.Dock = DockStyle.Fill;
            this.txtSummaryDetails.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSummaryDetails.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtSummaryDetails.Font = new Font("Consolas", 10F);
            this.txtSummaryDetails.BorderStyle = BorderStyle.None;
            this.txtSummaryDetails.ReadOnly = true;

            Panel spKpi = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };
            this.tabSummary.Controls.Add(this.txtSummaryDetails);
            this.tabSummary.Controls.Add(spKpi);
            this.tabSummary.Controls.Add(this.pnlKpiCard);

            // Tab 2 (Pascal)
            this.txtPascalView.Dock = DockStyle.Fill;
            this.txtPascalView.BackColor = Color.FromArgb(15, 23, 42);
            this.txtPascalView.ForeColor = Color.FromArgb(56, 189, 248);
            this.txtPascalView.Font = new Font("Consolas", 10.5F);
            this.txtPascalView.BorderStyle = BorderStyle.None;
            this.txtPascalView.ReadOnly = true;
            this.tabPascal.Controls.Add(this.txtPascalView);

            // Tab 3 (Strings & Pagination)
            this.pnlPagination.Dock = DockStyle.Bottom;
            this.pnlPagination.Height = 40;
            this.pnlPagination.BackColor = Color.FromArgb(22, 32, 50);

            this.btnPrevPage.Text = "◀ Önceki";
            this.btnPrevPage.Location = new Point(10, 6);
            this.btnPrevPage.Size = new Size(80, 28);
            this.btnPrevPage.FlatStyle = FlatStyle.Flat;
            this.btnPrevPage.BackColor = Color.FromArgb(30, 41, 59);
            this.btnPrevPage.ForeColor = Color.White;

            this.lblPageInfo.Text = "Sayfa: 0 / 0  (Toplam 0)";
            this.lblPageInfo.Location = new Point(100, 11);
            this.lblPageInfo.Size = new Size(200, 20);
            this.lblPageInfo.ForeColor = Color.FromArgb(203, 213, 225);

            this.btnNextPage.Text = "Sonraki ▶";
            this.btnNextPage.Location = new Point(310, 6);
            this.btnNextPage.Size = new Size(80, 28);
            this.btnNextPage.FlatStyle = FlatStyle.Flat;
            this.btnNextPage.BackColor = Color.FromArgb(30, 41, 59);
            this.btnNextPage.ForeColor = Color.White;

            this.pnlPagination.Controls.Add(this.btnPrevPage);
            this.pnlPagination.Controls.Add(this.lblPageInfo);
            this.pnlPagination.Controls.Add(this.btnNextPage);

            this.txtStringsView.Dock = DockStyle.Fill;
            this.txtStringsView.BackColor = Color.FromArgb(15, 23, 42);
            this.txtStringsView.ForeColor = Color.FromArgb(245, 158, 11);
            this.txtStringsView.Font = new Font("Consolas", 9.5F);
            this.txtStringsView.BorderStyle = BorderStyle.None;
            this.txtStringsView.ReadOnly = true;

            this.tabStrings.Controls.Add(this.txtStringsView);
            this.tabStrings.Controls.Add(this.pnlPagination);

            this.tabOutput.TabPages.Add(this.tabSummary);
            this.tabOutput.TabPages.Add(this.tabPascal);
            this.tabOutput.TabPages.Add(this.tabStrings);

            this.pnlRight.Controls.Add(this.tabOutput);
            this.pnlRight.Controls.Add(this.pnlToolbar);

            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);

            this.ResumeLayout(false);
        }
    }
}
