using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace MathApp.Controls
{
    public partial class MultivariableTaylorControl : UserControl
    {
        private List<VariableInputRow> variableRows = new List<VariableInputRow>();

        public MultivariableTaylorControl()
        {
            InitializeComponent();
            btnAddVar.Click += BtnAddVar_Click;
            btnCalculate.Click += BtnCalculate_Click;
            
            // Add initial variables inputs (x and y default)
            AddVariableRow("x", "0");
            AddVariableRow("y", "0");
        }

        private void BtnAddVar_Click(object sender, EventArgs e)
        {
            AddVariableRow("", "0");
        }

        private void AddVariableRow(string varName, string val)
        {
            VariableInputRow row = new VariableInputRow(varName, val);
            row.OnRemove += (s, e) => 
            {
                pnlVariables.Controls.Remove(row.Panel);
                variableRows.Remove(row);
            };
            
            pnlVariables.Controls.Add(row.Panel);
            variableRows.Add(row);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string eq = txtEquation.Text;
                int degree = (int)numOrder.Value;
                Dictionary<string, decimal> points = new Dictionary<string, decimal>();

                foreach(var row in variableRows)
                {
                    string v = row.Variable;
                    if(string.IsNullOrWhiteSpace(v)) continue;
                    
                    decimal p = 0;
                    if(!decimal.TryParse(row.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out p))
                    {
                        txtResult.Text = "Invalid number format for variable: " + v;
                        return;
                    }
                    
                    if(points.ContainsKey(v))
                    {
                        txtResult.Text = "Duplicate variable: " + v;
                        return;
                    }
                    points.Add(v, p);
                }

                if(points.Count == 0)
                {
                    txtResult.Text = "Please add at least one variable.";
                    return;
                }

                string result = NumericalMethods.MethodMultiVar_TaylorSeries(eq, degree, points);
                
                // Formatting clean up
                result = result.Replace("+ -", "- ").Replace("+-", "- ").Replace("*", " * ");
                
                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                txtResult.Text = "Error: " + ex.Message;
            }
        }
    }

    // Helper class for dynamic rows
    public class VariableInputRow
    {
        public Panel Panel { get; private set; }
        public TextBox TxtVariable { get; private set; }
        public TextBox TxtValue { get; private set; }
        public EventHandler OnRemove;

        public string Variable => TxtVariable.Text.Trim();
        public string Value => TxtValue.Text.Trim();

        public VariableInputRow(string varName, string val)
        {
            Panel = new Panel();
            Panel.Size = new Size(500, 30);
            
            Label lblVar = new Label { Text = "Var:", Location = new Point(0, 5), AutoSize = true, ForeColor = Color.White };
            TxtVariable = new TextBox { Text = varName, Location = new Point(40, 2), Size = new Size(100, 23), BackColor = Color.FromArgb(40,40,40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            
            Label lblVal = new Label { Text = "Point:", Location = new Point(160, 5), AutoSize = true, ForeColor = Color.White };
            TxtValue = new TextBox { Text = val, Location = new Point(210, 2), Size = new Size(100, 23), BackColor = Color.FromArgb(40,40,40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            
            Button btnRemove = new Button { Text = "X", Location = new Point(330, 2), Size = new Size(30, 23), BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Click += (s, e) => OnRemove?.Invoke(this, EventArgs.Empty);

            Panel.Controls.Add(lblVar);
            Panel.Controls.Add(TxtVariable);
            Panel.Controls.Add(lblVal);
            Panel.Controls.Add(TxtValue);
            Panel.Controls.Add(btnRemove);
        }
    }
}
