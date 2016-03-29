using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using ActiproSoftware.SyntaxEditor.Addons.CSharp;
using ActiproSoftware.SyntaxEditor.Addons.DotNet.Dom;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer.Views
{
    public class CodeView : DockContent, ICutCopyPasteTarget
    {
        private IWorkflowItem m_NodeItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public SyntaxEditor syntaxEditor;
        public ToolStripComboBox toolStripComboBoxCodeSection;
        private ToolStripButton toolStripButtonBuild;
        private ImageList reflectionImageList;
        private string m_RootAssembly;

        private bool alreadyShowingCode = false;
        public bool handledClose = false;

        public bool RefreshCodeViewHeader = false;

        private ProcessView mView;
        private ToolStripButton btnComment;
        private ToolStripButton btnUnComment;
        private ToolStripButton btnIndent;
        private ToolStripButton btnRemoveIndent;

        private ProcessView holdCurrentView = null;

        internal static string ProjectPath
        {
            get
            {
                return Path.GetFullPath(Path.GetDirectoryName(Application.ExecutablePath) + @"\..\..\");
            }
        }

        public CodeView()
            : base()
        {
            Text = "Code View";
            InitializeComponent();
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);

            Assembly Asm = Assembly.GetExecutingAssembly();
            SemanticParserService.Start();

            MainForm.App.cSharpLanguage = new CSharpSyntaxLanguage();
            MainForm.App.dotNetProjectResolver = new DotNetProjectResolver();
            MainForm.App.dotNetProjectResolver.CachePath = CodeView.ProjectPath + @"Cache\";
            syntaxEditor.Document.Language = MainForm.App.cSharpLanguage;
            syntaxEditor.Document.LanguageData = MainForm.App.dotNetProjectResolver;

            // Initialize a .NET project resolver with some typical assemblies

            MainForm.App.dotNetProjectResolver.AddExternalReferenceForMSCorLib();
            MainForm.App.dotNetProjectResolver.AddExternalReferenceForSystemAssembly("System");
            MainForm.App.dotNetProjectResolver.AddExternalReferenceForSystemAssembly("System.Data");
            this.syntaxEditor.BracketHighlightingVisible = true;
            this.syntaxEditor.Document.HeaderText = "public class invisiClass \n { ";
            this.syntaxEditor.Document.FooterText = "}";

            // Load reflection data for some external code files that for this sample we will assume are part of the
            // same "project" as the two files being edited in SyntaxEditor... this allows for the reflection data
            // for the external code files to be used for IntelliPrompt in addition to external assembly references...
            // The following line will load the code for the ColorButton class that is stored in the sample project...
            // IDE applications that used "project"-based concepts would queue up all the files in the project using
            // lines like this, making one call for each code file in the "project"...

            reflectionImageList = SyntaxEditor.ReflectionImageList;

            string mCode = MainForm.App.UpdateCustomVariableClass();
            MainForm.App.dotNetProjectResolver.SourceProjectContent.LoadForCode(MainForm.App.cSharpLanguage, "CustomVariables", mCode);
            AddUsingStatements();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeView));
            ActiproSoftware.SyntaxEditor.Document document1 = new ActiproSoftware.SyntaxEditor.Document();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxCodeSection = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonBuild = new System.Windows.Forms.ToolStripButton();
            this.btnComment = new System.Windows.Forms.ToolStripButton();
            this.btnUnComment = new System.Windows.Forms.ToolStripButton();
            this.btnIndent = new System.Windows.Forms.ToolStripButton();
            this.btnRemoveIndent = new System.Windows.Forms.ToolStripButton();
            this.syntaxEditor = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripComboBoxCodeSection,
            this.toolStripSeparator1,
            this.toolStripButtonBuild,
            this.btnComment,
            this.btnUnComment,
            this.btnIndent,
            this.btnRemoveIndent});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(755, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "View Code For: ";
            // 
            // toolStripComboBoxCodeSection
            // 
            this.toolStripComboBoxCodeSection.AutoSize = false;
            this.toolStripComboBoxCodeSection.AutoToolTip = true;
            this.toolStripComboBoxCodeSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxCodeSection.Name = "toolStripComboBoxCodeSection";
            this.toolStripComboBoxCodeSection.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripComboBoxCodeSection.Size = new System.Drawing.Size(300, 23);
            this.toolStripComboBoxCodeSection.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            this.toolStripComboBoxCodeSection.Click += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonBuild
            // 
            this.toolStripButtonBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonBuild.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonBuild.Image")));
            this.toolStripButtonBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonBuild.Name = "toolStripButtonBuild";
            this.toolStripButtonBuild.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonBuild.Text = "Build";
            this.toolStripButtonBuild.Click += new System.EventHandler(this.OnMnuBuildClicked);
            // 
            // btnComment
            // 
            this.btnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnComment.Image = ((System.Drawing.Image)(resources.GetObject("btnComment.Image")));
            this.btnComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(23, 22);
            this.btnComment.Text = "Comment";
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // btnUnComment
            // 
            this.btnUnComment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUnComment.Image = ((System.Drawing.Image)(resources.GetObject("btnUnComment.Image")));
            this.btnUnComment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnComment.Name = "btnUnComment";
            this.btnUnComment.Size = new System.Drawing.Size(23, 22);
            this.btnUnComment.Text = "UnComment";
            this.btnUnComment.Click += new System.EventHandler(this.btnUnComment_Click);
            // 
            // btnIndent
            // 
            this.btnIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnIndent.Image = ((System.Drawing.Image)(resources.GetObject("btnIndent.Image")));
            this.btnIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnIndent.Name = "btnIndent";
            this.btnIndent.Size = new System.Drawing.Size(23, 22);
            this.btnIndent.Text = "Increase Indent";
            this.btnIndent.Click += new System.EventHandler(this.btnIndent_Click);
            // 
            // btnRemoveIndent
            // 
            this.btnRemoveIndent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveIndent.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveIndent.Image")));
            this.btnRemoveIndent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveIndent.Name = "btnRemoveIndent";
            this.btnRemoveIndent.Size = new System.Drawing.Size(23, 22);
            this.btnRemoveIndent.Text = "Decrease Indent";
            this.btnRemoveIndent.Click += new System.EventHandler(this.btnRemoveIndent_Click);
            // 
            // syntaxEditor
            // 
            this.syntaxEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            document1.AutoCaseCorrectEnabled = true;
            document1.Outlining.Mode = ActiproSoftware.SyntaxEditor.OutliningMode.Automatic;
            this.syntaxEditor.Document = document1;
            this.syntaxEditor.Location = new System.Drawing.Point(0, 25);
            this.syntaxEditor.Name = "syntaxEditor";
            this.syntaxEditor.Size = new System.Drawing.Size(755, 191);
            this.syntaxEditor.TabIndex = 1;
            this.syntaxEditor.WordWrap = ActiproSoftware.SyntaxEditor.WordWrapType.Word;
            this.syntaxEditor.KeyTyped += new ActiproSoftware.SyntaxEditor.KeyTypedEventHandler(this.syntaxEditor_KeyTyped);
            this.syntaxEditor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.syntaxEditor_KeyUp);
            this.syntaxEditor.Leave += new System.EventHandler(this.syntaxEditor_Leave);
            // 
            // CodeView
            // 
            this.ClientSize = new System.Drawing.Size(755, 216);
            this.ControlBox = false;
            this.Controls.Add(this.syntaxEditor);
            this.Controls.Add(this.toolStrip1);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeView";
            this.TabText = "Code View";
            this.Text = "Code View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeView_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CodeView_FormClosed);
            this.Enter += new System.EventHandler(this.CodeView_Enter);
            this.Leave += new System.EventHandler(this.CodeView_Leave);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void syntaxEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                return;

            string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
            //check if the stageActivity message has is being changed and if so set the StageTransitionProperty to overridden
            if (CodeItem == "OnStageActivity")
            {
                BaseActivity a = m_NodeItem as BaseActivity;
                string stmOnly = string.Empty;
                if (a.StageTransitionMessage.IndexOf(" [CodeChanged") > -1)
                {
                    stmOnly = a.StageTransitionMessage.Substring(0, a.StageTransitionMessage.IndexOf(" [CodeChanged"));
                }
                else
                {
                    stmOnly = a.StageTransitionMessage;
                }

                Regex regex = new Regex("(?<=return\\s*\\\").*(?=\\\";)");
                Match m = regex.Match(this.syntaxEditor.Text);
                if (m.Success)
                {
                    string result = m.Value;

                    // check if its the same as the current stage transition
                    if (result != stmOnly)
                    {
                        // we need to update the stage transition message
                        a.StageTransitionMessage = result + " [CodeChanged]";
                        a.CodeChanged = true;
                        MainForm.App.FireOnStageTransitionMessageChangedEvent(PropertyType.BusinessStageTransition);
                    }
                }
                else
                {
                    // there is no stage transitionmessage so set it to empty
                    a.StageTransitionMessage = "";
                    MainForm.App.FireOnStageTransitionMessageChangedEvent(PropertyType.BusinessStageTransition);
                    return;
                }
            }
        }

        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            MessageBox.Show(this.syntaxEditor.ContextMenu.MenuItems.Count.ToString());
        }

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            AttachCode();
            if (MainForm.App.GetCurrentView() != null)
            {
                ProcessView PV = MainForm.App.GetCurrentView();

                if (PV.Selection.Count == 1)
                {
                    BaseItem Bi = PV.Selection.First as BaseItem;
                    View_OnWorkFlowItemSelected(Bi);
                }
                else
                {
                    if (MainForm.App.m_BrowserView != null)
                    {
                        if (MainForm.App.m_BrowserView.treeViewProc.SelectedNode != null)
                        {
                            RolesCollectionItem roleItem = MainForm.App.m_BrowserView.treeViewProc.SelectedNode.Tag as RolesCollectionItem;
                            if (roleItem != null && MainForm.App.roleSelectedInPropertyGrid == false && MainForm.App.variableSelectedInPropertyGrid == false
                                && MainForm.App.formSelectedInPropertyGrid == false)
                            {
                                if (MainForm.App.GetCurrentView() != null)
                                {
                                    MainForm.App.GetCurrentView().Selection.Clear();
                                }
                                alreadyShowingCode = true;
                                this.View_OnWorkFlowItemSelected(roleItem);
                            }
                        }
                    }
                }
            }

            AppDomain curDomain = AppDomain.CurrentDomain;

            curDomain.AssemblyResolve += new ResolveEventHandler(MyReflectionOnlyResolveEventHandler);

            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x8)//&& MainForm.App.m_ErrorsView == null && MainForm.App.isCompiling == false)
            {
                if (handledClose == false)
                {
                    //DetachCode();
                    MainForm.App.mnuItemCodeView.Checked = false;
                    AppDomain curDomain = AppDomain.CurrentDomain;
                    mView = MainForm.App.GetCurrentView();
                    MainForm.App.GetCurrentView().Focus();

                    handledClose = true;
                }
            }
            else
            {
                if (handledClose == false)
                {
                    holdCurrentView = MainForm.App.GetCurrentView();
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible == false)
            {
                if (holdCurrentView != null)
                {
                    MainForm.App.mnuItemCodeView.Checked = false;
                    holdCurrentView.Focus();
                }
                MainForm.App.OnProcessViewActivated -= new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
                MainForm.App.OnProcessViewDeactivated -= new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
                MainForm.App.OnGeneralCustomFormClosed -= new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);
            }
        }

        #endregion Overrides

        #region Misc

        public void AttachCode()
        {
            // ensure we have actually unhooked before rehooking
            if (alreadyShowingCode == false)
            {
                DetachCode();
            }

            ProcessView PV = MainForm.App.GetCurrentView();
            if (PV != null)
            {
                PV.OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
                PV.OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(View_OnWorkFlowItemUnSelected);
                PV.OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(PV_OnPropertyChanged);
                PV.OnStageTransitionMessageChanged += new ProcessView.OnStageTransitionMessageChangedHandler(PV_OnStageTransitionMessageChanged);

                if (m_NodeItem == null)//&& PV.Selection.Count == 1)
                {
                    m_NodeItem = PV.Selection.First as BaseItem;
                    View_OnWorkFlowItemSelected(m_NodeItem);
                }
            }
            MainForm.App.OnRoleItemSelected += new MainForm.RoleItemSelectedHandler(App_OnRoleItemSelected);
        }

        private void PV_OnStageTransitionMessageChanged(string NewStageTransitionMessage)
        {
            if (toolStripComboBoxCodeSection.SelectedItem != null)
            {
                BaseActivity b = m_NodeItem as BaseActivity;
                string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
                this.syntaxEditor.Text = m_NodeItem.RefreshCodeSectionData(CodeItem);
                b.UpdateStageTransitionMessage(NewStageTransitionMessage);
            }
        }

        private void App_OnRoleItemSelected(RolesCollectionItem roleItem)
        {
            if (roleItem != null)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().Selection.Count < 2)
                    {
                        if (roleItem.AvailableCodeSections != null)
                        {
                            try
                            {
                                toolStripComboBoxCodeSection.BeginUpdate();
                                m_NodeItem = roleItem;
                                ClearView();
                                //      populate the combobox
                                toolStripComboBoxCodeSection.Items.AddRange(roleItem.AvailableCodeSections);
                                toolStripComboBoxCodeSection.EndUpdate();

                                if (toolStripComboBoxCodeSection.Items.Count > 0)
                                    toolStripComboBoxCodeSection.SelectedIndex = 0;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            else
                ClearView();
        }

        public void DetachCode()
        {
            ProcessView PV = MainForm.App.GetCurrentView();
            if (PV != null)
            {
                // set the codesection data back onto the item
                m_NodeItem = null;
            }

            ClearView();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxCodeSection.SelectedIndex != -1)
            {
                string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
                syntaxEditor.Text = m_NodeItem.GetCodeSectionData(CodeItem);
            }
            else
            {
                syntaxEditor.Text = "";
            }
            if (MainForm.App.m_FindReplaceView != null)
            {
                MainForm.App.m_FindReplaceView.FindText();
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            SaveCode();
        }

        public void View_OnWorkFlowItemSelected(IWorkflowItem SelectedItem)
        {
            if (SelectedItem != null)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().Selection.Count < 2)
                    {
                        if (SelectedItem.AvailableCodeSections != null)
                        {
                            try
                            {
                                toolStripComboBoxCodeSection.BeginUpdate();
                                m_NodeItem = SelectedItem;

                                //      populate the combobox

                                ClearView();
                                toolStripComboBoxCodeSection.Items.AddRange(SelectedItem.AvailableCodeSections);
                                toolStripComboBoxCodeSection.EndUpdate();

                                if (toolStripComboBoxCodeSection.Items.Count > 0)
                                    toolStripComboBoxCodeSection.SelectedIndex = 0;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            else
                ClearView();
        }

        private void View_OnWorkFlowItemUnSelected(IWorkflowItem UnSelectedItem)
        {
            // set the object that is losing selection's codesections
            try
            {
                if (UnSelectedItem != null && toolStripComboBoxCodeSection.SelectedIndex != -1)
                {
                    if (toolStripComboBoxCodeSection.SelectedIndex != -1)
                    {
                        string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
                        //Commented by Terence to fix OnEnter bug in Code window when state was changed
                        //---------------------
                        //m_NodeItem.SetCodeSectionData(CodeItem, syntaxEditor.Text);
                        //---------------------
                    }

                    m_NodeItem = null;
                    ClearView();
                }
            }
            catch
            {
            }
        }

        public void ClearView()
        {
            try
            {
                if (!MainForm.App.mainFormIsClosing)
                {
                    toolStripComboBoxCodeSection.Items.Clear();
                    if (syntaxEditor != null)
                    {
                        syntaxEditor.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void syntaxEditor_Leave(object sender, EventArgs e)
        {
        }

        public void SaveCode()
        {
            if (m_NodeItem != null && toolStripComboBoxCodeSection.SelectedIndex != -1)
            {
                if (this.syntaxEditor.Document != null)
                {
                    this.syntaxEditor.Document.HeaderText = "";
                    this.syntaxEditor.Document.FooterText = "";
                }
                string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
                m_NodeItem.SetCodeSectionData(CodeItem, syntaxEditor.Text);
            }
        }

        private void OnMnuBuildClicked(object sender, EventArgs e)
        {
            MainForm.App.BuildCode(true);
        }

        internal void SetErrorPosition(ErrorSource Source)
        {
            if (Source != null)
            {
                syntaxEditor.Document.LineIndicators.Clear();
                int Index = toolStripComboBoxCodeSection.FindStringExact(Source.Cp.CodeSection);
                if (Index != -1)
                    toolStripComboBoxCodeSection.SelectedIndex = Index;
                if (Source.Offset < syntaxEditor.Document.Length)
                {
                    syntaxEditor.Caret.Offset = Source.Offset;
                    if (syntaxEditor.Document.SpanIndicatorLayers.Count == 0)
                    {
                        SpanIndicatorLayer mIndicatorLayer = new SpanIndicatorLayer("mainSpanIndicatorLayer", 0);
                        this.syntaxEditor.Document.SpanIndicatorLayers.Add(mIndicatorLayer);
                    }
                    syntaxEditor.Document.SpanIndicatorLayers[0].Add(new SyntaxErrorSpanIndicator(), syntaxEditor.Document.Lines[syntaxEditor.Caret.EditPosition.Line].StartOffset, syntaxEditor.Document.Lines[syntaxEditor.Caret.EditPosition.Line].Length);
                }
            }
        }

        private void CodeView_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.App.OnRoleItemSelected -= new MainForm.RoleItemSelectedHandler(App_OnRoleItemSelected);
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnPropertyChanged -= new ProcessView.OnPropertyChangedHandler(PV_OnPropertyChanged);
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected -= new ProcessView.OnWorkFlowItemUnSelectedHandler(View_OnWorkFlowItemUnSelected);
            }
        }

        private void CodeView_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        #endregion Misc

        #region ICutCopyPasteTarget Members

        public void DoCut()
        {
            this.syntaxEditor.SelectedView.CutToClipboard();
        }

        public void DoCopy()
        {
            this.syntaxEditor.SelectedView.CopyToClipboard();
        }

        public void DoPaste()
        {
            this.syntaxEditor.SelectedView.PasteFromClipboard();
        }

        public void DoUndo()
        {
            this.syntaxEditor.Document.UndoRedo.Undo();
        }

        public void DoRedo()
        {
            this.syntaxEditor.Document.UndoRedo.Redo();
        }

        public void DoDelete()
        {
            this.syntaxEditor.SelectedView.SelectedText = "";
        }

        public void DoPrint()
        {
            syntaxEditor.Print(true);
        }

        public void DoSelectAll()
        {
            this.syntaxEditor.SelectedView.Selection.SelectAll();
        }

        private void CodeView_Leave(object sender, EventArgs e)
        {
            MainForm.App.CutCopyPasteTarget = null;
            if (syntaxEditor.IsDisposed == false || syntaxEditor.Disposing == false)
            {
                SaveCode();
            }
        }

        #endregion ICutCopyPasteTarget Members

        #region EventHandlers

        private void PV_OnPropertyChanged(PropertyType propType)
        {
            try
            {
                //SaveCode();
                switch (propType)
                {
                    case PropertyType.AutoForward:
                        {
                            toolStripComboBoxCodeSection.Items.Clear();
                            toolStripComboBoxCodeSection.BeginUpdate();
                            ClearView();
                            // populate the combobox
                            toolStripComboBoxCodeSection.Items.AddRange(m_NodeItem.AvailableCodeSections);
                            toolStripComboBoxCodeSection.EndUpdate();

                            if (toolStripComboBoxCodeSection.Items.Count > 0)
                                toolStripComboBoxCodeSection.SelectedIndex = 0;

                            break;
                        }

                    case PropertyType.Name:
                        {
                            if (toolStripComboBoxCodeSection.SelectedIndex != -1)
                            {
                                string CodeItem = toolStripComboBoxCodeSection.SelectedItem as string;
                                syntaxEditor.Text = m_NodeItem.GetCodeSectionData(CodeItem);
                            }
                            else
                            {
                                syntaxEditor.Text = "";
                            }
                            break;
                        }
                    case PropertyType.IsDynamic:
                        {
                            RolesCollectionItem mItem = m_NodeItem as RolesCollectionItem;
                            if (mItem != null)
                            {
                                if (mItem.IsDynamic)
                                {
                                    if (toolStripComboBoxCodeSection.SelectedIndex == -1)
                                    {
                                        toolStripComboBoxCodeSection.Items.AddRange(m_NodeItem.AvailableCodeSections);
                                        toolStripComboBoxCodeSection.EndUpdate();

                                        if (toolStripComboBoxCodeSection.Items.Count > 0)
                                            toolStripComboBoxCodeSection.SelectedIndex = 0;
                                    }
                                    else
                                    {
                                        //     syntaxEditor.Text = "";
                                    }
                                }
                                else
                                {
                                    ClearView();
                                }
                            }
                            break;
                        }
                }
            }
            catch
            {
            }
        }

        private void App_OnProcessViewDeactivated(ProcessView view)
        {
            DetachCode();
            try
            {
                toolStripComboBoxCodeSection.SelectedIndex = -1;
            }
            catch
            {
            }
        }

        private void App_OnProcessViewActivated(ProcessView view)
        {
            AttachCode();
        }

        private void App_OnGeneralCustomFormClosed(GeneralCustomFormType formType)
        {
            MainForm.App.UpdateCustomVariableMemberList();
        }

        private void CodeView_OnWorkFlowItemSelected(IWorkflowItem SelectedItem)
        {
            AttachCode();
        }

        private void CodeView_OnWorkFlowItemUnSelected(IWorkflowItem UnSelectedItem)
        {
            DetachCode();
        }

        private Assembly MyReflectionOnlyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Debug.WriteLine("Resolving");

            AssemblyName name = new AssemblyName(args.Name);

            Debug.WriteLine(name.FullName);

            Assembly[] Asms = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < Asms.Length; i++)
            {
                if (Asms[i].FullName == name.FullName)
                    return Asms[i];
            }
            return null;
        }

        #endregion EventHandlers

        private void CodeView_Enter(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message.ToString());
                ExceptionPolicy.HandleException(ex, "X2Designer");
            }
            MainForm.App.CutCopyPasteTarget = this;
            RefreshSyntaxEditorHeader();
        }

        public bool IsReferencedAssembly(List<ReferenceItem> Refs, Assembly FullAssembly)
        {
            for (int i = 0; i < Refs.Count; i++)
            {
                if (FullAssembly.FullName == Refs[i].FullName)
                    return true;
            }
            return false;
        }

        private static string AddTabs(int tabs)
        {
            string tabstr = "";
            for (int i = 0; i < tabs; i++)
                tabstr += "\t";
            return tabstr;
        }

        public void RefreshSyntaxEditorHeader()
        {
            try
            {
                if (MainForm.App.GetCurrentView() == null)
                {
                    return;
                }
                this.syntaxEditor.Document.HeaderText = "";
                RefreshCodeViewHeader = false;
                string headerText = "";

                headerText += "using " + "SAHL.X2Designer.Languages;\n";

                for (int x = 0; x < MainForm.App.GetCurrentView().Document.UsedUsingStatements.Count; x++)
                {
                    if (!headerText.Contains("using " + MainForm.App.GetCurrentView().Document.UsedUsingStatements[x]))
                    {
                        headerText += "using " + MainForm.App.GetCurrentView().Document.UsedUsingStatements[x] + ";\n";
                    }
                }

                this.syntaxEditor.Document.HeaderText = headerText + "\npublic class invisiClass \n { ";

                this.syntaxEditor.Document.FooterText = "}";
                this.Focus();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnUsingStatements_Click(object sender, EventArgs e)
        {
            frmManageUsingStatements mFrmUsingStatements = new frmManageUsingStatements();
            mFrmUsingStatements.ShowDialog();
            mFrmUsingStatements.Dispose();
            AddUsingStatements();
        }

        private void AddUsingStatements()
        {
            try
            {
                RefreshSyntaxEditorHeader();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void syntaxEditor_KeyTyped(object sender, KeyTypedEventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.CommentLines();
        }

        private void btnUnComment_Click(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.UncommentLines();
        }

        private void btnIndent_Click(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.Indent();
        }

        private void btnRemoveIndent_Click(object sender, EventArgs e)
        {
            syntaxEditor.SelectedView.Outdent();
        }
    }
}