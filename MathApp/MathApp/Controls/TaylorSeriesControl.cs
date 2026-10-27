using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
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

                bool prevRad = Function.UseRadians;
                Function.UseRadians = true;
                string series;
                List<string> derivLines = new List<string>();
                try
                {
                    decimal f0 = NumericalMethods.Method_EvaluateFunction(eq, variable, center);
                    derivLines.Add($"  f({center}) = {Math.Round(f0, 6).ToString("0.######", CultureInfo.InvariantCulture)}");

                    for (int k = 1; k <= degree; k++)
                    {
                        decimal step = k >= 4 ? 0.01m : 0.005m;
                        decimal dk = NumericalMethods.Method_NumericalDifferentiation(eq, k, variable, center, 4, step);
                        decimal coeff = dk / NumericalMethods.Factorial(k);
                        derivLines.Add($"  f^({k})({center}) = {Math.Round(dk, 5).ToString("0.#####", CultureInfo.InvariantCulture),-14}  =>  Katsayı (c_{k}) = {Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture)}");
                    }

                    series = NumericalMethods.Method_TaylorSeries(eq, degree, variable, center);
                }
                finally
                {
                    Function.UseRadians = prevRad;
                }

                lblHeroPolyValue.Text = $"P_{degree}({variable}) = {series}";
                lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

                txtResult.Text =
                    $"=== {degree}. Dereceden Taylor Polinomu (Merkez a = {center}) ===" + Environment.NewLine +
                    $"P_{degree}({variable}) = {series}" + Environment.NewLine + Environment.NewLine +
                    $"=== Noktadaki Sayısal Türevler ve Terim Katsayıları ===" + Environment.NewLine +
                    string.Join(Environment.NewLine, derivLines);
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
                lblHeroPolyValue.Text = "Hata Oluştu";
                lblHeroPolyValue.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }
    }
}
