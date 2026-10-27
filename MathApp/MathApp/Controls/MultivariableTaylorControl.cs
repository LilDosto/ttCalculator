using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

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

            btnEx1.Click += (s, e) => LoadPreset("sin(x)*cos(y)", 3, new[] { ("x", "0"), ("y", "0") });
            btnEx2.Click += (s, e) => LoadPreset("exp(x + y)", 2, new[] { ("x", "0"), ("y", "0") });
            btnEx3.Click += (s, e) => LoadPreset("x^2*y + y^3", 3, new[] { ("x", "1"), ("y", "2") });

            btnCopyMultiTaylor.Click += (s, e) => {
                if (!string.IsNullOrEmpty(txtResult.Text))
                {
                    Clipboard.SetText(txtResult.Text);
                    MessageBox.Show("Çok değişkenli Taylor serisi panoya kopyalandı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            txtEquation.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    BtnCalculate_Click(this, EventArgs.Empty);
                }
            };

            AddVariableRow("x", "0");
            AddVariableRow("y", "0");
        }

        private void LoadPreset(string eq, int order, (string v, string val)[] vars)
        {
            txtEquation.Text = eq;
            numOrder.Value = order;
            pnlVariables.Controls.Clear();
            variableRows.Clear();
            foreach (var item in vars)
            {
                AddVariableRow(item.v, item.val);
            }
            BtnCalculate_Click(this, EventArgs.Empty);
        }

        private void BtnAddVar_Click(object sender, EventArgs e)
        {
            string[] names = { "x", "y", "z", "w", "u", "v" };
            string suggested = variableRows.Count < names.Length ? names[variableRows.Count] : "v" + (variableRows.Count + 1);
            AddVariableRow(suggested, "0");
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
                string eq = txtEquation.Text.Trim();
                if (string.IsNullOrWhiteSpace(eq))
                {
                    txtResult.Text = "Lütfen bir çok değişkenli fonksiyon giriniz.";
                    return;
                }

                int degree = (int)numOrder.Value;
                Dictionary<string, decimal> points = new Dictionary<string, decimal>();

                foreach (var row in variableRows)
                {
                    string v = row.Variable;
                    if (string.IsNullOrWhiteSpace(v)) continue;

                    string valNorm = row.Value.Replace(',', '.');
                    if (!decimal.TryParse(valNorm, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal p))
                    {
                        txtResult.Text = $"❌ '{v}' değişkeni için geçerli bir sayı giriniz.";
                        return;
                    }

                    if (points.ContainsKey(v))
                    {
                        txtResult.Text = $"❌ '{v}' değişkeni birden fazla kez eklenmiş.";
                        return;
                    }
                    points.Add(v, p);
                }

                if (points.Count == 0)
                {
                    txtResult.Text = "⚠️ Lütfen en az bir değişken ekleyiniz.";
                    return;
                }

                bool prevRad = Function.UseRadians;
                Function.UseRadians = true;
                string result;
                try
                {
                    result = NumericalMethods.MethodMultiVar_TaylorSeries(eq, degree, points);
                }
                finally
                {
                    Function.UseRadians = prevRad;
                }

                string varsList = string.Join(", ", points.Keys);
                string centerList = string.Join(", ", points.Select(kv => $"{kv.Key}₀={kv.Value}"));

                txtResult.Text =
                    $"=== Çok Değişkenli Taylor Açılımı ({degree}. Derece, Çapraz Kısmi Türevler Dahil) ===" + Environment.NewLine +
                    $"Fonksiyon : f({varsList}) = {eq}" + Environment.NewLine +
                    $"Merkez    : ({centerList})" + Environment.NewLine + Environment.NewLine +
                    $"T_{degree}({varsList}) ≈ {result}";
            }
            catch (Exception ex)
            {
                txtResult.Text = "❌ Hata: " + ex.Message;
            }
        }
    }

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
            Panel = new Panel
            {
                Size = new Size(520, 34),
                BackColor = ThemeHelper.CardBackground,
                Margin = new Padding(0, 0, 0, 5)
            };

            Label lblVar = new Label
            {
                Text = "Değişken:",
                Location = new Point(10, 8),
                AutoSize = true,
                ForeColor = ThemeHelper.LightText,
                Font = new Font("Segoe UI", 9.5F)
            };

            TxtVariable = new TextBox
            {
                Text = varName,
                Location = new Point(85, 5),
                Size = new Size(80, 25),
                BackColor = ThemeHelper.InputBackground,
                ForeColor = Color.FromArgb(56, 189, 248),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10.5F, FontStyle.Bold),
                TextAlign = HorizontalAlignment.Center
            };

            Label lblVal = new Label
            {
                Text = "Merkez Noktası:",
                Location = new Point(185, 8),
                AutoSize = true,
                ForeColor = ThemeHelper.LightText,
                Font = new Font("Segoe UI", 9.5F)
            };

            TxtValue = new TextBox
            {
                Text = val,
                Location = new Point(295, 5),
                Size = new Size(100, 25),
                BackColor = ThemeHelper.InputBackground,
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 10.5F),
                TextAlign = HorizontalAlignment.Center
            };

            Button btnRemove = new Button
            {
                Text = "✕ Sil",
                Location = new Point(415, 4),
                Size = new Size(65, 26),
                BackColor = Color.FromArgb(153, 27, 27),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
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
