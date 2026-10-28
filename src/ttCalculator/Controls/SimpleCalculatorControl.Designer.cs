using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp.Controls
{
    partial class SimpleCalculatorControl
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
                "Bilimsel Hesap Makinesi",
                "Yüksek hassasiyetli (128-bit Decimal) bilimsel ifadeler, trigonometri, logaritma ve işlem geçmişi.");

            this.pnlMain = new Panel();
            this.pnlLeft = new Panel();
            this.pnlRightHistory = new Panel();
            this.pnlDisplayCard = new Panel();
            this.lblExpressionHistory = new Label();
            this.txtDisplay = new TextBox();
            this.pnlToolbar = new Panel();
            this.btnAngleMode = new Button();
            this.lblKeyboardHint = new Label();
            this.pnlButtons = new TableLayoutPanel();

            this.lblHistoryTitle = new Label();
            this.lstHistory = new ListBox();
            this.btnClearHistory = new Button();

            this.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRightHistory.SuspendLayout();
            this.pnlDisplayCard.SuspendLayout();
            this.pnlToolbar.SuspendLayout();

            // pnlMain
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Padding = new Padding(20);
            this.pnlMain.BackColor = ThemeHelper.DarkBackground;
            this.pnlMain.Controls.Add(this.pnlLeft);
            this.pnlMain.Controls.Add(this.pnlRightHistory);

            // pnlRightHistory
            this.pnlRightHistory.Dock = DockStyle.Right;
            this.pnlRightHistory.Width = 260;
            this.pnlRightHistory.Padding = new Padding(14, 0, 0, 0);
            this.pnlRightHistory.BackColor = ThemeHelper.DarkBackground;

            this.lblHistoryTitle.Dock = DockStyle.Top;
            this.lblHistoryTitle.Height = 32;
            this.lblHistoryTitle.Text = "📜 İşlem Geçmişi (Çift Tıkla)";
            this.lblHistoryTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            this.lblHistoryTitle.ForeColor = ThemeHelper.LightText;
            this.lblHistoryTitle.TextAlign = ContentAlignment.MiddleLeft;

            this.btnClearHistory.Dock = DockStyle.Bottom;
            this.btnClearHistory.Height = 34;
            this.btnClearHistory.Text = "Geçmişi Temizle";
            this.btnClearHistory.FlatStyle = FlatStyle.Flat;
            this.btnClearHistory.BackColor = ThemeHelper.ButtonSecondary;
            this.btnClearHistory.ForeColor = ThemeHelper.LightText;
            this.btnClearHistory.Font = new Font("Segoe UI", 9.5F);
            this.btnClearHistory.FlatAppearance.BorderSize = 0;
            this.btnClearHistory.Cursor = Cursors.Hand;

            this.lstHistory.Dock = DockStyle.Fill;
            this.lstHistory.BackColor = ThemeHelper.CardBackground;
            this.lstHistory.ForeColor = Color.FromArgb(186, 230, 253);
            this.lstHistory.BorderStyle = BorderStyle.FixedSingle;
            this.lstHistory.Font = new Font("Consolas", 10.5F);
            this.lstHistory.ItemHeight = 22;

            this.pnlRightHistory.Controls.Add(this.lstHistory);
            this.pnlRightHistory.Controls.Add(this.btnClearHistory);
            this.pnlRightHistory.Controls.Add(this.lblHistoryTitle);

            // pnlLeft
            this.pnlLeft.Dock = DockStyle.Fill;
            this.pnlLeft.BackColor = ThemeHelper.DarkBackground;

            // pnlDisplayCard
            this.pnlDisplayCard.Dock = DockStyle.Top;
            this.pnlDisplayCard.Height = 108;
            this.pnlDisplayCard.BackColor = ThemeHelper.CardBackground;
            this.pnlDisplayCard.Padding = new Padding(16, 10, 16, 10);

            this.lblExpressionHistory.Dock = DockStyle.Top;
            this.lblExpressionHistory.Height = 24;
            this.lblExpressionHistory.Text = "İfade giriniz veya klavyeyi kullanınız";
            this.lblExpressionHistory.Font = new Font("Consolas", 10.5F, FontStyle.Regular);
            this.lblExpressionHistory.ForeColor = ThemeHelper.MutedText;
            this.lblExpressionHistory.TextAlign = ContentAlignment.MiddleRight;

            this.txtDisplay.Dock = DockStyle.Fill;
            this.txtDisplay.BackColor = ThemeHelper.CardBackground;
            this.txtDisplay.BorderStyle = BorderStyle.None;
            this.txtDisplay.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Bold);
            this.txtDisplay.ForeColor = Color.White;
            this.txtDisplay.ReadOnly = true;
            this.txtDisplay.Text = "0";
            this.txtDisplay.TextAlign = HorizontalAlignment.Right;

            this.pnlDisplayCard.Controls.Add(this.txtDisplay);
            this.pnlDisplayCard.Controls.Add(this.lblExpressionHistory);

            // pnlToolbar
            this.pnlToolbar.Dock = DockStyle.Top;
            this.pnlToolbar.Height = 46;
            this.pnlToolbar.Padding = new Padding(0, 8, 0, 6);
            this.pnlToolbar.BackColor = ThemeHelper.DarkBackground;

            this.btnAngleMode.Dock = DockStyle.Left;
            this.btnAngleMode.Width = 220;
            this.btnAngleMode.FlatStyle = FlatStyle.Flat;
            this.btnAngleMode.BackColor = ThemeHelper.AccentBlue;
            this.btnAngleMode.ForeColor = Color.White;
            this.btnAngleMode.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            this.btnAngleMode.FlatAppearance.BorderSize = 0;
            this.btnAngleMode.Cursor = Cursors.Hand;

            this.lblKeyboardHint.Dock = DockStyle.Fill;
            this.lblKeyboardHint.Text = "💡 Klavye kısayolları aktif: Sayılar, +, -, *, /, Enter (=), Backspace (Sil), Esc (AC)";
            this.lblKeyboardHint.Font = new Font("Segoe UI", 9F);
            this.lblKeyboardHint.ForeColor = ThemeHelper.MutedText;
            this.lblKeyboardHint.TextAlign = ContentAlignment.MiddleRight;

            this.pnlToolbar.Controls.Add(this.lblKeyboardHint);
            this.pnlToolbar.Controls.Add(this.btnAngleMode);

            // pnlButtons
            this.pnlButtons.Dock = DockStyle.Fill;
            this.pnlButtons.BackColor = ThemeHelper.DarkBackground;

            this.pnlLeft.Controls.Add(this.pnlButtons);
            this.pnlLeft.Controls.Add(this.pnlToolbar);
            this.pnlLeft.Controls.Add(this.pnlDisplayCard);

            // SimpleCalculatorControl
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = ThemeHelper.DarkBackground;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlHeader);
            this.Name = "SimpleCalculatorControl";
            this.Size = new Size(880, 660);

            this.pnlToolbar.ResumeLayout(false);
            this.pnlDisplayCard.ResumeLayout(false);
            this.pnlDisplayCard.PerformLayout();
            this.pnlRightHistory.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Panel pnlMain;
        private Panel pnlLeft;
        private Panel pnlRightHistory;
        private Panel pnlDisplayCard;
        private Label lblExpressionHistory;
        private TextBox txtDisplay;
        private Panel pnlToolbar;
        private Button btnAngleMode;
        private Label lblKeyboardHint;
        private TableLayoutPanel pnlButtons;
        private Label lblHistoryTitle;
        private ListBox lstHistory;
        private Button btnClearHistory;
    }
}
