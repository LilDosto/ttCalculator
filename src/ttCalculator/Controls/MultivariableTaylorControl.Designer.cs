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
                "f(x, y, ...) fonksiyonları için tüm kısmi ve çapraz (mixed partial) türevleri içeren teğet düzlemi, Hessian formu ve tam Taylor açılımı.");

            this.pnlContent = new Panel();

            // Presets
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateExampleChip("sin(x)*cos(y) (0, 0)");
            this.btnEx2 = ThemeHelper.CreateExampleChip("exp(x + y) (0, 0)");
            this.btnEx3 = ThemeHelper.CreateExampleChip("x²y + y³ (1, 2)");
            this.btnEx4 = ThemeHelper.CreateExampleChip("x² + y² + z² (0, 0, 0)");
            this.btnEx5 = ThemeHelper.CreateExampleChip("ln(1 + x*y) (0, 0)");

            // Main Input Card
            this.pnlInputCard = new Panel();
            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.lblOrder = new Label();
            this.numOrder = new NumericUpDown();
            this.btnCalculate = ThemeHelper.CreateActionButton("📐 Seriyi Hesapla", 180, 36, true);

            // Variable Chips Area
            this.pnlVarCard = new Panel();
            this.lblVarTitle = new Label();
            this.pnlVariableChips = new FlowLayoutPanel();
            this.btnAddVar = ThemeHelper.CreateActionButton("+ Değişken Ekle", 130, 28, false);
            this.btnResetZero = new Button();

            // TabControl for Results
            this.tabResults = new TabControl();
            this.tabLayers = new TabPage();
            this.tabTable = new TabPage();
            this.tabReport = new TabPage();

            // Tab 1: Layered Cards (Linear/Tangent, Quadratic/Hessian, Full Expansion)
            this.pnlLayersScroll = new Panel();
            this.pnlTangentCard = new Panel();
            this.lblTangentTitle = new Label();
            this.lblTangentValue = new Label();
            this.lblGradientValue = new Label();
            this.btnCopyTangent = ThemeHelper.CreateCopyButton("📋 Teğet Düzlemini Kopyala");

            this.pnlHessianCard = new Panel();
            this.lblHessianTitle = new Label();
            this.lblHessianValue = new Label();
            this.lblHessianDet = new Label();

            this.pnlFullPolyCard = new Panel();
            this.lblFullPolyTitle = new Label();
            this.lblFullPolyValue = new Label();
            this.btnCopyFullPoly = ThemeHelper.CreateCopyButton("📋 Polinomu Kopyala");
            this.btnCopyLatex = ThemeHelper.CreateCopyButton("LaTeX");

            // Tab 2: Derivatives Grid
            this.dgvMultiTerms = new DataGridView();

            // Tab 3: Detailed Text Console
            this.txtResult = new TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.pnlContent.SuspendLayout();
            this.pnlInputCard.SuspendLayout();
            this.pnlVarCard.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabLayers.SuspendLayout();
            this.pnlLayersScroll.SuspendLayout();
            this.pnlTangentCard.SuspendLayout();
            this.pnlHessianCard.SuspendLayout();
            this.pnlFullPolyCard.SuspendLayout();
            this.tabTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiTerms)).BeginInit();
            this.tabReport.SuspendLayout();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(16, 4, 16, 10);
            this.pnlContent.AutoScroll = false;

            // Presets
            this.lblPresets.Text = "⚡ Hızlı Örnekler:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 6, 8, 0);

            this.pnlPresets.Dock = DockStyle.Top;
            this.pnlPresets.Height = 32;
            this.pnlPresets.WrapContents = false;
            this.pnlPresets.AutoScroll = false;
            this.pnlPresets.Margin = new Padding(0, 0, 0, 4);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);
            this.pnlPresets.Controls.Add(this.btnEx5);

            // Input Card
            this.pnlInputCard.Dock = DockStyle.Top;
            this.pnlInputCard.Height = 48;
            this.pnlInputCard.BackColor = ThemeHelper.CardBackground;
            this.pnlInputCard.Padding = new Padding(12, 8, 12, 8);
            this.pnlInputCard.Margin = new Padding(0, 0, 0, 4);

            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblEquation.Location = new Point(12, 16);
            this.lblEquation.Text = "Fonksiyon f(x, y, ...):";

            this.txtEquation.BackColor = Color.FromArgb(15, 23, 42);
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.Font = new Font("Consolas", 10.5F);
            this.txtEquation.Location = new Point(140, 12);
            this.txtEquation.Size = new Size(340, 24);
            this.txtEquation.Text = "sin(x)*cos(y)";

            this.lblOrder.AutoSize = true;
            this.lblOrder.ForeColor = ThemeHelper.LightText;
            this.lblOrder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblOrder.Location = new Point(500, 16);
            this.lblOrder.Text = "Derece (n):";

            this.numOrder.BackColor = Color.FromArgb(15, 23, 42);
            this.numOrder.ForeColor = Color.White;
            this.numOrder.BorderStyle = BorderStyle.FixedSingle;
            this.numOrder.Font = new Font("Consolas", 10F);
            this.numOrder.Location = new Point(575, 13);
            this.numOrder.Maximum = 5;
            this.numOrder.Minimum = 1;
            this.numOrder.Size = new Size(50, 23);
            this.numOrder.Value = 3;

            this.btnCalculate.Location = new Point(650, 7);

            this.pnlInputCard.Controls.Add(this.lblEquation);
            this.pnlInputCard.Controls.Add(this.txtEquation);
            this.pnlInputCard.Controls.Add(this.lblOrder);
            this.pnlInputCard.Controls.Add(this.numOrder);
            this.pnlInputCard.Controls.Add(this.btnCalculate);

            // Variable Chips Card
            this.pnlVarCard.Dock = DockStyle.Top;
            this.pnlVarCard.Height = 72;
            this.pnlVarCard.BackColor = Color.FromArgb(20, 28, 48);
            this.pnlVarCard.Padding = new Padding(10, 6, 10, 6);
            this.pnlVarCard.Margin = new Padding(0, 0, 0, 6);

            this.lblVarTitle.Text = "Değişkenler ve Merkez Noktaları (x₀):";
            this.lblVarTitle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblVarTitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblVarTitle.Location = new Point(10, 6);
            this.lblVarTitle.AutoSize = true;

            this.btnAddVar.Location = new Point(650, 6);
            this.btnAddVar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnResetZero.Text = "Tümü = 0";
            this.btnResetZero.Location = new Point(788, 6);
            this.btnResetZero.Size = new Size(76, 28);
            this.btnResetZero.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnResetZero.BackColor = Color.FromArgb(33, 44, 70);
            this.btnResetZero.ForeColor = Color.FromArgb(186, 230, 253);
            this.btnResetZero.FlatStyle = FlatStyle.Flat;
            this.btnResetZero.FlatAppearance.BorderSize = 0;
            this.btnResetZero.Font = new Font("Segoe UI", 8F);
            this.btnResetZero.Cursor = Cursors.Hand;

            this.pnlVariableChips.Location = new Point(10, 28);
            this.pnlVariableChips.Size = new Size(856, 38);
            this.pnlVariableChips.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlVariableChips.FlowDirection = FlowDirection.LeftToRight;
            this.pnlVariableChips.WrapContents = true;
            this.pnlVariableChips.AutoScroll = true;

            this.pnlVarCard.Controls.Add(this.lblVarTitle);
            this.pnlVarCard.Controls.Add(this.btnAddVar);
            this.pnlVarCard.Controls.Add(this.btnResetZero);
            this.pnlVarCard.Controls.Add(this.pnlVariableChips);

            // TabControl for Results (Dock Fill!)
            this.tabResults.Dock = DockStyle.Fill;
            this.tabResults.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

            // TAB 1: Layered Cards
            this.tabLayers.Text = "Katmanlı Yaklaşımlar (Doğrusal / Hessian / Polinom)";
            this.tabLayers.BackColor = Color.FromArgb(15, 23, 42);
            this.tabLayers.Padding = new Padding(8);

            this.pnlLayersScroll.Dock = DockStyle.Fill;
            this.pnlLayersScroll.AutoScroll = true;

            // Block 1: Tangent Plane (n=1)
            this.pnlTangentCard.Location = new Point(4, 4);
            this.pnlTangentCard.Size = new Size(840, 85);
            this.pnlTangentCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlTangentCard.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlTangentCard.Padding = new Padding(12, 8, 12, 8);

            this.lblTangentTitle.Text = "1. DERECE DOĞRUSAL YAKLAŞIM / TEĞET DÜZLEM (L(x, y))";
            this.lblTangentTitle.Location = new Point(12, 8);
            this.lblTangentTitle.AutoSize = true;
            this.lblTangentTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblTangentTitle.ForeColor = Color.FromArgb(56, 189, 248);

            this.lblTangentValue.Text = "L(x, y) = —";
            this.lblTangentValue.Location = new Point(12, 28);
            this.lblTangentValue.Size = new Size(620, 24);
            this.lblTangentValue.Font = new Font("Consolas", 11F, FontStyle.Bold);
            this.lblTangentValue.ForeColor = Color.White;

            this.lblGradientValue.Text = "Gradyan ∇f(a) = [ — ]";
            this.lblGradientValue.Location = new Point(12, 54);
            this.lblGradientValue.Size = new Size(620, 20);
            this.lblGradientValue.Font = new Font("Consolas", 9F);
            this.lblGradientValue.ForeColor = Color.FromArgb(148, 163, 184);

            this.btnCopyTangent.Location = new Point(650, 18);
            this.btnCopyTangent.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlTangentCard.Controls.Add(this.lblTangentTitle);
            this.pnlTangentCard.Controls.Add(this.lblTangentValue);
            this.pnlTangentCard.Controls.Add(this.lblGradientValue);
            this.pnlTangentCard.Controls.Add(this.btnCopyTangent);

            // Block 2: Quadratic / Hessian (n=2)
            this.pnlHessianCard.Location = new Point(4, 96);
            this.pnlHessianCard.Size = new Size(840, 115);
            this.pnlHessianCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlHessianCard.BackColor = Color.FromArgb(24, 33, 52);
            this.pnlHessianCard.Padding = new Padding(12, 8, 12, 8);

            this.lblHessianTitle.Text = "2. DERECE KUADRATİK YAKLAŞIM & HESSIAN MATRİSİ (Q(x, y))";
            this.lblHessianTitle.Location = new Point(12, 8);
            this.lblHessianTitle.AutoSize = true;
            this.lblHessianTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblHessianTitle.ForeColor = Color.FromArgb(168, 85, 247);

            this.lblHessianValue.Text = "Q(x, y) = —";
            this.lblHessianValue.Location = new Point(12, 28);
            this.lblHessianValue.Size = new Size(815, 24);
            this.lblHessianValue.Font = new Font("Consolas", 10.5F, FontStyle.Bold);
            this.lblHessianValue.ForeColor = Color.White;

            this.lblHessianDet.Text = "Hessian H(a) ve Ekstremum Durumu: —";
            this.lblHessianDet.Location = new Point(12, 56);
            this.lblHessianDet.Size = new Size(815, 48);
            this.lblHessianDet.Font = new Font("Consolas", 9F);
            this.lblHessianDet.ForeColor = Color.FromArgb(226, 232, 240);

            this.pnlHessianCard.Controls.Add(this.lblHessianTitle);
            this.pnlHessianCard.Controls.Add(this.lblHessianValue);
            this.pnlHessianCard.Controls.Add(this.lblHessianDet);

            // Block 3: Full Polynomial
            this.pnlFullPolyCard.Location = new Point(4, 218);
            this.pnlFullPolyCard.Size = new Size(840, 110);
            this.pnlFullPolyCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlFullPolyCard.BackColor = Color.FromArgb(20, 30, 48);
            this.pnlFullPolyCard.Padding = new Padding(12, 8, 12, 8);

            this.lblFullPolyTitle.Text = "TAM ÇOK DEĞİŞKENLİ POLİNOM AÇILIMI (T_n)";
            this.lblFullPolyTitle.Location = new Point(12, 8);
            this.lblFullPolyTitle.AutoSize = true;
            this.lblFullPolyTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblFullPolyTitle.ForeColor = Color.FromArgb(52, 211, 153);

            this.lblFullPolyValue.Text = "T_n = —";
            this.lblFullPolyValue.Location = new Point(12, 30);
            this.lblFullPolyValue.Size = new Size(650, 70);
            this.lblFullPolyValue.Font = new Font("Consolas", 10.5F, FontStyle.Bold);
            this.lblFullPolyValue.ForeColor = Color.FromArgb(52, 211, 153);

            this.btnCopyFullPoly.Location = new Point(680, 14);
            this.btnCopyFullPoly.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnCopyLatex.Location = new Point(780, 14);
            this.btnCopyLatex.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.pnlFullPolyCard.Controls.Add(this.lblFullPolyTitle);
            this.pnlFullPolyCard.Controls.Add(this.lblFullPolyValue);
            this.pnlFullPolyCard.Controls.Add(this.btnCopyFullPoly);
            this.pnlFullPolyCard.Controls.Add(this.btnCopyLatex);

            this.pnlLayersScroll.Controls.Add(this.pnlFullPolyCard);
            this.pnlLayersScroll.Controls.Add(this.pnlHessianCard);
            this.pnlLayersScroll.Controls.Add(this.pnlTangentCard);
            this.tabLayers.Controls.Add(this.pnlLayersScroll);

            // TAB 2: Table
            this.tabTable.Text = "Kısmi & Çapraz Türevler Tablosu";
            this.tabTable.BackColor = Color.FromArgb(24, 33, 52);
            this.tabTable.Padding = new Padding(6);
            SetupGrid(this.dgvMultiTerms);
            this.tabTable.Controls.Add(this.dgvMultiTerms);

            // TAB 3: Report
            this.tabReport.Text = "Analitik Rapor";
            this.tabReport.BackColor = Color.FromArgb(24, 33, 52);
            this.tabReport.Padding = new Padding(6);

            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(11, 15, 28);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);
            this.tabReport.Controls.Add(this.txtResult);

            this.tabResults.TabPages.Add(this.tabLayers);
            this.tabResults.TabPages.Add(this.tabTable);
            this.tabResults.TabPages.Add(this.tabReport);

            this.pnlContent.Controls.Add(this.tabResults);
            this.pnlContent.Controls.Add(this.pnlVarCard);
            this.pnlContent.Controls.Add(this.pnlInputCard);
            this.pnlContent.Controls.Add(this.pnlPresets);

            this.pnlPresets.SendToBack();
            this.pnlInputCard.SendToBack();
            this.pnlVarCard.SendToBack();
            this.tabResults.BringToFront();

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "MultivariableTaylorControl";
            this.Size = new Size(920, 720);

            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.pnlContent.ResumeLayout(false);
            this.pnlInputCard.ResumeLayout(false);
            this.pnlInputCard.PerformLayout();
            this.pnlVarCard.ResumeLayout(false);
            this.pnlVarCard.PerformLayout();
            this.tabResults.ResumeLayout(false);
            this.tabLayers.ResumeLayout(false);
            this.pnlLayersScroll.ResumeLayout(false);
            this.pnlTangentCard.ResumeLayout(false);
            this.pnlTangentCard.PerformLayout();
            this.pnlHessianCard.ResumeLayout(false);
            this.pnlHessianCard.PerformLayout();
            this.pnlFullPolyCard.ResumeLayout(false);
            this.pnlFullPolyCard.PerformLayout();
            this.tabTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMultiTerms)).EndInit();
            this.tabReport.ResumeLayout(false);
            this.tabReport.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.FromArgb(15, 23, 42);
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 33, 52);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(148, 163, 184);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 26;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(20, 28, 48);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Consolas", 9.5F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.GridColor = Color.FromArgb(40, 50, 75);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
        }

        private Panel pnlHeader;
        private Panel pnlContent;
        private FlowLayoutPanel pnlPresets;
        private Label lblPresets;
        private Button btnEx1;
        private Button btnEx2;
        private Button btnEx3;
        private Button btnEx4;
        private Button btnEx5;

        private Panel pnlInputCard;
        private Label lblEquation;
        private TextBox txtEquation;
        private Label lblOrder;
        private NumericUpDown numOrder;
        private Button btnCalculate;

        private Panel pnlVarCard;
        private Label lblVarTitle;
        private FlowLayoutPanel pnlVariableChips;
        private Button btnAddVar;
        private Button btnResetZero;

        private TabControl tabResults;
        private TabPage tabLayers;
        private Panel pnlLayersScroll;
        private Panel pnlTangentCard;
        private Label lblTangentTitle;
        private Label lblTangentValue;
        private Label lblGradientValue;
        private Button btnCopyTangent;

        private Panel pnlHessianCard;
        private Label lblHessianTitle;
        private Label lblHessianValue;
        private Label lblHessianDet;

        private Panel pnlFullPolyCard;
        private Label lblFullPolyTitle;
        private Label lblFullPolyValue;
        private Button btnCopyFullPoly;
        private Button btnCopyLatex;

        private TabPage tabTable;
        private DataGridView dgvMultiTerms;
        private TabPage tabReport;
        private TextBox txtResult;
    }
}
