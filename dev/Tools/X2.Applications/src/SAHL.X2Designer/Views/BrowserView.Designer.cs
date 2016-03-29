namespace SAHL.X2Designer.Views
{
    partial class BrowserView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserView));
            this.imageListBrowser = new System.Windows.Forms.ImageList(this.components);
            this.treeViewProc = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // imageListBrowser
            // 
            this.imageListBrowser.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBrowser.ImageStream")));
            this.imageListBrowser.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBrowser.Images.SetKeyName(0, "");
            this.imageListBrowser.Images.SetKeyName(1, "new workflow-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(2, "starting point-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(3, "user state 1-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(4, "system state-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(5, "common state-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(6, "archive state-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(7, "user activity 1-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(8, "externalActivity activity-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(9, "timer activity-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(10, "decision activity-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(11, "comment-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(12, "user security groups-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(13, "user state 1-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(14, "custom variable 1-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(15, "new form-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(16, "activities-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(17, "states-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(18, "forms-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(19, "comment-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(20, "system decision-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(21, "workflow activity.png");
            this.imageListBrowser.Images.SetKeyName(22, "user security group-16x16x32b.png");
            this.imageListBrowser.Images.SetKeyName(23, "holdState 16x16x32.png");
            // 
            // treeViewProc
            // 
            this.treeViewProc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewProc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProc.HideSelection = false;
            this.treeViewProc.ImageIndex = 0;
            this.treeViewProc.ImageList = this.imageListBrowser;
            this.treeViewProc.Location = new System.Drawing.Point(0, 0);
            this.treeViewProc.Name = "treeViewProc";
            this.treeViewProc.SelectedImageIndex = 0;
            this.treeViewProc.Size = new System.Drawing.Size(246, 391);
            this.treeViewProc.TabIndex = 0;
            this.treeViewProc.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProc_AfterSelect);
            this.treeViewProc.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.treeViewProc_ChangeUICues);
            this.treeViewProc.Click += new System.EventHandler(this.treeViewProc_Click);
            // 
            // BrowserView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 391);
            this.ControlBox = false;
            this.Controls.Add(this.treeViewProc);
            this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.DockAreas.DockRight)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrowserView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TabText = "Process Browser";
            this.Text = "Process Browser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListBrowser;
        public System.Windows.Forms.TreeView treeViewProc;
    }
}