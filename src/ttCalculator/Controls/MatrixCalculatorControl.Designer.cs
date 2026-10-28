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
        private Panel pnlCardA;
        private Label lblHeaderA;
        private NumericUpDown numRowsA;
        private NumericUpDown numColsA;
        private Label lblTimesA;
        private Button btnResizeA;
        private Button btnRandomA;
        private Button btnIdentityA;
        private Button btnZeroA;
        private Button btnSpdA;
        private Button btnPasteA;
        private DataGridView dgvMatrixA;

        // Matrix B section
        private Panel pnlCardB;
        private Label lblHeaderB;
        private NumericUpDown numRowsB;
        private NumericUpDown numColsB;
        private Label lblTimesB;
        private Button btnResizeB;
        private Button btnRandomB;
        private Button btnIdentityB;
        private Button btnZeroB;
        private Button btnCopyAToB;
        private Button btnSwapAB;
        private Button btnPasteB;
        private DataGridView dgvMatrixB;

        // Scalar & Presets
        private Panel pnlScalarPreset;
        private Label lblScalar;
        private TextBox txtScalar;
        private Button btnExSPD;
        private Button btnExGeneral;
        private Button btnExOrthogonal;

        // Segmented Operations Tabs
        private TabControl tabOperations;
        private TabPage tabUnary;
        private TabPage tabDecomp;
        private TabPage tabBinary;

        // Tab 1 Unary Buttons
        private Button btnAllProps;
        private Button btnInverse;
        private Button btnTranspose;
        private Button btnUnaryMinus;
        private Button btnTraceDet;
        private Button btnNorms;
        private Button btnCondRank;

        // Tab 2 Decompositions Buttons
        private Button btnLU;
        private Button btnQR;
        private Button btnCholesky;
        private Button btnSVD;
        private Button btnEigen;

        // Tab 3 Binary & Solvers Buttons
        private Button btnAddAB;
        private Button btnSubAB;
        private Button btnMulAB;
        private Button btnScalarMul;
        private Button btnElemMul;
        private Button btnElemDiv;
        private Button btnSolveAXB;
        private Button btnSolveTrans;

        // Right side results & actions
        private Panel pnlRight;
        private Panel pnlKpiContainer;
        private Panel pnlKpiDet;
        private Label lblKpiDetTitle;
        private Label lblKpiDetVal;
        private Panel pnlKpiRank;
        private Label lblKpiRankTitle;
        private Label lblKpiRankVal;
        private Panel pnlKpiCond;
        private Label lblKpiCondTitle;
        private Label lblKpiCondVal;

        private Panel pnlResultToolbar;
        private Label lblResultTitle;
        private Button btnAnsToA;
        private Button btnAnsToB;
        private Button btnCopyLatex;
        private Button btnCopyMatrix;
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

            // A controls
            this.pnlCardA = new Panel();
            this.lblHeaderA = new Label();
            this.numRowsA = new NumericUpDown();
            this.lblTimesA = new Label();
            this.numColsA = new NumericUpDown();
            this.btnResizeA = new Button();
            this.btnRandomA = new Button();
            this.btnIdentityA = new Button();
            this.btnZeroA = new Button();
            this.btnSpdA = new Button();
            this.btnPasteA = new Button();
            this.dgvMatrixA = new DataGridView();

            // B controls
            this.pnlCardB = new Panel();
            this.lblHeaderB = new Label();
            this.numRowsB = new NumericUpDown();
            this.lblTimesB = new Label();
            this.numColsB = new NumericUpDown();
            this.btnResizeB = new Button();
            this.btnRandomB = new Button();
            this.btnIdentityB = new Button();
            this.btnZeroB = new Button();
            this.btnCopyAToB = new Button();
            this.btnSwapAB = new Button();
            this.btnPasteB = new Button();
            this.dgvMatrixB = new DataGridView();

            // Presets & Scalar
            this.pnlScalarPreset = new Panel();
            this.lblScalar = new Label();
            this.txtScalar = new TextBox();
            this.btnExSPD = new Button();
            this.btnExGeneral = new Button();
            this.btnExOrthogonal = new Button();

            // Tabs
            this.tabOperations = new TabControl();
            this.tabUnary = new TabPage();
            this.tabDecomp = new TabPage();
            this.tabBinary = new TabPage();

            // Tab 1 buttons
            this.btnAllProps = new Button();
            this.btnInverse = new Button();
            this.btnTranspose = new Button();
            this.btnUnaryMinus = new Button();
            this.btnTraceDet = new Button();
            this.btnNorms = new Button();
            this.btnCondRank = new Button();

            // Tab 2 buttons
            this.btnLU = new Button();
            this.btnQR = new Button();
            this.btnCholesky = new Button();
            this.btnSVD = new Button();
            this.btnEigen = new Button();

            // Tab 3 buttons
            this.btnAddAB = new Button();
            this.btnSubAB = new Button();
            this.btnMulAB = new Button();
            this.btnScalarMul = new Button();
            this.btnElemMul = new Button();
            this.btnElemDiv = new Button();
            this.btnSolveAXB = new Button();
            this.btnSolveTrans = new Button();

            // Right side
            this.pnlRight = new Panel();
            this.pnlKpiContainer = new Panel();
            this.pnlKpiDet = new Panel();
            this.lblKpiDetTitle = new Label();
            this.lblKpiDetVal = new Label();
            this.pnlKpiRank = new Panel();
            this.lblKpiRankTitle = new Label();
            this.lblKpiRankVal = new Label();
            this.pnlKpiCond = new Panel();
            this.lblKpiCondTitle = new Label();
            this.lblKpiCondVal = new Label();

            this.pnlResultToolbar = new Panel();
            this.lblResultTitle = new Label();
            this.btnAnsToA = new Button();
            this.btnAnsToB = new Button();
            this.btnCopyLatex = new Button();
            this.btnCopyMatrix = new Button();
            this.txtResult = new RichTextBox();

            this.SuspendLayout();

            this.BackColor = Color.FromArgb(15, 23, 42);
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(12);

            // Left panel
            this.pnlLeft.Dock = DockStyle.Left;
            this.pnlLeft.Width = 470;
            this.pnlLeft.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlLeft.Padding = new Padding(12);
            this.pnlLeft.AutoScroll = true;

            this.lblTitle.Text = "Matris Cebiri & Ayrıştırma Stüdyosu";
            this.lblTitle.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(12, 10);
            this.lblTitle.AutoSize = true;

            this.lblDesc.Text = "A ve B matrisleri üzerinde analitik doğrusal cebir, LU/QR/SVD/Cholesky ayrıştırmaları ve AX=B sistem çözücüsü.";
            this.lblDesc.Font = new Font("Segoe UI", 8.5F);
            this.lblDesc.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblDesc.Location = new Point(12, 34);
            this.lblDesc.Size = new Size(440, 28);

            // ================== CARD A ==================
            this.pnlCardA.Location = new Point(12, 66);
            this.pnlCardA.Size = new Size(440, 165);
            this.pnlCardA.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlCardA.Padding = new Padding(8);

            this.lblHeaderA.Text = "A Matrisi:";
            this.lblHeaderA.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblHeaderA.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblHeaderA.Location = new Point(8, 8);
            this.lblHeaderA.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numRowsA)).BeginInit();
            this.numRowsA.Location = new Point(80, 6);
            this.numRowsA.Minimum = 1;
            this.numRowsA.Maximum = 8;
            this.numRowsA.Value = 3;
            this.numRowsA.Size = new Size(38, 24);
            this.numRowsA.BackColor = Color.FromArgb(15, 23, 42);
            this.numRowsA.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numRowsA)).EndInit();

            this.lblTimesA.Text = "×";
            this.lblTimesA.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblTimesA.Location = new Point(120, 8);
            this.lblTimesA.AutoSize = true;
            this.lblTimesA.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            ((System.ComponentModel.ISupportInitialize)(this.numColsA)).BeginInit();
            this.numColsA.Location = new Point(134, 6);
            this.numColsA.Minimum = 1;
            this.numColsA.Maximum = 8;
            this.numColsA.Value = 3;
            this.numColsA.Size = new Size(38, 24);
            this.numColsA.BackColor = Color.FromArgb(15, 23, 42);
            this.numColsA.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numColsA)).EndInit();

            StyleSmallBtn(btnResizeA, "Boyutla", new Point(176, 5), new Size(54, 25));
            StyleSmallBtn(btnRandomA, "Rastgele", new Point(233, 5), new Size(58, 25));
            StyleSmallBtn(btnIdentityA, "I", new Point(294, 5), new Size(28, 25));
            StyleSmallBtn(btnZeroA, "0", new Point(325, 5), new Size(28, 25));
            StyleSmallBtn(btnSpdA, "SPD", new Point(356, 5), new Size(40, 25));
            StyleSmallBtn(btnPasteA, "📋", new Point(399, 5), new Size(33, 25));

            SetupDataGridView(this.dgvMatrixA, new Point(8, 35), new Size(424, 122));

            this.pnlCardA.Controls.Add(this.lblHeaderA);
            this.pnlCardA.Controls.Add(this.numRowsA);
            this.pnlCardA.Controls.Add(this.lblTimesA);
            this.pnlCardA.Controls.Add(this.numColsA);
            this.pnlCardA.Controls.Add(this.btnResizeA);
            this.pnlCardA.Controls.Add(this.btnRandomA);
            this.pnlCardA.Controls.Add(this.btnIdentityA);
            this.pnlCardA.Controls.Add(this.btnZeroA);
            this.pnlCardA.Controls.Add(this.btnSpdA);
            this.pnlCardA.Controls.Add(this.btnPasteA);
            this.pnlCardA.Controls.Add(this.dgvMatrixA);

            // ================== CARD B ==================
            this.pnlCardB.Location = new Point(12, 237);
            this.pnlCardB.Size = new Size(440, 165);
            this.pnlCardB.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlCardB.Padding = new Padding(8);

            this.lblHeaderB.Text = "B Matrisi:";
            this.lblHeaderB.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblHeaderB.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblHeaderB.Location = new Point(8, 8);
            this.lblHeaderB.AutoSize = true;

            ((System.ComponentModel.ISupportInitialize)(this.numRowsB)).BeginInit();
            this.numRowsB.Location = new Point(80, 6);
            this.numRowsB.Minimum = 1;
            this.numRowsB.Maximum = 8;
            this.numRowsB.Value = 3;
            this.numRowsB.Size = new Size(38, 24);
            this.numRowsB.BackColor = Color.FromArgb(15, 23, 42);
            this.numRowsB.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numRowsB)).EndInit();

            this.lblTimesB.Text = "×";
            this.lblTimesB.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblTimesB.Location = new Point(120, 8);
            this.lblTimesB.AutoSize = true;
            this.lblTimesB.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            ((System.ComponentModel.ISupportInitialize)(this.numColsB)).BeginInit();
            this.numColsB.Location = new Point(134, 6);
            this.numColsB.Minimum = 1;
            this.numColsB.Maximum = 8;
            this.numColsB.Value = 3;
            this.numColsB.Size = new Size(38, 24);
            this.numColsB.BackColor = Color.FromArgb(15, 23, 42);
            this.numColsB.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numColsB)).EndInit();

            StyleSmallBtn(btnResizeB, "Boyutla", new Point(176, 5), new Size(54, 25));
            StyleSmallBtn(btnRandomB, "Rastgele", new Point(233, 5), new Size(58, 25));
            StyleSmallBtn(btnIdentityB, "I", new Point(294, 5), new Size(28, 25));
            StyleSmallBtn(btnZeroB, "0", new Point(325, 5), new Size(28, 25));
            StyleSmallBtn(btnCopyAToB, "A→B", new Point(356, 5), new Size(40, 25));
            StyleSmallBtn(btnPasteB, "📋", new Point(399, 5), new Size(33, 25));

            SetupDataGridView(this.dgvMatrixB, new Point(8, 35), new Size(424, 122));

            this.pnlCardB.Controls.Add(this.lblHeaderB);
            this.pnlCardB.Controls.Add(this.numRowsB);
            this.pnlCardB.Controls.Add(this.lblTimesB);
            this.pnlCardB.Controls.Add(this.numColsB);
            this.pnlCardB.Controls.Add(this.btnResizeB);
            this.pnlCardB.Controls.Add(this.btnRandomB);
            this.pnlCardB.Controls.Add(this.btnIdentityB);
            this.pnlCardB.Controls.Add(this.btnZeroB);
            this.pnlCardB.Controls.Add(this.btnCopyAToB);
            this.pnlCardB.Controls.Add(this.btnPasteB);
            this.pnlCardB.Controls.Add(this.dgvMatrixB);

            // ================== PRESETS & SCALAR ==================
            this.pnlScalarPreset.Location = new Point(12, 408);
            this.pnlScalarPreset.Size = new Size(440, 36);
            this.pnlScalarPreset.BackColor = Color.FromArgb(20, 28, 48);

            this.lblScalar.Text = "Skaler k:";
            this.lblScalar.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblScalar.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblScalar.Location = new Point(2, 9);
            this.lblScalar.AutoSize = true;

            this.txtScalar.Location = new Point(56, 6);
            this.txtScalar.Size = new Size(44, 24);
            this.txtScalar.BackColor = Color.FromArgb(15, 23, 42);
            this.txtScalar.ForeColor = Color.White;
            this.txtScalar.BorderStyle = BorderStyle.FixedSingle;
            this.txtScalar.Text = "2.5";
            this.txtScalar.TextAlign = HorizontalAlignment.Center;

            StyleSmallBtn(btnSwapAB, "A ↔ B Takas", new Point(106, 5), new Size(88, 25));
            StyleSmallBtn(btnExSPD, "Örnek SPD", new Point(198, 5), new Size(76, 25));
            StyleSmallBtn(btnExGeneral, "Örnek Genel", new Point(278, 5), new Size(80, 25));
            StyleSmallBtn(btnExOrthogonal, "Ortogonal", new Point(362, 5), new Size(72, 25));

            this.pnlScalarPreset.Controls.Add(this.lblScalar);
            this.pnlScalarPreset.Controls.Add(this.txtScalar);
            this.pnlScalarPreset.Controls.Add(this.btnSwapAB);
            this.pnlScalarPreset.Controls.Add(this.btnExSPD);
            this.pnlScalarPreset.Controls.Add(this.btnExGeneral);
            this.pnlScalarPreset.Controls.Add(this.btnExOrthogonal);

            // ================== TABBED OPERATIONS ==================
            this.tabOperations.Location = new Point(12, 448);
            this.tabOperations.Size = new Size(440, 230);
            this.tabOperations.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // TAB 1: Tekil Analiz
            this.tabUnary.Text = "Tekil Analiz (A)";
            this.tabUnary.BackColor = Color.FromArgb(24, 33, 52);
            this.tabUnary.Padding = new Padding(8);

            this.btnAllProps.Text = "📊 A Matrisi Tam Analizi (Det, Rank, İz, Normlar, Kondisyon)";
            this.btnAllProps.Location = new Point(8, 8);
            this.btnAllProps.Size = new Size(418, 32);
            this.btnAllProps.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAllProps.ForeColor = Color.White;
            this.btnAllProps.FlatStyle = FlatStyle.Flat;
            this.btnAllProps.FlatAppearance.BorderSize = 0;
            this.btnAllProps.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.btnAllProps.Cursor = Cursors.Hand;

            StyleOpBtn(btnInverse, "Ters Matris (A⁻¹)", new Point(8, 46), new Size(135, 30));
            StyleOpBtn(btnTranspose, "Transpoz (Aᵀ)", new Point(149, 46), new Size(135, 30));
            StyleOpBtn(btnUnaryMinus, "Negatifi (-A)", new Point(290, 46), new Size(136, 30));

            StyleOpBtn(btnTraceDet, "İz & Determinant", new Point(8, 82), new Size(135, 30));
            StyleOpBtn(btnNorms, "Matris Normları (1, 2, ∞, F)", new Point(149, 82), new Size(277, 30));

            StyleOpBtn(btnCondRank, "Kondisyon Sayısı κ(A) & Rank", new Point(8, 118), new Size(418, 30));

            this.tabUnary.Controls.Add(this.btnAllProps);
            this.tabUnary.Controls.Add(this.btnInverse);
            this.tabUnary.Controls.Add(this.btnTranspose);
            this.tabUnary.Controls.Add(this.btnUnaryMinus);
            this.tabUnary.Controls.Add(this.btnTraceDet);
            this.tabUnary.Controls.Add(this.btnNorms);
            this.tabUnary.Controls.Add(this.btnCondRank);

            // TAB 2: Ayrıştırmalar
            this.tabDecomp.Text = "Ayrıştırmalar";
            this.tabDecomp.BackColor = Color.FromArgb(24, 33, 52);
            this.tabDecomp.Padding = new Padding(8);

            StyleOpBtn(btnLU, "LU Ayrışımı (P·A = L·U)", new Point(8, 10), new Size(205, 32));
            StyleOpBtn(btnQR, "QR Ayrışımı (Householder)", new Point(221, 10), new Size(205, 32));
            StyleOpBtn(btnCholesky, "Cholesky (A = L·Lᵀ)", new Point(8, 48), new Size(205, 32));
            StyleOpBtn(btnSVD, "SVD (A = U·Σ·Vᵀ)", new Point(221, 48), new Size(205, 32));
            StyleOpBtn(btnEigen, "Özdeğerler & Özvektörler (A·v = λ·v)", new Point(8, 86), new Size(418, 32));

            this.tabDecomp.Controls.Add(this.btnLU);
            this.tabDecomp.Controls.Add(this.btnQR);
            this.tabDecomp.Controls.Add(this.btnCholesky);
            this.tabDecomp.Controls.Add(this.btnSVD);
            this.tabDecomp.Controls.Add(this.btnEigen);

            // TAB 3: İki Matrisli & Denklem Çözücü
            this.tabBinary.Text = "İki Matrisli & Denklem";
            this.tabBinary.BackColor = Color.FromArgb(24, 33, 52);
            this.tabBinary.Padding = new Padding(8);

            StyleBinaryBtn(btnAddAB, "A + B", new Point(8, 10), new Size(100, 30));
            StyleBinaryBtn(btnSubAB, "A - B", new Point(114, 10), new Size(100, 30));
            StyleBinaryBtn(btnMulAB, "A × B (Çarpım)", new Point(220, 10), new Size(100, 30));
            StyleBinaryBtn(btnScalarMul, "k · A", new Point(326, 10), new Size(100, 30));

            StyleBinaryBtn(btnElemMul, "A .* B (Hadamard)", new Point(8, 46), new Size(206, 30));
            StyleBinaryBtn(btnElemDiv, "A ./ B (Noktasal Böl)", new Point(220, 46), new Size(206, 30));

            StyleBinaryBtn(btnSolveAXB, "Lineer Sistem Çöz: A · X = B", new Point(8, 82), new Size(418, 32));
            StyleBinaryBtn(btnSolveTrans, "Transpoz Çözücü: Aᵀ · Xᵀ = Bᵀ", new Point(8, 120), new Size(418, 32));

            this.tabBinary.Controls.Add(this.btnAddAB);
            this.tabBinary.Controls.Add(this.btnSubAB);
            this.tabBinary.Controls.Add(this.btnMulAB);
            this.tabBinary.Controls.Add(this.btnScalarMul);
            this.tabBinary.Controls.Add(this.btnElemMul);
            this.tabBinary.Controls.Add(this.btnElemDiv);
            this.tabBinary.Controls.Add(this.btnSolveAXB);
            this.tabBinary.Controls.Add(this.btnSolveTrans);

            this.tabOperations.TabPages.Add(this.tabUnary);
            this.tabOperations.TabPages.Add(this.tabDecomp);
            this.tabOperations.TabPages.Add(this.tabBinary);

            this.pnlLeft.Controls.Add(this.lblTitle);
            this.pnlLeft.Controls.Add(this.lblDesc);
            this.pnlLeft.Controls.Add(this.pnlCardA);
            this.pnlLeft.Controls.Add(this.pnlCardB);
            this.pnlLeft.Controls.Add(this.pnlScalarPreset);
            this.pnlLeft.Controls.Add(this.tabOperations);

            // ================== RIGHT PANEL ==================
            this.pnlRight.Dock = DockStyle.Fill;
            this.pnlRight.Padding = new Padding(12, 0, 0, 0);

            // KPI Container
            this.pnlKpiContainer.Dock = DockStyle.Top;
            this.pnlKpiContainer.Height = 56;
            this.pnlKpiContainer.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlKpiContainer.Padding = new Padding(0, 0, 0, 8);

            this.pnlKpiDet = CreateKpiCard("det(A)", "—", Color.FromArgb(56, 189, 248), out this.lblKpiDetTitle, out this.lblKpiDetVal);
            this.pnlKpiDet.Dock = DockStyle.Left;
            this.pnlKpiDet.Width = 140;

            this.pnlKpiRank = CreateKpiCard("rank(A)", "—", Color.FromArgb(52, 211, 153), out this.lblKpiRankTitle, out this.lblKpiRankVal);
            this.pnlKpiRank.Dock = DockStyle.Left;
            this.pnlKpiRank.Width = 140;

            this.pnlKpiCond = CreateKpiCard("kondisyon κ(A)", "—", Color.FromArgb(251, 191, 36), out this.lblKpiCondTitle, out this.lblKpiCondVal);
            this.pnlKpiCond.Dock = DockStyle.Left;
            this.pnlKpiCond.Width = 160;

            this.pnlKpiContainer.Controls.Add(this.pnlKpiCond);
            this.pnlKpiContainer.Controls.Add(this.pnlKpiRank);
            this.pnlKpiContainer.Controls.Add(this.pnlKpiDet);

            // Result Toolbar
            this.pnlResultToolbar.Dock = DockStyle.Top;
            this.pnlResultToolbar.Height = 36;
            this.pnlResultToolbar.BackColor = Color.FromArgb(20, 28, 48);

            this.lblResultTitle.Text = "📊 Matris Analiz & Ayrıştırma Çıktısı:";
            this.lblResultTitle.Dock = DockStyle.Left;
            this.lblResultTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblResultTitle.ForeColor = ThemeHelper.LightText;
            this.lblResultTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.lblResultTitle.Padding = new Padding(8, 0, 0, 0);

            this.btnAnsToA.Text = "📥 Ans → A";
            this.btnAnsToA.Dock = DockStyle.Right;
            this.btnAnsToA.Width = 100;
            this.btnAnsToA.FlatStyle = FlatStyle.Flat;
            this.btnAnsToA.BackColor = Color.FromArgb(37, 99, 235);
            this.btnAnsToA.ForeColor = Color.White;
            this.btnAnsToA.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnAnsToA.FlatAppearance.BorderSize = 0;
            this.btnAnsToA.Cursor = Cursors.Hand;

            this.btnAnsToB.Text = "📥 Ans → B";
            this.btnAnsToB.Dock = DockStyle.Right;
            this.btnAnsToB.Width = 100;
            this.btnAnsToB.FlatStyle = FlatStyle.Flat;
            this.btnAnsToB.BackColor = Color.FromArgb(16, 185, 129);
            this.btnAnsToB.ForeColor = Color.White;
            this.btnAnsToB.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnAnsToB.FlatAppearance.BorderSize = 0;
            this.btnAnsToB.Cursor = Cursors.Hand;

            this.btnCopyLatex.Text = "LaTeX Kopyala";
            this.btnCopyLatex.Dock = DockStyle.Right;
            this.btnCopyLatex.Width = 110;
            this.btnCopyLatex.FlatStyle = FlatStyle.Flat;
            this.btnCopyLatex.BackColor = ThemeHelper.ButtonSecondary;
            this.btnCopyLatex.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnCopyLatex.Font = new Font("Segoe UI Semibold", 8.5F);
            this.btnCopyLatex.FlatAppearance.BorderSize = 0;
            this.btnCopyLatex.Cursor = Cursors.Hand;

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
            this.pnlResultToolbar.Controls.Add(this.btnCopyLatex);
            this.pnlResultToolbar.Controls.Add(this.btnCopyMatrix);
            this.pnlResultToolbar.Controls.Add(this.btnAnsToB);
            this.pnlResultToolbar.Controls.Add(this.btnAnsToA);

            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(11, 15, 28);
            this.txtResult.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtResult.Font = new Font("Consolas", 10F);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.ReadOnly = true;
            this.txtResult.WordWrap = false;

            this.pnlRight.Controls.Add(this.txtResult);
            this.pnlRight.Controls.Add(this.pnlResultToolbar);
            this.pnlRight.Controls.Add(this.pnlKpiContainer);

            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlLeft);
            this.ResumeLayout(false);
        }

        private Panel CreateKpiCard(string title, string defVal, Color accent, out Label lblTitle, out Label lblVal)
        {
            Panel p = new Panel
            {
                BackColor = Color.FromArgb(24, 33, 52),
                Margin = new Padding(0, 0, 8, 0),
                Padding = new Padding(10, 4, 10, 4)
            };

            lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                Dock = DockStyle.Top,
                Height = 16
            };

            lblVal = new Label
            {
                Text = defVal,
                Font = new Font("Consolas", 11.5F, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            p.Controls.Add(lblVal);
            p.Controls.Add(lblTitle);
            return p;
        }

        private void SetupDataGridView(DataGridView dgv, Point loc, Size sz)
        {
            dgv.Location = loc;
            dgv.Size = sz;
            dgv.BackgroundColor = Color.FromArgb(15, 23, 42);
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersWidth = 38;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 33, 52);
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 33, 52);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 22;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(20, 28, 48);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.GridColor = Color.FromArgb(40, 50, 75);
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void StyleSmallBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(33, 44, 70);
            b.ForeColor = Color.FromArgb(226, 232, 240);
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
            b.BackColor = Color.FromArgb(33, 44, 70);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }

        private void StyleBinaryBtn(Button b, string text, Point loc, Size sz)
        {
            b.Text = text;
            b.Location = loc;
            b.Size = sz;
            b.BackColor = Color.FromArgb(15, 60, 48);
            b.ForeColor = Color.FromArgb(220, 252, 231);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }
    }
}
