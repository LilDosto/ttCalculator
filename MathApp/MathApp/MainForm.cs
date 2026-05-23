using System;
using System.Drawing;
using System.Windows.Forms;
using MathApp.Controls;

namespace MathApp
{
    public partial class MainForm : Form
    {
        private SimpleCalculatorControl calcControl;
        private RootFinderControl rootControl;
        private SystemSolverControl systemControl;
        private TaylorSeriesControl taylorControl;
        private MultivariableTaylorControl multiTaylorControl;
        private GraphPlotterControl graphControl;

        public MainForm()
        {
            InitializeComponent();
            try { this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
            
            ThemeHelper.ApplyTheme(this);
            InitializeControls();
            
            // Default View
            ShowControl(calcControl);
            HighlightButton(btnNavCalc);

            btnNavCalc.Click += (s, e) => { ShowControl(calcControl); HighlightButton(btnNavCalc); };
            btnNavRoot.Click += (s, e) => { ShowControl(rootControl); HighlightButton(btnNavRoot); };
            btnNavSystem.Click += (s, e) => { ShowControl(systemControl); HighlightButton(btnNavSystem); };
            btnNavTaylor.Click += (s, e) => { ShowControl(taylorControl); HighlightButton(btnNavTaylor); };
            btnNavMultiTaylor.Click += (s, e) => { ShowControl(multiTaylorControl); HighlightButton(btnNavMultiTaylor); };
            btnNavGraph.Click += (s, e) => { ShowControl(graphControl); HighlightButton(btnNavGraph); };
        }

        private void InitializeControls()
        {
            calcControl = new SimpleCalculatorControl();
            calcControl.Dock = DockStyle.Fill;
            
            rootControl = new RootFinderControl();
            rootControl.Dock = DockStyle.Fill;
            
            systemControl = new SystemSolverControl();
            systemControl.Dock = DockStyle.Fill;

            taylorControl = new TaylorSeriesControl();
            taylorControl.Dock = DockStyle.Fill;

            multiTaylorControl = new MultivariableTaylorControl();
            multiTaylorControl.Dock = DockStyle.Fill;

            graphControl = new GraphPlotterControl();
            graphControl.Dock = DockStyle.Fill;
        }

        private void ShowControl(UserControl control)
        {
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(control);
        }

        private void HighlightButton(Button activeBtn)
        {
            Color activeColor = Color.FromArgb(0, 120, 215); // Accent Blue
            Color idleColor = Color.FromArgb(40, 40, 40);   // Sidebar BG

            btnNavCalc.BackColor = idleColor;
            btnNavRoot.BackColor = idleColor;
            btnNavSystem.BackColor = idleColor;
            btnNavTaylor.BackColor = idleColor;
            btnNavMultiTaylor.BackColor = idleColor;
            btnNavGraph.BackColor = idleColor;

            activeBtn.BackColor = activeColor;
        }
    }
}
