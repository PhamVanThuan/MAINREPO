namespace SAHL.X2Designer.Forms
{
    partial class frmManageExternalActivities
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
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.listViewExternalActivities = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(328, 13);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdAdd.TabIndex = 2;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdEdit
            // 
            this.cmdEdit.Location = new System.Drawing.Point(328, 42);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(75, 23);
            this.cmdEdit.TabIndex = 3;
            this.cmdEdit.Text = "Edit";
            this.cmdEdit.UseVisualStyleBackColor = true;
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(328, 71);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(328, 295);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.cmdDone_Click);
            // 
            // listViewExternalActivities
            // 
            this.listViewExternalActivities.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewExternalActivities.FullRowSelect = true;
            this.listViewExternalActivities.Location = new System.Drawing.Point(12, 12);
            this.listViewExternalActivities.Name = "listViewExternalActivities";
            this.listViewExternalActivities.Size = new System.Drawing.Size(310, 274);
            this.listViewExternalActivities.TabIndex = 1;
            this.listViewExternalActivities.UseCompatibleStateImageBehavior = false;
            // 
            // frmManageExternalActivities
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 330);
            this.Controls.Add(this.listViewExternalActivities);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.cmdEdit);
            this.Controls.Add(this.cmdAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManageExternalActivities";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage External Activity Sources";
            this.Load += new System.EventHandler(this.frmManageExternalActivities_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListView listViewExternalActivities;
    }
}