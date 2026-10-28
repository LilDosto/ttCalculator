using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class RootFinderControl
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
                "Denklem Kökü Bulucu (Root Finder)",
                "Newton-Raphson, Bisection, Secant ve Regula Falsi algoritmaları ile yüksek hassasiyetli sayısal kök bulma.");

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

            this.lblIterations = new Label();
            this.txtIterations = new TextBox();
            this.btnFindRoot = ThemeHelper.CreateActionButton("🔍 Kökü Hesapla", 180, 36, true);

            // Hero Summary Card
            this.pnlSummary = new Panel();
            this.lblRootSummaryTitle = new Label();
            this.lblRootValue = new Label();
            this.lblRootDetails = new Label();
            this.btnCopyResult = ThemeHelper.CreateCopyButton("📋 Kopyala");

            // Results details
            this.lblLogTitle = new Label();
            this.txtResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlInputsCard.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(24, 14, 24, 20);
            this.pnlContent.AutoScroll = true;

            // Presets row
            this.lblPresets.Text = "⚡ Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(24, 12);
            this.pnlPresets.Size = new Size(860, 34);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);

            // Inputs Card
            this.pnlInputsCard.Location = new Point(24, 52);
            this.pnlInputsCard.Size = new Size(860, 166);
            this.pnlInputsCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlInputsCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputsCard.Padding = new Padding(16, 12, 16, 12);

            void StyleLabel(Label l, string text, Point loc)
            {
                l.Text = text;
                l.Location = loc;
                l.AutoSize = true;
                l.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
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

            // Method Selector
            StyleLabel(this.lblMethod, "Nümerik Yöntem:", new Point(14, 15));
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
                "Newton-Raphson (Türev Tabanlı — En Hızlı Yakınsama)",
                "Bisection (İkiye Bölme — Garantili Aralık Yakınsaması)",
                "Secant (Kiriş Yöntemi — Türev Gerektirmeyen Yaklaşım)",
                "Regula Falsi (Doğrusal İnterpolasyon ile Kök Bulma)"
            });
            this.cmbMethod.Location = new Point(140, 12);
            this.cmbMethod.Size = new Size(440, 26);
            this.cmbMethod.Font = new Font("Segoe UI", 9.5F);
            this.cmbMethod.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;

            // Method description hint
            this.lblMethodDesc.Location = new Point(140, 42);
            this.lblMethodDesc.Size = new Size(700, 18);
            this.lblMethodDesc.Font = new Font("Segoe UI", 8.5F);
            this.lblMethodDesc.ForeColor = Color.FromArgb(125, 211, 252);
            this.lblMethodDesc.Text = "💡 Fonksiyonun analitik türevi ile teğet çizerek hızlıca köke ulaşır. Tek bir başlangıç (x₀) yeterlidir.";

            // Equation & Variable
            StyleLabel(this.lblEquation, "Denklem f(x) = 0:", new Point(14, 69));
            StyleInput(this.txtEquation, "x^3 - x - 2", new Point(140, 66), new Size(340, 26));

            StyleLabel(this.lblVariable, "Değişken:", new Point(495, 69));
            StyleInput(this.txtVariable, "x", new Point(565, 66), new Size(60, 26));

            // Numerical points
            StyleLabel(this.lblInitialPoint, "Başlangıç (x₀):", new Point(14, 102));
            StyleInput(this.txtInitialPoint, "1.5", new Point(140, 99), new Size(110, 26));

            StyleLabel(this.lblMinX, "Alt Sınır (a):", new Point(14, 102));
            StyleInput(this.txtMinX, "1", new Point(140, 99), new Size(110, 26));

            StyleLabel(this.lblMaxX, "Üst Sınır (b):", new Point(270, 102));
            StyleInput(this.txtMaxX, "2", new Point(360, 99), new Size(110, 26));

            StyleLabel(this.lblIterations, "Maks. İterasyon:", new Point(495, 102));
            StyleInput(this.txtIterations, "50", new Point(610, 99), new Size(65, 26));

            // Solve Button
            this.btnFindRoot.Location = new Point(140, 131);
            this.btnFindRoot.Size = new Size(220, 32);

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
            this.pnlInputsCard.Controls.Add(this.lblIterations);
            this.pnlInputsCard.Controls.Add(this.txtIterations);
            this.pnlInputsCard.Controls.Add(this.btnFindRoot);

            // Hero Summary Card
            this.pnlSummary.Location = new Point(24, 226);
            this.pnlSummary.Size = new Size(860, 68);
            this.pnlSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlSummary.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlSummary.Padding = new Padding(16, 8, 16, 8);

            this.lblRootSummaryTitle.Text = "HESAPLANAN KÖK DEĞERİ";
            this.lblRootSummaryTitle.Location = new Point(16, 8);
            this.lblRootSummaryTitle.AutoSize = true;
            this.lblRootSummaryTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblRootSummaryTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblRootValue.Text = "x ≈ —";
            this.lblRootValue.Location = new Point(14, 25);
            this.lblRootValue.AutoSize = true;
            this.lblRootValue.Font = new Font("Consolas", 15F, FontStyle.Bold);
            this.lblRootValue.ForeColor = Color.FromArgb(52, 211, 153); // Emerald 400

            this.lblRootDetails.Text = "Henüz hesaplama yapılmadı. \"Kökü Hesapla\" butonuna basınız.";
            this.lblRootDetails.Location = new Point(280, 29);
            this.lblRootDetails.AutoSize = true;
            this.lblRootDetails.Font = new Font("Segoe UI", 9F);
            this.lblRootDetails.ForeColor = Color.FromArgb(203, 213, 225);

            this.btnCopyResult.Location = new Point(740, 18);
            this.btnCopyResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlSummary.Controls.Add(this.lblRootSummaryTitle);
            this.pnlSummary.Controls.Add(this.lblRootValue);
            this.pnlSummary.Controls.Add(this.lblRootDetails);
            this.pnlSummary.Controls.Add(this.btnCopyResult);

            // Log Label
            this.lblLogTitle.Text = "📊 Detaylı İterasyon Adımları ve Hata Analizi:";
            this.lblLogTitle.Location = new Point(24, 302);
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblLogTitle.ForeColor = ThemeHelper.LightText;

            // Log Text Box
            this.txtResult.Location = new Point(24, 324);
            this.txtResult.Size = new Size(860, 240);
            this.txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtResult.BackColor = Color.FromArgb(15, 23, 42);
            this.txtResult.BorderStyle = BorderStyle.FixedSingle;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);

            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.pnlInputsCard);
            this.pnlContent.Controls.Add(this.pnlSummary);
            this.pnlContent.Controls.Add(this.lblLogTitle);
            this.pnlContent.Controls.Add(this.txtResult);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "RootFinderControl";
            this.Size = new Size(910, 680);

            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.pnlInputsCard.ResumeLayout(false);
            this.pnlInputsCard.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;

        private Panel pnlInputsCard;
        private Label lblMethod;
        public ComboBox cmbMethod;
        private Label lblMethodDesc;
        private Label lblEquation;
        public TextBox txtEquation;
        private Label lblVariable;
        public TextBox txtVariable;
        private Label lblInitialPoint;
        public TextBox txtInitialPoint;
        private Label lblMinX;
        public TextBox txtMinX;
        private Label lblMaxX;
        public TextBox txtMaxX;
        private Label lblIterations;
        public TextBox txtIterations;
        public Button btnFindRoot;

        private Panel pnlSummary;
        private Label lblRootSummaryTitle;
        private Label lblRootValue;
        private Label lblRootDetails;
        private Button btnCopyResult;

        private Label lblLogTitle;
        public TextBox txtResult;
    }
}
