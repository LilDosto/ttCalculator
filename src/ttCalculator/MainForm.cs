using System;
using System.Drawing;
using System.IO;
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

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // WS_EX_COMPOSITED (0x02000000) paints all descendants in bottom-to-top order using double-buffering.
                // Eliminates window tearing and redraw flicker.
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            ThemeHelper.EnableDoubleBuffering(this);
            LoadApplicationIcon();

            ThemeHelper.ApplyTheme(this);
            InitializeControls();

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

            pnlContent.SuspendLayout();
            // Pre-add all controls once to avoid Win32 HWND re-creation and flickering on tab switch
            pnlContent.Controls.AddRange(allControls);
            foreach (var ctrl in allControls)
            {
                ctrl.Visible = false;
                ThemeHelper.EnableDoubleBuffering(ctrl);
            }
            pnlContent.ResumeLayout(false);
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
            Color activeColor = ThemeHelper.AccentBlue;
            Color idleColor = ThemeHelper.SidebarBackground;

            Button[] allButtons = {
                btnNavCalc,
                btnNavRoot,
                btnNavSystem,
                btnNavMatrix,
                btnNavDerivative,
                btnNavTaylor,
                btnNavMultiTaylor,
                btnNavCombinatorics,
                btnNavGraph,
                btnNavOde,
                btnNavOdeSystem,
                btnNavPde,
                btnNavPdeSystem
            };

            foreach (var btn in allButtons)
            {
                if (btn == null) continue;
                if (btn == activeBtn)
                {
                    btn.BackColor = activeColor;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font(btn.Font, FontStyle.Bold);
                }
                else
                {
                    btn.BackColor = idleColor;
                    btn.ForeColor = Color.FromArgb(203, 213, 225);
                    btn.Font = new Font(btn.Font, FontStyle.Regular);
                }
            }
        }
    }
}
