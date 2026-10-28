using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class DerivativeCalculatorControl
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Panel pnlContent;

        // Top Settings Bar
        private Panel pnlTopSettings;
        private Label lblStepH;
        private TextBox txtStepH;
        private Button btnH1;
        private Button btnH2;
        private Button btnH3;
        private Label lblFormula;
        private ComboBox cmbFormula;
        private Label lblAcc;
        private ComboBox cmbAccuracy;

        // Panels Left & Right
        private Panel pnlSplit;
        private Panel grpSingle;
        private Panel grpPartial;

        // Single Variable Controls
        private Label lblSingleTitle;
        private Label lblSingleFn;
        private TextBox txtSingleFn;
        private Label lblSingleVar;
        private TextBox txtSingleVar;
        private Label lblSinglePoint;
        private TextBox txtSinglePoint;
        private Label lblSingleDeg;
        private NumericUpDown numSingleDegree;
        private Button btnCalcSingle;
        private FlowLayoutPanel pnlSingleExamples;
        private Button btnExSingle1;
        private Button btnExSingle2;
        private Button btnPlotTangent;

        // Multivariable Controls
        private Label lblPartialTitle;
        private Label lblPartialFn;
        private TextBox txtPartialFn;
        private Label lblTargetVar;
        private TextBox txtTargetVar;
        private Label lblDirectionVector;
        private TextBox txtDirectionVector;
        private FlowLayoutPanel pnlPartialVars;
        private Button btnAddVar;
        private Button btnCalcPartial;
        private Button btnCalcHessian;
        private FlowLayoutPanel pnlPartialExamples;
        private Button btnExPartial1;
        private Button btnExPartial2;

        // Bottom Tabbed Results Area
        private TabControl tabResults;
        private TabPage tabSummary;
        private TabPage tabHessian;
        private TabPage tabLog;

        // Tab 1: Summary Card
        private Panel pnlKpiCard;
        private Label lblKpiTitle;
        private Label lblKpiValue;
        private Label lblKpiSubtitle;
        private RichTextBox txtSummaryView;

        // Tab 2: Hessian Grid
        private DataGridView dgvHessian;
        private Label lblHessianAnalysis;

        // Tab 3: Detailed Log
        private Panel pnlLogToolbar;
        private Button btnCopyLog;
        private TextBox txtResult;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = ThemeHelper.CreateHeaderPanel(
                "Nümerik Türev & Kısmi Türev Stüdyosu (Differentiation)",
                "Merkezi sonlu farklar, ileri/geri farklar, yüksek mertebeli türevler, teğet doğrusu, gradyan (∇f) ve Hessian matrisi analizi.");

            this.pnlContent = new Panel();
            this.pnlTopSettings = new Panel();
            this.lblStepH = new Label();
            this.txtStepH = new TextBox();
            this.btnH1 = new Button();
            this.btnH2 = new Button();
            this.btnH3 = new Button();
            this.lblFormula = new Label();
            this.cmbFormula = new ComboBox();
            this.lblAcc = new Label();
            this.cmbAccuracy = new ComboBox();

            this.pnlSplit = new Panel();
            this.grpSingle = new Panel();
            this.grpPartial = new Panel();

            // Single
            this.lblSingleTitle = new Label();
            this.lblSingleFn = new Label();
            this.txtSingleFn = new TextBox();
            this.lblSingleVar = new Label();
            this.txtSingleVar = new TextBox();
            this.lblSinglePoint = new Label();
            this.txtSinglePoint = new TextBox();
            this.lblSingleDeg = new Label();
            this.numSingleDegree = new NumericUpDown();
            this.btnCalcSingle = new Button();
            this.btnPlotTangent = new Button();
            this.pnlSingleExamples = new FlowLayoutPanel();
            this.btnExSingle1 = ThemeHelper.CreateExampleChip("Örnek: sin(x)+x³");
            this.btnExSingle2 = ThemeHelper.CreateExampleChip("Örnek: exp(2x)");

            // Partial
            this.lblPartialTitle = new Label();
            this.lblPartialFn = new Label();
            this.txtPartialFn = new TextBox();
            this.lblTargetVar = new Label();
            this.txtTargetVar = new TextBox();
            this.lblDirectionVector = new Label();
            this.txtDirectionVector = new TextBox();
            this.pnlPartialVars = new FlowLayoutPanel();
            this.btnAddVar = new Button();
            this.btnCalcPartial = new Button();
            this.btnCalcHessian = new Button();
            this.pnlPartialExamples = new FlowLayoutPanel();
            this.btnExPartial1 = ThemeHelper.CreateExampleChip("Örnek: x²y + sin(y)");
            this.btnExPartial2 = ThemeHelper.CreateExampleChip("Örnek: exp(x*y)");

            // Tab Results
            this.tabResults = new TabControl();
            this.tabSummary = new TabPage();
            this.tabHessian = new TabPage();
            this.tabLog = new TabPage();

            this.pnlKpiCard = new Panel();
            this.lblKpiTitle = new Label();
            this.lblKpiValue = new Label();
            this.lblKpiSubtitle = new Label();
            this.txtSummaryView = new RichTextBox();

            this.dgvHessian = new DataGridView();
            this.lblHessianAnalysis = new Label();

            this.pnlLogToolbar = new Panel();
            this.btnCopyLog = ThemeHelper.CreateCopyButton("📋 Kopyala");
            this.txtResult = new TextBox();

            this.pnlContent.SuspendLayout();
            this.pnlTopSettings.SuspendLayout();
            this.pnlSplit.SuspendLayout();
            this.grpSingle.SuspendLayout();
            this.grpPartial.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tabSummary.SuspendLayout();
            this.tabHessian.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.pnlKpiCard.SuspendLayout();
            this.pnlLogToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSingleDegree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHessian)).BeginInit();
            this.SuspendLayout();

            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Padding = new Padding(16, 8, 16, 12);
            this.pnlContent.AutoScroll = true;

            // ==========================================
            // Top Settings Bar
            // ==========================================
            this.pnlTopSettings.Dock = DockStyle.Top;
            this.pnlTopSettings.Height = 44;
            this.pnlTopSettings.BackColor = Color.FromArgb(22, 32, 50);
            this.pnlTopSettings.Padding = new Padding(8);

            this.lblStepH.Text = "Adım (h):";
            this.lblStepH.AutoSize = true;
            this.lblStepH.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblStepH.Font = new Font("Segoe UI", 9F);
            this.lblStepH.Location = new Point(8, 12);

            this.txtStepH.Location = new Point(68, 9);
            this.txtStepH.Size = new Size(65, 23);
            this.txtStepH.Text = "0.001";
            this.txtStepH.Font = new Font("Consolas", 9.5F);
            this.txtStepH.BackColor = Color.FromArgb(15, 23, 42);
            this.txtStepH.ForeColor = Color.White;
            this.txtStepH.BorderStyle = BorderStyle.FixedSingle;

            void StyleQuickH(Button b, string text, Point loc)
            {
                b.Text = text;
                b.Location = loc;
                b.Size = new Size(42, 23);
                b.FlatStyle = FlatStyle.Flat;
                b.BackColor = Color.FromArgb(30, 41, 59);
                b.ForeColor = Color.FromArgb(56, 189, 248);
                b.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
                b.FlatAppearance.BorderSize = 0;
            }

            StyleQuickH(this.btnH1, "1e-3", new Point(138, 9));
            StyleQuickH(this.btnH2, "1e-5", new Point(183, 9));
            StyleQuickH(this.btnH3, "1e-7", new Point(228, 9));

            this.lblFormula.Text = "Fark Yöntemi:";
            this.lblFormula.AutoSize = true;
            this.lblFormula.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblFormula.Font = new Font("Segoe UI", 9F);
            this.lblFormula.Location = new Point(285, 12);

            this.cmbFormula.Items.AddRange(new object[] {
                "Merkezi Farklar (Central — En Yüksek Doğruluk)",
                "İleri Farklar (Forward Differences)",
                "Geri Farklar (Backward Differences)"
            });
            this.cmbFormula.SelectedIndex = 0;
            this.cmbFormula.Location = new Point(370, 9);
            this.cmbFormula.Size = new Size(250, 23);
            this.cmbFormula.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFormula.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbFormula.ForeColor = Color.White;
            this.cmbFormula.Font = new Font("Segoe UI", 8.5F);

            this.lblAcc.Text = "Doğruluk:";
            this.lblAcc.AutoSize = true;
            this.lblAcc.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblAcc.Font = new Font("Segoe UI", 9F);
            this.lblAcc.Location = new Point(635, 12);

            this.cmbAccuracy.Items.AddRange(new object[] {
                "2. Derece O(h²)",
                "4. Derece O(h⁴) - Önerilen",
                "6. Derece O(h⁶) - Maksimum"
            });
            this.cmbAccuracy.SelectedIndex = 1;
            this.cmbAccuracy.Location = new Point(700, 9);
            this.cmbAccuracy.Size = new Size(160, 23);
            this.cmbAccuracy.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccuracy.BackColor = Color.FromArgb(15, 23, 42);
            this.cmbAccuracy.ForeColor = Color.White;
            this.cmbAccuracy.Font = new Font("Segoe UI", 8.5F);

            this.pnlTopSettings.Controls.Add(this.lblStepH);
            this.pnlTopSettings.Controls.Add(this.txtStepH);
            this.pnlTopSettings.Controls.Add(this.btnH1);
            this.pnlTopSettings.Controls.Add(this.btnH2);
            this.pnlTopSettings.Controls.Add(this.btnH3);
            this.pnlTopSettings.Controls.Add(this.lblFormula);
            this.pnlTopSettings.Controls.Add(this.cmbFormula);
            this.pnlTopSettings.Controls.Add(this.lblAcc);
            this.pnlTopSettings.Controls.Add(this.cmbAccuracy);

            // ==========================================
            // Split Panel (Left: Single, Right: Partial)
            // ==========================================
            this.pnlSplit.Dock = DockStyle.Top;
            this.pnlSplit.Height = 230;
            this.pnlSplit.BackColor = Color.Transparent;
            this.pnlSplit.Padding = new Padding(0, 8, 0, 0);

            // Left Card (Single Variable)
            this.grpSingle.Dock = DockStyle.Left;
            this.grpSingle.Width = 430;
            this.grpSingle.BackColor = Color.FromArgb(22, 32, 50);
            this.grpSingle.Padding = new Padding(12);

            this.lblSingleTitle.Text = "📐 1. Tek Değişkenli Türev Laboratuvarı: f(x)";
            this.lblSingleTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblSingleTitle.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblSingleTitle.Location = new Point(10, 8);
            this.lblSingleTitle.AutoSize = true;

            this.lblSingleFn.Text = "f(x) =";
            this.lblSingleFn.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblSingleFn.Location = new Point(12, 36);
            this.lblSingleFn.AutoSize = true;

            this.txtSingleFn.Text = "sin(x) + x^3";
            this.txtSingleFn.Location = new Point(55, 33);
            this.txtSingleFn.Size = new Size(355, 24);
            this.txtSingleFn.Font = new Font("Consolas", 10F);
            this.txtSingleFn.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSingleFn.ForeColor = Color.White;
            this.txtSingleFn.BorderStyle = BorderStyle.FixedSingle;

            this.lblSingleVar.Text = "Değişken:";
            this.lblSingleVar.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblSingleVar.Location = new Point(12, 68);
            this.lblSingleVar.AutoSize = true;

            this.txtSingleVar.Text = "x";
            this.txtSingleVar.Location = new Point(75, 65);
            this.txtSingleVar.Size = new Size(40, 23);
            this.txtSingleVar.Font = new Font("Consolas", 10F);
            this.txtSingleVar.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSingleVar.ForeColor = Color.White;
            this.txtSingleVar.BorderStyle = BorderStyle.FixedSingle;

            this.lblSinglePoint.Text = "Nokta (x₀):";
            this.lblSinglePoint.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblSinglePoint.Location = new Point(125, 68);
            this.lblSinglePoint.AutoSize = true;

            this.txtSinglePoint.Text = "1";
            this.txtSinglePoint.Location = new Point(190, 65);
            this.txtSinglePoint.Size = new Size(60, 23);
            this.txtSinglePoint.Font = new Font("Consolas", 10F);
            this.txtSinglePoint.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSinglePoint.ForeColor = Color.White;
            this.txtSinglePoint.BorderStyle = BorderStyle.FixedSingle;

            this.lblSingleDeg.Text = "Mertebe:";
            this.lblSingleDeg.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblSingleDeg.Location = new Point(265, 68);
            this.lblSingleDeg.AutoSize = true;

            this.numSingleDegree.Location = new Point(325, 65);
            this.numSingleDegree.Size = new Size(45, 23);
            this.numSingleDegree.Minimum = 1;
            this.numSingleDegree.Maximum = 6;
            this.numSingleDegree.Value = 1;
            this.numSingleDegree.BackColor = Color.FromArgb(15, 23, 42);
            this.numSingleDegree.ForeColor = Color.White;

            this.btnCalcSingle.Text = "∂ Türevi Hesapla";
            this.btnCalcSingle.Location = new Point(12, 100);
            this.btnCalcSingle.Size = new Size(195, 32);
            this.btnCalcSingle.FlatStyle = FlatStyle.Flat;
            this.btnCalcSingle.BackColor = Color.FromArgb(14, 165, 233);
            this.btnCalcSingle.ForeColor = Color.White;
            this.btnCalcSingle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnCalcSingle.Cursor = Cursors.Hand;
            this.btnCalcSingle.FlatAppearance.BorderSize = 0;

            this.btnPlotTangent.Text = "📈 Teğeti Grafikte Çiz";
            this.btnPlotTangent.Location = new Point(215, 100);
            this.btnPlotTangent.Size = new Size(195, 32);
            this.btnPlotTangent.FlatStyle = FlatStyle.Flat;
            this.btnPlotTangent.BackColor = Color.FromArgb(16, 185, 129);
            this.btnPlotTangent.ForeColor = Color.White;
            this.btnPlotTangent.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.btnPlotTangent.Cursor = Cursors.Hand;
            this.btnPlotTangent.FlatAppearance.BorderSize = 0;

            this.pnlSingleExamples.Location = new Point(12, 142);
            this.pnlSingleExamples.Size = new Size(400, 32);
            this.pnlSingleExamples.BackColor = Color.Transparent;
            this.pnlSingleExamples.Controls.Add(this.btnExSingle1);
            this.pnlSingleExamples.Controls.Add(this.btnExSingle2);

            this.grpSingle.Controls.Add(this.lblSingleTitle);
            this.grpSingle.Controls.Add(this.lblSingleFn);
            this.grpSingle.Controls.Add(this.txtSingleFn);
            this.grpSingle.Controls.Add(this.lblSingleVar);
            this.grpSingle.Controls.Add(this.txtSingleVar);
            this.grpSingle.Controls.Add(this.lblSinglePoint);
            this.grpSingle.Controls.Add(this.txtSinglePoint);
            this.grpSingle.Controls.Add(this.lblSingleDeg);
            this.grpSingle.Controls.Add(this.numSingleDegree);
            this.grpSingle.Controls.Add(this.btnCalcSingle);
            this.grpSingle.Controls.Add(this.btnPlotTangent);
            this.grpSingle.Controls.Add(this.pnlSingleExamples);

            // Right Card (Multivariable, Gradient & Hessian)
            this.grpPartial.Dock = DockStyle.Fill;
            this.grpPartial.BackColor = Color.FromArgb(22, 32, 50);
            this.grpPartial.Padding = new Padding(12);

            this.lblPartialTitle.Text = "🌐 2. Çok Değişkenli Analiz, Gradyan (∇f) & Hessian";
            this.lblPartialTitle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblPartialTitle.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblPartialTitle.Location = new Point(10, 8);
            this.lblPartialTitle.AutoSize = true;

            this.lblPartialFn.Text = "f(...) =";
            this.lblPartialFn.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblPartialFn.Location = new Point(12, 36);
            this.lblPartialFn.AutoSize = true;

            this.txtPartialFn.Text = "x^2*y + sin(y)";
            this.txtPartialFn.Location = new Point(55, 33);
            this.txtPartialFn.Size = new Size(185, 24);
            this.txtPartialFn.Font = new Font("Consolas", 10F);
            this.txtPartialFn.BackColor = Color.FromArgb(15, 23, 42);
            this.txtPartialFn.ForeColor = Color.White;
            this.txtPartialFn.BorderStyle = BorderStyle.FixedSingle;

            this.lblTargetVar.Text = "Hedef ∂/∂:";
            this.lblTargetVar.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblTargetVar.Location = new Point(245, 36);
            this.lblTargetVar.AutoSize = true;

            this.txtTargetVar.Text = "x";
            this.txtTargetVar.Location = new Point(315, 33);
            this.txtTargetVar.Size = new Size(35, 23);
            this.txtTargetVar.Font = new Font("Consolas", 10F, FontStyle.Bold);
            this.txtTargetVar.BackColor = Color.FromArgb(15, 23, 42);
            this.txtTargetVar.ForeColor = Color.FromArgb(56, 189, 248);
            this.txtTargetVar.BorderStyle = BorderStyle.FixedSingle;
            this.txtTargetVar.TextAlign = HorizontalAlignment.Center;

            this.lblDirectionVector.Text = "Yön u:";
            this.lblDirectionVector.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblDirectionVector.Location = new Point(355, 36);
            this.lblDirectionVector.AutoSize = true;

            this.txtDirectionVector.Text = "1, 0";
            this.txtDirectionVector.Location = new Point(398, 33);
            this.txtDirectionVector.Size = new Size(50, 23);
            this.txtDirectionVector.Font = new Font("Consolas", 9F);
            this.txtDirectionVector.BackColor = Color.FromArgb(15, 23, 42);
            this.txtDirectionVector.ForeColor = Color.FromArgb(250, 204, 21);
            this.txtDirectionVector.BorderStyle = BorderStyle.FixedSingle;

            // Partial Variables List
            this.pnlPartialVars.Location = new Point(12, 65);
            this.pnlPartialVars.Size = new Size(436, 80);
            this.pnlPartialVars.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.pnlPartialVars.AutoScroll = true;
            this.pnlPartialVars.BackColor = Color.FromArgb(15, 23, 42);

            this.btnAddVar.Text = "+ Değişken";
            this.btnAddVar.Location = new Point(12, 150);
            this.btnAddVar.Size = new Size(90, 30);
            this.btnAddVar.FlatStyle = FlatStyle.Flat;
            this.btnAddVar.BackColor = Color.FromArgb(30, 41, 59);
            this.btnAddVar.ForeColor = Color.FromArgb(56, 189, 248);
            this.btnAddVar.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnAddVar.Cursor = Cursors.Hand;
            this.btnAddVar.FlatAppearance.BorderSize = 0;

            this.btnCalcPartial.Text = "∇ Gradyan & Kısmi";
            this.btnCalcPartial.Location = new Point(110, 150);
            this.btnCalcPartial.Size = new Size(160, 30);
            this.btnCalcPartial.FlatStyle = FlatStyle.Flat;
            this.btnCalcPartial.BackColor = Color.FromArgb(16, 185, 129);
            this.btnCalcPartial.ForeColor = Color.White;
            this.btnCalcPartial.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnCalcPartial.Cursor = Cursors.Hand;
            this.btnCalcPartial.FlatAppearance.BorderSize = 0;

            this.btnCalcHessian.Text = "🧮 Hessian & Ekstremum";
            this.btnCalcHessian.Location = new Point(278, 150);
            this.btnCalcHessian.Size = new Size(165, 30);
            this.btnCalcHessian.FlatStyle = FlatStyle.Flat;
            this.btnCalcHessian.BackColor = Color.FromArgb(147, 51, 234);
            this.btnCalcHessian.ForeColor = Color.White;
            this.btnCalcHessian.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.btnCalcHessian.Cursor = Cursors.Hand;
            this.btnCalcHessian.FlatAppearance.BorderSize = 0;

            this.pnlPartialExamples.Location = new Point(12, 188);
            this.pnlPartialExamples.Size = new Size(430, 32);
            this.pnlPartialExamples.BackColor = Color.Transparent;
            this.pnlPartialExamples.Controls.Add(this.btnExPartial1);
            this.pnlPartialExamples.Controls.Add(this.btnExPartial2);

            this.grpPartial.Controls.Add(this.lblPartialTitle);
            this.grpPartial.Controls.Add(this.lblPartialFn);
            this.grpPartial.Controls.Add(this.txtPartialFn);
            this.grpPartial.Controls.Add(this.lblTargetVar);
            this.grpPartial.Controls.Add(this.txtTargetVar);
            this.grpPartial.Controls.Add(this.lblDirectionVector);
            this.grpPartial.Controls.Add(this.txtDirectionVector);
            this.grpPartial.Controls.Add(this.pnlPartialVars);
            this.grpPartial.Controls.Add(this.btnAddVar);
            this.grpPartial.Controls.Add(this.btnCalcPartial);
            this.grpPartial.Controls.Add(this.btnCalcHessian);
            this.grpPartial.Controls.Add(this.pnlPartialExamples);

            // Add cards to Split
            Panel spBetween = new Panel { Dock = DockStyle.Left, Width = 12, BackColor = Color.Transparent };
            this.pnlSplit.Controls.Add(this.grpPartial);
            this.pnlSplit.Controls.Add(spBetween);
            this.pnlSplit.Controls.Add(this.grpSingle);

            // ==========================================
            // Bottom Tabbed Area
            // ==========================================
            this.tabResults.Dock = DockStyle.Fill;
            this.tabResults.ItemSize = new Size(160, 26);
            this.tabResults.SizeMode = TabSizeMode.Fixed;

            this.tabSummary.Text = "  Türev & Gradyan Kartı  ";
            this.tabSummary.BackColor = Color.FromArgb(15, 23, 42);
            this.tabSummary.Padding = new Padding(8);

            this.tabHessian.Text = "  Hessian Matrisi Tablosu  ";
            this.tabHessian.BackColor = Color.FromArgb(15, 23, 42);
            this.tabHessian.Padding = new Padding(8);

            this.tabLog.Text = "  Detaylı Analitik Rapor  ";
            this.tabLog.BackColor = Color.FromArgb(15, 23, 42);
            this.tabLog.Padding = new Padding(8);

            // Tab 1 (Summary)
            this.pnlKpiCard.Dock = DockStyle.Top;
            this.pnlKpiCard.Height = 85;
            this.pnlKpiCard.BackColor = Color.FromArgb(22, 32, 50);
            this.pnlKpiCard.Padding = new Padding(12);

            this.lblKpiTitle.Text = "TÜREV ANALİZ SONUCU";
            this.lblKpiTitle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            this.lblKpiTitle.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblKpiTitle.Location = new Point(12, 8);
            this.lblKpiTitle.AutoSize = true;

            this.lblKpiValue.Text = "—";
            this.lblKpiValue.Font = new Font("Consolas", 18F, FontStyle.Bold);
            this.lblKpiValue.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblKpiValue.Location = new Point(10, 24);
            this.lblKpiValue.AutoSize = true;

            this.lblKpiSubtitle.Text = "Hesaplama yapmak için yukarıdaki modüllerden birini çalıştırın.";
            this.lblKpiSubtitle.Font = new Font("Segoe UI", 8.25F);
            this.lblKpiSubtitle.ForeColor = Color.FromArgb(203, 213, 225);
            this.lblKpiSubtitle.Location = new Point(12, 60);
            this.lblKpiSubtitle.AutoSize = true;

            this.pnlKpiCard.Controls.Add(this.lblKpiTitle);
            this.pnlKpiCard.Controls.Add(this.lblKpiValue);
            this.pnlKpiCard.Controls.Add(this.lblKpiSubtitle);

            this.txtSummaryView.Dock = DockStyle.Fill;
            this.txtSummaryView.BackColor = Color.FromArgb(15, 23, 42);
            this.txtSummaryView.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtSummaryView.Font = new Font("Consolas", 9.5F);
            this.txtSummaryView.BorderStyle = BorderStyle.None;
            this.txtSummaryView.ReadOnly = true;

            Panel spKpi = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };
            this.tabSummary.Controls.Add(this.txtSummaryView);
            this.tabSummary.Controls.Add(spKpi);
            this.tabSummary.Controls.Add(this.pnlKpiCard);

            // Tab 2 (Hessian Grid)
            this.lblHessianAnalysis.Dock = DockStyle.Top;
            this.lblHessianAnalysis.Height = 35;
            this.lblHessianAnalysis.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblHessianAnalysis.ForeColor = Color.FromArgb(250, 204, 21);
            this.lblHessianAnalysis.Text = "Hessian matris analizi için 'Hessian & Ekstremum' butonuna tıklayınız.";

            this.dgvHessian.Dock = DockStyle.Fill;
            this.dgvHessian.BackgroundColor = Color.FromArgb(15, 23, 42);
            this.dgvHessian.BorderStyle = BorderStyle.None;
            this.dgvHessian.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.dgvHessian.GridColor = Color.FromArgb(47, 63, 86);
            this.dgvHessian.RowHeadersVisible = true;
            this.dgvHessian.AllowUserToAddRows = false;
            this.dgvHessian.AllowUserToDeleteRows = false;
            this.dgvHessian.ReadOnly = true;
            this.dgvHessian.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHessian.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            this.dgvHessian.DefaultCellStyle.ForeColor = Color.White;
            this.dgvHessian.DefaultCellStyle.Font = new Font("Consolas", 10F, FontStyle.Bold);
            this.dgvHessian.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvHessian.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            this.dgvHessian.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(56, 189, 248);
            this.dgvHessian.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.dgvHessian.EnableHeadersVisualStyles = false;

            this.tabHessian.Controls.Add(this.dgvHessian);
            this.tabHessian.Controls.Add(this.lblHessianAnalysis);

            // Tab 3 (Log)
            this.pnlLogToolbar.Dock = DockStyle.Top;
            this.pnlLogToolbar.Height = 34;
            this.pnlLogToolbar.BackColor = Color.Transparent;

            this.btnCopyLog.Dock = DockStyle.Right;
            this.btnCopyLog.Width = 85;
            this.pnlLogToolbar.Controls.Add(this.btnCopyLog);

            this.txtResult.Dock = DockStyle.Fill;
            this.txtResult.BackColor = Color.FromArgb(15, 23, 42);
            this.txtResult.ForeColor = Color.FromArgb(186, 230, 253);
            this.txtResult.BorderStyle = BorderStyle.None;
            this.txtResult.Multiline = true;
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = ScrollBars.Vertical;
            this.txtResult.Font = new Font("Consolas", 9.5F);

            this.tabLog.Controls.Add(this.txtResult);
            this.tabLog.Controls.Add(this.pnlLogToolbar);

            this.tabResults.TabPages.Add(this.tabSummary);
            this.tabResults.TabPages.Add(this.tabHessian);
            this.tabResults.TabPages.Add(this.tabLog);

            // Add main panels
            Panel spMain = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };
            this.pnlContent.Controls.Add(this.tabResults);
            this.pnlContent.Controls.Add(spMain);
            this.pnlContent.Controls.Add(this.pnlSplit);
            this.pnlContent.Controls.Add(this.pnlTopSettings);

            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Name = "DerivativeCalculatorControl";
            this.Size = new Size(930, 700);

            this.pnlContent.ResumeLayout(false);
            this.pnlTopSettings.ResumeLayout(false);
            this.pnlTopSettings.PerformLayout();
            this.pnlSplit.ResumeLayout(false);
            this.grpSingle.ResumeLayout(false);
            this.grpSingle.PerformLayout();
            this.grpPartial.ResumeLayout(false);
            this.grpPartial.PerformLayout();
            this.pnlKpiCard.ResumeLayout(false);
            this.pnlKpiCard.PerformLayout();
            this.pnlLogToolbar.ResumeLayout(false);
            this.tabSummary.ResumeLayout(false);
            this.tabHessian.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.tabResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSingleDegree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHessian)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
