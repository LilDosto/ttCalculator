using System;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class TaylorSeriesControl : UserControl
    {
        public TaylorSeriesControl()
        {
            InitializeComponent();
            btnCalculate.Click += BtnCalculate_Click;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string eq = txtFunction.Text;
                string variable = txtVariable.Text;
                int degree = (int)numOrder.Value;
                decimal center = decimal.Parse(txtCenterPoint.Text);

                string result = NumericalMethods.Method_TaylorSeries(eq, degree, variable, center);
                // Clean up result for display if needed
                result = result.Replace("+ -", "- ").Replace("+-", "- ");
                
                txtResult.Text = "Taylor Series approximation for f(x) =" + Environment.NewLine + 
                                 result;
            }
            catch (Exception ex)
            {
                txtResult.Text = "Error: " + ex.Message;
            }
        }
    }
}
