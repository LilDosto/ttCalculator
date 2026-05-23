
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlGraphConfig = new System.Windows.Forms.Panel();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            
            this.lblEquation = new System.Windows.Forms.Label();
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.lblMinX = new System.Windows.Forms.Label();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.lblMaxX = new System.Windows.Forms.Label();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.btnPlot = new System.Windows.Forms.Button();

            this.pnlControls.SuspendLayout();
            this.pnlGraphConfig.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Graph Plotter";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.pnlGraphConfig);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 60);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(800, 80);
            this.pnlControls.TabIndex = 1;

            // 
            // pnlGraphConfig
            // 
            this.pnlGraphConfig.Controls.Add(this.btnPlot);
            this.pnlGraphConfig.Controls.Add(this.txtMaxX);
            this.pnlGraphConfig.Controls.Add(this.lblMaxX);
            this.pnlGraphConfig.Controls.Add(this.txtMinX);
            this.pnlGraphConfig.Controls.Add(this.lblMinX);
            this.pnlGraphConfig.Controls.Add(this.txtEquation);
            this.pnlGraphConfig.Controls.Add(this.lblEquation);
            this.pnlGraphConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraphConfig.Location = new System.Drawing.Point(0, 0);
            this.pnlGraphConfig.Name = "pnlGraphConfig";
            this.pnlGraphConfig.Size = new System.Drawing.Size(800, 80);
            this.pnlGraphConfig.TabIndex = 0;

            int xPos = 20; int yPos = 25;

            // Equation
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = System.Drawing.Color.White;
            this.lblEquation.Text = "y = ";
            this.lblEquation.Location = new System.Drawing.Point(xPos, yPos);
            xPos += 30;
            
            this.txtEquation.Location = new System.Drawing.Point(xPos, yPos-3);
            this.txtEquation.Size = new System.Drawing.Size(200, 23);
            this.txtEquation.BackColor = System.Drawing.Color.FromArgb(40,40,40);
            this.txtEquation.ForeColor = System.Drawing.Color.White;
            this.txtEquation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            xPos += 220;

            // Min X
            this.lblMinX.AutoSize = true;
            this.lblMinX.ForeColor = System.Drawing.Color.White;
            this.lblMinX.Text = "Min X:";
            this.lblMinX.Location = new System.Drawing.Point(xPos, yPos);
            xPos += 50;
            
            this.txtMinX.Location = new System.Drawing.Point(xPos, yPos-3);
            this.txtMinX.Size = new System.Drawing.Size(50, 23);
            this.txtMinX.Text = "-10";
            this.txtMinX.BackColor = System.Drawing.Color.FromArgb(40,40,40);
            this.txtMinX.ForeColor = System.Drawing.Color.White;
            this.txtMinX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            xPos += 70;

            // Max X
            this.lblMaxX.AutoSize = true;
            this.lblMaxX.ForeColor = System.Drawing.Color.White;
            this.lblMaxX.Text = "Max X:";
            this.lblMaxX.Location = new System.Drawing.Point(xPos, yPos);
            xPos += 50;

            this.txtMaxX.Location = new System.Drawing.Point(xPos, yPos-3);
            this.txtMaxX.Size = new System.Drawing.Size(50, 23);
            this.txtMaxX.Text = "10";
            this.txtMaxX.BackColor = System.Drawing.Color.FromArgb(40,40,40);
            this.txtMaxX.ForeColor = System.Drawing.Color.White;
            this.txtMaxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            xPos += 70;

            // Plot Button
            this.btnPlot.Location = new System.Drawing.Point(xPos, yPos-5);
            this.btnPlot.Size = new System.Drawing.Size(100, 30);
            this.btnPlot.Text = "Plot";
            this.btnPlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlot.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPlot.ForeColor = System.Drawing.Color.White;
            this.btnPlot.FlatAppearance.BorderSize = 0;


            // 
            // pnlCanvas
            // 
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 140);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Size = new System.Drawing.Size(800, 460);
            this.pnlCanvas.TabIndex = 2;
            this.pnlCanvas.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.pnlCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanvas_Paint);
            this.pnlCanvas.Resize += new System.EventHandler(this.pnlCanvas_Resize);

            // 
            // GraphPlotterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.lblTitle);
            this.Name = "GraphPlotterControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlControls.ResumeLayout(false);
            this.pnlGraphConfig.ResumeLayout(false);
            this.pnlGraphConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Panel pnlGraphConfig;
        private System.Windows.Forms.Panel pnlCanvas;
        
        private System.Windows.Forms.Label lblEquation;
        private System.Windows.Forms.TextBox txtEquation;
        private System.Windows.Forms.Label lblMinX;
        private System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.Label lblMaxX;
        private System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.Button btnPlot;
    }
}
