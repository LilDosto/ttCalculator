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
    public partial class MultivariableTaylorControl : UserControl
    {
        private List<VariableChip> variableChips = new List<VariableChip>();
        private string _lastTangentPlane = "";
        private string _lastFullPoly = "";

        public MultivariableTaylorControl()
        {
            InitializeComponent();
            InitializeGridColumns();

            btnAddVar.Click += BtnAddVar_Click;
            btnResetZero.Click += BtnResetZero_Click;
            btnCalculate.Click += BtnCalculate_Click;

            btnEx1.Click += (s, e) => LoadPreset("sin(x)*cos(y)", 3, new[] { ("x", "0"), ("y", "0") });
            btnEx2.Click += (s, e) => LoadPreset("exp(x + y)", 2, new[] { ("x", "0"), ("y", "0") });
            btnEx3.Click += (s, e) => LoadPreset("x^2*y + y^3", 3, new[] { ("x", "1"), ("y", "2") });
            btnEx4.Click += (s, e) => LoadPreset("x^2 + y^2 + z^2", 2, new[] { ("x", "0"), ("y", "0"), ("z", "0") });
            btnEx5.Click += (s, e) => LoadPreset("ln(1 + x*y)", 3, new[] { ("x", "0"), ("y", "0") });

            btnCopyTangent.Click += (s, e) => {
                if (!string.IsNullOrEmpty(_lastTangentPlane))
                {
                    Clipboard.SetText(_lastTangentPlane);
                    MessageBox.Show("Teğet düzlemi formülü panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnCopyFullPoly.Click += (s, e) => {
                if (!string.IsNullOrEmpty(_lastFullPoly))
                {
                    Clipboard.SetText(_lastFullPoly);
                    MessageBox.Show("Çok değişkenli Taylor polinomu panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnCopyLatex.Click += (s, e) => {
                if (!string.IsNullOrEmpty(_lastFullPoly))
                {
                    string latex = "T_{" + numOrder.Value + "} = " + _lastFullPoly.Replace("*", " \\cdot ");
                    Clipboard.SetText(latex);
                    MessageBox.Show("Polinom LaTeX formatında panoya kopyalandı:\n\n" + latex, "LaTeX Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            txtEquation.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BtnCalculate_Click(this, EventArgs.Empty);
                }
            };

            AddVariableChip("x", "0");
            AddVariableChip("y", "0");

            LoadPreset("sin(x)*cos(y)", 3, new[] { ("x", "0"), ("y", "0") });
        }

        private void InitializeGridColumns()
        {
            dgvMultiTerms.Columns.Clear();
            dgvMultiTerms.Columns.Add("deg", "|α|");
            dgvMultiTerms.Columns.Add("op", "Kısmi Diferansiyel Operatörü");
            dgvMultiTerms.Columns.Add("val", "Noktadaki Değer");
            dgvMultiTerms.Columns.Add("denom", "Payda α!");
            dgvMultiTerms.Columns.Add("fracC", "Kesirli Katsayı");
            dgvMultiTerms.Columns.Add("decC", "Ondalık Katsayı");
            dgvMultiTerms.Columns.Add("term", "Polinom Terimi");

            dgvMultiTerms.Columns["deg"].Width = 45;
            dgvMultiTerms.Columns["op"].Width = 160;
            dgvMultiTerms.Columns["val"].Width = 110;
            dgvMultiTerms.Columns["denom"].Width = 70;
            dgvMultiTerms.Columns["fracC"].Width = 110;
            dgvMultiTerms.Columns["decC"].Width = 110;
        }

        private void LoadPreset(string eq, int order, (string v, string val)[] vars)
        {
            txtEquation.Text = eq;
            numOrder.Value = order;
            pnlVariableChips.Controls.Clear();
            variableChips.Clear();
            foreach (var item in vars)
            {
                AddVariableChip(item.v, item.val);
            }
            BtnCalculate_Click(this, EventArgs.Empty);
        }

        private void BtnAddVar_Click(object sender, EventArgs e)
        {
            string[] names = { "x", "y", "z", "w", "u", "v" };
            string suggested = variableChips.Count < names.Length ? names[variableChips.Count] : "v" + (variableChips.Count + 1);
            AddVariableChip(suggested, "0");
        }

        private void BtnResetZero_Click(object sender, EventArgs e)
        {
            foreach (var chip in variableChips)
            {
                chip.Value = "0";
            }
        }

        private void AddVariableChip(string varName, string val)
        {
            VariableChip chip = new VariableChip(varName, val);
            chip.OnRemove += (s, e) =>
            {
                pnlVariableChips.Controls.Remove(chip.Panel);
                variableChips.Remove(chip);
            };

            pnlVariableChips.Controls.Add(chip.Panel);
            variableChips.Add(chip);
        }

        private async void BtnCalculate_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalculate.Text;
            btnCalculate.Enabled = false;
            btnCalculate.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtEquation.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    txtResult.Text = "Lütfen bir çok değişkenli fonksiyon giriniz.";
                    return;
                }

                int degree = (int)numOrder.Value;
                Dictionary<string, decimal> points = new Dictionary<string, decimal>();

                foreach (var chip in variableChips)
                {
                    string v = chip.Variable;
                    if (string.IsNullOrWhiteSpace(v)) continue;

                    string valNorm = chip.Value.Replace(',', '.');
                    if (!decimal.TryParse(valNorm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal p))
                    {
                        txtResult.Text = $"❌ '{v}' değişkeni için geçerli bir merkez sayısı giriniz.";
                        return;
                    }

                    if (points.ContainsKey(v))
                    {
                        txtResult.Text = $"❌ '{v}' değişkeni birden fazla kez tanımlanmış.";
                        return;
                    }
                    points.Add(v, p);
                }

                if (points.Count == 0)
                {
                    txtResult.Text = "⚠️ Lütfen en az bir değişken tanımlayınız.";
                    return;
                }

                var calcResult = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        // 1. Detailed multi-variable evaluation
                        var detailed = NumericalMethods.MethodMultiVar_TaylorSeriesDetailed(eq, degree, points);

                        // 2. Compute f(a), Gradient, and Hessian
                        Function compiled = NumericalMethods.CompileFunction(eq);
                        decimal f0 = NumericalMethods.EvaluateCompiledMultiVar(compiled, points);
                        if (DecimalMath.Abs(f0) <= 0.00001m) f0 = 0m;

                        List<string> varNames = points.Keys.ToList();
                        int kVars = varNames.Count;

                        // Gradient components
                        decimal[] grad = new decimal[kVars];
                        List<string> linearTerms = new List<string>();
                        if (f0 != 0m) linearTerms.Add(NumericalMethods.FormatFractionalCoeff(f0));

                        for (int i = 0; i < kVars; i++)
                        {
                            int[] alpha1 = new int[kVars];
                            alpha1[i] = 1;
                            decimal d1 = NumericalMethods.ComputeMixedPartialDerivative(compiled, varNames, alpha1, points, 0.005m);
                            grad[i] = d1;

                            if (DecimalMath.Abs(d1) > 0.00001m)
                            {
                                string vName = varNames[i];
                                decimal aVal = points[vName];
                                string factor = aVal == 0m ? vName : (aVal > 0m ? $"({vName} - {aVal})" : $"({vName} + {DecimalMath.Abs(aVal)})");
                                var frac = NumericalMethods.ToFraction(d1);
                                string cStr = frac.HasValue ? (frac.Value.Denom == 1 ? $"{frac.Value.Num}" : $"{frac.Value.Num}/{frac.Value.Denom}") : Math.Round(d1, 4).ToString("0.####", CultureInfo.InvariantCulture);

                                string sign = d1 < 0 ? (linearTerms.Count == 0 ? "-" : "- ") : (linearTerms.Count == 0 ? "" : "+ ");
                                if (cStr == "1" || cStr == "-1")
                                    linearTerms.Add($"{sign}{factor}");
                                else
                                    linearTerms.Add($"{sign}{cStr.TrimStart('-')}*{factor}");
                            }
                        }

                        string tangentPlaneStr = linearTerms.Count == 0 ? "0" : string.Join(" ", linearTerms);

                        // Hessian components (kVars x kVars)
                        decimal[,] hessian = new decimal[kVars, kVars];
                        List<string> quadTerms = new List<string>(linearTerms);

                        for (int i = 0; i < kVars; i++)
                        {
                            for (int j = 0; j < kVars; j++)
                            {
                                int[] alpha2 = new int[kVars];
                                alpha2[i]++;
                                alpha2[j]++;
                                decimal d2 = NumericalMethods.ComputeMixedPartialDerivative(compiled, varNames, alpha2, points, 0.005m);
                                hessian[i, j] = d2;

                                if (i <= j && DecimalMath.Abs(d2) > 0.00001m)
                                {
                                    decimal coeff = i == j ? d2 / 2m : d2; // 1/2 * (2 * dx*dy) = dx*dy for cross terms
                                    string vi = varNames[i];
                                    string vj = varNames[j];
                                    decimal ai = points[vi];
                                    decimal aj = points[vj];
                                    string fi = ai == 0m ? vi : (ai > 0m ? $"({vi} - {ai})" : $"({vi} + {DecimalMath.Abs(ai)})");
                                    string fj = aj == 0m ? vj : (aj > 0m ? $"({vj} - {aj})" : $"({vj} + {DecimalMath.Abs(aj)})");
                                    string varPart = i == j ? (ai == 0m ? $"{vi}^2" : $"{fi}^2") : $"{fi}*{fj}";

                                    var frac = NumericalMethods.ToFraction(coeff);
                                    string cStr = frac.HasValue ? (frac.Value.Denom == 1 ? $"{frac.Value.Num}" : $"{frac.Value.Num}/{frac.Value.Denom}") : Math.Round(coeff, 4).ToString("0.####", CultureInfo.InvariantCulture);
                                    string sign = coeff < 0 ? (quadTerms.Count == 0 ? "-" : "- ") : (quadTerms.Count == 0 ? "" : "+ ");
                                    if (cStr == "1" || cStr == "-1")
                                        quadTerms.Add($"{sign}{varPart}");
                                    else
                                        quadTerms.Add($"{sign}{cStr.TrimStart('-')}*{varPart}");
                                }
                            }
                        }

                        string quadPolyStr = quadTerms.Count == 0 ? "0" : string.Join(" ", quadTerms);

                        // Hessian Determinant / Extremum diagnosis for 2 variables
                        string hessianDiagnosis = "";
                        if (kVars == 2)
                        {
                            decimal fxx = hessian[0, 0];
                            decimal fyy = hessian[1, 1];
                            decimal fxy = hessian[0, 1];
                            decimal detH = (fxx * fyy) - (fxy * fxy);

                            StringBuilder hsb = new StringBuilder();
                            hsb.AppendLine($"Hessian H(a) = [ [{Math.Round(fxx, 4)}, {Math.Round(fxy, 4)}], [{Math.Round(fxy, 4)}, {Math.Round(fyy, 4)}] ]");
                            hsb.Append($"det(H) = fxx·fyy - (fxy)² = {Math.Round(detH, 5)}  =>  ");

                            if (detH > 0.0001m)
                            {
                                if (fxx > 0.0001m) hsb.Append("det(H) > 0 ve fxx > 0  =>  ✓ YEREL MİNİMUM");
                                else if (fxx < -0.0001m) hsb.Append("det(H) > 0 ve fxx < 0  =>  ✓ YEREL MAKSİMUM");
                                else hsb.Append("det(H) > 0  =>  Ekstremum Noktası");
                            }
                            else if (detH < -0.0001m)
                            {
                                hsb.Append("det(H) < 0  =>  ⚠️ EYER NOKTASI (Saddle Point)");
                            }
                            else
                            {
                                hsb.Append("det(H) ≈ 0  =>  İkinci Türev Testi Sonuçsuz");
                            }
                            hessianDiagnosis = hsb.ToString();
                        }
                        else
                        {
                            StringBuilder hsb = new StringBuilder();
                            hsb.Append("Hessian H(a) [" + kVars + "×" + kVars + "]: ");
                            for (int r = 0; r < kVars; r++)
                            {
                                hsb.Append("[ ");
                                for (int c = 0; c < kVars; c++)
                                {
                                    hsb.Append(Math.Round(hessian[r, c], 3).ToString(CultureInfo.InvariantCulture));
                                    if (c < kVars - 1) hsb.Append(", ");
                                }
                                hsb.Append(" ] ");
                            }
                            hessianDiagnosis = hsb.ToString();
                        }

                        // Gradient string
                        string gradStr = "Gradyan ∇f(a) = [ " + string.Join(", ", grad.Select(g => Math.Round(g, 4).ToString("0.####", CultureInfo.InvariantCulture))) + " ]ᵀ";

                        // Parse table rows from detailed lines
                        List<(string deg, string op, string val, string denom, string fracC, string decC, string term)> gridRows = new List<(string, string, string, string, string, string, string)>();
                        foreach (var det in detailed.Details)
                        {
                            if (det.StartsWith("•"))
                            {
                                string clean = det.Substring(1).Trim();
                                // Example: "∂^1f / ∂x   = 1.00000   | Payda: 1     => Kesirli Katsayı: 1          | Ondalık: 1"
                                string[] parts = clean.Split(new string[] { "=", "|", "=>" }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length >= 4)
                                {
                                    string op = parts[0].Trim();
                                    string val = parts[1].Trim();
                                    string payda = parts[2].Replace("Payda:", "").Trim();
                                    string fracC = parts[3].Replace("Kesirli Katsayı:", "").Trim();
                                    string decC = parts.Length > 4 ? parts[4].Replace("Ondalık:", "").Trim() : fracC;

                                    string degStr = "1";
                                    if (op.Contains("∂^"))
                                    {
                                        int p1 = op.IndexOf("∂^") + 2;
                                        int p2 = op.IndexOf("f");
                                        if (p2 > p1) degStr = op.Substring(p1, p2 - p1);
                                    }

                                    gridRows.Add((degStr, op, val, payda, fracC, decC, ""));
                                }
                            }
                        }

                        return (detailed.Fractional, detailed.Factorial, detailed.Decimal, detailed.Details, tangentPlaneStr, quadPolyStr, gradStr, hessianDiagnosis, gridRows);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                _lastTangentPlane = $"L({string.Join(", ", points.Keys)}) = {calcResult.tangentPlaneStr}";
                _lastFullPoly = calcResult.Fractional;

                // Update Block 1: Tangent Plane
                lblTangentValue.Text = _lastTangentPlane;
                lblGradientValue.Text = calcResult.gradStr;

                // Update Block 2: Hessian
                lblHessianValue.Text = $"Q({string.Join(", ", points.Keys)}) = {calcResult.quadPolyStr}";
                lblHessianDet.Text = calcResult.hessianDiagnosis;

                // Update Block 3: Full Polynomial
                lblFullPolyValue.Text = $"T_{degree}({string.Join(", ", points.Keys)}) = {calcResult.Fractional}";

                // Populate dgvMultiTerms
                dgvMultiTerms.Rows.Clear();
                foreach (var row in calcResult.gridRows)
                {
                    dgvMultiTerms.Rows.Add(row.deg, row.op, row.val, row.denom, row.fracC, row.decC, row.term);
                }

                string varsList = string.Join(", ", points.Keys);
                string centerList = string.Join(", ", points.Select(kv => $"{kv.Key}₀ = {kv.Value}"));

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== ÇOK DEĞİŞKENLİ TAYLOR AÇILIMI ({degree}. Derece, Çapraz Kısmi Türevler Dahil) ===");
                sb.AppendLine($"Fonksiyon : f({varsList}) = {eq}");
                sb.AppendLine($"Merkez    : ({centerList})");
                sb.AppendLine();
                sb.AppendLine("📐 1) DOĞRUSAL YAKLAŞIM / TEĞET DÜZLEM (L):");
                sb.AppendLine($"   {_lastTangentPlane}");
                sb.AppendLine($"   {calcResult.gradStr}");
                sb.AppendLine();
                sb.AppendLine("⚡ 2) KUADRATİK YAKLAŞIM & HESSIAN FORMU (Q):");
                sb.AppendLine($"   Q({varsList}) = {calcResult.quadPolyStr}");
                sb.AppendLine($"   {calcResult.hessianDiagnosis}");
                sb.AppendLine();
                sb.AppendLine("🌟 3) KESİRLİ SADELEŞTİRİLMİŞ TAM POLİNOM (T_n):");
                sb.AppendLine($"   T_{degree}({varsList}) = {calcResult.Fractional}");
                sb.AppendLine();
                sb.AppendLine("📜 4) ANALİTİK FAKTÖRİYELLİ FORM [ (1/α!) · ∂^|α|f / ∂x^α ]:");
                sb.AppendLine($"   T_{degree}({varsList}) = {calcResult.Factorial}");
                sb.AppendLine();
                sb.AppendLine("🔢 5) ONDALIKLI SAYISAL YAKLAŞIM:");
                sb.AppendLine($"   T_{degree}({varsList}) = {calcResult.Decimal}");
                sb.AppendLine();
                sb.AppendLine(new string('=', 65));
                sb.AppendLine("=== KISMİ TÜREVLER, PAYDA VE TERİM KATSAYILARI DETAYI ===");
                sb.AppendLine(string.Join(Environment.NewLine, calcResult.Details));

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
                lblFullPolyValue.Text = "Hata Oluştu: " + ex.Message;
            }
            finally
            {
                btnCalculate.Enabled = true;
                btnCalculate.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }

    public class VariableChip
    {
        public Panel Panel { get; private set; }
        public TextBox TxtVariable { get; private set; }
        public TextBox TxtValue { get; private set; }
        public EventHandler OnRemove;

        public string Variable => TxtVariable.Text.Trim();
        public string Value
        {
            get => TxtValue.Text.Trim();
            set => TxtValue.Text = value;
        }

        public VariableChip(string varName, string val)
        {
            Panel = new Panel
            {
                Size = new Size(180, 32),
                BackColor = Color.FromArgb(24, 33, 52),
                Margin = new Padding(0, 0, 8, 4)
            };

            Label lblVar = new Label
            {
                Text = "Var:",
                Location = new Point(4, 7),
                AutoSize = true,
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold)
            };

            TxtVariable = new TextBox
            {
                Text = varName,
                Location = new Point(32, 5),
                Size = new Size(32, 22),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(56, 189, 248),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9.5F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center
            };

            Label lblVal = new Label
            {
                Text = "x₀:",
                Location = new Point(70, 7),
                AutoSize = true,
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold)
            };

            TxtValue = new TextBox
            {
                Text = val,
                Location = new Point(94, 5),
                Size = new Size(54, 22),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 9.5F),
                TextAlign = HorizontalAlignment.Center
            };

            Button btnRemove = new Button
            {
                Text = "✕",
                Location = new Point(154, 5),
                Size = new Size(22, 22),
                BackColor = Color.FromArgb(153, 27, 27),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Bold", 7.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) => OnRemove?.Invoke(this, EventArgs.Empty);

            Panel.Controls.Add(lblVar);
            Panel.Controls.Add(TxtVariable);
            Panel.Controls.Add(lblVal);
            Panel.Controls.Add(TxtValue);
            Panel.Controls.Add(btnRemove);
        }
    }

    public class VariableInputRow
    {
        public Panel Panel { get; private set; }
        public TextBox TxtVariable { get; private set; }
        public TextBox TxtValue { get; private set; }
        public EventHandler OnRemove;

        public string Variable => TxtVariable.Text.Trim();
        public string Value => TxtValue.Text.Trim();

        public VariableInputRow(string varName, string val)
        {
            Panel = new Panel
            {
                Size = new Size(520, 34),
                BackColor = ThemeHelper.CardBackground,
                Margin = new Padding(0, 0, 0, 5)
            };

            Label lblVar = new Label
            {
                Text = "Değişken:",
                Location = new Point(10, 8),
                AutoSize = true,
                ForeColor = ThemeHelper.LightText,
                Font = new Font("Segoe UI", 9.5F)
            };

            TxtVariable = new TextBox
            {
                Text = varName,
                Location = new Point(85, 5),
                Size = new Size(80, 25),
                BackColor = ThemeHelper.InputBackground,
                ForeColor = Color.FromArgb(56, 189, 248),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10.5F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center
            };

            Label lblVal = new Label
            {
                Text = "Nokta:",
                Location = new Point(185, 8),
                AutoSize = true,
                ForeColor = ThemeHelper.LightText,
                Font = new Font("Segoe UI", 9.5F)
            };

            TxtValue = new TextBox
            {
                Text = val,
                Location = new Point(245, 5),
                Size = new Size(100, 25),
                BackColor = ThemeHelper.InputBackground,
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10.5F),
                TextAlign = HorizontalAlignment.Center
            };

            Button btnRemove = new Button
            {
                Text = "✕ Sil",
                Location = new Point(365, 4),
                Size = new Size(65, 26),
                BackColor = Color.FromArgb(153, 27, 27),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) => OnRemove?.Invoke(this, EventArgs.Empty);

            Panel.Controls.Add(lblVar);
            Panel.Controls.Add(TxtVariable);
            Panel.Controls.Add(lblVal);
            Panel.Controls.Add(TxtValue);
            Panel.Controls.Add(btnRemove);
        }
    }
}
