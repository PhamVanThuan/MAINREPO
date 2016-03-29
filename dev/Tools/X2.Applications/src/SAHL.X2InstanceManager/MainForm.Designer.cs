namespace SAHL.X2InstanceManager
{
    partial class MainForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Help Desk");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Custom Variables");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Forms");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("External Activity Sources");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Roles");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Child Instances");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("History");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Parent Instance");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Work List");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Track List");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("10653488", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Contact Client", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("LOA");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Benefits");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("etc...");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("States", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("LifeOrigination", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Workflows", new System.Windows.Forms.TreeNode[] {
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Life", new System.Windows.Forms.TreeNode[] {
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Processes", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("SAHLS103", new System.Windows.Forms.TreeNode[] {
            treeNode20});
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("X2 Databases", new System.Windows.Forms.TreeNode[] {
            treeNode21});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkInstanceCount = new System.Windows.Forms.CheckBox();
            this.treeMain = new System.Windows.Forms.TreeView();
            this.imageListMain = new System.Windows.Forms.ImageList(this.components);
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listViewMain = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnectToDB = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.btnEngine = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveDB = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(851, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(92, 22);
            this.mnuExit.Text = "&Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.findToolStripMenuItem.Text = "F&ind";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            this.viewToolStripMenuItem.Visible = false;
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.statusStrip1);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(851, 406);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(851, 431);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chkInstanceCount);
            this.splitContainer1.Panel1.Controls.Add(this.treeMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlMain);
            this.splitContainer1.Size = new System.Drawing.Size(851, 384);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 2;
            // 
            // chkInstanceCount
            // 
            this.chkInstanceCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInstanceCount.AutoSize = true;
            this.chkInstanceCount.BackColor = System.Drawing.Color.White;
            this.chkInstanceCount.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkInstanceCount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkInstanceCount.Location = new System.Drawing.Point(135, 3);
            this.chkInstanceCount.Name = "chkInstanceCount";
            this.chkInstanceCount.Size = new System.Drawing.Size(133, 17);
            this.chkInstanceCount.TabIndex = 3;
            this.chkInstanceCount.Text = "Display Instance Count";
            this.chkInstanceCount.UseVisualStyleBackColor = false;
            // 
            // treeMain
            // 
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.HideSelection = false;
            this.treeMain.ImageIndex = 0;
            this.treeMain.ImageList = this.imageListMain;
            this.treeMain.Location = new System.Drawing.Point(0, 0);
            this.treeMain.Margin = new System.Windows.Forms.Padding(2);
            this.treeMain.Name = "treeMain";
            treeNode1.Name = "Node4";
            treeNode1.Text = "Help Desk";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Custom Variables";
            treeNode3.Name = "Node2";
            treeNode3.Text = "Forms";
            treeNode4.Name = "Node3";
            treeNode4.Text = "External Activity Sources";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Roles";
            treeNode6.Name = "Node10";
            treeNode6.Text = "Child Instances";
            treeNode7.Name = "Node12";
            treeNode7.Text = "History";
            treeNode8.Name = "Node11";
            treeNode8.Text = "Parent Instance";
            treeNode9.Name = "Node13";
            treeNode9.Text = "Work List";
            treeNode10.Name = "Node14";
            treeNode10.Text = "Track List";
            treeNode11.Name = "Node15";
            treeNode11.Text = "10653488";
            treeNode12.Name = "Node6";
            treeNode12.Text = "Contact Client";
            treeNode13.Name = "Node7";
            treeNode13.Text = "LOA";
            treeNode14.Name = "Node8";
            treeNode14.Text = "Benefits";
            treeNode15.Name = "Node9";
            treeNode15.Text = "etc...";
            treeNode16.Name = "Node5";
            treeNode16.Text = "States";
            treeNode17.Name = "Node0";
            treeNode17.Text = "LifeOrigination";
            treeNode18.Name = "Node5";
            treeNode18.Text = "Workflows";
            treeNode19.Name = "Node3";
            treeNode19.Text = "Life";
            treeNode20.Name = "Node2";
            treeNode20.Text = "Processes";
            treeNode21.Name = "Node1";
            treeNode21.Text = "SAHLS103";
            treeNode22.Name = "Node0";
            treeNode22.Text = "X2 Databases";
            this.treeMain.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode22});
            this.treeMain.SelectedImageIndex = 0;
            this.treeMain.Size = new System.Drawing.Size(271, 384);
            this.treeMain.TabIndex = 0;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.treeMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseClick);
            // 
            // imageListMain
            // 
            this.imageListMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMain.ImageStream")));
            this.imageListMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMain.Images.SetKeyName(0, "");
            this.imageListMain.Images.SetKeyName(1, "");
            this.imageListMain.Images.SetKeyName(2, "");
            this.imageListMain.Images.SetKeyName(3, "");
            this.imageListMain.Images.SetKeyName(4, "");
            this.imageListMain.Images.SetKeyName(5, "");
            this.imageListMain.Images.SetKeyName(6, "");
            this.imageListMain.Images.SetKeyName(7, "");
            this.imageListMain.Images.SetKeyName(8, "");
            this.imageListMain.Images.SetKeyName(9, "");
            this.imageListMain.Images.SetKeyName(10, "");
            this.imageListMain.Images.SetKeyName(11, "");
            this.imageListMain.Images.SetKeyName(12, "");
            this.imageListMain.Images.SetKeyName(13, "");
            this.imageListMain.Images.SetKeyName(14, "dieter.ico");
            this.imageListMain.Images.SetKeyName(15, "Workflows.png");
            this.imageListMain.Images.SetKeyName(16, "Process.png");
            this.imageListMain.Images.SetKeyName(17, "stop.gif");
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(577, 384);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listViewMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(569, 358);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listViewMain
            // 
            this.listViewMain.AllowColumnReorder = true;
            this.listViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewMain.FullRowSelect = true;
            this.listViewMain.Location = new System.Drawing.Point(3, 3);
            this.listViewMain.Name = "listViewMain";
            this.listViewMain.Size = new System.Drawing.Size(563, 352);
            this.listViewMain.TabIndex = 1;
            this.listViewMain.UseCompatibleStateImageBehavior = false;
            this.listViewMain.View = System.Windows.Forms.View.Details;
            this.listViewMain.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewMain_ColumnClick);
            this.listViewMain.SelectedIndexChanged += new System.EventHandler(this.listViewMain_SelectedIndexChanged);
            this.listViewMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewMain_MouseDoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(569, 358);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(851, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Ready";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnectToDB,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnStop,
            this.btnEngine,
            this.btnRemoveDB});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(164, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // btnConnectToDB
            // 
            this.btnConnectToDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConnectToDB.Image = ((System.Drawing.Image)(resources.GetObject("btnConnectToDB.Image")));
            this.btnConnectToDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnectToDB.Name = "btnConnectToDB";
            this.btnConnectToDB.Size = new System.Drawing.Size(23, 22);
            this.btnConnectToDB.Text = "Connect to X2 Database";
            this.btnConnectToDB.Visible = false;
            this.btnConnectToDB.Click += new System.EventHandler(this.btnConnectToDB_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(23, 22);
            this.btnRefresh.Text = "Refresh Selected Connection";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnEngine
            // 
            this.btnEngine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEngine.Image = ((System.Drawing.Image)(resources.GetObject("btnEngine.Image")));
            this.btnEngine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEngine.Name = "btnEngine";
            this.btnEngine.Size = new System.Drawing.Size(23, 22);
            this.btnEngine.Text = "Connect to X2 Engine";
            this.btnEngine.Visible = false;
            this.btnEngine.Click += new System.EventHandler(this.btnEngine_Click);
            // 
            // btnRemoveDB
            // 
            this.btnRemoveDB.Name = "btnRemoveDB";
            this.btnRemoveDB.Size = new System.Drawing.Size(23, 22);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 455);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "X2 Instance Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConnectToDB;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRemoveDB;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ImageList imageListMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listViewMain;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        public System.Windows.Forms.TreeView treeMain;
        public System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripButton btnEngine;
        private System.Windows.Forms.CheckBox chkInstanceCount;


    }
}

