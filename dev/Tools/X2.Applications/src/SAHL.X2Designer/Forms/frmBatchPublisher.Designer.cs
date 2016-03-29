namespace SAHL.X2Designer.Forms
{
    partial class frmBatchPublisher
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
            this.radDbSql = new System.Windows.Forms.RadioButton();
            this.radDbWindows = new System.Windows.Forms.RadioButton();
            this.cbxDbCatalog = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDbUser = new System.Windows.Forms.TextBox();
            this.cbxDbServer = new System.Windows.Forms.ComboBox();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dlgOpenMap = new System.Windows.Forms.OpenFileDialog();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnPublish = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxEngine = new System.Windows.Forms.ComboBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxX2Server = new System.Windows.Forms.ComboBox();
            this.radX2Windows = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.radX2Sql = new System.Windows.Forms.RadioButton();
            this.cbxX2Catalog = new System.Windows.Forms.ComboBox();
            this.txtX2Password = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtX2User = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupTimeouts = new System.Windows.Forms.GroupBox();
            this.txtCmdTimeout = new System.Windows.Forms.TextBox();
            this.txtConnectionTimeout = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblBackup = new System.Windows.Forms.Label();
            this.txtBackupFolder = new System.Windows.Forms.TextBox();
            this.chkBackupMaps = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dlgBackupFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.lblMapsToBePublished = new System.Windows.Forms.Label();
            this.chkCurrentMap = new System.Windows.Forms.CheckBox();
            this.lstMaps = new System.Windows.Forms.ListView();
            this.btnChangeVersion = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupTimeouts.SuspendLayout();
            this.SuspendLayout();
            // 
            // radDbSql
            // 
            this.radDbSql.AutoSize = true;
            this.radDbSql.Location = new System.Drawing.Point(9, 157);
            this.radDbSql.Name = "radDbSql";
            this.radDbSql.Size = new System.Drawing.Size(145, 17);
            this.radDbSql.TabIndex = 19;
            this.radDbSql.Text = "Sql Server Authentication";
            this.radDbSql.UseVisualStyleBackColor = true;
            // 
            // radDbWindows
            // 
            this.radDbWindows.AutoSize = true;
            this.radDbWindows.Location = new System.Drawing.Point(9, 137);
            this.radDbWindows.Name = "radDbWindows";
            this.radDbWindows.Size = new System.Drawing.Size(140, 17);
            this.radDbWindows.TabIndex = 18;
            this.radDbWindows.Text = "Windows Authentication";
            this.radDbWindows.UseVisualStyleBackColor = true;
            // 
            // cbxDbCatalog
            // 
            this.cbxDbCatalog.FormattingEnabled = true;
            this.cbxDbCatalog.Location = new System.Drawing.Point(92, 100);
            this.cbxDbCatalog.Name = "cbxDbCatalog";
            this.cbxDbCatalog.Size = new System.Drawing.Size(121, 21);
            this.cbxDbCatalog.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 103);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Default Catalog";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Username";
            // 
            // txtDbUser
            // 
            this.txtDbUser.Location = new System.Drawing.Point(92, 49);
            this.txtDbUser.Name = "txtDbUser";
            this.txtDbUser.Size = new System.Drawing.Size(121, 20);
            this.txtDbUser.TabIndex = 16;
            // 
            // cbxDbServer
            // 
            this.cbxDbServer.FormattingEnabled = true;
            this.cbxDbServer.Location = new System.Drawing.Point(92, 22);
            this.cbxDbServer.Name = "cbxDbServer";
            this.cbxDbServer.Size = new System.Drawing.Size(121, 21);
            this.cbxDbServer.TabIndex = 14;
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(92, 74);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.Size = new System.Drawing.Size(121, 20);
            this.txtDbPassword.TabIndex = 18;
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(173, 501);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 50;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dlgOpenMap
            // 
            this.dlgOpenMap.DefaultExt = "x2p";
            this.dlgOpenMap.Multiselect = true;
            this.dlgOpenMap.Title = "Browse for map(s)";
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(278, 501);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 52;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Password";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(482, 621);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 80;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnPublish
            // 
            this.btnPublish.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPublish.Location = new System.Drawing.Point(397, 621);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 60;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbxEngine);
            this.groupBox3.Controls.Add(this.txtPort);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(46, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(450, 53);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "X2 Engine";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server";
            // 
            // cbxEngine
            // 
            this.cbxEngine.FormattingEnabled = true;
            this.cbxEngine.Location = new System.Drawing.Point(92, 22);
            this.cbxEngine.Name = "cbxEngine";
            this.cbxEngine.Size = new System.Drawing.Size(121, 21);
            this.cbxEngine.TabIndex = 2;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(322, 22);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(121, 20);
            this.txtPort.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Server";
            // 
            // cbxX2Server
            // 
            this.cbxX2Server.FormattingEnabled = true;
            this.cbxX2Server.Location = new System.Drawing.Point(92, 22);
            this.cbxX2Server.Name = "cbxX2Server";
            this.cbxX2Server.Size = new System.Drawing.Size(121, 21);
            this.cbxX2Server.TabIndex = 6;
            // 
            // radX2Windows
            // 
            this.radX2Windows.AutoSize = true;
            this.radX2Windows.Location = new System.Drawing.Point(9, 137);
            this.radX2Windows.Name = "radX2Windows";
            this.radX2Windows.Size = new System.Drawing.Size(140, 17);
            this.radX2Windows.TabIndex = 4;
            this.radX2Windows.Text = "Windows Authentication";
            this.radX2Windows.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Username";
            // 
            // radX2Sql
            // 
            this.radX2Sql.AutoSize = true;
            this.radX2Sql.Location = new System.Drawing.Point(9, 157);
            this.radX2Sql.Name = "radX2Sql";
            this.radX2Sql.Size = new System.Drawing.Size(145, 17);
            this.radX2Sql.TabIndex = 17;
            this.radX2Sql.Text = "Sql Server Authentication";
            this.radX2Sql.UseVisualStyleBackColor = true;
            // 
            // cbxX2Catalog
            // 
            this.cbxX2Catalog.FormattingEnabled = true;
            this.cbxX2Catalog.Location = new System.Drawing.Point(92, 100);
            this.cbxX2Catalog.Name = "cbxX2Catalog";
            this.cbxX2Catalog.Size = new System.Drawing.Size(121, 21);
            this.cbxX2Catalog.TabIndex = 12;
            // 
            // txtX2Password
            // 
            this.txtX2Password.Location = new System.Drawing.Point(92, 74);
            this.txtX2Password.Name = "txtX2Password";
            this.txtX2Password.Size = new System.Drawing.Size(121, 20);
            this.txtX2Password.TabIndex = 10;
            this.txtX2Password.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtX2Password_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Default Catalog";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.radDbSql);
            this.groupBox2.Controls.Add(this.radDbWindows);
            this.groupBox2.Controls.Add(this.cbxDbCatalog);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtDbUser);
            this.groupBox2.Controls.Add(this.cbxDbServer);
            this.groupBox2.Controls.Add(this.txtDbPassword);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(276, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 185);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2AM Connection";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Server";
            // 
            // txtX2User
            // 
            this.txtX2User.Location = new System.Drawing.Point(92, 49);
            this.txtX2User.Name = "txtX2User";
            this.txtX2User.Size = new System.Drawing.Size(121, 20);
            this.txtX2User.TabIndex = 8;
            this.txtX2User.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtX2User_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Password";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radX2Sql);
            this.groupBox1.Controls.Add(this.cbxX2Catalog);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.radX2Windows);
            this.groupBox1.Controls.Add(this.txtX2User);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbxX2Server);
            this.groupBox1.Controls.Add(this.txtX2Password);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(46, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 185);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X2 Connection";
            // 
            // groupTimeouts
            // 
            this.groupTimeouts.Controls.Add(this.txtCmdTimeout);
            this.groupTimeouts.Controls.Add(this.txtConnectionTimeout);
            this.groupTimeouts.Controls.Add(this.label2);
            this.groupTimeouts.Controls.Add(this.label10);
            this.groupTimeouts.Location = new System.Drawing.Point(46, 263);
            this.groupTimeouts.Name = "groupTimeouts";
            this.groupTimeouts.Size = new System.Drawing.Size(450, 55);
            this.groupTimeouts.TabIndex = 39;
            this.groupTimeouts.TabStop = false;
            this.groupTimeouts.Text = "Timeouts";
            // 
            // txtCmdTimeout
            // 
            this.txtCmdTimeout.Location = new System.Drawing.Point(343, 23);
            this.txtCmdTimeout.Name = "txtCmdTimeout";
            this.txtCmdTimeout.Size = new System.Drawing.Size(68, 20);
            this.txtCmdTimeout.TabIndex = 32;
            this.txtCmdTimeout.Text = "90";
            // 
            // txtConnectionTimeout
            // 
            this.txtConnectionTimeout.Location = new System.Drawing.Point(140, 23);
            this.txtConnectionTimeout.Name = "txtConnectionTimeout";
            this.txtConnectionTimeout.Size = new System.Drawing.Size(68, 20);
            this.txtConnectionTimeout.TabIndex = 30;
            this.txtConnectionTimeout.Text = "90";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Command Timeout";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 44;
            this.label10.Text = "Connection Timeout";
            // 
            // lblBackup
            // 
            this.lblBackup.AutoSize = true;
            this.lblBackup.Location = new System.Drawing.Point(23, 570);
            this.lblBackup.Name = "lblBackup";
            this.lblBackup.Size = new System.Drawing.Size(76, 13);
            this.lblBackup.TabIndex = 40;
            this.lblBackup.Text = "Backup Folder";
            // 
            // txtBackupFolder
            // 
            this.txtBackupFolder.Location = new System.Drawing.Point(105, 567);
            this.txtBackupFolder.Name = "txtBackupFolder";
            this.txtBackupFolder.Size = new System.Drawing.Size(371, 20);
            this.txtBackupFolder.TabIndex = 41;
            // 
            // chkBackupMaps
            // 
            this.chkBackupMaps.AutoSize = true;
            this.chkBackupMaps.Location = new System.Drawing.Point(26, 544);
            this.chkBackupMaps.Name = "chkBackupMaps";
            this.chkBackupMaps.Size = new System.Drawing.Size(241, 17);
            this.chkBackupMaps.TabIndex = 42;
            this.chkBackupMaps.Text = "Automatically backup maps to specified folder";
            this.chkBackupMaps.UseVisualStyleBackColor = true;
            this.chkBackupMaps.CheckedChanged += new System.EventHandler(this.chkBackupMaps_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(482, 565);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(52, 23);
            this.btnBrowse.TabIndex = 43;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblMapsToBePublished
            // 
            this.lblMapsToBePublished.AutoSize = true;
            this.lblMapsToBePublished.Enabled = false;
            this.lblMapsToBePublished.Location = new System.Drawing.Point(23, 371);
            this.lblMapsToBePublished.Name = "lblMapsToBePublished";
            this.lblMapsToBePublished.Size = new System.Drawing.Size(108, 13);
            this.lblMapsToBePublished.TabIndex = 45;
            this.lblMapsToBePublished.Text = "Maps to be published";
            // 
            // chkCurrentMap
            // 
            this.chkCurrentMap.AutoSize = true;
            this.chkCurrentMap.Checked = true;
            this.chkCurrentMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCurrentMap.Location = new System.Drawing.Point(26, 341);
            this.chkCurrentMap.Name = "chkCurrentMap";
            this.chkCurrentMap.Size = new System.Drawing.Size(141, 17);
            this.chkCurrentMap.TabIndex = 40;
            this.chkCurrentMap.Text = "Publish current map only";
            this.chkCurrentMap.UseVisualStyleBackColor = true;
            this.chkCurrentMap.CheckedChanged += new System.EventHandler(this.chkCurrentMap_CheckedChanged);
            // 
            // lstMaps
            // 
            this.lstMaps.FullRowSelect = true;
            this.lstMaps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstMaps.Location = new System.Drawing.Point(26, 387);
            this.lstMaps.Name = "lstMaps";
            this.lstMaps.Size = new System.Drawing.Size(487, 108);
            this.lstMaps.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstMaps.TabIndex = 47;
            this.lstMaps.UseCompatibleStateImageBehavior = false;
            // 
            // btnChangeVersion
            // 
            this.btnChangeVersion.Enabled = false;
            this.btnChangeVersion.Location = new System.Drawing.Point(415, 501);
            this.btnChangeVersion.Name = "btnChangeVersion";
            this.btnChangeVersion.Size = new System.Drawing.Size(98, 23);
            this.btnChangeVersion.TabIndex = 55;
            this.btnChangeVersion.Text = "Change Version";
            this.btnChangeVersion.UseVisualStyleBackColor = true;
            this.btnChangeVersion.Click += new System.EventHandler(this.btnChangeVersion_Click);
            // 
            // frmBatchPublisher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 646);
            this.Controls.Add(this.btnChangeVersion);
            this.Controls.Add(this.lstMaps);
            this.Controls.Add(this.chkCurrentMap);
            this.Controls.Add(this.lblMapsToBePublished);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.chkBackupMaps);
            this.Controls.Add(this.lblBackup);
            this.Controls.Add(this.txtBackupFolder);
            this.Controls.Add(this.groupTimeouts);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchPublisher";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Batch Publishing";
            this.Load += new System.EventHandler(this.frmBatchPublisher_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBatchPublisher_FormClosing);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupTimeouts.ResumeLayout(false);
            this.groupTimeouts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radDbSql;
        private System.Windows.Forms.RadioButton radDbWindows;
        private System.Windows.Forms.ComboBox cbxDbCatalog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDbUser;
        private System.Windows.Forms.ComboBox cbxDbServer;
        private System.Windows.Forms.TextBox txtDbPassword;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog dlgOpenMap;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxEngine;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxX2Server;
        private System.Windows.Forms.RadioButton radX2Windows;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radX2Sql;
        private System.Windows.Forms.ComboBox cbxX2Catalog;
        private System.Windows.Forms.TextBox txtX2Password;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtX2User;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupTimeouts;
        private System.Windows.Forms.TextBox txtCmdTimeout;
        private System.Windows.Forms.TextBox txtConnectionTimeout;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblBackup;
        private System.Windows.Forms.TextBox txtBackupFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.FolderBrowserDialog dlgBackupFolder;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblMapsToBePublished;
        private System.Windows.Forms.CheckBox chkCurrentMap;
        private System.Windows.Forms.CheckBox chkBackupMaps;
        private System.Windows.Forms.ListView lstMaps;
        private System.Windows.Forms.Button btnChangeVersion;
    }
}