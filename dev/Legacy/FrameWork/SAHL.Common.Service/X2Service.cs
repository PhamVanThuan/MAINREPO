using Castle.ActiveRecord.Queries;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.X2.Common;
using SAHL.X2.Framework;
using SAHL.X2.Framework.Common;
using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IX2Service))]
    public class X2Service : IX2Service
    {
        public void LogIn(SAHLPrincipal principal)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            IX2Provider x2Provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Provider == null)
            {
                x2Provider = new X2WebEngineProvider();
                if (x2Provider == null)
                    throw new X2EngineException("X2 Engine Provider Failure. Cannot perform LogIn.");
            }

            X2ResponseBase R = x2Provider.LogIn();
            spc.X2Provider = x2Provider;

            if (R.IsErrorResponse == false)
            {
                X2LoginResponse LR = R as X2LoginResponse;

                if (x2Info == null)
                    x2Info = new X2Info();
                else
                    ClearX2Info(x2Info);

                x2Info.SessionID = LR.SessionId;
                spc.X2Info = x2Info;
            }
            else
                throw new X2EngineException(R.Exception.Value);
        }

        public void LogOut(SAHLPrincipal principal)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            X2Info x2Info = spc.X2Info as X2Info;
            IX2Provider provider = spc.X2Provider as IX2Provider;

            if (x2Info != null && !String.IsNullOrEmpty(x2Info.SessionID))
            {
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform LogOut.");
                X2ResponseBase R = provider.LogOut(x2Info.SessionID);

                if (R.IsErrorResponse == false)
                {
                    ClearX2Info(x2Info);
                    spc.X2Info = x2Info;
                }
                else
                    throw new X2EngineException(R.Exception.Value);
            }
            else
                throw new X2EngineException("No Session available. Cannot perform LogOut.");
        }

        public void CreateWorkFlowInstance(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            CreateWorkFlowInstance(principal, ProcessName, ProcessVersion, WorkFlowName, ActivityName, InputFields, IgnoreWarnings, null);
        }

        public void CreateWorkFlowInstance(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            DateTime startTime = DateTime.Now;
            string source = "CreateWorkFlowInstance";
            long instanceID = -1;
            string correlationId = string.Empty;

            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            IX2Provider provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform CreateWorkFlowInstance.");

                X2ResponseBase R = provider.CreateWorkFlowInstance(x2Info.SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, InputFields, IgnoreWarnings, Data);
                correlationId = R.RequestCorrelationId.ToString();
                spc.X2Provider = provider;
                HandleX2DomainMessages(R.Messages, principal);

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { Metrics.X2INSTANCEID, instanceID }, { Metrics.SessionID, x2Info.SessionID },
                { Metrics.X2WORKFLOWNAME, WorkFlowName }, { Metrics.X2PROCESSNAME, ProcessName }, { Metrics.X2ACTIVITYNAME, ActivityName },{Metrics.RESPONSECORRELATIONID,correlationId} };
                MetricsPlugin.Metrics.PublishLatencyMetric(startTime, DateTime.Now - startTime, source, parameters);

                if (R.IsErrorResponse == false)
                {
                    X2ActivityStartResponse SR = R as X2ActivityStartResponse;
                    if (SR != null)
                    {
                        x2Info.ActivityName = ActivityName;
                        x2Info.InstanceID = SR.InstanceId;
                        spc.X2Info = x2Info;
                        instanceID = SR.InstanceId;
                    }
                    else
                        throw new X2EngineException("CreateWorkFlowInstance failed, could not convert response.");
                }
                else
                    throw new X2EngineException(R.Exception.Value + "SessionID : " + x2Info.SessionID);
            }
            else
                throw new X2EngineException("No Session available. Cannot perform CreateWorkFlowInstance.");
        }

        public X2ServiceResponse CreateWorkFlowInstanceWithComplete(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            return CreateWorkFlowInstanceWithComplete(principal, ProcessName, ProcessVersion, WorkFlowName, ActivityName, InputFields, IgnoreWarnings, null);
        }

        public X2ServiceResponse CreateWorkFlowInstanceWithComplete(SAHLPrincipal principal, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            X2ServiceResponseWithInstance svcResp = null;
            DateTime startTime = DateTime.Now;
            string source = "CreateWorkFlowInstanceWithComplete";
            long instanceID = -1;
            string correlationId = string.Empty;

            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            IX2Provider provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform CreateWorkFlowInstanceWithComplete.");

                X2ResponseBase R = provider.CreateWorkFlowInstanceWithComplete(x2Info.SessionID, ProcessName, ProcessVersion, WorkFlowName, ActivityName, InputFields, IgnoreWarnings, Data);
                var completeResponse = R as X2ActivityCompleteResponse;
                correlationId = R.RequestCorrelationId.ToString();
                spc.X2Provider = provider;
                HandleX2DomainMessages(R.Messages, principal);
                svcResp = new X2ServiceResponseWithInstance(R.XMLResponse, R.IsErrorResponse, completeResponse.InstanceId);

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { Metrics.X2INSTANCEID, instanceID }, { Metrics.SessionID, x2Info.SessionID },
                { Metrics.X2WORKFLOWNAME, WorkFlowName }, { Metrics.X2PROCESSNAME, ProcessName }, { Metrics.X2ACTIVITYNAME, ActivityName },{Metrics.RESPONSECORRELATIONID,correlationId} };
                MetricsPlugin.Metrics.PublishLatencyMetric(startTime, DateTime.Now - startTime, source, parameters);

                if (R.IsErrorResponse == false)
                {
                    X2ActivityCompleteResponse CR = R as X2ActivityCompleteResponse;

                    if (CR != null)
                    {
                        x2Info.CurrentState = CR.StateName;
                        // clear out the activity as it has been successfully completed
                        x2Info.ActivityName = "";
                        spc.X2Info = x2Info;

                        // This check was done as the complete activity is sometimes called from batch jobs
                        // which are not done in the context of the CBO.
                        if (spc.NodeSets != null && spc.NodeSets.ContainsKey(CBONodeSetType.X2))
                        {
                            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;
                            nodeSet.SelectedContextNode = null;
                        }
                    }
                    else
                        throw new X2EngineException("CreateWorkFlowInstanceWithComplete failed, could not convert response.");
                }
                else
                {
                    X2ErrorResponse err = R as X2ErrorResponse;
                    if (null != spc.DomainMessages && spc.DomainMessages.Count > 0)
                    {
                        throw new DomainValidationException("X2 activity failed due to validation, please see Messages for details");
                    }
                    else
                    {
                        throw new X2EngineException(R.Exception.Value);
                    }
                }
            }
            else
                throw new X2EngineException("No Session available. Cannot perform CreateWorkFlowInstanceWithComplete.");

            return svcResp;
        }

        public void StartActivity(SAHLPrincipal principal, long InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            StartActivity(principal, InstanceID, ActivityName, InputFields, IgnoreWarnings, null);
        }

        public void StartActivity(SAHLPrincipal principal, long InstanceID, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            DateTime startTime = DateTime.Now;
            string source = "StartActivity";
            string correlationId = string.Empty;

            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            IX2Provider provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform StartActivity.");

                // we want to call start again.
                X2ResponseBase R = provider.StartActivity(x2Info.SessionID, InstanceID, ActivityName, IgnoreWarnings, Data);
                correlationId = R.RequestCorrelationId.ToString();
                spc.X2Provider = provider;

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { Metrics.X2INSTANCEID, InstanceID }, { Metrics.X2ACTIVITYNAME, ActivityName },
                { Metrics.SessionID, x2Info.SessionID }, { Metrics.RESPONSECORRELATIONID, correlationId } };
                MetricsPlugin.Metrics.PublishLatencyMetric(startTime, DateTime.Now - startTime, source, parameters);

                HandleX2DomainMessages(R.Messages, principal);
                if (R.IsErrorResponse == false)
                {
                    X2ActivityStartResponse SR = R as X2ActivityStartResponse;

                    if (SR != null)
                    {
                        x2Info.ActivityName = ActivityName;
                        x2Info.InstanceID = SR.InstanceId;
                        spc.X2Info = x2Info;
                    }
                    else
                        throw new X2EngineException("StartActivity failed, could not convert response.");
                }
                else if (((X2ErrorResponse)R).ErrorCode == 9)
                {
                    throw new X2InstanceLockedException(((X2ErrorResponse)R).Exception.Value);
                }
                else if (((X2ErrorResponse)R).ErrorCode == 12)
                {
                    List<string> Messages = new List<string>();
                    foreach (IX2Message msg in R.Messages)
                    {
                        Messages.Add(msg.Message);
                    }
                    throw new X2ActivityStartFailedException(R.Exception.Value, Messages);
                }
                else if (((X2ErrorResponse)R).ErrorCode == 10)
                {
                    // get reason list from the DMC
                }
                else
                    throw new X2EngineException(R.Exception.Value + "SessionID : " + x2Info.SessionID);
            }
            else
                throw new X2EngineException("No Session available. Cannot perform StartActivity.");
        }

        public X2ServiceResponse CompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings)
        {
            return CompleteActivity(principal, InputFields, IgnoreWarnings, null);
        }

        public X2ServiceResponse CompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            long instanceID = -1;
            DateTime startTime = DateTime.Now;
            string source = "CompleteActivity";
            string correlationId = string.Empty;

            X2ServiceResponse svcResp = null;
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            IX2Provider provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (x2Info.InstanceID == 0)
                    throw new X2EngineException("No Instance available. Cannot perform CompleteActivity.");

                if (x2Info.ActivityName == "")
                    throw new X2EngineException("No ActivityName available. Cannot perform CompleteActivity.");

                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform CompleteActivity.");

                instanceID = x2Info.InstanceID;
                X2ResponseBase R = provider.CompleteActivity(x2Info.SessionID, x2Info.InstanceID, x2Info.ActivityName, InputFields, IgnoreWarnings, Data);
                spc.X2Provider = provider;
                HandleX2DomainMessages(R.Messages, principal);
                svcResp = new X2ServiceResponse(R.XMLResponse, R.IsErrorResponse);
                correlationId = R.RequestCorrelationId.ToString();

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { Metrics.X2INSTANCEID, instanceID }, { Metrics.SessionID, x2Info.SessionID }, { Metrics.RESPONSECORRELATIONID, correlationId } };
                MetricsPlugin.Metrics.PublishLatencyMetric(startTime, DateTime.Now - startTime, source, parameters);

                if (R.IsErrorResponse == false)
                {
                    X2ActivityCompleteResponse CR = R as X2ActivityCompleteResponse;

                    if (CR != null)
                    {
                        x2Info.CurrentState = CR.StateName;
                        // clear out the activity as it has been successfully completed
                        x2Info.ActivityName = "";
                        spc.X2Info = x2Info;

                        // This check was done as the complete activity is sometimes called from batch jobs
                        // which are not done in the context of the CBO.
                        if (spc.NodeSets != null && spc.NodeSets.ContainsKey(CBONodeSetType.X2))
                        {
                            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;
                            nodeSet.SelectedContextNode = null;
                        }
                    }
                    else
                        throw new X2EngineException("CompleteActivity failed, could not convert response.");
                }
                else
                {
                    X2ErrorResponse err = R as X2ErrorResponse;
                    if (null != spc.DomainMessages && spc.DomainMessages.Count > 0)
                    {
                        throw new DomainValidationException("X2 activity failed due to validation, please see Messages for details");
                    }
                    else
                    {
                        throw new X2EngineException(R.Exception.Value);
                    }
                }
            }
            else
                throw new X2EngineException("No Session available. Cannot perform CompleteActivity.");

            return svcResp;
        }

        public X2ServiceResponse CreateCompleteActivity(SAHLPrincipal principal, Dictionary<string, string> InputFields, bool IgnoreWarnings, object Data)
        {
            long instanceID = -1;
            DateTime startTime = DateTime.Now;
            string source = "CompleteActivity";
            string correlationId = string.Empty;

            X2ServiceResponse svcResp = null;
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            IX2Provider provider = spc.X2Provider as IX2Provider;
            X2Info x2Info = spc.X2Info as X2Info;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (x2Info.InstanceID == 0)
                    throw new X2EngineException("No Instance available. Cannot perform CompleteActivity.");

                if (x2Info.ActivityName == "")
                    throw new X2EngineException("No ActivityName available. Cannot perform CompleteActivity.");

                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform CompleteActivity.");

                instanceID = x2Info.InstanceID;
                X2ResponseBase R = provider.CreateCompleteActivity(x2Info.SessionID, x2Info.InstanceID, x2Info.ActivityName, InputFields, IgnoreWarnings, Data);
                spc.X2Provider = provider;
                HandleX2DomainMessages(R.Messages, principal);
                svcResp = new X2ServiceResponse(R.XMLResponse, R.IsErrorResponse);
                correlationId = R.RequestCorrelationId.ToString();

                Dictionary<string, object> parameters = new Dictionary<string, object>() { { Metrics.X2INSTANCEID, instanceID }, { Metrics.SessionID, x2Info.SessionID }, { Metrics.RESPONSECORRELATIONID, correlationId } };
                MetricsPlugin.Metrics.PublishLatencyMetric(startTime, DateTime.Now - startTime, source, parameters);

                if (R.IsErrorResponse == false)
                {
                    X2ActivityCompleteResponse CR = R as X2ActivityCompleteResponse;

                    if (CR != null)
                    {
                        x2Info.CurrentState = CR.StateName;
                        // clear out the activity as it has been successfully completed
                        x2Info.ActivityName = "";
                        spc.X2Info = x2Info;

                        // This check was done as the complete activity is sometimes called from batch jobs
                        // which are not done in the context of the CBO.
                        if (spc.NodeSets != null && spc.NodeSets.ContainsKey(CBONodeSetType.X2))
                        {
                            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;
                            nodeSet.SelectedContextNode = null;
                        }
                    }
                    else
                        throw new X2EngineException("CompleteActivity failed, could not convert response.");
                }
                else
                {
                    X2ErrorResponse err = R as X2ErrorResponse;
                    if (null != spc.DomainMessages && spc.DomainMessages.Count > 0)
                    {
                        throw new DomainValidationException("X2 activity failed due to validation, please see Messages for details");
                    }
                    else
                    {
                        throw new X2EngineException(R.Exception.Value);
                    }
                }
            }
            else
                throw new X2EngineException("No Session available. Cannot perform CompleteActivity.");

            return svcResp;
        }

        /// <summary>
        /// Wrap the cancel activity
        /// Any x2 exceptions will be swallowed
        /// </summary>
        /// <param name="principal"></param>
        public void TryCancelActivity(SAHLPrincipal principal)
        {
            try
            {
                CancelActivity(principal);
            }
            catch (X2EngineException)
            {
                // to nothing add log later
            }
        }

        public void CancelActivity(SAHLPrincipal principal)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Info == null)
                LogIn(principal);

            X2Info x2Info = spc.X2Info as X2Info;
            IX2Provider provider = spc.X2Provider as IX2Provider;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (string.IsNullOrEmpty(x2Info.ActivityName))
                    return;
                if (x2Info.InstanceID == 0)
                    throw new X2EngineException("No Instance available. Cannot perform CancelActivity.");
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot perform CancelActivity.");
                X2ResponseBase R = provider.CancelActivity(x2Info.SessionID, x2Info.InstanceID, x2Info.ActivityName);
                spc.X2Provider = provider;
                HandleX2DomainMessages(R.Messages, principal);
                if (R.IsErrorResponse == false)
                {
                    X2ActivityCancelResponse CR = R as X2ActivityCancelResponse;
                    if (CR != null)
                    {
                        // clear out the activity as it has been successfully cancelled
                        x2Info.ActivityName = "";
                        spc.X2Info = x2Info;
                    }
                    else
                        throw new X2EngineException("CancelActivity failed, could not convert response.");
                }
                else
                    throw new X2EngineException(R.Exception.Value + "SessionID : " + x2Info.SessionID);
            }
            else
                throw new X2EngineException("No Session available. Cannot perform CancelActivity.");
        }

        private void ClearX2Info(IX2Info Info)
        {
            Info.SessionID = "";
            Info.ActivityName = "";
            Info.InstanceID = 0;
        }

        public IX2Info GetX2Info(SAHLPrincipal principal)
        {
            // get the cached data for the principal
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

            if (spc.X2Provider == null || spc.X2Info == null)
                LogIn(SAHLPrincipal.GetCurrent());

            return spc.X2Info as IX2Info;
        }

        public IEventList<IActivity> GetWorkFlowActivitiesForPrincipal(SAHLPrincipal principal, long WorkFlowID)
        {
            if (principal == null)
                return null;

            string query = String.Format("SELECT DISTINCT a FROM Activity_DAO a WHERE a.WorkFlow.ID = {0} AND a.State.ID is null", WorkFlowID);

            SimpleQuery q = new SimpleQuery(typeof(Activity_DAO), query);
            Activity_DAO[] result = Activity_DAO.ExecuteQuery(q) as Activity_DAO[];

            if (result == null)
                result = new Activity_DAO[0];

            List<Activity_DAO> list = new List<Activity_DAO>(result);

            for (int i = list.Count - 1; i > -1; i--)
            {
                bool found = false;

                for (int k = 0; k < list[i].SecurityGroups.Count; k++)
                {
                    if (principal.IsInRole(list[i].SecurityGroups[k].Name))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    list.RemoveAt(i);
            }

            return new DAOEventList<Activity_DAO, IActivity, Activity>(list);
        }

        public IEventList<IActivity> GetUserActivitiesForInstance(SAHLPrincipal principal, long InstanceID)
        {
            if (principal == null)
                return null;

            string HQL = "SELECT DISTINCT ias FROM InstanceActivitySecurity_DAO ias WHERE ias.Instance.ID = ? AND ias.Activity.ActivityType.ID = 1";// order by ias.Activity.Priority asc";

            SimpleQuery<InstanceActivitySecurity_DAO> q = new SimpleQuery<InstanceActivitySecurity_DAO>(HQL, InstanceID);
            InstanceActivitySecurity_DAO[] InstanceActivitySecurities = q.Execute();

            List<IActivity> list = new List<IActivity>();

            for (int i = 0; i < InstanceActivitySecurities.Length; i++)
            {
                if (InstanceActivitySecurities[i].ADUserName.ToLower() == principal.Identity.Name.ToLower())
                {
                    IActivity activity = new Activity(InstanceActivitySecurities[i].Activity);
                    int idx = list.Count;

                    for (int k = 0; k < list.Count; k++)
                    {
                        if (list[k].Priority > activity.Priority)
                        {
                            idx = k;
                            break;
                        }
                    }

                    list.Insert(idx, activity);
                }
                else if (principal.IsInRole(InstanceActivitySecurities[i].ADUserName))
                {
                    IActivity activity = new Activity(InstanceActivitySecurities[i].Activity);
                    int idx = list.Count;

                    for (int k = 0; k < list.Count; k++)
                    {
                        if (list[k].Priority > activity.Priority)
                        {
                            idx = k;
                            break;
                        }
                    }

                    list.Insert(idx, activity);
                }
            }

            return new EventList<IActivity>(list);
        }

        public IEventList<IForm> GetFormsForInstance(long InstanceID)
        {
            Instance_DAO i_dao = Instance_DAO.Find(InstanceID);
            Instance ins = new Instance(i_dao);

            return ins.State.Forms;
        }

        public IDictionary<string, object> GetX2DataRow(long InstanceID)
        {
            string x2Catalog = Properties.Settings.Default.X2Catalog;

            Instance_DAO i_dao = Instance_DAO.Find(InstanceID);
            Instance instance = new Instance(i_dao);

            string query = string.Format("SELECT * FROM {0}.X2DATA.{1} WHERE InstanceID = {2}", x2Catalog, instance.WorkFlow.StorageTable, InstanceID);
            DataTable DT = new DataTable();

            IDbConnection con = Helper.GetSQLDBConnection();
            Helper.FillFromQuery(DT, query, con, null);

            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    dict.Add(DT.Columns[i].ColumnName, DT.Rows[0].ItemArray[i]);
                }
            }

            return dict;
        }

        public DataTable GetX2DataRowByFieldAndKey(string x2StorageTable, string x2StorageTableField, int keyValue)
        {
            string x2Catalog = Properties.Settings.Default.X2Catalog;
            string query = string.Format("SELECT * FROM {0}.X2DATA.{1} WHERE {2} = {3}", x2Catalog, x2StorageTable, x2StorageTableField, keyValue);
            DataTable DT = new DataTable();

            IDbConnection con = Helper.GetSQLDBConnection();
            Helper.FillFromQuery(DT, query, con, null);
            return DT;
        }

        public void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data)
        {
            string x2Catalog = Properties.Settings.Default.X2Catalog;
            Instance_DAO i_dao = Instance_DAO.Find(InstanceID);
            Instance instance = new Instance(i_dao);

            string Query = "UPDATE {0}.X2DATA.{1} SET ";
            string Where = " WHERE InstanceID = {2} ";

            IDbConnection con = Helper.GetSQLDBConnection();
            ParameterCollection Params = new ParameterCollection();
            foreach (KeyValuePair<string, object> KP in X2Data)
            {
                Query += (KP.Key + " = @" + KP.Key + ", ");
                Params.Add(new SqlParameter("@" + KP.Key, KP.Value));
            }
            Query = Query.Substring(0, Query.Length - 2);
            Query += Where;
            Query = String.Format(Query, x2Catalog, instance.WorkFlow.StorageTable, InstanceID);
            Helper.ExecuteNonQuery(con, Query, Params);
        }

        public void GetCustomInstanceDetails(DataTable p_DT, string p_StatementName, int StateID, SAHLPrincipal principal)
        {
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
                SAHL.Common.X2.BusinessModel.State state = new SAHL.Common.X2.BusinessModel.State(State_DAO.Find(StateID));
                string ADUserGroupFilter = spc.GetCachedRolesAsStringForQuery(false, false);

                if (String.IsNullOrEmpty(ADUserGroupFilter))
                    ADUserGroupFilter = "'" + principal.Identity.Name + "'";

                // Fetch the query
                string Query = String.Format(UIStatementRepository.GetStatement("COMMON", p_StatementName), ADUserGroupFilter);

                ParameterCollection Parameters = new ParameterCollection();
                Helper.AddVarcharParameter(Parameters, "@ADUserName", principal.Identity.Name);
                Helper.AddIntParameter(Parameters, "@ProcessID", state.WorkFlow.Process.ID);
                Helper.AddIntParameter(Parameters, "@WorkflowID", state.WorkFlow.ID);
                Helper.AddIntParameter(Parameters, "@StateID", state.ID);

                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    con.Open();
                    IDataReader reader = Helper.ExecuteReader(con, Query, Parameters);
                    p_DT.Load(reader);
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("GetCustomInstanceDetails failed for statement {0}: ", p_StatementName) + ex.Message);
            }
        }

        public void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator, Dictionary<string, string> FieldInputs)
        {
            WorkFlowWizardNext(principal, CurrentViewName, Navigator, FieldInputs, null);
        }

        public void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator, string NavigateTo)
        {
            WorkFlowWizardNext(principal, CurrentViewName, Navigator, null, NavigateTo);
        }

        public void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator, Dictionary<string, string> FieldInputs, string NavigateTo)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;

            List<CBONode> nodes = nodeSet.ContextNodes;

            //the contextnodes collection is still the old one!!

            if (IsViewDefaultFormForState(principal, CurrentViewName) && nodes[0].Description != "Forms")
            {
                InstanceNode IN = GetInstanceNodeForSelectedNode(principal);
                if (IN == null)
                    return;

                StartActivity(principal, IN.InstanceID, nodes[0].Description, null, false);
                CompleteActivity(principal, FieldInputs, false);

                long InstanceID = IN.InstanceID;
                Instance_DAO i_dao = Instance_DAO.Find(InstanceID);
                i_dao.Refresh();

                if (String.IsNullOrEmpty(NavigateTo))
                {
                    if (i_dao.State.StateType.ID != 1) //system state
                    {
                        Navigator.Navigate("X2NonUserState");
                        return;
                    }

                    if (i_dao.State.Forms == null || i_dao.State.Forms.Count == 0)
                        Navigator.Navigate("X2WorkList");
                    else
                        Navigator.Navigate(i_dao.State.Forms[0].Name);
                }
                else
                    Navigator.Navigate(NavigateTo);

                //now refresh the contextnodes!!

                CBOManager CBOManager = new CBOManager();
                CBOManager.ClearContextNodes(principal, CBONodeSetType.X2);

                return;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Description == "Forms")
                {
                    Navigator.Navigate(nodes[i].ChildNodes[0].URL);
                    return;
                }
            }
        }

        public void WorkFlowWizardNext(SAHLPrincipal principal, string CurrentViewName, ISimpleNavigator Navigator)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            WorkFlowWizardNext(principal, CurrentViewName, Navigator, null, null);
        }

        public void WorkflowNavigate(SAHLPrincipal principal, ISimpleNavigator Navigator)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;
            List<CBONode> nodes = nodeSet.ContextNodes;
            CBOManager CBOManager = new CBOManager();

            X2Info x2Info = spc.X2Info as X2Info;

            Instance_DAO i_dao = Instance_DAO.Find(x2Info.InstanceID);
            i_dao.Refresh();
            string User = Thread.CurrentPrincipal.Identity.Name;
            bool CaseAssignedToUser = false;
            if (null != i_dao.WorkLists)
            {
                for (int i = 0; i < i_dao.WorkLists.Count; i++)
                {
                    if (i_dao.WorkLists[i].ADUserName.ToLower() == User.ToLower()
                    || Thread.CurrentPrincipal.IsInRole(i_dao.WorkLists[i].ADUserName))
                    {
                        CaseAssignedToUser = true;
                        break;
                    }
                }
            }
            if (CaseAssignedToUser)
            {
                if (i_dao.State.StateType.ID != 1) //system state
                {
                    Navigator.Navigate("X2NonUserState");
                    return;
                }
                else //user state
                {
                    if (i_dao.State.Forms == null || i_dao.State.Forms.Count == 0)
                        throw new Exception("Workflow setup error - there are no Forms defined for state " + i_dao.State.Name);
                    CBOManager.ClearContextNodes(principal, CBONodeSetType.X2);
                    Navigator.Navigate(i_dao.State.Forms[0].Name);
                    return;
                }
            }
            // remove the current node from the cbo if its not assigned to the user anymore (THIS REALLY SHOULD BE HAPPENING IN A SINGLE PLACE, needs refactoring)
            InstanceNode CWN = CBOManager.GetCurrentCBONode(principal, CBONodeSetType.X2) as InstanceNode;
            if (CWN != null && CWN.InstanceID == x2Info.InstanceID)
                CBOManager.RemoveCBOMenuNode(principal, CWN.NodePath, CBONodeSetType.X2);
            CBOManager.SetCurrentCBONode(principal, null, CBONodeSetType.X2);
            CBOManager.ClearContextNodes(principal, CBONodeSetType.X2);
            Navigator.Navigate("X2WorkFlowListSummary");
        }

        public bool IsViewDefaultFormForState(SAHLPrincipal principal, string ViewName)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;

            List<CBONode> nodes = nodeSet.ContextNodes;

            InstanceNode IN = GetInstanceNodeForSelectedNode(principal);
            if (IN == null)
                return false;
            long InstanceID = IN.InstanceID;
            Instance instance = new Instance(Instance_DAO.Find(InstanceID));

            if (ViewName == instance.State.Forms[0].Description || ViewName == instance.State.Forms[0].Name)
                return true;

            return false;
        }

        public string GetURLForCurrentState(long InstanceID)
        {
            Instance instance = new Instance(Instance_DAO.Find(InstanceID));

            if (instance.State != null && instance.State.Forms != null && instance.State.Forms.Count > 0)
                return instance.State.Forms[0].Name;

            return null;
        }

        public IWorkList GetWorkListItemByInstanceID(SAHLPrincipal principal, long InstanceID)
        {
            return WorkList.FindByUserAndInstanceID(principal, InstanceID);
        }

        public InstanceNode GetInstanceNodeForSelectedNode(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            CBONodeSet nodeSet = spc.NodeSets[CBONodeSetType.X2] as CBONodeSet;

            CBONode N = nodeSet.SelectedNode;
            return GetInstanceNodeForSelectedNode(N);
        }

        private InstanceNode GetInstanceNodeForSelectedNode(CBONode Node)
        {
            InstanceNode IN = Node as InstanceNode;
            if (IN != null)
                return IN;
            else
            {
                if (Node.ParentNode != null)
                    return GetInstanceNodeForSelectedNode(Node.ParentNode);
                else
                    return null;
            }
        }

        /// <summary>
        /// After a case has been reassigned ask the engine to recalc the security and peoples worklists
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        public bool RefreshWorkListAndSecurity(Int64 InstanceID)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            X2Info x2Info = spc.X2Info as X2Info;
            IX2Provider provider = spc.X2Provider as IX2Provider;

            if (x2Info != null && x2Info.SessionID != "")
            {
                if (provider == null)
                    throw new X2EngineException("X2 Engine Provider is null. Cannot continue.");

                List<ListRequestItem> Items = new List<ListRequestItem>();
                ListRequestItem li = new ListRequestItem(InstanceID, "");
                Items.Add(li);
                X2ResponseBase R = provider.ProcessListActivity(Items);
                spc.X2Provider = provider;
                spc.X2Info = x2Info;

                if (!R.IsErrorResponse)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ClearMetaCache()
        {
            bool clearUIStatementSuccess = false;
            bool clearDSCacheSuccess = false;

            clearUIStatementSuccess = ClearUIStatements();
            ClearLookups();
            clearDSCacheSuccess = ClearDSCache();

            return clearUIStatementSuccess && clearDSCacheSuccess;
        }

        /// <summary>
        /// Tells X2 to clear a specific lookup from memory.
        /// </summary>
        public void ClearLookup(string lookUpTableName)
        {
            ClearCache(CacheTypes.LookupItem, lookUpTableName);
        }

        /// <summary>
        /// Tells X2 to clear all cached lookups from memory.
        /// </summary>
        public void ClearLookups()
        {
            ClearCache(CacheTypes.Lookups, null);
        }

        /// <summary>
        ///  Tells X2 to clear all cached UI Statements from memory.
        /// </summary>
        public bool ClearUIStatements()
        {
            return ClearCache(CacheTypes.UIStatement, null);
        }

        /// <summary>
        /// Tells X2 to clear DS Cache from memory - this will ensure its clear from both the DS and X2.
        /// </summary>
        public bool ClearDSCache()
        {
            return ClearCache(CacheTypes.DomainService, null);
        }

        public void ClearRuleCache()
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IList<IProcess> processesToClear = x2Repo.GetProcessesByUserActivity(SAHL.Common.Constants.WorkFlowActivityName.ClearCache);

            // if we have found some then lets clear the requested cache in each of them
            if (processesToClear != null & processesToClear.Count > 0)
            {
                foreach (var process in processesToClear)
                {
                    try
                    {
                        CreateWorkFlowInstance(SAHLPrincipal.GetCurrent(), process.Name, "-1", process.WorkFlows[0].Name, SAHL.Common.Constants.WorkFlowActivityName.ClearRuleCache, null, false, null);
                        X2ServiceResponse response = CreateCompleteActivity(SAHLPrincipal.GetCurrent(), null, false, null);

                        if (response.IsError)
                        {
                            break;
                        }
                    }
                    catch (X2EngineException x2e)
                    {
                        if (x2e.Message.Contains("No routes available to service the request"))
                        {
                            // swallow it and move onto the next map
                            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                            spc.DomainMessages.Clear();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private bool ClearCache(CacheTypes cacheType, string lookUpTableName)
        {
            bool success = true;

            string data = cacheType.ToString();
            if (!String.IsNullOrEmpty(lookUpTableName))
            {
                data += "," + lookUpTableName;
            }

            try
            {
                X2ServiceResponse svcResp = null;
                var principal = SAHLPrincipal.GetCurrent();
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);

                if (spc.X2Info == null)
                    LogIn(principal);

                IX2Provider provider = spc.X2Provider as IX2Provider;
                X2Info x2Info = spc.X2Info as X2Info;

                if (x2Info != null && x2Info.SessionID != "")
                {
                    if (provider == null)
                    {
                        throw new X2EngineException("X2 Engine Provider is null. Cannot perform CompleteActivity.");
                    }

                    X2ResponseBase R = provider.RefreshCacheInX2NodeProcess(data);
                    spc.X2Provider = provider;
                    HandleX2DomainMessages(R.Messages, principal);
                    svcResp = new X2ServiceResponse(R.XMLResponse, R.IsErrorResponse);

                    if (svcResp.IsError)
                    {
                        return false;
                    }
                }
                else
                {
                    throw new X2EngineException("No Session available. Cannot perform CompleteActivity.");
                }
            }
            catch (X2EngineException x2e)
            {
                if (x2e.Message.Contains("No routes available to service the request"))
                {
                    // swallow it and move onto the next map
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    spc.DomainMessages.Clear();
                }
                else
                {
                    throw;
                }
            }
            return success;
        }

        private void HandleX2DomainMessages(IX2MessageCollection x2dmc, SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            for (int i = 0; i < x2dmc.Count; i++)
            {
                IX2Message msg = x2dmc[i];
                switch (msg.MessageType)
                {
                    case X2MessageType.Error:
                        {
                            spc.DomainMessages.Add(new Error(msg.Message, ""));
                            break;
                        }
                    case X2MessageType.Warning:
                        {
                            spc.DomainMessages.Add(new Warning(msg.Message, ""));
                            break;
                        }
                }
            }
        }
    }
}