using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class PdeSystemSolverControl
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
                "Kısmi Diferansiyel Denklem Sistemi Çözücü (Coupled PDE System)",
                "Bağlaşımlı Reaksiyon-Difüzyon Kısmi Diferansiyel Denklem Sistemlerini (u_t = Du·u_xx + F(u,v), v_t = Dv·v_xx + G(u,v)) çözer.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnExGrayScott = ThemeHelper.CreateExampleChip("Gray-Scott Deseni");
            this.btnExSchnakenberg = ThemeHelper.CreateExampleChip("Schnakenberg Modeli");
            this.btnExFitzHugh = ThemeHelper.CreateExampleChip("FitzHugh-Nagumo");
            this.btnExDiffusion = ThemeHelper.CreateExampleChip("Çift Difüzyon");

            this.lblReactionF = new Label();
            this.txtReactionF = new TextBox();

            this.lblReactionG = new Label();
            this.txtReactionG = new TextBox();

            this.lblInitU = new Label();
            this.txtInitU = new TextBox();

            this.lblInitV = new Label();
            this.txtInitV = new TextBox();

            this.lblDu = new Label();
            this.txtDu = new TextBox();

            this.lblDv = new Label();
            this.txtDv = new TextBox();

            this.lblL = new Label();
            this.txtL = new TextBox();

            this.lblT = new Label();
            this.txtT = new TextBox();

            this.lblNx = new Label();
            this.numNx = new NumericUpDown();

            this.lblNt = new Label();
            this.numNt = new NumericUpDown();

            this.btnSolve = ThemeHelper.CreateActionButton("🚀 PDE Sistemini Çöz", 160, 32, true);
            this.btnCopy = ThemeHelper.CreateCopyButton("📋 Tabloyu Kopyala");

            this.pnlCardResult = new Panel();
            this.lblSummary = new Label();
            this.lblDetails = new Label();

            this.txtOutput = new RichTextBox();

            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNt)).BeginInit();
            this.pnlCardResult.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 175;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(24, 8, 24, 8);

            this.lblPresets.Text = "Hazır Modeller:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 8, 0);

            this.pnlPresets.Location = new Point(24, 8);
            this.pnlPresets.Size = new Size(880, 32);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnExGrayScott);
            this.pnlPresets.Controls.Add(this.btnExSchnakenberg);
            this.pnlPresets.Controls.Add(this.btnExFitzHugh);
            this.pnlPresets.Controls.Add(this.btnExDiffusion);

            int y1 = 44;
            this.lblReactionF.Text = "F(u, v) [u reaksiyonu]:";
            this.lblReactionF.Location = new Point(24, y1 + 4);
            this.lblReactionF.AutoSize = true;
            this.lblReactionF.ForeColor = Color.FromArgb(147, 197, 253);

            this.txtReactionF.Location = new Point(160, y1);
            this.txtReactionF.Size = new Size(270, 25);
            this.txtReactionF.Text = "-u * v^2 + 0.04 * (1 - u)";
            this.txtReactionF.Font = new Font("Consolas", 10F);
            this.txtReactionF.BackColor = ThemeHelper.CardBackground;
            this.txtReactionF.ForeColor = Color.White;
            this.txtReactionF.BorderStyle = BorderStyle.FixedSingle;

            this.lblReactionG.Text = "G(u, v) [v reaksiyonu]:";
            this.lblReactionG.Location = new Point(450, y1 + 4);
            this.lblReactionG.AutoSize = true;
            this.lblReactionG.ForeColor = Color.FromArgb(249, 168, 212);

            this.txtReactionG.Location = new Point(590, y1);
            this.txtReactionG.Size = new Size(270, 25);
            this.txtReactionG.Text = "u * v^2 - 0.09 * v";
            this.txtReactionG.Font = new Font("Consolas", 10F);
            this.txtReactionG.BackColor = ThemeHelper.CardBackground;
            this.txtReactionG.ForeColor = Color.White;
            this.txtReactionG.BorderStyle = BorderStyle.FixedSingle;

            int y2 = 78;
            this.lblInitU.Text = "u(x, 0) =";
            this.lblInitU.Location = new Point(24, y2 + 4);
            this.lblInitU.AutoSize = true;
            this.lblInitU.ForeColor = ThemeHelper.LightText;

            this.txtInitU.Location = new Point(80, y2);
            this.txtInitU.Size = new Size(160, 25);
            this.txtInitU.Text = "1 - 0.5 * exp(-100*(x-0.5)^2)";
            this.txtInitU.Font = new Font("Consolas", 9.5F);
            this.txtInitU.BackColor = ThemeHelper.CardBackground;
            this.txtInitU.ForeColor = Color.White;
            this.txtInitU.BorderStyle = BorderStyle.FixedSingle;

            this.lblInitV.Text = "v(x, 0) =";
            this.lblInitV.Location = new Point(255, y2 + 4);
            this.lblInitV.AutoSize = true;
            this.lblInitV.ForeColor = ThemeHelper.LightText;

            this.txtInitV.Location = new Point(310, y2);
            this.txtInitV.Size = new Size(160, 25);
            this.txtInitV.Text = "0.25 * exp(-100*(x-0.5)^2)";
            this.txtInitV.Font = new Font("Consolas", 9.5F);
            this.txtInitV.BackColor = ThemeHelper.CardBackground;
            this.txtInitV.ForeColor = Color.White;
            this.txtInitV.BorderStyle = BorderStyle.FixedSingle;

            this.lblDu.Text = "Du:";
            this.lblDu.Location = new Point(485, y2 + 4);
            this.lblDu.AutoSize = true;
            this.lblDu.ForeColor = ThemeHelper.LightText;

            this.txtDu.Location = new Point(515, y2);
            this.txtDu.Size = new Size(55, 25);
            this.txtDu.Text = "0.001";
            this.txtDu.Font = new Font("Consolas", 9.5F);
            this.txtDu.BackColor = ThemeHelper.CardBackground;
            this.txtDu.ForeColor = Color.White;
            this.txtDu.BorderStyle = BorderStyle.FixedSingle;

            this.lblDv.Text = "Dv:";
            this.lblDv.Location = new Point(585, y2 + 4);
            this.lblDv.AutoSize = true;
            this.lblDv.ForeColor = ThemeHelper.LightText;

            this.txtDv.Location = new Point(615, y2);
            this.txtDv.Size = new Size(55, 25);
            this.txtDv.Text = "0.0005";
            this.txtDv.Font = new Font("Consolas", 9.5F);
            this.txtDv.BackColor = ThemeHelper.CardBackground;
            this.txtDv.ForeColor = Color.White;
            this.txtDv.BorderStyle = BorderStyle.FixedSingle;

            this.lblL.Text = "L:";
            this.lblL.Location = new Point(685, y2 + 4);
            this.lblL.AutoSize = true;
            this.lblL.ForeColor = ThemeHelper.LightText;

            this.txtL.Location = new Point(705, y2);
            this.txtL.Size = new Size(40, 25);
            this.txtL.Text = "1";
            this.txtL.Font = new Font("Consolas", 9.5F);
            this.txtL.BackColor = ThemeHelper.CardBackground;
            this.txtL.ForeColor = Color.White;
            this.txtL.BorderStyle = BorderStyle.FixedSingle;

            this.lblT.Text = "T:";
            this.lblT.Location = new Point(755, y2 + 4);
            this.lblT.AutoSize = true;
            this.lblT.ForeColor = ThemeHelper.LightText;

            this.txtT.Location = new Point(775, y2);
            this.txtT.Size = new Size(40, 25);
            this.txtT.Text = "1.0";
            this.txtT.Font = new Font("Consolas", 9.5F);
            this.txtT.BackColor = ThemeHelper.CardBackground;
            this.txtT.ForeColor = Color.White;
            this.txtT.BorderStyle = BorderStyle.FixedSingle;

            this.lblNx.Text = "Nx:";
            this.lblNx.Location = new Point(825, y2 + 4);
            this.lblNx.AutoSize = true;
            this.lblNx.ForeColor = ThemeHelper.LightText;

            this.numNx.Location = new Point(855, y2);
            this.numNx.Size = new Size(45, 25);
            this.numNx.Minimum = 5;
            this.numNx.Maximum = 200;
            this.numNx.Value = 25;
            this.numNx.BackColor = ThemeHelper.CardBackground;
            this.numNx.ForeColor = Color.White;

            int y3 = 120;
            this.lblNt.Text = "Zaman Adımı (Nt):";
            this.lblNt.Location = new Point(24, y3 + 6);
            this.lblNt.AutoSize = true;
            this.lblNt.ForeColor = ThemeHelper.LightText;

            this.numNt.Location = new Point(135, y3 + 2);
            this.numNt.Size = new Size(60, 25);
            this.numNt.Minimum = 10;
            this.numNt.Maximum = 2000;
            this.numNt.Value = 150;
            this.numNt.BackColor = ThemeHelper.CardBackground;
            this.numNt.ForeColor = Color.White;

            this.btnSolve.Location = new Point(220, y3);
            this.btnCopy.Location = new Point(390, y3);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblReactionF);
            this.pnlControls.Controls.Add(this.txtReactionF);
            this.pnlControls.Controls.Add(this.lblReactionG);
            this.pnlControls.Controls.Add(this.txtReactionG);
            this.pnlControls.Controls.Add(this.lblInitU);
            this.pnlControls.Controls.Add(this.txtInitU);
            this.pnlControls.Controls.Add(this.lblInitV);
            this.pnlControls.Controls.Add(this.txtInitV);
            this.pnlControls.Controls.Add(this.lblDu);
            this.pnlControls.Controls.Add(this.txtDu);
            this.pnlControls.Controls.Add(this.lblDv);
            this.pnlControls.Controls.Add(this.txtDv);
            this.pnlControls.Controls.Add(this.lblL);
            this.pnlControls.Controls.Add(this.txtL);
            this.pnlControls.Controls.Add(this.lblT);
            this.pnlControls.Controls.Add(this.txtT);
            this.pnlControls.Controls.Add(this.lblNx);
            this.pnlControls.Controls.Add(this.numNx);
            this.pnlControls.Controls.Add(this.lblNt);
            this.pnlControls.Controls.Add(this.numNt);
            this.pnlControls.Controls.Add(this.btnSolve);
            this.pnlControls.Controls.Add(this.btnCopy);

            // pnlCardResult
            this.pnlCardResult.Dock = DockStyle.Top;
            this.pnlCardResult.Height = 65;
            this.pnlCardResult.BackColor = ThemeHelper.CardBackground;
            this.pnlCardResult.Padding = new Padding(20, 8, 20, 8);

            this.lblSummary.Dock = DockStyle.Top;
            this.lblSummary.Height = 26;
            this.lblSummary.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            this.lblSummary.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblSummary.Text = "PDE Sistemi: Çözüm Bekleniyor";

            this.lblDetails.Dock = DockStyle.Bottom;
            this.lblDetails.Height = 20;
            this.lblDetails.Font = new Font("Segoe UI", 9F);
            this.lblDetails.ForeColor = ThemeHelper.MutedText;
            this.lblDetails.Text = "Hazır modelleri seçebilir veya kendi reaksiyon denklemlerinizi yazabilirsiniz.";

            this.pnlCardResult.Controls.Add(this.lblDetails);
            this.pnlCardResult.Controls.Add(this.lblSummary);

            // txtOutput
            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.FromArgb(11, 17, 32);
            this.txtOutput.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtOutput.Font = new Font("Consolas", 9.5F);
            this.txtOutput.BorderStyle = BorderStyle.None;
            this.txtOutput.ReadOnly = true;

            // Assemble PdeSystemSolverControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.pnlCardResult);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "PdeSystemSolverControl";
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
        private Button btnExGrayScott;
        private Button btnExSchnakenberg;
        private Button btnExFitzHugh;
        private Button btnExDiffusion;
        private Label lblReactionF;
        private TextBox txtReactionF;
        private Label lblReactionG;
        private TextBox txtReactionG;
        private Label lblInitU;
        private TextBox txtInitU;
        private Label lblInitV;
        private TextBox txtInitV;
        private Label lblDu;
        private TextBox txtDu;
        private Label lblDv;
        private TextBox txtDv;
        private Label lblL;
        private TextBox txtL;
        private Label lblT;
        private TextBox txtT;
        private Label lblNx;
        private NumericUpDown numNx;
        private Label lblNt;
        private NumericUpDown numNt;
        private Button btnSolve;
        private Button btnCopy;
        private Panel pnlCardResult;
        private Label lblSummary;
        private Label lblDetails;
        private RichTextBox txtOutput;
    }
}
