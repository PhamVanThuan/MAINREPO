namespace SAHL.X2Designer.Forms
{
    partial class frmRetrieve
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
            this.dlgOpenMap = new System.Windows.Forms.OpenFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxEnvironment = new System.Windows.Forms.ComboBox();
            this.dlgBackupFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlgOpenMap
            // 
            this.dlgOpenMap.DefaultExt = "x2p";
            this.dlgOpenMap.Multiselect = true;
            this.dlgOpenMap.Title = "Browse for map(s)";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(203, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 80;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnRetrieve.Location = new System.Drawing.Point(118, 71);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(75, 23);
            this.btnRetrieve.TabIndex = 60;
            this.btnRetrieve.Text = "Retrieve";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbxEnvironment);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 53);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "X2 Environment";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Retrieve From";
            // 
            // cbxEnvironment
            // 
            this.cbxEnvironment.FormattingEnabled = true;
            this.cbxEnvironment.Location = new System.Drawing.Point(92, 22);
            this.cbxEnvironment.Name = "cbxEnvironment";
            this.cbxEnvironment.Size = new System.Drawing.Size(129, 21);
            this.cbxEnvironment.TabIndex = 2;
            // 
            // frmRetrieve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 104);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRetrieve";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Map Retrieval";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmRetrieve_FormClosing);
            this.Load += new System.EventHandler(this.frmRetrieve_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog dlgOpenMap;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxEnvironment;
        private System.Windows.Forms.FolderBrowserDialog dlgBackupFolder;
    }
}