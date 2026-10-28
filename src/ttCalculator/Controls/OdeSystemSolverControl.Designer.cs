using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class OdeSystemSolverControl
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
                "Diferansiyel Denklem Sistemi Çözücü (System of ODEs / Yüksek Dereceli ODE)",
                "dY/dt = F(t, Y) vektörel diferansiyel denklem sistemlerini ve n. derece denklemleri RK4, RK8 ve Euler ile çözer.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("Harmonik Osilatör [y₁'=y₂, y₂'=-y₁]");
            this.btnEx2 = ThemeHelper.CreateExampleChip("Av-Avcı [Lotka-Volterra]");
            this.btnEx3 = ThemeHelper.CreateExampleChip("Sönümlü Sarkaç [x'=v, v'=-0.3v-sin(x)]");
            this.btnEx4 = ThemeHelper.CreateExampleChip("Lorenz Kaotik Çekicisi [3D]");

            this.lblEqCount = new Label();
            this.numEqCount = new NumericUpDown();
            this.lblMethod = new Label();
            this.cmbMethod = new ComboBox();

            this.lblT0 = new Label();
            this.txtT0 = new TextBox();
            this.lblTEnd = new Label();
            this.txtTEnd = new TextBox();
            this.lblSteps = new Label();
            this.numSteps = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 Sistemi Çöz", 140, 32, true);
            this.btnCopy = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala");

            this.pnlEquations = new Panel();

            this.pnlCardResult = new Panel();
            this.lblFinalValue = new Label();
            this.lblFinalDetails = new Label();

            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            this.pnlCardResult.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 110;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(24, 8, 24, 8);

            this.lblPresets.Text = "Hazır Sistemler:";
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

            int yRow = 48;
            this.lblEqCount.Text = "Denklem Sayısı:";
            this.lblEqCount.Location = new Point(24, yRow + 4);
            this.lblEqCount.AutoSize = true;
            this.lblEqCount.ForeColor = ThemeHelper.LightText;

            this.numEqCount.Location = new Point(125, yRow);
            this.numEqCount.Size = new Size(50, 25);
            this.numEqCount.Minimum = 1;
            this.numEqCount.Maximum = 6;
            this.numEqCount.Value = 2;
            this.numEqCount.BackColor = ThemeHelper.CardBackground;
            this.numEqCount.ForeColor = Color.White;

            this.lblMethod.Text = "Yöntem:";
            this.lblMethod.Location = new Point(190, yRow + 4);
            this.lblMethod.AutoSize = true;
            this.lblMethod.ForeColor = ThemeHelper.LightText;

            this.cmbMethod.Location = new Point(250, yRow);
            this.cmbMethod.Size = new Size(220, 25);
            this.cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMethod.BackColor = ThemeHelper.CardBackground;
            this.cmbMethod.ForeColor = Color.White;
            this.cmbMethod.Font = new Font("Segoe UI", 9.5F);
            this.cmbMethod.Items.AddRange(new object[] {
                "Runge-Kutta 4 (RK4 - Önerilen)",
                "Runge-Kutta 8 (RK8 - Yüksek Hassasiyet)",
                "Euler Yöntemi (1. Derece)"
            });
            this.cmbMethod.SelectedIndex = 0;

            this.lblT0.Text = "t₀:";
            this.lblT0.Location = new Point(485, yRow + 4);
            this.lblT0.AutoSize = true;
            this.lblT0.ForeColor = ThemeHelper.LightText;

            this.txtT0.Location = new Point(510, yRow);
            this.txtT0.Size = new Size(50, 25);
            this.txtT0.Text = "0";
            this.txtT0.Font = new Font("Consolas", 10F);
            this.txtT0.BackColor = ThemeHelper.CardBackground;
            this.txtT0.ForeColor = Color.White;
            this.txtT0.BorderStyle = BorderStyle.FixedSingle;

            this.lblTEnd.Text = "t_son:";
            this.lblTEnd.Location = new Point(570, yRow + 4);
            this.lblTEnd.AutoSize = true;
            this.lblTEnd.ForeColor = ThemeHelper.LightText;

            this.txtTEnd.Location = new Point(615, yRow);
            this.txtTEnd.Size = new Size(50, 25);
            this.txtTEnd.Text = "10";
            this.txtTEnd.Font = new Font("Consolas", 10F);
            this.txtTEnd.BackColor = ThemeHelper.CardBackground;
            this.txtTEnd.ForeColor = Color.White;
            this.txtTEnd.BorderStyle = BorderStyle.FixedSingle;

            this.lblSteps.Text = "Adım (N):";
            this.lblSteps.Location = new Point(675, yRow + 4);
            this.lblSteps.AutoSize = true;
            this.lblSteps.ForeColor = ThemeHelper.LightText;

            this.numSteps.Location = new Point(740, yRow);
            this.numSteps.Size = new Size(65, 25);
            this.numSteps.Minimum = 2;
            this.numSteps.Maximum = 5000;
            this.numSteps.Value = 100;
            this.numSteps.BackColor = ThemeHelper.CardBackground;
            this.numSteps.ForeColor = Color.White;

            this.btnSolve.Location = new Point(24, 78);
            this.btnCopy.Location = new Point(175, 78);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEqCount);
            this.pnlControls.Controls.Add(this.numEqCount);
            this.pnlControls.Controls.Add(this.lblMethod);
            this.pnlControls.Controls.Add(this.cmbMethod);
            this.pnlControls.Controls.Add(this.lblT0);
            this.pnlControls.Controls.Add(this.txtT0);
            this.pnlControls.Controls.Add(this.lblTEnd);
            this.pnlControls.Controls.Add(this.txtTEnd);
            this.pnlControls.Controls.Add(this.lblSteps);
            this.pnlControls.Controls.Add(this.numSteps);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.btnCopy);

            // pnlEquations (Dynamic rows for ODEs)
            this.pnlEquations.Dock = DockStyle.Top;
            this.pnlEquations.Height = 115;
            this.pnlEquations.AutoScroll = true;
            this.pnlEquations.BackColor = Color.FromArgb(20, 28, 45);
            this.pnlEquations.Padding = new Padding(24, 6, 24, 6);

            // pnlCardResult
            this.pnlCardResult.Dock = DockStyle.Top;
            this.pnlCardResult.Height = 65;
            this.pnlCardResult.BackColor = ThemeHelper.CardBackground;
            this.pnlCardResult.Padding = new Padding(20, 8, 20, 8);

            this.lblFinalValue.Dock = DockStyle.Top;
            this.lblFinalValue.Height = 26;
            this.lblFinalValue.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            this.lblFinalValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblFinalValue.Text = "Y(t_son) = —";

            this.lblFinalDetails.Dock = DockStyle.Bottom;
            this.lblFinalDetails.Height = 20;
            this.lblFinalDetails.Font = new Font("Segoe UI", 9F);
            this.lblFinalDetails.ForeColor = ThemeHelper.MutedText;
            this.lblFinalDetails.Text = "Sistem çözümü bekleniyor.";

            this.pnlCardResult.Controls.Add(this.lblFinalDetails);
            this.pnlCardResult.Controls.Add(this.lblFinalValue);

            // txtOutput
            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 17, 32);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            // Assemble OdeSystemSolverControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.pnlCardResult);
            this.Controls.Add(this.pnlEquations);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "OdeSystemSolverControl";
            this.Size = new Size(920, 680);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
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
        private Label lblEqCount;
        private NumericUpDown numEqCount;
        private Label lblMethod;
        private ComboBox cmbMethod;
        private Label lblT0;
        private TextBox txtT0;
        private Label lblTEnd;
        private TextBox txtTEnd;
        private Label lblSteps;
        private NumericUpDown numSteps;
        private Button btnSolve;
        private Button btnCopy;
        private Panel pnlEquations;
        private Panel pnlCardResult;
        private Label lblFinalValue;
        private Label lblFinalDetails;
        private RichTextBox txtOutput;
    }
}
