namespace SAHL.X2InstanceManager.Forms
{
    partial class frmCreateNewInstance
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
            this.btnCreate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCreate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyVal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtKeyName = new System.Windows.Forms.TextBox();
            this.txtIID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(171, 97);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 0;
            this.btnCreate.Text = "Create Case";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Create Activity";
            // 
            // txtCreate
            // 
            this.txtCreate.Location = new System.Drawing.Point(87, 30);
            this.txtCreate.Name = "txtCreate";
            this.txtCreate.Size = new System.Drawing.Size(159, 20);
            this.txtCreate.TabIndex = 19;
            this.txtCreate.Text = "Create Instance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Create User";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(87, 9);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(159, 20);
            this.txtUser.TabIndex = 21;
            this.txtUser.Text = "sahl\\bcuser";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Key Value";
            // 
            // txtKeyVal
            // 
            this.txtKeyVal.Location = new System.Drawing.Point(87, 71);
            this.txtKeyVal.Name = "txtKeyVal";
            this.txtKeyVal.Size = new System.Drawing.Size(159, 20);
            this.txtKeyVal.TabIndex = 23;
            this.txtKeyVal.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Map Key Name";
            // 
            // txtKeyName
            // 
            this.txtKeyName.Location = new System.Drawing.Point(87, 51);
            this.txtKeyName.Name = "txtKeyName";
            this.txtKeyName.Size = new System.Drawing.Size(159, 20);
            this.txtKeyName.TabIndex = 25;
            this.txtKeyName.Text = "ApplicationKey";
            // 
            // txtIID
            // 
            this.txtIID.Location = new System.Drawing.Point(87, 126);
            this.txtIID.Name = "txtIID";
            this.txtIID.Size = new System.Drawing.Size(159, 20);
            this.txtIID.TabIndex = 27;
            this.txtIID.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "InstanceID";
            // 
            // frmCreateNewInstance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 220);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtIID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKeyName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtKeyVal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCreate);
            this.Controls.Add(this.btnCreate);
            this.Name = "frmCreateNewInstance";
            this.Text = "frmCreateNewInstance";
            this.Load += new System.EventHandler(this.frmCreateNewInstance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtKeyName;
        private System.Windows.Forms.TextBox txtIID;
        private System.Windows.Forms.Label label5;
    }
}