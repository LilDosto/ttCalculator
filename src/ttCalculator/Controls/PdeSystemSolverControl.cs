using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class PdeSystemSolverControl : UserControl
    {
        public PdeSystemSolverControl()
        {
            InitializeComponent();
            SetupEvents();
            SetPresetGrayScott();
        }

        private void SetupEvents()
        {
            btnSolve.Click += async (s, e) => await SolveAsync();
            btnCopy.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtOutput.Text))
                {
                    Clipboard.SetText(txtOutput.Text);
                    MessageBox.Show("Sonuç tablosu panoya kopyalandı.", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnExGrayScott.Click += (s, e) => SetPresetGrayScott();
            btnExSchnakenberg.Click += (s, e) => SetPresetSchnakenberg();
            btnExFitzHugh.Click += (s, e) => SetPresetFitzHugh();
            btnExDiffusion.Click += (s, e) => SetPresetDiffusion();
        }

        private void SetPresetGrayScott()
        {
            txtReactionF.Text = "-u * v^2 + 0.04 * (1 - u)";
            txtReactionG.Text = "u * v^2 - 0.09 * v";
            txtInitU.Text = "1 - 0.5 * exp(-100 * (x - 0.5)^2)";
            txtInitV.Text = "0.25 * exp(-100 * (x - 0.5)^2)";
            txtDu.Text = "0.001";
            txtDv.Text = "0.0005";
            txtL.Text = "1";
            txtT.Text = "1.0";
            numNx.Value = 25;
            numNt.Value = 150;
        }

        private void SetPresetSchnakenberg()
        {
            txtReactionF.Text = "0.1 - u + u^2 * v";
            txtReactionG.Text = "0.9 - u^2 * v";
            txtInitU.Text = "1 + 0.1 * cos(2 * pi * x)";
            txtInitV.Text = "0.9 + 0.1 * cos(2 * pi * x)";
            txtDu.Text = "0.001";
            txtDv.Text = "0.01";
            txtL.Text = "1";
            txtT.Text = "1.0";
            numNx.Value = 25;
            numNt.Value = 150;
        }

        private void SetPresetFitzHugh()
        {
            txtReactionF.Text = "u - (u^3) / 3 - v";
            txtReactionG.Text = "0.08 * (u + 0.7 - 0.8 * v)";
            txtInitU.Text = "exp(-50 * (x - 0.5)^2)";
            txtInitV.Text = "0";
            txtDu.Text = "0.005";
            txtDv.Text = "0.0001";
            txtL.Text = "1";
            txtT.Text = "2.0";
            numNx.Value = 25;
            numNt.Value = 200;
        }

        private void SetPresetDiffusion()
        {
            txtReactionF.Text = "0";
            txtReactionG.Text = "0";
            txtInitU.Text = "sin(pi * x)";
            txtInitV.Text = "cos(pi * x)";
            txtDu.Text = "0.05";
            txtDv.Text = "0.01";
            txtL.Text = "1";
            txtT.Text = "0.5";
            numNx.Value = 25;
            numNt.Value = 150;
        }

        private async Task SolveAsync()
        {
            try
            {
                btnSolve.Enabled = false;
                btnSolve.Text = "⏳ Hesaplanıyor...";

                string fU = txtReactionF.Text.Trim();
                string gV = txtReactionG.Text.Trim();
                string initU = txtInitU.Text.Trim();
                string initV = txtInitV.Text.Trim();

                if (!decimal.TryParse(txtDu.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal du))
                    du = 0.001m;
                if (!decimal.TryParse(txtDv.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal dv))
                    dv = 0.0005m;
                if (!decimal.TryParse(txtL.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lengthL))
                    lengthL = 1m;
                if (!decimal.TryParse(txtT.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal totalTimeT))
                    totalTimeT = 1.0m;

                int nx = (int)numNx.Value;
                int nt = (int)numNt.Value;

                var grid = await Task.Run(() =>
                {
                    return DifferentialEquations.SolveCoupledPdeSystem(
                        fU,
                        gV,
                        initU,
                        initV,
                        lengthL,
                        totalTimeT,
                        nx,
                        nt,
                        du,
                        dv
                    );
                });

                lblSummary.Text = grid.SystemInfo;
                decimal dx = lengthL / (nx - 1);
                decimal dt = totalTimeT / (nt - 1);
                lblDetails.Text = $"Izgara: {nx} mekan × {nt} zaman | Boyut: x ∈ [0, {lengthL}], t ∈ [0, {totalTimeT}] | Sıfır-Akı (Neumann) Sınırları";

                // Format Table
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("==========================================================================================");
                sb.AppendLine("BAĞLAŞIMLI KISMİ DİFERANSİYEL DENKLEM SİSTEMİ (1B REAKSİYON-DİFÜZYON) ÇÖZÜMÜ");
                sb.AppendLine($"F(u,v) = {fU}  |  G(u,v) = {gV}");
                sb.AppendLine($"Du = {du:G4}, Dv = {dv:G4}, L = {lengthL}, T = {totalTimeT}, dx = {dx:F5}, dt = {dt:F5}");
                sb.AppendLine("==========================================================================================");

                // Time slices: t = 0, t = T/2, t = T
                int t0Idx = 0;
                int tMidIdx = nt / 2;
                int tEndIdx = nt - 1;

                decimal t0Val = grid.TGrid[t0Idx];
                decimal tMidVal = grid.TGrid[tMidIdx];
                decimal tEndVal = grid.TGrid[tEndIdx];

                sb.Append(string.Format("{0,-6} | {1,-8}", "Nokta", "x"));
                sb.Append(string.Format(" | {0,-12} | {1,-12}", $"u(t={t0Val:F2})", $"v(t={t0Val:F2})"));
                sb.Append(string.Format(" | {0,-12} | {1,-12}", $"u(t={tMidVal:F2})", $"v(t={tMidVal:F2})"));
                sb.Append(string.Format(" | {0,-12} | {1,-12}", $"u(t={tEndVal:F2})", $"v(t={tEndVal:F2})"));
                sb.AppendLine();
                sb.AppendLine(new string('-', 92));

                for (int i = 0; i < nx; i++)
                {
                    decimal xVal = grid.XGrid[i];
                    sb.Append(string.Format("{0,-6} | {1,-8:F3}", i, xVal));
                    sb.Append(string.Format(" | {0,-12:F5} | {1,-12:F5}", grid.U[i, t0Idx], grid.V[i, t0Idx]));
                    sb.Append(string.Format(" | {0,-12:F5} | {1,-12:F5}", grid.U[i, tMidIdx], grid.V[i, tMidIdx]));
                    sb.Append(string.Format(" | {0,-12:F5} | {1,-12:F5}", grid.U[i, tEndIdx], grid.V[i, tEndIdx]));
                    sb.AppendLine();
                }

                sb.AppendLine("==========================================================================================");
                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDE Sistemi çözümü sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSummary.Text = "Hata oluştu!";
                lblDetails.Text = ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = "🚀 PDE Sistemini Çöz";
            }
        }
    }
}
