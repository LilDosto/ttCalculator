
namespace MathApp.Controls
{
    partial class TaylorSeriesControl
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
            
            this.lblFunction = new System.Windows.Forms.Label();
            this.txtFunction = new System.Windows.Forms.TextBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.txtVariable = new System.Windows.Forms.TextBox();
            this.lblCenterPoint = new System.Windows.Forms.Label();
            this.txtCenterPoint = new System.Windows.Forms.TextBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.numOrder = new System.Windows.Forms.NumericUpDown();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();

            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
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
            this.lblTitle.Text = "Taylor Series Calculator";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.txtResult);
            this.pnlContent.Controls.Add(this.lblResult);
            this.pnlContent.Controls.Add(this.btnCalculate);
            this.pnlContent.Controls.Add(this.numOrder);
            this.pnlContent.Controls.Add(this.lblOrder);
            this.pnlContent.Controls.Add(this.txtCenterPoint);
            this.pnlContent.Controls.Add(this.lblCenterPoint);
            this.pnlContent.Controls.Add(this.txtVariable);
            this.pnlContent.Controls.Add(this.lblVariable);
            this.pnlContent.Controls.Add(this.txtFunction);
            this.pnlContent.Controls.Add(this.lblFunction);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 60);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 540);
            this.pnlContent.TabIndex = 1;

            int yObject = 20;

            void StyleLabel(System.Windows.Forms.Label l) {
                l.AutoSize = true;
                l.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                l.ForeColor = System.Drawing.Color.White;
            }

            StyleLabel(this.lblFunction);
            this.lblFunction.Location = new System.Drawing.Point(20, yObject);
            this.lblFunction.Text = "Function f(x):";
            
            this.txtFunction.Location = new System.Drawing.Point(150, yObject);
            this.txtFunction.Size = new System.Drawing.Size(400, 25);
            this.txtFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtFunction.ForeColor = System.Drawing.Color.White;
            this.txtFunction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            yObject += 50;

            StyleLabel(this.lblVariable);
            this.lblVariable.Location = new System.Drawing.Point(20, yObject);
            this.lblVariable.Text = "Variable (e.g. x):";
            this.txtVariable.Location = new System.Drawing.Point(150, yObject);
            this.txtVariable.Size = new System.Drawing.Size(100, 25);
            this.txtVariable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtVariable.ForeColor = System.Drawing.Color.White;
            this.txtVariable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVariable.Text = "x";
            yObject += 50;

            StyleLabel(this.lblCenterPoint);
            this.lblCenterPoint.Location = new System.Drawing.Point(20, yObject);
            this.lblCenterPoint.Text = "Center Point (a):";
            this.txtCenterPoint.Location = new System.Drawing.Point(150, yObject);
            this.txtCenterPoint.Size = new System.Drawing.Size(100, 25);
            this.txtCenterPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtCenterPoint.ForeColor = System.Drawing.Color.White;
            this.txtCenterPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCenterPoint.Text = "0";
            yObject += 50;

            StyleLabel(this.lblOrder);
            this.lblOrder.Location = new System.Drawing.Point(20, yObject);
            this.lblOrder.Text = "Order (n):";
            this.numOrder.Location = new System.Drawing.Point(150, yObject);
            this.numOrder.Minimum = 1;
            this.numOrder.Maximum = 20;
            this.numOrder.Value = 4;
            this.numOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.numOrder.ForeColor = System.Drawing.Color.White;
            this.numOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            yObject += 60;

            this.btnCalculate.Location = new System.Drawing.Point(150, yObject);
            this.btnCalculate.Text = "Calculate Series";
            this.btnCalculate.Size = new System.Drawing.Size(150, 40);
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCalculate.FlatAppearance.BorderSize = 0;
            yObject += 60;

            StyleLabel(this.lblResult);
            this.lblResult.Location = new System.Drawing.Point(20, yObject);
            this.lblResult.Text = "Result:";
            this.txtResult.Location = new System.Drawing.Point(150, yObject);
            this.txtResult.Size = new System.Drawing.Size(600, 150);
            this.txtResult.Multiline = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtResult.ForeColor = System.Drawing.Color.White;
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            // 
            // TaylorSeriesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTitle);
            this.Name = "TaylorSeriesControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblFunction;
        private System.Windows.Forms.TextBox txtFunction;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.TextBox txtVariable;
        private System.Windows.Forms.Label lblCenterPoint;
        private System.Windows.Forms.TextBox txtCenterPoint;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.NumericUpDown numOrder;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
    }
}
