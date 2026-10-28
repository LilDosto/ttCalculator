using System;
using System.Drawing;
using System.Globalization;
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
            UpdateAngleButtonState();
            btnAngleMode.Click += BtnAngleMode_Click;
            btnClearHistory.Click += (s, e) => lstHistory.Items.Clear();
            lstHistory.DoubleClick += LstHistory_DoubleClick;
        }

        private void BtnAngleMode_Click(object sender, EventArgs e)
        {
            Function.UseRadians = !Function.UseRadians;
            UpdateAngleButtonState();
        }

        private void UpdateAngleButtonState()
        {
            if (Function.UseRadians)
            {
                btnAngleMode.Text = "Açı Birimi: RAD (Radyan)";
                btnAngleMode.BackColor = Color.FromArgb(14, 165, 233);
            }
            else
            {
                btnAngleMode.Text = "Açı Birimi: DEG (Derece)";
                btnAngleMode.BackColor = Color.FromArgb(99, 102, 241);
            }
        }

        private void LstHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lstHistory.SelectedItem != null)
            {
                string item = lstHistory.SelectedItem.ToString();
                int eqIdx = item.LastIndexOf('=');
                if (eqIdx != -1 && eqIdx + 1 < item.Length)
                {
                    string val = item.Substring(eqIdx + 1).Trim();
                    currentExpression = val;
                    txtDisplay.Text = val;
                    lblExpressionHistory.Text = item.Substring(0, eqIdx).Trim() + " =";
                    isResultShown = true;
                }
            }
        }

        private void InitializeButtons()
        {
            pnlButtons.Controls.Clear();
            pnlButtons.ColumnCount = 6;
            pnlButtons.RowCount = 7;
            pnlButtons.ColumnStyles.Clear();
            pnlButtons.RowStyles.Clear();

            for (int c = 0; c < 6; c++)
                pnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6f));
            for (int r = 0; r < 7; r++)
                pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 7f));

            string[] layout = {
                // Row 0: Trig & Inverse Trig
                "sin(",  "cos(",  "tan(",  "cot(",  "asin(", "acos(",
                // Row 1: Hyperbolic, Inverse Tan & Logs
                "sinh(", "cosh(", "tanh(", "atan(", "ln(",   "log(",
                // Row 2: Powers, Exp, Grouping
                "sqrt(", "^",     "exp(",  "abs(",  "(",     ")",
                // Row 3: Constants & Symbols
                "pi",    "e",     "%",     "DEL",   "C",     "/",
                // Row 4: Digits 7,8,9, Ops
                "7",     "8",     "9",     "*",     "-",     "+",
                // Row 5: Digits 4,5,6
                "4",     "5",     "6",     "0",     ".",     "=",
                // Row 6: Digits 1,2,3
                "1",     "2",     "3",     "x",     "y",     "z"
            };

            // Let's arrange so the standard keypad is natural:
            // Let's reorder rows 4, 5, 6:
            // Row 3: pi, e, %, (, ), /
            // Row 4: 7, 8, 9, *, DEL, C
            // Row 5: 4, 5, 6, -, +, x
            // Row 6: 1, 2, 3, 0, ., =
            string[] standardKeypad = {
                // Row 0: Trig
                "sin(",  "cos(",  "tan(",  "cot(",  "asin(", "acos(",
                // Row 1: Hyperbolic & Logs
                "sinh(", "cosh(", "tanh(", "atan(", "ln(",   "log(",
                // Row 2: Roots, Powers & Constants
                "sqrt(", "^",     "exp(",  "abs(",  "pi",    "e",
                // Row 3: Grouping & Ops
                "(",     ")",     "%",     "DEL",   "C",     "/",
                // Row 4: Keypad Row 1
                "7",     "8",     "9",     "*",     "-",     "+",
                // Row 5: Keypad Row 2
                "4",     "5",     "6",     "x",     "y",     "z",
                // Row 6: Keypad Row 3 & Action
                "1",     "2",     "3",     "0",     ".",     "="
            };

            int index = 0;
            foreach (string text in standardKeypad)
            {
                Button btn = new Button
                {
                    Text = FormatButtonLabel(text),
                    Tag = text,
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(3),
                    Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
                btn.Click += Btn_Click;

                if (text == "=")
                {
                    btn.BackColor = ThemeHelper.AccentBlue;
                    btn.FlatAppearance.MouseOverBackColor = ThemeHelper.AccentBlueHover;
                    btn.FlatAppearance.BorderColor = ThemeHelper.AccentBlueHover;
                }
                else if (text == "C" || text == "DEL")
                {
                    btn.BackColor = Color.FromArgb(127, 29, 29);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
                    btn.FlatAppearance.BorderColor = Color.FromArgb(153, 27, 27);
                }
                else if (IsOperator(text) || text == "^")
                {
                    btn.BackColor = Color.FromArgb(51, 65, 85);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(71, 85, 105);
                }
                else if (text.EndsWith("(") || text == "pi" || text == "e" || text == "(" || text == ")")
                {
                    btn.BackColor = Color.FromArgb(30, 41, 59);
                    btn.ForeColor = Color.FromArgb(186, 230, 253);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 65, 85);
                }
                else if (text == "x" || text == "y" || text == "z")
                {
                    btn.BackColor = Color.FromArgb(24, 38, 55);
                    btn.ForeColor = Color.FromArgb(250, 204, 21);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 58, 80);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(15, 23, 42);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
                }

                pnlButtons.Controls.Add(btn, index % 6, index / 6);
                index++;
            }
        }

        private string FormatButtonLabel(string token)
        {
            return token switch
            {
                "sin(" => "sin",
                "cos(" => "cos",
                "tan(" => "tan",
                "cot(" => "cot",
                "asin(" => "sin⁻¹",
                "acos(" => "cos⁻¹",
                "atan(" => "tan⁻¹",
                "sinh(" => "sinh",
                "cosh(" => "cosh",
                "tanh(" => "tanh",
                "exp(" => "eˣ",
                "^" => "xʸ",
                "ln(" => "ln",
                "log(" => "log₁₀",
                "sqrt(" => "√x",
                "abs(" => "|x|",
                "pi" => "π",
                "DEL" => "⌫ Sil",
                "C" => "AC",
                "*" => "×",
                "/" => "÷",
                _ => token
            };
        }

        private bool IsOperator(string text)
        {
            return text == "+" || text == "-" || text == "*" || text == "/" || text == "^" || text == "%";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
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
                case Keys.Oemplus:
                    input = "+"; break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    input = "-"; break;
                case Keys.Multiply:
                    input = "*"; break;
                case Keys.Divide:
                case Keys.OemQuestion:
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
                    input = "="; break;

                default:
                    if (keyData == (Keys.Shift | Keys.D8)) input = "(";
                    else if (keyData == (Keys.Shift | Keys.D9)) input = ")";
                    else if (keyData == (Keys.Shift | Keys.D0)) input = "=";
                    else if (keyData == (Keys.Shift | Keys.D3)) input = "^";
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
            if (sender is Button btn && btn.Tag is string token)
            {
                ProcessInput(token);
            }
        }

        private void ProcessInput(string text)
        {
            if (text == "C")
            {
                currentExpression = "";
                txtDisplay.Text = "0";
                lblExpressionHistory.Text = "İfade giriniz veya klavyeyi kullanınız";
                isResultShown = false;
            }
            else if (text == "DEL")
            {
                if (isResultShown)
                {
                    currentExpression = "";
                    txtDisplay.Text = "0";
                    lblExpressionHistory.Text = "";
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
                        isResultShown = false;
                    }
                    else
                    {
                        currentExpression = "";
                        isResultShown = false;
                    }
                }
                currentExpression += text;
                txtDisplay.Text = currentExpression;
            }
        }

        private void Evaluate()
        {
            if (string.IsNullOrWhiteSpace(currentExpression)) return;

            try
            {
                string exprToEval = BalanceParentheses(currentExpression);
                Function f = new Function();
                f.Parse(exprToEval);
                if (f.Error)
                {
                    lblExpressionHistory.Text = "Hata: " + f.ErrorDescription;
                    txtDisplay.Text = "Hata";
                    isResultShown = true;
                    return;
                }

                f.Infix2Postfix();
                f.EvaluatePostfix();

                if (f.Error)
                {
                    lblExpressionHistory.Text = "Hata: " + f.ErrorDescription;
                    txtDisplay.Text = "Tanımsız";
                    isResultShown = true;
                    return;
                }

                decimal result = Math.Round(f.Result, 14);
                string resStr = result.ToString("0.##############", CultureInfo.InvariantCulture);

                string historyEntry = $"{exprToEval} = {resStr}";
                lstHistory.Items.Insert(0, historyEntry);

                lblExpressionHistory.Text = exprToEval + " =";
                txtDisplay.Text = resStr;
                currentExpression = resStr;
                isResultShown = true;
            }
            catch (Exception ex)
            {
                lblExpressionHistory.Text = "Hesaplama Hatası: " + ex.Message;
                txtDisplay.Text = "Hata";
                currentExpression = "";
                isResultShown = true;
            }
        }

        private string BalanceParentheses(string expr)
        {
            int open = 0;
            foreach (char c in expr)
            {
                if (c == '(') open++;
                else if (c == ')') open--;
            }
            if (open > 0)
            {
                return expr + new string(')', open);
            }
            return expr;
        }
    }
}
