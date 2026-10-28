using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class GraphPlotterControl : UserControl
    {
        public enum PointShape
        {
            Circle,
            Square,
            Cross,
            Diamond
        }

        public class PlotPoint
        {
            public float X { get; set; }
            public float Y { get; set; }
            public Color Color { get; set; } = Color.FromArgb(250, 204, 21);
            public int Size { get; set; } = 8;
            public PointShape Shape { get; set; } = PointShape.Circle;
            public string Label { get; set; } = "";

            public override string ToString() => $"({X:F2}, {Y:F2}) [{Shape}]";
        }

        public class PlotCurve
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Equation { get; set; }
            public Function Compiled { get; set; }
            public Color Color { get; set; }
            public float Thickness { get; set; } = 2.4f;
            public DashStyle DashStyle { get; set; } = DashStyle.Solid;
            public bool Visible { get; set; } = true;
            public bool ClampDomain { get; set; } = false;
            public float DomainMinX { get; set; } = -10f;
            public float DomainMaxX { get; set; } = 10f;
            public bool IsVerticalLine { get; set; } = false;
            public float VerticalX { get; set; } = 0f;
            public float VerticalMinY { get; set; } = 0f;
            public float VerticalMaxY { get; set; } = 0f;

            public List<List<PointF>> Segments { get; set; } = new List<List<PointF>>();

            public override string ToString()
            {
                string vis = Visible ? "✓" : "✗";
                if (IsVerticalLine) return $"{vis} [Doğru] x = {VerticalX:F2}";
                string clampStr = ClampDomain ? $" [{DomainMinX:F1}, {DomainMaxX:F1}]" : "";
                return $"{vis} y = {Equation}{clampStr}";
            }
        }

        private readonly List<PlotCurve> _curves = new List<PlotCurve>();
        private readonly List<PlotPoint> _points = new List<PlotPoint>();

        private readonly Color[] _palette = new Color[]
        {
            Color.FromArgb(56, 189, 248),   // Sky Blue
            Color.FromArgb(245, 158, 11),   // Amber
            Color.FromArgb(52, 211, 153),   // Emerald
            Color.FromArgb(244, 63, 94),    // Rose
            Color.FromArgb(168, 85, 247),   // Violet
            Color.FromArgb(236, 72, 153),   // Pink
            Color.FromArgb(249, 115, 22),   // Orange
            Color.FromArgb(20, 184, 166)    // Teal
        };
        private int _colorIndex = 0;

        private float rangeMinX = -10f;
        private float rangeMaxX = 10f;
        private float rangeMinY = -6f;
        private float rangeMaxY = 6f;

        private bool isDragging = false;
        private bool didMoveMouse = false;
        private Point lastMousePos;
        private Point? hoverMousePos = null;
        private Point lastHoverRecorded = new Point(-999, -999);
        private Timer resampleDebounceTimer;

        public GraphPlotterControl()
        {
            InitializeComponent();

            resampleDebounceTimer = new Timer { Interval = 50 };
            resampleDebounceTimer.Tick += (s, e) =>
            {
                resampleDebounceTimer.Stop();
                if (!isDragging)
                {
                    GenerateAllPoints(false, isDraft: false);
                    pnlCanvas.Invalidate();
                }
            };

            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                pnlCanvas,
                new object[] { true });

            SetupEvents();

            // Default preset
            AddFunctionCurve("sin(x)", false, -10f, 10f);
        }

        private void SetupEvents()
        {
            btnPlot.Click += (s, e) => AddCurrentInputCurve();
            txtEquation.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };
            txtMinX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };
            txtMaxX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };

            // Presets
            btnEx1.Click += (s, e) => { txtEquation.Text = "sin(x) * x"; AddCurrentInputCurve(); };
            btnEx2.Click += (s, e) => { txtEquation.Text = "x^3 - 3*x"; AddCurrentInputCurve(); };
            btnEx3.Click += (s, e) => { txtEquation.Text = "exp(-x^2/4)*cos(2*x)"; AddCurrentInputCurve(); };
            btnEx4.Click += (s, e) => { txtEquation.Text = "1 / (x^2 - 1)"; AddCurrentInputCurve(); };

            // Scaling & Zoom toolbar
            btnZoomXIn.Click += (s, e) => ScaleAxisX(0.7f);
            btnZoomXOut.Click += (s, e) => ScaleAxisX(1.4f);
            btnZoomX10.Click += (s, e) => ScaleAxisX(0.1f);
            btnZoomYIn.Click += (s, e) => ScaleAxisY(0.7f);
            btnZoomYOut.Click += (s, e) => ScaleAxisY(1.4f);
            btnAspect1to1.Click += (s, e) => LockAspect1to1();
            btnResetView.Click += (s, e) => ResetView();

            // Canvas Mouse Events
            pnlCanvas.MouseDown += PnlCanvas_MouseDown;
            pnlCanvas.MouseMove += PnlCanvas_MouseMove;
            pnlCanvas.MouseUp += PnlCanvas_MouseUp;
            pnlCanvas.MouseLeave += (s, e) => { hoverMousePos = null; pnlCanvas.Invalidate(); };
            pnlCanvas.MouseWheel += PnlCanvas_MouseWheel;

            // Curve management
            lstCurves.SelectedIndexChanged += LstCurves_SelectedIndexChanged;
            lstCurves.DoubleClick += (s, e) => EditSelectedCurve();
            lstCurves.KeyDown += (s, e) => { if (e.KeyCode == Keys.Delete) BtnCurveDelete_Click(this, EventArgs.Empty); };
            btnCurveColor.Click += BtnCurveColor_Click;
            cmbCurveStyle.SelectedIndexChanged += CmbCurveStyle_SelectedIndexChanged;
            cmbCurveThickness.SelectedIndexChanged += CmbCurveThickness_SelectedIndexChanged;
            btnCurveToggle.Click += BtnCurveToggle_Click;
            btnCurveDelete.Click += BtnCurveDelete_Click;
            btnCurveEdit.Click += (s, e) => EditSelectedCurve();
            btnCurveClearAll.Click += BtnCurveClearAll_Click;

            // Point management
            lstPoints.KeyDown += (s, e) => { if (e.KeyCode == Keys.Delete) BtnDeletePoint_Click(this, EventArgs.Empty); };
            btnAddPoint.Click += BtnAddPoint_Click;
            btnDeletePoint.Click += BtnDeletePoint_Click;
            btnClearPoints.Click += (s, e) => { _points.Clear(); RefreshPointsList(); pnlCanvas.Invalidate(); };
            btnPtColor.Click += (s, e) =>
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    cd.Color = btnPtColor.BackColor;
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        btnPtColor.BackColor = cd.Color;
                    }
                }
            };

            // Tools Tab
            btnCommonTangent.Click += BtnCommonTangent_Click;
            btnNearestPoints.Click += BtnNearestPoints_Click;
            btnPointTangent.Click += BtnPointTangent_Click;

            // Shapes Tab
            btnAddShape.Click += BtnAddShape_Click;
        }

        #region Scaling & Zooming

        private void ScaleAxisX(float factor)
        {
            float midX = (rangeMaxX + rangeMinX) / 2f;
            float halfW = ((rangeMaxX - rangeMinX) * factor) / 2f;
            if (halfW < 1e-4f || halfW > 1e6f) return;

            rangeMinX = midX - halfW;
            rangeMaxX = midX + halfW;
            txtMinX.Text = rangeMinX.ToString("0.##", CultureInfo.InvariantCulture);
            txtMaxX.Text = rangeMaxX.ToString("0.##", CultureInfo.InvariantCulture);

            GenerateAllPoints(false);
            pnlCanvas.Invalidate();
        }

        private void ScaleAxisY(float factor)
        {
            float midY = (rangeMaxY + rangeMinY) / 2f;
            float halfH = ((rangeMaxY - rangeMinY) * factor) / 2f;
            if (halfH < 1e-4f || halfH > 1e6f) return;

            rangeMinY = midY - halfH;
            rangeMaxY = midY + halfH;

            GenerateAllPoints(false);
            pnlCanvas.Invalidate();
        }

        private void LockAspect1to1()
        {
            float rangeW = rangeMaxX - rangeMinX;
            float w = Math.Max(1, pnlCanvas.Width);
            float h = Math.Max(1, pnlCanvas.Height);

            float targetH = rangeW * (h / w);
            float midY = (rangeMaxY + rangeMinY) / 2f;
            rangeMinY = midY - targetH / 2f;
            rangeMaxY = midY + targetH / 2f;

            GenerateAllPoints(false);
            pnlCanvas.Invalidate();
            lblStatus.Text = "Eksen oranları 1:1 olarak eşitlendi.";
        }

        private void ResetView()
        {
            rangeMinX = -10f;
            rangeMaxX = 10f;
            rangeMinY = -6f;
            rangeMaxY = 6f;
            txtMinX.Text = "-10";
            txtMaxX.Text = "10";
            GenerateAllPoints(false);
            pnlCanvas.Invalidate();
        }

        #endregion

        #region Curve Management

        private void AddCurrentInputCurve()
        {
            string eq = txtEquation.Text.Trim();
            if (string.IsNullOrWhiteSpace(eq)) return;

            float minX = ParseFloat(txtMinX.Text, -10f);
            float maxX = ParseFloat(txtMaxX.Text, 10f);
            if (minX >= maxX) { minX = -10f; maxX = 10f; }

            bool clamp = chkClampRange.Checked;
            AddFunctionCurve(eq, clamp, minX, maxX);
        }

        public void AddFunctionCurve(string eq, bool clamp, float minX, float maxX, Color? color = null, DashStyle dash = DashStyle.Solid, float thickness = 2.4f)
        {
            try
            {
                Function fn = NumericalMethods.CompileFunction(eq);
                Color c = color ?? _palette[_colorIndex % _palette.Length];
                _colorIndex++;

                var curve = new PlotCurve
                {
                    Equation = eq,
                    Compiled = fn,
                    Color = c,
                    Thickness = thickness,
                    DashStyle = dash,
                    Visible = true,
                    ClampDomain = clamp,
                    DomainMinX = minX,
                    DomainMaxX = maxX
                };

                _curves.Add(curve);
                RefreshCurvesList();
                RefreshToolCombos();

                GenerateCurveSegments(curve, isDraft: false);
                pnlCanvas.Invalidate();

                lblStatus.Text = $"✓ Eğri eklendi: y = {eq}";
                lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fonksiyon derlenemedi:\n{ex.Message}", "Formül Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "❌ Hata: " + ex.Message;
                lblStatus.ForeColor = ThemeHelper.DangerRed;
            }
        }

        public void AddVerticalLine(float x, float minY, float maxY, Color color, DashStyle dash = DashStyle.Solid, float thickness = 2f)
        {
            var curve = new PlotCurve
            {
                IsVerticalLine = true,
                VerticalX = x,
                VerticalMinY = minY,
                VerticalMaxY = maxY,
                Color = color,
                Thickness = thickness,
                DashStyle = dash,
                Visible = true
            };
            _curves.Add(curve);
            RefreshCurvesList();
            RefreshToolCombos();
            pnlCanvas.Invalidate();
        }

        private void RefreshCurvesList()
        {
            lstCurves.BeginUpdate();
            lstCurves.Items.Clear();
            foreach (var c in _curves)
            {
                lstCurves.Items.Add(c);
            }
            lstCurves.EndUpdate();
            if (_curves.Count > 0 && lstCurves.SelectedIndex < 0)
                lstCurves.SelectedIndex = _curves.Count - 1;
        }

        private void RefreshToolCombos()
        {
            var validCurves = _curves.Where(c => !c.IsVerticalLine).ToList();

            cmbToolF1.Items.Clear();
            cmbToolF2.Items.Clear();

            foreach (var c in validCurves)
            {
                cmbToolF1.Items.Add(c.Equation);
                cmbToolF2.Items.Add(c.Equation);
            }

            if (cmbToolF1.Items.Count > 0) cmbToolF1.SelectedIndex = 0;
            if (cmbToolF2.Items.Count > 1) cmbToolF2.SelectedIndex = 1;
            else if (cmbToolF2.Items.Count > 0) cmbToolF2.SelectedIndex = 0;
        }

        private void LstCurves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                btnCurveColor.BackColor = c.Color;
                if (c.DashStyle == DashStyle.Solid) cmbCurveStyle.SelectedIndex = 0;
                else if (c.DashStyle == DashStyle.Dash) cmbCurveStyle.SelectedIndex = 1;
                else cmbCurveStyle.SelectedIndex = 2;

                if (c.Thickness <= 1.2f) cmbCurveThickness.SelectedIndex = 0;
                else if (c.Thickness <= 2.2f) cmbCurveThickness.SelectedIndex = 1;
                else if (c.Thickness <= 3.2f) cmbCurveThickness.SelectedIndex = 2;
                else cmbCurveThickness.SelectedIndex = 3;
            }
        }

        private void BtnCurveColor_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    cd.Color = c.Color;
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        c.Color = cd.Color;
                        btnCurveColor.BackColor = cd.Color;
                        pnlCanvas.Invalidate();
                    }
                }
            }
        }

        private void CmbCurveStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                if (cmbCurveStyle.SelectedIndex == 0) c.DashStyle = DashStyle.Solid;
                else if (cmbCurveStyle.SelectedIndex == 1) c.DashStyle = DashStyle.Dash;
                else c.DashStyle = DashStyle.Dot;
                pnlCanvas.Invalidate();
            }
        }

        private void CmbCurveThickness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                c.Thickness = (cmbCurveThickness.SelectedIndex + 1) * 1.2f;
                pnlCanvas.Invalidate();
            }
        }

        private void BtnCurveToggle_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                c.Visible = !c.Visible;
                int idx = lstCurves.SelectedIndex;
                RefreshCurvesList();
                lstCurves.SelectedIndex = idx;
                pnlCanvas.Invalidate();
            }
        }

        private void BtnCurveDelete_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve c)
            {
                _curves.Remove(c);
                RefreshCurvesList();
                RefreshToolCombos();
                pnlCanvas.Invalidate();
                lblStatus.Text = "Eğri listeden silindi.";
            }
        }

        private void EditSelectedCurve()
        {
            if (lstCurves.SelectedItem is PlotCurve c && !c.IsVerticalLine)
            {
                txtEquation.Text = c.Equation;
                chkClampRange.Checked = c.ClampDomain;
                if (c.ClampDomain)
                {
                    txtMinX.Text = c.DomainMinX.ToString(CultureInfo.InvariantCulture);
                    txtMaxX.Text = c.DomainMaxX.ToString(CultureInfo.InvariantCulture);
                }
                txtEquation.Focus();
                lblStatus.Text = $"Eğri düzenleme için yüklendi: y = {c.Equation}";
            }
        }

        private void BtnCurveClearAll_Click(object sender, EventArgs e)
        {
            _curves.Clear();
            RefreshCurvesList();
            RefreshToolCombos();
            pnlCanvas.Invalidate();
        }

        #endregion

        #region Point Management

        private void RefreshPointsList()
        {
            lstPoints.BeginUpdate();
            lstPoints.Items.Clear();
            foreach (var p in _points)
            {
                lstPoints.Items.Add(p);
            }
            lstPoints.EndUpdate();
            if (_points.Count > 0 && lstPoints.SelectedIndex < 0)
                lstPoints.SelectedIndex = _points.Count - 1;
        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            float px = ParseFloat(txtPtX.Text, 0f);
            float py = ParseFloat(txtPtY.Text, 0f);

            PointShape shape = (PointShape)Math.Max(0, cmbPtShape.SelectedIndex);
            _points.Add(new PlotPoint
            {
                X = px,
                Y = py,
                Color = btnPtColor.BackColor,
                Shape = shape,
                Size = 8
            });

            RefreshPointsList();
            pnlCanvas.Invalidate();
            lblStatus.Text = $"Nokta eklendi: ({px:F2}, {py:F2})";
        }

        private void BtnDeletePoint_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedItem is PlotPoint p)
            {
                _points.Remove(p);
                RefreshPointsList();
                pnlCanvas.Invalidate();
                lblStatus.Text = "Nokta silindi.";
            }
        }

        #endregion

        #region Point Generation & Segment Caching

        private void GenerateAllPoints(bool autoFitY, bool isDraft = false)
        {
            foreach (var c in _curves)
            {
                if (c.Visible && !c.IsVerticalLine && c.Compiled != null)
                {
                    GenerateCurveSegments(c, isDraft);
                }
            }
        }

        private void GenerateCurveSegments(PlotCurve curve, bool isDraft = false)
        {
            curve.Segments.Clear();
            if (curve.Compiled == null) return;

            float drawMinX = curve.ClampDomain ? Math.Max(rangeMinX, curve.DomainMinX) : rangeMinX;
            float drawMaxX = curve.ClampDomain ? Math.Min(rangeMaxX, curve.DomainMaxX) : rangeMaxX;
            if (drawMinX >= drawMaxX) return;

            int sampleCount = isDraft
                ? Math.Min(200, Math.Max(100, pnlCanvas.Width / 3))
                : Math.Max(500, pnlCanvas.Width * 2);

            float step = (drawMaxX - drawMinX) / sampleCount;
            if (step <= 0) return;

            List<PointF> currentSegment = new List<PointF>();
            float? prevY = null;

            bool prevRad = Function.UseRadians;
            Function.UseRadians = true;
            try
            {
                for (int i = 0; i <= sampleCount; i++)
                {
                    float x = drawMinX + i * step;
                    try
                    {
                        decimal val = NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x);
                        float y = (float)val;

                        if (float.IsNaN(y) || float.IsInfinity(y) || Math.Abs(y) > 1e6f)
                        {
                            if (currentSegment.Count > 1) curve.Segments.Add(currentSegment);
                            currentSegment = new List<PointF>();
                            prevY = null;
                            continue;
                        }

                        // Asymptote jump filter
                        if (prevY.HasValue && Math.Abs(y - prevY.Value) > (rangeMaxY - rangeMinY) * 2.5f && (y * prevY.Value < 0))
                        {
                            if (currentSegment.Count > 1) curve.Segments.Add(currentSegment);
                            currentSegment = new List<PointF>();
                        }

                        currentSegment.Add(new PointF(x, y));
                        prevY = y;
                    }
                    catch
                    {
                        if (currentSegment.Count > 1) curve.Segments.Add(currentSegment);
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
                curve.Segments.Add(currentSegment);
            }
        }

        #endregion

        #region Mouse Canvas Handlers

        private void PnlCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoom = (e.Delta > 0) ? 0.85f : 1.18f;

            bool shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;
            bool ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (shift)
            {
                ScaleAxisX(zoom);
                return;
            }
            if (ctrl)
            {
                ScaleAxisY(zoom);
                return;
            }

            float rangeW = rangeMaxX - rangeMinX;
            float rangeH = rangeMaxY - rangeMinY;
            if (rangeW < 1e-4f && zoom < 1f) return;
            if (rangeW > 1e6f && zoom > 1f) return;

            float midX = (rangeMaxX + rangeMinX) / 2f;
            float midY = (rangeMaxY + rangeMinY) / 2f;

            rangeMinX = midX - (rangeW * zoom) / 2f;
            rangeMaxX = midX + (rangeW * zoom) / 2f;
            rangeMinY = midY - (rangeH * zoom) / 2f;
            rangeMaxY = midY + (rangeH * zoom) / 2f;

            txtMinX.Text = rangeMinX.ToString("0.##", CultureInfo.InvariantCulture);
            txtMaxX.Text = rangeMaxX.ToString("0.##", CultureInfo.InvariantCulture);

            GenerateAllPoints(false, isDraft: true);
            resampleDebounceTimer.Stop();
            resampleDebounceTimer.Start();
            pnlCanvas.Invalidate();
        }

        private void PnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            pnlCanvas.Focus();

            if (e.Button == MouseButtons.Right)
            {
                // Requirement 1: Right-click deletes individual point!
                float w = Math.Max(1, pnlCanvas.Width);
                float h = Math.Max(1, pnlCanvas.Height);
                float MapX(float x) => (x - rangeMinX) / (rangeMaxX - rangeMinX) * w;
                float MapY(float y) => h - ((y - rangeMinY) / (rangeMaxY - rangeMinY) * h);

                PlotPoint toRemove = null;
                for (int i = _points.Count - 1; i >= 0; i--)
                {
                    var p = _points[i];
                    float sx = MapX(p.X);
                    float sy = MapY(p.Y);
                    if (Math.Abs(sx - e.X) < 14 && Math.Abs(sy - e.Y) < 14)
                    {
                        toRemove = p;
                        break;
                    }
                }

                if (toRemove != null)
                {
                    _points.Remove(toRemove);
                    RefreshPointsList();
                    pnlCanvas.Invalidate();
                    lblStatus.Text = $"Nokta silindi: ({toRemove.X:F2}, {toRemove.Y:F2})";
                }
                return;
            }

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
            if (isDragging)
            {
                hoverMousePos = e.Location;
                int dx = e.X - lastMousePos.X;
                int dy = e.Y - lastMousePos.Y;

                if (Math.Abs(dx) > 2 || Math.Abs(dy) > 2) didMoveMouse = true;

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
                GenerateAllPoints(false, isDraft: true);
                resampleDebounceTimer.Stop();
                resampleDebounceTimer.Start();
                pnlCanvas.Invalidate();
            }
            else
            {
                if (Math.Abs(e.X - lastHoverRecorded.X) >= 2 || Math.Abs(e.Y - lastHoverRecorded.Y) >= 2)
                {
                    lastHoverRecorded = e.Location;
                    hoverMousePos = e.Location;
                    pnlCanvas.Invalidate();
                }
            }
        }

        private void PnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!didMoveMouse)
                {
                    // Snap point onto active curve or clicked coordinate
                    float w = Math.Max(1, pnlCanvas.Width);
                    float h = Math.Max(1, pnlCanvas.Height);
                    float xPct = e.X / w;
                    float logX = rangeMinX + xPct * (rangeMaxX - rangeMinX);

                    var activeCurve = _curves.FirstOrDefault(c => c.Visible && !c.IsVerticalLine && c.Compiled != null);
                    float logY;

                    if (activeCurve != null)
                    {
                        bool prevRad = Function.UseRadians;
                        Function.UseRadians = true;
                        try
                        {
                            decimal fx = NumericalMethods.EvaluateCompiled(activeCurve.Compiled, "x", (decimal)logX);
                            logY = (float)fx;
                        }
                        catch
                        {
                            float yPct = (h - e.Y) / h;
                            logY = rangeMinY + yPct * (rangeMaxY - rangeMinY);
                        }
                        finally
                        {
                            Function.UseRadians = prevRad;
                        }
                    }
                    else
                    {
                        float yPct = (h - e.Y) / h;
                        logY = rangeMinY + yPct * (rangeMaxY - rangeMinY);
                    }

                    _points.Add(new PlotPoint
                    {
                        X = logX,
                        Y = logY,
                        Color = btnPtColor.BackColor,
                        Shape = (PointShape)Math.Max(0, cmbPtShape.SelectedIndex),
                        Size = 8
                    });

                    RefreshPointsList();
                    lblStatus.Text = $"Nokta eklendi: ({logX:F2}, {logY:F2}) - Silmek için sağ tıklayın.";
                }

                isDragging = false;
                resampleDebounceTimer.Stop();
                GenerateAllPoints(false, isDraft: false);
                Cursor = Cursors.Cross;
                pnlCanvas.Invalidate();
            }
        }

        #endregion

        #region Rendering (Paint)

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float w = Math.Max(1, pnlCanvas.Width);
            float h = Math.Max(1, pnlCanvas.Height);

            float MapX(float x) => (x - rangeMinX) / (rangeMaxX - rangeMinX) * w;
            float MapY(float y) => h - ((y - rangeMinY) / (rangeMaxY - rangeMinY) * h);

            // 1. Grid
            DrawGrid(g, w, h, MapX, MapY);

            // 2. Axes
            using (Pen axisPen = new Pen(Color.FromArgb(148, 163, 184), 1.8f))
            {
                float y0 = MapY(0);
                float x0 = MapX(0);

                if (y0 >= 0 && y0 <= h) g.DrawLine(axisPen, 0, y0, w, y0);
                if (x0 >= 0 && x0 <= w) g.DrawLine(axisPen, x0, 0, x0, h);
            }

            // 3. Draw Curves
            foreach (var curve in _curves)
            {
                if (!curve.Visible) continue;

                if (curve.IsVerticalLine)
                {
                    float sx = MapX(curve.VerticalX);
                    float sy1 = MapY(curve.VerticalMinY);
                    float sy2 = MapY(curve.VerticalMaxY);
                    using (Pen p = new Pen(curve.Color, curve.Thickness) { DashStyle = curve.DashStyle })
                    {
                        g.DrawLine(p, sx, sy1, sx, sy2);
                    }
                    continue;
                }

                using (Pen glowPen = new Pen(Color.FromArgb(40, curve.Color), curve.Thickness + 4f))
                using (Pen linePen = new Pen(curve.Color, curve.Thickness) { DashStyle = curve.DashStyle })
                {
                    foreach (var seg in curve.Segments)
                    {
                        if (seg.Count < 2) continue;
                        List<PointF> screenPts = new List<PointF>(seg.Count);
                        foreach (var p in seg)
                        {
                            float sx = MapX(p.X);
                            float sy = MapY(p.Y);
                            if (sy < -5000f) sy = -5000f;
                            if (sy > h + 5000f) sy = h + 5000f;
                            screenPts.Add(new PointF(sx, sy));
                        }

                        try
                        {
                            PointF[] arr = screenPts.ToArray();
                            g.DrawLines(glowPen, arr);
                            g.DrawLines(linePen, arr);
                        }
                        catch { }
                    }
                }
            }

            // 4. Draw Marked Points
            using (Font markFont = new Font("Consolas", 8.5F, FontStyle.Bold))
            using (Brush bgTag = new SolidBrush(Color.FromArgb(200, 15, 23, 42)))
            {
                foreach (var p in _points)
                {
                    float sx = MapX(p.X);
                    float sy = MapY(p.Y);
                    if (sx < -50 || sx > w + 50 || sy < -50 || sy > h + 50) continue;

                    DrawCustomPoint(g, sx, sy, p);

                    string coord = $"({p.X:F2}, {p.Y:F2})";
                    SizeF sz = g.MeasureString(coord, markFont);
                    g.FillRectangle(bgTag, sx + 8, sy - 20, sz.Width + 4, sz.Height + 2);
                    using (Brush textBr = new SolidBrush(p.Color))
                    {
                        g.DrawString(coord, markFont, textBr, sx + 10, sy - 19);
                    }
                }
            }

            // 5. Live Hover Coordinate HUD
            if (hoverMousePos.HasValue && !isDragging)
            {
                float mx = hoverMousePos.Value.X;
                float logX = rangeMinX + (mx / w) * (rangeMaxX - rangeMinX);

                var activeCurve = _curves.FirstOrDefault(c => c.Visible && !c.IsVerticalLine && c.Compiled != null);
                if (activeCurve != null)
                {
                    bool prevRad = Function.UseRadians;
                    Function.UseRadians = true;
                    try
                    {
                        decimal fxDec = NumericalMethods.EvaluateCompiled(activeCurve.Compiled, "x", (decimal)logX);
                        float logY = (float)fxDec;
                        float sy = MapY(logY);

                        using (Pen dashPen = new Pen(Color.FromArgb(90, 248, 250, 252), 1f) { DashStyle = DashStyle.Dash })
                        {
                            g.DrawLine(dashPen, mx, 0, mx, h);
                            if (sy >= 0 && sy <= h)
                            {
                                g.DrawLine(dashPen, 0, sy, w, sy);
                                using (Brush dotBrush = new SolidBrush(activeCurve.Color))
                                {
                                    g.FillEllipse(dotBrush, mx - 4, sy - 4, 8, 8);
                                }
                            }
                        }

                        string coordInfo = $"x = {logX:F3}  |  f(x) = {logY:F4}  ({activeCurve.Equation})";
                        using (Font hudFont = new Font("Consolas", 9F, FontStyle.Bold))
                        using (Brush hudBg = new SolidBrush(Color.FromArgb(220, 15, 23, 42)))
                        using (Brush hudFg = new SolidBrush(Color.FromArgb(186, 230, 253)))
                        {
                            SizeF sz = g.MeasureString(coordInfo, hudFont);
                            RectangleF rect = new RectangleF(16, 12, sz.Width + 12, sz.Height + 8);
                            g.FillRectangle(hudBg, rect);
                            g.DrawString(coordInfo, hudFont, hudFg, rect.X + 6, rect.Y + 4);
                        }
                    }
                    catch { }
                    finally { Function.UseRadians = prevRad; }
                }
            }
        }

        private void DrawCustomPoint(Graphics g, float sx, float sy, PlotPoint p)
        {
            int sz = p.Size;
            float half = sz / 2f;
            using (Brush b = new SolidBrush(p.Color))
            using (Pen pen = new Pen(Color.White, 1.5f))
            {
                switch (p.Shape)
                {
                    case PointShape.Square:
                        g.FillRectangle(b, sx - half, sy - half, sz, sz);
                        g.DrawRectangle(pen, sx - half, sy - half, sz, sz);
                        break;

                    case PointShape.Diamond:
                        PointF[] diamond = new PointF[]
                        {
                            new PointF(sx, sy - half - 1),
                            new PointF(sx + half + 1, sy),
                            new PointF(sx, sy + half + 1),
                            new PointF(sx - half - 1, sy)
                        };
                        g.FillPolygon(b, diamond);
                        g.DrawPolygon(pen, diamond);
                        break;

                    case PointShape.Cross:
                        using (Pen thickPen = new Pen(p.Color, 2.5f))
                        {
                            g.DrawLine(thickPen, sx - half, sy - half, sx + half, sy + half);
                            g.DrawLine(thickPen, sx - half, sy + half, sx + half, sy - half);
                        }
                        break;

                    case PointShape.Circle:
                    default:
                        g.FillEllipse(b, sx - half, sy - half, sz, sz);
                        g.DrawEllipse(pen, sx - half - 1, sy - half - 1, sz + 2, sz + 2);
                        break;
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
            GenerateAllPoints(false, isDraft: false);
            pnlCanvas.Invalidate();
        }

        private float ParseFloat(string s, float def)
        {
            string norm = (s ?? "").Trim().Replace(',', '.');
            if (float.TryParse(norm, NumberStyles.Any, CultureInfo.InvariantCulture, out float v)) return v;
            return def;
        }

        #endregion

        #region Advanced Tools (Common Tangent, Closest Points, Tangent from Point)

        private void BtnCommonTangent_Click(object sender, EventArgs e)
        {
            string eq1 = cmbToolF1.SelectedItem?.ToString();
            string eq2 = cmbToolF2.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(eq1) || string.IsNullOrWhiteSpace(eq2))
            {
                MessageBox.Show("Lütfen araç için listeden iki fonksiyon seçin.", "Seçim Eksik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Function f1 = NumericalMethods.CompileFunction(eq1);
                Function f2 = NumericalMethods.CompileFunction(eq2);

                decimal EvalF1(decimal x) => NumericalMethods.EvaluateCompiled(f1, "x", x);
                decimal EvalF2(decimal x) => NumericalMethods.EvaluateCompiled(f2, "x", x);
                decimal DerivF1(decimal x) => EvalDeriv(f1, x);
                decimal DerivF2(decimal x) => EvalDeriv(f2, x);

                // Scan grid over [rangeMinX, rangeMaxX] for (x1, x2) minimizing common tangent loss
                decimal bestLoss = decimal.MaxValue;
                decimal bestX1 = 0m, bestX2 = 0m;

                int gridN = 40;
                decimal xStart = (decimal)rangeMinX;
                decimal xEnd = (decimal)rangeMaxX;
                decimal dx = (xEnd - xStart) / gridN;

                for (int i = 0; i <= gridN; i++)
                {
                    decimal x1 = xStart + i * dx;
                    decimal m1 = DerivF1(x1);
                    decimal y1 = EvalF1(x1);

                    for (int j = 0; j <= gridN; j++)
                    {
                        decimal x2 = xStart + j * dx;
                        if (DecimalMath.Abs(x1 - x2) < 0.05m) continue;

                        decimal m2 = DerivF2(x2);
                        decimal y2 = EvalF2(x2);
                        decimal chordSlope = (y2 - y1) / (x2 - x1);

                        decimal loss = (m1 - m2) * (m1 - m2) + (chordSlope - m1) * (chordSlope - m1);
                        if (loss < bestLoss)
                        {
                            bestLoss = loss;
                            bestX1 = x1;
                            bestX2 = x2;
                        }
                    }
                }

                if (bestLoss < 1.0m)
                {
                    decimal m = DerivF1(bestX1);
                    decimal c = EvalF1(bestX1) - m * bestX1;

                    string tanEq = $"{m:F4} * x + {c:F4}";
                    AddFunctionCurve(tanEq, false, rangeMinX, rangeMaxX, Color.FromArgb(250, 204, 21), DashStyle.Dash, 2.5f);

                    // Add contact points
                    _points.Add(new PlotPoint { X = (float)bestX1, Y = (float)EvalF1(bestX1), Color = Color.FromArgb(56, 189, 248), Shape = PointShape.Diamond });
                    _points.Add(new PlotPoint { X = (float)bestX2, Y = (float)EvalF2(bestX2), Color = Color.FromArgb(244, 63, 94), Shape = PointShape.Diamond });
                    RefreshPointsList();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("✓ ORTAK TEĞET BULUNDU:");
                    sb.AppendLine($"Doğru: y = {m:F4}·x + {c:F4}");
                    sb.AppendLine($"1. Değme Noktası: P1({bestX1:F3}, {EvalF1(bestX1):F3})");
                    sb.AppendLine($"2. Değme Noktası: P2({bestX2:F3}, {EvalF2(bestX2):F3})");
                    txtToolLog.Text = sb.ToString();
                }
                else
                {
                    txtToolLog.Text = "Belirtilen aralıkta ortak teğet bulunamadı veya eğriler uygun geometriye sahip değil.";
                }
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        private void BtnNearestPoints_Click(object sender, EventArgs e)
        {
            string eq1 = cmbToolF1.SelectedItem?.ToString();
            string eq2 = cmbToolF2.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(eq1) || string.IsNullOrWhiteSpace(eq2))
            {
                MessageBox.Show("Lütfen iki fonksiyon seçin.", "Seçim Eksik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Check if they intersect: f1 - f2 = 0
                string diffEq = $"({eq1}) - ({eq2})";
                var roots = NumericalMethods.FindAllRoots(diffEq, (decimal)rangeMinX, (decimal)rangeMaxX, "x");

                if (roots.Count > 0)
                {
                    StringBuilder sbInt = new StringBuilder();
                    sbInt.AppendLine("⚠️ FONKSİYONLAR KESİŞİYOR!");
                    sbInt.AppendLine($"Kesişme Kökleri ({roots.Count} adet):");
                    foreach (var r in roots)
                    {
                        sbInt.AppendLine($" • x = {r.Root:F5} (f(x)={r.FVal:G4})");
                    }
                    sbInt.AppendLine("Kesiştikleri için aralarındaki minimum mesafe: 0'dır.");
                    txtToolLog.Text = sbInt.ToString();
                    return;
                }

                // If no intersection, find points minimizing Euclidean distance
                Function f1 = NumericalMethods.CompileFunction(eq1);
                Function f2 = NumericalMethods.CompileFunction(eq2);

                decimal minDistSq = decimal.MaxValue;
                decimal bestX1 = 0m, bestX2 = 0m;
                decimal bestY1 = 0m, bestY2 = 0m;

                int gridN = 60;
                decimal xStart = (decimal)rangeMinX;
                decimal xEnd = (decimal)rangeMaxX;
                decimal dx = (xEnd - xStart) / gridN;

                for (int i = 0; i <= gridN; i++)
                {
                    decimal x1 = xStart + i * dx;
                    decimal y1 = 0m;
                    try { y1 = NumericalMethods.EvaluateCompiled(f1, "x", x1); } catch { continue; }

                    for (int j = 0; j <= gridN; j++)
                    {
                        decimal x2 = xStart + j * dx;
                        decimal y2 = 0m;
                        try { y2 = NumericalMethods.EvaluateCompiled(f2, "x", x2); } catch { continue; }

                        decimal dSq = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                        if (dSq < minDistSq)
                        {
                            minDistSq = dSq;
                            bestX1 = x1;
                            bestY1 = y1;
                            bestX2 = x2;
                            bestY2 = y2;
                        }
                    }
                }

                decimal dist = (decimal)Math.Sqrt((double)minDistSq);

                // Add points
                _points.Add(new PlotPoint { X = (float)bestX1, Y = (float)bestY1, Color = Color.FromArgb(236, 72, 153), Shape = PointShape.Diamond });
                _points.Add(new PlotPoint { X = (float)bestX2, Y = (float)bestY2, Color = Color.FromArgb(236, 72, 153), Shape = PointShape.Diamond });
                RefreshPointsList();

                // Connecting line equation
                decimal m = 0m, c = 0m;
                string lineEq;
                if (DecimalMath.Abs(bestX2 - bestX1) > 1e-5m)
                {
                    m = (bestY2 - bestY1) / (bestX2 - bestX1);
                    c = bestY1 - m * bestX1;
                    lineEq = $"{m:F4} * x + {c:F4}";
                    AddFunctionCurve(lineEq, false, rangeMinX, rangeMaxX, Color.FromArgb(236, 72, 153), DashStyle.Dot, 2f);
                }
                else
                {
                    AddVerticalLine((float)bestX1, (float)Math.Min(bestY1, bestY2), (float)Math.Max(bestY1, bestY2), Color.FromArgb(236, 72, 153), DashStyle.Dot);
                    lineEq = $"x = {bestX1:F4} (Düşey Doğru)";
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("✓ EN YAKIN NOKTALAR HESAPLANDI:");
                sb.AppendLine($"Kesişme: Yok (Denklem kökü bulunamadı)");
                sb.AppendLine($"P1({bestX1:F3}, {bestY1:F3}) ve P2({bestX2:F3}, {bestY2:F3})");
                sb.AppendLine($"Minimum Mesafe: {dist:F5} birim");
                sb.AppendLine($"Geçen Doğru: y = {m:F4}·x + {c:F4}");
                txtToolLog.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        private void BtnPointTangent_Click(object sender, EventArgs e)
        {
            string eq = cmbToolF1.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(eq))
            {
                MessageBox.Show("Lütfen 1. Fonksiyonu seçin.", "Fonksiyon Eksik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float px = ParseFloat(txtTanPointX.Text, 0f);
            float py = ParseFloat(txtTanPointY.Text, 3f);

            try
            {
                Function f = NumericalMethods.CompileFunction(eq);
                decimal x0 = (decimal)px;
                decimal y0 = (decimal)py;

                // Tangent condition: (f(xt) - y0) - f'(xt) * (xt - x0) = 0
                // Search for xt in [rangeMinX, rangeMaxX]
                decimal bestDiff = decimal.MaxValue;
                decimal bestXt = 0m;
                int nScan = 100;
                decimal xStart = (decimal)rangeMinX;
                decimal xEnd = (decimal)rangeMaxX;
                decimal dx = (xEnd - xStart) / nScan;

                for (int i = 0; i <= nScan; i++)
                {
                    decimal xt = xStart + i * dx;
                    try
                    {
                        decimal fx = NumericalMethods.EvaluateCompiled(f, "x", xt);
                        decimal dfx = EvalDeriv(f, xt);
                        decimal cond = (fx - y0) - dfx * (xt - x0);
                        if (DecimalMath.Abs(cond) < bestDiff)
                        {
                            bestDiff = DecimalMath.Abs(cond);
                            bestXt = xt;
                        }
                    }
                    catch { }
                }

                if (bestDiff < 0.5m)
                {
                    decimal m = EvalDeriv(f, bestXt);
                    decimal c = y0 - m * x0;

                    string tanEq = $"{m:F4} * x + {c:F4}";
                    AddFunctionCurve(tanEq, false, rangeMinX, rangeMaxX, Color.FromArgb(249, 115, 22), DashStyle.Dash, 2.2f);

                    // Add point and contact point
                    _points.Add(new PlotPoint { X = px, Y = py, Color = Color.FromArgb(249, 115, 22), Shape = PointShape.Cross });
                    _points.Add(new PlotPoint { X = (float)bestXt, Y = (float)NumericalMethods.EvaluateCompiled(f, "x", bestXt), Color = Color.FromArgb(52, 211, 153), Shape = PointShape.Diamond });
                    RefreshPointsList();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("✓ NOKTADAN ÇİZİLEN TEĞET DOĞRUSU:");
                    sb.AppendLine($"Verilen Nokta: P₀({px}, {py})");
                    sb.AppendLine($"Değme Noktası: P_teğet({bestXt:F3}, {NumericalMethods.EvaluateCompiled(f, "x", bestXt):F3})");
                    sb.AppendLine($"Teğet Eğim (m): {m:F4}");
                    sb.AppendLine($"Teğet Denklemi: y = {m:F4}·x + {c:F4}");
                    txtToolLog.Text = sb.ToString();
                }
                else
                {
                    txtToolLog.Text = "Verilen noktadan fonksiyona teğet kökü bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        #endregion

        #region Parametric Shapes

        private void BtnAddShape_Click(object sender, EventArgs e)
        {
            float cx = ParseFloat(txtShapeCenterX.Text, 0f);
            float cy = ParseFloat(txtShapeCenterY.Text, 0f);
            float d1 = Math.Max(0.1f, ParseFloat(txtShapeDim1.Text, 4f));
            float d2 = Math.Max(0.1f, ParseFloat(txtShapeDim2.Text, 2f));
            float angleDeg = ParseFloat(txtShapeAngle.Text, 0f);
            float angleRad = (float)(angleDeg * Math.PI / 180.0);
            PointF center = new PointF(cx, cy);

            PointF RotatePt(PointF pt)
            {
                if (Math.Abs(angleDeg) < 1e-3f) return pt;
                float cos = (float)Math.Cos(angleRad);
                float sin = (float)Math.Sin(angleRad);
                float dx = pt.X - center.X;
                float dy = pt.Y - center.Y;
                return new PointF(center.X + dx * cos - dy * sin, center.Y + dx * sin + dy * cos);
            }

            int shapeIdx = cmbShapeType.SelectedIndex;
            Color shapeColor = _palette[_colorIndex % _palette.Length];
            _colorIndex++;

            switch (shapeIdx)
            {
                case 0: // Daire (Çember)
                    string upCircle = $"{cy:F3} + sqrt({d1 * d1:F4} - (x - {cx:F3})^2)";
                    string lowCircle = $"{cy:F3} - sqrt({d1 * d1:F4} - (x - {cx:F3})^2)";
                    AddFunctionCurve(upCircle, true, cx - d1, cx + d1, shapeColor, DashStyle.Solid, 2.5f);
                    AddFunctionCurve(lowCircle, true, cx - d1, cx + d1, shapeColor, DashStyle.Solid, 2.5f);
                    lblStatus.Text = $"Parametrik Daire eklendi [Merkez: ({cx}, {cy}), Yarıçap: {d1}]";
                    break;

                case 1: // Kare (Kenar d1)
                    float hs = d1 / 2f;
                    PointF[] sq = new PointF[]
                    {
                        RotatePt(new PointF(cx - hs, cy - hs)),
                        RotatePt(new PointF(cx + hs, cy - hs)),
                        RotatePt(new PointF(cx + hs, cy + hs)),
                        RotatePt(new PointF(cx - hs, cy + hs))
                    };
                    for (int i = 0; i < 4; i++) AddLineSegmentCurve(sq[i], sq[(i + 1) % 4], shapeColor);
                    lblStatus.Text = $"Kare eklendi [Merkez: ({cx}, {cy}), Kenar: {d1}, Dönüş: {angleDeg}°]";
                    break;

                case 2: // Dikdörtgen (Genişlik d1, Yükseklik d2)
                    float hw = d1 / 2f;
                    float hh = d2 / 2f;
                    PointF[] rect = new PointF[]
                    {
                        RotatePt(new PointF(cx - hw, cy - hh)),
                        RotatePt(new PointF(cx + hw, cy - hh)),
                        RotatePt(new PointF(cx + hw, cy + hh)),
                        RotatePt(new PointF(cx - hw, cy + hh))
                    };
                    for (int i = 0; i < 4; i++) AddLineSegmentCurve(rect[i], rect[(i + 1) % 4], shapeColor);
                    lblStatus.Text = $"Dikdörtgen eklendi [Merkez: ({cx}, {cy}), {d1} × {d2}, Dönüş: {angleDeg}°]";
                    break;

                case 3: // Eşkenar Üçgen (Yarıçap d1)
                    PointF v1 = RotatePt(new PointF(cx, cy + d1));
                    PointF v2 = RotatePt(new PointF(cx - d1 * 0.866025f, cy - d1 * 0.5f));
                    PointF v3 = RotatePt(new PointF(cx + d1 * 0.866025f, cy - d1 * 0.5f));
                    AddLineSegmentCurve(v1, v2, shapeColor);
                    AddLineSegmentCurve(v2, v3, shapeColor);
                    AddLineSegmentCurve(v3, v1, shapeColor);
                    lblStatus.Text = $"Eşkenar Üçgen eklendi [Merkez: ({cx}, {cy}), Dönüş: {angleDeg}°]";
                    break;

                case 4: // Elips (a=d1, b=d2, Dönüş Açılı)
                    int nEll = 36;
                    PointF[] ell = new PointF[nEll];
                    for (int i = 0; i < nEll; i++)
                    {
                        double t = i * 2.0 * Math.PI / nEll;
                        ell[i] = RotatePt(new PointF((float)(cx + d1 * Math.Cos(t)), (float)(cy + d2 * Math.Sin(t))));
                    }
                    for (int i = 0; i < nEll; i++) AddLineSegmentCurve(ell[i], ell[(i + 1) % nEll], shapeColor);
                    lblStatus.Text = $"Elips eklendi [Merkez: ({cx}, {cy}), a={d1}, b={d2}, Dönüş: {angleDeg}°]";
                    break;

                case 5: // Düzgün Altıgen (Yarıçap d1)
                    PointF[] hex = new PointF[6];
                    for (int i = 0; i < 6; i++)
                    {
                        double ang = i * Math.PI / 3.0;
                        hex[i] = RotatePt(new PointF((float)(cx + d1 * Math.Cos(ang)), (float)(cy + d1 * Math.Sin(ang))));
                    }
                    for (int i = 0; i < 6; i++) AddLineSegmentCurve(hex[i], hex[(i + 1) % 6], shapeColor);
                    lblStatus.Text = $"Düzgün Altıgen eklendi [Merkez: ({cx}, {cy}), R={d1}, Dönüş: {angleDeg}°]";
                    break;
            }
        }

        private void AddLineSegmentCurve(PointF p1, PointF p2, Color color)
        {
            if (Math.Abs(p1.X - p2.X) < 1e-4f)
            {
                AddVerticalLine(p1.X, Math.Min(p1.Y, p2.Y), Math.Max(p1.Y, p2.Y), color, DashStyle.Solid, 2.5f);
                return;
            }

            float minX = Math.Min(p1.X, p2.X);
            float maxX = Math.Max(p1.X, p2.X);
            float m = (p2.Y - p1.Y) / (p2.X - p1.X);
            float c = p1.Y - m * p1.X;

            string eq = $"{m:F4} * x + {c:F4}";
            AddFunctionCurve(eq, true, minX, maxX, color, DashStyle.Solid, 2.5f);
        }

        private static decimal EvalDeriv(Function fn, decimal x)
        {
            decimal h = 0.0005m;
            try
            {
                decimal fPlus = NumericalMethods.EvaluateCompiled(fn, "x", x + h);
                decimal fMinus = NumericalMethods.EvaluateCompiled(fn, "x", x - h);
                return (fPlus - fMinus) / (2m * h);
            }
            catch
            {
                return 0m;
            }
        }

        #endregion
    }
}
