namespace SAHL.X2Designer.Forms
{
    partial class frmAddNuGetPackage
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
            this.txtNuGetPackageID = new System.Windows.Forms.TextBox();
            this.txtNuGetPackageVersion = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblNugetID = new System.Windows.Forms.Label();
            this.btnGetLatestVersion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(238, 75);
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
            this.btnCancel.Location = new System.Drawing.Point(320, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtNuGetPackageID
            // 
            this.txtNuGetPackageID.Location = new System.Drawing.Point(82, 6);
            this.txtNuGetPackageID.Name = "txtNuGetPackageID";
            this.txtNuGetPackageID.Size = new System.Drawing.Size(310, 20);
            this.txtNuGetPackageID.TabIndex = 15;
            // 
            // txtNuGetPackageVersion
            // 
            this.txtNuGetPackageVersion.Location = new System.Drawing.Point(82, 35);
            this.txtNuGetPackageVersion.Name = "txtNuGetPackageVersion";
            this.txtNuGetPackageVersion.Size = new System.Drawing.Size(104, 20);
            this.txtNuGetPackageVersion.TabIndex = 16;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(34, 38);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 17;
            this.lblVersion.Text = "Version";
            // 
            // lblNugetID
            // 
            this.lblNugetID.AutoSize = true;
            this.lblNugetID.Location = new System.Drawing.Point(12, 9);
            this.lblNugetID.Name = "lblNugetID";
            this.lblNugetID.Size = new System.Drawing.Size(64, 13);
            this.lblNugetID.TabIndex = 18;
            this.lblNugetID.Text = "Package ID";
            // 
            // btnGetLatestVersion
            // 
            this.btnGetLatestVersion.Location = new System.Drawing.Point(82, 61);
            this.btnGetLatestVersion.Name = "btnGetLatestVersion";
            this.btnGetLatestVersion.Size = new System.Drawing.Size(104, 23);
            this.btnGetLatestVersion.TabIndex = 19;
            this.btnGetLatestVersion.Text = "Get Latest Version";
            this.btnGetLatestVersion.UseVisualStyleBackColor = true;
            this.btnGetLatestVersion.Click += new System.EventHandler(this.btnGetLatestVersion_Click);
            // 
            // frmAddNuGetPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 110);
            this.Controls.Add(this.btnGetLatestVersion);
            this.Controls.Add(this.lblNugetID);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.txtNuGetPackageVersion);
            this.Controls.Add(this.txtNuGetPackageID);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddNuGetPackage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmAddNuGetPackage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtNuGetPackageID;
        private System.Windows.Forms.TextBox txtNuGetPackageVersion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblNugetID;
        private System.Windows.Forms.Button btnGetLatestVersion;
    }
}