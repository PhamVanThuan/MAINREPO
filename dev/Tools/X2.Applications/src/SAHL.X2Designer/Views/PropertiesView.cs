using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using WeifenLuo.WinFormsUI;
using System.Linq;

namespace SAHL.X2Designer.Views
{
    public class PropertiesView : DockContent
    {
        public ComboBox cbxProperties;

        private bool handledClose = false;

        //private List<comboItem> lstCombinedNodes = new List<comboItem>();
        public List<IWorkflowItem> lstCombinedNodes = new List<IWorkflowItem>();
        public PropertyGrid propertyGrid;
        public bool m_OwnSelection;
        public string lastSelectedProperty = "";

        int m_CurrentVK = 0;
        int m_CurrentSC = 0;

        public PropertiesView()
            : base()
        {
            Text = "Properties";
            InitializeComponent();
            this.propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;

            populateDropDown();
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnStageTransitionMessageChanged += new MainForm.OnStageTransitionMessageHandler(App_OnStageTransitionMessageChanged);
            if (MainForm.App.GetCurrentView() != null)
            {
                AttachProperties();
            }
        }

        private void App_OnStageTransitionMessageChanged(PropertyType propType)
        {
            m_OwnSelection = true;
            if (propertyGrid.SelectedObject != null)
            {
                object o = propertyGrid.SelectedObject;
                propertyGrid.SelectedObject = null;
                propertyGrid.SelectedObject = o;
                populateDropDown();
            }
        }

        private void InitializeComponent()
        {
            this.cbxProperties = new System.Windows.Forms.ComboBox();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            //
            // cbxProperties
            //
            this.cbxProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxProperties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProperties.FormattingEnabled = true;
            this.cbxProperties.Location = new System.Drawing.Point(0, 0);
            this.cbxProperties.Name = "cbxProperties";
            this.cbxProperties.Size = new System.Drawing.Size(246, 21);
            this.cbxProperties.TabIndex = 1;
            this.cbxProperties.SelectionChangeCommitted += new System.EventHandler(this.cbxProperties_SelectionChangeCommitted);
            this.cbxProperties.DropDown += new System.EventHandler(this.cbxProperties_DropDown);
            //
            // propertyGrid
            //
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 21);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(246, 370);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid_SelectedGridItemChanged);
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            //
            // PropertiesView
            //
            this.ClientSize = new System.Drawing.Size(246, 391);
            this.ControlBox = false;
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.cbxProperties);
            this.DockableAreas = ((WeifenLuo.WinFormsUI.DockAreas)(((((WeifenLuo.WinFormsUI.DockAreas.Float | WeifenLuo.WinFormsUI.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.DockAreas.DockBottom)));
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(5000, 100);
            this.Name = "PropertiesView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TabText = "Properties";
            this.Text = "Properties";
            this.Enter += new System.EventHandler(this.PropertiesView_Enter);
            this.ResumeLayout(false);
        }

        #region Misc

        public void populateDropDown()
        {
            if (MainForm.App.GetCurrentView() == null)
            {
                return;
            }
            cbxProperties.SelectedIndexChanged -= new EventHandler(cbxProperties_SelectionChangeCommitted);
            List<IWorkflowItem> lstNodesToBeSorted = new List<IWorkflowItem>();

            cbxProperties.Items.Clear();
            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
            {
                lstCombinedNodes.Clear();
                foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                {
                    if (o.GetType() == typeof(ClapperBoard))
                    {
                        lstCombinedNodes.Add(o as BaseItem);
                        break;
                    }
                }

                for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].Name.ToLower() != "originator")
                    {
                        lstNodesToBeSorted.Add(MainForm.App.GetCurrentView().Document.Roles[x] as IWorkflowItem);
                    }
                }

                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; x++)
                {
                    lstNodesToBeSorted.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x] as IWorkflowItem);
                }

                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                {
                    lstNodesToBeSorted.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x] as IWorkflowItem);
                }

                foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                {
                    if (o is BaseState | o is BaseActivity | o.GetType() == typeof(Comment))
                    {
                        lstNodesToBeSorted.Add(o as IWorkflowItem);
                    }
                }

                if (lstNodesToBeSorted.Count > 1)
                {
                    try
                    {
                        lstNodesToBeSorted = lstNodesToBeSorted.OrderBy(o => o.Name).ToList();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.ToString());
                    }

                }

                for (int x = 0; x < lstNodesToBeSorted.Count; x++)
                {
                    lstCombinedNodes.Add(lstNodesToBeSorted[x]);
                }

                for (int x = 0; x < lstCombinedNodes.Count; x++)
                {
                    string typeString = "";
                    BaseItem mItem = lstCombinedNodes[x] as BaseItem;
                    if (mItem == null)
                    {
                        if (lstCombinedNodes[x].WorkflowItemType == WorkflowItemType.RoleStatic)
                        {
                            typeString = "<Static Role>";
                        }
                        if (lstCombinedNodes[x].WorkflowItemType == WorkflowItemType.RoleDynamic)
                        {
                            typeString = "<Dynamic Role>";
                        }
                        if (lstCombinedNodes[x].WorkflowItemType == WorkflowItemType.CustomVariable)
                        {
                            typeString = "<Custom Variable>";
                        }
                        if (lstCombinedNodes[x].WorkflowItemType == WorkflowItemType.CustomForm)
                        {
                            typeString = "<Form>";
                        }
                    }
                    else
                    {
                        switch (mItem.WorkflowItemType)
                        {
                            case WorkflowItemType.None:
                                {
                                    typeString = "<Starting Point>";
                                    break;
                                }

                            case WorkflowItemType.SystemState:
                                {
                                    typeString = "<System State>";
                                    break;
                                }
                            case WorkflowItemType.SystemDecisionState:
                                {
                                    typeString = "<System Decision State>";
                                    break;
                                }
                            case WorkflowItemType.CommonState:
                                {
                                    typeString = "<Common State>";
                                    break;
                                }
                            case WorkflowItemType.UserState:
                                {
                                    typeString = "<User State>";
                                    break;
                                }
                            case WorkflowItemType.HoldState:
                                {
                                    typeString = "<Hold State>";
                                    break;
                                }
                            case WorkflowItemType.ArchiveState:
                                {
                                    typeString = "<Archive State>";
                                    break;
                                }
                            case WorkflowItemType.UserActivity:
                                {
                                    typeString = "<User Activity>";
                                    break;
                                }
                            case WorkflowItemType.ConditionalActivity:
                                {
                                    typeString = "<Conditional Activity>";
                                    break;
                                }
                            case WorkflowItemType.ExternalActivity:
                                {
                                    typeString = "<External Activity>";
                                    break;
                                }
                            case WorkflowItemType.CallWorkFlowActivity:
                                {
                                    typeString = "<CallWorkFlow Activity>";
                                    break;
                                }
                            case WorkflowItemType.ReturnWorkFlowActivity:
                                {
                                    typeString = "<ReturnWorkFlow Activity>";
                                    break;
                                }

                            case WorkflowItemType.TimedActivity:
                                {
                                    typeString = "<Timed Activity>";
                                    break;
                                }

                            case WorkflowItemType.Comment:
                                {
                                    typeString = "<Comment>";
                                    break;
                                }
                        }
                    }
                    if (lstCombinedNodes[x].GetType() != typeof(Comment) && lstCombinedNodes[x].GetType() != typeof(ClapperBoard))
                    {
                        cbxProperties.Items.Add(lstCombinedNodes[x].Name.ToString() + " " + typeString);
                    }
                    else if (lstCombinedNodes[x].GetType() == typeof(Comment))
                    {
                        Comment mCom = lstCombinedNodes[x] as Comment;
                        cbxProperties.Items.Add("Comment" + mCom.CommentID.ToString() + " " + typeString);
                    }
                    else if (lstCombinedNodes[x].GetType() == typeof(ClapperBoard))
                    {
                        cbxProperties.Items.Add("Starting Point");
                    }
                }
                for (int x = 0; x < lstCombinedNodes.Count; x++)
                {
                    if (lstCombinedNodes[x] == MainForm.App.GetCurrentView().Selection.Primary as BaseItem)
                    {
                        cbxProperties.SelectedIndex = x;
                        break;
                    }
                }
            }
            cbxProperties.SelectedIndexChanged += new EventHandler(cbxProperties_SelectionChangeCommitted);
        }

        private void AttachProperties()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(GeneralCustomFormClosed);
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(View_OnWorkFlowItemUnSelected);

                if (MainForm.App.GetCurrentView().Selection.Count > 0 && m_OwnSelection == false)
                {
                    BaseItem Bi = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                    if (Bi != null)
                    {
                        try
                        {
                            m_OwnSelection = true;
                            this.propertyGrid.SelectedObject = Bi.Properties;
                            populateDropDown();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        this.propertyGrid.SelectedObject = null;
                        cbxProperties.Items.Clear();
                        cbxProperties.Text = "";
                    }
                }
            }
            MainForm.App.OnCustomVariableItemSelected += new MainForm.CustomVariableItemSelectedHandler(App_OnCustomVariableItemSelected);
            MainForm.App.OnCustomFormItemSelected += new MainForm.CustomFormItemSelectedHandler(App_OnCustomFormItemSelected);
            MainForm.App.OnRoleItemSelected += new MainForm.RoleItemSelectedHandler(App_OnRoleItemSelected);
        }

        private void App_OnRoleItemSelected(RolesCollectionItem roleItem)
        {
            this.propertyGrid.SelectedObject = roleItem.Properties;
            populateDropDown();
        }

        private void App_OnCustomVariableItemSelected(CustomVariableItem varItem)
        {
            this.propertyGrid.SelectedObject = varItem.Properties;
            populateDropDown();
        }

        private void App_OnCustomFormItemSelected(CustomFormItem frmItem)
        {
            this.propertyGrid.SelectedObject = frmItem.Properties;
            populateDropDown();
        }

        private void DetachProperties()
        {
            m_OwnSelection = false;
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
            }
            if (propertyGrid.SelectedObject != null)
            {
                propertyGrid.SelectedObject = null;
                cbxProperties.Items.Clear();
            }
        }

        private void cbxProperties_DropDown(object sender, EventArgs e)
        {
            populateDropDown();
        }

        private void cbxProperties_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbxProperties.SelectedIndex != -1)
            {
                BaseItem i = lstCombinedNodes[cbxProperties.SelectedIndex] as BaseItem;
                if (i != null && MainForm.App.GetCurrentView().Selection.Primary != i)
                {
                    MainForm.App.GetCurrentView().Selection.Clear();

                    MainForm.App.GetCurrentView().Selection.Add(i);
                }
            }
            if (cbxProperties.SelectedIndex > -1)
            {
                RolesCollectionItem mRole = lstCombinedNodes[cbxProperties.SelectedIndex] as RolesCollectionItem;
                if (mRole != null)
                {
                    this.propertyGrid.SelectedObject = mRole.Properties;
                    if (MainForm.App.m_BrowserView != null)
                    {
                        TreeNode RoleNode = new TreeNode();
                        bool res = MainForm.App.m_BrowserView.hashTableRole.TryGetValue(mRole as IWorkflowItem, out RoleNode);
                        if (res == true)
                        {
                            MainForm.App.GetCurrentView().Selection.Clear();
                            MainForm.App.roleSelectedInPropertyGrid = true;
                            MainForm.App.m_BrowserView.treeViewProc.SelectedNode = RoleNode;
                            MainForm.App.roleSelectedInPropertyGrid = false;
                        }
                    }
                }
            }
            if (cbxProperties.SelectedIndex > -1)
            {
                CustomVariableItem mCustomVariable = lstCombinedNodes[cbxProperties.SelectedIndex] as CustomVariableItem;
                if (mCustomVariable != null)
                {
                    this.propertyGrid.SelectedObject = mCustomVariable.Properties;
                    if (MainForm.App.m_BrowserView != null)
                    {
                        TreeNode CustVarNode = new TreeNode();
                        bool res = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.TryGetValue(mCustomVariable as IWorkflowItem, out CustVarNode);
                        if (res == true)
                        {
                            MainForm.App.GetCurrentView().Selection.Clear();
                            MainForm.App.variableSelectedInPropertyGrid = true;
                            MainForm.App.m_BrowserView.treeViewProc.SelectedNode = CustVarNode;
                            MainForm.App.variableSelectedInPropertyGrid = false;
                        }
                    }
                }
            }

            if (cbxProperties.SelectedIndex > -1)
            {
                CustomFormItem mCustomForm = lstCombinedNodes[cbxProperties.SelectedIndex] as CustomFormItem;
                if (mCustomForm != null)
                {
                    this.propertyGrid.SelectedObject = mCustomForm.Properties;
                    if (MainForm.App.m_BrowserView != null)
                    {
                        TreeNode CustFormNode = new TreeNode();
                        bool res = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm.TryGetValue(mCustomForm as IWorkflowItem, out CustFormNode);
                        if (res == true)
                        {
                            MainForm.App.GetCurrentView().Selection.Clear();
                            MainForm.App.formSelectedInPropertyGrid = true;
                            MainForm.App.m_BrowserView.treeViewProc.SelectedNode = CustFormNode;
                            MainForm.App.formSelectedInPropertyGrid = false;
                        }
                    }
                }
            }
        }

        #endregion Misc

        #region Overrides

        protected override void OnClosed(EventArgs e)
        {
            MainForm.App.menuPropertiesChecked(false);
            base.OnClosed(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible == true)
            {
                GoObject holdObject = null;
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().Selection.Primary == null)
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
                                    this.View_OnWorkFlowItemSelected(roleItem);
                                }
                                CustomVariableItem varItem = MainForm.App.m_BrowserView.treeViewProc.SelectedNode.Tag as CustomVariableItem;
                                if (varItem != null && MainForm.App.roleSelectedInPropertyGrid == false && MainForm.App.variableSelectedInPropertyGrid == false
                                    && MainForm.App.formSelectedInPropertyGrid == false)
                                {
                                    if (MainForm.App.GetCurrentView() != null)
                                    {
                                        MainForm.App.GetCurrentView().Selection.Clear();
                                    }
                                    this.View_OnWorkFlowItemSelected(varItem);
                                }
                            }
                        }
                    }
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(holdObject);
                }
            }
            base.OnVisibleChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x10)
            {
                if (handledClose == false)
                {
                    MainForm.App.OnProcessViewActivated -= new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
                    MainForm.App.OnCustomVariableItemSelected -= new MainForm.CustomVariableItemSelectedHandler(App_OnCustomVariableItemSelected);
                    MainForm.App.OnProcessViewDeactivated -= new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
                    MainForm.App.OnCustomFormItemSelected -= new MainForm.CustomFormItemSelectedHandler(App_OnCustomFormItemSelected);
                    MainForm.App.OnRoleItemSelected -= new MainForm.RoleItemSelectedHandler(App_OnRoleItemSelected);

                    DetachProperties();
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().Focus();
                    }
                    handledClose = true;
                }
            }
            base.WndProc(ref m);
        }

        #endregion Overrides

        #region Event Handlers

        private void App_OnProcessViewDeactivated(ProcessView view)
        {
            m_OwnSelection = false;
            try
            {
                if (this.propertyGrid.SelectedObject != null)
                {
                    this.propertyGrid.SelectedObject = null;
                    cbxProperties.SelectedIndex = -1;
                }
            }
            catch
            {
            }
            DetachProperties();
        }

        private void App_OnProcessViewActivated(ProcessView view)
        {
            AttachProperties();
        }

        private void View_OnWorkFlowItemSelected(IWorkflowItem SelectedItem)
        {
            if (MainForm.App.PastingMultipleSelection == false)
            {
                if (this.Visible)
                {
                    if (m_OwnSelection == false && MainForm.App.documentIsBeingOpened == false)
                    {
                        if (MainForm.App.GetCurrentView() != null)
                        {
                            if (MainForm.App.GetCurrentView().Selection.Count < 2)
                            {
                                m_OwnSelection = true;
                                propertyGrid.SelectedObject = SelectedItem.Properties;
                                populateDropDown();
                            }
                        }
                    }
                }
            }
        }

        private void View_OnWorkFlowItemUnSelected(IWorkflowItem SelectedItem)
        {
            if (!this.Visible)
            {
                return;
            }
            m_OwnSelection = false;
            propertyGrid.SelectedObject = null;
            cbxProperties.SelectedIndex = -1;
        }

        private void GeneralCustomFormClosed(GeneralCustomFormType type)
        {
            if (this.propertyGrid.SelectedObject != null)
            {
                object o = this.propertyGrid.SelectedObject;
                this.propertyGrid.SelectedObject = null;
                this.propertyGrid.SelectedObject = o;
            }
        }

        #endregion Event Handlers

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (this.propertyGrid.SelectedObject is CustomVariableItemProperties)
                {
                    MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.CustomVariable);
                }
                else if (this.propertyGrid.SelectedObject is CustomFormItemProperties)
                {
                    MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.CustomForm);
                }
                if (e.ChangedItem.Label == "UseAutoForward")
                {
                    MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.AutoForward);
                }

                if (e.ChangedItem.Label == "IsDynamic")
                {
                    MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.IsDynamic);
                }

                if (e.ChangedItem.Label == "WorkFlowName")
                {
                    X2Generator.RenameWorkFlowInCodeHeaders(MainForm.App.GetCurrentView().Document.CurrentWorkFlow, e.OldValue.ToString());
                }

                if (e.ChangedItem.Label == "StateName" || e.ChangedItem.Label == "ActivityName"
                    || e.ChangedItem.Label == "Role" || e.ChangedItem.Label == "Name" || e.ChangedItem.Label.IndexOf("Name") > -1)
                {
                    MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.Name);
                }

                if (e.ChangedItem.Label == "StageTransitionMessage")
                {
                    BaseActivity b = MainForm.App.GetCurrentView().Selection.Primary as BaseActivity;
                    if (b != null)
                    {
                        MainForm.App.GetCurrentView().FireOnStageTransitionMessageChangedEvent(e.ChangedItem.Value.ToString());
                    }
                }

                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void PropertiesView_Enter(object sender, EventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().justSelectedObject = false;
            }
        }

        private void propertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (propertyGrid.SelectedGridItem != null)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().justSelectedObject == false && propertyGrid.SelectedGridItem != null)
                    {
                        lastSelectedProperty = propertyGrid.SelectedGridItem.Label;
                        MainForm.App.GetCurrentView().justSelectedObject = false;
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetFocus();

        private void SelectStageTransitionMessage(Keys keyCode)
        {
            if (MainForm.App.m_PropsView.propertyGrid.SelectedGridItem == null)
            {
                return;
            }
            GridItem p_gi = MainForm.App.m_PropsView.propertyGrid.SelectedGridItem.Parent.Parent;
            bool found = false;
            foreach (GridItem egi in p_gi.GridItems)
            {
                foreach (GridItem eegi in egi.GridItems)
                {
                    System.Diagnostics.Debug.WriteLine(eegi.Label);

                    if (eegi.Label.Contains("StageTransitionMessage"))
                    {
                        found = true;
                        break;
                    }
                }
                if (found == true)
                {
                    IntPtr FHandle = GetFocus();
                    PostMessage(FHandle, 0x100, m_CurrentVK, m_CurrentSC);
                    break;
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(

            IntPtr hWnd, // handle to destination window
            UInt32 Msg, // message
            Int32 wParam, // first message parameter
            Int32 lParam // second message parameter

        );

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().justSelectedObject = false;
            }
            base.OnKeyDown(e);
        }

        public void fireKey(Keys keyToFire)
        {
            KeyEventArgs mKeyEventArgs = new KeyEventArgs(keyToFire);
        }
    }
}