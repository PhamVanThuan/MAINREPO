namespace SAHL.X2InstanceManager.Forms
{
    partial class frmWorkflowEvents
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
            this.liEIDs = new System.Windows.Forms.ListBox();
            this.txtEXT = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // liEIDs
            // 
            this.liEIDs.FormattingEnabled = true;
            this.liEIDs.Location = new System.Drawing.Point(12, 12);
            this.liEIDs.Name = "liEIDs";
            this.liEIDs.Size = new System.Drawing.Size(189, 303);
            this.liEIDs.TabIndex = 0;
            this.liEIDs.SelectedIndexChanged += new System.EventHandler(this.liEIDs_SelectedIndexChanged);
            // 
            // txtEXT
            // 
            this.txtEXT.Location = new System.Drawing.Point(284, 13);
            this.txtEXT.Name = "txtEXT";
            this.txtEXT.ReadOnly = true;
            this.txtEXT.Size = new System.Drawing.Size(100, 20);
            this.txtEXT.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(208, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected EXT";
            // 
            // txtXML
            // 
            this.txtXML.Location = new System.Drawing.Point(207, 86);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(362, 69);
            this.txtXML.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ActivityXMLData example - ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(371, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "<FieldInputs><FieldInput Name = \"OfferKey\">123</FieldInput></FieldInputs>";
            // 
            // btnExt
            // 
            this.btnExt.Location = new System.Drawing.Point(208, 161);
            this.btnExt.Name = "btnExt";
            this.btnExt.Size = new System.Drawing.Size(173, 23);
            this.btnExt.TabIndex = 6;
            this.btnExt.Text = "Create External Activity";
            this.btnExt.UseVisualStyleBackColor = true;
            this.btnExt.Click += new System.EventHandler(this.btnExt_Click);
            // 
            // frmWorkflowEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 329);
            this.Controls.Add(this.btnExt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEXT);
            this.Controls.Add(this.liEIDs);
            this.Name = "frmWorkflowEvents";
            this.Text = "Fire Workflow Flag";
            this.Load += new System.EventHandler(this.frmWorkflowEvents_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox liEIDs;
        private System.Windows.Forms.TextBox txtEXT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExt;
    }
}