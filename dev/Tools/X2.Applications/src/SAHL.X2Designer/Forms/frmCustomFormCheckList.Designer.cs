namespace SAHL.X2Designer.Forms
{
    partial class frmCustomFormCheckList
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
            this.checkListForms = new System.Windows.Forms.CheckedListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lstAvailableForms = new System.Windows.Forms.ListBox();
            this.lsSelectedForms = new System.Windows.Forms.ListBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.btnDwn = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkListForms
            // 
            this.checkListForms.CheckOnClick = true;
            this.checkListForms.FormattingEnabled = true;
            this.checkListForms.Location = new System.Drawing.Point(1, 307);
            this.checkListForms.Name = "checkListForms";
            this.checkListForms.Size = new System.Drawing.Size(390, 259);
            this.checkListForms.TabIndex = 1;
            this.checkListForms.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(596, 259);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(513, 259);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.cmdDone_Click);
            // 
            // lstAvailableForms
            // 
            this.lstAvailableForms.FormattingEnabled = true;
            this.lstAvailableForms.Location = new System.Drawing.Point(12, 21);
            this.lstAvailableForms.Name = "lstAvailableForms";
            this.lstAvailableForms.Size = new System.Drawing.Size(243, 225);
            this.lstAvailableForms.Sorted = true;
            this.lstAvailableForms.TabIndex = 5;
            // 
            // lsSelectedForms
            // 
            this.lsSelectedForms.FormattingEnabled = true;
            this.lsSelectedForms.Location = new System.Drawing.Point(349, 21);
            this.lsSelectedForms.Name = "lsSelectedForms";
            this.lsSelectedForms.Size = new System.Drawing.Size(243, 225);
            this.lsSelectedForms.TabIndex = 6;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(270, 108);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(63, 23);
            this.cmdAdd.TabIndex = 7;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(270, 137);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(63, 23);
            this.cmdRemove.TabIndex = 8;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // btnDwn
            // 
            this.btnDwn.Location = new System.Drawing.Point(598, 50);
            this.btnDwn.Name = "btnDwn";
            this.btnDwn.Size = new System.Drawing.Size(75, 23);
            this.btnDwn.TabIndex = 10;
            this.btnDwn.Text = "Move Down";
            this.btnDwn.UseVisualStyleBackColor = true;
            this.btnDwn.Click += new System.EventHandler(this.btnDwn_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(598, 21);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "Move Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // frmCustomFormCheckList
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(683, 294);
            this.Controls.Add(this.btnDwn);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.lsSelectedForms);
            this.Controls.Add(this.lstAvailableForms);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.checkListForms);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCustomFormCheckList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Custom Form(s)";
            this.Load += new System.EventHandler(this.frmCustomFormCheckList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkListForms;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListBox lstAvailableForms;
        private System.Windows.Forms.ListBox lsSelectedForms;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Button btnDwn;
        private System.Windows.Forms.Button btnUp;
    }
}