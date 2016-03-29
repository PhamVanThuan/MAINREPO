using System;
using System.Collections.Generic;
using System.Data;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.ServiceFacade
{
    public enum SessionStateType
    {
        Valid,
        InValid,
        Expired
    }

    public class X2_SFE : ServiceFacadeBase
    {
        public void RemoveX2ExpiredSessions(ITransactionContext ctx, DateTime dtExpiredTime)
        {
            base.AuthenticateUser();

            try
            {
                X2Worker.RemoveX2ExpiredSessions(ctx, dtExpiredTime);
            }
            catch
            {
                throw;
            }
        }

        //public void GetSessionList(TransactionContext ctx, X2.SessionDataTable dt)
        //{
        //  base.AuthenticateUser();
        //  TransactionContext m_context = null;

        //  try
        //  {
        //    m_context = TransactionController.GetContext("X2");TransactionController.BeginTransactions(context);//"X2", "AuditConnectionString");//);//, p_Mi);
        //    X2Worker.GetSessionList(ctx, dt);
        //  }
        //  catch (Exception e)
        //  {
        //    if (m_context != null)
        //      TransactionController.RollBack(m_context);
        //    bool rethrow = ExceptionPolicy.HandleException(e, "Service Facade");
        //    if (rethrow)
        //      throw;
        //  }
        //}

        public string GetX2Session(string p_ADUserName)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = null;

            try
            {
                m_context = TransactionController.GetContext("X2");//"X2", "AuditConnectionString");//);//, p_Mi);
                string SessionID = X2Worker.GetX2Session(m_context, p_ADUserName);
                TransactionController.Commit(m_context);
                return SessionID;
            }
            catch
            {
                if (m_context != null)
                    TransactionController.RollBack(m_context);

                throw;
            }
            finally
            {
                ////base.//StoreMetric("GetX2Session", m_metricInfo);//);//, p_Mi);
                if (m_context != null)
                    m_context.Dispose();
            }
        }

        public void CloseX2Session(string p_SessionID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = null;

            try
            {
                m_context = TransactionController.GetContext("X2");//, "AuditConnectionString");//);//, p_Mi);
                X2Worker.CloseX2Session(m_context, p_SessionID);
                TransactionController.Commit(m_context);
            }
            catch
            {
                if (m_context != null)
                    TransactionController.RollBack(m_context);

                throw;
            }
            finally
            {
                ////base.//StoreMetric("CloseX2Session", m_metricInfo);//);//, p_Mi);
                if (m_context != null)
                    m_context.Dispose();
            }
        }

        public SessionStateType CheckX2Session(ITransactionContext p_Context, string p_SessionID, int p_Timeout)
        {
            base.AuthenticateUser();

            try
            {
                SAHL.X2.Framework.DataSets.X2.SessionDataTable SessionData = new SAHL.X2.Framework.DataSets.X2.SessionDataTable();
                X2Worker.CheckX2Session(p_Context, SessionData, p_SessionID);

                if (SessionData.Count == 0)
                    return SessionStateType.InValid;
                else
                    if (DateTime.Now.Subtract(SessionData[0].LastActivityTime).Minutes > p_Timeout)
                        return SessionStateType.Expired;
                    else
                        return SessionStateType.Valid;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateX2SessionLastActivity(ITransactionContext p_Context, string p_SessionID)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;
            try
            {
                X2Worker.UpdateX2SessionLastActivity(m_context, p_SessionID);
            }
            catch
            {
                throw;
            }
        }

        public Int64 CreateX2Instance(ITransactionContext p_Context, string p_ProcessName, string p_WorkFlowName, string p_ActivityName, string p_WorkFlowProvider, string p_ADUserName, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.CreateX2Instance(m_context, p_ProcessName, p_WorkFlowName, p_ActivityName, p_WorkFlowProvider, p_ADUserName, SourceInstanceID, ReturnActivityID);
            }
            catch
            {
                throw;
            }
        }

        public Int64 CreateX2SplitInstance(ITransactionContext p_Context, Int64 p_ParentInstanceID, string p_ActivityName, string p_WorkFlowProvider, string p_ADUserName, Int64? SourceInstanceID, Int32? ReturnActivityID)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.CreateX2SplitInstance(m_context, p_ParentInstanceID, p_ActivityName, p_WorkFlowProvider, p_ADUserName, SourceInstanceID, ReturnActivityID);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2CreationActivityData(ITransactionContext p_Context, dsX2ActivityInfo p_X2DS, string p_ProcessName, string p_WorkFlowName, string p_ActivityName)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2CreationActivityData(m_context, p_X2DS, p_ProcessName, p_WorkFlowName, p_ActivityName);//);//, p_Mi);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("GetX2CreationActivityData", m_metricInfo);//);//, p_Mi);
            }
        }

        //public void _GetX2ActivityData(TransactionContext p_Context, dsX2ActivityInfo p_X2DS, Int64 p_Instance, string p_ActivityName
        //{
        //    base.AuthenticateUser();
        //    string m_metricInfo = "Succeeded";
        //    TransactionContext m_context = p_Context;

        //    try
        //    {
        //        X2Worker._GetX2ActivityData(m_context, p_X2DS, p_Instance, p_ActivityName);
        //    }
        //    catch (Exception e)
        //    {
        //        m_metricInfo = "Failed";
        //        bool rethrow = ExceptionPolicy.HandleException(e, "Service Facade");
        //        if (rethrow)
        //            throw;
        //    }
        //    finally
        //    {
        //        base.StoreMetricWithContext(p_Context, "GetX2ActivityData", m_metricInfo);//);//, p_Mi);
        //    }
        //}

        public void GetX2InstanceActivityData(ITransactionContext p_Context, dsX2ActivityInfo p_X2DS, Int64 p_Instance)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2InstanceActivityData(m_context, p_X2DS, p_Instance);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("GetX2ActivityData", m_metricInfo);//);//, p_Mi);
            }
        }

        public void GetX2SystemActivityData(ITransactionContext p_Context, dsX2StateActivities p_X2DS, int p_StateID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2SystemActivityData(m_context, p_X2DS, p_StateID);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("GetX2SystemActivityData", m_metricInfo);//);//, p_Mi);
            }
        }

        public void GetX2ProcessAssemblyData(ITransactionContext p_Context, dsProcessAssembly.ProcessAssemblyDataTable p_X2ProcessAssemblies, int p_ProcessID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2ProcessAssemblyData(m_context, p_X2ProcessAssemblies, p_ProcessID);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("GetX2ProcessAssemblyData", m_metricInfo);//);//, p_Mi);
            }
        }

        public bool LockX2Instance(ITransactionContext p_Context, Int64 p_InstanceID, int p_InstanceLockTimeOut, int p_ActivityID, ref string p_ADUserName, ref string p_ActivityName)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.LockX2Instance(m_context, p_InstanceID, p_InstanceLockTimeOut, p_ActivityID, ref p_ADUserName, ref p_ActivityName);
            }
            catch
            {
                throw;
            }
        }

        public bool UnlockX2Instance(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.UnLockX2Instance(m_context, p_InstanceID);
            }
            catch
            {
                throw;
            }
        }

        public bool CheckX2LockedInstance(ITransactionContext p_Context, Int64 p_InstanceID, int p_InstanceLockTimeOut, ref string p_ADUserName, ref string p_ActivityName)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.CheckX2LockedInstance(m_context, p_InstanceID, p_InstanceLockTimeOut, ref p_ADUserName, ref p_ActivityName);
            }
            catch
            {
                throw;
            }
        }

        public void SaveX2WorkFlowHistory(ITransactionContext p_Context, Int64 p_InstanceID, ref int p_WorkFlowHistoryKey)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.SaveX2WorkFlowHistory(m_context, p_InstanceID, ref p_WorkFlowHistoryKey);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("SaveX2WorkFlowHistory", m_metricInfo);//);//, p_Mi);
            }
        }

        //public void GetX2StateLists(TransactionContext p_Context, dsX2StateLists p_X2StateDS, int p_StateID
        //{
        //    base.AuthenticateUser();
        //    string m_metricInfo = "Succeeded";
        //    TransactionContext m_context = p_Context;

        //    try
        //    {
        //        X2Worker.GetX2StateLists(m_context, p_X2StateDS, p_StateID);
        //    }
        //    catch (Exception e)
        //    {
        //        m_metricInfo = "Failed";
        //        bool rethrow = ExceptionPolicy.HandleException(e, "Service Facade");
        //        if (rethrow)
        //            throw;
        //    }
        //    finally
        //    {
        //        //base.//StoreMetric("GetX2StateLists", m_metricInfo);//);//, p_Mi);
        //    }
        //}

        public void UpdateX2StateLists(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2 p_X2StateDS, Int64 p_InstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                // delete from the worklist and track list
                X2Worker.DeleteX2WorkList(m_context, p_InstanceID);
                // X2Worker.DeleteX2TrackList(m_context, p_InstanceID);
                // update the worklist first
                X2Worker.UpdateX2WorkList(m_context, p_X2StateDS.WorkList);

                // update the worklistlog
                X2Worker.UpdateX2WorkListLog(m_context, p_X2StateDS.WorkListLog);
                // then the tracklist
                //X2Worker.UpdateX2TrackList(m_context, p_X2StateDS.TrackList);
            }
            catch
            {
                throw;
            }
        }

        public void InsertX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID, DateTime p_Time, int p_ActivityID, int p_Priority, string p_WorkFlowProvider)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.InsertX2ScheduledActivity(m_context, p_InstanceID, p_Time, p_ActivityID, p_Priority, p_WorkFlowProvider);
            }
            catch
            {
                throw;
            }
        }

        public void InsertX2InstanceActivitySecurity(ITransactionContext p_Context, Int64 p_InstanceID, int p_ActivityID, string p_ADUser)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.InsertX2InstanceActivitySecurity(m_context, p_InstanceID, p_ActivityID, p_ADUser);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteX2InstanceActivitySecurity(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.DeleteX2InstanceActivitySecurity(m_context, p_InstanceID);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.DeleteX2ScheduledActivity(m_context, p_InstanceID);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteX2ScheduledActivitySplit(ITransactionContext p_Context, Int64 p_InstanceID, int ActivityID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.DeleteX2ScheduledActivitySplit(m_context, p_InstanceID, ActivityID);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2SoonestScheduledActivity(ITransactionContext p_Context, dsX2ScheduledActivities.ScheduledActivityDataTable p_X2ScheduledActivities, string WorkFlowProviderName)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2SoonestScheduledActivity(m_context, p_X2ScheduledActivities, WorkFlowProviderName);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2ScheduledActivity(ITransactionContext p_Context, dsX2ScheduledActivities.ScheduledActivityDataTable p_X2ScheduledActivities, DateTime p_ScheduledTime)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2ScheduledActivity(m_context, p_X2ScheduledActivities, p_ScheduledTime);
            }
            catch
            {
                throw;
            }
        }

        public DateTime GetX2ScheduledActivityNextTime(ITransactionContext p_Context, DateTime p_ScheduledTime, string p_WorkFlowProviderName)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.GetX2ScheduledActivityNextTime(m_context, p_ScheduledTime, p_WorkFlowProviderName);
            }
            catch
            {
                throw;
            }
        }

        public int UpdateX2ScheduledActivity(ITransactionContext p_Context, Int64 p_InstanceID, string p_WorkFlowProviderName)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.UpdateX2ScheduledActivity(m_context, p_InstanceID, p_WorkFlowProviderName);
            }
            catch
            {
                throw;
            }
        }

        public void CancelX2Activity(ITransactionContext p_Context, Int64 p_InstanceID, bool p_WasCreate)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.CancelX2ScheduledActivity(m_context, p_InstanceID, p_WasCreate);
            }
            catch
            {
                throw;
            }
        }

        public int GetX2StateIDByNameForInstance(ITransactionContext p_Context, Int64 p_InstanceID, string p_StateName)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.GetX2StateIDByNameForInstance(m_context, p_InstanceID, p_StateName);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteX2ExternalActivity(ITransactionContext p_Context, int p_ID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.DeleteX2ExternalActivity(m_context, p_ID);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2ExternalActivity(ITransactionContext p_Context, dsX2ExternalActivities.ActiveExternalActivityDataTable p_X2ExternalActivities)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2ExternalActivity(m_context, p_X2ExternalActivities);
            }
            catch
            {
                throw;
            }
        }

        public int UpdateX2ExternalActivity(ITransactionContext p_Context, int p_ID, string p_WorkFlowProviderName)
        {
            base.AuthenticateUser();
            ITransactionContext m_context = p_Context;

            try
            {
                return X2Worker.UpdateX2ExternalActivity(m_context, p_ID, p_WorkFlowProviderName);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2ActivatedExternalActivities(ITransactionContext p_Context, dsX2ActivatedExternalActivities p_X2DS, int p_ActivatedByExternalActivityID, Int64 ActivatingInstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2ActivatedExternalActivities(m_context, p_X2DS, p_ActivatedByExternalActivityID, ActivatingInstanceID);
            }
            catch
            {
                throw;
            }
        }

        public void GetX2CreateInfo(ITransactionContext p_Context, int p_WorkFlowID, dsX2CreateInfo.CreateInfoDataTable p_X2CreateInfo)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2CreateInfo(m_context, p_WorkFlowID, p_X2CreateInfo);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteX2ActivatedExternalActivity(ITransactionContext p_Context, int p_ActivatedExternalActivityID)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.DeleteX2ActivatedExternalActivity(m_context, p_ActivatedExternalActivityID);
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        public void GetX2ProcessInfoByInstance(ITransactionContext p_Context, Int64 p_InstanceID, ref int p_ProcessID, ref string p_ProcessName, ref string p_WorkFlowName)
        {
            base.AuthenticateUser();

            ITransactionContext m_context = p_Context;

            try
            {
                X2Worker.GetX2ProcessInfoByInstance(m_context, p_InstanceID, ref p_ProcessID, ref p_ProcessName, ref p_WorkFlowName);
            }
            catch
            {
                throw;
            }
        }

        public void _UpdateX2ActiveExternalActivity(ITransactionContext p_Context, SAHL.X2.Framework.DataSets.X2.ActiveExternalActivityDataTable p_DT)
        {
            base.AuthenticateUser();

            ITransactionContext context = p_Context;

            try
            {
                X2Worker.UpdateX2ActiveExternalActivity(context, p_DT);
            }
            catch
            {
                TransactionController.RollBack(context);
                throw;
            }
        }

        public void UpdateX2ActiveExternalActivity(SAHL.X2.Framework.DataSets.X2.ActiveExternalActivityDataTable p_DT)
        {
            base.AuthenticateUser();
            ITransactionContext context = null;

            try
            {
                context = TransactionController.GetContext();
                X2Worker.UpdateX2ActiveExternalActivity(context, p_DT);
                TransactionController.Commit(context);
            }
            catch
            {
                TransactionController.RollBack(context);
                throw;
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }
        }

        public int GetNextWorkflowID(int p_ProcessID, string p_WorkFlowName)
        {
            base.AuthenticateUser();

            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                return X2Worker.GetNextWorkflowID(ctx, p_ProcessID, p_WorkFlowName);
            }
            catch
            {
                throw;
            }
        }

        //public int GetExternalActivityID(int p_WorkFlowID, string p_ExternalActivitySourceName
        //{
        //    base.AuthenticateUser();
        //    string m_metricInfo = "Succeeded";
        //    TransactionContext context = null;

        //    try
        //    {
        //        return X2Worker.GetExternalActivityID(context, p_WorkFlowID, p_ExternalActivitySourceName);
        //    }
        //    catch (Exception e)
        //    {
        //        m_metricInfo = "Failed";
        //        bool rethrow = ExceptionPolicy.HandleException(e, "Service Facade");
        //        if (rethrow)
        //            throw;
        //        return -1;
        //    }
        //    finally
        //    {
        //        //base.//StoreMetric("GetExternalActivityID", m_metricInfo);//);//, p_Mi);
        //    }
        //}

        // PAul C
        public void InsertError(ITransactionContext p_Context, Int64 InstanceID, string ErrorMessage, string StackTrace)
        {
            base.AuthenticateUser();

            try
            {
                X2Worker.InsertError(p_Context, InstanceID, ErrorMessage, StackTrace);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("InsertError", m_metricInfo, Mi);
            }
        }

        // Paul C
        public void DeleteX2Instance(Int64 InstanceID)
        {
            base.AuthenticateUser();

            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                X2Worker.DeleteX2Instance(ctx, InstanceID);
                TransactionController.Commit(ctx);
            }
            catch
            {
                throw;
            }
            finally
            {
                //base.//StoreMetric("DeleteX2Instance", m_metricInfo, Mi);
            }
        }

        // Paul C
        public int GetX2WorkflowID(string WorkflowName)
        {
            base.AuthenticateUser();

            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                return X2Worker.GetX2WorkFlowID(ctx, WorkflowName);
            }
            catch
            {
                throw;
            }
        }

        // Paul C
        public int GetX2ActivityID(string ActivityName, int WorkflowID, int? StateID)
        {
            base.AuthenticateUser();

            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                return X2Worker.GetX2ActivityID(ctx, ActivityName, WorkflowID, StateID);
            }
            catch
            {
                throw;
            }
        }

        // Paul C
        public int GetX2StateID(string StateName, int WorkflowID)
        {
            base.AuthenticateUser();
            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                return X2Worker.GetX2StateID(ctx, StateName, WorkflowID);
            }
            catch
            {
                throw;
            }
        }

        // Paul C
        public int GetX2ExternalActivityID(int WorkflowID, string ExternalActivityName)
        {
            base.AuthenticateUser();
            ITransactionContext ctx = TransactionController.GetContext("X2");
            try
            {
                return X2Worker.GetX2ExternalActivityID(ctx, WorkflowID, ExternalActivityName);
            }
            catch
            {
                throw;
            }
        }

        // Paul C
        //public int GetLatestProcessVersion(string ProcessName
        //{
        //    base.AuthenticateUser();
        //    ITransactionContext ctx = TransactionController.GetContext("X2"); ;

        //    try
        //    {
        //        return X2Worker.GetLatestProcessVersion(ctx, ProcessName);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //  Paul C - this is for my testing app that allows us to navigate and create cases
        public void GetStateActivityExternalForWorkflow(int WorkflowID, SAHL.X2.Framework.DataSets.X2 ds)
        {
            base.AuthenticateUser();
            ITransactionContext ctx = TransactionController.GetContext("X2");

            try
            {
                X2Worker.GetStateActivityExternalForWorkflow(ctx, WorkflowID, ds);
            }
            catch
            {
                throw;
            }
            finally
            {
                // not necessary
            }
        }

        public void GetX2MetaData(SAHL.X2.Framework.DataSets.X2 ds, string ProcessName, string Workflow)
        {
            base.AuthenticateUser();
            ITransactionContext ctx = null;
            try
            {
                ctx = TransactionController.GetContext("X2");
                X2Worker.GetMetaDataForWorkflow(ctx, ProcessName, Workflow, ds);
            }
            catch
            {
                throw;
            }
        }

        public void WorkflowAndProcessNameGet(ITransactionContext ctx, Int64 InstanceID, ref string WorkflowName, ref string ProcessName)
        {
            base.AuthenticateUser();
            try
            {
                ctx = TransactionController.GetContext("X2");
                DataRow dr = X2Worker.WorkflowAndProcessNameGet(ctx, InstanceID);
                if (null != dr)
                {
                    WorkflowName = dr["WorkflowName"].ToString();
                    ProcessName = dr["ProcessName"].ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        public void GetChildInstances(ITransactionContext ctx, Int64 InstanceID, SAHL.X2.Framework.DataSets.X2.InstanceDataTable dtParentInstance)
        {
            base.AuthenticateUser();
            try
            {
                X2Worker.GetChildInstances(ctx, InstanceID, dtParentInstance);
            }
            catch
            {
                throw;
            }
        }

        public Dictionary<int, string> GetProcessesToPreload(ITransactionContext ctx)
        {
            base.AuthenticateUser();
            try
            {
                return X2Worker.GetProcessesToPreload(ctx);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateStageTransitions(ITransactionContext ctx, SAHL.X2.Framework.DataSets.X2.StageTransitionDataTable dt)
        {
            base.AuthenticateUser();
            try
            {
                X2Worker.UpdateStageTransitions(ctx, dt);
            }
            catch
            {
                throw;
            }
        }

        public string GetX2WorkFlowNameForWorkflowID(ITransactionContext ctx, int WorkflowID)
        {
            base.AuthenticateUser();
            try
            {
                return X2Worker.GetX2WorkFlowNameForWorkflowID(ctx, WorkflowID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method copies the worklist data to the WorklistLog table and populates the HistoryKey with the Given parameter.
        /// </summary>
        /// <param name="p_X2DS"></param>
        /// <param name="p_WorkFlowHistoryID"></param>
        /// <param name="p_InstanceID"></param>
        public void TransposeWorkstateToLog(DataSets.X2 p_X2DS, Int32 p_WorkFlowHistoryID, Int64 p_InstanceID)
        {
            p_X2DS.WorkListLog.Clear();
            foreach (SAHL.X2.Framework.DataSets.X2.WorkListRow row in p_X2DS.WorkList)
            {
                SAHL.X2.Framework.DataSets.X2.WorkListLogRow logrow = p_X2DS.WorkListLog.NewWorkListLogRow();
                row.CopyRow(logrow);
                logrow.WorkflowHistoryID = p_WorkFlowHistoryID;
                p_X2DS.WorkListLog.AddWorkListLogRow(logrow);
            }
        }
    }

    public static class X2SFE_Extentions
    {
        public static void CopyRow(this SAHL.X2.Framework.DataSets.X2.WorkListRow p_WorkListRow, SAHL.X2.Framework.DataSets.X2.WorkListLogRow p_Target)
        {
            p_Target.ADUserName = p_WorkListRow.ADUserName;
            p_Target.InstanceID = p_WorkListRow.InstanceID;
            p_Target.ListDate = p_WorkListRow.ListDate;
            return;
        }
    }
}