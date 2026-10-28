using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class OdeSolverControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Diferansiyel Denklem Çözücü (1 Değişkenli ODE / Başlangıç Değer Problemi)",
                "dy/dx = f(x, y) başlangıç değer problemini Euler, Heun (RK2), RK4 ve RK8 (Dormand-Prince) ile çözer.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("y' = x + y  [y(0)=1]");
            this.btnEx2 = ThemeHelper.CreateExampleChip("y' = -2*x*y  [y(0)=1]");
            this.btnEx3 = ThemeHelper.CreateExampleChip("y' = cos(x) - y  [y(0)=0]");
            this.btnEx4 = ThemeHelper.CreateExampleChip("y' = y*(1 - y/10) [Lojistik]");

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblMethod = new Label();
            this.cmbMethod = new ComboBox();

            this.lblX0 = new Label();
            this.txtX0 = new TextBox();
            this.lblY0 = new Label();
            this.txtY0 = new TextBox();
            this.lblXEnd = new Label();
            this.txtXEnd = new TextBox();
            this.lblSteps = new Label();
            this.numSteps = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 Denklemi Çöz", 140, 32, true);
            this.btnCopy = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala");

            this.pnlCardResult = new Panel();
            this.lblFinalValue = new Label();
            this.lblFinalDetails = new Label();

            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            this.pnlCardResult.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 165;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(24, 8, 24, 8);

            // Presets
            this.lblPresets.Text = "Hazır Şablonlar:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(24, 8);
            this.pnlPresets.Size = new Size(880, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);

            int yRow1 = 48;
            this.lblEquation.Text = "dy/dx = f(x, y):";
            this.lblEquation.Location = new Point(24, yRow1 + 3);
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

            this.txtEquation.Location = new Point(135, yRow1);
            this.txtEquation.Size = new Size(240, 26);
            this.txtEquation.Text = "x + y";
            this.txtEquation.Font = new Font("Consolas", 10.5F);
            this.txtEquation.BackColor = ThemeHelper.CardBackground;
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;

            this.lblMethod.Text = "Nümerik Yöntem:";
            this.lblMethod.Location = new Point(390, yRow1 + 3);
            this.lblMethod.AutoSize = true;
            this.lblMethod.ForeColor = ThemeHelper.LightText;
            this.lblMethod.Font = new Font("Segoe UI", 9.5F);

            this.cmbMethod.Location = new Point(505, yRow1);
            this.cmbMethod.Size = new Size(250, 25);
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.BackColor = ThemeHelper.CardBackground;
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.Font = new Font("Segoe UI", 9.5F);
            this.cmbMethod.Items.AddRange(new object[] {
                "Runge-Kutta 4 (RK4 - Önerilen)",
                "Runge-Kutta 8 (RK8 - Ultra Hassas)",
                "Heun / Geliştirilmiş Euler (RK2)",
                "Euler Yöntemi (1. Derece Standart)"
            });
            this.cmbMethod.SelectedIndex = 0;

            this.btnSolve.Location = new Point(765, yRow1 - 2);

            int yRow2 = 88;
            this.lblX0.Text = "x₀ (Başlangıç):";
            this.lblX0.Location = new Point(24, yRow2 + 4);
            this.lblX0.AutoSize = true;
            this.lblX0.ForeColor = ThemeHelper.LightText;

            this.txtX0.Location = new Point(115, yRow2);
            this.txtX0.Size = new Size(55, 25);
            this.txtX0.Text = "0";
            this.txtX0.Font = new Font("Consolas", 10F);
            this.txtX0.BackColor = ThemeHelper.CardBackground;
            this.txtX0.ForeColor = Color.White;
            this.txtX0.BorderStyle = BorderStyle.FixedSingle;

            this.lblY0.Text = "y(x₀):";
            this.lblY0.Location = new Point(185, yRow2 + 4);
            this.lblY0.AutoSize = true;
            this.lblY0.ForeColor = ThemeHelper.LightText;

            this.txtY0.Location = new Point(225, yRow2);
            this.txtY0.Size = new Size(55, 25);
            this.txtY0.Text = "1";
            this.txtY0.Font = new Font("Consolas", 10F);
            this.txtY0.BackColor = ThemeHelper.CardBackground;
            this.txtY0.ForeColor = Color.White;
            this.txtY0.BorderStyle = BorderStyle.FixedSingle;

            this.lblXEnd.Text = "x_son (Hedef):";
            this.lblXEnd.Location = new Point(295, yRow2 + 4);
            this.lblXEnd.AutoSize = true;
            this.lblXEnd.ForeColor = ThemeHelper.LightText;

            this.txtXEnd.Location = new Point(385, yRow2);
            this.txtXEnd.Size = new Size(60, 25);
            this.txtXEnd.Text = "2";
            this.txtXEnd.Font = new Font("Consolas", 10F);
            this.txtXEnd.BackColor = ThemeHelper.CardBackground;
            this.txtXEnd.ForeColor = Color.White;
            this.txtXEnd.BorderStyle = BorderStyle.FixedSingle;

            this.lblSteps.Text = "Adım Sayısı (N):";
            this.lblSteps.Location = new Point(460, yRow2 + 4);
            this.lblSteps.AutoSize = true;
            this.lblSteps.ForeColor = ThemeHelper.LightText;

            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            this.numSteps.Location = new Point(560, yRow2);
            this.numSteps.Size = new Size(70, 25);
            this.numSteps.Minimum = 2;
            this.numSteps.Maximum = 5000;
            this.numSteps.Value = 50;
            this.numSteps.BackColor = ThemeHelper.CardBackground;
            this.numSteps.ForeColor = Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();

            this.btnCopy.Location = new Point(650, yRow2 - 1);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEquation);
            this.pnlControls.Controls.Add(this.txtEquation);
            this.pnlControls.Controls.Add(this.lblMethod);
            this.pnlControls.Controls.Add(this.cmbMethod);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.lblX0);
            this.pnlControls.Controls.Add(this.txtX0);
            this.pnlControls.Controls.Add(this.lblY0);
            this.pnlControls.Controls.Add(this.txtY0);
            this.pnlControls.Controls.Add(this.lblXEnd);
            this.pnlControls.Controls.Add(this.txtXEnd);
            this.pnlControls.Controls.Add(this.lblSteps);
            this.pnlControls.Controls.Add(this.numSteps);
            this.pnlControls.Controls.Add(this.btnCopy);

            // pnlCardResult (Hero card)
            this.pnlCardResult.Dock = DockStyle.Top;
            this.pnlCardResult.Height = 70;
            this.pnlCardResult.BackColor = ThemeHelper.CardBackground;
            this.pnlCardResult.Padding = new Padding(20, 10, 20, 10);

            this.lblFinalValue.Dock = DockStyle.Top;
            this.lblFinalValue.Height = 28;
            this.lblFinalValue.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            this.lblFinalValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblFinalValue.Text = "y(x_son) ≈ —";

            this.lblFinalDetails.Dock = DockStyle.Bottom;
            this.lblFinalDetails.Height = 22;
            this.lblFinalDetails.Font = new Font("Segoe UI", 9F);
            this.lblFinalDetails.ForeColor = ThemeHelper.MutedText;
            this.lblFinalDetails.Text = "Hesaplama bekleniyor.";

            this.pnlCardResult.Controls.Add(this.lblFinalDetails);
            this.pnlCardResult.Controls.Add(this.lblFinalValue);

            // txtOutput
            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 17, 32);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            // Assemble OdeSolverControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.pnlCardResult);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "OdeSolverControl";
            this.Size = new Size(920, 660);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlCardResult.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlControls;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;
        private Button btnEx4;
        private Label lblEquation;
        private TextBox txtEquation;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Button btnSolve;
        private Label lblX0;
        private TextBox txtX0;
        private Label lblY0;
        private TextBox txtY0;
        private Label lblXEnd;
        private TextBox txtXEnd;
        private Label lblSteps;
        private NumericUpDown numSteps;
        private Button btnCopy;
        private Panel pnlCardResult;
        private Label lblFinalValue;
        private Label lblFinalDetails;
        private RichTextBox txtOutput;
    }
}
