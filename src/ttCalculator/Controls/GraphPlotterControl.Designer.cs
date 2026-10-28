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
                "Gelişmiş Çoklu Fonksiyon ve Geometri Grafiği Stüdyosu",
                "Çoklu katmanlı eğriler, kutupsal (polar) fonksiyonlar, otomatik kritik nokta tespiti (kök, extremum, büküm), teğet inceleme ve parametre sürgüleri.");

            this.pnlControls = new Panel();
            this.pnlPresets = new FlowLayoutPanel();
            this.lblPresets = new Label();
            this.btnEx1 = ThemeHelper.CreateCompactChip("sin(x) * x");
            this.btnEx2 = ThemeHelper.CreateCompactChip("x³ - 3x");
            this.btnEx3 = ThemeHelper.CreateCompactChip("e^(-x²/4)*cos(2x)");
            this.btnEx4 = ThemeHelper.CreateCompactChip("1 / (x² - 1)");

            this.lblEquation = new Label();
            this.txtEquation = new TextBox();
            this.chkPolar = new CheckBox();
            this.lblMinX = new Label();
            this.txtMinX = new TextBox();
            this.lblMaxX = new Label();
            this.txtMaxX = new TextBox();
            this.chkClampRange = new CheckBox();
            this.btnPlot = ThemeHelper.CreateActionButton("+ Ekle / Çiz", 96, 27, true);

            // Scale & Analysis Toolbar buttons (Segmented Style)
            this.btnZoomXOut = ThemeHelper.CreateSegmentedButton("X −", 46, 26);
            this.btnZoomXIn = ThemeHelper.CreateSegmentedButton("X +", 46, 26);
            this.btnZoomX10 = ThemeHelper.CreateSegmentedButton("10×", 42, 26);
            this.pnlSep1 = new Panel();
            this.btnZoomYOut = ThemeHelper.CreateSegmentedButton("Y −", 46, 26);
            this.btnZoomYIn = ThemeHelper.CreateSegmentedButton("Y +", 46, 26);
            this.pnlSep2 = new Panel();
            this.btnAspect1to1 = ThemeHelper.CreateSegmentedButton("1:1", 46, 26);
            this.btnResetView = ThemeHelper.CreateSegmentedButton("Sıfırla", 58, 26);
            this.pnlSepMain = new Panel();
            this.pnlControlsBorder = new Panel();
            this.chkCriticalPoints = new CheckBox();
            this.chkInspectMode = new CheckBox();
            this.chkShowAsymptotes = new CheckBox();

            this.pnlRightSide = new Panel();
            this.tabManager = new TabControl();
            this.tabFunctions = new TabPage();
            this.tabPoints = new TabPage();
            this.tabSliders = new TabPage();
            this.tabTools = new TabPage();
            this.tabShapes = new TabPage();

            // Function Tab Controls
            this.lstCurves = new ListBox();
            this.btnCurveColor = new Button();
            this.cmbCurveStyle = new ComboBox();
            this.cmbCurveThickness = new ComboBox();
            this.btnCurveToggle = new Button();
            this.btnCurveDelete = new Button();
            this.btnCurveEdit = new Button();
            this.btnCurveClearAll = new Button();

            // Points Tab Controls
            this.lstPoints = new ListBox();
            this.lblPtX = new Label();
            this.txtPtX = new TextBox();
            this.lblPtY = new Label();
            this.txtPtY = new TextBox();
            this.lblPtShape = new Label();
            this.cmbPtShape = new ComboBox();
            this.btnPtColor = new Button();
            this.btnAddPoint = new Button();
            this.btnDeletePoint = new Button();
            this.btnClearPoints = new Button();
            this.lblPointHint = new Label();

            // Sliders Tab Controls
            this.lblParamA = new Label();
            this.tbParamA = new TrackBar();
            this.lblParamB = new Label();
            this.tbParamB = new TrackBar();
            this.lblParamC = new Label();
            this.tbParamC = new TrackBar();
            this.lblParamK = new Label();
            this.tbParamK = new TrackBar();
            this.btnAnimate = ThemeHelper.CreateActionButton("Canlı Dalga Animasyonu", 284, 30, true);
            this.btnResetParams = ThemeHelper.CreateActionButton("Parametreleri Sıfırla", 284, 28, false);
            this.lblSliderHint = new Label();

            // Tools Tab Controls
            this.lblToolF1 = new Label();
            this.cmbToolF1 = new ComboBox();
            this.lblToolF2 = new Label();
            this.cmbToolF2 = new ComboBox();
            this.btnCommonTangent = ThemeHelper.CreateActionButton("Ortak Teğet Doğrusunu Bul", 284, 28, true);
            this.btnNearestPoints = ThemeHelper.CreateActionButton("En Yakın Noktalar ve Doğru", 284, 28, false);
            this.lblToolSep = new Label();
            this.lblToolPointTan = new Label();
            this.txtTanPointX = new TextBox();
            this.txtTanPointY = new TextBox();
            this.btnPointTangent = ThemeHelper.CreateActionButton("Noktadan Teğet Çiz", 284, 28, false);
            this.txtToolLog = new RichTextBox();
            this.btnExportImage = ThemeHelper.CreateActionButton("Grafiği Resim (PNG) Kaydet", 284, 28, false);
            this.btnExportCsv = ThemeHelper.CreateActionButton("Değer Tablosunu CSV İndir", 284, 28, false);
            this.btnExportLatex = ThemeHelper.CreateActionButton("LaTeX (TikZ) Kodu Üret", 284, 28, false);

            // Shapes Tab Controls
            this.lblShapeType = new Label();
            this.cmbShapeType = new ComboBox();
            this.lblShapeCenter = new Label();
            this.txtShapeCenterX = new TextBox();
            this.txtShapeCenterY = new TextBox();
            this.lblShapeDim1 = new Label();
            this.txtShapeDim1 = new TextBox();
            this.lblShapeDim2 = new Label();
            this.txtShapeDim2 = new TextBox();
            this.lblShapeAngle = new Label();
            this.txtShapeAngle = new TextBox();
            this.btnAddShape = ThemeHelper.CreateActionButton("Şekli Grafiğe Ekle", 284, 32, true);

            this.pnlStatusBar = new Panel();
            this.lblStatus = new Label();
            this.pnlCanvas = new Panel();

            this.pnlControls.SuspendLayout();
            this.pnlRightSide.SuspendLayout();
            this.tabManager.SuspendLayout();
            this.tabFunctions.SuspendLayout();
            this.tabPoints.SuspendLayout();
            this.tabSliders.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.tabShapes.SuspendLayout();
            this.pnlStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamK)).BeginInit();
            this.SuspendLayout();

            // pnlHeader: Spatially balanced, unclipped
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 72;

            // pnlControls: Modern Card Container (#111827 / CardBackground)
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 84;
            this.pnlControls.BackColor = Color.FromArgb(17, 24, 39);
            this.pnlControls.Padding = new Padding(14, 8, 14, 8);

            // ==================== ROW 1: Function Input & Quick Presets ====================
            int yRow1 = 10;
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblEquation.Text = "f(x) =";
            this.lblEquation.Location = new Point(14, yRow1 + 3);

            this.txtEquation.Location = new Point(56, yRow1);
            this.txtEquation.Size = new Size(180, 25);
            this.txtEquation.Text = "sin(x)";
            this.txtEquation.Font = new Font("Consolas", 10.5F, FontStyle.Bold);
            this.txtEquation.BackColor = Color.FromArgb(10, 17, 30);
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;

            this.chkPolar.Text = "Polar r(θ)";
            this.chkPolar.ForeColor = Color.FromArgb(56, 189, 248);
            this.chkPolar.Font = new Font("Segoe UI", 9F);
            this.chkPolar.Location = new Point(242, yRow1 + 2);
            this.chkPolar.Size = new Size(78, 22);

            this.lblMinX.AutoSize = true;
            this.lblMinX.ForeColor = ThemeHelper.MutedText;
            this.lblMinX.Font = new Font("Segoe UI", 9F);
            this.lblMinX.Text = "Min:";
            this.lblMinX.Location = new Point(326, yRow1 + 3);

            this.txtMinX.Location = new Point(358, yRow1);
            this.txtMinX.Size = new Size(42, 25);
            this.txtMinX.Text = "-10";
            this.txtMinX.Font = new Font("Consolas", 9.5F);
            this.txtMinX.BackColor = Color.FromArgb(10, 17, 30);
            this.txtMinX.ForeColor = Color.White;
            this.txtMinX.BorderStyle = BorderStyle.FixedSingle;

            this.lblMaxX.AutoSize = true;
            this.lblMaxX.ForeColor = ThemeHelper.MutedText;
            this.lblMaxX.Font = new Font("Segoe UI", 9F);
            this.lblMaxX.Text = "Max:";
            this.lblMaxX.Location = new Point(406, yRow1 + 3);

            this.txtMaxX.Location = new Point(440, yRow1);
            this.txtMaxX.Size = new Size(42, 25);
            this.txtMaxX.Text = "10";
            this.txtMaxX.Font = new Font("Consolas", 9.5F);
            this.txtMaxX.BackColor = Color.FromArgb(10, 17, 30);
            this.txtMaxX.ForeColor = Color.White;
            this.txtMaxX.BorderStyle = BorderStyle.FixedSingle;

            this.chkClampRange.Text = "Kısıtla";
            this.chkClampRange.ForeColor = ThemeHelper.LightText;
            this.chkClampRange.Font = new Font("Segoe UI", 9F);
            this.chkClampRange.Location = new Point(488, yRow1 + 2);
            this.chkClampRange.Size = new Size(64, 22);

            this.btnPlot.Location = new Point(558, yRow1 - 1);

            // Presets (Quick Chips)
            this.lblPresets.Text = "Hızlı:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 4, 6, 0);

            this.pnlPresets.Location = new Point(668, yRow1);
            this.pnlPresets.Size = new Size(460, 26);
            this.pnlPresets.BackColor = Color.Transparent;
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);

            // ==================== ROW 2: Axis Navigation & Live Analysis ====================
            int yRow2 = 46;
            this.btnZoomXOut.Location = new Point(14, yRow2);
            this.btnZoomXIn.Location = new Point(62, yRow2);
            this.btnZoomX10.Location = new Point(110, yRow2);

            this.pnlSep1.Location = new Point(158, yRow2 + 2);
            this.pnlSep1.Size = new Size(1, 22);
            this.pnlSep1.BackColor = ThemeHelper.BorderColor;

            this.btnZoomYOut.Location = new Point(165, yRow2);
            this.btnZoomYIn.Location = new Point(213, yRow2);

            this.pnlSep2.Location = new Point(265, yRow2 + 2);
            this.pnlSep2.Size = new Size(1, 22);
            this.pnlSep2.BackColor = ThemeHelper.BorderColor;

            this.btnAspect1to1.Location = new Point(272, yRow2);
            this.btnResetView.Location = new Point(320, yRow2);

            this.pnlSepMain.Location = new Point(384, yRow2 + 2);
            this.pnlSepMain.Size = new Size(1, 22);
            this.pnlSepMain.BackColor = ThemeHelper.BorderLight;

            this.chkCriticalPoints.Text = "● Kritik Noktalar";
            this.chkCriticalPoints.ForeColor = Color.FromArgb(52, 211, 153);
            this.chkCriticalPoints.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            this.chkCriticalPoints.Location = new Point(396, yRow2 + 2);
            this.chkCriticalPoints.Size = new Size(140, 22);

            this.chkInspectMode.Text = "● Teğet İncele";
            this.chkInspectMode.ForeColor = Color.FromArgb(56, 189, 248);
            this.chkInspectMode.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            this.chkInspectMode.Location = new Point(542, yRow2 + 2);
            this.chkInspectMode.Size = new Size(115, 22);

            this.chkShowAsymptotes.Text = "● Asimptotlar";
            this.chkShowAsymptotes.ForeColor = Color.FromArgb(244, 63, 94);
            this.chkShowAsymptotes.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            this.chkShowAsymptotes.Location = new Point(664, yRow2 + 2);
            this.chkShowAsymptotes.Size = new Size(110, 22);

            this.pnlControlsBorder.Dock = DockStyle.Bottom;
            this.pnlControlsBorder.Height = 1;
            this.pnlControlsBorder.BackColor = ThemeHelper.BorderColor;

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEquation);
            this.pnlControls.Controls.Add(this.txtEquation);
            this.pnlControls.Controls.Add(this.chkPolar);
            this.pnlControls.Controls.Add(this.lblMinX);
            this.pnlControls.Controls.Add(this.txtMinX);
            this.pnlControls.Controls.Add(this.lblMaxX);
            this.pnlControls.Controls.Add(this.txtMaxX);
            this.pnlControls.Controls.Add(this.chkClampRange);
            this.pnlControls.Controls.Add(this.btnPlot);

            this.pnlControls.Controls.Add(this.btnZoomXOut);
            this.pnlControls.Controls.Add(this.btnZoomXIn);
            this.pnlControls.Controls.Add(this.btnZoomX10);
            this.pnlControls.Controls.Add(this.pnlSep1);
            this.pnlControls.Controls.Add(this.btnZoomYOut);
            this.pnlControls.Controls.Add(this.btnZoomYIn);
            this.pnlControls.Controls.Add(this.pnlSep2);
            this.pnlControls.Controls.Add(this.btnAspect1to1);
            this.pnlControls.Controls.Add(this.btnResetView);
            this.pnlControls.Controls.Add(this.pnlSepMain);
            this.pnlControls.Controls.Add(this.chkCriticalPoints);
            this.pnlControls.Controls.Add(this.chkInspectMode);
            this.pnlControls.Controls.Add(this.chkShowAsymptotes);
            this.pnlControls.Controls.Add(this.pnlControlsBorder);

            // pnlRightSide (Tabbed Manager)
            this.pnlRightSide.Dock = DockStyle.Right;
            this.pnlRightSide.Width = 320;
            this.pnlRightSide.BackColor = ThemeHelper.CardBackground;
            this.pnlRightSide.Padding = new Padding(4);

            this.tabManager.Dock = DockStyle.Fill;
            this.tabManager.Controls.Add(this.tabFunctions);
            this.tabManager.Controls.Add(this.tabPoints);
            this.tabManager.Controls.Add(this.tabSliders);
            this.tabManager.Controls.Add(this.tabTools);
            this.tabManager.Controls.Add(this.tabShapes);
            this.pnlRightSide.Controls.Add(this.tabManager);

            // tabFunctions
            this.tabFunctions.Text = "Eğriler";
            this.tabFunctions.BackColor = ThemeHelper.DarkBackground;
            this.tabFunctions.Padding = new Padding(8);

            this.lstCurves.Dock = DockStyle.Top;
            this.lstCurves.Height = 220;
            this.lstCurves.BackColor = Color.FromArgb(11, 17, 32);
            this.lstCurves.ForeColor = Color.White;
            this.lstCurves.Font = new Font("Consolas", 9.5F);

            this.btnCurveColor.Text = "Renk";
            this.btnCurveColor.Size = new Size(70, 26);
            this.btnCurveColor.Location = new Point(8, 230);
            this.btnCurveColor.BackColor = Color.FromArgb(30, 41, 59);
            this.btnCurveColor.ForeColor = Color.White;
            this.btnCurveColor.FlatStyle = FlatStyle.Flat;

            this.cmbCurveStyle.Location = new Point(82, 230);
            this.cmbCurveStyle.Size = new Size(110, 25);
            this.cmbCurveStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCurveStyle.BackColor = ThemeHelper.CardBackground;
            this.cmbCurveStyle.ForeColor = Color.White;

            this.cmbCurveThickness.Location = new Point(196, 230);
            this.cmbCurveThickness.Size = new Size(95, 25);
            this.cmbCurveThickness.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCurveThickness.BackColor = ThemeHelper.CardBackground;
            this.cmbCurveThickness.ForeColor = Color.White;

            this.btnCurveToggle.Text = "Göster/Gizle";
            this.btnCurveToggle.Size = new Size(135, 28);
            this.btnCurveToggle.Location = new Point(8, 265);
            this.btnCurveToggle.BackColor = Color.FromArgb(14, 116, 144);
            this.btnCurveToggle.ForeColor = Color.White;
            this.btnCurveToggle.FlatStyle = FlatStyle.Flat;

            this.btnCurveDelete.Text = "Eğriyi Sil";
            this.btnCurveDelete.Size = new Size(135, 28);
            this.btnCurveDelete.Location = new Point(155, 265);
            this.btnCurveDelete.BackColor = Color.FromArgb(185, 28, 28);
            this.btnCurveDelete.ForeColor = Color.White;
            this.btnCurveDelete.FlatStyle = FlatStyle.Flat;

            this.btnCurveEdit.Text = "Düzenle";
            this.btnCurveEdit.Size = new Size(135, 28);
            this.btnCurveEdit.Location = new Point(8, 300);
            this.btnCurveEdit.BackColor = Color.FromArgb(40, 58, 85);
            this.btnCurveEdit.ForeColor = Color.White;
            this.btnCurveEdit.FlatStyle = FlatStyle.Flat;

            this.btnCurveClearAll.Text = "Tümünü Sil";
            this.btnCurveClearAll.Size = new Size(135, 28);
            this.btnCurveClearAll.Location = new Point(155, 300);
            this.btnCurveClearAll.BackColor = Color.FromArgb(127, 29, 29);
            this.btnCurveClearAll.ForeColor = Color.White;
            this.btnCurveClearAll.FlatStyle = FlatStyle.Flat;

            this.tabFunctions.Controls.Add(this.lstCurves);
            this.tabFunctions.Controls.Add(this.btnCurveColor);
            this.tabFunctions.Controls.Add(this.cmbCurveStyle);
            this.tabFunctions.Controls.Add(this.cmbCurveThickness);
            this.tabFunctions.Controls.Add(this.btnCurveToggle);
            this.tabFunctions.Controls.Add(this.btnCurveDelete);
            this.tabFunctions.Controls.Add(this.btnCurveEdit);
            this.tabFunctions.Controls.Add(this.btnCurveClearAll);

            // tabPoints
            this.tabPoints.Text = "Noktalar";
            this.tabPoints.BackColor = ThemeHelper.DarkBackground;
            this.tabPoints.Padding = new Padding(8);

            this.lstPoints.Dock = DockStyle.Top;
            this.lstPoints.Height = 180;
            this.lstPoints.BackColor = Color.FromArgb(11, 17, 32);
            this.lstPoints.ForeColor = Color.White;
            this.lstPoints.Font = new Font("Consolas", 9.5F);

            this.lblPtX.Text = "X:";
            this.lblPtX.Location = new Point(8, 192);
            this.lblPtX.AutoSize = true;
            this.lblPtX.ForeColor = ThemeHelper.LightText;

            this.txtPtX.Location = new Point(28, 189);
            this.txtPtX.Size = new Size(60, 24);
            this.txtPtX.Text = "0";
            this.txtPtX.Font = new Font("Consolas", 9F);
            this.txtPtX.BackColor = ThemeHelper.CardBackground;
            this.txtPtX.ForeColor = Color.White;

            this.lblPtY.Text = "Y:";
            this.lblPtY.Location = new Point(96, 192);
            this.lblPtY.AutoSize = true;
            this.lblPtY.ForeColor = ThemeHelper.LightText;

            this.txtPtY.Location = new Point(116, 189);
            this.txtPtY.Size = new Size(60, 24);
            this.txtPtY.Text = "0";
            this.txtPtY.Font = new Font("Consolas", 9F);
            this.txtPtY.BackColor = ThemeHelper.CardBackground;
            this.txtPtY.ForeColor = Color.White;

            this.btnPtColor.Text = "Renk";
            this.btnPtColor.Location = new Point(184, 188);
            this.btnPtColor.Size = new Size(48, 25);
            this.btnPtColor.BackColor = Color.FromArgb(250, 204, 21);
            this.btnPtColor.FlatStyle = FlatStyle.Flat;

            this.btnAddPoint.Text = "+ Ekle";
            this.btnAddPoint.Location = new Point(236, 188);
            this.btnAddPoint.Size = new Size(58, 25);
            this.btnAddPoint.BackColor = ThemeHelper.AccentBlue;
            this.btnAddPoint.ForeColor = Color.White;
            this.btnAddPoint.FlatStyle = FlatStyle.Flat;

            this.lblPtShape.Text = "Şekil:";
            this.lblPtShape.Location = new Point(8, 225);
            this.lblPtShape.AutoSize = true;
            this.lblPtShape.ForeColor = ThemeHelper.LightText;

            this.cmbPtShape.Location = new Point(50, 222);
            this.cmbPtShape.Size = new Size(130, 24);
            this.cmbPtShape.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPtShape.BackColor = ThemeHelper.CardBackground;
            this.cmbPtShape.ForeColor = Color.White;

            this.btnDeletePoint.Text = "Seçiliyi Sil";
            this.btnDeletePoint.Location = new Point(8, 255);
            this.btnDeletePoint.Size = new Size(135, 27);
            this.btnDeletePoint.BackColor = Color.FromArgb(185, 28, 28);
            this.btnDeletePoint.ForeColor = Color.White;
            this.btnDeletePoint.FlatStyle = FlatStyle.Flat;

            this.btnClearPoints.Text = "Tümünü Sil";
            this.btnClearPoints.Location = new Point(152, 255);
            this.btnClearPoints.Size = new Size(135, 27);
            this.btnClearPoints.BackColor = Color.FromArgb(127, 29, 29);
            this.btnClearPoints.ForeColor = Color.White;
            this.btnClearPoints.FlatStyle = FlatStyle.Flat;

            this.lblPointHint.Text = "İpucu: Grafik tuvalinde bir noktanın üzerine sağ tıklayarak da o noktayı silebilirsiniz.";
            this.lblPointHint.Location = new Point(8, 295);
            this.lblPointHint.Size = new Size(280, 50);
            this.lblPointHint.ForeColor = ThemeHelper.MutedText;
            this.lblPointHint.Font = new Font("Segoe UI", 8.5F);

            this.tabPoints.Controls.Add(this.lstPoints);
            this.tabPoints.Controls.Add(this.lblPtX);
            this.tabPoints.Controls.Add(this.txtPtX);
            this.tabPoints.Controls.Add(this.lblPtY);
            this.tabPoints.Controls.Add(this.txtPtY);
            this.tabPoints.Controls.Add(this.btnPtColor);
            this.tabPoints.Controls.Add(this.btnAddPoint);
            this.tabPoints.Controls.Add(this.lblPtShape);
            this.tabPoints.Controls.Add(this.cmbPtShape);
            this.tabPoints.Controls.Add(this.btnDeletePoint);
            this.tabPoints.Controls.Add(this.btnClearPoints);
            this.tabPoints.Controls.Add(this.lblPointHint);

            // tabSliders
            this.tabSliders.Text = "Parametreler";
            this.tabSliders.BackColor = ThemeHelper.DarkBackground;
            this.tabSliders.Padding = new Padding(8);

            this.lblParamA.Text = "a = 1.00";
            this.lblParamA.Location = new Point(8, 8);
            this.lblParamA.AutoSize = true;
            this.lblParamA.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblParamA.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.tbParamA.Location = new Point(8, 28);
            this.tbParamA.Size = new Size(284, 45);
            this.tbParamA.Minimum = -100;
            this.tbParamA.Maximum = 100;
            this.tbParamA.Value = 10;
            this.tbParamA.TickFrequency = 20;

            this.lblParamB.Text = "b = 1.00";
            this.lblParamB.Location = new Point(8, 72);
            this.lblParamB.AutoSize = true;
            this.lblParamB.ForeColor = Color.FromArgb(52, 211, 153);
            this.lblParamB.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.tbParamB.Location = new Point(8, 92);
            this.tbParamB.Size = new Size(284, 45);
            this.tbParamB.Minimum = -100;
            this.tbParamB.Maximum = 100;
            this.tbParamB.Value = 10;
            this.tbParamB.TickFrequency = 20;

            this.lblParamC.Text = "c = 0.00";
            this.lblParamC.Location = new Point(8, 136);
            this.lblParamC.AutoSize = true;
            this.lblParamC.ForeColor = Color.FromArgb(245, 158, 11);
            this.lblParamC.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.tbParamC.Location = new Point(8, 156);
            this.tbParamC.Size = new Size(284, 45);
            this.tbParamC.Minimum = -100;
            this.tbParamC.Maximum = 100;
            this.tbParamC.Value = 0;
            this.tbParamC.TickFrequency = 20;

            this.lblParamK.Text = "k = 1.00";
            this.lblParamK.Location = new Point(8, 200);
            this.lblParamK.AutoSize = true;
            this.lblParamK.ForeColor = Color.FromArgb(168, 85, 247);
            this.lblParamK.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            this.tbParamK.Location = new Point(8, 220);
            this.tbParamK.Size = new Size(284, 45);
            this.tbParamK.Minimum = -100;
            this.tbParamK.Maximum = 100;
            this.tbParamK.Value = 10;
            this.tbParamK.TickFrequency = 20;

            this.btnAnimate.Location = new Point(8, 268);
            this.btnResetParams.Location = new Point(8, 302);

            this.lblSliderHint.Text = "İpucu: Formülünüze a, b, c veya k değişkenlerini ekleyin (ör: a*sin(b*x + c)). Sürgüleri hareket ettirerek dalga animasyonunu canlı izleyin.";
            this.lblSliderHint.Location = new Point(8, 340);
            this.lblSliderHint.Size = new Size(284, 60);
            this.lblSliderHint.ForeColor = ThemeHelper.MutedText;
            this.lblSliderHint.Font = new Font("Segoe UI", 8.5F);

            this.tabSliders.Controls.Add(this.lblParamA);
            this.tabSliders.Controls.Add(this.tbParamA);
            this.tabSliders.Controls.Add(this.lblParamB);
            this.tabSliders.Controls.Add(this.tbParamB);
            this.tabSliders.Controls.Add(this.lblParamC);
            this.tabSliders.Controls.Add(this.tbParamC);
            this.tabSliders.Controls.Add(this.lblParamK);
            this.tabSliders.Controls.Add(this.tbParamK);
            this.tabSliders.Controls.Add(this.btnAnimate);
            this.tabSliders.Controls.Add(this.btnResetParams);
            this.tabSliders.Controls.Add(this.lblSliderHint);

            // tabTools
            this.tabTools.Text = "Araçlar";
            this.tabTools.BackColor = ThemeHelper.DarkBackground;
            this.tabTools.Padding = new Padding(8);

            this.lblToolF1.Text = "1. Fonksiyon:";
            this.lblToolF1.Location = new Point(8, 6);
            this.lblToolF1.AutoSize = true;
            this.lblToolF1.ForeColor = ThemeHelper.LightText;

            this.cmbToolF1.Location = new Point(8, 24);
            this.cmbToolF1.Size = new Size(284, 25);
            this.cmbToolF1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolF1.BackColor = ThemeHelper.CardBackground;
            this.cmbToolF1.ForeColor = Color.White;

            this.lblToolF2.Text = "2. Fonksiyon:";
            this.lblToolF2.Location = new Point(8, 52);
            this.lblToolF2.AutoSize = true;
            this.lblToolF2.ForeColor = ThemeHelper.LightText;

            this.cmbToolF2.Location = new Point(8, 70);
            this.cmbToolF2.Size = new Size(284, 25);
            this.cmbToolF2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolF2.BackColor = ThemeHelper.CardBackground;
            this.cmbToolF2.ForeColor = Color.White;

            this.btnCommonTangent.Location = new Point(8, 100);
            this.btnNearestPoints.Location = new Point(8, 132);

            this.lblToolSep.Text = "────────────────────────────";
            this.lblToolSep.Location = new Point(8, 163);
            this.lblToolSep.Size = new Size(284, 14);
            this.lblToolSep.ForeColor = ThemeHelper.MutedText;

            this.lblToolPointTan.Text = "Noktadan Teğet (x₀, y₀):";
            this.lblToolPointTan.Location = new Point(8, 178);
            this.lblToolPointTan.AutoSize = true;
            this.lblToolPointTan.ForeColor = ThemeHelper.LightText;

            this.txtTanPointX.Location = new Point(8, 198);
            this.txtTanPointX.Size = new Size(70, 24);
            this.txtTanPointX.Text = "0";
            this.txtTanPointX.Font = new Font("Consolas", 9.5F);
            this.txtTanPointX.BackColor = ThemeHelper.CardBackground;
            this.txtTanPointX.ForeColor = Color.White;

            this.txtTanPointY.Location = new Point(85, 198);
            this.txtTanPointY.Size = new Size(70, 24);
            this.txtTanPointY.Text = "3";
            this.txtTanPointY.Font = new Font("Consolas", 9.5F);
            this.txtTanPointY.BackColor = ThemeHelper.CardBackground;
            this.txtTanPointY.ForeColor = Color.White;

            this.btnPointTangent.Location = new Point(8, 226);

            this.txtToolLog.Location = new Point(8, 258);
            this.txtToolLog.Size = new Size(284, 90);
            this.txtToolLog.BackColor = Color.FromArgb(11, 17, 32);
            this.txtToolLog.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtToolLog.Font = new Font("Consolas", 8F);
            this.txtToolLog.ReadOnly = true;

            this.btnExportImage.Location = new Point(8, 355);
            this.btnExportCsv.Location = new Point(8, 387);
            this.btnExportLatex.Location = new Point(8, 419);

            this.tabTools.Controls.Add(this.lblToolF1);
            this.tabTools.Controls.Add(this.cmbToolF1);
            this.tabTools.Controls.Add(this.lblToolF2);
            this.tabTools.Controls.Add(this.cmbToolF2);
            this.tabTools.Controls.Add(this.btnCommonTangent);
            this.tabTools.Controls.Add(this.btnNearestPoints);
            this.tabTools.Controls.Add(this.lblToolSep);
            this.tabTools.Controls.Add(this.lblToolPointTan);
            this.tabTools.Controls.Add(this.txtTanPointX);
            this.tabTools.Controls.Add(this.txtTanPointY);
            this.tabTools.Controls.Add(this.btnPointTangent);
            this.tabTools.Controls.Add(this.txtToolLog);
            this.tabTools.Controls.Add(this.btnExportImage);
            this.tabTools.Controls.Add(this.btnExportCsv);
            this.tabTools.Controls.Add(this.btnExportLatex);

            // tabShapes
            this.tabShapes.Text = "Şekiller";
            this.tabShapes.BackColor = ThemeHelper.DarkBackground;
            this.tabShapes.Padding = new Padding(8);

            this.lblShapeType.Text = "Şekil Türü:";
            this.lblShapeType.Location = new Point(8, 12);
            this.lblShapeType.AutoSize = true;
            this.lblShapeType.ForeColor = ThemeHelper.LightText;

            this.cmbShapeType.Location = new Point(8, 32);
            this.cmbShapeType.Size = new Size(284, 25);
            this.cmbShapeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbShapeType.BackColor = ThemeHelper.CardBackground;
            this.cmbShapeType.ForeColor = Color.White;

            this.lblShapeCenter.Text = "Merkez (x, y):";
            this.lblShapeCenter.Location = new Point(8, 68);
            this.lblShapeCenter.AutoSize = true;
            this.lblShapeCenter.ForeColor = ThemeHelper.LightText;

            this.txtShapeCenterX.Location = new Point(8, 88);
            this.txtShapeCenterX.Size = new Size(70, 24);
            this.txtShapeCenterX.Text = "0";
            this.txtShapeCenterX.Font = new Font("Consolas", 9.5F);
            this.txtShapeCenterX.BackColor = ThemeHelper.CardBackground;
            this.txtShapeCenterX.ForeColor = Color.White;

            this.txtShapeCenterY.Location = new Point(85, 88);
            this.txtShapeCenterY.Size = new Size(70, 24);
            this.txtShapeCenterY.Text = "0";
            this.txtShapeCenterY.Font = new Font("Consolas", 9.5F);
            this.txtShapeCenterY.BackColor = ThemeHelper.CardBackground;
            this.txtShapeCenterY.ForeColor = Color.White;

            this.lblShapeDim1.Text = "Yarıçap / Kenar A:";
            this.lblShapeDim1.Location = new Point(8, 122);
            this.lblShapeDim1.AutoSize = true;
            this.lblShapeDim1.ForeColor = ThemeHelper.LightText;

            this.txtShapeDim1.Location = new Point(8, 142);
            this.txtShapeDim1.Size = new Size(140, 24);
            this.txtShapeDim1.Text = "4";
            this.txtShapeDim1.Font = new Font("Consolas", 9.5F);
            this.txtShapeDim1.BackColor = ThemeHelper.CardBackground;
            this.txtShapeDim1.ForeColor = Color.White;

            this.lblShapeDim2.Text = "Yükseklik / Kenar B:";
            this.lblShapeDim2.Location = new Point(8, 175);
            this.lblShapeDim2.AutoSize = true;
            this.lblShapeDim2.ForeColor = ThemeHelper.LightText;

            this.txtShapeDim2.Location = new Point(8, 195);
            this.txtShapeDim2.Size = new Size(140, 24);
            this.txtShapeDim2.Text = "2";
            this.txtShapeDim2.Font = new Font("Consolas", 9.5F);
            this.txtShapeDim2.BackColor = ThemeHelper.CardBackground;
            this.txtShapeDim2.ForeColor = Color.White;

            this.lblShapeAngle.Text = "Dönüş (°):";
            this.lblShapeAngle.Location = new Point(8, 228);
            this.lblShapeAngle.AutoSize = true;
            this.lblShapeAngle.ForeColor = ThemeHelper.LightText;

            this.txtShapeAngle.Location = new Point(8, 248);
            this.txtShapeAngle.Size = new Size(140, 24);
            this.txtShapeAngle.Text = "0";
            this.txtShapeAngle.Font = new Font("Consolas", 9.5F);
            this.txtShapeAngle.BackColor = ThemeHelper.CardBackground;
            this.txtShapeAngle.ForeColor = Color.White;

            this.btnAddShape.Location = new Point(8, 290);

            this.tabShapes.Controls.Add(this.lblShapeType);
            this.tabShapes.Controls.Add(this.cmbShapeType);
            this.tabShapes.Controls.Add(this.lblShapeCenter);
            this.tabShapes.Controls.Add(this.txtShapeCenterX);
            this.tabShapes.Controls.Add(this.txtShapeCenterY);
            this.tabShapes.Controls.Add(this.lblShapeDim1);
            this.tabShapes.Controls.Add(this.txtShapeDim1);
            this.tabShapes.Controls.Add(this.lblShapeDim2);
            this.tabShapes.Controls.Add(this.txtShapeDim2);
            this.tabShapes.Controls.Add(this.lblShapeAngle);
            this.tabShapes.Controls.Add(this.txtShapeAngle);
            this.tabShapes.Controls.Add(this.btnAddShape);

            // pnlStatusBar
            this.pnlStatusBar.Dock = DockStyle.Bottom;
            this.pnlStatusBar.Height = 26;
            this.pnlStatusBar.BackColor = Color.FromArgb(11, 15, 28);
            this.pnlStatusBar.Controls.Add(this.lblStatus);

            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblStatus.Font = new Font("Segoe UI", 8.5F);
            this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.lblStatus.Padding = new Padding(8, 0, 0, 0);
            this.lblStatus.Text = "Hazır. Sol tık: Nokta ekle | Sağ tık: Nokta sil | Shift+Tekerlek: X Genişlet | Ctrl+Tekerlek: Y Genişlet";

            // pnlCanvas
            this.pnlCanvas.Dock = DockStyle.Fill;
            this.pnlCanvas.BackColor = Color.FromArgb(15, 23, 42);
            this.pnlCanvas.Cursor = Cursors.Cross;

            // Form assembly
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.pnlRightSide);
            this.Controls.Add(this.pnlStatusBar);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.pnlRightSide.ResumeLayout(false);
            this.tabManager.ResumeLayout(false);
            this.tabFunctions.ResumeLayout(false);
            this.tabPoints.ResumeLayout(false);
            this.tabPoints.PerformLayout();
            this.tabSliders.ResumeLayout(false);
            this.tabSliders.PerformLayout();
            this.tabTools.ResumeLayout(false);
            this.tabTools.PerformLayout();
            this.tabShapes.ResumeLayout(false);
            this.tabShapes.PerformLayout();
            this.pnlStatusBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbParamA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamK)).EndInit();
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
        private CheckBox chkPolar;
        private Label lblMinX;
        private TextBox txtMinX;
        private Label lblMaxX;
        private TextBox txtMaxX;
        private CheckBox chkClampRange;
        private Button btnPlot;

        private Button btnZoomXIn;
        private Button btnZoomXOut;
        private Button btnZoomX10;
        private Button btnZoomYIn;
        private Button btnZoomYOut;
        private Button btnAspect1to1;
        private Button btnResetView;
        private CheckBox chkCriticalPoints;
        private CheckBox chkInspectMode;
        private CheckBox chkShowAsymptotes;
        private Panel pnlSep1;
        private Panel pnlSep2;
        private Panel pnlSepMain;
        private Panel pnlControlsBorder;

        private Panel pnlRightSide;
        private TabControl tabManager;
        private TabPage tabFunctions;
        private TabPage tabPoints;
        private TabPage tabSliders;
        private TabPage tabTools;
        private TabPage tabShapes;

        private ListBox lstCurves;
        private Button btnCurveColor;
        private ComboBox cmbCurveStyle;
        private ComboBox cmbCurveThickness;
        private Button btnCurveToggle;
        private Button btnCurveDelete;
        private Button btnCurveEdit;
        private Button btnCurveClearAll;

        private ListBox lstPoints;
        private Label lblPtX;
        private TextBox txtPtX;
        private Label lblPtY;
        private TextBox txtPtY;
        private Label lblPtShape;
        private ComboBox cmbPtShape;
        private Button btnPtColor;
        private Button btnAddPoint;
        private Button btnDeletePoint;
        private Button btnClearPoints;
        private Label lblPointHint;

        private Label lblParamA;
        private TrackBar tbParamA;
        private Label lblParamB;
        private TrackBar tbParamB;
        private Label lblParamC;
        private TrackBar tbParamC;
        private Label lblParamK;
        private TrackBar tbParamK;
        private Button btnAnimate;
        private Button btnResetParams;
        private Label lblSliderHint;

        private Label lblToolF1;
        private ComboBox cmbToolF1;
        private Label lblToolF2;
        private ComboBox cmbToolF2;
        private Button btnCommonTangent;
        private Button btnNearestPoints;
        private Label lblToolSep;
        private Label lblToolPointTan;
        private TextBox txtTanPointX;
        private TextBox txtTanPointY;
        private Button btnPointTangent;
        private RichTextBox txtToolLog;
        private Button btnExportImage;
        private Button btnExportCsv;
        private Button btnExportLatex;

        private Label lblShapeType;
        private ComboBox cmbShapeType;
        private Label lblShapeCenter;
        private TextBox txtShapeCenterX;
        private TextBox txtShapeCenterY;
        private Label lblShapeDim1;
        private TextBox txtShapeDim1;
        private Label lblShapeDim2;
        private TextBox txtShapeDim2;
        private Label lblShapeAngle;
        private TextBox txtShapeAngle;
        private Button btnAddShape;

        private Panel pnlStatusBar;
        private Label lblStatus;
        private Panel pnlCanvas;
    }
}
