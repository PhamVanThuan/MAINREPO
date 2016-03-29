namespace SAHL.X2Designer.Forms
{
    partial class frmCheckedListProperty
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
            this.checkedListMain = new System.Windows.Forms.CheckedListBox();
            this.cmdDone = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListMain
            // 
            this.checkedListMain.FormattingEnabled = true;
            this.checkedListMain.Location = new System.Drawing.Point(52, 12);
            this.checkedListMain.Name = "checkedListMain";
            this.checkedListMain.Size = new System.Drawing.Size(178, 199);
            this.checkedListMain.TabIndex = 8;
            // 
            // cmdDone
            // 
            this.cmdDone.Location = new System.Drawing.Point(214, 261);
            this.cmdDone.Name = "cmdDone";
            this.cmdDone.Size = new System.Drawing.Size(75, 23);
            this.cmdDone.TabIndex = 7;
            this.cmdDone.Text = "Done";
            this.cmdDone.UseVisualStyleBackColor = true;
            this.cmdDone.Click += new System.EventHandler(this.cmdDone_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(144, 221);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdAdd.TabIndex = 5;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(63, 221);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdRemove.TabIndex = 6;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // frmCheckedListProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 286);
            this.Controls.Add(this.checkedListMain);
            this.Controls.Add(this.cmdDone);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmdRemove);
            this.Name = "frmCheckedListProperty";
            this.ShowInTaskbar = false;
            this.Text = "Select";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListMain;
        private System.Windows.Forms.Button cmdDone;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdRemove;
    }
}