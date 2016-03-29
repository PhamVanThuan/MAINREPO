namespace SAHL.X2Designer.Forms
{
    partial class frmMapVariables
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblWorkFlowVariables = new System.Windows.Forms.Label();
            this.listViewMap = new System.Windows.Forms.ListView();
            this.btnMap = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.cmdAddVar = new System.Windows.Forms.Button();
            this.cmdDeleteVar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdUndoAdd = new System.Windows.Forms.Button();
            this.cmdUndoDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmdUndoMap = new System.Windows.Forms.Button();
            this.listViewWorkFlowVar = new System.Windows.Forms.ListView();
            this.listViewDatabaseVar = new System.Windows.Forms.ListView();
            this.listViewDeleteVar = new System.Windows.Forms.ListView();
            this.listViewAddVar = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(369, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Deleted Variables";
            // 
            // lblWorkFlowVariables
            // 
            this.lblWorkFlowVariables.AutoSize = true;
            this.lblWorkFlowVariables.Location = new System.Drawing.Point(107, 8);
            this.lblWorkFlowVariables.Name = "lblWorkFlowVariables";
            this.lblWorkFlowVariables.Size = new System.Drawing.Size(75, 13);
            this.lblWorkFlowVariables.TabIndex = 3;
            this.lblWorkFlowVariables.Text = "New Variables";
            // 
            // listViewMap
            // 
            this.listViewMap.FullRowSelect = true;
            this.listViewMap.Location = new System.Drawing.Point(10, 168);
            this.listViewMap.MultiSelect = false;
            this.listViewMap.Name = "listViewMap";
            this.listViewMap.Size = new System.Drawing.Size(557, 119);
            this.listViewMap.TabIndex = 5;
            this.listViewMap.UseCompatibleStateImageBehavior = false;
            this.listViewMap.View = System.Windows.Forms.View.Details;
            this.listViewMap.SelectedIndexChanged += new System.EventHandler(this.listViewMap_SelectedIndexChanged);
            // 
            // btnMap
            // 
            this.btnMap.Location = new System.Drawing.Point(256, 45);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(63, 23);
            this.btnMap.TabIndex = 2;
            this.btnMap.Text = "Map";
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(95, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Maps From";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(408, 477);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cmdAddVar
            // 
            this.cmdAddVar.Location = new System.Drawing.Point(91, 120);
            this.cmdAddVar.Name = "cmdAddVar";
            this.cmdAddVar.Size = new System.Drawing.Size(92, 24);
            this.cmdAddVar.TabIndex = 1;
            this.cmdAddVar.Text = "Confirm Add";
            this.cmdAddVar.UseVisualStyleBackColor = true;
            this.cmdAddVar.Click += new System.EventHandler(this.cmdAddVar_Click);
            // 
            // cmdDeleteVar
            // 
            this.cmdDeleteVar.Location = new System.Drawing.Point(390, 120);
            this.cmdDeleteVar.Name = "cmdDeleteVar";
            this.cmdDeleteVar.Size = new System.Drawing.Size(92, 24);
            this.cmdDeleteVar.TabIndex = 4;
            this.cmdDeleteVar.Text = "Confirm Delete";
            this.cmdDeleteVar.UseVisualStyleBackColor = true;
            this.cmdDeleteVar.Click += new System.EventHandler(this.cmdDeleteVar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 323);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Variables to be Added";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(369, 323);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Variables to be Deleted";
            // 
            // cmdUndoAdd
            // 
            this.cmdUndoAdd.Location = new System.Drawing.Point(104, 435);
            this.cmdUndoAdd.Name = "cmdUndoAdd";
            this.cmdUndoAdd.Size = new System.Drawing.Size(63, 23);
            this.cmdUndoAdd.TabIndex = 8;
            this.cmdUndoAdd.Text = "Undo";
            this.cmdUndoAdd.UseVisualStyleBackColor = true;
            this.cmdUndoAdd.Click += new System.EventHandler(this.cmdUndoAdd_Click);
            // 
            // cmdUndoDelete
            // 
            this.cmdUndoDelete.Location = new System.Drawing.Point(396, 435);
            this.cmdUndoDelete.Name = "cmdUndoDelete";
            this.cmdUndoDelete.Size = new System.Drawing.Size(63, 23);
            this.cmdUndoDelete.TabIndex = 10;
            this.cmdUndoDelete.Text = "Undo";
            this.cmdUndoDelete.UseVisualStyleBackColor = true;
            this.cmdUndoDelete.Click += new System.EventHandler(this.cmdUndoDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(491, 477);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmdUndoMap
            // 
            this.cmdUndoMap.Location = new System.Drawing.Point(256, 293);
            this.cmdUndoMap.Name = "cmdUndoMap";
            this.cmdUndoMap.Size = new System.Drawing.Size(63, 23);
            this.cmdUndoMap.TabIndex = 6;
            this.cmdUndoMap.Text = "Undo";
            this.cmdUndoMap.UseVisualStyleBackColor = true;
            this.cmdUndoMap.Click += new System.EventHandler(this.cmdUndoMap_Click);
            // 
            // listViewWorkFlowVar
            // 
            this.listViewWorkFlowVar.FullRowSelect = true;
            this.listViewWorkFlowVar.HideSelection = false;
            this.listViewWorkFlowVar.Location = new System.Drawing.Point(10, 24);
            this.listViewWorkFlowVar.MultiSelect = false;
            this.listViewWorkFlowVar.Name = "listViewWorkFlowVar";
            this.listViewWorkFlowVar.Size = new System.Drawing.Size(238, 90);
            this.listViewWorkFlowVar.TabIndex = 0;
            this.listViewWorkFlowVar.UseCompatibleStateImageBehavior = false;
            this.listViewWorkFlowVar.View = System.Windows.Forms.View.Details;
            this.listViewWorkFlowVar.SelectedIndexChanged += new System.EventHandler(this.listViewWorkFlowVar_SelectedIndexChanged);
            // 
            // listViewDatabaseVar
            // 
            this.listViewDatabaseVar.FullRowSelect = true;
            this.listViewDatabaseVar.HideSelection = false;
            this.listViewDatabaseVar.Location = new System.Drawing.Point(326, 24);
            this.listViewDatabaseVar.MultiSelect = false;
            this.listViewDatabaseVar.Name = "listViewDatabaseVar";
            this.listViewDatabaseVar.Size = new System.Drawing.Size(240, 90);
            this.listViewDatabaseVar.TabIndex = 3;
            this.listViewDatabaseVar.UseCompatibleStateImageBehavior = false;
            this.listViewDatabaseVar.View = System.Windows.Forms.View.Details;
            this.listViewDatabaseVar.SelectedIndexChanged += new System.EventHandler(this.listViewDatabaseVar_SelectedIndexChanged);
            // 
            // listViewDeleteVar
            // 
            this.listViewDeleteVar.FullRowSelect = true;
            this.listViewDeleteVar.Location = new System.Drawing.Point(326, 339);
            this.listViewDeleteVar.MultiSelect = false;
            this.listViewDeleteVar.Name = "listViewDeleteVar";
            this.listViewDeleteVar.Size = new System.Drawing.Size(240, 90);
            this.listViewDeleteVar.TabIndex = 9;
            this.listViewDeleteVar.UseCompatibleStateImageBehavior = false;
            this.listViewDeleteVar.View = System.Windows.Forms.View.Details;
            // 
            // listViewAddVar
            // 
            this.listViewAddVar.FullRowSelect = true;
            this.listViewAddVar.Location = new System.Drawing.Point(10, 339);
            this.listViewAddVar.MultiSelect = false;
            this.listViewAddVar.Name = "listViewAddVar";
            this.listViewAddVar.Size = new System.Drawing.Size(238, 90);
            this.listViewAddVar.TabIndex = 7;
            this.listViewAddVar.UseCompatibleStateImageBehavior = false;
            this.listViewAddVar.View = System.Windows.Forms.View.Details;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(251, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Maps To";
            // 
            // frmMapVariables
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(575, 502);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listViewDeleteVar);
            this.Controls.Add(this.listViewAddVar);
            this.Controls.Add(this.listViewDatabaseVar);
            this.Controls.Add(this.listViewWorkFlowVar);
            this.Controls.Add(this.cmdUndoMap);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmdUndoDelete);
            this.Controls.Add(this.cmdUndoAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdDeleteVar);
            this.Controls.Add(this.cmdAddVar);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnMap);
            this.Controls.Add(this.listViewMap);
            this.Controls.Add(this.lblWorkFlowVariables);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapVariables";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Variables";
            this.Load += new System.EventHandler(this.frmMapVariables_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWorkFlowVariables;
        private System.Windows.Forms.ListView listViewMap;
        private System.Windows.Forms.Button btnMap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button cmdAddVar;
        private System.Windows.Forms.Button cmdDeleteVar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdUndoAdd;
        private System.Windows.Forms.Button cmdUndoDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button cmdUndoMap;
        private System.Windows.Forms.ListView listViewWorkFlowVar;
        private System.Windows.Forms.ListView listViewDatabaseVar;
        private System.Windows.Forms.ListView listViewDeleteVar;
        private System.Windows.Forms.ListView listViewAddVar;
        private System.Windows.Forms.Label label5;
    }
}