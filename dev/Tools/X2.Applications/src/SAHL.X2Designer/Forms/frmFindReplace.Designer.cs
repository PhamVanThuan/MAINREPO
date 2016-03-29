namespace SAHL.X2Designer.Forms
{
    partial class frmFindReplace
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabFind = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioCurrentWorkFlowOnly = new System.Windows.Forms.RadioButton();
            this.radioAllWorkflows = new System.Windows.Forms.RadioButton();
            this.checkMatchCase = new System.Windows.Forms.CheckBox();
            this.cmdFind = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboLookIn = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFindWhat = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkConfirmReplacement = new System.Windows.Forms.CheckBox();
            this.checkReplaceMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.checkReplaceMatchCase = new System.Windows.Forms.CheckBox();
            this.cmdReplace = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtReplaceReplaceWithWhat = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReplaceFindWhat = new System.Windows.Forms.TextBox();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.colItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLocation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControlMain.SuspendLayout();
            this.tabFind.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabFind);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Location = new System.Drawing.Point(8, 4);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(391, 206);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMain.TabIndex = 8;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            this.tabControlMain.TabIndexChanged += new System.EventHandler(this.tabControlMain_TabIndexChanged);
            // 
            // tabFind
            // 
            this.tabFind.Controls.Add(this.groupBox1);
            this.tabFind.Controls.Add(this.checkMatchCase);
            this.tabFind.Controls.Add(this.cmdFind);
            this.tabFind.Controls.Add(this.label2);
            this.tabFind.Controls.Add(this.comboLookIn);
            this.tabFind.Controls.Add(this.label1);
            this.tabFind.Controls.Add(this.txtFindWhat);
            this.tabFind.Location = new System.Drawing.Point(4, 22);
            this.tabFind.Name = "tabFind";
            this.tabFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabFind.Size = new System.Drawing.Size(383, 180);
            this.tabFind.TabIndex = 0;
            this.tabFind.Text = "Find";
            this.tabFind.UseVisualStyleBackColor = true;
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
            this.radioCurrentWorkFlowOnly.CheckedChanged += new System.EventHandler(this.radioCurrentWorkFlowOnly_CheckedChanged);
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
            this.radioAllWorkflows.CheckedChanged += new System.EventHandler(this.radioAllWorkflows_CheckedChanged);
            // 
            // checkMatchCase
            // 
            this.checkMatchCase.AutoSize = true;
            this.checkMatchCase.Location = new System.Drawing.Point(13, 75);
            this.checkMatchCase.Name = "checkMatchCase";
            this.checkMatchCase.Size = new System.Drawing.Size(83, 17);
            this.checkMatchCase.TabIndex = 2;
            this.checkMatchCase.Text = "Match Case";
            this.checkMatchCase.UseVisualStyleBackColor = true;
            this.checkMatchCase.CheckedChanged += new System.EventHandler(this.checkMatchCase_CheckedChanged);
            // 
            // cmdFind
            // 
            this.cmdFind.Location = new System.Drawing.Point(302, 151);
            this.cmdFind.Name = "cmdFind";
            this.cmdFind.Size = new System.Drawing.Size(75, 23);
            this.cmdFind.TabIndex = 3;
            this.cmdFind.Text = "&Find";
            this.cmdFind.UseVisualStyleBackColor = true;
            this.cmdFind.Click += new System.EventHandler(this.cmdFind_Click);
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
            // comboLookIn
            // 
            this.comboLookIn.AutoCompleteCustomSource.AddRange(new string[] {
            "All Objects",
            "Activities",
            "States",
            "Comments",
            "Code"});
            this.comboLookIn.FormattingEnabled = true;
            this.comboLookIn.Items.AddRange(new object[] {
            "All",
            "Activity Names",
            "Comments",
            "Code",
            "Properties",
            "State Names"});
            this.comboLookIn.Location = new System.Drawing.Point(92, 39);
            this.comboLookIn.Name = "comboLookIn";
            this.comboLookIn.Size = new System.Drawing.Size(204, 21);
            this.comboLookIn.TabIndex = 1;
            this.comboLookIn.Text = "All";
            this.comboLookIn.SelectedIndexChanged += new System.EventHandler(this.comboLookIn_SelectedIndexChanged);
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
            // txtFindWhat
            // 
            this.txtFindWhat.Location = new System.Drawing.Point(92, 8);
            this.txtFindWhat.Name = "txtFindWhat";
            this.txtFindWhat.Size = new System.Drawing.Size(204, 20);
            this.txtFindWhat.TabIndex = 0;
            this.txtFindWhat.TextChanged += new System.EventHandler(this.txtFindWhat_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkConfirmReplacement);
            this.tabPage2.Controls.Add(this.checkReplaceMatchWholeWord);
            this.tabPage2.Controls.Add(this.checkReplaceMatchCase);
            this.tabPage2.Controls.Add(this.cmdReplace);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtReplaceReplaceWithWhat);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtReplaceFindWhat);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(383, 180);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Replace";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkConfirmReplacement
            // 
            this.checkConfirmReplacement.AutoSize = true;
            this.checkConfirmReplacement.Checked = true;
            this.checkConfirmReplacement.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkConfirmReplacement.Location = new System.Drawing.Point(13, 107);
            this.checkConfirmReplacement.Name = "checkConfirmReplacement";
            this.checkConfirmReplacement.Size = new System.Drawing.Size(127, 17);
            this.checkConfirmReplacement.TabIndex = 8;
            this.checkConfirmReplacement.Text = "Confirm Replacement";
            this.checkConfirmReplacement.UseVisualStyleBackColor = true;
            // 
            // checkReplaceMatchWholeWord
            // 
            this.checkReplaceMatchWholeWord.AutoSize = true;
            this.checkReplaceMatchWholeWord.Location = new System.Drawing.Point(13, 155);
            this.checkReplaceMatchWholeWord.Name = "checkReplaceMatchWholeWord";
            this.checkReplaceMatchWholeWord.Size = new System.Drawing.Size(119, 17);
            this.checkReplaceMatchWholeWord.TabIndex = 10;
            this.checkReplaceMatchWholeWord.Text = "Match Whole Word";
            this.checkReplaceMatchWholeWord.UseVisualStyleBackColor = true;
            this.checkReplaceMatchWholeWord.CheckedChanged += new System.EventHandler(this.checkReplaceMatchWholeWord_CheckedChanged);
            // 
            // checkReplaceMatchCase
            // 
            this.checkReplaceMatchCase.AutoSize = true;
            this.checkReplaceMatchCase.Location = new System.Drawing.Point(13, 130);
            this.checkReplaceMatchCase.Name = "checkReplaceMatchCase";
            this.checkReplaceMatchCase.Size = new System.Drawing.Size(83, 17);
            this.checkReplaceMatchCase.TabIndex = 9;
            this.checkReplaceMatchCase.Text = "Match Case";
            this.checkReplaceMatchCase.UseVisualStyleBackColor = true;
            this.checkReplaceMatchCase.CheckedChanged += new System.EventHandler(this.checkReplaceMatchCase_CheckedChanged);
            // 
            // cmdReplace
            // 
            this.cmdReplace.Location = new System.Drawing.Point(302, 151);
            this.cmdReplace.Name = "cmdReplace";
            this.cmdReplace.Size = new System.Drawing.Size(75, 23);
            this.cmdReplace.TabIndex = 7;
            this.cmdReplace.Text = "&Replace";
            this.cmdReplace.UseVisualStyleBackColor = true;
            this.cmdReplace.Click += new System.EventHandler(this.cmdReplace_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Replace with :";
            // 
            // txtReplaceReplaceWithWhat
            // 
            this.txtReplaceReplaceWithWhat.Location = new System.Drawing.Point(91, 39);
            this.txtReplaceReplaceWithWhat.Name = "txtReplaceReplaceWithWhat";
            this.txtReplaceReplaceWithWhat.Size = new System.Drawing.Size(204, 20);
            this.txtReplaceReplaceWithWhat.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(91, 70);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(69, 20);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "Code View";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Look In :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Find What :";
            // 
            // txtReplaceFindWhat
            // 
            this.txtReplaceFindWhat.Location = new System.Drawing.Point(91, 8);
            this.txtReplaceFindWhat.Name = "txtReplaceFindWhat";
            this.txtReplaceFindWhat.Size = new System.Drawing.Size(204, 20);
            this.txtReplaceFindWhat.TabIndex = 5;
            this.txtReplaceFindWhat.TextChanged += new System.EventHandler(this.txtReplaceFindWhat_TextChanged);
            // 
            // listViewResults
            // 
            this.listViewResults.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colItem,
            this.colType,
            this.colLocation});
            this.listViewResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewResults.HideSelection = false;
            this.listViewResults.Location = new System.Drawing.Point(8, 209);
            this.listViewResults.MultiSelect = false;
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(387, 81);
            this.listViewResults.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewResults.TabIndex = 11;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            this.listViewResults.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewResults_DrawItem);
            this.listViewResults.ItemActivate += new System.EventHandler(this.listViewResults_ItemActivate);
            // 
            // colItem
            // 
            this.colItem.Text = "Item";
            this.colItem.Width = 124;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 139;
            // 
            // colLocation
            // 
            this.colLocation.Text = "Position";
            this.colLocation.Width = 100;
            // 
            // frmFindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 292);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.listViewResults);
            this.Location = new System.Drawing.Point(500, 200);
            this.Name = "frmFindReplace";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find / Replace";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmFindReplace_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFindReplace_FormClosed);
            this.Enter += new System.EventHandler(this.frmFindReplace_Enter);
            this.tabControlMain.ResumeLayout(false);
            this.tabFind.ResumeLayout(false);
            this.tabFind.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabFind;
        private System.Windows.Forms.CheckBox checkMatchCase;
        private System.Windows.Forms.Button cmdFind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboLookIn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFindWhat;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button cmdReplace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtReplaceReplaceWithWhat;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReplaceFindWhat;
        private System.Windows.Forms.ColumnHeader colItem;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colLocation;
        private System.Windows.Forms.CheckBox checkReplaceMatchWholeWord;
        private System.Windows.Forms.CheckBox checkReplaceMatchCase;
        private System.Windows.Forms.CheckBox checkConfirmReplacement;
        public System.Windows.Forms.ListView listViewResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioCurrentWorkFlowOnly;
        private System.Windows.Forms.RadioButton radioAllWorkflows;

    }
}