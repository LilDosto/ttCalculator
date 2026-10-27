using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DotNetMatrix;

namespace MathApp.Controls
{
    public partial class CombinatoricsControl : UserControl
    {
        public CombinatoricsControl()
        {
            InitializeComponent();

            btnCalcComb.Click += BtnCalcComb_Click;
            btnPascal.Click += BtnPascal_Click;
            btnGenStrings.Click += BtnGenStrings_Click;
            btnPolarConvert.Click += BtnPolarConvert_Click;
            btnSpecialCalc.Click += BtnSpecialCalc_Click;

            btnCopyOutput.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtOutput.Text))
                {
                    Clipboard.SetText(txtOutput.Text);
                    MessageBox.Show("Sonuç raporu panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            txtCoordX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPolarConvert_Click(this, EventArgs.Empty); } };
            txtCoordY.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPolarConvert_Click(this, EventArgs.Empty); } };
            txtSpecialX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSpecialCalc_Click(this, EventArgs.Empty); } };
            txtBase.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSpecialCalc_Click(this, EventArgs.Empty); } };
            txtAlphabet.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnGenStrings_Click(this, EventArgs.Empty); } };
        }

        private void BtnCalcComb_Click(object sender, EventArgs e)
        {
            try
            {
                int n = (int)numN.Value;
                int r = (int)numR.Value;

                if (r > n)
                {
                    txtOutput.Text = "Hata: r (k) değeri n değerinden büyük olamaz!";
                    return;
                }

                string factNStr = n <= 27 ? NumericalMethods.Factorial(n).ToString("N0") : $"{n}! (decimal sınırını aşıyor: > 7.9×10²⁸)";
                string factRStr = r <= 27 ? NumericalMethods.Factorial(r).ToString("N0") : $"{r}! (decimal sınırını aşıyor: > 7.9×10²⁸)";
                decimal perm = NumericalMethods.Permutation(n, r);
                decimal comb = NumericalMethods.Combination(n, r);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== KOMBINATORIK & PERMUTASYON HESABI ===");
                sb.AppendLine($"n = {n},  r (k) = {r}");
                sb.AppendLine();
                sb.AppendLine($"1. Faktöriyel n! :");
                sb.AppendLine($"   {factNStr}");
                sb.AppendLine();
                sb.AppendLine($"2. Faktöriyel r! :");
                sb.AppendLine($"   {factRStr}");
                sb.AppendLine();
                sb.AppendLine($"3. Permütasyon P(n, r) = n! / (n - r)! :");
                sb.AppendLine($"   P({n}, {r}) = {perm:N0}");
                sb.AppendLine();
                sb.AppendLine($"4. Kombinasyon C(n, r) = n! / [r! * (n - r)!] :");
                sb.AppendLine($"   C({n}, {r}) = {comb:N0}");
                sb.AppendLine();
                sb.AppendLine($"Olasılık Bilgisi:");
                sb.AppendLine($"   {n} elemanlı bir kümeden {r} eleman {comb:N0} farklı şekilde seçilebilir.");
                sb.AppendLine($"   Sıralı seçim (dizilim) sayısı ise {perm:N0}'dir.");

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Hesaplama Hatası: " + ex.Message;
            }
        }

        private void BtnPascal_Click(object sender, EventArgs e)
        {
            try
            {
                int n = (int)numN.Value;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== PASCAL UCGENI & BINOM KATSAYILARI (Derece n = {n}) ===");
                sb.AppendLine($"(a + b)^{n} açılımının katsayıları:");
                sb.AppendLine();

                for (int row = 0; row <= Math.Min(n, 25); row++)
                {
                    sb.Append($"n = {row,2} | ");
                    for (int k = 0; k <= row; k++)
                    {
                        decimal c = NumericalMethods.Combination(row, k);
                        sb.Append(c.ToString("N0") + (k < row ? ", " : ""));
                    }
                    sb.AppendLine();
                }

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Hata: " + ex.Message;
            }
        }

        private void BtnGenStrings_Click(object sender, EventArgs e)
        {
            try
            {
                string alphabet = txtAlphabet.Text;
                string prefix = txtPrefix.Text;
                int length = (int)numLength.Value;

                if (string.IsNullOrEmpty(alphabet))
                {
                    txtOutput.Text = "Lütfen karakter kümesini (alfabe) giriniz.";
                    return;
                }

                long totalCount = (long)Math.Pow(alphabet.Length, length);
                if (totalCount > 20000)
                {
                    DialogResult dr = MessageBox.Show(
                        $"Üretilecek toplam kombinasyon sayısı {totalCount:N0}. " +
                        "Bu işlem arayüzü yavaşlatabilir. Devam etmek istiyor musunuz?",
                        "Büyük Kombinasyon Uyarısı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (dr != DialogResult.Yes) return;
                }

                string generated = NumericalMethods.GenerateCombinations(alphabet, prefix, length);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== KARAKTER KOMBINASYON URETECI ===");
                sb.AppendLine($"Alfabe        : '{alphabet}' (Uzunluk: {alphabet.Length})");
                sb.AppendLine($"Önek (Prefix) : '{prefix}'");
                sb.AppendLine($"Dizilim Boyutu: {length}");
                sb.AppendLine($"Toplam Sonuç  : {totalCount:N0} adet");
                sb.AppendLine(new string('-', 45));
                sb.Append(generated);

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Kombinasyon Üretim Hatası: " + ex.Message;
            }
        }

        private void BtnPolarConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtCoordX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal x) ||
                    !decimal.TryParse(txtCoordY.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal y))
                {
                    txtOutput.Text = "Lütfen geçerli X ve Y sayısal koordinatları giriniz.";
                    return;
                }

                // Maths.Hypot and DecimalMath.Atan2
                decimal rHypot = Maths.Hypot(x, y);
                decimal r = DecimalMath.Sqrt(x * x + y * y);
                decimal thetaRad = DecimalMath.Atan2(y, x);
                decimal thetaDeg = thetaRad * (180m / DecimalMath.PI);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== 2B VEKTOR & KUTUPSAL (POLAR) KOORDINAT HESABI ===");
                sb.AppendLine($"Kartezyen Koordinatlar : X = {x},  Y = {y}");
                sb.AppendLine();
                sb.AppendLine($"1. Hipotenüs / Yarıçap (r) = sqrt(X² + Y²):");
                sb.AppendLine($"   r (DecimalMath)     = {r:G18}");
                sb.AppendLine($"   r (Maths.Hypot)     = {rHypot:G18}");
                sb.AppendLine();
                sb.AppendLine($"2. Açı θ = Atan2(Y, X):");
                sb.AppendLine($"   θ (Radyan)          = {thetaRad:G18} rad");
                sb.AppendLine($"   θ (Derece)          = {thetaDeg:G18}°");
                sb.AppendLine();
                sb.AppendLine($"Kutupsal Gösterim      : (r = {r:G8}, θ = {thetaDeg:F4}°)");

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Koordinat Dönüşüm Hatası: " + ex.Message;
            }
        }

        private void BtnSpecialCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtSpecialX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal x))
                {
                    txtOutput.Text = "Lütfen x için geçerli bir sayı giriniz.";
                    return;
                }

                decimal baseN = 2m;
                if (!string.IsNullOrWhiteSpace(txtBase.Text))
                {
                    decimal.TryParse(txtBase.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out baseN);
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== OZEL FONKSIYON HESAPLAMALARI (x = {x}) ===");
                sb.AppendLine();

                // Hiperbolik Fonksiyonlar
                sb.AppendLine("-- Hiperbolik Fonksiyonlar --");
                try { sb.AppendLine($"   sinh(x) = {DecimalMath.Sinh(x):G16}"); } catch (Exception ex) { sb.AppendLine($"   sinh(x): {ex.Message}"); }
                try { sb.AppendLine($"   cosh(x) = {DecimalMath.Cosh(x):G16}"); } catch (Exception ex) { sb.AppendLine($"   cosh(x): {ex.Message}"); }
                try { sb.AppendLine($"   tanh(x) = {DecimalMath.Tanh(x):G16}"); } catch (Exception ex) { sb.AppendLine($"   tanh(x): {ex.Message}"); }
                sb.AppendLine();

                // Ters Trigonometrik
                sb.AppendLine("-- Ters Trigonometrik Fonksiyonlar --");
                if (Math.Abs(x) <= 1m)
                {
                    try { sb.AppendLine($"   asin(x) = {DecimalMath.Asin(x):G16} rad ({(DecimalMath.Asin(x) * 180m / DecimalMath.PI):F4}°)"); } catch { }
                    try { sb.AppendLine($"   acos(x) = {DecimalMath.Acos(x):G16} rad ({(DecimalMath.Acos(x) * 180m / DecimalMath.PI):F4}°)"); } catch { }
                }
                else
                {
                    sb.AppendLine("   asin(x) & acos(x): Tanımsız (|x| <= 1 olmalıdır)");
                }
                try { sb.AppendLine($"   atan(x) = {DecimalMath.Atan(x):G16} rad ({(DecimalMath.Atan(x) * 180m / DecimalMath.PI):F4}°)"); } catch { }
                sb.AppendLine();

                // Logaritmalar & Üs
                sb.AppendLine("-- Üstel & Logaritmik --");
                try { sb.AppendLine($"   exp(x)  = e^{x} = {DecimalMath.Exp(x):G16}"); } catch (Exception ex) { sb.AppendLine($"   exp(x): {ex.Message}"); }
                if (x > 0m)
                {
                    try { sb.AppendLine($"   ln(x)   = {DecimalMath.Log(x):G16}"); } catch { }
                    try { sb.AppendLine($"   log10(x)= {DecimalMath.Log10(x):G16}"); } catch { }
                    if (baseN > 0m && baseN != 1m)
                    {
                        try
                        {
                            decimal logBaseN = DecimalMath.Log(x) / DecimalMath.Log(baseN);
                            sb.AppendLine($"   log_{baseN}(x) = {logBaseN:G16}");
                        }
                        catch { }
                    }
                }
                else
                {
                    sb.AppendLine("   ln(x), log10(x): Tanımsız (x > 0 olmalıdır)");
                }

                txtOutput.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtOutput.Text = "Hata: " + ex.Message;
            }
        }
    }
}
