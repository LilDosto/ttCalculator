using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class RootFinderControl : UserControl
    {
        public class IterationStep
        {
            public int Step { get; set; }
            public decimal X { get; set; }
            public decimal Fx { get; set; }
            public decimal AbsError { get; set; }
            public decimal RelError { get; set; }
        }

        private List<IterationStep> _steps = new List<IterationStep>();
        private decimal? _lastRoot = null;
        private string _lastEquation = "";
        private decimal _targetTolerance = 1e-7m;

        public RootFinderControl()
        {
            InitializeComponent();
            ThemeHelper.StyleModernTabControl(tabResults);

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pnlConvergencePlot, new object[] { true });

            pnlConvergencePlot.Paint += PnlConvergencePlot_Paint;

            cmbMethod.SelectedIndex = 0;
            UpdateVisibility();
            cmbMethod.SelectedIndexChanged += (s, e) => UpdateVisibility();
            btnFindRoot.Click += BtnFindRoot_Click;

            btnEx1.Click += (s, e) => LoadPreset("x^3 - x - 2", "x", 0, "1.5", "1", "2");
            btnEx2.Click += (s, e) => LoadPreset("cos(x) - x", "x", 1, "0.5", "0", "1");
            btnEx3.Click += (s, e) => LoadPreset("exp(x) - 3*x", "x", 2, "0.5", "0", "1");

            btnPlotOnGraph.Click += BtnPlotOnGraph_Click;
            btnCopyResult.Click += BtnCopyResult_Click;

            txtEquation.KeyDown += Input_KeyDown;
            txtInitialPoint.KeyDown += Input_KeyDown;
            txtMinX.KeyDown += Input_KeyDown;
            txtMaxX.KeyDown += Input_KeyDown;
            txtTolerance.KeyDown += Input_KeyDown;
            txtIterations.KeyDown += Input_KeyDown;

            // Setup DataGridView Columns
            dgvIterations.Columns.Clear();
            dgvIterations.Columns.Add("Step", "Adım (k)");
            dgvIterations.Columns.Add("Xk", "x_k (Tahmin)");
            dgvIterations.Columns.Add("Fxk", "f(x_k)");
            dgvIterations.Columns.Add("AbsErr", "Mutlak Hata |Δx|");
            dgvIterations.Columns.Add("RelErr", "Bağıl Hata (%)");

            dgvIterations.Columns["Step"].Width = 80;
            dgvIterations.Columns["Xk"].Width = 180;
            dgvIterations.Columns["Fxk"].Width = 160;
            dgvIterations.Columns["AbsErr"].Width = 160;
            dgvIterations.Columns["RelErr"].Width = 120;
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
                    lblMethodDesc.Text = "💡 Newton-Raphson: Analitik teğet doğrusu ile kuadratik hızda köke ulaşır. Tek bir başlangıç (x₀) yeterlidir.";
                    break;
                case 1:
                    lblMethodDesc.Text = "💡 Bisection (İkiye Bölme): [a, b] aralığında f(a)·f(b) < 0 ise kökü bulmayı garanti eder. Kararlıdır.";
                    lblMinX.Text = "Alt Sınır (a):";
                    lblMaxX.Text = "Üst Sınır (b):";
                    break;
                case 2:
                    lblMethodDesc.Text = "💡 Secant (Kiriş): Türev gerektirmeden iki nokta (x₀, x₁) arasındaki kiriş doğrusuyla hızla yaklaşır.";
                    lblMinX.Text = "Başlangıç 1 (x₀):";
                    lblMaxX.Text = "Başlangıç 2 (x₁):";
                    break;
                case 3:
                    lblMethodDesc.Text = "💡 Regula Falsi: Doğrusal interpolasyonla kök sınırlarını daraltır. [a, b] kapalı aralığı ister.";
                    lblMinX.Text = "Alt Sınır (a):";
                    lblMaxX.Text = "Üst Sınır (b):";
                    break;
                case 4:
                    lblMethodDesc.Text = "🎯 Çoklu Kök Taraması: [a, b] aralığındaki tüm kökleri 500 adımlı ızgara ile otomatik tespit eder.";
                    lblMinX.Text = "Arama Altı (a):";
                    lblMaxX.Text = "Arama Üstü (b):";
                    break;
            }
        }

        private decimal ParseDecimal(string input, string fieldName, decimal defVal = 0m)
        {
            string norm = (input ?? "").Trim().Replace(',', '.');
            if (string.IsNullOrWhiteSpace(norm)) return defVal;
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
                    MessageBox.Show("Lütfen bir denklem giriniz (Örn: x^3 - x - 2).", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _lastEquation = eq;
                string varName = string.IsNullOrWhiteSpace(txtVariable.Text) ? "x" : txtVariable.Text.Trim();
                if (!int.TryParse(txtIterations.Text.Trim(), out int maxIter) || maxIter < 1) maxIter = 50;

                _targetTolerance = ParseDecimal(txtTolerance.Text, "Tolerans (ε)", 1e-7m);
                if (_targetTolerance <= 0m) _targetTolerance = 1e-7m;

                int methodIdx = cmbMethod.SelectedIndex;
                decimal x0 = ParseDecimal(txtInitialPoint.Text, "Başlangıç (x₀)", 1.5m);
                decimal minX = ParseDecimal(txtMinX.Text, "Alt Sınır (a)", 1m);
                decimal maxX = ParseDecimal(txtMaxX.Text, "Üst Sınır (b)", 2m);

                _steps.Clear();
                bool converged = false;
                decimal rootVal = 0m;
                string statusMessage = "";

                await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        Function fn = NumericalMethods.CompileFunction(eq);

                        decimal Eval(decimal xVal) => NumericalMethods.EvaluateCompiled(fn, varName, xVal);
                        decimal EvalDeriv(decimal xVal)
                        {
                            decimal h = 0.0001m;
                            return (Eval(xVal + h) - Eval(xVal - h)) / (2m * h);
                        }

                        // ==========================================
                        // Method 0: Newton-Raphson
                        // ==========================================
                        if (methodIdx == 0)
                        {
                            decimal curX = x0;
                            for (int k = 1; k <= maxIter; k++)
                            {
                                decimal fx = Eval(curX);
                                decimal dfx = EvalDeriv(curX);

                                if (Math.Abs(dfx) < 1e-12m)
                                {
                                    throw new InvalidOperationException($"Sıfıra bölme hatası: f'({curX:F4}) türevi sıfır veya sıfıra çok yakın oldu. Farklı bir başlangıç noktası (x₀) seçin.");
                                }

                                decimal nextX = curX - (fx / dfx);
                                decimal absErr = Math.Abs(nextX - curX);
                                decimal relErr = (Math.Abs(nextX) > 1e-10m) ? (absErr / Math.Abs(nextX)) * 100m : absErr * 100m;

                                _steps.Add(new IterationStep
                                {
                                    Step = k,
                                    X = nextX,
                                    Fx = Eval(nextX),
                                    AbsError = absErr,
                                    RelError = relErr
                                });

                                if (absErr < _targetTolerance || Math.Abs(Eval(nextX)) < _targetTolerance)
                                {
                                    converged = true;
                                    rootVal = nextX;
                                    statusMessage = "Toleransa ulaşıldı (Başarıyla Yakınsadı).";
                                    break;
                                }

                                curX = nextX;
                            }
                            if (!converged && _steps.Count > 0)
                            {
                                rootVal = _steps.Last().X;
                                statusMessage = "Maksimum iterasyonda toleransa ulaşılamadı (Divergence / Yavaş Yakınsama).";
                            }
                        }
                        // ==========================================
                        // Method 1: Bisection (İkiye Bölme)
                        // ==========================================
                        else if (methodIdx == 1)
                        {
                            decimal a = minX;
                            decimal b = maxX;
                            decimal fa = Eval(a);
                            decimal fb = Eval(b);

                            if (fa * fb > 0m)
                            {
                                statusMessage = $"Uyarı: f(a)={fa:F3} ve f(b)={fb:F3} aynı işaretli! [a, b] aralığında kök bulunmayabilir.";
                            }

                            decimal prevMid = a;
                            for (int k = 1; k <= maxIter; k++)
                            {
                                decimal mid = (a + b) / 2m;
                                decimal fmid = Eval(mid);
                                decimal absErr = Math.Abs(mid - prevMid);
                                decimal relErr = (Math.Abs(mid) > 1e-10m) ? (absErr / Math.Abs(mid)) * 100m : absErr * 100m;

                                _steps.Add(new IterationStep
                                {
                                    Step = k,
                                    X = mid,
                                    Fx = fmid,
                                    AbsError = (b - a) / 2m,
                                    RelError = relErr
                                });

                                if ((b - a) / 2m < _targetTolerance || Math.Abs(fmid) < _targetTolerance)
                                {
                                    converged = true;
                                    rootVal = mid;
                                    statusMessage = "Aralık tolerans altına indi (Başarıyla Yakınsadı).";
                                    break;
                                }

                                if (fa * fmid <= 0m)
                                {
                                    b = mid;
                                    fb = fmid;
                                }
                                else
                                {
                                    a = mid;
                                    fa = fmid;
                                }
                                prevMid = mid;
                            }
                            if (!converged && _steps.Count > 0)
                            {
                                rootVal = _steps.Last().X;
                                statusMessage = "Maksimum adım tamamlandı.";
                            }
                        }
                        // ==========================================
                        // Method 2: Secant (Kiriş)
                        // ==========================================
                        else if (methodIdx == 2)
                        {
                            decimal xPrev = minX; // x0
                            decimal xCur = maxX;  // x1

                            for (int k = 1; k <= maxIter; k++)
                            {
                                decimal fPrev = Eval(xPrev);
                                decimal fCur = Eval(xCur);

                                decimal denom = fCur - fPrev;
                                if (Math.Abs(denom) < 1e-12m)
                                {
                                    throw new InvalidOperationException("Kiriş eğimi sıfırlandı: f(x_k) - f(x_{k-1}) sıfıra çok yakın.");
                                }

                                decimal xNext = xCur - fCur * (xCur - xPrev) / denom;
                                decimal absErr = Math.Abs(xNext - xCur);
                                decimal relErr = (Math.Abs(xNext) > 1e-10m) ? (absErr / Math.Abs(xNext)) * 100m : absErr * 100m;

                                _steps.Add(new IterationStep
                                {
                                    Step = k,
                                    X = xNext,
                                    Fx = Eval(xNext),
                                    AbsError = absErr,
                                    RelError = relErr
                                });

                                if (absErr < _targetTolerance || Math.Abs(Eval(xNext)) < _targetTolerance)
                                {
                                    converged = true;
                                    rootVal = xNext;
                                    statusMessage = "Kiriş yöntemiyle yakınsama sağlandı.";
                                    break;
                                }

                                xPrev = xCur;
                                xCur = xNext;
                            }
                            if (!converged && _steps.Count > 0)
                            {
                                rootVal = _steps.Last().X;
                                statusMessage = "Maksimum iterasyon doldu.";
                            }
                        }
                        // ==========================================
                        // Method 3: Regula Falsi
                        // ==========================================
                        else if (methodIdx == 3)
                        {
                            decimal a = minX;
                            decimal b = maxX;
                            decimal prevC = a;

                            for (int k = 1; k <= maxIter; k++)
                            {
                                decimal fa = Eval(a);
                                decimal fb = Eval(b);

                                decimal denom = fb - fa;
                                if (Math.Abs(denom) < 1e-12m) break;

                                decimal c = (a * fb - b * fa) / denom;
                                decimal fc = Eval(c);
                                decimal absErr = Math.Abs(c - prevC);
                                decimal relErr = (Math.Abs(c) > 1e-10m) ? (absErr / Math.Abs(c)) * 100m : absErr * 100m;

                                _steps.Add(new IterationStep
                                {
                                    Step = k,
                                    X = c,
                                    Fx = fc,
                                    AbsError = absErr,
                                    RelError = relErr
                                });

                                if (Math.Abs(fc) < _targetTolerance || absErr < _targetTolerance)
                                {
                                    converged = true;
                                    rootVal = c;
                                    statusMessage = "Regula Falsi ile yakınsama sağlandı.";
                                    break;
                                }

                                if (fa * fc <= 0m) b = c;
                                else a = c;
                                prevC = c;
                            }
                            if (!converged && _steps.Count > 0) rootVal = _steps.Last().X;
                        }
                        // ==========================================
                        // Method 4: All Roots in Interval [a, b]
                        // ==========================================
                        else if (methodIdx == 4)
                        {
                            var allRoots = NumericalMethods.FindAllRoots(eq, minX, maxX, varName);
                            if (allRoots.Count > 0)
                            {
                                converged = true;
                                rootVal = allRoots.First().Root;
                                statusMessage = $"[{minX}, {maxX}] aralığında toplam {allRoots.Count} kök bulundu.";

                                int stepIdx = 1;
                                foreach (var r in allRoots)
                                {
                                    _steps.Add(new IterationStep
                                    {
                                        Step = stepIdx++,
                                        X = r.Root,
                                        Fx = r.FVal,
                                        AbsError = 0m,
                                        RelError = 0m
                                    });
                                }
                            }
                            else
                            {
                                statusMessage = $"Belirtilen [{minX}, {maxX}] aralığında reel kök bulunamadı.";
                            }
                        }
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                _lastRoot = rootVal;

                // Update KPI Card
                if (converged || _steps.Count > 0)
                {
                    lblRootValue.Text = $"x* = {rootVal:F10}";
                    lblBadgeIter.Text = $"İterasyon: {_steps.Count} Adım";
                    lblBadgeError.Text = $"Son Hata: {(_steps.Count > 0 ? _steps.Last().AbsError.ToString("E3", CultureInfo.InvariantCulture) : "0")}";
                    lblBadgeStatus.Text = converged ? "✓ Başarıyla Yakınsadı" : "⚠️ Yakınsamadı";
                    lblBadgeStatus.BackColor = converged ? Color.FromArgb(6, 78, 59) : Color.FromArgb(127, 29, 29);
                    lblBadgeStatus.ForeColor = converged ? Color.FromArgb(52, 211, 153) : Color.FromArgb(244, 63, 94);

                    lblRootDetails.Text = $"f(x*) = {NumericalMethods.Method_EvaluateFunction(eq, varName, rootVal):E4}  •  {statusMessage}";
                }
                else
                {
                    lblRootValue.Text = "Kök Bulunamadı";
                    lblBadgeStatus.Text = "Hata / Tanımsız";
                    lblRootDetails.Text = statusMessage;
                }

                // Populate Grid
                dgvIterations.Rows.Clear();
                foreach (var st in _steps)
                {
                    dgvIterations.Rows.Add(
                        st.Step,
                        st.X.ToString("F9", CultureInfo.InvariantCulture),
                        st.Fx.ToString("E4", CultureInfo.InvariantCulture),
                        st.AbsError.ToString("E4", CultureInfo.InvariantCulture),
                        st.RelError.ToString("F4", CultureInfo.InvariantCulture) + "%"
                    );
                }

                // Invalidate Plot
                pnlConvergencePlot.Invalidate();

                // Detailed Text Report
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=======================================================================");
                sb.AppendLine("  DENKLEM KÖKÜ BULUCU ANALİTİK RAPORU");
                sb.AppendLine("=======================================================================");
                sb.AppendLine($"  Denklem       : f({varName}) = {eq} = 0");
                sb.AppendLine($"  Yöntem        : {cmbMethod.SelectedItem}");
                sb.AppendLine($"  Tolerans (ε)  : {_targetTolerance:E2}");
                sb.AppendLine($"  Maks. Adım    : {maxIter}");
                sb.AppendLine();
                sb.AppendLine($"  HESAPLANAN KÖK: x* = {rootVal:F10}");
                sb.AppendLine($"  f(x*) Değeri  : {NumericalMethods.Method_EvaluateFunction(eq, varName, rootVal):G18}");
                sb.AppendLine($"  Toplam Adım   : {_steps.Count}");
                sb.AppendLine($"  Durum Raporu  : {statusMessage}");
                sb.AppendLine();
                sb.AppendLine("--- İterasyon Detayları ---");
                foreach (var st in _steps)
                {
                    sb.AppendLine($"  Adım {st.Step,2}: x = {st.X:F9}  │  f(x) = {st.Fx,12:E4}  │  |Δx| = {st.AbsError:E4}");
                }
                txtResult.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kök bulma hatası:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblRootValue.Text = "Hata Oluştu";
                lblRootDetails.Text = ex.Message;
                lblBadgeStatus.Text = "❌ Hata";
                lblBadgeStatus.BackColor = Color.FromArgb(127, 29, 29);
            }
            finally
            {
                btnFindRoot.Enabled = true;
                btnFindRoot.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void PnlConvergencePlot_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlConvergencePlot.Width;
            int h = pnlConvergencePlot.Height;

            // Background
            using (Brush bg = new SolidBrush(Color.FromArgb(15, 23, 42)))
            {
                g.FillRectangle(bg, 0, 0, w, h);
            }

            if (_steps.Count < 2)
            {
                using (Font f = new Font("Segoe UI", 9F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("Yakınsama grafiği için en az 2 iterasyon adımı gereklidir.", f, b, 20, 20);
                }
                return;
            }

            int padLeft = 65;
            int padRight = 30;
            int padTop = 30;
            int padBottom = 40;

            float plotW = w - padLeft - padRight;
            float plotH = h - padTop - padBottom;
            if (plotW <= 10 || plotH <= 10) return;

            // Compute Log10 of Errors
            List<float> logErrors = new List<float>();
            foreach (var st in _steps)
            {
                double err = (double)Math.Max(1e-16m, st.AbsError);
                logErrors.Add((float)Math.Log10(err));
            }

            float minLog = logErrors.Min();
            float maxLog = Math.Max(0f, logErrors.Max());
            if (Math.Abs(maxLog - minLog) < 1f) { maxLog += 0.5f; minLog -= 0.5f; }

            float MapX(int stepIdx) => padLeft + (stepIdx / (float)(_steps.Count - 1)) * plotW;
            float MapY(float logVal) => padTop + (1f - (logVal - minLog) / (maxLog - minLog)) * plotH;

            // Draw Grid Lines & Y Labels
            using (Pen gridPen = new Pen(Color.FromArgb(30, 41, 59), 1f) { DashStyle = DashStyle.Dash })
            using (Font f = new Font("Segoe UI", 8F))
            using (Brush fontBr = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                for (float lv = (float)Math.Floor(minLog); lv <= maxLog; lv += 2f)
                {
                    float sy = MapY(lv);
                    if (sy >= padTop && sy <= padTop + plotH)
                    {
                        g.DrawLine(gridPen, padLeft, sy, padLeft + plotW, sy);
                        g.DrawString($"10^{lv:F0}", f, fontBr, 10, sy - 7);
                    }
                }

                // X Axis Steps
                for (int s = 0; s < _steps.Count; s += Math.Max(1, _steps.Count / 8))
                {
                    float sx = MapX(s);
                    g.DrawLine(gridPen, sx, padTop, sx, padTop + plotH);
                    g.DrawString($"k={_steps[s].Step}", f, fontBr, sx - 10, padTop + plotH + 8);
                }

                // Title
                g.DrawString("📉 Hata Yakınsama Hızı Grafiği (Log10(|Δx|) vs İterasyon)", new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), fontBr, padLeft, 8);
            }

            // Draw Tolerance Line
            float tolLog = (float)Math.Log10((double)_targetTolerance);
            float tolY = MapY(tolLog);
            if (tolY >= padTop && tolY <= padTop + plotH)
            {
                using (Pen tolPen = new Pen(Color.FromArgb(244, 63, 94), 1.2f) { DashStyle = DashStyle.Dot })
                using (Font tf = new Font("Segoe UI", 7.5F))
                using (Brush tb = new SolidBrush(Color.FromArgb(244, 63, 94)))
                {
                    g.DrawLine(tolPen, padLeft, tolY, padLeft + plotW, tolY);
                    g.DrawString($"Hedef ε ({_targetTolerance:E1})", tf, tb, padLeft + plotW - 90, tolY - 14);
                }
            }

            // Draw Convergence Line & Points
            List<PointF> pts = new List<PointF>();
            for (int i = 0; i < logErrors.Count; i++)
            {
                pts.Add(new PointF(MapX(i), MapY(logErrors[i])));
            }

            using (Pen linePen = new Pen(Color.FromArgb(52, 211, 153), 2.2f))
            using (Brush dotBrush = new SolidBrush(Color.FromArgb(250, 204, 21)))
            {
                if (pts.Count > 1)
                {
                    g.DrawLines(linePen, pts.ToArray());
                }
                foreach (var p in pts)
                {
                    g.FillEllipse(dotBrush, p.X - 3.5f, p.Y - 3.5f, 7f, 7f);
                }
            }
        }

        private void BtnPlotOnGraph_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastEquation))
            {
                MessageBox.Show("Önce bir fonksiyon kökü hesaplayınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float? rootX = _lastRoot.HasValue ? (float)_lastRoot.Value : null;
            MainForm.Instance?.NavigateToGraph(_lastEquation, rootX, 0f);
        }

        private void BtnCopyResult_Click(object sender, EventArgs e)
        {
            string copyText = "";
            if (tabResults.SelectedTab == tabText) copyText = txtResult.Text;
            else if (tabResults.SelectedTab == tabGrid)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Adım\tx_k\tf(x_k)\t|Δx|\tBağıl Hata");
                foreach (DataGridViewRow r in dgvIterations.Rows)
                {
                    sb.AppendLine($"{r.Cells[0].Value}\t{r.Cells[1].Value}\t{r.Cells[2].Value}\t{r.Cells[3].Value}\t{r.Cells[4].Value}");
                }
                copyText = sb.ToString();
            }
            else
            {
                copyText = $"Kök: {_lastRoot}\nDenklem: {_lastEquation}";
            }

            if (!string.IsNullOrEmpty(copyText))
            {
                Clipboard.SetText(copyText);
                MessageBox.Show("Sonuç panoya kopyalandı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
