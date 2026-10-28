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
    public partial class OdeSystemSolverControl : UserControl
    {
        private class EqRow
        {
            public Panel Container;
            public Label LblDeriv;
            public TextBox TxtEquation;
            public Label LblInit;
            public TextBox TxtInitVal;
        }

        private readonly List<EqRow> _rows = new List<EqRow>();

        public OdeSystemSolverControl()
        {
            InitializeComponent();
            SetupEvents();
            BuildRows(2);
            SetPresetHarmonic();
        }

        private void SetupEvents()
        {
            numEqCount.ValueChanged += (s, e) => BuildRows((int)numEqCount.Value);
            btnSolve.Click += async (s, e) => await SolveAsync();
            btnCopy.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtOutput.Text))
                {
                    Clipboard.SetText(txtOutput.Text);
                    MessageBox.Show("Sonuç tablosu panoya kopyalandı.", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnEx1.Click += (s, e) => SetPresetHarmonic();
            btnEx2.Click += (s, e) => SetPresetLotkaVolterra();
            btnEx3.Click += (s, e) => SetPresetDampedPendulum();
            btnEx4.Click += (s, e) => SetPresetLorenz();
        }

        private void BuildRows(int count)
        {
            pnlEquations.SuspendLayout();
            pnlEquations.Controls.Clear();
            _rows.Clear();

            int yOffset = 4;
            for (int i = 1; i <= count; i++)
            {
                Panel pnlRow = new Panel
                {
                    Location = new Point(10, yOffset),
                    Size = new Size(860, 32),
                    BackColor = Color.Transparent
                };

                Label lblD = new Label
                {
                    Text = $"dy{i}/dt =",
                    Location = new Point(0, 5),
                    Size = new Size(70, 22),
                    ForeColor = Color.FromArgb(147, 197, 253),
                    Font = new Font("Consolas", 10.5F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleRight
                };

                TextBox txtEq = new TextBox
                {
                    Location = new Point(78, 4),
                    Size = new Size(380, 24),
                    Font = new Font("Consolas", 10F),
                    BackColor = ThemeHelper.CardBackground,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = i == 1 ? "y2" : "-y1"
                };

                Label lblInit = new Label
                {
                    Text = $"y{i}(t₀) =",
                    Location = new Point(475, 5),
                    Size = new Size(75, 22),
                    ForeColor = ThemeHelper.LightText,
                    Font = new Font("Consolas", 9.5F),
                    TextAlign = ContentAlignment.MiddleRight
                };

                TextBox txtInit = new TextBox
                {
                    Location = new Point(555, 4),
                    Size = new Size(90, 24),
                    Font = new Font("Consolas", 10F),
                    BackColor = ThemeHelper.CardBackground,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = i == 1 ? "1" : "0"
                };

                pnlRow.Controls.Add(lblD);
                pnlRow.Controls.Add(txtEq);
                pnlRow.Controls.Add(lblInit);
                pnlRow.Controls.Add(txtInit);

                pnlEquations.Controls.Add(pnlRow);
                _rows.Add(new EqRow
                {
                    Container = pnlRow,
                    LblDeriv = lblD,
                    TxtEquation = txtEq,
                    LblInit = lblInit,
                    TxtInitVal = txtInit
                });

                yOffset += 34;
            }

            pnlEquations.Height = Math.Max(75, yOffset + 8);
            pnlEquations.ResumeLayout(true);
        }

        private void SetPresetHarmonic()
        {
            numEqCount.Value = 2;
            txtT0.Text = "0";
            txtTEnd.Text = "6.28318";
            numSteps.Value = 100;
            cmbMethod.SelectedIndex = 0; // RK4

            if (_rows.Count >= 2)
            {
                _rows[0].TxtEquation.Text = "y2";
                _rows[0].TxtInitVal.Text = "1";
                _rows[1].TxtEquation.Text = "-y1";
                _rows[1].TxtInitVal.Text = "0";
            }
        }

        private void SetPresetLotkaVolterra()
        {
            numEqCount.Value = 2;
            txtT0.Text = "0";
            txtTEnd.Text = "15";
            numSteps.Value = 200;
            cmbMethod.SelectedIndex = 0;

            if (_rows.Count >= 2)
            {
                _rows[0].TxtEquation.Text = "0.667 * y1 - 1.333 * y1 * y2";
                _rows[0].TxtInitVal.Text = "1";
                _rows[1].TxtEquation.Text = "-1.0 * y2 + 1.0 * y1 * y2";
                _rows[1].TxtInitVal.Text = "1";
            }
        }

        private void SetPresetDampedPendulum()
        {
            numEqCount.Value = 2;
            txtT0.Text = "0";
            txtTEnd.Text = "20";
            numSteps.Value = 200;
            cmbMethod.SelectedIndex = 0;

            if (_rows.Count >= 2)
            {
                _rows[0].TxtEquation.Text = "y2";
                _rows[0].TxtInitVal.Text = "1.57";
                _rows[1].TxtEquation.Text = "-0.25 * y2 - sin(y1)";
                _rows[1].TxtInitVal.Text = "0";
            }
        }

        private void SetPresetLorenz()
        {
            numEqCount.Value = 3;
            txtT0.Text = "0";
            txtTEnd.Text = "10";
            numSteps.Value = 500;
            cmbMethod.SelectedIndex = 0;

            if (_rows.Count >= 3)
            {
                _rows[0].TxtEquation.Text = "10 * (y2 - y1)";
                _rows[0].TxtInitVal.Text = "1";
                _rows[1].TxtEquation.Text = "y1 * (28 - y3) - y2";
                _rows[1].TxtInitVal.Text = "1";
                _rows[2].TxtEquation.Text = "y1 * y2 - (8/3) * y3";
                _rows[2].TxtInitVal.Text = "1";
            }
        }

        private async Task SolveAsync()
        {
            try
            {
                btnSolve.Enabled = false;
                btnSolve.Text = "⏳ Hesaplanıyor...";

                int n = _rows.Count;
                List<string> eqList = new List<string>(n);
                List<string> varNames = new List<string>(n);
                Dictionary<string, decimal> inits = new Dictionary<string, decimal>();

                for (int i = 0; i < n; i++)
                {
                    string vName = $"y{i + 1}";
                    varNames.Add(vName);

                    string eqText = _rows[i].TxtEquation.Text.Trim();
                    if (string.IsNullOrWhiteSpace(eqText))
                        throw new ArgumentException($"{vName} türev denklemi boş bırakılamaz.");
                    eqList.Add(eqText);

                    if (!decimal.TryParse(_rows[i].TxtInitVal.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal iv))
                        iv = 0m;
                    inits[vName] = iv;
                }

                if (!decimal.TryParse(txtT0.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal t0))
                    t0 = 0m;
                if (!decimal.TryParse(txtTEnd.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal tEnd))
                    tEnd = 10m;

                int steps = (int)numSteps.Value;
                string method = cmbMethod.SelectedItem?.ToString() ?? "RK4";

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var results = await Task.Run(() =>
                {
                    return DifferentialEquations.SolveOdeSystem(
                        eqList,
                        varNames,
                        inits,
                        t0,
                        tEnd,
                        steps,
                        method,
                        "t"
                    );
                });
                sw.Stop();

                if (results.Count == 0)
                {
                    lblFinalValue.Text = "Çözüm üretilemedi.";
                    return;
                }

                var last = results.Last();
                StringBuilder sbFinal = new StringBuilder();
                sbFinal.Append($"Y({last.T:F4}) = [ ");
                for (int i = 0; i < varNames.Count; i++)
                {
                    string v = varNames[i];
                    decimal val = last.YValues.ContainsKey(v) ? last.YValues[v] : 0m;
                    sbFinal.Append($"{v}={val:F6}");
                    if (i < varNames.Count - 1) sbFinal.Append(", ");
                }
                sbFinal.Append(" ]");
                lblFinalValue.Text = sbFinal.ToString();

                decimal dt = (tEnd - t0) / steps;
                lblFinalDetails.Text = $"Yöntem: {method} | Adım Aralığı (h): {dt:F6} | Toplam Nokta: {results.Count} | ⏱️ {sw.ElapsedMilliseconds} ms";

                // Build Table
                StringBuilder sbOut = new StringBuilder();
                sbOut.AppendLine("==========================================================================================");
                sbOut.AppendLine($"DİFERANSİYEL DENKLEM SİSTEMİ NÜMERİK ÇÖZÜM TABLOSU ({method})");
                sbOut.AppendLine($"Aralık: [{t0}, {tEnd}], Adım Sayısı (N): {steps}, Adım Büyüklüğü (h): {dt:F6}");
                sbOut.AppendLine("==========================================================================================");

                // Header
                sbOut.Append(string.Format("{0,-8} | {1,-12}", "Adım", "t"));
                foreach (var v in varNames)
                {
                    sbOut.Append(string.Format(" | {0,-14}", v));
                }
                sbOut.AppendLine();
                sbOut.AppendLine(new string('-', 24 + varNames.Count * 17));

                int printInterval = Math.Max(1, results.Count / 100);
                for (int i = 0; i < results.Count; i++)
                {
                    if (i % printInterval == 0 || i == results.Count - 1)
                    {
                        var row = results[i];
                        sbOut.Append(string.Format("{0,-8} | {1,-12:F6}", i, row.T));
                        foreach (var v in varNames)
                        {
                            decimal val = row.YValues.ContainsKey(v) ? row.YValues[v] : 0m;
                            sbOut.Append(string.Format(" | {0,-14:F8}", val));
                        }
                        sbOut.AppendLine();
                    }
                }

                sbOut.AppendLine("==========================================================================================");
                txtOutput.Text = sbOut.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Çözüm sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblFinalValue.Text = "Hata oluştu!";
                lblFinalDetails.Text = ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = "🚀 Sistemi Çöz";
            }
        }
    }
}
