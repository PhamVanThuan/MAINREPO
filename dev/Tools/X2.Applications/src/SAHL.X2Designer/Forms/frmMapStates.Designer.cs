namespace SAHL.X2Designer.Forms
{
    partial class frmMapStates
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
            this.listViewDatabaseStates = new System.Windows.Forms.ListView();
            this.listViewWorkFlowStates = new System.Windows.Forms.ListView();
            this.btnUndoMap = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.listViewMap = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkMigrateInstances = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listViewDatabaseStates
            // 
            this.listViewDatabaseStates.FullRowSelect = true;
            this.listViewDatabaseStates.HideSelection = false;
            this.listViewDatabaseStates.Location = new System.Drawing.Point(384, 27);
            this.listViewDatabaseStates.MultiSelect = false;
            this.listViewDatabaseStates.Name = "listViewDatabaseStates";
            this.listViewDatabaseStates.Size = new System.Drawing.Size(301, 143);
            this.listViewDatabaseStates.TabIndex = 1;
            this.listViewDatabaseStates.UseCompatibleStateImageBehavior = false;
            this.listViewDatabaseStates.View = System.Windows.Forms.View.Details;
            // 
            // listViewWorkFlowStates
            // 
            this.listViewWorkFlowStates.FullRowSelect = true;
            this.listViewWorkFlowStates.HideSelection = false;
            this.listViewWorkFlowStates.Location = new System.Drawing.Point(9, 27);
            this.listViewWorkFlowStates.MultiSelect = false;
            this.listViewWorkFlowStates.Name = "listViewWorkFlowStates";
            this.listViewWorkFlowStates.Size = new System.Drawing.Size(301, 143);
            this.listViewWorkFlowStates.TabIndex = 0;
            this.listViewWorkFlowStates.UseCompatibleStateImageBehavior = false;
            this.listViewWorkFlowStates.View = System.Windows.Forms.View.Details;
            // 
            // btnUndoMap
            // 
            this.btnUndoMap.Enabled = false;
            this.btnUndoMap.Location = new System.Drawing.Point(10, 321);
            this.btnUndoMap.Name = "btnUndoMap";
            this.btnUndoMap.Size = new System.Drawing.Size(63, 23);
            this.btnUndoMap.TabIndex = 7;
            this.btnUndoMap.Text = "Undo";
            this.btnUndoMap.UseVisualStyleBackColor = true;
            this.btnUndoMap.Click += new System.EventHandler(this.cmdUndoMap_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(610, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(529, 339);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(315, 27);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(63, 23);
            this.btnMap.TabIndex = 2;
            this.btnMap.Text = "Map";
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // listViewMap
            // 
            this.listViewMap.FullRowSelect = true;
            this.listViewMap.Location = new System.Drawing.Point(9, 197);
            this.listViewMap.MultiSelect = false;
            this.listViewMap.Name = "listViewMap";
            this.listViewMap.Size = new System.Drawing.Size(676, 119);
            this.listViewMap.TabIndex = 3;
            this.listViewMap.UseCompatibleStateImageBehavior = false;
            this.listViewMap.View = System.Windows.Forms.View.Details;
            this.listViewMap.SelectedIndexChanged += new System.EventHandler(this.listViewMap_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "New States:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(382, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Existing States:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 180);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Mapped States:";
            // 
            // chkMigrateInstances
            // 
            this.chkMigrateInstances.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMigrateInstances.AutoSize = true;
            this.chkMigrateInstances.Checked = true;
            this.chkMigrateInstances.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMigrateInstances.Location = new System.Drawing.Point(357, 343);
            this.chkMigrateInstances.Margin = new System.Windows.Forms.Padding(2);
            this.chkMigrateInstances.Name = "chkMigrateInstances";
            this.chkMigrateInstances.Size = new System.Drawing.Size(157, 17);
            this.chkMigrateInstances.TabIndex = 11;
            this.chkMigrateInstances.Text = "Migrate Exisiting Instances?";
            this.chkMigrateInstances.UseVisualStyleBackColor = true;
            // 
            // frmMapStates
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(694, 371);
            this.Controls.Add(this.chkMigrateInstances);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewDatabaseStates);
            this.Controls.Add(this.listViewWorkFlowStates);
            this.Controls.Add(this.btnUndoMap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnMap);
            this.Controls.Add(this.listViewMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapStates";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "States";
            this.Load += new System.EventHandler(this.frmMapStates_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewDatabaseStates;
        private System.Windows.Forms.ListView listViewWorkFlowStates;
        private System.Windows.Forms.Button btnUndoMap;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnMap;
        private System.Windows.Forms.ListView listViewMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox chkMigrateInstances;
    }
}