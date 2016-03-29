namespace SAHL.X2Designer.Forms
{
    partial class frmSelectConnStr
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
            this.li = new System.Windows.Forms.ListBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // li
            // 
            this.li.FormattingEnabled = true;
            this.li.Location = new System.Drawing.Point(12, 12);
            this.li.Name = "li";
            this.li.Size = new System.Drawing.Size(120, 147);
            this.li.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(138, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Select";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(138, 41);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(75, 23);
            this.btnNone.TabIndex = 2;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // frmSelectConnStr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 177);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.li);
            this.Name = "frmSelectConnStr";
            this.Text = "Select Server";
            this.Load += new System.EventHandler(this.frmSelectConnStr_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox li;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNone;
    }
}