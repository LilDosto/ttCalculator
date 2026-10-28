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
    public partial class SystemSolverControl : UserControl
    {
        private List<TextBox> equationsInputs = new List<TextBox>();
        private List<TextBox> variablesInputs = new List<TextBox>();
        private List<TextBox> initialPointsInputs = new List<TextBox>();

        public SystemSolverControl()
        {
            InitializeComponent();
            cmbSolverMethod.SelectedIndex = 0;
            UpdateMethodHint();
            cmbSolverMethod.SelectedIndexChanged += (s, e) => UpdateMethodHint();

            btnGenerate.Click += BtnGenerate_Click;
            btnSolveSystem.Click += BtnSolveSystem_Click;

            btnExLinear.Click += (s, e) => {
                cmbSolverMethod.SelectedIndex = 2; // LU Decomposition
                LoadPreset(
                    new[] { "2*x + y = 5", "x - y = 1" },
                    new[] { "x", "y" },
                    new[] { "0", "0" });
            };

            btnExGaussSeidel.Click += (s, e) => {
                cmbSolverMethod.SelectedIndex = 1; // Gauss-Seidel
                LoadPreset(
                    new[] { "4*x + y - z = 7", "x - 5*y - z = -8", "2*x + y + 6*z = 9" },
                    new[] { "x", "y", "z" },
                    new[] { "0", "0", "0" });
            };

            btnExNonlinear.Click += (s, e) => {
                cmbSolverMethod.SelectedIndex = 0; // Newton-Raphson
                LoadPreset(
                    new[] { "x^2 + y^2 - 4", "x - y" },
                    new[] { "x", "y" },
                    new[] { "1.5", "1.0" });
            };

            btnCopyHero.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtSystemResult.Text))
                {
                    Clipboard.SetText(txtSystemResult.Text);
                    MessageBox.Show("Sistem çözümü ve detayları panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Populate default 2 equations immediately on startup
            LoadPreset(
                new[] { "2*x + y = 5", "x - y = 1" },
                new[] { "x", "y" },
                new[] { "0", "0" },
                autoSolve: false);
        }

        private void UpdateMethodHint()
        {
            switch (cmbSolverMethod.SelectedIndex)
            {
                case 0:
                    lblMethodHint.Text = "💡 Newton-Raphson: Doğrusal olmayan (polinom, trigonometrik, üstel) sistemler için teğet jakobiyen tabanlı genel çözücü. Başlangıç noktaları girilmelidir.";
                    break;
                case 1:
                    lblMethodHint.Text = "💡 Gauss-Seidel: Doğrusal sistemler için ardışık iterasyon yöntemi. Köşegen baskınlık otomatik denetlenir ve satırlar sıralanır.";
                    break;
                case 2:
                    lblMethodHint.Text = "💡 LU Ayrışımı: Doğrusal sistemler için A = P·L·U matris çarpanlarına ayırma ile doğrudan, kesin ve en hızlı çözücü.";
                    break;
                case 3:
                    lblMethodHint.Text = "💡 Ters Matris: Doğrusal sistemlerde X = A⁻¹·b doğrudan ters matris formülü ile analitik çözüm sağlar.";
                    break;
                case 4:
                    lblMethodHint.Text = "💡 QR Ayrışımı: Householder ortogonal dönüşümü ile kare veya aşırı belirlenmiş sistemler için en küçük kareler çözümü sunar.";
                    break;
            }
        }

        private void LoadPreset(string[] eqs, string[] vars, string[] inits, bool autoSolve = true)
        {
            numEqCount.Value = eqs.Length;
            GenerateRows(eqs.Length, eqs, vars, inits);
            if (autoSolve)
            {
                BtnSolveSystem_Click(this, EventArgs.Empty);
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            int count = (int)numEqCount.Value;
            string[] defaultVars = { "x", "y", "z", "w", "u", "v", "t", "s" };
            string[] eqs = new string[count];
            string[] vars = new string[count];
            string[] inits = new string[count];

            for (int i = 0; i < count; i++)
            {
                eqs[i] = i < equationsInputs.Count ? equationsInputs[i].Text : "";
                vars[i] = i < variablesInputs.Count ? variablesInputs[i].Text : (i < defaultVars.Length ? defaultVars[i] : "x" + (i + 1));
                inits[i] = i < initialPointsInputs.Count ? initialPointsInputs[i].Text : "1";
            }

            GenerateRows(count, eqs, vars, inits);
        }

        private void GenerateRows(int count, string[] eqs, string[] vars, string[] inits)
        {
            pnlEquations.SuspendLayout();
            pnlEquations.Controls.Clear();
            equationsInputs.Clear();
            variablesInputs.Clear();
            initialPointsInputs.Clear();

            for (int i = 0; i < count; i++)
            {
                Panel p = new Panel
                {
                    Size = new Size(820, 36),
                    BackColor = ThemeHelper.CardBackground,
                    Margin = new Padding(0, 0, 0, 4)
                };

                Label lblEq = new Label
                {
                    Text = $"Denklem {i + 1}:",
                    Location = new Point(8, 9),
                    Size = new Size(78, 20),
                    ForeColor = ThemeHelper.LightText,
                    Font = new Font("Segoe UI", 9F)
                };

                TextBox txtEq = new TextBox
                {
                    Location = new Point(90, 6),
                    Size = new Size(330, 24),
                    BackColor = Color.FromArgb(15, 23, 42),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10F),
                    Text = (eqs != null && i < eqs.Length) ? eqs[i] : ""
                };
                txtEq.KeyDown += Input_KeyDown;

                Label lblVar = new Label
                {
                    Text = "Değişken:",
                    Location = new Point(430, 9),
                    Size = new Size(62, 20),
                    ForeColor = ThemeHelper.LightText,
                    Font = new Font("Segoe UI", 8.5F)
                };

                TextBox txtVar = new TextBox
                {
                    Location = new Point(494, 6),
                    Size = new Size(50, 24),
                    BackColor = Color.FromArgb(15, 23, 42),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10F),
                    Text = (vars != null && i < vars.Length) ? vars[i] : ("x" + (i + 1))
                };
                txtVar.KeyDown += Input_KeyDown;

                Label lblInit = new Label
                {
                    Text = "Başlangıç (x₀):",
                    Location = new Point(554, 9),
                    Size = new Size(88, 20),
                    ForeColor = ThemeHelper.LightText,
                    Font = new Font("Segoe UI", 8.5F)
                };

                TextBox txtInit = new TextBox
                {
                    Location = new Point(644, 6),
                    Size = new Size(70, 24),
                    BackColor = Color.FromArgb(15, 23, 42),
                    BorderStyle = BorderStyle.FixedSingle,
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 10F),
                    Text = (inits != null && i < inits.Length) ? inits[i] : "1"
                };
                txtInit.KeyDown += Input_KeyDown;

                p.Controls.Add(lblEq);
                p.Controls.Add(txtEq);
                p.Controls.Add(lblVar);
                p.Controls.Add(txtVar);
                p.Controls.Add(lblInit);
                p.Controls.Add(txtInit);

                equationsInputs.Add(txtEq);
                variablesInputs.Add(txtVar);
                initialPointsInputs.Add(txtInit);

                pnlEquations.Controls.Add(p);
            }

            pnlEquations.ResumeLayout();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnSolveSystem_Click(this, EventArgs.Empty);
            }
        }

        private async void BtnSolveSystem_Click(object sender, EventArgs e)
        {
            string origBtnText = btnSolveSystem.Text;
            btnSolveSystem.Enabled = false;
            btnSolveSystem.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                int count = equationsInputs.Count;
                if (count == 0) return;

                List<string> eqs = new List<string>();
                Dictionary<string, decimal> dict = new Dictionary<string, decimal>();

                for (int i = 0; i < count; i++)
                {
                    string eq = equationsInputs[i].Text.Trim();
                    if (string.IsNullOrWhiteSpace(eq))
                    {
                        txtSystemResult.Text = $"⚠️ Lütfen {i + 1}. denklemi doldurunuz.";
                        lblHeroSolution.Text = "Eksik Denklem";
                        lblHeroSolution.ForeColor = Color.FromArgb(239, 68, 68);
                        return;
                    }
                    eqs.Add(eq);

                    string varName = variablesInputs[i].Text.Trim();
                    if (string.IsNullOrWhiteSpace(varName))
                    {
                        txtSystemResult.Text = $"⚠️ Lütfen {i + 1}. denklemin değişken adını giriniz.";
                        return;
                    }

                    string initStr = initialPointsInputs[i].Text.Trim().Replace(',', '.');
                    if (!decimal.TryParse(initStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                    {
                        txtSystemResult.Text = $"⚠️ '{varName}' için geçerli bir başlangıç sayısı giriniz.";
                        return;
                    }

                    if (dict.ContainsKey(varName))
                    {
                        txtSystemResult.Text = $"❌ Hata: '{varName}' değişkeni birden fazla kez tanımlanmış.";
                        return;
                    }
                    dict.Add(varName, val);
                }

                if (!int.TryParse(txtSystemIterations.Text.Trim(), out int iterCount) || iterCount < 1)
                    iterCount = 50;

                int methodIdx = cmbSolverMethod.SelectedIndex;

                var (results, stepLogs) = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    Dictionary<string, decimal> resDict;
                    List<string> logs;
                    try
                    {
                        if (methodIdx == 0) // Multivariable Newton-Raphson
                        {
                            resDict = NumericalMethods.SolveEquationSystemWithLogs(eqs, dict, iterCount, out logs);
                        }
                        else if (methodIdx == 1) // Gauss-Seidel
                        {
                            resDict = NumericalMethods.Method_GaussSeidel(eqs, dict, iterCount, out logs);
                        }
                        else if (methodIdx == 2) // LU Decomposition
                        {
                            resDict = NumericalMethods.Method_LinearMatrixSolve(eqs, dict.Keys.ToList(), "LU", out logs);
                        }
                        else if (methodIdx == 3) // Inverse Matrix
                        {
                            resDict = NumericalMethods.Method_LinearMatrixSolve(eqs, dict.Keys.ToList(), "INVERSE", out logs);
                        }
                        else // QR Decomposition
                        {
                            resDict = NumericalMethods.Method_LinearMatrixSolve(eqs, dict.Keys.ToList(), "QR", out logs);
                        }
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                    return (resDict, logs);
                });

                if (results != null)
                {
                    List<string> resLines = new List<string>();
                    resLines.Add("=== ÇÖZÜM SONUÇLARI ===");
                    StringBuilder heroSb = new StringBuilder();
                    int k = 0;
                    foreach (var kvp in results)
                    {
                        decimal rounded = Math.Round(kvp.Value, 8);
                        string valStr = rounded.ToString("0.########", CultureInfo.InvariantCulture);
                        resLines.Add($"  {kvp.Key} = {valStr}");

                        if (k > 0) heroSb.Append(",  ");
                        heroSb.Append($"{kvp.Key} = {valStr}");
                        k++;
                    }

                    lblHeroSolution.Text = heroSb.ToString();
                    lblHeroSolution.ForeColor = Color.FromArgb(52, 211, 153);
                    lblHeroStatus.Text = $"✅ {results.Count} değişkenli sistem başarıyla çözüldü.";

                    resLines.Add("");
                    resLines.Add("=== DETAYLI ÇÖZÜM ADIMLARI ===");
                    resLines.AddRange(stepLogs);

                    txtSystemResult.Text = string.Join(Environment.NewLine, resLines);
                }
                else
                {
                    lblHeroSolution.Text = "Çözüm Bulunamadı";
                    lblHeroSolution.ForeColor = Color.FromArgb(239, 68, 68);
                    lblHeroStatus.Text = "Belirtilen tolerans ve iterasyon sınırında sistem çözülemedi.";
                    txtSystemResult.Text = "Sistem çözülemedi.";
                }
            }
            catch (Exception ex)
            {
                lblHeroSolution.Text = "Hata Oluştu";
                lblHeroSolution.ForeColor = Color.FromArgb(239, 68, 68);
                lblHeroStatus.Text = ex.Message;
                txtSystemResult.Text = "❌ Hata: " + ex.Message;
            }
            finally
            {
                btnSolveSystem.Enabled = true;
                btnSolveSystem.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }
}
