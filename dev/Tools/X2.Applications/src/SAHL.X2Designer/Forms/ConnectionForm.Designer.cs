namespace SAHL.X2Designer.Forms
{
    partial class ConnectionForm
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxCatalog = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radSQLAuth = new System.Windows.Forms.RadioButton();
            this.radWindowsAuth = new System.Windows.Forms.RadioButton();
            this.cbxServerName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefreshServers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(296, 216);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 32;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDone
            // 
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDone.Location = new System.Drawing.Point(215, 216);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 23);
            this.btnDone.TabIndex = 31;
            this.btnDone.Text = "OK";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            this.btnDone.Enter += new System.EventHandler(this.btnDone_Enter);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(10, 216);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 30;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Default Catalog:";
            // 
            // cbxCatalog
            // 
            this.cbxCatalog.DisplayMember = "name";
            this.cbxCatalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCatalog.FormattingEnabled = true;
            this.cbxCatalog.Location = new System.Drawing.Point(128, 178);
            this.cbxCatalog.Name = "cbxCatalog";
            this.cbxCatalog.Size = new System.Drawing.Size(240, 21);
            this.cbxCatalog.TabIndex = 29;
            this.cbxCatalog.Enter += new System.EventHandler(this.cbxCatalog_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(128, 152);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(240, 20);
            this.txtPassword.TabIndex = 28;
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(128, 125);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(240, 20);
            this.txtUserName.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "User Name:";
            // 
            // radSQLAuth
            // 
            this.radSQLAuth.AutoSize = true;
            this.radSQLAuth.Location = new System.Drawing.Point(82, 91);
            this.radSQLAuth.Name = "radSQLAuth";
            this.radSQLAuth.Size = new System.Drawing.Size(151, 17);
            this.radSQLAuth.TabIndex = 26;
            this.radSQLAuth.Text = "SQL Server Authentication";
            this.radSQLAuth.UseVisualStyleBackColor = true;
            this.radSQLAuth.CheckedChanged += new System.EventHandler(this.radSQLAuth_CheckedChanged);
            // 
            // radWindowsAuth
            // 
            this.radWindowsAuth.AutoSize = true;
            this.radWindowsAuth.Checked = true;
            this.radWindowsAuth.Location = new System.Drawing.Point(82, 68);
            this.radWindowsAuth.Name = "radWindowsAuth";
            this.radWindowsAuth.Size = new System.Drawing.Size(140, 17);
            this.radWindowsAuth.TabIndex = 25;
            this.radWindowsAuth.TabStop = true;
            this.radWindowsAuth.Text = "Windows Authentication";
            this.radWindowsAuth.UseVisualStyleBackColor = true;
            this.radWindowsAuth.CheckedChanged += new System.EventHandler(this.radWindowsAuth_CheckedChanged);
            // 
            // cbxServerName
            // 
            this.cbxServerName.Enabled = false;
            this.cbxServerName.FormattingEnabled = true;
            this.cbxServerName.Location = new System.Drawing.Point(128, 12);
            this.cbxServerName.Name = "cbxServerName";
            this.cbxServerName.Size = new System.Drawing.Size(240, 21);
            this.cbxServerName.Sorted = true;
            this.cbxServerName.TabIndex = 24;
            this.cbxServerName.Text = "Retrieving List...";
            this.cbxServerName.SelectedIndexChanged += new System.EventHandler(this.cbxServerName_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Server:";
            // 
            // btnRefreshServers
            // 
            this.btnRefreshServers.Location = new System.Drawing.Point(293, 39);
            this.btnRefreshServers.Name = "btnRefreshServers";
            this.btnRefreshServers.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshServers.TabIndex = 37;
            this.btnRefreshServers.Text = "Refresh";
            this.btnRefreshServers.UseVisualStyleBackColor = true;
            this.btnRefreshServers.Click += new System.EventHandler(this.btnRefreshServers_Click);
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(383, 244);
            this.Controls.Add(this.btnRefreshServers);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxCatalog);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.radSQLAuth);
            this.Controls.Add(this.radWindowsAuth);
            this.Controls.Add(this.cbxServerName);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConnectionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to X2 Database";
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radSQLAuth;
        private System.Windows.Forms.RadioButton radWindowsAuth;
        private System.Windows.Forms.ComboBox cbxServerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefreshServers;
        public System.Windows.Forms.ComboBox cbxCatalog;
    }
}