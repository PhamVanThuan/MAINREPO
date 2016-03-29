namespace SAHL.X2InstanceManager.Forms
{
    partial class frmInstanceMigrationErrors
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
            this.listViewErrors = new System.Windows.Forms.ListView();
            this.colInstanceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblErrorMessageHeader = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewErrors
            // 
            this.listViewErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInstanceID});
            this.listViewErrors.Location = new System.Drawing.Point(92, 44);
            this.listViewErrors.Name = "listViewErrors";
            this.listViewErrors.Size = new System.Drawing.Size(366, 230);
            this.listViewErrors.TabIndex = 0;
            this.listViewErrors.UseCompatibleStateImageBehavior = false;
            this.listViewErrors.View = System.Windows.Forms.View.Details;
            // 
            // colInstanceID
            // 
            this.colInstanceID.Text = "InstanceID";
            this.colInstanceID.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(154, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "The following instances failed to migrate:";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(473, 491);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 34;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblErrorMessageHeader
            // 
            this.lblErrorMessageHeader.AutoSize = true;
            this.lblErrorMessageHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorMessageHeader.Location = new System.Drawing.Point(231, 277);
            this.lblErrorMessageHeader.Name = "lblErrorMessageHeader";
            this.lblErrorMessageHeader.Size = new System.Drawing.Size(88, 13);
            this.lblErrorMessageHeader.TabIndex = 35;
            this.lblErrorMessageHeader.Text = "Error Message";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrorMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblErrorMessage.Location = new System.Drawing.Point(89, 292);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(369, 165);
            this.lblErrorMessage.TabIndex = 36;
            this.lblErrorMessage.Text = "label3";
            // 
            // frmInstanceMigrationErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 516);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.lblErrorMessageHeader);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewErrors);
            this.MinimizeBox = false;
            this.Name = "frmInstanceMigrationErrors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instance Migration Errors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewErrors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ColumnHeader colInstanceID;
        private System.Windows.Forms.Label lblErrorMessageHeader;
        private System.Windows.Forms.Label lblErrorMessage;
    }
}