using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class SystemSolverControl
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
                "Denklem Sistemi Çözücü (System Solver)",
                "Newton-Raphson (Genel/Doğrusal Olmayan), Gauss-Seidel, LU Ayrışımı, Matris Tersi ve QR yöntemleriyle sistem çözümü.");

            this.pnlContent = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExLinear = ThemeHelper.CreateExampleChip("Örnek 1: LU ile Doğrusal (2x+y=5, x-y=1)");
            this.btnExGaussSeidel = ThemeHelper.CreateExampleChip("Örnek 2: Gauss-Seidel (3x3)");
            this.btnExNonlinear = ThemeHelper.CreateExampleChip("Örnek 3: Newton-Raphson (x²+y²=4, x-y=0)");

            this.pnlSettingsCard = new Panel();
            this.lblMethod = new Label();
            this.cmbSolverMethod = new ComboBox();
            this.lblMethodHint = new Label();
            this.lblEqCount = new Label();
            this.numEqCount = new NumericUpDown();
            this.btnGenerate = ThemeHelper.CreateActionButton("Boyut Değiştir", 110, 28, false);
            this.lblSystemIterations = new Label();
            this.txtSystemIterations = new TextBox();
            this.btnSolveSystem = ThemeHelper.CreateActionButton("⚡ Sistemi Çöz", 160, 32, true);

            this.pnlEquations = new FlowLayoutPanel();

            // Hero Solution Card
            this.pnlSolutionHero = new Panel();
            this.lblHeroTitle = new Label();
            this.lblHeroSolution = new Label();
            this.lblHeroStatus = new Label();
            this.btnCopyHero = ThemeHelper.CreateCopyButton("📋 Çözümü Kopyala");

            this.lblSystemResult = new Label();
            this.txtSystemResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlSettingsCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).BeginInit();
            this.pnlSolutionHero.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(24, 12, 24, 20);
            this.pnlContent.AutoScroll = true;

            // Presets
            this.lblPresets.Text = "⚡ Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(24, 10);
            this.pnlPresets.Size = new Size(860, 32);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExLinear);
            this.pnlPresets.Controls.Add(this.btnExGaussSeidel);
            this.pnlPresets.Controls.Add(this.btnExNonlinear);

            // Settings Card
            this.pnlSettingsCard.Location = new Point(24, 46);
            this.pnlSettingsCard.Size = new Size(860, 80);
            this.pnlSettingsCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlSettingsCard.BackColor = ThemeHelper.CardBackground;
            this.pnlSettingsCard.Padding = new Padding(14, 10, 14, 10);

            this.lblMethod.Text = "Çözücü:";
            this.lblMethod.Location = new Point(14, 12);
            this.lblMethod.AutoSize = true;
            this.lblMethod.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblMethod.ForeColor = ThemeHelper.LightText;

            this.cmbSolverMethod.FormattingEnabled = true;
            this.cmbSolverMethod.Items.AddRange(new object[] {
                "Çok Değişkenli Newton-Raphson (Genel / Nonlineer)",
                "Gauss-Seidel İterasyonu (Köşegen Baskın Doğrusal)",
                "LU Ayrışımı (L·U·X = P·b Doğrusal — Hızlı)",
                "Ters Matris Yöntemi (X = A⁻¹·b Doğrusal)",
                "QR En Küçük Kareler Çözümü (Q·R·X = b)"
            });
            this.cmbSolverMethod.Location = new Point(72, 8);
            this.cmbSolverMethod.Size = new Size(330, 25);
            this.cmbSolverMethod.Font = new Font("Segoe UI", 9F);
            this.cmbSolverMethod.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbSolverMethod.ForeColor = Color.White;
            this.cmbSolverMethod.DropDownStyle = ComboBoxStyle.DropDownList;

            this.lblEqCount.Text = "Denklem Sayısı:";
            this.lblEqCount.Location = new Point(415, 12);
            this.lblEqCount.AutoSize = true;
            this.lblEqCount.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblEqCount.ForeColor = ThemeHelper.LightText;

            this.numEqCount.Location = new Point(510, 9);
            this.numEqCount.Size = new Size(45, 25);
            this.numEqCount.Minimum = 2;
            this.numEqCount.Maximum = 6;
            this.numEqCount.Value = 2;
            this.numEqCount.BackColor = Color.FromArgb(15, 23, 42);
            this.numEqCount.ForeColor = Color.White;

            this.btnGenerate.Location = new Point(562, 8);

            this.btnSolveSystem.Location = new Point(685, 6);
            this.btnSolveSystem.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.lblMethodHint.Location = new Point(14, 46);
            this.lblMethodHint.Size = new Size(830, 24);
            this.lblMethodHint.Font = new Font("Segoe UI", 8.5F);
            this.lblMethodHint.ForeColor = Color.FromArgb(125, 211, 252);
            this.lblMethodHint.Text = "💡 Yöntem ipucu burada görüntülenecektir.";

            this.lblSystemIterations.Text = "İterasyon:";
            this.lblSystemIterations.Location = new Point(680, 48);
            this.lblSystemIterations.AutoSize = true;
            this.lblSystemIterations.Font = new Font("Segoe UI", 8.5F);
            this.lblSystemIterations.ForeColor = ThemeHelper.MutedText;

            this.txtSystemIterations.Location = new Point(745, 45);
            this.txtSystemIterations.Size = new Size(45, 23);
            this.txtSystemIterations.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSystemIterations.BorderStyle = BorderStyle.FixedSingle;
            this.txtSystemIterations.ForeColor = Color.White;
            this.txtSystemIterations.Font = new Font("Consolas", 9F);
            this.txtSystemIterations.Text = "50";

            this.pnlSettingsCard.Controls.Add(this.lblMethod);
            this.pnlSettingsCard.Controls.Add(this.cmbSolverMethod);
            this.pnlSettingsCard.Controls.Add(this.lblEqCount);
            this.pnlSettingsCard.Controls.Add(this.numEqCount);
            this.pnlSettingsCard.Controls.Add(this.btnGenerate);
            this.pnlSettingsCard.Controls.Add(this.btnSolveSystem);
            this.pnlSettingsCard.Controls.Add(this.lblMethodHint);
            this.pnlSettingsCard.Controls.Add(this.lblSystemIterations);
            this.pnlSettingsCard.Controls.Add(this.txtSystemIterations);

            // Equations panel
            this.pnlEquations.Location = new Point(24, 132);
            this.pnlEquations.Size = new Size(860, 115);
            this.pnlEquations.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlEquations.AutoScroll = true;

            // Hero Solution Card
            this.pnlSolutionHero.Location = new Point(24, 252);
            this.pnlSolutionHero.Size = new Size(860, 64);
            this.pnlSolutionHero.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlSolutionHero.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlSolutionHero.Padding = new Padding(16, 8, 16, 8);

            this.lblHeroTitle.Text = "SİSTEM ÇÖZÜM VEKTÖRÜ (X)";
            this.lblHeroTitle.Location = new Point(14, 8);
            this.lblHeroTitle.AutoSize = true;
            this.lblHeroTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblHeroTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblHeroSolution.Text = "X = [ — ]";
            this.lblHeroSolution.Location = new Point(12, 26);
            this.lblHeroSolution.AutoSize = true;
            this.lblHeroSolution.Font = new Font("Consolas", 14F, FontStyle.Bold);
            this.lblHeroSolution.ForeColor = Color.FromArgb(52, 211, 153);

            this.lblHeroStatus.Text = "Henüz çözülmedi.";
            this.lblHeroStatus.Location = new Point(320, 30);
            this.lblHeroStatus.AutoSize = true;
            this.lblHeroStatus.Font = new Font("Segoe UI", 9F);
            this.lblHeroStatus.ForeColor = Color.FromArgb(203, 213, 225);

            this.btnCopyHero.Location = new Point(720, 18);
            this.btnCopyHero.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlSolutionHero.Controls.Add(this.lblHeroTitle);
            this.pnlSolutionHero.Controls.Add(this.lblHeroSolution);
            this.pnlSolutionHero.Controls.Add(this.lblHeroStatus);
            this.pnlSolutionHero.Controls.Add(this.btnCopyHero);

            // Detailed Result Log
            this.lblSystemResult.Text = "📊 Detaylı Çözüm Adımları, Matris Formu ve Hata Analizi:";
            this.lblSystemResult.Location = new Point(24, 322);
            this.lblSystemResult.AutoSize = true;
            this.lblSystemResult.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblSystemResult.ForeColor = ThemeHelper.LightText;

            this.txtSystemResult.Location = new Point(24, 344);
            this.txtSystemResult.Size = new Size(860, 230);
            this.txtSystemResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtSystemResult.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSystemResult.BorderStyle = BorderStyle.FixedSingle;
            this.txtSystemResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtSystemResult.Multiline = true;
            this.txtSystemResult.ReadOnly = true;
            this.txtSystemResult.ScrollBars = ScrollBars.Vertical;
            this.txtSystemResult.Font = new Font("Consolas", 9.5F);

            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.pnlSettingsCard);
            this.pnlContent.Controls.Add(this.pnlEquations);
            this.pnlContent.Controls.Add(this.pnlSolutionHero);
            this.pnlContent.Controls.Add(this.lblSystemResult);
            this.pnlContent.Controls.Add(this.txtSystemResult);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "SystemSolverControl";
            this.Size = new Size(910, 680);

            this.pnlSettingsCard.ResumeLayout(false);
            this.pnlSettingsCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).EndInit();
            this.pnlSolutionHero.ResumeLayout(false);
            this.pnlSolutionHero.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnExLinear;
        private Button btnExGaussSeidel;
        private Button btnExNonlinear;

        private Panel pnlSettingsCard;
        private Label lblMethod;
        public ComboBox cmbSolverMethod;
        private Label lblMethodHint;
        private Label lblEqCount;
        public NumericUpDown numEqCount;
        public Button btnGenerate;
        private Label lblSystemIterations;
        public TextBox txtSystemIterations;
        public Button btnSolveSystem;

        public FlowLayoutPanel pnlEquations;

        private Panel pnlSolutionHero;
        private Label lblHeroTitle;
        private Label lblHeroSolution;
        private Label lblHeroStatus;
        private Button btnCopyHero;

        private Label lblSystemResult;
        public TextBox txtSystemResult;
    }
}
