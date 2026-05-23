using System;
using System.Drawing;
using System.Windows.Forms;

namespace MathApp
{
    public static class ThemeHelper
    {
        // Color Palette
        public static Color DarkBackground = Color.FromArgb(32, 32, 32);
        public static Color DarkPanel = Color.FromArgb(45, 45, 48);
        public static Color LightText = Color.FromArgb(241, 241, 241);
        public static Color AccentBlue = Color.FromArgb(0, 122, 204);
        public static Color AccentDarkBlue = Color.FromArgb(14, 99, 156);
        public static Color BorderColor = Color.FromArgb(63, 63, 70);

        public static void ApplyTheme(Form form)
        {
            form.BackColor = DarkBackground;
            form.ForeColor = LightText;
            
            ApplyRecursively(form.Controls);
        }

        public static void ApplyRecursively(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = DarkPanel;
                    btn.ForeColor = LightText;
                    
                    // Simple hover effect hook if not already handled
                    // But we don't want to override custom event handlers easily.
                    // Just set initial properties.
                }
                else if (c is TextBox txt)
                {
                    txt.BackColor = Color.FromArgb(30, 30, 30);
                    txt.ForeColor = LightText;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (c is Panel pnl)
                {
                    // pnl.BackColor = DarkBackground; // Don't override panel colors if they are structural
                    // Apply to children
                    ApplyRecursively(pnl.Controls);
                }
                else if (c is Label lbl)
                {
                    lbl.ForeColor = LightText;
                }
                else if (c is GroupBox grp)
                {
                    grp.ForeColor = LightText;
                    ApplyRecursively(grp.Controls);
                }
                
                if (c.HasChildren && !(c is Panel) && !(c is GroupBox))
                {
                    ApplyRecursively(c.Controls);
                }
            }
        }
    }
}
