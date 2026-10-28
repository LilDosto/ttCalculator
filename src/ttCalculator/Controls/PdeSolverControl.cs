using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class PdeSolverControl : UserControl
    {
        public PdeSolverControl()
        {
            InitializeComponent();
            SetupEvents();
            SetPresetHeat();
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

            btnExHeat.Click += (s, e) => SetPresetHeat();
            btnExWave.Click += (s, e) => SetPresetWave();
            btnExAdvect.Click += (s, e) => SetPresetAdvection();
            btnExGauss.Click += (s, e) => SetPresetGauss();

            cmbPdeType.SelectedIndexChanged += (s, e) =>
            {
                int sel = cmbPdeType.SelectedIndex;
                bool isWave = sel == 2;
                bool isLaplace = sel == 4;

                lblInitVel.Visible = isWave;
                txtInitVel.Visible = isWave;

                lblInitU.Visible = !isLaplace;
                txtInitU.Visible = !isLaplace;
                lblT.Visible = !isLaplace;
                txtT.Visible = !isLaplace;
                lblNt.Visible = !isLaplace;
                numNt.Visible = !isLaplace;

                if (sel == 0 || sel == 1) lblParam.Text = "Difüzyon Katsayısı (α):";
                else if (sel == 2) lblParam.Text = "Dalga Hızı (c):";
                else if (sel == 3) lblParam.Text = "Taşınım Hızı (v):";
                else lblParam.Text = "Boşta (Laplace):";
            };
        }

        private void SetPresetHeat()
        {
            cmbPdeType.SelectedIndex = 0;
            txtInitU.Text = "sin(pi * x)";
            txtInitVel.Text = "0";
            txtParam.Text = "0.1";
            txtL.Text = "1";
            txtT.Text = "0.5";
            numNx.Value = 25;
            numNt.Value = 150;
            txtBcLeft.Text = "0";
            txtBcRight.Text = "0";
        }

        private void SetPresetWave()
        {
            cmbPdeType.SelectedIndex = 2;
            txtInitU.Text = "sin(pi * x)";
            txtInitVel.Text = "0";
            txtParam.Text = "2";
            txtL.Text = "1";
            txtT.Text = "1.0";
            numNx.Value = 25;
            numNt.Value = 200;
            txtBcLeft.Text = "0";
            txtBcRight.Text = "0";
        }

        private void SetPresetAdvection()
        {
            cmbPdeType.SelectedIndex = 3;
            txtInitU.Text = "exp(-40 * (x - 0.3)^2)";
            txtInitVel.Text = "0";
            txtParam.Text = "1";
            txtL.Text = "1";
            txtT.Text = "0.5";
            numNx.Value = 40;
            numNt.Value = 150;
            txtBcLeft.Text = "0";
            txtBcRight.Text = "0";
        }

        private void SetPresetGauss()
        {
            cmbPdeType.SelectedIndex = 1; // Crank-Nicolson
            txtInitU.Text = "exp(-100 * (x - 0.5)^2)";
            txtInitVel.Text = "0";
            txtParam.Text = "0.2";
            txtL.Text = "1";
            txtT.Text = "0.3";
            numNx.Value = 31;
            numNt.Value = 200;
            txtBcLeft.Text = "0";
            txtBcRight.Text = "0";
        }

        private async Task SolveAsync()
        {
            try
            {
                btnSolve.Enabled = false;
                btnSolve.Text = "⏳ Hesaplanıyor...";

                int sel = cmbPdeType.SelectedIndex;

                if (!decimal.TryParse(txtParam.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal paramVal))
                    paramVal = 0.1m;
                if (!decimal.TryParse(txtL.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lengthL))
                    lengthL = 1m;
                if (!decimal.TryParse(txtT.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal totalTimeT))
                    totalTimeT = 0.5m;

                int nx = (int)numNx.Value;
                int nt = (int)numNt.Value;

                if (!decimal.TryParse(txtBcLeft.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bcLeft))
                    bcLeft = 0m;
                if (!decimal.TryParse(txtBcRight.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal bcRight))
                    bcRight = 0m;

                if (sel == 4)
                {
                    // 2D Laplace
                    decimal topBc = 100m;
                    decimal bottomBc = 0m;
                    var laplace = await Task.Run(() =>
                    {
                        return DifferentialEquations.Solve2DLaplace(nx, nx, bcLeft, bcRight, topBc, bottomBc);
                    });

                    lblStability.Text = $"2B Laplace Denklemi (SOR): {laplace.Iterations} iterasyonda yakınsadı (Kalıntı: {laplace.FinalResidual:G4})";
                    lblDetails.Text = $"Izgara: {nx} × {nx} düğüm | Sınırlar: Sol={bcLeft}, Sağ={bcRight}, Üst={topBc}, Alt={bottomBc}";

                    StringBuilder sbL = new StringBuilder();
                    sbL.AppendLine("==========================================================================================");
                    sbL.AppendLine("2 BOYUTLU ELLİPTİK LAPLACE DENKLEMİ (∇²u = u_xx + u_yy = 0) POTANSİYEL DAĞILIMI");
                    sbL.AppendLine($"Izgara: {nx} x {nx} | İterasyon: {laplace.Iterations} | Yakınsama Residual: {laplace.FinalResidual:G4}");
                    sbL.AppendLine("==========================================================================================");

                    int stepX = Math.Max(1, nx / 10);
                    int stepY = Math.Max(1, nx / 10);

                    sbL.Append(string.Format("{0,-8}", "y \\ x"));
                    for (int i = 0; i < nx; i += stepX)
                    {
                        decimal xPos = (decimal)i / (nx - 1);
                        sbL.Append(string.Format(" | {0,-8:F2}", xPos));
                    }
                    sbL.AppendLine();
                    sbL.AppendLine(new string('-', 100));

                    for (int j = nx - 1; j >= 0; j -= stepY)
                    {
                        decimal yPos = (decimal)j / (nx - 1);
                        sbL.Append(string.Format("{0,-8:F2}", yPos));
                        for (int i = 0; i < nx; i += stepX)
                        {
                            sbL.Append(string.Format(" | {0,-8:F2}", laplace.Grid[i, j]));
                        }
                        sbL.AppendLine();
                    }
                    sbL.AppendLine("==========================================================================================");
                    txtOutput.Text = sbL.ToString();
                    return;
                }

                DifferentialEquations.PdeType pdeType;
                if (sel == 0) pdeType = DifferentialEquations.PdeType.HeatEquation;
                else if (sel == 1) pdeType = DifferentialEquations.PdeType.HeatEquationCrankNicolson;
                else if (sel == 2) pdeType = DifferentialEquations.PdeType.WaveEquation;
                else pdeType = DifferentialEquations.PdeType.AdvectionEquation;

                string initU = txtInitU.Text.Trim();
                string initVel = txtInitVel.Text.Trim();

                var grid = await Task.Run(() =>
                {
                    return DifferentialEquations.Solve1DPde(
                        pdeType,
                        initU,
                        lengthL,
                        totalTimeT,
                        nx,
                        nt,
                        paramVal,
                        bcLeft,
                        bcRight,
                        initVel
                    );
                });

                lblStability.Text = grid.StabilityNote;
                decimal dx = lengthL / (nx - 1);
                decimal dt = totalTimeT / (nt - 1);
                lblDetails.Text = $"Izgara: {nx} mekan × {nt} zaman noktası | Δx = {dx:F4}, Δt = {dt:F6} | x ∈ [0, {lengthL}], t ∈ [0, {totalTimeT}]";

                // Format Table of Time Slices
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("==========================================================================================");
                sb.AppendLine($"1 BOYUTLU KISMİ DİFERANSİYEL DENKLEM (PDE) ÇÖZÜM TABLOSU");
                sb.AppendLine(grid.StabilityNote);
                sb.AppendLine($"Izgara: {nx} mekan noktası, {nt} zaman adımı | dx = {dx:F5}, dt = {dt:F5}");
                sb.AppendLine("==========================================================================================");

                int[] timeIndices = new int[]
                {
                    0,
                    nt / 4,
                    nt / 2,
                    (3 * nt) / 4,
                    nt - 1
                };

                sb.Append(string.Format("{0,-8} | {1,-10}", "Nokta i", "x"));
                foreach (int tIdx in timeIndices)
                {
                    decimal tVal = grid.TGrid[tIdx];
                    sb.Append(string.Format(" | {0,-14}", $"u(x, t={tVal:F3})"));
                }
                sb.AppendLine();
                sb.AppendLine(new string('-', 22 + timeIndices.Length * 17));

                for (int i = 0; i < nx; i++)
                {
                    decimal xVal = grid.XGrid[i];
                    sb.Append(string.Format("{0,-8} | {1,-10:F4}", i, xVal));
                    foreach (int tIdx in timeIndices)
                    {
                        decimal uVal = grid.U[i, tIdx];
                        sb.Append(string.Format(" | {0,-14:F6}", uVal));
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("==========================================================================================");
                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDE çözümü sırasında hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStability.Text = "Hata oluştu!";
                lblDetails.Text = ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = "🚀 PDE'yi Çöz";
            }
        }
    }
}
