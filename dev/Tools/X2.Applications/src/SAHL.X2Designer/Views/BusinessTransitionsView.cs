using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer.Views
{
    public partial class BusinessStageTransitionsView : DockContent
    {
        private TreeView treeView;
        private List<itm> lstChildGroupNodes;
        //private IBusinessStageTransitions m_StageTransitions;
        List<itm> lstGroups;
        List<itm> lstDefinitions;
        string ConnString = "";
        private List<BusinessStageItem> lstBusinessStageItems;
        List<BusinessStageItem> m_StageTransitions;

        public BusinessStageTransitionsView()
        {
            InitializeComponent();
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(BusinessStageTransitions_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(BusinessStageTransitions_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(BusinessTransitionsView_OnPropertyChanged);
            }
        }

        private void App_OnGeneralCustomFormClosed(SAHL.X2Designer.Items.GeneralCustomFormType formType)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mSelection = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mSelection);
                }
            }
        }

        private void App_OnProcessViewDeactivated(ProcessView v)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(BusinessStageTransitions_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected -= new ProcessView.OnWorkFlowItemUnSelectedHandler(BusinessStageTransitions_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged -= new ProcessView.OnPropertyChangedHandler(BusinessTransitionsView_OnPropertyChanged);
            }
            ClearView();
        }

        private void App_OnProcessViewActivated(ProcessView v)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(BusinessStageTransitions_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(BusinessStageTransitions_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(BusinessTransitionsView_OnPropertyChanged);
            }
        }

        private void BusinessStageTransitions_OnWorkFlowItemSelected(SAHL.X2Designer.Items.IWorkflowItem SelectedItem)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection.Count > 1)
                {
                    ClearView();
                    return;
                }
            }

            IBusinessStageTransitions bst = SelectedItem as IBusinessStageTransitions;
            if (null == bst) return;
            m_StageTransitions = bst.BusinessStageTransitions;

            if (m_StageTransitions != null)
            {
                treeView = new TreeView();
                if (BuildStageTransitionTree() == DialogResult.Cancel)
                {
                    return;
                }
                //checkedBox1.Sorted = true;
                tabControlMain.TabPages.Add(new TabPage("Business Stage Transitions"));
                tabControlMain.TabPages[0].Controls.Add(treeView);
                treeView.Dock = DockStyle.Fill;
                treeView.CheckBoxes = true;
                PopulateStageTransitions();
                treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView_NodeMouseClick);
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView.HitTest(e.X, e.Y).Location == TreeViewHitTestLocations.StateImage)
            {
                loops(e.Node.Nodes, e.Node.Checked);
            }

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }

            m_StageTransitions.Clear();
            RecursivelyAddSelectedTransitions(treeView.Nodes);

            IBusinessStageTransitions ibst = MainForm.App.GetCurrentView().Selection.Primary as IBusinessStageTransitions;
            if (null != ibst)
            {
                ibst.BusinessStageTransitions = m_StageTransitions;
            }
        }

        private void loops(TreeNodeCollection nods, Boolean iSchecked)
        {
            //prevent index or error..recurrent loop to check all child nodes..

            if (nods.Count > 0)
            {
                foreach (TreeNode nod in nods)
                {
                    nod.Checked = iSchecked;

                    loops(nod.Nodes, iSchecked);
                }
            }
        }

        private void RecursivelyAddSelectedTransitions(TreeNodeCollection nods)
        {
            //prevent index or error..recurrent loop to check all child nodes..

            if (nods.Count > 0)
            {
                foreach (TreeNode nod in nods)
                {
                    if (nod.Checked)
                    {
                        foreach (BusinessStageItem i in lstBusinessStageItems)
                        {
                            if (nod.Tag == null)
                            {
                                continue;
                            }
                            if (i.SDSDGKey == nod.Tag.ToString())
                            {
                                m_StageTransitions.Add(i);
                                break;
                            }
                        }
                    }

                    RecursivelyAddSelectedTransitions(nod.Nodes);
                }
            }
        }

        private void BusinessStageTransitions_OnWorkFlowItemUnSelected(SAHL.X2Designer.Items.IWorkflowItem UnSelectedItem)
        {
            ClearView();
        }

        private void BusinessTransitionsView_OnPropertyChanged(PropertyType propType)
        {
            if (propType == PropertyType.BusinessStageTransition)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mSelection = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mSelection);
                }
            }
        }

        private void ClearView()
        {
            for (int x = tabControlMain.TabPages.Count - 1; x > -1; x--)
            {
                for (int y = 0; y < tabControlMain.TabPages[x].Controls.Count; y++)
                {
                    switch (x)
                    {
                        case 0:
                            {
                                CheckedListBox checkedBox1 = tabControlMain.TabPages[x].Controls[0] as CheckedListBox;
                                if (checkedBox1 == null)
                                {
                                    ComboBox comboBox1 = tabControlMain.TabPages[x].Controls[0] as ComboBox;
                                    if (comboBox1 != null)
                                    {
                                        comboBox1.SelectedValueChanged -= new EventHandler(comboBox1_SelectedValueChanged);
                                    }
                                }
                                break;
                            }
                    }
                    tabControlMain.TabPages[x].Controls.RemoveAt(y);
                }
                tabControlMain.Controls.Remove(tabControlMain.TabPages[x]);
            }
        }

        private void PopulateStageTransitions()
        {
            for (int x = 0; x < m_StageTransitions.Count; x++)
            {
                TreeNode[] lstFound = treeView.Nodes.Find(m_StageTransitions[x].SDSDGKey, true);
                if (lstFound.Length > 0)
                {
                    TreeNode foundNode = lstFound[0];
                    foundNode.Checked = true;
                    foundNode.Expand();
                    ExpandBranch(foundNode);
                }
            }
        }

        private void ExpandBranch(TreeNode node)
        {
            while (node.Parent != null)
            {
                node.Parent.Expand();
                node = node.Parent;
                ExpandBranch(node);
            }
        }

        private void SetNodeCheckedTrue(TreeNodeCollection nods, string stageDefinitionKey)
        {
            //prevent index or error..recurrent loop to check all child nodes..

            if (nods.Count > 0)
            {
                foreach (TreeNode nod in nods)
                {
                    if (nod.Tag == null)
                        continue;

                    if (nod.Tag.ToString() == stageDefinitionKey)
                    {
                        nod.Checked = true;
                        break;
                    }

                    SetNodeCheckedTrue(nod.Nodes, stageDefinitionKey);
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void BusinessStageTransitionsView_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.App.mnuBusinessStageTransitions.Checked = false;
        }

        private void BusinessStageTransitionsView_Load(object sender, EventArgs e)
        {
            lstGroups = new List<itm>();
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mObj = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mObj);
                }
            }
        }

        private DialogResult BuildStageTransitionTree()
        {
            lstGroups.Clear();
            treeView = new TreeView();
            //try
            //{
            if (MainForm.App.GetCurrentView() == null)
            {
                return DialogResult.OK;
            }
            if (MainForm.App.GetCurrentView().Document == null)
            {
                return DialogResult.OK;
            }
            if ((MainForm.App.GetCurrentView().Document.BusinessStageConnectionString.Length == 0))
            {
                ConnectionForm CF = new ConnectionForm();
                if (Helpers.ShowX2ConnForm(CF, false))
                    MainForm.App.GetCurrentView().Document.BusinessStageConnectionString = CF.ConnectionString;
                else
                    return DialogResult.Cancel;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(MainForm.App.GetCurrentView().Document.BusinessStageConnectionString);
            cmd.Connection.Open();
            lstBusinessStageItems = new List<BusinessStageItem>();
            cmd.CommandText = "select g.StageDefinitionGroupKey,g.Description,g.ParentKey from StageDefinitionGroup g  where g.parentKey is null order by g.Description";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    itm groupItem = new itm();
                    groupItem.Key = reader[0].ToString();
                    groupItem.Description = reader[1].ToString();
                    groupItem.ParentKey = reader[2].ToString();
                    lstGroups.Add(groupItem);
                }
            }
            reader.Close();
            for (int x = 0; x < lstGroups.Count; x++)
            {
                TreeNode tNode = new TreeNode();
                tNode.Text = lstGroups[x].Description; //+ " (" + lstGroups[x].Key.ToString() + ")";
                treeView.Nodes.Add(tNode);
                lstChildGroupNodes = new List<itm>();
                PopulateGroupNodes(tNode, lstGroups[x]);
                PopulateChildNodes(tNode, lstGroups[x]);
            }
            cmd.Connection.Close();
            cmd.Dispose();
            MainForm.App.GetCurrentView().Document.BusinessStages = lstBusinessStageItems;
            return MainForm.App.TestBusinessStageTransitions();
        }

        private void PopulateGroupNodes(TreeNode parentNode, itm parentItem)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(MainForm.App.GetCurrentView().Document.BusinessStageConnectionString);
            cmd.Connection.Open();
            cmd.CommandText = "select g.StageDefinitionGroupKey,g.Description,g.ParentKey from StageDefinitionGroup g  where g.parentKey = " + parentItem.Key + " order by g.Description";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader != null)
            {
                while (reader.Read())
                {
                    itm groupItem = new itm();
                    groupItem.Key = reader[0].ToString();
                    groupItem.Description = reader[1].ToString();
                    groupItem.ParentKey = reader[2].ToString();
                    lstChildGroupNodes.Add(groupItem);
                    TreeNode tNode = new TreeNode();
                    tNode.Text = groupItem.Description; //+ " (" + lstGroups[x].Key.ToString() + ")";
                    parentNode.Nodes.Add(tNode);
                    PopulateChildNodes(tNode, groupItem);
                    PopulateGroupNodes(tNode, groupItem);
                }
            }
            reader.Close();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            cmd.Dispose();
        }

        private DialogResult PopulateChildNodes(TreeNode parentNode, itm parentItem)
        {
            if (parentItem.Key.Length > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = new SqlConnection(MainForm.App.GetCurrentView().Document.BusinessStageConnectionString);
                cmd.Connection.Open();

                cmd.CommandText = "select sdsdk.StageDefinitionStageDefinitionGroupKey, SD.Description,sgd.Description as DefinitionGroupDesc  "
                            + "from StageDefinition SD  "
                            + "join StageDefinitionStageDefinitionGroup sdsdk "
                            + "on sdsdk.StageDefinitionKey = sd.StageDefinitionKey "
                            + "join StageDefinitionGroup sgd "
                            + "on sgd.StageDefinitionGroupKey=sdsdk.StageDefinitionGroupKey "
                            + "where SD.GeneralStatusKey = 1 and sd.IsComposite = 0 and sdsdk.StageDefinitionGroupKey =" + parentItem.Key.ToString() + " "
                            + "order by sgd.Description,SD.Description;";

                SqlDataReader reader = cmd.ExecuteReader();

                lstDefinitions = new List<itm>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        BusinessStageItem i = new BusinessStageItem();
                        i.SDSDGKey = reader[0].ToString();
                        i.DefinitionGroupDescription = reader[2].ToString();
                        i.DefinitionDescription = reader[1].ToString();
                        lstBusinessStageItems.Add(i);
                        itm definitionItem = new itm();
                        definitionItem.Key = reader[0].ToString();
                        definitionItem.Description = reader[1].ToString();
                        definitionItem.ParentKey = reader[0].ToString();
                        lstDefinitions.Add(definitionItem);
                        TreeNode node = new TreeNode();
                        node.Tag = i.SDSDGKey;
                        node.Name = definitionItem.Key;
                        node.Text = definitionItem.Description + " (" + definitionItem.Key + ")";
                        parentNode.Nodes.Add(node);
                    }
                }
                reader.Close();
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
            return DialogResult.OK;
        }
    }

    public class itm
    {
        private string _key;
        private string _description;
        private string _parentKey;

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string ParentKey
        {
            get
            {
                return _parentKey;
            }
            set
            {
                _parentKey = value;
            }
        }
    }
}