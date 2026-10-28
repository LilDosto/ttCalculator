using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class DerivativeCalculatorControl : UserControl
    {
        private List<VariableInputRow> partialRows = new List<VariableInputRow>();

        // Saved state for Graph Plotter navigation
        private string _lastSingleFn = "";
        private string _lastTangentEq = "";
        private float _lastX0 = 0f;
        private float _lastY0 = 0f;

        public DerivativeCalculatorControl()
        {
            InitializeComponent();
            ThemeHelper.StyleModernTabControl(tabResults);

            btnH1.Click += (s, e) => txtStepH.Text = "0.001";
            btnH2.Click += (s, e) => txtStepH.Text = "0.00001";
            btnH3.Click += (s, e) => txtStepH.Text = "0.0000001";

            btnCalcSingle.Click += BtnCalcSingle_Click;
            btnPlotTangent.Click += BtnPlotTangent_Click;
            btnAddVar.Click += BtnAddVar_Click;
            btnCalcPartial.Click += BtnCalcPartial_Click;
            btnCalcHessian.Click += BtnCalcHessian_Click;

            btnCopyLog.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("Türev raporu panoya kopyalandı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnExSingle1.Click += (s, e) =>
            {
                txtSingleFn.Text = "sin(x) + x^3";
                txtSingleVar.Text = "x";
                txtSinglePoint.Text = "1";
                numSingleDegree.Value = 1;
                BtnCalcSingle_Click(this, EventArgs.Empty);
            };
            btnExSingle2.Click += (s, e) =>
            {
                txtSingleFn.Text = "exp(2*x)";
                txtSingleVar.Text = "x";
                txtSinglePoint.Text = "0";
                numSingleDegree.Value = 2;
                BtnCalcSingle_Click(this, EventArgs.Empty);
            };

            btnExPartial1.Click += (s, e) => LoadPartialPreset("x^2*y + sin(y)", "x", new[] { ("x", "2"), ("y", "1") });
            btnExPartial2.Click += (s, e) => LoadPartialPreset("exp(x*y) + x^3*y^2", "x", new[] { ("x", "1"), ("y", "1") });

            AddPartialRow("x", "2");
            AddPartialRow("y", "1");
        }

        private void LoadPartialPreset(string eq, string targetVar, (string v, string val)[] vars)
        {
            txtPartialFn.Text = eq;
            txtTargetVar.Text = targetVar;
            pnlPartialVars.Controls.Clear();
            partialRows.Clear();
            foreach (var item in vars)
            {
                AddPartialRow(item.v, item.val);
            }
            BtnCalcPartial_Click(this, EventArgs.Empty);
        }

        private void BtnAddVar_Click(object sender, EventArgs e)
        {
            string[] names = { "x", "y", "z", "w", "u", "v" };
            string suggested = partialRows.Count < names.Length ? names[partialRows.Count] : "v" + (partialRows.Count + 1);
            AddPartialRow(suggested, "1");
        }

        private void AddPartialRow(string varName, string val)
        {
            VariableInputRow row = new VariableInputRow(varName, val);
            row.Panel.Width = 390;
            row.OnRemove += (s, e) =>
            {
                pnlPartialVars.Controls.Remove(row.Panel);
                partialRows.Remove(row);
            };
            pnlPartialVars.Controls.Add(row.Panel);
            partialRows.Add(row);
        }

        private decimal ParseDec(string s, string field, decimal defVal = 0m)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (string.IsNullOrWhiteSpace(norm)) return defVal;
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal v))
                return v;
            throw new ArgumentException($"'{field}' için geçerli bir sayı giriniz.");
        }

        #region 1. Single Variable Differentiation

        private async void BtnCalcSingle_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalcSingle.Text;
            btnCalcSingle.Enabled = false;
            btnCalcSingle.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtSingleFn.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    MessageBox.Show("Lütfen bir fonksiyon ifadesi giriniz (Örn: sin(x) + x^3).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _lastSingleFn = eq;
                string v = string.IsNullOrWhiteSpace(txtSingleVar.Text) ? "x" : txtSingleVar.Text.Trim();
                decimal pt = ParseDec(txtSinglePoint.Text, "Nokta (x₀)", 1m);
                decimal h = ParseDec(txtStepH.Text, "Adım Boyutu (h)", 0.001m);
                if (h <= 0m) h = 0.001m;

                int deg = (int)numSingleDegree.Value;
                int acc = (cmbAccuracy.SelectedIndex + 1) * 2; // 2, 4, 6
                int formulaIdx = cmbFormula.SelectedIndex; // 0: Central, 1: Forward, 2: Backward

                _lastX0 = (float)pt;

                await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fVal = NumericalMethods.Method_EvaluateFunction(eq, v, pt);
                        _lastY0 = (float)fVal;

                        decimal dVal;
                        string methodUsed;
                        if (formulaIdx == 1)
                        {
                            dVal = NumericalMethods.Method_ForwardBinomialDerivative(eq, deg, v, pt, h);
                            methodUsed = "İleri Binom Sonlu Farklar Formülü";
                        }
                        else if (formulaIdx == 2)
                        {
                            dVal = NumericalMethods.Method_BackwardDifferenceDerivative(eq, deg, v, pt, h);
                            methodUsed = "Geri Sonlu Farklar Formülü";
                        }
                        else
                        {
                            dVal = NumericalMethods.Method_NumericalDifferentiation(eq, deg, v, pt, acc, h);
                            methodUsed = $"Merkezi Sonlu Farklar Matrisi O(h^{acc})";
                        }

                        // Calculate First Derivative for Tangent Line
                        decimal mSlope = (deg == 1) ? dVal : NumericalMethods.Method_NumericalDifferentiation(eq, 1, v, pt, acc, h);
                        decimal cIntercept = fVal - mSlope * pt;
                        _lastTangentEq = $"{mSlope:F4}*x + {cIntercept:F4}";

                        // Update UI
                        this.Invoke((Action)(() =>
                        {
                            lblKpiTitle.Text = $"TEK DEĞİŞKENLİ TÜREV ANALİZİ [{deg}. Mertebe]";
                            lblKpiValue.Text = $"f^({deg})({pt}) = {dVal:F8}";
                            lblKpiSubtitle.Text = $"Teğet Doğrusu: y = {_lastTangentEq}  •  f({pt}) = {fVal:F6}  •  Eğim: {mSlope:F6}";

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("=======================================================================");
                            sb.AppendLine($"  TEK DEĞİŞKENLİ NÜMERİK TÜREV VE TEĞET ANALİZİ ({deg}. Derece)");
                            sb.AppendLine("=======================================================================");
                            sb.AppendLine($"  Fonksiyon           : f({v}) = {eq}");
                            sb.AppendLine($"  Değerlendirme Noktası : {v}₀ = {pt}  =>  f({pt}) = {fVal:G18}");
                            sb.AppendLine($"  Seçilen Yöntem      : {methodUsed}");
                            sb.AppendLine($"  Adım Boyutu (h)     : {h}   |   Hassasiyet Derecesi: O(h^{acc})");
                            sb.AppendLine();
                            sb.AppendLine($"  1. HESAPLANAN {deg}. MERTEBE TÜREV:");
                            sb.AppendLine($"     • f^({deg})({pt}) = {dVal:G18}");
                            sb.AppendLine();
                            sb.AppendLine("  2. TEĞET VE NORMAL DOĞRU DENKLEMLERİ:");
                            sb.AppendLine($"     • Teğet Eğimi (m)  : m = f'({pt}) = {mSlope:F6}");
                            sb.AppendLine($"     • Teğet Doğrusu    : y - y₀ = m(x - x₀)  =>  y = {_lastTangentEq}");
                            if (Math.Abs(mSlope) > 0.0001m)
                            {
                                decimal nSlope = -1m / mSlope;
                                decimal nIntercept = fVal - nSlope * pt;
                                sb.AppendLine($"     • Normal Doğrusu   : y - y₀ = (-1/m)(x - x₀)  =>  y = {nSlope:F4}*x + {nIntercept:F4}");
                            }
                            sb.AppendLine();
                            sb.AppendLine("  3. YÖNTEMLERİN KARŞILAŞTIRMASI:");
                            decimal cMat = NumericalMethods.Method_NumericalDifferentiation(eq, deg, v, pt, acc, h);
                            decimal fBin = NumericalMethods.Method_ForwardBinomialDerivative(eq, deg, v, pt, h);
                            decimal bDiff = NumericalMethods.Method_BackwardDifferenceDerivative(eq, deg, v, pt, h);
                            sb.AppendLine($"     • Merkezi Farklar (Central) : {cMat:G16}");
                            sb.AppendLine($"     • İleri Farklar (Forward)   : {fBin:G16}");
                            sb.AppendLine($"     • Geri Farklar (Backward)   : {bDiff:G16}");
                            sb.AppendLine();
                            sb.AppendLine("  4. TÜM MERTEBELER ÖZETİ (1'den 4'e kadar):");
                            for (int k = 1; k <= 4; k++)
                            {
                                decimal dk = NumericalMethods.Method_NumericalDifferentiation(eq, k, v, pt, acc, (k >= 3 ? Math.Max(h, 0.005m) : h));
                                sb.AppendLine($"     • f^({k})({pt}) = {dk:G14}");
                            }

                            txtSummaryView.Text = sb.ToString();
                            txtResult.Text = sb.ToString();
                            tabResults.SelectedTab = tabSummary;
                        }));
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Türev hesaplama hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCalcSingle.Enabled = true;
                btnCalcSingle.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void BtnPlotTangent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastSingleFn))
            {
                MessageBox.Show("Önce tek değişkenli türev hesabı yapınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MainForm.Instance?.NavigateToGraph(_lastSingleFn, _lastX0, _lastY0);
            if (!string.IsNullOrWhiteSpace(_lastTangentEq))
            {
                MainForm.Instance?.NavigateToGraph(_lastTangentEq);
            }
        }

        #endregion

        #region 2. Multivariable Analysis, Gradient & Directional Derivative

        private async void BtnCalcPartial_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalcPartial.Text;
            btnCalcPartial.Enabled = false;
            btnCalcPartial.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtPartialFn.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq)) return;

                string targetVar = txtTargetVar.Text.Trim();
                decimal h = ParseDec(txtStepH.Text, "Adım Boyutu (h)", 0.001m);
                int acc = (cmbAccuracy.SelectedIndex + 1) * 2;

                Dictionary<string, decimal> pts = new Dictionary<string, decimal>();
                foreach (var row in partialRows)
                {
                    if (string.IsNullOrWhiteSpace(row.Variable)) continue;
                    pts[row.Variable] = ParseDec(row.Value, row.Variable);
                }

                if (!pts.ContainsKey(targetVar))
                {
                    MessageBox.Show($"Hedef türev değişkeni '{targetVar}', değişken listesinde tanımlı olmalıdır.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Direction vector parsing
                string dirRaw = txtDirectionVector.Text.Trim();
                List<decimal> uVec = new List<decimal>();
                if (!string.IsNullOrWhiteSpace(dirRaw))
                {
                    var tokens = dirRaw.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tok in tokens)
                    {
                        if (decimal.TryParse(tok.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal uVal))
                            uVec.Add(uVal);
                    }
                }

                await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fVal = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pts);
                        decimal partialVal = NumericalMethods.Method_NumericalPartialDifferentiation(eq, 1, targetVar, pts, acc, h);

                        // Gradient Vector
                        Dictionary<string, decimal> gradient = new Dictionary<string, decimal>();
                        foreach (string vName in pts.Keys)
                        {
                            decimal df = NumericalMethods.Method_NumericalPartialDifferentiation(eq, 1, vName, pts, acc, h);
                            gradient[vName] = df;
                        }

                        // Directional Derivative
                        decimal? dirDeriv = null;
                        if (uVec.Count == pts.Count)
                        {
                            decimal normSq = uVec.Sum(u => u * u);
                            if (normSq > 0m)
                            {
                                decimal norm = DecimalMath.Sqrt(normSq);
                                decimal sum = 0m;
                                int idx = 0;
                                foreach (var kv in gradient)
                                {
                                    sum += kv.Value * (uVec[idx] / norm);
                                    idx++;
                                }
                                dirDeriv = sum;
                            }
                        }

                        this.Invoke((Action)(() =>
                        {
                            string gradStr = string.Join(",  ", gradient.Select(kv => $"{kv.Value:F4}"));
                            lblKpiTitle.Text = $"GRADYAN VEKTÖRÜ ∇f({string.Join(",", pts.Keys)})";
                            lblKpiValue.Text = $"∇f = [ {gradStr} ]";
                            lblKpiSubtitle.Text = $"∂f/∂{targetVar} = {partialVal:F6}  •  f({string.Join(",", pts.Values.Select(v => v.ToString("G4")))}) = {fVal:F6}";

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("=======================================================================");
                            sb.AppendLine("  ÇOK DEĞİŞKENLİ KISMİ TÜREV, GRADYAN VE YÖNLÜ TÜREV RAPORU");
                            sb.AppendLine("=======================================================================");
                            sb.AppendLine($"  Fonksiyon : f({string.Join(", ", pts.Keys)}) = {eq}");
                            sb.AppendLine($"  Nokta     : ({string.Join(", ", pts.Select(kv => $"{kv.Key} = {kv.Value}"))}) => f = {fVal:G18}");
                            sb.AppendLine($"  Adım (h)  : {h}   |   Doğruluk: O(h^{acc})");
                            sb.AppendLine();
                            sb.AppendLine($"  1. '{targetVar}' DEĞİŞKENİNE GÖRE 1. MERTEBE KISMİ TÜREV:");
                            sb.AppendLine($"     • ∂f / ∂{targetVar} = {partialVal:G18}");
                            sb.AppendLine();
                            sb.AppendLine("  2. NOKTADAKİ GRADYAN VEKTÖRÜ (∇f):");
                            foreach (var kv in gradient)
                            {
                                sb.AppendLine($"     • ∂f / ∂{kv.Key,-6} = {kv.Value:F8}");
                            }
                            sb.AppendLine($"     ∇f = [ {gradStr} ]ᵀ");
                            sb.AppendLine($"     |∇f| (Büyüklük/Maksimum Artış Eğimi) = {Math.Sqrt((double)gradient.Sum(kv => kv.Value * kv.Value)):F8}");
                            sb.AppendLine();

                            if (dirDeriv.HasValue)
                            {
                                sb.AppendLine("  3. YÖNLÜ TÜREV (Directional Derivative - D_u f):");
                                sb.AppendLine($"     • Birim Yön Vektörü (u) : [ {string.Join(", ", uVec.Select(u => u.ToString("F3")))} ]");
                                sb.AppendLine($"     • D_u f = ∇f · u        = {dirDeriv.Value:G18}");
                            }

                            txtSummaryView.Text = sb.ToString();
                            txtResult.Text = sb.ToString();
                            tabResults.SelectedTab = tabSummary;
                        }));
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kısmi türev hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCalcPartial.Enabled = true;
                btnCalcPartial.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region 3. Hessian Matrix & Local Extremum Test

        private async void BtnCalcHessian_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalcHessian.Text;
            btnCalcHessian.Enabled = false;
            btnCalcHessian.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtPartialFn.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq)) return;

                decimal h = ParseDec(txtStepH.Text, "Adım Boyutu (h)", 0.001m);
                int acc = (cmbAccuracy.SelectedIndex + 1) * 2;

                Dictionary<string, decimal> pts = new Dictionary<string, decimal>();
                foreach (var row in partialRows)
                {
                    if (string.IsNullOrWhiteSpace(row.Variable)) continue;
                    pts[row.Variable] = ParseDec(row.Value, row.Variable);
                }

                if (pts.Count < 2)
                {
                    MessageBox.Show("Hessian matrisi için en az 2 değişken tanımlanmalıdır (Örn: x ve y).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                List<string> varNames = pts.Keys.ToList();
                int n = varNames.Count;
                decimal[,] hessian = new decimal[n, n];

                await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        for (int i = 0; i < n; i++)
                        {
                            for (int j = 0; j < n; j++)
                            {
                                string vi = varNames[i];
                                string vj = varNames[j];

                                if (i == j)
                                {
                                    // Second partial derivative ∂²f / ∂vi²
                                    hessian[i, j] = NumericalMethods.Method_NumericalPartialDifferentiation(eq, 2, vi, pts, acc, h);
                                }
                                else
                                {
                                    // Mixed partial derivative ∂²f / ∂vi∂vj via Central Difference:
                                    // [f(vi+h, vj+h) - f(vi+h, vj-h) - f(vi-h, vj+h) + f(vi-h, vj-h)] / (4h²)
                                    var pPlusPlus = new Dictionary<string, decimal>(pts);
                                    var pPlusMinus = new Dictionary<string, decimal>(pts);
                                    var pMinusPlus = new Dictionary<string, decimal>(pts);
                                    var pMinusMinus = new Dictionary<string, decimal>(pts);

                                    pPlusPlus[vi] += h; pPlusPlus[vj] += h;
                                    pPlusMinus[vi] += h; pPlusMinus[vj] -= h;
                                    pMinusPlus[vi] -= h; pMinusPlus[vj] += h;
                                    pMinusMinus[vi] -= h; pMinusMinus[vj] -= h;

                                    decimal fPP = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pPlusPlus);
                                    decimal fPM = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pPlusMinus);
                                    decimal fMP = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pMinusPlus);
                                    decimal fMM = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pMinusMinus);

                                    decimal mixed = (fPP - fPM - fMP + fMM) / (4m * h * h);
                                    hessian[i, j] = mixed;
                                }
                            }
                        }

                        this.Invoke((Action)(() =>
                        {
                            // Populate DataGridView
                            dgvHessian.Columns.Clear();
                            dgvHessian.Rows.Clear();

                            for (int j = 0; j < n; j++)
                            {
                                dgvHessian.Columns.Add($"col_{varNames[j]}", $"∂ / ∂{varNames[j]}");
                            }

                            for (int i = 0; i < n; i++)
                            {
                                object[] rowValues = new object[n];
                                for (int j = 0; j < n; j++)
                                {
                                    rowValues[j] = hessian[i, j].ToString("F6", CultureInfo.InvariantCulture);
                                }
                                int rIdx = dgvHessian.Rows.Add(rowValues);
                                dgvHessian.Rows[rIdx].HeaderCell.Value = $"∂ / ∂{varNames[i]}";
                            }

                            // 2x2 Extremum Test
                            if (n == 2)
                            {
                                decimal fxx = hessian[0, 0];
                                decimal fyy = hessian[1, 1];
                                decimal fxy = hessian[0, 1];
                                decimal detH = fxx * fyy - (fxy * fxy);

                                if (detH > 0m && fxx > 0m)
                                {
                                    lblHessianAnalysis.Text = $"✓ D = {detH:F6} > 0 ve f_xx > 0  =>  Bu nokta YEREL MİNİMUM noktasıdır.";
                                    lblHessianAnalysis.ForeColor = Color.FromArgb(52, 211, 153); // Emerald
                                }
                                else if (detH > 0m && fxx < 0m)
                                {
                                    lblHessianAnalysis.Text = $"✓ D = {detH:F6} > 0 ve f_xx < 0  =>  Bu nokta YEREL MAKSİMUM noktasıdır.";
                                    lblHessianAnalysis.ForeColor = Color.FromArgb(56, 189, 248); // Sky Blue
                                }
                                else if (detH < 0m)
                                {
                                    lblHessianAnalysis.Text = $"⚠️ D = {detH:F6} < 0  =>  Bu nokta bir EYER NOKTASIDIR (Saddle Point).";
                                    lblHessianAnalysis.ForeColor = Color.FromArgb(244, 63, 94); // Rose
                                }
                                else
                                {
                                    lblHessianAnalysis.Text = $"❓ D = {detH:F6} = 0  =>  İkinci türev testi belirsizdir.";
                                    lblHessianAnalysis.ForeColor = Color.FromArgb(245, 158, 11); // Amber
                                }
                            }
                            else
                            {
                                lblHessianAnalysis.Text = $"{n}×{n} Simetrik Hessian Matrisi başarıyla hesaplandı.";
                                lblHessianAnalysis.ForeColor = Color.FromArgb(168, 85, 247);
                            }

                            tabResults.SelectedTab = tabHessian;
                        }));
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hessian hesaplama hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCalcHessian.Enabled = true;
                btnCalcHessian.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        #endregion
    }
}
