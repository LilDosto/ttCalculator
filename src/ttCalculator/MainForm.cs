using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MathApp.Controls;

namespace MathApp
{
    public partial class MainForm : Form
    {
        private SimpleCalculatorControl calcControl;
        private RootFinderControl rootControl;
        private SystemSolverControl systemControl;
        private MatrixCalculatorControl matrixControl;
        private DerivativeCalculatorControl derivativeControl;
        private TaylorSeriesControl taylorControl;
        private MultivariableTaylorControl multiTaylorControl;
        private CombinatoricsControl combinatoricsControl;
        private GraphPlotterControl graphControl;
        private OdeSolverControl odeControl;
        private OdeSystemSolverControl odeSystemControl;
        private PdeSolverControl pdeControl;
        private PdeSystemSolverControl pdeSystemControl;

        private UserControl[] allControls;
        private Button[] allNavButtons;
        private Button activeNavButton;

        public static MainForm Instance { get; private set; }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // WS_EX_COMPOSITED (0x02000000) paints all descendants in bottom-to-top order using double-buffering.
                // Eliminates window tearing and redraw flicker completely.
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public MainForm()
        {
            Instance = this;
            InitializeComponent();
            ThemeHelper.EnableDoubleBuffering(this);
            LoadApplicationIcon();

            ThemeHelper.ApplyTheme(this);
            InitializeControls();
            SetupSearchFilter();
            SetupHotkeys();

            // Default View
            ShowControl(calcControl);
            HighlightButton(btnNavCalc);

            btnNavCalc.Click += (s, e) => { ShowControl(calcControl); HighlightButton(btnNavCalc); };
            btnNavRoot.Click += (s, e) => { ShowControl(rootControl); HighlightButton(btnNavRoot); };
            btnNavSystem.Click += (s, e) => { ShowControl(systemControl); HighlightButton(btnNavSystem); };
            btnNavMatrix.Click += (s, e) => { ShowControl(matrixControl); HighlightButton(btnNavMatrix); };
            btnNavDerivative.Click += (s, e) => { ShowControl(derivativeControl); HighlightButton(btnNavDerivative); };
            btnNavTaylor.Click += (s, e) => { ShowControl(taylorControl); HighlightButton(btnNavTaylor); };
            btnNavMultiTaylor.Click += (s, e) => { ShowControl(multiTaylorControl); HighlightButton(btnNavMultiTaylor); };
            btnNavCombinatorics.Click += (s, e) => { ShowControl(combinatoricsControl); HighlightButton(btnNavCombinatorics); };
            btnNavGraph.Click += (s, e) => { ShowControl(graphControl); HighlightButton(btnNavGraph); };

            btnNavOde.Click += (s, e) => { ShowControl(odeControl); HighlightButton(btnNavOde); };
            btnNavOdeSystem.Click += (s, e) => { ShowControl(odeSystemControl); HighlightButton(btnNavOdeSystem); };
            btnNavPde.Click += (s, e) => { ShowControl(pdeControl); HighlightButton(btnNavPde); };
            btnNavPdeSystem.Click += (s, e) => { ShowControl(pdeSystemControl); HighlightButton(btnNavPdeSystem); };
        }

        private void LoadApplicationIcon()
        {
            try
            {
                string icoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.ico");
                if (File.Exists(icoPath))
                {
                    this.Icon = new Icon(icoPath);
                }
                else
                {
                    this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
            }
            catch { }
        }

        private void InitializeControls()
        {
            calcControl = new SimpleCalculatorControl { Dock = DockStyle.Fill };
            rootControl = new RootFinderControl { Dock = DockStyle.Fill };
            systemControl = new SystemSolverControl { Dock = DockStyle.Fill };
            matrixControl = new MatrixCalculatorControl { Dock = DockStyle.Fill };
            derivativeControl = new DerivativeCalculatorControl { Dock = DockStyle.Fill };
            taylorControl = new TaylorSeriesControl { Dock = DockStyle.Fill };
            multiTaylorControl = new MultivariableTaylorControl { Dock = DockStyle.Fill };
            combinatoricsControl = new CombinatoricsControl { Dock = DockStyle.Fill };
            graphControl = new GraphPlotterControl { Dock = DockStyle.Fill };
            odeControl = new OdeSolverControl { Dock = DockStyle.Fill };
            odeSystemControl = new OdeSystemSolverControl { Dock = DockStyle.Fill };
            pdeControl = new PdeSolverControl { Dock = DockStyle.Fill };
            pdeSystemControl = new PdeSystemSolverControl { Dock = DockStyle.Fill };

            allControls = new UserControl[] {
                calcControl,
                rootControl,
                systemControl,
                matrixControl,
                derivativeControl,
                taylorControl,
                multiTaylorControl,
                combinatoricsControl,
                graphControl,
                odeControl,
                odeSystemControl,
                pdeControl,
                pdeSystemControl
            };

            allNavButtons = new Button[] {
                btnNavCalc,
                btnNavGraph,
                btnNavCombinatorics,
                btnNavRoot,
                btnNavSystem,
                btnNavDerivative,
                btnNavMatrix,
                btnNavTaylor,
                btnNavMultiTaylor,
                btnNavOde,
                btnNavOdeSystem,
                btnNavPde,
                btnNavPdeSystem
            };

            // Custom paint active indicator bar on navigation buttons
            foreach (var btn in allNavButtons)
            {
                btn.Paint += (s, pe) =>
                {
                    if (s == activeNavButton)
                    {
                        using (Brush barBrush = new SolidBrush(ThemeHelper.AccentBlue))
                        {
                            pe.Graphics.FillRectangle(barBrush, 0, 4, 4, btn.Height - 8);
                        }
                    }
                };
            }

            pnlContent.SuspendLayout();
            pnlContent.Controls.AddRange(allControls);
            foreach (var ctrl in allControls)
            {
                ctrl.Visible = false;
                ThemeHelper.EnableDoubleBuffering(ctrl);
            }
            pnlContent.ResumeLayout(false);
        }

        private void SetupSearchFilter()
        {
            string placeholder = "🔍 Modül ara (Ctrl+F)...";
            txtSearchNav.GotFocus += (s, e) =>
            {
                if (txtSearchNav.Text == placeholder)
                {
                    txtSearchNav.Text = "";
                    txtSearchNav.ForeColor = Color.White;
                }
            };

            txtSearchNav.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchNav.Text))
                {
                    txtSearchNav.Text = placeholder;
                    txtSearchNav.ForeColor = Color.FromArgb(148, 163, 184);
                }
            };

            txtSearchNav.TextChanged += (s, e) =>
            {
                string query = txtSearchNav.Text.Trim();
                if (query == placeholder) return;

                bool empty = string.IsNullOrWhiteSpace(query);
                foreach (var btn in allNavButtons)
                {
                    if (empty)
                    {
                        btn.Visible = true;
                    }
                    else
                    {
                        btn.Visible = btn.Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                }
            };

            txtSearchNav.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    var firstMatch = allNavButtons.FirstOrDefault(b => b.Visible);
                    firstMatch?.PerformClick();
                }
            };
        }

        private void SetupHotkeys()
        {
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.F)
                {
                    txtSearchNav.Focus();
                    txtSearchNav.SelectAll();
                    e.SuppressKeyPress = true;
                }
                else if (e.Control && e.KeyCode == Keys.D1) { btnNavCalc.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D2) { btnNavGraph.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D3) { btnNavCombinatorics.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D4) { btnNavRoot.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D5) { btnNavSystem.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D6) { btnNavDerivative.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D7) { btnNavMatrix.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D8) { btnNavTaylor.PerformClick(); e.SuppressKeyPress = true; }
                else if (e.Control && e.KeyCode == Keys.D9) { btnNavOde.PerformClick(); e.SuppressKeyPress = true; }
            };
        }

        public void NavigateToGraph(string equation, float? pointX = null, float? pointY = null)
        {
            if (!string.IsNullOrWhiteSpace(equation))
            {
                graphControl.AddFunctionCurve(equation, false, -10f, 10f);
            }
            if (pointX.HasValue && pointY.HasValue)
            {
                graphControl.AddMarkedPoint(pointX.Value, pointY.Value, "Kök", Color.FromArgb(16, 185, 129));
            }
            ShowControl(graphControl);
            HighlightButton(btnNavGraph);
        }

        private void ShowControl(UserControl control)
        {
            if (control == null || allControls == null) return;

            pnlContent.SuspendLayout();
            foreach (var ctrl in allControls)
            {
                if (ctrl != control && ctrl.Visible)
                {
                    ctrl.Visible = false;
                }
            }

            if (!control.Visible)
            {
                control.Visible = true;
            }
            control.BringToFront();
            pnlContent.ResumeLayout(true);
        }

        private void HighlightButton(Button activeBtn)
        {
            activeNavButton = activeBtn;
            Color activeBg = Color.FromArgb(24, 33, 52);
            Color idleBg = ThemeHelper.SidebarBackground;

            foreach (var btn in allNavButtons)
            {
                if (btn == null) continue;
                if (btn == activeBtn)
                {
                    btn.BackColor = activeBg;
                    btn.ForeColor = Color.FromArgb(56, 189, 248);
                    btn.Font = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold);
                }
                else
                {
                    btn.BackColor = idleBg;
                    btn.ForeColor = Color.FromArgb(203, 213, 225);
                    btn.Font = new Font("Segoe UI", 9.2F, FontStyle.Regular);
                }
                btn.Invalidate();
            }
        }
    }
}
