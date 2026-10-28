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
                "Gelişmiş Çoklu Fonksiyon & Geometri Grafiği Çizici (Advanced Graph Plotter)",
                "Çoklu fonksiyon desteği, ortak teğetler, en yakın nokta analizi, eksen sündürme, hazır parametrik şekiller ve tekil nokta yönetimi.");

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
            this.chkClampRange = new CheckBox();
            this.btnPlot = ThemeHelper.CreateActionButton("➕ Çiz / Ekle", 110, 28, true);

            // Scale Toolbar buttons
            this.btnZoomXIn = ThemeHelper.CreateActionButton("↔️ X Sündür (+)", 105, 26, false);
            this.btnZoomXOut = ThemeHelper.CreateActionButton("X Daralt (-)", 90, 26, false);
            this.btnZoomX10 = ThemeHelper.CreateActionButton("X: 10×", 65, 26, false);
            this.btnZoomYIn = ThemeHelper.CreateActionButton("↕️ Y Sündür (+)", 105, 26, false);
            this.btnZoomYOut = ThemeHelper.CreateActionButton("Y Daralt (-)", 90, 26, false);
            this.btnAspect1to1 = ThemeHelper.CreateActionButton("1:1 Oran", 75, 26, false);
            this.btnResetView = ThemeHelper.CreateActionButton("🔄 Sıfırla", 75, 26, false);

            this.pnlRightSide = new Panel();
            this.tabManager = new TabControl();
            this.tabFunctions = new TabPage();
            this.tabPoints = new TabPage();
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

            // Tools Tab Controls
            this.lblToolF1 = new Label();
            this.cmbToolF1 = new ComboBox();
            this.lblToolF2 = new Label();
            this.cmbToolF2 = new ComboBox();
            this.btnCommonTangent = ThemeHelper.CreateActionButton("📐 Ortak Teğet Doğrusunu Bul", 260, 30, true);
            this.btnNearestPoints = ThemeHelper.CreateActionButton("📍 En Yakın Noktalar & Doğru", 260, 30, false);
            this.lblToolSep = new Label();
            this.lblToolPointTan = new Label();
            this.txtTanPointX = new TextBox();
            this.txtTanPointY = new TextBox();
            this.btnPointTangent = ThemeHelper.CreateActionButton("📏 Noktadan Teğet Çiz", 260, 30, false);
            this.txtToolLog = new RichTextBox();

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
            this.btnAddShape = ThemeHelper.CreateActionButton("📐 Şekli Grafiğe Ekle", 260, 32, true);

            this.pnlStatusBar = new Panel();
            this.lblStatus = new Label();
            this.pnlCanvas = new Panel();

            this.pnlControls.SuspendLayout();
            this.pnlRightSide.SuspendLayout();
            this.tabManager.SuspendLayout();
            this.tabFunctions.SuspendLayout();
            this.tabPoints.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.tabShapes.SuspendLayout();
            this.pnlStatusBar.SuspendLayout();
            this.SuspendLayout();

            // pnlControls
            this.pnlControls.Dock = DockStyle.Top;
            this.pnlControls.Height = 104;
            this.pnlControls.BackColor = ThemeHelper.DarkBackground;
            this.pnlControls.Padding = new Padding(16, 6, 16, 6);

            this.lblPresets.Text = "Hızlı:";
            this.lblPresets.AutoSize = true;
            this.lblPresets.ForeColor = ThemeHelper.MutedText;
            this.lblPresets.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            this.lblPresets.Margin = new Padding(0, 4, 6, 0);

            this.pnlPresets.Location = new Point(16, 6);
            this.pnlPresets.Size = new Size(950, 30);
            this.pnlPresets.Controls.Add(this.lblPresets);
            this.pnlPresets.Controls.Add(this.btnEx1);
            this.pnlPresets.Controls.Add(this.btnEx2);
            this.pnlPresets.Controls.Add(this.btnEx3);
            this.pnlPresets.Controls.Add(this.btnEx4);

            int yRow1 = 40;
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = ThemeHelper.LightText;
            this.lblEquation.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.lblEquation.Text = "f(x) =";
            this.lblEquation.Location = new Point(16, yRow1 + 3);

            this.txtEquation.Location = new Point(60, yRow1);
            this.txtEquation.Size = new Size(180, 25);
            this.txtEquation.Text = "sin(x)";
            this.txtEquation.Font = new Font("Consolas", 10.5F);
            this.txtEquation.BackColor = ThemeHelper.CardBackground;
            this.txtEquation.ForeColor = Color.White;
            this.txtEquation.BorderStyle = BorderStyle.FixedSingle;

            this.lblMinX.AutoSize = true;
            this.lblMinX.ForeColor = ThemeHelper.LightText;
            this.lblMinX.Text = "Min X:";
            this.lblMinX.Location = new Point(250, yRow1 + 4);

            this.txtMinX.Location = new Point(292, yRow1);
            this.txtMinX.Size = new Size(50, 25);
            this.txtMinX.Text = "-10";
            this.txtMinX.Font = new Font("Consolas", 9.5F);
            this.txtMinX.BackColor = ThemeHelper.CardBackground;
            this.txtMinX.ForeColor = Color.White;
            this.txtMinX.BorderStyle = BorderStyle.FixedSingle;

            this.lblMaxX.AutoSize = true;
            this.lblMaxX.ForeColor = ThemeHelper.LightText;
            this.lblMaxX.Text = "Max X:";
            this.lblMaxX.Location = new Point(348, yRow1 + 4);

            this.txtMaxX.Location = new Point(392, yRow1);
            this.txtMaxX.Size = new Size(50, 25);
            this.txtMaxX.Text = "10";
            this.txtMaxX.Font = new Font("Consolas", 9.5F);
            this.txtMaxX.BackColor = ThemeHelper.CardBackground;
            this.txtMaxX.ForeColor = Color.White;
            this.txtMaxX.BorderStyle = BorderStyle.FixedSingle;

            this.chkClampRange.Text = "[Min, Max] Kısıtla";
            this.chkClampRange.ForeColor = ThemeHelper.LightText;
            this.chkClampRange.Location = new Point(452, yRow1 + 3);
            this.chkClampRange.Size = new Size(125, 22);

            this.btnPlot.Location = new Point(570, yRow1 - 1);

            int yRow2 = 72;
            this.btnZoomXIn.Location = new Point(16, yRow2);
            this.btnZoomXOut.Location = new Point(126, yRow2);
            this.btnZoomX10.Location = new Point(221, yRow2);
            this.btnZoomYIn.Location = new Point(291, yRow2);
            this.btnZoomYOut.Location = new Point(401, yRow2);
            this.btnAspect1to1.Location = new Point(496, yRow2);
            this.btnResetView.Location = new Point(576, yRow2);

            this.pnlControls.Controls.Add(this.pnlPresets);
            this.pnlControls.Controls.Add(this.lblEquation);
            this.pnlControls.Controls.Add(this.txtEquation);
            this.pnlControls.Controls.Add(this.lblMinX);
            this.pnlControls.Controls.Add(this.txtMinX);
            this.pnlControls.Controls.Add(this.lblMaxX);
            this.pnlControls.Controls.Add(this.txtMaxX);
            this.pnlControls.Controls.Add(this.chkClampRange);
            this.pnlControls.Controls.Add(this.btnPlot);
            this.pnlControls.Controls.Add(this.btnZoomXIn);
            this.pnlControls.Controls.Add(this.btnZoomXOut);
            this.pnlControls.Controls.Add(this.btnZoomX10);
            this.pnlControls.Controls.Add(this.btnZoomYIn);
            this.pnlControls.Controls.Add(this.btnZoomYOut);
            this.pnlControls.Controls.Add(this.btnAspect1to1);
            this.pnlControls.Controls.Add(this.btnResetView);

            // pnlRightSide (Tabbed Manager)
            this.pnlRightSide.Dock = DockStyle.Right;
            this.pnlRightSide.Width = 320;
            this.pnlRightSide.BackColor = ThemeHelper.CardBackground;
            this.pnlRightSide.Padding = new Padding(4);

            this.tabManager.Dock = DockStyle.Fill;
            this.tabManager.Controls.Add(this.tabFunctions);
            this.tabManager.Controls.Add(this.tabPoints);
            this.tabManager.Controls.Add(this.tabTools);
            this.tabManager.Controls.Add(this.tabShapes);

            // tabFunctions
            this.tabFunctions.Text = "📈 Eğriler";
            this.tabFunctions.BackColor = ThemeHelper.DarkBackground;
            this.tabFunctions.Padding = new Padding(8);

            this.lstCurves.Dock = DockStyle.Top;
            this.lstCurves.Height = 220;
            this.lstCurves.BackColor = Color.FromArgb(11, 17, 32);
            this.lstCurves.ForeColor = Color.White;
            this.lstCurves.Font = new Font("Consolas", 9.5F);

            this.btnCurveColor.Text = "🎨 Renk";
            this.btnCurveColor.Size = new Size(85, 26);
            this.btnCurveColor.Location = new Point(8, 230);
            this.btnCurveColor.BackColor = Color.FromArgb(30, 41, 59);
            this.btnCurveColor.ForeColor = Color.White;
            this.btnCurveColor.FlatStyle = FlatStyle.Flat;

            this.cmbCurveStyle.Location = new Point(100, 230);
            this.cmbCurveStyle.Size = new Size(95, 25);
            this.cmbCurveStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCurveStyle.BackColor = ThemeHelper.CardBackground;
            this.cmbCurveStyle.ForeColor = Color.White;
            this.cmbCurveStyle.Items.AddRange(new object[] { "Düz Çizgi", "Kesikli", "Noktalı" });
            this.cmbCurveStyle.SelectedIndex = 0;

            this.cmbCurveThickness.Location = new Point(202, 230);
            this.cmbCurveThickness.Size = new Size(90, 25);
            this.cmbCurveThickness.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCurveThickness.BackColor = ThemeHelper.CardBackground;
            this.cmbCurveThickness.ForeColor = Color.White;
            this.cmbCurveThickness.Items.AddRange(new object[] { "1 px", "2 px", "3 px", "4 px" });
            this.cmbCurveThickness.SelectedIndex = 1;

            this.btnCurveToggle.Text = "👁️ Göster/Gizle";
            this.btnCurveToggle.Size = new Size(138, 28);
            this.btnCurveToggle.Location = new Point(8, 265);
            this.btnCurveToggle.BackColor = Color.FromArgb(30, 41, 59);
            this.btnCurveToggle.ForeColor = Color.White;
            this.btnCurveToggle.FlatStyle = FlatStyle.Flat;

            this.btnCurveDelete.Text = "🗑️ Eğriyi Sil";
            this.btnCurveDelete.Size = new Size(138, 28);
            this.btnCurveDelete.Location = new Point(154, 265);
            this.btnCurveDelete.BackColor = Color.FromArgb(153, 27, 27);
            this.btnCurveDelete.ForeColor = Color.White;
            this.btnCurveDelete.FlatStyle = FlatStyle.Flat;

            this.btnCurveEdit.Text = "✏️ Düzenle";
            this.btnCurveEdit.Size = new Size(138, 28);
            this.btnCurveEdit.Location = new Point(8, 298);
            this.btnCurveEdit.BackColor = Color.FromArgb(30, 41, 59);
            this.btnCurveEdit.ForeColor = Color.White;
            this.btnCurveEdit.FlatStyle = FlatStyle.Flat;

            this.btnCurveClearAll.Text = "🧹 Tümünü Sil";
            this.btnCurveClearAll.Size = new Size(138, 28);
            this.btnCurveClearAll.Location = new Point(154, 298);
            this.btnCurveClearAll.BackColor = Color.FromArgb(45, 55, 72);
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
            this.tabPoints.Text = "📍 Noktalar";
            this.tabPoints.BackColor = ThemeHelper.DarkBackground;
            this.tabPoints.Padding = new Padding(8);

            this.lstPoints.Dock = DockStyle.Top;
            this.lstPoints.Height = 180;
            this.lstPoints.BackColor = Color.FromArgb(11, 17, 32);
            this.lstPoints.ForeColor = Color.White;
            this.lstPoints.Font = new Font("Consolas", 9.5F);

            int py = 190;
            this.lblPtX.Text = "X:";
            this.lblPtX.Location = new Point(8, py + 3);
            this.lblPtX.AutoSize = true;
            this.lblPtX.ForeColor = ThemeHelper.LightText;

            this.txtPtX.Location = new Point(28, py);
            this.txtPtX.Size = new Size(60, 24);
            this.txtPtX.Text = "0";
            this.txtPtX.Font = new Font("Consolas", 9.5F);
            this.txtPtX.BackColor = ThemeHelper.CardBackground;
            this.txtPtX.ForeColor = Color.White;

            this.lblPtY.Text = "Y:";
            this.lblPtY.Location = new Point(95, py + 3);
            this.lblPtY.AutoSize = true;
            this.lblPtY.ForeColor = ThemeHelper.LightText;

            this.txtPtY.Location = new Point(115, py);
            this.txtPtY.Size = new Size(60, 24);
            this.txtPtY.Text = "0";
            this.txtPtY.Font = new Font("Consolas", 9.5F);
            this.txtPtY.BackColor = ThemeHelper.CardBackground;
            this.txtPtY.ForeColor = Color.White;

            this.btnPtColor.Text = "🎨";
            this.btnPtColor.Location = new Point(182, py);
            this.btnPtColor.Size = new Size(35, 25);
            this.btnPtColor.BackColor = Color.FromArgb(250, 204, 21);
            this.btnPtColor.FlatStyle = FlatStyle.Flat;

            this.btnAddPoint.Text = "➕ Ekle";
            this.btnAddPoint.Location = new Point(222, py);
            this.btnAddPoint.Size = new Size(70, 25);
            this.btnAddPoint.BackColor = ThemeHelper.AccentBlue;
            this.btnAddPoint.ForeColor = Color.White;
            this.btnAddPoint.FlatStyle = FlatStyle.Flat;

            int py2 = 222;
            this.lblPtShape.Text = "Şekil:";
            this.lblPtShape.Location = new Point(8, py2 + 3);
            this.lblPtShape.AutoSize = true;
            this.lblPtShape.ForeColor = ThemeHelper.LightText;

            this.cmbPtShape.Location = new Point(50, py2);
            this.cmbPtShape.Size = new Size(125, 25);
            this.cmbPtShape.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPtShape.BackColor = ThemeHelper.CardBackground;
            this.cmbPtShape.ForeColor = Color.White;
            this.cmbPtShape.Items.AddRange(new object[] { "Daire", "Kare", "Çarpı", "Elmas" });
            this.cmbPtShape.SelectedIndex = 0;

            this.btnDeletePoint.Text = "🗑️ Seçiliyi Sil";
            this.btnDeletePoint.Size = new Size(138, 28);
            this.btnDeletePoint.Location = new Point(8, 255);
            this.btnDeletePoint.BackColor = Color.FromArgb(153, 27, 27);
            this.btnDeletePoint.ForeColor = Color.White;
            this.btnDeletePoint.FlatStyle = FlatStyle.Flat;

            this.btnClearPoints.Text = "🧹 Tümünü Sil";
            this.btnClearPoints.Size = new Size(138, 28);
            this.btnClearPoints.Location = new Point(154, 255);
            this.btnClearPoints.BackColor = Color.FromArgb(45, 55, 72);
            this.btnClearPoints.ForeColor = Color.White;
            this.btnClearPoints.FlatStyle = FlatStyle.Flat;

            this.lblPointHint.Text = "💡 İpucu: Grafik tuvalinde bir noktanın üzerine sağ tıklayarak da o noktayı doğrudan silebilirsiniz.";
            this.lblPointHint.Location = new Point(8, 290);
            this.lblPointHint.Size = new Size(284, 50);
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

            // tabTools
            this.tabTools.Text = "🛠️ Araçlar";
            this.tabTools.BackColor = ThemeHelper.DarkBackground;
            this.tabTools.Padding = new Padding(8);

            this.lblToolF1.Text = "1. Fonksiyon:";
            this.lblToolF1.Location = new Point(8, 8);
            this.lblToolF1.AutoSize = true;
            this.lblToolF1.ForeColor = ThemeHelper.LightText;

            this.cmbToolF1.Location = new Point(8, 28);
            this.cmbToolF1.Size = new Size(284, 25);
            this.cmbToolF1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolF1.BackColor = ThemeHelper.CardBackground;
            this.cmbToolF1.ForeColor = Color.White;

            this.lblToolF2.Text = "2. Fonksiyon:";
            this.lblToolF2.Location = new Point(8, 60);
            this.lblToolF2.AutoSize = true;
            this.lblToolF2.ForeColor = ThemeHelper.LightText;

            this.cmbToolF2.Location = new Point(8, 80);
            this.cmbToolF2.Size = new Size(284, 25);
            this.cmbToolF2.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbToolF2.BackColor = ThemeHelper.CardBackground;
            this.cmbToolF2.ForeColor = Color.White;

            this.btnCommonTangent.Location = new Point(8, 115);
            this.btnNearestPoints.Location = new Point(8, 150);

            this.lblToolSep.Text = "────────────────────────────";
            this.lblToolSep.Location = new Point(8, 185);
            this.lblToolSep.Size = new Size(284, 16);
            this.lblToolSep.ForeColor = ThemeHelper.MutedText;

            this.lblToolPointTan.Text = "Noktadan Teğet (x₀, y₀):";
            this.lblToolPointTan.Location = new Point(8, 203);
            this.lblToolPointTan.AutoSize = true;
            this.lblToolPointTan.ForeColor = ThemeHelper.LightText;

            this.txtTanPointX.Location = new Point(8, 225);
            this.txtTanPointX.Size = new Size(70, 24);
            this.txtTanPointX.Text = "0";
            this.txtTanPointX.Font = new Font("Consolas", 9.5F);
            this.txtTanPointX.BackColor = ThemeHelper.CardBackground;
            this.txtTanPointX.ForeColor = Color.White;

            this.txtTanPointY.Location = new Point(85, 225);
            this.txtTanPointY.Size = new Size(70, 24);
            this.txtTanPointY.Text = "3";
            this.txtTanPointY.Font = new Font("Consolas", 9.5F);
            this.txtTanPointY.BackColor = ThemeHelper.CardBackground;
            this.txtTanPointY.ForeColor = Color.White;

            this.btnPointTangent.Location = new Point(8, 255);

            this.txtToolLog.Location = new Point(8, 292);
            this.txtToolLog.Size = new Size(284, 140);
            this.txtToolLog.BackColor = Color.FromArgb(11, 17, 32);
            this.txtToolLog.ForeColor = Color.FromArgb(226, 232, 240);
            this.txtToolLog.Font = new Font("Consolas", 8.5F);
            this.txtToolLog.ReadOnly = true;

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

            // tabShapes
            this.tabShapes.Text = "📐 Şekiller";
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
            this.cmbShapeType.Items.AddRange(new object[] {
                "Daire (Çember)",
                "Kare",
                "Dikdörtgen",
                "Eşkenar Üçgen",
                "Elips",
                "Düzgün Altıgen"
            });
            this.cmbShapeType.SelectedIndex = 0;

            int sy = 68;
            this.lblShapeCenter.Text = "Merkez (x, y):";
            this.lblShapeCenter.Location = new Point(8, sy);
            this.lblShapeCenter.AutoSize = true;
            this.lblShapeCenter.ForeColor = ThemeHelper.LightText;

            this.txtShapeCenterX.Location = new Point(8, sy + 20);
            this.txtShapeCenterX.Size = new Size(60, 24);
            this.txtShapeCenterX.Text = "0";
            this.txtShapeCenterX.Font = new Font("Consolas", 9.5F);
            this.txtShapeCenterX.BackColor = ThemeHelper.CardBackground;
            this.txtShapeCenterX.ForeColor = Color.White;

            this.txtShapeCenterY.Location = new Point(75, sy + 20);
            this.txtShapeCenterY.Size = new Size(60, 24);
            this.txtShapeCenterY.Text = "0";
            this.txtShapeCenterY.Font = new Font("Consolas", 9.5F);
            this.txtShapeCenterY.BackColor = ThemeHelper.CardBackground;
            this.txtShapeCenterY.ForeColor = Color.White;

            int sy2 = 120;
            this.lblShapeDim1.Text = "Yarıçap / Kenar A:";
            this.lblShapeDim1.Location = new Point(8, sy2);
            this.lblShapeDim1.AutoSize = true;
            this.lblShapeDim1.ForeColor = ThemeHelper.LightText;

            this.txtShapeDim1.Location = new Point(8, sy2 + 20);
            this.txtShapeDim1.Size = new Size(80, 24);
            this.txtShapeDim1.Text = "4";
            this.txtShapeDim1.Font = new Font("Consolas", 9.5F);
            this.txtShapeDim1.BackColor = ThemeHelper.CardBackground;
            this.txtShapeDim1.ForeColor = Color.White;

            this.lblShapeDim2.Text = "Yükseklik / Kenar B:";
            this.lblShapeDim2.Location = new Point(100, sy2);
            this.lblShapeDim2.AutoSize = true;
            this.lblShapeDim2.ForeColor = ThemeHelper.LightText;

            this.txtShapeDim2.Location = new Point(100, sy2 + 20);
            this.txtShapeDim2.Size = new Size(80, 24);
            this.txtShapeDim2.Text = "2";
            this.txtShapeDim2.Font = new Font("Consolas", 9.5F);
            this.txtShapeDim2.BackColor = ThemeHelper.CardBackground;
            this.txtShapeDim2.ForeColor = Color.White;

            this.lblShapeAngle.Text = "Dönüş (°):";
            this.lblShapeAngle.Location = new Point(190, sy2);
            this.lblShapeAngle.AutoSize = true;
            this.lblShapeAngle.ForeColor = ThemeHelper.LightText;

            this.txtShapeAngle.Location = new Point(190, sy2 + 20);
            this.txtShapeAngle.Size = new Size(60, 24);
            this.txtShapeAngle.Text = "0";
            this.txtShapeAngle.Font = new Font("Consolas", 9.5F);
            this.txtShapeAngle.BackColor = ThemeHelper.CardBackground;
            this.txtShapeAngle.ForeColor = Color.White;

            this.btnAddShape.Location = new Point(8, 180);

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

            this.pnlRightSide.Controls.Add(this.tabManager);

            // pnlStatusBar
            this.pnlStatusBar.Dock = DockStyle.Bottom;
            this.pnlStatusBar.Height = 28;
            this.pnlStatusBar.BackColor = Color.FromArgb(11, 17, 32);
            this.pnlStatusBar.Padding = new Padding(16, 4, 16, 4);

            this.lblStatus.Dock = DockStyle.Fill;
            this.lblStatus.ForeColor = ThemeHelper.MutedText;
            this.lblStatus.Font = new Font("Segoe UI", 9F);
            this.lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.lblStatus.Text = "Hazır. Sol tık: Nokta ekle | Sağ tık: Nokta sil | Shift+Tekerlek: X sündür | Ctrl+Tekerlek: Y sündür";
            this.pnlStatusBar.Controls.Add(this.lblStatus);

            // pnlCanvas
            this.pnlCanvas.Dock = DockStyle.Fill;
            this.pnlCanvas.BackColor = Color.FromArgb(11, 17, 32);
            this.pnlCanvas.Cursor = Cursors.Cross;
            this.pnlCanvas.Paint += new PaintEventHandler(this.pnlCanvas_Paint);
            this.pnlCanvas.Resize += new EventHandler(this.pnlCanvas_Resize);

            // Assemble GraphPlotterControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.pnlStatusBar);
            this.Controls.Add(this.pnlRightSide);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "GraphPlotterControl";
            this.Size = new Size(1100, 720);

            this.pnlControls.ResumeLayout(false);
            this.pnlControls.PerformLayout();
            this.tabFunctions.ResumeLayout(false);
            this.tabPoints.ResumeLayout(false);
            this.tabPoints.PerformLayout();
            this.tabTools.ResumeLayout(false);
            this.tabTools.PerformLayout();
            this.tabShapes.ResumeLayout(false);
            this.tabShapes.PerformLayout();
            this.tabManager.ResumeLayout(false);
            this.pnlRightSide.ResumeLayout(false);
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
        private CheckBox chkClampRange;
        private Button btnPlot;

        private Button btnZoomXIn;
        private Button btnZoomXOut;
        private Button btnZoomX10;
        private Button btnZoomYIn;
        private Button btnZoomYOut;
        private Button btnAspect1to1;
        private Button btnResetView;

        private Panel pnlRightSide;
        private TabControl tabManager;
        private TabPage tabFunctions;
        private TabPage tabPoints;
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
