using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class SystemSolverControl : UserControl
    {
        private List<TextBox> equationsInputs = new List<TextBox>();
        private List<TextBox> variablesInputs = new List<TextBox>();
        private List<TextBox> initialPointsInputs = new List<TextBox>();

        public SystemSolverControl()
        {
            InitializeComponent();
            btnGenerate.Click += BtnGenerate_Click;
            btnSolveSystem.Click += BtnSolveSystem_Click;
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            pnlEquations.Controls.Clear();
            equationsInputs.Clear();
            variablesInputs.Clear();
            initialPointsInputs.Clear();

            int count = (int)numEqCount.Value;

            for (int i = 0; i < count; i++)
            {
                Panel p = new Panel();
                p.Size = new Size(720, 30);
                p.BackColor = Color.FromArgb(30,30,30);
                
                Label lblEq = new Label { Text = "Eq " + (i+1), Location = new Point(0, 5), AutoSize = true, ForeColor = Color.White };
                TextBox txtEq = new TextBox { Location = new Point(50, 3), Size = new Size(300, 23), BackColor = Color.FromArgb(40,40,40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
                
                Label lblVar = new Label { Text = "Var", Location = new Point(360, 5), AutoSize = true, ForeColor = Color.White };
                TextBox txtVar = new TextBox { Location = new Point(390, 3), Size = new Size(50, 23), BackColor = Color.FromArgb(40,40,40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
                
                Label lblInit = new Label { Text = "Init", Location = new Point(450, 5), AutoSize = true, ForeColor = Color.White };
                TextBox txtInit = new TextBox { Location = new Point(480, 3), Size = new Size(50, 23), Text="0", BackColor = Color.FromArgb(40,40,40), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

                p.Controls.Add(lblEq);
                p.Controls.Add(txtEq);
                p.Controls.Add(lblVar);
                p.Controls.Add(txtVar);
                p.Controls.Add(lblInit);
                p.Controls.Add(txtInit);

                pnlEquations.Controls.Add(p);
                
                equationsInputs.Add(txtEq);
                variablesInputs.Add(txtVar);
                initialPointsInputs.Add(txtInit);
            }
        }

        private void BtnSolveSystem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> eqs = new List<string>();
                Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
                
                for(int i=0; i<equationsInputs.Count; i++)
                {
                    eqs.Add(equationsInputs[i].Text);
                    
                    decimal val = 0;
                    if (decimal.TryParse(initialPointsInputs[i].Text, out val)) { }
                    
                    string varName = variablesInputs[i].Text.Trim();
                    if(string.IsNullOrEmpty(varName)) continue;

                    if(dict.ContainsKey(varName))
                    {
                        txtSystemResult.Text = "Error: Duplicate variable '" + varName + "'.";
                        return;
                    }
                    dict.Add(varName, val);
                }

                int iterCount = 100;
                int.TryParse(txtSystemIterations.Text, out iterCount);

                Dictionary<string, decimal> results = NumericalMethods.MethodMultiVar_SolveEquationSystem(eqs, dict, iterCount);
                
                if (results != null)
                {
                    List<string> resLines = new List<string>();
                    foreach(var kvp in results)
                    {
                        resLines.Add(kvp.Key + ": " + kvp.Value.ToString("F8"));
                    }
                    txtSystemResult.Text = string.Join(Environment.NewLine, resLines);
                }
                else
                    txtSystemResult.Text = "Failed to solve.";
            }
            catch (Exception ex)
            {
                txtSystemResult.Text = "Error: " + ex.Message;
            }
        }
    }
}
