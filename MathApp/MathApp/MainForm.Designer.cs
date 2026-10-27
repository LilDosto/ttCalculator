using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp
{
    partial class MainForm
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
            this.pnlSidebar = new Panel();
            this.pnlNavButtons = new Panel();
            this.pnlContent = new Panel();
            this.pnlLogo = new Panel();
            this.lblLogo = new Label();
            this.lblLogoSub = new Label();
            this.lblVersion = new Label();

            this.lblCat1 = new Label();
            this.btnNavCalc = new Button();
            this.btnNavGraph = new Button();
            this.btnNavCombinatorics = new Button();

            this.lblCat2 = new Label();
            this.btnNavRoot = new Button();
            this.btnNavSystem = new Button();
            this.btnNavDerivative = new Button();

            this.lblCat3 = new Label();
            this.btnNavMatrix = new Button();
            this.btnNavTaylor = new Button();
            this.btnNavMultiTaylor = new Button();

            this.pnlSidebar.SuspendLayout();
            this.pnlNavButtons.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.SuspendLayout();

            // pnlSidebar (Left Navigation Bar)
            this.pnlSidebar.BackColor = ThemeHelper.SidebarBackground;
            this.pnlSidebar.Dock = DockStyle.Left;
            this.pnlSidebar.Width = 245;
            this.pnlSidebar.Padding = new Padding(8, 0, 8, 8);

            // pnlLogo
            this.pnlLogo.Dock = DockStyle.Top;
            this.pnlLogo.Height = 80;
            this.pnlLogo.Padding = new Padding(10, 12, 10, 6);

            this.lblLogo.Dock = DockStyle.Top;
            this.lblLogo.Height = 32;
            this.lblLogo.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold);
            this.lblLogo.ForeColor = Color.FromArgb(56, 189, 248);
            this.lblLogo.Text = "ƒ(x) ttCalculator";
            this.lblLogo.TextAlign = ContentAlignment.MiddleLeft;

            this.lblLogoSub.Dock = DockStyle.Top;
            this.lblLogoSub.Height = 20;
            this.lblLogoSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            this.lblLogoSub.ForeColor = ThemeHelper.MutedText;
            this.lblLogoSub.Text = "Nümerik Matematik Stüdyosu";
            this.lblLogoSub.TextAlign = ContentAlignment.MiddleLeft;

            this.pnlLogo.Controls.Add(this.lblLogoSub);
            this.pnlLogo.Controls.Add(this.lblLogo);

            // pnlNavButtons container with autoscroll support
            this.pnlNavButtons.Dock = DockStyle.Fill;
            this.pnlNavButtons.AutoScroll = true;

            void StyleNavCategory(Label lbl, string text)
            {
                lbl.Dock = DockStyle.Top;
                lbl.Height = 28;
                lbl.Text = text;
                lbl.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
                lbl.ForeColor = Color.FromArgb(100, 116, 139);
                lbl.TextAlign = ContentAlignment.BottomLeft;
                lbl.Padding = new Padding(12, 0, 0, 4);
            }

            void StyleNavButton(Button btn, string text, int index)
            {
                btn.Dock = DockStyle.Top;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
                btn.FlatAppearance.MouseDownBackColor = ThemeHelper.AccentBlue;
                btn.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                btn.ForeColor = Color.FromArgb(203, 213, 225);
                btn.Size = new Size(229, 38);
                btn.TabIndex = index + 1;
                btn.Text = text;
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(14, 0, 0, 0);
                btn.Cursor = Cursors.Hand;
                btn.UseVisualStyleBackColor = false;
            }

            // Categories
            StyleNavCategory(this.lblCat1, "HESAPLAMA & TEMEL");
            StyleNavButton(this.btnNavCalc, "🧮  Hesap Makinesi", 0);
            StyleNavButton(this.btnNavGraph, "📈  Grafik Çizici", 1);
            StyleNavButton(this.btnNavCombinatorics, "🎲  Kombinatorik & Özel", 2);

            StyleNavCategory(this.lblCat2, "NÜMERİK ANALİZ");
            StyleNavButton(this.btnNavRoot, "🔍  Denklem Kökü Bulucu", 3);
            StyleNavButton(this.btnNavSystem, "⚡  Denklem Sistemi Çözücü", 4);
            StyleNavButton(this.btnNavDerivative, "∂  Türev & Gradyan", 5);

            StyleNavCategory(this.lblCat3, "LİNEER CEBİR & SERİLER");
            StyleNavButton(this.btnNavMatrix, "🔢  Matris & Ayrışım", 6);
            StyleNavButton(this.btnNavTaylor, "📐  Taylor Serisi (1D)", 7);
            StyleNavButton(this.btnNavMultiTaylor, "🌐  Çok Değişkenli Taylor", 8);

            // Footer version label
            this.lblVersion.Dock = DockStyle.Bottom;
            this.lblVersion.Height = 26;
            this.lblVersion.Text = "v2.0 • 128-bit Decimal Motoru";
            this.lblVersion.Font = new Font("Segoe UI", 8F);
            this.lblVersion.ForeColor = Color.FromArgb(100, 116, 139);
            this.lblVersion.TextAlign = ContentAlignment.MiddleCenter;

            // Add navigation controls in reverse order for DockStyle.Top inside pnlNavButtons
            // Group 3
            this.pnlNavButtons.Controls.Add(this.btnNavMultiTaylor);
            this.pnlNavButtons.Controls.Add(this.btnNavTaylor);
            this.pnlNavButtons.Controls.Add(this.btnNavMatrix);
            this.pnlNavButtons.Controls.Add(this.lblCat3);

            // Group 2
            this.pnlNavButtons.Controls.Add(this.btnNavDerivative);
            this.pnlNavButtons.Controls.Add(this.btnNavSystem);
            this.pnlNavButtons.Controls.Add(this.btnNavRoot);
            this.pnlNavButtons.Controls.Add(this.lblCat2);

            // Group 1
            this.pnlNavButtons.Controls.Add(this.btnNavCombinatorics);
            this.pnlNavButtons.Controls.Add(this.btnNavGraph);
            this.pnlNavButtons.Controls.Add(this.btnNavCalc);
            this.pnlNavButtons.Controls.Add(this.lblCat1);

            // Assemble pnlSidebar
            this.pnlSidebar.Controls.Add(this.pnlNavButtons);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Controls.Add(this.lblVersion);

            // pnlContent
            this.pnlContent.BackColor = ThemeHelper.DarkBackground;
            this.pnlContent.Dock = DockStyle.Fill;

            // MainForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1180, 740);
            this.MinimumSize = new Size(1020, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "MainForm";
            this.Text = "ttCalculator — Gelişmiş Matematik & Nümerik Analiz Stüdyosu";

            this.pnlSidebar.ResumeLayout(false);
            this.pnlNavButtons.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel pnlSidebar;
        private Panel pnlNavButtons;
        private Panel pnlContent;
        private Panel pnlLogo;
        private Label lblLogo;
        private Label lblLogoSub;
        private Label lblVersion;

        private Label lblCat1;
        private Button btnNavCalc;
        private Button btnNavGraph;
        private Button btnNavCombinatorics;

        private Label lblCat2;
        private Button btnNavRoot;
        private Button btnNavSystem;
        private Button btnNavDerivative;

        private Label lblCat3;
        private Button btnNavMatrix;
        private Button btnNavTaylor;
        private Button btnNavMultiTaylor;
    }
}
