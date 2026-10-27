using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class DerivativeCalculatorControl
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
                "Nümerik Türev & Kısmi Türev Stüdyosu (Differentiation)",
                "Merkezi sonlu farklar matrisi, ileri binom ve geri farklar yöntemleriyle yüksek dereceli türev, kısmi türev ve gradyan (∇f) analizi.");

            this.pnlContent = new Panel();
            this.pnlTopSettings = new Panel();
            this.lblStepH = new Label();
            this.txtStepH = new TextBox();
            this.lblAcc = new Label();
            this.cmbAccuracy = new ComboBox();

            this.grpSingle = new GroupBox();
            this.lblSingleFn = new Label();
            this.txtSingleFn = new TextBox();
            this.lblSingleVar = new Label();
            this.txtSingleVar = new TextBox();
            this.lblSinglePoint = new Label();
            this.txtSinglePoint = new TextBox();
            this.lblSingleDeg = new Label();
            this.numSingleDegree = new NumericUpDown();
            this.btnCalcSingle = ThemeHelper.CreateActionButton("∂ Tek Değişkenli Türev", 190, 34, true);
            this.btnExSingle1 = ThemeHelper.CreateExampleChip("Örnek: sin(x)+x³");
            this.btnExSingle2 = ThemeHelper.CreateExampleChip("Örnek: exp(2x)");

            this.grpPartial = new GroupBox();
            this.lblPartialFn = new Label();
            this.txtPartialFn = new TextBox();
            this.lblTargetVar = new Label();
            this.txtTargetVar = new TextBox();
            this.lblPartialDeg = new Label();
            this.numPartialDegree = new NumericUpDown();
            this.pnlPartialVars = new FlowLayoutPanel();
            this.btnAddVar = ThemeHelper.CreateActionButton("+ Değişken", 110, 28, false);
            this.btnCalcPartial = ThemeHelper.CreateActionButton("∇ Kısmi Türev & Gradyan", 210, 34, true);
            this.btnExPartial1 = ThemeHelper.CreateExampleChip("Örnek: x²y + sin(y)");
            this.btnExPartial2 = ThemeHelper.CreateExampleChip("Örnek: exp(x*y)");

            this.lblResult = new Label();
            this.txtResult = new TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.numSingleDegree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartialDegree)).BeginInit();
            this.pnlContent.SuspendLayout();
            this.pnlTopSettings.SuspendLayout();
            this.grpSingle.SuspendLayout();
            this.grpPartial.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(20, 12, 20, 20);

            // Top settings bar
            this.pnlTopSettings.Dock = DockStyle.Top;
            this.pnlTopSettings.Height = 38;

            this.lblStepH.Text = "Adım Boyutu (h):";
            this.lblStepH.AutoSize = true;
            this.lblStepH.ForeColor = ThemeHelper.LightText;
            this.lblStepH.Font = new Font("Segoe UI", 9.5F);
            this.lblStepH.Location = new Point(4, 8);

            this.txtStepH.Location = new Point(115, 5);
            this.txtStepH.Size = new Size(75, 25);
            this.txtStepH.Text = "0.001";
            this.txtStepH.Font = new Font("Consolas", 10F);
            this.txtStepH.BackColor = ThemeHelper.CardBackground;
            this.txtStepH.ForeColor = Color.White;
            this.txtStepH.BorderStyle = BorderStyle.FixedSingle;

            this.lblAcc.Text = "Matris Hassasiyet Derecesi:";
            this.lblAcc.AutoSize = true;
            this.lblAcc.ForeColor = ThemeHelper.LightText;
            this.lblAcc.Font = new Font("Segoe UI", 9.5F);
            this.lblAcc.Location = new Point(215, 8);

            this.cmbAccuracy.Items.AddRange(new object[] {
                "2. Derece O(h²)",
                "4. Derece O(h⁴) - Önerilen",
                "6. Derece O(h⁶) - Yüksek"
            });
            this.cmbAccuracy.Location = new Point(385, 5);
            this.cmbAccuracy.Size = new Size(200, 25);
            this.cmbAccuracy.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccuracy.BackColor = ThemeHelper.CardBackground;
            this.cmbAccuracy.ForeColor = Color.White;
            this.cmbAccuracy.Font = new Font("Segoe UI", 9.5F);

            this.pnlTopSettings.Controls.Add(this.cmbAccuracy);
            this.pnlTopSettings.Controls.Add(this.lblAcc);
            this.pnlTopSettings.Controls.Add(this.txtStepH);
            this.pnlTopSettings.Controls.Add(this.lblStepH);

            // grpSingle
            this.grpSingle.Location = new Point(20, 54);
            this.grpSingle.Size = new Size(405, 225);
            this.grpSingle.ForeColor = ThemeHelper.LightText;
            this.grpSingle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.grpSingle.Text = "1. Tek Değişkenli Türev: dⁿf(x) / dxⁿ";

            this.lblSingleFn.Text = "f(x) =";
            this.lblSingleFn.AutoSize = true;
            this.lblSingleFn.Location = new Point(14, 30);
            this.txtSingleFn.Location = new Point(75, 27);
            this.txtSingleFn.Size = new Size(310, 26);
            this.txtSingleFn.Text = "sin(x) + x^3";
            this.txtSingleFn.Font = new Font("Consolas", 10.5F);
            this.txtSingleFn.BackColor = ThemeHelper.CardBackground;
            this.txtSingleFn.ForeColor = Color.White;
            this.txtSingleFn.BorderStyle = BorderStyle.FixedSingle;

            this.lblSingleVar.Text = "Değişken:";
            this.lblSingleVar.AutoSize = true;
            this.lblSingleVar.Location = new Point(14, 68);
            this.txtSingleVar.Location = new Point(85, 65);
            this.txtSingleVar.Size = new Size(50, 26);
            this.txtSingleVar.Text = "x";
            this.txtSingleVar.Font = new Font("Consolas", 10.5F);
            this.txtSingleVar.BackColor = ThemeHelper.CardBackground;
            this.txtSingleVar.ForeColor = Color.White;
            this.txtSingleVar.BorderStyle = BorderStyle.FixedSingle;

            this.lblSinglePoint.Text = "Nokta (x₀):";
            this.lblSinglePoint.AutoSize = true;
            this.lblSinglePoint.Location = new Point(150, 68);
            this.txtSinglePoint.Location = new Point(230, 65);
            this.txtSinglePoint.Size = new Size(65, 26);
            this.txtSinglePoint.Text = "1";
            this.txtSinglePoint.Font = new Font("Consolas", 10.5F);
            this.txtSinglePoint.BackColor = ThemeHelper.CardBackground;
            this.txtSinglePoint.ForeColor = Color.White;
            this.txtSinglePoint.BorderStyle = BorderStyle.FixedSingle;

            this.lblSingleDeg.Text = "Türev Derecesi:";
            this.lblSingleDeg.AutoSize = true;
            this.lblSingleDeg.Location = new Point(14, 106);
            this.numSingleDegree.Location = new Point(120, 104);
            this.numSingleDegree.Size = new Size(60, 26);
            this.numSingleDegree.Minimum = 1;
            this.numSingleDegree.Maximum = 6;
            this.numSingleDegree.Value = 1;
            this.numSingleDegree.Font = new Font("Consolas", 10.5F);
            this.numSingleDegree.BackColor = ThemeHelper.CardBackground;
            this.numSingleDegree.ForeColor = Color.White;
            this.numSingleDegree.BorderStyle = BorderStyle.FixedSingle;

            this.btnCalcSingle.Location = new Point(195, 100);
            this.btnExSingle1.Location = new Point(14, 155);
            this.btnExSingle2.Location = new Point(165, 155);

            this.grpSingle.Controls.Add(this.btnExSingle2);
            this.grpSingle.Controls.Add(this.btnExSingle1);
            this.grpSingle.Controls.Add(this.btnCalcSingle);
            this.grpSingle.Controls.Add(this.numSingleDegree);
            this.grpSingle.Controls.Add(this.lblSingleDeg);
            this.grpSingle.Controls.Add(this.txtSinglePoint);
            this.grpSingle.Controls.Add(this.lblSinglePoint);
            this.grpSingle.Controls.Add(this.txtSingleVar);
            this.grpSingle.Controls.Add(this.lblSingleVar);
            this.grpSingle.Controls.Add(this.txtSingleFn);
            this.grpSingle.Controls.Add(this.lblSingleFn);

            // grpPartial
            this.grpPartial.Location = new Point(440, 54);
            this.grpPartial.Size = new Size(440, 225);
            this.grpPartial.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.grpPartial.ForeColor = ThemeHelper.LightText;
            this.grpPartial.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.grpPartial.Text = "2. Çok Değişkenli Kısmi Türev & Gradyan (∂ⁿf / ∂xᵢⁿ)";

            this.lblPartialFn.Text = "f(...) =";
            this.lblPartialFn.AutoSize = true;
            this.lblPartialFn.Location = new Point(14, 28);
            this.txtPartialFn.Location = new Point(65, 25);
            this.txtPartialFn.Size = new Size(195, 26);
            this.txtPartialFn.Text = "x^2*y + sin(y)";
            this.txtPartialFn.Font = new Font("Consolas", 10F);
            this.txtPartialFn.BackColor = ThemeHelper.CardBackground;
            this.txtPartialFn.ForeColor = Color.White;
            this.txtPartialFn.BorderStyle = BorderStyle.FixedSingle;

            this.lblTargetVar.Text = "Türev Değişkeni:";
            this.lblTargetVar.AutoSize = true;
            this.lblTargetVar.Location = new Point(268, 28);
            this.txtTargetVar.Location = new Point(378, 25);
            this.txtTargetVar.Size = new Size(45, 26);
            this.txtTargetVar.Text = "x";
            this.txtTargetVar.Font = new Font("Consolas", 10F, FontStyle.Bold);
            this.txtTargetVar.BackColor = ThemeHelper.CardBackground;
            this.txtTargetVar.ForeColor = Color.FromArgb(56, 189, 248);
            this.txtTargetVar.BorderStyle = BorderStyle.FixedSingle;
            this.txtTargetVar.TextAlign = HorizontalAlignment.Center;

            this.pnlPartialVars.Location = new Point(14, 58);
            this.pnlPartialVars.Size = new Size(410, 110);
            this.pnlPartialVars.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPartialVars.AutoScroll = true;
            this.pnlPartialVars.BackColor = Color.FromArgb(11, 17, 32);

            this.lblPartialDeg.Text = "Derece:";
            this.lblPartialDeg.AutoSize = true;
            this.lblPartialDeg.Location = new Point(14, 182);
            this.numPartialDegree.Location = new Point(68, 179);
            this.numPartialDegree.Size = new Size(48, 26);
            this.numPartialDegree.Minimum = 1;
            this.numPartialDegree.Maximum = 4;
            this.numPartialDegree.Value = 1;
            this.numPartialDegree.BackColor = ThemeHelper.CardBackground;
            this.numPartialDegree.ForeColor = Color.White;
            this.numPartialDegree.BorderStyle = BorderStyle.FixedSingle;

            this.btnAddVar.Location = new Point(122, 178);
            this.btnCalcPartial.Location = new Point(238, 175);

            this.grpPartial.Controls.Add(this.btnCalcPartial);
            this.grpPartial.Controls.Add(this.btnAddVar);
            this.grpPartial.Controls.Add(this.numPartialDegree);
            this.grpPartial.Controls.Add(this.lblPartialDeg);
            this.grpPartial.Controls.Add(this.pnlPartialVars);
            this.grpPartial.Controls.Add(this.txtTargetVar);
            this.grpPartial.Controls.Add(this.lblTargetVar);
            this.grpPartial.Controls.Add(this.txtPartialFn);
            this.grpPartial.Controls.Add(this.lblPartialFn);

            // Result area
            this.lblResult.Text = "📊 Nümerik Türev, Kısmi Türev ve Gradyan Raporu:";
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.lblResult.ForeColor = ThemeHelper.LightText;
            this.lblResult.Location = new Point(20, 290);

            this.btnCopyDerivative = ThemeHelper.CreateCopyButton("📋 Kopyala");
            this.btnCopyDerivative.Location = new Point(780, 284);
            this.btnCopyDerivative.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.txtResult.Location = new Point(20, 316);
            this.txtResult.Size = new Size(860, 245);
            this.txtResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.BackColor = ThemeHelper.CardBackground;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.BorderStyle = BorderStyle.FixedSingle;
            this.txtResult.Font = new Font("Consolas", 10F);

            this.pnlContent.Controls.Add(this.btnCopyDerivative);
            this.pnlContent.Controls.Add(this.txtResult);
            this.pnlContent.Controls.Add(this.lblResult);
            this.pnlContent.Controls.Add(this.grpPartial);
            this.pnlContent.Controls.Add(this.grpSingle);
            this.pnlContent.Controls.Add(this.pnlTopSettings);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "DerivativeCalculatorControl";
            this.Size = new Size(900, 660);

            ((System.ComponentModel.ISupportInitialize)(this.numSingleDegree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartialDegree)).EndInit();
            this.pnlTopSettings.ResumeLayout(false);
            this.pnlTopSettings.PerformLayout();
            this.grpSingle.ResumeLayout(false);
            this.grpSingle.PerformLayout();
            this.grpPartial.ResumeLayout(false);
            this.grpPartial.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlTopSettings;
        private Label lblStepH;
        private TextBox txtStepH;
        private Label lblAcc;
        private ComboBox cmbAccuracy;
        private GroupBox grpSingle;
        private Label lblSingleFn;
        private TextBox txtSingleFn;
        private Label lblSingleVar;
        private TextBox txtSingleVar;
        private Label lblSinglePoint;
        private TextBox txtSinglePoint;
        private Label lblSingleDeg;
        private NumericUpDown numSingleDegree;
        private Button btnCalcSingle;
        private Button btnExSingle1;
        private Button btnExSingle2;
        private GroupBox grpPartial;
        private Label lblPartialFn;
        private TextBox txtPartialFn;
        private Label lblTargetVar;
        private TextBox txtTargetVar;
        private Label lblPartialDeg;
        private NumericUpDown numPartialDegree;
        private FlowLayoutPanel pnlPartialVars;
        private Button btnAddVar;
        private Button btnCalcPartial;
        private Button btnExPartial1;
        private Button btnExPartial2;
        private Label lblResult;
        private Button btnCopyDerivative;
        private TextBox txtResult;
    }
}
