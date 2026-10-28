using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class MatrixCalculatorControl
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlLeft;
        private Label lblTitle;
        private Label lblDesc;

        // Matrix A section
        private Label lblHeaderA;
        private NumericUpDown numRowsA;
        private NumericUpDown numColsA;
        private Button btnResizeA;
        private Button btnRandomA;
        private Button btnIdentityA;
        private Button btnZeroA;
        private Panel pnlGridA;

        // Matrix B section
        private Label lblHeaderB;
        private NumericUpDown numRowsB;
        private NumericUpDown numColsB;
        private Button btnResizeB;
        private Button btnRandomB;
        private Button btnIdentityB;
        private Button btnZeroB;
        private Button btnCopyAToB;
        private Panel pnlGridB;

        // Scalar & Presets
        private Label lblScalar;
        private TextBox txtScalar;
        private Button btnExSPD;
        private Button btnExGeneral;

        // Single Matrix A Operations
        private Label lblOpsA;
        private Button btnAllProps;
        private Button btnLU;
        private Button btnQR;
        private Button btnCholesky;
        private Button btnSVD;
        private Button btnEigen;
        private Button btnInverse;

        // Dual Matrix A & B Operations
        private Label lblOpsAB;
        private Button btnAddAB;
        private Button btnSubAB;
        private Button btnMulAB;
        private Button btnElemMul;
        private Button btnElemDiv;
        private Button btnScalarMul;
        private Button btnSolveAXB;
        private Button btnSolveTrans;

        // Right side results & actions
        private Panel pnlRight;
        private Panel pnlResultToolbar;
        private Button btnAnsToA;
        private Button btnCopyMatrix;
        private Label lblResultTitle;
        private RichTextBox txtResult;

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

            this.lblHeaderA = new Label();
            this.numRowsA = new NumericUpDown();
            this.numColsA = new NumericUpDown();
            this.btnResizeA = new Button();
            this.btnRandomA = new Button();
            this.btnIdentityA = new Button();
            this.btnZeroA = new Button();
            this.pnlGridA = new Panel();

            this.lblHeaderB = new Label();
            this.numRowsB = new NumericUpDown();
            this.numColsB = new NumericUpDown();
            this.btnResizeB = new Button();
            this.btnRandomB = new Button();
            this.btnIdentityB = new Button();
            this.btnZeroB = new Button();
            this.btnCopyAToB = new Button();
            this.pnlGridB = new Panel();

            this.lblScalar = new Label();
            this.txtScalar = new TextBox();
            this.btnExSPD = new Button();
            this.btnExGeneral = new Button();

            this.lblOpsA = new Label();
            this.btnAllProps = new Button();
            this.btnLU = new Button();
            this.btnQR = new Button();
            this.btnCholesky = new Button();
            this.btnSVD = new Button();
            this.btnEigen = new Button();
            this.btnInverse = new Button();

            this.lblOpsAB = new Label();
            this.btnAddAB = new Button();
            this.btnSubAB = new Button();
            this.btnMulAB = new Button();
            this.btnElemMul = new Button();
            this.btnElemDiv = new Button();
            this.btnScalarMul = new Button();
            this.btnSolveAXB = new Button();
            this.btnSolveTrans = new Button();

            this.pnlRight = new Panel();
            this.pnlResultToolbar = new Panel();
            this.lblResultTitle = new Label();
            this.btnAnsToA = new Button();
            this.btnCopyMatrix = new Button();
            this.txtResult = new RichTextBox();

            this.SuspendLayout();

            this.BackColor = Color.FromArgb(24, 25, 32);
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(12);

            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 445;
            this.pnlLeft.BackColor = Color.FromArgb(30, 32, 42);
            this.pnlLeft.Padding = new Padding(12);
            this.pnlLeft.AutoScroll = true;

            this.lblTitle.Text = "Matris Cebiri & Ayrıştırma Stüdyosu";
            this.lblTitle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(12, 10);
            this.lblTitle.AutoSize = true;

            this.lblDesc.Text = "A ve B matrisleri üzerinde cebir, AX=B sistemleri ve LU/QR/SVD ayrıştırmaları.";
            this.lblDesc.Font = new Font("Segoe UI", 8.5F);
            this.lblDesc.ForeColor = Color.FromArgb(165, 175, 195);
            this.lblDesc.Location = new Point(12, 34);
            this.lblDesc.Size = new Size(415, 18);

            // --- MATRIX A ---
            this.lblHeaderA.Text = "A Matrisi (m × n):";
            this.lblHeaderA.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblHeaderA.ForeColor = Color.FromArgb(110, 190, 255);
            this.lblHeaderA.Location = new Point(12, 56);
            this.lblHeaderA.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numRowsA)).BeginInit();
            this.numRowsA.Location = new Point(130, 54);
            this.numRowsA.Minimum = 1;
            this.numRowsA.Maximum = 6;
            this.numRowsA.Value = 3;
            this.numRowsA.Size = new Size(38, 24);
            this.numRowsA.BackColor = Color.FromArgb(40, 43, 56);
            this.numRowsA.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numRowsA)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(this.numColsA)).BeginInit();
            this.numColsA.Location = new Point(172, 54);
            this.numColsA.Minimum = 1;
            this.numColsA.Maximum = 6;
            this.numColsA.Value = 3;
            this.numColsA.Size = new Size(38, 24);
            this.numColsA.BackColor = Color.FromArgb(40, 43, 56);
            this.numColsA.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numColsA)).EndInit();

            StyleSmallBtn(btnResizeA, "Boyutla", new Point(214, 53), new Size(54, 25));
            StyleSmallBtn(btnRandomA, "Rastgele", new Point(272, 53), new Size(58, 25));
            StyleSmallBtn(btnIdentityA, "I", new Point(334, 53), new Size(34, 25));
            StyleSmallBtn(btnZeroA, "0", new Point(372, 53), new Size(34, 25));

            this.pnlGridA.Location = new Point(12, 82);
            this.pnlGridA.Size = new Size(410, 118);
            this.pnlGridA.BackColor = Color.FromArgb(24, 26, 34);
            this.pnlGridA.AutoScroll = true;

            // --- MATRIX B ---
            this.lblHeaderB.Text = "B Matrisi (Sağ Taraf):";
            this.lblHeaderB.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblHeaderB.ForeColor = Color.FromArgb(140, 225, 175);
            this.lblHeaderB.Location = new Point(12, 206);
            this.lblHeaderB.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numRowsB)).BeginInit();
            this.numRowsB.Location = new Point(130, 204);
            this.numRowsB.Minimum = 1;
            this.numRowsB.Maximum = 6;
            this.numRowsB.Value = 3;
            this.numRowsB.Size = new Size(38, 24);
            this.numRowsB.BackColor = Color.FromArgb(40, 43, 56);
            this.numRowsB.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numRowsB)).EndInit();

            ((System.ComponentModel.ISupportInitialize)(this.numColsB)).BeginInit();
            this.numColsB.Location = new Point(172, 204);
            this.numColsB.Minimum = 1;
            this.numColsB.Maximum = 6;
            this.numColsB.Value = 3;
            this.numColsB.Size = new Size(38, 24);
            this.numColsB.BackColor = Color.FromArgb(40, 43, 56);
            this.numColsB.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numColsB)).EndInit();

            StyleSmallBtn(btnResizeB, "Boyutla", new Point(214, 203), new Size(54, 25));
            StyleSmallBtn(btnRandomB, "Rastgele", new Point(272, 203), new Size(58, 25));
            StyleSmallBtn(btnIdentityB, "I", new Point(334, 203), new Size(34, 25));
            StyleSmallBtn(btnZeroB, "0", new Point(372, 203), new Size(34, 25));

            this.pnlGridB.Location = new Point(12, 232);
            this.pnlGridB.Size = new Size(410, 118);
            this.pnlGridB.BackColor = Color.FromArgb(24, 26, 34);
            this.pnlGridB.AutoScroll = true;

            // --- SCALAR & PRESETS ---
            this.lblScalar.Text = "Skaler k:";
            this.lblScalar.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.lblScalar.ForeColor = Color.FromArgb(190, 200, 215);
            this.lblScalar.Location = new Point(12, 358);
            this.lblScalar.AutoSize = true;

            this.txtScalar.Location = new Point(68, 355);
            this.txtScalar.Size = new Size(46, 24);
            this.txtScalar.BackColor = Color.FromArgb(40, 43, 56);
            this.txtScalar.ForeColor = Color.White;
            this.txtScalar.BorderStyle = BorderStyle.FixedSingle;
            this.txtScalar.Text = "2.5";

            StyleSmallBtn(btnCopyAToB, "A→B Kopyala", new Point(118, 354), new Size(88, 25));
            StyleSmallBtn(btnExSPD, "Örnek SPD", new Point(210, 354), new Size(96, 25));
            StyleSmallBtn(btnExGeneral, "Örnek Genel", new Point(310, 354), new Size(98, 25));

            // --- A OPERATIONS ---
            this.lblOpsA.Text = "A Matrisi Analiz & Ayrıştırma (LU, QR, SVD, Cholesky, Özdeğer):";
            this.lblOpsA.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.lblOpsA.ForeColor = Color.FromArgb(180, 195, 220);
            this.lblOpsA.Location = new Point(12, 384);
            this.lblOpsA.AutoSize = true;

            this.btnAllProps.Text = "📊 A Matrisi Tam Analiz (Det, Rank, Norm2, Kondisyon, Tersi)";
            this.btnAllProps.Location = new Point(12, 403);
            this.btnAllProps.Size = new Size(410, 30);
            this.btnAllProps.BackColor = Color.FromArgb(46, 125, 225);
            this.btnAllProps.ForeColor = Color.White;
            this.btnAllProps.FlatStyle = FlatStyle.Flat;
            this.btnAllProps.FlatAppearance.BorderSize = 0;
            this.btnAllProps.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.btnAllProps.Cursor = Cursors.Hand;

            StyleOpBtn(btnLU, "LU Ayrıştırma", new Point(12, 437), new Size(132, 27));
            StyleOpBtn(btnQR, "QR & Householder", new Point(150, 437), new Size(134, 27));
            StyleOpBtn(btnCholesky, "Cholesky (LLᵀ)", new Point(290, 437), new Size(132, 27));
            StyleOpBtn(btnSVD, "SVD (U·Σ·Vᵀ)", new Point(12, 467), new Size(132, 27));
            StyleOpBtn(btnEigen, "Özdeğer (λ, V)", new Point(150, 467), new Size(134, 27));
            StyleOpBtn(btnInverse, "Ters & Transpoz", new Point(290, 467), new Size(132, 27));

            // --- A & B BINARY OPERATIONS ---
            this.lblOpsAB.Text = "İki Matrisli Cebir (A ve B) & Matris Denklem Çözücü:";
            this.lblOpsAB.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.lblOpsAB.ForeColor = Color.FromArgb(140, 225, 175);
            this.lblOpsAB.Location = new Point(12, 498);
            this.lblOpsAB.AutoSize = true;

            StyleBinaryBtn(btnAddAB, "A + B", new Point(12, 517), new Size(98, 27));
            StyleBinaryBtn(btnSubAB, "A - B", new Point(116, 517), new Size(98, 27));
            StyleBinaryBtn(btnMulAB, "A × B (Çarpım)", new Point(220, 517), new Size(98, 27));
            StyleBinaryBtn(btnScalarMul, "k · A", new Point(324, 517), new Size(98, 27));

            StyleBinaryBtn(btnElemMul, "A .* B (Hadamard)", new Point(12, 547), new Size(132, 27));
            StyleBinaryBtn(btnElemDiv, "A ./ B (Eleman Böl)", new Point(150, 547), new Size(134, 27));
            StyleBinaryBtn(btnSolveAXB, "Çöz: A·X = B", new Point(290, 547), new Size(132, 27));

            this.btnSolveTrans.Text = "Transpoz Denklem Çözücü: Aᵀ · Xᵀ = Bᵀ  (SolveTranspose)";
            this.btnSolveTrans.Location = new Point(12, 577);
            this.btnSolveTrans.Size = new Size(410, 27);
            this.btnSolveTrans.BackColor = Color.FromArgb(38, 85, 78);
            this.btnSolveTrans.ForeColor = Color.White;
            this.btnSolveTrans.FlatStyle = FlatStyle.Flat;
            this.btnSolveTrans.FlatAppearance.BorderSize = 0;
            this.btnSolveTrans.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            this.btnSolveTrans.Cursor = Cursors.Hand;

            this.pnlLeft.Controls.Add(this.lblTitle);
            this.pnlLeft.Controls.Add(this.lblDesc);
            this.pnlLeft.Controls.Add(this.lblHeaderA);
            this.pnlLeft.Controls.Add(this.numRowsA);
            this.pnlLeft.Controls.Add(this.numColsA);
            this.pnlLeft.Controls.Add(this.btnResizeA);
            this.pnlLeft.Controls.Add(this.btnRandomA);
            this.pnlLeft.Controls.Add(this.btnIdentityA);
            this.pnlLeft.Controls.Add(this.btnZeroA);
            this.pnlLeft.Controls.Add(this.pnlGridA);

            this.pnlLeft.Controls.Add(this.lblHeaderB);
            this.pnlLeft.Controls.Add(this.numRowsB);
            this.pnlLeft.Controls.Add(this.numColsB);
            this.pnlLeft.Controls.Add(this.btnResizeB);
            this.pnlLeft.Controls.Add(this.btnRandomB);
            this.pnlLeft.Controls.Add(this.btnIdentityB);
            this.pnlLeft.Controls.Add(this.btnZeroB);
            this.pnlLeft.Controls.Add(this.pnlGridB);

            this.pnlLeft.Controls.Add(this.lblScalar);
            this.pnlLeft.Controls.Add(this.txtScalar);
            this.pnlLeft.Controls.Add(this.btnCopyAToB);
            this.pnlLeft.Controls.Add(this.btnExSPD);
            this.pnlLeft.Controls.Add(this.btnExGeneral);

            this.pnlLeft.Controls.Add(this.lblOpsA);
            this.pnlLeft.Controls.Add(this.btnAllProps);
            this.pnlLeft.Controls.Add(this.btnLU);
            this.pnlLeft.Controls.Add(this.btnQR);
            this.pnlLeft.Controls.Add(this.btnCholesky);
            this.pnlLeft.Controls.Add(this.btnSVD);
            this.pnlLeft.Controls.Add(this.btnEigen);
            this.pnlLeft.Controls.Add(this.btnInverse);

            this.pnlLeft.Controls.Add(this.lblOpsAB);
            this.pnlLeft.Controls.Add(this.btnAddAB);
            this.pnlLeft.Controls.Add(this.btnSubAB);
            this.pnlLeft.Controls.Add(this.btnMulAB);
            this.pnlLeft.Controls.Add(this.btnScalarMul);
            this.pnlLeft.Controls.Add(this.btnElemMul);
            this.pnlLeft.Controls.Add(this.btnElemDiv);
            this.pnlLeft.Controls.Add(this.btnSolveAXB);
            this.pnlLeft.Controls.Add(this.btnSolveTrans);

            // Right Pane with Toolbar
            this.pnlRight.Dock = DockStyle.Fill;
            this.pnlRight.Padding = new Padding(12, 0, 0, 0);

            this.pnlResultToolbar.Dock = DockStyle.Top;
            this.pnlResultToolbar.Height = 36;
            this.pnlResultToolbar.BackColor = Color.FromArgb(24, 25, 32);

            this.lblResultTitle.Text = "📊 Matris Analiz & İşlem Çıktısı:";
            this.lblResultTitle.Dock = DockStyle.Left;
            this.lblResultTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblResultTitle.ForeColor = ThemeHelper.LightText;
            this.lblResultTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.btnAnsToA.Text = "📥 Sonucu Matris A'ya Aktar (Ans→A)";
            this.btnAnsToA.Dock = DockStyle.Right;
            this.btnAnsToA.Width = 220;
            this.btnAnsToA.FlatStyle = FlatStyle.Flat;
            this.btnAnsToA.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAnsToA.ForeColor = Color.White;
            this.btnAnsToA.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnAnsToA.FlatAppearance.BorderSize = 0;
            this.btnAnsToA.Cursor = Cursors.Hand;

            this.btnCopyMatrix.Text = "📋 Kopyala";
            this.btnCopyMatrix.Dock = DockStyle.Right;
            this.btnCopyMatrix.Width = 85;
            this.btnCopyMatrix.FlatStyle = FlatStyle.Flat;
            this.btnCopyMatrix.BackColor = ThemeHelper.ButtonSecondary;
            this.btnCopyMatrix.ForeColor = Color.White;
            this.btnCopyMatrix.Font = new Font("Segoe UI", 8.5F);
            this.btnCopyMatrix.FlatAppearance.BorderSize = 0;
            this.btnCopyMatrix.Cursor = Cursors.Hand;

            this.pnlResultToolbar.Controls.Add(this.lblResultTitle);
            this.pnlResultToolbar.Controls.Add(this.btnCopyMatrix);
            this.pnlResultToolbar.Controls.Add(this.btnAnsToA);

            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(18, 19, 25);
            this.txtResult.ForeColor = Color.FromArgb(225, 232, 245);
            this.txtResult.Font = new Font("Consolas", 10F);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.ReadOnly = true;

            this.pnlRight.Controls.Add(this.txtResult);
            this.pnlRight.Controls.Add(this.pnlResultToolbar);

            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.ResumeLayout(false);
        }

        private void StyleSmallBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(45, 50, 68);
            b.ForeColor = Color.FromArgb(215, 225, 245);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 8F);
            b.Cursor = Cursors.Hand;
        }

        private void StyleOpBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(52, 60, 84);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }

        private void StyleBinaryBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(40, 95, 76);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }
    }
}
