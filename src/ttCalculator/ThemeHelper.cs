using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MathApp
{
    public static class ThemeHelper
    {
        // Modern Dark Slate / Neon Accent Color Palette
        public static readonly Color DarkBackground = Color.FromArgb(15, 23, 42);     // #0F172A Slate 900
        public static readonly Color SidebarBackground = Color.FromArgb(11, 15, 28);  // #0B0F1C Deep Void Slate
        public static readonly Color CardBackground = Color.FromArgb(24, 33, 52);     // #182134 Modern Card Slate
        public static readonly Color CardBackgroundHover = Color.FromArgb(30, 41, 64);// #1E2940 Hover Card
        public static readonly Color CardHeader = Color.FromArgb(33, 44, 70);         // #212C46 Card Header
        public static readonly Color InputBackground = Color.FromArgb(15, 23, 42);    // #0F172A Slate 900
        public static readonly Color ButtonSecondary = Color.FromArgb(47, 59, 82);    // #2F3B52 Slate 700
        public static readonly Color ButtonHover = Color.FromArgb(64, 78, 107);       // #404E6B Slate 600
        public static readonly Color AccentBlue = Color.FromArgb(14, 165, 233);       // #0EA5E9 Sky 500
        public static readonly Color AccentBlueHover = Color.FromArgb(56, 189, 248);  // #38BDF8 Sky 400
        public static readonly Color AccentIndigo = Color.FromArgb(99, 102, 241);     // #6366F1 Indigo 500
        public static readonly Color AccentPurple = Color.FromArgb(168, 85, 247);     // #A855F7 Purple 500
        public static readonly Color LightText = Color.FromArgb(248, 250, 252);       // #F8FAFC Slate 50
        public static readonly Color MutedText = Color.FromArgb(148, 163, 184);       // #94A3B8 Slate 400
        public static readonly Color BorderColor = Color.FromArgb(51, 65, 85);        // #334155 Slate 700
        public static readonly Color BorderLight = Color.FromArgb(71, 85, 105);       // #475569 Slate 600
        public static readonly Color DangerRed = Color.FromArgb(239, 68, 68);         // #EF4444 Red 500
        public static readonly Color SuccessGreen = Color.FromArgb(16, 185, 129);     // #10B981 Emerald 500
        public static readonly Color AmberWarning = Color.FromArgb(245, 158, 11);     // #F59E0B Amber 500

        public static void ApplyTheme(Form form)
        {
            form.BackColor = DarkBackground;
            form.ForeColor = LightText;
        }

        public static Button CreateActionButton(string text, int width = 160, int height = 36, bool isPrimary = true)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = isPrimary ? AccentBlue : ButtonSecondary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = isPrimary ? AccentBlueHover : ButtonHover;
            btn.FlatAppearance.MouseDownBackColor = isPrimary ? Color.FromArgb(2, 132, 199) : Color.FromArgb(30, 41, 59);
            return btn;
        }

        public static Button CreateExampleChip(string text)
        {
            Button btn = new Button
            {
                Text = text,
                AutoSize = true,
                MinimumSize = new Size(80, 28),
                Padding = new Padding(10, 2, 10, 2),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(20, 29, 47),
                ForeColor = Color.FromArgb(186, 230, 253),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 8, 0),
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 233);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 58, 138);
            return btn;
        }

        public static Button CreateCompactChip(string text)
        {
            Button btn = new Button
            {
                Text = text,
                AutoSize = true,
                MinimumSize = new Size(54, 24),
                Height = 24,
                Padding = new Padding(6, 1, 6, 1),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(24, 33, 52),
                ForeColor = Color.FromArgb(186, 230, 253),
                Font = new Font("Segoe UI", 8.2F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 0),
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 58, 138);
            return btn;
        }

        public static Button CreateSegmentedButton(string text, int width = 48, int height = 26)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(24, 33, 52),
                ForeColor = Color.FromArgb(226, 232, 240),
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 65, 96);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(14, 165, 233);
            return btn;
        }

        public static Panel CreateHeaderPanel(string title, string subtitle)
        {
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = Color.FromArgb(13, 20, 36),
                Padding = new Padding(24, 10, 24, 8)
            };

            // Vibrant left accent indicator pill
            Panel accentPill = new Panel
            {
                Location = new Point(14, 14),
                Size = new Size(4, 44),
                BackColor = AccentBlue
            };

            Label lblTitle = new Label
            {
                Text = title,
                UseMnemonic = false,
                Location = new Point(26, 11),
                AutoSize = false,
                Size = new Size(950, 26),
                Font = new Font("Segoe UI Semibold", 14.5F, FontStyle.Bold),
                ForeColor = LightText,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblSub = new Label
            {
                Text = subtitle,
                UseMnemonic = false,
                Location = new Point(26, 39),
                AutoSize = false,
                Size = new Size(950, 22),
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

            pnlHeader.Controls.Add(accentPill);
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
                Padding = new Padding(10, 2, 10, 2)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ButtonHover;
            return btn;
        }

        public static void StyleModernTabControl(TabControl tabControl)
        {
            if (tabControl == null) return;

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(Math.Max(58, (tabControl.Width > 50 ? tabControl.Width - 14 : 306) / Math.Max(1, tabControl.TabCount)), 32);

            tabControl.DrawItem += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                TabPage page = tabControl.TabPages[e.Index];
                Rectangle tabRect = tabControl.GetTabRect(e.Index);
                bool isSelected = (tabControl.SelectedIndex == e.Index);

                Color bg = isSelected ? CardBackground : Color.FromArgb(13, 20, 36);
                using (Brush b = new SolidBrush(bg))
                {
                    g.FillRectangle(b, tabRect);
                }

                if (isSelected)
                {
                    // Active accent underline
                    using (Brush barBrush = new SolidBrush(AccentBlue))
                    {
                        g.FillRectangle(barBrush, tabRect.X + 4, tabRect.Bottom - 3, tabRect.Width - 8, 3);
                    }
                }

                Color textColor = isSelected ? Color.FromArgb(56, 189, 248) : MutedText;
                using (Font font = new Font("Segoe UI Semibold", 8.5F, isSelected ? FontStyle.Bold : FontStyle.Regular))
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(page.Text, font, textBrush, tabRect, sf);
                }
            };
        }

        public static void EnableDoubleBuffering(Control control)
        {
            if (control == null) return;
            try
            {
                var prop = typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                prop?.SetValue(control, true, null);
            }
            catch { }

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }
    }
}
