namespace SAHL.X2Designer.Forms
{
    partial class frmAddEditRoles
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtRole = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.checkDynamic = new System.Windows.Forms.CheckBox();
            this.lblDynamic = new System.Windows.Forms.Label();
            this.groupRoleType = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxSelectWorkFlow = new System.Windows.Forms.ComboBox();
            this.radioWorkFlow = new System.Windows.Forms.RadioButton();
            this.radioGlobal = new System.Windows.Forms.RadioButton();
            this.groupRoleType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(201, 286);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 15;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(311, 286);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(107, 204);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(303, 22);
            this.txtDescription.TabIndex = 12;
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(107, 174);
            this.txtRole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(303, 22);
            this.txtRole.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(7, 210);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(87, 17);
            this.lblDescription.TabIndex = 14;
            this.lblDescription.Text = "Description :";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(7, 177);
            this.lblRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(45, 17);
            this.lblRole.TabIndex = 13;
            this.lblRole.Text = "Role :";
            // 
            // checkDynamic
            // 
            this.checkDynamic.AutoSize = true;
            this.checkDynamic.Location = new System.Drawing.Point(107, 244);
            this.checkDynamic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkDynamic.Name = "checkDynamic";
            this.checkDynamic.Size = new System.Drawing.Size(18, 17);
            this.checkDynamic.TabIndex = 17;
            this.checkDynamic.UseVisualStyleBackColor = true;
            this.checkDynamic.CheckedChanged += new System.EventHandler(this.checkDynamic_CheckedChanged);
            this.checkDynamic.CheckStateChanged += new System.EventHandler(this.checkDynamic_CheckStateChanged);
            // 
            // lblDynamic
            // 
            this.lblDynamic.AutoSize = true;
            this.lblDynamic.Location = new System.Drawing.Point(7, 244);
            this.lblDynamic.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDynamic.Name = "lblDynamic";
            this.lblDynamic.Size = new System.Drawing.Size(70, 17);
            this.lblDynamic.TabIndex = 18;
            this.lblDynamic.Text = "Dynamic :";
            // 
            // groupRoleType
            // 
            this.groupRoleType.Controls.Add(this.label1);
            this.groupRoleType.Controls.Add(this.cbxSelectWorkFlow);
            this.groupRoleType.Controls.Add(this.radioWorkFlow);
            this.groupRoleType.Controls.Add(this.radioGlobal);
            this.groupRoleType.Location = new System.Drawing.Point(16, 10);
            this.groupRoleType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupRoleType.Name = "groupRoleType";
            this.groupRoleType.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupRoleType.Size = new System.Drawing.Size(394, 122);
            this.groupRoleType.TabIndex = 21;
            this.groupRoleType.TabStop = false;
            this.groupRoleType.Text = "Role Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Select WorkFlow :";
            // 
            // cbxSelectWorkFlow
            // 
            this.cbxSelectWorkFlow.FormattingEnabled = true;
            this.cbxSelectWorkFlow.Location = new System.Drawing.Point(145, 70);
            this.cbxSelectWorkFlow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxSelectWorkFlow.Name = "cbxSelectWorkFlow";
            this.cbxSelectWorkFlow.Size = new System.Drawing.Size(241, 24);
            this.cbxSelectWorkFlow.TabIndex = 22;
            // 
            // radioWorkFlow
            // 
            this.radioWorkFlow.AutoSize = true;
            this.radioWorkFlow.Location = new System.Drawing.Point(177, 23);
            this.radioWorkFlow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioWorkFlow.Name = "radioWorkFlow";
            this.radioWorkFlow.Size = new System.Drawing.Size(90, 21);
            this.radioWorkFlow.TabIndex = 22;
            this.radioWorkFlow.Text = "WorkFlow";
            this.radioWorkFlow.UseVisualStyleBackColor = true;
            this.radioWorkFlow.Click += new System.EventHandler(this.radioWorkFlow_Click);
            this.radioWorkFlow.CheckedChanged += new System.EventHandler(this.radioWorkFlow_CheckedChanged);
            // 
            // radioGlobal
            // 
            this.radioGlobal.AutoSize = true;
            this.radioGlobal.Checked = true;
            this.radioGlobal.Location = new System.Drawing.Point(80, 23);
            this.radioGlobal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioGlobal.Name = "radioGlobal";
            this.radioGlobal.Size = new System.Drawing.Size(70, 21);
            this.radioGlobal.TabIndex = 21;
            this.radioGlobal.TabStop = true;
            this.radioGlobal.Text = "Global";
            this.radioGlobal.UseVisualStyleBackColor = true;
            // 
            // frmAddEditRoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 329);
            this.Controls.Add(this.groupRoleType);
            this.Controls.Add(this.lblDynamic);
            this.Controls.Add(this.checkDynamic);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtRole);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblRole);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditRoles";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Roles";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddEditRoles_Load);
            this.groupRoleType.ResumeLayout(false);
            this.groupRoleType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TextBox txtDescription;
        public System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblDynamic;
        public System.Windows.Forms.CheckBox checkDynamic;
        private System.Windows.Forms.GroupBox groupRoleType;
        public System.Windows.Forms.RadioButton radioWorkFlow;
        public System.Windows.Forms.RadioButton radioGlobal;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbxSelectWorkFlow;
    }
}