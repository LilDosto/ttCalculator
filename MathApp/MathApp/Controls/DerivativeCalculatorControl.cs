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

        public DerivativeCalculatorControl()
        {
            InitializeComponent();

            cmbAccuracy.SelectedIndex = 1; // 4th order default
            btnCalcSingle.Click += BtnCalcSingle_Click;
            btnAddVar.Click += BtnAddVar_Click;
            btnCalcPartial.Click += BtnCalcPartial_Click;

            btnCopyDerivative.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("Türev raporu panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnExSingle1.Click += (s, e) => {
                txtSingleFn.Text = "sin(x) + x^3";
                txtSingleVar.Text = "x";
                txtSinglePoint.Text = "1";
                numSingleDegree.Value = 1;
                BtnCalcSingle_Click(this, EventArgs.Empty);
            };
            btnExSingle2.Click += (s, e) => {
                txtSingleFn.Text = "exp(2*x)";
                txtSingleVar.Text = "x";
                txtSinglePoint.Text = "0";
                numSingleDegree.Value = 3;
                BtnCalcSingle_Click(this, EventArgs.Empty);
            };

            btnExPartial1.Click += (s, e) => LoadPartialPreset("x^2*y + sin(y)", "x", 1, new[] { ("x", "2"), ("y", "1") });
            btnExPartial2.Click += (s, e) => LoadPartialPreset("exp(x*y) + x^3*y^2", "x", 2, new[] { ("x", "1"), ("y", "1") });

            AddPartialRow("x", "1");
            AddPartialRow("y", "2");
        }

        private void LoadPartialPreset(string eq, string targetVar, int degree, (string v, string val)[] vars)
        {
            txtPartialFn.Text = eq;
            txtTargetVar.Text = targetVar;
            numPartialDegree.Value = degree;
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
            row.OnRemove += (s, e) =>
            {
                pnlPartialVars.Controls.Remove(row.Panel);
                partialRows.Remove(row);
            };
            pnlPartialVars.Controls.Add(row.Panel);
            partialRows.Add(row);
        }

        private decimal ParseDec(string s, string field)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal v))
                return v;
            throw new ArgumentException($"'{field}' için geçerli bir sayı giriniz.");
        }

        private async void BtnCalcSingle_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalcSingle.Text;
            btnCalcSingle.Enabled = false;
            btnCalcSingle.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtSingleFn.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq)) return;

                string v = string.IsNullOrWhiteSpace(txtSingleVar.Text) ? "x" : txtSingleVar.Text.Trim();
                decimal pt = ParseDec(txtSinglePoint.Text, "Nokta (x₀)");
                decimal h = ParseDec(txtStepH.Text, "Adım Boyutu (h)");
                int deg = (int)numSingleDegree.Value;
                int acc = (cmbAccuracy.SelectedIndex + 1) * 2; // 2, 4, 6

                string resultText = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fVal = NumericalMethods.Method_EvaluateFunction(eq, v, pt);
                        decimal centralMat = NumericalMethods.Method_NumericalDifferentiation(eq, deg, v, pt, acc, h);
                        decimal forwardBin = NumericalMethods.Method_ForwardBinomialDerivative(eq, deg, v, pt, h);
                        decimal backwardDiff = NumericalMethods.Method_BackwardDifferenceDerivative(eq, deg, v, pt, h);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"=== TEK DEĞİŞKENLİ NÜMERİK TÜREV ANALİZİ ({deg}. Derece) ===");
                        sb.AppendLine($"  Fonksiyon       : f({v}) = {eq}");
                        sb.AppendLine($"  Değerlendirme   : {v}₀ = {pt}   =>   f({pt}) = {Math.Round(fVal, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine($"  Adım Boyutu (h) : {h}   |   Hassasiyet: O(h^{acc})");
                        sb.AppendLine();
                        sb.AppendLine("--- Farklı Nümerik Yöntemlerin Karşılaştırması ---");
                        sb.AppendLine($"  1) Merkezi Sonlu Farklar Matrisi (Method_NumericalDifferentiation) : {Math.Round(centralMat, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine($"  2) İleri Binom Sonlu Farklar     (Method_Derivative)               : {Math.Round(forwardBin, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine($"  3) Geri Sonlu Farklar Formülü    (Method_BackwardDerivative)       : {Math.Round(backwardDiff, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine();
                        sb.AppendLine("--- 1'den 4. Dereceye Kadar Tüm Türev Özeti ---");
                        for (int k = 1; k <= 4; k++)
                        {
                            decimal step = k >= 3 ? Math.Max(h, 0.005m) : h;
                            decimal dk = NumericalMethods.Method_NumericalDifferentiation(eq, k, v, pt, acc, step);
                            sb.AppendLine($"  • f^({k})({pt}) = {Math.Round(dk, 6).ToString("0.######", CultureInfo.InvariantCulture)}");
                        }

                        return sb.ToString();
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                txtResult.Text = resultText;
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
            }
            finally
            {
                btnCalcSingle.Enabled = true;
                btnCalcSingle.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

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
                int deg = (int)numPartialDegree.Value;
                decimal h = ParseDec(txtStepH.Text, "Adım Boyutu (h)");
                int acc = (cmbAccuracy.SelectedIndex + 1) * 2;

                Dictionary<string, decimal> pts = new Dictionary<string, decimal>();
                foreach (var row in partialRows)
                {
                    if (string.IsNullOrWhiteSpace(row.Variable)) continue;
                    pts[row.Variable] = ParseDec(row.Value, row.Variable);
                }

                if (!pts.ContainsKey(targetVar))
                {
                    txtResult.Text = $"⚠️ Hedef değişken '{targetVar}', değişken listesinde tanımlı olmalıdır.";
                    return;
                }

                string resultText = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fVal = NumericalMethods.MethodMultiVar_EvaluateFunction(eq, pts);
                        decimal partialMat = NumericalMethods.Method_NumericalPartialDifferentiation(eq, deg, targetVar, pts, acc, h);
                        decimal partialBin = NumericalMethods.Method_ForwardBinomialPartialDerivative(eq, deg, targetVar, pts, h);

                        StringBuilder sb = new StringBuilder();
                        string ptStr = string.Join(", ", pts.Select(kv => $"{kv.Key}={kv.Value}"));
                        sb.AppendLine($"=== KISMİ TÜREV VE GRADYAN ANALİZİ (∂^{deg}f / ∂{targetVar}^{deg}) ===");
                        sb.AppendLine($"  Fonksiyon  : f({string.Join(",", pts.Keys)}) = {eq}");
                        sb.AppendLine($"  Nokta      : ({ptStr})   =>   f = {Math.Round(fVal, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine();
                        sb.AppendLine($"--- '{targetVar}' Değişkenine Göre {deg}. Derece Kısmi Türev ---");
                        sb.AppendLine($"  • Merkezi Matris Yöntemi (Method_NumericalPartialDifferentiation) : {Math.Round(partialMat, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine($"  • İleri Binom Yöntemi    (Method_PartialDerivative)               : {Math.Round(partialBin, 8).ToString("0.########", CultureInfo.InvariantCulture)}");
                        sb.AppendLine();
                        sb.AppendLine("--- Noktadaki Gradyan Vektörü (∇f) ---");
                        List<string> gradParts = new List<string>();
                        foreach (string vName in pts.Keys)
                        {
                            decimal df = NumericalMethods.Method_NumericalPartialDifferentiation(eq, 1, vName, pts, acc, h);
                            decimal rDf = Math.Round(df, 6);
                            gradParts.Add(rDf.ToString("0.######", CultureInfo.InvariantCulture));
                            sb.AppendLine($"  • ∂f / ∂{vName} = {rDf.ToString("0.######", CultureInfo.InvariantCulture)}");
                        }
                        sb.AppendLine($"  ∇f({ptStr}) = [ {string.Join(",  ", gradParts)} ]");

                        return sb.ToString();
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                txtResult.Text = resultText;
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
            }
            finally
            {
                btnCalcPartial.Enabled = true;
                btnCalcPartial.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }
}
