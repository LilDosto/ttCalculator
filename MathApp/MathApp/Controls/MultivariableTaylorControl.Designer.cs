using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class MultivariableTaylorControl
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
                "Çok Değişkenli Taylor Serisi (Multivariable Taylor)",
                "f(x, y, ...) fonksiyonları için tüm kısmi ve çapraz (mixed partial) türevleri içeren tam çok değişkenli Taylor açılımı.");

            this.pnlContent = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("Örnek 1: sin(x)*cos(y)");
            this.btnEx2 = ThemeHelper.CreateExampleChip("Örnek 2: exp(x + y)");
            this.btnEx3 = ThemeHelper.CreateExampleChip("Örnek 3: x²y + y³ (1, 2)");

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblOrder = new Label();
            this.numOrder = new NumericUpDown();
            this.grpVariables = new GroupBox();
            this.pnlVariables = new FlowLayoutPanel();
            this.btnAddVar = ThemeHelper.CreateActionButton("+ Değişken Ekle", 150, 30, false);
            this.btnCalculate = ThemeHelper.CreateActionButton("📐 Seriyi Hesapla", 190, 38, true);
            this.lblResult = new Label();
            this.txtResult = new TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.grpVariables.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(24, 16, 24, 24);

            this.lblPresets.Text = "Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 6, 10, 0);

            this.pnlPresets.Location = new Point(24, 14);
            this.pnlPresets.Size = new Size(800, 36);
            this.pnlPresets.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);

            int y = 60;

            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI", 10F);
            this.lblEquation.Location = new Point(24, y + 3);
            this.lblEquation.Text = "Fonksiyon f(x,y,...):";

            this.txtEquation.BackColor = ThemeHelper.CardBackground;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.Font = new Font("Consolas", 11F);
            this.txtEquation.Location = new Point(165, y);
            this.txtEquation.Size = new Size(290, 27);
            this.txtEquation.Text = "sin(x)*cos(y)";

            this.lblOrder.AutoSize = true;
            this.lblOrder.ForeColor = ThemeHelper.LightText;
            this.lblOrder.Font = new Font("Segoe UI", 10F);
            this.lblOrder.Location = new Point(475, y + 3);
            this.lblOrder.Text = "Derece (n):";

            this.numOrder.BackColor = ThemeHelper.CardBackground;
            this.numOrder.ForeColor = Color.White;
            this.numOrder.BorderStyle = BorderStyle.FixedSingle;
            this.numOrder.Font = new Font("Consolas", 11F);
            this.numOrder.Location = new Point(560, y);
            this.numOrder.Maximum = 5;
            this.numOrder.Minimum = 1;
            this.numOrder.Size = new Size(65, 27);
            this.numOrder.Value = 3;

            this.btnCalculate.Location = new Point(645, y - 5);
            y += 46;

            this.grpVariables.Controls.Add(this.pnlVariables);
            this.grpVariables.Controls.Add(this.btnAddVar);
            this.grpVariables.ForeColor = ThemeHelper.LightText;
            this.grpVariables.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.grpVariables.Location = new Point(24, y);
            this.grpVariables.Size = new Size(820, 195);
            this.grpVariables.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.grpVariables.TabStop = false;
            this.grpVariables.Text = "Değişkenler ve Açılım Merkez Noktaları";

            this.pnlVariables.AutoScroll = true;
            this.pnlVariables.FlowDirection = FlowDirection.TopDown;
            this.pnlVariables.WrapContents = false;
            this.pnlVariables.Location = new Point(12, 26);
            this.pnlVariables.Size = new Size(796, 122);
            this.pnlVariables.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlVariables.BackColor = Color.FromArgb(11, 17, 32);
            this.pnlVariables.Padding = new Padding(6);

            this.btnAddVar.Location = new Point(12, 155);
            y += 208;

            this.lblResult.AutoSize = true;
            this.lblResult.ForeColor = ThemeHelper.LightText;
            this.lblResult.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.lblResult.Location = new Point(24, y);
            this.lblResult.Text = "📊 Çok Değişkenli Taylor Polinomu:";

            this.btnCopyMultiTaylor = ThemeHelper.CreateCopyButton("📋 Kopyala");
            this.btnCopyMultiTaylor.Location = new Point(750, y - 4);
            this.btnCopyMultiTaylor.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            y += 26;

            this.txtResult.BackColor = ThemeHelper.CardBackground;
            this.txtResult.BorderStyle = BorderStyle.FixedSingle;
            this.txtResult.Font = new Font("Consolas", 10.5F);
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Location = new Point(24, y);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Size = new Size(820, 215);
            this.txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.pnlContent.Controls.Add(this.btnCopyMultiTaylor);
            this.pnlContent.Controls.Add(this.pnlPresets);
            this.pnlContent.Controls.Add(this.txtResult);
            this.pnlContent.Controls.Add(this.lblResult);
            this.pnlContent.Controls.Add(this.btnCalculate);
            this.pnlContent.Controls.Add(this.grpVariables);
            this.pnlContent.Controls.Add(this.numOrder);
            this.pnlContent.Controls.Add(this.lblOrder);
            this.pnlContent.Controls.Add(this.txtEquation);
            this.pnlContent.Controls.Add(this.lblEquation);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "MultivariableTaylorControl";
            this.Size = new Size(880, 660);

            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.grpVariables.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;
        private Label lblEquation;
        private TextBox txtEquation;
        private Label lblOrder;
        private NumericUpDown numOrder;
        private GroupBox grpVariables;
        private FlowLayoutPanel pnlVariables;
        private Button btnAddVar;
        private Button btnCalculate;
        private Label lblResult;
        private Button btnCopyMultiTaylor;
        private TextBox txtResult;
    }
}
