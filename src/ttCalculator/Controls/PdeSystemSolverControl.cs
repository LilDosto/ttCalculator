using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class PdeSystemSolverControl : UserControl
    {
        private DifferentialEquations.PdeSystemSolutionGrid _currentGrid = null;
        private Bitmap _turingCache = null;
        private Timer _animTimer = new Timer();
        private decimal _minU = 0m, _maxU = 1m;
        private decimal _minV = 0m, _maxV = 1m;

        private PointF? _turingHoverPoint = null;
        private (decimal X, decimal T, decimal U, decimal V)? _turingHoverData = null;

        public PdeSystemSolverControl()
        {
            InitializeComponent();

            btnSolve.Click += BtnSolve_Click;
            btnExportCsv.Click += (s, e) => ExportDataAsCsv();
            btnExportHeatmapPng.Click += (s, e) => ExportTuringAsPng();

            btnExGrayScott.Click += (s, e) => LoadPreset(
                "-u*v^2 + 0.03*(1 - u)",
                "u*v^2 - (0.03 + 0.062)*v",
                "1 - exp(-120*(x-0.5)^2)",
                "exp(-120*(x-0.5)^2)",
                "0.00002", "0.00001", "1", "50", 60, 250);

            btnExSchnakenberg.Click += (s, e) => LoadPreset(
                "0.1 - u + u^2*v",
                "0.9 - u^2*v",
                "1 + 0.1*cos(pi*x)",
                "0.9 + 0.1*sin(pi*x)",
                "1", "10", "20", "2", 60, 200);

            btnExFitzHugh.Click += (s, e) => LoadPreset(
                "u*(u - 0.1)*(1 - u) - v",
                "0.01*(u - 0.5*v)",
                "exp(-50*(x-0.2)^2)",
                "0",
                "1", "0", "1", "20", 60, 200);

            btnExDiffusion.Click += (s, e) => LoadPreset(
                "-0.5*u + 0.2*v",
                "0.2*u - 0.5*v",
                "exp(-40*(x-0.3)^2)",
                "exp(-40*(x-0.7)^2)",
                "0.5", "1", "1", "1", 50, 150);

            btnPlayPause.Click += (s, e) => ToggleAnimation();
            trkTimeStep.Scroll += (s, e) => UpdateAnimationDisplay();

            _animTimer.Interval = 33;
            _animTimer.Tick += (s, e) => AdvanceAnimation();

            pnlDualCanvas.Paint += PnlDualCanvas_Paint;
            pnlDualCanvas.Resize += (s, e) => pnlDualCanvas.Invalidate();

            pnlTuringCanvas.Paint += PnlTuringCanvas_Paint;
            pnlTuringCanvas.MouseMove += PnlTuringCanvas_MouseMove;
            pnlTuringCanvas.MouseLeave += (s, e) => { _turingHoverPoint = null; _turingHoverData = null; pnlTuringCanvas.Invalidate(); };
            pnlTuringCanvas.Resize += (s, e) => pnlTuringCanvas.Invalidate();

            pnlPhaseCanvas.Paint += PnlPhaseCanvas_Paint;
            pnlPhaseCanvas.Resize += (s, e) => pnlPhaseCanvas.Invalidate();

            // Default
            LoadPreset(
                "-u*v^2 + 0.03*(1 - u)",
                "u*v^2 - (0.03 + 0.062)*v",
                "1 - exp(-120*(x-0.5)^2)",
                "exp(-120*(x-0.5)^2)",
                "0.00002", "0.00001", "1", "50", 60, 250);
        }

        private void LoadPreset(string f, string g, string u0, string v0, string du, string dv, string L, string T, int Nx, int Nt)
        {
            txtReactionF.Text = f;
            txtReactionG.Text = g;
            txtInitU.Text = u0;
            txtInitV.Text = v0;
            txtDu.Text = du;
            txtDv.Text = dv;
            txtL.Text = L;
            txtT.Text = T;
            numNx.Value = Nx;
            numNt.Value = Nt;

            BtnSolve_Click(this, EventArgs.Empty);
        }

        private decimal ParseDec(string s, string field)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal v))
                return v;
            throw new ArgumentException($"'{field}' için geçerli bir sayı giriniz.");
        }

        private async void BtnSolve_Click(object sender, EventArgs e)
        {
            string origBtnText = btnSolve.Text;
            btnSolve.Enabled = false;
            btnSolve.Text = "⏳ Desen Simüle Ediliyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string f = txtReactionF.Text.Trim();
                string g = txtReactionG.Text.Trim();
                string initU = txtInitU.Text.Trim();
                string initV = txtInitV.Text.Trim();
                decimal du = ParseDec(txtDu.Text, "Du");
                decimal dv = ParseDec(txtDv.Text, "Dv");
                decimal L = ParseDec(txtL.Text, "Uzunluk L");
                decimal T = ParseDec(txtT.Text, "Süre T");
                int Nx = (int)numNx.Value;
                int Nt = (int)numNt.Value;

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var grid = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        return DifferentialEquations.SolveCoupledPdeSystem(f, g, initU, initV, L, T, Nx, Nt, du, dv);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });
                sw.Stop();

                if (grid != null && grid.U != null && grid.V != null)
                {
                    _currentGrid = grid;

                    int nx = grid.XGrid.Length;
                    int nt = grid.TGrid.Length;

                    _minU = decimal.MaxValue; _maxU = decimal.MinValue;
                    _minV = decimal.MaxValue; _maxV = decimal.MinValue;

                    for (int i = 0; i < nx; i++)
                    {
                        for (int n = 0; n < nt; n++)
                        {
                            decimal u = grid.U[i, n];
                            if (u < _minU) _minU = u;
                            if (u > _maxU) _maxU = u;

                            decimal v = grid.V[i, n];
                            if (v < _minV) _minV = v;
                            if (v > _maxV) _maxV = v;
                        }
                    }

                    if (_maxU - _minU < 0.0001m) _maxU = _minU + 1m;
                    if (_maxV - _minV < 0.0001m) _maxV = _minV + 1m;

                    // Generate Turing Heatmap Bitmap
                    GenerateTuringBitmap(grid, nx, nt);

                    // Setup animation controls
                    trkTimeStep.Maximum = nt - 1;
                    trkTimeStep.Value = 0;
                    UpdateAnimationDisplay();

                    // Report text
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=== BAĞLAŞIMLI PDE (REAKSİYON-DİFÜZYON) ÇÖZÜM RAPORU ===");
                    sb.AppendLine($"  Reaksiyon F(u, v)   : {f}");
                    sb.AppendLine($"  Reaksiyon G(u, v)   : {g}");
                    sb.AppendLine($"  Difüzyon Du, Dv     : {du}, {dv}");
                    sb.AppendLine($"  Uzay & Zaman        : x ∈ [0, {L}] ({Nx} nokta), t ∈ [0, {T}] ({Nt} adım)");
                    sb.AppendLine($"  Çözüm Süresi        : {sw.ElapsedMilliseconds} ms");
                    sb.AppendLine($"  u Değer Aralığı     : [{Math.Round(_minU, 5)}, {Math.Round(_maxU, 5)}]");
                    sb.AppendLine($"  v Değer Aralığı     : [{Math.Round(_minV, 5)}, {Math.Round(_maxV, 5)}]");
                    sb.AppendLine(new string('=', 65));

                    txtOutput.Text = sb.ToString();

                    pnlDualCanvas.Invalidate();
                    pnlTuringCanvas.Invalidate();
                    pnlPhaseCanvas.Invalidate();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "❌ Hata: " + ex.Message;
                MessageBox.Show("Sistem Çözüm Hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void GenerateTuringBitmap(DifferentialEquations.PdeSystemSolutionGrid grid, int nx, int nt)
        {
            _turingCache?.Dispose();
            _turingCache = new Bitmap(nx, nt, PixelFormat.Format32bppArgb);

            decimal span = _maxU - _minU;
            if (span == 0m) span = 1m;

            for (int t = 0; t < nt; t++)
            {
                int imgY = nt - 1 - t;
                for (int x = 0; x < nx; x++)
                {
                    decimal val = grid.U[x, t];
                    float norm = (float)((val - _minU) / span);
                    if (norm < 0f) norm = 0f;
                    if (norm > 1f) norm = 1f;

                    Color c = ValueToColormap(norm);
                    _turingCache.SetPixel(x, imgY, c);
                }
            }
        }

        private Color ValueToColormap(float v)
        {
            if (v <= 0.25f)
            {
                float t = v / 0.25f;
                return Color.FromArgb((int)(15 + t * (6 - 15)), (int)(23 + t * (182 - 23)), (int)(70 + t * (212 - 70)));
            }
            else if (v <= 0.5f)
            {
                float t = (v - 0.25f) / 0.25f;
                return Color.FromArgb((int)(6 + t * (16 - 6)), (int)(182 + t * (185 - 182)), (int)(212 + t * (129 - 212)));
            }
            else if (v <= 0.75f)
            {
                float t = (v - 0.5f) / 0.25f;
                return Color.FromArgb((int)(16 + t * (245 - 16)), (int)(185 + t * (158 - 185)), (int)(129 + t * (11 - 129)));
            }
            else
            {
                float t = (v - 0.75f) / 0.25f;
                return Color.FromArgb((int)(245 + t * (239 - 245)), (int)(158 + t * (68 - 158)), (int)(11 + t * (68 - 11)));
            }
        }

        private void ToggleAnimation()
        {
            if (_currentGrid == null) return;
            if (_animTimer.Enabled)
            {
                _animTimer.Stop();
                btnPlayPause.Text = "▶ Oynat";
                btnPlayPause.BackColor = Color.FromArgb(16, 185, 129);
            }
            else
            {
                _animTimer.Start();
                btnPlayPause.Text = "⏸ Durdur";
                btnPlayPause.BackColor = Color.FromArgb(239, 68, 68);
            }
        }

        private void AdvanceAnimation()
        {
            if (_currentGrid == null) return;
            int nt = _currentGrid.TGrid.Length;
            int speed = cmbAnimSpeed.SelectedIndex == 1 ? 2 : (cmbAnimSpeed.SelectedIndex == 2 ? 5 : 1);
            int next = (trkTimeStep.Value + speed) % nt;
            trkTimeStep.Value = next;
            UpdateAnimationDisplay();
        }

        private void UpdateAnimationDisplay()
        {
            if (_currentGrid == null) return;
            int curStep = trkTimeStep.Value;
            decimal curT = _currentGrid.TGrid[curStep];
            lblCurrentTime.Text = $"t = {curT:F3}s ({curStep}/{_currentGrid.TGrid.Length - 1})";
            pnlDualCanvas.Invalidate();
            pnlPhaseCanvas.Invalidate();
        }

        private void PnlDualCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = pnlDualCanvas.Width;
            int h = pnlDualCanvas.Height;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentGrid == null)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("PDE sistemi çözüldükten sonra canlı çift konsantrasyon dalgası u(x) & v(x) burada oynatılır.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            int curStep = Math.Min(trkTimeStep.Value, _currentGrid.TGrid.Length - 1);
            int nx = _currentGrid.XGrid.Length;
            decimal L = _currentGrid.XGrid[nx - 1];

            int padL = 45, padB = 30, padT = 32, padR = 25;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            decimal globalMin = Math.Min(_minU, _minV);
            decimal globalMax = Math.Max(_maxU, _maxV);
            decimal span = globalMax - globalMin;
            if (span == 0m) span = 1m;
            decimal yMin = globalMin - span * 0.08m;
            decimal yMax = globalMax + span * 0.08m;
            decimal ySpan = yMax - yMin;

            float ToScreenX(decimal x) => padL + (float)(x / L * (decimal)plotW);
            float ToScreenY(decimal val) => padT + (float)((yMax - val) / ySpan * (decimal)plotH);

            // Grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(24, 33, 52), 1f) { DashStyle = DashStyle.Dash })
            using (Font lblFont = new Font("Consolas", 7.5F))
            using (Brush lblBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                for (int i = 0; i <= 5; i++)
                {
                    decimal gx = (i / 5m) * L;
                    float sx = ToScreenX(gx);
                    g.DrawLine(gridPen, sx, padT, sx, h - padB);
                    g.DrawString(Math.Round(gx, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, sx - 10, h - padB + 4);

                    decimal gy = yMin + (ySpan * i / 5m);
                    float sy = ToScreenY(gy);
                    g.DrawLine(gridPen, padL, sy, w - padR, sy);
                    g.DrawString(Math.Round(gy, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, 4, sy - 6);
                }
            }

            // Zero line
            float zeroY = ToScreenY(0m);
            if (zeroY >= padT && zeroY <= h - padB)
            {
                using (Pen zeroPen = new Pen(Color.FromArgb(71, 85, 105), 1.2f))
                {
                    g.DrawLine(zeroPen, padL, zeroY, w - padR, zeroY);
                }
            }

            // Box
            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            // Points for u(x) and v(x)
            List<PointF> uPts = new List<PointF>(nx);
            List<PointF> vPts = new List<PointF>(nx);

            for (int i = 0; i < nx; i++)
            {
                decimal x = _currentGrid.XGrid[i];
                uPts.Add(new PointF(ToScreenX(x), ToScreenY(_currentGrid.U[i, curStep])));
                vPts.Add(new PointF(ToScreenX(x), ToScreenY(_currentGrid.V[i, curStep])));
            }

            // Area fill for u (translucent blue)
            List<PointF> uPoly = new List<PointF>(uPts);
            uPoly.Add(new PointF(padL + plotW, ToScreenY(0m)));
            uPoly.Add(new PointF(padL, ToScreenY(0m)));
            using (Brush uFill = new SolidBrush(Color.FromArgb(35, 56, 189, 248)))
            {
                g.FillPolygon(uFill, uPoly.ToArray());
            }

            // Curve for u
            using (Pen uPen = new Pen(Color.FromArgb(56, 189, 248), 2.2f))
            {
                uPen.LineJoin = LineJoin.Round;
                for (int i = 0; i < uPts.Count - 1; i++) g.DrawLine(uPen, uPts[i], uPts[i + 1]);
            }

            // Curve for v (rose)
            using (Pen vPen = new Pen(Color.FromArgb(244, 63, 94), 2.2f))
            {
                vPen.LineJoin = LineJoin.Round;
                for (int i = 0; i < vPts.Count - 1; i++) g.DrawLine(vPen, vPts[i], vPts[i + 1]);
            }

            // Legend
            decimal curT = _currentGrid.TGrid[curStep];
            using (Font fLeg = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
            using (Brush bU = new SolidBrush(Color.FromArgb(56, 189, 248)))
            using (Brush bV = new SolidBrush(Color.FromArgb(244, 63, 94)))
            {
                g.DrawString("— u(x) Aktivatör / Bileşen 1", fLeg, bU, padL + 8, 8);
                g.DrawString("— v(x) İnhibitör / Bileşen 2", fLeg, bV, padL + 210, 8);
            }
        }

        private void PnlTuringCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = pnlTuringCanvas.Width;
            int h = pnlTuringCanvas.Height;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentGrid == null || _turingCache == null)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("Sistem çözüldüğünde Mekan-Zaman Turing Deseni Haritası burada çizilir.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            int padL = 48, padB = 30, padT = 20, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            g.InterpolationMode = InterpolationMode.Bilinear;
            g.DrawImage(_turingCache, new RectangleF(padL, padT, plotW, plotH));

            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            using (Font lblFont = new Font("Consolas", 7.5F))
            using (Brush lblBrush = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                decimal L = _currentGrid.XGrid[_currentGrid.XGrid.Length - 1];
                decimal T = _currentGrid.TGrid[_currentGrid.TGrid.Length - 1];

                for (int i = 0; i <= 5; i++)
                {
                    float sx = padL + (i / 5f) * plotW;
                    decimal gx = (i / 5m) * L;
                    g.DrawString(Math.Round(gx, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, sx - 10, h - padB + 4);

                    float sy = padT + (1f - (i / 5f)) * plotH;
                    decimal gt = (i / 5m) * T;
                    g.DrawString(Math.Round(gt, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, 4, sy - 6);
                }
                g.DrawString("Mekan (x) →", lblFont, lblBrush, padL + plotW * 0.45f, h - padB + 16);
                g.DrawString("Zaman (t) ↑", lblFont, lblBrush, 4, padT - 14);
            }

            // Crosshair
            if (_turingHoverPoint.HasValue && _turingHoverData.HasValue)
            {
                PointF hp = _turingHoverPoint.Value;
                using (Pen crossPen = new Pen(Color.FromArgb(255, 255, 255), 1f) { DashStyle = DashStyle.Dot })
                {
                    g.DrawLine(crossPen, hp.X, padT, hp.X, h - padB);
                    g.DrawLine(crossPen, padL, hp.Y, padL + plotW, hp.Y);
                }

                var d = _turingHoverData.Value;
                string tip = $"x = {Math.Round(d.X, 3)}\nt = {Math.Round(d.T, 3)}s\nu = {Math.Round(d.U, 4)}\nv = {Math.Round(d.V, 4)}";
                using (Font tipFont = new Font("Consolas", 8F, FontStyle.Bold))
                {
                    SizeF sz = g.MeasureString(tip, tipFont);
                    float tx = hp.X + 10;
                    float ty = hp.Y - sz.Height - 6;
                    if (tx + sz.Width + 8 > w) tx = hp.X - sz.Width - 12;
                    if (ty < padT) ty = hp.Y + 12;

                    RectangleF tr = new RectangleF(tx, ty, sz.Width + 8, sz.Height + 6);
                    using (Brush bg = new SolidBrush(Color.FromArgb(240, 24, 33, 52)))
                    using (Pen bp = new Pen(Color.FromArgb(56, 189, 248), 1.2f))
                    using (Brush txtB = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(bg, tr);
                        g.DrawRectangle(bp, tr.X, tr.Y, tr.Width, tr.Height);
                        g.DrawString(tip, tipFont, txtB, tx + 4, ty + 3);
                    }
                }
            }
        }

        private void PnlTuringCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentGrid == null) return;
            int w = pnlTuringCanvas.Width;
            int h = pnlTuringCanvas.Height;
            int padL = 48, padB = 30, padT = 20, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            if (e.X >= padL && e.X <= padL + plotW && e.Y >= padT && e.Y <= padT + plotH)
            {
                float relX = (e.X - padL) / plotW;
                float relY = 1f - ((e.Y - padT) / plotH);

                int nx = _currentGrid.XGrid.Length;
                int nt = _currentGrid.TGrid.Length;

                int xi = Math.Max(0, Math.Min(nx - 1, (int)(relX * (nx - 1))));
                int ti = Math.Max(0, Math.Min(nt - 1, (int)(relY * (nt - 1))));

                decimal xVal = _currentGrid.XGrid[xi];
                decimal tVal = _currentGrid.TGrid[ti];
                decimal uVal = _currentGrid.U[xi, ti];
                decimal vVal = _currentGrid.V[xi, ti];

                _turingHoverPoint = new PointF(e.X, e.Y);
                _turingHoverData = (xVal, tVal, uVal, vVal);
            }
            else
            {
                _turingHoverPoint = null;
                _turingHoverData = null;
            }
            pnlTuringCanvas.Invalidate();
        }

        private void PnlPhaseCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = pnlPhaseCanvas.Width;
            int h = pnlPhaseCanvas.Height;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentGrid == null) return;

            int curStep = Math.Min(trkTimeStep.Value, _currentGrid.TGrid.Length - 1);
            int nx = _currentGrid.XGrid.Length;

            int padL = 48, padB = 30, padT = 25, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            decimal spanU = _maxU - _minU; if (spanU == 0m) spanU = 1m;
            decimal spanV = _maxV - _minV; if (spanV == 0m) spanV = 1m;

            float ToScreenX(decimal u) => padL + (float)((u - _minU) / spanU * (decimal)plotW);
            float ToScreenY(decimal v) => padT + (float)((_maxV - v) / spanV * (decimal)plotH);

            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            List<PointF> phasePts = new List<PointF>(nx);
            for (int i = 0; i < nx; i++)
            {
                phasePts.Add(new PointF(ToScreenX(_currentGrid.U[i, curStep]), ToScreenY(_currentGrid.V[i, curStep])));
            }

            using (Pen phasePen = new Pen(Color.FromArgb(251, 191, 36), 2f))
            {
                phasePen.LineJoin = LineJoin.Round;
                for (int i = 0; i < phasePts.Count - 1; i++)
                {
                    g.DrawLine(phasePen, phasePts[i], phasePts[i + 1]);
                }
            }

            using (Font titleF = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
            using (Brush txtB = new SolidBrush(Color.FromArgb(251, 191, 36)))
            {
                decimal curT = _currentGrid.TGrid[curStep];
                g.DrawString($"Faz Portresi: v(x) vs u(x) ── t = {curT:F3} sn", titleF, txtB, padL + 8, 6);
            }
        }

        private void ExportDataAsCsv()
        {
            if (_currentGrid == null)
            {
                MessageBox.Show("Önce PDE sistemini çözünüz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Dosyası (*.csv)|*.csv";
                sfd.FileName = "Reaksiyon_Difuzyon_Cozum.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("x,t,u,v");
                    int nx = _currentGrid.XGrid.Length;
                    int nt = _currentGrid.TGrid.Length;
                    for (int n = 0; n < nt; n++)
                    {
                        for (int i = 0; i < nx; i++)
                        {
                            sb.AppendLine($"{_currentGrid.XGrid[i]},{_currentGrid.TGrid[n]},{_currentGrid.U[i, n]},{_currentGrid.V[i, n]}");
                        }
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Veriler CSV olarak başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ExportTuringAsPng()
        {
            if (_turingCache == null) return;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Resmi (*.png)|*.png";
                sfd.FileName = "Turing_Deseni_Haritasi.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bmp = new Bitmap(1200, 800))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.FromArgb(11, 15, 28));
                        g.DrawImage(_turingCache, new Rectangle(60, 40, 1080, 720));
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Turing deseni haritası yüksek çözünürlükte kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
