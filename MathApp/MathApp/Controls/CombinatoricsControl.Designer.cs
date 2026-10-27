using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class CombinatoricsControl
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlLeft;
        private Label lblTitle;
        private Label lblDesc;

        // Group 1: Combinatorics & Permutations
        private GroupBox grpComb;
        private Label lblN;
        private NumericUpDown numN;
        private Label lblR;
        private NumericUpDown numR;
        private Button btnCalcComb;
        private Button btnPascal;

        // Group 2: String Combination Generator
        private GroupBox grpStrings;
        private Label lblAlphabet;
        private TextBox txtAlphabet;
        private Label lblPrefix;
        private TextBox txtPrefix;
        private Label lblLength;
        private NumericUpDown numLength;
        private Button btnGenStrings;

        // Group 3: Polar & Vector Coordinates
        private GroupBox grpPolar;
        private Label lblCoordX;
        private TextBox txtCoordX;
        private Label lblCoordY;
        private TextBox txtCoordY;
        private Button btnPolarConvert;

        // Group 4: Special Functions
        private GroupBox grpSpecial;
        private Label lblSpecialX;
        private TextBox txtSpecialX;
        private Label lblBase;
        private TextBox txtBase;
        private Button btnSpecialCalc;

        private RichTextBox txtOutput;
        private Panel pnlRightHeader;
        private Label lblOutputTitle;
        private Button btnCopyOutput;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlLeft = new Panel();
            this.lblTitle = new Label();
            this.lblDesc = new Label();

            this.grpComb = new GroupBox();
            this.lblN = new Label();
            this.numN = new NumericUpDown();
            this.lblR = new Label();
            this.numR = new NumericUpDown();
            this.btnCalcComb = new Button();
            this.btnPascal = new Button();

            this.grpStrings = new GroupBox();
            this.lblAlphabet = new Label();
            this.txtAlphabet = new TextBox();
            this.lblPrefix = new Label();
            this.txtPrefix = new TextBox();
            this.lblLength = new Label();
            this.numLength = new NumericUpDown();
            this.btnGenStrings = new Button();

            this.grpPolar = new GroupBox();
            this.lblCoordX = new Label();
            this.txtCoordX = new TextBox();
            this.lblCoordY = new Label();
            this.txtCoordY = new TextBox();
            this.btnPolarConvert = new Button();

            this.grpSpecial = new GroupBox();
            this.lblSpecialX = new Label();
            this.txtSpecialX = new TextBox();
            this.lblBase = new Label();
            this.txtBase = new TextBox();
            this.btnSpecialCalc = new Button();

            this.txtOutput = new RichTextBox();
            this.SuspendLayout();

            this.BackColor = Color.FromArgb(24, 25, 32);
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(12);

            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 380;
            this.pnlLeft.BackColor = Color.FromArgb(30, 32, 42);
            this.pnlLeft.Padding = new Padding(12);
            this.pnlLeft.AutoScroll = true;

            this.lblTitle.Text = "Kombinatorik & Özel Fonksiyonlar";
            this.lblTitle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(12, 10);
            this.lblTitle.AutoSize = true;

            this.lblDesc.Text = "Faktöriyel, Kombinasyon, Permütasyon, Dizi Üreteci, Polar/Atan2 ve Hiperbolik Fonksiyonlar.";
            this.lblDesc.Font = new Font("Segoe UI", 8.5F);
            this.lblDesc.ForeColor = Color.FromArgb(165, 175, 195);
            this.lblDesc.Location = new Point(12, 34);
            this.lblDesc.Size = new Size(350, 28);

            // Group 1: Combinatorics
            StyleGroupBox(grpComb, "1. Kombinasyon, Permütasyon & Faktöriyel", new Point(12, 68), new Size(350, 115));
            StyleLabel(lblN, "n:", new Point(15, 26));
            ((System.ComponentModel.ISupportInitialize)(this.numN)).BeginInit();
            this.numN.Location = new Point(36, 23);
            this.numN.Minimum = 0;
            this.numN.Maximum = 60;
            this.numN.Value = 10;
            this.numN.Size = new Size(55, 23);
            this.numN.BackColor = Color.FromArgb(40, 43, 56);
            this.numN.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numN)).EndInit();

            StyleLabel(lblR, "r (k):", new Point(105, 26));
            ((System.ComponentModel.ISupportInitialize)(this.numR)).BeginInit();
            this.numR.Location = new Point(145, 23);
            this.numR.Minimum = 0;
            this.numR.Maximum = 60;
            this.numR.Value = 3;
            this.numR.Size = new Size(55, 23);
            this.numR.BackColor = Color.FromArgb(40, 43, 56);
            this.numR.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numR)).EndInit();

            StyleActionBtn(btnCalcComb, "n!, P(n,r), C(n,r) Hesapla", new Point(15, 54), new Size(320, 26));
            StyleActionBtn(btnPascal, "Pascal Üçgeni (Binom)", new Point(15, 83), new Size(320, 24));
            this.btnPascal.BackColor = Color.FromArgb(45, 52, 70);

            grpComb.Controls.Add(lblN);
            grpComb.Controls.Add(numN);
            grpComb.Controls.Add(lblR);
            grpComb.Controls.Add(numR);
            grpComb.Controls.Add(btnCalcComb);
            grpComb.Controls.Add(btnPascal);

            // Group 2: Strings
            StyleGroupBox(grpStrings, "2. Karakter Kombinasyon Üreteci", new Point(12, 190), new Size(350, 115));
            StyleLabel(lblAlphabet, "Alfabe:", new Point(15, 24));
            this.txtAlphabet.Location = new Point(68, 21);
            this.txtAlphabet.Size = new Size(110, 23);
            this.txtAlphabet.BackColor = Color.FromArgb(40, 43, 56);
            this.txtAlphabet.ForeColor = Color.White;
            this.txtAlphabet.BorderStyle = BorderStyle.FixedSingle;
            this.txtAlphabet.Text = "ABC";

            StyleLabel(lblLength, "Boyut:", new Point(190, 24));
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.numLength.Location = new Point(238, 21);
            this.numLength.Minimum = 1;
            this.numLength.Maximum = 6;
            this.numLength.Value = 3;
            this.numLength.Size = new Size(45, 23);
            this.numLength.BackColor = Color.FromArgb(40, 43, 56);
            this.numLength.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();

            StyleLabel(lblPrefix, "Önek:", new Point(15, 52));
            this.txtPrefix.Location = new Point(68, 49);
            this.txtPrefix.Size = new Size(110, 23);
            this.txtPrefix.BackColor = Color.FromArgb(40, 43, 56);
            this.txtPrefix.ForeColor = Color.White;
            this.txtPrefix.BorderStyle = BorderStyle.FixedSingle;
            this.txtPrefix.Text = "";

            StyleActionBtn(btnGenStrings, "Dizilimleri Üret", new Point(190, 48), new Size(145, 25));
            this.btnGenStrings.BackColor = Color.FromArgb(45, 95, 80);

            grpStrings.Controls.Add(lblAlphabet);
            grpStrings.Controls.Add(txtAlphabet);
            grpStrings.Controls.Add(lblLength);
            grpStrings.Controls.Add(numLength);
            grpStrings.Controls.Add(lblPrefix);
            grpStrings.Controls.Add(txtPrefix);
            grpStrings.Controls.Add(btnGenStrings);

            // Group 3: Polar & Vector
            StyleGroupBox(grpPolar, "3. Kutupsal (Polar) & Atan2 / Hipotenüs", new Point(12, 312), new Size(350, 95));
            StyleLabel(lblCoordX, "X:", new Point(15, 24));
            this.txtCoordX.Location = new Point(36, 21);
            this.txtCoordX.Size = new Size(80, 23);
            this.txtCoordX.BackColor = Color.FromArgb(40, 43, 56);
            this.txtCoordX.ForeColor = Color.White;
            this.txtCoordX.BorderStyle = BorderStyle.FixedSingle;
            this.txtCoordX.Text = "3";

            StyleLabel(lblCoordY, "Y:", new Point(130, 24));
            this.txtCoordY.Location = new Point(152, 21);
            this.txtCoordY.Size = new Size(80, 23);
            this.txtCoordY.BackColor = Color.FromArgb(40, 43, 56);
            this.txtCoordY.ForeColor = Color.White;
            this.txtCoordY.BorderStyle = BorderStyle.FixedSingle;
            this.txtCoordY.Text = "4";

            StyleActionBtn(btnPolarConvert, "(r, θ) ve Hipotenüs Hesapla", new Point(15, 54), new Size(320, 26));

            grpPolar.Controls.Add(lblCoordX);
            grpPolar.Controls.Add(txtCoordX);
            grpPolar.Controls.Add(lblCoordY);
            grpPolar.Controls.Add(txtCoordY);
            grpPolar.Controls.Add(btnPolarConvert);

            // Group 4: Special Functions
            StyleGroupBox(grpSpecial, "4. Hiperbolik, Ters Trig & Özel Logaritma", new Point(12, 414), new Size(350, 105));
            StyleLabel(lblSpecialX, "x:", new Point(15, 24));
            this.txtSpecialX.Location = new Point(36, 21);
            this.txtSpecialX.Size = new Size(80, 23);
            this.txtSpecialX.BackColor = Color.FromArgb(40, 43, 56);
            this.txtSpecialX.ForeColor = Color.White;
            this.txtSpecialX.BorderStyle = BorderStyle.FixedSingle;
            this.txtSpecialX.Text = "0.75";

            StyleLabel(lblBase, "Taban b:", new Point(130, 24));
            this.txtBase.Location = new Point(188, 21);
            this.txtBase.Size = new Size(50, 23);
            this.txtBase.BackColor = Color.FromArgb(40, 43, 56);
            this.txtBase.ForeColor = Color.White;
            this.txtBase.BorderStyle = BorderStyle.FixedSingle;
            this.txtBase.Text = "2";

            StyleActionBtn(btnSpecialCalc, "sinh, cosh, asin, acos, atan, log Hesapla", new Point(15, 54), new Size(320, 26));
            this.btnSpecialCalc.BackColor = Color.FromArgb(50, 110, 190);

            grpSpecial.Controls.Add(lblSpecialX);
            grpSpecial.Controls.Add(txtSpecialX);
            grpSpecial.Controls.Add(lblBase);
            grpSpecial.Controls.Add(txtBase);
            grpSpecial.Controls.Add(btnSpecialCalc);

            this.pnlLeft.Controls.Add(this.lblTitle);
            this.pnlLeft.Controls.Add(this.lblDesc);
            this.pnlLeft.Controls.Add(this.grpComb);
            this.pnlLeft.Controls.Add(this.grpStrings);
            this.pnlLeft.Controls.Add(this.grpPolar);
            this.pnlLeft.Controls.Add(this.grpSpecial);

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(18, 19, 25);
            this.txtOutput.ForeColor = Color.FromArgb(225, 232, 245);
            this.txtOutput.Font = new Font("Consolas", 10F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            this.pnlRightHeader = new Panel { Dock = DockStyle.Top, Height = 34, BackColor = Color.FromArgb(24, 25, 32) };
            this.lblOutputTitle = new Label
            {
                Text = "📊 Hesaplama Sonuç Raporu:",
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = ThemeHelper.LightText,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.btnCopyOutput = new Button
            {
                Text = "📋 Kopyala",
                Dock = DockStyle.Right,
                Width = 90,
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeHelper.ButtonSecondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            this.btnCopyOutput.FlatAppearance.BorderSize = 0;

            this.pnlRightHeader.Controls.Add(this.lblOutputTitle);
            this.pnlRightHeader.Controls.Add(this.btnCopyOutput);

            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 0, 0) };
            pnlRight.Controls.Add(this.txtOutput);
            pnlRight.Controls.Add(this.pnlRightHeader);

            this.Controls.Add(pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.ResumeLayout(false);
        }

        private void StyleGroupBox(GroupBox g, string text, Point loc, Size sz)
        {
            g.Text = text;
            g.Location = loc;
            g.Size = sz;
            g.ForeColor = Color.FromArgb(160, 210, 255);
            g.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        }

        private void StyleLabel(Label l, string text, Point loc)
        {
            l.Text = text;
            l.Location = loc;
            l.AutoSize = true;
            l.ForeColor = Color.FromArgb(200, 210, 230);
            l.Font = new Font("Segoe UI", 8.5F);
        }

        private void StyleActionBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(46, 125, 225);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }
    }
}
