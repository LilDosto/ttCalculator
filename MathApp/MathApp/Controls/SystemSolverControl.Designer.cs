
namespace MathApp.Controls
{
    partial class SystemSolverControl
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
            this.pnlContent = new System.Windows.Forms.Panel();

            this.lblEqCount = new System.Windows.Forms.Label();
            this.numEqCount = new System.Windows.Forms.NumericUpDown();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.pnlEquations = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSystemIterations = new System.Windows.Forms.Label();
            this.txtSystemIterations = new System.Windows.Forms.TextBox();
            this.btnSolveSystem = new System.Windows.Forms.Button();
            this.lblSystemResult = new System.Windows.Forms.Label();
            this.txtSystemResult = new System.Windows.Forms.TextBox();

            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).BeginInit();
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
            this.lblTitle.Text = "Equation System Solver";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtSystemResult);
            this.pnlContent.Controls.Add(this.lblSystemResult);
            this.pnlContent.Controls.Add(this.btnSolveSystem);
            this.pnlContent.Controls.Add(this.txtSystemIterations);
            this.pnlContent.Controls.Add(this.lblSystemIterations);
            this.pnlContent.Controls.Add(this.pnlEquations);
            this.pnlContent.Controls.Add(this.btnGenerate);
            this.pnlContent.Controls.Add(this.numEqCount);
            this.pnlContent.Controls.Add(this.lblEqCount);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 60);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 540);
            this.pnlContent.TabIndex = 1;

            // Simple layout
            int yObject = 20;
            void StyleLabel(System.Windows.Forms.Label l) {
                l.AutoSize = true;
                l.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                l.ForeColor = System.Drawing.Color.White;
            }

            StyleLabel(this.lblEqCount);
            this.lblEqCount.Location = new System.Drawing.Point(20, yObject);
            this.lblEqCount.Text = "Number of Equations:";
            
            this.numEqCount.Location = new System.Drawing.Point(180, yObject);
            this.numEqCount.Minimum = 1;
            this.numEqCount.Value = 2;
            this.numEqCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.numEqCount.ForeColor = System.Drawing.Color.White;
            this.numEqCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            this.btnGenerate.Location = new System.Drawing.Point(310, yObject - 2);
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Size = new System.Drawing.Size(100, 25);
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            yObject += 50;

            this.pnlEquations.Location = new System.Drawing.Point(20, yObject);
            this.pnlEquations.Size = new System.Drawing.Size(750, 300);
            this.pnlEquations.AutoScroll = true;
            this.pnlEquations.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlEquations.WrapContents = false;
            this.pnlEquations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlEquations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            yObject += 320;

            StyleLabel(this.lblSystemIterations);
            this.lblSystemIterations.Location = new System.Drawing.Point(20, yObject);
            this.lblSystemIterations.Text = "Iterations:";
            this.txtSystemIterations.Location = new System.Drawing.Point(100, yObject);
            this.txtSystemIterations.Text = "100";
            this.txtSystemIterations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtSystemIterations.ForeColor = System.Drawing.Color.White;
            this.txtSystemIterations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            yObject += 50;

            this.btnSolveSystem.Location = new System.Drawing.Point(20, yObject);
            this.btnSolveSystem.Text = "Solve System";
            this.btnSolveSystem.Size = new System.Drawing.Size(150, 40);
            this.btnSolveSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSolveSystem.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSolveSystem.ForeColor = System.Drawing.Color.White;
            this.btnSolveSystem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSolveSystem.FlatAppearance.BorderSize = 0;
            yObject += 60;
            
            StyleLabel(this.lblSystemResult);
            this.lblSystemResult.Location = new System.Drawing.Point(20, yObject);
            this.lblSystemResult.Text = "Result:";
            this.txtSystemResult.Location = new System.Drawing.Point(100, yObject);
            this.txtSystemResult.Size = new System.Drawing.Size(650, 100);
            this.txtSystemResult.Multiline = true;
            this.txtSystemResult.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtSystemResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtSystemResult.ForeColor = System.Drawing.Color.White;
            this.txtSystemResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;


            // 
            // SystemSolverControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Name = "SystemSolverControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEqCount)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblEqCount;
        public System.Windows.Forms.NumericUpDown numEqCount;
        public System.Windows.Forms.Button btnGenerate;
        public System.Windows.Forms.FlowLayoutPanel pnlEquations;
        private System.Windows.Forms.Label lblSystemIterations;
        public System.Windows.Forms.TextBox txtSystemIterations;
        public System.Windows.Forms.Button btnSolveSystem;
        private System.Windows.Forms.Label lblSystemResult;
        public System.Windows.Forms.TextBox txtSystemResult;
    }
}
