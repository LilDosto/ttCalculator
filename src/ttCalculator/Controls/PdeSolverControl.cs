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
    public partial class PdeSolverControl : UserControl
    {
        private DifferentialEquations.PdeSolutionGrid _currentGrid = null;
        private Bitmap _heatmapCache = null;
        private Timer _animTimer = new Timer();
        private decimal _minU = 0m, _maxU = 1m;

        private PointF? _heatmapHoverPoint = null;
        private (decimal X, decimal T, decimal U)? _heatmapHoverData = null;

        public PdeSolverControl()
        {
            InitializeComponent();

            btnSolve.Click += BtnSolve_Click;
            btnCopyPdeTsv.Click += (s, e) => CopyGridAsTsv();
            btnExportHeatmapPng.Click += (s, e) => ExportHeatmapAsPng();

            btnExHeat.Click += (s, e) => LoadPreset(0, "sin(pi*x)", "0", "1", "1", "0.2", 50, 200, "0", "0");
            btnExWave.Click += (s, e) => LoadPreset(2, "sin(pi*x)", "0", "2", "1", "1", 80, 300, "0", "0");
            btnExAdvect.Click += (s, e) => LoadPreset(3, "exp(-50*(x-0.2)^2)", "0", "1.5", "1", "0.5", 100, 250, "0", "0");
            btnExGauss.Click += (s, e) => LoadPreset(0, "exp(-100*(x-0.5)^2)", "0", "0.5", "1", "0.1", 60, 200, "0", "0");

            cmbPdeType.SelectedIndexChanged += (s, e) => UpdateLiveCflStatus();
            txtL.TextChanged += (s, e) => UpdateLiveCflStatus();
            txtT.TextChanged += (s, e) => UpdateLiveCflStatus();
            txtParam.TextChanged += (s, e) => UpdateLiveCflStatus();
            numNx.ValueChanged += (s, e) => UpdateLiveCflStatus();
            numNt.ValueChanged += (s, e) => UpdateLiveCflStatus();

            btnPlayPause.Click += (s, e) => ToggleAnimation();
            trkTimeStep.Scroll += (s, e) => UpdateAnimationDisplay();
            cmbAnimSpeed.SelectedIndexChanged += (s, e) => UpdateAnimSpeed();

            _animTimer.Interval = 33; // ~30 fps
            _animTimer.Tick += (s, e) => AdvanceAnimation();

            pnlHeatmapCanvas.Paint += PnlHeatmapCanvas_Paint;
            pnlHeatmapCanvas.MouseMove += PnlHeatmapCanvas_MouseMove;
            pnlHeatmapCanvas.MouseLeave += (s, e) => { _heatmapHoverPoint = null; _heatmapHoverData = null; pnlHeatmapCanvas.Invalidate(); };
            pnlHeatmapCanvas.Resize += (s, e) => pnlHeatmapCanvas.Invalidate();

            pnlAnimCanvas.Paint += PnlAnimCanvas_Paint;
            pnlAnimCanvas.Resize += (s, e) => pnlAnimCanvas.Invalidate();

            // Default
            UpdateLiveCflStatus();
            LoadPreset(0, "sin(pi*x)", "0", "1", "1", "0.2", 50, 200, "0", "0");
        }

        private void LoadPreset(int pdeIdx, string initU, string initVel, string param, string L, string T, int Nx, int Nt, string bcL, string bcR)
        {
            cmbPdeType.SelectedIndex = pdeIdx;
            txtInitU.Text = initU;
            txtInitVel.Text = initVel;
            txtParam.Text = param;
            txtL.Text = L;
            txtT.Text = T;
            numNx.Value = Nx;
            numNt.Value = Nt;
            txtBcLeft.Text = bcL;
            txtBcRight.Text = bcR;

            UpdateLiveCflStatus();
            BtnSolve_Click(this, EventArgs.Empty);
        }

        private void UpdateLiveCflStatus()
        {
            try
            {
                decimal L = ParseDecSilent(txtL.Text, 1m);
                decimal T = ParseDecSilent(txtT.Text, 0.2m);
                decimal param = ParseDecSilent(txtParam.Text, 1m);
                int Nx = (int)numNx.Value;
                int Nt = (int)numNt.Value;

                if (L <= 0 || T <= 0 || Nx < 2 || Nt < 2) return;

                decimal dx = L / (Nx - 1);
                decimal dt = T / (Nt - 1);
                int pdeIdx = cmbPdeType.SelectedIndex;

                if (pdeIdx == 0) // Heat FTCS
                {
                    decimal r = (param * dt) / (dx * dx);
                    if (r <= 0.5m)
                    {
                        lblCflText.Text = $"✓ Kararlı (FTCS): r = α·Δt / Δx² = {r:F4} ≤ 0.5 (Açık Şema Doğal Kararlı)";
                        lblCflText.ForeColor = Color.FromArgb(52, 211, 153);
                    }
                    else
                    {
                        int subs = (int)Math.Min(1000m, Math.Ceiling(r / 0.45m));
                        lblCflText.Text = $"⚡ Uyarlamalı FTCS: r = {r:F2} > 0.5 (Çözücü otomatik {subs} alt-adımlama ile kararlılığı sağlar)";
                        lblCflText.ForeColor = Color.FromArgb(56, 189, 248);
                    }
                }
                else if (pdeIdx == 1) // Heat Crank-Nicolson
                {
                    decimal r = (param * dt) / (dx * dx);
                    lblCflText.Text = $"✓ Koşulsuz Kararlı (Crank-Nicolson): r = {r:F4} (Tüm r değerleri için mutlak kararlı)";
                    lblCflText.ForeColor = Color.FromArgb(52, 211, 153);
                }
                else if (pdeIdx == 2) // Wave Leapfrog
                {
                    decimal cfl = (param * dt) / dx;
                    if (cfl <= 1.0m)
                    {
                        lblCflText.Text = $"✓ Kararlı (Leapfrog Dalga): CFL = c·Δt / Δx = {cfl:F4} ≤ 1.0 (Courant Koşulu Sağlandı)";
                        lblCflText.ForeColor = Color.FromArgb(52, 211, 153);
                    }
                    else
                    {
                        int subs = (int)Math.Min(1000m, Math.Ceiling(cfl / 0.9m));
                        lblCflText.Text = $"⚡ Uyarlamalı Dalga: CFL = {cfl:F2} > 1.0 (Çözücü otomatik {subs} alt-adımlama ile Courant koşulunu sağlar)";
                        lblCflText.ForeColor = Color.FromArgb(56, 189, 248);
                    }
                }
                else // Advection
                {
                    decimal cfl = (param * dt) / dx;
                    if (DecimalMath.Abs(cfl) <= 1.0m)
                    {
                        lblCflText.Text = $"✓ Kararlı (Upwind Taşınım): CFL = v·Δt / Δx = {cfl:F4} ≤ 1.0";
                        lblCflText.ForeColor = Color.FromArgb(52, 211, 153);
                    }
                    else
                    {
                        int subs = (int)Math.Min(1000m, Math.Ceiling(DecimalMath.Abs(cfl) / 0.9m));
                        lblCflText.Text = $"⚡ Uyarlamalı Taşınım: CFL = {cfl:F2} > 1.0 (Çözücü otomatik {subs} alt-adımlama ile kararlılığı sağlar)";
                        lblCflText.ForeColor = Color.FromArgb(56, 189, 248);
                    }
                }
            }
            catch { }
        }

        private decimal ParseDecSilent(string s, decimal def)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal v))
                return v;
            return def;
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
            btnSolve.Text = "⏳ Simüle Ediliyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                int pdeIdx = cmbPdeType.SelectedIndex;
                DifferentialEquations.PdeType pdeType = DifferentialEquations.PdeType.HeatEquation;
                if (pdeIdx == 1) pdeType = DifferentialEquations.PdeType.HeatEquationCrankNicolson;
                else if (pdeIdx == 2) pdeType = DifferentialEquations.PdeType.WaveEquation;
                else if (pdeIdx == 3) pdeType = DifferentialEquations.PdeType.AdvectionEquation;

                string initU = txtInitU.Text.Trim();
                string initVel = txtInitVel.Text.Trim();
                decimal param = ParseDec(txtParam.Text, "α / c / v");
                decimal L = ParseDec(txtL.Text, "Uzunluk L");
                decimal T = ParseDec(txtT.Text, "Süre T");
                int Nx = (int)numNx.Value;
                int Nt = (int)numNt.Value;
                decimal bcL = ParseDec(txtBcLeft.Text, "Sol Sınır u(0)");
                decimal bcR = ParseDec(txtBcRight.Text, "Sağ Sınır u(L)");

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var grid = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        return DifferentialEquations.Solve1DPde(pdeType, initU, L, T, Nx, Nt, param, bcL, bcR, initVel);
                    }
                    finally
                    {
                        Function.UseRadians = prevRad;
                    }
                });
                sw.Stop();

                if (grid != null && grid.U != null)
                {
                    _currentGrid = grid;

                    // Min and Max values for colormap
                    _minU = decimal.MaxValue;
                    _maxU = decimal.MinValue;
                    int nx = grid.XGrid.Length;
                    int nt = grid.TGrid.Length;

                    for (int i = 0; i < nx; i++)
                    {
                        for (int n = 0; n < nt; n++)
                        {
                            decimal val = grid.U[i, n];
                            if (val < _minU) _minU = val;
                            if (val > _maxU) _maxU = val;
                        }
                    }

                    if (_maxU - _minU < 0.0001m) _maxU = _minU + 1m;

                    // Generate Heatmap Bitmap
                    GenerateHeatmapBitmap(grid, nx, nt);

                    // Setup animation controls
                    trkTimeStep.Maximum = nt - 1;
                    trkTimeStep.Value = 0;
                    UpdateAnimationDisplay();

                    // Populate Snapshot Table
                    PopulateSnapshotTable(grid, nx, nt);

                    // Report text
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=== KISMİ DİFERANSİYEL DENKLEM (1D PDE) ÇÖZÜM RAPORU ===");
                    sb.AppendLine($"  Tip                 : {cmbPdeType.SelectedItem}");
                    sb.AppendLine($"  Başlangıç Koşulu    : u(x, 0) = {initU}");
                    sb.AppendLine($"  Mekan & Zaman Ağı   : x ∈ [0, {L}] ({Nx} nokta), t ∈ [0, {T}] ({Nt} adım)");
                    sb.AppendLine($"  Parametre (α/c/v)   : {param}");
                    sb.AppendLine($"  Sınırlar [u(0),u(L)]: [{bcL}, {bcR}]");
                    sb.AppendLine($"  Kararlılık Analizi  : {grid.StabilityNote}");
                    sb.AppendLine($"  Çözüm Süresi        : {sw.ElapsedMilliseconds} ms");
                    sb.AppendLine($"  Değer Aralığı [Min, Maks]: [{Math.Round(_minU, 5)}, {Math.Round(_maxU, 5)}]");
                    sb.AppendLine(new string('=', 65));

                    txtOutput.Text = sb.ToString();

                    pnlHeatmapCanvas.Invalidate();
                    pnlAnimCanvas.Invalidate();
                }
            }
            catch (Exception ex)
            {
                string msg = ex is OverflowException
                    ? "Sayısal taşma (Overflow): Parametreler veya fonksiyon değerleri hesaplama sınırlarını aştı. Lütfen parametreleri kontrol ediniz."
                    : ex.Message;
                txtOutput.Text = "❌ Hata: " + msg;
                MessageBox.Show("PDE Çözüm Hatası: " + msg, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void GenerateHeatmapBitmap(DifferentialEquations.PdeSolutionGrid grid, int nx, int nt)
        {
            _heatmapCache?.Dispose();
            _heatmapCache = new Bitmap(nx, nt, PixelFormat.Format32bppArgb);

            decimal span = _maxU - _minU;
            if (span == 0m) span = 1m;

            for (int t = 0; t < nt; t++)
            {
                // In image coordinates, y=0 is top (t=T), y=nt-1 is bottom (t=0)
                int imgY = nt - 1 - t;
                for (int x = 0; x < nx; x++)
                {
                    decimal val = grid.U[x, t];
                    float norm = (float)((val - _minU) / span);
                    if (norm < 0f) norm = 0f;
                    if (norm > 1f) norm = 1f;

                    Color c = ValueToColormap(norm);
                    _heatmapCache.SetPixel(x, imgY, c);
                }
            }
        }

        private Color ValueToColormap(float v)
        {
            // Turbo / Thermal spectrum: 0 = Dark Navy, 0.25 = Cyan, 0.5 = Emerald, 0.75 = Amber/Yellow, 1 = Red
            if (v <= 0.25f)
            {
                float t = v / 0.25f;
                return Color.FromArgb(
                    (int)(15 + t * (6 - 15)),
                    (int)(23 + t * (182 - 23)),
                    (int)(70 + t * (212 - 70)));
            }
            else if (v <= 0.5f)
            {
                float t = (v - 0.25f) / 0.25f;
                return Color.FromArgb(
                    (int)(6 + t * (16 - 6)),
                    (int)(182 + t * (185 - 182)),
                    (int)(212 + t * (129 - 212)));
            }
            else if (v <= 0.75f)
            {
                float t = (v - 0.5f) / 0.25f;
                return Color.FromArgb(
                    (int)(16 + t * (245 - 16)),
                    (int)(185 + t * (158 - 185)),
                    (int)(129 + t * (11 - 129)));
            }
            else
            {
                float t = (v - 0.75f) / 0.25f;
                return Color.FromArgb(
                    (int)(245 + t * (239 - 245)),
                    (int)(158 + t * (68 - 158)),
                    (int)(11 + t * (68 - 11)));
            }
        }

        private void PnlHeatmapCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = pnlHeatmapCanvas.Width;
            int h = pnlHeatmapCanvas.Height;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentGrid == null || _heatmapCache == null)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("PDE çözüldüğünde 2B Mekan-Zaman Isı Haritası (Heatmap) burada çizilir.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            int padL = 48, padB = 30, padT = 20, padR = 75;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            // Draw heatmap bitmap
            g.InterpolationMode = InterpolationMode.Bilinear;
            g.DrawImage(_heatmapCache, new RectangleF(padL, padT, plotW, plotH));

            // Axis box
            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            // Grid tick labels
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
                g.DrawString("x (Mekan) →", lblFont, lblBrush, padL + plotW * 0.45f, h - padB + 16);
                g.DrawString("t (Zaman) ↑", lblFont, lblBrush, 4, padT - 14);
            }

            // Draw color bar legend on the right side
            float cbX = w - padR + 15;
            float cbW = 16;
            float cbH = plotH;
            for (int y = 0; y < (int)cbH; y++)
            {
                float norm = 1f - (y / cbH);
                using (Pen cPen = new Pen(ValueToColormap(norm)))
                {
                    g.DrawLine(cPen, cbX, padT + y, cbX + cbW, padT + y);
                }
            }
            using (Pen cbBorder = new Pen(Color.FromArgb(51, 65, 85), 1f))
            using (Font fMini = new Font("Consolas", 7F))
            using (Brush bMini = new SolidBrush(Color.FromArgb(226, 232, 240)))
            {
                g.DrawRectangle(cbBorder, cbX, padT, cbW, cbH);
                g.DrawString(Math.Round(_maxU, 2).ToString(CultureInfo.InvariantCulture), fMini, bMini, cbX + cbW + 4, padT - 4);
                g.DrawString(Math.Round((_maxU + _minU) / 2m, 2).ToString(CultureInfo.InvariantCulture), fMini, bMini, cbX + cbW + 4, padT + cbH * 0.5f - 4);
                g.DrawString(Math.Round(_minU, 2).ToString(CultureInfo.InvariantCulture), fMini, bMini, cbX + cbW + 4, padT + cbH - 8);
            }

            // Crosshair & Tooltip
            if (_heatmapHoverPoint.HasValue && _heatmapHoverData.HasValue)
            {
                PointF hp = _heatmapHoverPoint.Value;
                using (Pen crossPen = new Pen(Color.FromArgb(255, 255, 255), 1f) { DashStyle = DashStyle.Dot })
                {
                    g.DrawLine(crossPen, hp.X, padT, hp.X, h - padB);
                    g.DrawLine(crossPen, padL, hp.Y, padL + plotW, hp.Y);
                }

                var d = _heatmapHoverData.Value;
                string tip = $"x = {Math.Round(d.X, 3)}\nt = {Math.Round(d.T, 3)}s\nu = {Math.Round(d.U, 4)}";
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

        private void PnlHeatmapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentGrid == null) return;
            int w = pnlHeatmapCanvas.Width;
            int h = pnlHeatmapCanvas.Height;
            int padL = 48, padB = 30, padT = 20, padR = 75;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            if (e.X >= padL && e.X <= padL + plotW && e.Y >= padT && e.Y <= padT + plotH)
            {
                float relX = (e.X - padL) / plotW;
                float relY = 1f - ((e.Y - padT) / plotH); // 0 at bottom, 1 at top

                int nx = _currentGrid.XGrid.Length;
                int nt = _currentGrid.TGrid.Length;

                int xi = Math.Max(0, Math.Min(nx - 1, (int)(relX * (nx - 1))));
                int ti = Math.Max(0, Math.Min(nt - 1, (int)(relY * (nt - 1))));

                decimal xVal = _currentGrid.XGrid[xi];
                decimal tVal = _currentGrid.TGrid[ti];
                decimal uVal = _currentGrid.U[xi, ti];

                _heatmapHoverPoint = new PointF(e.X, e.Y);
                _heatmapHoverData = (xVal, tVal, uVal);
            }
            else
            {
                _heatmapHoverPoint = null;
                _heatmapHoverData = null;
            }
            pnlHeatmapCanvas.Invalidate();
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

        private void UpdateAnimSpeed()
        {
            // Speed adjusted in AdvanceAnimation
        }

        private void UpdateAnimationDisplay()
        {
            if (_currentGrid == null) return;
            int curStep = trkTimeStep.Value;
            decimal curT = _currentGrid.TGrid[curStep];
            lblCurrentTime.Text = $"t = {curT:F3}s ({curStep}/{_currentGrid.TGrid.Length - 1})";
            pnlAnimCanvas.Invalidate();
        }

        private void PnlAnimCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int w = pnlAnimCanvas.Width;
            int h = pnlAnimCanvas.Height;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentGrid == null)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("PDE çözüldükten sonra canlı dalga/ısı animasyonu burada oynatılır.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            int curStep = Math.Min(trkTimeStep.Value, _currentGrid.TGrid.Length - 1);
            int nx = _currentGrid.XGrid.Length;
            decimal L = _currentGrid.XGrid[nx - 1];

            int padL = 45, padB = 30, padT = 30, padR = 25;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            decimal spanU = _maxU - _minU;
            if (spanU == 0m) spanU = 1m;
            decimal yMin = _minU - spanU * 0.1m;
            decimal yMax = _maxU + spanU * 0.1m;
            decimal ySpan = yMax - yMin;

            float ToScreenX(decimal x) => padL + (float)(x / L * (decimal)plotW);
            float ToScreenY(decimal u) => padT + (float)((yMax - u) / ySpan * (decimal)plotH);

            // Grid & Ticks
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

            // Build polygon for area fill under the curve
            List<PointF> curvePts = new List<PointF>(nx);
            for (int i = 0; i < nx; i++)
            {
                decimal x = _currentGrid.XGrid[i];
                decimal u = _currentGrid.U[i, curStep];
                curvePts.Add(new PointF(ToScreenX(x), ToScreenY(u)));
            }

            // Area fill
            List<PointF> polyPts = new List<PointF>(curvePts);
            polyPts.Add(new PointF(padL + plotW, ToScreenY(0m)));
            polyPts.Add(new PointF(padL, ToScreenY(0m)));

            using (Brush fillBrush = new SolidBrush(Color.FromArgb(40, 56, 189, 248)))
            {
                g.FillPolygon(fillBrush, polyPts.ToArray());
            }

            // Curve line
            using (Pen linePen = new Pen(Color.FromArgb(56, 189, 248), 2.5f))
            {
                linePen.LineJoin = LineJoin.Round;
                for (int i = 0; i < curvePts.Count - 1; i++)
                {
                    g.DrawLine(linePen, curvePts[i], curvePts[i + 1]);
                }
            }

            // Title
            decimal curT = _currentGrid.TGrid[curStep];
            using (Font titleF = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            using (Brush txtB = new SolidBrush(Color.FromArgb(52, 211, 153)))
            {
                g.DrawString($"u(x) Mekansal Dalga Profili ── t = {curT:F3} sn (Adım: {curStep})", titleF, txtB, padL + 6, 8);
            }
        }

        private void PopulateSnapshotTable(DifferentialEquations.PdeSolutionGrid grid, int nx, int nt)
        {
            dgvSnapshots.Columns.Clear();
            dgvSnapshots.Columns.Add("x", "Mekan (x)");
            dgvSnapshots.Columns["x"].Width = 85;

            int[] snapSteps = { 0, nt / 4, nt / 2, (3 * nt) / 4, nt - 1 };
            for (int s = 0; s < snapSteps.Length; s++)
            {
                int stepIdx = snapSteps[s];
                decimal tVal = grid.TGrid[stepIdx];
                dgvSnapshots.Columns.Add("t" + s, $"u(x, t={tVal:F2})");
            }

            dgvSnapshots.Rows.Clear();
            for (int i = 0; i < nx; i++)
            {
                object[] row = new object[1 + snapSteps.Length];
                row[0] = Math.Round(grid.XGrid[i], 3).ToString(CultureInfo.InvariantCulture);
                for (int s = 0; s < snapSteps.Length; s++)
                {
                    decimal u = grid.U[i, snapSteps[s]];
                    row[1 + s] = Math.Round(u, 5).ToString(CultureInfo.InvariantCulture);
                }
                dgvSnapshots.Rows.Add(row);
            }
        }

        private void CopyGridAsTsv()
        {
            if (dgvSnapshots.Rows.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            List<string> headers = new List<string>();
            foreach (DataGridViewColumn col in dgvSnapshots.Columns) headers.Add(col.HeaderText);
            sb.AppendLine(string.Join("\t", headers));

            foreach (DataGridViewRow r in dgvSnapshots.Rows)
            {
                List<string> vals = new List<string>();
                for (int i = 0; i < dgvSnapshots.Columns.Count; i++) vals.Add(r.Cells[i].Value?.ToString() ?? "");
                sb.AppendLine(string.Join("\t", vals));
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Zaman kesitleri tablosu panoya (TSV formatında) kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportHeatmapAsPng()
        {
            if (_heatmapCache == null)
            {
                MessageBox.Show("Önce PDE simülasyonunu çözünüz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Resmi (*.png)|*.png";
                sfd.FileName = "1D_PDE_Isi_Haritasi.png";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bmp = new Bitmap(1200, 800))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        // Render into 1200x800 bitmap
                        g.Clear(Color.FromArgb(11, 15, 28));
                        g.DrawImage(_heatmapCache, new Rectangle(60, 40, 1050, 700));
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Isı haritası yüksek çözünürlükte (1200x800) başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
