using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class TaylorSeriesControl : UserControl
    {
        public TaylorSeriesControl()
        {
            InitializeComponent();
            btnCalculate.Click += BtnCalculate_Click;

            btnExSin.Click += (s, e) => LoadPreset("sin(x)", "x", "0", 5);
            btnExExp.Click += (s, e) => LoadPreset("exp(x)", "x", "0", 4);
            btnExLn.Click += (s, e) => LoadPreset("ln(x)", "x", "1", 4);

            btnCopyPoly.Click += (s, e) => {
                if (!string.IsNullOrEmpty(lblHeroPolyValue.Text) && lblHeroPolyValue.Text != "P_n(x) = —")
                {
                    Clipboard.SetText(lblHeroPolyValue.Text);
                    MessageBox.Show("Taylor polinom açılımı panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            txtFunction.KeyDown += Input_KeyDown;
            txtVariable.KeyDown += Input_KeyDown;
            txtCenterPoint.KeyDown += Input_KeyDown;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnCalculate_Click(this, EventArgs.Empty);
            }
        }

        private void LoadPreset(string fn, string varName, string center, int order)
        {
            txtFunction.Text = fn;
            txtVariable.Text = varName;
            txtCenterPoint.Text = center;
            numOrder.Value = order;
            BtnCalculate_Click(this, EventArgs.Empty);
        }

        private async void BtnCalculate_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalculate.Text;
            btnCalculate.Enabled = false;
            btnCalculate.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtFunction.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    txtResult.Text = "Lütfen bir fonksiyon giriniz (Örn: sin(x)).";
                    lblHeroPolyValue.Text = "Lütfen fonksiyon giriniz.";
                    lblHeroPolyValue.ForeColor = Color.FromArgb(239, 68, 68);
                    return;
                }

                string variable = string.IsNullOrWhiteSpace(txtVariable.Text) ? "x" : txtVariable.Text.Trim();
                int degree = (int)numOrder.Value;

                string centerStr = txtCenterPoint.Text.Trim().Replace(',', '.');
                if (!decimal.TryParse(centerStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal center))
                {
                    txtResult.Text = "❌ Hata: Merkez noktası (a) için geçerli bir sayı giriniz.";
                    return;
                }

                var (polyFractional, polyFactorial, polyDecimal, derivLines) = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    List<string> lines = new List<string>();
                    List<string> fracTerms = new List<string>();
                    List<string> factTerms = new List<string>();
                    List<string> decTerms = new List<string>();

                    try
                    {
                        decimal f0 = NumericalMethods.Method_EvaluateFunction(eq, variable, center);
                        if (DecimalMath.Abs(f0) <= 0.00001m) f0 = 0m;

                        string f0Frac = NumericalMethods.FormatFractionalCoeff(f0);
                        lines.Add($"[k = 0] f({center}) = {f0Frac}  ({Math.Round(f0, 6).ToString("0.######", CultureInfo.InvariantCulture)})");

                        if (f0 != 0m)
                        {
                            fracTerms.Add(f0Frac);
                            factTerms.Add(f0Frac);
                            decTerms.Add(Math.Round(f0, 6).ToString("0.######", CultureInfo.InvariantCulture));
                        }

                        for (int k = 1; k <= degree; k++)
                        {
                            decimal step = k >= 4 ? 0.01m : 0.005m;
                            decimal dk = NumericalMethods.Method_NumericalDifferentiation(eq, k, variable, center, 4, step);
                            decimal fact = NumericalMethods.Factorial(k);
                            decimal coeff = dk / fact;

                            if (DecimalMath.Abs(coeff) <= 0.00001m)
                            {
                                lines.Add($"[k = {k}] f^({k})({center}) ≈ 0  =>  Katsayı = 0");
                                continue;
                            }

                            string varPart;
                            if (center == 0m) varPart = k == 1 ? variable : $"{variable}^{k}";
                            else if (center > 0m) varPart = k == 1 ? $"({variable} - {center})" : $"({variable} - {center})^{k}";
                            else varPart = k == 1 ? $"({variable} + {DecimalMath.Abs(center)})" : $"({variable} + {DecimalMath.Abs(center)})^{k}";

                            // 1. Fractional Form (Simplified Fraction)
                            var frac = NumericalMethods.ToFraction(coeff);
                            string termFrac;
                            if (frac.HasValue)
                            {
                                long num = frac.Value.Num;
                                long den = frac.Value.Denom;
                                string sign = num < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                                long absNum = Math.Abs(num);
                                if (den == 1)
                                {
                                    termFrac = absNum == 1 ? $"{sign}{varPart}" : $"{sign}{absNum}*{varPart}";
                                }
                                else
                                {
                                    termFrac = absNum == 1 ? $"{sign}(1/{den})*{varPart}" : $"{sign}({absNum}/{den})*{varPart}";
                                }
                            }
                            else
                            {
                                string sign = coeff < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                                termFrac = $"{sign}{Math.Round(DecimalMath.Abs(coeff), 5).ToString("0.#####", CultureInfo.InvariantCulture)}*{varPart}";
                            }
                            fracTerms.Add(termFrac);

                            // 2. Factorial Form f^(k)(a) / k!
                            var derFrac = NumericalMethods.ToFraction(dk);
                            string termFact;
                            string factSign = dk < 0 ? (factTerms.Count == 0 ? "-" : "- ") : (factTerms.Count == 0 ? "" : "+ ");
                            decimal absDk = DecimalMath.Abs(dk);
                            if (derFrac.HasValue && derFrac.Value.Denom == 1)
                            {
                                long dVal = Math.Abs(derFrac.Value.Num);
                                if (dVal == 1) termFact = $"{factSign}(1/{k}!)*{varPart}";
                                else termFact = $"{factSign}({dVal}/{k}!)*{varPart}";
                            }
                            else
                            {
                                string dStr = Math.Round(absDk, 4).ToString("0.####", CultureInfo.InvariantCulture);
                                termFact = $"{factSign}({dStr}/{k}!)*{varPart}";
                            }
                            factTerms.Add(termFact);

                            // 3. Decimal Form
                            string decSign = coeff < 0 ? (decTerms.Count == 0 ? "-" : "- ") : (decTerms.Count == 0 ? "" : "+ ");
                            string decStr = Math.Round(DecimalMath.Abs(coeff), 6).ToString("0.######", CultureInfo.InvariantCulture);
                            decTerms.Add($"{decSign}{decStr}*{varPart}");

                            string coeffDisplay = frac.HasValue ? (frac.Value.Denom == 1 ? $"{frac.Value.Num}" : $"{frac.Value.Num}/{frac.Value.Denom}") : Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture);
                            lines.Add($"[k = {k,2}] f^({k})({center}) = {Math.Round(dk, 5).ToString("0.#####", CultureInfo.InvariantCulture),-10} | {k}! = {fact,-8} => Kesirli: {coeffDisplay,-12} | Ondalık: {Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture)}");
                        }

                        string finalFrac = fracTerms.Count == 0 ? "0" : string.Join(" ", fracTerms);
                        string finalFact = factTerms.Count == 0 ? "0" : string.Join(" ", factTerms);
                        string finalDec = decTerms.Count == 0 ? "0" : string.Join(" ", decTerms);

                        return (finalFrac, finalFact, finalDec, lines);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                lblHeroPolyValue.Text = $"P_{degree}({variable}) = {polyFractional}";
                lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== {degree}. DERECEDEN TAYLOR POLİNOMU (Merkez a = {center}) ===");
                sb.AppendLine($"Fonksiyon : f({variable}) = {eq}");
                sb.AppendLine();
                sb.AppendLine("📐 1) KESİRLİ SADELEŞTİRİLMİŞ FORM:");
                sb.AppendLine($"   P_{degree}({variable}) = {polyFractional}");
                sb.AppendLine();
                sb.AppendLine("📜 2) ANALİTİK FAKTÖRİYELLİ FORM [ f^(k)(a)/k! ]:");
                sb.AppendLine($"   P_{degree}({variable}) = {polyFactorial}");
                sb.AppendLine();
                sb.AppendLine("🔢 3) ONDALIKLI SAYISAL YAKLAŞIM:");
                sb.AppendLine($"   P_{degree}({variable}) = {polyDecimal}");
                sb.AppendLine();
                sb.AppendLine(new string('=', 65));
                sb.AppendLine("=== NOKTADAKİ TÜREVLER, FAKTÖRİYEL VE TERİM KATSAYILARI TABLOSU ===");
                sb.AppendLine(string.Join(Environment.NewLine, derivLines));

                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
                lblHeroPolyValue.Text = "Hata Oluştu";
                lblHeroPolyValue.ForeColor = Color.FromArgb(239, 68, 68);
            }
            finally
            {
                btnCalculate.Enabled = true;
                btnCalculate.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }
}
