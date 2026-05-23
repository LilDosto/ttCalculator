
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
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.btnNavCalc = new System.Windows.Forms.Button();
            this.btnNavRoot = new System.Windows.Forms.Button();
            this.btnNavSystem = new System.Windows.Forms.Button();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();

            this.btnNavTaylor = new System.Windows.Forms.Button();
            this.btnNavMultiTaylor = new System.Windows.Forms.Button(); // New
            this.btnNavGraph = new System.Windows.Forms.Button();

            this.pnlSidebar.SuspendLayout();
            this.pnlLogo.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.pnlSidebar.Controls.Add(this.btnNavGraph);
            this.pnlSidebar.Controls.Add(this.btnNavMultiTaylor); // New
            this.pnlSidebar.Controls.Add(this.btnNavTaylor);
            this.pnlSidebar.Controls.Add(this.btnNavSystem);
            this.pnlSidebar.Controls.Add(this.btnNavRoot);
            this.pnlSidebar.Controls.Add(this.btnNavCalc);
            this.pnlSidebar.Controls.Add(this.pnlLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Right; // Sidebar on Right as requested
            this.pnlSidebar.Location = new System.Drawing.Point(600, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(200, 600);
            this.pnlSidebar.TabIndex = 0;

            // 
            // pnlLogo
            // 
            this.pnlLogo.Controls.Add(this.lblLogo);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(200, 100);
            this.pnlLogo.TabIndex = 0;

            // 
            // lblLogo
            // 
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Light", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(200, 100);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "Math App";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Helper for Nav Buttons
            void StyleNavButton(System.Windows.Forms.Button btn, string text, int index)
            {
                btn.Dock = System.Windows.Forms.DockStyle.Top;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
                btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Location = new System.Drawing.Point(0, 100 + (index * 60));
                btn.Name = "btnNav" + index;
                btn.Size = new System.Drawing.Size(200, 60);
                btn.TabIndex = index + 1;
                btn.Text = text;
                btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                btn.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
                btn.UseVisualStyleBackColor = true;
            }

            StyleNavButton(this.btnNavCalc, "Calculator", 0);
            StyleNavButton(this.btnNavRoot, "Root Finder", 1);
            StyleNavButton(this.btnNavSystem, "System Solver", 2);
            StyleNavButton(this.btnNavTaylor, "Taylor Series", 3);
            StyleNavButton(this.btnNavMultiTaylor, "MultiVar Taylor", 4); // New
            StyleNavButton(this.btnNavGraph, "Graph Plotter", 5);

            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20))))); // Dark Content Area
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(600, 600);
            this.pnlContent.TabIndex = 1;

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "MainForm";
            this.Text = "Math Application";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlLogo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnNavCalc;
        private System.Windows.Forms.Button btnNavRoot;
        private System.Windows.Forms.Button btnNavSystem;
        private System.Windows.Forms.Button btnNavTaylor;
        private System.Windows.Forms.Button btnNavMultiTaylor; // New
        private System.Windows.Forms.Button btnNavGraph;
        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label lblLogo;
    }
}
