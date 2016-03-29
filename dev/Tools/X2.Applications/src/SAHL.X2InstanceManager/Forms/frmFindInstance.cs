using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAHL.X2InstanceManager.Items;
using SAHL.X2InstanceManager.Misc;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.X2InstanceManager.Forms
{
    public partial class frmFindInstance : Form
    {
        private enum SearchType
        {
            SpecificDate,
            DateRange
        }
        private ListViewColumnSorter lvwColumnSorter;
        private string m_Connection;
        private int m_WorkFlowID;
        public string CurrentlySelectedProcessName;
        public List<CriteriaItem> lstCriteria = new List<CriteriaItem>();
        public string Workflow { get; set; }
        public string InstanceId { get; set; }
        public string StateName { get; set; }
        private string dbConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SAHL.X2InstanceManager.Properties.Settings.DBConnectionString"].ConnectionString;

        public frmFindInstance()
        {
            MainForm.SetThreadPrincipal("X2");
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            btnAboutInstance.Enabled = false;
            btnEditInstance.Enabled = false;
            btnRebuildInstance.Enabled = false;
            btnUnlockInstance.Enabled = false;
            btnMoveTo.Enabled = false;
            btnReAssignInstance.Enabled = false;
            this.listViewResults.ListViewItemSorter = lvwColumnSorter;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            btnAboutInstance.Enabled = false;
            btnEditInstance.Enabled = false;
            btnRebuildInstance.Enabled = false;
            btnUnlockInstance.Enabled = false;
            btnMoveTo.Enabled = false;
            btnReAssignInstance.Enabled = false;

            if (lstCriteria.Count == 0)
            {
                MessageBox.Show(this, "You cannot search without specifying any criteria!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CurrentlySelectedProcessName = MainForm.App.GetProcessName();
            lblStatus.Text = "Searching...";
            this.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            MainForm.App.searching = true;
            List<TreeNode> workFlowNodes = new List<TreeNode>();
            TreeNode mWorkFlowNode = MainForm.App.GetWorkFlowNode(MainForm.App.treeMain.SelectedNode);

            workFlowNodes = MainForm.App.GetAllWorkFlowNodes(MainForm.App.treeMain.SelectedNode);

            m_Connection = MainForm.App.GetConnectionForCurrentBranch();

            List<ListViewItem> mList = new List<ListViewItem>();
            this.listViewResults.Items.Clear();
            WorkFlowItem mWorkFlowItem = workFlowNodes[0].Tag as WorkFlowItem;
            mList = Search();
            for (int y = 0; y < mList.Count; y++)
            {
                listViewResults.Items.Add(mList[y]);
            }

            Cursor = Cursors.Default;
            if (listViewResults.Items.Count == 1)
            {
                lblStatus.Text = listViewResults.Items.Count + " result!";
            }
            else
            {
                lblStatus.Text = listViewResults.Items.Count + " results!";
            }
            MainForm.App.searching = false;
            this.Cursor = Cursors.Default;
        }

        private List<ListViewItem> Search()
        {
            List<ListViewItem> mList = new List<ListViewItem>();
            SqlDataReader mReader = null;
            SqlConnection Connection = null;
            SqlCommand cmd = null;
            try
            {
                Connection = new SqlConnection(m_Connection);
                Connection.Open();
                cmd = Connection.CreateCommand();
                StringBuilder cmdText = new StringBuilder();
                cmdText.AppendLine("select distinct Top(201)[X2].Instance.[ID],[X2].Instance.[Name], ");
                cmdText.AppendLine("[X2].State.[Name],[X2].Instance.[CreationDate], [X2].State.[ID], [X2].Workflow.[Name],[X2].Instance.CreatorADUsername ");
                cmdText.AppendLine("from [X2].Instance ");
                cmdText.AppendLine("inner join [X2].State ");
                cmdText.AppendLine("on [X2].Instance.[StateID] = [X2].State.[ID] ");
                cmdText.AppendLine("inner join [X2].Workflow ");
                cmdText.AppendLine("on [X2].Workflow.[ID] = [X2].Instance.[WorkflowID] ");
                cmdText.AppendLine("inner join [X2].Process ");
                cmdText.AppendLine("on [X2].Process.[ID] = [X2].Workflow.[ProcessID] ");
                cmdText.AppendLine("where [X2].Process.[Name] = @ProcessName");

                cmd.Parameters.Add("@ProcessName",SqlDbType.VarChar);
                cmd.Parameters[0].Value = MainForm.App.GetProcessName();
                         
                for (int x = 0; x < lstCriteria.Count; x++)
                {
                    if (lstCriteria[x].value != null && lstCriteria[x].value.Length > 0)
                    {
                        switch (lstCriteria[x].Description)
                        {

                            case "Instance ID":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].Instance.ID = " + lstCriteria[x].value);
                                    }
                                    else
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].Instance.ID != " + lstCriteria[x].value);
                                    }
                                    break;
                                }
                            case "Instance Name":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Name] = '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Name] LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    else
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Name] != '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Name] NOT LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    break;
                                }
                            case "Subject":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Subject] = '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Subject] LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    else
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Subject] != '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[Subject] NOT LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    break;
                                }
                            case "State Name":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[State].[Name] = '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[State].[Name] LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    else
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[State].[Name] != '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[State].[Name] NOT LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    break;
                                }
                            case "Creation Date":
                                {
                                    DateTime startDate = DateTime.Now;
                                    DateTime endDate = DateTime.Now;
                                    bool endDateSet = false;
                                    if (lstCriteria[x].value.Contains(" -> "))
                                    {
                                        int pos = lstCriteria[x].value.IndexOf(" -> ");
                                        string val1 = lstCriteria[x].value.Substring(0, pos);
                                        startDate = DateTime.Parse(val1);
                                        string val2 = lstCriteria[x].value.Substring(pos + 3, lstCriteria[x].value.Length - (pos + 3));
                                        endDate = DateTime.Parse(val2);
                                        endDateSet = true;
                                    }
                                    else
                                    {
                                        startDate = DateTime.Parse(lstCriteria[x].value);
                                    }
                                    if (endDateSet == false)
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreationDate] >= '" + DateTime.Parse(lstCriteria[x].value).ToShortDateString() + " 00:00:01 AM' and [X2].[Instance].[CreationDate] <= '" + DateTime.Parse(lstCriteria[x].value).ToShortDateString() + " 11:59:59 PM'");
                                    }
                                    else
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreationDate] >= '" + startDate.ToShortDateString() + " 00:00:01 AM' and [X2].[Instance].[CreationDate] <= '" + endDate.ToShortDateString() + " 11:59:59 PM'");
                                    }

                                    break;
                                }
                            case "ADUsername (Creator)":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreatorADUsername] = '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {

                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreatorADUsername] LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                           

                                    }                                
                                    else
                                    {
                                        if (lstCriteria[x].Explicit)
                                        {

                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreatorADUsername] != '" + lstCriteria[x].value + "'");
                                        }
                                        else
                                        {
                                            cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Instance].[CreatorADUsername] NOT LIKE '%" + lstCriteria[x].value + "%'");
                                        }
                                    }
                                    break;
                                }
                            case "Workflow Name":
                                {
                                    if (lstCriteria[x].equalsType == EqualsType.EqualTo)
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Workflow].[Name] = '" + lstCriteria[x].value + "'");
                                    }
                                    else
                                    {
                                        cmdText.AppendLine(lstCriteria[x].andOr.ToString() + " [X2].[Workflow].[Name] != '" + lstCriteria[x].value + "'");
                                    }
                                    break;
                                }
                        }
                    }
                }
                cmd.CommandText = cmdText.ToString();
                mReader = cmd.ExecuteReader();
                int rowCount = 0;
                while (mReader.Read())
                {
                    ListViewItem mItem = new ListViewItem(new string[] { mReader[5].ToString(), mReader[1].ToString(), mReader[2].ToString(), mReader[3].ToString(), mReader[4].ToString(), mReader[0].ToString(),mReader[6].ToString() });
                    mItem.Tag = mReader[0].ToString();
                    if (rowCount < 200)
                    {
                        mList.Add(mItem);
                    }
                    rowCount++;
                }
                mReader.Close();
                if (rowCount > 200)
                {
                    MessageBox.Show(this, "Only the first 200 rows have been returned!","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                return mList;
            }
            catch (Exception errExc)
            {
                MessageBox.Show(this, errExc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return mList;
            }
            finally
            {

                Connection.Close();
                cmd.Dispose();
            }
        }

        private void listViewResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lblStatus.Text = "Locating Instance...";
            Application.DoEvents();
            if (listViewResults.SelectedIndices.Count != 0)
            {
                MainForm.App.searching = true;

                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                // by default nodes are found via the instance name (offerkey)
                string description = listViewResults.SelectedItems[0].SubItems[1].Text.ToString(); // this is the instance name 
                // if there is no instance name then use the instance id
                if (String.IsNullOrEmpty(description))
                    description = listViewResults.SelectedItems[0].SubItems[5].Text.ToString(); // this is the instance id

                TreeNode[] foundNodes = MainForm.App.treeMain.Nodes.Find(description, true);
                if (foundNodes.Length > 0)
                {
                    MainForm.App.treeMain.SelectedNode = foundNodes[0];
                }
                else
                {
                    TreeNode[] foundStatesNodes = null;
                    int WorkFlowID = -1;
                    TreeNode workFlowNode = null;
                    WorkFlowItem o = null;
                    TreeNode holdNode = null;
                    workFlowNode = MainForm.App.GetWorkFlowNode(MainForm.App.treeMain.SelectedNode);

                    List<TreeNode> nodes = MainForm.App.GetAllWorkFlowNodes(MainForm.App.treeMain.SelectedNode);
                    if (nodes.Count == 0)
                    {
                        MessageBox.Show(this, "Cannot find the node", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    for (int x = 0; x < nodes.Count; x++)
                    {
                        MainForm.App.treeMain.SelectedNode = nodes[x];
                        o = nodes[x].Tag as WorkFlowItem;
                        WorkFlowID = o.WorkFlowID;
                        holdNode = null;

                        string nodeText = nodeText = nodes[x].Text;

                        if (nodes[x].Text.Contains("("))
                        {
                            nodeText = nodes[x].Text.Substring(0, nodes[x].Text.IndexOf(" ("));
                        }

                        nodeText.Trim();

                        if (nodeText == listViewResults.Items[listViewResults.SelectedIndices[0]].SubItems[0].Text)
                        {
                            MainForm.App.treeMain.SelectedNode = nodes[x];
                            for (int z = 0; z < nodes[x].Nodes.Count; z++)
                            {
                                if (nodes[x].Nodes[z].Text == "States")
                                {
                                    holdNode = nodes[x].Nodes[z];
                                    break;
                                }
                            }
                        }
                        if (holdNode != null)
                        {
                            break;
                        }


                    }

                    if (holdNode == null)
                    {
                        lblStatus.Text = "Ready";
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    MainForm.App.CreateStateList(WorkFlowID, holdNode);

                    foundStatesNodes = MainForm.App.treeMain.Nodes.Find(listViewResults.SelectedItems[0].SubItems[2].Text, true);
                    if (foundStatesNodes.Length > 0)
                    {
                        MainForm.App.CreateInstanceNodesForState(int.Parse(listViewResults.SelectedItems[0].SubItems[4].Text.ToString()), foundStatesNodes[0]);
                    }
                    foundNodes = MainForm.App.treeMain.Nodes.Find(listViewResults.SelectedItems[0].SubItems[5].Text, true);
                    if (foundNodes.Length > 0)
                    {
                        MainForm.App.treeMain.SelectedNode = foundNodes[0];
                    }
                }
                InstanceItem mInstanceItem = MainForm.App.treeMain.SelectedNode.Tag as InstanceItem;
                MainForm.App.tabControlMain.TabPages[0].Text = "Instance (ID " + listViewResults.SelectedItems[0].Tag.ToString() + ")";
                MainForm.App.CreateInstanceListForSelectedInstance(int.Parse(listViewResults.SelectedItems[0].Tag.ToString()));
                MainForm.App.searching = false;
            }
            Cursor = Cursors.Default;
            lblStatus.Text = "Ready";
        }

        private void frmFindInstance_Load(object sender, EventArgs e)
        {
         
            for (int x = 0; x < lstCriteria.Count; x++)
            {
                cbxAvailableCriteria.Items.Add(lstCriteria[x].Description);               
            }
            cbxAvailableCriteria.Text = "--Please Select--";
            cbxAvailableCriteria.SelectedIndex = 3;

            if (MainForm.App.GetConnectionForCurrentBranch() == null || MainForm.App.GetWorkFlowNode(MainForm.App.treeMain.SelectedNode) == null
                && MainForm.App.treeMain.SelectedNode.Tag.ToString() != "Process")
            {
                DisableControls();
            }
            else
            {
                EnableControls();
            }           
        }

        internal void EnableControls()
        {           
            lblStatus.Text = "Ready";
            cbxAvailableCriteria.Enabled = true;
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnRemove.Enabled = true;
            btnFind.Enabled = true;
            listViewResults.Enabled = true;
      
        }

        internal void DisableControls()
        {
            listViewResults.Items.Clear();
            listViewResults.Enabled = false;
            cbxAvailableCriteria.Enabled = false;
            lblStatus.Text = "Disabled";
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            btnRemove.Enabled = false;
            btnFind.Enabled = false;          

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {            
            switch (cbxAvailableCriteria.Text)
            {
                case "Instance ID":
                    {
                        AddInstanceIDCriteria();
                        break;
                    }
                case "Creation Date":
                    {
                        AddCreationDateCriteria();
                        break;
                    }
                case "ADUsername (Creator)":
                    {
                        AddADUsernameCriteria();
                        break;
                    }
                case "Instance Name":
                    {
                        AddInstanceNameCriteria();
                        break;
                    }
                case "Subject":
                    {
                        AddInstanceSubjectCriteria();
                        break;
                    }
                case "State Name":
                    {
                        AddInstanceStateCriteria();
                        break;
                    }
                case "Workflow Name":
                    {
                        AddWorkflowCriteria();
                        break;
                    }
            }

        }

        private void AddInstanceNameCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "Instance Name";           
            frmFindByText mfrm = new frmFindByText(null,AndOr.NotSet,EqualsType.NotSet,false);
            mfrm.Text = "Instance Name Criteria";
            mfrm.lblDescription.Text = "Specify Name :";
            DialogResult res =  mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }

                mItem.value = mfrm.txtValue.Text;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(), mItem.Explicit.ToString(), mItem.value });
                listViewCriteria.Items.Add(lItem);               
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddInstanceIDCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "Instance ID";      
            frmFindByText mfrm = new frmFindByText(null, AndOr.NotSet,EqualsType.NotSet,false);
            mfrm.Text = "Instance ID Criteria";
            mfrm.lblDescription.Text = "Specify ID :";
            mfrm.checkExplicit.Enabled = false;
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }

                mItem.value = mfrm.txtValue.Text;
                mItem.Explicit = true;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(), mItem.Explicit.ToString(), mItem.value });
                listViewCriteria.Items.Add(lItem);
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddInstanceSubjectCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "Subject";  
            frmFindByText mfrm = new frmFindByText(null, AndOr.NotSet,EqualsType.NotSet,false);
            mfrm.Text = "Instance Subject Criteria";
            mfrm.lblDescription.Text = "Specify Subject :";
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }

                mItem.value = mfrm.txtValue.Text;
                mItem.Explicit = mfrm.checkExplicit.Checked;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(), mItem.Explicit.ToString(), mItem.value });
                listViewCriteria.Items.Add(lItem);
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddInstanceStateCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "State Name";        
            frmFindByText mfrm = new frmFindByText(null, AndOr.NotSet,EqualsType.NotSet,false);
            mfrm.Text = "State Name Criteria";
            mfrm.lblDescription.Text = "Specify State Name :";
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }

                mItem.value = mfrm.txtValue.Text;
                mItem.Explicit = mfrm.checkExplicit.Checked;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(), mItem.Explicit.ToString(), mItem.value });
                listViewCriteria.Items.Add(lItem);
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddADUsernameCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "ADUsername (Creator)";  
            frmFindByText mfrm = new frmFindByText(null, AndOr.NotSet,EqualsType.NotSet,false);
            mfrm.Text = "ADUsername Criteria";
            mfrm.lblDescription.Text = "Specify Username :";
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }
                mItem.Explicit = mfrm.checkExplicit.Checked;
                mItem.value = mfrm.txtValue.Text;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(), mItem.Explicit.ToString(),  mItem.value });
                listViewCriteria.Items.Add(lItem);               

                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddWorkflowCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "Workflow Name";  
            frmFindByWorkflow mfrm = new frmFindByWorkflow(null,AndOr.NotSet,EqualsType.NotSet);
            mfrm.Text = "Workflow Name Criteria";
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }

                mItem.value = mfrm.cbxWorkflow.Text;
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(), mItem.equalsType.ToString(),"", mItem.value });
                listViewCriteria.Items.Add(lItem);
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void AddCreationDateCriteria()
        {
            CriteriaItem mItem = new CriteriaItem();
            mItem.Description = "Creation Date";  
            frmFindByDate mfrm = new frmFindByDate(null,AndOr.NotSet);
      
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioSpecific.Checked)
                {
                    mItem.value = mfrm.dateTimePicker1.Value.ToShortDateString();
                }
                else
                {
                    mItem.value = mfrm.dateTimePicker1.Value.ToShortDateString() + " -> " + mfrm.dateTimePicker2.Value.ToShortDateString();
                }
               
                lstCriteria.Add(mItem);
                ListViewItem lItem = new ListViewItem(new string[] { mItem.Description, mItem.andOr.ToString(),"","", mItem.value });
                listViewCriteria.Items.Add(lItem);               
                cbxAvailableCriteria.Text = "--Please Select--";
            }
            mfrm.Dispose();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewCriteria.SelectedIndices.Count > 0)
            {
                for (int x = 0; x < lstCriteria.Count; x++)
                {
                    if (lstCriteria[x].Description == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text
                        && lstCriteria[x].andOr.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text
                        && lstCriteria[x].value.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text)
                    {
                        lstCriteria.RemoveAt(x);
                        break;
                    }
                }

                listViewCriteria.Items.RemoveAt(listViewCriteria.SelectedIndices[0]);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            CriteriaItem cItem = null;
            if (listViewCriteria.SelectedItems.Count > 0)
            {
                for (int x = 0; x < lstCriteria.Count; x++)
                {
                    if (lstCriteria[x].Description == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text)
                    {
                        cItem = lstCriteria[x];
                        break;
                    }
                }
                if (cItem != null)
                {
                    if (cItem.Description != "Creation Date" && cItem.Description != "Workflow Name")
                    {
                        EditTextCriteria();
                    }
                    else
                    {
                        if (cItem.Description == "Creation Date")
                        {
                            EditCreationDateCriteria();
                        }
                        else if (cItem.Description == "Workflow Name")
                        {
                            EditWorkFlowNameCriteria();
                        }
                    }
                }
            }
        }

        private void EditCreationDateCriteria()
        {
            CriteriaItem mItem = null;
            for (int x = 0; x < lstCriteria.Count; x++)
            {
                if (lstCriteria[x].Description == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text
                    && lstCriteria[x].andOr.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text
                    && lstCriteria[x].value == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text)
                {
                    mItem = lstCriteria[x];
                    break;
                }
            }
            if (mItem == null)
            {
                MessageBox.Show(this, "Cannot find item to edit!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mItem.Description = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text;
            mItem.value = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text;
            if (listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text == "And")
            {
                mItem.andOr = AndOr.And;
            }
            else
            {
                mItem.andOr = AndOr.Or;
            }
            frmFindByDate mfrm = new frmFindByDate(mItem.value, mItem.andOr);           
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioSpecific.Checked)
                {
                    mItem.value = mfrm.dateTimePicker1.Value.ToString();
                }
                else
                {
                    mItem.value = mfrm.dateTimePicker1.Value.ToString() + " -> " + mfrm.dateTimePicker2.Value.ToString();
                }

                ListViewItem lItem = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]];
                lItem.SubItems[0].Text = mItem.Description;
                lItem.SubItems[1].Text = mItem.andOr.ToString();
                lItem.SubItems[2].Text = "";
                lItem.SubItems[3].Text = "";
                lItem.SubItems[4].Text = mItem.value.ToString();
            }
            mfrm.Dispose();  
        }

        private void EditWorkFlowNameCriteria()
        {
            CriteriaItem mItem = null;
            for (int x = 0; x < lstCriteria.Count; x++)
            {
                if (lstCriteria[x].Description == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text
                    && lstCriteria[x].andOr.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text
                    && lstCriteria[x].value == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text)
                {
                    mItem = lstCriteria[x];
                    break;
                }
            }
            if (mItem == null)
            {
                MessageBox.Show(this, "Cannot find item to edit!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mItem.Description = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text;
            mItem.value = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text;
            if (listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text == "And")
            {
                mItem.andOr = AndOr.And;
            }
            else
            {
                mItem.andOr = AndOr.Or;
            }
            if (listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[2].Text == "EqualTo")
            {
                mItem.equalsType = EqualsType.EqualTo;
            }
            else
            {
                mItem.equalsType = EqualsType.NotEqualTo;
            }
            frmFindByWorkflow mfrm = new frmFindByWorkflow(mItem.value, mItem.andOr,mItem.equalsType);          
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }
                mItem.value = mfrm.cbxWorkflow.Text;
                ListViewItem lItem = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]];
                lItem.SubItems[0].Text = mItem.Description;
                lItem.SubItems[1].Text = mItem.andOr.ToString();
                lItem.SubItems[2].Text = mItem.equalsType.ToString();
                lItem.SubItems[3].Text = "";
                lItem.SubItems[4].Text = mItem.value.ToString();
            }
            mfrm.Dispose();  
        }

        private void EditTextCriteria()
        {
            CriteriaItem mItem = null;
            for (int x = 0; x < lstCriteria.Count; x++)
            {
                if (lstCriteria[x].Description == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text
                    && lstCriteria[x].andOr.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text
                    && lstCriteria[x].equalsType.ToString() == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[2].Text
                    && lstCriteria[x].value == listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text)
                {
                    mItem = lstCriteria[x];
                    break;
                }
            }
            if (mItem == null)
            {
                MessageBox.Show(this, "Cannot find item to edit!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mItem.Description = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[0].Text;
            if(mItem.Description != "Instance ID")
            {
            mItem.Explicit = bool.Parse(listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[3].Text);
            }
            else
            {
                mItem.Explicit = true;
            }
            mItem.value = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[4].Text;
            if (listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[1].Text == "And")
            {
                mItem.andOr = AndOr.And;
            }
            else
            {
                mItem.andOr = AndOr.Or;
            }
            if (listViewCriteria.Items[listViewCriteria.SelectedIndices[0]].SubItems[2].Text == "EqualTo")
            {
                mItem.equalsType = EqualsType.EqualTo;
            }
            else
            {
                mItem.equalsType = EqualsType.NotEqualTo;
            }

            frmFindByText mfrm = new frmFindByText(mItem.value, mItem.andOr,mItem.equalsType,mItem.Explicit);
            //mfrm.Text = "Instance ID Criteria";
            //mfrm.lblDescription.Text = "Specify Subject :";
            DialogResult res = mfrm.ShowDialog();
            if (res == DialogResult.OK)
            {
                if (mfrm.radioAnd.Checked)
                {
                    mItem.andOr = AndOr.And;
                }
                else
                {
                    mItem.andOr = AndOr.Or;
                }
                if (mfrm.radioEqual.Checked)
                {
                    mItem.equalsType = EqualsType.EqualTo;
                }
                else
                {
                    mItem.equalsType = EqualsType.NotEqualTo;
                }
                mItem.value = mfrm.txtValue.Text;
                mItem.Explicit = mfrm.checkExplicit.Checked;
                ListViewItem lItem = listViewCriteria.Items[listViewCriteria.SelectedIndices[0]];
                lItem.SubItems[0].Text = mItem.Description;
                lItem.SubItems[1].Text = mItem.andOr.ToString();
                lItem.SubItems[2].Text = mItem.equalsType.ToString();
                lItem.SubItems[3].Text = mItem.Explicit.ToString();
                lItem.SubItems[4].Text = mItem.value.ToString();    
            }
            mfrm.Dispose();  
        }

        private void listViewResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            listViewResults.Sort();
        }        

        private void listViewResults_SelectedIndexChanged(object sender, EventArgs e)
        {           
            if (listViewResults.SelectedIndices.Count != 0)
            {
                btnAboutInstance.Enabled = true;
                btnEditInstance.Enabled = true;
                btnRebuildInstance.Enabled = true;
                btnUnlockInstance.Enabled = true;
                btnMoveTo.Enabled = true;
                btnReAssignInstance.Enabled = true;

                Workflow = listViewResults.SelectedItems[0].Text.ToString(); // this is the instance name 
                InstanceId = listViewResults.SelectedItems[0].SubItems[5].Text.ToString(); // this is the instance id                                
                StateName = listViewResults.SelectedItems[0].SubItems[2].Text.ToString(); // this is the state name

                btnUnlockInstance.Enabled = IsInstanceLocked(int.Parse(InstanceId));
                            
            }
        }

        public bool IsInstanceLocked(int instanceId)
        {
            bool result = true;

            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["SAHL.X2InstanceManager.Properties.Settings.DBConnectionString"].ConnectionString;

            SqlConnection Connection = null;
            SqlCommand cmd = null;
            SqlTransaction mTransaction = null;

            int mID = instanceId;
            try
            {
                Connection = new SqlConnection(connection);
                Connection.Open();
                cmd = Connection.CreateCommand();

                cmd.CommandText = "select activityDate,activityADUserName from [X2].Instance where ID = @InstanceID";
                cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                cmd.Parameters[0].Value = mID;
                SqlDataReader mReader = cmd.ExecuteReader();
                while (mReader.Read())
                {
                    if (mReader[0].ToString().Length == 0 && mReader[1].ToString().Length == 0)
                    {
                        result = false;
                    }
                }
                mReader.Close();
                mReader.Dispose();
            }
            catch
            {
                MessageBox.Show(this, "An error occurred while trying to get information for the instance!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (mTransaction != null)
                    mTransaction.Rollback();
            }

            return result;
        }
        
        private void btnAboutInstance_Click(object sender, EventArgs e)
        {
            frmAboutInstance frm = new frmAboutInstance(long.Parse(InstanceId), Workflow, MainForm.App.GetProcessName(), MainForm.EngineConnectionString);
            frm.TopMost = true;
            frm.ShowDialog(this);            
            frm.Dispose();
        }

        private void btnMoveTo_Click(object sender, EventArgs e)
        {
            frmMove frm = new frmMove(long.Parse(InstanceId), Workflow, MainForm.App.GetProcessName(), MainForm.EngineConnectionString);
            frm.TopMost = true;
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void btnRebuildInstance_Click(object sender, EventArgs e)
        {
            // rebuild Instance Activity and WorkList/TrackList
            this.Cursor = Cursors.WaitCursor;
            lblStatus.Text = "Rebuilding Instance...";
            Application.DoEvents();
            
            int instanceId = int.Parse(InstanceId);
            if (instanceId != 0)
            {
                SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder(dbConnection);
                SCSB.InitialCatalog = "warehouse";                
                string AuditConn = SCSB.ToString();
                IActiveDataTransaction Tran = null;
                try
                {                                        
                    var engine = new EngineConnector(Workflow, MainForm.App.GetProcessName(), MainForm.EngineConnectionString);

                    X2ErrorResponse resp = engine.RecalcSecurity(Convert.ToInt64(instanceId), "");
                    if (null != resp)
                    {
                        throw new Exception(string.Format("Unable to rebuild instance {0}.\r\n{1}", instanceId, resp.Exception.Value));
                    }

                    MessageBox.Show(this, "Instance Rebuilt.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,ex.Message);
                }
                finally
                {
                    if (null != Tran)
                        Tran.Dispose();
                }
            }
            lblStatus.Text = "Ready";
            this.Cursor = Cursors.Default;
        }

        private void btnUnlockInstance_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Processing Unlock...";
            this.Cursor = Cursors.WaitCursor;
            SqlConnection Connection = null;
            SqlCommand cmd = null;
            SqlTransaction mTransaction = null;
            
            int mID = int.Parse(InstanceId);
            try
            {
                Connection = new SqlConnection(dbConnection);
                Connection.Open();
                mTransaction = Connection.BeginTransaction();
                cmd = Connection.CreateCommand();
                cmd.Transaction = mTransaction;

                cmd.CommandText = "update [X2].Instance set  activityDate = null,activityADUserName = null where ID = @InstanceID";
                cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                cmd.Parameters[0].Value = mID;
                cmd.ExecuteNonQuery();

                if (mTransaction != null)
                    mTransaction.Commit();
                MessageBox.Show(this, "Instance Unlocked.");                
                btnUnlockInstance.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (Connection != null)
                    Connection.Dispose();
            }
            lblStatus.Text = "Ready";
            this.Cursor = Cursors.Default;
        }

        private void btnEditInstance_Click(object sender, EventArgs e)
        {
            frmEditInstanceData mFrm = new frmEditInstanceData(Workflow, dbConnection);            

            mFrm.InstanceID = long.Parse(InstanceId);
            mFrm.Text = mFrm.Text + " - " + MainForm.App.GetProcessName();
            mFrm.TopMost = true;
            mFrm.ShowDialog(this);
            mFrm.Dispose();
        }


        private void btnReAssignInstance_Click(object sender, EventArgs e)
        {
            frmEditInstanceData mFrm = new frmEditInstanceData(Workflow, dbConnection);

            frmReassign frm = new frmReassign(long.Parse(InstanceId), Workflow, CurrentlySelectedProcessName, MainForm.EngineConnectionString);
            frm.ShowDialog(this);
            frm.Dispose();
        }


    }
}
