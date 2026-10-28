using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MathApp.Core;

namespace MathApp.Controls
{
    public partial class GraphPlotterControl : UserControl
    {
        public enum PointShape
        {
            Circle,
            Square,
            Diamond,
            Cross
        }

        public class PlotPoint
        {
            public float X { get; set; }
            public float Y { get; set; }
            public Color Color { get; set; } = Color.FromArgb(250, 204, 21);
            public int Size { get; set; } = 8;
            public PointShape Shape { get; set; } = PointShape.Circle;
            public string Label { get; set; } = "";
            public string PointType { get; set; } = "Serbest";
            public bool IsGlider { get; set; } = false;
            public string BoundEquation { get; set; } = null;

            public override string ToString() => $"({X:F2}, {Y:F2}) [{PointType}] [{Shape}]";
        }

        public class SnapResult
        {
            public float ScreenX { get; set; }
            public float ScreenY { get; set; }
            public float WorldX { get; set; }
            public float WorldY { get; set; }
            public string SnapType { get; set; } = "Serbest";
            public Color SnapColor { get; set; } = Color.FromArgb(250, 204, 21);
            public string Description { get; set; } = "";
            public bool IsSnapped { get; set; } = false;
            public string BoundEquation { get; set; } = null;
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
            public bool IsPolar { get; set; } = false;

            public List<List<PointF>> Segments { get; set; } = new List<List<PointF>>();

            public override string ToString()
            {
                string vis = Visible ? "✓" : "✗";
                if (IsVerticalLine) return $"{vis} [Doğru] x = {VerticalX:F2}";
                if (IsPolar) return $"{vis} [Polar] r = {Equation}";
                string clampStr = ClampDomain ? $" [{DomainMinX:F1}, {DomainMaxX:F1}]" : "";
                return $"{vis} y = {Equation}{clampStr}";
            }
        }

        public class CriticalPoint
        {
            public float X { get; set; }
            public float Y { get; set; }
            public string TypeName { get; set; }
            public Color Color { get; set; }
            public string Details { get; set; }
        }

        private readonly List<PlotCurve> _curves = new List<PlotCurve>();
        private readonly List<PlotPoint> _points = new List<PlotPoint>();
        private readonly List<CriticalPoint> _criticalPoints = new List<CriticalPoint>();
        private readonly List<float> _asymptotePoles = new List<float>();

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
        private Point mouseDownPos;
        private Point lastMousePos;
        private Point? hoverMousePos = null;
        private Point lastHoverRecorded = new Point(-999, -999);
        private Timer resampleDebounceTimer;

        // Dynamic Parameters (Sliders)
        private float _paramA = 1.0f;
        private float _paramB = 1.0f;
        private float _paramC = 0.0f;
        private float _paramK = 1.0f;
        private Timer _animTimer;
        private float _animPhase = 0f;

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
                    DetectCriticalPoints();
                    pnlCanvas.Invalidate();
                }
            };

            _animTimer = new Timer { Interval = 33 }; // ~30-60 FPS
            _animTimer.Tick += AnimTimer_Tick;

            typeof(Panel).InvokeMember(
                "DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                pnlCanvas,
                new object[] { true });

            SetupEvents();
            ThemeHelper.StyleModernTabControl(tabManager);

            // Default preset
            AddFunctionCurve("sin(x)", false, -10f, 10f);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GenerateAllPoints(false, isDraft: false);
            DetectCriticalPoints();
            pnlCanvas.Invalidate();
        }

        private void SetupEvents()
        {
            btnPlot.Click += (s, e) => AddCurrentInputCurve();
            txtEquation.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };
            txtMinX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };
            txtMaxX.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; AddCurrentInputCurve(); } };

            // Presets
            btnEx1.Click += (s, e) => { txtEquation.Text = "sin(x) * x"; chkPolar.Checked = false; AddCurrentInputCurve(); };
            btnEx2.Click += (s, e) => { txtEquation.Text = "x^3 - 3*x"; chkPolar.Checked = false; AddCurrentInputCurve(); };
            btnEx3.Click += (s, e) => { txtEquation.Text = "exp(-x^2/4)*cos(2*x)"; chkPolar.Checked = false; AddCurrentInputCurve(); };
            btnEx4.Click += (s, e) => { txtEquation.Text = "1 / (x^2 - 1)"; chkPolar.Checked = false; AddCurrentInputCurve(); };

            // Scaling & Zoom toolbar
            btnZoomXIn.Click += (s, e) => ScaleAxisX(0.7f);
            btnZoomXOut.Click += (s, e) => ScaleAxisX(1.4f);
            btnZoomX10.Click += (s, e) => ScaleAxisX(0.1f);
            btnZoomYIn.Click += (s, e) => ScaleAxisY(0.7f);
            btnZoomYOut.Click += (s, e) => ScaleAxisY(1.4f);
            btnAspect1to1.Click += (s, e) => LockAspect1to1();
            btnResetView.Click += (s, e) => ResetView();

            chkCriticalPoints.CheckedChanged += (s, e) => { DetectCriticalPoints(); pnlCanvas.Invalidate(); };
            chkInspectMode.CheckedChanged += (s, e) => pnlCanvas.Invalidate();
            chkShowAsymptotes.CheckedChanged += (s, e) => pnlCanvas.Invalidate();

            // Canvas Mouse & Paint Events
            pnlCanvas.Paint += pnlCanvas_Paint;
            pnlCanvas.MouseDown += PnlCanvas_MouseDown;
            pnlCanvas.MouseMove += PnlCanvas_MouseMove;
            pnlCanvas.MouseUp += PnlCanvas_MouseUp;
            pnlCanvas.MouseLeave += (s, e) => { hoverMousePos = null; pnlCanvas.Invalidate(); };
            pnlCanvas.MouseWheel += PnlCanvas_MouseWheel;
            pnlCanvas.Resize += (s, e) =>
            {
                if (pnlCanvas.Width > 10 && pnlCanvas.Height > 10)
                {
                    GenerateAllPoints(false, isDraft: false);
                    pnlCanvas.Invalidate();
                }
            };

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

            // Parameters Tab Events (Sliders)
            tbParamA.Scroll += (s, e) => { _paramA = tbParamA.Value / 10f; lblParamA.Text = $"a = {_paramA:F2}"; UpdateParametersAndReplot(); };
            tbParamB.Scroll += (s, e) => { _paramB = tbParamB.Value / 10f; lblParamB.Text = $"b = {_paramB:F2}"; UpdateParametersAndReplot(); };
            tbParamC.Scroll += (s, e) => { _paramC = tbParamC.Value / 10f; lblParamC.Text = $"c = {_paramC:F2}"; UpdateParametersAndReplot(); };
            tbParamK.Scroll += (s, e) => { _paramK = tbParamK.Value / 10f; lblParamK.Text = $"k = {_paramK:F2}"; UpdateParametersAndReplot(); };

            btnAnimate.Click += BtnAnimate_Click;
            btnResetParams.Click += (s, e) =>
            {
                tbParamA.Value = 10; _paramA = 1.0f; lblParamA.Text = "a = 1.00";
                tbParamB.Value = 10; _paramB = 1.0f; lblParamB.Text = "b = 1.00";
                tbParamC.Value = 0;  _paramC = 0.0f; lblParamC.Text = "c = 0.00";
                tbParamK.Value = 10; _paramK = 1.0f; lblParamK.Text = "k = 1.00";
                UpdateParametersAndReplot();
            };

            // Tools Tab
            btnCommonTangent.Click += BtnCommonTangent_Click;
            btnNearestPoints.Click += BtnNearestPoints_Click;
            btnPointTangent.Click += BtnPointTangent_Click;

            // Export Events
            btnExportImage.Click += BtnExportImage_Click;
            btnExportCsv.Click += BtnExportCsv_Click;
            btnExportLatex.Click += BtnExportLatex_Click;

            // Shapes Tab
            btnAddShape.Click += BtnAddShape_Click;

            // Populate combo options
            cmbCurveStyle.Items.AddRange(new object[] { "Düz (Solid)", "Kesikli (Dash)", "Noktalı (Dot)" });
            cmbCurveStyle.SelectedIndex = 0;

            cmbCurveThickness.Items.AddRange(new object[] { "1 px", "2 px", "3 px", "4 px", "6 px" });
            cmbCurveThickness.SelectedIndex = 1;

            cmbPtShape.Items.AddRange(new object[] { "Daire", "Kare", "Baklava", "Artı (+)" });
            cmbPtShape.SelectedIndex = 0;

            cmbShapeType.Items.AddRange(new object[] { "Çember / Daire", "Kare / Dikdörtgen", "Eşkenar Üçgen", "Çokgen (5-Gen)", "Çokgen (6-Gen)" });
            cmbShapeType.SelectedIndex = 0;
        }

        #region Parameters & Sliders

        private void SetCurveParameters(PlotCurve curve)
        {
            if (curve?.Compiled == null) return;
            curve.Compiled.SetVariable("a", (decimal)_paramA);
            curve.Compiled.SetVariable("b", (decimal)_paramB);
            curve.Compiled.SetVariable("c", (decimal)_paramC);
            curve.Compiled.SetVariable("k", (decimal)_paramK);
        }

        private void UpdateParametersAndReplot()
        {
            foreach (var c in _curves)
            {
                if (c.Visible && !c.IsVerticalLine)
                {
                    SetCurveParameters(c);
                    GenerateCurveSegments(c, isDraft: false);
                }
            }
            DetectCriticalPoints();
            pnlCanvas.Invalidate();
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            _animPhase += 0.06f;
            float animatedC = (float)Math.Sin(_animPhase) * 5f;
            _paramC = (float)Math.Round(animatedC, 2);
            tbParamC.Value = Math.Max(-100, Math.Min(100, (int)(_paramC * 10)));
            lblParamC.Text = $"c = {_paramC:F2}";

            UpdateParametersAndReplot();
        }

        private void BtnAnimate_Click(object sender, EventArgs e)
        {
            if (_animTimer.Enabled)
            {
                _animTimer.Stop();
                btnAnimate.Text = "▶ Canlı Dalga Animasyonu";
                btnAnimate.BackColor = ThemeHelper.AccentBlue;
            }
            else
            {
                _animTimer.Start();
                btnAnimate.Text = "⏸ Animasyonu Durdur";
                btnAnimate.BackColor = ThemeHelper.AmberWarning;
            }
        }

        #endregion

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
            DetectCriticalPoints();
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
            DetectCriticalPoints();
            pnlCanvas.Invalidate();
        }

        private void LockAspect1to1()
        {
            float rangeW = rangeMaxX - rangeMinX;
            float w = Math.Max(1, pnlCanvas.Width);
            float h = Math.Max(1, pnlCanvas.Height);

            float targetRangeH = rangeW * (h / w);
            float midY = (rangeMaxY + rangeMinY) / 2f;

            rangeMinY = midY - targetRangeH / 2f;
            rangeMaxY = midY + targetRangeH / 2f;

            GenerateAllPoints(false);
            DetectCriticalPoints();
            pnlCanvas.Invalidate();
            lblStatus.Text = "Eksenler 1:1 orana kilitlendi.";
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
            DetectCriticalPoints();
            pnlCanvas.Invalidate();
            lblStatus.Text = "Görünüm varsayılana sıfırlandı.";
        }

        #endregion

        #region Curve & Function Management

        private void AddCurrentInputCurve()
        {
            string eq = txtEquation.Text.Trim();
            if (string.IsNullOrWhiteSpace(eq)) return;

            float minX = ParseFloat(txtMinX.Text, -10f);
            float maxX = ParseFloat(txtMaxX.Text, 10f);
            if (minX >= maxX) { minX = -10f; maxX = 10f; }

            bool clamp = chkClampRange.Checked;
            bool isPolar = chkPolar.Checked || eq.StartsWith("r=", StringComparison.OrdinalIgnoreCase);

            if (isPolar && eq.StartsWith("r=", StringComparison.OrdinalIgnoreCase))
            {
                eq = eq.Substring(2).Trim();
            }

            AddFunctionCurve(eq, clamp, minX, maxX, isPolar: isPolar);
        }

        public void AddFunctionCurve(string eq, bool clamp, float minX, float maxX, Color? color = null, DashStyle dash = DashStyle.Solid, float thickness = 2.4f, bool isPolar = false)
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
                    DomainMaxX = maxX,
                    IsPolar = isPolar
                };

                SetCurveParameters(curve);
                _curves.Add(curve);
                RefreshCurvesList();
                RefreshToolCombos();

                GenerateCurveSegments(curve, isDraft: false);
                DetectCriticalPoints();
                pnlCanvas.Invalidate();

                lblStatus.Text = isPolar ? $"✓ Polar eğri eklendi: r = {eq}" : $"✓ Eğri eklendi: y = {eq}";
                lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fonksiyon derlenemedi:\n{ex.Message}", "Formül Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "❌ Hata: " + ex.Message;
                lblStatus.ForeColor = ThemeHelper.DangerRed;
            }
        }

        public void AddMarkedPoint(float x, float y, string pointType = "Serbest", Color? color = null)
        {
            _points.Add(new PlotPoint
            {
                X = x,
                Y = y,
                Color = color ?? Color.FromArgb(16, 185, 129),
                Shape = PointShape.Circle,
                Size = 10,
                PointType = pointType
            });
            RefreshPointsList();
            pnlCanvas.Invalidate();
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

            if (pnlCurvesContainer == null) return;

            pnlCurvesContainer.SuspendLayout();
            pnlCurvesContainer.Controls.Clear();

            if (_curves.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Henüz eğri eklenmedi.\n\nÜst paneldeki 'f(x) =' kutusuna formül yazıp\n'+ Ekle / Çiz' butonuna basarak eğri ekleyebilirsiniz.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Font = new Font("Segoe UI", 9F)
                };
                pnlCurvesContainer.Controls.Add(lblEmpty);
            }
            else
            {
                for (int i = _curves.Count - 1; i >= 0; i--)
                {
                    var curve = _curves[i];
                    pnlCurvesContainer.Controls.Add(CreateCurveCard(curve));
                }
            }

            if (lblCurvesCount != null)
                lblCurvesCount.Text = $"Toplam {_curves.Count} Eğri";

            pnlCurvesContainer.ResumeLayout();
        }

        private Panel CreateCurveCard(PlotCurve curve)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(22, 32, 50),
                Padding = new Padding(4)
            };

            Panel div = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(35, 48, 72)
            };
            card.Controls.Add(div);

            // Color picker swatch
            Button btnColor = new Button
            {
                Location = new Point(8, 14),
                Size = new Size(20, 20),
                BackColor = curve.Color,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnColor.FlatAppearance.BorderSize = 1;
            btnColor.FlatAppearance.BorderColor = Color.FromArgb(100, 116, 139);
            btnColor.Click += (s, e) =>
            {
                using (ColorDialog cd = new ColorDialog { Color = curve.Color })
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        curve.Color = cd.Color;
                        btnColor.BackColor = cd.Color;
                        pnlCanvas.Invalidate();
                    }
                }
            };
            card.Controls.Add(btnColor);

            // Badge & equation
            string badgeText = curve.IsPolar ? "r(θ)" : (curve.IsVerticalLine ? "x =" : "y =");
            Color badgeColor = curve.IsPolar ? Color.FromArgb(56, 189, 248) : Color.FromArgb(168, 85, 247);

            Label lblBadge = new Label
            {
                Location = new Point(34, 10),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                ForeColor = badgeColor,
                Text = badgeText
            };
            card.Controls.Add(lblBadge);

            Label lblEq = new Label
            {
                Location = new Point(62, 8),
                Size = new Size(155, 20),
                Font = new Font("Consolas", 9.5F, FontStyle.Bold),
                ForeColor = curve.Visible ? Color.White : Color.FromArgb(100, 116, 139),
                Text = curve.IsVerticalLine ? $"{curve.VerticalX:F2}" : curve.Equation
            };
            card.Controls.Add(lblEq);

            string domainText = curve.ClampDomain ? $"[{curve.DomainMinX:F1}, {curve.DomainMaxX:F1}]" : "Tüm Aralık";
            string styleText = curve.DashStyle switch { DashStyle.Dash => "Kesikli", DashStyle.Dot => "Noktalı", _ => "Düz" };
            Label lblDetails = new Label
            {
                Location = new Point(34, 28),
                Size = new Size(185, 16),
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Text = $"{domainText} • {styleText} • {curve.Thickness:F0}px"
            };
            card.Controls.Add(lblDetails);

            // Visibility button
            Button btnVis = new Button
            {
                Location = new Point(224, 12),
                Size = new Size(34, 24),
                FlatStyle = FlatStyle.Flat,
                Text = curve.Visible ? "Açık" : "Gizli",
                Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
                BackColor = curve.Visible ? Color.FromArgb(20, 50, 75) : Color.FromArgb(30, 35, 48),
                ForeColor = curve.Visible ? Color.FromArgb(56, 189, 248) : Color.FromArgb(100, 116, 139),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnVis.FlatAppearance.BorderSize = 1;
            btnVis.FlatAppearance.BorderColor = curve.Visible ? Color.FromArgb(14, 165, 233) : Color.FromArgb(51, 65, 85);
            btnVis.Click += (s, e) =>
            {
                curve.Visible = !curve.Visible;
                RefreshCurvesList();
                DetectCriticalPoints();
                pnlCanvas.Invalidate();
            };
            card.Controls.Add(btnVis);

            // Edit button
            Button btnEdit = new Button
            {
                Location = new Point(262, 12),
                Size = new Size(40, 24),
                FlatStyle = FlatStyle.Flat,
                Text = "Yükle",
                Font = new Font("Segoe UI", 7.5F),
                BackColor = Color.FromArgb(30, 41, 64),
                ForeColor = Color.FromArgb(226, 232, 240),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnEdit.FlatAppearance.BorderSize = 1;
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
            btnEdit.Click += (s, e) =>
            {
                if (!curve.IsVerticalLine)
                {
                    txtEquation.Text = curve.Equation;
                    chkPolar.Checked = curve.IsPolar;
                    if (curve.ClampDomain)
                    {
                        chkClampRange.Checked = true;
                        txtMinX.Text = curve.DomainMinX.ToString("0.##", CultureInfo.InvariantCulture);
                        txtMaxX.Text = curve.DomainMaxX.ToString("0.##", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        chkClampRange.Checked = false;
                    }
                    txtEquation.Focus();
                    lblStatus.Text = $"Eğri düzenleme için kutuya aktarıldı: {curve.Equation}";
                }
            };
            card.Controls.Add(btnEdit);

            // Delete button
            Button btnDel = new Button
            {
                Location = new Point(306, 12),
                Size = new Size(26, 24),
                FlatStyle = FlatStyle.Flat,
                Text = "✕",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 20, 25),
                ForeColor = Color.FromArgb(244, 63, 94),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnDel.FlatAppearance.BorderSize = 1;
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(127, 29, 29);
            btnDel.Click += (s, e) =>
            {
                _curves.Remove(curve);
                RefreshCurvesList();
                RefreshToolCombos();
                DetectCriticalPoints();
                pnlCanvas.Invalidate();
            };
            card.Controls.Add(btnDel);

            return card;
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
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                btnCurveColor.BackColor = curve.Color;
                cmbCurveThickness.SelectedIndex = curve.Thickness switch
                {
                    1f => 0,
                    2.4f or 2f => 1,
                    3f => 2,
                    4f => 3,
                    6f => 4,
                    _ => 1
                };

                cmbCurveStyle.SelectedIndex = curve.DashStyle switch
                {
                    DashStyle.Solid => 0,
                    DashStyle.Dash => 1,
                    DashStyle.Dot => 2,
                    _ => 0
                };
            }
        }

        private void BtnCurveColor_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                using (ColorDialog cd = new ColorDialog { Color = curve.Color })
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        curve.Color = cd.Color;
                        btnCurveColor.BackColor = cd.Color;
                        pnlCanvas.Invalidate();
                    }
                }
            }
        }

        private void CmbCurveStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                curve.DashStyle = cmbCurveStyle.SelectedIndex switch
                {
                    1 => DashStyle.Dash,
                    2 => DashStyle.Dot,
                    _ => DashStyle.Solid
                };
                pnlCanvas.Invalidate();
            }
        }

        private void CmbCurveThickness_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                curve.Thickness = cmbCurveThickness.SelectedIndex switch
                {
                    0 => 1f,
                    1 => 2.4f,
                    2 => 3f,
                    3 => 4f,
                    4 => 6f,
                    _ => 2.4f
                };
                pnlCanvas.Invalidate();
            }
        }

        private void BtnCurveToggle_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                curve.Visible = !curve.Visible;
                RefreshCurvesList();
                DetectCriticalPoints();
                pnlCanvas.Invalidate();
            }
        }

        private void BtnCurveDelete_Click(object sender, EventArgs e)
        {
            if (lstCurves.SelectedItem is PlotCurve curve)
            {
                _curves.Remove(curve);
                RefreshCurvesList();
                RefreshToolCombos();
                DetectCriticalPoints();
                pnlCanvas.Invalidate();
            }
        }

        private void EditSelectedCurve()
        {
            if (lstCurves.SelectedItem is PlotCurve curve && !curve.IsVerticalLine)
            {
                txtEquation.Text = curve.Equation;
                chkPolar.Checked = curve.IsPolar;
                if (curve.ClampDomain)
                {
                    chkClampRange.Checked = true;
                    txtMinX.Text = curve.DomainMinX.ToString("0.##", CultureInfo.InvariantCulture);
                    txtMaxX.Text = curve.DomainMaxX.ToString("0.##", CultureInfo.InvariantCulture);
                }
                else
                {
                    chkClampRange.Checked = false;
                }
                txtEquation.Focus();
                lblStatus.Text = $"Eğri düzenleme için kutuya yüklendi: {curve.Equation}";
            }
        }

        private void BtnCurveClearAll_Click(object sender, EventArgs e)
        {
            _curves.Clear();
            RefreshCurvesList();
            RefreshToolCombos();
            _criticalPoints.Clear();
            _asymptotePoles.Clear();
            pnlCanvas.Invalidate();
            lblStatus.Text = "Tüm eğriler temizlendi.";
        }

        #endregion

        #region Point Generation, Asymptote & Critical Points

        private void GenerateAllPoints(bool autoFitY, bool isDraft = false)
        {
            _asymptotePoles.Clear();
            foreach (var c in _curves)
            {
                if (c.Visible && !c.IsVerticalLine && c.Compiled != null)
                {
                    SetCurveParameters(c);
                    GenerateCurveSegments(c, isDraft);
                }
            }
        }

        private void GenerateCurveSegments(PlotCurve curve, bool isDraft = false)
        {
            curve.Segments.Clear();
            if (curve.Compiled == null) return;

            // 1. Polar Curve Sampling
            if (curve.IsPolar)
            {
                float minTheta = 0f;
                float maxTheta = (float)(4 * Math.PI);
                int polarSamples = isDraft ? 400 : 1200;
                float dTheta = (maxTheta - minTheta) / polarSamples;

                List<PointF> polarSeg = new List<PointF>();
                bool prevRadP = Function.UseRadians;
                Function.UseRadians = true;

                try
                {
                    for (int i = 0; i <= polarSamples; i++)
                    {
                        float theta = minTheta + i * dTheta;
                        try
                        {
                            curve.Compiled.SetVariable("theta", (decimal)theta);
                            curve.Compiled.SetVariable("x", (decimal)theta);
                            curve.Compiled.SetVariable("t", (decimal)theta);
                            curve.Compiled.EvaluatePostfix();
                            float r = (float)curve.Compiled.Result;

                            if (!float.IsNaN(r) && !float.IsInfinity(r) && Math.Abs(r) < 1e5f)
                            {
                                float px = (float)(r * Math.Cos(theta));
                                float py = (float)(r * Math.Sin(theta));
                                polarSeg.Add(new PointF(px, py));
                            }
                            else
                            {
                                if (polarSeg.Count > 1) curve.Segments.Add(polarSeg);
                                polarSeg = new List<PointF>();
                            }
                        }
                        catch
                        {
                            if (polarSeg.Count > 1) curve.Segments.Add(polarSeg);
                            polarSeg = new List<PointF>();
                        }
                    }
                    if (polarSeg.Count > 1) curve.Segments.Add(polarSeg);
                }
                finally
                {
                    Function.UseRadians = prevRadP;
                }
                return;
            }

            // 2. Cartesian Curve Sampling
            float drawMinX = curve.ClampDomain ? Math.Max(rangeMinX, curve.DomainMinX) : rangeMinX;
            float drawMaxX = curve.ClampDomain ? Math.Min(rangeMaxX, curve.DomainMaxX) : rangeMaxX;
            if (drawMinX >= drawMaxX) return;

            int sampleCount = isDraft
                ? Math.Min(250, Math.Max(100, pnlCanvas.Width / 3))
                : Math.Max(600, pnlCanvas.Width * 2);

            float step = (drawMaxX - drawMinX) / sampleCount;
            if (step <= 0) return;

            List<PointF> currentSegment = new List<PointF>();
            float? prevY = null;
            float? prevX = null;

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

                        // Robust Asymptote Jump Filter: prevents drawing false lines connecting across poles
                        float viewHeight = rangeMaxY - rangeMinY;
                        bool isAsymptoteJump = prevY.HasValue && (
                            Math.Abs(y - prevY.Value) > viewHeight * 1.5f && (
                                (y * prevY.Value < 0) || Math.Abs(y) > viewHeight * 2f || Math.Abs(prevY.Value) > viewHeight * 2f
                            )
                        );

                        if (isAsymptoteJump)
                        {
                            if (currentSegment.Count > 1) curve.Segments.Add(currentSegment);
                            currentSegment = new List<PointF>();

                            // Record asymptote vertical pole location
                            if (prevX.HasValue && !_asymptotePoles.Any(p => Math.Abs(p - x) < step * 2))
                            {
                                _asymptotePoles.Add((prevX.Value + x) / 2f);
                            }
                        }

                        currentSegment.Add(new PointF(x, y));
                        prevY = y;
                        prevX = x;
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

        private void DetectCriticalPoints()
        {
            _criticalPoints.Clear();
            if (!chkCriticalPoints.Checked) return;

            var activeCurves = _curves.Where(c => c.Visible && !c.IsVerticalLine && !c.IsPolar && c.Compiled != null).ToList();
            if (activeCurves.Count == 0) return;

            int steps = 140;
            float dx = (rangeMaxX - rangeMinX) / steps;
            if (dx <= 0) return;

            foreach (var curve in activeCurves)
            {
                SetCurveParameters(curve);
                float? prevX = null;
                decimal? prevY = null;
                decimal? prevD1 = null;
                decimal? prevD2 = null;

                for (int i = 0; i <= steps; i++)
                {
                    float x = rangeMinX + i * dx;
                    try
                    {
                        decimal yDec = NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x);
                        if (Math.Abs(yDec) > 1000m) continue;

                        decimal h = 0.005m;
                        decimal d1 = (NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x + h) -
                                      NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x - h)) / (2m * h);
                        decimal d2 = (NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x + h) -
                                      2m * yDec +
                                      NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)x - h)) / (h * h);

                        // 1. Root (f(x) = 0)
                        if (prevY.HasValue && prevY.Value * yDec <= 0m && Math.Abs(yDec - prevY.Value) < 25m)
                        {
                            float rootX = x - (float)(yDec * (decimal)dx / (yDec - prevY.Value == 0 ? 1m : (yDec - prevY.Value)));
                            if (rootX >= rangeMinX && rootX <= rangeMaxX && !_criticalPoints.Any(cp => Math.Abs(cp.X - rootX) < dx && cp.TypeName == "Kök"))
                            {
                                _criticalPoints.Add(new CriticalPoint
                                {
                                    X = rootX,
                                    Y = 0f,
                                    TypeName = "Kök",
                                    Color = Color.FromArgb(16, 185, 129),
                                    Details = $"Kök [f(x)=0]: ({rootX:F3}, 0.000)"
                                });
                            }
                        }

                        // 2. Extremum (f'(x) = 0)
                        if (prevD1.HasValue && prevD1.Value * d1 <= 0m && Math.Abs(d1 - prevD1.Value) < 60m)
                        {
                            float extX = x - dx / 2f;
                            float extY = (float)yDec;
                            if (extX >= rangeMinX && extX <= rangeMaxX)
                            {
                                bool isMax = d2 < 0m;
                                string type = isMax ? "Yerel Maks" : "Yerel Min";
                                Color col = isMax ? Color.FromArgb(244, 63, 94) : Color.FromArgb(56, 189, 248);

                                if (!_criticalPoints.Any(cp => Math.Abs(cp.X - extX) < dx && cp.TypeName == type))
                                {
                                    _criticalPoints.Add(new CriticalPoint
                                    {
                                        X = extX,
                                        Y = extY,
                                        TypeName = type,
                                        Color = col,
                                        Details = $"{type} [f'=0]: ({extX:F3}, {extY:F3})"
                                    });
                                }
                            }
                        }

                        // 3. Inflection Point (f''(x) = 0)
                        if (prevD2.HasValue && prevD2.Value * d2 <= 0m && Math.Abs(d2 - prevD2.Value) < 120m)
                        {
                            float inflX = x - dx / 2f;
                            float inflY = (float)yDec;
                            if (inflX >= rangeMinX && inflX <= rangeMaxX && !_criticalPoints.Any(cp => Math.Abs(cp.X - inflX) < dx && cp.TypeName == "Büküm"))
                            {
                                _criticalPoints.Add(new CriticalPoint
                                {
                                    X = inflX,
                                    Y = inflY,
                                    TypeName = "Büküm",
                                    Color = Color.FromArgb(245, 158, 11),
                                    Details = $"Büküm [f''=0]: ({inflX:F3}, {inflY:F3})"
                                });
                            }
                        }

                        prevX = x;
                        prevY = yDec;
                        prevD1 = d1;
                        prevD2 = d2;
                    }
                    catch { }
                }
            }

            // 4. Intersections between curves
            if (activeCurves.Count >= 2)
            {
                for (int a = 0; a < activeCurves.Count; a++)
                {
                    for (int b = a + 1; b < activeCurves.Count; b++)
                    {
                        var c1 = activeCurves[a];
                        var c2 = activeCurves[b];
                        decimal? prevDiff = null;

                        for (int i = 0; i <= steps; i++)
                        {
                            float x = rangeMinX + i * dx;
                            try
                            {
                                decimal y1 = NumericalMethods.EvaluateCompiled(c1.Compiled, "x", (decimal)x);
                                decimal y2 = NumericalMethods.EvaluateCompiled(c2.Compiled, "x", (decimal)x);
                                decimal diff = y1 - y2;

                                if (prevDiff.HasValue && prevDiff.Value * diff <= 0m && Math.Abs(diff - prevDiff.Value) < 30m)
                                {
                                    float intX = x - dx / 2f;
                                    float intY = (float)y1;
                                    if (!_criticalPoints.Any(cp => Math.Abs(cp.X - intX) < dx && cp.TypeName == "Kesişim"))
                                    {
                                        _criticalPoints.Add(new CriticalPoint
                                        {
                                            X = intX,
                                            Y = intY,
                                            TypeName = "Kesişim",
                                            Color = Color.FromArgb(168, 85, 247),
                                            Details = $"Kesişim: ({intX:F3}, {intY:F3})"
                                        });
                                    }
                                }
                                prevDiff = diff;
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        #endregion

        #region Canvas Mouse Interaction

        public SnapResult GetSnapCoordinate(Point mousePos, float w, float h)
        {
            float MapX(float x) => (x - rangeMinX) / (rangeMaxX - rangeMinX) * w;
            float MapY(float y) => h - ((y - rangeMinY) / (rangeMaxY - rangeMinY) * h);
            float InvMapX(float sx) => rangeMinX + (sx / w) * (rangeMaxX - rangeMinX);
            float InvMapY(float sy) => rangeMinY + ((h - sy) / h) * (rangeMaxY - rangeMinY);

            float NormalizeVal(float val)
            {
                if (Math.Abs(val) < 1e-4f) return 0f;
                return (float)Math.Round(val, 4);
            }

            float mouseWorldX = InvMapX(mousePos.X);
            float mouseWorldY = InvMapY(mousePos.Y);

            // ==========================================
            // Tier 1: Critical Points (Roots, Max, Min, Inflection, Intersection) <= 14px
            // ==========================================
            if (chkCriticalPoints.Checked && _criticalPoints.Count > 0)
            {
                CriticalPoint closestCp = null;
                float minDistSq = 14f * 14f;

                foreach (var cp in _criticalPoints)
                {
                    float csx = MapX(cp.X);
                    float csy = MapY(cp.Y);
                    float dSq = (mousePos.X - csx) * (mousePos.X - csx) + (mousePos.Y - csy) * (mousePos.Y - csy);
                    if (dSq <= minDistSq)
                    {
                        minDistSq = dSq;
                        closestCp = cp;
                    }
                }

                if (closestCp != null)
                {
                    float wx = NormalizeVal(closestCp.X);
                    float wy = NormalizeVal(closestCp.Y);
                    return new SnapResult
                    {
                        ScreenX = MapX(wx),
                        ScreenY = MapY(wy),
                        WorldX = wx,
                        WorldY = wy,
                        SnapType = closestCp.TypeName,
                        SnapColor = closestCp.Color,
                        Description = closestCp.Details,
                        IsSnapped = true
                    };
                }
            }

            // ==========================================
            // Tier 2: Snap-to-Curve (f(x) or vertical line) <= 12px
            // ==========================================
            var activeCurves = _curves.Where(c => c.Visible && (c.Compiled != null || c.IsVerticalLine) && !c.IsPolar).ToList();
            PlotCurve bestCurve = null;
            float bestCurveDist = 12f;
            float bestCurveWorldX = 0f;
            float bestCurveWorldY = 0f;
            float bestCurveScreenX = 0f;
            float bestCurveScreenY = 0f;

            bool prevRad = Function.UseRadians;
            Function.UseRadians = true;
            try
            {
                foreach (var curve in activeCurves)
                {
                    if (curve.IsVerticalLine)
                    {
                        float vxScreen = MapX(curve.VerticalX);
                        float hDist = Math.Abs(mousePos.X - vxScreen);
                        if (hDist <= bestCurveDist)
                        {
                            float wy = mouseWorldY;
                            if (wy >= curve.VerticalMinY && wy <= curve.VerticalMaxY)
                            {
                                bestCurveDist = hDist;
                                bestCurve = curve;
                                bestCurveWorldX = NormalizeVal(curve.VerticalX);
                                bestCurveWorldY = NormalizeVal(wy);
                                bestCurveScreenX = vxScreen;
                                bestCurveScreenY = mousePos.Y;
                            }
                        }
                        continue;
                    }

                    if (curve.ClampDomain && (mouseWorldX < curve.DomainMinX || mouseWorldX > curve.DomainMaxX))
                        continue;

                    try
                    {
                        SetCurveParameters(curve);
                        decimal valDec = NumericalMethods.EvaluateCompiled(curve.Compiled, "x", (decimal)mouseWorldX);
                        float valY = (float)valDec;
                        if (!float.IsNaN(valY) && !float.IsInfinity(valY) && Math.Abs(valY) < 1e5f)
                        {
                            float csy = MapY(valY);
                            float vDist = Math.Abs(mousePos.Y - csy);
                            if (vDist <= bestCurveDist)
                            {
                                bestCurveDist = vDist;
                                bestCurve = curve;
                                bestCurveWorldX = NormalizeVal(mouseWorldX);
                                bestCurveWorldY = NormalizeVal(valY);
                                bestCurveScreenX = mousePos.X;
                                bestCurveScreenY = csy;
                            }
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                Function.UseRadians = prevRad;
            }

            if (bestCurve != null)
            {
                return new SnapResult
                {
                    ScreenX = bestCurveScreenX,
                    ScreenY = bestCurveScreenY,
                    WorldX = bestCurveWorldX,
                    WorldY = bestCurveWorldY,
                    SnapType = "Eğri",
                    SnapColor = bestCurve.Color,
                    Description = $"Eğri [{bestCurve.Equation}]: ({bestCurveWorldX:F2}, {bestCurveWorldY:F2})",
                    IsSnapped = true,
                    BoundEquation = bestCurve.Equation
                };
            }

            // ==========================================
            // Tier 3: Snap-to-Axes & Origin <= 8px
            // ==========================================
            float ox = MapX(0f);
            float oy = MapY(0f);

            // Origin (0, 0) <= 10px
            float distOriginSq = (mousePos.X - ox) * (mousePos.X - ox) + (mousePos.Y - oy) * (mousePos.Y - oy);
            if (distOriginSq <= 10f * 10f)
            {
                return new SnapResult
                {
                    ScreenX = ox,
                    ScreenY = oy,
                    WorldX = 0f,
                    WorldY = 0f,
                    SnapType = "Orijin",
                    SnapColor = Color.FromArgb(250, 204, 21),
                    Description = "Orijin (0.00, 0.00)",
                    IsSnapped = true
                };
            }

            // X-Axis (y = 0) <= 8px
            if (Math.Abs(mousePos.Y - oy) <= 8f)
            {
                float wx = NormalizeVal(mouseWorldX);
                return new SnapResult
                {
                    ScreenX = MapX(wx),
                    ScreenY = oy,
                    WorldX = wx,
                    WorldY = 0f,
                    SnapType = "X-Ekseni",
                    SnapColor = Color.FromArgb(148, 163, 184),
                    Description = $"X Ekseni: ({wx:F2}, 0.00)",
                    IsSnapped = true
                };
            }

            // Y-Axis (x = 0) <= 8px
            if (Math.Abs(mousePos.X - ox) <= 8f)
            {
                float wy = NormalizeVal(mouseWorldY);
                return new SnapResult
                {
                    ScreenX = ox,
                    ScreenY = MapY(wy),
                    WorldX = 0f,
                    WorldY = wy,
                    SnapType = "Y-Ekseni",
                    SnapColor = Color.FromArgb(148, 163, 184),
                    Description = $"Y Ekseni: (0.00, {wy:F2})",
                    IsSnapped = true
                };
            }

            // ==========================================
            // Tier 4: Snap-to-Grid <= 8px (bypassed if Ctrl is pressed)
            // ==========================================
            if ((ModifierKeys & Keys.Control) != Keys.Control)
            {
                float stepW = CalculateStep(rangeMaxX - rangeMinX, 10);
                float stepH = CalculateStep(rangeMaxY - rangeMinY, 8);
                if (stepW > 0 && stepH > 0)
                {
                    float gridX = (float)Math.Round(mouseWorldX / stepW) * stepW;
                    float gridY = (float)Math.Round(mouseWorldY / stepH) * stepH;
                    float gsx = MapX(gridX);
                    float gsy = MapY(gridY);
                    float gDistSq = (mousePos.X - gsx) * (mousePos.X - gsx) + (mousePos.Y - gsy) * (mousePos.Y - gsy);
                    if (gDistSq <= 8f * 8f)
                    {
                        gridX = NormalizeVal(gridX);
                        gridY = NormalizeVal(gridY);
                        return new SnapResult
                        {
                            ScreenX = gsx,
                            ScreenY = gsy,
                            WorldX = gridX,
                            WorldY = gridY,
                            SnapType = "Izgara",
                            SnapColor = Color.FromArgb(100, 116, 139),
                            Description = $"Izgara: ({gridX:F2}, {gridY:F2})",
                            IsSnapped = true
                        };
                    }
                }
            }

            // ==========================================
            // Tier 5: Free Point (Normalized)
            // ==========================================
            float freeX = NormalizeVal(mouseWorldX);
            float freeY = NormalizeVal(mouseWorldY);
            return new SnapResult
            {
                ScreenX = mousePos.X,
                ScreenY = mousePos.Y,
                WorldX = freeX,
                WorldY = freeY,
                SnapType = "Serbest",
                SnapColor = btnPtColor.BackColor,
                Description = $"({freeX:F2}, {freeY:F2})",
                IsSnapped = false
            };
        }

        private void PnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                didMoveMouse = false;
                mouseDownPos = e.Location;
                lastMousePos = e.Location;
                Cursor = Cursors.SizeAll;
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Delete point under right click
                float w = Math.Max(1, pnlCanvas.Width);
                float h = Math.Max(1, pnlCanvas.Height);

                PlotPoint toRemove = null;
                foreach (var p in _points)
                {
                    float sx = (p.X - rangeMinX) / (rangeMaxX - rangeMinX) * w;
                    float sy = h - ((p.Y - rangeMinY) / (rangeMaxY - rangeMinY) * h);

                    float distSq = (e.X - sx) * (e.X - sx) + (e.Y - sy) * (e.Y - sy);
                    if (distSq <= 144f) // 12px radius tolerance
                    {
                        toRemove = p;
                        break;
                    }
                }

                if (toRemove != null)
                {
                    _points.Remove(toRemove);
                    RefreshPointsList();
                    lblStatus.Text = $"Nokta silindi [{toRemove.PointType}]: ({toRemove.X:F2}, {toRemove.Y:F2})";
                    lblStatus.ForeColor = Color.FromArgb(244, 63, 94);
                    pnlCanvas.Invalidate();
                }
            }
        }

        private void PnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            hoverMousePos = e.Location;

            if (isDragging)
            {
                int dragDx = e.X - mouseDownPos.X;
                int dragDy = e.Y - mouseDownPos.Y;
                if (Math.Abs(dragDx) > 5 || Math.Abs(dragDy) > 5)
                {
                    didMoveMouse = true;
                }

                if (didMoveMouse)
                {
                    int dx = e.X - lastMousePos.X;
                    int dy = e.Y - lastMousePos.Y;

                    float rangeW = rangeMaxX - rangeMinX;
                    float rangeH = rangeMaxY - rangeMinY;

                    float shiftX = (dx / (float)pnlCanvas.Width) * rangeW;
                    float shiftY = (dy / (float)pnlCanvas.Height) * rangeH;

                    rangeMinX -= shiftX;
                    rangeMaxX -= shiftX;
                    rangeMinY += shiftY;
                    rangeMaxY += shiftY;

                    lastMousePos = e.Location;

                    GenerateAllPoints(false, isDraft: true);
                }
                pnlCanvas.Invalidate();
            }
            else
            {
                pnlCanvas.Invalidate();
            }
        }

        private void PnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!didMoveMouse)
                {
                    float w = Math.Max(1, pnlCanvas.Width);
                    float h = Math.Max(1, pnlCanvas.Height);

                    var snap = GetSnapCoordinate(e.Location, w, h);
                    Color ptColor = snap.IsSnapped ? snap.SnapColor : btnPtColor.BackColor;
                    PointShape ptShape = (PointShape)Math.Max(0, cmbPtShape.SelectedIndex);

                    _points.Add(new PlotPoint
                    {
                        X = snap.WorldX,
                        Y = snap.WorldY,
                        Color = ptColor,
                        Shape = ptShape,
                        Size = 8,
                        PointType = snap.SnapType,
                        IsGlider = (snap.SnapType == "Eğri"),
                        BoundEquation = snap.BoundEquation
                    });

                    RefreshPointsList();
                    lblStatus.Text = $"Nokta eklendi [{snap.SnapType}]: ({snap.WorldX:F2}, {snap.WorldY:F2}) - Silmek için sağ tıklayın.";
                    lblStatus.ForeColor = Color.FromArgb(52, 211, 153);
                }

                isDragging = false;
                resampleDebounceTimer.Stop();
                GenerateAllPoints(false, isDraft: false);
                DetectCriticalPoints();
                Cursor = Cursors.Cross;
                pnlCanvas.Invalidate();
            }
        }

        private void PnlCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = (e.Delta > 0) ? 0.85f : 1.18f;

            float w = Math.Max(1, pnlCanvas.Width);
            float h = Math.Max(1, pnlCanvas.Height);

            float mouseLogX = rangeMinX + (e.X / w) * (rangeMaxX - rangeMinX);
            float mouseLogY = rangeMinY + ((h - e.Y) / h) * (rangeMaxY - rangeMinY);

            bool isShift = (ModifierKeys & Keys.Shift) == Keys.Shift;
            bool isCtrl = (ModifierKeys & Keys.Control) == Keys.Control;

            if (isShift)
            {
                rangeMinX = mouseLogX + (rangeMinX - mouseLogX) * zoomFactor;
                rangeMaxX = mouseLogX + (rangeMaxX - mouseLogX) * zoomFactor;
                lblStatus.Text = "X Ekseni ölçekleniyor (Shift+Tekerlek)...";
            }
            else if (isCtrl)
            {
                rangeMinY = mouseLogY + (rangeMinY - mouseLogY) * zoomFactor;
                rangeMaxY = mouseLogY + (rangeMaxY - mouseLogY) * zoomFactor;
                lblStatus.Text = "Y Ekseni ölçekleniyor (Ctrl+Tekerlek)...";
            }
            else
            {
                rangeMinX = mouseLogX + (rangeMinX - mouseLogX) * zoomFactor;
                rangeMaxX = mouseLogX + (rangeMaxX - mouseLogX) * zoomFactor;
                rangeMinY = mouseLogY + (rangeMinY - mouseLogY) * zoomFactor;
                rangeMaxY = mouseLogY + (rangeMaxY - mouseLogY) * zoomFactor;
            }

            txtMinX.Text = rangeMinX.ToString("0.##", CultureInfo.InvariantCulture);
            txtMaxX.Text = rangeMaxX.ToString("0.##", CultureInfo.InvariantCulture);

            GenerateAllPoints(false, isDraft: true);
            pnlCanvas.Invalidate();

            resampleDebounceTimer.Stop();
            resampleDebounceTimer.Start();
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

            // 1. Adaptive Grid with Sub-Divisions
            DrawAdaptiveGrid(g, w, h, MapX, MapY);

            // 2. Axes
            using (Pen axisPen = new Pen(Color.FromArgb(148, 163, 184), 1.8f))
            {
                float y0 = MapY(0);
                float x0 = MapX(0);

                if (y0 >= 0 && y0 <= h) g.DrawLine(axisPen, 0, y0, w, y0);
                if (x0 >= 0 && x0 <= w) g.DrawLine(axisPen, x0, 0, x0, h);
            }

            // 3. Vertical Asymptotes (if enabled)
            if (chkShowAsymptotes.Checked && _asymptotePoles.Count > 0)
            {
                using (Pen asympPen = new Pen(Color.FromArgb(244, 63, 94), 1.4f) { DashStyle = DashStyle.Dash })
                using (Font asympFont = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
                using (Brush asympBr = new SolidBrush(Color.FromArgb(244, 63, 94)))
                {
                    foreach (var pole in _asymptotePoles)
                    {
                        float sx = MapX(pole);
                        if (sx >= 0 && sx <= w)
                        {
                            g.DrawLine(asympPen, sx, 0, sx, h);
                            g.DrawString($"x = {pole:F2}", asympFont, asympBr, sx + 4, 12);
                        }
                    }
                }
            }

            // 4. Draw Curves
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

            // 5. Critical Points (Roots, Min/Max, Inflection, Intersections)
            if (chkCriticalPoints.Checked && _criticalPoints.Count > 0)
            {
                using (Font cpFont = new Font("Segoe UI", 8F, FontStyle.Bold))
                using (Brush haloBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
                {
                    foreach (var cp in _criticalPoints)
                    {
                        float sx = MapX(cp.X);
                        float sy = MapY(cp.Y);
                        if (sx < 0 || sx > w || sy < 0 || sy > h) continue;

                        using (Brush ptBrush = new SolidBrush(cp.Color))
                        using (Pen outlinePen = new Pen(Color.White, 1.2f))
                        {
                            g.FillEllipse(haloBrush, sx - 8, sy - 8, 16, 16);
                            g.FillEllipse(ptBrush, sx - 4, sy - 4, 8, 8);
                            g.DrawEllipse(outlinePen, sx - 4, sy - 4, 8, 8);
                        }
                    }
                }
            }

            // 6. Draw Marked Points
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

            // 7. Dynamic Magnetic Snap Indicator ("Hayalet Halka") & Inspect Mode HUD
            if (hoverMousePos.HasValue && !isDragging)
            {
                var snap = GetSnapCoordinate(hoverMousePos.Value, w, h);

                // Draw Snap Ghost Ring ("Hayalet Halka") if snapped or on hover
                if (snap.IsSnapped)
                {
                    float sx = snap.ScreenX;
                    float sy = snap.ScreenY;

                    if (sx >= 0 && sx <= w && sy >= 0 && sy <= h)
                    {
                        // Semi-transparent halo (outer glow)
                        using (Brush haloBrush = new SolidBrush(Color.FromArgb(50, snap.SnapColor)))
                        {
                            g.FillEllipse(haloBrush, sx - 10, sy - 10, 20, 20);
                        }
                        // Crisply rendered magnetic target ring
                        using (Pen ringPen = new Pen(snap.SnapColor, 1.8f))
                        {
                            g.DrawEllipse(ringPen, sx - 6, sy - 6, 12, 12);
                        }
                        // Center dot
                        using (Brush dotBrush = new SolidBrush(Color.White))
                        {
                            g.FillEllipse(dotBrush, sx - 2, sy - 2, 4, 4);
                        }

                        // Compact badge preview when inspect mode is not actively displaying the large HUD box
                        if (!chkInspectMode.Checked)
                        {
                            using (Font badgeFont = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
                            using (Brush badgeBg = new SolidBrush(Color.FromArgb(220, 15, 23, 42)))
                            using (Pen badgeBorder = new Pen(snap.SnapColor, 1f))
                            using (Brush badgeTextBrush = new SolidBrush(Color.White))
                            using (Brush typeTextBrush = new SolidBrush(snap.SnapColor))
                            {
                                string typeTag = $"[{snap.SnapType}] ";
                                string coordTag = $"({snap.WorldX:F2}, {snap.WorldY:F2})";

                                SizeF szType = g.MeasureString(typeTag, badgeFont);
                                SizeF szCoord = g.MeasureString(coordTag, badgeFont);
                                float totalW = szType.Width + szCoord.Width + 14;
                                float totalH = 22;

                                float bx = Math.Min(w - totalW - 10, Math.Max(10, sx + 12));
                                float by = Math.Min(h - totalH - 10, Math.Max(10, sy - totalH - 6));

                                g.FillRectangle(badgeBg, bx, by, totalW, totalH);
                                g.DrawRectangle(badgeBorder, bx, by, totalW, totalH);

                                g.DrawString(typeTag, badgeFont, typeTextBrush, bx + 6, by + 4);
                                g.DrawString(coordTag, badgeFont, badgeTextBrush, bx + 6 + szType.Width - 4, by + 4);
                            }
                        }
                    }
                }

                // If Inspect Mode is explicitly checked, draw tangent line and acrylic HUD
                if (chkInspectMode.Checked)
                {
                    var activeCurve = _curves.FirstOrDefault(c => c.Visible && !c.IsVerticalLine && !c.IsPolar && c.Compiled != null);
                    if (activeCurve != null)
                    {
                        bool prevRad = Function.UseRadians;
                        Function.UseRadians = true;
                        try
                        {
                            SetCurveParameters(activeCurve);
                            float targetX = snap.WorldX;
                            decimal fxDec = NumericalMethods.EvaluateCompiled(activeCurve.Compiled, "x", (decimal)targetX);
                            float logY = (float)fxDec;
                            float sy = MapY(logY);
                            float sx = MapX(targetX);

                            decimal dfxDec = EvalDeriv(activeCurve.Compiled, (decimal)targetX);
                            float slope = (float)dfxDec;

                            // Inspect Tangent Line
                            if (!float.IsNaN(slope) && Math.Abs(slope) < 1e4f)
                            {
                                using (Pen tanLinePen = new Pen(Color.FromArgb(0, 210, 255), 2f))
                                using (Pen normPen = new Pen(Color.FromArgb(168, 85, 247), 1.2f) { DashStyle = DashStyle.Dash })
                                {
                                    float x1 = rangeMinX;
                                    float y1 = logY + slope * (x1 - targetX);
                                    float x2 = rangeMaxX;
                                    float y2 = logY + slope * (x2 - targetX);

                                    g.DrawLine(tanLinePen, MapX(x1), MapY(y1), MapX(x2), MapY(y2));

                                    // Normal line: slope = -1 / slope
                                    if (Math.Abs(slope) > 0.001f)
                                    {
                                        float nSlope = -1f / slope;
                                        float ny1 = logY + nSlope * (x1 - targetX);
                                        float ny2 = logY + nSlope * (x2 - targetX);
                                        g.DrawLine(normPen, MapX(x1), MapY(ny1), MapX(x2), MapY(ny2));
                                    }
                                }
                            }

                            using (Pen dashPen = new Pen(Color.FromArgb(90, 248, 250, 252), 1f) { DashStyle = DashStyle.Dash })
                            {
                                g.DrawLine(dashPen, sx, 0, sx, h);
                                if (sy >= 0 && sy <= h)
                                {
                                    g.DrawLine(dashPen, 0, sy, w, sy);
                                    using (Brush dotBrush = new SolidBrush(snap.IsSnapped ? snap.SnapColor : activeCurve.Color))
                                    {
                                        g.FillEllipse(dotBrush, sx - 5, sy - 5, 10, 10);
                                        g.DrawEllipse(Pens.White, sx - 5, sy - 5, 10, 10);
                                    }
                                }
                            }

                            // Modern Acrylic HUD Card
                            using (Font hudFont = new Font("Segoe UI", 9F, FontStyle.Bold))
                            using (Brush hudBg = new SolidBrush(Color.FromArgb(230, 11, 15, 28)))
                            using (Pen hudBorder = new Pen(Color.FromArgb(0, 210, 255), 1.5f))
                            using (Brush textCyan = new SolidBrush(Color.FromArgb(0, 210, 255)))
                            using (Brush textWhite = new SolidBrush(Color.White))
                            using (Brush textYellow = new SolidBrush(Color.FromArgb(250, 204, 21)))
                            {
                                string tLine1 = snap.IsSnapped ? snap.Description : $"x = {targetX:F3}  |  f(x) = {logY:F3}";
                                string tLine2 = $"Teğet: y = {slope:F2}x + {(logY - slope * targetX):F2}  |  dy/dx = {slope:F3}";

                                float boxW = Math.Max(260, Math.Max(g.MeasureString(tLine1, hudFont).Width, g.MeasureString(tLine2, hudFont).Width) + 24);
                                float boxH = 48;
                                float bx = Math.Min(w - boxW - 12, Math.Max(12, sx + 12));
                                float by = Math.Min(h - boxH - 12, Math.Max(12, sy - boxH - 12));

                                g.FillRectangle(hudBg, bx, by, boxW, boxH);
                                g.DrawRectangle(hudBorder, bx, by, boxW, boxH);

                                g.DrawString(tLine1, hudFont, snap.IsSnapped ? textYellow : textCyan, bx + 10, by + 6);
                                g.DrawString(tLine2, hudFont, textWhite, bx + 10, by + 25);
                            }
                        }
                        catch { }
                        finally
                        {
                            Function.UseRadians = prevRad;
                        }
                    }
                }
            }
        }

        private void DrawAdaptiveGrid(Graphics g, float w, float h, Func<float, float> mapX, Func<float, float> mapY)
        {
            float rangeW = rangeMaxX - rangeMinX;
            float stepW = CalculateStep(rangeW, 10);
            float minorW = stepW / 5f;

            float rangeH = rangeMaxY - rangeMinY;
            float stepH = CalculateStep(rangeH, 8);
            float minorH = stepH / 5f;

            // 1. Minor Sub-Grid Lines
            using (Pen minorPen = new Pen(Color.FromArgb(22, 32, 48), 1f))
            {
                if (minorW > 0)
                {
                    float startX = (float)Math.Floor(rangeMinX / minorW) * minorW;
                    for (float x = startX; x <= rangeMaxX; x += minorW)
                    {
                        float sx = mapX(x);
                        g.DrawLine(minorPen, sx, 0, sx, h);
                    }
                }
                if (minorH > 0)
                {
                    float startY = (float)Math.Floor(rangeMinY / minorH) * minorH;
                    for (float y = startY; y <= rangeMaxY; y += minorH)
                    {
                        float sy = mapY(y);
                        g.DrawLine(minorPen, 0, sy, w, sy);
                    }
                }
            }

            // 2. Major Grid Lines & Numbers
            using (Pen majorPen = new Pen(Color.FromArgb(45, 60, 85), 1.2f))
            using (Font font = new Font("Segoe UI", 8.5F, FontStyle.Regular))
            using (Brush brush = new SolidBrush(Color.FromArgb(148, 163, 184)))
            {
                if (stepW > 0)
                {
                    float startX = (float)Math.Floor(rangeMinX / stepW) * stepW;
                    for (float x = startX; x <= rangeMaxX; x += stepW)
                    {
                        float sx = mapX(x);
                        g.DrawLine(majorPen, sx, 0, sx, h);
                        if (Math.Abs(x) > 1e-4f)
                        {
                            g.DrawString(x.ToString("0.##", CultureInfo.InvariantCulture), font, brush, sx + 3, h - 20);
                        }
                    }
                }

                if (stepH > 0)
                {
                    float startY = (float)Math.Floor(rangeMinY / stepH) * stepH;
                    for (float y = startY; y <= rangeMaxY; y += stepH)
                    {
                        float sy = mapY(y);
                        g.DrawLine(majorPen, 0, sy, w, sy);
                        if (Math.Abs(y) > 1e-4f)
                        {
                            g.DrawString(y.ToString("0.##", CultureInfo.InvariantCulture), font, brush, 6, sy - 16);
                        }
                    }
                }
            }
        }

        private float CalculateStep(float range, int targetSteps)
        {
            float rawStep = range / targetSteps;
            double log = Math.Log10(rawStep);
            double baseExponent = Math.Floor(log);
            double fraction = rawStep / Math.Pow(10, baseExponent);

            double niceFraction;
            if (fraction <= 1.5) niceFraction = 1;
            else if (fraction <= 3.5) niceFraction = 2;
            else if (fraction <= 7.5) niceFraction = 5;
            else niceFraction = 10;

            return (float)(niceFraction * Math.Pow(10, baseExponent));
        }

        private void DrawCustomPoint(Graphics g, float sx, float sy, PlotPoint p)
        {
            int sz = p.Size;
            int half = sz / 2;

            using (Brush b = new SolidBrush(p.Color))
            using (Pen pen = new Pen(Color.White, 1.2f))
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

        private decimal EvalDeriv(Function fn, decimal x)
        {
            decimal h = 0.001m;
            decimal y1 = NumericalMethods.EvaluateCompiled(fn, "x", x + h);
            decimal y0 = NumericalMethods.EvaluateCompiled(fn, "x", x - h);
            return (y1 - y0) / (2m * h);
        }

        #endregion

        #region Points List Management

        private void RefreshPointsList()
        {
            lstPoints.BeginUpdate();
            lstPoints.Items.Clear();
            foreach (var p in _points)
            {
                lstPoints.Items.Add(p);
            }
            lstPoints.EndUpdate();

            if (pnlPointsContainer == null) return;

            pnlPointsContainer.SuspendLayout();
            pnlPointsContainer.Controls.Clear();

            if (_points.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Henüz serbest nokta eklenmedi.\n\nGrafik tuvali üzerine tıklayarak veya aşağıdaki giriş alanından koordinat belirterek nokta ekleyebilirsiniz.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(100, 116, 139),
                    Font = new Font("Segoe UI", 9F)
                };
                pnlPointsContainer.Controls.Add(lblEmpty);
            }
            else
            {
                for (int i = _points.Count - 1; i >= 0; i--)
                {
                    var pt = _points[i];
                    pnlPointsContainer.Controls.Add(CreatePointCard(pt));
                }
            }

            if (lblPointsCount != null)
                lblPointsCount.Text = $"Toplam {_points.Count} Nokta";

            pnlPointsContainer.ResumeLayout();
        }

        private Panel CreatePointCard(PlotPoint pt)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(22, 32, 50),
                Padding = new Padding(4)
            };

            Panel div = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(35, 48, 72)
            };
            card.Controls.Add(div);

            // Shape and color icon swatch
            string shapeIcon = pt.Shape switch
            {
                PointShape.Square => "■",
                PointShape.Diamond => "◆",
                PointShape.Cross => "+",
                _ => "●"
            };

            Label lblIcon = new Label
            {
                Location = new Point(6, 12),
                Size = new Size(18, 20),
                ForeColor = pt.Color,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = shapeIcon,
                TextAlign = ContentAlignment.MiddleCenter
            };
            card.Controls.Add(lblIcon);

            // Coordinate label
            Label lblCoord = new Label
            {
                Location = new Point(28, 5),
                Size = new Size(180, 18),
                Font = new Font("Consolas", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Text = $"({pt.X:F2}, {pt.Y:F2})"
            };
            card.Controls.Add(lblCoord);

            // Badge for PointType
            Color badgeBg = pt.PointType switch
            {
                "Kök" => Color.FromArgb(16, 185, 129),
                "Yerel Maks" => Color.FromArgb(244, 63, 94),
                "Yerel Min" => Color.FromArgb(56, 189, 248),
                "Büküm" => Color.FromArgb(245, 158, 11),
                "Kesişim" => Color.FromArgb(168, 85, 247),
                "Eğri" => Color.FromArgb(14, 165, 233),
                "Orijin" => Color.FromArgb(250, 204, 21),
                "X-Ekseni" => Color.FromArgb(148, 163, 184),
                "Y-Ekseni" => Color.FromArgb(148, 163, 184),
                "Izgara" => Color.FromArgb(100, 116, 139),
                _ => Color.FromArgb(71, 85, 105)
            };

            string typeLabelText = string.IsNullOrEmpty(pt.PointType) ? "Serbest" : pt.PointType;
            if (pt.IsGlider && !string.IsNullOrEmpty(pt.BoundEquation))
            {
                typeLabelText = $"Eğri: {pt.BoundEquation}";
            }

            Label lblBadge = new Label
            {
                Location = new Point(28, 24),
                Size = new Size(200, 16),
                Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold),
                ForeColor = badgeBg,
                Text = $"[{typeLabelText}] • {pt.Shape}"
            };
            card.Controls.Add(lblBadge);

            // Delete button
            Button btnDel = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(card.Width - 34, 9),
                Size = new Size(26, 26),
                FlatStyle = FlatStyle.Flat,
                Text = "✕",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(45, 20, 25),
                ForeColor = Color.FromArgb(244, 63, 94),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnDel.FlatAppearance.BorderSize = 1;
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(127, 29, 29);
            btnDel.Click += (s, e) =>
            {
                _points.Remove(pt);
                RefreshPointsList();
                pnlCanvas.Invalidate();
            };
            card.Controls.Add(btnDel);

            return card;
        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            float px = ParseFloat(txtPtX.Text, 0f);
            float py = ParseFloat(txtPtY.Text, 0f);

            string ptType = (px == 0f && py == 0f) ? "Orijin" : (px == 0f ? "Y-Ekseni" : (py == 0f ? "X-Ekseni" : "Serbest"));

            _points.Add(new PlotPoint
            {
                X = px,
                Y = py,
                Color = btnPtColor.BackColor,
                Shape = (PointShape)Math.Max(0, cmbPtShape.SelectedIndex),
                Size = 8,
                PointType = ptType
            });

            RefreshPointsList();
            pnlCanvas.Invalidate();
        }

        private void BtnDeletePoint_Click(object sender, EventArgs e)
        {
            if (lstPoints.SelectedIndex >= 0 && lstPoints.SelectedIndex < _points.Count)
            {
                _points.RemoveAt(lstPoints.SelectedIndex);
                RefreshPointsList();
                pnlCanvas.Invalidate();
            }
        }

        #endregion

        #region Exporting (Image, CSV, LaTeX)

        private void BtnExportImage_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PNG Resmi (*.png)|*.png|JPEG Resmi (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp",
                FileName = "ttCalculator_Grafik.png",
                Title = "Grafiği Resim Olarak Kaydet"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Bitmap bmp = new Bitmap(pnlCanvas.Width, pnlCanvas.Height))
                        {
                            pnlCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, pnlCanvas.Width, pnlCanvas.Height));
                            bmp.Save(sfd.FileName);
                        }
                        MessageBox.Show("Grafik görseli başarıyla dışa aktarıldı:\n" + sfd.FileName, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dışa aktarma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            var validCurves = _curves.Where(c => c.Visible && !c.IsVerticalLine && !c.IsPolar && c.Compiled != null).ToList();
            if (validCurves.Count == 0)
            {
                MessageBox.Show("Dışa aktarılacak görünür bir kartezyen eğri bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV Dosyası (*.csv)|*.csv",
                FileName = "ttCalculator_DegerTablosu.csv",
                Title = "Değer Tablosunu CSV Olarak İndir"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("x");
                        foreach (var c in validCurves)
                        {
                            sb.Append($",\"{c.Equation}\"");
                        }
                        sb.AppendLine();

                        float step = 0.1f;
                        for (float x = rangeMinX; x <= rangeMaxX + 1e-5f; x += step)
                        {
                            sb.Append(x.ToString("F3", CultureInfo.InvariantCulture));
                            foreach (var c in validCurves)
                            {
                                try
                                {
                                    SetCurveParameters(c);
                                    decimal y = NumericalMethods.EvaluateCompiled(c.Compiled, "x", (decimal)x);
                                    sb.Append("," + ((float)y).ToString("F4", CultureInfo.InvariantCulture));
                                }
                                catch
                                {
                                    sb.Append(",NaN");
                                }
                            }
                            sb.AppendLine();
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                        MessageBox.Show("Değer tablosu CSV formatında başarıyla kaydedildi:\n" + sfd.FileName, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("CSV oluşturma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExportLatex_Click(object sender, EventArgs e)
        {
            var validCurves = _curves.Where(c => c.Visible && !c.IsVerticalLine && !c.IsPolar).ToList();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("% ttCalculator v3.3 LaTeX Pgfplots / TikZ Kodu");
            sb.AppendLine("\\begin{tikzpicture}");
            sb.AppendLine("\\begin{axis}[");
            sb.AppendLine($"    xmin={rangeMinX:F1}, xmax={rangeMaxX:F1},");
            sb.AppendLine($"    ymin={rangeMinY:F1}, ymax={rangeMaxY:F1},");
            sb.AppendLine("    axis lines=middle,");
            sb.AppendLine("    grid=both,");
            sb.AppendLine("    xlabel={$x$},");
            sb.AppendLine("    ylabel={$y$},");
            sb.AppendLine("    samples=200,");
            sb.AppendLine("    legend pos=outer north east");
            sb.AppendLine("]");

            string[] colors = { "blue", "red", "green!70!black", "orange", "purple", "cyan" };
            int cIdx = 0;
            foreach (var c in validCurves)
            {
                string col = colors[cIdx % colors.Length];
                cIdx++;
                string tikzExpr = c.Equation.Replace("sin(", "sin(deg(")
                                           .Replace("cos(", "cos(deg(")
                                           .Replace("tan(", "tan(deg(")
                                           .Replace("^", "^");
                sb.AppendLine($"\\addplot[color={col}, thick, domain={rangeMinX:F1}:{rangeMaxX:F1}] {{{tikzExpr}}};");
                sb.AppendLine($"\\addlegendentry{{${c.Equation}$}}");
            }

            sb.AppendLine("\\end{axis}");
            sb.AppendLine("\\end{tikzpicture}");

            using (Form dlg = new Form
            {
                Text = "LaTeX (TikZ / Pgfplots) Kodu",
                Size = new Size(580, 420),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = ThemeHelper.DarkBackground,
                ForeColor = Color.White
            })
            {
                RichTextBox txt = new RichTextBox
                {
                    Dock = DockStyle.Top,
                    Height = 310,
                    Text = sb.ToString(),
                    Font = new Font("Consolas", 10F),
                    BackColor = ThemeHelper.CardBackground,
                    ForeColor = Color.FromArgb(0, 210, 255),
                    ReadOnly = true
                };

                Button btnCopy = ThemeHelper.CreateActionButton("📋 Panoya Kopyala", 160, 32, true);
                btnCopy.Location = new Point(200, 325);
                btnCopy.Click += (cs, ce) =>
                {
                    Clipboard.SetText(sb.ToString());
                    MessageBox.Show("LaTeX kodu panoya kopyalandı!", "Kopyalandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dlg.Close();
                };

                dlg.Controls.Add(txt);
                dlg.Controls.Add(btnCopy);
                dlg.ShowDialog(this);
            }
        }

        #endregion

        #region Tools & Geometry (Common Tangent, Closest Points, Point Tangent)

        private void BtnCommonTangent_Click(object sender, EventArgs e)
        {
            if (cmbToolF1.SelectedIndex < 0 || cmbToolF2.SelectedIndex < 0)
            {
                txtToolLog.Text = "Lütfen iki fonksiyon seçin.";
                return;
            }

            string eq1 = cmbToolF1.SelectedItem.ToString();
            string eq2 = cmbToolF2.SelectedItem.ToString();

            if (eq1 == eq2)
            {
                txtToolLog.Text = "Hata: İki farklı fonksiyon seçmelisiniz.";
                return;
            }

            try
            {
                Function f1 = NumericalMethods.CompileFunction(eq1);
                Function f2 = NumericalMethods.CompileFunction(eq2);

                txtToolLog.Text = $"Ortak Teğet Çözülüyor...\nf₁(x) = {eq1}\nf₂(x) = {eq2}\n\n";

                float x1Cand = -1f;
                float x2Cand = 1f;
                bool found = false;

                for (int iter = 0; iter < 40; iter++)
                {
                    decimal y1 = NumericalMethods.EvaluateCompiled(f1, "x", (decimal)x1Cand);
                    decimal y2 = NumericalMethods.EvaluateCompiled(f2, "x", (decimal)x2Cand);

                    decimal m1 = EvalDeriv(f1, (decimal)x1Cand);
                    decimal m2 = EvalDeriv(f2, (decimal)x2Cand);

                    if (Math.Abs(x2Cand - x1Cand) < 1e-4f) { x2Cand += 0.1f; continue; }

                    decimal secantM = (y2 - y1) / (decimal)(x2Cand - x1Cand);

                    decimal err1 = m1 - m2;
                    decimal err2 = m1 - secantM;

                    if (Math.Abs(err1) < 0.0001m && Math.Abs(err2) < 0.0001m)
                    {
                        found = true;
                        break;
                    }

                    x1Cand -= (float)(err1 * 0.1m);
                    x2Cand += (float)(err2 * 0.1m);
                }

                if (found)
                {
                    decimal m = EvalDeriv(f1, (decimal)x1Cand);
                    decimal y1 = NumericalMethods.EvaluateCompiled(f1, "x", (decimal)x1Cand);
                    decimal n = y1 - m * (decimal)x1Cand;

                    string tanEq = $"{m:F4}*x + {n:F4}";
                    txtToolLog.AppendText($"✓ Ortak Teğet Bulundu:\n");
                    txtToolLog.AppendText($"Teğet 1: x₁ = {x1Cand:F3}\n");
                    txtToolLog.AppendText($"Teğet 2: x₂ = {x2Cand:F3}\n");
                    txtToolLog.AppendText($"Doğru: y = {tanEq}\n");

                    AddFunctionCurve(tanEq, false, -20f, 20f, Color.FromArgb(236, 72, 153), DashStyle.Dash, 2.2f);
                }
                else
                {
                    txtToolLog.AppendText("Bu aralıkta ortak teğet yakınsayamadı (Kök bulunamadı).");
                }
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        private void BtnNearestPoints_Click(object sender, EventArgs e)
        {
            if (cmbToolF1.SelectedIndex < 0 || cmbToolF2.SelectedIndex < 0)
            {
                txtToolLog.Text = "Lütfen iki fonksiyon seçin.";
                return;
            }

            string eq1 = cmbToolF1.SelectedItem.ToString();
            string eq2 = cmbToolF2.SelectedItem.ToString();

            try
            {
                Function f1 = NumericalMethods.CompileFunction(eq1);
                Function f2 = NumericalMethods.CompileFunction(eq2);

                txtToolLog.Text = $"En Yakın Noktalar Analizi:\nf₁(x) = {eq1}\nf₂(x) = {eq2}\n\n";

                float bestX1 = 0, bestX2 = 0;
                float minDistSq = float.MaxValue;

                int steps = 60;
                float stepX = (rangeMaxX - rangeMinX) / steps;

                for (int i = 0; i <= steps; i++)
                {
                    float x1 = rangeMinX + i * stepX;
                    decimal y1Dec = NumericalMethods.EvaluateCompiled(f1, "x", (decimal)x1);
                    float y1 = (float)y1Dec;

                    for (int j = 0; j <= steps; j++)
                    {
                        float x2 = rangeMinX + j * stepX;
                        decimal y2Dec = NumericalMethods.EvaluateCompiled(f2, "x", (decimal)x2);
                        float y2 = (float)y2Dec;

                        float dSq = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                        if (dSq < minDistSq)
                        {
                            minDistSq = dSq;
                            bestX1 = x1;
                            bestX2 = x2;
                        }
                    }
                }

                float minY1 = (float)NumericalMethods.EvaluateCompiled(f1, "x", (decimal)bestX1);
                float minY2 = (float)NumericalMethods.EvaluateCompiled(f2, "x", (decimal)bestX2);
                float dist = (float)Math.Sqrt(minDistSq);

                txtToolLog.AppendText($"En Kısa Mesafe: {dist:F4}\n");
                txtToolLog.AppendText($"P₁ (f₁ üzerinde): ({bestX1:F3}, {minY1:F3})\n");
                txtToolLog.AppendText($"P₂ (f₂ üzerinde): ({bestX2:F3}, {minY2:F3})\n");

                _points.Add(new PlotPoint { X = bestX1, Y = minY1, Color = Color.Cyan, Shape = PointShape.Diamond, PointType = "En Yakın" });
                _points.Add(new PlotPoint { X = bestX2, Y = minY2, Color = Color.Magenta, Shape = PointShape.Diamond, PointType = "En Yakın" });
                RefreshPointsList();

                if (Math.Abs(bestX2 - bestX1) > 0.001f)
                {
                    float m = (minY2 - minY1) / (bestX2 - bestX1);
                    float n = minY1 - m * bestX1;
                    string lineEq = $"{m:F4}*x + {n:F4}";
                    txtToolLog.AppendText($"Aralarındaki Doğru: y = {lineEq}\n");
                    AddFunctionCurve(lineEq, true, Math.Min(bestX1, bestX2), Math.Max(bestX1, bestX2), Color.Yellow, DashStyle.Dot, 2f);
                }
                else
                {
                    AddVerticalLine(bestX1, Math.Min(minY1, minY2), Math.Max(minY1, minY2), Color.Yellow, DashStyle.Dot, 2f);
                }

                pnlCanvas.Invalidate();
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        private void BtnPointTangent_Click(object sender, EventArgs e)
        {
            if (cmbToolF1.SelectedIndex < 0)
            {
                txtToolLog.Text = "Lütfen 1. Fonksiyonu seçin.";
                return;
            }

            string eq = cmbToolF1.SelectedItem.ToString();
            float x0 = ParseFloat(txtTanPointX.Text, 0f);
            float y0 = ParseFloat(txtTanPointY.Text, 0f);

            try
            {
                Function f = NumericalMethods.CompileFunction(eq);
                txtToolLog.Text = $"Noktadan Teğet Analizi:\nNokta: ({x0}, {y0})\nf(x) = {eq}\n\n";

                float xt = x0;
                bool found = false;

                for (int iter = 0; iter < 50; iter++)
                {
                    decimal yt = NumericalMethods.EvaluateCompiled(f, "x", (decimal)xt);
                    decimal mt = EvalDeriv(f, (decimal)xt);

                    decimal res = yt - (decimal)y0 - mt * (decimal)(xt - x0);
                    if (Math.Abs(res) < 0.0001m)
                    {
                        found = true;
                        break;
                    }

                    decimal dRes = -EvalDeriv(f, (decimal)xt + 0.01m) + EvalDeriv(f, (decimal)xt - 0.01m);
                    if (Math.Abs(dRes) < 1e-6m) dRes = 1m;

                    xt -= (float)(res / dRes * 0.1m);
                }

                if (found)
                {
                    decimal mt = EvalDeriv(f, (decimal)xt);
                    decimal nt = (decimal)y0 - mt * (decimal)x0;
                    string tanEq = $"{mt:F4}*x + {nt:F4}";

                    txtToolLog.AppendText($"✓ Teğet Değme Noktası: x_t = {xt:F4}\n");
                    txtToolLog.AppendText($"Teğet Doğrusu: y = {tanEq}\n");

                    _points.Add(new PlotPoint { X = x0, Y = y0, Color = Color.Lime, Shape = PointShape.Cross, PointType = "Teğet Değme" });
                    RefreshPointsList();

                    AddFunctionCurve(tanEq, false, -20f, 20f, Color.Lime, DashStyle.Dash, 2.2f);
                }
                else
                {
                    txtToolLog.AppendText("Bu noktadan fonksiyona teğet çözümü bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                txtToolLog.Text = "Hata: " + ex.Message;
            }
        }

        #endregion

        #region Parametric Shapes (Primitives)

        private void BtnAddShape_Click(object sender, EventArgs e)
        {
            float cx = ParseFloat(txtShapeCenterX.Text, 0f);
            float cy = ParseFloat(txtShapeCenterY.Text, 0f);
            float d1 = Math.Max(0.1f, ParseFloat(txtShapeDim1.Text, 4f));
            float d2 = Math.Max(0.1f, ParseFloat(txtShapeDim2.Text, 2f));
            float angleDeg = ParseFloat(txtShapeAngle.Text, 0f);
            float angleRad = angleDeg * (float)Math.PI / 180f;

            int shapeIdx = cmbShapeType.SelectedIndex;
            Color sc = _palette[_colorIndex % _palette.Length];
            _colorIndex++;

            switch (shapeIdx)
            {
                case 0: // Circle / Ellipse
                    AddFunctionCurve($"{cy} + {d2} * sqrt(max(0, 1 - ((x - {cx}) / {d1})^2))", true, cx - d1, cx + d1, sc);
                    AddFunctionCurve($"{cy} - {d2} * sqrt(max(0, 1 - ((x - {cx}) / {d1})^2))", true, cx - d1, cx + d1, sc);
                    lblStatus.Text = $"Elips eklendi (R1={d1}, R2={d2})";
                    break;

                case 1: // Rectangle / Square
                    AddParametricPolygon(new PointF[]
                    {
                        RotatePoint(new PointF(-d1, -d2), angleRad, cx, cy),
                        RotatePoint(new PointF(d1, -d2), angleRad, cx, cy),
                        RotatePoint(new PointF(d1, d2), angleRad, cx, cy),
                        RotatePoint(new PointF(-d1, d2), angleRad, cx, cy)
                    }, sc);
                    lblStatus.Text = $"Dikdörtgen eklendi (Genişlik={d1 * 2}, Yükseklik={d2 * 2}, Açı={angleDeg}°)";
                    break;

                case 2: // Triangle
                    float hTri = d1 * (float)Math.Sqrt(3) / 2f;
                    AddParametricPolygon(new PointF[]
                    {
                        RotatePoint(new PointF(0, hTri * 2f / 3f), angleRad, cx, cy),
                        RotatePoint(new PointF(d1 / 2f, -hTri / 3f), angleRad, cx, cy),
                        RotatePoint(new PointF(-d1 / 2f, -hTri / 3f), angleRad, cx, cy)
                    }, sc);
                    lblStatus.Text = $"Üçgen eklendi (Kenar={d1}, Açı={angleDeg}°)";
                    break;

                case 3: // Pentagon
                    AddRegularPolygon(cx, cy, d1, 5, angleRad, sc);
                    lblStatus.Text = $"Düzgün Beşgen eklendi (R={d1})";
                    break;

                case 4: // Hexagon
                    AddRegularPolygon(cx, cy, d1, 6, angleRad, sc);
                    lblStatus.Text = $"Düzgün Altıgen eklendi (R={d1})";
                    break;
            }
        }

        private void AddRegularPolygon(float cx, float cy, float radius, int sides, float angleRad, Color color)
        {
            PointF[] vertices = new PointF[sides];
            for (int i = 0; i < sides; i++)
            {
                float th = angleRad + i * (2f * (float)Math.PI / sides);
                vertices[i] = new PointF(cx + radius * (float)Math.Cos(th), cy + radius * (float)Math.Sin(th));
            }
            AddParametricPolygon(vertices, color);
        }

        private void AddParametricPolygon(PointF[] vertices, Color color)
        {
            int n = vertices.Length;
            for (int i = 0; i < n; i++)
            {
                PointF p1 = vertices[i];
                PointF p2 = vertices[(i + 1) % n];

                if (Math.Abs(p2.X - p1.X) < 1e-4f)
                {
                    AddVerticalLine(p1.X, Math.Min(p1.Y, p2.Y), Math.Max(p1.Y, p2.Y), color);
                }
                else
                {
                    float m = (p2.Y - p1.Y) / (p2.X - p1.X);
                    float b = p1.Y - m * p1.X;
                    string eq = $"{m:F4}*x + {b:F4}";
                    AddFunctionCurve(eq, true, Math.Min(p1.X, p2.X), Math.Max(p1.X, p2.X), color);
                }
            }
        }

        private PointF RotatePoint(PointF pt, float angleRad, float cx, float cy)
        {
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            return new PointF(
                cx + (pt.X * cos - pt.Y * sin),
                cy + (pt.X * sin + pt.Y * cos)
            );
        }

        private float ParseFloat(string s, float fallback)
        {
            if (string.IsNullOrWhiteSpace(s)) return fallback;
            if (float.TryParse(s.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out float val))
                return val;
            return fallback;
        }

        #endregion
    }
}
