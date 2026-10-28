using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class OdeSolverControl : UserControl
    {
        private List<DifferentialEquations.OdeStepResult> _currentResults = new List<DifferentialEquations.OdeStepResult>();
        private string _lastEquation = "x + y";
        private decimal _lastX0 = 0m, _lastY0 = 1m, _lastXEnd = 2m;
        private PointF? _hoverPoint = null;
        private (decimal X, decimal Y, decimal Slope)? _hoverData = null;

        public OdeSolverControl()
        {
            InitializeComponent();
            InitializeStepsGrid();

            btnSolve.Click += BtnSolve_Click;
            btnCopyTsv.Click += (s, e) => CopyGridAsTsv();
            btnExportPng.Click += (s, e) => ExportCanvasAsPng();

            btnNavToGraph.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(_lastEquation))
                {
                    MainForm.Instance?.NavigateToGraph(_lastEquation, (float)_lastX0, (float)_lastY0);
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

            pnlOdeCanvas.Paint += PnlOdeCanvas_Paint;
            pnlOdeCanvas.MouseMove += PnlOdeCanvas_MouseMove;
            pnlOdeCanvas.MouseLeave += (s, e) => { _hoverPoint = null; _hoverData = null; pnlOdeCanvas.Invalidate(); };
            pnlOdeCanvas.Resize += (s, e) => pnlOdeCanvas.Invalidate();

            // Load initial preset
            LoadPreset("x + y", "0", "1", "2", 50, 0);
        }

        private void InitializeStepsGrid()
        {
            dgvSteps.Columns.Clear();
            dgvSteps.Columns.Add("step", "Adım (i)");
            dgvSteps.Columns.Add("x", "x_i");
            dgvSteps.Columns.Add("y", "y_i (Hesaplanan)");
            dgvSteps.Columns.Add("slope", "Eğim (dy/dx)");

            dgvSteps.Columns["step"].Width = 65;
            dgvSteps.Columns["x"].Width = 90;
            dgvSteps.Columns["y"].Width = 140;
            dgvSteps.Columns["slope"].Width = 120;
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

                var sw = System.Diagnostics.Stopwatch.StartNew();
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
                sw.Stop();

                if (results != null && results.Count > 0)
                {
                    _currentResults = results;
                    _lastEquation = eq;
                    _lastX0 = x0;
                    _lastY0 = y0;
                    _lastXEnd = xEnd;

                    var last = results[results.Count - 1];
                    string lastYStr = last.Y.ToString("0.########", CultureInfo.InvariantCulture);
                    string lastXStr = last.X.ToString("0.####", CultureInfo.InvariantCulture);
                    decimal stepSize = (xEnd - x0) / steps;

                    lblKpiFinalVal.Text = $"y({lastXStr}) ≈ {lastYStr}";
                    lblKpiFinalVal.ForeColor = Color.FromArgb(52, 211, 153);
                    lblKpiSub.Text = $"✅ {method} başarıyla tamamlandı.";

                    lblMetricSteps.Text = $"• Toplam Adım (N) : {steps}";
                    lblMetricH.Text = $"• Adım Boyutu (h) : {stepSize.ToString("0.######", CultureInfo.InvariantCulture)}";
                    lblMetricElapsed.Text = $"• Çözüm Süresi    : {sw.ElapsedMilliseconds} ms ({sw.ElapsedTicks} ticks)";
                    lblMetricMethod.Text = $"• Yöntem          : {method}";

                    // Populate DataGridView
                    dgvSteps.Rows.Clear();
                    foreach (var r in results)
                    {
                        dgvSteps.Rows.Add(
                            dgvSteps.Rows.Count,
                            r.X.ToString("0.######", CultureInfo.InvariantCulture),
                            r.Y.ToString("0.########", CultureInfo.InvariantCulture),
                            r.Slope.ToString("0.######", CultureInfo.InvariantCulture)
                        );
                    }

                    // Populate RichTextBox Report
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=== DİFERANSİYEL DENKLEM NÜMERİK ÇÖZÜM TABLOSU ===");
                    sb.AppendLine($"  Denklem       : dy/dx = {eq}");
                    sb.AppendLine($"  Başlangıç     : y({x0}) = {y0}");
                    sb.AppendLine($"  Hedef Aralık  : [{x0}, {xEnd}]  (N = {steps} adım, h = {stepSize.ToString("0.######", CultureInfo.InvariantCulture)})");
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

                    pnlOdeCanvas.Invalidate();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "❌ Hata: " + ex.Message;
                lblKpiFinalVal.Text = "Hata Oluştu";
                lblKpiFinalVal.ForeColor = Color.FromArgb(239, 68, 68);
                lblKpiSub.Text = ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void PnlOdeCanvas_Paint(object sender, PaintEventArgs e)
        {
            RenderCanvas(e.Graphics, pnlOdeCanvas.Width, pnlOdeCanvas.Height);
        }

        private void RenderCanvas(Graphics g, int w, int h)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentResults == null || _currentResults.Count < 2)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("Diferansiyel denklem çözüldüğünde y(x) çözüm eğrisi burada çizilir.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            // Find bounds
            decimal minX = _currentResults[0].X;
            decimal maxX = _currentResults[_currentResults.Count - 1].X;
            decimal minY = _currentResults[0].Y;
            decimal maxY = _currentResults[0].Y;

            foreach (var r in _currentResults)
            {
                if (r.Y < minY) minY = r.Y;
                if (r.Y > maxY) maxY = r.Y;
            }

            // Add margin
            decimal spanX = maxX - minX;
            if (spanX == 0m) spanX = 1m;
            decimal spanY = maxY - minY;
            if (spanY == 0m) spanY = 1m;

            minX -= spanX * 0.05m;
            maxX += spanX * 0.05m;
            minY -= spanY * 0.1m;
            maxY += spanY * 0.1m;

            int padLeft = 45;
            int padBottom = 30;
            int padTop = 20;
            int padRight = 20;

            float plotW = w - padLeft - padRight;
            float plotH = h - padTop - padBottom;
            if (plotW < 10 || plotH < 10) return;

            float ToScreenX(decimal x) => padLeft + (float)((x - minX) / (maxX - minX) * (decimal)plotW);
            float ToScreenY(decimal y) => padTop + (float)((maxY - y) / (maxY - minY) * (decimal)plotH);

            // Draw subtle grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(24, 33, 52), 1f) { DashStyle = DashStyle.Dash })
            using (Font lblFont = new Font("Consolas", 7.5F))
            using (Brush lblBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                for (int i = 0; i <= 5; i++)
                {
                    decimal gx = minX + (spanX * i / 5m);
                    float sx = ToScreenX(gx);
                    g.DrawLine(gridPen, sx, padTop, sx, h - padBottom);
                    g.DrawString(Math.Round(gx, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, sx - 12, h - padBottom + 4);

                    decimal gy = minY + (spanY * i / 5m);
                    float sy = ToScreenY(gy);
                    g.DrawLine(gridPen, padLeft, sy, w - padRight, sy);
                    g.DrawString(Math.Round(gy, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, 4, sy - 6);
                }
            }

            // Draw coordinate axis box
            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padLeft, padTop, plotW, plotH);
            }

            // Draw Solution Curve
            List<PointF> pts = new List<PointF>(_currentResults.Count);
            foreach (var r in _currentResults)
            {
                pts.Add(new PointF(ToScreenX(r.X), ToScreenY(r.Y)));
            }

            using (Pen curvePen = new Pen(Color.FromArgb(16, 185, 129), 2.5f))
            {
                curvePen.LineJoin = LineJoin.Round;
                for (int i = 0; i < pts.Count - 1; i++)
                {
                    g.DrawLine(curvePen, pts[i], pts[i + 1]);
                }
            }

            // Highlight Start Point (x0, y0) in Amber
            PointF startPt = pts[0];
            using (Brush amberBrush = new SolidBrush(Color.FromArgb(245, 158, 11)))
            {
                g.FillEllipse(amberBrush, startPt.X - 4.5f, startPt.Y - 4.5f, 9f, 9f);
            }

            // Highlight End Point in Emerald
            PointF endPt = pts[pts.Count - 1];
            using (Brush greenBrush = new SolidBrush(Color.FromArgb(52, 211, 153)))
            {
                g.FillEllipse(greenBrush, endPt.X - 5.5f, endPt.Y - 5.5f, 11f, 11f);
            }

            // Crosshair & Tooltip
            if (_hoverPoint.HasValue && _hoverData.HasValue)
            {
                PointF hp = _hoverPoint.Value;
                using (Pen crossPen = new Pen(Color.FromArgb(14, 165, 233), 1f) { DashStyle = DashStyle.Dot })
                {
                    g.DrawLine(crossPen, hp.X, padTop, hp.X, h - padBottom);
                    g.DrawLine(crossPen, padLeft, hp.Y, w - padRight, hp.Y);
                }

                using (Brush hoverDot = new SolidBrush(Color.FromArgb(56, 189, 248)))
                {
                    g.FillEllipse(hoverDot, hp.X - 4f, hp.Y - 4f, 8f, 8f);
                }

                // Tooltip pill
                var d = _hoverData.Value;
                string tip = $"x = {Math.Round(d.X, 4)}\ny = {Math.Round(d.Y, 5)}\ndy/dx = {Math.Round(d.Slope, 4)}";
                using (Font tipFont = new Font("Consolas", 8.5F, FontStyle.Bold))
                {
                    SizeF sz = g.MeasureString(tip, tipFont);
                    float tipX = hp.X + 12;
                    float tipY = hp.Y - sz.Height - 6;
                    if (tipX + sz.Width + 8 > w) tipX = hp.X - sz.Width - 14;
                    if (tipY < padTop) tipY = hp.Y + 12;

                    RectangleF tipRect = new RectangleF(tipX, tipY, sz.Width + 10, sz.Height + 6);
                    using (Brush bgBrush = new SolidBrush(Color.FromArgb(240, 24, 33, 52)))
                    using (Pen borderPen = new Pen(Color.FromArgb(56, 189, 248), 1.2f))
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(bgBrush, tipRect);
                        g.DrawRectangle(borderPen, tipRect.X, tipRect.Y, tipRect.Width, tipRect.Height);
                        g.DrawString(tip, tipFont, textBrush, tipX + 5, tipY + 3);
                    }
                }
            }

            // Top legend
            using (Font legFont = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
            using (Brush bCyan = new SolidBrush(Color.FromArgb(52, 211, 153)))
            using (Brush bAmber = new SolidBrush(Color.FromArgb(245, 158, 11)))
            {
                g.DrawString("— y(x) Çözüm Eğrisi", legFont, bCyan, padLeft + 6, padTop + 6);
                g.DrawString("● Başlangıç (x₀, y₀)", legFont, bAmber, padLeft + 140, padTop + 6);
            }
        }

        private void PnlOdeCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentResults == null || _currentResults.Count < 2) return;

            int w = pnlOdeCanvas.Width;
            int h = pnlOdeCanvas.Height;
            int padLeft = 45, padBottom = 30, padTop = 20, padRight = 20;
            float plotW = w - padLeft - padRight;
            float plotH = h - padTop - padBottom;
            if (plotW < 10 || plotH < 10) return;

            decimal minX = _currentResults[0].X;
            decimal maxX = _currentResults[_currentResults.Count - 1].X;
            decimal minY = _currentResults[0].Y;
            decimal maxY = _currentResults[0].Y;
            foreach (var r in _currentResults)
            {
                if (r.Y < minY) minY = r.Y;
                if (r.Y > maxY) maxY = r.Y;
            }

            decimal spanX = maxX - minX;
            if (spanX == 0m) spanX = 1m;
            decimal spanY = maxY - minY;
            if (spanY == 0m) spanY = 1m;
            minX -= spanX * 0.05m;
            maxX += spanX * 0.05m;
            minY -= spanY * 0.1m;
            maxY += spanY * 0.1m;

            float ToScreenX(decimal x) => padLeft + (float)((x - minX) / (maxX - minX) * (decimal)plotW);
            float ToScreenY(decimal y) => padTop + (float)((maxY - y) / (maxY - minY) * (decimal)plotH);

            // Find closest result point to mouse X
            DifferentialEquations.OdeStepResult closest = null;
            float closestDist = float.MaxValue;
            PointF closestScreen = PointF.Empty;

            foreach (var r in _currentResults)
            {
                PointF sp = new PointF(ToScreenX(r.X), ToScreenY(r.Y));
                float dx = sp.X - e.X;
                float dy = sp.Y - e.Y;
                float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = r;
                    closestScreen = sp;
                }
            }

            if (closest != null && closestDist < 50f)
            {
                _hoverPoint = closestScreen;
                _hoverData = (closest.X, closest.Y, closest.Slope);
            }
            else
            {
                _hoverPoint = null;
                _hoverData = null;
            }
            pnlOdeCanvas.Invalidate();
        }

        private void CopyGridAsTsv()
        {
            if (dgvSteps.Rows.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Adım\tx\ty\tEğim");
            foreach (DataGridViewRow r in dgvSteps.Rows)
            {
                sb.AppendLine($"{r.Cells[0].Value}\t{r.Cells[1].Value}\t{r.Cells[2].Value}\t{r.Cells[3].Value}");
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Adım tablosu panoya (TSV formatında) kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportCanvasAsPng()
        {
            if (_currentResults == null || _currentResults.Count < 2)
            {
                MessageBox.Show("Önce diferansiyel denklemi çözünüz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Resmi (*.png)|*.png";
                sfd.FileName = "ODE_Cozum_Egrisi.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bmp = new Bitmap(1200, 800))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        RenderCanvas(g, 1200, 800);
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Grafik yüksek çözünürlükte (1200x800) başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
