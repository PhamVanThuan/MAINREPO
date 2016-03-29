namespace SAHL.X2Designer.Views
{
    partial class CodeErrors
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeErrors));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonErrors = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWarnings = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMsgs = new System.Windows.Forms.ToolStripButton();
            this.listViewErrors = new System.Windows.Forms.ListView();
            this.ErrorImage = new System.Windows.Forms.ColumnHeader();
            this.ErrorCode = new System.Windows.Forms.ColumnHeader();
            this.ErrorSource = new System.Windows.Forms.ColumnHeader();
            this.ErrorDescription = new System.Windows.Forms.ColumnHeader();
            this.ErrorLine = new System.Windows.Forms.ColumnHeader();
            this.ErrorColumn = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonErrors,
            this.toolStripButtonWarnings,
            this.toolStripButtonMsgs});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(755, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonErrors
            // 
            this.toolStripButtonErrors.Checked = true;
            this.toolStripButtonErrors.CheckOnClick = true;
            this.toolStripButtonErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonErrors.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonErrors.Image")));
            this.toolStripButtonErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonErrors.Name = "toolStripButtonErrors";
            this.toolStripButtonErrors.Size = new System.Drawing.Size(56, 22);
            this.toolStripButtonErrors.Text = "Errors";
            this.toolStripButtonErrors.ToolTipText = "Errors";
            this.toolStripButtonErrors.Click += new System.EventHandler(this.toolStripButtonErrors_Click);
            // 
            // toolStripButtonWarnings
            // 
            this.toolStripButtonWarnings.CheckOnClick = true;
            this.toolStripButtonWarnings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWarnings.Image")));
            this.toolStripButtonWarnings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWarnings.Name = "toolStripButtonWarnings";
            this.toolStripButtonWarnings.Size = new System.Drawing.Size(72, 22);
            this.toolStripButtonWarnings.Text = "Warnings";
            this.toolStripButtonWarnings.Click += new System.EventHandler(this.toolStripButtonWarnings_Click);
            // 
            // toolStripButtonMsgs
            // 
            this.toolStripButtonMsgs.CheckOnClick = true;
            this.toolStripButtonMsgs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMsgs.Image")));
            this.toolStripButtonMsgs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMsgs.Name = "toolStripButtonMsgs";
            this.toolStripButtonMsgs.Size = new System.Drawing.Size(74, 22);
            this.toolStripButtonMsgs.Text = "Messages";
            // 
            // listViewErrors
            // 
            this.listViewErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ErrorImage,
            this.ErrorCode,
            this.ErrorSource,
            this.ErrorDescription,
            this.ErrorLine,
            this.ErrorColumn});
            this.listViewErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewErrors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewErrors.FullRowSelect = true;
            this.listViewErrors.GridLines = true;
            this.listViewErrors.Location = new System.Drawing.Point(0, 25);
            this.listViewErrors.MultiSelect = false;
            this.listViewErrors.Name = "listViewErrors";
            this.listViewErrors.Size = new System.Drawing.Size(755, 191);
            this.listViewErrors.TabIndex = 1;
            this.listViewErrors.UseCompatibleStateImageBehavior = false;
            this.listViewErrors.View = System.Windows.Forms.View.Details;
            this.listViewErrors.DoubleClick += new System.EventHandler(this.listViewErrors_DoubleClick);
            // 
            // ErrorImage
            // 
            this.ErrorImage.Text = "";
            this.ErrorImage.Width = 25;
            // 
            // ErrorCode
            // 
            this.ErrorCode.Text = "";
            this.ErrorCode.Width = 25;
            // 
            // ErrorSource
            // 
            this.ErrorSource.Text = "Source";
            this.ErrorSource.Width = 150;
            // 
            // ErrorDescription
            // 
            this.ErrorDescription.Text = "Description";
            this.ErrorDescription.Width = 450;
            // 
            // ErrorLine
            // 
            this.ErrorLine.Text = "Error Line";
            this.ErrorLine.Width = 80;
            // 
            // ErrorColumn
            // 
            this.ErrorColumn.Text = "Error Column";
            this.ErrorColumn.Width = 80;
            // 
            // CodeErrors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 216);
            this.Controls.Add(this.listViewErrors);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeErrors";
            this.TabText = "Errors";
            this.Text = "Errors";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonErrors;
        private System.Windows.Forms.ToolStripButton toolStripButtonWarnings;
        private System.Windows.Forms.ToolStripButton toolStripButtonMsgs;
        private System.Windows.Forms.ColumnHeader ErrorCode;
        private System.Windows.Forms.ColumnHeader ErrorDescription;
        private System.Windows.Forms.ColumnHeader ErrorImage;
        private System.Windows.Forms.ColumnHeader ErrorLine;
        private System.Windows.Forms.ColumnHeader ErrorColumn;
        private System.Windows.Forms.ColumnHeader ErrorSource;
        public System.Windows.Forms.ListView listViewErrors;
    }
}