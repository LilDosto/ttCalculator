using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class GraphPlotterControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Etkileşimli Fonksiyon Grafiği Çizici (Graph Plotter)",
                "Gerçek zamanlı yakınlaştırma (Zoom), kaydırma (Pan), asimptot algılama ve eğri üzerinde koordinat inceleme.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("sin(x) * x");
            this.btnEx2 = ThemeHelper.CreateExampleChip("x³ - 3x");
            this.btnEx3 = ThemeHelper.CreateExampleChip("e^(-x²/4)*cos(2x)");
            this.btnEx4 = ThemeHelper.CreateExampleChip("1 / (x² - 1)");

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblMinX = new Label();
            this.txtMinX = new TextBox();
            this.lblMaxX = new Label();
            this.txtMaxX = new TextBox();
            this.btnPlot = ThemeHelper.CreateActionButton("📈 Grafiği Çiz", 130, 32, true);
            this.btnResetView = ThemeHelper.CreateActionButton("🔄 Sıfırla", 100, 32, false);
            this.btnClearMarks = ThemeHelper.CreateActionButton("📍 Noktaları Sil", 130, 32, false);

            this.pnlStatusBar = new Panel();
            this.lblStatus = new Label();
            this.pnlCanvas = new Panel();

            this.pnlControls.SuspendLayout();
            this.pnlStatusBar.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 96;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(24, 8, 24, 8);

            this.lblPresets.Text = "Hızlı Fonksiyonlar:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 5, 10, 0);

            this.pnlPresets.Location = new Point(24, 8);
            this.pnlPresets.Size = new Size(820, 34);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);

            int y = 52;

            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            this.lblEquation.Text = "f(x) =";
            this.lblEquation.Location = new Point(24, y + 3);

            this.txtEquation.Location = new Point(74, y);
            this.txtEquation.Size = new Size(210, 27);
            this.txtEquation.Text = "sin(x)";
            this.txtEquation.Font = new Font("Consolas", 11F);
            this.txtEquation.BackColor = ThemeHelper.CardBackground;
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;

            this.lblMinX.AutoSize = true;
            this.lblMinX.ForeColor = ThemeHelper.LightText;
            this.lblMinX.Font = new Font("Segoe UI", 9.5F);
            this.lblMinX.Text = "Min X:";
            this.lblMinX.Location = new Point(296, y + 4);

            this.txtMinX.Location = new Point(344, y);
            this.txtMinX.Size = new Size(55, 27);
            this.txtMinX.Text = "-10";
            this.txtMinX.Font = new Font("Consolas", 10.5F);
            this.txtMinX.BackColor = ThemeHelper.CardBackground;
            this.txtMinX.ForeColor = Color.White;
            this.txtMinX.BorderStyle = BorderStyle.FixedSingle;

            this.lblMaxX.AutoSize = true;
            this.lblMaxX.ForeColor = ThemeHelper.LightText;
            this.lblMaxX.Font = new Font("Segoe UI", 9.5F);
            this.lblMaxX.Text = "Max X:";
            this.lblMaxX.Location = new Point(408, y + 4);

            this.txtMaxX.Location = new Point(458, y);
            this.txtMaxX.Size = new Size(55, 27);
            this.txtMaxX.Text = "10";
            this.txtMaxX.Font = new Font("Consolas", 10.5F);
            this.txtMaxX.BackColor = ThemeHelper.CardBackground;
            this.txtMaxX.ForeColor = Color.White;
            this.txtMaxX.BorderStyle = BorderStyle.FixedSingle;

            this.btnPlot.Location = new Point(525, y - 2);
            this.btnResetView.Location = new Point(663, y - 2);
            this.btnClearMarks.Location = new Point(771, y - 2);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.btnClearMarks);
            this.pnlControls.Controls.Add(this.btnResetView);
            this.pnlControls.Controls.Add(this.btnPlot);
            this.pnlControls.Controls.Add(this.txtMaxX);
            this.pnlControls.Controls.Add(this.lblMaxX);
            this.pnlControls.Controls.Add(this.txtMinX);
            this.pnlControls.Controls.Add(this.lblMinX);
            this.pnlControls.Controls.Add(this.txtEquation);
            this.pnlControls.Controls.Add(this.lblEquation);

            // pnlStatusBar
            this.pnlStatusBar.Dock = DockStyle.Bottom;
            this.pnlStatusBar.Height = 28;
            this.pnlStatusBar.BackColor = Color.FromArgb(11, 17, 32);
            this.pnlStatusBar.Padding = new Padding(16, 4, 16, 4);

            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.ForeColor = ThemeHelper.MutedText;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.lblStatus.Text = "Hazır.";
            this.pnlStatusBar.Controls.Add(this.lblStatus);

            // pnlCanvas
            this.pnlCanvas.Dock = DockStyle.Fill;
            this.pnlCanvas.BackColor = Color.FromArgb(11, 17, 32);
            this.pnlCanvas.Cursor = Cursors.Cross;
            this.pnlCanvas.Paint += new PaintEventHandler(this.pnlCanvas_Paint);
            this.pnlCanvas.Resize += new EventHandler(this.pnlCanvas_Resize);

            // GraphPlotterControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.pnlStatusBar);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "GraphPlotterControl";
            this.Size = new Size(920, 660);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlStatusBar.ResumeLayout(false);
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
        private Label lblMinX;
        private TextBox txtMinX;
        private Label lblMaxX;
        private TextBox txtMaxX;
        private Button btnPlot;
        private Button btnResetView;
        private Button btnClearMarks;
        private Panel pnlStatusBar;
        private Label lblStatus;
        private Panel pnlCanvas;
    }
}
