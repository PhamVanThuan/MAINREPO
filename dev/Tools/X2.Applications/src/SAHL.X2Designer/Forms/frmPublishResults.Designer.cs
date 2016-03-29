namespace SAHL.X2Designer.Forms
{
    partial class frmPublishResults
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
            this.gvResults = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblPublisherMessage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblX2EngineServer = new System.Windows.Forms.Label();
            this.lblX2DatabaseServer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // gvResults
            // 
            this.gvResults.AllowUserToAddRows = false;
            this.gvResults.AllowUserToDeleteRows = false;
            this.gvResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvResults.Location = new System.Drawing.Point(12, 65);
            this.gvResults.MultiSelect = false;
            this.gvResults.Name = "gvResults";
            this.gvResults.RowHeadersVisible = false;
            this.gvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvResults.Size = new System.Drawing.Size(492, 190);
            this.gvResults.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(429, 262);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPublisherMessage
            // 
            this.lblPublisherMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPublisherMessage.Location = new System.Drawing.Point(12, 6);
            this.lblPublisherMessage.Name = "lblPublisherMessage";
            this.lblPublisherMessage.Size = new System.Drawing.Size(504, 18);
            this.lblPublisherMessage.TabIndex = 2;
            this.lblPublisherMessage.Text = "Processing of the maps is complete. Publisher results are summarised below.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enviroment :";
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(91, 38);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(29, 13);
            this.lblEnvironment.TabIndex = 4;
            this.lblEnvironment.Tag = "";
            this.lblEnvironment.Text = "DEV";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(189, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "X2 Server :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(374, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Database :";
            // 
            // lblX2EngineServer
            // 
            this.lblX2EngineServer.AutoSize = true;
            this.lblX2EngineServer.Location = new System.Drawing.Point(257, 38);
            this.lblX2EngineServer.Name = "lblX2EngineServer";
            this.lblX2EngineServer.Size = new System.Drawing.Size(67, 13);
            this.lblX2EngineServer.TabIndex = 7;
            this.lblX2EngineServer.Tag = "";
            this.lblX2EngineServer.Text = "SAHLS114a";
            // 
            // lblX2DatabaseServer
            // 
            this.lblX2DatabaseServer.AutoSize = true;
            this.lblX2DatabaseServer.Location = new System.Drawing.Point(441, 38);
            this.lblX2DatabaseServer.Name = "lblX2DatabaseServer";
            this.lblX2DatabaseServer.Size = new System.Drawing.Size(52, 13);
            this.lblX2DatabaseServer.TabIndex = 8;
            this.lblX2DatabaseServer.Tag = "";
            this.lblX2DatabaseServer.Text = "sahls103a";
            // 
            // frmPublishResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 294);
            this.Controls.Add(this.lblX2DatabaseServer);
            this.Controls.Add(this.lblX2EngineServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblEnvironment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPublisherMessage);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gvResults);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPublishResults";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Publisher Results";
            this.Load += new System.EventHandler(this.frmPublishResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gvResults;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblPublisherMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblX2EngineServer;
        private System.Windows.Forms.Label lblX2DatabaseServer;
    }
}