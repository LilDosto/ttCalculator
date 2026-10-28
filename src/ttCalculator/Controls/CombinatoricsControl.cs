using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetMatrix;

namespace MathApp.Controls
{
    public partial class CombinatoricsControl : UserControl
    {
        // Vector Preview State
        private float _lastVecX = 4f;
        private float _lastVecY = 3f;
        private float _lastVecR = 5f;
        private float _lastVecThetaDeg = 36.87f;
        private bool _hasVector = false;

        // Pagination for Strings Generator
        private List<string> _generatedStrings = new List<string>();
        private int _currentPage = 1;
        private const int PageSize = 1000;

        // Current JSON export object
        private readonly Dictionary<string, object> _currentReportData = new Dictionary<string, object>();

        public CombinatoricsControl()
        {
            InitializeComponent();
            ThemeHelper.StyleModernTabControl(tabOutput);

            // Double buffering on vector canvas
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pnlVectorCanvas, new object[] { true });

            pnlVectorCanvas.Paint += PnlVectorCanvas_Paint;

            btnCalcComb.Click += BtnCalcComb_Click;
            btnPascal.Click += BtnPascal_Click;
            btnGenStrings.Click += BtnGenStrings_Click;
            btnPolarConvert.Click += BtnPolarConvert_Click;
            btnSpecialCalc.Click += BtnSpecialCalc_Click;

            btnCopyOutput.Click += BtnCopyOutput_Click;
            btnExportJson.Click += BtnExportJson_Click;
            btnClearOutput.Click += BtnClearOutput_Click;

            btnPrevPage.Click += (s, e) => { if (_currentPage > 1) { _currentPage--; RenderStringsPage(); } };
            btnNextPage.Click += (s, e) => { if (_currentPage * PageSize < _generatedStrings.Count) { _currentPage++; RenderStringsPage(); } };

            txtCoordX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPolarConvert_Click(this, EventArgs.Empty); } };
            txtCoordY.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPolarConvert_Click(this, EventArgs.Empty); } };
            txtSpecialX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSpecialCalc_Click(this, EventArgs.Empty); } };
            txtBase.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnSpecialCalc_Click(this, EventArgs.Empty); } };
            txtAlphabet.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnGenStrings_Click(this, EventArgs.Empty); } };

            // Default run polar vector
            BtnPolarConvert_Click(this, EventArgs.Empty);
        }

        #region 1. Combinatorics & Permutations

        private void BtnCalcComb_Click(object sender, EventArgs e)
        {
            try
            {
                int n = (int)numN.Value;
                int r = (int)numR.Value;

                if (r > n)
                {
                    MessageBox.Show("r (seçim sayısı) değeri n (eleman sayısı) değerinden büyük olamaz!", "Hatalı Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string factNStr = n <= 27 ? NumericalMethods.Factorial(n).ToString("N0") : $"{n}! (> 7.9×10²⁸)";
                string factRStr = r <= 27 ? NumericalMethods.Factorial(r).ToString("N0") : $"{r}! (> 7.9×10²⁸)";
                string factNRStr = (n - r) <= 27 ? NumericalMethods.Factorial(n - r).ToString("N0") : $"{n - r}! (> 7.9×10²⁸)";

                decimal perm = NumericalMethods.Permutation(n, r);
                decimal comb = NumericalMethods.Combination(n, r);

                lblKpiTitle.Text = $"KOMBİNASYON C({n}, {r}) & PERMÜTASYON P({n}, {r})";
                lblKpiValue.Text = $"C({n}, {r}) = {comb:N0}";
                lblKpiSubtitle.Text = $"Sıralı Dizilim Sayısı P({n}, {r}) = {perm:N0}  •  Faktöriyel: {n}! = {factNStr}";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=======================================================================");
                sb.AppendLine("  AYRIK MATEMATİK & OLASILIK HESAPLAMA ADIMLARI");
                sb.AppendLine("=======================================================================");
                sb.AppendLine($"  Girdi Parametreleri : n = {n} (Küme boyutu),  r = {r} (Seçilen eleman)");
                sb.AppendLine();
                sb.AppendLine("  1. FAKTÖRİYEL ADIMLARI:");
                sb.AppendLine($"     • n!     = {n}! = {factNStr}");
                sb.AppendLine($"     • r!     = {r}! = {factRStr}");
                sb.AppendLine($"     • (n-r)! = ({n}-{r})! = {factNRStr}");
                sb.AppendLine();
                sb.AppendLine("  2. PERMÜTASYON (Sıralı Seçim - Order Matters):");
                sb.AppendLine("     Formül: P(n, r) = n! / (n - r)!");
                sb.AppendLine($"     Hesap : P({n}, {r}) = {factNStr} / {factNRStr}");
                sb.AppendLine($"     Sonuç : P({n}, {r}) = {perm:N0}");
                sb.AppendLine();
                sb.AppendLine("  3. KOMBİNASYON (Sırasız Seçim / Alt Küme - Order Doesn't Matter):");
                sb.AppendLine("     Formül: C(n, r) = n! / [ r! · (n - r)! ]");
                sb.AppendLine($"     Hesap : C({n}, {r}) = {factNStr} / [ {factRStr} · {factNRStr} ]");
                sb.AppendLine($"     Sonuç : C({n}, {r}) = {comb:N0}");
                sb.AppendLine();
                sb.AppendLine("  4. BİNOM TEOREMİ & OLASILIK YORUMU:");
                sb.AppendLine($"     • {n} elemanlı bir kümeden {r} elemanlık {comb:N0} farklı alt küme oluşturulabilir.");
                sb.AppendLine($"     • (a + b)^{n} binom açılımında a^{n - r} · b^{r} teriminin katsayısı {comb:N0}'dir.");
                sb.AppendLine($"     • Rastgele seçilen r elemanın belirli bir sırada dizilme olasılığı: 1 / {perm:N0}");

                txtSummaryDetails.Text = sb.ToString();
                tabOutput.SelectedTab = tabSummary;

                // Update report data
                _currentReportData["type"] = "Combinatorics";
                _currentReportData["n"] = n;
                _currentReportData["r"] = r;
                _currentReportData["combination"] = comb.ToString(CultureInfo.InvariantCulture);
                _currentReportData["permutation"] = perm.ToString(CultureInfo.InvariantCulture);
                _currentReportData["factorial_n"] = factNStr;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hesaplama hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnPascal_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                int n = (int)numN.Value;
                int maxRow = Math.Min(n, 30);

                string result = await Task.Run(() =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"=======================================================================");
                    sb.AppendLine($"  PASCAL ÜÇGENİ & BİNOM KATSAYILARI (Derece n = {n})");
                    sb.AppendLine($"  (a + b)^n = ∑ [ C(n, k) · a^(n-k) · b^k ]");
                    sb.AppendLine($"=======================================================================");
                    sb.AppendLine();

                    for (int row = 0; row <= maxRow; row++)
                    {
                        sb.Append($"  n = {row,2} │ ");
                        List<string> rowVals = new List<string>();
                        for (int k = 0; k <= row; k++)
                        {
                            decimal c = NumericalMethods.Combination(row, k);
                            rowVals.Add(c.ToString("N0"));
                        }
                        sb.AppendLine(string.Join("  ", rowVals));
                    }

                    if (n > 30)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"  (Not: Tablo taşmasını önlemek amacıyla ilk 30 satır gösterilmektedir.)");
                    }

                    return sb.ToString();
                });

                txtPascalView.Text = result;
                tabOutput.SelectedTab = tabPascal;

                lblKpiTitle.Text = $"PASCAL ÜÇGENİ";
                lblKpiValue.Text = $"(a + b)^{n} Açılımı";
                lblKpiSubtitle.Text = $"Toplam {n + 1} terim katsayısı Pascal sekmesinde gösterilmektedir.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Pascal üçgeni hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region 2. String Generator with Asynchronous Chunking & Pagination

        private async void BtnGenStrings_Click(object sender, EventArgs e)
        {
            string alphabet = txtAlphabet.Text;
            string prefix = txtPrefix.Text;
            int length = (int)numLength.Value;

            if (string.IsNullOrEmpty(alphabet))
            {
                MessageBox.Show("Lütfen karakter kümesini (alfabe) giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long totalCount = (long)Math.Pow(alphabet.Length, length);
            if (totalCount > 500000)
            {
                DialogResult dr = MessageBox.Show(
                    $"Üretilecek toplam kombinasyon sayısı {totalCount:N0}.\n" +
                    "Bu işlem yüksek bellek tüketebilir. Devam etmek istiyor musunuz?",
                    "Büyük Veri Güvenlik Uyarısı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dr != DialogResult.Yes) return;
            }

            btnGenStrings.Enabled = false;
            pbStrings.Visible = true;
            pbStrings.Style = ProgressBarStyle.Marquee;
            Cursor = Cursors.WaitCursor;

            try
            {
                _generatedStrings = await Task.Run(() =>
                {
                    List<string> list = new List<string>((int)Math.Min(totalCount, 500000));
                    void Recurse(string cur, int rem)
                    {
                        if (rem == 0)
                        {
                            list.Add(cur);
                            return;
                        }
                        for (int i = 0; i < alphabet.Length; i++)
                        {
                            Recurse(cur + alphabet[i], rem - 1);
                        }
                    }
                    Recurse(prefix, length);
                    return list;
                });

                _currentPage = 1;
                RenderStringsPage();
                tabOutput.SelectedTab = tabStrings;

                lblKpiTitle.Text = "DİZİLİM ÜRETECİ SONUCU";
                lblKpiValue.Text = $"{_generatedStrings.Count:N0} Kombinasyon";
                lblKpiSubtitle.Text = $"Alfabe: '{alphabet}' • Uzunluk: {length} • Önek: '{prefix}'";

                _currentReportData["type"] = "StringCombinations";
                _currentReportData["alphabet"] = alphabet;
                _currentReportData["length"] = length;
                _currentReportData["total_count"] = _generatedStrings.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dizilim üretim hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pbStrings.Visible = false;
                btnGenStrings.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void RenderStringsPage()
        {
            if (_generatedStrings.Count == 0)
            {
                txtStringsView.Text = "Henüz dizilim üretilmedi.";
                lblPageInfo.Text = "Sayfa: 0 / 0 (Toplam 0)";
                btnPrevPage.Enabled = false;
                btnNextPage.Enabled = false;
                return;
            }

            int totalPages = (int)Math.Ceiling(_generatedStrings.Count / (double)PageSize);
            _currentPage = Math.Max(1, Math.Min(_currentPage, totalPages));

            int startIdx = (_currentPage - 1) * PageSize;
            int count = Math.Min(PageSize, _generatedStrings.Count - startIdx);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"=== DİZİLİM SAYFASI: {_currentPage} / {totalPages} (Gösterilen: {startIdx + 1} - {startIdx + count} / Toplam {_generatedStrings.Count:N0}) ===");
            sb.AppendLine();

            int col = 0;
            for (int i = 0; i < count; i++)
            {
                sb.Append($"{_generatedStrings[startIdx + i],-16}");
                col++;
                if (col % 5 == 0) sb.AppendLine();
            }
            if (col % 5 != 0) sb.AppendLine();

            txtStringsView.Text = sb.ToString();
            lblPageInfo.Text = $"Sayfa: {_currentPage} / {totalPages}  (Toplam {_generatedStrings.Count:N0})";
            btnPrevPage.Enabled = _currentPage > 1;
            btnNextPage.Enabled = _currentPage < totalPages;
        }

        #endregion

        #region 3. Polar & Vector Coordinates & Mini Canvas

        private void BtnPolarConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtCoordX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal x) ||
                    !decimal.TryParse(txtCoordY.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal y))
                {
                    MessageBox.Show("Lütfen geçerli sayısal X ve Y koordinatları giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal rHypot = Maths.Hypot(x, y);
                decimal r = DecimalMath.Sqrt(x * x + y * y);
                decimal thetaRad = DecimalMath.Atan2(y, x);
                decimal thetaDeg = thetaRad * (180m / DecimalMath.PI);

                _lastVecX = (float)x;
                _lastVecY = (float)y;
                _lastVecR = (float)r;
                _lastVecThetaDeg = (float)thetaDeg;
                _hasVector = true;
                pnlVectorCanvas.Invalidate();

                lblKpiTitle.Text = "KUTUPSAL (POLAR) KOORDİNAT DÖNÜŞÜMÜ";
                lblKpiValue.Text = $"r = {r:F4},  θ = {thetaDeg:F2}°";
                lblKpiSubtitle.Text = $"Kartezyen: (X = {x}, Y = {y})  •  Açı: {thetaRad:F6} radyan";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=======================================================================");
                sb.AppendLine("  2B VEKTÖR & KUTUPSAL (POLAR) KOORDİNAT ANALİZİ");
                sb.AppendLine("=======================================================================");
                sb.AppendLine($"  Kartezyen Giriş : X = {x:G},  Y = {y:G}");
                sb.AppendLine();
                sb.AppendLine("  1. YARIÇAP / BÜYÜKLÜK (Norm - r):");
                sb.AppendLine("     Formül: r = √(X² + Y²)");
                sb.AppendLine($"     Hesap : r = √({x}² + {y}²) = {r:G18}");
                sb.AppendLine($"     Hypot : Maths.Hypot(X, Y)  = {rHypot:G18}");
                sb.AppendLine();
                sb.AppendLine("  2. KUTUPSAL AÇI (Argüman - θ):");
                sb.AppendLine("     Formül: θ = atan2(Y, X)");
                sb.AppendLine($"     Radyan: θ = {thetaRad:G18} rad");
                sb.AppendLine($"     Derece: θ = {thetaDeg:G18}°");
                sb.AppendLine();
                sb.AppendLine("  3. VEKTÖREL VE TRİGONOMETRİK BİLEŞENLER:");
                sb.AppendLine($"     • Birim Vektör u : ({((r > 0) ? (x / r) : 0):F4}, {((r > 0) ? (y / r) : 0):F4})");
                sb.AppendLine($"     • Kompleks Form  : z = {x} + {y}i = {r:F4} · e^(i·{thetaRad:F4})");
                sb.AppendLine($"     • Euler Formu    : {r:F4} · (cos({thetaDeg:F2}°) + i·sin({thetaDeg:F2}°))");

                txtSummaryDetails.Text = sb.ToString();
                tabOutput.SelectedTab = tabSummary;

                _currentReportData["type"] = "PolarCoordinates";
                _currentReportData["x"] = x.ToString(CultureInfo.InvariantCulture);
                _currentReportData["y"] = y.ToString(CultureInfo.InvariantCulture);
                _currentReportData["r"] = r.ToString(CultureInfo.InvariantCulture);
                _currentReportData["theta_rad"] = thetaRad.ToString(CultureInfo.InvariantCulture);
                _currentReportData["theta_deg"] = thetaDeg.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koordinat dönüşüm hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PnlVectorCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlVectorCanvas.Width;
            int h = pnlVectorCanvas.Height;

            // Background
            using (Brush bg = new SolidBrush(Color.FromArgb(15, 23, 42)))
            {
                g.FillRectangle(bg, 0, 0, w, h);
            }

            int cx = w / 2;
            int cy = h / 2;

            // Draw Grid & Axes
            using (Pen axisPen = new Pen(Color.FromArgb(51, 65, 85), 1.2f))
            {
                g.DrawLine(axisPen, 10, cy, w - 10, cy); // X axis
                g.DrawLine(axisPen, cx, 10, cx, h - 10); // Y axis
            }

            // Draw concentric target circles
            using (Pen circlePen = new Pen(Color.FromArgb(30, 41, 59), 1f) { DashStyle = DashStyle.Dash })
            {
                g.DrawEllipse(circlePen, cx - 35, cy - 35, 70, 70);
                g.DrawEllipse(circlePen, cx - 60, cy - 60, 120, 120);
            }

            if (!_hasVector) return;

            // Calculate scaled vector point
            float maxR = Math.Max(0.001f, _lastVecR);
            float scale = Math.Min(w, h) * 0.38f / maxR;
            float vx = cx + _lastVecX * scale;
            float vy = cy - _lastVecY * scale; // Y inverted for screen

            // Draw Angle Arc
            if (_lastVecR > 0.01f)
            {
                float arcRad = 26f;
                float startAngle = 0f;
                float sweepAngle = -_lastVecThetaDeg; // Screen coordinate angles clockwise
                using (Pen arcPen = new Pen(Color.FromArgb(56, 189, 248), 1.5f))
                {
                    g.DrawArc(arcPen, cx - arcRad, cy - arcRad, arcRad * 2, arcRad * 2, startAngle, sweepAngle);
                }
            }

            // Draw Vector Arrow
            using (Pen vecPen = new Pen(Color.FromArgb(52, 211, 153), 2.5f))
            {
                vecPen.CustomEndCap = new AdjustableArrowCap(5, 6);
                g.DrawLine(vecPen, cx, cy, vx, vy);
            }

            // Vector terminal dot
            using (Brush dotBrush = new SolidBrush(Color.FromArgb(250, 204, 21)))
            {
                g.FillEllipse(dotBrush, vx - 4, vy - 4, 8, 8);
            }

            // Coordinate labels
            using (Font f = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
            using (Brush textBr = new SolidBrush(Color.White))
            using (Brush cyanBr = new SolidBrush(Color.FromArgb(56, 189, 248)))
            using (Brush greenBr = new SolidBrush(Color.FromArgb(52, 211, 153)))
            {
                g.DrawString($"P({_lastVecX:F1}, {_lastVecY:F1})", f, textBr, vx + 6, vy - 14);
                g.DrawString($"r = {_lastVecR:F2}", f, greenBr, 8, 8);
                g.DrawString($"θ = {_lastVecThetaDeg:F1}°", f, cyanBr, 8, 24);
            }
        }

        #endregion

        #region 4. Special & Hyperbolic Functions

        private void BtnSpecialCalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtSpecialX.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal x))
                {
                    MessageBox.Show("Lütfen x için geçerli bir sayı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal baseN = 2m;
                if (!string.IsNullOrWhiteSpace(txtBase.Text))
                {
                    decimal.TryParse(txtBase.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out baseN);
                }

                lblKpiTitle.Text = $"ÖZEL FONKSİYON ANALİZİ (x = {x})";
                lblKpiValue.Text = $"sinh(x) = {DecimalMath.Sinh(x):F4}, cosh(x) = {DecimalMath.Cosh(x):F4}";
                lblKpiSubtitle.Text = $"tanh(x) = {DecimalMath.Tanh(x):F4}  •  exp(x) = {DecimalMath.Exp(x):F4}  •  log_{baseN}(x)";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=======================================================================");
                sb.AppendLine($"  İLERİ ANALİTİK & ÖZEL FONKSİYON TABLOSU (x = {x})");
                sb.AppendLine("=======================================================================");
                sb.AppendLine();

                // Hiperbolik
                sb.AppendLine("  1. HİPERBOLİK FONKSİYONLAR:");
                try { sb.AppendLine($"     • sinh(x) = (e^x - e^(-x)) / 2  = {DecimalMath.Sinh(x):G18}"); } catch (Exception ex) { sb.AppendLine($"     • sinh(x): {ex.Message}"); }
                try { sb.AppendLine($"     • cosh(x) = (e^x + e^(-x)) / 2  = {DecimalMath.Cosh(x):G18}"); } catch (Exception ex) { sb.AppendLine($"     • cosh(x): {ex.Message}"); }
                try { sb.AppendLine($"     • tanh(x) = sinh(x) / cosh(x)    = {DecimalMath.Tanh(x):G18}"); } catch (Exception ex) { sb.AppendLine($"     • tanh(x): {ex.Message}"); }
                sb.AppendLine();

                // Ters Trigonometrik
                sb.AppendLine("  2. TERS TRİGONOMETRİK FONKSİYONLAR (Arc-functions):");
                if (Math.Abs(x) <= 1m)
                {
                    try { decimal a = DecimalMath.Asin(x); sb.AppendLine($"     • asin(x) = {a:G16} rad ({(a * 180m / DecimalMath.PI):F4}°)"); } catch { }
                    try { decimal a = DecimalMath.Acos(x); sb.AppendLine($"     • acos(x) = {a:G16} rad ({(a * 180m / DecimalMath.PI):F4}°)"); } catch { }
                }
                else
                {
                    sb.AppendLine("     • asin(x) & acos(x): Tanımsız (|x| ≤ 1 olmalıdır)");
                }
                try { decimal a = DecimalMath.Atan(x); sb.AppendLine($"     • atan(x) = {a:G16} rad ({(a * 180m / DecimalMath.PI):F4}°)"); } catch { }
                sb.AppendLine();

                // Logaritmalar
                sb.AppendLine("  3. ÜSTEL VE LOGARİTMİK FONKSİYONLAR:");
                try { sb.AppendLine($"     • exp(x)   = e^{x}  = {DecimalMath.Exp(x):G18}"); } catch (Exception ex) { sb.AppendLine($"     • exp(x): {ex.Message}"); }
                if (x > 0m)
                {
                    try { sb.AppendLine($"     • ln(x)    = log_e(x) = {DecimalMath.Log(x):G18}"); } catch { }
                    try { sb.AppendLine($"     • log10(x) = log_10(x)= {DecimalMath.Log10(x):G18}"); } catch { }
                    if (baseN > 0m && baseN != 1m)
                    {
                        try
                        {
                            decimal logBaseN = DecimalMath.Log(x) / DecimalMath.Log(baseN);
                            sb.AppendLine($"     • log_{baseN}(x) = ln(x) / ln({baseN}) = {logBaseN:G18}");
                        }
                        catch { }
                    }
                }
                else
                {
                    sb.AppendLine("     • ln(x) & log(x): Tanımsız (x > 0 olmalıdır)");
                }

                txtSummaryDetails.Text = sb.ToString();
                tabOutput.SelectedTab = tabSummary;

                _currentReportData["type"] = "SpecialFunctions";
                _currentReportData["x"] = x.ToString(CultureInfo.InvariantCulture);
                _currentReportData["sinh"] = DecimalMath.Sinh(x).ToString(CultureInfo.InvariantCulture);
                _currentReportData["cosh"] = DecimalMath.Cosh(x).ToString(CultureInfo.InvariantCulture);
                _currentReportData["tanh"] = DecimalMath.Tanh(x).ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Özel fonksiyon hesabı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Toolbar Actions (Copy, Export JSON, Clear)

        private void BtnCopyOutput_Click(object sender, EventArgs e)
        {
            string textToCopy = "";
            if (tabOutput.SelectedTab == tabSummary) textToCopy = txtSummaryDetails.Text;
            else if (tabOutput.SelectedTab == tabPascal) textToCopy = txtPascalView.Text;
            else if (tabOutput.SelectedTab == tabStrings) textToCopy = txtStringsView.Text;

            if (!string.IsNullOrEmpty(textToCopy))
            {
                Clipboard.SetText(textToCopy);
                MessageBox.Show("Rapor metni panoya kopyalandı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnExportJson_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "JSON Dosyası (*.json)|*.json",
                FileName = "ttCalculator_Kombinatorik_Rapor.json",
                Title = "Hesaplama Sonuçlarını JSON Olarak Kaydet"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder json = new StringBuilder();
                        json.AppendLine("{");
                        json.AppendLine($"  \"timestamp\": \"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\",");
                        json.AppendLine($"  \"app\": \"ttCalculator v3.4\",");
                        json.AppendLine("  \"data\": {");
                        int idx = 0;
                        foreach (var kv in _currentReportData)
                        {
                            json.Append($"    \"{kv.Key}\": \"{kv.Value}\"");
                            idx++;
                            if (idx < _currentReportData.Count) json.Append(",");
                            json.AppendLine();
                        }
                        json.AppendLine("  }");
                        json.AppendLine("}");

                        File.WriteAllText(sfd.FileName, json.ToString(), Encoding.UTF8);
                        MessageBox.Show("Rapor JSON formatında başarıyla kaydedildi:\n" + sfd.FileName, "Kayıt Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("JSON kaydetme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnClearOutput_Click(object sender, EventArgs e)
        {
            txtSummaryDetails.Clear();
            txtPascalView.Clear();
            txtStringsView.Clear();
            lblKpiValue.Text = "—";
            lblKpiTitle.Text = "SONUÇ DEĞERİ";
            lblKpiSubtitle.Text = "Hesaplama yapmak için sol paneldeki modüllerden birini çalıştırın.";
            _generatedStrings.Clear();
            _hasVector = false;
            pnlVectorCanvas.Invalidate();
        }

        #endregion
    }
}
