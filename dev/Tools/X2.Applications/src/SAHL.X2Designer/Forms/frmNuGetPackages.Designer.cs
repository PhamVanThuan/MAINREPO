namespace SAHL.X2Designer.Forms
{
    partial class frmNuGetPackages
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
			this.btnOk = new System.Windows.Forms.Button();
			this.listViewNuGetPackages = new System.Windows.Forms.ListView();
			this.colPackageName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPackageVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(460, 262);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(380, 262);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 7;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// listViewNuGetPackages
			// 
			this.listViewNuGetPackages.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewNuGetPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPackageName,
            this.colPackageVersion});
			this.listViewNuGetPackages.FullRowSelect = true;
			this.listViewNuGetPackages.Location = new System.Drawing.Point(12, 9);
			this.listViewNuGetPackages.MultiSelect = false;
			this.listViewNuGetPackages.Name = "listViewNuGetPackages";
			this.listViewNuGetPackages.Size = new System.Drawing.Size(514, 234);
			this.listViewNuGetPackages.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listViewNuGetPackages.TabIndex = 12;
			this.listViewNuGetPackages.UseCompatibleStateImageBehavior = false;
			this.listViewNuGetPackages.View = System.Windows.Forms.View.Details;
			// 
			// colPackageName
			// 
			this.colPackageName.Text = "Package";
			this.colPackageName.Width = 367;
			// 
			// colPackageVersion
			// 
			this.colPackageVersion.Text = "Version";
			this.colPackageVersion.Width = 70;
			// 
			// frmManageNuGetPackages
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(538, 286);
			this.Controls.Add(this.listViewNuGetPackages);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNuGetPackages";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NuGet Packages";
			this.Load += new System.EventHandler(this.frmManageNuGetPackages_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListView listViewNuGetPackages;
        private System.Windows.Forms.ColumnHeader colPackageName;
		private System.Windows.Forms.ColumnHeader colPackageVersion;
    }
}