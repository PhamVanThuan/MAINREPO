namespace SAHL.X2InstanceManager.Forms
{
    partial class frmFindInstance
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
            this.btnFind = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioCurrentWorkFlowOnly = new System.Windows.Forms.RadioButton();
            this.radioAllWorkflows = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.colWorkflow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCreationDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colStateID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInstanceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colADUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbxAvailableCriteria = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.listViewCriteria = new System.Windows.Forms.ListView();
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAndOr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEquals = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colExplicit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblAvailableCriteria = new System.Windows.Forms.Label();
            this.btnUnlockInstance = new System.Windows.Forms.Button();
            this.btnMoveTo = new System.Windows.Forms.Button();
            this.btnAboutInstance = new System.Windows.Forms.Button();
            this.btnRebuildInstance = new System.Windows.Forms.Button();
            this.btnEditInstance = new System.Windows.Forms.Button();
            this.btnReAssignInstance = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(843, 172);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(57, 23);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "&Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Look In :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioCurrentWorkFlowOnly);
            this.groupBox1.Controls.Add(this.radioAllWorkflows);
            this.groupBox1.Location = new System.Drawing.Point(14, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 49);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WorkFlows to search";
            // 
            // radioCurrentWorkFlowOnly
            // 
            this.radioCurrentWorkFlowOnly.AutoSize = true;
            this.radioCurrentWorkFlowOnly.Location = new System.Drawing.Point(119, 19);
            this.radioCurrentWorkFlowOnly.Name = "radioCurrentWorkFlowOnly";
            this.radioCurrentWorkFlowOnly.Size = new System.Drawing.Size(134, 17);
            this.radioCurrentWorkFlowOnly.TabIndex = 11;
            this.radioCurrentWorkFlowOnly.Text = "Current WorkFlow Only";
            this.radioCurrentWorkFlowOnly.UseVisualStyleBackColor = true;
            // 
            // radioAllWorkflows
            // 
            this.radioAllWorkflows.AutoSize = true;
            this.radioAllWorkflows.Checked = true;
            this.radioAllWorkflows.Location = new System.Drawing.Point(6, 19);
            this.radioAllWorkflows.Name = "radioAllWorkflows";
            this.radioAllWorkflows.Size = new System.Drawing.Size(92, 17);
            this.radioAllWorkflows.TabIndex = 10;
            this.radioAllWorkflows.TabStop = true;
            this.radioAllWorkflows.Text = "All WorkFlows";
            this.radioAllWorkflows.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(13, 75);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Match Case";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(302, 151);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Find";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteCustomSource.AddRange(new string[] {
            "All Objects",
            "Activities",
            "States",
            "Comments",
            "Code"});
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "All",
            "Activity Names",
            "Comments",
            "Code",
            "Properties",
            "State Names"});
            this.comboBox1.Location = new System.Drawing.Point(92, 39);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(204, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "All";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Find What :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(92, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(204, 20);
            this.textBox1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 355);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(912, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // listViewResults
            // 
            this.listViewResults.AllowColumnReorder = true;
            this.listViewResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWorkflow,
            this.colName,
            this.colState,
            this.colCreationDate,
            this.colStateID,
            this.colInstanceID,
            this.colADUser,
            this.colSubject});
            this.listViewResults.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(0, 201);
            this.listViewResults.MultiSelect = false;
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(912, 154);
            this.listViewResults.TabIndex = 14;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewResults_ColumnClick);
            this.listViewResults.SelectedIndexChanged += new System.EventHandler(this.listViewResults_SelectedIndexChanged);
            this.listViewResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewResults_MouseDoubleClick);
            // 
            // colWorkflow
            // 
            this.colWorkflow.Text = "Workflow";
            this.colWorkflow.Width = 120;
            // 
            // colName
            // 
            this.colName.DisplayIndex = 2;
            this.colName.Text = "Instance Name";
            this.colName.Width = 160;
            // 
            // colState
            // 
            this.colState.DisplayIndex = 4;
            this.colState.Text = "State Name";
            this.colState.Width = 120;
            // 
            // colCreationDate
            // 
            this.colCreationDate.DisplayIndex = 5;
            this.colCreationDate.Text = "Creation Date";
            this.colCreationDate.Width = 121;
            // 
            // colStateID
            // 
            this.colStateID.DisplayIndex = 6;
            this.colStateID.Text = "State ID";
            this.colStateID.Width = 0;
            // 
            // colInstanceID
            // 
            this.colInstanceID.DisplayIndex = 1;
            this.colInstanceID.Text = "Instance ID";
            this.colInstanceID.Width = 75;
            // 
            // colADUser
            // 
            this.colADUser.DisplayIndex = 7;
            this.colADUser.Text = "ADUsername (Creator)";
            this.colADUser.Width = 127;
            // 
            // colSubject
            // 
            this.colSubject.DisplayIndex = 3;
            this.colSubject.Text = "Subject";
            this.colSubject.Width = 170;
            // 
            // cbxAvailableCriteria
            // 
            this.cbxAvailableCriteria.FormattingEnabled = true;
            this.cbxAvailableCriteria.Items.AddRange(new object[] {
            "ADUsername (Creator)",
            "Creation Date",
            "Instance ID",
            "Instance Name",
            "State Name",
            "Subject",
            "Workflow Name"});
            this.cbxAvailableCriteria.Location = new System.Drawing.Point(145, 12);
            this.cbxAvailableCriteria.Name = "cbxAvailableCriteria";
            this.cbxAvailableCriteria.Size = new System.Drawing.Size(221, 21);
            this.cbxAvailableCriteria.Sorted = true;
            this.cbxAvailableCriteria.TabIndex = 15;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(372, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 23);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // listViewCriteria
            // 
            this.listViewCriteria.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDescription,
            this.colAndOr,
            this.colEquals,
            this.colExplicit,
            this.colValue});
            this.listViewCriteria.FullRowSelect = true;
            this.listViewCriteria.Location = new System.Drawing.Point(0, 41);
            this.listViewCriteria.Name = "listViewCriteria";
            this.listViewCriteria.Size = new System.Drawing.Size(837, 118);
            this.listViewCriteria.TabIndex = 17;
            this.listViewCriteria.UseCompatibleStateImageBehavior = false;
            this.listViewCriteria.View = System.Windows.Forms.View.Details;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 351;
            // 
            // colAndOr
            // 
            this.colAndOr.Text = "And/Or";
            this.colAndOr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colAndOr.Width = 65;
            // 
            // colEquals
            // 
            this.colEquals.Text = "Equal/Not Equal";
            this.colEquals.Width = 94;
            // 
            // colExplicit
            // 
            this.colExplicit.Text = "Explicit";
            this.colExplicit.Width = 52;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 250;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(843, 69);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(57, 23);
            this.btnEdit.TabIndex = 18;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(843, 110);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(57, 23);
            this.btnRemove.TabIndex = 19;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblAvailableCriteria
            // 
            this.lblAvailableCriteria.AutoSize = true;
            this.lblAvailableCriteria.Location = new System.Drawing.Point(17, 15);
            this.lblAvailableCriteria.Name = "lblAvailableCriteria";
            this.lblAvailableCriteria.Size = new System.Drawing.Size(122, 13);
            this.lblAvailableCriteria.TabIndex = 20;
            this.lblAvailableCriteria.Text = "Available Search Criteria";
            // 
            // btnUnlockInstance
            // 
            this.btnUnlockInstance.Location = new System.Drawing.Point(20, 174);
            this.btnUnlockInstance.Name = "btnUnlockInstance";
            this.btnUnlockInstance.Size = new System.Drawing.Size(88, 23);
            this.btnUnlockInstance.TabIndex = 21;
            this.btnUnlockInstance.Text = "&Unlock";
            this.btnUnlockInstance.UseVisualStyleBackColor = false;
            this.btnUnlockInstance.Click += new System.EventHandler(this.btnUnlockInstance_Click);
            // 
            // btnMoveTo
            // 
            this.btnMoveTo.Location = new System.Drawing.Point(113, 174);
            this.btnMoveTo.Name = "btnMoveTo";
            this.btnMoveTo.Size = new System.Drawing.Size(88, 23);
            this.btnMoveTo.TabIndex = 22;
            this.btnMoveTo.Text = "&Move To";
            this.btnMoveTo.UseVisualStyleBackColor = false;
            this.btnMoveTo.Click += new System.EventHandler(this.btnMoveTo_Click);
            // 
            // btnAboutInstance
            // 
            this.btnAboutInstance.Location = new System.Drawing.Point(207, 174);
            this.btnAboutInstance.Name = "btnAboutInstance";
            this.btnAboutInstance.Size = new System.Drawing.Size(88, 23);
            this.btnAboutInstance.TabIndex = 23;
            this.btnAboutInstance.Text = "&About";
            this.btnAboutInstance.UseVisualStyleBackColor = false;
            this.btnAboutInstance.Click += new System.EventHandler(this.btnAboutInstance_Click);
            // 
            // btnRebuildInstance
            // 
            this.btnRebuildInstance.Location = new System.Drawing.Point(301, 174);
            this.btnRebuildInstance.Name = "btnRebuildInstance";
            this.btnRebuildInstance.Size = new System.Drawing.Size(88, 23);
            this.btnRebuildInstance.TabIndex = 24;
            this.btnRebuildInstance.Text = "&Rebuild";
            this.btnRebuildInstance.UseVisualStyleBackColor = false;
            this.btnRebuildInstance.Click += new System.EventHandler(this.btnRebuildInstance_Click);
            // 
            // btnEditInstance
            // 
            this.btnEditInstance.Location = new System.Drawing.Point(394, 174);
            this.btnEditInstance.Name = "btnEditInstance";
            this.btnEditInstance.Size = new System.Drawing.Size(88, 23);
            this.btnEditInstance.TabIndex = 25;
            this.btnEditInstance.Text = "&Edit";
            this.btnEditInstance.UseVisualStyleBackColor = false;
            this.btnEditInstance.Click += new System.EventHandler(this.btnEditInstance_Click);
            // 
            // btnReAssignInstance
            // 
            this.btnReAssignInstance.Location = new System.Drawing.Point(488, 174);
            this.btnReAssignInstance.Name = "btnReAssignInstance";
            this.btnReAssignInstance.Size = new System.Drawing.Size(88, 23);
            this.btnReAssignInstance.TabIndex = 25;
            this.btnReAssignInstance.Text = "&Re-Assign";
            this.btnReAssignInstance.UseVisualStyleBackColor = false;
            this.btnReAssignInstance.Click += new System.EventHandler(this.btnReAssignInstance_Click);
            // 
            // frmFindInstance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 377);
            this.Controls.Add(this.btnReAssignInstance);
            this.Controls.Add(this.btnEditInstance);
            this.Controls.Add(this.btnRebuildInstance);
            this.Controls.Add(this.btnAboutInstance);
            this.Controls.Add(this.btnMoveTo);
            this.Controls.Add(this.btnUnlockInstance);
            this.Controls.Add(this.lblAvailableCriteria);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.listViewCriteria);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cbxAvailableCriteria);
            this.Controls.Add(this.listViewResults);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFindInstance";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            //this.TopMost = true;
            this.Load += new System.EventHandler(this.frmFindInstance_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioCurrentWorkFlowOnly;
        private System.Windows.Forms.RadioButton radioAllWorkflows;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colState;
        private System.Windows.Forms.ColumnHeader colCreationDate;
        private System.Windows.Forms.ColumnHeader colStateID;
        private System.Windows.Forms.ColumnHeader colInstanceID;
        public System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ColumnHeader colWorkflow;
        private System.Windows.Forms.ComboBox cbxAvailableCriteria;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colAndOr;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.ColumnHeader colADUser;
        private System.Windows.Forms.ColumnHeader colSubject;
        private System.Windows.Forms.Label lblAvailableCriteria;
        public System.Windows.Forms.ListView listViewCriteria;
        private System.Windows.Forms.ColumnHeader colEquals;
        private System.Windows.Forms.ColumnHeader colExplicit;
        public System.Windows.Forms.Button btnUnlockInstance;
        public System.Windows.Forms.Button btnMoveTo;
        public System.Windows.Forms.Button btnAboutInstance;
        public System.Windows.Forms.Button btnRebuildInstance;
        public System.Windows.Forms.Button btnEditInstance;
        public System.Windows.Forms.Button btnReAssignInstance;
    }
}