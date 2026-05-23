using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class RootFinderControl : UserControl
    {
        public RootFinderControl()
        {
            InitializeComponent();
            cmbMethod.SelectedIndex = 0;
            UpdateVisibility();
            cmbMethod.SelectedIndexChanged += (s, e) => UpdateVisibility();
            btnFindRoot.Click += BtnFindRoot_Click;
        }

        private void UpdateVisibility()
        {
            string method = cmbMethod.SelectedItem?.ToString() ?? "";
            bool isBracket = method == "Bisection" || method == "Regula Falsi";
            
            lblInitialPoint.Visible = !isBracket;
            txtInitialPoint.Visible = !isBracket;
            
            lblMinX.Visible = isBracket;
            txtMinX.Visible = isBracket;
            lblMaxX.Visible = isBracket;
            txtMaxX.Visible = isBracket;
        }

        private void BtnFindRoot_Click(object sender, EventArgs e)
        {
            try
            {
                string eq = txtEquation.Text;
                string variable = txtVariable.Text;
                int iterCount = 100;
                int.TryParse(txtIterations.Text, out iterCount);
                string method = cmbMethod.SelectedItem.ToString();
                
                List<string> logs = new List<string>();

                if (method == "Newton-Raphson")
                {
                    decimal initialP = decimal.Parse(txtInitialPoint.Text);
                    logs = NumericalMethods.Method_NewtonRaphson(eq, initialP, iterCount);
                }
                else if (method == "Bisection")
                {
                    decimal minX = decimal.Parse(txtMinX.Text);
                    decimal maxX = decimal.Parse(txtMaxX.Text);
                    logs = NumericalMethods.Method_Bisection(eq, minX, maxX, iterCount);
                }
                else if (method == "Secant")
                {
                    decimal minX = decimal.Parse(txtMinX.Text);
                    decimal maxX = decimal.Parse(txtMaxX.Text); 
                    logs = NumericalMethods.Method_Secant(eq, minX, maxX, iterCount);
                }
                else if (method == "Regula Falsi")
                {
                     decimal minX = decimal.Parse(txtMinX.Text);
                    decimal maxX = decimal.Parse(txtMaxX.Text);
                    logs = NumericalMethods.Method_RegulaFalsi(eq, minX, maxX, iterCount);
                }
                else
                {
                     logs = new List<string> { method + " method not yet active." };
                }

                txtResult.Text = string.Join(Environment.NewLine, logs);
            }
            catch (Exception ex)
            {
                txtResult.Text = "Error: " + ex.Message;
            }
        }
    }
}
