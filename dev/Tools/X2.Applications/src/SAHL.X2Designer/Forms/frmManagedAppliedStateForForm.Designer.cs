namespace SAHL.X2Designer.Forms
{
    partial class frmManagedAppliedStateForForm
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
            this.chb = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // chb
            // 
            this.chb.FormattingEnabled = true;
            this.chb.Location = new System.Drawing.Point(12, 12);
            this.chb.Name = "chb";
            this.chb.Size = new System.Drawing.Size(387, 364);
            this.chb.TabIndex = 1;
            this.chb.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chb_ItemCheck);
            // 
            // frmManagedAppliedStateForForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 384);
            this.Controls.Add(this.chb);
            this.Name = "frmManagedAppliedStateForForm";
            this.Text = "Managed Applied States For Forms";
            this.Load += new System.EventHandler(this.frmManagedAppliedStateForForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chb;
    }
}