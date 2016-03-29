namespace SAHL.X2InstanceManager.Forms
{
    partial class frmFindByDate
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
            this.radioSpecific = new System.Windows.Forms.RadioButton();
            this.radioRange = new System.Windows.Forms.RadioButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.lblAnd = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupAndOr = new System.Windows.Forms.GroupBox();
            this.radioAnd = new System.Windows.Forms.RadioButton();
            this.radioOr = new System.Windows.Forms.RadioButton();
            this.groupAndOr.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioSpecific
            // 
            this.radioSpecific.AutoSize = true;
            this.radioSpecific.Checked = true;
            this.radioSpecific.Location = new System.Drawing.Point(51, 82);
            this.radioSpecific.Name = "radioSpecific";
            this.radioSpecific.Size = new System.Drawing.Size(89, 17);
            this.radioSpecific.TabIndex = 0;
            this.radioSpecific.TabStop = true;
            this.radioSpecific.Text = "Specific Date";
            this.radioSpecific.UseVisualStyleBackColor = true;
            // 
            // radioRange
            // 
            this.radioRange.AutoSize = true;
            this.radioRange.Location = new System.Drawing.Point(154, 82);
            this.radioRange.Name = "radioRange";
            this.radioRange.Size = new System.Drawing.Size(83, 17);
            this.radioRange.TabIndex = 1;
            this.radioRange.Text = "Date Range";
            this.radioRange.UseVisualStyleBackColor = true;
            this.radioRange.CheckedChanged += new System.EventHandler(this.radioRange_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(61, 118);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // lblAnd
            // 
            this.lblAnd.AutoSize = true;
            this.lblAnd.Location = new System.Drawing.Point(133, 154);
            this.lblAnd.Name = "lblAnd";
            this.lblAnd.Size = new System.Drawing.Size(25, 13);
            this.lblAnd.TabIndex = 3;
            this.lblAnd.Text = "and";
            this.lblAnd.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(61, 183);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 4;
            this.dateTimePicker2.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(128, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(209, 230);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupAndOr
            // 
            this.groupAndOr.Controls.Add(this.radioAnd);
            this.groupAndOr.Controls.Add(this.radioOr);
            this.groupAndOr.Location = new System.Drawing.Point(88, 12);
            this.groupAndOr.Name = "groupAndOr";
            this.groupAndOr.Size = new System.Drawing.Size(108, 43);
            this.groupAndOr.TabIndex = 10;
            this.groupAndOr.TabStop = false;
            this.groupAndOr.Text = "Where";
            // 
            // radioAnd
            // 
            this.radioAnd.AutoSize = true;
            this.radioAnd.Location = new System.Drawing.Point(4, 19);
            this.radioAnd.Name = "radioAnd";
            this.radioAnd.Size = new System.Drawing.Size(44, 17);
            this.radioAnd.TabIndex = 1;
            this.radioAnd.TabStop = true;
            this.radioAnd.Text = "And";
            this.radioAnd.UseVisualStyleBackColor = true;
            // 
            // radioOr
            // 
            this.radioOr.AutoSize = true;
            this.radioOr.Location = new System.Drawing.Point(66, 19);
            this.radioOr.Name = "radioOr";
            this.radioOr.Size = new System.Drawing.Size(36, 17);
            this.radioOr.TabIndex = 0;
            this.radioOr.TabStop = true;
            this.radioOr.Text = "Or";
            this.radioOr.UseVisualStyleBackColor = true;
            // 
            // frmFindByDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 258);
            this.Controls.Add(this.groupAndOr);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.lblAnd);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.radioRange);
            this.Controls.Add(this.radioSpecific);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFindByDate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Creation Date Filter";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmFindByDate_Load);
            this.groupAndOr.ResumeLayout(false);
            this.groupAndOr.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAnd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.DateTimePicker dateTimePicker1;
        public System.Windows.Forms.DateTimePicker dateTimePicker2;
        public System.Windows.Forms.RadioButton radioSpecific;
        public System.Windows.Forms.RadioButton radioRange;
        private System.Windows.Forms.GroupBox groupAndOr;
        public System.Windows.Forms.RadioButton radioAnd;
        public System.Windows.Forms.RadioButton radioOr;
    }
}