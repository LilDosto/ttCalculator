using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetMatrix;

namespace MathApp.Controls
{
    public partial class MatrixCalculatorControl : UserControl
    {
        private int rowsA = 3, colsA = 3;
        private int rowsB = 3, colsB = 3;
        private GeneralMatrix lastResultMatrix = null;

        public MatrixCalculatorControl()
        {
            InitializeComponent();

            // Setup Matrix A events
            btnResizeA.Click += (s, e) => BuildGrid(dgvMatrixA, (int)numRowsA.Value, (int)numColsA.Value, true);
            btnRandomA.Click += (s, e) => FillRandom(dgvMatrixA);
            btnIdentityA.Click += (s, e) => FillIdentity(dgvMatrixA);
            btnZeroA.Click += (s, e) => FillZero(dgvMatrixA);
            btnSpdA.Click += (s, e) => FillSPD(dgvMatrixA);
            btnPasteA.Click += (s, e) => PasteFromClipboard(dgvMatrixA, numRowsA, numColsA);

            // Setup Matrix B events
            btnResizeB.Click += (s, e) => BuildGrid(dgvMatrixB, (int)numRowsB.Value, (int)numColsB.Value, false);
            btnRandomB.Click += (s, e) => FillRandom(dgvMatrixB);
            btnIdentityB.Click += (s, e) => FillIdentity(dgvMatrixB);
            btnZeroB.Click += (s, e) => FillZero(dgvMatrixB);
            btnCopyAToB.Click += (s, e) => CopyMatrixGrid(dgvMatrixA, dgvMatrixB, numRowsB, numColsB);
            btnSwapAB.Click += (s, e) => SwapMatrices();
            btnPasteB.Click += (s, e) => PasteFromClipboard(dgvMatrixB, numRowsB, numColsB);

            // Clipboard shortcuts inside grids
            dgvMatrixA.KeyDown += (s, e) => HandleGridKeyDown(dgvMatrixA, e, numRowsA, numColsA);
            dgvMatrixB.KeyDown += (s, e) => HandleGridKeyDown(dgvMatrixB, e, numRowsB, numColsB);

            // Presets
            btnExSPD.Click += (s, e) => LoadPresetMatrices(
                new decimal[,] { { 4, 12, -16 }, { 12, 37, -43 }, { -16, -43, 98 } },
                new decimal[,] { { 1, 2, 0 }, { 2, 5, 1 }, { 0, 1, 3 } });

            btnExGeneral.Click += (s, e) => LoadPresetMatrices(
                new decimal[,] { { 2, 1, 1 }, { 4, -6, 0 }, { -2, 7, 2 } },
                new decimal[,] { { 1, 0, 2 }, { -1, 3, 1 }, { 2, 1, 4 } });

            btnExOrthogonal.Click += (s, e) => LoadPresetMatrices(
                new decimal[,] { { 0, 1, 0 }, { -1, 0, 0 }, { 0, 0, 1 } },
                new decimal[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } });

            // Results toolbar
            btnAnsToA.Click += (s, e) => TransferResultToGrid(dgvMatrixA, numRowsA, numColsA, "A");
            btnAnsToB.Click += (s, e) => TransferResultToGrid(dgvMatrixB, numRowsB, numColsB, "B");
            btnCopyMatrix.Click += (s, e) => CopyResultText();
            btnCopyLatex.Click += (s, e) => CopyResultLatex();

            // Tab 1 Unary Operations
            btnAllProps.Click += (s, e) => RunUnaryOperation("ALL");
            btnInverse.Click += (s, e) => RunUnaryOperation("INVERSE");
            btnTranspose.Click += (s, e) => RunUnaryOperation("TRANSPOSE");
            btnUnaryMinus.Click += (s, e) => RunUnaryOperation("UNARY_MINUS");
            btnTraceDet.Click += (s, e) => RunUnaryOperation("TRACE_DET");
            btnNorms.Click += (s, e) => RunUnaryOperation("NORMS");
            btnCondRank.Click += (s, e) => RunUnaryOperation("COND_RANK");

            // Tab 2 Decompositions
            btnLU.Click += (s, e) => RunUnaryOperation("LU");
            btnQR.Click += (s, e) => RunUnaryOperation("QR");
            btnCholesky.Click += (s, e) => RunUnaryOperation("CHOLESKY");
            btnSVD.Click += (s, e) => RunUnaryOperation("SVD");
            btnEigen.Click += (s, e) => RunUnaryOperation("EIGEN");

            // Tab 3 Binary & Solvers
            btnAddAB.Click += (s, e) => RunBinaryOperation("ADD");
            btnSubAB.Click += (s, e) => RunBinaryOperation("SUB");
            btnMulAB.Click += (s, e) => RunBinaryOperation("MUL");
            btnScalarMul.Click += (s, e) => RunBinaryOperation("SCALAR");
            btnElemMul.Click += (s, e) => RunBinaryOperation("ELEM_MUL");
            btnElemDiv.Click += (s, e) => RunBinaryOperation("ELEM_DIV");
            btnSolveAXB.Click += (s, e) => RunBinaryOperation("SOLVE");
            btnSolveTrans.Click += (s, e) => RunBinaryOperation("SOLVE_TRANS");

            // Initialize default grids
            BuildGrid(dgvMatrixA, 3, 3, true);
            BuildGrid(dgvMatrixB, 3, 3, false);

            LoadPresetMatrices(
                new decimal[,] { { 4, 12, -16 }, { 12, 37, -43 }, { -16, -43, 98 } },
                new decimal[,] { { 1, 2, 0 }, { 2, 5, 1 }, { 0, 1, 3 } },
                autoRun: false);
        }

        private void BuildGrid(DataGridView dgv, int rows, int cols, bool isMatrixA)
        {
            if (isMatrixA) { rowsA = rows; colsA = cols; }
            else { rowsB = rows; colsB = cols; }

            dgv.SuspendLayout();
            dgv.Columns.Clear();
            dgv.Rows.Clear();

            for (int c = 0; c < cols; c++)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    HeaderText = "C" + (c + 1),
                    Name = "Col" + c,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
                };
                dgv.Columns.Add(col);
            }

            for (int r = 0; r < rows; r++)
            {
                int rowIdx = dgv.Rows.Add();
                dgv.Rows[rowIdx].HeaderCell.Value = "R" + (r + 1);
                for (int c = 0; c < cols; c++)
                {
                    dgv.Rows[rowIdx].Cells[c].Value = (r == c) ? "1" : "0";
                }
            }

            dgv.ResumeLayout();
            if (isMatrixA) UpdateKpisSilent();
        }

        private void FillIdentity(DataGridView dgv)
        {
            for (int r = 0; r < dgv.RowCount; r++)
                for (int c = 0; c < dgv.ColumnCount; c++)
                    dgv.Rows[r].Cells[c].Value = (r == c) ? "1" : "0";

            if (dgv == dgvMatrixA) UpdateKpisSilent();
        }

        private void FillZero(DataGridView dgv)
        {
            for (int r = 0; r < dgv.RowCount; r++)
                for (int c = 0; c < dgv.ColumnCount; c++)
                    dgv.Rows[r].Cells[c].Value = "0";

            if (dgv == dgvMatrixA) UpdateKpisSilent();
        }

        private void FillRandom(DataGridView dgv)
        {
            Random rnd = new Random();
            for (int r = 0; r < dgv.RowCount; r++)
                for (int c = 0; c < dgv.ColumnCount; c++)
                    dgv.Rows[r].Cells[c].Value = rnd.Next(-9, 10).ToString(CultureInfo.InvariantCulture);

            if (dgv == dgvMatrixA) UpdateKpisSilent();
        }

        private void FillSPD(DataGridView dgv)
        {
            int n = Math.Min(dgv.RowCount, dgv.ColumnCount);
            if (dgv.RowCount != dgv.ColumnCount)
            {
                MessageBox.Show("Simetrik Pozitif Tanımlı (SPD) matris yalnızca kare matrisler için oluşturulabilir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Random rand = new Random();
            decimal[][] R = new decimal[n][];
            for (int i = 0; i < n; i++)
            {
                R[i] = new decimal[n];
                for (int j = 0; j < n; j++)
                {
                    R[i][j] = rand.Next(-3, 4);
                }
            }

            // A = R^T * R + n * I
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    decimal sum = 0m;
                    for (int k = 0; k < n; k++)
                    {
                        sum += R[k][i] * R[k][j];
                    }
                    if (i == j) sum += (decimal)n + 1m;
                    dgv.Rows[i].Cells[j].Value = sum.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (dgv == dgvMatrixA) UpdateKpisSilent();
        }

        private void CopyMatrixGrid(DataGridView source, DataGridView target, NumericUpDown numR, NumericUpDown numC)
        {
            numR.Value = source.RowCount;
            numC.Value = source.ColumnCount;
            BuildGrid(target, source.RowCount, source.ColumnCount, target == dgvMatrixA);

            for (int r = 0; r < source.RowCount; r++)
            {
                for (int c = 0; c < source.ColumnCount; c++)
                {
                    target.Rows[r].Cells[c].Value = source.Rows[r].Cells[c].Value;
                }
            }
        }

        private void SwapMatrices()
        {
            int rA = dgvMatrixA.RowCount;
            int cA = dgvMatrixA.ColumnCount;
            int rB = dgvMatrixB.RowCount;
            int cB = dgvMatrixB.ColumnCount;

            string[,] dataA = new string[rA, cA];
            for (int i = 0; i < rA; i++)
                for (int j = 0; j < cA; j++)
                    dataA[i, j] = dgvMatrixA.Rows[i].Cells[j].Value?.ToString() ?? "0";

            string[,] dataB = new string[rB, cB];
            for (int i = 0; i < rB; i++)
                for (int j = 0; j < cB; j++)
                    dataB[i, j] = dgvMatrixB.Rows[i].Cells[j].Value?.ToString() ?? "0";

            numRowsA.Value = rB;
            numColsA.Value = cB;
            BuildGrid(dgvMatrixA, rB, cB, true);
            for (int i = 0; i < rB; i++)
                for (int j = 0; j < cB; j++)
                    dgvMatrixA.Rows[i].Cells[j].Value = dataB[i, j];

            numRowsB.Value = rA;
            numColsB.Value = cA;
            BuildGrid(dgvMatrixB, rA, cA, false);
            for (int i = 0; i < rA; i++)
                for (int j = 0; j < cA; j++)
                    dgvMatrixB.Rows[i].Cells[j].Value = dataA[i, j];

            UpdateKpisSilent();
        }

        private void HandleGridKeyDown(DataGridView dgv, KeyEventArgs e, NumericUpDown numR, NumericUpDown numC)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteFromClipboard(dgv, numR, numC);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopyGridToClipboard(dgv);
                e.Handled = true;
            }
        }

        private void PasteFromClipboard(DataGridView dgv, NumericUpDown numR, NumericUpDown numC)
        {
            try
            {
                string text = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(text)) return;

                string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0) return;

                int neededRows = Math.Min(8, lines.Length);
                int neededCols = 1;
                foreach (var line in lines)
                {
                    string[] tokens = line.Split(new[] { '\t', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length == 1 && line.Contains(' '))
                        tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length > neededCols) neededCols = Math.Min(8, tokens.Length);
                }

                if (neededRows > dgv.RowCount || neededCols > dgv.ColumnCount)
                {
                    int finalR = Math.Max(dgv.RowCount, neededRows);
                    int finalC = Math.Max(dgv.ColumnCount, neededCols);
                    numR.Value = finalR;
                    numC.Value = finalC;
                    BuildGrid(dgv, finalR, finalC, dgv == dgvMatrixA);
                }

                int startRow = dgv.CurrentCell?.RowIndex ?? 0;
                int startCol = dgv.CurrentCell?.ColumnIndex ?? 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    int tr = startRow + i;
                    if (tr >= dgv.RowCount) break;

                    string[] parts = lines[i].Split(new[] { '\t', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 1 && lines[i].Contains(' '))
                        parts = lines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < parts.Length; j++)
                    {
                        int tc = startCol + j;
                        if (tc >= dgv.ColumnCount) break;
                        string clean = parts[j].Trim().Replace(',', '.');
                        if (decimal.TryParse(clean, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                        {
                            dgv.Rows[tr].Cells[tc].Value = val.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }

                if (dgv == dgvMatrixA) UpdateKpisSilent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Panodan yapıştırma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CopyGridToClipboard(DataGridView dgv)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int r = 0; r < dgv.RowCount; r++)
                {
                    List<string> rowVals = new List<string>();
                    for (int c = 0; c < dgv.ColumnCount; c++)
                    {
                        rowVals.Add(dgv.Rows[r].Cells[c].Value?.ToString() ?? "0");
                    }
                    sb.AppendLine(string.Join("\t", rowVals));
                }
                Clipboard.SetText(sb.ToString());
            }
            catch { }
        }

        private void LoadPresetMatrices(decimal[,] dataA, decimal[,] dataB, bool autoRun = true)
        {
            numRowsA.Value = dataA.GetLength(0);
            numColsA.Value = dataA.GetLength(1);
            BuildGrid(dgvMatrixA, dataA.GetLength(0), dataA.GetLength(1), true);
            for (int i = 0; i < rowsA; i++)
                for (int j = 0; j < colsA; j++)
                    dgvMatrixA.Rows[i].Cells[j].Value = dataA[i, j].ToString(CultureInfo.InvariantCulture);

            numRowsB.Value = dataB.GetLength(0);
            numColsB.Value = dataB.GetLength(1);
            BuildGrid(dgvMatrixB, dataB.GetLength(0), dataB.GetLength(1), false);
            for (int i = 0; i < rowsB; i++)
                for (int j = 0; j < colsB; j++)
                    dgvMatrixB.Rows[i].Cells[j].Value = dataB[i, j].ToString(CultureInfo.InvariantCulture);

            UpdateKpisSilent();
            if (autoRun) RunUnaryOperation("ALL");
        }

        private GeneralMatrix ReadMatrixFromGrid(DataGridView dgv, string name)
        {
            int r = dgv.RowCount;
            int c = dgv.ColumnCount;
            decimal[][] arr = new decimal[r][];
            for (int i = 0; i < r; i++)
            {
                arr[i] = new decimal[c];
                for (int j = 0; j < c; j++)
                {
                    string raw = dgv.Rows[i].Cells[j].Value?.ToString()?.Trim() ?? "0";
                    raw = raw.Replace(',', '.');
                    if (!decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                    {
                        throw new ArgumentException($"{name} matrisinin ({i + 1}, {j + 1}) hücresinde geçersiz sayı: '{raw}'");
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

        private string MatrixToLatex(GeneralMatrix m)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"\begin{bmatrix}");
            for (int i = 0; i < m.RowDimension; i++)
            {
                List<string> rowVals = new List<string>();
                for (int j = 0; j < m.ColumnDimension; j++)
                {
                    decimal v = Math.Round(m.GetElement(i, j), 5);
                    if (DecimalMath.Abs(v) < 0.000001m) v = 0m;
                    rowVals.Add(v.ToString("0.#####", CultureInfo.InvariantCulture));
                }
                sb.Append("  " + string.Join(" & ", rowVals));
                if (i < m.RowDimension - 1) sb.AppendLine(@" \\");
                else sb.AppendLine();
            }
            sb.AppendLine(@"\end{bmatrix}");
            return sb.ToString();
        }

        private void UpdateKpisSilent()
        {
            try
            {
                GeneralMatrix A = ReadMatrixFromGrid(dgvMatrixA, "A");
                if (A.RowDimension == A.ColumnDimension)
                {
                    decimal det = Math.Round(A.Determinant(), 4);
                    lblKpiDetVal.Text = det.ToString(CultureInfo.InvariantCulture);
                    lblKpiDetVal.ForeColor = DecimalMath.Abs(det) < 1e-10m ? Color.FromArgb(239, 68, 68) : Color.FromArgb(56, 189, 248);
                }
                else
                {
                    lblKpiDetVal.Text = "(Kare Değil)";
                    lblKpiDetVal.ForeColor = Color.FromArgb(148, 163, 184);
                }

                SingularValueDecomposition svd = A.SVD();
                lblKpiRankVal.Text = svd.Rank().ToString();
                decimal cond = Math.Round(svd.Condition(), 2);
                lblKpiCondVal.Text = cond > 1e6m ? "> 10⁶ (İll-cond)" : cond.ToString(CultureInfo.InvariantCulture);
                lblKpiCondVal.ForeColor = cond > 1e4m ? Color.FromArgb(245, 158, 11) : Color.FromArgb(52, 211, 153);
            }
            catch
            {
                lblKpiDetVal.Text = "—";
                lblKpiRankVal.Text = "—";
                lblKpiCondVal.Text = "—";
            }
        }

        private void TransferResultToGrid(DataGridView targetDgv, NumericUpDown numR, NumericUpDown numC, string targetName)
        {
            if (lastResultMatrix == null)
            {
                MessageBox.Show("Aktarılacak matris sonucu bulunamadı. Önce bir işlem yapınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int r = Math.Min(8, lastResultMatrix.RowDimension);
            int c = Math.Min(8, lastResultMatrix.ColumnDimension);

            numR.Value = r;
            numC.Value = c;
            BuildGrid(targetDgv, r, c, targetDgv == dgvMatrixA);

            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    targetDgv.Rows[i].Cells[j].Value = Math.Round(lastResultMatrix.GetElement(i, j), 6).ToString(CultureInfo.InvariantCulture);

            if (targetDgv == dgvMatrixA) UpdateKpisSilent();

            MessageBox.Show($"Sonuç matrisi ({r}×{c}) başarıyla Matris {targetName}'ye aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CopyResultText()
        {
            if (!string.IsNullOrEmpty(txtResult.Text))
            {
                Clipboard.SetText(txtResult.Text);
                MessageBox.Show("Matris çıktısı panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CopyResultLatex()
        {
            try
            {
                GeneralMatrix m = lastResultMatrix ?? ReadMatrixFromGrid(dgvMatrixA, "A");
                string latex = MatrixToLatex(m);
                Clipboard.SetText(latex);
                MessageBox.Show("Matris LaTeX formatında (bmatrix) panoya kopyalandı:\n\n" + latex, "LaTeX Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LaTeX kopyalama hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void RunUnaryOperation(string op)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                GeneralMatrix A = ReadMatrixFromGrid(dgvMatrixA, "A");
                int rA = A.RowDimension;
                int cA = A.ColumnDimension;

                if ((op == "LU" || op == "QR") && rA < cA)
                {
                    txtResult.Text = "⚠️ LU ve QR Ayrışımları için satır sayısı sütun sayısından küçük olamaz (m >= n).";
                    return;
                }
                if (op == "CHOLESKY" && rA != cA)
                {
                    txtResult.Text = "⚠️ Cholesky Ayrışımı (A = L·Lᵀ) yalnızca kare (N × N) matrislere uygulanabilir.";
                    return;
                }
                if (op == "EIGEN" && rA != cA)
                {
                    txtResult.Text = "⚠️ Özdeğer & Özvektör analizi (A·v = λ·v) yalnızca kare matrisler için tanımlıdır.";
                    return;
                }
                if (op == "INVERSE" && rA != cA)
                {
                    txtResult.Text = "⚠️ Ters matris (A⁻¹) ve determinant yalnızca kare (N × N) matrisler için hesaplanabilir.";
                    return;
                }

                var (resultMatrix, outputText) = await Task.Run(() =>
                {
                    StringBuilder sb = new StringBuilder();
                    GeneralMatrix res = null;

                    if (op == "ALL")
                    {
                        sb.AppendLine($"=================================================================");
                        sb.AppendLine($"           A MATRİSİ KAPSAMLI ANALİZİ  [{rA} × {cA}]");
                        sb.AppendLine($"=================================================================");
                        sb.AppendLine($"  • İz (Trace)                   : {Math.Round(A.Trace(), 6)}");
                        sb.AppendLine($"  • 1-Normu (Maksimum Sütun Top) : {Math.Round(A.Norm1(), 5)}");
                        sb.AppendLine($"  • ∞-Normu (Maksimum Satır Top) : {Math.Round(A.NormInf(), 5)}");
                        sb.AppendLine($"  • Frobenius Normu (||A||_F)    : {Math.Round(A.NormF(), 5)}");

                        try
                        {
                            SingularValueDecomposition svd = A.SVD();
                            sb.AppendLine($"  • 2-Normu (Spektral Norm)      : {Math.Round(svd.Norm2(), 5)}");
                            sb.AppendLine($"  • Matris Rankı (Rank)          : {svd.Rank()}");
                            sb.AppendLine($"  • Kondisyon Sayısı κ(A)        : {Math.Round(svd.Condition(), 5)}");
                        }
                        catch { }

                        if (rA == cA)
                        {
                            decimal det = A.Determinant();
                            sb.AppendLine($"  • Determinant det(A)           : {Math.Round(det, 6)}");
                        }

                        sb.AppendLine();
                        sb.AppendLine("--- Transpoz (Aᵀ) ---");
                        GeneralMatrix trans = A.Transpose();
                        sb.Append(FormatMatrix(trans));

                        sb.AppendLine();
                        sb.AppendLine("--- Negatif Matris (-A) ---");
                        sb.Append(FormatMatrix(A.UnaryMinus()));

                        if (rA == cA)
                        {
                            sb.AppendLine();
                            sb.AppendLine("--- Ters Matris (A⁻¹) ---");
                            decimal det = A.Determinant();
                            if (DecimalMath.Abs(det) <= 1e-12m)
                            {
                                sb.AppendLine("  ⚠️ UYARI: Matris tekildir (det(A) ≈ 0). Tersi yoktur veya sayısal olarak tekildir.");
                            }
                            else
                            {
                                res = A.Inverse();
                                sb.Append(FormatMatrix(res));
                            }
                        }
                    }
                    else if (op == "INVERSE")
                    {
                        decimal det = A.Determinant();
                        sb.AppendLine($"=== TERS MATRİS (A⁻¹) & DETERMİNANT [{rA} × {cA}] ===");
                        sb.AppendLine($"Determinant det(A) = {Math.Round(det, 6)}");
                        sb.AppendLine();
                        if (DecimalMath.Abs(det) <= 1e-12m)
                        {
                            sb.AppendLine("❌ UYARI: Matris tekildir (Singular, det(A) ≈ 0). Ters matris hesaplanamaz!");
                        }
                        else
                        {
                            res = A.Inverse();
                            sb.AppendLine("--- Ters Matris (A⁻¹) ---");
                            sb.Append(FormatMatrix(res));
                            sb.AppendLine();
                            sb.AppendLine("Doğrulama: A · A⁻¹ ≈ I");
                            sb.Append(FormatMatrix(A * res));
                        }
                    }
                    else if (op == "TRANSPOSE")
                    {
                        res = A.Transpose();
                        sb.AppendLine($"=== TRANSPOZ MATRİS (Aᵀ) [{res.RowDimension} × {res.ColumnDimension}] ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "UNARY_MINUS")
                    {
                        res = A.UnaryMinus();
                        sb.AppendLine($"=== NEGATİF MATRİS (-A) [{rA} × {cA}] ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "TRACE_DET")
                    {
                        sb.AppendLine($"=== İZ (TRACE) & DETERMİNANT [{rA} × {cA}] ===");
                        sb.AppendLine($"  • Matris İzi tr(A) = ∑ A[i,i] = {Math.Round(A.Trace(), 6)}");
                        if (rA == cA)
                        {
                            decimal det = A.Determinant();
                            sb.AppendLine($"  • Determinant det(A)          = {Math.Round(det, 6)}");
                            sb.AppendLine($"  • Tersinirlik Durumu          = {(DecimalMath.Abs(det) > 1e-12m ? "Tersinir (Non-singular) ✓" : "Tekil (Singular) ✕")}");
                        }
                        else
                        {
                            sb.AppendLine("  (Determinant yalnızca kare matrisler için tanımlıdır.)");
                        }
                    }
                    else if (op == "NORMS")
                    {
                        sb.AppendLine($"=== MATRİS NORMLARI [{rA} × {cA}] ===");
                        sb.AppendLine($"  • 1-Normu (Maks. Mutlak Sütun Toplamı) : {Math.Round(A.Norm1(), 6)}");
                        sb.AppendLine($"  • ∞-Normu (Maks. Mutlak Satır Toplamı) : {Math.Round(A.NormInf(), 6)}");
                        sb.AppendLine($"  • Frobenius Normu (Öklid / ||A||_F)    : {Math.Round(A.NormF(), 6)}");
                        try
                        {
                            SingularValueDecomposition svd = A.SVD();
                            sb.AppendLine($"  • 2-Normu (Spektral / Maks. Tekil Değer): {Math.Round(svd.Norm2(), 6)}");
                        }
                        catch { }
                    }
                    else if (op == "COND_RANK")
                    {
                        SingularValueDecomposition svd = A.SVD();
                        sb.AppendLine($"=== KONDİSYON SAYISI & RANK [{rA} × {cA}] ===");
                        sb.AppendLine($"  • Matris Rankı (Rank)       : {svd.Rank()} / min({rA},{cA})");
                        sb.AppendLine($"  • Spektral Kondisyon κ(A)   : {Math.Round(svd.Condition(), 6)}");
                        sb.AppendLine();
                        if (svd.Condition() > 1e6m)
                            sb.AppendLine("  ⚠️ Uyarı: Kondisyon sayısı çok yüksek! Sistem sayısal olarak kötü koşulludur (Ill-conditioned).");
                        else
                            sb.AppendLine("  ✓ Sistem sayısal olarak iyi koşulludur (Well-conditioned).");
                    }
                    else if (op == "LU")
                    {
                        LUDecomposition lu = A.LUD();
                        sb.AppendLine("=== LU AYRIŞIMI (P·A = L·U) ===");
                        sb.AppendLine($"Tersinirlik: {(lu.IsNonSingular ? "Tersinir (Non-Singular) ✓" : "Tekil (Singular) ✕")}");
                        if (rA == cA)
                            sb.AppendLine($"Determinant: {Math.Round(lu.Determinant(), 6)}");
                        sb.AppendLine($"Pivot Permütasyonu: [ {string.Join(", ", lu.Pivot)} ]");
                        sb.AppendLine();
                        sb.AppendLine("--- Alt Üçgensel Matris (L) ---");
                        sb.Append(FormatMatrix(lu.L));
                        sb.AppendLine();
                        sb.AppendLine("--- Üst Üçgensel Matris (U) ---");
                        sb.Append(FormatMatrix(lu.U));
                        res = lu.U;
                    }
                    else if (op == "QR")
                    {
                        QRDecomposition qr = A.QRD();
                        sb.AppendLine("=== QR AYRIŞIMI (Householder Ortogonalizasyonu: A = Q·R) ===");
                        sb.AppendLine($"Tam Rank Durumu (Full Rank): {(qr.FullRank ? "Evet ✓" : "Hayır ✕")}");
                        sb.AppendLine();
                        sb.AppendLine("--- Ortogonal Matris (Q) [Qᵀ · Q = I] ---");
                        sb.Append(FormatMatrix(qr.Q));
                        sb.AppendLine();
                        sb.AppendLine("--- Üst Üçgensel Matris (R) ---");
                        sb.Append(FormatMatrix(qr.R));
                        res = qr.Q;
                    }
                    else if (op == "CHOLESKY")
                    {
                        CholeskyDecomposition chol = A.chol();
                        sb.AppendLine("=== CHOLESKY AYRIŞIMI (A = L·Lᵀ) ===");
                        sb.AppendLine($"Simetrik Pozitif Tanımlı (SPD): {(chol.SPD ? "Evet ✓ (A = L·Lᵀ geçerli)" : "Hayır ✕")}");
                        if (!chol.SPD)
                        {
                            sb.AppendLine();
                            sb.AppendLine("⚠️ UYARI: Bu matris Simetrik Pozitif Tanımlı (SPD) değildir!");
                            sb.AppendLine("  • Cholesky için A = Aᵀ olmalı ve tüm özdeğerler > 0 olmalıdır.");
                            sb.AppendLine("  • 'SPD' butonuna veya 'Örnek SPD'ye tıklayarak geçerli bir test matrisi oluşturabilirsiniz.");
                        }
                        sb.AppendLine();
                        sb.AppendLine("--- Cholesky Alt Üçgensel Çarpanı (L) ---");
                        res = chol.GetL();
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "SVD")
                    {
                        SingularValueDecomposition svd = A.SVD();
                        sb.AppendLine("=== TEKİL DEĞER AYRIŞIMI (SVD: A = U · Σ · Vᵀ) ===");
                        sb.AppendLine($"  • Matris Rankı (Rank)      : {svd.Rank()}");
                        sb.AppendLine($"  • 2-Normu (Spektral Norm)  : {Math.Round(svd.Norm2(), 6)}");
                        sb.AppendLine($"  • Kondisyon Sayısı κ(A)    : {Math.Round(svd.Condition(), 6)}");
                        sb.AppendLine();
                        sb.AppendLine("--- Tekil Değerler Köşegen Matrisi (Σ) ---");
                        sb.Append(FormatMatrix(svd.S));
                        sb.AppendLine();
                        sb.AppendLine("--- Sol Tekil Vektörler Matrisi (U) ---");
                        sb.Append(FormatMatrix(svd.GetU()));
                        sb.AppendLine();
                        sb.AppendLine("--- Sağ Tekil Vektörler Matrisi (V) ---");
                        sb.Append(FormatMatrix(svd.GetV()));
                        res = svd.S;
                    }
                    else if (op == "EIGEN")
                    {
                        EigenvalueDecomposition eig = A.Eigen();
                        sb.AppendLine("=== ÖZDEĞER VE ÖZVEKTÖR ANALİZİ (A · v = λ · v) ===");
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
                        res = eig.GetV();
                    }

                    return (res, sb.ToString());
                });

                if (resultMatrix != null)
                {
                    lastResultMatrix = resultMatrix;
                }
                txtResult.Text = outputText;
                UpdateKpisSilent();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Analiz Hatası: " + ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void RunBinaryOperation(string op)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                GeneralMatrix A = ReadMatrixFromGrid(dgvMatrixA, "A");
                GeneralMatrix B = null;
                decimal k = 0m;

                if (op == "SCALAR")
                {
                    string sRaw = txtScalar.Text.Trim().Replace(',', '.');
                    if (!decimal.TryParse(sRaw, NumberStyles.Any, CultureInfo.InvariantCulture, out k))
                    {
                        txtResult.Text = "❌ Geçerli bir skaler çarpan (k) giriniz.";
                        return;
                    }
                }
                else
                {
                    B = ReadMatrixFromGrid(dgvMatrixB, "B");
                }

                // Dimension checks
                if (op == "ADD" || op == "SUB" || op == "ELEM_MUL" || op == "ELEM_DIV")
                {
                    if (A.RowDimension != B.RowDimension || A.ColumnDimension != B.ColumnDimension)
                    {
                        txtResult.Text = $"⚠️ Boyut Uyuşmazlığı: Bu işlem için A [{A.RowDimension}×{A.ColumnDimension}] ve B [{B.RowDimension}×{B.ColumnDimension}] matrislerinin boyutları birebir eşit olmalıdır.";
                        return;
                    }
                }
                else if (op == "MUL")
                {
                    if (A.ColumnDimension != B.RowDimension)
                    {
                        txtResult.Text = $"⚠️ Çarpım Boyut Uyuşmazlığı: A'nın sütun sayısı ({A.ColumnDimension}), B'nin satır sayısına ({B.RowDimension}) eşit olmalıdır.";
                        return;
                    }
                }
                else if (op == "SOLVE")
                {
                    if (A.RowDimension != B.RowDimension)
                    {
                        txtResult.Text = $"⚠️ Lineer Sistem Boyut Hatası: A·X = B denkleminde A'nın satır sayısı ({A.RowDimension}), B'nin satır sayısına ({B.RowDimension}) eşit olmalıdır.";
                        return;
                    }
                }

                var (resultMatrix, outputText) = await Task.Run(() =>
                {
                    StringBuilder sb = new StringBuilder();
                    GeneralMatrix res = null;

                    if (op == "SCALAR")
                    {
                        res = A.Multiply(k);
                        sb.AppendLine($"=== SKALER ÇARPIM (k · A,  k = {k}) ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "ADD")
                    {
                        res = A + B;
                        sb.AppendLine($"=== MATRİS TOPLAMI (A + B) [{res.RowDimension} × {res.ColumnDimension}] ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "SUB")
                    {
                        res = A - B;
                        sb.AppendLine($"=== MATRİS FARKI (A - B) [{res.RowDimension} × {res.ColumnDimension}] ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "MUL")
                    {
                        res = A * B;
                        sb.AppendLine($"=== MATRİS ÇARPIMI (A × B) [{A.RowDimension}×{A.ColumnDimension}] · [{B.RowDimension}×{B.ColumnDimension}] => [{res.RowDimension}×{res.ColumnDimension}] ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "ELEM_MUL")
                    {
                        res = A.ArrayMultiply(B);
                        sb.AppendLine("=== HADAMARD NOKTASAL ÇARPIM (A .* B) ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "ELEM_DIV")
                    {
                        res = A.ArrayRightDivide(B);
                        sb.AppendLine("=== NOKTASAL ELEMAN BÖLME (A ./ B) ===");
                        sb.Append(FormatMatrix(res));
                    }
                    else if (op == "SOLVE")
                    {
                        res = A.Solve(B);
                        sb.AppendLine("=== LİNEER SİSTEM ÇÖZÜCÜ (A · X = B  =>  X) ===");
                        sb.Append(FormatMatrix(res));
                        sb.AppendLine();
                        sb.AppendLine("Doğrulama: A · X - B ≈ 0 (Kalıntı)");
                        GeneralMatrix residual = (A * res) - B;
                        sb.Append(FormatMatrix(residual));
                    }
                    else if (op == "SOLVE_TRANS")
                    {
                        res = A.SolveTranspose(B);
                        sb.AppendLine("=== TRANSPOZ SİSTEM ÇÖZÜCÜ (Aᵀ · Xᵀ = Bᵀ) ===");
                        sb.Append(FormatMatrix(res));
                    }

                    return (res, sb.ToString());
                });

                if (resultMatrix != null)
                {
                    lastResultMatrix = resultMatrix;
                }
                txtResult.Text = outputText;
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ İşlem Hatası: " + ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
