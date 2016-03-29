namespace SAHL.X2Designer.Forms
{
    partial class frmInvalidBusinessStageTransitionWarning
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.WorkFlow = new System.Windows.Forms.ColumnHeader();
            this.colActivity = new System.Windows.Forms.ColumnHeader();
            this.colGroupDescription = new System.Windows.Forms.ColumnHeader();
            this.colDefinitionDescription = new System.Windows.Forms.ColumnHeader();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.WorkFlow,
            this.colActivity,
            this.colGroupDescription,
            this.colDefinitionDescription});
            this.listView1.Location = new System.Drawing.Point(12, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(524, 234);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "The Following Business Stage Transitions do not exist in the database:";
            // 
            // WorkFlow
            // 
            this.WorkFlow.Text = "WorkFlow";
            this.WorkFlow.Width = 120;
            // 
            // colActivity
            // 
            this.colActivity.Text = "Activity";
            this.colActivity.Width = 120;
            // 
            // colGroupDescription
            // 
            this.colGroupDescription.Text = "Group Description";
            this.colGroupDescription.Width = 140;
            // 
            // colDefinitionDescription
            // 
            this.colDefinitionDescription.Text = "Definition Description";
            this.colDefinitionDescription.Width = 140;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(458, 289);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(377, 289);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 33;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmInvalidBusinessStageTransitionWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 324);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvalidBusinessStageTransitionWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Business Stage Transition Warnings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader WorkFlow;
        private System.Windows.Forms.ColumnHeader colActivity;
        private System.Windows.Forms.ColumnHeader colGroupDescription;
        private System.Windows.Forms.ColumnHeader colDefinitionDescription;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
    }
}