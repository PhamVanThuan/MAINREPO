namespace SAHL.X2Designer.Forms
{
    partial class frmRetrievePublishedProcess
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
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.listViewProcess = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new System.Drawing.Point(464, 252);
            this.cmdOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(100, 28);
            this.cmdOk.TabIndex = 0;
            this.cmdOk.Text = "OK";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(572, 252);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 28);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // listViewProcess
            // 
            this.listViewProcess.FullRowSelect = true;
            this.listViewProcess.Location = new System.Drawing.Point(16, 15);
            this.listViewProcess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewProcess.MultiSelect = false;
            this.listViewProcess.Name = "listViewProcess";
            this.listViewProcess.Size = new System.Drawing.Size(655, 214);
            this.listViewProcess.TabIndex = 2;
            this.listViewProcess.UseCompatibleStateImageBehavior = false;
            this.listViewProcess.View = System.Windows.Forms.View.Details;
            // 
            // frmRetrievePublishedProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 292);
            this.ControlBox = false;
            this.Controls.Add(this.listViewProcess);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmRetrievePublishedProcess";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Retrieve Published Process";
            this.Load += new System.EventHandler(this.frmRetrievePublishedProcess_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        public System.Windows.Forms.ListView listViewProcess;
    }
}