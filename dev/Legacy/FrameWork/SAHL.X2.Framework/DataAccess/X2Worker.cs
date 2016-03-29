using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data.SqlClient;
using System.Text;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.DataAccess
{
    public class X2Worker
    {
        #region Public Members

        #region used by pauls debug tools

        // PC
        public static void GetX2ProcessAndWorkflowList(ITransactionContext ctx, SAHL.X2.Framework.DataSets.X2 ds)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select Max(P.ID) ID, P.Name from X2.Process P group by P.Name ");
                sb.Append("select Max(W.ID) ID, W.Name from X2.Workflow W group by W.Name");
                StringCollection TableMapping = new StringCollection();
                TableMapping.Add("Process");
                TableMapping.Add("Workflow");
                WorkerHelper.FillFromQuery(ds, TableMapping, sb.ToString(), ctx, null);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2StateAndActivityList(ITransactionContext ctx, SAHL.X2.Framework.DataSets.X2 ds, int WorkFlowID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from X2.State where WorkflowID={0} and type=1 order by Name ", WorkFlowID);
                sb.AppendFormat("select * from X2.Activity where WorkflowID={0} and type=1 order by Name", WorkFlowID);
                StringCollection TableMapping = new StringCollection();
                TableMapping.Add("State");
                TableMapping.Add("Activity");
                WorkerHelper.FillFromQuery(ds, TableMapping, sb.ToString(), ctx, null);
            }
            catch
            {
                throw;
            }
        }

        #endregion used by pauls debug tools

        // PC -
        public static string GetX2WorkFlowNameForWorkflowID(ITransactionContext ctx, int WorkflowID)
        {
            try
            {
                string query = Repository.Get("X2", "WorkflowNameForIDGet", ctx);
                ParameterCollection para = new ParameterCollection();
                WorkerHelper.AddIntParameter(para, "@WorkflowID", WorkflowID);
                object o = WorkerHelper.ExecuteScalar(ctx, query, para);
                return o.ToString();
            }
            catch
            {
                throw;
            }
        }

        // PC
        public static DataRow WorkflowAndProcessNameGet(ITransactionContext ctx, Int64 InstanceID)
        {
            try
            {
                DataSet ds = new DataSet();
                ParameterCollection param = new ParameterCollection();
                WorkerHelper.AddBigIntParameter(param, "@InstanceID", InstanceID);
                WorkerHelper.Fill(ds, "Table", "X2", "MetaDataWorkflowAndProcessNameGet", ctx, param);
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
            }
            catch
            {
                throw;
            }
            return null;
        }

        // PC
        public static void GetMetaDataForWorkflow(ITransactionContext ctx, string ProcessName, string WorkflowName, SAHL.X2.Framework.DataSets.X2 ds)
        {
            try
            {
                //ds.EnforceConstraints = false;
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddVarcharParameter(Parameters, "@ProcessName", ProcessName);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowName", WorkflowName);
                StringCollection TableMapping = new StringCollection();
                TableMapping.Add("Workflow");
                TableMapping.Add("State");
                TableMapping.Add("ActivityType");
                TableMapping.Add("Activity");
                TableMapping.Add("SecurityGroup");
                TableMapping.Add("ExternalActivity");
                TableMapping.Add("ActivitySecurity");
                TableMapping.Add("StateWorkList");
                TableMapping.Add("Workflowsecurity");
                TableMapping.Add("Process");
                TableMapping.Add("StageActivity");
                TableMapping.Add("StageDefinition");
                TableMapping.Add("StageDefinitionGroup");
                TableMapping.Add("ADUser"); // will be a prob ifprod doesnt have this
                TableMapping.Add("WorkflowActivity");

                WorkerHelper.Fill(ds, TableMapping, "X2", "MetaDataGet", ctx, Parameters);
            }
            catch
            {
                throw;
            }
        }

        // PC
        public static void GetStateActivityExternalForWorkflow(ITransactionContext ctx, int WorkFlowID, SAHL.X2.Framework.DataSets.X2 ds)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddIntParameter(Parameters, "@WorkFlowID", WorkFlowID);
                StringCollection TableMapping = new StringCollection();
                TableMapping.Add("State");
                TableMapping.Add("Activity");
                TableMapping.Add("ExternalActivity");
                TableMapping.Add("Form");
                WorkerHelper.Fill(ds, TableMapping, "X2", "GetStateActivityExternalForWorkflow", ctx, Parameters);
            }
            catch
            {
                throw;
            }
        }

        // PC
        public static void InsertExternalActivity(ITransactionContext ctx, int ExtActivityID, int WorkflowID, Int64 ActivatingInstanceID,
          DateTime ActivationTime, string ActivityXMLData, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddIntParameter(Parameters, "@ExternalActivityID", ExtActivityID);
                WorkerHelper.AddIntParameter(Parameters, "@WorkflowID", WorkflowID);
                WorkerHelper.AddBigIntParameter(Parameters, "@ActivatingInstanceID", ActivatingInstanceID);
                WorkerHelper.AddDateParameter(Parameters, "@ActivationTime", ActivationTime);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityXMLData", ActivityXMLData);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProviderName", "");
                //WorkerHelper.AddBigIntParameter(Parameters, "@SourceInstanceID", SourceInstanceID);
                //WorkerHelper.AddIntParameter(Parameters, "@ReturnActivityID", ReturnActivityID);
                string query = Repository.Get("X2", "ActiveExternalActivityInsert", ctx);
                int Results = WorkerHelper.ExecuteNonQuery(ctx, query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        //PC
        public static int GetX2ExternalActivityID(ITransactionContext ctx, int WorkflowID, string ExternalActivityName)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddIntParameter(Parameters, "@WorkflowID", WorkflowID);
                WorkerHelper.AddVarcharParameter(Parameters, "@Name", ExternalActivityName);
                string query = Repository.Get("X2", "ExternalActivityIDGet", ctx);
                object o = WorkerHelper.ExecuteScalar(ctx, query, Parameters);
                return Convert.ToInt32(o);
            }
            catch
            {
                throw;
            }
        }

        //PC
        public static int GetX2WorkFlowID(ITransactionContext ctx, string WorkFlowName)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkflowName", WorkFlowName);
                string query = Repository.Get("X2", "X2WorkflowIDGet", ctx);
                object o = WorkerHelper.ExecuteScalar(ctx, query, Parameters);
                return Convert.ToInt32(o);
            }
            catch
            {
                throw;
            }
        }

        //PC
        public static int GetX2ActivityID(ITransactionContext ctx, string ActivityName, int WorkflowID, int? StateID)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", ActivityName);
                WorkerHelper.AddIntParameter(Parameters, "@WorkflowID", WorkflowID);
                if (null == StateID)
                {
                    WorkerHelper.AddParameter(Parameters, "@StateID", SqlDbType.Int, ParameterDirection.Input, DBNull.Value);
                }
                else
                {
                    WorkerHelper.AddIntParameter(Parameters, "@StateID", (int)StateID);
                }
                string query = Repository.Get("X2", "X2ActivityIDGet", ctx);
                object o = WorkerHelper.ExecuteScalar(ctx, query, Parameters);
                return Convert.ToInt32(o);
            }
            catch
            {
                throw;
            }
        }

        //PC
        public static int GetX2StateID(ITransactionContext ctx, string StateName, int WorkflowID)
        {
            try
            {
                ParameterCollection Parameters = new ParameterCollection();
                WorkerHelper.AddVarcharParameter(Parameters, "@StateName", StateName);
                WorkerHelper.AddIntParameter(Parameters, "@WorkflowID", WorkflowID);
                string query = Repository.Get("X2", "X2StateIDGet", ctx);
                object o = WorkerHelper.ExecuteScalar(ctx, query, Parameters);
                return Convert.ToInt32(o);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateStageTransitions(ITransactionContext ctx, SAHL.X2.Framework.DataSets.X2.StageTransitionDataTable dt)
        {
            try
            {
                UpdateInformation UpdateInfo = new UpdateInformation();
                UpdateInfo.ApplicationName = "X2";

                UpdateInfo.InsertName = "StageTransitionInsert";
                UpdateInfo.InsertParameters = new ParameterCollection();
                // Insert
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@StageDefinitionStageDefinitionGroupKey", "StageDefinitionStageDefinitionGroupKey");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@GenericKey", "GenericKey");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@ADUserKey", "ADUserKey");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@Comments", "Comments");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@StartTransitionDate", "TransitionDate");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@EndTransitionDate", "EndTransitionDate");
                WorkerHelper.AddLinkedParameter(UpdateInfo.InsertParameters,
                      "@StageTransitionKey",
                      System.Data.SqlDbType.Int,
                      System.Data.ParameterDirection.Output,
                      "StageTransitionKey");
                WorkerHelper.Update(dt, ctx, UpdateInfo);
            }
            catch
            {
                throw;
            }
        }

        //public static void GetSessionList(TransactionContext ctx, X2.SessionDataTable dt)
        //{
        //  try
        //  {
        //    string query = Repository.Get("X2", "GetSessionList");
        //    WorkerHelper.Fill(dt, "X2", "GetSessionList", ctx, null);
        //  }
        //  catch (Exception ex)
        //  {
        //    bool rethrow = ExceptionPolicy.HandleException(ex, "DataAccess");
        //    if (rethrow)
        //      throw;
        //  }
        //}

        public static string GetX2Session(ITransactionContext p_Context, string ADUserName)
        {
            try
            {
                UpdateInformation UpdateInfo = new UpdateInformation();
                UpdateInfo.InsertName = "SessionInsert";
                UpdateInfo.InsertParameters = new ParameterCollection();
                UpdateInfo.ApplicationName = "X2";
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@SessionID", "SessionID");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ADUserName", "ADUserName");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@SessionStartTime", "SessionStartTime");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@LastActivityTime", "LastActivityTime");
                SAHL.X2.Framework.DataSets.X2.SessionDataTable SDT = new SAHL.X2.Framework.DataSets.X2.SessionDataTable();
                string SessionID = Guid.NewGuid().ToString();
                SDT.AddSessionRow(SessionID, ADUserName, DateTime.Now, DateTime.Now);

                WorkerHelper.Update(SDT, p_Context, UpdateInfo);
                return SessionID;
            }
            catch
            {
                throw;
            }
        }

        public static void RemoveX2ExpiredSessions(ITransactionContext p_Context, DateTime dtExpiredTime)
        {
            try
            {
                ParameterCollection parameter = new ParameterCollection();
                WorkerHelper.AddDateParameter(parameter, "@NowTime", dtExpiredTime);
                string Query = "";
                Query = Repository.Get("X2", "RemoveExpiredSessions", p_Context);
                WorkerHelper.ExecuteNonQuery(p_Context, Query, parameter);
            }
            catch
            {
                throw;
            }
        }

        public static void CloseX2Session(ITransactionContext p_Context, string p_SessionID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@SessionID", p_SessionID);
                Query = Repository.Get("X2", "SessionClose", p_Context);

                // Do it
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void CheckX2Session(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.SessionDataTable X2SessionData, string p_SessionID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@SessionID", p_SessionID);
                Query = Repository.Get("X2", "SessionCheck", p_Context);

                //Fill it.
                WorkerHelper.FillFromQuery(X2SessionData, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateX2SessionLastActivity(ITransactionContext p_Context, string p_SessionID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@SessionID", p_SessionID);
                WorkerHelper.AddDateParameter(Parameters, "@LastActivityTime", DateTime.Now);
                Query = Repository.Get("X2", "SessionLastActivityUpdate", p_Context);

                // Do it
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2InstanceData(ITransactionContext p_Context, Int64 p_InstanceID, dsX2InstanceData.X2InstanceDataDataTable p_InstanceData)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

                Query = Repository.Get("X2", "InstanceDataGet", p_Context);

                // Fill it.
                WorkerHelper.FillFromQuery(p_InstanceData, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void SaveX2InstanceData(ITransactionContext p_Context, dsX2InstanceData.X2InstanceDataDataTable p_InstanceData)
        {
            try
            {
                UpdateInformation UpdateInfo = new UpdateInformation();
                UpdateInfo.ApplicationName = "X2";

                UpdateInfo.UpdateName = "InstanceDataUpdate";
                UpdateInfo.UpdateParameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@ID", "ID");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.UpdateParameters, "@Name", "Name");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.UpdateParameters, "@Subject", "Subject");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@StateID", "StateID");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.UpdateParameters, "@StateChangeDate", "StateChangeDate");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@Priority", "Priority");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.UpdateParameters, "@DeadlineDate", "DeadlineDate");
                WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.UpdateParameters, "@SourceInstanceID", "SourceInstanceID");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@ReturnActivityID", "ReturnActivityID");

                // Update it.
                WorkerHelper.Update(p_InstanceData, p_Context, UpdateInfo);
            }
            catch
            {
                throw;
            }
        }

        public static Int64 CreateX2Instance(ITransactionContext p_Context, string p_ProcessName, string p_WorkFlowName, string p_ActivityName, string p_WorkFlowProvider, string p_ADUserName, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@ProcessName", p_ProcessName);
                //WorkerHelper.AddVarcharParameter(Parameters, "@ProcessVersion", p_ProcessVersion);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowName", p_WorkFlowName);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProvider", p_WorkFlowProvider);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", p_ActivityName);
                WorkerHelper.AddVarcharParameter(Parameters, "@CreatorADUserName", p_ADUserName);
                DateTime TimeStamp = DateTime.Now;
                WorkerHelper.AddDateParameter(Parameters, "@CreationDate", TimeStamp);
                WorkerHelper.AddDateParameter(Parameters, "@ActivityDate", TimeStamp);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityADUserName", p_ADUserName);
                WorkerHelper.AddBigIntParameter(Parameters, "@SourceInstanceID", SourceInstanceID);
                WorkerHelper.AddBigIntParameter(Parameters, "@ReturnActivityID", ReturnActivityID);
                SqlParameter Instance = WorkerHelper.AddParameter(Parameters, "@InstanceID", System.Data.SqlDbType.BigInt, System.Data.ParameterDirection.InputOutput, -1);

                Query = Repository.Get("X2", "InstanceCreate", p_Context);

                // Do it
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
                //object oInstanceID = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (Instance.Value != DBNull.Value)
                    return Convert.ToInt64(Instance.Value);
                else
                    throw new Exception("Create Instance Failed without an explanation.");
            }
            catch
            {
                throw;
            }
        }

        public static Int64 CreateX2SplitInstance(ITransactionContext p_Context, Int64 p_ParentInstanceID, string p_ActivityName, string p_WorkFlowProvider, string p_ADUserName, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, @"ParentInstanceID", p_ParentInstanceID);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProvider", p_WorkFlowProvider);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", p_ActivityName);
                WorkerHelper.AddVarcharParameter(Parameters, "@CreatorADUserName", p_ADUserName);
                DateTime TimeStamp = DateTime.Now;
                WorkerHelper.AddDateParameter(Parameters, "@CreationDate", TimeStamp);
                WorkerHelper.AddDateParameter(Parameters, "@ActivityDate", TimeStamp);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityADUserName", p_ADUserName);
                WorkerHelper.AddBigIntParameter(Parameters, "@SourceInstanceID", SourceInstanceID);
                WorkerHelper.AddIntParameter(Parameters, "@ReturnActivityID", ReturnActivityID);

                Query = Repository.Get("X2", "InstanceSplitCreate", p_Context);

                // Do it
                object oInstance = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (oInstance != DBNull.Value && oInstance != null)
                    return Convert.ToInt64(oInstance);
                else
                    throw new Exception("Create Split Instance Failed without an explanation.");
            }
            catch
            {
                throw;
            }
        }

        public static Int64 CreateX2InstanceData(ITransactionContext p_Context, string p_ProcessName, string p_WorkFlowName, int p_ParentInstanceID, string p_ActivityName, string p_WorkFlowProvider, string p_ADUserName, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();
                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@ProcessName", p_ProcessName);
                //WorkerHelper.AddVarcharParameter(Parameters, "@ProcessVersion", p_ProcessVersion);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowName", p_WorkFlowName);
                WorkerHelper.AddIntParameter(Parameters, @"ParentInstanceID", p_ParentInstanceID);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProvider", p_WorkFlowProvider);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", p_ActivityName);
                WorkerHelper.AddVarcharParameter(Parameters, "@CreatorADUserName", p_ADUserName);
                DateTime TimeStamp = DateTime.Now;
                WorkerHelper.AddDateParameter(Parameters, "@CreationDate", TimeStamp);
                WorkerHelper.AddDateParameter(Parameters, "@ActivityDate", TimeStamp);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityADUserName", p_ADUserName);
                WorkerHelper.AddBigIntParameter(Parameters, "@SourceInstanceID", SourceInstanceID);
                WorkerHelper.AddIntParameter(Parameters, "@ReturnActivityID", ReturnActivityID);
                SqlParameter Instance = WorkerHelper.AddParameter(Parameters, "@InstanceID", System.Data.SqlDbType.BigInt, System.Data.ParameterDirection.InputOutput, -1);

                Query = Repository.Get("X2", "InstanceCreate", p_Context);

                // Do it
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
                //object oInstanceID = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (Instance.Value != DBNull.Value)
                    return Convert.ToInt64(Instance.Value);
                else
                    throw new Exception("Create Instance Failed without an explanation.");
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2CreationActivityData(ITransactionContext p_Context, dsX2ActivityInfo p_X2DS, string p_ProcessName, string p_WorkFlowName, string p_ActivityName/*, Metrics p_Mi*/)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddVarcharParameter(Parameters, "@ProcessName", p_ProcessName);
                //WorkerHelper.AddVarcharParameter(Parameters, "@ProcessVersion", p_ProcessVersion);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowName", p_WorkFlowName);
                WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", p_ActivityName);

                Query = Repository.Get("X2", "ActivityCreateInfoGet", p_Context);
                StringCollection TableMappings = new StringCollection();
                TableMappings.Add("Process");
                TableMappings.Add("WorkFlow");
                TableMappings.Add("Activity");
                TableMappings.Add("ActivitySecurity");
                TableMappings.Add("Form");
                // Fill it.
                WorkerHelper.FillFromQuery(p_X2DS, TableMappings, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        //public static void _GetX2ActivityData(TransactionContext p_Context, dsX2ActivityInfo p_X2DS, Int64 p_Instance, string p_ActivityName)
        //{
        //    try
        //    {
        //        string Query = "";

        //        // Create a collection
        //        ParameterCollection Parameters = new ParameterCollection();

        //        // Add the required parameters
        //        WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_Instance);
        //        WorkerHelper.AddVarcharParameter(Parameters, "@ActivityName", p_ActivityName);
        //        if (null == p_Context)
        //          Query = Repository.GetWithContext(p_Context, "X2", "ActivityInfoGet");
        //        else
        //          Query = Repository.Get("X2", "ActivityInfoGet");
        //        StringCollection TableMappings = new StringCollection();
        //        TableMappings.Add("Process");
        //        TableMappings.Add("WorkFlow");
        //        TableMappings.Add("Activity");
        //        TableMappings.Add("ActivitySecurity");
        //        TableMappings.Add("Form");
        //        TableMappings.Add("WorkFlowActivity");
        //        // Fill it.
        //        WorkerHelper.FillFromQuery(p_X2DS, TableMappings, Query, p_Context, Parameters);

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, "DataAccess");
        //        if (rethrow)
        //            throw;
        //    }
        //}

        public static void GetX2InstanceActivityData(ITransactionContext p_Context, dsX2ActivityInfo p_X2DS, Int64 p_Instance)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_Instance);

                Query = Repository.Get("X2", "InstanceActivitiesGet", p_Context);
                StringCollection TableMappings = new StringCollection();
                TableMappings.Add("Activity");
                TableMappings.Add("ActivitySecurity");

                // Fill it.
                WorkerHelper.FillFromQuery(p_X2DS, TableMappings, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2SystemActivityData(ITransactionContext p_Context, dsX2StateActivities p_X2DS, int p_StateID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@StateID", p_StateID);

                Query = Repository.Get("X2", "StateSystemActivitiesGet", p_Context);
                StringCollection TableMappings = new StringCollection();
                TableMappings.Add("ActivityType");
                TableMappings.Add("Activity");
                TableMappings.Add("State");
                TableMappings.Add("WorkFlowActivity");
                // Fill it.
                WorkerHelper.FillFromQuery(p_X2DS, TableMappings, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2ProcessAssemblyData(ITransactionContext p_Context, dsProcessAssembly.ProcessAssemblyDataTable p_X2ProcessAssemblies, int p_ProcessID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@ProcessID", p_ProcessID);

                Query = Repository.Get("X2", "ProcessAssemblyGet", p_Context);
                // Fill it.
                WorkerHelper.FillFromQuery(p_X2ProcessAssemblies, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static bool LockX2Instance(ITransactionContext p_Context, Int64 p_InstanceID, int p_InstanceLockTimeOut, int p_ActivityID, ref string p_ADUserName, ref string p_ActivityName)
        {
            try
            {
                // int NumRows = 0;
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddIntParameter(Parameters, "@ActivityID", p_ActivityID);
                SqlParameter ActivityUserParam = new SqlParameter("@ActivityADUserName", SqlDbType.VarChar);
                ActivityUserParam.Value = p_ADUserName;
                ActivityUserParam.Direction = ParameterDirection.InputOutput;
                Parameters.Add(ActivityUserParam);

                WorkerHelper.AddIntParameter(Parameters, "@TimeOut", p_InstanceLockTimeOut);
                SqlParameter ActivityNameParam = new SqlParameter("@ActivityName", SqlDbType.VarChar);
                ActivityNameParam.Value = p_ActivityName;
                ActivityNameParam.Direction = ParameterDirection.Output;
                Parameters.Add(ActivityNameParam);

                SqlParameter NumRowsParam = new SqlParameter("@NumRowsAffected", SqlDbType.Int);
                NumRowsParam.Direction = ParameterDirection.Output;
                Parameters.Add(NumRowsParam);

                Query = Repository.Get("X2", "LockInstance", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
                if (ActivityUserParam.Value != DBNull.Value || ActivityUserParam.Value != null)
                    p_ADUserName = ActivityUserParam.Value.ToString();
                if (NumRowsParam.Value != DBNull.Value && Convert.ToInt32(NumRowsParam.Value) == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public static bool UnLockX2Instance(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            try
            {
                // int NumRows = 0;
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

                SqlParameter NumRowsParam = new SqlParameter("@NumRowsAffected", SqlDbType.Int);
                NumRowsParam.Direction = ParameterDirection.Output;
                Parameters.Add(NumRowsParam);

                Query = Repository.Get("X2", "UnLockInstance", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
                if (NumRowsParam.Value != DBNull.Value && Convert.ToInt32(NumRowsParam.Value) == 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public static bool CheckX2LockedInstance(ITransactionContext p_Context, Int64 p_InstanceID, int p_InstanceLockTimeOut, ref string p_ADUserName, ref string p_ActivityName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                SqlParameter ActivityUserParam = new SqlParameter("@ActivityADUserName", SqlDbType.VarChar);
                ActivityUserParam.Value = p_ADUserName;
                ActivityUserParam.Direction = ParameterDirection.InputOutput;
                Parameters.Add(ActivityUserParam);

                WorkerHelper.AddIntParameter(Parameters, "@TimeOut", p_InstanceLockTimeOut);
                SqlParameter ActivityNameParam = new SqlParameter("@ActivityName", SqlDbType.VarChar);
                ActivityNameParam.Value = p_ActivityName;
                ActivityNameParam.Direction = ParameterDirection.InputOutput;
                Parameters.Add(ActivityNameParam);

                SqlParameter NumRowsParam = new SqlParameter("@NumRowsAffected", SqlDbType.Int);
                NumRowsParam.Direction = ParameterDirection.Output;
                Parameters.Add(NumRowsParam);

                Query = Repository.Get("X2", "CheckLockedInstance", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);

                if (NumRowsParam.Value != DBNull.Value && Convert.ToInt32(NumRowsParam.Value) == 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public static void SaveX2WorkFlowHistory(ITransactionContext p_Context, Int64 p_InstanceID, ref int p_WorkflowHistoryKey)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                SqlParameter WFHistoryKey = new SqlParameter("@WorkflowHistoryKey", SqlDbType.Int);
                WFHistoryKey.Direction = ParameterDirection.Output;
                Parameters.Add(WFHistoryKey);
                Query = Repository.Get("X2", "WorkFlowHistoryInsert", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);

                if (WFHistoryKey.Value != DBNull.Value && Convert.ToInt32(WFHistoryKey.Value) != 0)
                    p_WorkflowHistoryKey = Convert.ToInt32(WFHistoryKey.Value);
                else
                {
                    p_WorkflowHistoryKey = (int)0;
                }
            }
            catch
            {
                throw;
            }
        }

        //public static void GetX2StateLists(TransactionContext p_Context, dsX2StateLists p_X2StateDS, int p_StateID)
        //{
        //    try
        //    {
        //        string Query = "";

        //        // Create a collection
        //        ParameterCollection Parameters = new ParameterCollection();

        //        // Add the required parameters
        //        WorkerHelper.AddIntParameter(Parameters, "@StateID", p_StateID);

        //        Query = Repository.Get("X2", "StateListGet");
        //        StringCollection TableMappings = new StringCollection();
        //        TableMappings.Add("WorkListSecurityGroup");
        //        TableMappings.Add("TrackListSecurityGroup");

        //        // Fill it.
        //        WorkerHelper.FillFromQuery(p_X2StateDS, TableMappings, Query, p_Context, Parameters);

        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, "DataAccess");
        //        if (rethrow)
        //            throw;
        //    }
        //}

        public static void UpdateX2WorkListLog(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.WorkListLogDataTable p_X2WorkListLog)
        {
            try
            {
                UpdateInformation UpdateInfo = new UpdateInformation(null, "WorkListLogInsert", null, "X2", true);

                ParameterCollection ParamCol = new ParameterCollection();
                WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@InstanceID", "InstanceID");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ADUserName", "ADUserName");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@ListDate", "ListDate");
                WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@WorkflowHistoryId", "WorkflowHistoryId");

                WorkerHelper.Update(p_X2WorkListLog, p_Context, UpdateInfo);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateX2WorkList(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.WorkListDataTable p_X2WorkList)
        {
            try
            {
                UpdateInformation UpdateInfo = new UpdateInformation(null, "WorkListInsert", null, "X2", true);

                ParameterCollection ParamCol = new ParameterCollection();
                WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@InstanceID", "InstanceID");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ADUserName", "ADUserName");
                WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@ListDate", "ListDate");
                WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@Message", "Message");

                WorkerHelper.Update(p_X2WorkList, p_Context, UpdateInfo);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2WorkList(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

                Query = Repository.Get("X2", "WorkListDelete", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        //TODO: remove these methods
        //public static void UpdateX2TrackList(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.TrackListDataTable p_X2TrackList)
        //{
        //    try
        //    {
        //        UpdateInformation UpdateInfo = new UpdateInformation(null, "TrackListInsert", null, "X2", true);

        //        ParameterCollection ParamCol = new ParameterCollection();
        //        WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@InstanceID", "InstanceID");
        //        WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ADUserName", "ADUserName");
        //        WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@ListDate", "ListDate");

        //        WorkerHelper.Update(p_X2TrackList, p_Context, UpdateInfo);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public static void DeleteX2TrackList(ITransactionContext p_Context, Int64 p_InstanceID)
        //{
        //    try
        //    {
        //        string Query = "";

        //        // Create a collection
        //        ParameterCollection Parameters = new ParameterCollection();

        //        // Add the required parameters
        //        WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

        //        Query = Repository.Get("X2", "TrackListDelete", p_Context);

        //        // Fill it.
        //        WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static void InsertX2InstanceActivitySecurity(ITransactionContext p_Context, Int64 p_InstanceID, int p_ActivityID, string p_ADUser)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddIntParameter(Parameters, "@ActivityID", p_ActivityID);
                WorkerHelper.AddVarcharParameter(Parameters, "@ADUserName", p_ADUser);

                Query = Repository.Get("X2", "InstanceActivitySecurityInsert", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2InstanceActivitySecurity(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

                Query = Repository.Get("X2", "InstanceActivitySecurityDelete", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void InsertX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID, DateTime p_Time, int p_ActivityID, int p_Priority, string p_WorkFlowProvider)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddDateParameter(Parameters, "@Time", p_Time);
                WorkerHelper.AddIntParameter(Parameters, "@ActivityID", p_ActivityID);
                WorkerHelper.AddIntParameter(Parameters, "@Priority", p_Priority);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProviderName", p_WorkFlowProvider);

                Query = Repository.Get("X2", "ScheduledActivityInsert", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);

                Query = Repository.Get("X2", "ScheduledActivityDelete", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2ScheduledActivitySplit(ITransactionContext p_Context, Int64 p_InstanceID, int ActivityID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddIntParameter(Parameters, "@ActivityID", ActivityID);

                Query = Repository.Get("X2", "ScheduledActivitySplitDelete", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2SoonestScheduledActivity(ITransactionContext p_Context, dsX2ScheduledActivities.ScheduledActivityDataTable p_X2ScheduledActivities, string WorkFlowProviderName)
        {
            try
            {
                string Query = "";
                Query = Repository.Get("X2", "ScheduledActivityGetSoonest", p_Context);
                ParameterCollection param = new ParameterCollection();
                WorkerHelper.AddVarcharParameter(param, "@WorkFlowProviderName", WorkFlowProviderName);
                WorkerHelper.FillFromQuery(p_X2ScheduledActivities, Query, p_Context, param);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2ScheduledActivity(ITransactionContext p_Context, dsX2ScheduledActivities.ScheduledActivityDataTable p_X2ScheduledActivities, DateTime p_ScheduledTime)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddDateParameter(Parameters, "@Time", p_ScheduledTime);

                Query = Repository.Get("X2", "ScheduledActivityGet", p_Context);

                // Fill it.
                WorkerHelper.FillFromQuery(p_X2ScheduledActivities, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static DateTime GetX2ScheduledActivityNextTime(ITransactionContext p_Context, DateTime p_ScheduledTime, string p_WorkFlowProviderName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddDateParameter(Parameters, "@Time", p_ScheduledTime);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProviderName", p_WorkFlowProviderName);

                Query = Repository.Get("X2", "ScheduledActivityNextTimeGet", p_Context);

                // Fill it.
                object o = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (o != null)
                {
                    return Convert.ToDateTime(o);
                }
                else
                    return DateTime.MinValue;
            }
            catch
            {
                throw;
            }
        }

        public static int UpdateX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID, string p_WorkFlowProviderName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProviderName", p_WorkFlowProviderName);

                Query = Repository.Get("X2", "ScheduledActivityUpdate", p_Context);

                // Fill it.
                return WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void CancelX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID, bool p_WasCreate)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddBitParameter(Parameters, "@WasCreate", p_WasCreate);

                Query = Repository.Get("X2", "ActivityCancel", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static int GetX2StateIDByNameForInstance(ITransactionContext p_Context, Int64 p_InstanceID, string p_StateName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                WorkerHelper.AddVarcharParameter(Parameters, "@StateName", p_StateName);

                Query = Repository.Get("X2", "StateIDForStateName", p_Context);

                // Fill it.
                object o = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                else
                    return -1;
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2ExternalActivity(ITransactionContext p_Context, dsX2ExternalActivities.ActiveExternalActivityDataTable p_X2ExternalActivities)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                Query = Repository.Get("X2", "ExternalActivityGet", p_Context);

                // Fill it.
                WorkerHelper.FillFromQuery(p_X2ExternalActivities, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2ExternalActivity(ITransactionContext p_Context, int p_ID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@ID", p_ID);

                Query = Repository.Get("X2", "ExternalActivityDelete", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static int UpdateX2ExternalActivity(ITransactionContext p_Context, int p_ID, string p_WorkFlowProviderName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@ID", p_ID);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowProviderName", p_WorkFlowProviderName);

                Query = Repository.Get("X2", "ExternalActivityUpdate", p_Context);

                // Fill it.
                return WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2ActivatedExternalActivities(ITransactionContext p_Context, dsX2ActivatedExternalActivities p_X2DS, int p_ActivatedByExternalActivityID, Int64 ActivatingInstanceID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@ActivatedByExternalActivityID", p_ActivatedByExternalActivityID);
                WorkerHelper.AddBigIntParameter(Parameters, "@IID", ActivatingInstanceID);

                Query = Repository.Get("X2", "ActivatedExternalActivitiesGet", p_Context);
                StringCollection TableMappings = new StringCollection();
                TableMappings.Add("ExternalActivityTarget");
                TableMappings.Add("Activity");
                // Fill it.
                WorkerHelper.FillFromQuery(p_X2DS, TableMappings, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2ActivatedExternalActivity(ITransactionContext p_Context, int p_ActivatedExternalActivityID)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@ID", p_ActivatedExternalActivityID);

                Query = Repository.Get("X2", "ActivatedExternalActivityDelete", p_Context);
                // execute it
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2CreateInfo(ITransactionContext p_Context, int p_WorkFlowID, dsX2CreateInfo.CreateInfoDataTable p_X2CreateInfo)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@WorkFlowID", p_WorkFlowID);

                Query = Repository.Get("X2", "CreateInfoGet", p_Context);
                // Fill it.
                WorkerHelper.FillFromQuery(p_X2CreateInfo, Query, p_Context, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetX2ProcessInfoByInstance(ITransactionContext p_Context, Int64 p_InstanceID, ref int p_ProcessID, ref string p_ProcessName, ref string p_WorkFlowName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", p_InstanceID);
                SqlParameter P1 = WorkerHelper.AddParameter(Parameters, "@ProcessID", SqlDbType.Int, ParameterDirection.Output, p_ProcessID);
                SqlParameter P2 = WorkerHelper.AddParameter(Parameters, "@ProcessName", SqlDbType.VarChar, ParameterDirection.Output, p_ProcessName);
                P2.Size = 50;
                SqlParameter P3 = WorkerHelper.AddParameter(Parameters, "@WorkFlowName", SqlDbType.VarChar, ParameterDirection.Output, p_WorkFlowName);
                P3.Size = 50;

                Query = Repository.Get("X2", "ProcessInfoByInstanceGet", p_Context);
                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);

                p_ProcessID = Convert.ToInt32(P1.Value);
                p_ProcessName = Convert.ToString(P2.Value);
                p_WorkFlowName = Convert.ToString(P3.Value);
            }
            catch
            {
                throw;
            }
        }

        public static void UpdateX2ActiveExternalActivity(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.ActiveExternalActivityDataTable DT)
        {
            UpdateInformation UpdateInfo = new UpdateInformation("UpdateX2ActiveExternalActivity", "InsertX2ActiveExternalActivity", "DeleteX2ActiveExternalActivity", "COMMON", true);

            // the update parameters.
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@ID", "ID");
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@ExternalActivityID", "ExternalActivityID");
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@WorkFlowID", "WorkFlowID");
            WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.UpdateParameters, "@ActivatingInstanceID", "ActivatingInstanceID");
            WorkerHelper.AddLinkedDateParameter(UpdateInfo.UpdateParameters, "@ActivationTime", "ActivationTime");
            WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.UpdateParameters, "@ActivityXMLData", "ActivityXMLData");
            WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.UpdateParameters, "@WorkFlowProviderName", "WorkFlowProviderName");
            //WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.UpdateParameters, "@SourceInstanceID", "SourceInstanceID");
            // WorkerHelper.AddLinkedIntParameter(UpdateInfo.UpdateParameters, "@ReturnActivityID", "ReturnActivityID");

            // the insert parameters.
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@ExternalActivityID", "ExternalActivityID");
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.InsertParameters, "@WorkFlowID", "WorkFlowID");
            WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@ActivatingInstanceID", "ActivatingInstanceID");
            WorkerHelper.AddLinkedDateParameter(UpdateInfo.InsertParameters, "@ActivationTime", "ActivationTime");
            WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@ActivityXMLData", "ActivityXMLData");
            WorkerHelper.AddLinkedVarcharParameter(UpdateInfo.InsertParameters, "@WorkFlowProviderName", "WorkFlowProviderName");
            //WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@SourceInstanceID", "SourceInstanceID");
            //WorkerHelper.AddLinkedBigIntParameter(UpdateInfo.InsertParameters, "@ReturnActivityID", "ReturnActivityID");
            //output the key
            WorkerHelper.AddLinkedParameter(UpdateInfo.InsertParameters, "@ID", System.Data.SqlDbType.Int, System.Data.ParameterDirection.Output, "ID");

            // Delete Parameters
            WorkerHelper.AddLinkedIntParameter(UpdateInfo.DeleteParameters, "@ID", "ID");

            WorkerHelper.Update(DT, p_Context, UpdateInfo);
        }

        //public static int GetLatestProcessVersion(ITransactionContext p_Context, string p_ProcessName)
        //{
        //    try
        //    {
        //        string Query = "";

        //        // Create a collection
        //        ParameterCollection Parameters = new ParameterCollection();

        //        // Add the required parameters
        //        WorkerHelper.AddVarcharParameter(Parameters, "@ProcessName", p_ProcessName);

        //        Query = Repository.Get("X2", "GetLatestProcessVersion");

        //        // Fill it.
        //        object o = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
        //        if (o != null)
        //        {
        //            return Convert.ToInt32(o);
        //        }
        //        else
        //            return -1;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static int GetNextWorkflowID(ITransactionContext p_Context, int p_ProcessID, string p_WorkFlowName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@ProcessID", p_ProcessID);
                WorkerHelper.AddVarcharParameter(Parameters, "@WorkFlowName", p_WorkFlowName);

                Query = Repository.Get("X2", "GetNextWorkflowID", p_Context);

                // Fill it.
                object o = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                else
                    return -1;
            }
            catch
            {
                throw;
            }
        }

        public static int GetExternalActivityID(ITransactionContext p_Context, int p_WorkFlowID, string p_ExternalActivitySourceName)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddIntParameter(Parameters, "@WorkFlowID", p_WorkFlowID);
                WorkerHelper.AddVarcharParameter(Parameters, "@ExternalActivitySourceName", p_ExternalActivitySourceName);

                Query = Repository.Get("X2", "GetExternalActivityID", p_Context);

                // Fill it.
                object o = WorkerHelper.ExecuteScalar(p_Context, Query, Parameters);
                if (o != null)
                {
                    return Convert.ToInt32(o);
                }
                else
                    return -1;
            }
            catch
            {
                throw;
            }
        }

        public static void InsertError(ITransactionContext p_Context, Int64 InstanceID, string ErrorMessage, string StackTrace)
        {
            try
            {
                string Query = "";

                // Create a collection
                ParameterCollection Parameters = new ParameterCollection();

                // Add the required parameters
                WorkerHelper.AddBigIntParameter(Parameters, "@InstanceID", InstanceID);
                WorkerHelper.AddVarcharParameter(Parameters, "@Message", ErrorMessage);
                WorkerHelper.AddVarcharParameter(Parameters, "@StackTrace", StackTrace);

                Query = Repository.Get("X2", "ErrorInstanceInsert", p_Context);

                // Fill it.
                WorkerHelper.ExecuteNonQuery(p_Context, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteX2Instance(ITransactionContext ctx, Int64 InstanceID)
        {
            try
            {
                string Query = "";
                Query = Repository.Get("X2", "DeleteX2Instance", ctx);
                Query = Query.Replace("@InstanceID", InstanceID.ToString());
                ParameterCollection Parameters = new ParameterCollection();

                // Fill it.
                WorkerHelper.ExecuteNonQuery(ctx, Query, Parameters);
            }
            catch
            {
                throw;
            }
        }

        public static void GetChildInstances(ITransactionContext ctx, Int64 InstanceID, SAHL.X2.Framework.DataSets.X2.InstanceDataTable dtParentInstance)
        {
            try
            {
                ParameterCollection param = new ParameterCollection();
                WorkerHelper.AddBigIntParameter(param, "@InstanceID", InstanceID);
                WorkerHelper.Fill(dtParentInstance, "X2", "ChildInstanceGet", ctx, param);
            }
            catch
            {
                throw;
            }
        }

        public static Dictionary<int, string> GetProcessesToPreload(ITransactionContext ctx)
        {
            try
            {
                Dictionary<int, string> processesToPreload = new Dictionary<int, string>();
                DataTable availableProcesses = new DataTable();
                WorkerHelper.FillFromQuery(availableProcesses, "select max(id) as Id, Name from X2.Process where id > 100 and Name not in ('RCS', 'Delete Debit Order')  group by Name order by Name", ctx, null);

                foreach (DataRow row in availableProcesses.Rows)
                {
                    processesToPreload.Add(Convert.ToInt32(row[0]), Convert.ToString(row[1]));
                }

                return processesToPreload;
            }
            catch
            {
                throw;
            }
        }

        #endregion Public Members
    }
}