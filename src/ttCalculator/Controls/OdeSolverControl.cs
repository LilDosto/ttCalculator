using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class OdeSolverControl : UserControl
    {
        public OdeSolverControl()
        {
            InitializeComponent();

            btnSolve.Click += BtnSolve_Click;
            btnCopy.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(txtOutput.Text))
                {
                    Clipboard.SetText(txtOutput.Text);
                    MessageBox.Show("Çözüm tablosu panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnEx1.Click += (s, e) => LoadPreset("x + y", "0", "1", "2", 50, 0);
            btnEx2.Click += (s, e) => LoadPreset("-2*x*y", "0", "1", "2", 50, 0);
            btnEx3.Click += (s, e) => LoadPreset("cos(x) - y", "0", "0", "6.28318", 60, 0);
            btnEx4.Click += (s, e) => LoadPreset("y*(1 - y/10)", "0", "1", "8", 80, 0);

            txtEquation.KeyDown += Input_KeyDown;
            txtX0.KeyDown += Input_KeyDown;
            txtY0.KeyDown += Input_KeyDown;
            txtXEnd.KeyDown += Input_KeyDown;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSolve_Click(this, EventArgs.Empty);
            }
        }

        private void LoadPreset(string eq, string x0, string y0, string xEnd, int steps, int methodIdx)
        {
            txtEquation.Text = eq;
            txtX0.Text = x0;
            txtY0.Text = y0;
            txtXEnd.Text = xEnd;
            numSteps.Value = Math.Max(2, Math.Min(5000, steps));
            cmbMethod.SelectedIndex = methodIdx;
            BtnSolve_Click(this, EventArgs.Empty);
        }

        private decimal ParseDec(string s, string field)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal v))
                return v;
            throw new ArgumentException($"'{field}' alanı için geçerli bir sayı giriniz.");
        }

        private async void BtnSolve_Click(object sender, EventArgs e)
        {
            string origBtnText = btnSolve.Text;
            btnSolve.Enabled = false;
            btnSolve.Text = "⏳ Çözülüyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtEquation.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    txtOutput.Text = "Lütfen dy/dx diferansiyel denklemini giriniz.";
                    return;
                }

                decimal x0 = ParseDec(txtX0.Text, "x₀ (Başlangıç)");
                decimal y0 = ParseDec(txtY0.Text, "y(x₀)");
                decimal xEnd = ParseDec(txtXEnd.Text, "x_son (Hedef)");
                int steps = (int)numSteps.Value;
                string method = cmbMethod.SelectedItem?.ToString() ?? "RK4";

                var results = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        return DifferentialEquations.SolveOde(eq, x0, y0, xEnd, steps, method);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                if (results != null && results.Count > 0)
                {
                    var last = results[results.Count - 1];
                    string lastYStr = last.Y.ToString("0.########", CultureInfo.InvariantCulture);
                    string lastXStr = last.X.ToString("0.####", CultureInfo.InvariantCulture);

                    lblFinalValue.Text = $"y({lastXStr}) ≈ {lastYStr}";
                    lblFinalValue.ForeColor = Color.FromArgb(52, 211, 153);
                    lblFinalDetails.Text = $"✅ {method} ile {steps} adımda h = {((xEnd - x0) / steps).ToString("0.######", CultureInfo.InvariantCulture)} adım boyutuyla başarıyla çözüldü.";

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=== DİFERANSİYEL DENKLEM NÜMERİK ÇÖZÜM TABLOSU ===");
                    sb.AppendLine($"  Denklem       : dy/dx = {eq}");
                    sb.AppendLine($"  Başlangıç     : y({x0}) = {y0}");
                    sb.AppendLine($"  Hedef Aralık  : [{x0}, {xEnd}]  (N = {steps} adım, h = {((xEnd - x0) / steps).ToString("0.######", CultureInfo.InvariantCulture)})");
                    sb.AppendLine($"  Yöntem        : {method}");
                    sb.AppendLine(new string('=', 70));
                    sb.AppendLine($"{"Adım",-6} | {"x",-16} | {"y (Hesaplanan)",-20} | {"Eğim (dy/dx)",-18}");
                    sb.AppendLine(new string('-', 70));

                    for (int i = 0; i < results.Count; i++)
                    {
                        var r = results[i];
                        sb.AppendLine($"[{i,4}] | {r.X.ToString("0.######", CultureInfo.InvariantCulture),-16} | {r.Y.ToString("0.########", CultureInfo.InvariantCulture),-20} | {r.Slope.ToString("0.######", CultureInfo.InvariantCulture),-18}");
                    }

                    txtOutput.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "❌ Hata: " + ex.Message;
                lblFinalValue.Text = "Hata Oluştu";
                lblFinalValue.ForeColor = Color.FromArgb(239, 68, 68);
                lblFinalDetails.Text = ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }
}
