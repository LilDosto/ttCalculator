using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class GraphPlotterControl : UserControl
    {
        private List<List<PointF>> segments = new List<List<PointF>>();
        private List<PointF> markedPoints = new List<PointF>();

        private float rangeMinX = -10f;
        private float rangeMaxX = 10f;
        private float rangeMinY = -6f;
        private float rangeMaxY = 6f;

        private string currentEq = "sin(x)";
        private Function compiledFn = null;

        private bool isDragging = false;
        private bool didMoveMouse = false;
        private Point lastMousePos;
        private Point? hoverMousePos = null;

        public GraphPlotterControl()
        {
            InitializeComponent();

            // Enable DoubleBuffering on pnlCanvas to prevent flickering during pan/zoom
            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                pnlCanvas,
                new object[] { true });

            btnPlot.Click += BtnPlot_Click;
            btnResetView.Click += BtnResetView_Click;
            btnClearMarks.Click += (s, e) => { markedPoints.Clear(); pnlCanvas.Invalidate(); };

            btnEx1.Click += (s, e) => LoadPreset("sin(x) * x", -10f, 10f);
            btnEx2.Click += (s, e) => LoadPreset("x^3 - 3*x", -3f, 3f);
            btnEx3.Click += (s, e) => LoadPreset("exp(-x^2/4)*cos(2*x)", -6f, 6f);
            btnEx4.Click += (s, e) => LoadPreset("1 / (x^2 - 1)", -5f, 5f);

            txtEquation.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPlot_Click(this, EventArgs.Empty); } };
            txtMinX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPlot_Click(this, EventArgs.Empty); } };
            txtMaxX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BtnPlot_Click(this, EventArgs.Empty); } };

            pnlCanvas.MouseDown += PnlCanvas_MouseDown;
            pnlCanvas.MouseMove += PnlCanvas_MouseMove;
            pnlCanvas.MouseUp += PnlCanvas_MouseUp;
            pnlCanvas.MouseLeave += (s, e) => { hoverMousePos = null; pnlCanvas.Invalidate(); };
            pnlCanvas.MouseWheel += PnlCanvas_MouseWheel;

            // Plot default function on startup
            PlotNewEquation(true);
        }

        private void LoadPreset(string eq, float minX, float maxX)
        {
            txtEquation.Text = eq;
            txtMinX.Text = minX.ToString(CultureInfo.InvariantCulture);
            txtMaxX.Text = maxX.ToString(CultureInfo.InvariantCulture);
            PlotNewEquation(true);
        }

        private void BtnPlot_Click(object sender, EventArgs e)
        {
            PlotNewEquation(true);
        }

        private void BtnResetView_Click(object sender, EventArgs e)
        {
            txtMinX.Text = "-10";
            txtMaxX.Text = "10";
            PlotNewEquation(true);
        }

        private void PlotNewEquation(bool autoFitY)
        {
            currentEq = txtEquation.Text.Trim();
            if (string.IsNullOrWhiteSpace(currentEq)) return;

            rangeMinX = ParseFloat(txtMinX.Text, -10f);
            rangeMaxX = ParseFloat(txtMaxX.Text, 10f);
            if (rangeMinX >= rangeMaxX)
            {
                rangeMinX = -10f;
                rangeMaxX = 10f;
            }

            try
            {
                compiledFn = NumericalMethods.CompileFunction(currentEq);
                lblStatus.Text = $"✓ Çizildi: y = {currentEq}   |   Sol Tık + Sürükle: Kaydır   |   Tekerlek: Yakınlaştır   |   Tıkla: Nokta İşaretle";
                lblStatus.ForeColor = ThemeHelper.MutedText;
            }
            catch (Exception ex)
            {
                compiledFn = null;
                segments.Clear();
                lblStatus.Text = "❌ Denklem Hatası: " + ex.Message;
                lblStatus.ForeColor = ThemeHelper.DangerRed;
                pnlCanvas.Invalidate();
                return;
            }

            GeneratePoints(autoFitY);
            pnlCanvas.Invalidate();
        }

        private float ParseFloat(string s, float def)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (float.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out float v)) return v;
            return def;
        }

        private void GeneratePoints(bool autoFitY)
        {
            segments.Clear();
            if (compiledFn == null) return;

            int sampleCount = Math.Max(600, pnlCanvas.Width * 2);
            float step = (rangeMaxX - rangeMinX) / sampleCount;
            if (step <= 0) return;

            float tempMinY = float.MaxValue;
            float tempMaxY = float.MinValue;

            List<PointF> currentSegment = new List<PointF>();
            float? prevY = null;

            bool prevRad = Function.UseRadians;
            Function.UseRadians = true;
            try
            {
                for (int i = 0; i <= sampleCount; i++)
                {
                    float x = rangeMinX + i * step;
                    try
                    {
                        decimal val = NumericalMethods.EvaluateCompiled(compiledFn, "x", (decimal)x);
                        float y = (float)val;

                        if (float.IsNaN(y) || float.IsInfinity(y) || Math.Abs(y) > 1e6f)
                        {
                            if (currentSegment.Count > 1) segments.Add(currentSegment);
                            currentSegment = new List<PointF>();
                            prevY = null;
                            continue;
                        }

                        // Detect vertical asymptote jump (e.g. 1/x or tan(x))
                        if (prevY.HasValue && Math.Abs(y - prevY.Value) > (rangeMaxY - rangeMinY) * 2.5f && (y * prevY.Value < 0))
                        {
                            if (currentSegment.Count > 1) segments.Add(currentSegment);
                            currentSegment = new List<PointF>();
                        }

                        currentSegment.Add(new PointF(x, y));
                        prevY = y;

                        if (Math.Abs(y) < 500f)
                        {
                            if (y < tempMinY) tempMinY = y;
                            if (y > tempMaxY) tempMaxY = y;
                        }
                    }
                    catch
                    {
                        if (currentSegment.Count > 1) segments.Add(currentSegment);
                        currentSegment = new List<PointF>();
                        prevY = null;
                    }
                }
            }
            finally
            {
                Function.UseRadians = prevRad;
            }

            if (currentSegment.Count > 1)
            {
                segments.Add(currentSegment);
            }

            if (autoFitY)
            {
                if (tempMinY < tempMaxY)
                {
                    float h = tempMaxY - tempMinY;
                    if (h < 1e-3f) h = 4f;
                    rangeMinY = tempMinY - h * 0.18f;
                    rangeMaxY = tempMaxY + h * 0.18f;
                }
                else
                {
                    rangeMinY = -10f;
                    rangeMaxY = 10f;
                }
            }
        }

        private void PnlCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoom = (e.Delta > 0) ? 0.85f : 1.18f;

            float rangeW = rangeMaxX - rangeMinX;
            float rangeH = rangeMaxY - rangeMinY;
            if (rangeW < 1e-4f && zoom < 1f) return;
            if (rangeW > 1e6f && zoom > 1f) return;

            float midX = (rangeMaxX + rangeMinX) / 2f;
            float midY = (rangeMaxY + rangeMinY) / 2f;

            float halfW = (rangeW * zoom) / 2f;
            float halfH = (rangeH * zoom) / 2f;

            rangeMinX = midX - halfW;
            rangeMaxX = midX + halfW;
            rangeMinY = midY - halfH;
            rangeMaxY = midY + halfH;

            txtMinX.Text = rangeMinX.ToString("0.##", CultureInfo.InvariantCulture);
            txtMaxX.Text = rangeMaxX.ToString("0.##", CultureInfo.InvariantCulture);

            GeneratePoints(false);
            pnlCanvas.Invalidate();
        }

        private void PnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            pnlCanvas.Focus();
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                didMoveMouse = false;
                lastMousePos = e.Location;
                Cursor = Cursors.SizeAll;
            }
        }

        private void PnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            hoverMousePos = e.Location;

            if (isDragging)
            {
                int dx = e.X - lastMousePos.X;
                int dy = e.Y - lastMousePos.Y;

                if (Math.Abs(dx) > 2 || Math.Abs(dy) > 2)
                {
                    didMoveMouse = true;
                }

                float rangeW = rangeMaxX - rangeMinX;
                float rangeH = rangeMaxY - rangeMinY;

                float w = Math.Max(1, pnlCanvas.Width);
                float h = Math.Max(1, pnlCanvas.Height);

                float moveX = -dx * (rangeW / w);
                float moveY = dy * (rangeH / h);

                rangeMinX += moveX;
                rangeMaxX += moveX;
                rangeMinY += moveY;
                rangeMaxY += moveY;

                txtMinX.Text = rangeMinX.ToString("0.##", CultureInfo.InvariantCulture);
                txtMaxX.Text = rangeMaxX.ToString("0.##", CultureInfo.InvariantCulture);

                lastMousePos = e.Location;
                GeneratePoints(false);
            }

            pnlCanvas.Invalidate();
        }

        private void PnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!didMoveMouse && compiledFn != null)
                {
                    // Snap marked point onto the function curve at clicked X!
                    float w = Math.Max(1, pnlCanvas.Width);
                    float xPct = e.X / w;
                    float logX = rangeMinX + xPct * (rangeMaxX - rangeMinX);

                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fx = NumericalMethods.EvaluateCompiled(compiledFn, "x", (decimal)logX);
                        markedPoints.Add(new PointF(logX, (float)fx));
                    }
                    catch
                    {
                        float h = Math.Max(1, pnlCanvas.Height);
                        float yPct = (h - e.Y) / h;
                        float logY = rangeMinY + yPct * (rangeMaxY - rangeMinY);
                        markedPoints.Add(new PointF(logX, logY));
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                }

                isDragging = false;
                Cursor = Cursors.Cross;
                pnlCanvas.Invalidate();
            }
        }

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float w = Math.Max(1, pnlCanvas.Width);
            float h = Math.Max(1, pnlCanvas.Height);

            float MapX(float x) => (x - rangeMinX) / (rangeMaxX - rangeMinX) * w;
            float MapY(float y) => h - ((y - rangeMinY) / (rangeMaxY - rangeMinY) * h);

            // 1. Draw Grid
            DrawGrid(g, w, h, MapX, MapY);

            // 2. Draw Axes
            using (Pen axisPen = new Pen(Color.FromArgb(148, 163, 184), 1.8f))
            {
                float y0 = MapY(0);
                float x0 = MapX(0);

                if (y0 >= 0 && y0 <= h) g.DrawLine(axisPen, 0, y0, w, y0);
                if (x0 >= 0 && x0 <= w) g.DrawLine(axisPen, x0, 0, x0, h);
            }

            // 3. Draw Function Curve Segments
            if (segments.Count > 0)
            {
                using (Pen glowPen = new Pen(Color.FromArgb(45, 56, 189, 248), 6f))
                using (Pen graphPen = new Pen(Color.FromArgb(56, 189, 248), 2.4f))
                {
                    foreach (var seg in segments)
                    {
                        if (seg.Count < 2) continue;
                        List<PointF> screenPoints = new List<PointF>(seg.Count);
                        foreach (var p in seg)
                        {
                            float sx = MapX(p.X);
                            float sy = MapY(p.Y);
                            if (sy < -5000f) sy = -5000f;
                            if (sy > h + 5000f) sy = h + 5000f;
                            screenPoints.Add(new PointF(sx, sy));
                        }

                        try
                        {
                            PointF[] arr = screenPoints.ToArray();
                            g.DrawLines(glowPen, arr);
                            g.DrawLines(graphPen, arr);
                        }
                        catch { }
                    }
                }
            }

            // 4. Draw Live Hover Crosshair & f(x) Value
            if (hoverMousePos.HasValue && !isDragging && compiledFn != null)
            {
                float mx = hoverMousePos.Value.X;
                float logX = rangeMinX + (mx / w) * (rangeMaxX - rangeMinX);

                bool prevRad = Function.UseRadians;
                Function.UseRadians = true;
                try
                {
                    decimal fxDec = NumericalMethods.EvaluateCompiled(compiledFn, "x", (decimal)logX);
                    float logY = (float)fxDec;
                    float sy = MapY(logY);

                    using (Pen dashPen = new Pen(Color.FromArgb(90, 248, 250, 252), 1f))
                    {
                        dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        g.DrawLine(dashPen, mx, 0, mx, h);
                        if (sy >= 0 && sy <= h)
                        {
                            g.DrawLine(dashPen, 0, sy, w, sy);
                            using (Brush dotBrush = new SolidBrush(Color.FromArgb(56, 189, 248)))
                            {
                                g.FillEllipse(dotBrush, mx - 4, sy - 4, 8, 8);
                            }
                        }
                    }

                    string coordInfo = $"x = {logX:F3}  |  f(x) = {logY:F4}";
                    using (Font hudFont = new Font("Consolas", 9.5F, FontStyle.Bold))
                    using (Brush hudBg = new SolidBrush(Color.FromArgb(210, 15, 23, 42)))
                    using (Brush hudFg = new SolidBrush(Color.FromArgb(186, 230, 253)))
                    {
                        SizeF sz = g.MeasureString(coordInfo, hudFont);
                        RectangleF rect = new RectangleF(w - sz.Width - 24, 12, sz.Width + 12, sz.Height + 8);
                        g.FillRectangle(hudBg, rect);
                        g.DrawString(coordInfo, hudFont, hudFg, rect.X + 6, rect.Y + 4);
                    }
                }
                catch { }
                finally
                {
                    Function.UseRadians = prevRad;
                }
            }

            // 5. Draw Marked Points
            using (Brush markBrush = new SolidBrush(Color.FromArgb(250, 204, 21)))
            using (Brush bgTag = new SolidBrush(Color.FromArgb(200, 15, 23, 42)))
            using (Pen markPen = new Pen(Color.FromArgb(244, 63, 94), 2.2f))
            using (Font markFont = new Font("Consolas", 9F, FontStyle.Bold))
            {
                foreach (var p in markedPoints)
                {
                    float sx = MapX(p.X);
                    float sy = MapY(p.Y);
                    if (sx < -50 || sx > w + 50 || sy < -50 || sy > h + 50) continue;

                    g.FillEllipse(markBrush, sx - 4, sy - 4, 8, 8);
                    g.DrawEllipse(markPen, sx - 6, sy - 6, 12, 12);

                    string coord = $"({p.X:F2}, {p.Y:F2})";
                    SizeF sz = g.MeasureString(coord, markFont);
                    g.FillRectangle(bgTag, sx + 8, sy - 22, sz.Width + 4, sz.Height + 2);
                    g.DrawString(coord, markFont, markBrush, sx + 10, sy - 21);
                }
            }
        }

        private void DrawGrid(Graphics g, float w, float h, Func<float, float> mapX, Func<float, float> mapY)
        {
            using (Pen gridPen = new Pen(Color.FromArgb(30, 41, 59), 1))
            using (Font font = new Font("Segoe UI", 8.5f))
            using (Brush brush = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                float rangeW = rangeMaxX - rangeMinX;
                float stepW = CalculateStep(rangeW, 10);

                float rangeH = rangeMaxY - rangeMinY;
                float stepH = CalculateStep(rangeH, 8);

                if (stepW > 0)
                {
                    float startX = (float)Math.Floor(rangeMinX / stepW) * stepW;
                    for (float x = startX; x <= rangeMaxX; x += stepW)
                    {
                        float sx = mapX(x);
                        g.DrawLine(gridPen, sx, 0, sx, h);
                        g.DrawString(x.ToString("0.##", CultureInfo.InvariantCulture), font, brush, sx + 3, h - 20);
                    }
                }

                if (stepH > 0)
                {
                    float startY = (float)Math.Floor(rangeMinY / stepH) * stepH;
                    for (float y = startY; y <= rangeMaxY; y += stepH)
                    {
                        float sy = mapY(y);
                        g.DrawLine(gridPen, 0, sy, w, sy);
                        g.DrawString(y.ToString("0.##", CultureInfo.InvariantCulture), font, brush, 6, sy - 16);
                    }
                }
            }
        }

        private float CalculateStep(float range, int targetSteps)
        {
            if (range <= 0 || float.IsNaN(range) || float.IsInfinity(range)) return 1f;
            float rawStep = range / targetSteps;
            float mag = (float)Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
            float normStep = rawStep / mag;

            if (normStep < 1.5f) return 1f * mag;
            if (normStep < 3.5f) return 2f * mag;
            if (normStep < 7.5f) return 5f * mag;
            return 10f * mag;
        }

        private void pnlCanvas_Resize(object sender, EventArgs e)
        {
            pnlCanvas.Invalidate();
        }
    }
}
