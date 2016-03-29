namespace SAHL.X2Designer.Forms
{
    partial class frmMoveLink
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
            this.btnOk = new System.Windows.Forms.Button();
            this.listFromNode = new System.Windows.Forms.ListBox();
            this.listToNode = new System.Windows.Forms.ListBox();
            this.lblFromState = new System.Windows.Forms.Label();
            this.lblToState = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(270, 244);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // listFromNode
            // 
            this.listFromNode.FormattingEnabled = true;
            this.listFromNode.Location = new System.Drawing.Point(10, 22);
            this.listFromNode.Name = "listFromNode";
            this.listFromNode.Size = new System.Drawing.Size(202, 212);
            this.listFromNode.TabIndex = 6;
            // 
            // listToNode
            // 
            this.listToNode.FormattingEnabled = true;
            this.listToNode.Location = new System.Drawing.Point(225, 22);
            this.listToNode.Name = "listToNode";
            this.listToNode.Size = new System.Drawing.Size(202, 212);
            this.listToNode.TabIndex = 7;
            // 
            // lblFromState
            // 
            this.lblFromState.AutoSize = true;
            this.lblFromState.Location = new System.Drawing.Point(91, 6);
            this.lblFromState.Name = "lblFromState";
            this.lblFromState.Size = new System.Drawing.Size(58, 13);
            this.lblFromState.TabIndex = 8;
            this.lblFromState.Text = "From State";
            // 
            // lblToState
            // 
            this.lblToState.AutoSize = true;
            this.lblToState.Location = new System.Drawing.Point(311, 6);
            this.lblToState.Name = "lblToState";
            this.lblToState.Size = new System.Drawing.Size(48, 13);
            this.lblToState.TabIndex = 9;
            this.lblToState.Text = "To State";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(352, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // frmMoveLink
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 279);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblToState);
            this.Controls.Add(this.lblFromState);
            this.Controls.Add(this.listToNode);
            this.Controls.Add(this.listFromNode);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMoveLink";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Move Link";
            this.Load += new System.EventHandler(this.frmMoveLink_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListBox listFromNode;
        private System.Windows.Forms.ListBox listToNode;
        private System.Windows.Forms.Label lblFromState;
        private System.Windows.Forms.Label lblToState;
        private System.Windows.Forms.Button btnCancel;
    }
}