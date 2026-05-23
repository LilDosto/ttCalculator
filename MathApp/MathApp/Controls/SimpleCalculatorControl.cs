using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    public partial class SimpleCalculatorControl : UserControl
    {
        private string currentExpression = "";
        private bool isResultShown = false;

        public SimpleCalculatorControl()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
             // ... (Keeping initialization logic same, but for brevity I will rewrite it to ensure file integrity)
             // Actually, I should preserve the exact logic.
             // Re-implementing InitializeButtons to Match previous content exactly
            
            this.pnlButtons.RowCount = 5;
            this.pnlButtons.RowStyles.Clear();
            for(int i=0; i<5; i++) this.pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            string[] gridLayout = {
                "C", "(", ")", "/",
                "7", "8", "9", "*",
                "4", "5", "6", "-",
                "1", "2", "3", "+",
                "0", ".", "DEL", "="
            };

            int index = 0;
            foreach (string text in gridLayout)
            {
                Button btn = new Button();
                btn.Text = text;
                btn.Dock = DockStyle.Fill;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
                btn.ForeColor = Color.White;
                btn.Click += Btn_Click;
                
                // Tag lookup for keyboard maybe?
                btn.Tag = text;

                if (IsOperator(text))
                {
                    btn.BackColor = Color.FromArgb(40, 40, 40);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
                }
                else if (text == "=")
                {
                    btn.BackColor = Color.FromArgb(0, 120, 215);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 140, 240);
                }
                else if (text == "C" || text == "DEL")
                {
                     btn.BackColor = Color.FromArgb(50, 50, 50);
                     btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(30, 30, 30);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 50, 50);
                }

                pnlButtons.Controls.Add(btn, index % 4, index / 4);
                index++;
            }
        }

        private bool IsOperator(string text)
        {
            return text == "+" || text == "-" || text == "*" || text == "/";
        }
        
        // --- Keyboard Support ---
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Map keys
            string input = "";
            bool handled = true;

            switch (keyData)
            {
                case Keys.NumPad0:
                case Keys.D0: input = "0"; break;
                case Keys.NumPad1:
                case Keys.D1: input = "1"; break;
                case Keys.NumPad2:
                case Keys.D2: input = "2"; break;
                case Keys.NumPad3:
                case Keys.D3: input = "3"; break;
                case Keys.NumPad4:
                case Keys.D4: input = "4"; break;
                case Keys.NumPad5:
                case Keys.D5: input = "5"; break;
                case Keys.NumPad6:
                case Keys.D6: input = "6"; break;
                case Keys.NumPad7:
                case Keys.D7: input = "7"; break;
                case Keys.NumPad8:
                case Keys.D8: input = "8"; break;
                case Keys.NumPad9:
                case Keys.D9: input = "9"; break;
                
                case Keys.Add:
                case Keys.Oemplus: // Careful with Shift+=
                    input = "+"; break; 
                case Keys.Subtract:
                case Keys.OemMinus:
                    input = "-"; break;
                case Keys.Multiply:
                    input = "*"; break;
                case Keys.Divide:
                case Keys.OemQuestion: // /
                    input = "/"; break;
                
                case Keys.Decimal:
                case Keys.OemPeriod:
                case Keys.Oemcomma:
                    input = "."; break;
                
                case Keys.Back:
                case Keys.Delete:
                    input = "DEL"; break;
                    
                case Keys.Escape:
                    input = "C"; break;

                case Keys.Enter:
                case Keys.Return:
                    input = "="; break;
                
                // Shift+8 (*) etc are handled by KeyChar usually vs Keys, 
                // but ProcessCmdKey sees Keys.
                // For simplicity let's stick to Numpad and basics.
                // Shift handling in ProcessCmdKey is tricky (keyData & Keys.Shift)
                default:
                    // Check for shift combinations
                    if (keyData == (Keys.Shift | Keys.D8)) input = "*";
                    else if (keyData == (Keys.Shift | Keys.D9)) input = "(";
                    else if (keyData == (Keys.Shift | Keys.D0)) input = ")";
                    else if (keyData == (Keys.Shift | Keys.Oemplus)) input = "+"; 
                    else handled = false;
                    break;
            }

            if (handled && input != "")
            {
                ProcessInput(input);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            ProcessInput(btn.Text);
            
            // Remove focus from button so future key presses go to control/form?
            // Actually ProcessCmdKey bubbles up so it's fine.
            // But if button has focus, Enter triggers Click AND ProcessCmdKey might double trigger
            // if we are not careful. 
            // ProcessCmdKey is called BEFORE key events. returning true eats it.
            // So Enter won't click the focused button if we return true.
        }

        private void ProcessInput(string text)
        {
            if (text == "C")
            {
                currentExpression = "";
                txtDisplay.Text = "0";
                isResultShown = false;
            }
            else if (text == "DEL")
            {
                if (isResultShown)
                {
                    currentExpression = "";
                    txtDisplay.Text = "0";
                    isResultShown = false;
                }
                else if (currentExpression.Length > 0)
                {
                    currentExpression = currentExpression.Substring(0, currentExpression.Length - 1);
                    txtDisplay.Text = currentExpression.Length > 0 ? currentExpression : "0";
                }
            }
            else if (text == "=")
            {
                Evaluate();
            }
            else
            {
                if (isResultShown)
                {
                    if (IsOperator(text))
                    {
                        isResultShown = false; // Continue with result
                    }
                    else
                    {
                        currentExpression = ""; // Start new
                        isResultShown = false;
                    }
                }
                currentExpression += text;
                txtDisplay.Text = currentExpression;
            }
        }

        private void Evaluate()
        {
            try
            {
                Function f = new Function();
                f.Parse(currentExpression);
                f.Infix2Postfix();
                f.EvaluatePostfix();
                
                decimal result = f.m_result;
                string resStr = result.ToString("G29", System.Globalization.CultureInfo.InvariantCulture);
                
                txtDisplay.Text = resStr;
                currentExpression = resStr;
                isResultShown = true;
            }
            catch (Exception)
            {
                txtDisplay.Text = "Error";
                currentExpression = "";
                isResultShown = true; // allow typing to clear
            }
        }
        
        private bool IsOperator(string text) // Redundant but kept for safety if used elsewhere
        { 
             return text == "+" || text == "-" || text == "*" || text == "/"; 
        }
    }
}
