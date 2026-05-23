
namespace MathApp.Controls
{
    partial class RootFinderControl
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
            
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.lblEquation = new System.Windows.Forms.Label();
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.txtVariable = new System.Windows.Forms.TextBox();
            this.lblInitialPoint = new System.Windows.Forms.Label();
            this.txtInitialPoint = new System.Windows.Forms.TextBox();
            this.lblMinX = new System.Windows.Forms.Label();
            this.txtMinX = new System.Windows.Forms.TextBox();
            this.lblMaxX = new System.Windows.Forms.Label();
            this.txtMaxX = new System.Windows.Forms.TextBox();
            this.lblIterations = new System.Windows.Forms.Label();
            this.txtIterations = new System.Windows.Forms.TextBox();
            this.btnFindRoot = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();

            this.pnlContent.SuspendLayout();
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
            this.lblTitle.Text = "Equation Root Finder";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtResult);
            this.pnlContent.Controls.Add(this.lblResult);
            this.pnlContent.Controls.Add(this.btnFindRoot);
            this.pnlContent.Controls.Add(this.txtIterations);
            this.pnlContent.Controls.Add(this.lblIterations);
            this.pnlContent.Controls.Add(this.txtMaxX);
            this.pnlContent.Controls.Add(this.lblMaxX);
            this.pnlContent.Controls.Add(this.txtMinX);
            this.pnlContent.Controls.Add(this.lblMinX);
            this.pnlContent.Controls.Add(this.txtInitialPoint);
            this.pnlContent.Controls.Add(this.lblInitialPoint);
            this.pnlContent.Controls.Add(this.txtVariable);
            this.pnlContent.Controls.Add(this.lblVariable);
            this.pnlContent.Controls.Add(this.txtEquation);
            this.pnlContent.Controls.Add(this.lblEquation);
            this.pnlContent.Controls.Add(this.cmbMethod);
            this.pnlContent.Controls.Add(this.lblMethod);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 60);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 540);
            this.pnlContent.TabIndex = 1;

            // Simple layout similar to previous, but styling updated in logic or here
            int yObject = 20;

            // Helper to style labels
            void StyleLabel(System.Windows.Forms.Label l) {
                l.AutoSize = true;
                l.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                l.ForeColor = System.Drawing.Color.White;
            }

            StyleLabel(this.lblMethod);
            this.lblMethod.Location = new System.Drawing.Point(20, yObject);
            this.lblMethod.Text = "Method:";
            
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] { "Newton-Raphson", "Bisection", "Secant", "Regula Falsi" });
            this.cmbMethod.Location = new System.Drawing.Point(150, yObject);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(200, 25);
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            yObject += 50;

            StyleLabel(this.lblEquation);
            this.lblEquation.Location = new System.Drawing.Point(20, yObject);
            this.lblEquation.Text = "Equation:";
            this.txtEquation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtEquation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEquation.ForeColor = System.Drawing.Color.White;
            this.txtEquation.Location = new System.Drawing.Point(150, yObject);
            this.txtEquation.Size = new System.Drawing.Size(400, 25);
            yObject += 50;

            StyleLabel(this.lblVariable);
            this.lblVariable.Location = new System.Drawing.Point(20, yObject);
            this.lblVariable.Text = "Variable (e.g. x):";
            this.txtVariable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtVariable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVariable.ForeColor = System.Drawing.Color.White;
            this.txtVariable.Location = new System.Drawing.Point(150, yObject);
            this.txtVariable.Size = new System.Drawing.Size(100, 25);
            yObject += 50;

            StyleLabel(this.lblInitialPoint);
            this.lblInitialPoint.Location = new System.Drawing.Point(20, yObject);
            this.lblInitialPoint.Text = "Initial Point:";
            this.txtInitialPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtInitialPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInitialPoint.ForeColor = System.Drawing.Color.White;
            this.txtInitialPoint.Location = new System.Drawing.Point(150, yObject);
            this.txtInitialPoint.Size = new System.Drawing.Size(100, 25);
            
            StyleLabel(this.lblMinX);
            this.lblMinX.Location = new System.Drawing.Point(20, yObject); 
            this.lblMinX.Text = "Min X:";
            this.txtMinX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtMinX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMinX.ForeColor = System.Drawing.Color.White;
            this.txtMinX.Location = new System.Drawing.Point(150, yObject);
            this.txtMinX.Size = new System.Drawing.Size(100, 25);
            
            StyleLabel(this.lblMaxX);
            this.lblMaxX.Location = new System.Drawing.Point(280, yObject);
            this.lblMaxX.Text = "Max X:";
            this.txtMaxX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtMaxX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMaxX.ForeColor = System.Drawing.Color.White;
            this.txtMaxX.Location = new System.Drawing.Point(340, yObject);
            this.txtMaxX.Size = new System.Drawing.Size(100, 25);
            yObject += 50;

            StyleLabel(this.lblIterations);
            this.lblIterations.Location = new System.Drawing.Point(20, yObject);
            this.lblIterations.Text = "Iterations:";
            this.txtIterations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtIterations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIterations.ForeColor = System.Drawing.Color.White;
            this.txtIterations.Location = new System.Drawing.Point(150, yObject);
            this.txtIterations.Size = new System.Drawing.Size(100, 25);
            this.txtIterations.Text = "100";
            yObject += 60;

            this.btnFindRoot.Location = new System.Drawing.Point(150, yObject);
            this.btnFindRoot.Name = "btnFindRoot";
            this.btnFindRoot.Size = new System.Drawing.Size(150, 40);
            this.btnFindRoot.Text = "Find Root";
            this.btnFindRoot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindRoot.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnFindRoot.ForeColor = System.Drawing.Color.White;
            this.btnFindRoot.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnFindRoot.FlatAppearance.BorderSize = 0;
            yObject += 60;

            StyleLabel(this.lblResult);
            this.lblResult.Location = new System.Drawing.Point(20, yObject);
            this.lblResult.Text = "Result:";
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResult.ForeColor = System.Drawing.Color.White;
            this.txtResult.Location = new System.Drawing.Point(150, yObject);
            this.txtResult.Size = new System.Drawing.Size(600, 150);
            this.txtResult.Multiline = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 10F);

            // 
            // RootFinderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Name = "RootFinderControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblMethod;
        public System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label lblEquation;
        public System.Windows.Forms.TextBox txtEquation;
        private System.Windows.Forms.Label lblVariable;
        public System.Windows.Forms.TextBox txtVariable;
        private System.Windows.Forms.Label lblInitialPoint;
        public System.Windows.Forms.TextBox txtInitialPoint;
        private System.Windows.Forms.Label lblMinX;
        public System.Windows.Forms.TextBox txtMinX;
        private System.Windows.Forms.Label lblMaxX;
        public System.Windows.Forms.TextBox txtMaxX;
        private System.Windows.Forms.Label lblIterations;
        public System.Windows.Forms.TextBox txtIterations;
        public System.Windows.Forms.Button btnFindRoot;
        private System.Windows.Forms.Label lblResult;
        public System.Windows.Forms.TextBox txtResult;
    }
}
