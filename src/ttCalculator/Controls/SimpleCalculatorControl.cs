using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MathApp.Core;

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
            switch (Function.CurrentAngleUnit)
            {
                case AngleUnit.Radian:
                    Function.CurrentAngleUnit = AngleUnit.Degree;
                    break;
                case AngleUnit.Degree:
                    Function.CurrentAngleUnit = AngleUnit.Gradian;
                    break;
                case AngleUnit.Gradian:
                default:
                    Function.CurrentAngleUnit = AngleUnit.Radian;
                    break;
            }
            UpdateAngleButtonState();
        }

        private void UpdateAngleButtonState()
        {
            switch (Function.CurrentAngleUnit)
            {
                case AngleUnit.Radian:
                    btnAngleMode.Text = "Açı Birimi: RAD (Radyan)";
                    btnAngleMode.BackColor = Color.FromArgb(14, 165, 233);
                    break;
                case AngleUnit.Degree:
                    btnAngleMode.Text = "Açı Birimi: DEG (Derece)";
                    btnAngleMode.BackColor = Color.FromArgb(99, 102, 241);
                    break;
                case AngleUnit.Gradian:
                    btnAngleMode.Text = "Açı Birimi: GRAD (Gradian)";
                    btnAngleMode.BackColor = Color.FromArgb(16, 185, 129);
                    break;
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

            string[] standardKeypad = {
                // Row 0: Trig
                "sin(",  "cos(",  "tan(",  "cot(",  "asin(", "acos(",
                // Row 1: Hyperbolic & Logs
                "sinh(", "cosh(", "tanh(", "atan(", "ln(",   "log(",
                // Row 2: Roots, Powers & Constants
                "sqrt(", "^",     "exp(",  "abs(",  "pi",    "e",
                // Row 3: Advanced, Grouping, Clear
                "!",     "(",     ")",     "%",     "DEL",   "C",
                // Row 4: Keypad Row 1
                "7",     "8",     "9",     "/",     "*",     "-",
                // Row 5: Keypad Row 2
                "4",     "5",     "6",     "+",     "x",     "y",
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
                else if (IsOperator(text) || text == "^" || text == "!")
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
                "!" => "n!",
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
                    if (keyData == (Keys.Shift | Keys.D1)) input = "!";
                    else if (keyData == (Keys.Shift | Keys.D8)) input = "(";
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

                currentExpression = CalculatorEngine.SanitizeInput(currentExpression, text);
                txtDisplay.Text = string.IsNullOrEmpty(currentExpression) ? "0" : currentExpression;
            }
        }

        private async void Evaluate()
        {
            if (string.IsNullOrWhiteSpace(currentExpression)) return;

            string exprToEval = currentExpression;
            lblExpressionHistory.Text = exprToEval + " = ...";
            Cursor = Cursors.WaitCursor;

            try
            {
                var result = await CalculatorEngine.EvaluateAsync(exprToEval, Function.CurrentAngleUnit);

                if (!result.IsSuccess)
                {
                    lblExpressionHistory.Text = $"{exprToEval} = Hata: {result.ErrorMessage}";
                    txtDisplay.Text = result.ErrorCategory;
                    isResultShown = true;
                    return;
                }

                string resStr = result.FormattedValue;
                string historyEntry = $"{exprToEval} = {resStr}";

                // Bounded LRU history cap (max 50 items)
                lstHistory.Items.Insert(0, historyEntry);
                while (lstHistory.Items.Count > 50)
                {
                    lstHistory.Items.RemoveAt(lstHistory.Items.Count - 1);
                }

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
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
