using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2Designer.Datasets;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmMapStates : Form
    {
        private WorkFlow m_WorkFlow;
        private SqlTransaction m_Transaction;
        private SqlConnection m_Connection;
        private ProcessDocument m_Doc;
        private int m_NewProcessID;
        public int OriginalWorkFlowID = -1;
        public List<StateChangeItem> lstStateNameChanges = new List<StateChangeItem>();

        public frmMapStates(SqlTransaction Transaction, SqlConnection Connection, ProcessDocument pDocument, WorkFlow workFlow, int ProcessID)
        {
            InitializeComponent();

            m_Connection = Connection;
            m_Transaction = Transaction;
            m_Doc = pDocument;
            m_WorkFlow = workFlow;
            m_NewProcessID = ProcessID;

            ColumnHeader colName = new ColumnHeader();
            colName.Text = "WorkFlow States";
            colName.Width = 150;

            ColumnHeader colType = new ColumnHeader();
            colType.Text = "Type";
            colType.Width = 80;

            ColumnHeader colName2 = new ColumnHeader();
            colName2.Text = "Database States";
            colName2.Width = 150;

            ColumnHeader colType2 = new ColumnHeader();
            colType2.Text = "Type";
            colType2.Width = 80;

            ColumnHeader colInstances = new ColumnHeader();
            colInstances.Text = "Instances";
            colInstances.Width = 100;

            ColumnHeader colName3 = new ColumnHeader();
            colName3.Text = "WorkFlow State";
            colName3.Width = 150;

            ColumnHeader colType3 = new ColumnHeader();
            colType3.Text = "Type";
            colType3.Width = 80;

            ColumnHeader colName4 = new ColumnHeader();
            colName4.Text = "Database State";
            colName4.Width = 150;

            ColumnHeader colType4 = new ColumnHeader();
            colType4.Text = "Type";
            colType4.Width = 80;

            ColumnHeader colInstances2 = new ColumnHeader();
            colInstances2.Text = "Instances";
            colInstances2.Width = 100;

            listViewWorkFlowStates.Columns.Add(colName);
            listViewWorkFlowStates.Columns.Add(colType);

            listViewDatabaseStates.Columns.Add(colName2);
            listViewDatabaseStates.Columns.Add(colType2);
            listViewDatabaseStates.Columns.Add(colInstances);

            listViewMap.Columns.Add(colName3);
            listViewMap.Columns.Add(colType3);
            listViewMap.Columns.Add(colName4);
            listViewMap.Columns.Add(colType4);
            listViewMap.Columns.Add(colInstances2);
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            bool canContinue = true;
            if (listViewWorkFlowStates.SelectedIndices.Count > 0 && listViewDatabaseStates.SelectedIndices.Count > 0)
            {
                if (listViewWorkFlowStates.Items[listViewWorkFlowStates.SelectedIndices[0]].SubItems[1].Text != listViewDatabaseStates.Items[listViewDatabaseStates.SelectedIndices[0]].SubItems[1].Text)
                {
                    DialogResult res = MessageBox.Show("Are you sure you want to map two states of different types?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (res == DialogResult.No)
                    {
                        canContinue = false;
                    }
                }
                if (canContinue)
                {
                    string[] lst = new string[5];
                    lst[0] = listViewWorkFlowStates.Items[listViewWorkFlowStates.SelectedIndices[0]].SubItems[0].Text;
                    lst[1] = listViewWorkFlowStates.Items[listViewWorkFlowStates.SelectedIndices[0]].SubItems[1].Text;
                    lst[2] = listViewDatabaseStates.Items[listViewDatabaseStates.SelectedIndices[0]].SubItems[0].Text;
                    lst[3] = listViewDatabaseStates.Items[listViewDatabaseStates.SelectedIndices[0]].SubItems[1].Text;
                    lst[4] = listViewDatabaseStates.Items[listViewDatabaseStates.SelectedIndices[0]].SubItems[2].Text;

                    string[] newItem = new string[] { lst[0], lst[1], lst[2], lst[3], lst[4] };
                    ListViewItem li = new ListViewItem(newItem);
                    listViewMap.Items.Add(li);
                    StateChangeItem mItem = new StateChangeItem();
                    mItem.OldName = lst[2];
                    mItem.NewName = lst[0];
                    lstStateNameChanges.Add(mItem);
                    listViewWorkFlowStates.Items.RemoveAt(listViewWorkFlowStates.SelectedIndices[0]);
                    listViewDatabaseStates.Items.RemoveAt(listViewDatabaseStates.SelectedIndices[0]);
                }
            }
        }

        private void cmdUndoMap_Click(object sender, EventArgs e)
        {
            if (listViewMap.SelectedIndices.Count != 0)
            {
                ListViewItem mItem = listViewMap.SelectedItems[0];
                string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                ListViewItem undoItem = new ListViewItem(mStr);
                listViewWorkFlowStates.Items.Add(undoItem);

                ListViewItem mItem2 = listViewMap.SelectedItems[0];
                string[] mStr2 = new string[3] { mItem2.SubItems[2].Text, mItem2.SubItems[3].Text, mItem2.SubItems[4].Text };
                ListViewItem undoItem2 = new ListViewItem(mStr2);
                listViewDatabaseStates.Items.Add(undoItem2);

                listViewMap.Items.RemoveAt(listViewMap.SelectedIndices[0]);
                for (int x = 0; x < lstStateNameChanges.Count; x++)
                {
                    if (lstStateNameChanges[x].OldName == mStr2[0])
                    {
                        lstStateNameChanges.RemoveAt(x);
                        break;
                    }
                }
            }
        }

        private void frmMapStates_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Map States for " + m_WorkFlow.WorkFlowName;
                //Get the previous processID
                SqlCommand Cmd = null;
                Cmd = m_Connection.CreateCommand();
                Cmd.Transaction = m_Transaction;
                Cmd.CommandText = "select top 1 ID from [X2].Process where [Name] = @PROCESS_NAME and ID != @NewProcessID order by Version DESC";
                SqlParameter nameParam = new SqlParameter("@PROCESS_NAME", SqlDbType.VarChar);
                nameParam.Value = MainForm.App.GetCurrentView().Document.Name;
                Cmd.Parameters.Add(nameParam);
                Cmd.Parameters.Add("@NewProcessID", SqlDbType.Int);
                Cmd.Parameters["@NewProcessID"].Value = m_NewProcessID;
                object mProcIDObj = Cmd.ExecuteScalar();
                if (mProcIDObj == DBNull.Value || mProcIDObj == null)
                {
                    this.DialogResult = DialogResult.OK;
                    return;
                }
                int ProcID = (int)mProcIDObj;

                Cmd.Parameters.Clear();
                //Get the previous WorkFlowID for the current workflow
                Cmd.CommandText = "select [ID] from [X2].WorkFlow where ProcessID = @ProcessID "
                            + "and [Name] = @WorkFlowName";
                SqlParameter procParam = new SqlParameter("@ProcessID", SqlDbType.Int);
                procParam.Value = ProcID;
                Cmd.Parameters.Add(procParam);
                SqlParameter workFlowNameParam = new SqlParameter("@WorkFlowName", SqlDbType.VarChar);
                workFlowNameParam.Value = m_WorkFlow.WorkFlowName;
                Cmd.Parameters.Add(workFlowNameParam);

                object wkFlowObj = Cmd.ExecuteScalar();
                if (wkFlowObj == DBNull.Value)
                {
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                if (wkFlowObj == null)
                {
                    this.DialogResult = DialogResult.OK;
                    return;
                }
                int WorkFlowID = (int)wkFlowObj;
                OriginalWorkFlowID = WorkFlowID;
                //Get all the states out the db for the old workflow
                SqlDataAdapter SDA = new SqlDataAdapter();
                SqlCommand mCommand = new SqlCommand("select  S.ID, S.[Name], ST.[Name] as TypeName from [X2].State S inner join " +
                                                    "[X2].StateType  ST on S.[Type] = ST.ID where S.WorkFlowID = @WorkFlowID", m_Connection);
                SqlParameter workFlowParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
                workFlowParam.Value = WorkFlowID;
                mCommand.Parameters.Add(workFlowParam);
                mCommand.Connection = m_Connection;
                mCommand.Transaction = m_Transaction;
                SDA.SelectCommand = mCommand;
                DataSet mSet = new DataSet();
                SDA.Fill(mSet);
                for (int x = 0; x < mSet.Tables[0].Rows.Count; x++)
                {
                    SqlCommand InstanceCmd = null;
                    InstanceCmd = m_Connection.CreateCommand();
                    InstanceCmd.Transaction = m_Transaction;
                    InstanceCmd.CommandText = "Select count(*) from [X2].Instance where WorkFlowID = @WorkFlowID and StateID = @StateID";
                    SqlParameter workFlowParam2 = new SqlParameter("@WorkFlowID", SqlDbType.Int);
                    SqlParameter stateParam = new SqlParameter("@StateID", SqlDbType.Int);
                    workFlowParam2.Value = WorkFlowID;
                    stateParam.Value = mSet.Tables[0].Rows[x]["ID"];
                    InstanceCmd.Parameters.Add(workFlowParam2);
                    InstanceCmd.Parameters.Add(stateParam);
                    object instanceCountObj = InstanceCmd.ExecuteScalar();
                    int instanceCount = (int)instanceCountObj;
                    string[] str = new string[] { mSet.Tables[0].Rows[x]["Name"].ToString(), mSet.Tables[0].Rows[x]["TypeName"].ToString(), instanceCount.ToString() };
                    ListViewItem mItem = new ListViewItem(str);
                    listViewDatabaseStates.Items.Add(mItem);
                }
                SDA.Dispose();

                //PublishingLookups dsLookups = new PublishingLookups();
                //GetLookups(m_Transaction, m_Connection, dsLookups);

                for (int x = 0; x < m_WorkFlow.States.Count; x++)
                {
                    if (m_WorkFlow.States[x] is CommonState)
                    {
                        continue;
                    }
                    bool found = false;
                    for (int y = 0; y < listViewDatabaseStates.Items.Count; y++)
                    {
                        if (m_WorkFlow.States[x].Name == listViewDatabaseStates.Items[y].SubItems[0].Text
                            && TranslateType(m_WorkFlow.States[x]) == listViewDatabaseStates.Items[y].SubItems[1].Text)
                        {
                            string[] strMap = new string[5] { m_WorkFlow.States[x].Name, listViewDatabaseStates.Items[y].SubItems[1].Text, listViewDatabaseStates.Items[y].SubItems[0].Text, listViewDatabaseStates.Items[y].SubItems[1].Text, listViewDatabaseStates.Items[y].SubItems[2].Text };
                            ListViewItem mListItem = new ListViewItem(strMap);
                            listViewMap.Items.Add(mListItem);
                            listViewDatabaseStates.Items.RemoveAt(y);
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        string[] strMap = new string[2] { m_WorkFlow.States[x].Name, TranslateType(m_WorkFlow.States[x]) };
                        ListViewItem mListItem = new ListViewItem(strMap);
                        listViewWorkFlowStates.Items.Add(mListItem);
                    }
                }
                //If there are no states to map, check whether there are instances that can be migrated for this workflow
                //and if not, don't show the form

                if (listViewWorkFlowStates.Items.Count == 0 || listViewDatabaseStates.Items.Count == 0)
                {
                    Cmd.Parameters.Clear();
                    Cmd.CommandText = "Select count(*) from [X2].Instance where WorkFlowID = @WorkFlowID";
                    Cmd.Parameters.Add(new SqlParameter("@WorkFlowID", SqlDbType.Int));
                    Cmd.Parameters[0].Value = WorkFlowID;
                    object instanceObj = Cmd.ExecuteScalar();
                    int instanceCount = (int)instanceObj;
                    if ((instanceCount == 0) || (Helpers.Migrate))
                    {
                        DialogResult = DialogResult.OK;
                        return;
                    }
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.ToString());
                ExceptionPolicy.HandleException(except, "X2Designer");
            }
        }

        private string TranslateType(BaseState mState)
        {
            switch (mState.WorkflowItemType)
            {
                case WorkflowItemType.ArchiveState:
                    {
                        return "Archive";
                    }
                case WorkflowItemType.UserState:
                    {
                        return "User";
                    }
                case WorkflowItemType.SystemState:
                    {
                        return "System";
                    }
                case WorkflowItemType.SystemDecisionState:
                    {
                        return "SystemDecision";
                    }
                case WorkflowItemType.HoldState:
                    {
                        return "Hold";
                    }
                default:
                    return "";
            }
        }

        private void GetLookups(SqlTransaction Transaction, SqlConnection Connection, PublishingLookups dsLookups)
        {
            SqlCommand Cmd = null;
            Cmd = Connection.CreateCommand();
            Cmd.Transaction = Transaction;

            SqlDataAdapter SDA = new SqlDataAdapter();
            SqlCommand mCommand = new SqlCommand("select * from [X2].ActivityType;Select* from [X2].ExternalActivityTarget;select * from [X2].StateType", Connection);
            mCommand.Connection = Connection;
            mCommand.Transaction = Transaction;
            SDA.SelectCommand = mCommand;
            SDA.TableMappings.Add("Table", "ActivityType");
            SDA.TableMappings.Add("Table1", "ExternalActivityTarget");
            SDA.TableMappings.Add("Table2", "StateType");
            SDA.Fill(dsLookups);
        }

        public static int GetStateType(BaseState State, PublishingLookups.StateTypeDataTable StateTypes)
        {
            PublishingLookups.StateTypeRow[] Rows;

            switch (State.WorkflowItemType)
            {
                case WorkflowItemType.UserState:
                    Rows = StateTypes.Select("Name = 'User'") as PublishingLookups.StateTypeRow[];
                    if (Rows.Length == 1)
                        return Rows[0].ID;
                    break;
                case WorkflowItemType.SystemState:
                    Rows = StateTypes.Select("Name = 'System'") as PublishingLookups.StateTypeRow[];
                    if (Rows.Length == 1)
                        return Rows[0].ID;
                    break;
                case WorkflowItemType.SystemDecisionState:
                    Rows = StateTypes.Select("Name = 'SystemDecision'") as PublishingLookups.StateTypeRow[];
                    if (Rows.Length == 1)
                        return Rows[0].ID;
                    break;
                case WorkflowItemType.CommonState:
                    Rows = StateTypes.Select("Name = 'Common'") as PublishingLookups.StateTypeRow[];
                    if (Rows.Length == 1)
                        return Rows[0].ID;
                    break;
                case WorkflowItemType.ArchiveState:
                    Rows = StateTypes.Select("Name = 'Archive'") as PublishingLookups.StateTypeRow[];
                    if (Rows.Length == 1)
                        return Rows[0].ID;
                    break;
                default:
                    return -1;
            }
            return -1;
        }

        private void listViewMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewMap.SelectedIndices.Count == 0)
                btnUndoMap.Enabled = false;
            else
                btnUndoMap.Enabled = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //// migrate any previous old instances

            //    SqlCommand Cmd = null;
            //    Cmd = m_Connection.CreateCommand();
            //    Cmd.Transaction = m_Transaction;
            //    Cmd.CommandText = "select top 1 ID from [X2].Process where [Name] = @PROCESS_NAME and ID != @NewProcessID order by Version DESC";
            //    SqlParameter nameParam = new SqlParameter("@PROCESS_NAME", SqlDbType.VarChar);
            //    nameParam.Value = MainForm.App.GetCurrentView().Document.Name;
            //    Cmd.Parameters.Add(nameParam);
            //    Cmd.Parameters.Add("@NewProcessID", SqlDbType.Int);
            //    Cmd.Parameters["@NewProcessID"].Value = m_NewProcessID;
            //    object mProcIDObj = Cmd.ExecuteScalar();
            //    if (mProcIDObj == DBNull.Value)
            //    {
            //        this.DialogResult = DialogResult.Cancel;
            //        return;
            //    }
            //    int ProcID = (int)mProcIDObj;

            //    Cmd.Parameters.Clear();
            //    Cmd.CommandText = "select max(ID) from [X2].WorkFlow where ProcessID = @ProcessID";
            //    SqlParameter procParam = new SqlParameter("@ProcessID", SqlDbType.Int);
            //    procParam.Value = ProcID;
            //    Cmd.Parameters.Add(procParam);

            //    object wkFlowObj = Cmd.ExecuteScalar();
            //    if (wkFlowObj == DBNull.Value)
            //    {
            //        this.DialogResult = DialogResult.Cancel;
            //        return;
            //    }
            //    int WorkFlowID = (int)wkFlowObj;

            //    SqlDataAdapter SDA = new SqlDataAdapter();
            //    SqlCommand mCommand = new SqlCommand("select  S.ID, S.[Name], ST.[Name] as TypeName from [X2].State S inner join " +
            //                                        "[X2].StateType  ST on S.[Type] = ST.ID where S.WorkFlowID = @WorkFlowID", m_Connection);
            //    SqlParameter workFlowParam = new SqlParameter("@WorkFlowID", SqlDbType.Int);
            //    workFlowParam.Value = WorkFlowID;
            //    mCommand.Parameters.Add(workFlowParam);
            //    mCommand.Connection = m_Connection;
            //    mCommand.Transaction = m_Transaction;
            //    SDA.SelectCommand = mCommand;
            //    DataSet mSet = new DataSet();
            //    SDA.Fill(mSet);
        }
    }

    public class StateChangeItem
    {
        private string m_OldName;
        private string m_NewName;

        public StateChangeItem(string OldName, string NewName)
        {
            this.m_OldName = OldName;
            this.m_NewName = NewName;
        }

        public StateChangeItem()
        {
        }

        public string OldName
        {
            get
            {
                return m_OldName;
            }
            set
            {
                m_OldName = value;
            }
        }

        public string NewName
        {
            get
            {
                return m_NewName;
            }
            set
            {
                m_NewName = value;
            }
        }
    }
}