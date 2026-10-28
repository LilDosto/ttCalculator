using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class PdeSolverControl
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
                "Kısmi Diferansiyel Denklem Çözücü (1D PDE Solver)",
                "1 Boyutlu Isı (FTCS), Dalga (Leapfrog) ve Taşınım (Upwind) denklemlerini sonlu farklar yöntemi ile çözer.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExHeat = ThemeHelper.CreateExampleChip("Isı Yayılımı [sin(πx)]");
            this.btnExWave = ThemeHelper.CreateExampleChip("Titreşen Dalga [c=2, sin(πx)]");
            this.btnExAdvect = ThemeHelper.CreateExampleChip("Taşınım Dalgası [Advection]");
            this.btnExGauss = ThemeHelper.CreateExampleChip("Gauss Sıcaklık Paketi");

            this.lblPdeType = new Label();
            this.cmbPdeType = new ComboBox();

            this.lblInitU = new Label();
            this.txtInitU = new TextBox();

            this.lblInitVel = new Label();
            this.txtInitVel = new TextBox();

            this.lblParam = new Label();
            this.txtParam = new TextBox();

            this.lblL = new Label();
            this.txtL = new TextBox();

            this.lblT = new Label();
            this.txtT = new TextBox();

            this.lblNx = new Label();
            this.numNx = new NumericUpDown();

            this.lblNt = new Label();
            this.numNt = new NumericUpDown();

            this.lblBc = new Label();
            this.txtBcLeft = new TextBox();
            this.txtBcRight = new TextBox();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 PDE'yi Çöz", 140, 32, true);
            this.btnCopy = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala");

            this.pnlCardResult = new Panel();
            this.lblStability = new Label();
            this.lblDetails = new Label();

            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).BeginInit();
            this.pnlCardResult.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 168;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(24, 8, 24, 8);

            this.lblPresets.Text = "Örnek Senaryolar:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(24, 8);
            this.pnlPresets.Size = new Size(880, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExHeat);
            this.pnlPresets.Controls.Add(this.btnExWave);
            this.pnlPresets.Controls.Add(this.btnExAdvect);
            this.pnlPresets.Controls.Add(this.btnExGauss);

            int y1 = 46;
            this.lblPdeType.Text = "PDE Türü:";
            this.lblPdeType.Location = new Point(24, y1 + 4);
            this.lblPdeType.AutoSize = true;
            this.lblPdeType.ForeColor = ThemeHelper.LightText;

            this.cmbPdeType.Location = new Point(90, y1);
            this.cmbPdeType.Size = new Size(240, 25);
            this.cmbPdeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPdeType.BackColor = ThemeHelper.CardBackground;
            this.cmbPdeType.ForeColor = Color.White;
            this.cmbPdeType.Font = new Font("Segoe UI", 9.5F);
            this.cmbPdeType.Items.AddRange(new object[] {
                "Isı Denklemi [u_t = α·u_xx]",
                "Dalga Denklemi [u_tt = c²·u_xx]",
                "Taşınım Denklemi [u_t + v·u_x = 0]"
            });
            this.cmbPdeType.SelectedIndex = 0;

            this.lblInitU.Text = "u(x, 0) =";
            this.lblInitU.Location = new Point(345, y1 + 4);
            this.lblInitU.AutoSize = true;
            this.lblInitU.ForeColor = ThemeHelper.LightText;

            this.txtInitU.Location = new Point(410, y1);
            this.txtInitU.Size = new Size(220, 25);
            this.txtInitU.Text = "sin(pi * x)";
            this.txtInitU.Font = new Font("Consolas", 10F);
            this.txtInitU.BackColor = ThemeHelper.CardBackground;
            this.txtInitU.ForeColor = Color.White;
            this.txtInitU.BorderStyle = BorderStyle.FixedSingle;

            this.lblInitVel.Text = "u_t(x, 0) =";
            this.lblInitVel.Location = new Point(640, y1 + 4);
            this.lblInitVel.AutoSize = true;
            this.lblInitVel.ForeColor = ThemeHelper.LightText;

            this.txtInitVel.Location = new Point(710, y1);
            this.txtInitVel.Size = new Size(160, 25);
            this.txtInitVel.Text = "0";
            this.txtInitVel.Font = new Font("Consolas", 10F);
            this.txtInitVel.BackColor = ThemeHelper.CardBackground;
            this.txtInitVel.ForeColor = Color.White;
            this.txtInitVel.BorderStyle = BorderStyle.FixedSingle;

            int y2 = 82;
            this.lblParam.Text = "Parametre (α / c / v):";
            this.lblParam.Location = new Point(24, y2 + 4);
            this.lblParam.AutoSize = true;
            this.lblParam.ForeColor = ThemeHelper.LightText;

            this.txtParam.Location = new Point(155, y2);
            this.txtParam.Size = new Size(60, 25);
            this.txtParam.Text = "0.1";
            this.txtParam.Font = new Font("Consolas", 10F);
            this.txtParam.BackColor = ThemeHelper.CardBackground;
            this.txtParam.ForeColor = Color.White;
            this.txtParam.BorderStyle = BorderStyle.FixedSingle;

            this.lblL.Text = "Uzunluk L:";
            this.lblL.Location = new Point(225, y2 + 4);
            this.lblL.AutoSize = true;
            this.lblL.ForeColor = ThemeHelper.LightText;

            this.txtL.Location = new Point(295, y2);
            this.txtL.Size = new Size(50, 25);
            this.txtL.Text = "1";
            this.txtL.Font = new Font("Consolas", 10F);
            this.txtL.BackColor = ThemeHelper.CardBackground;
            this.txtL.ForeColor = Color.White;
            this.txtL.BorderStyle = BorderStyle.FixedSingle;

            this.lblT.Text = "Süre T:";
            this.lblT.Location = new Point(355, y2 + 4);
            this.lblT.AutoSize = true;
            this.lblT.ForeColor = ThemeHelper.LightText;

            this.txtT.Location = new Point(405, y2);
            this.txtT.Size = new Size(50, 25);
            this.txtT.Text = "0.5";
            this.txtT.Font = new Font("Consolas", 10F);
            this.txtT.BackColor = ThemeHelper.CardBackground;
            this.txtT.ForeColor = Color.White;
            this.txtT.BorderStyle = BorderStyle.FixedSingle;

            this.lblNx.Text = "Nx (Mekan):";
            this.lblNx.Location = new Point(465, y2 + 4);
            this.lblNx.AutoSize = true;
            this.lblNx.ForeColor = ThemeHelper.LightText;

            this.numNx.Location = new Point(545, y2);
            this.numNx.Size = new Size(55, 25);
            this.numNx.Minimum = 5;
            this.numNx.Maximum = 200;
            this.numNx.Value = 25;
            this.numNx.BackColor = ThemeHelper.CardBackground;
            this.numNx.ForeColor = Color.White;

            this.lblNt.Text = "Nt (Zaman):";
            this.lblNt.Location = new Point(610, y2 + 4);
            this.lblNt.AutoSize = true;
            this.lblNt.ForeColor = ThemeHelper.LightText;

            this.numNt.Location = new Point(690, y2);
            this.numNt.Size = new Size(60, 25);
            this.numNt.Minimum = 5;
            this.numNt.Maximum = 2000;
            this.numNt.Value = 150;
            this.numNt.BackColor = ThemeHelper.CardBackground;
            this.numNt.ForeColor = Color.White;

            this.lblBc.Text = "Sınırlar [u(0), u(L)]:";
            this.lblBc.Location = new Point(760, y2 + 4);
            this.lblBc.AutoSize = true;
            this.lblBc.ForeColor = ThemeHelper.LightText;

            this.txtBcLeft.Location = new Point(870, y2);
            this.txtBcLeft.Size = new Size(35, 25);
            this.txtBcLeft.Text = "0";
            this.txtBcLeft.Font = new Font("Consolas", 9F);
            this.txtBcLeft.BackColor = ThemeHelper.CardBackground;
            this.txtBcLeft.ForeColor = Color.White;
            this.txtBcLeft.BorderStyle = BorderStyle.FixedSingle;

            this.txtBcRight.Location = new Point(910, y2);
            this.txtBcRight.Size = new Size(35, 25);
            this.txtBcRight.Text = "0";
            this.txtBcRight.Font = new Font("Consolas", 9F);
            this.txtBcRight.BackColor = ThemeHelper.CardBackground;
            this.txtBcRight.ForeColor = Color.White;
            this.txtBcRight.BorderStyle = BorderStyle.FixedSingle;

            int y3 = 120;
            this.btnSolve.Location = new Point(24, y3);
            this.btnCopy.Location = new Point(175, y3);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblPdeType);
            this.pnlControls.Controls.Add(this.cmbPdeType);
            this.pnlControls.Controls.Add(this.lblInitU);
            this.pnlControls.Controls.Add(this.txtInitU);
            this.pnlControls.Controls.Add(this.lblInitVel);
            this.pnlControls.Controls.Add(this.txtInitVel);
            this.pnlControls.Controls.Add(this.lblParam);
            this.pnlControls.Controls.Add(this.txtParam);
            this.pnlControls.Controls.Add(this.lblL);
            this.pnlControls.Controls.Add(this.txtL);
            this.pnlControls.Controls.Add(this.lblT);
            this.pnlControls.Controls.Add(this.txtT);
            this.pnlControls.Controls.Add(this.lblNx);
            this.pnlControls.Controls.Add(this.numNx);
            this.pnlControls.Controls.Add(this.lblNt);
            this.pnlControls.Controls.Add(this.numNt);
            this.pnlControls.Controls.Add(this.lblBc);
            this.pnlControls.Controls.Add(this.txtBcLeft);
            this.pnlControls.Controls.Add(this.txtBcRight);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.btnCopy);

            // pnlCardResult
            this.pnlCardResult.Dock = DockStyle.Top;
            this.pnlCardResult.Height = 65;
            this.pnlCardResult.BackColor = ThemeHelper.CardBackground;
            this.pnlCardResult.Padding = new Padding(20, 8, 20, 8);

            this.lblStability.Dock = DockStyle.Top;
            this.lblStability.Height = 26;
            this.lblStability.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            this.lblStability.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblStability.Text = "Kararlılık & Yöntem: Bekleniyor";

            this.lblDetails.Dock = DockStyle.Bottom;
            this.lblDetails.Height = 20;
            this.lblDetails.Font = new Font("Segoe UI", 9F);
            this.lblDetails.ForeColor = ThemeHelper.MutedText;
            this.lblDetails.Text = "PDE çözümü bekleniyor.";

            this.pnlCardResult.Controls.Add(this.lblDetails);
            this.pnlCardResult.Controls.Add(this.lblStability);

            // txtOutput
            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 17, 32);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            // Assemble PdeSolverControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.pnlCardResult);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "PdeSolverControl";
            this.Size = new Size(960, 680);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).EndInit();
            this.pnlCardResult.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlControls;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnExHeat;
        private Button btnExWave;
        private Button btnExAdvect;
        private Button btnExGauss;
        private Label lblPdeType;
        private ComboBox cmbPdeType;
        private Label lblInitU;
        private TextBox txtInitU;
        private Label lblInitVel;
        private TextBox txtInitVel;
        private Label lblParam;
        private TextBox txtParam;
        private Label lblL;
        private TextBox txtL;
        private Label lblT;
        private TextBox txtT;
        private Label lblNx;
        private NumericUpDown numNx;
        private Label lblNt;
        private NumericUpDown numNt;
        private Label lblBc;
        private TextBox txtBcLeft;
        private TextBox txtBcRight;
        private Button btnSolve;
        private Button btnCopy;
        private Panel pnlCardResult;
        private Label lblStability;
        private Label lblDetails;
        private RichTextBox txtOutput;
    }
}
