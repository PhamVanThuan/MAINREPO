namespace SAHL.X2InstanceManager.Forms
{
    partial class frmMove
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
            this.btnMove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // li
            // 
            this.li.FormattingEnabled = true;
            this.li.Location = new System.Drawing.Point(12, 12);
            this.li.Name = "li";
            this.li.Size = new System.Drawing.Size(309, 225);
            this.li.TabIndex = 0;
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(327, 12);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(75, 23);
            this.btnMove.TabIndex = 1;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(327, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 249);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.li);
            this.Name = "frmMove";
            this.Text = "frmMove";
            this.Load += new System.EventHandler(this.frmMove_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox li;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnCancel;
    }
}