using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp
{
    public static class ThemeHelper
    {
        // Modern Dark Slate Color Palette
        public static readonly Color DarkBackground = Color.FromArgb(15, 23, 42);     // #0F172A Slate 900
        public static readonly Color SidebarBackground = Color.FromArgb(11, 17, 32);  // #0B1120 Deep Slate
        public static readonly Color CardBackground = Color.FromArgb(30, 41, 59);     // #1E293B Slate 800
        public static readonly Color CardHeader = Color.FromArgb(40, 53, 76);         // #28354C
        public static readonly Color InputBackground = Color.FromArgb(15, 23, 42);    // #0F172A Slate 900
        public static readonly Color ButtonSecondary = Color.FromArgb(51, 65, 85);    // #334155 Slate 700
        public static readonly Color ButtonHover = Color.FromArgb(71, 85, 105);       // #475569 Slate 600
        public static readonly Color AccentBlue = Color.FromArgb(14, 165, 233);       // #0EA5E9 Sky 500
        public static readonly Color AccentBlueHover = Color.FromArgb(56, 189, 248);  // #38BDF8 Sky 400
        public static readonly Color AccentIndigo = Color.FromArgb(99, 102, 241);     // #6366F1 Indigo 500
        public static readonly Color LightText = Color.FromArgb(248, 250, 252);       // #F8FAFC Slate 50
        public static readonly Color MutedText = Color.FromArgb(148, 163, 184);       // #94A3B8 Slate 400
        public static readonly Color BorderColor = Color.FromArgb(51, 65, 85);        // #334155 Slate 700
        public static readonly Color DangerRed = Color.FromArgb(239, 68, 68);         // #EF4444 Red 500
        public static readonly Color SuccessGreen = Color.FromArgb(16, 185, 129);     // #10B981 Emerald 500
        public static readonly Color AmberWarning = Color.FromArgb(245, 158, 11);     // #F59E0B Amber 500

        public static void ApplyTheme(Form form)
        {
            form.BackColor = DarkBackground;
            form.ForeColor = LightText;
        }

        public static Button CreateActionButton(string text, int width = 160, int height = 38, bool isPrimary = true)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = isPrimary ? AccentBlue : ButtonSecondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isPrimary ? AccentBlueHover : ButtonHover;
            return btn;
        }

        public static Button CreateExampleChip(string text)
        {
            Button btn = new Button
            {
                Text = text,
                AutoSize = true,
                MinimumSize = new Size(80, 28),
                Padding = new Padding(8, 2, 8, 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.FromArgb(186, 230, 253),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 8, 0),
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(56, 189, 248);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 65, 85);
            return btn;
        }

        public static Panel CreateHeaderPanel(string title, string subtitle)
        {
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = Color.FromArgb(15, 23, 42),
                Padding = new Padding(24, 12, 24, 8)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 30,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = LightText,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblSub = new Label
            {
                Text = subtitle,
                Dock = DockStyle.Bottom,
                AutoSize = false,
                Height = 22,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = MutedText,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Panel divider = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = BorderColor
            };

            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(divider);
            return pnlHeader;
        }

        public static Label CreateHintBadge(string text)
        {
            Label lbl = new Label
            {
                Text = text,
                AutoSize = false,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(186, 230, 253),
                BackColor = Color.FromArgb(20, 32, 50),
                Padding = new Padding(8, 4, 8, 4),
                TextAlign = ContentAlignment.MiddleLeft
            };
            return lbl;
        }

        public static Button CreateCopyButton(string text = "📋 Sonucu Kopyala")
        {
            Button btn = new Button
            {
                Text = text,
                AutoSize = true,
                Height = 28,
                FlatStyle = FlatStyle.Flat,
                BackColor = ButtonSecondary,
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand,
                Padding = new Padding(8, 2, 8, 2)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ButtonHover;
            return btn;
        }
    }
}
