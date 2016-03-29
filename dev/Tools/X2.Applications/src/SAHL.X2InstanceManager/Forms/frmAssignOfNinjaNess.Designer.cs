namespace SAHL.X2InstanceManager.Forms
{
    partial class frmAssignOfNinjaNess
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReassign = new System.Windows.Forms.Button();
            this.liReassignTo = new System.Windows.Forms.ListBox();
            this.liDynamicRoles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(441, 35);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 19);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReassign
            // 
            this.btnReassign.Location = new System.Drawing.Point(441, 12);
            this.btnReassign.Margin = new System.Windows.Forms.Padding(2);
            this.btnReassign.Name = "btnReassign";
            this.btnReassign.Size = new System.Drawing.Size(104, 19);
            this.btnReassign.TabIndex = 19;
            this.btnReassign.Text = "Reassign To";
            this.btnReassign.UseVisualStyleBackColor = true;
            this.btnReassign.Click += new System.EventHandler(this.btnReassign_Click);
            // 
            // liReassignTo
            // 
            this.liReassignTo.FormattingEnabled = true;
            this.liReassignTo.Location = new System.Drawing.Point(229, 12);
            this.liReassignTo.Name = "liReassignTo";
            this.liReassignTo.Size = new System.Drawing.Size(207, 160);
            this.liReassignTo.TabIndex = 18;
            this.liReassignTo.Click += new System.EventHandler(this.liReassignTo_Click);
            // 
            // liDynamicRoles
            // 
            this.liDynamicRoles.FormattingEnabled = true;
            this.liDynamicRoles.Location = new System.Drawing.Point(12, 12);
            this.liDynamicRoles.Name = "liDynamicRoles";
            this.liDynamicRoles.Size = new System.Drawing.Size(207, 381);
            this.liDynamicRoles.TabIndex = 21;
            this.liDynamicRoles.Click += new System.EventHandler(this.liDynamicRoles_Click);
            // 
            // frmAssignOfNinjaNess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 412);
            this.Controls.Add(this.liDynamicRoles);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReassign);
            this.Controls.Add(this.liReassignTo);
            this.Name = "frmAssignOfNinjaNess";
            this.Text = "Reassign Form of Ninjaness";
            this.Load += new System.EventHandler(this.frmAssignOfNinjaNess_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReassign;
        private System.Windows.Forms.ListBox liReassignTo;
        private System.Windows.Forms.ListBox liDynamicRoles;
    }
}