using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MathApp.Controls
{
    public partial class GraphPlotterControl : UserControl
    {
        private List<PointF> points = new List<PointF>();
        private List<PointF> markedPoints = new List<PointF>(); // Points marked by user
        
        // Logical ranges
        private float rangeMinX = -10;
        private float rangeMaxX = 10;
        private float rangeMinY = -10; 
        private float rangeMaxY = 10;
        
        private string currentEq = "";

        // Mouse interaction state
        private bool isDragging = false;
        private Point lastMousePos;
        
        public GraphPlotterControl()
        {
            InitializeComponent();
            btnPlot.Click += BtnPlot_Click;
            
            // Enable double buffering
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            // Attach mouse events to the canvas panel
            pnlCanvas.MouseDown += PnlCanvas_MouseDown;
            pnlCanvas.MouseMove += PnlCanvas_MouseMove;
            pnlCanvas.MouseUp += PnlCanvas_MouseUp;
            pnlCanvas.MouseClick += PnlCanvas_MouseClick;
            pnlCanvas.Resize += pnlCanvas_Resize;
            pnlCanvas.Paint += pnlCanvas_Paint;
        }

        private void BtnPlot_Click(object sender, EventArgs e)
        {
            currentEq = txtEquation.Text;
            markedPoints.Clear(); // Clear marks on new plot? Or keep them? Let's clear for fresh start.
            if (string.IsNullOrWhiteSpace(currentEq)) return;

            rangeMinX = parse(txtMinX.Text, -10);
            rangeMaxX = parse(txtMaxX.Text, 10);

            GeneratePoints();
            pnlCanvas.Invalidate();
        }

        private float parse(string s, float def)
        {
            if (float.TryParse(s, out float v)) return v;
            return def;
        }

        private void GeneratePoints()
        {
            points.Clear();
            float step = (rangeMaxX - rangeMinX) / 2000f; // Higher resolution

            float tempMinY = float.MaxValue;
            float tempMaxY = float.MinValue;

            for (float x = rangeMinX; x <= rangeMaxX; x += step)
            {
                try
                {
                    decimal val = NumericalMethods.Method_EvaluateFunction(currentEq, "x", (decimal)x);
                    float y = (float)val;

                    // Clamp excessively large values to avoid float overflow issues in GDI+
                    if (y > 10000) y = 10000;
                    if (y < -10000) y = -10000;

                    points.Add(new PointF(x, y));

                    if (y < tempMinY) tempMinY = y;
                    if (y > tempMaxY) tempMaxY = y;
                }
                catch
                {
                    // Ignore calculation errors (e.g. division by zero, out of domain)
                }
            }
            
            // Auto-scale Y if it's the first plot or if we want to snap to range
            // But if we are panning, we probably shouldn't auto-reset Y every time unless explicit.
            // For now, let's set Y range based on data with some padding
            if (tempMinY < tempMaxY)
            {
                float h = tempMaxY - tempMinY;
                if (h == 0) h = 10;
                rangeMinY = tempMinY - h * 0.1f;
                rangeMaxY = tempMaxY + h * 0.1f;
            }
            else
            {
                rangeMinY = -10;
                rangeMaxY = 10;
            }
        }

        // --- Mouse Interaction ---

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // Zoom Factor
            float zoom = (e.Delta > 0) ? 0.9f : 1.1f;
            
            // Calculate center
            float rangeW = rangeMaxX - rangeMinX;
            float rangeH = rangeMaxY - rangeMinY;
            
            float midX = (rangeMaxX + rangeMinX) / 2;
            float midY = (rangeMaxY + rangeMinY) / 2;

            // Apply zoom centered
            float halfW = (rangeW * zoom) / 2;
            float halfH = (rangeH * zoom) / 2;
            
            rangeMinX = midX - halfW;
            rangeMaxX = midX + halfW;
            rangeMinY = midY - halfH;
            rangeMaxY = midY + halfH;
            
            GeneratePoints();
            pnlCanvas.Invalidate();
            
            base.OnMouseWheel(e); // Let it bubble if needed
        }

        private void PnlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePos = e.Location;
                Cursor = Cursors.SizeAll;
            }
        }

        private void PnlCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                float dx = e.X - lastMousePos.X;
                float dy = e.Y - lastMousePos.Y;

                float rangeW = rangeMaxX - rangeMinX;
                float rangeH = rangeMaxY - rangeMinY;

                float w = pnlCanvas.Width;
                float h = pnlCanvas.Height;

                // Scale pixel delta to logical delta
                // Note: dy is inverted because screen Y grows down, but math Y grows up
                float moveX = -dx * (rangeW / w);
                float moveY = dy * (rangeH / h); 

                rangeMinX += moveX;
                rangeMaxX += moveX;
                rangeMinY += moveY;
                rangeMaxY += moveY;

                lastMousePos = e.Location;
                
                // Re-generate points if we moved significantly to ensure we cover the new X range?
                // Or just shift the view? 
                // If we shift X, we need new data points for the new X area.
                // So we should re-generate.
                GeneratePoints(); 
                
                pnlCanvas.Invalidate();
            }
        }

        private void PnlCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                Cursor = Cursors.Default;
            }
        }

        private void PnlCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            // Right click or simple click (if not dragging) to mark point
            if (!isDragging && e.Button == MouseButtons.Left)
            {
                // Unmap screen to logical
                float w = pnlCanvas.Width;
                float h = pnlCanvas.Height;
                
                float xPct = e.X / w;
                float yPct = (h - e.Y) / h; // Invert Y

                float logX = rangeMinX + xPct * (rangeMaxX - rangeMinX);
                float logY = rangeMinY + yPct * (rangeMaxY - rangeMinY);

                markedPoints.Add(new PointF(logX, logY));
                pnlCanvas.Invalidate();
            }
        }

        // --- Rendering ---

        private void pnlCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float w = pnlCanvas.Width;
            float h = pnlCanvas.Height;

            // Map functions
            float MapX(float x) => (x - rangeMinX) / (rangeMaxX - rangeMinX) * w;
            float MapY(float y) => h - ((y - rangeMinY) / (rangeMaxY - rangeMinY) * h);

            // 1. Draw Grid
            DrawGrid(g, w, h, MapX, MapY);

            // 2. Draw Axes
            using (Pen axisPen = new Pen(Color.White, 2))
            {
                float y0 = MapY(0);
                float x0 = MapX(0);

                if (y0 >= 0 && y0 <= h) g.DrawLine(axisPen, 0, y0, w, y0);
                if (x0 >= 0 && x0 <= w) g.DrawLine(axisPen, x0, 0, x0, h);
            }

            // 3. Draw Function
            if (points.Count > 1)
            {
                using (Pen graphPen = new Pen(Color.Cyan, 2))
                {
                    List<PointF> screenPoints = new List<PointF>();
                    foreach (var p in points)
                    {
                        screenPoints.Add(new PointF(MapX(p.X), MapY(p.Y)));
                    }

                    // Draw valid segments
                    // We shouldn't connect jumps between valid/invalid or huge leaps
                    // For simplicity, we draw lines.
                    try 
                    {
                       // Clipping helps performance but GDI+ handles basic out-of-bounds well
                       // unless coordinates are floats.NaN or Infinity.
                       g.DrawLines(graphPen, screenPoints.ToArray());
                    } 
                    catch { }
                }
            }

            // 4. Draw Marked Points
            using(Brush markBrush = new SolidBrush(Color.Yellow))
            using(Pen markPen = new Pen(Color.Red, 2))
            using(Font markFont = new Font("Segoe UI", 9))
            {
                foreach(var p in markedPoints)
                {
                    float sx = MapX(p.X);
                    float sy = MapY(p.Y);
                    
                    // Draw X mark
                    float r = 5;
                    g.DrawLine(markPen, sx - r, sy - r, sx + r, sy + r);
                    g.DrawLine(markPen, sx + r, sy - r, sx - r, sy + r);
                    
                    // Draw coordinates text
                    string coord = $"({p.X:F2}, {p.Y:F2})";
                    g.DrawString(coord, markFont, markBrush, sx + 5, sy - 20);
                }
            }
        }

        private void DrawGrid(Graphics g, float w, float h, Func<float, float> mapX, Func<float, float> mapY)
        {
            using (Pen gridPen = new Pen(Color.FromArgb(50, 50, 50), 1))
            using (Font font = new Font("Segoe UI", 8))
            using (Brush brush = new SolidBrush(Color.Gray))
            {
                // Calculate roughly how many lines we want
                int targetLinesW = 10;
                int targetLinesH = 10;

                float rangeW = rangeMaxX - rangeMinX;
                float stepW = CalculateStep(rangeW, targetLinesW);

                float rangeH = rangeMaxY - rangeMinY;
                float stepH = CalculateStep(rangeH, targetLinesH);

                // Vertical Grid Lines
                float startX = (float)Math.Floor(rangeMinX / stepW) * stepW;
                for (float x = startX; x <= rangeMaxX; x += stepW)
                {
                    float sx = mapX(x);
                    g.DrawLine(gridPen, sx, 0, sx, h);
                    g.DrawString(x.ToString("0.##"), font, brush, sx + 2, h - 20);
                }

                // Horizontal Grid Lines
                float startY = (float)Math.Floor(rangeMinY / stepH) * stepH;
                for (float y = startY; y <= rangeMaxY; y += stepH)
                {
                    float sy = mapY(y);
                    g.DrawLine(gridPen, 0, sy, w, sy);
                    g.DrawString(y.ToString("0.##"), font, brush, 5, sy - 15);
                }
            }
        }
        
        private float CalculateStep(float range, int targetSteps)
        {
             float rawStep = range / targetSteps;
             float mag = (float)Math.Pow(10, Math.Floor(Math.Log10(rawStep)));
             float normStep = rawStep / mag;
             
             if (normStep < 1.5) return 1 * mag;
             if (normStep < 3.5) return 2 * mag;
             if (normStep < 7.5) return 5 * mag;
             return 10 * mag;
        }

        private void pnlCanvas_Resize(object sender, EventArgs e)
        {
            pnlCanvas.Invalidate();
        }
    }
}
