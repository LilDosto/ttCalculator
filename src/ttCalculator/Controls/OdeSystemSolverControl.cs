using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class OdeSystemSolverControl : UserControl
    {
        private List<TextBox> txtEqList = new List<TextBox>();
        private List<TextBox> txtInitList = new List<TextBox>();
        private List<DifferentialEquations.OdeSystemStepResult> _currentResults = new List<DifferentialEquations.OdeSystemStepResult>();
        private List<string> _currentVarNames = new List<string>();

        private PointF? _phaseHoverPoint = null;
        private (decimal X, decimal Y)? _phaseHoverData = null;

        private static readonly Color[] ComponentColors = new Color[]
        {
            Color.FromArgb(56, 189, 248),  // Sky Blue (y1)
            Color.FromArgb(52, 211, 153),  // Emerald (y2)
            Color.FromArgb(251, 191, 36),  // Amber (y3)
            Color.FromArgb(168, 85, 247),  // Purple (y4)
            Color.FromArgb(244, 63, 94),   // Rose (y5)
            Color.FromArgb(6, 182, 212)    // Cyan (y6)
        };

        public OdeSystemSolverControl()
        {
            InitializeComponent();

            numEqCount.ValueChanged += (s, e) => RebuildEquationRows((int)numEqCount.Value);
            btnSolve.Click += BtnSolve_Click;
            btnCopySystemTsv.Click += (s, e) => CopyGridAsTsv();

            btnExportPhasePng.Click += (s, e) => ExportCanvasAsPng(pnlPhaseCanvas, "Faz_Portresi.png", RenderPhasePortrait);
            btnExportTimePng.Click += (s, e) => ExportCanvasAsPng(pnlTimeCanvas, "Zaman_Serisi.png", RenderTimeSeries);

            btnEx1.Click += (s, e) => LoadPresetHarmonic();
            btnEx2.Click += (s, e) => LoadPresetLotkaVolterra();
            btnEx3.Click += (s, e) => LoadPresetLorenz();
            btnEx4.Click += (s, e) => LoadPresetPendulum();

            cmbAxisX.SelectedIndexChanged += (s, e) => pnlPhaseCanvas.Invalidate();
            cmbAxisY.SelectedIndexChanged += (s, e) => pnlPhaseCanvas.Invalidate();

            pnlPhaseCanvas.Paint += (s, e) => RenderPhasePortrait(e.Graphics, pnlPhaseCanvas.Width, pnlPhaseCanvas.Height);
            pnlPhaseCanvas.MouseMove += PnlPhaseCanvas_MouseMove;
            pnlPhaseCanvas.MouseLeave += (s, e) => { _phaseHoverPoint = null; _phaseHoverData = null; pnlPhaseCanvas.Invalidate(); };
            pnlPhaseCanvas.Resize += (s, e) => pnlPhaseCanvas.Invalidate();

            pnlTimeCanvas.Paint += (s, e) => RenderTimeSeries(e.Graphics, pnlTimeCanvas.Width, pnlTimeCanvas.Height);
            pnlTimeCanvas.Resize += (s, e) => pnlTimeCanvas.Invalidate();

            // Default setup
            RebuildEquationRows(2);
            LoadPresetHarmonic();
        }

        private void RebuildEquationRows(int count)
        {
            pnlEquations.SuspendLayout();
            pnlEquations.Controls.Clear();
            txtEqList.Clear();
            txtInitList.Clear();

            int rowH = 28;
            for (int i = 0; i < count; i++)
            {
                int idx = i + 1;
                int yPos = i * (rowH + 4);

                Label lblEq = new Label
                {
                    Text = $"dy{idx}/dt =",
                    Location = new Point(8, yPos + 4),
                    Size = new Size(65, 20),
                    ForeColor = Color.FromArgb(56, 189, 248),
                    Font = new Font("Consolas", 9.5F, FontStyle.Bold)
                };

                TextBox txtEq = new TextBox
                {
                    Location = new Point(78, yPos + 1),
                    Size = new Size(240, 24),
                    BackColor = Color.FromArgb(24, 33, 52),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Consolas", 10F),
                    Text = idx == 1 ? "y2" : "-y1"
                };

                Label lblInit = new Label
                {
                    Text = $"y{idx}(t₀) =",
                    Location = new Point(330, yPos + 4),
                    Size = new Size(65, 20),
                    ForeColor = ThemeHelper.LightText,
                    Font = new Font("Consolas", 9F)
                };

                TextBox txtInit = new TextBox
                {
                    Location = new Point(400, yPos + 1),
                    Size = new Size(70, 24),
                    BackColor = Color.FromArgb(24, 33, 52),
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = new Font("Consolas", 10F),
                    Text = idx == 1 ? "1" : "0",
                    TextAlign = HorizontalAlignment.Center
                };

                pnlEquations.Controls.Add(lblEq);
                pnlEquations.Controls.Add(txtEq);
                pnlEquations.Controls.Add(lblInit);
                pnlEquations.Controls.Add(txtInit);

                txtEqList.Add(txtEq);
                txtInitList.Add(txtInit);
            }
            pnlEquations.ResumeLayout();

            UpdateAxisSelectors(count);
        }

        private void UpdateAxisSelectors(int count)
        {
            cmbAxisX.Items.Clear();
            cmbAxisY.Items.Clear();
            for (int i = 1; i <= count; i++)
            {
                cmbAxisX.Items.Add("y" + i);
                cmbAxisY.Items.Add("y" + i);
            }
            if (cmbAxisX.Items.Count > 0) cmbAxisX.SelectedIndex = 0;
            if (cmbAxisY.Items.Count > 1) cmbAxisY.SelectedIndex = 1;
            else if (cmbAxisY.Items.Count > 0) cmbAxisY.SelectedIndex = 0;
        }

        private void LoadPresetHarmonic()
        {
            numEqCount.Value = 2;
            txtEqList[0].Text = "y2";
            txtInitList[0].Text = "1";
            txtEqList[1].Text = "-y1";
            txtInitList[1].Text = "0";
            txtT0.Text = "0";
            txtTEnd.Text = "15";
            numSteps.Value = 300;
            cmbMethod.SelectedIndex = 0;
            BtnSolve_Click(this, EventArgs.Empty);
        }

        private void LoadPresetLotkaVolterra()
        {
            numEqCount.Value = 2;
            txtEqList[0].Text = "1.5*y1 - y1*y2";
            txtInitList[0].Text = "10";
            txtEqList[1].Text = "-3*y2 + y1*y2";
            txtInitList[1].Text = "5";
            txtT0.Text = "0";
            txtTEnd.Text = "15";
            numSteps.Value = 600;
            cmbMethod.SelectedIndex = 0;
            BtnSolve_Click(this, EventArgs.Empty);
        }

        private void LoadPresetLorenz()
        {
            numEqCount.Value = 3;
            txtEqList[0].Text = "10*(y2 - y1)";
            txtInitList[0].Text = "1";
            txtEqList[1].Text = "y1*(28 - y3) - y2";
            txtInitList[1].Text = "1";
            txtEqList[2].Text = "y1*y2 - 2.6667*y3";
            txtInitList[2].Text = "1";
            txtT0.Text = "0";
            txtTEnd.Text = "25";
            numSteps.Value = 1500;
            cmbMethod.SelectedIndex = 0;
            BtnSolve_Click(this, EventArgs.Empty);
        }

        private void LoadPresetPendulum()
        {
            numEqCount.Value = 2;
            txtEqList[0].Text = "y2";
            txtInitList[0].Text = "3";
            txtEqList[1].Text = "-sin(y1) - 0.2*y2";
            txtInitList[1].Text = "0";
            txtT0.Text = "0";
            txtTEnd.Text = "25";
            numSteps.Value = 500;
            cmbMethod.SelectedIndex = 0;
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
            btnSolve.Text = "⏳ Çözülüyor...";
            Cursor = Cursors.WaitCursor;

            try
            {
                int count = (int)numEqCount.Value;
                List<string> equations = new List<string>();
                List<string> varNames = new List<string>();
                Dictionary<string, decimal> inits = new Dictionary<string, decimal>();

                for (int i = 0; i < count; i++)
                {
                    string v = "y" + (i + 1);
                    varNames.Add(v);
                    string eq = txtEqList[i].Text.Trim();
                    if (string.IsNullOrWhiteSpace(eq))
                    {
                        txtOutput.Text = $"Lütfen d{v}/dt denklemini giriniz.";
                        return;
                    }
                    equations.Add(eq);
                    decimal iv = ParseDec(txtInitList[i].Text, $"{v}(t₀)");
                    inits[v] = iv;
                }

                decimal t0 = ParseDec(txtT0.Text, "t₀");
                decimal tEnd = ParseDec(txtTEnd.Text, "t_son");
                int steps = (int)numSteps.Value;
                string method = cmbMethod.SelectedItem?.ToString() ?? "RK4";

                var sw = System.Diagnostics.Stopwatch.StartNew();
                var results = await Task.Run(() =>
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        return DifferentialEquations.SolveOdeSystem(equations, varNames, inits, t0, tEnd, steps, method);
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
                    _currentVarNames = varNames;

                    decimal stepH = (tEnd - t0) / steps;
                    var last = results[results.Count - 1];

                    // Format result vector
                    List<string> lastVals = new List<string>();
                    for (int i = 0; i < count; i++)
                    {
                        string vn = varNames[i];
                        decimal val = last.YValues.ContainsKey(vn) ? last.YValues[vn] : 0m;
                        lastVals.Add($"{vn} = {Math.Round(val, 6).ToString(CultureInfo.InvariantCulture)}");
                    }
                    lblVectorValue.Text = "Y = [\n  " + string.Join("\n  ", lastVals) + "\n]";

                    lblSysMetricSteps.Text = $"• Adım Sayısı (N) : {steps}";
                    lblSysMetricH.Text = $"• Adım Boyutu (h) : {stepH.ToString("0.######", CultureInfo.InvariantCulture)}";
                    lblSysMetricTime.Text = $"• Çözüm Süresi    : {sw.ElapsedMilliseconds} ms";
                    lblSysMetricMethod.Text = $"• Yöntem          : {method}";

                    // Populate DataGridView
                    dgvSystemSteps.Columns.Clear();
                    dgvSystemSteps.Columns.Add("step", "Adım");
                    dgvSystemSteps.Columns.Add("t", "Zaman (t)");
                    dgvSystemSteps.Columns["step"].Width = 55;
                    dgvSystemSteps.Columns["t"].Width = 85;

                    for (int i = 0; i < count; i++)
                    {
                        dgvSystemSteps.Columns.Add(varNames[i], varNames[i]);
                    }

                    dgvSystemSteps.Rows.Clear();
                    for (int step = 0; step < results.Count; step++)
                    {
                        var r = results[step];
                        object[] row = new object[2 + count];
                        row[0] = step;
                        row[1] = Math.Round(r.T, 4).ToString(CultureInfo.InvariantCulture);
                        for (int i = 0; i < count; i++)
                        {
                            string vn = varNames[i];
                            decimal v = r.YValues.ContainsKey(vn) ? r.YValues[vn] : 0m;
                            row[2 + i] = Math.Round(v, 6).ToString(CultureInfo.InvariantCulture);
                        }
                        dgvSystemSteps.Rows.Add(row);
                    }

                    // Report text
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("=== DİFERANSİYEL SİSTEM NÜMERİK ÇÖZÜM RAPORU ===");
                    sb.AppendLine($"  Denklem Sayısı : {count}");
                    sb.AppendLine($"  Zaman Aralığı  : [{t0}, {tEnd}]  (N = {steps}, h = {stepH.ToString("0.######", CultureInfo.InvariantCulture)})");
                    sb.AppendLine($"  Yöntem         : {method}");
                    for (int i = 0; i < count; i++)
                    {
                        sb.AppendLine($"  d{varNames[i]}/dt = {equations[i]}  [{varNames[i]}(t₀) = {inits[varNames[i]]}]");
                    }
                    sb.AppendLine(new string('=', 65));
                    sb.AppendLine("SON ÇÖZÜM VEKTÖRÜ Y(t_son):");
                    foreach (var lv in lastVals) sb.AppendLine("  • " + lv);

                    txtOutput.Text = sb.ToString();

                    pnlPhaseCanvas.Invalidate();
                    pnlTimeCanvas.Invalidate();
                }
            }
            catch (Exception ex)
            {
                txtOutput.Text = "❌ Hata: " + ex.Message;
                lblVectorValue.Text = "Hata Oluştu: " + ex.Message;
            }
            finally
            {
                btnSolve.Enabled = true;
                btnSolve.Text = origBtnText;
                Cursor = Cursors.Default;
            }
        }

        private void RenderPhasePortrait(Graphics g, int w, int h)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentResults == null || _currentResults.Count < 2 || _currentVarNames.Count < 1)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("Sistem çözüldüğünde 2B Faz Uzayı Portresi (Phase Portrait) burada çizilir.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            string xVar = cmbAxisX.SelectedItem?.ToString() ?? "y1";
            string yVar = cmbAxisY.SelectedItem?.ToString() ?? (_currentVarNames.Count > 1 ? "y2" : "y1");

            decimal minX = decimal.MaxValue, maxX = decimal.MinValue;
            decimal minY = decimal.MaxValue, maxY = decimal.MinValue;

            foreach (var r in _currentResults)
            {
                decimal vx = r.YValues.ContainsKey(xVar) ? r.YValues[xVar] : 0m;
                decimal vy = r.YValues.ContainsKey(yVar) ? r.YValues[yVar] : 0m;
                if (vx < minX) minX = vx;
                if (vx > maxX) maxX = vx;
                if (vy < minY) minY = vy;
                if (vy > maxY) maxY = vy;
            }

            decimal spanX = maxX - minX;
            if (spanX == 0m) spanX = 1m;
            decimal spanY = maxY - minY;
            if (spanY == 0m) spanY = 1m;
            minX -= spanX * 0.08m;
            maxX += spanX * 0.08m;
            minY -= spanY * 0.08m;
            maxY += spanY * 0.08m;

            int padL = 48, padB = 30, padT = 20, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            float ToScreenX(decimal x) => padL + (float)((x - minX) / (maxX - minX) * (decimal)plotW);
            float ToScreenY(decimal y) => padT + (float)((maxY - y) / (maxY - minY) * (decimal)plotH);

            // Draw grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(24, 33, 52), 1f) { DashStyle = DashStyle.Dash })
            using (Font lblFont = new Font("Consolas", 7.5F))
            using (Brush lblBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                for (int i = 0; i <= 5; i++)
                {
                    decimal gx = minX + (spanX * i / 5m);
                    float sx = ToScreenX(gx);
                    g.DrawLine(gridPen, sx, padT, sx, h - padB);
                    g.DrawString(Math.Round(gx, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, sx - 10, h - padB + 4);

                    decimal gy = minY + (spanY * i / 5m);
                    float sy = ToScreenY(gy);
                    g.DrawLine(gridPen, padL, sy, w - padR, sy);
                    g.DrawString(Math.Round(gy, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, 4, sy - 6);
                }
            }

            // Axis box
            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            // Draw Phase Trajectory with Directional Arrows
            List<PointF> pts = new List<PointF>(_currentResults.Count);
            foreach (var r in _currentResults)
            {
                decimal vx = r.YValues.ContainsKey(xVar) ? r.YValues[xVar] : 0m;
                decimal vy = r.YValues.ContainsKey(yVar) ? r.YValues[yVar] : 0m;
                pts.Add(new PointF(ToScreenX(vx), ToScreenY(vy)));
            }

            using (Pen trajPen = new Pen(Color.FromArgb(56, 189, 248), 2f))
            {
                trajPen.LineJoin = LineJoin.Round;
                for (int i = 0; i < pts.Count - 1; i++)
                {
                    g.DrawLine(trajPen, pts[i], pts[i + 1]);
                }
            }

            // Draw motion direction arrows every N steps
            int arrowInterval = Math.Max(15, pts.Count / 12);
            using (Pen arrowPen = new Pen(Color.FromArgb(52, 211, 153), 1.8f))
            {
                arrowPen.CustomEndCap = new AdjustableArrowCap(3.5f, 5f);
                for (int i = arrowInterval; i < pts.Count - 2; i += arrowInterval)
                {
                    PointF p1 = pts[i];
                    PointF p2 = pts[i + 1];
                    float dx = p2.X - p1.X;
                    float dy = p2.Y - p1.Y;
                    if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                    {
                        g.DrawLine(arrowPen, p1.X, p1.Y, p1.X + dx * 1.5f, p1.Y + dy * 1.5f);
                    }
                }
            }

            // Highlight start point (Amber) & end point (Emerald)
            PointF pStart = pts[0];
            using (Brush bStart = new SolidBrush(Color.FromArgb(245, 158, 11)))
            {
                g.FillEllipse(bStart, pStart.X - 4f, pStart.Y - 4f, 8f, 8f);
            }

            PointF pEnd = pts[pts.Count - 1];
            using (Brush bEnd = new SolidBrush(Color.FromArgb(52, 211, 153)))
            {
                g.FillEllipse(bEnd, pEnd.X - 5f, pEnd.Y - 5f, 10f, 10f);
            }

            // Hover tooltip
            if (_phaseHoverPoint.HasValue && _phaseHoverData.HasValue)
            {
                PointF hp = _phaseHoverPoint.Value;
                using (Pen crossPen = new Pen(Color.FromArgb(52, 211, 153), 1f) { DashStyle = DashStyle.Dot })
                {
                    g.DrawLine(crossPen, hp.X, padT, hp.X, h - padB);
                    g.DrawLine(crossPen, padL, hp.Y, w - padR, hp.Y);
                }

                string tip = $"{xVar} = {Math.Round(_phaseHoverData.Value.X, 4)}\n{yVar} = {Math.Round(_phaseHoverData.Value.Y, 4)}";
                using (Font tipFont = new Font("Consolas", 8.5F, FontStyle.Bold))
                {
                    SizeF sz = g.MeasureString(tip, tipFont);
                    float tx = hp.X + 10;
                    float ty = hp.Y - sz.Height - 6;
                    if (tx + sz.Width + 8 > w) tx = hp.X - sz.Width - 14;
                    if (ty < padT) ty = hp.Y + 12;

                    RectangleF tr = new RectangleF(tx, ty, sz.Width + 8, sz.Height + 6);
                    using (Brush bg = new SolidBrush(Color.FromArgb(240, 24, 33, 52)))
                    using (Pen bp = new Pen(Color.FromArgb(52, 211, 153), 1.2f))
                    using (Brush txtB = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(bg, tr);
                        g.DrawRectangle(bp, tr.X, tr.Y, tr.Width, tr.Height);
                        g.DrawString(tip, tipFont, txtB, tx + 4, ty + 3);
                    }
                }
            }

            // Axis labels
            using (Font titleF = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
            using (Brush txtB = new SolidBrush(Color.FromArgb(186, 230, 253)))
            {
                g.DrawString($"Yörünge: {yVar} vs {xVar}", titleF, txtB, padL + 8, padT + 6);
            }
        }

        private void PnlPhaseCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentResults == null || _currentResults.Count < 2 || _currentVarNames.Count < 1) return;

            string xVar = cmbAxisX.SelectedItem?.ToString() ?? "y1";
            string yVar = cmbAxisY.SelectedItem?.ToString() ?? (_currentVarNames.Count > 1 ? "y2" : "y1");

            int w = pnlPhaseCanvas.Width;
            int h = pnlPhaseCanvas.Height;
            int padL = 48, padB = 30, padT = 20, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            decimal minX = decimal.MaxValue, maxX = decimal.MinValue;
            decimal minY = decimal.MaxValue, maxY = decimal.MinValue;

            foreach (var r in _currentResults)
            {
                decimal vx = r.YValues.ContainsKey(xVar) ? r.YValues[xVar] : 0m;
                decimal vy = r.YValues.ContainsKey(yVar) ? r.YValues[yVar] : 0m;
                if (vx < minX) minX = vx;
                if (vx > maxX) maxX = vx;
                if (vy < minY) minY = vy;
                if (vy > maxY) maxY = vy;
            }

            decimal spanX = maxX - minX;
            if (spanX == 0m) spanX = 1m;
            decimal spanY = maxY - minY;
            if (spanY == 0m) spanY = 1m;
            minX -= spanX * 0.08m;
            maxX += spanX * 0.08m;
            minY -= spanY * 0.08m;
            maxY += spanY * 0.08m;

            float ToScreenX(decimal x) => padL + (float)((x - minX) / (maxX - minX) * (decimal)plotW);
            float ToScreenY(decimal y) => padT + (float)((maxY - y) / (maxY - minY) * (decimal)plotH);

            float closestDist = float.MaxValue;
            PointF closestScreen = PointF.Empty;
            (decimal X, decimal Y)? closestData = null;

            foreach (var r in _currentResults)
            {
                decimal vx = r.YValues.ContainsKey(xVar) ? r.YValues[xVar] : 0m;
                decimal vy = r.YValues.ContainsKey(yVar) ? r.YValues[yVar] : 0m;
                PointF sp = new PointF(ToScreenX(vx), ToScreenY(vy));
                float dx = sp.X - e.X;
                float dy = sp.Y - e.Y;
                float d = (float)Math.Sqrt(dx * dx + dy * dy);
                if (d < closestDist)
                {
                    closestDist = d;
                    closestScreen = sp;
                    closestData = (vx, vy);
                }
            }

            if (closestData != null && closestDist < 50f)
            {
                _phaseHoverPoint = closestScreen;
                _phaseHoverData = closestData;
            }
            else
            {
                _phaseHoverPoint = null;
                _phaseHoverData = null;
            }
            pnlPhaseCanvas.Invalidate();
        }

        private void RenderTimeSeries(Graphics g, int w, int h)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.FromArgb(11, 15, 28));

            if (_currentResults == null || _currentResults.Count < 2 || _currentVarNames.Count < 1)
            {
                using (Font f = new Font("Segoe UI", 9.5F))
                using (Brush b = new SolidBrush(Color.FromArgb(148, 163, 184)))
                {
                    g.DrawString("Sistem çözüldüğünde zaman serisi (t vs y₁, y₂, ...) grafiği burada çizilir.", f, b, new RectangleF(20, 20, w - 40, 60));
                }
                return;
            }

            decimal minT = _currentResults[0].T;
            decimal maxT = _currentResults[_currentResults.Count - 1].T;
            decimal minY = decimal.MaxValue, maxY = decimal.MinValue;

            foreach (var r in _currentResults)
            {
                foreach (var vn in _currentVarNames)
                {
                    if (r.YValues.ContainsKey(vn))
                    {
                        decimal val = r.YValues[vn];
                        if (val < minY) minY = val;
                        if (val > maxY) maxY = val;
                    }
                }
            }

            decimal spanT = maxT - minT;
            if (spanT == 0m) spanT = 1m;
            decimal spanY = maxY - minY;
            if (spanY == 0m) spanY = 1m;
            minY -= spanY * 0.08m;
            maxY += spanY * 0.08m;

            int padL = 48, padB = 30, padT = 32, padR = 20;
            float plotW = w - padL - padR;
            float plotH = h - padT - padB;
            if (plotW < 10 || plotH < 10) return;

            float ToScreenX(decimal t) => padL + (float)((t - minT) / (spanT) * (decimal)plotW);
            float ToScreenY(decimal y) => padT + (float)((maxY - y) / (maxY - minY) * (decimal)plotH);

            // Grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(24, 33, 52), 1f) { DashStyle = DashStyle.Dash })
            using (Font lblFont = new Font("Consolas", 7.5F))
            using (Brush lblBrush = new SolidBrush(Color.FromArgb(100, 116, 139)))
            {
                for (int i = 0; i <= 5; i++)
                {
                    decimal gt = minT + (spanT * i / 5m);
                    float sx = ToScreenX(gt);
                    g.DrawLine(gridPen, sx, padT, sx, h - padB);
                    g.DrawString(Math.Round(gt, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, sx - 10, h - padB + 4);

                    decimal gy = minY + (spanY * i / 5m);
                    float sy = ToScreenY(gy);
                    g.DrawLine(gridPen, padL, sy, w - padR, sy);
                    g.DrawString(Math.Round(gy, 2).ToString(CultureInfo.InvariantCulture), lblFont, lblBrush, 4, sy - 6);
                }
            }

            // Box
            using (Pen boxPen = new Pen(Color.FromArgb(51, 65, 85), 1.5f))
            {
                g.DrawRectangle(boxPen, padL, padT, plotW, plotH);
            }

            // Draw each component curve
            for (int vIdx = 0; vIdx < _currentVarNames.Count; vIdx++)
            {
                string vn = _currentVarNames[vIdx];
                Color c = ComponentColors[vIdx % ComponentColors.Length];

                List<PointF> linePts = new List<PointF>(_currentResults.Count);
                foreach (var r in _currentResults)
                {
                    decimal val = r.YValues.ContainsKey(vn) ? r.YValues[vn] : 0m;
                    linePts.Add(new PointF(ToScreenX(r.T), ToScreenY(val)));
                }

                using (Pen curvePen = new Pen(c, 2.2f))
                {
                    curvePen.LineJoin = LineJoin.Round;
                    for (int i = 0; i < linePts.Count - 1; i++)
                    {
                        g.DrawLine(curvePen, linePts[i], linePts[i + 1]);
                    }
                }
            }

            // Top Legend
            float legX = padL + 8;
            using (Font legFont = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
            {
                for (int vIdx = 0; vIdx < _currentVarNames.Count; vIdx++)
                {
                    string vn = _currentVarNames[vIdx];
                    Color c = ComponentColors[vIdx % ComponentColors.Length];
                    using (Brush cb = new SolidBrush(c))
                    {
                        g.FillEllipse(cb, legX, 10, 8, 8);
                        g.DrawString(vn + "(t)", legFont, cb, legX + 12, 7);
                    }
                    legX += 60;
                }
            }
        }

        private void CopyGridAsTsv()
        {
            if (dgvSystemSteps.Rows.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            List<string> headers = new List<string>();
            foreach (DataGridViewColumn col in dgvSystemSteps.Columns) headers.Add(col.HeaderText);
            sb.AppendLine(string.Join("\t", headers));

            foreach (DataGridViewRow r in dgvSystemSteps.Rows)
            {
                List<string> vals = new List<string>();
                for (int i = 0; i < dgvSystemSteps.Columns.Count; i++) vals.Add(r.Cells[i].Value?.ToString() ?? "");
                sb.AppendLine(string.Join("\t", vals));
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("Veri tablosu panoya (TSV formatında) kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportCanvasAsPng(Panel canvas, string defName, Action<Graphics, int, int> renderFn)
        {
            if (_currentResults == null || _currentResults.Count < 2)
            {
                MessageBox.Show("Önce diferansiyel sistemini çözünüz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG Resmi (*.png)|*.png";
                sfd.FileName = defName;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (Bitmap bmp = new Bitmap(1200, 800))
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        renderFn(g, 1200, 800);
                        bmp.Save(sfd.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Grafik yüksek çözünürlükte (1200x800) başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
