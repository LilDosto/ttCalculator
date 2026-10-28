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
                "Bir f(x) fonksiyonunun verilen a merkez noktasındaki n. dereceden Taylor / Maclaurin polinom açılımı ve türev katsayıları.");

            this.pnlContent = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExSin = ThemeHelper.CreateExampleChip("Örnek 1: sin(x) (a=0, n=5)");
            this.btnExExp = ThemeHelper.CreateExampleChip("Örnek 2: exp(x) (a=0, n=4)");
            this.btnExLn = ThemeHelper.CreateExampleChip("Örnek 3: ln(x) (a=1, n=4)");

            this.pnlInputCard = new Panel();
            this.lblFunction = new Label();
            this.txtFunction = new TextBox();
            this.lblVariable = new Label();
            this.txtVariable = new TextBox();
            this.lblCenterPoint = new Label();
            this.txtCenterPoint = new TextBox();
            this.lblOrder = new Label();
            this.numOrder = new NumericUpDown();
            this.btnCalculate = ThemeHelper.CreateActionButton("📐 Taylor Polinomunu Hesapla", 220, 36, true);

            // Hero Polynomial Card
            this.pnlHeroPoly = new Panel();
            this.lblHeroPolyTitle = new Label();
            this.lblHeroPolyValue = new Label();
            this.btnCopyPoly = ThemeHelper.CreateCopyButton("📋 Polinomu Kopyala");

            this.lblResult = new Label();
            this.txtResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlInputCard.SuspendLayout();
            this.pnlHeroPoly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
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
            this.pnlPresets.Controls.Add(this.btnExSin);
            this.pnlPresets.Controls.Add(this.btnExExp);
            this.pnlPresets.Controls.Add(this.btnExLn);

            // Input Card
            this.pnlInputCard.Location = new Point(24, 46);
            this.pnlInputCard.Size = new Size(860, 96);
            this.pnlInputCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlInputCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputCard.Padding = new Padding(14, 10, 14, 10);

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
                t.Font = new Font("Consolas", 10.5F);
                t.Text = def;
            }

            StyleLabel(this.lblFunction, "Fonksiyon f(x):", new Point(14, 16));
            StyleInput(this.txtFunction, "sin(x)", new Point(110, 13), new Size(320, 26));

            StyleLabel(this.lblVariable, "Değişken:", new Point(445, 16));
            StyleInput(this.txtVariable, "x", new Point(510, 13), new Size(50, 26));

            StyleLabel(this.lblCenterPoint, "Merkez (a):", new Point(575, 16));
            StyleInput(this.txtCenterPoint, "0", new Point(650, 13), new Size(60, 26));

            StyleLabel(this.lblOrder, "Derece (n):", new Point(14, 56));
            this.numOrder.Location = new Point(110, 53);
            this.numOrder.Size = new Size(55, 26);
            this.numOrder.Minimum = 1;
            this.numOrder.Maximum = 12;
            this.numOrder.Value = 5;
            this.numOrder.BackColor = Color.FromArgb(15, 23, 42);
            this.numOrder.ForeColor = Color.White;

            this.btnCalculate.Location = new Point(180, 50);

            this.pnlInputCard.Controls.Add(this.lblFunction);
            this.pnlInputCard.Controls.Add(this.txtFunction);
            this.pnlInputCard.Controls.Add(this.lblVariable);
            this.pnlInputCard.Controls.Add(this.txtVariable);
            this.pnlInputCard.Controls.Add(this.lblCenterPoint);
            this.pnlInputCard.Controls.Add(this.txtCenterPoint);
            this.pnlInputCard.Controls.Add(this.lblOrder);
            this.pnlInputCard.Controls.Add(this.numOrder);
            this.pnlInputCard.Controls.Add(this.btnCalculate);

            // Hero Polynomial Card
            this.pnlHeroPoly.Location = new Point(24, 150);
            this.pnlHeroPoly.Size = new Size(860, 70);
            this.pnlHeroPoly.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlHeroPoly.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlHeroPoly.Padding = new Padding(16, 8, 16, 8);

            this.lblHeroPolyTitle.Text = "HESAPLANAN TAYLOR POLİNOMU";
            this.lblHeroPolyTitle.Location = new Point(14, 8);
            this.lblHeroPolyTitle.AutoSize = true;
            this.lblHeroPolyTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblHeroPolyTitle.ForeColor = Color.FromArgb(148, 163, 184);

            this.lblHeroPolyValue.Text = "P_n(x) = —";
            this.lblHeroPolyValue.Location = new Point(12, 26);
            this.lblHeroPolyValue.AutoSize = true;
            this.lblHeroPolyValue.Font = new Font("Consolas", 12F, FontStyle.Bold);
            this.lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

            this.btnCopyPoly.Location = new Point(700, 18);
            this.btnCopyPoly.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlHeroPoly.Controls.Add(this.lblHeroPolyTitle);
            this.pnlHeroPoly.Controls.Add(this.lblHeroPolyValue);
            this.pnlHeroPoly.Controls.Add(this.btnCopyPoly);

            // Result
            this.lblResult.Text = "📊 Noktadaki Sayısal Türevler ve Terim Katsayıları:";
            this.lblResult.Location = new Point(24, 230);
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblResult.ForeColor = ThemeHelper.LightText;

            this.txtResult.Location = new Point(24, 254);
            this.txtResult.Size = new Size(860, 320);
            this.txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtResult.BackColor = Color.FromArgb(15, 23, 42);
            this.txtResult.BorderStyle = BorderStyle.FixedSingle;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);

            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.pnlInputCard);
            this.pnlContent.Controls.Add(this.pnlHeroPoly);
            this.pnlContent.Controls.Add(this.lblResult);
            this.pnlContent.Controls.Add(this.txtResult);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "TaylorSeriesControl";
            this.Size = new Size(910, 680);

            this.pnlInputCard.ResumeLayout(false);
            this.pnlInputCard.PerformLayout();
            this.pnlHeroPoly.ResumeLayout(false);
            this.pnlHeroPoly.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnExSin;
        private Button btnExExp;
        private Button btnExLn;

        private Panel pnlInputCard;
        private Label lblFunction;
        public TextBox txtFunction;
        private Label lblVariable;
        public TextBox txtVariable;
        private Label lblCenterPoint;
        public TextBox txtCenterPoint;
        private Label lblOrder;
        public NumericUpDown numOrder;
        public Button btnCalculate;

        private Panel pnlHeroPoly;
        private Label lblHeroPolyTitle;
        private Label lblHeroPolyValue;
        private Button btnCopyPoly;

        private Label lblResult;
        public TextBox txtResult;
    }
}
