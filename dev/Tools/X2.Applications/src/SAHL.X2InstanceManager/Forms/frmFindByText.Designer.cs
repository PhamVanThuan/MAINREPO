namespace SAHL.X2InstanceManager.Forms
{
    partial class frmFindByText
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
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioEqual = new System.Windows.Forms.RadioButton();
            this.radioNotEqual = new System.Windows.Forms.RadioButton();
            this.radioOr = new System.Windows.Forms.RadioButton();
            this.radioAnd = new System.Windows.Forms.RadioButton();
            this.checkExplicit = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(11, 131);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(88, 23);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Description :";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(103, 131);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(229, 20);
            this.txtValue.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(176, 187);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(261, 187);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.radioOr);
            this.groupBox1.Controls.Add(this.radioAnd);
            this.groupBox1.Location = new System.Drawing.Point(58, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 104);
            this.groupBox1.TabIndex = 11;
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
            this.radioAnd.CheckedChanged += new System.EventHandler(this.radioAnd_CheckedChanged);
            // 
            // checkExplicit
            // 
            this.checkExplicit.AutoSize = true;
            this.checkExplicit.Location = new System.Drawing.Point(15, 193);
            this.checkExplicit.Name = "checkExplicit";
            this.checkExplicit.Size = new System.Drawing.Size(113, 17);
            this.checkExplicit.TabIndex = 12;
            this.checkExplicit.Text = "Match whole word";
            this.checkExplicit.UseVisualStyleBackColor = true;
            // 
            // frmFindByText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 240);
            this.ControlBox = false;
            this.Controls.Add(this.checkExplicit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmFindByText";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Criteria";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmFindByText_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.Label lblDescription;
        public System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioAnd;
        public System.Windows.Forms.RadioButton radioOr;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.RadioButton radioEqual;
        public System.Windows.Forms.RadioButton radioNotEqual;
        public System.Windows.Forms.CheckBox checkExplicit;
    }
}