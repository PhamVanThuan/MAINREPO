using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2Designer.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Threading;
using SAHL.X2InstanceManager.Items;
using SAHL.X2InstanceManager.Forms;
using System.Security.Principal;
using SAHL.X2InstanceManager.Misc;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Framework.DataSets;
using System.Reflection;
using System.Collections;
using System.Configuration;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2.Framework.Common;
using System.Diagnostics;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework;
using System.Collections.Specialized;

namespace SAHL.X2InstanceManager
{
    public partial class MainForm : Form
    {
        internal static void SetThreadPrincipal(string ADUser)
        {
            WindowsPrincipal p = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            Thread.CurrentPrincipal = p;
        }

        internal static void ClearThreadPrincipal()
        {
            Thread.CurrentPrincipal = null;
        }

        private static MainForm m_MainForm;
        static string ConnectionStringX2;
        public static string EngineConnectionString;
        public frmFindInstance m_FindForm;
        public delegate void ServerReadyHandler();
        private ListViewColumnSorter lvwColumnSorter;
        private bool mustStop = false;
        public bool searching = false;

        private enum LogDateRange
        {
            All,
            Today,
            Yesterday,
            LastWeek,
            Older
        }

        public const string AuthorizedSecurityGroups = @"SAHL\IT Developers";

        public MainForm()
        {

            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewMain.ListViewItemSorter = lvwColumnSorter;
            m_MainForm = this;
        }
        private enum StateSelectionType
        {
            selected,
            all
        }

        public static MainForm App
        {
            get
            {
                return m_MainForm;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Thread.CurrentPrincipal = new System.Security.Principal.WindowsPrincipal(WindowsIdentity.GetCurrent());
            treeMain.Nodes.Clear();
            lvwColumnSorter = null;
            listViewMain.Columns.Clear();
            tabControlMain.TabPages[0].Text = "";
            tabControlMain.Controls.Remove(tabControlMain.TabPages[1]);

            // conect to the configured database and build tree
            btnConnectToDB_Click(sender, e);

            this.Text = this.Text += String.IsNullOrEmpty(Application.ProductVersion) ? "" : " - Version : " + Application.ProductVersion;
        }

        #region ButtonHandling

        private void btnConnectToDB_Click(object sender, EventArgs e)
        {
            SetThreadPrincipal("X2");
            ConnectionStringX2 = Properties.Settings.Default.DBConnectionString;
            SAHL.X2InstanceManager.Misc.DBMan.SetConnString(ConnectionStringX2);
            EngineConnectionString = Properties.Settings.Default.X2WebHost_Url;

            BuildTree(ConnectionStringX2, General.BuildType.New, null);
            if (treeMain.Nodes.Count > 0)
            {
                treeMain.SelectedNode = treeMain.Nodes[0].Nodes[0];
                if (treeMain.SelectedNode.Nodes.Count > 0)
                {
                    treeMain.SelectedNode = treeMain.SelectedNode.Nodes[0];
                }
            }
        }

        private TreeNode GetTopNode(TreeNode treeNode)
        {

            List<TreeNode> topNodes = new List<TreeNode>();
            for (int x = 0; x < treeMain.Nodes[0].Nodes.Count; x++)
            {
                topNodes.Add(treeMain.Nodes[0].Nodes[x]);
            }

            TreeNode checkNode = treeMain.SelectedNode;
            bool foundTopNode = false;
            while (foundTopNode == false)
            {
                for (int x = 0; x < topNodes.Count; x++)
                {
                    if (checkNode == topNodes[x])
                    {
                        foundTopNode = true;
                        break;
                    }
                }
                if (foundTopNode == false)
                {
                    if (checkNode.Parent == null)
                    {
                        return null;
                    }
                    else
                    {
                        checkNode = checkNode.Parent;
                    }
                }
            }
            return checkNode;
        }


        public TreeNode GetWorkFlowNode(TreeNode treeNode)
        {
            try
            {
                if (treeNode == null)
                {
                    return null;
                }
                if (treeNode != null)
                {
                    if (treeNode.Tag is WorkFlowItem)
                    {
                        return treeNode;
                    }
                }
                while (treeNode.Parent != null && treeNode.Parent.Tag is WorkFlowItem == false)
                {
                    treeNode = treeNode.Parent;
                }
                return treeNode.Parent;
            }
            catch
            {
                return null;
            }
        }

        public List<TreeNode> GetAllWorkFlowNodes(TreeNode treeNode)
        {
            List<TreeNode> itms = new List<TreeNode>();
            try
            {
                
                if (treeNode.Tag == null)
                    return itms;
                while (treeNode.Tag.ToString() != "Process")
                {
                    treeNode = treeNode.Parent;
                }

                for (int x = 0; x < treeNode.Nodes.Count; x++)
                {
                    if (treeNode.Nodes[x].Tag is WorkFlowItem)
                    {
                        itms.Add(treeNode.Nodes[x]);
                    }
                }
                return itms;
            }
            catch
            {
                return itms;
            }
            finally
            {
                
            }
        }

        private string GetWorkFlowName(TreeNode treeNode)
        {
            while (treeNode.Parent.Tag is WorkFlowItem == false || treeNode.Parent.Text == "States")
            {
                treeNode = treeNode.Parent;
            }
            WorkFlowItem mItem = treeNode.Parent.Tag as WorkFlowItem;
            return mItem.WorkFlowName;
        }

        #endregion

        #region Menu Handling

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void History_Command(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode.Tag is InstanceItem)
            {              
                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;                
                CreateHistoryForInstance(mItem.ID);
            }
        }

        private void EditInstance_Command(object sender, EventArgs e)
        {
            frmEditInstanceData mFrm = new frmEditInstanceData(GetWorkFlowName(treeMain.SelectedNode),GetConnectionForCurrentBranch());
            InstanceItem o = treeMain.SelectedNode.Tag as InstanceItem;
           
            mFrm.InstanceID = o.ID;
            mFrm.Text = mFrm.Text + " - " + treeMain.SelectedNode.Text;
            mFrm.ShowDialog();
            mFrm.Dispose();
        }


        private void ReassignNinja_Command(object sender, EventArgs e)
        {
            MainForm.SetThreadPrincipal("X2");
            string WFName = GetWorkFlowName(treeMain.SelectedNode);
            frmEditInstanceData mFrm = new frmEditInstanceData(WFName, GetConnectionForCurrentBranch());
            InstanceItem o = treeMain.SelectedNode.Tag as InstanceItem;

            frmAssignOfNinjaNess frm = new frmAssignOfNinjaNess(o.ID, WFName, GetProcessName(), EngineConnectionString);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void Reassign_Command(object sender, EventArgs e)
        {
            MainForm.SetThreadPrincipal("X2");
            string WFName = GetWorkFlowName(treeMain.SelectedNode);
            frmEditInstanceData mFrm = new frmEditInstanceData(WFName, GetConnectionForCurrentBranch());
            InstanceItem o = treeMain.SelectedNode.Tag as InstanceItem;

            frmReassign frm = new frmReassign(o.ID, WFName, GetProcessName(), EngineConnectionString);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void ParentInstance_Command(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode.Tag is InstanceItem)
            {
                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                int count = CreateInstanceListForSelectedInstance((mItem.ParentInstance));
                if (count == 0)
                {
                    MessageBox.Show("Parent Instance Not Available!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    tabControlMain.TabPages[0].Text = "Parent Instance";
                }

            }
        }

        private void UnlockInstance_Command(object sender, EventArgs e)
        {
            // Ask the user which X2 Engine to connect to

            SqlConnection Connection = null;
            SqlCommand cmd = null;
            SqlTransaction mTransaction=null;
            InstanceItem mInstance = treeMain.SelectedNode.Tag as InstanceItem;
            int mID = mInstance.ID;
            try
            {
                Connection = new SqlConnection(GetConnectionForCurrentBranch());
                Connection.Open();
                mTransaction = Connection.BeginTransaction();
                cmd = Connection.CreateCommand();
                cmd.Transaction = mTransaction;

                cmd.CommandText = "update [X2].Instance set  activityDate = null,activityADUserName = null where ID = @InstanceID";
                cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                cmd.Parameters[0].Value = mID;
                cmd.ExecuteNonQuery();

                treeMain.SelectedNode.ImageIndex = 50;
                treeMain.SelectedNode.SelectedImageIndex = 50;

                if (mTransaction != null)
                    mTransaction.Commit();
            }
            catch
            {
                MessageBox.Show("An error occurred while trying to unlock the instance!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if(mTransaction != null)
                    mTransaction.Rollback();
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (Connection != null)
                    Connection.Dispose();
            }
        }

        private void MoveTo_Command(object sender, EventArgs e)
        {
            string WFName = GetWorkFlowName(treeMain.SelectedNode);
            frmEditInstanceData mFrm = new frmEditInstanceData(WFName, GetConnectionForCurrentBranch());
            InstanceItem o = treeMain.SelectedNode.Tag as InstanceItem;

            frmMove frm = new frmMove(o.ID, WFName, GetProcessName(), EngineConnectionString);
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void AboutInstance_Command(object sender, EventArgs e)
        {
            string WFName = GetWorkFlowName(treeMain.SelectedNode);
            frmEditInstanceData mFrm = new frmEditInstanceData(WFName, GetConnectionForCurrentBranch());
            InstanceItem o = treeMain.SelectedNode.Tag as InstanceItem;

            frmAboutInstance frm = new frmAboutInstance(o.ID, WFName, GetProcessName(), EngineConnectionString);
            frm.ShowDialog(this);
            frm.Dispose();

        }

        private void Rebuild_Command(object sender, EventArgs e)
        {
            
            // rebuild Instance Activity and WorkList/TrackList
            this.Cursor = Cursors.WaitCursor;
            lblStatus.Text = "Rebuilding Instance...";
            Application.DoEvents();
            InstanceItem mInstance = treeMain.SelectedNode.Tag as InstanceItem;
            if (mInstance != null)
            {
                int mID = mInstance.ID;
                string ConnStr = GetConnectionForCurrentBranch();
                SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder(ConnStr);
                SCSB.InitialCatalog = "warehouse";
                string X2Conn = ConnStr;
                string AuditConn = SCSB.ToString();
                IActiveDataTransaction Tran = null;
                try
                {
                    Rebuild_Internal(mID);

                    MessageBox.Show("Instance Rebuilt.");
                }
                catch(Exception E)
                {
                    MessageBox.Show(E.Message);
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

        private void Rebuild_Internal(Int64 InstanceID)
        {
            string WFName = GetWorkFlowName(treeMain.SelectedNode);
            var engine = new EngineConnector(WFName,GetProcessName(),EngineConnectionString);

            X2ErrorResponse resp = engine.RecalcSecurity(Convert.ToInt64(InstanceID), "");
            if (null != resp)
            {
                throw new Exception(string.Format("Unable to rebuild instance {0}.\r\n{1}", InstanceID, resp.Exception.Value));
            }
        }

        private void CreateNew(object sender, EventArgs e)
        {
            string WF = treeMain.SelectedNode.Text;
            string PC = treeMain.SelectedNode.Parent.Text;
            int pos = WF.IndexOf('(');
            if (pos > 0)
                WF = WF.Remove(pos-1);
            frmCreateNewInstance frm = new frmCreateNewInstance(PC, WF, EngineConnectionString);
            frm.ShowDialog();
            frm.Dispose();
        }

        enum enInstanceRebuildType { Workflow, State}

        private void RebuildInstances(enInstanceRebuildType instanceRebuildType)
        {
            Cursor = Cursors.WaitCursor;

            string sql = String.Empty;

            switch (instanceRebuildType)
            {
                
                case enInstanceRebuildType.Workflow:
                    WorkFlowItem w = treeMain.SelectedNode.Tag as WorkFlowItem;
                    sql = string.Format("select ID from x2.x2.instance where WorkFlowID={0}", w.WorkFlowID);
                    lblStatus.Text = "Rebuilding All Instances for Workflow...";
                    break;
                case enInstanceRebuildType.State:
                    StateItem s = treeMain.SelectedNode.Tag as StateItem;
                    sql = string.Format("select ID from x2.x2.instance where StateID={0}", s.ID);
                    lblStatus.Text = "Rebuilding All Instances for State...";
                    break;
                default:
                    break;
            }

            Application.DoEvents();

            SqlConnection conn = null;
            DataSet ds = new DataSet();

            IActiveDataTransaction Tran = null;

            try
            {
                Tran = IActiveDataTransactionFactory.BeginTransaction("Rebuild All", TranCallerType.ExternalEngine);

                conn = new SqlConnection(ConnectionStringX2);
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.Fill(ds);
                }
                List<ListRequestItem> Items = new List<ListRequestItem>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ListRequestItem Item = new ListRequestItem(Convert.ToInt64(dr[0]), "");
                    Items.Add(Item);
                }

                IX2Provider engine = new X2EngineProviderFactory().GetX2EngineProvider();

                X2ResponseBase resp = engine.ProcessListActivity(Items);
                if (!resp.IsErrorResponse)
                {
                    //X2RebuildWorklistResponse lstResponses = resp as X2RebuildWorklistResponse;
                    //if (lstResponses.ItemList != null && lstResponses.ItemList.Count > 0)
                    //{
                    //    frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(lstResponses.ItemList, "");
                    //    mFrmErrors.ShowDialog();
                    //    mFrmErrors.Dispose();

                    //}
                    //else
                    //{
                    MessageBox.Show("All Instances Successfully Rebuilt !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
                else
                {
                    if (Items.Count > 0)
                    {
                        frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(Items, resp.Exception.Value);
                        mFrmErrors.ShowDialog();
                        mFrmErrors.Dispose();

                    }
                    else
                    {
                        MessageBox.Show("An unknown error has occurred !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
            catch (Exception ex)
            {
                if (null != Tran)
                    IActiveDataTransactionFactory.RollBackTransaction(Tran, false);

                MessageBox.Show(ex.ToString(), "Unable to Load InstanceID's for State");
            }
            finally
            {
                if (null != conn)
                    conn.Close();
            }

            Cursor = Cursors.Default;
            lblStatus.Text = "Ready";

        }
        private void RebuildAll_Command(object sender, EventArgs e)
        {
            RebuildInstances(enInstanceRebuildType.Workflow);

            //// rebuild Instance Activity and WorkList/TrackList
            //string ConnStr = GetConnectionForCurrentBranch();
            ////SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder(ConnStr);
            ////SCSB.InitialCatalog = "warehouse";
            //string X2Conn = ConnStr;
            ////string AuditConn = SCSB.ToString();
            //SqlDataReader mReader = null;
            //SqlConnection Connection = null;
            //SqlCommand cmd = null;
            //IActiveDataTransaction Tran = null;

            //Cursor = Cursors.WaitCursor;
            //lblStatus.Text = "Rebuilding All Instances...";
            //Application.DoEvents();


            //try
            //{
            //    Tran = IActiveDataTransactionFactory.BeginTransaction("Rebuild All", TranCallerType.ExternalEngine);

            //    WorkFlowItem WFI = treeMain.SelectedNode.Tag as WorkFlowItem;
            //    Int32 WorkFlowID = WFI.WorkFlowID;

            //    Connection = new SqlConnection(GetConnectionForCurrentBranch());
            //    Connection.Open();

            //    cmd = Connection.CreateCommand();

            //    cmd.CommandText = "select top 5 ID from [X2].Instance where WorkFlowID = @WorkFlowID";
            //    cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
            //    cmd.Parameters[0].Value = WorkFlowID;
            //    mReader = cmd.ExecuteReader();
            //    List<ListRequestItem> itms = new List<ListRequestItem>();
            //    while (mReader.Read())
            //    {
            //        Int64 InstanceID = mReader.GetInt64(0);

            //        ListRequestItem mItem = new ListRequestItem(InstanceID, "");
            //        itms.Add(mItem);
            //    }
            //    IX2Provider engine = new X2EngineProviderFactory().GetX2EngineProvider();

            //    X2ResponseBase resp = engine.ProcessListActivity(itms);
            //    if (!resp.IsErrorResponse)
            //    {
            //        X2RebuildWorklistResponse lstResponses = resp as X2RebuildWorklistResponse;
            //        if (lstResponses.ItemList.Count > 0)
            //        {
            //            frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(lstResponses.ItemList,"");
            //            mFrmErrors.ShowDialog();
            //            mFrmErrors.Dispose();

            //        }
            //    }
            //    else
            //    {
            //        if (itms.Count > 0)
            //        {
            //            frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(itms, resp.Exception.Value);
            //            mFrmErrors.ShowDialog();
            //            mFrmErrors.Dispose();

            //        }
            //        else
            //        {
            //            MessageBox.Show("An unknown error has occurred !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }

            //    }

            //    MessageBox.Show("All Instances Successfully Rebuilt !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception E)
            //{
            //    if (null != Tran)
            //        IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
            //    MessageBox.Show("Instance Rebuilding Failed !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    if (mReader != null)
            //    {
            //        mReader.Close();
            //        mReader.Dispose();
            //    }
            //    if (cmd != null)
            //        cmd.Dispose();

            //    if (Connection != null)
            //        Connection.Close();
            //}
            //Cursor = Cursors.Default;
            //lblStatus.Text = "Ready";
        }

        private void RebuildStateInstances(object sender, EventArgs e)
        {
            RebuildInstances(enInstanceRebuildType.State);

        //    StateItem s = treeMain.SelectedNode.Tag as StateItem;
        //    string SQL = string.Format("select ID from x2.x2.instance where StateID={0}",s.ID);
        //    SqlConnection conn = null;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        conn = new SqlConnection(ConnectionStringX2);
        //        conn.Open();
        //        using (SqlDataAdapter da = new SqlDataAdapter(SQL, conn))
        //        {
        //            da.Fill(ds);
        //        }
        //        List<ListRequestItem> Items = new List<ListRequestItem>();

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            ListRequestItem Item = new ListRequestItem(Convert.ToInt64(dr[0]), "");
        //            Items.Add(Item);
        //        }

        //        IX2Provider engine = new X2EngineProviderFactory().GetX2EngineProvider();

        //        X2ResponseBase resp = engine.ProcessListActivity(Items);
        //        if (!resp.IsErrorResponse)
        //        {
        //            X2RebuildWorklistResponse lstResponses = resp as X2RebuildWorklistResponse;
        //            if (lstResponses.ItemList.Count > 0)
        //            {
        //                frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(lstResponses.ItemList,"");
        //                mFrmErrors.ShowDialog();
        //                mFrmErrors.Dispose();

        //            }
        //            else
        //            {
        //                MessageBox.Show("Complete.");
        //            }
        //        }
        //        else
        //        {
        //            if (Items.Count > 0)
        //            {
        //                frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(Items, resp.Exception.Value);
        //                mFrmErrors.ShowDialog();
        //                mFrmErrors.Dispose();

        //            }
        //            else
        //            {
        //                MessageBox.Show("An unknown error has occurred !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }

        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString(), "Unable to Load InstanceID's for State");
        //    }
        //    finally
        //    {
        //        if (null != conn)
        //            conn.Close();
        //    }
        }

        public string GetConnectionForCurrentBranch()
        {
            if (treeMain.SelectedNode != null)
            {
                TreeNode topNode = GetTopNode(treeMain.SelectedNode);
                if (topNode != null)
                {
                    return topNode.Tag.ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void WorkList_Command(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode.Tag is InstanceItem)
            {
                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                CreateWorkListTrackListForSelectedInstance(SAHL.X2InstanceManager.Items.General.ListType.WorkList, mItem.ID);
            }
        }

        private void TrackList_Command(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode.Tag is InstanceItem)
            {
                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                CreateWorkListTrackListForSelectedInstance(SAHL.X2InstanceManager.Items.General.ListType.TrackList, mItem.ID);
            }
        }
        #endregion

        #region TreePopulation


        private void BuildTree(string ConnectionString, SAHL.X2InstanceManager.Items.General.BuildType buildType, TreeNode serverNodeToRefresh)
        {
            if (String.IsNullOrEmpty(ConnectionString))
                return;

            SqlConnection Connection = null;

            Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            if (treeMain.Nodes.Count == 0)
            {
                TreeNode rootNode = new TreeNode();
                rootNode.Text = "X2 Databases";
                rootNode.ImageIndex = 0;
                rootNode.SelectedImageIndex = 0;
                treeMain.Nodes.Add(rootNode);
            }

            TreeNode serverNode = null;
            if (buildType == General.BuildType.Refresh)
            {
                serverNode = serverNodeToRefresh;
            }
            else
            {
                serverNode = new TreeNode();
                serverNode.Text = Connection.DataSource + "       ( " + EngineConnectionString + " )";
                serverNode.Tag = ConnectionString;
                serverNode.ImageIndex = 1;
                serverNode.SelectedImageIndex = 1;
                treeMain.Nodes[0].Nodes.Add(serverNode);
            }
            TreeNode dbNode = new TreeNode();
            dbNode.Text = Connection.Database;
            dbNode.ImageIndex = 0;
            dbNode.Tag = "db";
            dbNode.SelectedImageIndex = 0;
            serverNode.Nodes.Add(dbNode);

            TreeNode processNode = new TreeNode();
            processNode.Text = "Processes";
            processNode.ImageIndex = 16;
            processNode.SelectedImageIndex = 16;
            dbNode.Nodes.Add(processNode);
            processNode.Tag = "Processes";
            AddProcessNodes(Connection, processNode);

            Connection.Close();

        }

        private void AddProcessNodes(SqlConnection Connection, TreeNode processNode)
        {
            List<TreeNode> lstProcesses = new List<TreeNode>();
            SqlCommand cmd = null;
            
            cmd = Connection.CreateCommand();
 
            cmd.CommandText = "Select Distinct [Name] from [X2].Process order by [Name]";
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                TreeNode procNode = new TreeNode();
                procNode.Text = (string)mReader[0];
                procNode.Tag = "Process";
                procNode.ImageIndex = 15;
                procNode.SelectedImageIndex = 15;
                processNode.Nodes.Add(procNode);
                lstProcesses.Add(procNode);
               
            }
            mReader.Close();
            cmd.Dispose();
            for (int x = 0; x < lstProcesses.Count; x++)
            {
                AddWorkFlows(Connection, lstProcesses[x]);
            }
        }

        private void AddWorkFlows(SqlConnection Connection, TreeNode processNode)
        {
            List<int> lstWorkFlows = new List<int>();
            List<TreeNode> lstWorkFlowNodes = new List<TreeNode>();
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select max(ID) from [X2].Process (nolock) where [Name] = @ProcessName";
            SqlParameter nameParam = new SqlParameter("@ProcessName", SqlDbType.VarChar);            
            nameParam.Value = processNode.Text;
            cmd.Parameters.Add(nameParam);
            object objID = cmd.ExecuteScalar();
            int processID = (int)objID;
            cmd = null;

            cmd = Connection.CreateCommand();

            cmd.CommandText = "select ID,[Name] from [X2].WorkFlow (nolock) where ProcessID = @ProcessID order by [Name]";
            SqlParameter IDParam = new SqlParameter("@ProcessID", SqlDbType.Int);
            IDParam.Value = processID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                int WorkFlowID = (int)mReader[0];
                string WorkFlowName = (string)mReader[1];
                TreeNode workFlowNode = new TreeNode();
                workFlowNode.Text = WorkFlowName;
                WorkFlowItem mWorkItem = new WorkFlowItem();
                mWorkItem.WorkFlowID = WorkFlowID;
                mWorkItem.WorkFlowName = WorkFlowName;
                workFlowNode.Tag = mWorkItem;
                workFlowNode.ImageIndex = 4;
                workFlowNode.SelectedImageIndex = 4;        

                processNode.Nodes.Add(workFlowNode);
                lstWorkFlowNodes.Add(workFlowNode);
                lstWorkFlows.Add(WorkFlowID);

                TreeNode CustomVarNode = new TreeNode();
                CustomVarNode.Text = "Custom Variables";
                CustomVarNode.Tag = WorkFlowID;
                CustomVarNode.ImageIndex = 5;
                CustomVarNode.SelectedImageIndex = 5;
                workFlowNode.Nodes.Add(CustomVarNode);

                TreeNode FormNode = new TreeNode();
                FormNode.Text = "Forms";
                FormNode.Tag = WorkFlowID;
                FormNode.ImageIndex = 6;
                FormNode.SelectedImageIndex = 6;
                workFlowNode.Nodes.Add(FormNode);

                TreeNode ExternalActivityNode = new TreeNode();
                ExternalActivityNode.Text = "External Activity Sources";
                ExternalActivityNode.Tag = WorkFlowID;
                ExternalActivityNode.ImageIndex = 7;
                ExternalActivityNode.SelectedImageIndex = 7;
                workFlowNode.Nodes.Add(ExternalActivityNode);

                TreeNode RoleNode = new TreeNode();
                RoleNode.Text = "Roles";
                RoleNode.Tag = WorkFlowID;
                RoleNode.ImageIndex = 8;
                RoleNode.SelectedImageIndex = 8;
                workFlowNode.Nodes.Add(RoleNode);

                TreeNode ActivityNode = new TreeNode();
                ActivityNode.Text = "Activities";
                ActivityNode.Tag = WorkFlowID;
                ActivityNode.ImageIndex = 9;
                ActivityNode.SelectedImageIndex = 9;
                workFlowNode.Nodes.Add(ActivityNode);

                TreeNode StateNode = new TreeNode();
                StateNode.Text = "States";
                StateNode.Name = "States";
                StateNode.Tag = WorkFlowID;
                StateNode.ImageIndex = 10;
                StateNode.SelectedImageIndex = 10;
                workFlowNode.Nodes.Add(StateNode);
            }
            mReader.Close();
            mReader.Dispose();

            if (chkInstanceCount.Checked)
            {
                for (int x = 0; x < lstWorkFlowNodes.Count; x++)
                {
                    cmd = Connection.CreateCommand();
                    cmd.CommandText = "select count(*) from [X2].Instance (nolock) "
                                       + "where [X2].Instance.WorkFlowID = @WorkFlowID";
                    cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
                    WorkFlowItem wrkFlowItem = lstWorkFlowNodes[x].Tag as WorkFlowItem;
                    cmd.Parameters["@WorkFlowID"].Value = wrkFlowItem.WorkFlowID;
                    object o = cmd.ExecuteScalar();
                    if (o != null)
                    {
                        int y = (int)o;
                        lstWorkFlowNodes[x].Text += " (" + y.ToString() + ")";
                    }
                    cmd.Dispose();
                }
            }

            tabControlMain.TabPages[0].Text = "";

            
        }

        private void CreateCustomVariableList(int WorkFlowID)
        {
            List<CustomVariableItem> lstCustomVariables = new List<CustomVariableItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
       

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select [Name] from [X2].WorkFlow (nolock) where ID = @WorkFlowID order by [Name]";
            SqlParameter IDParam = new SqlParameter("@WorkFlowID", SqlDbType.VarChar);
            IDParam.Value = WorkFlowID;
            cmd.Parameters.Add(IDParam);
            object objName = cmd.ExecuteScalar();
            string workFlowName = (string)objName;


            cmd.CommandText = "select Column_Name,Data_Type from INFORMATION_SCHEMA.columns where table_name = @WorkflowName and TABLE_SCHEMA = 'X2DATA' order by Column_Name";
            SqlParameter nameParam = new SqlParameter("@WorkflowName", SqlDbType.VarChar);
            nameParam.Value = workFlowName;
            cmd.Parameters.Add(nameParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                CustomVariableItem mItem = new CustomVariableItem();
                mItem.Name = mReader[0].ToString();
                mItem.Type = mReader[1].ToString();
                lstCustomVariables.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
          
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateCustomVariableList(lstCustomVariables);      

        }


        private void PopulateCustomVariableList(List<CustomVariableItem> lstCustomVariables)
        {
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Variable Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader typeHeader = new ColumnHeader();
            typeHeader.Text = "Type";
            typeHeader.Width = 100;
            typeHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(nameHeader);
            listViewMain.Columns.Add(typeHeader);

            for (int x = 0; x < lstCustomVariables.Count; x++)
            {
                string[] strItem = new string[]     {   lstCustomVariables[x].Name, 
                                                        lstCustomVariables[x].Type,                                                        
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                listViewMain.Items.Add(mItem);
            }
            tabControlMain.TabPages[0].Text = "Custom Variables";
        }

        private void CreateCustomFormList(int WorkFlowID)
        {
            List<CustomFormItem> lstCustomForms = new List<CustomFormItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
         
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();


            cmd.CommandText = "select [Name],[Description] from [X2].Form (nolock) where WorkFlowID = @WorkFlowID order by [Name]";
            SqlParameter IDParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            IDParam.Value = WorkFlowID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                CustomFormItem mItem = new CustomFormItem();
                mItem.Name = mReader[0].ToString();
                mItem.Description = mReader[1].ToString();
                lstCustomForms.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
         
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateCustomFormList(lstCustomForms);

        }

        private void CreateExternalActivityList(int WorkFlowID)
        {
            List<ExternalActivityItem> lstExternalActivities = new List<ExternalActivityItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
           
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select [Name],[Description] from [X2].ExternalActivity (nolock) where WorkFlowID = @WorkFlowID order by [Name]";
            SqlParameter IDParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            IDParam.Value = WorkFlowID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                ExternalActivityItem mItem = new ExternalActivityItem();
                mItem.Name = mReader[0].ToString();
                mItem.Description = mReader[1].ToString();                
                lstExternalActivities.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();         
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateExternalActivityList(lstExternalActivities);  
        }

        private void PopulateExternalActivityList(List<ExternalActivityItem> lstExternalActivities)
        {
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader descriptionHeader = new ColumnHeader();
            descriptionHeader.Text = "Description";
            descriptionHeader.Width = 250;
            descriptionHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(nameHeader);
            listViewMain.Columns.Add(descriptionHeader);            

            for (int x = 0; x < lstExternalActivities.Count; x++)
            {
                string[] strItem = new string[]     {   lstExternalActivities[x].Name, 
                                                        lstExternalActivities[x].Description                                                        
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                listViewMain.Items.Add(mItem);
            }

            tabControlMain.TabPages[0].Text = "External Activity Sources";
        }

        private void PopulateCustomFormList(List<CustomFormItem> lstCustomForms)
        {
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Form Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader typeHeader = new ColumnHeader();
            typeHeader.Text = "Description";
            typeHeader.Width = 100;
            typeHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(nameHeader);
            listViewMain.Columns.Add(typeHeader);

            for (int x = 0; x < lstCustomForms.Count; x++)
            {
                string[] strItem = new string[]     {   lstCustomForms[x].Name, 
                                                        lstCustomForms[x].Description,                                                        
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                listViewMain.Items.Add(mItem);
            }
            tabControlMain.TabPages[0].Text = "Forms";
        }

        private void CreateRoleList(int WorkFlowID)
        {
            List<RoleItem> lstCustomForms = new List<RoleItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
          

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select [Name],[Description],isDynamic from [X2].SecurityGroup (nolock) where WorkFlowID = @WorkFlowID order by [Name]";
            SqlParameter IDParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            IDParam.Value = WorkFlowID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                RoleItem mItem = new RoleItem();
                mItem.Name = mReader[0].ToString();
                mItem.Description = mReader[1].ToString();
                mItem.IsDynamic = (bool)mReader[2];
                lstCustomForms.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
      
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateRoleList(lstCustomForms);

        }

        private void PopulateRoleList(List<RoleItem> lstRoles)
        {
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Role Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader descriptionHeader = new ColumnHeader();
            descriptionHeader.Text = "Description";
            descriptionHeader.Width = 250;
            descriptionHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader isDynamicHeader = new ColumnHeader();
            isDynamicHeader.Text = "Is Dynamic";
            isDynamicHeader.Width = 100;
            isDynamicHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(nameHeader);
            listViewMain.Columns.Add(descriptionHeader);
            listViewMain.Columns.Add(isDynamicHeader);

            for (int x = 0; x < lstRoles.Count; x++)
            {
                string[] strItem = new string[]     {   lstRoles[x].Name, 
                                                        lstRoles[x].Description,  
                                                        lstRoles[x].IsDynamic.ToString()                                  
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                listViewMain.Items.Add(mItem);
            }

            tabControlMain.TabPages[0].Text = "Roles";
        }

        private void CreateWorkListTrackListForSelectedInstance(SAHL.X2InstanceManager.Items.General.ListType listType, int InstanceID)
        {
            
             List<WorkListTrackListItem> listWorkListItems = new List<WorkListTrackListItem>();

             SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
         
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
   

            switch (listType)
            {
                case General.ListType.TrackList:
                    {
                        cmd.CommandText = "Select ADUserName,ListDate from [X2].TrackList (nolock) where InstanceID = @InstanceID order by ListDate";
                        break;
                    }
                case General.ListType.WorkList:
                    {
                        cmd.CommandText = "Select ADUserName,ListDate,Message from [X2].WorkList (nolock) where InstanceID = @InstanceID order by ListDate";
                        break;
                    }
            }
            cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));

            cmd.Parameters["@InstanceID"].Value = InstanceID;
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                WorkListTrackListItem mItem = new WorkListTrackListItem();
                mItem.ADUserName = mReader[0].ToString();
                mItem.ListDate = mReader[1].ToString();
                if (listType == General.ListType.WorkList)
                {
                    mItem.Message = mReader[2].ToString();
                }
                listWorkListItems.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
   
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateWorkListTrackList(listType, listWorkListItems);
        }

        private void PopulateWorkListTrackList(General.ListType listType, List<WorkListTrackListItem> listWorkListTrackListItems)
        {
            ColumnHeader ADUserNameHeader = new ColumnHeader();
            ADUserNameHeader.Text = "ADUserName";
            ADUserNameHeader.Width = 150;
            ADUserNameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader ListDateHeader = new ColumnHeader();
            ListDateHeader.Text = "List Date";
            ListDateHeader.Width = 150;
            ListDateHeader.TextAlign = HorizontalAlignment.Left;


            ColumnHeader MessageHeader = new ColumnHeader();
            MessageHeader.Text = "Message";
            MessageHeader.Width = 200;
            MessageHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(ADUserNameHeader);
            listViewMain.Columns.Add(ListDateHeader);
            if (listType == General.ListType.WorkList)
            {
                tabControlMain.TabPages[0].Text = "Work List";
                listViewMain.Columns.Add(MessageHeader);
            }
            else
            {
                tabControlMain.TabPages[0].Text = "Track List";
            }

            string[] strItem = null;

            for (int x = 0; x < listWorkListTrackListItems.Count; x++)
            {
                if (listType == General.ListType.WorkList)
                {
                    strItem = new string[]     {   listWorkListTrackListItems[x].ADUserName, 
                                                        listWorkListTrackListItems[x].ListDate,
                                                        listWorkListTrackListItems[x].Message 
                                                    };
                }
                else
                {
                    strItem = new string[]     {   listWorkListTrackListItems[x].ADUserName, 
                                                        listWorkListTrackListItems[x].ListDate                                                        
                                                    };
                }
                ListViewItem mItem = new ListViewItem(strItem);
                MainForm.App.listViewMain.Items.Add(mItem);
            }
        }

        private void CreateActivityList(int WorkFlowID)
        {
            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
         

            List<ActivityItem> lstActivities = new List<ActivityItem>();
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
      

            cmd.CommandText =   "select	Activity1.[Name], " 
		                        + "ActivityType.Name, "
		                        + "S1.[Name] as [FromState], "
		                        + "S2.[Name] as [ToState], "
		                        + "Activity1.Priority, "
                                + "ExternalActivityTarget.[Name], "
		                        + "Activity2.[Name] "
                                + "from "
                                + "[X2].Activity Activity1 (nolock) "
                                + "inner join [X2].ActivityType (nolock) "
                                + "on "
                                + "Activity1.[Type] = ActivityType.[ID] "
                                + "inner join [X2].State S1 (nolock) "
                                + "on "
		                        + "Activity1.StateID = S1.ID "
                                + "inner join [X2].State S2 (nolock) " 
                                + "on "
		                        + "Activity1.NextStateID = S2.ID "
                                + "left outer join [X2].ExternalActivityTarget (nolock) "
                                + "on "
                                + "Activity1.ExternalActivityTarget = ExternalActivityTarget.ID "
                                + "left outer join [X2].Activity Activity2 (nolock) "
                                + "on "
		                        + "Activity1.ActivatedByExternalActivity = Activity2.ID "
                                + "where "
		                        + "Activity1.WorkFlowID = @WorkFlowID "
                                + "order by "
                                + "Activity1.[Name]";

            SqlParameter paramID = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            paramID.Value = WorkFlowID;
            cmd.Parameters.Add(paramID);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                ActivityItem mItem = new ActivityItem();
                mItem.Name = (string)mReader[0];
                mItem.Type = (string)mReader[1];               
                mItem.FromState = (string)mReader[2];
                mItem.ToState = (string)mReader[3];
                mItem.Priority = (int)mReader[4];
                if (mReader[5].ToString().Length != 0)
                {
                    mItem.ExternalActivityTarget = (string)mReader[5];
                }
                else
                {
                    mItem.ExternalActivityTarget = "NULL";
                }
                if (mReader[6].ToString().Length != 0)
                {
                    mItem.ActivatedByExternalActivity = (string)mReader[6];
                }
                else
                {
                    mItem.ActivatedByExternalActivity = "NULL";
                }

                lstActivities.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
        
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateActivityList(lstActivities);
            
        }

        private void PopulateActivityList(List<ActivityItem> lstActivities)
        {
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Activity Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader typeHeader = new ColumnHeader();
            typeHeader.Text = "Type";
            typeHeader.Width = 100;
            typeHeader.TextAlign = HorizontalAlignment.Left;


            ColumnHeader fromStateHeader = new ColumnHeader();
            fromStateHeader.Text = "From State";
            fromStateHeader.Width = 120;
            fromStateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader toStateHeader = new ColumnHeader();
            toStateHeader.Text = "To State";
            toStateHeader.Width = 120;
            toStateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader priorityHeader = new ColumnHeader();
            priorityHeader.Text = "Priority";
            priorityHeader.Width = 67;
            priorityHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader externalActivityTargetHeader = new ColumnHeader();
            externalActivityTargetHeader.Text = "External Activity Target";
            externalActivityTargetHeader.Width = 150;
            externalActivityTargetHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader activatedByExternalActivityHeader = new ColumnHeader();
            activatedByExternalActivityHeader.Text = "Activated By External Activity";
            activatedByExternalActivityHeader.Width = 175;
            activatedByExternalActivityHeader.TextAlign = HorizontalAlignment.Left;

            MainForm.App.listViewMain.Clear();

            MainForm.App.listViewMain.Columns.Add(nameHeader);
            MainForm.App.listViewMain.Columns.Add(typeHeader);
            MainForm.App.listViewMain.Columns.Add(fromStateHeader);
            MainForm.App.listViewMain.Columns.Add(toStateHeader);
            MainForm.App.listViewMain.Columns.Add(priorityHeader);
            MainForm.App.listViewMain.Columns.Add(externalActivityTargetHeader);
            MainForm.App.listViewMain.Columns.Add(activatedByExternalActivityHeader);


            for (int x = 0; x < lstActivities.Count; x++)
            {
                string[] strItem = new string[]     {   lstActivities[x].Name, 
                                                        lstActivities[x].Type, 
                                                        lstActivities[x].FromState, 
                                                        lstActivities[x].ToState, 
                                                        lstActivities[x].Priority.ToString(),
                                                        lstActivities[x].ExternalActivityTarget,
                                                        lstActivities[x].ActivatedByExternalActivity
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                MainForm.App.listViewMain.Items.Add(mItem);
            }
            tabControlMain.TabPages[0].Text = "Activities";
        }

        public void CreateStateList(int WorkFlowID, TreeNode StateParentNode)
        {
            StateParentNode.Nodes.Clear();
            List<StateItem> lstStates = new List<StateItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();


            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select [X2].State.[ID],[X2].State.[Name],[X2].StateType.[Name] as [StateType],ForwardState from [X2].State (nolock) Inner Join [X2].StateType (nolock) on [X2].State.Type = [X2].StateType.ID where WorkFlowID = @WorkFlowID order by [X2].State.[Name]";
            SqlParameter IDParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            IDParam.Value = WorkFlowID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                StateItem mItem = new StateItem();
                mItem.ID = Convert.ToInt32(mReader[0].ToString());
                mItem.Name = mReader[1].ToString();
                mItem.Type = mReader[2].ToString();
                mItem.ForwardState = mReader[3].ToString();
                lstStates.Add(mItem);


                TreeNode stateNode = new TreeNode();
                stateNode.Text = mReader[1].ToString();
                stateNode.Tag = mItem;
                stateNode.ImageIndex = GetImageIndexForState(mReader[2].ToString());
                stateNode.SelectedImageIndex = GetImageIndexForState(mReader[2].ToString());
                stateNode.Name = mReader[1].ToString();
                StateParentNode.Nodes.Add(stateNode);
            }
            mReader.Close();
            mReader.Dispose();
            cmd.Dispose();
            cmd = null;

            if (chkInstanceCount.Checked)
            {
                cmd = Connection.CreateCommand();
                cmd.CommandTimeout = 999;

                foreach (TreeNode n in StateParentNode.Nodes)
                {
                    cmd.CommandText = "select count(*) from [X2].Instance (nolock) where StateID = @StateIDParam and WorkFlowID = @WorkFlowID";
                    cmd.Parameters.Add(new SqlParameter("@StateIDParam", SqlDbType.Int));
                    StateItem sItem = (StateItem)n.Tag;
                    cmd.Parameters["@StateIDParam"].Value = sItem.ID;
                    cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
                    cmd.Parameters["@WorkFlowID"].Value = WorkFlowID;
                    object objCount = cmd.ExecuteScalar();
                    int count = (int)objCount;
                    n.Text += " (" + count.ToString() + ")";
                    cmd.Parameters.Clear();
                }
                cmd.Dispose();
                cmd = null;
                Connection.Close();
            }

            PopulateStateList(lstStates, StateSelectionType.all);
        }

        private void CreateStateListForSelectedState(int stateID, TreeNode stateNode)
        {

            List<StateItem> lstStates = new List<StateItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
          

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
          
            cmd.CommandText = "select [X2].State.[ID],[X2].State.[Name],[X2].StateType.[Name] as [StateType],ForwardState from [X2].State (nolock) Inner Join [X2].StateType (nolock) on [X2].State.Type = [X2].StateType.ID where [X2].State.[ID] = @StateID order by [X2].State.[Name]";
            SqlParameter IDParam = new SqlParameter("@StateID", SqlDbType.Int);
            IDParam.Value = stateID;
            cmd.Parameters.Add(IDParam);
            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                StateItem mItem = new StateItem();
                mItem.Name = mReader[1].ToString();
                mItem.Type = mReader[2].ToString();
                mItem.ForwardState = mReader[3].ToString();
                lstStates.Add(mItem);
            }
            mReader.Close();
            mReader.Dispose();
          
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateStateList(lstStates,StateSelectionType.selected);
            CreateInstanceListForState(stateID);
            CreateInstanceNodesForState(stateID, stateNode);

        }

        private void PopulateStateList(List<StateItem> lstStates,StateSelectionType selType)
        {            
            if (selType == StateSelectionType.selected)
            {
                if (tabControlMain.TabPages.Count == 1)
                {
                    tabControlMain.Controls.Add(new TabPage("State Info"));
                }
            }
            else
            {
                if (tabControlMain.TabPages.Count == 2)
                {
                    tabControlMain.Controls.Remove(tabControlMain.TabPages[1]);
                }
            }
            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "State Name";
            nameHeader.Width = 150;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader typeHeader = new ColumnHeader();
            typeHeader.Text = "Type";
            typeHeader.Width = 75;
            typeHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader stateForwardHeader = new ColumnHeader();
            stateForwardHeader.Text = "Forward State";
            stateForwardHeader.Width = 100;
            stateForwardHeader.TextAlign = HorizontalAlignment.Left;
            ListView listViewStateInfo = new ListView();
 
            if (selType == StateSelectionType.selected)
            {
                listViewStateInfo.View = View.Details;
                if (tabControlMain.TabPages[1].Controls.Count == 0)
                {
                    listViewStateInfo.Dock = DockStyle.Fill;
                    listViewStateInfo.FullRowSelect = true;
                    tabControlMain.TabPages[1].Controls.Add(listViewStateInfo);
                    listViewStateInfo.Columns.Add(nameHeader);
                    listViewStateInfo.Columns.Add(typeHeader);
                    listViewStateInfo.Columns.Add(stateForwardHeader);

                }
                listViewStateInfo.Items.Clear();
            }
            else
            {
                listViewMain.Clear();
                listViewMain.Columns.Add(nameHeader);
                listViewMain.Columns.Add(typeHeader);
                listViewMain.Columns.Add(stateForwardHeader);
            }
            for (int x = 0; x < lstStates.Count; x++)
            {
                string[] strItem = new string[]     {   lstStates[x].Name, 
                                                        lstStates[x].Type,  
                                                        lstStates[x].ForwardState.ToString()                                  
                                                    };
                ListViewItem mItem = new ListViewItem(strItem);
                if (selType == StateSelectionType.selected)
                {
                    listViewStateInfo.Items.Add(mItem);
                    tabControlMain.TabPages[0].Text = "Instances";
                    listViewMain.Clear();
                }
                else
                {
                    listViewMain.Items.Add(mItem);
                    tabControlMain.TabPages[0].Text = "States";
                }
            }
        }

        private void CreateInstanceListForState(int stateID)
        {
            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
    
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
            cmd.CommandTimeout = 999;
            cmd.CommandText = "select i.ID, i.Name,i.Subject,i.CreatorADUserName, i.ActivityADUserName, a.name, wl.ADUserName "
            + "from [X2].Instance i (nolock) "
            + "left outer join x2.activity a (nolock) on i.activityid=a.id "
            + "left outer join x2.worklist wl (nolock) on i.id=wl.instanceid "
            + "where i.StateID = @StateID ";
            cmd.Parameters.Add(new SqlParameter("@StateID", SqlDbType.Int));
            cmd.Parameters["@StateID"].Value = stateID;
            SqlDataReader mReader = cmd.ExecuteReader();
            int count = 0;
            while (mReader.Read())
            {
                count++;
                if (count <= 50)
                {
                    SingleInstance mItem = new SingleInstance(mReader);
                    PopulateSingleInstanceList(mItem);
                }
                else
                {
                    break;
                }
            }

            mReader.Close();
            mReader.Dispose();
       
            cmd.Dispose();
            cmd = null;
            Connection.Close();
        }

        private void CreateInstanceListForWorkFlow(int WorkFlowID)
        {
            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
         
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select top 50 s.name, st.name, i.id, i.name, i.subject, i.CreatorADUserName, I.ActivityADUSerName, a.Name, WL.ADUSerName, I.ParentInstanceID, i.SourceInstanceID, s.ID "
                            + "from x2.instance i (nolock)  "
                            + "join x2.state s (nolock) on i.stateid=s.id "
                            + "join x2.statetype st (nolock) on s.type = st.id "
                            + "left outer join x2.activity a (nolock) on i.activityid=a.id "
                            + "left outer join x2.worklist wl (nolock) on i.id=wl.instanceid "
                            + "where i.workflowid=@workflowid "
                            + "order by i.id desc";
            cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
            cmd.Parameters["@WorkFlowID"].Value = WorkFlowID;
            SqlDataReader mReader = cmd.ExecuteReader();
            int count = 0;
            listViewMain.Clear();
            mustStop = false;
            btnStop.Enabled = true;
            Application.DoEvents();
            int y = 0;
            while (mReader.Read() && mustStop == false)
            {

                if (y == 100)
                {
                    Application.DoEvents();
                    y = 0;
                }
                if (mustStop == true)
                {

                    btnStop.Enabled = false;
                    if (MessageBox.Show("Stop generating instance list?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        mustStop = false;
                        btnStop.Enabled = true;
                    }

                }
                y++;
                count++;
                WorkFlowInstanceItem mItem = new WorkFlowInstanceItem(mReader);
                PopulateWorkFlowInstanceList(mItem);
                y++;
            }
            mustStop = false;
            btnStop.Enabled = false;
            mReader.Close();
            mReader.Dispose();
          
            cmd.Dispose();
            cmd = null;
            Connection.Close();
         }
        
        public void CreateInstanceNodesForState(int stateID,TreeNode StateNode)
        {

            StateNode.Nodes.Clear();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
       
            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();

            cmd.CommandText = "select [ID],[Name],[ParentInstanceID],activityDate,activityADUserName from [X2].Instance (nolock) where StateID = @StateID order by [ID]";
            cmd.Parameters.Add(new SqlParameter("@StateID",SqlDbType.VarChar));
            cmd.Parameters["@StateID"].Value = stateID;
            SqlDataReader mReader = cmd.ExecuteReader();
            while(mReader.Read())
            {
                bool isLocked = true;
                TreeNode instanceNode = new TreeNode();
                instanceNode.Text = mReader[0].ToString() + " : " + mReader[1].ToString();
                instanceNode.Name = mReader[0].ToString();
                InstanceItem mItem = new InstanceItem();
                mItem.ID = Convert.ToInt32( mReader[0].ToString());
                if (mReader[2].ToString().Length > 0)
                {
                    mItem.ParentInstance = Convert.ToInt32(mReader[2].ToString());
                }
                if (mReader[3].ToString().Length == 0 && mReader[4].ToString().Length == 0)
                {
                    isLocked = false;
                }

                instanceNode.Tag = mItem;
                instanceNode.ImageIndex = 50;
                instanceNode.SelectedImageIndex = 50;
                if (isLocked)
                {
                    instanceNode.ImageIndex = 14;
                    instanceNode.SelectedImageIndex = 14;
                }
                StateNode.Nodes.Add(instanceNode);

                TreeNode instanceHistoryNode = new TreeNode();
                instanceHistoryNode.Text = "History";
                instanceHistoryNode.Tag = mItem;
                instanceHistoryNode.ImageIndex = 50;
                instanceHistoryNode.SelectedImageIndex = 50;
              
                instanceNode.Nodes.Add(instanceHistoryNode);

                TreeNode ParentInstanceNode = new TreeNode();
                ParentInstanceNode.Text = "Parent Instance";
                ParentInstanceNode.Tag = mItem;
                ParentInstanceNode.ImageIndex = 50;
                ParentInstanceNode.SelectedImageIndex = 50;
                instanceNode.Nodes.Add(ParentInstanceNode);

                TreeNode instanceTrackListNode = new TreeNode();
                instanceTrackListNode.Text = "Track List";
                instanceTrackListNode.Tag = mItem;
                instanceTrackListNode.ImageIndex = 50;
                instanceTrackListNode.SelectedImageIndex = 50;
                instanceNode.Nodes.Add(instanceTrackListNode);

                TreeNode instanceWorkListNode = new TreeNode();
                instanceWorkListNode.Text = "Work List";
                instanceWorkListNode.Tag = mItem;
                instanceWorkListNode.ImageIndex = 50;
                instanceWorkListNode.SelectedImageIndex = 50;
                instanceNode.Nodes.Add(instanceWorkListNode);
            }
            mReader.Close();
            mReader.Dispose();
          
            cmd.Dispose();
            cmd = null;
            Connection.Close();
        }

        public int CreateInstanceListForSelectedInstance(int InstanceID)
        {

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
          

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
            cmd.CommandText = "select i.ID, i.Name,i.Subject,i.CreatorADUserName, i.ActivityADUserName, a.name, wl.ADUserName "
            + "from [X2].Instance i (nolock) "
            + "left outer join x2.activity a (nolock) on i.activityid=a.id "
            + "left outer join x2.worklist wl (nolock) on i.id=wl.instanceid "
            + "where i.ID = @InstanceID ";
            cmd.Parameters.Add(new SqlParameter("@InstanceID",SqlDbType.Int));
            cmd.Parameters["@InstanceID"].Value = InstanceID;
            SqlDataReader mReader = cmd.ExecuteReader();
            int count = 0;
            while (mReader.Read())
            {
                count++;
                SingleInstance mItem = new SingleInstance(mReader);
                PopulateSingleInstanceList(mItem);
            }

            mReader.Close();
            mReader.Dispose();
      
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            return count;
        }

        public void _PopulateInstanceList(InstanceItem mItem)
        {
            ColumnHeader idHeader = new ColumnHeader();
            idHeader.Text = "Instance ID";
            idHeader.Width = 175;
            idHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Instance Name";
            nameHeader.Width = 175;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader SubjectHeader = new ColumnHeader();
            SubjectHeader.Text = "Subject";
            SubjectHeader.Width = 175;
            SubjectHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader creatorADUserNameHeader = new ColumnHeader();
            creatorADUserNameHeader.Text = "Creator ADUserName";
            creatorADUserNameHeader.Width = 150;
            creatorADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader CreationDateHeader = new ColumnHeader();
            CreationDateHeader.Text = "Creation Date";
            CreationDateHeader.Width = 150;
            CreationDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader StateChangedDateHeader = new ColumnHeader();
            StateChangedDateHeader.Text = "State Changed Date";
            StateChangedDateHeader.Width = 150;
            StateChangedDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader DeadlineDateHeader = new ColumnHeader();
            DeadlineDateHeader.Text = "Deadline Date";
            DeadlineDateHeader.Width = 150;
            DeadlineDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityDateHeader = new ColumnHeader();
            ActivityDateHeader.Text = "ActivityDate";
            ActivityDateHeader.Width = 150;
            ActivityDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityADUserNameHeader = new ColumnHeader();
            ActivityADUserNameHeader.Text = "Activity ADUserName";
            ActivityADUserNameHeader.Width = 150;
            ActivityADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityNameHeader = new ColumnHeader();
            ActivityNameHeader.Text = "Activity Name";
            ActivityNameHeader.Width = 100;
            ActivityNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader PriorityHeader = new ColumnHeader();
            PriorityHeader.Text = "Priority";
            PriorityHeader.Width = 100;
            PriorityHeader.TextAlign = HorizontalAlignment.Left;
            if (listViewMain.Columns.Count == 0 || listViewMain.Columns[0].Text != "Instance Name")
            {
                listViewMain.Clear();
                listViewMain.Columns.Add(idHeader);
                listViewMain.Columns.Add(nameHeader);
                listViewMain.Columns.Add(SubjectHeader);
                listViewMain.Columns.Add(creatorADUserNameHeader);
                listViewMain.Columns.Add(CreationDateHeader);
                listViewMain.Columns.Add(StateChangedDateHeader);
                listViewMain.Columns.Add(DeadlineDateHeader);
                listViewMain.Columns.Add(ActivityDateHeader);
                listViewMain.Columns.Add(ActivityADUserNameHeader);
                listViewMain.Columns.Add(ActivityNameHeader);
                listViewMain.Columns.Add(PriorityHeader);

            }
            else
            {
                if (treeMain.SelectedNode.Tag is InstanceItem)
                {
                    listViewMain.Items.Clear();
                }
            }
            
            string[] strItem = new string[]     {       mItem.ID.ToString(),
                                                        mItem.Name,
                                                        mItem.Subject, 
                                                        mItem.CreatorADUserName, 
                                                        mItem.CreationDate, 
                                                        mItem.StateChangeDate,
                                                        mItem.DeadlineDate,
                                                        mItem.ActivityDate,
                                                        mItem.ActivityADUserName,
                                                        mItem.ActivityName,
                                                        mItem.Priority
                                                    };
            ListViewItem listItem = new ListViewItem(strItem);
            listItem.Tag = mItem;
            if (mItem.Locked)
            {
                listItem.ForeColor = Color.Red;
            }
            MainForm.App.listViewMain.Items.Add(listItem);
        }

        public void PopulateSingleInstanceList(SingleInstance mItem)
        {
            ColumnHeader idHeader = new ColumnHeader();
            idHeader.Text = "Instance ID";
            idHeader.Width = 175;
            idHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Instance Name";
            nameHeader.Width = 175;
            nameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader SubjectHeader = new ColumnHeader();
            SubjectHeader.Text = "Subject";
            SubjectHeader.Width = 175;
            SubjectHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader creatorADUserNameHeader = new ColumnHeader();
            creatorADUserNameHeader.Text = "Creator ADUserName";
            creatorADUserNameHeader.Width = 150;
            creatorADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader LockUser = new ColumnHeader();
            LockUser.Text = "Lock User";
            LockUser.Width = 150;
            LockUser.TextAlign = HorizontalAlignment.Left;

            ColumnHeader LockActivity = new ColumnHeader();
            LockActivity.Text = "Lock Activity";
            LockActivity.Width = 150;
            LockActivity.TextAlign = HorizontalAlignment.Left;

            ColumnHeader AssignedUser = new ColumnHeader();
            AssignedUser.Text = "Assigned User";
            AssignedUser.Width = 150;
            AssignedUser.TextAlign = HorizontalAlignment.Left;

            if (listViewMain.Columns.Count == 0 || listViewMain.Columns[0].Text != "Instance Name")
            {
                listViewMain.Clear();
                listViewMain.Columns.Add(idHeader);
                listViewMain.Columns.Add(nameHeader);
                listViewMain.Columns.Add(SubjectHeader);
                listViewMain.Columns.Add(creatorADUserNameHeader);
                listViewMain.Columns.Add(LockUser);
                listViewMain.Columns.Add(LockActivity);
                listViewMain.Columns.Add(AssignedUser);

            }
            else
            {
                if (treeMain.SelectedNode.Tag is InstanceItem)
                {
                    listViewMain.Items.Clear();
                }
            }

            string[] strItem = new string[]     {       mItem.ID.ToString(),
                                                        mItem.Name,
                                                        mItem.Subject, 
                                                        mItem.CreatorADUserName, 
                                                        mItem.LockedActivity, 
                                                        mItem.LockUser,
                                                        mItem.WorkListUser,
                                                    };
            ListViewItem listItem = new ListViewItem(strItem);
            listItem.Tag = mItem;
            MainForm.App.listViewMain.Items.Add(listItem);
        }

        private void PopulateWorkFlowInstanceList(WorkFlowInstanceItem mItem)
        {
            ColumnHeader StateNameHeader = new ColumnHeader();
            StateNameHeader.Text = "State Name";
            StateNameHeader.Width = 175;
            StateNameHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader StateTypeHeader = new ColumnHeader();
            StateTypeHeader.Text = "State Type";
            StateTypeHeader.Width = 75;
            StateTypeHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader idHeader = new ColumnHeader();
            idHeader.Text = "Instance ID";
            idHeader.Width = 175;
            idHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader nameHeader = new ColumnHeader();
            nameHeader.Text = "Instance Name";
            nameHeader.Width = 175;
            nameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader SubjectHeader = new ColumnHeader();
            SubjectHeader.Text = "Subject";
            SubjectHeader.Width = 175;
            SubjectHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader creatorADUserNameHeader = new ColumnHeader();
            creatorADUserNameHeader.Text = "Creator ADUserName";
            creatorADUserNameHeader.Width = 150;
            creatorADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityADUserNameHeader = new ColumnHeader();
            ActivityADUserNameHeader.Text = "Activity ADUserName";
            ActivityADUserNameHeader.Width = 150;
            ActivityADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityNameHeader = new ColumnHeader();
            ActivityNameHeader.Text = "Activity Name";
            ActivityNameHeader.Width = 100;
            ActivityNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader AssignedUser = new ColumnHeader();
            AssignedUser.Text = "Assigned User";
            AssignedUser.Width = 100;
            AssignedUser.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ParentInstanceID = new ColumnHeader();
            ParentInstanceID.Text = "Parent InstanceID";
            ParentInstanceID.Width = 100;
            ParentInstanceID.TextAlign = HorizontalAlignment.Left;
            ColumnHeader SourceInstance = new ColumnHeader();
            SourceInstance.Text = "Source InstanceID";
            SourceInstance.Width = 100;
            SourceInstance.TextAlign = HorizontalAlignment.Left;  
       

            if (listViewMain.Columns.Count == 0)
            {
                listViewMain.Clear();

                listViewMain.Columns.Add(StateNameHeader);
                listViewMain.Columns.Add(StateTypeHeader);
                listViewMain.Columns.Add(idHeader);
                listViewMain.Columns.Add(nameHeader);
                listViewMain.Columns.Add(SubjectHeader);
                listViewMain.Columns.Add(creatorADUserNameHeader);
                
                listViewMain.Columns.Add(ActivityADUserNameHeader);
                listViewMain.Columns.Add(ActivityNameHeader);
                listViewMain.Columns.Add(AssignedUser);
                listViewMain.Columns.Add(ParentInstanceID);
                listViewMain.Columns.Add(SourceInstance);

            }

            string[] strItem = new string[]     {       mItem.StateName,
                                                        mItem.StateType,    
                                                        mItem.InstanceName,
                                                        mItem.ID.ToString(),
                                                        mItem.InstanceName,
                                                        mItem.Subject, 
                                                        mItem.CreatorADUserName, 
                                                        mItem.ActivityADUserName,
                                                        mItem.ActivityName,
                mItem.AssignedUser, mItem.ParentInstanceID.ToString(), mItem.SourceInstanceID.ToString()
                                                    };
            ListViewItem listItem = new ListViewItem(strItem);
            listItem.Tag = mItem;
            if (mItem.Locked)
            {
                listItem.ForeColor = Color.Red;
            }
            //            tabControlMain.TabPages[0].Text = "Instance";
            MainForm.App.listViewMain.Items.Add(listItem);
            
        }

        private void CreateHistoryForInstance(int InstanceID)
        {

            List<InstanceHistoryItem> lstHistoryItem = new List<InstanceHistoryItem>();

            SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
            Connection.Open();
         

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
         

            cmd.CommandText = "select [X2].WorkFlowHistory.[ID], "
                            + "[X2].State.[Name],[X2].Activity.[Name], "
                            + "[X2].WorkFlowHistory.CreatorADUserName, "
                            + "[X2].WorkFlowHistory.CreationDate, "
                            + "[X2].WorkFlowHistory.StateChangeDate, "
                            + "[X2].WorkFlowHistory.DeadlineDate, "
                            + "[X2].WorkFlowHistory.ActivityDate, "
                            + "[X2].WorkFlowHistory.Priority "
                            + "from [X2].WorkFlowHistory (nolock) "
                            + "inner join [X2].State (nolock) "
                            + "on [X2].WorkFlowHistory.StateID = [X2].State.ID "
                            + "inner join [X2].Activity (nolock) "
                            + "on [X2].WorkFlowHistory.ActivityID = [X2].Activity.ID "
                            + "where [X2].WorkFlowHistory.InstanceID = @InstanceID "
                            + "order by [X2].WorkFlowHistory.[StateChangeDate]";

            cmd.Parameters.Add(new SqlParameter("@InstanceID",SqlDbType.Int));
            cmd.Parameters["@InstanceID"].Value = InstanceID;

            SqlDataReader mReader = cmd.ExecuteReader();

            while (mReader.Read())
            {
                InstanceHistoryItem mItem = new InstanceHistoryItem();
                mItem.ID = Convert.ToInt32(mReader[0].ToString());
                mItem.State = mReader[1].ToString();
                mItem.Activity = mReader[2].ToString();
                mItem.CreatorADUserName = mReader[3].ToString();
                mItem.CreationDate = mReader[4].ToString();
                mItem.StateChangeDate = mReader[5].ToString();
                mItem.DeadlineDate = mReader[6].ToString();
                mItem.ActivityDate = mReader[7].ToString();
                mItem.Priority = mReader[8].ToString();
                lstHistoryItem.Add(mItem);
            }

            mReader.Close();
            mReader.Dispose();
          
            cmd.Dispose();
            cmd = null;
            Connection.Close();
            PopulateInstanceHistory(lstHistoryItem);
        }

        private void PopulateInstanceHistory(List<InstanceHistoryItem> lstHistoryItem)
        {
            ColumnHeader StateHeader = new ColumnHeader();
            StateHeader.Text = "State";
            StateHeader.Width = 150;
            StateHeader.TextAlign = HorizontalAlignment.Center;

            ColumnHeader ActivityHeader = new ColumnHeader();
            ActivityHeader.Text = "Activity";
            ActivityHeader.Width = 100;
            ActivityHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader creatorADUserNameHeader = new ColumnHeader();
            creatorADUserNameHeader.Text = "Creator ADUserName";
            creatorADUserNameHeader.Width = 150;
            creatorADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader CreationDateHeader = new ColumnHeader();
            CreationDateHeader.Text = "Creation Date";
            CreationDateHeader.Width = 150;
            CreationDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader StateChangedDateHeader = new ColumnHeader();
            StateChangedDateHeader.Text = "State Change Date";
            StateChangedDateHeader.Width = 150;
            StateChangedDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader DeadlineDateHeader = new ColumnHeader();
            DeadlineDateHeader.Text = "Deadline Date";
            DeadlineDateHeader.Width = 150;
            DeadlineDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ActivityDateHeader = new ColumnHeader();
            ActivityDateHeader.Text = "ActivityDate";
            ActivityDateHeader.Width = 150;
            ActivityDateHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ADUserNameHeader = new ColumnHeader();
            ADUserNameHeader.Text = "ADUserName";
            ADUserNameHeader.Width = 150;
            ADUserNameHeader.TextAlign = HorizontalAlignment.Left;

            ColumnHeader PriorityHeader = new ColumnHeader();
            PriorityHeader.Text = "Priority";
            PriorityHeader.Width = 100;
            PriorityHeader.TextAlign = HorizontalAlignment.Left;

            listViewMain.Clear();

            listViewMain.Columns.Add(StateHeader);
            listViewMain.Columns.Add(ActivityHeader);
            listViewMain.Columns.Add(creatorADUserNameHeader);
            listViewMain.Columns.Add(CreationDateHeader);
            listViewMain.Columns.Add(StateChangedDateHeader);
            listViewMain.Columns.Add(DeadlineDateHeader);
            listViewMain.Columns.Add(ActivityDateHeader);
            listViewMain.Columns.Add(ADUserNameHeader);
            listViewMain.Columns.Add(PriorityHeader);

            for (int x = 0; x < lstHistoryItem.Count; x++)
            {
                string[] strItem = new string[]     {   lstHistoryItem[x].State,
                                                    lstHistoryItem[x].Activity, 
                                                    lstHistoryItem[x].CreatorADUserName,                                                    
                                                    lstHistoryItem[x].CreationDate, 
                                                    lstHistoryItem[x].StateChangeDate,
                                                    lstHistoryItem[x].DeadlineDate,
                                                    lstHistoryItem[x].ActivityDate,
                                                    lstHistoryItem[x].ADUserName,
                                                    lstHistoryItem[x].Priority
                                                    };
                ListViewItem listItem = new ListViewItem(strItem);
                MainForm.App.listViewMain.Items.Add(listItem);
            }
            tabControlMain.TabPages[0].Text = "Instance History";

        }

        private int GetImageIndexForState(string StateType)
        {          
            switch (StateType)
            {
                case "User":
                    {
                        return 9;
                    }
                case "Common":
                    {
                        return 10;
                    }
                case "System":
                    {
                        return 11;
                    }
                case "SystemDecision":
                    {
                        return 12;
                    }

                case "Archive":
                    {
                        return 13;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        #endregion

        public void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (searching == false)
            {
                if (lvwColumnSorter != null)
                {
                    lvwColumnSorter.SortColumn = 0;
                }
                if (m_FindForm != null)
                {
                    if (GetConnectionForCurrentBranch() == null || GetWorkFlowNode(treeMain.SelectedNode) == null
                        && treeMain.SelectedNode.Tag.ToString() != "Process")
                    {
                        m_FindForm.DisableControls();
                        m_FindForm.listViewResults.Items.Clear();
                    }
                    else
                    {
                        m_FindForm.EnableControls();
                        if (GetProcessName() != m_FindForm.CurrentlySelectedProcessName)
                        {
                            for (int x = 0; x < m_FindForm.listViewCriteria.Items.Count; x++)
                            {
                                if (m_FindForm.listViewCriteria.Items[x].SubItems[0].Text == "Workflow Name")
                                {
                                    m_FindForm.listViewCriteria.Items.RemoveAt(x);
                                    m_FindForm.listViewResults.Items.Clear();
                                    for (int y = 0; y < m_FindForm.lstCriteria.Count; y++)
                                    {
                                        if (m_FindForm.lstCriteria[y].Description == "Workflow Name")
                                        {
                                            m_FindForm.lstCriteria.RemoveAt(y);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    List<TreeNode> itms = GetAllWorkFlowNodes(treeMain.SelectedNode);
                    if (itms.Count == 0)
                    {
                        m_FindForm.listViewResults.Items.Clear();
                    }
                    else
                    {
                        
                        for (int x = 0; x < m_FindForm.listViewResults.Items.Count; x++)
                        {
                        bool found = false;
                            for (int y = 0; y < itms.Count; y++)
                            {
                                if (m_FindForm.listViewResults.Items[x].SubItems[0].Text == itms[y].Text)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (found == false)
                            {
                                m_FindForm.listViewResults.Items.RemoveAt(x);
                            }
                        }
                    }
                }
               
                this.Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Retrieving Data...";
                Application.DoEvents();
                if (e.Node.Tag is StateItem == false)
                {
                    if (tabControlMain.TabPages.Count > 1)
                    {
                        tabControlMain.Controls.Remove(tabControlMain.TabPages[1]);
                    }
                }
                this.Cursor = Cursors.WaitCursor;
                switch (e.Node.Text)
                {
                    case "Activities":
                        {
                            CreateActivityList((int)e.Node.Tag);
                            break;
                        }
                    case "Custom Variables":
                        {
                            CreateCustomVariableList((int)e.Node.Tag);
                            break;
                        }
                    case "External Activity Sources":
                        {
                            CreateExternalActivityList((int)e.Node.Tag);
                            break;
                        }
                    case "Forms":
                        {
                            CreateCustomFormList((int)e.Node.Tag);
                            break;
                        }
                    case "Roles":
                        {
                            CreateRoleList((int)e.Node.Tag);
                            break;
                        }
                     case "States":
                        {

                            CreateStateList((int)e.Node.Tag, e.Node);
                            break;
                        }
                    case "History":
                        {
                            if (treeMain.SelectedNode.Tag is InstanceItem)
                            {
                                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                                CreateHistoryForInstance(mItem.ID);
                                CreateHistoryNodesForInstance(mItem.ID, treeMain.SelectedNode);
                            }
                            break;
                        }
                    case "Parent Instance":
                        {
                            if (treeMain.SelectedNode.Tag is InstanceItem)
                            {
                                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                                CreateInstanceListForSelectedInstance(mItem.ParentInstance);
                                int count = CreateInstanceListForSelectedInstance((mItem.ParentInstance));
                                if (count == 0)
                                {
                                    MessageBox.Show("Parent Instance Not Available!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    tabControlMain.TabPages[0].Text = "Parent Instance";
                                }

                            }
                            break;
                        }
                    case "Work List":
                        {
                            if (treeMain.SelectedNode.Tag is InstanceItem)
                            {
                                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                                CreateWorkListTrackListForSelectedInstance(SAHL.X2InstanceManager.Items.General.ListType.WorkList, mItem.ID);
                            }
                            break;
                        }
                    case "Track List":
                        {
                            if (treeMain.SelectedNode.Tag is InstanceItem)
                            {
                                InstanceItem mItem = (InstanceItem)treeMain.SelectedNode.Tag;
                                CreateWorkListTrackListForSelectedInstance(SAHL.X2InstanceManager.Items.General.ListType.TrackList, mItem.ID);
                            }
                            break;
                        }
                    default:
                        {
                            if (e.Node.Tag is StateItem)
                            {
                                StateItem mItem = (StateItem)e.Node.Tag;
                                CreateStateListForSelectedState(mItem.ID, e.Node);
                            }
                            else if (e.Node.Tag is InstanceItem)
                            {
                                InstanceItem mItem = (InstanceItem)e.Node.Tag;
                                CreateInstanceListForSelectedInstance(mItem.ID);
                                InstanceItem mInstanceItem = treeMain.SelectedNode.Tag as InstanceItem;
                                tabControlMain.TabPages[0].Text = "Instance (ID " + mInstanceItem.ID.ToString() + ")";

                            }
                            else if (e.Node.Tag is InstanceVersionHistoryItem)
                            {
                                InstanceVersionHistoryItem mItem = (InstanceVersionHistoryItem)e.Node.Tag;
                                CreateHistoryForInstanceVersion(mItem);
                                tabControlMain.TabPages[0].Text = "Instance History";
                            }
                            else if (e.Node.Tag is WorkFlowItem)
                            {
                                WorkFlowItem o = e.Node.Tag as WorkFlowItem;
                                int workFlowID = o.WorkFlowID;
                                CreateInstanceListForWorkFlow(workFlowID);
                                tabControlMain.TabPages[0].Text = "All Instances";
                            }
                            else
                            {
                                tabControlMain.TabPages[0].Text = "";
                                listViewMain.Clear();
                            }
                            break;
                        }
                }
                lvwColumnSorter = new ListViewColumnSorter();
                this.listViewMain.ListViewItemSorter = lvwColumnSorter;
                Application.DoEvents();

                lblStatus.Text = "Ready";
                this.Cursor = Cursors.Default;
            }
        }
       
        private void CreateHistoryForInstanceVersion(InstanceVersionHistoryItem mItem)
        {
            List<InstanceHistoryItem> lstHistoryItem = new List<InstanceHistoryItem>();

            SqlDataReader mReader = null;
            SqlConnection Connection = null;
            try
            {
                Connection = new SqlConnection(GetConnectionForCurrentBranch());
                Connection.Open();

                SqlCommand cmd = null;
                cmd = Connection.CreateCommand();

                cmd.CommandText = "Select WFH.ID, "
                                    + "S.[Name],[X2].Activity.[Name], "
                                    + "WFH.CreatorADUserName, "
                                    + "WFH.CreationDate, "
                                    + "WFH.StateChangeDate, "
                                    + "WFH.DeadlineDate, "
                                    + "WFH.ActivityDate, "
                                    + "WFH.Priority "
                                    + "from X2.WorkFlowHistory WFH (nolock) "
                                    + "inner join X2.State S (nolock) "
                                    + "on "
                                    + "S.ID = WFH.StateID "
                                    + "inner join X2.WorkFlow W (nolock) "
                                    + "on "
                                    + "W.ID = S.WorkFlowid "
                                    + "inner join [X2].Activity (nolock) "
                                    + "on WFH.ActivityID = [X2].Activity.ID "
                                    + "where instanceid = @InstanceID "
                                    + "and W.ID = @WorkFlowID "                             
                                    + "order by W.ID";

                cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                cmd.Parameters[0].Value = mItem.InstanceID;
                cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
                cmd.Parameters[1].Value = mItem.WorkFlowID;
               
                mReader = cmd.ExecuteReader();
                while (mReader.Read())
                {
                    InstanceHistoryItem mHistoryItem = new InstanceHistoryItem();
                    mHistoryItem.ID = Convert.ToInt32(mReader[0].ToString());
                    mHistoryItem.State = mReader[1].ToString();
                    mHistoryItem.Activity = mReader[2].ToString();
                    mHistoryItem.CreatorADUserName = mReader[3].ToString();
                    mHistoryItem.CreationDate = mReader[4].ToString();
                    mHistoryItem.StateChangeDate = mReader[5].ToString();
                    mHistoryItem.DeadlineDate = mReader[6].ToString();
                    mHistoryItem.ActivityDate = mReader[7].ToString();
                    mHistoryItem.Priority = mReader[8].ToString();
                    lstHistoryItem.Add(mHistoryItem);
                   
                }
                mReader.Close();
                mReader.Dispose();
                PopulateInstanceHistory(lstHistoryItem);
            }
            catch (Exception errExcept)
            {
                MessageBox.Show(errExcept.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {                           
                Connection.Close();
            }
        }

        private void CreateHistoryNodesForInstance(int InstanceID,TreeNode instanceHistoryNode)
        {
            SqlDataReader mReader = null ;
            SqlConnection Connection = null;
            try
            {
                Connection = new SqlConnection(GetConnectionForCurrentBranch());
                Connection.Open();

                SqlCommand cmd = null;
                cmd = Connection.CreateCommand();

                cmd.CommandText = "select W.Name + ' ' + convert(varchar(10),W.ID) as WorkFlow, W.ID, WFH.ID, W.Name "
                                    + "from X2.WorkFlowHistory WFH (nolock) "
                                    + "inner join X2.State S (nolock) "
                                    + "on "
                                    + "S.ID = WFH.StateID "
                                    + "inner join X2.WorkFlow W (nolock) "
                                    + "on "
                                    + "W.ID = S.WorkFlowid "
                                    + "where instanceid = @InstanceID "
                                    + "order by W.ID";

                cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                cmd.Parameters[0].Value = InstanceID;
                mReader = cmd.ExecuteReader();
                while (mReader.Read())
                {
                    bool foundVersion = false;
                    foreach (TreeNode mCheckNode in instanceHistoryNode.Nodes)
                    {
                        if (mCheckNode.Text == mReader[0].ToString())
                        {
                            foundVersion = true;
                            break;
                        }
                    }
                    if (foundVersion == false)
                    {
                        InstanceVersionHistoryItem mItem = new InstanceVersionHistoryItem();
                        mItem.InstanceID = InstanceID;
                        mItem.WorkFlowID = int.Parse(mReader[1].ToString());
                        mItem.WorkFlowHistoryID = int.Parse(mReader[2].ToString());
                        mItem.WorkFlowName = mReader[3].ToString();

                        TreeNode mHistoryNode = new TreeNode();
                        mHistoryNode.Text = mReader[0].ToString();
                        mHistoryNode.SelectedImageIndex = 50;
                        mHistoryNode.ImageIndex = 50;
                        mHistoryNode.Tag = mItem;                        
                        instanceHistoryNode.Nodes.Add(mHistoryNode);                        
                    }
                }
            }
            catch
            {
            }
            finally
            {            
            mReader.Close();
            mReader.Dispose();
            Connection.Close();
            }
        }

        private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ContextMenu cm = null;
            if (e.Button == MouseButtons.Right)
            {
                treeMain.SelectedNode = e.Node;
                if (e.Node.Tag is InstanceItem)
                {
                    cm = new ContextMenu();
                    cm.MenuItems.Add(new MenuItem("Unlock Instance", new EventHandler(this.UnlockInstance_Command)));
                    cm.MenuItems.Add(new MenuItem("Move To", new EventHandler(this.MoveTo_Command)));
                    cm.MenuItems.Add(new MenuItem("About Instance", new EventHandler(AboutInstance_Command)));
                    cm.MenuItems.Add(new MenuItem("Rebuild Instance", new EventHandler(this.Rebuild_Command)));
                    cm.MenuItems.Add(new MenuItem("Edit", new EventHandler(this.EditInstance_Command)));
                    cm.MenuItems.Add(new MenuItem("Reassign", new EventHandler(this.Reassign_Command)));
                    //cm.MenuItems.Add(new MenuItem("Reassign of Ninjaness", new EventHandler(this.ReassignNinja_Command)));

                    InstanceItem mInstance = e.Node.Tag as InstanceItem;
                    int mID = mInstance.ID;

                    SqlConnection Connection = new SqlConnection(GetConnectionForCurrentBranch());
                    Connection.Open();

                    SqlCommand cmd = null;
                    cmd = Connection.CreateCommand();


                    cmd.CommandText = "select activityDate,activityADUserName from [X2].Instance where ID = @InstanceID";
                    cmd.Parameters.Add(new SqlParameter("@InstanceID", SqlDbType.Int));
                    cmd.Parameters[0].Value = mID;
                    SqlDataReader mReader = cmd.ExecuteReader();
                    while (mReader.Read())
                    {
                        if (mReader[0].ToString().Length == 0 && mReader[1].ToString().Length == 0)
                        {
                            cm.MenuItems[0].Enabled = false;
                        }
                    }
                    mReader.Close();
                    mReader.Dispose();

                    cmd.Dispose();
                    cmd = null;
                    Connection.Close();
                    Point P = PointToScreen(e.Location);
                    cm.Show(this, P);

                }
                else if (e.Node.Tag is WorkFlowItem)
                {
                    cm = new ContextMenu();
                    cm.MenuItems.Add(new MenuItem("Rebuild All Instances", new EventHandler(this.RebuildAll_Command)));
                    cm.MenuItems.Add(new MenuItem("Create New Case", new EventHandler(this.CreateNew)));
                    Point P = PointToScreen(e.Location);
                    cm.Show(this, P);
                }
                else if (e.Node.Tag is StateItem)
                {
                    cm = new ContextMenu();
                    cm.MenuItems.Add(new MenuItem("Rebuild Instances", new EventHandler(this.RebuildStateInstances)));
                    Point P = PointToScreen(e.Location);
                    cm.Show(this, P);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null)
            {
                return;
            }
            TreeNode holdNode = treeMain.SelectedNode;

            TreeNode topNode = GetTopNode(holdNode);

            topNode.Nodes.Clear();
            BuildTree(topNode.Tag.ToString(),SAHL.X2InstanceManager.Items.General.BuildType.Refresh,topNode);
            try
            {
                topNode.Nodes[0].Nodes[0].Expand();
                treeMain.SelectedNode = topNode.Nodes[0].Nodes[1];
            }
            catch
            {
                
            }


        }

        private void listViewMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {            
            if (tabControlMain.TabPages[0].Text == "All Instances")
            {
                if (listViewMain.SelectedIndices.Count > 0)
                {
                    WorkFlowInstanceItem mItem = listViewMain.Items[listViewMain.SelectedIndices[0]].Tag as WorkFlowInstanceItem;
                    TreeNode[] foundNodes = treeMain.Nodes.Find(mItem.ID.ToString(),true);
                    if (foundNodes.Length > 0)
                    {
                        treeMain.SelectedNode = foundNodes[0];
                    }
                    else
                    {
                        WorkFlowItem o = treeMain.SelectedNode.Tag as WorkFlowItem;
                        int WorkFlowID = o.WorkFlowID;
                        TreeNode holdNode = null;                       
                        TreeNode[] foundStatesNodes = treeMain.Nodes.Find("States", true);
                        bool foundNode = false;
                        while (foundNode == false)
                        {
                            for (int x = 0; x < foundStatesNodes.Length; x++)
                            {
                                if(GetWorkFlowNode(foundStatesNodes[x]) == treeMain.SelectedNode)
                                {
                                    foundNode = true;
                                    holdNode = foundStatesNodes[x];
                                    break;
                                }
                            }
                        }
                        CreateStateList(WorkFlowID,holdNode);
                        foundStatesNodes = treeMain.Nodes.Find(mItem.StateName, true);
                        if (foundStatesNodes.Length > 0)
                        {
                            CreateInstanceNodesForState(mItem.StateID, foundStatesNodes[0]);
                        }
                        foundNodes = treeMain.Nodes.Find(mItem.ID.ToString(), true);
                        if (foundNodes.Length > 0)
                        {
                            treeMain.SelectedNode = foundNodes[0];
                        }
                    }
                    InstanceItem mInstanceItem = treeMain.SelectedNode.Tag as InstanceItem;
                    tabControlMain.TabPages[0].Text = "Instance (ID " + mInstanceItem.ID.ToString() + ")";
                }                
            }
            if (tabControlMain.TabPages[0].Text == "Instances" && tabControlMain.TabPages[1].Text == "State Info")
            {
                if (listViewMain.SelectedIndices.Count > 0)
                {
                    //InstanceItem mItem = listViewMain.Items[listViewMain.SelectedIndices[0]].Tag as InstanceItem;

                    var tag = listViewMain.Items[listViewMain.SelectedIndices[0]].Tag as SingleInstance;

                    TreeNode[] foundNodes = null;
                    if (tag != null)
                    {
                        foundNodes = treeMain.Nodes.Find(tag.ID.ToString(), true);

                        if ((foundNodes != null) && (foundNodes.Length > 0))
                        {
                            treeMain.SelectedNode = foundNodes[0];
                        }
                        else
                        {
                            StateItem o = treeMain.SelectedNode.Tag as StateItem;
                            int StateID = o.ID;
                            CreateInstanceNodesForState(StateID, treeMain.SelectedNode);
                            foundNodes = treeMain.Nodes.Find(tag.ID.ToString(), true);
                            if (foundNodes.Length > 0)
                            {
                                treeMain.SelectedNode = foundNodes[0];
                            }
                        }
                        StateItem mInstanceItem = treeMain.SelectedNode.Tag as StateItem;
                        tabControlMain.TabPages[0].Text = "Instance (ID " + tag.ID.ToString() + ")";
                    }
                }      
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_FindForm = new frmFindInstance();
            m_FindForm.ShowDialog(this);
        }

        private void listViewMain_ColumnClick(object sender, ColumnClickEventArgs e)
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

            this.listViewMain.Sort();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutBox mBox = new AboutBox();
            mBox.ShowDialog();
            mBox.Dispose();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mustStop = true;
        }

        public string GetProcessName()
        {
            TreeNode node = null;
            if (treeMain.SelectedNode.Tag.ToString() == "Process")
            {
                return treeMain.SelectedNode.Text;
            }
            node = treeMain.SelectedNode.Parent;
            while (node.Tag.ToString() != "Process")
            {
                node = node.Parent;
            }
            return node.Text;
        }

        private void listViewMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEngine_Click(object sender, EventArgs e)
        {

        }
    }
}