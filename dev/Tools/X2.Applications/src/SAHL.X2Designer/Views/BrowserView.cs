using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer.Views
{
    public partial class BrowserView : DockContent
    {
        public Dictionary<GoObject, TreeNode> hashTable;
        public Dictionary<IWorkflowItem, TreeNode> hashTableRole;

        public BrowserView()
        {
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);

            hashTable = new Dictionary<GoObject, TreeNode>();
            hashTableRole = new Dictionary<IWorkflowItem, TreeNode>();

            InitializeComponent();
            PopulateBrowser();
            AttachTree();
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection.Count > 0)
                {
                    BaseItem mItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mItem);
                }
            }
        }

        #region Misc

        private void AttachTree()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(View_OnWorkFlowItemUnSelected);

                MainForm.App.GetCurrentView().OnObjectChanged += new ProcessView.OnObjectChangedHandler(PV_OnObjectChanged);
                MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(GeneralCustomFormClosed);
                MainForm.App.GetCurrentView().OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(BrowserView_OnPropertyChanged);
            }
        }

        private void DetachTree()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(View_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected -= new ProcessView.OnWorkFlowItemUnSelectedHandler(View_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnObjectChanged -= new ProcessView.OnObjectChangedHandler(PV_OnObjectChanged);
                MainForm.App.OnGeneralCustomFormClosed -= new MainForm.GeneralCustomFormClosedHandler(GeneralCustomFormClosed);
                MainForm.App.GetCurrentView().OnPropertyChanged -= new ProcessView.OnPropertyChangedHandler(BrowserView_OnPropertyChanged);
            }
        }

        public void PopulateBrowser()
        {
            if (treeViewProc.IsDisposed == true)
            {
                return;
            }
            treeViewProc.Nodes.Clear();
            hashTable.Clear();

            if (hashTableRole != null)
            {
                hashTableRole.Clear();
            }

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableForm.Clear();
                MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableVar.Clear();
                MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableRoles.Clear();
            }

            if (MainForm.App.GetCurrentView() != null)
            {
                treeViewProc.ImageList = this.imageListBrowser;
                TreeNode documentNode = new TreeNode();
                documentNode.Text = "Process";
                documentNode.ImageIndex = 0;
                treeViewProc.Nodes.Add(documentNode);
                TreeNode roleNode = new TreeNode();
                roleNode.Text = "Global Roles";
                roleNode.ImageIndex = 12;
                roleNode.SelectedImageIndex = 12;
                documentNode.Nodes.Add(roleNode);
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].Name.ToLower() != "originator")
                    {
                        if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.Global)
                        {
                            TreeNode mNode = new TreeNode();
                            mNode.Text = MainForm.App.GetCurrentView().Document.Roles[x].Name.ToString();
                            mNode.Tag = MainForm.App.GetCurrentView().Document.Roles[x];
                            mNode.ImageIndex = 22;
                            mNode.SelectedImageIndex = 22;
                            hashTableRole.Add(MainForm.App.GetCurrentView().Document.Roles[x] as IWorkflowItem, mNode);
                            roleNode.Nodes.Add(mNode);
                        }
                    }
                }

                for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                {
                    TreeNode workflowNode = new TreeNode();
                    workflowNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName;
                    workflowNode.ImageIndex = 1;
                    workflowNode.SelectedImageIndex = 1;
                    workflowNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[x];
                    hashTable.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x], workflowNode);
                    documentNode.Nodes.Add(workflowNode);
                    TreeNode WorkFlowRoleContainerNode = new TreeNode();
                    WorkFlowRoleContainerNode.Text = "WorkFlow Roles";
                    WorkFlowRoleContainerNode.ImageIndex = 12;
                    WorkFlowRoleContainerNode.SelectedImageIndex = 12;
                    workflowNode.Nodes.Add(WorkFlowRoleContainerNode);
                    for (int y = 0; y < MainForm.App.GetCurrentView().Document.Roles.Count; y++)
                    {
                        if (MainForm.App.GetCurrentView().Document.Roles[y].RoleType == RoleType.WorkFlow && MainForm.App.GetCurrentView().Document.Roles[y].WorkFlowItem == MainForm.App.GetCurrentView().Document.WorkFlows[x])
                        {
                            TreeNode CusRoleNode = new TreeNode();
                            CusRoleNode.Text = MainForm.App.GetCurrentView().Document.Roles[y].Name;
                            CusRoleNode.Tag = MainForm.App.GetCurrentView().Document.Roles[y];
                            CusRoleNode.ImageIndex = 22;
                            CusRoleNode.SelectedImageIndex = 22;
                            MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableRoles.Add(MainForm.App.GetCurrentView().Document.Roles[y] as IWorkflowItem, workflowNode);
                            WorkFlowRoleContainerNode.Nodes.Add(CusRoleNode);
                        }
                    }
                    TreeNode CustomVariableContainerNode = new TreeNode();
                    CustomVariableContainerNode.Text = "Custom Variables";
                    CustomVariableContainerNode.ImageIndex = 14;
                    CustomVariableContainerNode.SelectedImageIndex = 14;
                    workflowNode.Nodes.Add(CustomVariableContainerNode);
                    for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables.Count; y++)
                    {
                        TreeNode CustVarNode = new TreeNode();
                        CustVarNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[y].Name;
                        CustVarNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[y];
                        CustVarNode.ImageIndex = 14;
                        CustVarNode.SelectedImageIndex = 14;
                        MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableVar.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x].CustomVariables[y] as IWorkflowItem, CustVarNode);
                        CustomVariableContainerNode.Nodes.Add(CustVarNode);
                    }
                    TreeNode CustomFormsContainerNode = new TreeNode();
                    CustomFormsContainerNode.Text = "Custom Forms";
                    CustomFormsContainerNode.ImageIndex = 18;
                    CustomFormsContainerNode.SelectedImageIndex = 18;
                    workflowNode.Nodes.Add(CustomFormsContainerNode);
                    for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms.Count; y++)
                    {
                        TreeNode CustFormNode = new TreeNode();
                        CustFormNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms[y].Name;
                        CustFormNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms[y];
                        CustFormNode.ImageIndex = 15;
                        CustFormNode.SelectedImageIndex = 15;
                        MainForm.App.GetCurrentView().Document.WorkFlows[x].hashTableForm.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x].Forms[y] as IWorkflowItem, CustFormNode);
                        CustomFormsContainerNode.Nodes.Add(CustFormNode);
                    }

                    TreeNode StateContainer = new TreeNode();
                    StateContainer.Text = "States";
                    StateContainer.ImageIndex = 17;
                    StateContainer.SelectedImageIndex = 17;
                    TreeNode ActivityContainer = new TreeNode();
                    ActivityContainer.ImageIndex = 16;
                    ActivityContainer.SelectedImageIndex = 16;
                    ActivityContainer.Text = "Activities";
                    TreeNode CommentContainer = new TreeNode();
                    CommentContainer.ImageIndex = 19;
                    CommentContainer.SelectedImageIndex = 19;
                    CommentContainer.Text = "Comments";
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[x])
                    {
                        BaseItem bi = o as BaseItem;

                        if (o.GetType() == typeof(ClapperBoard))
                        {
                            TreeNode clapperNode = new TreeNode();
                            clapperNode.Text = "Starting Point";
                            clapperNode.Tag = o;
                            BaseItem i = o as BaseItem;
                            hashTable.Add(i, clapperNode);
                            clapperNode.ImageIndex = 2;
                            clapperNode.SelectedImageIndex = 2;
                            workflowNode.Nodes.Add(clapperNode);
                            clapperNode.Nodes.Add(StateContainer);
                            clapperNode.Nodes.Add(ActivityContainer);
                            clapperNode.Nodes.Add(CommentContainer);
                        }
                        if (o is BaseState)
                        {
                            TreeNode stateNode = new TreeNode();

                            stateNode.Text = bi.Name;
                            stateNode.Tag = o;
                            BaseItem i = o as BaseItem;
                            if (i != null)
                            {
                                hashTable.Add(i, stateNode);
                            }
                            stateNode.ImageIndex = GetImageIndex(o);
                            stateNode.SelectedImageIndex = stateNode.ImageIndex;

                            StateContainer.Nodes.Add(stateNode);
                        }
                        if (o is BaseActivity)
                        {
                            TreeNode activityNode = new TreeNode();
                            activityNode.Text = bi.Name;
                            activityNode.Tag = o;
                            hashTable.Add(o as BaseItem, activityNode);
                            activityNode.ImageIndex = GetImageIndex(o);
                            activityNode.SelectedImageIndex = activityNode.ImageIndex;
                            ActivityContainer.Nodes.Add(activityNode);
                        }
                        if (o.GetType() == typeof(Comment))
                        {
                            TreeNode commentNode = new TreeNode();
                            Comment mCom = o as Comment;
                            commentNode.Text = "Comment" + mCom.CommentID.ToString();
                            commentNode.Tag = o;
                            hashTable.Add(o as BaseItem, commentNode);
                            commentNode.ImageIndex = 19;
                            commentNode.SelectedImageIndex = 19;
                            CommentContainer.Nodes.Add(commentNode);
                        }
                    }
                }
            }
        }

        private int GetImageIndex(GoObject o)
        {
            BaseItem i = o as BaseItem;
            switch (i.WorkflowItemType)
            {
                case WorkflowItemType.UserState:
                    {
                        return 3;
                    }
                case WorkflowItemType.SystemState:
                    {
                        return 4;
                    }
                case WorkflowItemType.CommonState:
                    {
                        return 5;
                    }
                case WorkflowItemType.ArchiveState:
                    {
                        return 6;
                    }

                case WorkflowItemType.UserActivity:
                    {
                        return 7;
                    }
                case WorkflowItemType.ExternalActivity:
                    {
                        return 8;
                    }
                case WorkflowItemType.TimedActivity:
                    {
                        return 9;
                    }
                case WorkflowItemType.ConditionalActivity:
                    {
                        return 10;
                    }
                case WorkflowItemType.SystemDecisionState:
                    {
                        return 20;
                    }
                case WorkflowItemType.CallWorkFlowActivity:
                    {
                        return 21;
                    }
                case WorkflowItemType.ReturnWorkFlowActivity:
                    {
                        return 1;
                    }
                case WorkflowItemType.HoldState:
                    {
                        return 23;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }

        private void treeViewProc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewProc.SelectedNode != null)
            {
                if (treeViewProc.SelectedNode.Tag != null)
                {
                    BaseItem i = treeViewProc.SelectedNode.Tag as BaseItem;
                    if (i != null)
                    {
                        if (MainForm.App.GetCurrentView() != null)
                        {
                            if (MainForm.App.GetCurrentView().Selection.Primary != i)
                            {
                                MainForm.App.GetCurrentView().Document.StartTransaction();
                                MainForm.App.GetCurrentView().Document.SelectWorkFlow(i.Parent as WorkFlow);
                                MainForm.App.GetCurrentView().Selection.Clear();
                                MainForm.App.GetCurrentView().Selection.Add(i);

                                MainForm.App.GetCurrentView().FinishTransaction("Change BrowserView Node");
                            }
                        }
                    }

                    RolesCollectionItem roleItem = treeViewProc.SelectedNode.Tag as RolesCollectionItem;
                    if (roleItem != null && MainForm.App.roleSelectedInPropertyGrid == false && MainForm.App.variableSelectedInPropertyGrid == false
                        && MainForm.App.formSelectedInPropertyGrid == false)
                    {
                        MainForm.App.FireRoleItemSelectedEvent(roleItem);
                    }

                    if (treeViewProc.SelectedNode != null)
                    {
                        CustomVariableItem varItem = treeViewProc.SelectedNode.Tag as CustomVariableItem;
                        if (varItem != null && MainForm.App.roleSelectedInPropertyGrid == false && MainForm.App.variableSelectedInPropertyGrid == false
                        && MainForm.App.formSelectedInPropertyGrid == false)
                        {
                            if (MainForm.App.GetCurrentView() != null)
                            {
                                MainForm.App.GetCurrentView().Selection.Clear();
                            }
                            if (MainForm.App.m_CodeView != null)
                            {
                                MainForm.App.m_CodeView.ClearView();
                            }
                            MainForm.App.FireCustomVariableItemSelectedEvent(varItem);
                        }
                    }

                    if (treeViewProc.SelectedNode != null)
                    {
                        CustomFormItem formItem = treeViewProc.SelectedNode.Tag as CustomFormItem;
                        if (formItem != null && MainForm.App.roleSelectedInPropertyGrid == false && MainForm.App.variableSelectedInPropertyGrid == false
                        && MainForm.App.formSelectedInPropertyGrid == false)
                        {
                            if (MainForm.App.GetCurrentView() != null)
                            {
                                MainForm.App.GetCurrentView().Selection.Clear();
                            }
                            if (MainForm.App.m_CodeView != null)
                            {
                                MainForm.App.m_CodeView.ClearView();
                            }
                            MainForm.App.FireCustomFormItemSelectedEvent(formItem);
                        }
                    }

                    if (MainForm.App.m_PropsView != null)
                    {
                        if (e.Node.Tag.GetType() == typeof(RolesCollectionItem))
                        {
                            for (int x = 0; x < MainForm.App.m_PropsView.lstCombinedNodes.Count; x++)
                            {
                                RolesCollectionItem ri = e.Node.Tag as RolesCollectionItem;
                                if (MainForm.App.m_PropsView.lstCombinedNodes[x].Name == ri.Name && MainForm.App.m_PropsView.lstCombinedNodes[x].GetType() == typeof(RolesCollectionItem))
                                {
                                    MainForm.App.m_PropsView.cbxProperties.SelectedIndex = x;
                                    break;
                                }
                            }
                        }
                    }

                    if (MainForm.App.m_PropsView != null)
                    {
                        if (e.Node.Tag.GetType() == typeof(CustomVariableItem))
                        {
                            for (int x = 0; x < MainForm.App.m_PropsView.lstCombinedNodes.Count; x++)
                            {
                                CustomVariableItem cvi = e.Node.Tag as CustomVariableItem;
                                if (MainForm.App.m_PropsView.lstCombinedNodes[x].Name == cvi.Name && MainForm.App.m_PropsView.lstCombinedNodes[x].GetType() == typeof(CustomVariableItem))
                                {
                                    MainForm.App.m_PropsView.cbxProperties.SelectedIndex = x;
                                    break;
                                }
                            }
                        }
                    }

                    if (MainForm.App.m_PropsView != null)
                    {
                        if (e.Node.Tag.GetType() == typeof(CustomFormItem))
                        {
                            for (int x = 0; x < MainForm.App.m_PropsView.lstCombinedNodes.Count; x++)
                            {
                                CustomFormItem cfi = e.Node.Tag as CustomFormItem;
                                if (MainForm.App.m_PropsView.lstCombinedNodes[x].Name == cfi.Name && MainForm.App.m_PropsView.lstCombinedNodes[x].GetType() == typeof(CustomFormItem))
                                {
                                    MainForm.App.m_PropsView.cbxProperties.SelectedIndex = x;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().Selection.Clear();
                    }
                    if (MainForm.App.m_PropsView != null)
                    {
                        if (MainForm.App.m_PropsView.propertyGrid.SelectedObject != null)
                        {
                            MainForm.App.m_PropsView.propertyGrid.SelectedObject = null;
                        }
                    }
                    if (MainForm.App.m_CodeView != null)
                    {
                        MainForm.App.m_CodeView.ClearView();
                    }
                }
            }
        }

        public void RefreshRoles()
        {
            treeViewProc.Nodes[0].Nodes[0].Nodes.Clear();
            hashTableRole.Clear();

            if (MainForm.App.GetCurrentView() != null)
            {
                TreeNode currentWorkFlowNode = new TreeNode();

                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles.Clear();
                bool res = hashTable.TryGetValue(MainForm.App.GetCurrentView().Document.CurrentWorkFlow, out currentWorkFlowNode);
                if (res == true)
                {
                    currentWorkFlowNode.Nodes[0].Nodes.Clear();
                }
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                {
                    TreeNode mNode = new TreeNode();
                    mNode.Text = MainForm.App.GetCurrentView().Document.Roles[x].Name.ToString();
                    mNode.Tag = MainForm.App.GetCurrentView().Document.Roles[x];
                    mNode.ImageIndex = 22;
                    mNode.SelectedImageIndex = 22;
                    if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.Global)
                    {
                        this.treeViewProc.Nodes[0].Nodes[0].Nodes.Add(mNode);
                        hashTableRole.Add(MainForm.App.GetCurrentView().Document.Roles[x] as IWorkflowItem, mNode);
                    }
                    else
                    {
                        RolesCollectionItem mInstance = mNode.Tag as RolesCollectionItem;
                        if (mInstance.WorkFlowItem == currentWorkFlowNode.Tag as WorkFlow)
                        {
                            currentWorkFlowNode.Nodes[0].Nodes.Add(mNode);
                            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles.Add(MainForm.App.GetCurrentView().Document.Roles[x] as IWorkflowItem, mNode);
                        }
                    }
                }
            }
        }

        public void RefreshVariables()
        {
            TreeNode currentWorkFlowNode = new TreeNode();
            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.Clear();
            bool res = hashTable.TryGetValue(MainForm.App.GetCurrentView().Document.CurrentWorkFlow, out currentWorkFlowNode);
            if (res == true)
            {
                currentWorkFlowNode.Nodes[1].Nodes.Clear();

                if (MainForm.App.GetCurrentView() != null)
                {
                    for (int y = 0; y < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; y++)
                    {
                        TreeNode mNode = new TreeNode();
                        mNode.Text = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[y].Name.ToString();
                        mNode.Tag = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[y];
                        mNode.ImageIndex = 14;
                        mNode.SelectedImageIndex = 14;
                        currentWorkFlowNode.Nodes[1].Nodes.Add(mNode);
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[y] as IWorkflowItem, mNode);
                    }
                }
            }
        }

        public void RefreshForms()
        {
            TreeNode currentFormNode = new TreeNode();
            MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm.Clear();
            bool res = hashTable.TryGetValue(MainForm.App.GetCurrentView().Document.CurrentWorkFlow, out currentFormNode);
            if (res == true)
            {
                currentFormNode.Nodes[2].Nodes.Clear();

                if (MainForm.App.GetCurrentView() != null)
                {
                    for (int y = 0; y < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; y++)
                    {
                        TreeNode mNode = new TreeNode();
                        mNode.Text = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[y].Name.ToString();
                        mNode.Tag = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[y];
                        mNode.ImageIndex = 15;
                        mNode.SelectedImageIndex = 15;
                        currentFormNode.Nodes[2].Nodes.Add(mNode);
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[y] as IWorkflowItem, mNode);
                    }
                }
            }
        }

        public void SetupBrowser()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1])
                {
                    BaseItem bi = o as BaseItem;

                    if (o.GetType() == typeof(ClapperBoard))
                    {
                        if (treeViewProc.Nodes[0].LastNode.Nodes.Count == 0)
                        {
                            TreeNode WorkFlowRoleContainerNode = new TreeNode();
                            WorkFlowRoleContainerNode.Text = "WorkFlow Roles";
                            WorkFlowRoleContainerNode.ImageIndex = 12;
                            WorkFlowRoleContainerNode.SelectedImageIndex = 12;
                            treeViewProc.Nodes[0].LastNode.Nodes.Add(WorkFlowRoleContainerNode);
                            for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].WorkFlowRoles.Count; y++)
                            {
                                TreeNode CusRoleNode = new TreeNode();
                                CusRoleNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables[y].Name;
                                CusRoleNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables[y];
                                CusRoleNode.ImageIndex = 22;
                                CusRoleNode.SelectedImageIndex = 22;
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles.Add(MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].WorkFlowRoles[y] as IWorkflowItem, CusRoleNode);
                                WorkFlowRoleContainerNode.Nodes.Add(CusRoleNode);
                            }

                            TreeNode CustomVariableContainerNode = new TreeNode();
                            CustomVariableContainerNode.Text = "Custom Variables";
                            CustomVariableContainerNode.ImageIndex = 14;
                            CustomVariableContainerNode.SelectedImageIndex = 14;
                            treeViewProc.Nodes[0].LastNode.Nodes.Add(CustomVariableContainerNode);
                            for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables.Count; y++)
                            {
                                TreeNode CustVarNode = new TreeNode();
                                CustVarNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables[y].Name;
                                CustVarNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables[y];
                                CustVarNode.ImageIndex = 14;
                                CustVarNode.SelectedImageIndex = 14;
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.Add(MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].CustomVariables[y] as IWorkflowItem, CustVarNode);
                                CustomVariableContainerNode.Nodes.Add(CustVarNode);
                            }
                            TreeNode CustomFormsContainerNode = new TreeNode();
                            CustomFormsContainerNode.Text = "Custom Forms";
                            CustomFormsContainerNode.ImageIndex = 18;
                            CustomFormsContainerNode.SelectedImageIndex = 18;
                            treeViewProc.Nodes[0].LastNode.Nodes.Add(CustomFormsContainerNode);
                            for (int y = 0; y < MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].Forms.Count; y++)
                            {
                                TreeNode CustFormNode = new TreeNode();
                                CustFormNode.Text = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].Forms[y].Name;
                                CustFormNode.Tag = MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].Forms[y];
                                CustFormNode.ImageIndex = 15;
                                CustFormNode.SelectedImageIndex = 15;
                                MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].hashTableForm.Add(MainForm.App.GetCurrentView().Document.WorkFlows[MainForm.App.GetCurrentView().Document.WorkFlows.Length - 1].Forms[y] as IWorkflowItem, CustFormNode);
                                CustomFormsContainerNode.Nodes.Add(CustFormNode);
                            }

                            TreeNode StateContainer = new TreeNode();
                            StateContainer.Text = "States";
                            StateContainer.ImageIndex = 17;
                            StateContainer.SelectedImageIndex = 17;
                            TreeNode ActivityContainer = new TreeNode();
                            ActivityContainer.ImageIndex = 16;
                            ActivityContainer.SelectedImageIndex = 16;
                            ActivityContainer.Text = "Activities";
                            TreeNode CommentContainer = new TreeNode();
                            CommentContainer.ImageIndex = 19;
                            CommentContainer.SelectedImageIndex = 19;
                            CommentContainer.Text = "Comments";

                            TreeNode clapperNode = new TreeNode();
                            clapperNode.Text = "Starting Point";
                            clapperNode.Tag = o;
                            BaseItem i = o as BaseItem;
                            hashTable.Add(i, clapperNode);
                            clapperNode.ImageIndex = 2;
                            clapperNode.SelectedImageIndex = 2;
                            treeViewProc.Nodes[0].LastNode.Nodes.Add(clapperNode);

                            clapperNode.Nodes.Add(StateContainer);
                            clapperNode.Nodes.Add(ActivityContainer);
                            clapperNode.Nodes.Add(CommentContainer);
                        }
                    }
                }
            }
        }

        #endregion Misc

        #region Overrides

        protected override void OnClosed(EventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.OnProcessViewActivated -= new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
                MainForm.App.OnProcessViewDeactivated -= new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
                MainForm.App.menuBrowserChecked(false);
                DetachTree();
            }
            base.OnClosed(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible == true)
            {
            }
            if (this.Visible == false)
            {
            }

            base.OnVisibleChanged(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x8)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().Focus();
                }
            }
            base.WndProc(ref m);
        }

        #endregion Overrides

        #region Event Handlers

        private void App_OnProcessViewDeactivated(ProcessView view)
        {
            DetachTree();
            hashTable = null;

            treeViewProc.Nodes.Clear();
        }

        private void App_OnProcessViewActivated(ProcessView view)
        {
            if (hashTable == null)
            {
                hashTable = new Dictionary<GoObject, TreeNode>();
                PopulateBrowser();
            }
            AttachTree();
        }

        private void View_OnWorkFlowItemSelected(IWorkflowItem SelectedItem)
        {
            if (MainForm.App.PastingMultipleSelection == false)
            {
                if (hashTable == null)
                {
                    return;
                }
                if (SelectedItem as BaseItem != null && MainForm.App.documentIsBeingOpened == false)
                {
                    if (MainForm.App.GetCurrentView().Selection.Count < 2)
                    {
                        BaseItem mItem = SelectedItem as BaseItem;
                        TreeNode x = null;
                        if (hashTable.TryGetValue(SelectedItem as BaseItem, out x) == true)
                        {
                            treeViewProc.SelectedNode = null;
                            treeViewProc.SelectedNode = x;
                        }
                    }
                }
                else
                {
                }
            }
        }

        private void View_OnWorkFlowItemUnSelected(IWorkflowItem SelectedItem)
        {
            treeViewProc.SelectedNode = null;
        }

        private void PV_OnObjectChanged(ObjectChangeType ChangeType, GoChangedEventArgs e)
        {
            try
            {
                if (e.GoObject == null)
                {
                    return;
                }
                if (hashTable == null)
                {
                    return;
                }
                if (MainForm.App.documentIsBeingOpened == false)
                {
                    switch (ChangeType)
                    {
                        case ObjectChangeType.Inserted:
                            {
                                if (e.NewValue is BaseState || e.NewValue is BaseActivity
                                 || e.NewValue.GetType() == typeof(Comment))
                                {
                                    TreeNode x = null;
                                    bool res = hashTable.TryGetValue(MainForm.App.GetCurrentView().Document.CurrentWorkFlow, out x);
                                    if (res == true)
                                    {
                                        if (e.NewValue is BaseState)
                                        {
                                            TreeNode newState = new TreeNode();
                                            BaseState bs = e.NewValue as BaseState;
                                            newState.Text = bs.Text;
                                            newState.Tag = e.NewValue;
                                            newState.ImageIndex = GetImageIndex(e.NewValue as GoObject);
                                            newState.SelectedImageIndex = newState.ImageIndex;
                                            x.Nodes[3].Nodes[0].Nodes.Add(newState);
                                            hashTable.Add(e.NewValue as GoObject, newState);
                                        }
                                        else if (e.NewValue is BaseActivity)
                                        {
                                            TreeNode newActivity = new TreeNode();
                                            BaseActivity ba = e.NewValue as BaseActivity;
                                            newActivity.Text = ba.Text;
                                            newActivity.Tag = e.NewValue;
                                            newActivity.ImageIndex = GetImageIndex(e.NewValue as GoObject);
                                            newActivity.SelectedImageIndex = newActivity.ImageIndex;
                                            x.Nodes[3].Nodes[1].Nodes.Add(newActivity);
                                            hashTable.Add(e.NewValue as GoObject, newActivity);
                                        }
                                        else if (e.NewValue.GetType() == typeof(Comment))
                                        {
                                            TreeNode newComment = new TreeNode();
                                            Comment mComment = e.NewValue as Comment;
                                            newComment.Text = "Comment" + mComment.CommentID;
                                            newComment.Tag = e.NewValue;
                                            newComment.ImageIndex = 19;
                                            newComment.SelectedImageIndex = 19;
                                            x.Nodes[2].Nodes[2].Nodes.Add(newComment);
                                            hashTable.Add(e.NewValue as GoObject, newComment);
                                        }
                                    }
                                }

                                break;
                            }
                        case ObjectChangeType.Deleted:
                            {
                                if (e.OldValue is BaseState | e.OldValue is BaseActivity
                                   | e.OldValue.GetType() == typeof(Comment))
                                {
                                    TreeNode x = null;
                                    bool res = hashTable.TryGetValue(e.OldValue as GoObject, out x);
                                    if (res != false)
                                    {
                                        x.Remove();
                                    }
                                }
                                break;
                            }
                        case ObjectChangeType.Renamed:
                            {
                                TreeNode x = null;
                                if (e.GoObject.Parent is BaseState || e.GoObject.Parent is BaseActivity
                                || e.GoObject.Parent.GetType() == typeof(WorkFlow))
                                {
                                    BaseItem mItem = e.GoObject.Parent as BaseItem;
                                    bool res = hashTable.TryGetValue(e.GoObject.Parent, out x);
                                    if (res != false)
                                    {
                                        x.Text = e.NewValue.ToString();
                                    }
                                }
                                break;
                            }
                        case ObjectChangeType.WorkFlowInserted:
                            {
                                TreeNode n = new TreeNode();
                                n.ImageIndex = 1;
                                WorkFlow m = e.GoObject as WorkFlow;
                                n.Text = m.WorkFlowName;
                                n.Tag = e.GoObject;
                                treeViewProc.Nodes[0].Nodes.Add(n);
                                hashTable.Add(e.GoObject, n);
                                break;
                            }
                        case ObjectChangeType.WorkFlowDeleted:
                            {
                                TreeNode x = null;
                                GoObject mItem = e.GoObject as GoObject;
                                bool res = hashTable.TryGetValue(mItem, out x);
                                if (res != false)
                                {
                                    x.Remove(); ;
                                }
                                break;
                            }
                    }
                }
            }
            catch
            {
            }
        }

        private void BrowserView_OnPropertyChanged(PropertyType propType)
        {
            switch (propType)
            {
                case PropertyType.Name:
                    {
                        RefreshRoles();
                        GoObject o = MainForm.App.GetCurrentView().Selection.Primary;
                        MainForm.App.GetCurrentView().Selection.Clear();
                        MainForm.App.GetCurrentView().Selection.Add(o);
                        break;
                    }
                case PropertyType.CustomVariable:
                    {
                        RefreshVariables();
                        break;
                    }

                case PropertyType.CustomForm:
                    {
                        RefreshForms();
                        break;
                    }
            }
        }

        private void GeneralCustomFormClosed(GeneralCustomFormType type)
        {
            GoObject holdSelection = null;
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection.Primary != null)
                {
                    holdSelection = MainForm.App.GetCurrentView().Selection.Primary;
                }
            }
            PopulateBrowser();
            if (holdSelection != null)
            {
                MainForm.App.GetCurrentView().Selection.Clear();
                MainForm.App.GetCurrentView().Selection.Add(holdSelection);
            }
        }

        #endregion Event Handlers

        private void treeViewProc_ChangeUICues(object sender, UICuesEventArgs e)
        {
        }

        private void treeViewProc_Click(object sender, EventArgs e)
        {
        }
    }
}