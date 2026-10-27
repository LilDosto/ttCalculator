using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DotNetMatrix;

namespace MathApp.Controls
{
    public partial class MatrixCalculatorControl : UserControl
    {
        private TextBox[,] gridCellsA;
        private TextBox[,] gridCellsB;
        private int rowsA = 3, colsA = 3;
        private int rowsB = 3, colsB = 3;
        private GeneralMatrix lastResultMatrix = null;

        public MatrixCalculatorControl()
        {
            InitializeComponent();

            btnResizeA.Click += (s, e) => BuildGridA((int)numRowsA.Value, (int)numColsA.Value);
            btnRandomA.Click += (s, e) => FillRandom(gridCellsA, rowsA, colsA);
            btnIdentityA.Click += (s, e) => FillIdentity(gridCellsA, rowsA, colsA);
            btnZeroA.Click += (s, e) => FillZero(gridCellsA, rowsA, colsA);

            btnResizeB.Click += (s, e) => BuildGridB((int)numRowsB.Value, (int)numColsB.Value);
            btnRandomB.Click += (s, e) => FillRandom(gridCellsB, rowsB, colsB);
            btnIdentityB.Click += (s, e) => FillIdentity(gridCellsB, rowsB, colsB);
            btnZeroB.Click += (s, e) => FillZero(gridCellsB, rowsB, colsB);
            btnCopyAToB.Click += BtnCopyAToB_Click;

            btnAnsToA.Click += BtnAnsToA_Click;
            btnCopyMatrix.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("Matris çıktısı panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnExSPD.Click += (s, e) => LoadPresetMatrices(
                new decimal[,] { { 4, 12, -16 }, { 12, 37, -43 }, { -16, -43, 98 } },
                new decimal[,] { { 1, 2, 0 }, { 2, 5, 1 }, { 0, 1, 3 } });

            btnExGeneral.Click += (s, e) => LoadPresetMatrices(
                new decimal[,] { { 2, 1, 1 }, { 4, -6, 0 }, { -2, 7, 2 } },
                new decimal[,] { { 1, 0, 2 }, { -1, 3, 1 }, { 2, 1, 4 } });

            // Single Matrix A Decompositions & Properties
            btnAllProps.Click += (s, e) => RunUnaryOperation("ALL");
            btnLU.Click += (s, e) => RunUnaryOperation("LU");
            btnQR.Click += (s, e) => RunUnaryOperation("QR");
            btnCholesky.Click += (s, e) => RunUnaryOperation("CHOLESKY");
            btnSVD.Click += (s, e) => RunUnaryOperation("SVD");
            btnEigen.Click += (s, e) => RunUnaryOperation("EIGEN");
            btnInverse.Click += (s, e) => RunUnaryOperation("INVERSE");

            // Dual Matrix (A & B) Arithmetic & Solvers
            btnAddAB.Click += (s, e) => RunBinaryOperation("ADD");
            btnSubAB.Click += (s, e) => RunBinaryOperation("SUB");
            btnMulAB.Click += (s, e) => RunBinaryOperation("MUL");
            btnElemMul.Click += (s, e) => RunBinaryOperation("ELEM_MUL");
            btnElemDiv.Click += (s, e) => RunBinaryOperation("ELEM_DIV");
            btnScalarMul.Click += (s, e) => RunBinaryOperation("SCALAR");
            btnSolveAXB.Click += (s, e) => RunBinaryOperation("SOLVE");
            btnSolveTrans.Click += (s, e) => RunBinaryOperation("SOLVE_TRANS");

            LoadPresetMatrices(
                new decimal[,] { { 4, 12, -16 }, { 12, 37, -43 }, { -16, -43, 98 } },
                new decimal[,] { { 1, 2, 0 }, { 2, 5, 1 }, { 0, 1, 3 } },
                autoRun: false);
        }

        private void LoadPresetMatrices(decimal[,] dataA, decimal[,] dataB, bool autoRun = true)
        {
            numRowsA.Value = dataA.GetLength(0);
            numColsA.Value = dataA.GetLength(1);
            BuildGridA(dataA.GetLength(0), dataA.GetLength(1));
            for (int i = 0; i < rowsA; i++)
                for (int j = 0; j < colsA; j++)
                    gridCellsA[i, j].Text = dataA[i, j].ToString(CultureInfo.InvariantCulture);

            numRowsB.Value = dataB.GetLength(0);
            numColsB.Value = dataB.GetLength(1);
            BuildGridB(dataB.GetLength(0), dataB.GetLength(1));
            for (int i = 0; i < rowsB; i++)
                for (int j = 0; j < colsB; j++)
                    gridCellsB[i, j].Text = dataB[i, j].ToString(CultureInfo.InvariantCulture);

            if (autoRun) RunUnaryOperation("ALL");
        }

        private void BtnCopyAToB_Click(object sender, EventArgs e)
        {
            numRowsB.Value = rowsA;
            numColsB.Value = colsA;
            BuildGridB(rowsA, colsA);
            for (int i = 0; i < rowsA; i++)
                for (int j = 0; j < colsA; j++)
                    gridCellsB[i, j].Text = gridCellsA[i, j].Text;
        }

        private void BuildGridA(int r, int c)
        {
            rowsA = r; colsA = c;
            gridCellsA = BuildGridInternal(pnlGridA, r, c);
        }

        private void BuildGridB(int r, int c)
        {
            rowsB = r; colsB = c;
            gridCellsB = BuildGridInternal(pnlGridB, r, c);
        }

        private TextBox[,] BuildGridInternal(Panel container, int rows, int cols)
        {
            container.SuspendLayout();
            container.Controls.Clear();
            TextBox[,] cells = new TextBox[rows, cols];
            int cellW = 58;
            int cellH = 26;
            int gap = 4;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    TextBox tb = new TextBox
                    {
                        Location = new Point(6 + j * (cellW + gap), 6 + i * (cellH + gap)),
                        Size = new Size(cellW, cellH),
                        BackColor = ThemeHelper.CardBackground,
                        ForeColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle,
                        Font = new Font("Consolas", 10F, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        Text = (i == j) ? "1" : "0"
                    };
                    cells[i, j] = tb;
                    container.Controls.Add(tb);
                }
            }
            container.ResumeLayout();
            return cells;
        }

        private void FillRandom(TextBox[,] cells, int r, int c)
        {
            decimal[][] rand = Matrix.GenerateRandomMatrix(r, c, -9, 9);
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    cells[i, j].Text = Math.Round(rand[i][j], 0).ToString(CultureInfo.InvariantCulture);
        }

        private void FillZero(TextBox[,] cells, int r, int c)
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    cells[i, j].Text = "0";
        }

        private void BtnAnsToA_Click(object sender, EventArgs e)
        {
            if (lastResultMatrix == null)
            {
                MessageBox.Show("Aktarılacak matris sonucu bulunamadı. Önce bir işlem yapınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int r = Math.Min(6, lastResultMatrix.RowDimension);
            int c = Math.Min(6, lastResultMatrix.ColumnDimension);

            numRowsA.Value = r;
            numColsA.Value = c;
            BuildGridA(r, c);

            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    gridCellsA[i, j].Text = Math.Round(lastResultMatrix.GetElement(i, j), 6).ToString(CultureInfo.InvariantCulture);

            MessageBox.Show($"Sonuç matrisi ({r}x{c}) başarıyla Matris A'ya aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FillIdentity(TextBox[,] cells, int r, int c)
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    cells[i, j].Text = (i == j) ? "1" : "0";
        }

        private GeneralMatrix ReadMatrix(TextBox[,] cells, int r, int c, string name)
        {
            decimal[][] arr = new decimal[r][];
            for (int i = 0; i < r; i++)
            {
                arr[i] = new decimal[c];
                for (int j = 0; j < c; j++)
                {
                    string raw = cells[i, j].Text.Trim().Replace(',', '.');
                    if (!decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                    {
                        throw new ArgumentException($"{name} matrisinin ({i + 1}, {j + 1}) hücresinde geçersiz sayı: '{cells[i, j].Text}'");
                    }
                    arr[i][j] = val;
                }
            }
            return new GeneralMatrix(arr);
        }

        private string FormatMatrix(GeneralMatrix m)
        {
            StringBuilder sb = new StringBuilder();
            decimal[][] a = m.Array;
            for (int i = 0; i < m.RowDimension; i++)
            {
                sb.Append("  [ ");
                for (int j = 0; j < m.ColumnDimension; j++)
                {
                    decimal v = Math.Round(a[i][j], 5);
                    if (DecimalMath.Abs(v) < 0.000001m) v = 0m;
                    sb.Append($"{v.ToString("0.#####", CultureInfo.InvariantCulture),11}");
                    if (j < m.ColumnDimension - 1) sb.Append("  ");
                }
                sb.AppendLine(" ]");
            }
            return sb.ToString();
        }

        private void RunBinaryOperation(string op)
        {
            try
            {
                GeneralMatrix A = ReadMatrix(gridCellsA, rowsA, colsA, "A");
                StringBuilder sb = new StringBuilder();

                if (op == "SCALAR")
                {
                    string sRaw = txtScalar.Text.Trim().Replace(',', '.');
                    if (!decimal.TryParse(sRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal k))
                    {
                        txtResult.Text = "❌ Geçerli bir skaler çarpan (k) giriniz.";
                        return;
                    }
                    GeneralMatrix res = A.Multiply(k);
                    lastResultMatrix = res;
                    sb.AppendLine($"=== SKALER ÇARPIM (k · A,  k = {k}) ===");
                    sb.Append(FormatMatrix(res));
                    txtResult.Text = sb.ToString();
                    return;
                }

                GeneralMatrix B = ReadMatrix(gridCellsB, rowsB, colsB, "B");

                if (op == "ADD")
                {
                    lastResultMatrix = A + B;
                    sb.AppendLine("=== MATRİS TOPLAMI (A + B) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "SUB")
                {
                    lastResultMatrix = A - B;
                    sb.AppendLine("=== MATRİS FARKI (A - B) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "MUL")
                {
                    lastResultMatrix = A * B;
                    sb.AppendLine($"=== MATRİS ÇARPIMI (A × B) [{rowsA}x{colsA}] · [{rowsB}x{colsB}] ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "ELEM_MUL")
                {
                    lastResultMatrix = A.ArrayMultiply(B);
                    sb.AppendLine("=== ELEMAN BAZLI (HADAMARD) ÇARPIM (A .* B) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "ELEM_DIV")
                {
                    lastResultMatrix = A.ArrayRightDivide(B);
                    sb.AppendLine("=== ELEMAN BAZLI BÖLME (A ./ B) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "SOLVE")
                {
                    lastResultMatrix = A.Solve(B);
                    sb.AppendLine("=== MATRİS DENKLEMİ ÇÖZÜMÜ (A · X = B  =>  X) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }
                else if (op == "SOLVE_TRANS")
                {
                    lastResultMatrix = A.SolveTranspose(B);
                    sb.AppendLine("=== TRANSPOZ MATRİS DENKLEMİ ÇÖZÜMÜ (Aᵀ · Xᵀ = Bᵀ) ===");
                    sb.Append(FormatMatrix(lastResultMatrix));
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ İşlem Hatası: " + ex.Message;
            }
        }

        private void RunUnaryOperation(string op)
        {
            try
            {
                GeneralMatrix A = ReadMatrix(gridCellsA, rowsA, colsA, "A");
                StringBuilder sb = new StringBuilder();

                if (op == "ALL")
                {
                    sb.AppendLine($"=== A MATRİSİ TAM ANALİZİ ({rowsA}x{colsA}) ===");
                    sb.AppendLine($"  • İz (Trace)              : {Math.Round(A.Trace(), 6)}");
                    sb.AppendLine($"  • 1-Normu (Norm1)         : {Math.Round(A.Norm1(), 5)}");
                    sb.AppendLine($"  • Sonsuz-Normu (NormInf)  : {Math.Round(A.NormInf(), 5)}");
                    sb.AppendLine($"  • Frobenius Normu (NormF) : {Math.Round(A.NormF(), 5)}");

                    try
                    {
                        SingularValueDecomposition svd = A.SVD();
                        sb.AppendLine($"  • 2-Normu (Spektral Norm) : {Math.Round(svd.Norm2(), 5)}");
                        sb.AppendLine($"  • Matris Rankı (Rank)     : {svd.Rank()}");
                        sb.AppendLine($"  • Kondisyon Sayısı (Cond) : {Math.Round(svd.Condition(), 5)}");
                    }
                    catch { }

                    if (rowsA == colsA)
                    {
                        decimal det = A.Determinant();
                        sb.AppendLine($"  • Determinant det(A)      : {Math.Round(det, 6)}");
                    }

                    sb.AppendLine();
                    sb.AppendLine("--- Transpoz (Aᵀ) ---");
                    sb.Append(FormatMatrix(A.Transpose()));

                    sb.AppendLine();
                    sb.AppendLine("--- Negatif Matris (-A / UnaryMinus) ---");
                    sb.Append(FormatMatrix(A.UnaryMinus()));

                    if (rowsA == colsA)
                    {
                        try
                        {
                            sb.AppendLine();
                            sb.AppendLine("--- Ters Matris (A⁻¹) ---");
                            sb.Append(FormatMatrix(A.Inverse()));
                        }
                        catch
                        {
                            sb.AppendLine("  (Matris tekil [singular] olduğu için tersi yoktur.)");
                        }
                    }
                }
                else if (op == "LU")
                {
                    if (rowsA < colsA)
                    {
                        txtResult.Text = "⚠️ LU Ayrışımı için satır sayısı sütun sayısına eşit veya büyük (m >= n) olmalıdır.";
                        return;
                    }
                    LUDecomposition lu = A.LUD();
                    sb.AppendLine("=== LU AYRIŞIMI (P·A = L·U) ===");
                    sb.AppendLine($"Tekil Olmama Durumu (Non-Singular): {(lu.IsNonSingular ? "Evet (Tersinir)" : "Hayır (Tekil)")}");
                    if (rowsA == colsA)
                        sb.AppendLine($"Determinant: {Math.Round(lu.Determinant(), 6)}");
                    sb.AppendLine($"Pivot Vektörü: [ {string.Join(", ", lu.Pivot)} ]");
                    sb.AppendLine();
                    sb.AppendLine("--- Alt Üçgensel Matris (L) ---");
                    sb.Append(FormatMatrix(lu.L));
                    sb.AppendLine();
                    sb.AppendLine("--- Üst Üçgensel Matris (U) ---");
                    sb.Append(FormatMatrix(lu.U));
                }
                else if (op == "QR")
                {
                    if (rowsA < colsA)
                    {
                        txtResult.Text = "⚠️ QR Ayrışımı için satır sayısı sütun sayısına eşit veya büyük (m >= n) olmalıdır.";
                        return;
                    }
                    QRDecomposition qr = A.QRD();
                    sb.AppendLine("=== QR AYRIŞIMI (A = Q·R) ===");
                    sb.AppendLine($"Tam Rank (Full Rank): {(qr.FullRank ? "Evet" : "Hayır")}");
                    sb.AppendLine();
                    sb.AppendLine("--- Ortogonal Matris (Q) ---");
                    sb.Append(FormatMatrix(qr.Q));
                    sb.AppendLine();
                    sb.AppendLine("--- Üst Üçgensel Matris (R) ---");
                    sb.Append(FormatMatrix(qr.R));
                    sb.AppendLine();
                    sb.AppendLine("--- Householder Yansıma Vektörleri Matrisi (H) ---");
                    sb.Append(FormatMatrix(qr.H));
                }
                else if (op == "CHOLESKY")
                {
                    if (rowsA != colsA)
                    {
                        txtResult.Text = "⚠️ Cholesky Ayrışımı yalnızca kare matrisler için uygulanabilir.";
                        return;
                    }
                    CholeskyDecomposition chol = A.chol();
                    sb.AppendLine("=== CHOLESKY AYRIŞIMI (A = L·Lᵀ) ===");
                    sb.AppendLine($"Simetrik Pozitif Tanımlı (SPD): {(chol.SPD ? "Evet ✓" : "Hayır ✕")}");
                    if (!chol.SPD)
                    {
                        sb.AppendLine("⚠️ Uyarı: Bu matris simetrik pozitif tanımlı (SPD) değildir! 'Örnek: SPD Matrisi' butonuna tıklayarak test edebilirsiniz.");
                    }
                    sb.AppendLine();
                    sb.AppendLine("--- Cholesky Alt Üçgensel Çarpanı (L) ---");
                    sb.Append(FormatMatrix(chol.GetL()));
                }
                else if (op == "SVD")
                {
                    SingularValueDecomposition svd = A.SVD();
                    sb.AppendLine("=== TEKİL DEĞER AYRIŞIMI (SVD: A = U·S·Vᵀ) ===");
                    sb.AppendLine($"Matris Rankı (Rank)      : {svd.Rank()}");
                    sb.AppendLine($"2-Normu (Norm2)          : {Math.Round(svd.Norm2(), 6)}");
                    sb.AppendLine($"Kondisyon Sayısı (Cond)  : {Math.Round(svd.Condition(), 6)}");
                    sb.AppendLine();
                    sb.AppendLine("--- Tekil Değerler Matrisi (S) ---");
                    sb.Append(FormatMatrix(svd.S));
                    sb.AppendLine();
                    sb.AppendLine("--- Sol Tekil Vektörler (U) ---");
                    sb.Append(FormatMatrix(svd.GetU()));
                    sb.AppendLine();
                    sb.AppendLine("--- Sağ Tekil Vektörler (V) ---");
                    sb.Append(FormatMatrix(svd.GetV()));
                }
                else if (op == "EIGEN")
                {
                    if (rowsA != colsA)
                    {
                        txtResult.Text = "⚠️ Özdeğer & Özvektör analizi yalnızca kare (N x N) matrisler için yapılabilir.";
                        return;
                    }
                    EigenvalueDecomposition eig = A.Eigen();
                    sb.AppendLine("=== ÖZDEĞER VE ÖZVEKTÖR ANALİZİ (A·v = λ·v) ===");
                    decimal[] realEigs = eig.RealEigenvalues;
                    decimal[] imagEigs = eig.ImagEigenvalues;
                    for (int i = 0; i < realEigs.Length; i++)
                    {
                        decimal re = Math.Round(realEigs[i], 6);
                        decimal im = Math.Round(imagEigs[i], 6);
                        string imStr = DecimalMath.Abs(im) > 0.00001m ? $" {(im >= 0 ? "+" : "-")} {DecimalMath.Abs(im)}i" : "";
                        sb.AppendLine($"  λ_{i + 1} = {re.ToString("0.######", CultureInfo.InvariantCulture)}{imStr}");
                    }
                    sb.AppendLine();
                    sb.AppendLine("--- Blok Diyagonal Özdeğer Matrisi (D) ---");
                    sb.Append(FormatMatrix(eig.D));
                    sb.AppendLine();
                    sb.AppendLine("--- Özvektörler Matrisi (V) ---");
                    sb.Append(FormatMatrix(eig.GetV()));
                }
                else if (op == "INVERSE")
                {
                    if (rowsA != colsA)
                    {
                        txtResult.Text = "⚠️ Ters matris ve determinant yalnızca kare (N x N) matrisler için hesaplanabilir.";
                        return;
                    }
                    decimal det = A.Determinant();
                    sb.AppendLine($"=== TERS MATRİS (A⁻¹) & DETERMİNANT ===");
                    sb.AppendLine($"Determinant det(A) = {Math.Round(det, 6)}");
                    sb.AppendLine();
                    if (DecimalMath.Abs(det) <= 1e-12m)
                    {
                        sb.AppendLine("❌ Matrisin determinantı 0 olduğu için matris tekildir (Singular), tersi alınamaz.");
                    }
                    else
                    {
                        sb.AppendLine("--- Ters Matris (A⁻¹) ---");
                        lastResultMatrix = A.Inverse();
                        sb.Append(FormatMatrix(lastResultMatrix));
                    }
                }

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hesaplama Hatası: " + ex.Message;
            }
        }
    }
}
