using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SAHL.X2Engine2.Providers
{
    public class WorkflowDataProvider : IWorkflowDataProvider
    {

        public const string Context = "X2";
        private ICache cache;
        private ICacheKeyGenerator cacheKeyGenerator;

        public WorkflowDataProvider(ICache cache, ICacheKeyGenerator cacheKeyGenerator)
        {
            this.cache = cache;
            this.cacheKeyGenerator = cacheKeyGenerator;
        }

        private string GetKeyById<T>(int id, string context)
        {
            CacheKeyById<T> cacheKey = new CacheKeyById<T>(context, id);
            string key = cacheKeyGenerator.GetKey<IKeyedCacheKey, T>(cacheKey);
            return key;
        }

        private string GetKeyByName<T>(string name, string context)
        {
            CacheKeyByName<T> cacheKey = new CacheKeyByName<T>(context, name);
            string key = cacheKeyGenerator.GetKey<INamedCacheKey, T>(cacheKey);
            return key;
        }

        private void AddCacheItem<T>(string key, T value)
        {
            lock (cache)
            {
                if (!cache.Contains(key))
                {
                    cache.AddItem<T>(key, value);
                }
            }
        }

        private static object lockGetStateById = new object();
        public StateDataModel GetStateById(int stateId)
        {
            lock (lockGetStateById)
            {
                string key = GetKeyById<StateDataModel>(stateId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<StateDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<StateDataModel>(SAHL.Core.Data.Models.X2.UIStatements.statedatamodel_selectbykey, new { PrimaryKey = stateId });
                    AddCacheItem<StateDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetStateDataModel = new object();
        public StateDataModel GetStateDataModel(string autoforwardStateName, string sourceWorkflowName)
        {
            lock (lockGetStateDataModel)
            {
                string key = GetKeyByName<StateDataModel>(string.Format("{0}_{1}", autoforwardStateName, sourceWorkflowName), Context);
                if (cache.Contains(key))
                    return cache.GetItem<StateDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<StateDataModel>(new StateByAutoForwardAndWorkflowNameSqlStatement(autoforwardStateName, sourceWorkflowName));
                    AddCacheItem<StateDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetUserActivitiesForState = new object();
        public IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel> GetUserActivitiesForState(int stateId)
        {
            lock (lockGetUserActivitiesForState)
            {
                string key = GetKeyById<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(stateId, string.Format("{0}_User", Context));
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<SAHL.Core.Data.Models.X2.ActivityDataModel>(new UserActivitiesForStateSqlStatement(stateId));
                    AddCacheItem<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetSystemActivitiesForState = new object();
        public IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel> GetSystemActivitiesForState(int stateId)
        {
            lock (lockGetSystemActivitiesForState)
            {
                string key = GetKeyById<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(stateId, string.Format("{0}_System", Context));
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<SAHL.Core.Data.Models.X2.ActivityDataModel>(new SystemActivitiesForStateSqlStatement(stateId));
                    AddCacheItem<IEnumerable<SAHL.Core.Data.Models.X2.ActivityDataModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetActivitySecurityForActivity = new object();
        public IEnumerable<ActivitySecurityDataModel> GetActivitySecurityForActivity(int activityId)
        {
            lock (lockGetActivitySecurityForActivity)
            {
                string key = GetKeyById<IEnumerable<ActivitySecurityDataModel>>(activityId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<ActivitySecurityDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<ActivitySecurityDataModel>(new ActivitySecurityByActivitySqlStatement(activityId));
                    AddCacheItem<IEnumerable<ActivitySecurityDataModel>>(key, result);
                    return result;
                }
            }
        }

        private static object lockGetStateWorkList = new object();
        public IEnumerable<StateWorkListDataModel> GetStateWorkList(int stateID)
        {
            lock (lockGetStateWorkList)
            {
                string key = GetKeyById<IEnumerable<StateWorkListDataModel>>(stateID, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<StateWorkListDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<StateWorkListDataModel>(new StateWorkListForStateSqlStatement(stateID));
                    AddCacheItem<IEnumerable<StateWorkListDataModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetSecurityGroup = new object();
        public SecurityGroupDataModel GetSecurityGroup(int securityGroupID)
        {
            lock (lockGetSecurityGroup)
            {
                string key = GetKeyById<SecurityGroupDataModel>(securityGroupID, Context);
                if (cache.Contains(key))
                    return cache.GetItem<SecurityGroupDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<SecurityGroupDataModel>(SAHL.Core.Data.Models.X2.UIStatements.securitygroupdatamodel_selectbykey, new { PrimaryKey = securityGroupID });
                    AddCacheItem<SecurityGroupDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetStageActivities = new object();
        public IEnumerable<StageActivityDataModel> GetStageActivities(int activityId)
        {
            lock (lockGetStageActivities)
            {
                string key = GetKeyById<IEnumerable<StageActivityDataModel>>(activityId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<StageActivityDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<StageActivityDataModel>(new StageActivityByActivitySqlStatement(activityId));
                    AddCacheItem<IEnumerable<StageActivityDataModel>>(key, result);
                    return result;
                }
            }
        }

        // pass throug
        public WorkFlowDataModel GetWorkflow(InstanceDataModel instance)
        {
            return GetWorkflowById(instance.WorkFlowID);
        }
        private static object lockGetWorkflowById = new object();
        public WorkFlowDataModel GetWorkflowById(int workflowId)
        {
            lock (lockGetWorkflowById)
            {
                string key = GetKeyById<WorkFlowDataModel>(workflowId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<WorkFlowDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<WorkFlowDataModel>(SAHL.Core.Data.Models.X2.UIStatements.workflowdatamodel_selectbykey, new { PrimaryKey = workflowId });
                    AddCacheItem<WorkFlowDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetADUser = new object();
        public ADUserDataModel GetADUser(string userName)
        {
            lock (lockGetADUser)
            {
                string key = GetKeyByName<ADUserDataModel>(userName, Context);
                if (cache.Contains(key))
                    return cache.GetItem<ADUserDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<ADUserDataModel>(new ADUserByAdUserNameSqlStatement(userName));
                    AddCacheItem<ADUserDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetActivity = new object();
        public SAHL.Core.Data.Models.X2.ActivityDataModel GetActivity(int activityId)
        {
            lock (lockGetActivity)
            {
                string key = GetKeyById<SAHL.Core.Data.Models.X2.ActivityDataModel>(activityId, string.Format("{0}_ByActivityID", Context));
                if (cache.Contains(key))
                    return cache.GetItem<SAHL.Core.Data.Models.X2.ActivityDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<SAHL.Core.Data.Models.X2.ActivityDataModel>(SAHL.Core.Data.Models.X2.UIStatements.activitydatamodel_selectbykey, new { PrimaryKey = activityId });
                    AddCacheItem<SAHL.Core.Data.Models.X2.ActivityDataModel>(key, result);
                    return result;
                }
            }
        }

        private static object lockGetActivityByActivatingExternalActivity = new object();
        public SAHL.Core.Data.Models.X2.ActivityDataModel GetActivityByActivatingExternalActivity(int externalActivityId, int currentStateForInstance)
        {
            lock (lockGetActivityByActivatingExternalActivity)
            {
                string key = GetKeyByName<SAHL.Core.Data.Models.X2.ActivityDataModel>(string.Format("{0}_{1}", externalActivityId, currentStateForInstance), string.Format("{0}_ByExternalActivityID", Context));
                if (cache.Contains(key))
                    return cache.GetItem<SAHL.Core.Data.Models.X2.ActivityDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<SAHL.Core.Data.Models.X2.ActivityDataModel>(new ActivityByActivatingExternalActivityIdSqlStatement(externalActivityId, currentStateForInstance));
                    AddCacheItem<SAHL.Core.Data.Models.X2.ActivityDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetActivityByNameAndWorkflowName = new object();
        public Activity GetActivityByNameAndWorkflowName(string activityName, string workflowName)
        {
            lock (lockGetActivityByNameAndWorkflowName)
            {
                string key = GetKeyByName<Activity>(string.Format("{0}_{1}", activityName, workflowName), Context);
                if (cache.Contains(key))
                    return cache.GetItem<Activity>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<Activity>(new ActivityByNameAndWorkflowNameSqlStatement(activityName, workflowName));
                    AddCacheItem<Activity>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetActivityById = new object();
        public Activity GetActivityById(int activityId)
        {
            lock (lockGetActivityById)
            {
                string key = GetKeyById<Activity>(activityId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<Activity>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<Activity>(new ActivityByIdSqlStatement(activityId));
                    AddCacheItem<Activity>(key, result);
                    return result;
                }
            }
        }
        public Activity GetWorkflowVersionedActivity(long instanceId, string activityName)
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.Select<Activity>(new GetActivityByInstanceIdAndActivityId(instanceId, activityName));
                return result.First();
            }
        }
        public Activity GetActivityForInstanceAndName(long instanceId, string activityName)
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.Select<Activity>(new ActivityByActivityNameAndInstanceIdSqlStatement(instanceId, activityName));
                return result.First();
            }
        }
        private static object lockGetWorkflowActivitiesForState = new object();
        public IEnumerable<WorkFlowActivityDataModel> GetWorkflowActivitiesForState(int stateId)
        {
            lock (lockGetWorkflowActivitiesForState)
            {
                string key = GetKeyById<IEnumerable<WorkFlowActivityDataModel>>(stateId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<WorkFlowActivityDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<WorkFlowActivityDataModel>(new WorkflowActivitiesForStateSqlStatement(stateId));
                    AddCacheItem<IEnumerable<WorkFlowActivityDataModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetWorkflowActivityDataModelById = new object();
        public WorkFlowActivityDataModel GetWorkflowActivityDataModelById(int workflowActivityId)
        {
            lock (lockGetWorkflowActivityDataModelById)
            {
                string key = GetKeyById<WorkFlowActivityDataModel>(workflowActivityId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<WorkFlowActivityDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<WorkFlowActivityDataModel>(SAHL.Core.Data.Models.X2.UIStatements.workflowactivitydatamodel_selectbykey, new { PrimaryKey = workflowActivityId });
                    AddCacheItem<WorkFlowActivityDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetWorkflowActivityById = new object();
        public WorkflowActivity GetWorkflowActivityById(int workflowActivityId)
        {
            lock (lockGetWorkflowActivityById)
            {
                string key = GetKeyById<WorkflowActivity>(workflowActivityId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<WorkflowActivity>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<WorkflowActivity>(new WorkflowActivityByIdSqlStatement(workflowActivityId));
                    AddCacheItem<WorkflowActivity>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetProcessById = new object();
        public ProcessDataModel GetProcessById(int processId)
        {
            lock (lockGetProcessById)
            {
                string key = GetKeyById<ProcessDataModel>(processId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<ProcessDataModel>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.SelectOne<ProcessDataModel>(SAHL.Core.Data.Models.X2.UIStatements.processdatamodel_selectbykey, new { PrimaryKey = processId });
                    AddCacheItem<ProcessDataModel>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetConfiguredProcesses = new object();
        public IEnumerable<ProcessViewModel> GetConfiguredProcesses(IEnumerable<string> configuredProcessNames)
        {
            lock (lockGetConfiguredProcesses)
            {
                string inClause = string.Empty;
                foreach (string processName in configuredProcessNames)
                {
                    inClause += "'" + processName + "',";
                }

                inClause = inClause.Remove(inClause.Length - 1, 1);

                string key = GetKeyByName<IEnumerable<ProcessViewModel>>(inClause, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<ProcessViewModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<ProcessViewModel>(new ConfiguredProcessesSqlStatement(inClause));
                    AddCacheItem<IEnumerable<ProcessViewModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetProcessAssemblies = new object();
        public IEnumerable<ProcessAssemblyDataModel> GetProcessAssemblies(int processId)
        {
            lock (lockGetProcessAssemblies)
            {
                string key = GetKeyById<IEnumerable<ProcessAssemblyDataModel>>(processId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<ProcessAssemblyDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<ProcessAssemblyDataModel>(new ProcessAssembliesByProcessIdSqlStatement(processId));
                    AddCacheItem<IEnumerable<ProcessAssemblyDataModel>>(key, result);
                    return result;
                }
            }
        }
        private static object lockGetProcessAssemblyNuGetInfoByProcessId = new object();
        public IEnumerable<ProcessAssemblyNugetInfoDataModel> GetProcessAssemblyNuGetInfoByProcessId(int processId)
        {
            lock (lockGetProcessAssemblyNuGetInfoByProcessId)
            {
                string key = GetKeyById<IEnumerable<ProcessAssemblyNugetInfoDataModel>>(processId, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<ProcessAssemblyNugetInfoDataModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<ProcessAssemblyNugetInfoDataModel>(new ProcessAssemblyNugetInfoByProcessIdSqlStatement(processId));
                    AddCacheItem<IEnumerable<ProcessAssemblyNugetInfoDataModel>>(key, result);
                    return result;
                }
            }
        }
        //pass through
        public string GetWorkflowName(InstanceDataModel instance)
        {
            return GetWorkflow(instance).Name;
        }
        //pass through
        public string GetProcessName(InstanceDataModel instance)
        {
            int processId = GetWorkflow(instance).ProcessID;
            return GetProcessById(processId).Name;
        }

        #region not to be cached

        public InstanceDataModel GetInstanceDataModel(long instanceID)
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.SelectOne<InstanceDataModel>(SAHL.Core.Data.Models.X2.UIStatements.instancedatamodel_selectbykey, new { PrimaryKey = instanceID });
                return result;
            }
        }

        public IEnumerable<ActivatedExternalActivitiesViewModel> GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(int activatingExternalActivityID, long? instanceId)
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.Select<ActivatedExternalActivitiesViewModel>(new ActivatedExternalActivitiesByExternalActivityIDAndInstanceId(activatingExternalActivityID, instanceId));
                return result;
            }
        }

        public IEnumerable<ScheduledActivityDataModel> GetAllScheduledTimerActivities()
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.Select(new GetAllScheduledTimerActivitiesSqlStatement());
                return result;
            }
        }

        public ScheduledActivityDataModel GetScheduledActivity(long instanceId, int activityId)
        {
            using (var db = new Db().InReadOnlyWorkflowContext())
            {
                var result = db.SelectOne(new ScheduledActivityForInstanceAndActivity(instanceId, activityId));
                return result;
            }
        }

        #endregion not to be cached

        public void RemoveScheduledActivity(long instanceId, int activityId)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteWhere<ScheduledActivityDataModel>("InstanceId = @InstanceId and ActivityId = @ActivityId", new { InstanceId = instanceId, ActivityId = activityId });
                db.Complete();
            }
        }

        public void RemoveScheduledActivity(int scheduledActivityId)
        {
            using (var db = new Db().InWorkflowContext())
            {
                db.DeleteByKey<ScheduledActivityDataModel, int>(scheduledActivityId);
                db.Complete();
            }
        }

        private static object lockGetSupportedWorkflows = new object();
        public IEnumerable<ProcessWorkflowViewModel> GetSupportedWorkflows(IEnumerable<string> supportedProcessNames)
        {
            lock (lockGetSupportedWorkflows)
            {
                string inClause = string.Empty;
                foreach (string processName in supportedProcessNames)
                {
                    inClause += "'" + processName + "',";
                }

                inClause = inClause.Remove(inClause.Length - 1, 1);

                string key = GetKeyByName<IEnumerable<ProcessWorkflowViewModel>>(inClause, Context);
                if (cache.Contains(key))
                    return cache.GetItem<IEnumerable<ProcessWorkflowViewModel>>(key);

                using (var db = new Db().InReadOnlyWorkflowContext())
                {
                    var result = db.Select<ProcessWorkflowViewModel>(new SupportedWorkflowsSqlStatement(inClause));
                    AddCacheItem<IEnumerable<ProcessWorkflowViewModel>>(key, result);
                    return result;
                }
            }

        }
    }
}