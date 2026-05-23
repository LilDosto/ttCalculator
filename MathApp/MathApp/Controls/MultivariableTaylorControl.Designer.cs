
namespace MathApp.Controls
{
    partial class MultivariableTaylorControl
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
            this.lblEquation = new System.Windows.Forms.Label();
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.lblOrder = new System.Windows.Forms.Label();
            this.numOrder = new System.Windows.Forms.NumericUpDown();
            this.grpVariables = new System.Windows.Forms.GroupBox();
            this.pnlVariables = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddVar = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.grpVariables.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(280, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Multivariable Taylor Series";
            // 
            // lblEquation
            // 
            this.lblEquation.AutoSize = true;
            this.lblEquation.ForeColor = System.Drawing.Color.White;
            this.lblEquation.Location = new System.Drawing.Point(23, 70);
            this.lblEquation.Name = "lblEquation";
            this.lblEquation.Size = new System.Drawing.Size(89, 15);
            this.lblEquation.TabIndex = 1;
            this.lblEquation.Text = "Function f(...) =";
            // 
            // txtEquation
            // 
            this.txtEquation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtEquation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEquation.ForeColor = System.Drawing.Color.White;
            this.txtEquation.Location = new System.Drawing.Point(118, 68);
            this.txtEquation.Name = "txtEquation";
            this.txtEquation.Size = new System.Drawing.Size(300, 23);
            this.txtEquation.TabIndex = 2;
            this.txtEquation.Text = "sin(x)*cos(y)";
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.ForeColor = System.Drawing.Color.White;
            this.lblOrder.Location = new System.Drawing.Point(440, 70);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(74, 15);
            this.lblOrder.TabIndex = 3;
            this.lblOrder.Text = "Series Order:";
            // 
            // numOrder
            // 
            this.numOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.numOrder.ForeColor = System.Drawing.Color.White;
            this.numOrder.Location = new System.Drawing.Point(520, 68);
            this.numOrder.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numOrder.Name = "numOrder";
            this.numOrder.Size = new System.Drawing.Size(60, 23);
            this.numOrder.TabIndex = 4;
            this.numOrder.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // grpVariables
            // 
            this.grpVariables.Controls.Add(this.pnlVariables);
            this.grpVariables.Controls.Add(this.btnAddVar);
            this.grpVariables.ForeColor = System.Drawing.Color.White;
            this.grpVariables.Location = new System.Drawing.Point(23, 110);
            this.grpVariables.Name = "grpVariables";
            this.grpVariables.Size = new System.Drawing.Size(557, 200);
            this.grpVariables.TabIndex = 5;
            this.grpVariables.TabStop = false;
            this.grpVariables.Text = "Variables & Expansion Points";
            // 
            // pnlVariables
            // 
            this.pnlVariables.AutoScroll = true;
            this.pnlVariables.Location = new System.Drawing.Point(10, 25);
            this.pnlVariables.Name = "pnlVariables";
            this.pnlVariables.Size = new System.Drawing.Size(530, 130);
            this.pnlVariables.TabIndex = 1;
            // 
            // btnAddVar
            // 
            this.btnAddVar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAddVar.FlatAppearance.BorderSize = 0;
            this.btnAddVar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVar.Location = new System.Drawing.Point(10, 165);
            this.btnAddVar.Name = "btnAddVar";
            this.btnAddVar.Size = new System.Drawing.Size(120, 25);
            this.btnAddVar.TabIndex = 0;
            this.btnAddVar.Text = "+ Add Variable";
            this.btnAddVar.UseVisualStyleBackColor = false;
            // 
            // btnCalculate
            // 
            this.btnCalculate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCalculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalculate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCalculate.ForeColor = System.Drawing.Color.White;
            this.btnCalculate.Location = new System.Drawing.Point(23, 330);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(150, 40);
            this.btnCalculate.TabIndex = 6;
            this.btnCalculate.Text = "Calculate Expansion";
            this.btnCalculate.UseVisualStyleBackColor = false;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.ForeColor = System.Drawing.Color.White;
            this.lblResult.Location = new System.Drawing.Point(23, 390);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(42, 15);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "Result:";
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResult.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtResult.ForeColor = System.Drawing.Color.Cyan;
            this.txtResult.Location = new System.Drawing.Point(23, 410);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(557, 150);
            this.txtResult.TabIndex = 8;
            // 
            // MultivariableTaylorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.grpVariables);
            this.Controls.Add(this.numOrder);
            this.Controls.Add(this.lblOrder);
            this.Controls.Add(this.txtEquation);
            this.Controls.Add(this.lblEquation);
            this.Controls.Add(this.lblTitle);
            this.Name = "MultivariableTaylorControl";
            this.Size = new System.Drawing.Size(600, 600);
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.grpVariables.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEquation;
        private System.Windows.Forms.TextBox txtEquation;
        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.NumericUpDown numOrder;
        private System.Windows.Forms.GroupBox grpVariables;
        private System.Windows.Forms.FlowLayoutPanel pnlVariables;
        private System.Windows.Forms.Button btnAddVar;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.TextBox txtResult;
    }
}
