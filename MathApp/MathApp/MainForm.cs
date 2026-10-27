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

        public MainForm()
        {
            InitializeComponent();
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
        }

        private void ShowControl(UserControl control)
        {
            pnlContent.SuspendLayout();
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(control);
            pnlContent.ResumeLayout();
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
                btnNavGraph
            };

            foreach (Button b in allButtons)
            {
                b.BackColor = idleColor;
                b.ForeColor = ThemeHelper.MutedText;
                b.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }

            activeBtn.BackColor = activeColor;
            activeBtn.ForeColor = Color.White;
            activeBtn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Environment.Exit(0);
        }
    }
}
