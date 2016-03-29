namespace SAHL.X2InstanceManager.Forms
{
    partial class frmReassign
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
            this.liReassignTo = new System.Windows.Forms.ListBox();
            this.btnReassign = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // liReassignTo
            // 
            this.liReassignTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liReassignTo.FormattingEnabled = true;
            this.liReassignTo.Location = new System.Drawing.Point(12, 12);
            this.liReassignTo.Name = "liReassignTo";
            this.liReassignTo.Size = new System.Drawing.Size(207, 160);
            this.liReassignTo.TabIndex = 14;
            // 
            // btnReassign
            // 
            this.btnReassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReassign.Location = new System.Drawing.Point(224, 12);
            this.btnReassign.Margin = new System.Windows.Forms.Padding(2);
            this.btnReassign.Name = "btnReassign";
            this.btnReassign.Size = new System.Drawing.Size(104, 19);
            this.btnReassign.TabIndex = 16;
            this.btnReassign.Text = "Reassign To";
            this.btnReassign.UseVisualStyleBackColor = true;
            this.btnReassign.Click += new System.EventHandler(this.btnReassign_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(224, 35);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 19);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmReassign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 205);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReassign);
            this.Controls.Add(this.liReassignTo);
            this.Name = "frmReassign";
            this.Text = "frmReassign";
            this.Load += new System.EventHandler(this.frmReassign_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox liReassignTo;
        private System.Windows.Forms.Button btnReassign;
        private System.Windows.Forms.Button btnCancel;
    }
}