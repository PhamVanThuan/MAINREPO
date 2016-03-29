namespace SAHL.X2Designer.Forms
{
    partial class frmAddUsingStatement
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
          this.lblExternalActivity = new System.Windows.Forms.Label();
          this.liUsing = new System.Windows.Forms.ListBox();
          this.SuspendLayout();
          // 
          // btnOk
          // 
          this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
          this.btnOk.Location = new System.Drawing.Point(190, 181);
          this.btnOk.Name = "btnOk";
          this.btnOk.Size = new System.Drawing.Size(75, 23);
          this.btnOk.TabIndex = 13;
          this.btnOk.Text = "OK";
          this.btnOk.UseVisualStyleBackColor = true;
          this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
          // 
          // btnCancel
          // 
          this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.btnCancel.Location = new System.Drawing.Point(272, 181);
          this.btnCancel.Name = "btnCancel";
          this.btnCancel.Size = new System.Drawing.Size(75, 23);
          this.btnCancel.TabIndex = 14;
          this.btnCancel.Text = "Cancel";
          this.btnCancel.UseVisualStyleBackColor = true;
          // 
          // lblExternalActivity
          // 
          this.lblExternalActivity.AutoSize = true;
          this.lblExternalActivity.Location = new System.Drawing.Point(5, 15);
          this.lblExternalActivity.Name = "lblExternalActivity";
          this.lblExternalActivity.Size = new System.Drawing.Size(61, 13);
          this.lblExternalActivity.TabIndex = 12;
          this.lblExternalActivity.Text = "Statement :";
          // 
          // liUsing
          // 
          this.liUsing.FormattingEnabled = true;
          this.liUsing.Location = new System.Drawing.Point(72, 12);
          this.liUsing.Name = "liUsing";
          this.liUsing.Size = new System.Drawing.Size(275, 160);
          this.liUsing.TabIndex = 15;
          // 
          // frmAddUsingStatement
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(356, 216);
          this.Controls.Add(this.liUsing);
          this.Controls.Add(this.btnOk);
          this.Controls.Add(this.btnCancel);
          this.Controls.Add(this.lblExternalActivity);
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "frmAddUsingStatement";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Add Using Statement";
          this.Load += new System.EventHandler(this.frmAddUsingStatement_Load);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblExternalActivity;
      private System.Windows.Forms.ListBox liUsing;
    }
}