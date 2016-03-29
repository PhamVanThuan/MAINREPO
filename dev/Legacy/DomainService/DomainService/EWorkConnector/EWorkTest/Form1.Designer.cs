namespace EWorkTest
{
    partial class Form1
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
            this.btnCreateFolder = new System.Windows.Forms.Button();
            this.txtFID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAction1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAppKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAttKey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtReasonID = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateFolder
            // 
            this.btnCreateFolder.Location = new System.Drawing.Point(109, 28);
            this.btnCreateFolder.Name = "btnCreateFolder";
            this.btnCreateFolder.Size = new System.Drawing.Size(75, 23);
            this.btnCreateFolder.TabIndex = 0;
            this.btnCreateFolder.Text = "CreateFolder";
            this.btnCreateFolder.UseVisualStyleBackColor = true;
            this.btnCreateFolder.Click += new System.EventHandler(this.btnCreateFolder_Click);
            // 
            // txtFID
            // 
            this.txtFID.Location = new System.Drawing.Point(69, 57);
            this.txtFID.Name = "txtFID";
            this.txtFID.Size = new System.Drawing.Size(318, 20);
            this.txtFID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "FolderID";
            // 
            // btnAction1
            // 
            this.btnAction1.Location = new System.Drawing.Point(12, 83);
            this.btnAction1.Name = "btnAction1";
            this.btnAction1.Size = new System.Drawing.Size(93, 23);
            this.btnAction1.TabIndex = 3;
            this.btnAction1.Text = "OnAction";
            this.btnAction1.UseVisualStyleBackColor = true;
            this.btnAction1.Click += new System.EventHandler(this.btnAction1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "AppKey";
            // 
            // txtAppKey
            // 
            this.txtAppKey.Location = new System.Drawing.Point(287, 86);
            this.txtAppKey.Name = "txtAppKey";
            this.txtAppKey.Size = new System.Drawing.Size(100, 20);
            this.txtAppKey.TabIndex = 7;
            this.txtAppKey.Text = "691178";
            this.txtAppKey.TextChanged += new System.EventHandler(this.txtAppKey_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Assigned User";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(287, 141);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 9;
            this.txtUser.Text = "3071";
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Attorney Key";
            // 
            // txtAttKey
            // 
            this.txtAttKey.Location = new System.Drawing.Point(287, 115);
            this.txtAttKey.Name = "txtAttKey";
            this.txtAttKey.Size = new System.Drawing.Size(100, 20);
            this.txtAttKey.TabIndex = 11;
            this.txtAttKey.Text = "19";
            this.txtAttKey.TextChanged += new System.EventHandler(this.txtAttKey_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(190, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "MapName";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(252, 31);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(100, 20);
            this.txtMapName.TabIndex = 13;
            this.txtMapName.Text = "Pipeline";
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(109, 84);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(89, 20);
            this.txtAction.TabIndex = 15;
            this.txtAction.Text = "Action1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(202, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "ReasonID";
            // 
            // txtReasonID
            // 
            this.txtReasonID.Location = new System.Drawing.Point(287, 167);
            this.txtReasonID.Name = "txtReasonID";
            this.txtReasonID.Size = new System.Drawing.Size(100, 20);
            this.txtReasonID.TabIndex = 16;
            this.txtReasonID.Text = "19";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 28);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 18;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 195);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtReasonID);
            this.Controls.Add(this.txtAction);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMapName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtAttKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAppKey);
            this.Controls.Add(this.btnAction1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFID);
            this.Controls.Add(this.btnCreateFolder);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateFolder;
        private System.Windows.Forms.TextBox txtFID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAction1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAppKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAttKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReasonID;
        private System.Windows.Forms.Button btnLogin;
    }
}

