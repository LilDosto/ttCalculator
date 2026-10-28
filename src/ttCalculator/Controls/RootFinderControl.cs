using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class RootFinderControl : UserControl
    {
        public RootFinderControl()
        {
            InitializeComponent();
            cmbMethod.SelectedIndex = 0;
            UpdateVisibility();
            cmbMethod.SelectedIndexChanged += (s, e) => UpdateVisibility();
            btnFindRoot.Click += BtnFindRoot_Click;

            btnEx1.Click += (s, e) => LoadPreset("x^3 - x - 2", "x", 0, "1.5", "1", "2");
            btnEx2.Click += (s, e) => LoadPreset("cos(x) - x", "x", 1, "0.5", "0", "1");
            btnEx3.Click += (s, e) => LoadPreset("e^x - 3*x", "x", 2, "0.5", "0", "1");

            btnCopyResult.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("Sonuç ve iterasyon adımları panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Enter key triggers search
            txtEquation.KeyDown += Input_KeyDown;
            txtInitialPoint.KeyDown += Input_KeyDown;
            txtMinX.KeyDown += Input_KeyDown;
            txtMaxX.KeyDown += Input_KeyDown;
            txtIterations.KeyDown += Input_KeyDown;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnFindRoot_Click(this, EventArgs.Empty);
            }
        }

        private void LoadPreset(string eq, string varName, int methodIdx, string initPt, string minX, string maxX)
        {
            txtEquation.Text = eq;
            txtVariable.Text = varName;
            cmbMethod.SelectedIndex = methodIdx;
            txtInitialPoint.Text = initPt;
            txtMinX.Text = minX;
            txtMaxX.Text = maxX;
            BtnFindRoot_Click(this, EventArgs.Empty);
        }

        private void UpdateVisibility()
        {
            int idx = cmbMethod.SelectedIndex;
            bool isSinglePoint = (idx == 0); // Newton-Raphson

            lblInitialPoint.Visible = isSinglePoint;
            txtInitialPoint.Visible = isSinglePoint;

            lblMinX.Visible = !isSinglePoint;
            txtMinX.Visible = !isSinglePoint;
            lblMaxX.Visible = !isSinglePoint;
            txtMaxX.Visible = !isSinglePoint;

            switch (idx)
            {
                case 0:
                    lblMethodDesc.Text = "💡 Fonksiyonun analitik türevi ile teğet çizerek hızlıca köke ulaşır. Tek bir başlangıç tahmini (x₀) yeterlidir.";
                    break;
                case 1:
                    lblMethodDesc.Text = "💡 [a, b] aralığında f(a)·f(b) < 0 ise kökü bulmayı garanti eder. Güvenli ve kararlıdır.";
                    lblMinX.Text = "Alt Sınır (a):";
                    lblMaxX.Text = "Üst Sınır (b):";
                    break;
                case 2:
                    lblMethodDesc.Text = "💡 Türev almadan iki nokta arasındaki kiriş eğimi ile yaklaşır. İki başlangıç tahmini (x₀, x₁) ister.";
                    lblMinX.Text = "Başlangıç 1 (x₀):";
                    lblMaxX.Text = "Başlangıç 2 (x₁):";
                    break;
                case 3:
                    lblMethodDesc.Text = "💡 Doğrusal interpolasyon ile aralığı daraltarak köke ulaşır. [a, b] aralığı ister.";
                    lblMinX.Text = "Alt Sınır (a):";
                    lblMaxX.Text = "Üst Sınır (b):";
                    break;
                case 4:
                    lblMethodDesc.Text = "🎯 [a, b] aralığını 500 adımlı ızgara taramasıyla inceler; aralıktaki TÜM kökleri yüksek hassasiyetle bulur.";
                    lblMinX.Text = "Arama Alt Sınırı (a):";
                    lblMaxX.Text = "Arama Üst Sınırı (b):";
                    break;
            }
        }

        private decimal ParseDecimal(string input, string fieldName)
        {
            string norm = (input ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal val))
                return val;
            throw new ArgumentException($"'{fieldName}' alanı için geçerli bir sayı giriniz.");
        }

        private async void BtnFindRoot_Click(object sender, EventArgs e)
        {
            string origBtnText = btnFindRoot.Text;
            btnFindRoot.Enabled = false;
            btnFindRoot.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string eq = txtEquation.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    txtResult.Text = "Lütfen bir denklem giriniz (Örn: x^3 - x - 2).";
                    lblRootValue.Text = "x ≈ —";
                    lblRootDetails.Text = "Lütfen denklem giriniz.";
                    return;
                }

                string variable = string.IsNullOrWhiteSpace(txtVariable.Text) ? "x" : txtVariable.Text.Trim();
                if (!int.TryParse(txtIterations.Text.Trim(), out int iterCount) || iterCount < 1)
                    iterCount = 50;

                int idx = cmbMethod.SelectedIndex;
                string initPtText = txtInitialPoint.Text;
                string minXText = txtMinX.Text;
                string maxXText = txtMaxX.Text;

                if (idx == 4) // Find All Roots in Interval
                {
                    decimal minX = ParseDecimal(minXText, "Arama Alt Sınırı (a)");
                    decimal maxX = ParseDecimal(maxXText, "Arama Üst Sınırı (b)");

                    var allRoots = await Task.Run(() =>
                    {
                        bool prevRad = Function.UseRadians;
                        Function.UseRadians = true;
                        try
                        {
                            return NumericalMethods.FindAllRoots(eq, minX, maxX, variable);
                        }
                        finally
                        {
                            Function.UseRadians = prevRad;
                        }
                    });

                    if (allRoots.Count == 0)
                    {
                        txtResult.Text = $"[{minX}, {maxX}] aralığında kök bulunamadı.";
                        lblRootValue.Text = "Kök Bulunamadı";
                        lblRootValue.ForeColor = Color.FromArgb(239, 68, 68);
                        lblRootDetails.Text = $"[{minX}, {maxX}] aralığında f({variable}) = 0 sağlayan kök bulunamadı.";
                    }
                    else
                    {
                        var rootLines = new List<string>
                        {
                            $"=== [{minX}, {maxX}] ARALIĞINDA BULUNAN TÜM KÖKLER ===",
                            $"Denklem           : f({variable}) = {eq}",
                            $"Tarama Aralığı    : [{minX}, {maxX}]",
                            $"Bulunan Kök Sayısı: {allRoots.Count} adet",
                            new string('-', 55)
                        };

                        for (int k = 0; k < allRoots.Count; k++)
                        {
                            string rStr = allRoots[k].Root.ToString("0.##########", CultureInfo.InvariantCulture);
                            string fStr = allRoots[k].FVal.ToString("G5", CultureInfo.InvariantCulture);
                            rootLines.Add($"  {k + 1}) {variable}_{k + 1} = {rStr,-16}  =>  f({variable}_{k + 1}) = {fStr}");
                        }

                        txtResult.Text = string.Join(Environment.NewLine, rootLines);

                        string heroText = string.Join(",  ", allRoots.Take(4).Select((r, k) => $"{variable}_{k + 1} ≈ {r.Root.ToString("0.#####", CultureInfo.InvariantCulture)}"));
                        if (allRoots.Count > 4) heroText += $" ... (+{allRoots.Count - 4} kök daha)";

                        lblRootValue.Text = heroText;
                        lblRootValue.ForeColor = Color.FromArgb(52, 211, 153);
                        lblRootDetails.Text = $"🎯 [{minX}, {maxX}] aralığında toplam {allRoots.Count} adet reel kök başarıyla bulundu.";
                    }
                    return;
                }

                var (logs, finalRoot) = await Task.Run(() =>
                {
                    List<string> stepLogs;
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        if (idx == 0) // Newton-Raphson
                        {
                            decimal initialP = ParseDecimal(initPtText, "Başlangıç Noktası (x₀)");
                            stepLogs = NumericalMethods.Method_NewtonRaphson(eq, initialP, iterCount, variable);
                        }
                        else if (idx == 1) // Bisection
                        {
                            decimal minX = ParseDecimal(minXText, "Alt Sınır (a)");
                            decimal maxX = ParseDecimal(maxXText, "Üst Sınır (b)");
                            stepLogs = NumericalMethods.Method_Bisection(eq, minX, maxX, iterCount, variable);
                        }
                        else if (idx == 2) // Secant
                        {
                            decimal minX = ParseDecimal(minXText, "Başlangıç 1 (x₀)");
                            decimal maxX = ParseDecimal(maxXText, "Başlangıç 2 (x₁)");
                            stepLogs = NumericalMethods.Method_Secant(eq, minX, maxX, iterCount, variable);
                        }
                        else // Regula Falsi
                        {
                            decimal minX = ParseDecimal(minXText, "Alt Sınır (a)");
                            decimal maxX = ParseDecimal(maxXText, "Üst Sınır (b)");
                            stepLogs = NumericalMethods.Method_RegulaFalsi(eq, minX, maxX, iterCount, variable);
                        }
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }

                    string rootFound = "";
                    for (int i = stepLogs.Count - 1; i >= 0; i--)
                    {
                        string line = stepLogs[i];
                        if (line.Contains("BULUNAN KÖK") || line.Contains("Root found:") || line.Contains($"{variable} =") || line.Contains("x ="))
                        {
                            rootFound = line;
                            break;
                        }
                    }

                    return (stepLogs, rootFound);
                });

                txtResult.Text = string.Join(Environment.NewLine, logs);

                if (!string.IsNullOrEmpty(finalRoot))
                {
                    string displayRoot = finalRoot.Replace(">>>", "").Replace("BULUNAN KÖK:", "").Replace("Root found:", "").Trim();
                    int parenIdx = displayRoot.IndexOf('(');
                    if (parenIdx > 0) displayRoot = displayRoot.Substring(0, parenIdx).Trim();
                    lblRootValue.Text = displayRoot;
                    lblRootDetails.Text = $"✅ {logs.Count} iterasyon adımında başarıyla hesaplandı.";
                    lblRootValue.ForeColor = Color.FromArgb(52, 211, 153);
                }
                else if (logs.Count > 0)
                {
                    lblRootValue.Text = "Tamamlandı";
                    lblRootDetails.Text = $"İterasyonlar tamamlandı ({logs.Count} adım).";
                    lblRootValue.ForeColor = Color.FromArgb(56, 189, 248);
                }
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
                lblRootValue.Text = "Hata Oluştu";
                lblRootValue.ForeColor = Color.FromArgb(239, 68, 68);
                lblRootDetails.Text = ex.Message;
            }
            finally
            {
                btnFindRoot.Enabled = true;
                btnFindRoot.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }
    }
}
