namespace SAHL.X2InstanceManager.Forms
{
    partial class frmFindByWorkflow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbxWorkflow = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioEqual = new System.Windows.Forms.RadioButton();
            this.radioNotEqual = new System.Windows.Forms.RadioButton();
            this.radioOr = new System.Windows.Forms.RadioButton();
            this.radioAnd = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxWorkflow
            // 
            this.cbxWorkflow.FormattingEnabled = true;
            this.cbxWorkflow.Location = new System.Drawing.Point(124, 143);
            this.cbxWorkflow.Name = "cbxWorkflow";
            this.cbxWorkflow.Size = new System.Drawing.Size(190, 21);
            this.cbxWorkflow.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(27, 146);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(91, 13);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Select Workflow :";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(243, 178);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(162, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.radioOr);
            this.groupBox1.Controls.Add(this.radioAnd);
            this.groupBox1.Location = new System.Drawing.Point(53, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 104);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Where";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioEqual);
            this.groupBox2.Controls.Add(this.radioNotEqual);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Location = new System.Drawing.Point(31, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 45);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // radioEqual
            // 
            this.radioEqual.AutoSize = true;
            this.radioEqual.Checked = true;
            this.radioEqual.Location = new System.Drawing.Point(4, 19);
            this.radioEqual.Name = "radioEqual";
            this.radioEqual.Size = new System.Drawing.Size(64, 17);
            this.radioEqual.TabIndex = 1;
            this.radioEqual.TabStop = true;
            this.radioEqual.Text = "Equal to";
            this.radioEqual.UseVisualStyleBackColor = true;
            // 
            // radioNotEqual
            // 
            this.radioNotEqual.AutoSize = true;
            this.radioNotEqual.Location = new System.Drawing.Point(78, 19);
            this.radioNotEqual.Name = "radioNotEqual";
            this.radioNotEqual.Size = new System.Drawing.Size(84, 17);
            this.radioNotEqual.TabIndex = 0;
            this.radioNotEqual.Text = "Not Equal to";
            this.radioNotEqual.UseVisualStyleBackColor = true;
            // 
            // radioOr
            // 
            this.radioOr.AutoSize = true;
            this.radioOr.Location = new System.Drawing.Point(125, 19);
            this.radioOr.Name = "radioOr";
            this.radioOr.Size = new System.Drawing.Size(36, 17);
            this.radioOr.TabIndex = 0;
            this.radioOr.Text = "Or";
            this.radioOr.UseVisualStyleBackColor = true;
            // 
            // radioAnd
            // 
            this.radioAnd.AutoSize = true;
            this.radioAnd.Checked = true;
            this.radioAnd.Location = new System.Drawing.Point(65, 19);
            this.radioAnd.Name = "radioAnd";
            this.radioAnd.Size = new System.Drawing.Size(44, 17);
            this.radioAnd.TabIndex = 1;
            this.radioAnd.TabStop = true;
            this.radioAnd.Text = "And";
            this.radioAnd.UseVisualStyleBackColor = true;
            // 
            // frmFindByWorkflow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 209);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cbxWorkflow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmFindByWorkflow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Workflow Criteria";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmFindByWorkflow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox cbxWorkflow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.RadioButton radioEqual;
        public System.Windows.Forms.RadioButton radioNotEqual;
        public System.Windows.Forms.RadioButton radioOr;
        public System.Windows.Forms.RadioButton radioAnd;
    }
}