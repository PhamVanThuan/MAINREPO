namespace SAHL.X2Designer.Forms
{
    partial class frmManageUsingStatements
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
          this.btnDelete = new System.Windows.Forms.Button();
          this.btnAdd = new System.Windows.Forms.Button();
          this.listViewUsingStatements = new System.Windows.Forms.ListView();
          this.colUsing = new System.Windows.Forms.ColumnHeader();
          this.colCustom = new System.Windows.Forms.ColumnHeader();
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
          // btnDelete
          // 
          this.btnDelete.Location = new System.Drawing.Point(460, 38);
          this.btnDelete.Name = "btnDelete";
          this.btnDelete.Size = new System.Drawing.Size(75, 23);
          this.btnDelete.TabIndex = 10;
          this.btnDelete.Text = "Remove";
          this.btnDelete.UseVisualStyleBackColor = true;
          this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
          // 
          // btnAdd
          // 
          this.btnAdd.Location = new System.Drawing.Point(460, 9);
          this.btnAdd.Name = "btnAdd";
          this.btnAdd.Size = new System.Drawing.Size(75, 23);
          this.btnAdd.TabIndex = 9;
          this.btnAdd.Text = "Add";
          this.btnAdd.UseVisualStyleBackColor = true;
          this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
          // 
          // listViewUsingStatements
          // 
          this.listViewUsingStatements.Activation = System.Windows.Forms.ItemActivation.OneClick;
          this.listViewUsingStatements.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colUsing,
            this.colCustom});
          this.listViewUsingStatements.FullRowSelect = true;
          this.listViewUsingStatements.Location = new System.Drawing.Point(12, 9);
          this.listViewUsingStatements.Name = "listViewUsingStatements";
          this.listViewUsingStatements.Size = new System.Drawing.Size(442, 234);
          this.listViewUsingStatements.Sorting = System.Windows.Forms.SortOrder.Ascending;
          this.listViewUsingStatements.TabIndex = 12;
          this.listViewUsingStatements.UseCompatibleStateImageBehavior = false;
          this.listViewUsingStatements.View = System.Windows.Forms.View.Details;
          // 
          // colUsing
          // 
          this.colUsing.Text = "Using Clauses";
          this.colUsing.Width = 367;
          // 
          // colCustom
          // 
          this.colCustom.Text = "Custom";
          this.colCustom.Width = 70;
          // 
          // frmManageUsingStatements
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(538, 286);
          this.Controls.Add(this.listViewUsingStatements);
          this.Controls.Add(this.btnDelete);
          this.Controls.Add(this.btnAdd);
          this.Controls.Add(this.btnCancel);
          this.Controls.Add(this.btnOk);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "frmManageUsingStatements";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Manage Using Statements";
          this.Load += new System.EventHandler(this.frmManageUsingStatements_Load);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView listViewUsingStatements;
        private System.Windows.Forms.ColumnHeader colUsing;
        private System.Windows.Forms.ColumnHeader colCustom;
    }
}