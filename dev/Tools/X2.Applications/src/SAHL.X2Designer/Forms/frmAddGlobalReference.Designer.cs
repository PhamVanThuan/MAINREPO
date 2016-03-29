namespace SAHL.X2Designer.Forms
{
    partial class frmAddGlobalReference
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
            this.listViewReferences = new System.Windows.Forms.ListView();
            this.btnAddGlobal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(477, 250);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // listViewReferences
            // 
            this.listViewReferences.FullRowSelect = true;
            this.listViewReferences.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewReferences.Location = new System.Drawing.Point(12, 12);
            this.listViewReferences.MultiSelect = false;
            this.listViewReferences.Name = "listViewReferences";
            this.listViewReferences.Size = new System.Drawing.Size(540, 232);
            this.listViewReferences.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewReferences.TabIndex = 9;
            this.listViewReferences.UseCompatibleStateImageBehavior = false;
            // 
            // btnAddGlobal
            // 
            this.btnAddGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGlobal.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddGlobal.Location = new System.Drawing.Point(393, 250);
            this.btnAddGlobal.Name = "btnAddGlobal";
            this.btnAddGlobal.Size = new System.Drawing.Size(75, 23);
            this.btnAddGlobal.TabIndex = 10;
            this.btnAddGlobal.Text = "Add";
            this.btnAddGlobal.UseVisualStyleBackColor = true;
            this.btnAddGlobal.Click += new System.EventHandler(this.btnAddGlobal_Click);
            // 
            // frmAddGlobalReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(564, 281);
            this.Controls.Add(this.listViewReferences);
            this.Controls.Add(this.btnAddGlobal);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddGlobalReference";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Global Reference";
            this.Load += new System.EventHandler(this.frmReferences_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView listViewReferences;
        private System.Windows.Forms.Button btnAddGlobal;

    }
}