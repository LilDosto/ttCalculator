using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class TaylorSeriesControl : UserControl
    {
        private string _lastOriginalFn = "sin(x)";
        private string _lastPolyEquation = "";
        private decimal _lastCenter = 0m;
        private decimal _lastF0 = 0m;
        private List<(decimal Coeff, int Power)> _lastTerms = new List<(decimal, int)>();
        private string _lastVariable = "x";
        private bool _hasCalculated = false;

        public TaylorSeriesControl()
        {
            InitializeComponent();
            InitializeGridColumns();

            btnCalculate.Click += BtnCalculate_Click;

            btnExSin.Click += (s, e) => LoadPreset("sin(x)", "x", "0", 5);
            btnExCos.Click += (s, e) => LoadPreset("cos(x)", "x", "0", 6);
            btnExExp.Click += (s, e) => LoadPreset("exp(x)", "x", "0", 5);
            btnExLn.Click += (s, e) => LoadPreset("ln(1+x)", "x", "0", 4);
            btnExGeom.Click += (s, e) => LoadPreset("1/(1-x)", "x", "0", 5);
            btnExAtan.Click += (s, e) => LoadPreset("atan(x)", "x", "0", 5);

            btnMaclaurin.Click += (s, e) => {
                txtCenterPoint.Text = "0";
                BtnCalculate_Click(this, EventArgs.Empty);
            };

            btnCopyPoly.Click += (s, e) => {
                if (!string.IsNullOrEmpty(lblHeroPolyValue.Text) && lblHeroPolyValue.Text != "P_n(x) = —")
                {
                    Clipboard.SetText(lblHeroPolyValue.Text);
                    MessageBox.Show("Taylor polinom açılımı panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnCopyLatex.Click += (s, e) => {
                if (!string.IsNullOrEmpty(_lastPolyEquation))
                {
                    string latex = "P_{" + numOrder.Value + "}(" + _lastVariable + ") = " + _lastPolyEquation.Replace("*", " \\cdot ");
                    Clipboard.SetText(latex);
                    MessageBox.Show("Polinom LaTeX formatında panoya kopyalandı:\n\n" + latex, "LaTeX Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            btnNavToGraph.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(_lastPolyEquation))
                {
                    // Also send original function if desired, or send polynomial
                    MainForm.Instance?.NavigateToGraph(_lastPolyEquation, (float)_lastCenter, (float)_lastF0);
                }
                else
                {
                    MessageBox.Show("Lütfen önce bir Taylor polinomu hesaplayınız.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            txtFunction.KeyDown += Input_KeyDown;
            txtVariable.KeyDown += Input_KeyDown;
            txtCenterPoint.KeyDown += Input_KeyDown;

            pnlMiniCanvas.Paint += PnlMiniCanvas_Paint;
            pnlMiniCanvas.Resize += (s, e) => pnlMiniCanvas.Invalidate();

            // Auto run first preset
            LoadPreset("sin(x)", "x", "0", 5);
        }

        private void InitializeGridColumns()
        {
            dgvTerms.Columns.Clear();
            dgvTerms.Columns.Add("k", "k");
            dgvTerms.Columns.Add("derivSym", "Mertebe f^(k)");
            dgvTerms.Columns.Add("derivVal", "Noktadaki Değer f^(k)(a)");
            dgvTerms.Columns.Add("fact", "k!");
            dgvTerms.Columns.Add("coeffFrac", "Kesirli c_k");
            dgvTerms.Columns.Add("coeffDec", "Ondalık c_k");
            dgvTerms.Columns.Add("termStr", "Polinom Terimi");

            dgvTerms.Columns["k"].Width = 45;
            dgvTerms.Columns["k"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTerms.Columns["derivSym"].Width = 95;
            dgvTerms.Columns["derivVal"].Width = 140;
            dgvTerms.Columns["fact"].Width = 70;
            dgvTerms.Columns["fact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTerms.Columns["coeffFrac"].Width = 120;
            dgvTerms.Columns["coeffDec"].Width = 120;
            dgvTerms.Columns["termStr"].MinimumWidth = 160;

            dgvError.Columns.Clear();
            dgvError.Columns.Add("testPoint", "Test Noktası (x)");
            dgvError.Columns.Add("fVal", "Gerçek Değer f(x)");
            dgvError.Columns.Add("polyVal", "Taylor P_n(x)");
            dgvError.Columns.Add("absErr", "Mutlak Hata |f - P_n|");
            dgvError.Columns.Add("relErr", "Bağıl Hata (%)");
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

        private async void BtnCalculate_Click(object sender, EventArgs e)
        {
            string origBtnText = btnCalculate.Text;
            btnCalculate.Enabled = false;
            btnCalculate.Text = "⏳ Hesaplanıyor...";
            Cursor = Cursors.WaitCursor;

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

                var calcData = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    List<string> lines = new List<string>();
                    List<string> fracTerms = new List<string>();
                    List<string> factTerms = new List<string>();
                    List<string> decTerms = new List<string>();
                    List<(int k, string sym, string dVal, string fact, string fracC, string decC, string term)> termRows = new List<(int, string, string, string, string, string, string)>();
                    List<(decimal Coeff, int Power)> parsedTerms = new List<(decimal, int)>();

                    try
                    {
                        decimal f0 = NumericalMethods.Method_EvaluateFunction(eq, variable, center);
                        if (DecimalMath.Abs(f0) <= 0.00001m) f0 = 0m;

                        string f0Frac = NumericalMethods.FormatFractionalCoeff(f0);
                        string f0Dec = Math.Round(f0, 6).ToString("0.######", CultureInfo.InvariantCulture);
                        lines.Add($"[k = 0] f({center}) = {f0Frac}  ({f0Dec})");

                        termRows.Add((0, $"f({variable})", f0Dec, "1", f0Frac, f0Dec, f0 != 0m ? f0Frac : "0"));
                        parsedTerms.Add((f0, 0));

                        if (f0 != 0m)
                        {
                            fracTerms.Add(f0Frac);
                            factTerms.Add(f0Frac);
                            decTerms.Add(f0Dec);
                        }

                        for (int k = 1; k <= degree; k++)
                        {
                            decimal step = k >= 4 ? 0.01m : 0.005m;
                            decimal dk = NumericalMethods.Method_NumericalDifferentiation(eq, k, variable, center, 4, step);
                            decimal fact = NumericalMethods.Factorial(k);
                            decimal coeff = dk / fact;

                            string varPart;
                            if (center == 0m) varPart = k == 1 ? variable : $"{variable}^{k}";
                            else if (center > 0m) varPart = k == 1 ? $"({variable} - {center})" : $"({variable} - {center})^{k}";
                            else varPart = k == 1 ? $"({variable} + {DecimalMath.Abs(center)})" : $"({variable} + {DecimalMath.Abs(center)})^{k}";

                            var frac = NumericalMethods.ToFraction(coeff);
                            string coeffFracStr = frac.HasValue ? (frac.Value.Denom == 1 ? $"{frac.Value.Num}" : $"{frac.Value.Num}/{frac.Value.Denom}") : Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture);
                            string coeffDecStr = Math.Round(coeff, 6).ToString("0.######", CultureInfo.InvariantCulture);
                            string dValStr = Math.Round(dk, 5).ToString("0.#####", CultureInfo.InvariantCulture);

                            if (DecimalMath.Abs(coeff) <= 0.00001m)
                            {
                                lines.Add($"[k = {k}] f^({k})({center}) ≈ 0  =>  Katsayı = 0");
                                termRows.Add((k, $"f^({k})({variable})", dValStr, fact.ToString(CultureInfo.InvariantCulture), "0", "0", "0"));
                                continue;
                            }

                            parsedTerms.Add((coeff, k));

                            // 1. Fractional Form
                            string termFrac;
                            if (frac.HasValue)
                            {
                                long num = frac.Value.Num;
                                long den = frac.Value.Denom;
                                string sign = num < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                                long absNum = Math.Abs(num);
                                if (den == 1)
                                    termFrac = absNum == 1 ? $"{sign}{varPart}" : $"{sign}{absNum}*{varPart}";
                                else
                                    termFrac = absNum == 1 ? $"{sign}(1/{den})*{varPart}" : $"{sign}({absNum}/{den})*{varPart}";
                            }
                            else
                            {
                                string sign = coeff < 0 ? (fracTerms.Count == 0 ? "-" : "- ") : (fracTerms.Count == 0 ? "" : "+ ");
                                termFrac = $"{sign}{Math.Round(DecimalMath.Abs(coeff), 5).ToString("0.#####", CultureInfo.InvariantCulture)}*{varPart}";
                            }
                            fracTerms.Add(termFrac);
                            termRows.Add((k, $"f^({k})({variable})", dValStr, fact.ToString(CultureInfo.InvariantCulture), coeffFracStr, coeffDecStr, termFrac));

                            // 2. Factorial Form
                            var derFrac = NumericalMethods.ToFraction(dk);
                            string factSign = dk < 0 ? (factTerms.Count == 0 ? "-" : "- ") : (factTerms.Count == 0 ? "" : "+ ");
                            decimal absDk = DecimalMath.Abs(dk);
                            string termFact;
                            if (derFrac.HasValue && derFrac.Value.Denom == 1)
                            {
                                long dVal = Math.Abs(derFrac.Value.Num);
                                termFact = dVal == 1 ? $"{factSign}(1/{k}!)*{varPart}" : $"{factSign}({dVal}/{k}!)*{varPart}";
                            }
                            else
                            {
                                string dStr = Math.Round(absDk, 4).ToString("0.####", CultureInfo.InvariantCulture);
                                termFact = $"{factSign}({dStr}/{k}!)*{varPart}";
                            }
                            factTerms.Add(termFact);

                            // 3. Decimal Form
                            string decSign = coeff < 0 ? (decTerms.Count == 0 ? "-" : "- ") : (decTerms.Count == 0 ? "" : "+ ");
                            decTerms.Add($"{decSign}{coeffDecStr}*{varPart}");

                            lines.Add($"[k = {k,2}] f^({k})({center}) = {dValStr,-10} | {k}! = {fact,-8} => Kesirli: {coeffFracStr,-12} | Ondalık: {coeffDecStr}");
                        }

                        string finalFrac = fracTerms.Count == 0 ? "0" : string.Join(" ", fracTerms);
                        string finalFact = factTerms.Count == 0 ? "0" : string.Join(" ", factTerms);
                        string finalDec = decTerms.Count == 0 ? "0" : string.Join(" ", decTerms);

                        // Error evaluation at sample points around center
                        List<(decimal x, decimal fVal, decimal polyVal, decimal absErr, decimal relErr)> errRows = new List<(decimal, decimal, decimal, decimal, decimal)>();
                        decimal[] testDeltas = { -2m, -1m, -0.5m, -0.1m, 0.1m, 0.5m, 1m, 2m };
                        foreach (var d in testDeltas)
                        {
                            decimal tx = center + d;
                            try
                            {
                                decimal realF = NumericalMethods.Method_EvaluateFunction(eq, variable, tx);
                                decimal polyF = 0m;
                                foreach (var t in parsedTerms)
                                {
                                    polyF += t.Coeff * DecimalMath.Pow(tx - center, t.Power);
                                }
                                decimal absE = DecimalMath.Abs(realF - polyF);
                                decimal relE = DecimalMath.Abs(realF) > 1e-9m ? (absE / DecimalMath.Abs(realF)) * 100m : 0m;
                                errRows.Add((tx, realF, polyF, absE, relE));
                            }
                            catch { }
                        }

                        return (finalFrac, finalFact, finalDec, lines, termRows, parsedTerms, errRows, f0);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });

                _lastOriginalFn = eq;
                _lastPolyEquation = calcData.finalFrac;
                _lastCenter = center;
                _lastF0 = calcData.f0;
                _lastTerms = calcData.parsedTerms;
                _lastVariable = variable;
                _hasCalculated = true;

                lblHeroPolyValue.Text = $"P_{degree}({variable}) = {calcData.finalFrac}";
                lblHeroPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

                // Populate dgvTerms
                dgvTerms.Rows.Clear();
                foreach (var r in calcData.termRows)
                {
                    dgvTerms.Rows.Add(r.k, r.sym, r.dVal, r.fact, r.fracC, r.decC, r.term);
                }

                // Populate dgvError
                dgvError.Rows.Clear();
                foreach (var er in calcData.errRows)
                {
                    dgvError.Rows.Add(
                        Math.Round(er.x, 3).ToString(CultureInfo.InvariantCulture),
                        Math.Round(er.fVal, 6).ToString(CultureInfo.InvariantCulture),
                        Math.Round(er.polyVal, 6).ToString(CultureInfo.InvariantCulture),
                        Math.Round(er.absErr, 6).ToString(CultureInfo.InvariantCulture),
                        Math.Round(er.relErr, 3).ToString(CultureInfo.InvariantCulture) + "%"
                    );
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== {degree}. DERECEDEN TAYLOR POLİNOMU (Merkez a = {center}) ===");
                sb.AppendLine($"Fonksiyon : f({variable}) = {eq}");
                sb.AppendLine();
                sb.AppendLine("📐 1) KESİRLİ SADELEŞTİRİLMİŞ FORM:");
                sb.AppendLine($"   P_{degree}({variable}) = {calcData.finalFrac}");
                sb.AppendLine();
                sb.AppendLine("📜 2) ANALİTİK FAKTÖRİYELLİ FORM [ f^(k)(a)/k! ]:");
                sb.AppendLine($"   P_{degree}({variable}) = {calcData.finalFact}");
                sb.AppendLine();
                sb.AppendLine("🔢 3) ONDALIKLI SAYISAL YAKLAŞIM:");
                sb.AppendLine($"   P_{degree}({variable}) = {calcData.finalDec}");
                sb.AppendLine();
                sb.AppendLine(new string('=', 65));
                sb.AppendLine("=== NOKTADAKİ TÜREVLER, FAKTÖRİYEL VE TERİM KATSAYILARI TABLOSU ===");
                sb.AppendLine(string.Join(Environment.NewLine, calcData.lines));

                txtResult.Text = sb.ToString();

                pnlMiniCanvas.Invalidate();
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
                lblHeroPolyValue.Text = "Hata Oluştu: " + ex.Message;
                lblHeroPolyValue.ForeColor = Color.FromArgb(239, 68, 68);
            }
            finally
            {
                btnCalculate.Enabled = true;
                btnCalculate.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void PnlMiniCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlMiniCanvas.Width;
            int h = pnlMiniCanvas.Height;
            if (w < 40 || h < 40) return;

            g.Clear(Color.FromArgb(11, 15, 28));

            if (!_hasCalculated || _lastTerms == null || _lastTerms.Count == 0)
            {
                using (Font f = new Font("Segoe UI", 9F))
                using (Brush b = new SolidBrush(Color.FromArgb(100, 116, 139)))
                {
                    g.DrawString("Hesaplama yapıldığında 2D yakınsama eğrisi burada gösterilir.", f, b, new RectangleF(10, 20, w - 20, 60));
                }
                return;
            }

            // Coordinate bounds around center point with natural aspect ratio
            decimal aspect = (decimal)w / Math.Max(1, h);
            decimal yHalfSpan = 3.5m;
            decimal xSpan = yHalfSpan * (aspect > 1.2m ? aspect * 0.75m : 1.2m);
            decimal xMin = _lastCenter - xSpan;
            decimal xMax = _lastCenter + xSpan;
            decimal yMin = -yHalfSpan;
            decimal yMax = yHalfSpan;

            // Transform helpers
            float ToScreenX(decimal x) => (float)((x - xMin) / (xMax - xMin) * (decimal)w);
            float ToScreenY(decimal y) => (float)((yMax - y) / (yMax - yMin) * (decimal)h);

            // Draw grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(24, 33, 52), 1f) { DashStyle = DashStyle.Dash })
            {
                for (decimal gx = Math.Floor(xMin); gx <= Math.Ceiling(xMax); gx += 1m)
                {
                    float sx = ToScreenX(gx);
                    g.DrawLine(gridPen, sx, 0, sx, h);
                }
                for (decimal gy = Math.Floor(yMin); gy <= Math.Ceiling(yMax); gy += 1m)
                {
                    float sy = ToScreenY(gy);
                    g.DrawLine(gridPen, 0, sy, w, sy);
                }
            }

            // Draw primary axes
            using (Pen axisPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                float zeroX = ToScreenX(0m);
                float zeroY = ToScreenY(0m);
                if (zeroX >= 0 && zeroX <= w) g.DrawLine(axisPen, zeroX, 0, zeroX, h);
                if (zeroY >= 0 && zeroY <= h) g.DrawLine(axisPen, 0, zeroY, w, zeroY);
            }

            // Dotted vertical line at center a
            using (Pen centerLinePen = new Pen(Color.FromArgb(245, 158, 11), 1f) { DashStyle = DashStyle.Dot })
            {
                float cx = ToScreenX(_lastCenter);
                g.DrawLine(centerLinePen, cx, 0, cx, h);
            }

            // Plot original function f(x) (Cyan)
            List<PointF> fPoints = new List<PointF>();
            List<PointF> pPoints = new List<PointF>();
            int steps = Math.Max(80, w);

            bool prevRad = Function.UseRadians;
            Function.UseRadians = true;
            try
            {
                for (int i = 0; i <= steps; i++)
                {
                    decimal curX = xMin + ((decimal)i / steps) * (xMax - xMin);

                    // True f(x)
                    try
                    {
                        decimal yVal = NumericalMethods.Method_EvaluateFunction(_lastOriginalFn, _lastVariable, curX);
                        if (DecimalMath.Abs(yVal) < 100m)
                        {
                            fPoints.Add(new PointF(ToScreenX(curX), ToScreenY(yVal)));
                        }
                    }
                    catch { }

                    // Taylor P_n(x)
                    try
                    {
                        decimal pVal = 0m;
                        foreach (var term in _lastTerms)
                        {
                            pVal += term.Coeff * DecimalMath.Pow(curX - _lastCenter, term.Power);
                        }
                        if (DecimalMath.Abs(pVal) < 100m)
                        {
                            pPoints.Add(new PointF(ToScreenX(curX), ToScreenY(pVal)));
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                Function.UseRadians = prevRad;
            }

            // Draw f(x) curve
            if (fPoints.Count > 1)
            {
                using (Pen fPen = new Pen(Color.FromArgb(56, 189, 248), 2f))
                {
                    for (int i = 0; i < fPoints.Count - 1; i++)
                    {
                        if (Math.Abs(fPoints[i].Y - fPoints[i + 1].Y) < h * 0.8f)
                            g.DrawLine(fPen, fPoints[i], fPoints[i + 1]);
                    }
                }
            }

            // Draw Taylor curve
            if (pPoints.Count > 1)
            {
                using (Pen pPen = new Pen(Color.FromArgb(16, 185, 129), 2f))
                {
                    for (int i = 0; i < pPoints.Count - 1; i++)
                    {
                        if (Math.Abs(pPoints[i].Y - pPoints[i + 1].Y) < h * 0.8f)
                            g.DrawLine(pPen, pPoints[i], pPoints[i + 1]);
                    }
                }
            }

            // Draw marked center point (a, f(a))
            float ptX = ToScreenX(_lastCenter);
            float ptY = ToScreenY(_lastF0);
            if (ptX >= -10 && ptX <= w + 10 && ptY >= -10 && ptY <= h + 10)
            {
                using (Brush dotBrush = new SolidBrush(Color.FromArgb(245, 158, 11)))
                {
                    g.FillEllipse(dotBrush, ptX - 4f, ptY - 4f, 8f, 8f);
                }
            }

            // Draw Legend at top
            using (Font f = new Font("Segoe UI", 7.5F))
            using (Brush bCyan = new SolidBrush(Color.FromArgb(56, 189, 248)))
            using (Brush bGreen = new SolidBrush(Color.FromArgb(16, 185, 129)))
            using (Brush bAmber = new SolidBrush(Color.FromArgb(245, 158, 11)))
            {
                g.DrawString("— f(x)", f, bCyan, 8, 6);
                g.DrawString("— P_n(x)", f, bGreen, 56, 6);
                g.DrawString("● a", f, bAmber, 114, 6);
            }
        }
    }
}
