using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ActiproSoftware.SyntaxEditor.Addons.CSharp;
using ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Win32;
using Northwoods.Go;
using SAHL.Tools.Workflow.Common.Compiler;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.Properties;
using SAHL.X2Designer.Publishing;
using SAHL.X2Designer.Views;
using SAHL.X2Designer.XML;
using WeifenLuo.WinFormsUI;
using NHibernate;
using SAHL.Tools.Workflow.Common.Database;
using NuGet;
using SAHL.Tools.Workflow.Common.ReferenceChecking;


namespace SAHL.X2Designer
{
    [Serializable]
    public partial class MainForm : Form
    {
        public bool formSelectedInPropertyGrid = false;
        public bool roleSelectedInPropertyGrid = false;
        public bool variableSelectedInPropertyGrid = false;
        public bool documentIsBeingOpened = false;
        public bool newDocumentBeingCreated = false;
        public bool isDrawingActivity = false;
        public bool isCompiling = false;

        public PropertiesView m_PropsView;
        public CodeView m_CodeView;
        public BrowserView m_BrowserView;
        public SecurityView m_SecurityView;
        public frmFindReplace m_FindReplaceView;
        public CodeErrors m_ErrorsView;
        public BusinessStageTransitionsView m_BusinessStageTransitionsView;

        public DotNetProjectResolver dotNetProjectResolver;
        public CSharpSyntaxLanguage cSharpLanguage;

        ICutCopyPasteTarget m_CutCopyTarget;

        //The following delegates and events handle the setting of objects to unselectable when a toolbar
        //button relating to an activity is checked

        public delegate void ActivityItemCheckedHandler(object sender);
        public delegate void ActivityItemUnCheckedHandler(object sender);
        public event ActivityItemCheckedHandler onActivityItemChecked;
        public event ActivityItemUnCheckedHandler onActivityItemUnChecked;

        //The following delegates and events are used to notify views as to when a external activity,role,custom variable
        //or custom form has been closed
        public delegate void GeneralCustomFormClosedHandler(GeneralCustomFormType formType);
        public event GeneralCustomFormClosedHandler OnGeneralCustomFormClosed;

        //Delegates and events for handling processViews
        public delegate void ProcessViewActivatedHandler(ProcessView v);
        public event ProcessViewActivatedHandler OnProcessViewActivated;
        public delegate void ProcessViewDeactivatedHandler(ProcessView v);
        public event ProcessViewDeactivatedHandler OnProcessViewDeactivated;

        //Handling for Roles Collection
        public delegate void RoleItemSelectedHandler(RolesCollectionItem roleItem);
        public event RoleItemSelectedHandler OnRoleItemSelected;

        //Handling for Custom Variable Collection
        public delegate void CustomVariableItemSelectedHandler(CustomVariableItem varItem);
        public event CustomVariableItemSelectedHandler OnCustomVariableItemSelected;

        //Handling for Custom Form Collection
        public delegate void CustomFormItemSelectedHandler(CustomFormItem frmItem);
        public event CustomFormItemSelectedHandler OnCustomFormItemSelected;

        //Handling for Changing of StageTransitionMessage
        public delegate void OnStageTransitionMessageHandler(PropertyType propType);
        public event OnStageTransitionMessageHandler OnStageTransitionMessageChanged;

        public delegate void ServerReadyHandler();
        public event ServerReadyHandler OnServersReady;

        public List<ProcessViewItem> lstViewsToBeSaved = new List<ProcessViewItem>();
        public List<RolesCollectionItem> m_Roles = new List<RolesCollectionItem>();

        public bool mainFormIsClosing = false;
        private bool mainFormCloseCancel = false;

        protected MruStripMenuInline mruMenu;
        static string mruRegKey = "SOFTWARE\\X2Designer\\MruFiles";

        protected string curFileName;

        private System.Data.DataTable m_ServersTable;

        private string[] CommandLineArgs;

        public bool isAddingExternalReferences = false;

        public DockContent DC = new DockContent();

        public System.Windows.Forms.Panel panel1 = new System.Windows.Forms.Panel();
        private System.Windows.Forms.Panel panel2 = new System.Windows.Forms.Panel();

        public bool loadingFromFile = false;

        private bool setShowPropertiesOnTop = false;
        private bool setShowBrowserOnTop = false;

        public bool PastingMultipleSelection = false;

        public List<SelectionItem> lstMultipleSelectionItems = new List<SelectionItem>();
        public List<CustomLinkItem> lstMultipleSelectionLinks = new List<CustomLinkItem>();

        private System.Windows.Forms.CheckBox chkLegacyMap;
        private System.Windows.Forms.CheckBox chkHaloV3Viewable;

        public MainForm(string[] args)
        {
            CommandLineArgs = args;

            string FilePath = Path.GetDirectoryName(Application.ExecutablePath);
            Associator.Associate(".x2p", "X2Designer", "X2 Designer Process file.", FilePath + "\\X2.ico", Application.ExecutablePath);

            InitializeComponent();

            this.Text = this.Text += String.IsNullOrEmpty(Application.ProductVersion) ? "" : " - Version : " + Application.ProductVersion;

            lstMainReferences.Add("SAHL.Common.dll");
            lstMainReferences.Add("SAHL.Common.DataAccess.dll");
            lstMainReferences.Add("SAHL.Common.Datasets.dll");
            lstMainReferences.Add("SAHL.Common.X2.dll");
            lstMainReferences.Add("SAHL.Common.X2.Interfaces.dll");

            string buildDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Build";
            if (Directory.Exists(buildDirectory) == false)
            {
                Directory.CreateDirectory(buildDirectory);
            }
            else
            {
                string[] files = Directory.GetFiles(buildDirectory);
                for (int x = 0; x < files.Length; x++)
                {
                    File.Delete(files[x]);
                }
            }

            System.Drawing.Size mySize = new Size(16, 16);
            GoImage.DefaultResourceManager = new ResourceManager("SAHL.X2Designer.Images", this.GetType().Assembly);
            myMainForm = this;

            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(mruRegKey);
            if (regKey != null)
            {
                regKey.Close();
            }

            mruMenu = new MruStripMenuInline(mnuItemFile, mnuRecentFiles, new MruStripMenu.ClickedHandler(OnMruFile), mruRegKey + "\\MRU");

            RefreshServers();
        }

        public static MainForm App
        {
            get { return myMainForm; }
        }

        public ICutCopyPasteTarget CutCopyPasteTarget
        {
            get
            {
                return m_CutCopyTarget;
            }
            set
            {
                m_CutCopyTarget = value;
            }
        }

        public bool isRetrieved = false;
        private string _rootfolder = "";

        /// <summary>
        /// Gets or Sets the Rootfolder that the Designer uses
        /// </summary>
        public string RootFolder
        {
            get
            {
                if (isRetrieved)
                {
                    return _rootfolder;
                }
                else
                {
                        return null;
                }
            }
            set
            {
                _rootfolder = value;
            }
        }

        public List<string> lstMainReferences = new List<string>();

        // globally useful methods
        private static MainForm myMainForm = null;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // check if there are command line arguments
            if (CommandLineArgs != null && CommandLineArgs.Length > 0)
            {
                for (int i = 0; i < CommandLineArgs.Length; i++)
                {
                    if (File.Exists(CommandLineArgs[i]))
                    {
                        loadingFromFile = true;
                        loadDocumentFromFile(CommandLineArgs[i]);
                        loadingFromFile = false;
                    }
                }
            }
            else
            {
                createNewProcessForm();
                MainForm.App.GetCurrentView().Document.FixedSize = false;
                MainForm.App.GetCurrentView().Document.ComputeBounds();
            }

            panel1.Dock = DockStyle.Fill;
            panel2.Dock = DockStyle.Fill;

            cbxZoom.SelectedIndex = 3;
            ShowProcessBrowserWindow();
            ShowPropertiesWindow();

            // add checkboxes for "Legacy Map" & "Halo v3 Viewable" to toolstip
            chkLegacyMap = new CheckBox();
            chkLegacyMap.Text = "Legacy Map";
            ToolStripControlHost host = new ToolStripControlHost(chkLegacyMap);
            toolStripBuild.Items.Insert(toolStripBuild.Items.Count, host);

            chkHaloV3Viewable = new CheckBox();
            chkHaloV3Viewable.Text = "Halo v3 Viewable";
            host = new ToolStripControlHost(chkHaloV3Viewable);
            toolStripBuild.Items.Insert(toolStripBuild.Items.Count, host);


            this.WindowState = FormWindowState.Maximized;
        }

        #region ServerHandling

        public DataTable Servers
        {
            get
            {
                return m_ServersTable;
            }
        }

        public void RefreshServers()
        {
            Thread Load = new Thread(new ThreadStart(PopulateServers));
            Load.Start();
        }

        private void PopulateServers()
        {

            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            m_ServersTable = instance.GetDataSources();

            if (OnServersReady != null)
            {
                OnServersReady();
            }
        }

        #endregion ServerHandling

        #region General Toolbar Handlers

        private void OnNewClick(object sender, EventArgs e)
        {
            if (m_CodeView != null)
            {
                m_CodeView.ClearView();
            }
            createNewProcessForm();
        }

        private void OnOpenClick(object sender, EventArgs e)
        {
            if (MainForm.App.m_FindReplaceView != null)
            {
                m_FindReplaceView.Close();
                m_FindReplaceView.Dispose();
            }
            OpenFileDialog mDialog = new OpenFileDialog();
            mDialog.DefaultExt = ".x2p";
            mDialog.FileName = "*.x2p";
            mDialog.Filter = "X2Designer Files | *.x2p";
            this.setStatusBar("Open Document...");
            DialogResult res = mDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    // open the file
                    OpenDocument(mDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred during the loading of the document!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                    this.setStatusBar("Open Document Failed");
                    ExceptionPolicy.HandleException(ex, "X2Designer");
                }
            }

            mDialog.Dispose();
        }

        private void OpenDocument(string fileName)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is ProcessForm)
                {
                    ProcessForm p = f as ProcessForm;
                    if (p.View.Name == fileName)
                    {
                        MessageBox.Show("This Process is already open!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.setStatusBar("Open Document Failed");
                        return;
                    }
                }
            }

            string xmlFile = "";
            Application.DoEvents();

            documentIsBeingOpened = true;
            XML.XMLOpenDocument myOpenedDocument = new SAHL.X2Designer.XML.XMLOpenDocument(fileName, xmlFile);

            Application.DoEvents();

            MainForm.App.GetCurrentView().Name = fileName;

            MainForm.App.GetCurrentView().Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[0]);
            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Collapse();
            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Expand();
            Application.DoEvents();

            chkLegacyMap.Checked = MainForm.App.GetCurrentView().Document.IsLegacy;
            chkHaloV3Viewable.Checked = MainForm.App.GetCurrentView().Document.HaloV3Viewable;

            for (int z = 0; z < MainForm.App.GetCurrentView().Document.WorkFlows.Length; z++)
            {
                MainForm.App.GetCurrentView().Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[z]);

                foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[z])
                {
                    BaseActivity i = o as BaseActivity;
                    if (i != null)
                    {
                        for (int x = 0; x < myOpenedDocument.lstNodeItems.Count; x++)
                        {
                            if (i == myOpenedDocument.lstNodeItems[x].Item)
                            {
                                float holdX = myOpenedDocument.lstNodeItems[x].Position.X;
                                float holdY = myOpenedDocument.lstNodeItems[x].Position.Y;

                                i.Position = new PointF(holdX, holdY);
                                continue;
                            }
                        }
                        continue;
                    }

                    BaseState bs = o as BaseState;
                    if (bs != null)
                    {
                        for (int x = 0; x < myOpenedDocument.lstNodeItems.Count; x++)
                        {
                            if (bs == myOpenedDocument.lstNodeItems[x].Item as BaseState)
                            {
                                float holdX = myOpenedDocument.lstNodeItems[x].Position.X;
                                float holdY = myOpenedDocument.lstNodeItems[x].Position.Y;

                                bs.Position = new PointF(holdX, holdY);

                                continue;
                            }
                        }
                        continue;
                    }

                    if (o is Comment)
                    {
                        Comment mComment = o as Comment;
                        for (int x = 0; x < myOpenedDocument.lstNodeItems.Count; x++)
                        {
                            if (mComment == myOpenedDocument.lstNodeItems[x].Item as Comment)
                            {
                                float holdX = myOpenedDocument.lstNodeItems[x].Position.X;
                                float holdY = myOpenedDocument.lstNodeItems[x].Position.Y;

                                mComment.Position = new PointF(holdX, holdY);
                                continue;
                            }
                        }
                        continue;
                    }

                    InvisibleAnchorNode inv = o as InvisibleAnchorNode;
                    if (inv != null)
                    {
                        for (int x = 0; x < myOpenedDocument.lstNodeItems.Count; x++)
                        {
                            if (inv == myOpenedDocument.lstNodeItems[x].Item as InvisibleAnchorNode)
                            {
                                float holdX = myOpenedDocument.lstNodeItems[x].Position.X;
                                float holdY = myOpenedDocument.lstNodeItems[x].Position.Y;

                                inv.Position = new PointF(holdX, holdY);
                                continue;
                            }
                        }
                        continue;
                    }

                    ClapperBoard cl = o as ClapperBoard;
                    if (cl != null)
                    {
                        for (int x = 0; x < myOpenedDocument.lstNodeItems.Count; x++)
                        {
                            if (cl == myOpenedDocument.lstNodeItems[x].Item as ClapperBoard)
                            {
                                float holdX = myOpenedDocument.lstNodeItems[x].Position.X;
                                float holdY = myOpenedDocument.lstNodeItems[x].Position.Y;

                                cl.Position = new PointF(holdX, holdY);
                                continue;
                            }
                        }
                        continue;
                    }
                }
            }

            documentIsBeingOpened = false;

            Cursor = Cursors.Default;
            MainForm.App.GetCurrentView().Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[0]);

            if (m_BrowserView != null)
            {
                m_BrowserView.PopulateBrowser();
            }

            this.setStatusBar("Document Opened Successfully");
            ProcessView processView = this.GetCurrentView();
            if (processView.Document.UndoManager != null)
                processView.Document.UndoManager.Clear();
            processView.setModified(false);
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            CloseDocument();
        }

        private void CloseDocument()
        {
            if (this.ActiveMdiChild != null)
            {
                ActiveMdiChild.Close();
            }
        }

        private void OnCloseAllClick(object sender, EventArgs e)
        {
            CloseAllDocuments();
        }

        private void CloseAllDocuments()
        {
            ArrayList Children = new ArrayList(MdiChildren);
            while (Children.Count > 0)
            {
                ((Form)Children[0]).Close();
                Children.RemoveAt(0);
            }
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SaveProcess(true);

            this.Cursor = Cursors.Default;
        }

        private bool SaveProcess(bool updateStatusBar)
        {
            List<CallWorkFlowActivity> lstCallWorkFlowActivitiesNotSet = X2Publisher.CheckIfCallWorkFlowActivityPropertiesSet(this.GetCurrentView().Document);
            if (lstCallWorkFlowActivitiesNotSet.Count > 0)
            {
                frmCallWorkFlowActivitiesNotSet mFrm = new frmCallWorkFlowActivitiesNotSet(lstCallWorkFlowActivitiesNotSet);
                mFrm.ShowDialog();
                MessageBox.Show("Document has NOT been saved.");
                return false;
            }
            return Save(this.GetCurrentView(), this.Text, updateStatusBar);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0010) // Win32.WM_CLOSE
            {
                mainFormIsClosing = true;
                mainFormCloseCancel = false;
            }
            base.WndProc(ref m);
        }

        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
            if (GetCurrentView() != null)
            {
                cbxZoom.Text = GetCurrentView().m_ZoomValue;
                unCheckToolStripButtons();
            }
        }

        public void setStatusBar(string text)
        {
            lblStatus.Text = text;
            Application.DoEvents();
        }

        private void OnSaveAsClick(object sender, EventArgs e)
        {
            SaveAs(this.GetCurrentView(), this.GetCurrentView().Text);
        }

        public virtual bool Save(ProcessView View, string viewName, bool updateStatusBar)
        {
            if (ValidateSave() == false)
                return false;

            String loc = View.Document.Location;
            WorkFlow originalWorkFlow = null;

            if (MainForm.App.GetCurrentView() != null)
                originalWorkFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;

            if (loc != "")
            {
                if (m_CodeView != null)
                {
                    if (m_CodeView.syntaxEditor != null)
                    {
                        if (m_CodeView.syntaxEditor.Document != null)
                            m_CodeView.SaveCode();
                    }
                }
                if (updateStatusBar)
                {
                    this.setStatusBar("Saving Document");
                    Application.DoEvents();
                }

                MainForm.App.GetCurrentView().Document.IsLegacy = chkLegacyMap.Checked;
                MainForm.App.GetCurrentView().Document.HaloV3Viewable = chkHaloV3Viewable.Checked;

                XML.XMLSaveDocument myXML = new SAHL.X2Designer.XML.XMLSaveDocument(loc);
                if (originalWorkFlow == null)
                    MainForm.App.GetCurrentView().Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[0]);
                else
                    MainForm.App.GetCurrentView().Document.SelectWorkFlow(originalWorkFlow);

                if (updateStatusBar) 
                {
                    this.setStatusBar("Document Saved");
                    Application.DoEvents();
                }

                mruMenu.AddFile(loc);
                this.GetCurrentView().setModified(false);
                return true;
            }
            else
            {
                try
                {
                    return MainForm.App.SaveAs(View, viewName);
                }
                catch (Exception e)
                {
                    ExceptionPolicy.HandleException(e, "X2Designer");
                    return false;
                }
            }
        }

        public bool SaveAs(ProcessView View, string viewName)
        {
            if (ValidateSave() == false)
                return false;

            SaveFileDialog mDialog = new SaveFileDialog();
            WorkFlow originalWorkFlow = null;
            this.Cursor = Cursors.WaitCursor;
            if (MainForm.App.GetCurrentView() != null)
            {
                originalWorkFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;
            }
            mDialog.DefaultExt = "*.x2p";
            mDialog.Filter = "X2Designer Files | *.x2p";
            mDialog.Title = viewName;
            mDialog.FileName = this.GetCurrentView().Text + ".x2p";
            if (lblStatus.Text == "Ready")
            {
                setStatusBar("Saving...");
            }
            DialogResult res = mDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (Path.GetFileName(mDialog.FileName).Length > 30)
                {
                    MessageBox.Show("Process Filename cannot exceed 30 characters!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    lblStatus.Text = "Save Cancelled!";
                    Cursor = Cursors.Default;
                    return false;
                }
                else
                {
                    try
                    {
                        if (m_CodeView != null)
                        {
                            if (m_CodeView.syntaxEditor != null)
                            {
                                if (m_CodeView.syntaxEditor.Document != null)
                                {
                                    m_CodeView.SaveCode();
                                }
                            }
                        }
                        if (curFileName != mDialog.FileName)
                        {
                            mruMenu.AddFile(mDialog.FileName);
                            curFileName = mDialog.FileName;
                        }

                        this.Cursor = Cursors.WaitCursor;
                        XML.XMLSaveDocument mySavedDoc = new SAHL.X2Designer.XML.XMLSaveDocument(mDialog.FileName);
                        View.Document.Location = mDialog.FileName;
                        View.Document.Name = Path.GetFileNameWithoutExtension(View.Document.Location);

                        if (originalWorkFlow == null)
                        {
                            MainForm.App.GetCurrentView().Document.SelectWorkFlow(MainForm.App.GetCurrentView().Document.WorkFlows[0]);
                        }
                        else
                        {
                            MainForm.App.GetCurrentView().Document.SelectWorkFlow(originalWorkFlow);
                        }
                        GetCurrentView().UpdateTitle();

                        this.Cursor = Cursors.Default;
                        this.setStatusBar("Document Saved");
                        this.Cursor = Cursors.Default;

                        this.GetCurrentView().setModified(false);
                        return true;
                    }
                    catch (Exception err)
                    {
                        this.Cursor = Cursors.Default;
                        if (this.GetCurrentView() != null)
                        {
                            this.GetCurrentView().Cursor = Cursors.Default;
                        }
                        MessageBox.Show("An error has occurred during the save!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(err.ToString());
                        this.setStatusBar("Error Occurred During Saving");
                        ExceptionPolicy.HandleException(err, "X2Designer");
                        return false;
                    }
                }
            }
            else
            {
                if (res == DialogResult.Cancel)
                {
                    this.setStatusBar("Save Cancelled");
                    return false;
                }
                else
                {
                    if (res == DialogResult.No)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private bool ValidateSave()
        {
            bool valid = true;
            if (chkLegacyMap.Checked && chkHaloV3Viewable.Checked)
            {
                MessageBox.Show("Legacy map cannot be viewed on Halo v3.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lblStatus.Text = "Save Cancelled!";
                Cursor = Cursors.Default;
                valid = false;
            }

            return valid;
        }

        private void OnPublishClick(object sender, EventArgs e)
        {
            // save current process first
            if (SaveProcess(true) == false)
            {
                MessageBox.Show("An error has occurred during the save - map cannot be compiled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setStatusBar("Error(s) Encountered");
                this.Cursor = Cursors.Default;
            }
            else
            {
                // open the publishing form
                frmPublisher frmBP = new frmPublisher(MainForm.App.GetCurrentView() == null ? "" : MainForm.App.GetCurrentView().Name);
                if (frmBP.ShowDialog() == DialogResult.Cancel)
                {
                    frmBP.Close();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                // get properties from publishing form
                string databaseServer = frmBP.DatabaseServer;

                // close the publishing form
                frmBP.Close();

                // validate if we can continue
                if (GetCurrentView() == null)
                {
                    MessageBox.Show("No map to publish!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // setup publishing enviroment variables
                X2Publisher.ConnectionStringX2 = "Data Source=" + databaseServer + ";Initial Catalog=" + Properties.Settings.Default.DefaultCatalogX2 + ";Persist Security Info=True;User ID=" + Properties.Settings.Default.DBUsername + ";Password=" + Properties.Settings.Default.DBPassword + ";Connect Timeout=" + Properties.Settings.Default.ConnectionTimeout;
                X2Publisher.ConnectionString2AM = "Data Source=" + databaseServer + ";Initial Catalog=" + Properties.Settings.Default.DefaultCatalog2AM + ";Persist Security Info=True;User ID=" + Properties.Settings.Default.DBUsername + ";Password=" + Properties.Settings.Default.DBPassword + ";Connect Timeout=" + Properties.Settings.Default.ConnectionTimeout;
                X2Publisher.cmdTimeout = Properties.Settings.Default.CommandTimeout;

                bool publishSuccessfull = true;
                List<string> compilerErrorMessages = new List<string>();
                List<string> publisherErrorMessages = new List<string>();
                List<PublisherResults> publisherResultsList = new List<PublisherResults>();
                int noOfInstancesRecalculated = 0;

                // publish the map
                ProcessDocument map = GetCurrentView().Document;
                if (!InitialMapChecks(map))
                    return;

                // do the publish - security recalc happens inside here if required
                publishSuccessfull = X2Publisher.Publish(map, Application.ExecutablePath, out compilerErrorMessages, out publisherErrorMessages, out noOfInstancesRecalculated);

                this.Cursor = Cursors.Default;

                if (publishSuccessfull == false)
                {
                    if (publishSuccessfull == false)
                        ShowPublishErrors(compilerErrorMessages, publisherErrorMessages);
                }
                else
                {
                    if (noOfInstancesRecalculated > 0)
                        MessageBox.Show("Map has been published successfully ! " + noOfInstancesRecalculated + " instances have had their security recalculated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Map has been published successfully ! No instances required security recalculation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowPublishErrors(List<string> compilerErrorMessages, List<string> publisherErrorMessages)
        {
            if (compilerErrorMessages.Count > 0)
            {
                StringBuilder errorMessage = new StringBuilder("Unable to compile this map.\r\n\r\nThis map will not be published.");
                MessageBox.Show(errorMessage.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool InitialMapChecks(ProcessDocument doc)
        {
            bool missingGenericKeysOrKeyVariable = false;
            foreach (WorkFlow w in doc.WorkFlows)
            {
                if (w.GenericKeyTypeKey == -1)
                {
                    missingGenericKeysOrKeyVariable = true;
                    MessageBox.Show("The GenericKeyType has not been set for " + w.WorkFlowName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ClapperBoard CB = null;
                foreach (GoObject o in w)
                {
                    if (o.GetType() == typeof(ClapperBoard))
                    {
                        CB = o as ClapperBoard;
                        break;
                    }
                }
                if (CB == null)
                {
                    MessageBox.Show("Cannot find starting point for workflow " + w.WorkFlowName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string keyVar = "";
                    if (CB.KeyVariable != null)
                    {
                        keyVar = CB.KeyVariable.Name.ToString();
                    }
                    else
                    {
                        MessageBox.Show("The KeyVariable property has not been set for " + w.WorkFlowName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        missingGenericKeysOrKeyVariable = true;
                    }
                }
            }
            if (missingGenericKeysOrKeyVariable)
            {
                MessageBox.Show("Publishing has been cancelled !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void OnRetrieveClick(object sender, EventArgs e)
        {
            string ProcessID = "";

            // open the retrieval form
            frmRetrieve frmRetrieve = new frmRetrieve(MainForm.App.GetCurrentView() == null ? "" : MainForm.App.GetCurrentView().Name);
            if (frmRetrieve.ShowDialog() == DialogResult.Cancel)
            {
                frmRetrieve.Close();
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            // get properties from retrieval form
            string databaseServer = frmRetrieve.DatabaseServer;

            // close the retrieval form
            frmRetrieve.Close();

            // build up a connectionstring
            SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder();
            SCSB.DataSource = databaseServer;
            SCSB.InitialCatalog = Properties.Settings.Default.DefaultCatalogX2;
            SCSB.UserID = Properties.Settings.Default.DBUsername;
            SCSB.Password = Properties.Settings.Default.DBPassword;
            SCSB.IntegratedSecurity = true;

            frmRetrievePublishedProcess fRetrieve = new frmRetrievePublishedProcess();

            SqlConnection Connection = new SqlConnection(SCSB.ToString());

            Connection.Open();
            SqlTransaction Transaction = Connection.BeginTransaction();

            SqlCommand Cmd = null;
            Cmd = Connection.CreateCommand();
            Cmd.Transaction = Transaction;
            string mQuery = "select [ID],[Name],[Version] from [X2].Process order by [Name],[ID] desc ,[Version]";
            //string mQuery = "select [ID],[Name],[Version] from [X2].Process order by [ID],[Name],[Version]";
            Cmd.CommandText = mQuery;
            SqlDataReader mReader = Cmd.ExecuteReader();
            while (mReader.Read())
            {
                string[,] lst = new string[1, 3];
                lst[0, 0] = mReader["ID"].ToString();
                lst[0, 1] = mReader["Name"].ToString();
                lst[0, 2] = mReader["Version"].ToString();
                string[] mStr = new string[] { lst[0, 0], lst[0, 1], lst[0, 2] };
                ListViewItem mItem = new ListViewItem(mStr);
                fRetrieve.listViewProcess.Items.Add(mItem);
            }
            mReader.Close();

            object dbDesignerData = null;
            string dbConfigFile = null;
            DialogResult res = fRetrieve.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (fRetrieve.listViewProcess.SelectedIndices.Count > 0)
                {
                    ProcessID = fRetrieve.listViewProcess.Items[fRetrieve.listViewProcess.SelectedIndices[0]].SubItems[0].Text;
                    SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
                    idParam.Value = Convert.ToInt32(ProcessID);

                    // get the designer data
                    Cmd.CommandText = "Select [DesignerData] from [x2].[X2].Process where [ID] = @ID";
                    Cmd.Parameters.Add(idParam);
                    dbDesignerData = Cmd.ExecuteScalar();

                    // get the config file
                    Cmd.CommandText = "Select [ConfigFile] from [x2].[X2].Process where [ID] = @ID";
                    dbConfigFile = Cmd.ExecuteScalar().ToString();
                }

                byte[] mDesignerData = (byte[])dbDesignerData;

                FileStream FS = null;

                SaveFileDialog mDialog = new SaveFileDialog();
                mDialog.DefaultExt = "x2p";
                mDialog.Filter = "X2 Designer File | *.x2p";
                res = mDialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    try
                    {
                        // save the map
                        FS = new FileStream(mDialog.FileName, FileMode.Create);
                        foreach (byte b in mDesignerData)
                        {
                            FS.WriteByte(b);
                        }
                        FS.Flush();
                        FS.Close();

                        string configFileName = mDialog.FileName.Replace(".x2p", ".config");
                        
                        // save the config file
                        File.WriteAllText(configFileName, dbConfigFile);


                        System.Xml.XmlDocument xdoc = new XmlDocument();
                        xdoc.Load(mDialog.FileName);

                        XMLHandling.FlagMapAsRetrieved(mDialog.FileName, mDialog.FileName);

                        fRetrieve.Dispose();
                        Transaction.Commit();
                        Connection.Dispose();
                        Connection = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error was encountered while trying to save the file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExceptionPolicy.HandleException(ex, "X2Designer");
                        return;
                    }
                    try
                    {
                        OpenDocument(mDialog.FileName);
                    }
                    catch (Exception ex1)
                    {
                        MessageBox.Show("An error was encountered while trying to open the extracted file!\nMake sure the file is in the correct format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ExceptionPolicy.HandleException(ex1, "X2Designer");
                    }
                }
            }
        }

        private void OnPrintClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
                m_CutCopyTarget.DoPrint();
        }

        private void OnPrintPreviewClick(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.PrintPreview();
            }
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnUndoClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
                m_CutCopyTarget.DoUndo();
        }

        private void OnRedoClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
            {
                m_CutCopyTarget.DoRedo();
            }
        }

        private void OnCutClick(object sender, EventArgs e)
        {
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().clipBoardHasContents = true;
                this.GetCurrentView().pasteLocation = new PointF(0, 0);

                if (m_CutCopyTarget != null)
                {
                    m_CutCopyTarget.DoCut();
                }
            }
        }

        private void OnCopyClick(object sender, EventArgs e)
        {
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().clipBoardHasContents = true;
                this.GetCurrentView().pasteLocation = new PointF(0, 0);

                if (m_CutCopyTarget != null)
                {
                    m_CutCopyTarget.DoCopy();
                }
            }
        }

        private void OnPasteClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
            {
                m_CutCopyTarget.DoPaste();
            }
        }

        public void PasteSelection()
        {
            if (lstMultipleSelectionItems.Count == 0)
            {
                return;
            }
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().clipBoardHasContents = false;

                if (m_CutCopyTarget != null && lstMultipleSelectionItems.Count == 0)
                {
                    m_CutCopyTarget.DoPaste();
                }
                else
                {
                    PasteMultipleSelection(GetCurrentView().pasteLocation);
                }
            }
        }

        private void PasteMultipleSelection(PointF pasteLocation)
        {
            if (lstMultipleSelectionItems.Count == 0)
            {
                return;
            }

            this.GetCurrentView().Document.StartTransaction();
            this.GetCurrentView().BeginUpdate();
            this.lblStatus.Text = "Pasting Selection...";
            MainForm.App.Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            List<NewlyCopiedItem> lstNewItems = new List<NewlyCopiedItem>();

            PastingMultipleSelection = true;
            if (pasteLocation == new PointF(0, 0))
            {
                pasteLocation = new PointF(200, 200);
            }

            for (int x = 0; x < lstMultipleSelectionItems.Count; x++)
            {
                float X;
                float Y;

                BaseItem i = lstMultipleSelectionItems[x].baseItem;
                if (i == null)
                {
                    lstMultipleSelectionItems.Clear();
                    lstMultipleSelectionLinks.Clear();
                    PastingMultipleSelection = false;
                    this.GetCurrentView().EndUpdate();
                    this.GetCurrentView().Document.FinishTransaction("Paste Selection");
                    GetCurrentView().Refresh();

                    this.lblStatus.Text = "Ready";
                    MainForm.App.Cursor = Cursors.Default;
                    Application.DoEvents();
                    return;
                }
                if (x == 0)
                {
                    X = pasteLocation.X;
                    Y = pasteLocation.Y;
                }
                else
                {
                    X = lstMultipleSelectionItems[x].NewLocation.X + lstMultipleSelectionItems[0].NewLocation.X;
                    Y = lstMultipleSelectionItems[x].NewLocation.Y + lstMultipleSelectionItems[0].NewLocation.Y;
                }
                switch (i.WorkflowItemBaseType)
                {
                    case WorkflowItemBaseType.State:
                        {
                            BaseState mState = null;
                            switch (i.WorkflowItemType)
                            {
                                case WorkflowItemType.UserState:
                                    {
                                        mState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateUserState(new PointF(X, Y));
                                        break;
                                    }
                                case WorkflowItemType.CommonState:
                                    {
                                        mState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateCommonState(new PointF(X, Y));
                                        break;
                                    }
                                case WorkflowItemType.SystemState:
                                    {
                                        mState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateSystemState(new PointF(X, Y));
                                        break;
                                    }
                                case WorkflowItemType.SystemDecisionState:
                                    {
                                        mState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateSystemDecisionState(new PointF(X, Y));
                                        break;
                                    }
                                case WorkflowItemType.ArchiveState:
                                    {
                                        mState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateArchiveState(new PointF(X, Y));
                                        break;
                                    }
                            }

                            if (x == 0)
                            {
                                lstMultipleSelectionItems[0].NewLocation = mState.Location;
                            }
                            if (!GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(lstMultipleSelectionItems[x].baseItem))
                            {
                                mState.Name = lstMultipleSelectionItems[x].baseItem.Name;
                            }
                            lstMultipleSelectionItems[x].NewName = mState.Name;
                            lstMultipleSelectionItems[x].NewItem = mState;
                            NewlyCopiedItem mNewItem = new NewlyCopiedItem();
                            mNewItem.NewItem = mState;
                            mNewItem.Location = new PointF(X + (32 - 2.3854F), Y + 32);
                            lstNewItems.Add(mNewItem);
                            lstMultipleSelectionItems[x].baseItem.Copy(mState);
                            for (int y = 0; y < lstMultipleSelectionItems[x].baseItem.AvailableCodeSections.Length; y++)
                            {
                                string codeSection = lstMultipleSelectionItems[x].baseItem.AvailableCodeSections[y];
                                string codeSectionData = lstMultipleSelectionItems[x].baseItem.GetCodeSectionData(codeSection);

                                mState.SetCodeSectionData(codeSection, codeSectionData);
                                string codeDataToReplace = mState.GetCodeSectionData(codeSection);
                                if (lstMultipleSelectionItems[x].OriginalWorkflow != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                {
                                    codeDataToReplace = codeDataToReplace.Replace(X2Generator.FixWorkFlowName(lstMultipleSelectionItems[x].OriginalWorkflow.WorkFlowName), X2Generator.FixWorkFlowName(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName));
                                }
                                if (codeSection.ToLower().Contains("onenter"))
                                {
                                    X2Generator.ReplaceStateEnterHeader(lstMultipleSelectionItems[x].baseItem.Name, mState.Name, ref codeDataToReplace);
                                    mState.SetCodeSectionData(codeSection, codeDataToReplace);
                                }
                                if (codeSection.ToLower().Contains("onexit"))
                                {
                                    X2Generator.ReplaceStateExitHeader(lstMultipleSelectionItems[x].baseItem.Name, mState.Name, ref codeDataToReplace);
                                    mState.SetCodeSectionData(codeSection, codeDataToReplace);
                                }
                                if (codeSection.ToLower().Contains("forward"))
                                {
                                    X2Generator.ReplaceStateAutoForwardHeader(lstMultipleSelectionItems[x].baseItem.Name, mState.Name, ref codeDataToReplace);
                                    mState.SetCodeSectionData(codeSection, codeDataToReplace);
                                }
                                if (codeSection.ToLower().Contains("onreturn"))
                                {
                                    X2Generator.ReplaceReturnHeader(lstMultipleSelectionItems[x].baseItem.Name, mState.Name, ref codeDataToReplace);
                                    mState.SetCodeSectionData(codeSection, codeDataToReplace);
                                }
                            }
                            break;
                        }
                    case WorkflowItemBaseType.Activity:
                        {
                            if (i.WorkflowItemType != WorkflowItemType.None)
                            {
                                BaseActivity mActivity = null;
                                switch (i.WorkflowItemType)
                                {
                                    case WorkflowItemType.UserActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateUserActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                    case WorkflowItemType.ExternalActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateExternalActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                    case WorkflowItemType.ConditionalActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateConditionalActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                    case WorkflowItemType.TimedActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateTimedActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                    case WorkflowItemType.CallWorkFlowActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateCallWorkFlowActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                    case WorkflowItemType.ReturnWorkFlowActivity:
                                        {
                                            mActivity = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateReturnWorkFlowActivity(new PointF(X, Y), null);
                                            break;
                                        }
                                }

                                if (x == 0)
                                {
                                    lstMultipleSelectionItems[0].NewLocation = mActivity.Location;
                                }
                                if (!GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(lstMultipleSelectionItems[x].baseItem))
                                {
                                    mActivity.Name = lstMultipleSelectionItems[x].baseItem.Name;
                                }
                                lstMultipleSelectionItems[x].NewName = mActivity.Name;
                                lstMultipleSelectionItems[x].NewItem = mActivity;
                                NewlyCopiedItem mNewItem = new NewlyCopiedItem();
                                mNewItem.NewItem = mActivity;
                                mNewItem.Location = new PointF(X + (32 - 2.3854F), Y + 32);
                                lstNewItems.Add(mNewItem);
                                lstMultipleSelectionItems[x].baseItem.Copy(mActivity);

                                for (int y = 0; y < lstMultipleSelectionItems[x].baseItem.AvailableCodeSections.Length; y++)
                                {
                                    string codeSection = lstMultipleSelectionItems[x].baseItem.AvailableCodeSections[y];
                                    string codeSectionData = lstMultipleSelectionItems[x].baseItem.GetCodeSectionData(codeSection);

                                    mActivity.SetCodeSectionData(codeSection, codeSectionData);
                                    string codeDataToReplace = mActivity.GetCodeSectionData(codeSection);

                                    if (lstMultipleSelectionItems[x].OriginalWorkflow != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                    {
                                        codeDataToReplace = codeDataToReplace.Replace(X2Generator.FixWorkFlowName(lstMultipleSelectionItems[x].OriginalWorkflow.WorkFlowName), X2Generator.FixWorkFlowName(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName));
                                    }
                                    if (codeSection.ToLower().Contains("onstart"))
                                    {
                                        X2Generator.ReplaceActivityStartHeader(lstMultipleSelectionItems[x].baseItem.Name, mActivity.Name, ref codeDataToReplace);
                                        mActivity.SetCodeSectionData(codeSection, codeDataToReplace);
                                    }
                                    if (codeSection.ToLower().Contains("oncomplete"))
                                    {
                                        X2Generator.ReplaceActivityCompleteHeader(lstMultipleSelectionItems[x].baseItem.Name, mActivity.Name, ref codeDataToReplace);
                                        mActivity.SetCodeSectionData(codeSection, codeDataToReplace);
                                    }
                                    if (codeSection.ToLower().Contains("ontimedactivity"))
                                    {
                                        X2Generator.ReplaceActivityTimerHeader(lstMultipleSelectionItems[x].baseItem.Name, mActivity.Name, ref codeDataToReplace);
                                        mActivity.SetCodeSectionData(codeSection, codeDataToReplace);
                                    }
                                }
                            }
                            break;
                        }

                    case WorkflowItemBaseType.Comment:
                        {
                            Comment mComment = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CreateComment(new PointF(X, Y));
                            lstMultipleSelectionItems[x].baseItem.Copy(mComment);
                            if (x == 0)
                            {
                                lstMultipleSelectionItems[0].NewLocation = mComment.Location;
                            }
                            if (!GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(lstMultipleSelectionItems[x].baseItem))
                            {
                                mComment.Name = lstMultipleSelectionItems[x].baseItem.Name;
                            }
                            lstMultipleSelectionItems[x].NewName = mComment.Name;
                            lstMultipleSelectionItems[x].NewItem = mComment;
                            NewlyCopiedItem mNewItem = new NewlyCopiedItem();
                            mNewItem.NewItem = mComment;
                            mNewItem.Location = new PointF(X + 32, Y + 32);
                            lstNewItems.Add(mNewItem);
                            break;
                        }
                }
            }

            BaseItem newFromNode = null;
            BaseItem newToNode = null;

            CustomLink l = null;
            for (int x = 0; x < MainForm.App.lstMultipleSelectionLinks.Count; x++)
            {
                l = MainForm.App.lstMultipleSelectionLinks[x].customLink;
                BaseItem fromNode = l.FromNode as BaseItem;
                BaseItem toNode = l.ToNode as BaseItem;

                for (int y = 0; y < lstMultipleSelectionItems.Count; y++)
                {
                    if (fromNode is ClapperBoard)
                    {
                        foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                        {
                            if (o is ClapperBoard)
                            {
                                newFromNode = o as BaseItem;
                                break;
                            }
                        }
                    }
                    else if (fromNode.Name == lstMultipleSelectionItems[y].baseItem.Name)
                    {
                        foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                        {
                            BaseItem i = o as BaseItem;
                            if (i != null)
                            {
                                if (i.WorkflowItemType == fromNode.WorkflowItemType)
                                {
                                    if (lstMultipleSelectionItems[y].NewName != null)
                                    {
                                        if (i.Name == lstMultipleSelectionItems[y].NewName)
                                        {
                                            newFromNode = i;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        foreach (GoObject obj in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                        {
                                            if (obj is ClapperBoard)
                                            {
                                                newFromNode = obj as BaseItem;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for (int y = 0; y < lstMultipleSelectionItems.Count; y++)
                {
                    if (toNode.Name == lstMultipleSelectionItems[y].baseItem.Name)
                    {
                        foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                        {
                            BaseItem i = o as BaseItem;
                            if (i != null)
                            {
                                if (i.WorkflowItemType == toNode.WorkflowItemType
                                    && i.Name == lstMultipleSelectionItems[y].NewName)
                                {
                                    newToNode = i;
                                    break;
                                }
                            }
                        }
                    }
                }

                MultiPortNodePort mpnp1 = null;
                MultiPortNodePort mpnp2 = null;
                GoPort holdLeftPort = null;
                GoPort holdRightPort = null;
                foreach (MultiPortNodePort p in newFromNode.Ports)
                {
                    mpnp1 = p;
                    break;
                }
                if (mpnp1 == null)
                {
                    mpnp1 = (MultiPortNodePort)newFromNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                    mpnp1.Size = new SizeF(55, 55);
                    mpnp1.Center = newFromNode.Location;
                    mpnp1.Visible = false;
                }
                holdLeftPort = mpnp1;

                foreach (MultiPortNodePort p in newToNode.Ports)
                {
                    mpnp2 = p;
                    break;
                }
                if (mpnp2 == null)
                {
                    mpnp2 = (MultiPortNodePort)newToNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                    mpnp2.Size = new SizeF(55, 55);
                    mpnp2.Center = newToNode.Location;
                    mpnp2.Visible = false;
                }
                holdRightPort = mpnp2;

                CustomLink newLink = new CustomLink();
                newLink.FromPort = holdLeftPort;
                newLink.ToPort = holdRightPort;

                if (newToNode.WorkflowItemType != WorkflowItemType.ReturnWorkFlowActivity)
                {
                    newLink.ToArrow = true;
                }
                else
                {
                    newLink.FromArrow = true;
                }

                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.InsertBefore(null, newLink);
                NewlyCopiedItem mNewItem = new NewlyCopiedItem();
                mNewItem.NewItem = newLink;
                lstNewItems.Add(mNewItem);
            }

            GetCurrentView().holdSelection.RemoveAllSelectionHandles();
            GetCurrentView().holdSelection.Clear();
            GetCurrentView().Selection.Clear();

            for (int x = 0; x < lstNewItems.Count; x++)
            {
                lstNewItems[x].NewItem.Location = lstNewItems[x].Location;
            }

            for (int x = 0; x < lstNewItems.Count; x++)
            {
                GetCurrentView().Selection.Add(lstNewItems[x].NewItem);
            }
            GetCurrentView().Refresh();

            WorkFlow currentWorkFlow = this.GetCurrentView().Document.CurrentWorkFlow;
            PointF currentWorkFlowLoc = currentWorkFlow.Location;
            if (this.GetCurrentView().Document.CurrentWorkFlow != lstMultipleSelectionItems[0].OriginalWorkflow)
            {
                this.GetCurrentView().Document.SelectWorkFlow(lstMultipleSelectionItems[0].OriginalWorkflow);
            }

            if (lstMultipleSelectionItems[0].OperationType == CutCopyOperationType.Cut)
            {
                for (int x = 0; x < lstMultipleSelectionItems.Count; x++)
                {
                    foreach (GoObject o in this.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        BaseItem i = o as BaseItem;
                        if (i != null)
                        {
                            if (i == lstMultipleSelectionItems[x].baseItem)
                            {
                                switch (i.WorkflowItemBaseType)
                                {
                                    case WorkflowItemBaseType.State:
                                        {
                                            for (int z = 0; z < this.GetCurrentView().Document.CurrentWorkFlow.States.Count; z++)
                                            {
                                                if (this.GetCurrentView().Document.CurrentWorkFlow.States[z] == i)
                                                {
                                                    this.GetCurrentView().Document.CurrentWorkFlow.States.RemoveAt(z);
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case WorkflowItemBaseType.Activity:
                                        {
                                            for (int z = 0; z < this.GetCurrentView().Document.CurrentWorkFlow.Activities.Count; z++)
                                            {
                                                if (this.GetCurrentView().Document.CurrentWorkFlow.Activities[z] == i)
                                                {
                                                    this.GetCurrentView().Document.CurrentWorkFlow.Activities.RemoveAt(z);
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    case WorkflowItemBaseType.Comment:
                                        {
                                            foreach (GoObject obj in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                                            {
                                                BaseItem c = obj as BaseItem;
                                                if (c != null)
                                                {
                                                    if (c == lstMultipleSelectionItems[x].baseItem)
                                                    {
                                                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Remove(o);
                                                        break;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                }
                                lstMultipleSelectionItems[x].baseItem = null;
                                i.Remove();
                                break;
                            }
                        }
                    }
                }
            }

            if (m_BrowserView != null)
            {
                m_BrowserView.PopulateBrowser();
            }

            this.GetCurrentView().Document.SelectWorkFlow(currentWorkFlow);
            currentWorkFlow.Location = currentWorkFlowLoc;

            GoObject mSelItem = null;
            if (this.GetCurrentView().Selection.Count == 1)
            {
                mSelItem = this.GetCurrentView().Selection.Primary;
            }

            for (int x = 0; x < lstMultipleSelectionItems.Count; x++)
            {
                if (lstMultipleSelectionItems[x].OriginalWorkflow == this.GetCurrentView().Document.CurrentWorkFlow
                    && lstMultipleSelectionItems[x].OperationType == CutCopyOperationType.Cut)
                {
                    lstMultipleSelectionItems[x].NewItem.Name = lstMultipleSelectionItems[x].OriginalName;
                }
            }

            //lstMultipleSelectionItems.Clear();
            //lstMultipleSelectionLinks.Clear();
            PastingMultipleSelection = false;

            if (mSelItem != null)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().holdSelection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(mSelItem);
            }

            GetCurrentView().Selection.AddAllSelectionHandles();
            this.GetCurrentView().EndUpdate();
            this.GetCurrentView().Document.FinishTransaction("Paste Selection");
            GetCurrentView().Refresh();

            this.lblStatus.Text = "Ready";
            MainForm.App.Cursor = Cursors.Default;
            Application.DoEvents();
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
                m_CutCopyTarget.DoDelete();
        }

        private void OnFindAndReplaceClick(object sender, EventArgs e)
        {
            FindReplace();
        }

        private void OnSelectAllClick(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
            {
                m_CutCopyTarget.DoSelectAll();
            }
        }

        private void OnBringToFrontClick(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.BringToFront();
            }
        }

        private void OnSendToBackClick(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.SendToBack();
            }
        }

        private void OnViewPropertiesClick(object sender, EventArgs e)
        {
            ShowPropertiesWindow();
        }

        public void ShowPropertiesWindow()
        {
            bool mustShowPropertiesOnTop = false;
            BaseItem holdSelectedItem = null;
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection.Primary != null)
                {
                    holdSelectedItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                }
            }

            unCheckToolStripButtons();

            ProcessView view = GetCurrentView();
            if (view == null)
            {
                return;
            }

            if (m_PropsView == null || mnuProperties.Checked == true)
            {
                m_PropsView = new PropertiesView();
                mnuProperties.Checked = true;
            }
            else
            {
                if (m_PropsView.Visible == false && m_PropsView.IsDisposed == false)
                {
                    mustShowPropertiesOnTop = true;
                }
                if (setShowPropertiesOnTop)
                {
                    m_PropsView = new PropertiesView();
                    mnuProperties.Checked = true;
                    setShowPropertiesOnTop = false;
                }
                else
                {
                    m_PropsView.Visible = false;
                    m_PropsView.Close();
                    mnuProperties.Checked = false;
                }
            }

            if (mnuProperties.Checked == true)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (m_BrowserView != null & m_BrowserView.Visible != false)
                    {
                        m_PropsView.Show(m_BrowserView.Pane, DockAlignment.Bottom, 0.5);
                    }
                    else
                    {
                        m_PropsView.Show(dockPanel, DockState.DockLeft);
                    }
                    MainForm.App.GetCurrentView().Focus();
                    if (holdSelectedItem != null && MainForm.App.GetCurrentView().Selection.Count == 0)
                    {
                        MainForm.App.GetCurrentView().Selection.Add(holdSelectedItem);
                    }
                }
            }
            if (mustShowPropertiesOnTop)
            {
                setShowPropertiesOnTop = true;
                ShowPropertiesWindow();
            }
        }

        private void OnCloseAllWindowsClick(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
        }

        private void OnClickExportAsImage(object sender, EventArgs e)
        {
            SaveFileDialog FS = new SaveFileDialog();
            FS.Filter = "BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpg";
            FS.Title = "Export Process as Image.";
            FS.FilterIndex = 4;
            FS.AddExtension = true;
            ProcessView PV = MainForm.App.GetCurrentView();
            if (PV != null)
            {
                FS.FileName = Path.GetFileNameWithoutExtension(PV.Document.Name);
                if (FS.ShowDialog() == DialogResult.OK)
                {
                    Bitmap BMP = PV.GetBitmapFromCollection(PV.Document);
                    Stream outf = File.OpenWrite(FS.FileName);
                    string ext = Path.GetExtension(FS.FileName);
                    ImageFormat IFmt = ImageFormat.Jpeg;
                    switch (ext.ToLower())
                    {
                        case "bmp":
                            IFmt = ImageFormat.Bmp;
                            break;
                        case "gif":
                            IFmt = ImageFormat.Gif;
                            break;
                        case "png":
                            IFmt = ImageFormat.Png;
                            break;
                        case "jpg":
                            IFmt = ImageFormat.Jpeg;
                            break;
                    }
                    BMP.Save(outf, IFmt);
                    outf.Close();
                }
            }
        }

        #endregion General Toolbar Handlers

        #region Tools Toolbar Handlers

        private void OnPointerClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            Cursor = Cursors.Default;
        }

        private void btnNewMap_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            if (m_CodeView != null)
            {
                m_CodeView.SaveCode();
                m_CodeView.ClearView();
            }
            if (m_SecurityView != null)
            {
                m_SecurityView.ClearView();
            }
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.Document.CreateWorkFlow("new", null);
            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
            {
                if (o is ClapperBoard)
                {
                    PointF floatPoint = new PointF(81.5F, 100.0911F);
                    ClapperBoard mClapper = o as ClapperBoard;
                    mClapper.Position = floatPoint;
                    break;
                }
            }
            this.setStatusBar("Ready");
        }

        private void OnUserStateClick(object sender, EventArgs e)
        {
            // get the active process window
            // get the view
            // set the active item type on the view
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.UserState;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnSystemStateClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.SystemState;
            if (GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnCommonStateClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.CommonState;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void btnReturnWorkflowActivity_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;

            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ReturnWorkFlowActivity;
            if (this.GetCurrentView() != null)
            {
                isDrawingActivity = true;
                this.GetCurrentView().toolBarItemChecked = true;
            }
        }

        private void OnArchiveStateClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ArchiveState;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnUserActivityClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;

            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.UserActivity;
            if (this.GetCurrentView() != null)
            {
                isDrawingActivity = true;
                this.GetCurrentView().toolBarItemChecked = true;
            }
        }

        private void btnTimedActivity_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            isDrawingActivity = true;
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.TimedActivity;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnExternalActivityClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            isDrawingActivity = true;
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ExternalActivity;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnConditionalActivityClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            isDrawingActivity = true;
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ConditionalActivity;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnCommentClick(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.Comment;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void OnCodeViewClicked(object sender, EventArgs e)
        {
            unCheckToolStripButtons();

            ShowCodeView();
        }

        private void OnProcessExplorerClick(object sender, EventArgs e)
        {
            ShowProcessBrowserWindow();
        }

        public void ShowProcessBrowserWindow()
        {
            bool mustShowBrowserOnTop = false;
            unCheckToolStripButtons();

            ProcessView view = GetCurrentView();
            if (view == null)
            {
                return;
            }

            if (m_BrowserView == null | mnuProcessBrowser.Checked == true)
            {
                m_BrowserView = new BrowserView();
                mnuProcessBrowser.Checked = true;
            }
            else
            {
                if (m_BrowserView.Visible == false && m_BrowserView.IsDisposed == false)
                {
                    mustShowBrowserOnTop = true;
                }
                if (setShowBrowserOnTop)
                {
                    m_BrowserView = new BrowserView();
                    mnuProcessBrowser.Checked = true;
                    setShowBrowserOnTop = false;
                }
                else
                {
                    BaseItem holdItem = null;
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        if (MainForm.App.GetCurrentView().Selection.Count > 0)
                        {
                            holdItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                        }
                    }
                    m_BrowserView.Visible = false;
                    m_BrowserView.Close();
                    mnuProcessBrowser.Checked = false;
                    if (holdItem != null)
                    {
                        MainForm.App.GetCurrentView().Selection.Clear();
                        MainForm.App.GetCurrentView().Selection.Add(holdItem);
                    }
                }
            }

            if (mnuProcessBrowser.Checked == true)
            {
                if (m_PropsView != null && m_PropsView.Visible != false)
                {
                    m_BrowserView.Show(m_PropsView.Pane, DockAlignment.Top, 0.5);
                }
                else
                {
                    m_BrowserView.Show(dockPanel, DockState.DockLeft);
                }

                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().Focus();
                }
            }
            if (mustShowBrowserOnTop)
            {
                setShowBrowserOnTop = true;
                ShowProcessBrowserWindow();
            }
        }

        private void OnBuildClicked(object sender, EventArgs e)
        {
            BuildCode(true);
        }

        public void BuildCode(bool showMessages)
        {
            this.Cursor = Cursors.WaitCursor;
            setStatusBar("Compiling...");

            // save current process first
            if (SaveProcess(false) == false)
            {
                MessageBox.Show("An error has occurred during the save - map cannot be compiled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                setStatusBar("Error(s) Encountered");
                this.Cursor = Cursors.Default;
            }
            else
            {
                // lets increment the build coune tfor this map.
                this.GetCurrentView().Document.BuildCount++;

                isCompiling = true;
                if (m_CodeView != null)
                {
                    m_CodeView.syntaxEditor.Document.HeaderText = "";
                    m_CodeView.syntaxEditor.Document.FooterText = "";
                }
                if (m_CodeView != null)
                {
                    if (m_CodeView.syntaxEditor.Document != null)
                    {
                        if (m_CodeView.syntaxEditor.Document.SpanIndicatorLayers.Count > 0)
                        {
                            for (int x = 0; x < m_CodeView.syntaxEditor.Document.SpanIndicatorLayers.Count; x++)
                            {
                                m_CodeView.syntaxEditor.Document.SpanIndicatorLayers[x].Clear();
                            }
                        }
                    }
                    m_CodeView.SaveCode();
                }

                // we only want to do the nuget package resolver if this is the first build of the map
                bool resolveNuGetPackages = this.GetCurrentView().Document.BuildCount == 1 ? true : false;
                    
                CompilerResults Results = this.Compile(resolveNuGetPackages);

                if (Results != null)
                {
                    if (Results.Errors.HasErrors)
                    {
                        this.Cursor = Cursors.Default;

                        if (showMessages)
                        {
                            MessageBox.Show("X2 Process Compiled with " + Results.Errors.Count.ToString() + " error(s).", "X2 Process Compiler Result.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            setStatusBar(Results.Errors.Count + " Error(s) Encountered");
                        }
                        MainForm.App.ShowErrorsView(Results.Errors);
                    }
                    else
                    {
                        //My Code
                        //---------------------
                        bool bDuplicates = false;
                        ArrayList arrActivityNodes = new ArrayList();
                        StringBuilder strDuplicates = new StringBuilder();
                        //Loop through each workflow map
                        foreach (WorkFlow wflow in MainForm.App.GetCurrentView().Document.WorkFlows)
                        {
                            //Loop through each state in the workflow map
                            foreach (BaseState bState in wflow.States)
                            {
                                if (!(bState is CommonState))
                                {
                                    arrActivityNodes.Clear();
                                    foreach (CustomLink lnk in bState.Links)
                                    {
                                        BaseActivity myParent = lnk.ToPort.Node as BaseActivity;
                                        if (myParent != null)
                                        {
                                            arrActivityNodes.Add(myParent);
                                        }
                                    }

                                    //Store all duplicate activities in an arraylist for checking so that
                                    //it wont be added again
                                    ArrayList arrDuplicates = new ArrayList();
                                    //find any duplicate priorities
                                    foreach (BaseActivity bActivity1 in arrActivityNodes)
                                    {
                                        foreach (BaseActivity bActivity2 in arrActivityNodes)
                                        {
                                            if (bActivity1 != bActivity2 && (bActivity1.Priority == bActivity2.Priority) && !(bActivity1 is ReturnWorkflowActivity) && !(bActivity2 is ReturnWorkflowActivity))
                                            {
                                                //Loop through arraylist to check if the activities have aready been added
                                                //If not, then add them to the list
                                                bool bFound = false;
                                                foreach (BaseActivity bact in arrDuplicates)
                                                {
                                                    if (bActivity1 == bact || bActivity2 == bact)
                                                    {
                                                        bFound = true;
                                                        break;
                                                    }
                                                }
                                                if (!bFound)
                                                {
                                                    arrDuplicates.Add(bActivity1);
                                                    arrDuplicates.Add(bActivity2);
                                                    bDuplicates = true;
                                                    strDuplicates.Append("Duplicate Priority Error(s) - Workflow: " + wflow.WorkFlowName + "|Activities:" + bActivity1.Name + " and " + bActivity2.Name + " have duplicate priorities. \n");
                                                    setStatusBar("Error(s) Encountered");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        this.Cursor = Cursors.Default;

                        //---------------------
                        if (!bDuplicates)
                        {
                            setStatusBar("Compilation Successful - 0 Errors");
                        }
                        else
                        {
                            MessageBox.Show(strDuplicates.ToString(), "X2 Process Compiler Result.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (m_CodeView != null)
                        {
                            if (m_CodeView.syntaxEditor.Document != null)
                            {
                                m_CodeView.syntaxEditor.Document.LineIndicators.Clear();
                            }
                        }
                        if (m_ErrorsView != null)
                        {
                            m_ErrorsView.listViewErrors.Items.Clear();
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;

                    MessageBox.Show("X2 Process Compiled with error(s).", "X2 Process Compiler Result.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    setStatusBar("Error(s) Encountered");
                }
                isCompiling = false;

                if (m_CodeView != null)
                {
                    m_CodeView.RefreshSyntaxEditorHeader();
                }
            }
        }

        private CompilerResults Compile(bool resolveNuGetPackages)
        {
            this.Cursor = Cursors.WaitCursor;

            using (Compiler compiler = new Compiler())
            {
                CompilerResults results = null;

                // get the x2map name              
                string x2Map = MainForm.App.GetCurrentView().Document.Location;

                if (!String.IsNullOrEmpty(x2Map))
                {
                    // get the folder where the nuget binaries are to be loaded
                    string binariesDirectory = Path.Combine(Path.GetDirectoryName(x2Map), "Binaries");

                    if (resolveNuGetPackages)
                    {
                        setStatusBar("Resolving NuGet Packages...");

                        var packageResolver = new PackageResolver(new List<IPackageRepository>{
				            PackageRepositoryFactory.Default.CreateRepository(SAHL.X2Designer.Properties.Settings.Default.OfficialNuGetUrl),
				            PackageRepositoryFactory.Default.CreateRepository(SAHL.X2Designer.Properties.Settings.Default.SAHLNuGetUrl),
			            });
                        packageResolver.ResolvePackages(
                            packagesToUpdate: String.Empty,
                            workflowMapLocation: x2Map,
                            binariesLocation: binariesDirectory);
                    }

                    setStatusBar("Compiling...");

                    results = compiler.Compile(x2Map, new CompilerOptions(binariesDirectory, binariesDirectory, true));

                    X2Generator.CurrentCode = compiler.LastCompiledSourceCode;

                }

                this.Cursor = Cursors.Default;

                return results;
            }
        }

        public void unCheckToolStripButtons()
        {
            if (MainForm.App.documentIsBeingOpened == false)
            {
                if (toolStripTools != null)
                {
                    for (int x = 0; x < toolStripTools.Items.Count; x++)
                    {
                        ToolStripButton Btn = toolStripTools.Items[x] as ToolStripButton;
                        if (Btn != null)
                        {
                            Btn.Checked = false;
                        }
                    }
                    if (onActivityItemUnChecked != null)
                    {
                        onActivityItemUnChecked(null);
                    }
                }
                btnSelect.Checked = true;
                btnPublish.Enabled = true;
                Cursor = Cursors.Default;
                if (this.GetCurrentView() != null)
                {
                    this.GetCurrentView().toolBarItemChecked = false;
                }

                this.setStatusBar("Ready");
            }
        }

        public void unCheckToolStripButtons(string exceptButton)
        {
            if (toolStripTools != null)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().lastSelectedItem = null;
                }
                for (int x = 0; x < toolStripTools.Items.Count; x++)
                {
                    ToolStripButton Btn = toolStripTools.Items[x] as ToolStripButton;
                    if (Btn != null && Btn.Text != exceptButton)
                    {
                        Btn.Checked = false;
                    }
                    isDrawingActivity = false;
                }
            }
        }

        public void enableToolStripTools()
        {
            toolStripTools.Enabled = true;
            toolStripBuild.Enabled = true;

            toolStripButtonBuildNoNuGet.Enabled = true;
            btnManageUsingStatements.Enabled = true;
            btnPublish.Enabled = true;
            toolStripButtonResetCode.Enabled = true;
            btnManageNuGetPackges.Enabled = true;

            mnuItemEdit.Enabled = true;
            mnuItemClose.Enabled = true;
            mnuItemCloseAll.Enabled = true;
            mnuItemCopy.Enabled = true;
            mnuItemCut.Enabled = true;
            mnuItemDelete.Enabled = true;
            mnuItemFindReplace.Enabled = true;
            mnuItemPaste.Enabled = true;
            mnuItemPrint.Enabled = true;
            mnuItemPublish.Enabled = true;
            mnuItemPrintPreview.Enabled = true;
            mnuItemPublish.Enabled = true;
            mnuItemRedo.Enabled = true;
            mnuItemRetrieve.Enabled = true;
            mnuItemSave.Enabled = true;
            mnuItemSaveAs.Enabled = true;
            mnuItemSelectAll.Enabled = true;
            mnuItemSendToBack.Enabled = true;
            mnuItemUndo.Enabled = true;
            mnuItemWindow.Enabled = true;
            mnuItemView.Enabled = true;
        }

        public void disableToolStripStandard()
        {
            for (int x = 0; x < toolStripStandard.Items.Count; x++)
            {
                if (toolStripStandard.Items[x].Name != btnRetrieve.Name)
                {
                    toolStripStandard.Items[x].Enabled = false;
                }
            }
            btnNewProcess.Enabled = true;
            btnOpenProcess.Enabled = true;
            btnPublish.Enabled = true;
        }

        public void disableToolStripTools()
        {
            toolStripTools.Enabled = false;
            toolStripButtonBuildNoNuGet.Enabled = false;
            btnManageUsingStatements.Enabled = false;
            toolStripButtonResetCode.Enabled = false;
            btnManageNuGetPackges.Enabled = false;
            mnuItemEdit.Enabled = false;
            mnuItemClose.Enabled = false;
            mnuItemCloseAll.Enabled = false;
            mnuItemCopy.Enabled = false;
            mnuItemCut.Enabled = false;
            mnuItemDelete.Enabled = false;
            mnuItemFindReplace.Enabled = false;
            mnuItemPaste.Enabled = false;
            mnuItemPrint.Enabled = false;
            mnuItemPrintPreview.Enabled = false;
            mnuItemRedo.Enabled = false;
            mnuItemSave.Enabled = false;
            mnuItemSaveAs.Enabled = false;
            mnuItemSelectAll.Enabled = false;
            mnuItemSendToBack.Enabled = false;
            mnuItemUndo.Enabled = false;
            mnuItemWindow.Enabled = false;
            mnuItemView.Enabled = false;
        }

        public void enableToolStripStandard()
        {
            for (int x = 0; x < toolStripStandard.Items.Count; x++)
            {
                toolStripStandard.Items[x].Enabled = true;
            }
        }

        private void btnCustomForms_Click(object sender, EventArgs e)
        {
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().StartTransaction();
                frmManageCustomForms mCustForm = new frmManageCustomForms(this.GetCurrentView().Document);
                mCustForm.TopMost = true;
                mCustForm.ShowDialog();
                mCustForm.Dispose();
                if (OnGeneralCustomFormClosed != null)
                {
                    OnGeneralCustomFormClosed(GeneralCustomFormType.CustomForm);
                }
                this.GetCurrentView().FinishTransaction("Modify Custom Forms");
            }
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            this.GetCurrentView().StartTransaction();
            frmManageRoles mFrmManageRoles = new frmManageRoles(this.GetCurrentView().Document);
            mFrmManageRoles.TopMost = true;
            mFrmManageRoles.ShowDialog();
            mFrmManageRoles.Dispose();
            if (OnGeneralCustomFormClosed != null)
            {
                OnGeneralCustomFormClosed(GeneralCustomFormType.Role);
            }
            this.GetCurrentView().FinishTransaction("Modify External Activities");
        }

        #endregion Tools Toolbar Handlers

        #region Menu Items

        private void toolStripStandard_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            unCheckToolStripButtons();
        }

        public void menuPropertiesChecked(bool check)
        {
            mnuProperties.Checked = check;
        }

        public void menuBrowserChecked(bool check)
        {
            mnuProcessBrowser.Checked = check;
        }

        public void menuCodeChecked(bool check)
        {
            mnuItemCodeView.Checked = check;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.ZoomIn();
                cbxZoom.SelectedIndex = -1;
            }
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.ZoomOut();
                cbxZoom.SelectedIndex = -1;
            }
        }

        private void btnZoomToFit_Click(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                view.ZoomToFit();
                cbxZoom.Text = "Fit";
            }
        }

        private void cbxZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProcessView view = GetCurrentView();
            if (view != null)
            {
                if (cbxZoom.Text != "Fit")
                {
                    if (cbxZoom.SelectedIndex != -1)
                    {
                        view.ZoomNormal();
                        view.ZoomToScale(int.Parse(cbxZoom.Text.Remove(cbxZoom.Text.Length - 1)));
                    }
                }
                else
                {
                    view.ZoomToFit();
                    cbxZoom.Text = "Fit";
                }
                view.m_ZoomValue = cbxZoom.Text;
            }
        }

        private void mnuWindows_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void createNewProcessForm()
        {
            ProcessView mView = null;

            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().Document.StartTransaction();
                mView = this.GetCurrentView();
                if (m_CodeView != null)
                {
                    m_CodeView.AttachCode();
                }
            }

            newDocumentBeingCreated = true;
            string holdProcessName = GetNextProcessName();
            ProcessForm canvas = new ProcessForm("new", holdProcessName);
            canvas.View.StartTransaction();
            canvas.View.NewLinkPrototype = new CustomLink();
            canvas.MdiParent = this;
            canvas.Text = holdProcessName;
            canvas.Show();
            newDocumentBeingCreated = false;
            setStatusBar("Ready");
            canvas.View.FinishTransaction("new canvas");

            MainForm.App.GetCurrentView().Document.UndoManager = new GoUndoManager();

            MainForm.App.GetCurrentView().Document.IsModified = false;
            MainForm.App.GetCurrentView().UpdateTitle();

            if (mView != null)
            {
                this.GetCurrentView().Document.FinishTransaction("Create Document");
            }
        }

        private void toolStripTools_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.ToString().Contains("Activity"))
            {
                onActivityItemChecked(sender);
            }
        }

        private void btnExternalActivities_Click(object sender, EventArgs e)
        {
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().StartTransaction();

                Forms.frmManageExternalActivities fExternalActivities = new SAHL.X2Designer.Forms.frmManageExternalActivities(this.GetCurrentView().Document);
                fExternalActivities.TopMost = true;
                fExternalActivities.ShowDialog();
                fExternalActivities.Dispose();
                if (OnGeneralCustomFormClosed != null)
                {
                    OnGeneralCustomFormClosed(GeneralCustomFormType.ExternalActivity);
                }
                this.GetCurrentView().FinishTransaction("Modify External Activities");
            }
        }

        private void btnCustomVariables_Click(object sender, EventArgs e)
        {
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().StartTransaction();
                Forms.frmManageCustomVariables fCustomVar = new SAHL.X2Designer.Forms.frmManageCustomVariables(this.GetCurrentView().Document);
                fCustomVar.TopMost = true;
                fCustomVar.ShowDialog();
                fCustomVar.Dispose();

                if (OnGeneralCustomFormClosed != null)
                {
                    OnGeneralCustomFormClosed(GeneralCustomFormType.CustomVariable);
                }

                this.GetCurrentView().FinishTransaction("Modify Custom Variables");
            }
        }

        private void cbxZoom_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
        }

        private void mnuItemMoveLink_Click(object sender, EventArgs e)
        {
        }

        public void ShowErrorsView(CompilerErrorCollection Errors)
        {
            if (m_ErrorsView == null || m_ErrorsView.IsDisposed)
            {
                m_ErrorsView = new CodeErrors();
                m_ErrorsView.Show(dockPanel, WeifenLuo.WinFormsUI.DockState.DockBottom);
            }
            else
            {
                m_ErrorsView.Activate();
            }

            m_ErrorsView.ShowErrors(Errors);
        }

        public void ShowCodeViewWindow(ErrorSource Source, bool DisplayingErrors)
        {
            if (DisplayingErrors)
            {
                if (m_CodeView == null)
                {
                    ShowCodeView();
                }
            }
            else
            {
                ShowCodeView();
            }
            if (m_CodeView != null && DisplayingErrors)
            {
                m_CodeView.handledClose = false;
                m_CodeView.Activate();
                m_CodeView.SetErrorPosition(Source);
            }
        }

        public void ShowCodeView()
        {
            try
            {
                unCheckToolStripButtons();

                ProcessView view = GetCurrentView();
                if (view == null)
                {
                    return;
                }

                if (m_CodeView == null || mnuItemCodeView.Checked == true)
                {
                    if (m_CodeView == null)
                    {
                        m_CodeView = new CodeView();
                    }
                    else
                    {
                        m_CodeView.Close();
                        m_CodeView.Dispose();

                        m_CodeView = new CodeView();
                    }

                    mnuProperties.Checked = true;
                }
                else
                {
                    if (m_CodeView.IsDisposed == false)
                    {
                        m_CodeView.Visible = false;
                        m_CodeView.Close();
                    }
                    m_CodeView = null;
                    mnuItemCodeView.Checked = false;
                }

                if (mnuItemCodeView.Checked == true)
                {
                    m_CodeView.ClearView();

                    m_CodeView.Show(dockPanel, WeifenLuo.WinFormsUI.DockState.DockBottom);
                    mnuItemCodeView.Checked = true;
                }

                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().Focus();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        public void FindReplace()
        {
            if (m_FindReplaceView == null || m_FindReplaceView.Visible != true)
            {
                frmFindReplace mFindReplace = new frmFindReplace();
                m_FindReplaceView = mFindReplace;
                mFindReplace.Show();
            }
        }

        internal void ProcessViewActivated()
        {
            enableToolStripTools();
            enableToolStripStandard();

            if (GetCurrentView() != null)
            {
                cbxZoom.Text = GetCurrentView().m_ZoomValue;
                unCheckToolStripButtons();
            }

            if (OnProcessViewActivated != null)
            {
                for (int x = 0; x < GetCurrentView().Document.WorkFlows.Length; x++)
                {
                    if (this.GetCurrentView().Document.WorkFlows[x].hashTableForm != null)
                    {
                        this.GetCurrentView().Document.WorkFlows[x].hashTableForm.Clear();
                    }
                    if (this.GetCurrentView().Document.WorkFlows[x].hashTableRoles != null)
                    {
                        this.GetCurrentView().Document.WorkFlows[x].hashTableRoles.Clear();
                    }
                    if (this.GetCurrentView().Document.WorkFlows[x].hashTableVar != null)
                    {
                        this.GetCurrentView().Document.WorkFlows[x].hashTableVar.Clear();
                    }
                }
                this.GetCurrentView().Selection.Clear();
                OnProcessViewActivated(this.GetCurrentView());
                this.GetCurrentView().Document.UndoManager = new GoUndoManager();
                if (this.GetCurrentView().Document.CurrentWorkFlow != null)
                {
                    foreach (GoObject o in this.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        if (o is ClapperBoard)
                        {
                            break;
                        }
                    }
                }
            }
        }

        internal void ProcessViewDeactivated()
        {
            disableToolStripTools();
            disableToolStripStandard();

            if (OnProcessViewDeactivated != null)
                OnProcessViewDeactivated(this.GetCurrentView());

            if (this.MdiChildren.Length == 0)
            {
                toolStripStandard.Enabled = false;
            }
        }

        private void mnuItemUndo_Click(object sender, EventArgs e)
        {
            if (m_CutCopyTarget != null)
            {
                m_CutCopyTarget.DoUndo();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainFormCloseCancel)
            {
                e.Cancel = true;
                mainFormIsClosing = false;
            }
            else
            {
                mruMenu.SaveToRegistry();
            }
        }

        #endregion Menu Items

        #region Misc

        private string GetNextProcessName()
        {
            Form[] Children = this.MdiChildren;
            int NextId = Children.Length;
            bool ValidId = false;

            while (!ValidId)
            {
                bool Found = false;
                for (int i = 0; i < Children.Length; i++)
                {
                    if (Children[i] is ProcessForm)
                    {
                        if (Children[i].Name == "Process" + NextId.ToString())
                        {
                            Found = true;
                            break;
                        }
                    }
                }
                if (!Found)
                    ValidId = true;
                else
                    NextId++;
            }

            return "Process" + NextId;
        }

        internal void SaveCodeView()
        {
            if (m_CodeView != null)
                m_CodeView.SaveCode();
        }

        public void FireRoleItemSelectedEvent(RolesCollectionItem roleItem)
        {
            if (OnRoleItemSelected != null)
            {
                OnRoleItemSelected(roleItem);
            }
        }

        public void FireCustomVariableItemSelectedEvent(CustomVariableItem varItem)
        {
            if (OnCustomVariableItemSelected != null)
            {
                OnCustomVariableItemSelected(varItem);
            }
        }

        public void FireCustomFormItemSelectedEvent(CustomFormItem frmItem)
        {
            if (OnCustomFormItemSelected != null)
                OnCustomFormItemSelected(frmItem);
        }

        public void FireOnStageTransitionMessageChangedEvent(PropertyType propType)
        {
            if (OnStageTransitionMessageChanged != null)
                OnStageTransitionMessageChanged(propType);
        }

        public ProcessView GetCurrentView()
        {
            if (this.ActiveMdiChild != null && (this.ActiveMdiChild is ProcessForm))
                return ((ProcessForm)this.ActiveMdiChild).View as ProcessView;
            else
                return null;
        }

        public string GetPathAndFileName(string name, bool WantBuildLoactaion)
        {
            if (lstMainReferences.Contains(name) && !WantBuildLoactaion)
                return Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Path.GetFileName(name);
            else
                return Path.GetDirectoryName(Application.ExecutablePath) + "\\Build\\" + Path.GetFileName(name);
        }

        public DialogResult GetBusinessStageItems(string ConnString)
        {
            try
            {
                if (GetCurrentView() == null)
                    return DialogResult.OK;
                if (GetCurrentView().Document == null)
                    return DialogResult.OK;
                if (GetCurrentView().Document.BusinessStages.Count > 0)
                    GetCurrentView().Document.BusinessStages.Clear();
                if ((MainForm.App.GetCurrentView().Document.BusinessStageConnectionString.Length == 0) || ConnString == "")
                {
                    if (ConnString.Length < 1)
                    {
                        ConnectionForm CF = new ConnectionForm();
                        if (Helpers.ShowX2ConnForm(CF, false))
                        {
                            MainForm.App.GetCurrentView().Document.BusinessStageConnectionString = CF.ConnectionString;
                        }
                        else
                            return DialogResult.Cancel;
                    }
                    else
                    {
                        MainForm.App.GetCurrentView().Document.BusinessStageConnectionString = ConnString;
                    }
                }
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(MainForm.App.GetCurrentView().Document.BusinessStageConnectionString);
                cmd.Connection.Open();
                cmd.CommandText = "select sgd.Description, SD.Description, sdsdk.StageDefinitionStageDefinitionGroupKey "
                            + "from StageDefinition SD  "
                            + "join StageDefinitionStageDefinitionGroup sdsdk "
                            + "on sdsdk.StageDefinitionKey = sd.StageDefinitionKey "
                            + "join StageDefinitionGroup sgd "
                            + "on sgd.StageDefinitionGroupKey=sdsdk.StageDefinitionGroupKey "
                            + "where SD.GeneralStatusKey = 1 and sd.IsComposite = 0 "
                            + "order by sgd.Description,SD.Description;";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        BusinessStageItem bi = new BusinessStageItem();
                        bi.SDSDGKey = reader[2].ToString();
                        bi.DefinitionGroupDescription = reader[0].ToString();
                        bi.DefinitionDescription = reader[1].ToString();
                        GetCurrentView().Document.BusinessStages.Add(bi);
                    }
                }
                reader.Close();
                return TestBusinessStageTransitions();
            }
            catch
            {
                MessageBox.Show("GetBusinessStageItems : Could not connect! Please ensure database connection is correct.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.App.GetCurrentView().Document.BusinessStageConnectionString = "";
                return TestBusinessStageTransitions();
            }
        }

        public DialogResult TestBusinessStageTransitions()
        {
            List<InvalidStageTransitionItem> lstInvalidStageTransitions = new List<InvalidStageTransitionItem>();
            foreach (WorkFlow w in this.GetCurrentView().Document.WorkFlows)
            {
                foreach (BaseActivity a in w.Activities)
                {
                    IBusinessStageTransitions busItem = a as IBusinessStageTransitions;
                    if (busItem != null)
                    {
                        for (int x = 0; x < busItem.BusinessStageTransitions.Count; x++)
                        {
                            bool found = false;
                            for (int y = 0; y < this.GetCurrentView().Document.BusinessStages.Count; y++)
                            {
                                // TRAC #14530 - Stage Tranisitions should rely on key only
                                if (busItem.BusinessStageTransitions[x].SDSDGKey == this.GetCurrentView().Document.BusinessStages[y].SDSDGKey)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                InvalidStageTransitionItem item = new InvalidStageTransitionItem();
                                item.SDSDGKey = busItem.BusinessStageTransitions[x].SDSDGKey;
                                BaseItem bi = busItem as BaseItem;
                                item.ActivityName = bi.Name;
                                item.workFlow = w;
                                item.DefinitionGroupDescription = busItem.BusinessStageTransitions[x].DefinitionGroupDescription;
                                item.DefinitionDescription = busItem.BusinessStageTransitions[x].DefinitionDescription;
                                lstInvalidStageTransitions.Add(item);
                            }
                        }
                    }
                }
            }
            if (lstInvalidStageTransitions.Count > 0)
            {
                frmInvalidBusinessStageTransitionWarning mFrm = new frmInvalidBusinessStageTransitionWarning(lstInvalidStageTransitions);
                DialogResult res = mFrm.ShowDialog();
                mFrm.Dispose();
                return res;
            }
            else
            {
                return DialogResult.OK;
            }
        }

        #endregion Misc

        # region MRU Files

        private void menuClearRegistryOnExit_Click(object sender, EventArgs e)
        {
        }

        private void OnMruFile(int number, String filename)
        {
            mruMenu.SetFirstFile(number);
            if (File.Exists(filename))
                OpenDocument(filename);
            else
            {
                MessageBox.Show("The file '" + filename + "'cannot be opened and will be removed from the Recent list(s)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mruMenu.RemoveFile(number);
            }
        }

        # endregion

        private void mnuItemHelp_Click(object sender, EventArgs e)
        {
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            AboutBox ABox = new AboutBox();
            ABox.ShowDialog();
            ABox.Dispose();
        }

        private void btnSystemDecisionState_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.SystemDecisionState;
            if (GetCurrentView() != null)
                this.GetCurrentView().toolBarItemChecked = true;

            this.setStatusBar("Ready");
        }

        private void btnWorkFlowActivity_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            isDrawingActivity = true;
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.CallWorkFlowActivity;
            if (this.GetCurrentView() != null)
                this.GetCurrentView().toolBarItemChecked = true;

            this.setStatusBar("Ready");
        }

        private void OnResetCodeSections_Click(object sender, EventArgs e)
        {
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            if (processForm != null)
            {
                if (processForm.View != null)
                {
                    // activities
                    for (int i = 0; i < processForm.View.Document.CurrentWorkFlow.Activities.Count; i++)
                    {
                        string[] CodeSecs = processForm.View.Document.CurrentWorkFlow.Activities[i].AvailableCodeSections;

                        for (int k = 0; k < CodeSecs.Length; k++)
                        {
                            processForm.View.Document.CurrentWorkFlow.Activities[i].SetCodeSectionData(CodeSecs[k], "");
                        }
                    }

                    // states
                    for (int i = 0; i < processForm.View.Document.CurrentWorkFlow.States.Count; i++)
                    {
                        string[] CodeSecs = processForm.View.Document.CurrentWorkFlow.States[i].AvailableCodeSections;

                        for (int k = 0; k < CodeSecs.Length; k++)
                        {
                            processForm.View.Document.CurrentWorkFlow.States[i].SetCodeSectionData(CodeSecs[k], "");
                        }
                    }
                    // roles
                    for (int i = 0; i < processForm.View.Document.Roles.Count; i++)
                    {
                        if (processForm.View.Document.Roles[i].WorkFlowItem == processForm.View.Document.CurrentWorkFlow)
                        {
                            string[] CodeSecs = processForm.View.Document.Roles[i].AvailableCodeSections;

                            for (int k = 0; k < CodeSecs.Length; k++)
                            {
                                processForm.View.Document.Roles[i].SetCodeSectionData(CodeSecs[k], "");
                            }
                        }
                    }
                }
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void loadDocumentFromFile(string FileName)
        {
            try
            {
                OpenDocument(FileName);
                GetCurrentView().Document.CurrentWorkFlow.ComputeBorder();
            }
            catch (Exception err)
            {
                MessageBox.Show("An error has occurred during the loading of the document!\r\n\r\n" + err.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionPolicy.HandleException(err, "X2Designer");
            }
        }

        private void CreateActivityOutputForWorkFlow(WorkFlow wFlow, StreamWriter fStream)
        {
            fStream.WriteLine("   Activities");
            fStream.WriteLine("   __________");

            List<UserActivity> lstActivities = new List<UserActivity>();
            foreach (BaseActivity b in wFlow.Activities)
            {
                if (b is UserActivity)
                {
                    UserActivity mActivity = b as UserActivity;
                    lstActivities.Add(mActivity);
                }
            }

            lstActivities.Sort();

            for (int y = 0; y < lstActivities.Count; y++)
            {
                fStream.WriteLine(" ");
                fStream.WriteLine(" ");
                fStream.WriteLine("    " + lstActivities[y].Name + " (" + lstActivities[y].WorkflowItemType.ToString() + ")");
                fStream.WriteLine(" ");
                fStream.WriteLine("              Access");
                fStream.WriteLine("              ______");
                List<string> lstAccess = new List<string>();
                for (int z = 0; z < lstActivities[y].Access.Count; z++)
                {
                    if (lstActivities[y].Access[z].IsChecked)
                    {
                        lstAccess.Add(lstActivities[y].Access[z].RoleItem.Name);
                    }
                }
                lstAccess.Sort();

                for (int z = 0; z < lstAccess.Count; z++)
                {
                    fStream.WriteLine("              " + lstAccess[z]);
                }
            }
        }

        private void CreateStateOutputForWorkFlow(WorkFlow wFlow, StreamWriter fStream)
        {
            fStream.WriteLine("   States");
            fStream.WriteLine("   ______");
            fStream.WriteLine(" ");
            List<BaseStateWithLists> lstStates = new List<BaseStateWithLists>();
            foreach (BaseState mState in wFlow.States)
            {
                if (mState is BaseStateWithLists)
                {
                    BaseStateWithLists mListState = mState as BaseStateWithLists;
                    lstStates.Add(mListState);
                }
            }
            lstStates.Sort();

            foreach (BaseStateWithLists state in lstStates)
            {
                fStream.WriteLine(" ");
                fStream.WriteLine("    " + state.Name + " (" + state.WorkflowItemType.ToString() + ")");
                fStream.WriteLine(" ");
                fStream.WriteLine("           TrackList");
                fStream.WriteLine("           _________");

                List<string> lstTrackList = new List<string>();

                for (int z = 0; z < state.TrackList.Count; z++)
                {
                    if (state.TrackList[z].IsChecked)
                    {
                        lstTrackList.Add(state.TrackList[z].RoleItem.Name);
                    }
                }
                lstTrackList.Sort();
                for (int z = 0; z < lstTrackList.Count; z++)
                {
                    fStream.WriteLine("           " + lstTrackList[z]);
                }

                fStream.WriteLine(" ");

                fStream.WriteLine("           WorkList");
                fStream.WriteLine("           ________");

                List<string> lstWorkList = new List<string>();
                for (int z = 0; z < state.WorkList.Count; z++)
                {
                    if (state.WorkList[z].IsChecked)
                    {
                        lstWorkList.Add(state.WorkList[z].RoleItem.Name);
                    }
                }
                lstWorkList.Sort();
                for (int z = 0; z < lstWorkList.Count; z++)
                {
                    fStream.WriteLine("           " + lstWorkList[z]);
                }
                fStream.WriteLine(" ");
            }
        }

        private void CreateCommonStateOutputForWorkFlow(WorkFlow wFlow, StreamWriter fStream)
        {
            fStream.WriteLine(" ");
            fStream.WriteLine("   Common States");
            fStream.WriteLine("   _____________");
            fStream.WriteLine(" ");
            List<CommonState> lstStates = new List<CommonState>();
            foreach (BaseState mState in wFlow.States)
            {
                if (mState is CommonState)
                {
                    CommonState mCommonState = mState as CommonState;
                    lstStates.Add(mCommonState);
                }
            }
            lstStates.Sort();

            foreach (CommonState state in lstStates)
            {
                fStream.WriteLine(" ");
                fStream.WriteLine("    " + state.Name);
                fStream.WriteLine(" ");
                fStream.WriteLine("           Applies To");
                fStream.WriteLine("           _________");

                List<string> lstAppliesTo = new List<string>();

                for (int z = 0; z < state.AppliesTo.Count; z++)
                {
                    lstAppliesTo.Add(state.AppliesTo[z].Name);
                }
                lstAppliesTo.Sort();
                for (int z = 0; z < lstAppliesTo.Count; z++)
                {
                    fStream.WriteLine("           " + lstAppliesTo[z]);
                }

                fStream.WriteLine(" ");
            }
        }

        private void mnuItemSecurityView_Click(object sender, EventArgs e)
        {
            ShowSecurityWindow();
        }

        public void ShowSecurityWindow()
        {
            unCheckToolStripButtons();

            ProcessView view = GetCurrentView();
            if (view == null)
            {
                return;
            }

            if (m_SecurityView == null | mnuItemSecurityView.Checked == false)
            {
                m_SecurityView = new SecurityView();
                mnuItemSecurityView.Checked = true;
            }
            else
            {
                BaseItem holdItem = null;
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().Selection.Count > 0)
                    {
                        holdItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                    }
                }
                m_SecurityView.Close();
                mnuItemSecurityView.Checked = false;
                if (holdItem != null)
                {
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(holdItem);
                }
            }

            if (mnuItemSecurityView.Checked == true)
            {
                m_SecurityView.Show(dockPanel, new Rectangle(1000, 200, 200, 240));
                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().Focus();
                }
            }
        }

        private void ShowBusinessStageTransitionsView()
        {
            unCheckToolStripButtons();

            ProcessView view = GetCurrentView();
            if (view == null)
            {
                return;
            }

            if (m_BusinessStageTransitionsView == null || mnuBusinessStageTransitions.Checked == false)
            {
                m_BusinessStageTransitionsView = new BusinessStageTransitionsView();
                mnuBusinessStageTransitions.Checked = true;
            }
            else
            {
                BaseItem holdItem = null;
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().Selection.Count > 0)
                    {
                        holdItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                    }
                }
                m_BusinessStageTransitionsView.Close();
                mnuBusinessStageTransitions.Checked = false;
                if (holdItem != null)
                {
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(holdItem);
                }
            }

            if (mnuBusinessStageTransitions.Checked == true)
            {
                m_BusinessStageTransitionsView.Show(dockPanel, new Rectangle(1000, 200, 200, 240));
                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().Focus();
                }
            }
        }

        private void mnuItemView_Click(object sender, EventArgs e)
        {
        }

        private void btnGenerateSecurityDocument_Click(object sender, EventArgs e)
        {
            if (MainForm.App.GetCurrentView() == null)
                return;

            SaveFileDialog mDialog = new SaveFileDialog();
            mDialog.DefaultExt = "*.txt";
            mDialog.Filter = "Text File | *.txt";
            mDialog.Title = "Security Document";
            mDialog.FileName = this.GetCurrentView().Text + "_Security.txt";
            DialogResult res = mDialog.ShowDialog();
            if (res != DialogResult.OK)
                return;

            StreamWriter fStream = new StreamWriter(mDialog.FileName);
            fStream.WriteLine("Document : " + GetCurrentView().Document.Name);
            StringBuilder mBuilder = new StringBuilder();
            mBuilder.Append("___________");
            for (int x = 0; x < GetCurrentView().Document.Name.Length; x++)
            {
                mBuilder.Append("_");
            }

            fStream.WriteLine(mBuilder.ToString());

            fStream.WriteLine(" ");

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                WorkFlow wFlow = MainForm.App.GetCurrentView().Document.WorkFlows[x];
                fStream.WriteLine(" ");
                fStream.WriteLine(" ");
                fStream.WriteLine(" " + wFlow.WorkFlowName);
                mBuilder = null;
                mBuilder = new StringBuilder();
                mBuilder.Append(" ");
                for (int y = 0; y < wFlow.WorkFlowName.Length; y++)
                {
                    mBuilder.Append("_");
                }

                fStream.WriteLine(mBuilder.ToString());

                fStream.WriteLine(" ");

                CreateStateOutputForWorkFlow(wFlow, fStream);

                fStream.WriteLine(" ");

                CreateActivityOutputForWorkFlow(wFlow, fStream);

                fStream.WriteLine(" ");

                CreateCommonStateOutputForWorkFlow(wFlow, fStream);
            }

            fStream.Close();
            fStream.Dispose();
        }

        private void btnManageUsingStatements_Click(object sender, EventArgs e)
        {
            frmManageUsingStatements mFrmUsingStatements = new frmManageUsingStatements();
            mFrmUsingStatements.ShowDialog();
            mFrmUsingStatements.Dispose();
        }

        private void btnManageNuGetPackges_Click(object sender, EventArgs e)
        {
            frmNuGetPackages nuGetPackagesForm = new frmNuGetPackages();
            nuGetPackagesForm.ShowDialog();
            nuGetPackagesForm.Dispose();
        }

        private void mnuBusinessStageTransitions_Click(object sender, EventArgs e)
        {
            ShowBusinessStageTransitionsView();
        }

        public void UpdateCustomVariableMemberList()
        {
            string mCode = UpdateCustomVariableClass();
            MainForm.App.dotNetProjectResolver.SourceProjectContent.Clear();
            dotNetProjectResolver.SourceProjectContent.LoadForCode(cSharpLanguage, "CustomVariables", mCode);
        }

        public string UpdateCustomVariableClass()
        {
            StringBuilder SB = new StringBuilder();

            WorkFlow workFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;

            X2Generator.GenerateWorkFlowData(workFlow, SB, 0);
            string DataCode = SB.ToString();
            return DataCode;
        }

        private void btnReturnWorkFlowActivity_Click(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;

            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ReturnWorkFlowActivity;
            if (this.GetCurrentView() != null)
            {
                isDrawingActivity = true;
                this.GetCurrentView().toolBarItemChecked = true;
            }
        }

        private void btnReturnWorkFlowActivity_Click_1(object sender, EventArgs e)
        {
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;

            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.ReturnWorkFlowActivity;
            if (this.GetCurrentView() != null)
            {
                isDrawingActivity = true;
                this.GetCurrentView().toolBarItemChecked = true;
            }
        }

        private void btnGenericKeyType_Click(object sender, EventArgs e)
        {
            this.GetCurrentView().StartTransaction();
            frmGenericKeyType frm = new frmGenericKeyType(GetCurrentView().Document.CurrentWorkFlow.GenericKeyTypeKey);
            DialogResult dr = frm.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                this.GetCurrentView().FinishTransaction("Select Generic Key Type");
                return;
            }
            // set the generic key type
            GetCurrentView().Document.CurrentWorkFlow.GenericKeyTypeKey = frm.GenericKeyTypeKey;
            GetCurrentView().Document.CurrentWorkFlow.GenericKeyType = frm.GenericKeyType;

            if (OnGeneralCustomFormClosed != null)
            {
                OnGeneralCustomFormClosed(GeneralCustomFormType.GenericKeyType);
            }
            this.GetCurrentView().FinishTransaction("Select Generic Key Type");
        }

        private void compilePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Path.GetDirectoryName(Application.ExecutablePath) + "\\Build\\";
            FormDebug f = new FormDebug(s, false);
            f.ShowDialog();
        }

        private void clearSavedConnectionStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helpers.ClearCurrent();
            MessageBox.Show("Done");
        }

        private void unappliedCommonStatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bob = false;
            WorkFlow[] WorkFlows = GetCurrentView().Document.WorkFlows;
            for (int i = 0; i < WorkFlows.Length; i++)
            {
                List<string> zeroCount = new List<string>();
                foreach (BaseState b in WorkFlows[i].States)
                {
                    if (b is CommonState)
                    {
                        CommonState c = b as CommonState;
                        if (c != null)
                        {
                            if (c.AppliesTo.Count == 0)
                            {
                                zeroCount.Add(c.Name);
                                bob = true;
                            }
                        }
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (string s in zeroCount)
                {
                    sb.AppendFormat(",{0}", s);
                }
                if (sb.Length > 0)
                    sb.Remove(0, 1);
                MessageBox.Show(sb.ToString(), WorkFlows[i].WorkFlowName);
            }
            if (!bob)
                MessageBox.Show("All common states have been applied");
        }

        private void xMLPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Path.GetDirectoryName(Helpers.XMLPath);
            FormDebug f = new FormDebug(s, true);
            f.ShowDialog();
        }

        private void btnHoldState_Click(object sender, EventArgs e)
        {
            // get the active process window
            // get the view
            // set the active item type on the view
            unCheckToolStripButtons(sender.ToString());
            ProcessForm processForm = this.ActiveMdiChild as ProcessForm;
            processForm.View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.HoldState;
            if (this.GetCurrentView() != null)
            {
                this.GetCurrentView().toolBarItemChecked = true;
            }
            this.setStatusBar("Ready");
        }

        private void mnuRecentFiles_Click(object sender, EventArgs e)
        {
        }

        private void mnuItemMapVersion_Click(object sender, EventArgs e)
        {
            frmMapVersion frmVer = new frmMapVersion();
            if (this.GetCurrentView().Document.Location.Length == 0)
            {
                MessageBox.Show("Can only update the version on a saved map!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmVer.lblMapName.Text = this.GetCurrentView().Document.Location;
            frmVer.lblCurrentVersion.Text = XMLHandling.GetMapVersion(this.GetCurrentView().Document.Location);
            frmVer.ShowDialog();
            if (frmVer.DialogResult == DialogResult.OK)
            {
                //change map version

                string xmlFile = this.GetCurrentView().Document.Location;
                MainForm.App.documentIsBeingOpened = true;
                if (MainForm.App.GetCurrentView() != null)
                {
                }
                if (frmVer.txtNewVersion.Text.Length < 1)
                {
                    XMLHandling.SetMapVersion(this.GetCurrentView().Document.Location, xmlFile, "1");
                    this.GetCurrentView().Document.MapVersion = "1";
                }
                else
                {
                    XMLHandling.SetMapVersion(this.GetCurrentView().Document.Location, xmlFile, frmVer.txtNewVersion.Text);
                    this.GetCurrentView().Document.MapVersion = frmVer.txtNewVersion.Text;
                }
            }
            this.GetCurrentView().UpdateTitle();
        }

        private void dockPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        [Serializable]
        public class ProcessViewItem
        {
            ProcessView m_ProcessView;
            String m_ProcessViewName;

            public ProcessView view
            {
                get
                {
                    return m_ProcessView;
                }
                set
                {
                    m_ProcessView = value;
                }
            }

            public string ProcessName
            {
                get
                {
                    return m_ProcessViewName;
                }
                set
                {
                    m_ProcessViewName = value;
                }
            }
        }

        [Serializable]
        public class Associator
        {
            // Associate file extension with progID, description, icon and application
            public static void Associate(string extension, string progID, string description, string icon, string application)
            {
                Registry.ClassesRoot.CreateSubKey(extension).SetValue("", progID);
                if (progID != null && progID.Length > 0)
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progID))
                    {
                        if (description != null)
                            key.SetValue("", description);
                        if (icon != null)
                            key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(icon));
                        if (application != null)
                            key.CreateSubKey(@"Shell\Open\Command").SetValue("", ToShortPathName(application) + " \"%1\"");
                    }
            }

            // Return true if extension already associated in registry
            public static bool IsAssociated(string extension)
            {
                return (Registry.ClassesRoot.OpenSubKey(extension, false) != null);
            }

            [DllImport("Kernel32.dll")]
            private static extern uint GetShortPathName(string lpszLongPath, [Out] StringBuilder lpszShortPath, uint cchBuffer);

            // Return short path format of a file name
            private static string ToShortPathName(string longName)
            {
                StringBuilder s = new StringBuilder(1000);
                uint iSize = (uint)s.Capacity;
                uint iRet = GetShortPathName(longName, s, iSize);
                return s.ToString();
            }
        }
    }

    public class InvalidStageTransitionItem
    {
        private string m_ActivityName;
        private WorkFlow m_WorkFlow;
        private string m_SDSDGKey;
        private string m_DefinitionGroupDescription;
        private string m_DefinitionDescription;

        public string SDSDGKey
        {
            get
            {
                return m_SDSDGKey;
            }
            set
            {
                m_SDSDGKey = value;
            }
        }

        public string ActivityName
        {
            get
            {
                return m_ActivityName;
            }
            set
            {
                m_ActivityName = value;
            }
        }

        public WorkFlow workFlow
        {
            get
            {
                return m_WorkFlow;
            }
            set
            {
                m_WorkFlow = value;
            }
        }

        public string DefinitionGroupDescription
        {
            get
            {
                return m_DefinitionGroupDescription;
            }
            set
            {
                m_DefinitionGroupDescription = value;
            }
        }

        public string DefinitionDescription
        {
            get
            {
                return m_DefinitionDescription;
            }
            set
            {
                m_DefinitionDescription = value;
            }
        }
    }
}