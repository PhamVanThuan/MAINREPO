namespace SAHL.X2Designer.Views
{
    partial class BusinessStageTransitionsView
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(222, 229);
            this.tabControlMain.TabIndex = 3;
            // 
            // BusinessStageTransitionsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 229);
            this.Controls.Add(this.tabControlMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BusinessStageTransitionsView";
            this.ShowInTaskbar = false;
            this.TabText = "Business Stage Transitions";
            this.Text = "Business Stage Transitions";
            this.Load += new System.EventHandler(this.BusinessStageTransitionsView_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BusinessStageTransitionsView_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;

    }
}