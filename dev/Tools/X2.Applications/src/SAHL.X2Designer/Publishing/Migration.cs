using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2Designer.Forms;

namespace SAHL.X2Designer.Publishing
{
    public class DB
    {
        private SqlConnection conn;
        private SqlTransaction trans;

        public DB(SqlConnection conn, SqlTransaction trans)
        {
            this.conn = conn;
            this.trans = trans;
        }

        public void PopulateDataSet(DataSet ds, string TableName, string Query)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                cmd.Transaction = trans;
                cmd.CommandText = Query;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds, TableName);
            }
            catch (Exception e)
            {
                bool rethrow = ExceptionPolicy.HandleException(e, "X2Designer");
                if (rethrow)
                    throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public object GetSingleValue(string Query)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                cmd.Transaction = trans;
                cmd.CommandText = Query;
                return (cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                bool rethrow = ExceptionPolicy.HandleException(e, "X2Designer");
                if (rethrow)
                    throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return null;
        }

        public void UpdateInstances(List<string> Updates)
        {
            SqlCommand cmd = null;
            try
            {
                foreach (string query in Updates)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                    cmd.Transaction = trans;
                    cmd.CommandText = query;
                    int RowsAffected = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception e)
            {
                bool rethrow = ExceptionPolicy.HandleException(e, "X2Designer");
                if (rethrow)
                    throw;
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
            }
        }
    }

    public class X2MigrateHelpers
    {
        DB db;
        Dictionary<int, string> StateIDMap = new Dictionary<int, string>();
        Dictionary<int, string> ActivityIDMap = new Dictionary<int, string>();
        Dictionary<int, string> ExtActivityIDMap = new Dictionary<int, string>();
        string strQuery = string.Empty;
        List<string> Updates = new List<string>();
        DataSet dsNewWorkFlow = new DataSet();
        int LatestWorkFlowID = -1;
        int originalWorkFlowID = -1;
        private List<int> lstStatesWithChangedSecurity = new List<int>();
        private List<int> NonMatchingActivities = new List<int>();

        public X2MigrateHelpers(List<StateChangeItem> NewMappedStates, SqlConnection conn, SqlTransaction trans, string WorkFlowName, int newProcID)
        {
            db = new DB(conn, trans);
            LatestWorkFlowID = GetLatestWorkFlowID(WorkFlowName);
            GetLatestStateList(dsNewWorkFlow, LatestWorkFlowID);
            GetLatestExternalActivityList(dsNewWorkFlow, LatestWorkFlowID);
            GetLatestActivityList(dsNewWorkFlow, LatestWorkFlowID);

            SqlCommand Cmd = null;
            SqlDataReader mReader = null;
            try
            {
                Cmd = conn.CreateCommand();
                Cmd.CommandTimeout = ConnectionForm.CmdTimeout;
                Cmd.Transaction = trans;
                Cmd.CommandText = "select top 1 ID from [X2].Process where [Name] = @PROCESS_NAME and ID != @NewProcessID order by Version DESC";
                SqlParameter nameParam = new SqlParameter("@PROCESS_NAME", SqlDbType.VarChar);
                nameParam.Value = MainForm.App.GetCurrentView().Document.Name;
                Cmd.Parameters.Add(nameParam);
                Cmd.Parameters.Add("@NewProcessID", SqlDbType.Int);
                Cmd.Parameters["@NewProcessID"].Value = newProcID;
                object mProcIDObj = Cmd.ExecuteScalar();
                if (mProcIDObj == DBNull.Value || mProcIDObj == null)
                {
                    return;
                }
                int oldProcID = (int)mProcIDObj;

                Cmd.Parameters.Clear();

                Cmd.CommandText = "select [ID] from [X2].WorkFlow where ProcessID = @ProcessID "
                            + "and [Name] = @WorkFlowName";
                SqlParameter procParam = new SqlParameter("@ProcessID", SqlDbType.Int);
                procParam.Value = oldProcID;
                Cmd.Parameters.Add(procParam);
                SqlParameter workFlowNameParam = new SqlParameter("@WorkFlowName", SqlDbType.VarChar);
                workFlowNameParam.Value = WorkFlowName;
                Cmd.Parameters.Add(workFlowNameParam);

                object wkFlowObj = Cmd.ExecuteScalar();
                if (wkFlowObj == DBNull.Value)
                {
                    return;
                }
                originalWorkFlowID = (int)wkFlowObj;
                Cmd = null;
                Cmd = conn.CreateCommand();
                Cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                Cmd.Transaction = trans;

                #region

                //                string sgStr =   //-- get states that could possibly be affected by added activities
                //                    "select I.StateID from X2.Instance I "
                //                    +  "inner join "
                //                    +  "X2.Workflow WF "
                //                    +  "on "
                //                    +  "I.workflowid = WF.ID "
                //                    +  "and "
                //                    +  "WF.processid in(@NewProcessID, @OldProcessID) "
                //                    +  "inner join "
                //                    +  "( "
                //    // -- find any added activities
                //                    +  "select A.StateID from X2.Activity A "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "A.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid = @NewProcessID "
                //                    + "where "
                //                    + "A.Name not in "
                //                    + "( "
                //                    + "select B.Name from X2.Activity B "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "B.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid = @OldProcessID "
                //                    + ") "
                //                    + ") ADDACT "
                //                    + "on "
                //                    + "ADDACT.StateID = I.StateID "
                //                    + "group by I.StateID "
                //                    + "union "
                ////-- get states that could possibly be affected by removed activities
                //                    + "select I.StateID from X2.Instance I "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "I.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid in(@NewProcessID, @OldProcessID) "
                //                    + "inner join "
                //                    + "( "
                ////-- find any removed activities
                //                    + "select A.StateID from X2.Activity A "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "A.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid = @OldProcessID "
                //                    + "where "
                //                    + "A.Name not in "
                //                    + "( "
                //                    + "select B.Name from X2.Activity B "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "B.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid = @NewProcessID "
                //                    + ") "
                //                    + ") ADDACT "
                //                    + "on "
                //                    + "ADDACT.StateID = I.StateID "
                //                    + "group by I.StateID "
                //                    + "union "
                ////-- get states that could possibly be affected by activities that have had security added
                //                    + "select I.StateID from X2.Instance I "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "I.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid in(@NewProcessID, @OldProcessID) "
                //                    + "inner join "
                //                    + "( "
                //                    + "select A.StateID from X2.ActivitySecurity ASec "
                //                    + "inner join "
                //                    + "X2.Activity A "
                //                    + "on "
                //                    + "ASec.ActivityID = A.ID "
                //                    + "inner join "
                //                    + "X2.Workflow wf "
                //                    + "on "
                //                    + "wf.id = A.Workflowid "
                //                    + "and "
                //                    + "wf.processid = @NewProcessID "
                //                    + "inner join "
                //                    + "X2.SecurityGroup SG "
                //                    + "on "
                //                    + "ASec.SecurityGroupID = SG.ID "
                //                    + "where "
                //                    + "SG.Name not in  "
                //                    + "( "
                //                    + "select SG.Name from X2.ActivitySecurity ASec "
                //                    + "inner join "
                //                    + "X2.Activity A "
                //                    + "on "
                //                    + "ASec.ActivityID = A.ID "
                //                    + "inner join "
                //                    + "X2.Workflow wf "
                //                    + "on "
                //                    + "wf.id = A.Workflowid "
                //                    + "and "
                //                    + "wf.processid = @OldProcessID "
                //                    + "inner join "
                //                    + "X2.SecurityGroup SG "
                //                    + "on "
                //                    + "ASec.SecurityGroupID = SG.ID "
                //                    + "	) "
                //                    + ") ADDSEC "
                //                    + "on "
                //                    + "ADDSEC.StateID = I.StateID "
                //                    + "group by I.StateID "
                //                    + "union "
                ////-- get states that could possibly be affected by activities that have had security removed
                //                    + "select I.StateID from X2.Instance I "
                //                    + "inner join "
                //                    + "X2.Workflow WF "
                //                    + "on "
                //                    + "I.workflowid = WF.ID "
                //                    + "and "
                //                    + "WF.processid in(@NewProcessID, @OldProcessID) "
                //                    + "inner join "
                //                    + "( "
                //                    + "select A.StateID from X2.ActivitySecurity ASec "
                //                    + "inner join "
                //                    + "X2.Activity A "
                //                    + "on "
                //                    + "ASec.ActivityID = A.ID "
                //                    + "inner join "
                //                    + "X2.Workflow wf "
                //                    + "on "
                //                    + "wf.id = A.Workflowid "
                //                    + "and "
                //                    + "wf.processid = @OldProcessID "
                //                    + "inner join "
                //                    + "X2.SecurityGroup SG "
                //                    + "on "
                //                    + "ASec.SecurityGroupID = SG.ID "
                //                    + "where "
                //                    + "SG.Name not in "
                //                    + "( "
                //                    + "select SG.Name from X2.ActivitySecurity ASec "
                //                    + "inner join "
                //                    + "X2.Activity A "
                //                    + "on "
                //                    + "ASec.ActivityID = A.ID "
                //                    + "inner join "
                //                    + "X2.Workflow wf "
                //                    + "on "
                //                    + "wf.id = A.Workflowid "
                //                    + "and "
                //                    + "wf.processid = @NewProcessID "
                //                    + "inner join "
                //                    + "X2.SecurityGroup SG "
                //                    + "on "
                //                    + "ASec.SecurityGroupID = SG.ID "
                //                    + ") "
                //                    + ") ADDSEC "
                //                    + "on "
                //                    + "ADDSEC.StateID = I.StateID "
                //                    + "group by I.StateID ";
                //                Cmd.CommandText = sgStr;
                //                Cmd.Parameters.Add(new SqlParameter("@oldProcessID", SqlDbType.Int));
                //                Cmd.Parameters.Add(new SqlParameter("@newProcessID", SqlDbType.Int));

                //                Cmd.Parameters[0].Value = oldProcID;
                //                Cmd.Parameters[1].Value = newProcID;

                //                mReader = Cmd.ExecuteReader();
                //                while (mReader.Read())
                //                {
                //                    if (mReader.GetInt32(0) == LatestWorkFlowID)
                //                    {
                //                        lstStatesWithChangedSecurity.Add(mReader.GetInt32(3));
                //                    }
                //                }

                #endregion

                //                mReader.Close();
                // update the state dataset with the new mapped states
                // if a state name has changed from StateA to StateB then we
                // need to update the dataset so all references to State B(new name) are
                // StateA(old name) This will work cause the STATEID will be that of StateA.
                // Does that make sense? sortof I think... maybe
                if (null != NewMappedStates)
                {
                    foreach (DataRow dr in dsNewWorkFlow.Tables["State"].Rows)
                    {
                        // check if this State is one that has been renames
                        foreach (StateChangeItem si in NewMappedStates)
                        {
                            if (dr["Name"].ToString() == si.NewName)
                            {
                                // set to the old name so we can still lookup against it
                                // The ID is the new ID so the mappings in the Instance table
                                //will be correct.
                                dr["Name"] = si.OldName;
                                continue;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (mReader != null)
                {
                    mReader.Close();
                    mReader.Dispose();
                }
                if (Cmd != null)
                {
                    Cmd.Dispose();
                }
            }
        }

        private List<Int64> GetInstanceListForWorkFLowID(int WorkFlowIDToMigrate)
        {
            List<Int64> InstanceForWorkFlow = new List<Int64>();
            DataSet ds = new DataSet();
            // Get List of Instances for this WorkFLowID
            string strQuery = string.Format("Select ID from X2.Instance where WorkFlowID={0} order by ID desc", WorkFlowIDToMigrate);
            db.PopulateDataSet(ds, "TEMP", strQuery);
            foreach (DataRow dr in ds.Tables["TEMP"].Rows)
            {
                InstanceForWorkFlow.Add(Convert.ToInt64(dr["ID"]));
            }
            ds.Tables.Remove("TEMP");
            return InstanceForWorkFlow;
        }

        private int GetLatestWorkFlowID(string WorkFlowName)
        {
            string strQuery = string.Format("declare @WID int select @WID = (select max(ID) from X2.Workflow where Name='{0}') select @WID", WorkFlowName);
            return Convert.ToInt32(db.GetSingleValue(strQuery));
        }

        private void GetLatestStateList(DataSet dsNewWorkFlow, int LatestWorkFlowID)
        {
            string strQuery = "select * from X2.State where WorkFlowID=" + LatestWorkFlowID;
            db.PopulateDataSet(dsNewWorkFlow, "State", strQuery);
            foreach (DataRow dr in dsNewWorkFlow.Tables["State"].Rows)
            {
                StateIDMap.Add(Convert.ToInt32(dr["ID"]), dr["Name"].ToString());
            }
        }

        private void GetLatestExternalActivityList(DataSet dsNewWorkFlow, int LatestWorkFlowID)
        {
            string strQuery = "select * from X2.ExternalActivity where WorkFlowID=" + LatestWorkFlowID;
            db.PopulateDataSet(dsNewWorkFlow, "ExternalActivity", strQuery);
            foreach (DataRow dr in dsNewWorkFlow.Tables["ExternalActivity"].Rows)
            {
                ExtActivityIDMap.Add(Convert.ToInt32(dr["ID"]), dr["Name"].ToString());
            }
        }

        private void GetLatestActivityList(DataSet dsNewWorkFlow, int LatestWorkFlowID)
        {
            string strQuery = "select * from X2.Activity where WorkFlowID=" + LatestWorkFlowID;
            db.PopulateDataSet(dsNewWorkFlow, "Activity", strQuery);
            foreach (DataRow dr in dsNewWorkFlow.Tables["Activity"].Rows)
            {
                ActivityIDMap.Add(Convert.ToInt32(dr["ID"]), dr["Name"].ToString());
            }
        }

        private int GetStateIDForStateName(DataSet dsNewWorkFlow, string CurrentStateString)
        {
            CurrentStateString = CurrentStateString.Replace("'", "''");
            string where = string.Format("name='{0}'", CurrentStateString);
            DataRow[] drc = dsNewWorkFlow.Tables["State"].Select(where);
            int NewStateID = Convert.ToInt32(drc[0]["ID"]);
            return NewStateID;
        }

        private int GetLatestActivityIDForActivityName(DataSet dsNewWorkFlow, string CurrentActivityString)
        {
            CurrentActivityString = CurrentActivityString.Replace("'", "''");
            string where = string.Format("name='{0}'", CurrentActivityString);
            DataRow[] drc = dsNewWorkFlow.Tables["Activity"].Select(where);
            int NewActivityID = Convert.ToInt32(drc[0]["ID"]);
            return NewActivityID;
        }

        private int GetLatestExtActivityIDForExtActivityName(DataSet dsNewWorkFlow, string ExtActivityName)
        {
            ExtActivityName = ExtActivityName.Replace("'", "''");
            string where = string.Format("name='{0}'", ExtActivityName);
            DataRow[] drc = dsNewWorkFlow.Tables["ExternalActivity"].Select(where);
            int NewID = Convert.ToInt32(drc[0]["ID"]);
            return NewID;
        }

        private string GetActivityStringForActivityID(int ActivityID)
        {
            if (ActivityIDMap.ContainsKey(ActivityID))
                return ActivityIDMap[ActivityID];

            string ActivityName = db.GetSingleValue(string.Format("select Name from X2.Activity where ID={0}", ActivityID)).ToString();
            ActivityIDMap.Add(ActivityID, ActivityName);

            return ActivityName;
        }

        private string GetExtActivityStringForExtActivityID(int ExtActivityID)
        {
            if (ExtActivityIDMap.ContainsKey(ExtActivityID))
                return ExtActivityIDMap[ExtActivityID];

            string Name = db.GetSingleValue(string.Format("select Name from X2.ExternalActivity where ID={0}", ExtActivityID)).ToString();
            ExtActivityIDMap.Add(ExtActivityID, Name);

            return Name;
        }

        private void MigrateActiveExternalActivities(int WorkFlowIDToMigrate)
        {
            db.PopulateDataSet(dsNewWorkFlow, "TEMP", string.Format("select * from X2.ActiveExternalActivity (nolock) where WorkFlowID={0}", WorkFlowIDToMigrate));
            foreach (DataRow dr in dsNewWorkFlow.Tables["TEMP"].Rows)
            {
                int CurrentExtActivityID = Convert.ToInt32(dr["ExternalActivityID"]);
                string ExtActivityName = GetExtActivityStringForExtActivityID(CurrentExtActivityID);
                int NewExtActivityID = GetLatestExtActivityIDForExtActivityName(dsNewWorkFlow, ExtActivityName);
            }
            dsNewWorkFlow.Tables.Remove("TEMP");
        }

        protected void MigrateSingle(int WorkFlowIDToMigrate, Int64 CurrentInstanceID, SqlTransaction Transaction, SqlConnection Connection)
        {
            #region Instance

            string strQuery = "select I.*, S.Name StateName from X2.Instance I join X2.State S on I.StateID=S.ID where I.ID=" + CurrentInstanceID;
            db.PopulateDataSet(dsNewWorkFlow, "Instance", strQuery);
            // Get the workflowID for this instance
            int CurrentWorkFlowID = Convert.ToInt32(dsNewWorkFlow.Tables["Instance"].Rows[0]["WorkFlowID"]);
            string CurrentStateString = dsNewWorkFlow.Tables["Instance"].Rows[0]["StateName"].ToString();
            int NewStateID = GetStateIDForStateName(dsNewWorkFlow, CurrentStateString);

            Int32? NewReturnActivityID = null;
            if (!DBNull.Equals(DBNull.Value, dsNewWorkFlow.Tables["Instance"].Rows[0]["ReturnActivityID"]))
            {
                // Migrate the ReturnStateID
                strQuery = string.Format("select I.*, A.Name ActivityName from x2.Instance I join x2.Activity A on I.ReturnActivityID=A.ID where I.ID={0}", CurrentInstanceID);
                db.PopulateDataSet(dsNewWorkFlow, "TEMP", strQuery);
                string CurrentReturnActivityName = dsNewWorkFlow.Tables["TEMP"].Rows[0]["ActivityName"].ToString();
                NewReturnActivityID = GetLatestActivityIDForActivityName(dsNewWorkFlow, CurrentReturnActivityName);
            }
            // update the instance table for this instance with new StateID and WorkFlowID
            string strUpdate = string.Format("update X2.Instance set StateID={0},WorkFlowID={1}, ActivityDate=NULL, ActivityADUserName=NULL, ActivityID=NULL, ReturnActivityID={3} where ID={2}", NewStateID, LatestWorkFlowID, CurrentInstanceID, NewReturnActivityID);
            Updates.Add(strUpdate);

            // Migrate ActivityID to NEW ActivityID in X2.SchedulesActivity
            strQuery = string.Format("select * from X2.ScheduledActivity where InstanceID={0}", CurrentInstanceID);
            db.PopulateDataSet(dsNewWorkFlow, "TEMP", strQuery);
            foreach (DataRow dr in dsNewWorkFlow.Tables["TEMP"].Rows)
            {
                int OldActivityID = Convert.ToInt32(dr["ActivityID"]);
                string ActivityName = GetActivityStringForActivityID(OldActivityID);
                int NewActivityID = GetLatestActivityIDForActivityName(dsNewWorkFlow, ActivityName);
                Updates.Add(string.Format("update X2.ScheduledActivity set ActivityID={0} where ActivityID={1} and InstanceID={2}",
                    NewActivityID, OldActivityID, CurrentInstanceID));
            }
            dsNewWorkFlow.Tables.Remove("TEMP");
            // delete from tables

            SqlCommand cmd = null;
            cmd = Connection.CreateCommand();
            cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
            cmd.Transaction = Transaction;
            cmd.CommandText = "";

            //if (lstStatesWithChangedSecurity.Contains(WorkFlowIDToMigrate))
            //{
            Updates.Add(string.Format("delete from X2.InstanceActivitySecurity where InstanceID={0}", CurrentInstanceID));
            // }
            Updates.Add(string.Format("delete from X2.WorkList where InstanceID={0}", CurrentInstanceID));
            Updates.Add(string.Format("delete from X2.tracklist where InstanceID={0}", CurrentInstanceID));
            dsNewWorkFlow.Tables.Remove("Instance");

            #endregion
        }

        public void Migrate(int WorkFlowIDToMigrate, SqlTransaction Transaction, SqlConnection connection)
        {
            //int WorkFlowIDToMigrate  = (LatestWorkFlowID - 1);
            List<Int64> InstanceForWorkFlow = GetInstanceListForWorkFLowID(WorkFlowIDToMigrate);

            // migrate the ActiveExternalActivities for this workflow
            MigrateActiveExternalActivities(WorkFlowIDToMigrate);

            foreach (Int64 CurrentInstanceID in InstanceForWorkFlow)
            //Int64 CurrentInstanceID = InstanceForWorkFlow[0];
            {
                MigrateSingle(WorkFlowIDToMigrate, CurrentInstanceID, Transaction, connection);
            }

            // this only needs to be done once per workflow
            db.UpdateInstances(Updates);
            //return Updates;

            //db.UpdateInstance(Updates);
        }

        public static void MigrateReturnStateIDs(SqlTransaction Transaction, SqlConnection connection)
        {
            SqlCommand Cmd = null;
            try
            {
                Cmd = connection.CreateCommand();
                Cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                Cmd.Transaction = Transaction;
                Stream Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SAHL.X2Designer.Resources.MapReturnActivity.sql");
                StreamReader SR = new StreamReader(Stream);
                string Query = SR.ReadToEnd();
                Cmd.CommandText = Query;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
            finally
            {
                if (Cmd != null)
                    Cmd.Dispose();
                Cmd = null;
            }
        }

        public static void MigrateWorkFlow(SqlTransaction Transaction, SqlConnection connection, int OldWorkFlowID, int NewWorkFlowID, List<StateChangeItem> MappedStateItems, int Timeout)
        {
            SqlCommand Cmd = null;
            try
            {
                // create the mapped states table and populate it
                Cmd = connection.CreateCommand();
                Cmd.CommandTimeout = Timeout;
                Cmd.Transaction = Transaction;

                // execute the mapstates sql on the new workflow
                Stream Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SAHL.X2Designer.Resources.MapStates.sql");
                StreamReader SR = new StreamReader(Stream);
                string Query = SR.ReadToEnd();
                Cmd.Parameters.Add(new SqlParameter("@NewWorkFlowID", SqlDbType.Int));
                Cmd.Parameters.Add(new SqlParameter("@OldWorkFlowID", SqlDbType.Int));
                Cmd.Parameters["@NewWorkFlowID"].Value = NewWorkFlowID;
                Cmd.Parameters["@OldWorkFlowID"].Value = OldWorkFlowID;
                Cmd.CommandText = Query;
                Cmd.ExecuteNonQuery();

                // delete mapped states table
                //Query = "drop table #mappedstates";
                //Cmd.CommandText = Query;
                //Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                throw new Exception("MigrateWorkFlow()", ex);
            }
            finally
            {
                if (Cmd != null)
                    Cmd.Dispose();
            }
        }

        public static List<Int64> MigrateFinalise(SqlTransaction Transaction, SqlConnection connection, int OldProcessID, int NewProcessID)
        {
            List<Int64> Instances = new List<Int64>();
            SqlCommand Cmd = null;
            try
            {
                // execute the CompareSecurity sql on the new workflow
                Stream Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SAHL.X2Designer.Resources.CompareSecurity.sql");
                StreamReader SR = new StreamReader(Stream);
                DataTable DT = new DataTable();
                string Query = SR.ReadToEnd();
                Cmd = connection.CreateCommand();
                Cmd.CommandTimeout = Properties.Settings.Default.CommandTimeout;
                Cmd.Transaction = Transaction;
                Cmd.CommandText = Query;
                Cmd.Parameters.Add(new SqlParameter("@NewProcessID", SqlDbType.Int));
                Cmd.Parameters.Add(new SqlParameter("@OldProcessID", SqlDbType.Int));
                Cmd.Parameters["@NewProcessID"].Value = NewProcessID;
                Cmd.Parameters["@OldProcessID"].Value = OldProcessID;
                SqlDataAdapter SDA = new SqlDataAdapter(Cmd);
                //Cmd.ExecuteNonQuery();
                SDA.Fill(DT);

                if (DT != null)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        Int64 InstanceID = Convert.ToInt64(DT.Rows[i][0]);
                        if (!Instances.Contains(InstanceID))
                            Instances.Add(InstanceID);
                    }
                }
                return Instances;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Cmd != null)
                    Cmd.Dispose();
            }
        }

        public static bool MigrateRebuildInstances(SqlTransaction Transaction, SqlConnection connection, List<Int64> Instances)
        {
            try
            {
                List<ListRequestItem> lstRequests = new List<ListRequestItem>();
                for (int i = 0; i < Instances.Count; i++)
                {
                    ListRequestItem itm = new ListRequestItem(Instances[i], "");
                    lstRequests.Add(itm);
                }
                string val = RegistryHelper.GetObject(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Elzaris Technologies\\X2Designer", "X2EngineConnection").ToString();
                X2EngineProvider engine = new X2EngineProvider(val);
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("X2"), null);
                X2ResponseBase resp = engine.ProcessListActivity(lstRequests);
                if (!resp.IsErrorResponse)
                {
                    X2RebuildWorklistResponse lstResponses = resp as X2RebuildWorklistResponse;
                    if (lstResponses.ItemList.Count > 0)
                    {
                        frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(lstResponses.ItemList);
                        mFrmErrors.ShowDialog();
                        mFrmErrors.Dispose();
                        return false;
                    }
                }
                else
                {
                    if (lstRequests.Count > 0)
                    {
                        frmInstanceMigrationErrors mFrmErrors = new frmInstanceMigrationErrors(lstRequests);
                        mFrmErrors.Text = resp.Exception.ToString();
                        mFrmErrors.ShowDialog();
                        mFrmErrors.Dispose();
                        return false;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionPolicy.HandleException(e, "X2Designer");
                return false;
            }
        }
    }
}