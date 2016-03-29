namespace SAHL.X2Designer.Forms
{
    partial class frmManageReferences
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
            this.listViewReferences = new System.Windows.Forms.ListView();
            this.btnAddGlobal = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddFromFramework = new System.Windows.Forms.Button();
            this.btnAddFromExternalBinaries = new System.Windows.Forms.Button();
            this.btnAddFromDomainService = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewReferences
            // 
            this.listViewReferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewReferences.FullRowSelect = true;
            this.listViewReferences.Location = new System.Drawing.Point(12, 12);
            this.listViewReferences.Name = "listViewReferences";
            this.listViewReferences.Size = new System.Drawing.Size(746, 278);
            this.listViewReferences.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewReferences.TabIndex = 0;
            this.listViewReferences.UseCompatibleStateImageBehavior = false;
            this.listViewReferences.SelectedIndexChanged += new System.EventHandler(this.listViewReferences_SelectedIndexChanged);
            // 
            // btnAddGlobal
            // 
            this.btnAddGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGlobal.Location = new System.Drawing.Point(773, 34);
            this.btnAddGlobal.Name = "btnAddGlobal";
            this.btnAddGlobal.Size = new System.Drawing.Size(75, 36);
            this.btnAddGlobal.TabIndex = 6;
            this.btnAddGlobal.Text = "Global";
            this.btnAddGlobal.UseVisualStyleBackColor = true;
            this.btnAddGlobal.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(773, 306);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 29);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(773, 225);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 39);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove Reference";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddFromFramework
            // 
            this.btnAddFromFramework.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFromFramework.Location = new System.Drawing.Point(773, 76);
            this.btnAddFromFramework.Name = "btnAddFromFramework";
            this.btnAddFromFramework.Size = new System.Drawing.Size(75, 36);
            this.btnAddFromFramework.TabIndex = 10;
            this.btnAddFromFramework.Text = "SAHL Framework";
            this.btnAddFromFramework.UseVisualStyleBackColor = true;
            this.btnAddFromFramework.Click += new System.EventHandler(this.btnAddFromFramework_Click);
            // 
            // btnAddFromExternalBinaries
            // 
            this.btnAddFromExternalBinaries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFromExternalBinaries.Location = new System.Drawing.Point(773, 118);
            this.btnAddFromExternalBinaries.Name = "btnAddFromExternalBinaries";
            this.btnAddFromExternalBinaries.Size = new System.Drawing.Size(75, 36);
            this.btnAddFromExternalBinaries.TabIndex = 11;
            this.btnAddFromExternalBinaries.Text = "External Binaries";
            this.btnAddFromExternalBinaries.UseVisualStyleBackColor = true;
            this.btnAddFromExternalBinaries.Click += new System.EventHandler(this.btnAddFromExternalBinaries_Click);
            // 
            // btnAddFromDomainService
            // 
            this.btnAddFromDomainService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFromDomainService.Location = new System.Drawing.Point(773, 160);
            this.btnAddFromDomainService.Name = "btnAddFromDomainService";
            this.btnAddFromDomainService.Size = new System.Drawing.Size(75, 36);
            this.btnAddFromDomainService.TabIndex = 12;
            this.btnAddFromDomainService.Text = "Domain Service";
            this.btnAddFromDomainService.UseVisualStyleBackColor = true;
            this.btnAddFromDomainService.Click += new System.EventHandler(this.btnAddFromDomainService_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(765, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Add Reference";
            // 
            // frmManageReferences
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 339);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddFromDomainService);
            this.Controls.Add(this.btnAddFromExternalBinaries);
            this.Controls.Add(this.btnAddFromFramework);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddGlobal);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.listViewReferences);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManageReferences";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage References";
            this.Load += new System.EventHandler(this.frmManageReferences_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewReferences;
        private System.Windows.Forms.Button btnAddGlobal;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAddFromFramework;
        private System.Windows.Forms.Button btnAddFromExternalBinaries;
        private System.Windows.Forms.Button btnAddFromDomainService;
        private System.Windows.Forms.Label label1;
    }
}