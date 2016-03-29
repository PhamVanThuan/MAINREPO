using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data.Models.X2;
using SAHL.UI.Halo.Shared.Models;
using SAHL.UI.Halo.Shared.Configuration.Caching;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public class HaloWorkflowTileActionProvider : IHaloWorkflowTileActionProvider
    {
        private const string X2ProcessCacheName          = "X2.Process";
        private const string X2WorkflowCacheName         = "X2.Workflow";
        private const string X2WorkflowActivityCacheName = "X2.Workflow.Activity";

        private readonly IDbFactory dbFactory;
        private readonly IHaloConfigurationCacheManager cacheManager;

        private const string UiStatementLoadAllProcesses =
@"
SELECT max(ID) as Id, Name 
FROM [X2].[X2].[Process] process
GROUP By Name";

        private const string UiStatementLoadProcess =
@"
SELECT process.* 
FROM [X2].[X2].[Process] process
WHERE process.ID = {0}";

        private const string UiStatementLoadWorkflow =
@"
SELECT	WF.*
FROM	[X2].[X2].[Workflow] WF
JOIN	(SELECT	MAX(PR.[ID]) AS ID, PR.[Name], MAX(PR.[Version]) AS [Version]
         FROM	[X2].[X2].[Process] PR
         GROUP BY PR.[Name]) PR ON WF.[ProcessID] = PR.[ID]
WHERE	[WF].[GenericKeyTypeKey] = {0}";

        private const string UiStatementLoadWorkflowActivities =
@"
SELECT	AC.*
FROM	[X2].[X2].[Activity] AC
JOIN	(SELECT	WF.*
         FROM	[X2].[X2].[Workflow] WF
         JOIN	(SELECT	MAX(PR.[ID]) AS ID, PR.[Name], MAX(PR.[Version]) AS [Version]
                 FROM	[X2].[X2].[Process] PR
                 GROUP BY PR.[Name]) PR ON WF.[ProcessID] = PR.[ID]
                 WHERE	[WF].[GenericKeyTypeKey] = {0}) Workflow ON AC.[WorkFlowID] = Workflow.ID";

        private const string UiStatementLoadInstanceId =
@"
SELECT  [InstanceId]
FROM    [X2].[X2Data].{0}
WHERE   [GenericKey] = {1}";

        private const string UiStatementLoadActivityForUserAndCapabilities =
@"
SELECT Distinct sec.*
    FROM    [X2].[X2].[InstanceActivitySecurity] sec	
    WHERE sec.InstanceId = {0}
    and sec.ADUserName in ({1})";

        public HaloWorkflowTileActionProvider(IDbFactory dbFactory, IHaloConfigurationCacheManager cacheManager)
        {
            if (dbFactory == null) { throw new ArgumentNullException("dbFactory"); }
            if (cacheManager == null) { throw new ArgumentNullException("cacheManager"); }

            this.dbFactory = dbFactory;
            this.cacheManager = cacheManager;
        }

        public IEnumerable<IHaloWorkflowAction> GetTileActions(BusinessContext businessContext, string roleName, string[] capabilities)
        {
            if (businessContext == null) { throw new ArgumentNullException("businessContext"); }

            if (!this.LoadX2CacheItems(businessContext)) { return null; }

            var instanceActivities = this.RetrieveWorkflowInstances(businessContext, roleName, capabilities);
            if (instanceActivities == null) { return null; }

            var tileActions = new List<IHaloWorkflowAction>();
            var sequence = 1;

            foreach (var instanceActivity in instanceActivities)
            {
                var workflowAction = this.CreateHaloWorkflowAction(businessContext, instanceActivity, sequence);
                if (workflowAction == null) { continue; }

                if (tileActions.Any(x => x.Name == workflowAction.Name && x.InstanceId == workflowAction.InstanceId)) { continue; }

                tileActions.Add(workflowAction);
                sequence++;
            }

            return tileActions;
        }

        public virtual string GetUiStatementToLoadWorkflow(BusinessContext businessContext)
        {
            return string.Format(UiStatementLoadWorkflow, Convert.ToInt32(businessContext.BusinessKey.KeyType));
        }

        public virtual string GetUiStatementToLoadWorkflowActivities(BusinessContext businessContext)
        {
            return string.Format(UiStatementLoadWorkflowActivities, Convert.ToInt32(businessContext.BusinessKey.KeyType));
        }

        public virtual string GetUiStatementToLoadInstanceIds(string dataTable, BusinessContext businessContext)
        {
            return string.Format(UiStatementLoadInstanceId, dataTable, Convert.ToInt32(businessContext.BusinessKey.Key));
        }

        public virtual string GetUiStatementToLoadInstanceActivities(long instanceId, string capabilities)
        {
            return string.Format(UiStatementLoadActivityForUserAndCapabilities, instanceId, capabilities);
        }

        private bool LoadX2CacheItems(BusinessContext businessContext)
        {
            var allProcesses = this.RetrieveProcesses();
            if (allProcesses == null || !allProcesses.Any()) { return false; }

            var allWorkflows = this.RetrieveWorkFlows(businessContext);
            if (allWorkflows == null || !allWorkflows.Any()) { return false; }

            var workFlowActivities = this.RetrieveWorkFlowActivities(businessContext);
            return workFlowActivities != null && workFlowActivities.Any();
        }

        private HaloWorkflowAction CreateHaloWorkflowAction(BusinessContext businessContext, InstanceActivitySecurityDataModel instanceActivity, int sequence)
        {
            var activityDataModel = this.cacheManager.Find(this.WorkflowActivitiesCacheName(businessContext), instanceActivity.ActivityID.ToString());
            if (activityDataModel == null) { return null; }

            var workFlowDataModel = this.cacheManager.Find(this.WorkflowCacheName(businessContext), activityDataModel.WorkFlowID.ToString());
            if (workFlowDataModel == null) { return null; }

            var process = this.cacheManager.Find(HaloWorkflowTileActionProvider.X2ProcessCacheName, workFlowDataModel.ProcessID.ToString());
            if (process == null) { return null; }

            var workflowAction = new HaloWorkflowAction(activityDataModel.Name, "icon-spin", "Workflow", sequence,
                                                        process.Name, workFlowDataModel.Name, instanceActivity.InstanceID);
            return workflowAction;
        }

        private IEnumerable<InstanceActivitySecurityDataModel> RetrieveWorkflowInstances(BusinessContext businessContext, 
                                                                                         string roleName, string[] capabilities)
        {
            if (capabilities.Length == 0) { return null; }

            var allWorkflows = this.RetrieveWorkFlows(businessContext);
            if (allWorkflows == null) { return null; }

            var instanceIds = this.RetrieveInstanceIds(allWorkflows, businessContext);
            if (instanceIds == null || !instanceIds.Any()) { return null; }

            var instanceActivities = this.RetrieveInstanceActivitiesForUserCapabilities(instanceIds,
                                                                                        string.Format("{0}, '{1}'", capabilities.Select(x => string.Format("'{0}'", x)).Aggregate((c,n)=>c+", "+n), roleName));
            return instanceActivities;
        }

        private IEnumerable<X2ProcessInfoModel> RetrieveProcesses()
        {
            var allProcesses = this.cacheManager.FindAll(HaloWorkflowTileActionProvider.X2ProcessCacheName);
            if (allProcesses == null || !allProcesses.Any())
            {
                allProcesses = this.LoadX2ProcessesFromDatabaseIntoCacheManager(HaloWorkflowTileActionProvider.X2ProcessCacheName);
            }

            return allProcesses == null || !allProcesses.Any()
                        ? null
                        : allProcesses.Cast<X2ProcessInfoModel>().ToList();
        }

        private IEnumerable<X2ProcessInfoModel> LoadX2ProcessesFromDatabaseIntoCacheManager(string cacheName)
        {
            try
            {
                using (var db = this.dbFactory.NewDb().InWorkflowContext())
                {
                    var processDataModels = db.Select<X2ProcessInfoModel>(HaloWorkflowTileActionProvider.UiStatementLoadAllProcesses);
                    if (!processDataModels.Any()) { return null; }

                    var cacheItems = processDataModels.ToDictionary<X2ProcessInfoModel, string, dynamic>(processDataModel => processDataModel.ID.ToString(),
                                                                                                       processDataModel => processDataModel);
                    this.cacheManager.AddRange(cacheName, cacheItems);
                    return processDataModels;
                }
            }
            catch (Exception runtimeException)
            {
                var errorMessage = string.Format("WorkflowTileActionProvider.LoadX2ProcessesFromDatabaseIntoCacheManager failed\n{0}", runtimeException);
                Trace.WriteLine(errorMessage);
            }

            return null;
        }

        private IEnumerable<WorkFlowDataModel> RetrieveWorkFlows(BusinessContext businessContext)
        {
            var cacheName = this.WorkflowCacheName(businessContext);

            var allWorkflows = this.cacheManager.FindAll(cacheName);
            if (allWorkflows == null || !allWorkflows.Any())
            {
                allWorkflows = this.LoadX2WorkflowsFromDatabaseIntoCacheManager(businessContext, cacheName);
            }

            return allWorkflows == null || !allWorkflows.Any()
                        ? null
                        : allWorkflows.Cast<WorkFlowDataModel>().ToList();
        }

        private string WorkflowCacheName(BusinessContext businessContext)
        {
            return string.Format("{0}.{1}", HaloWorkflowTileActionProvider.X2WorkflowCacheName, businessContext.BusinessKey.KeyType);
        }

        private IEnumerable<WorkFlowDataModel> LoadX2WorkflowsFromDatabaseIntoCacheManager(BusinessContext businessContext, string cacheName)
        {
            var sqlStatement = this.GetUiStatementToLoadWorkflow(businessContext);

            try
            {
                using (var db = this.dbFactory.NewDb().InWorkflowContext())
                {
                    var workFlowDataModels = db.Select<WorkFlowDataModel>(sqlStatement);
                    if (!workFlowDataModels.Any()) { return null; }

                    var cacheItems = workFlowDataModels.ToDictionary<WorkFlowDataModel, string, dynamic>(workFlowDataModel => workFlowDataModel.ID.ToString(),
                                                                                                         workFlowDataModel => workFlowDataModel);
                    this.cacheManager.AddRange(cacheName, cacheItems);

                    return workFlowDataModels;
                }
            }
            catch (Exception runtimeException)
            {
                var errorMessage = string.Format("WorkflowTileActionProvider.LoadX2WorkflowsFromDatabaseIntoCacheManager failed\n{0}", runtimeException);
                Trace.WriteLine(errorMessage);
            }

            return null;
        }

        private IEnumerable<ActivityDataModel> RetrieveWorkFlowActivities(BusinessContext businessContext)
        {
            var cacheName = this.WorkflowActivitiesCacheName(businessContext);

            var allWorkflowActivities = this.cacheManager.FindAll(cacheName);
            if (allWorkflowActivities == null || !allWorkflowActivities.Any())
            {
                allWorkflowActivities = this.LoadX2WorkflowActivitiesFromDatabaseIntoCacheManager(businessContext, cacheName);
            }

            return allWorkflowActivities == null || !allWorkflowActivities.Any()
                        ? null
                        : allWorkflowActivities.Cast<ActivityDataModel>().ToList();
        }

        private string WorkflowActivitiesCacheName(BusinessContext businessContext)
        {
            return string.Format("{0}.{1}", HaloWorkflowTileActionProvider.X2WorkflowActivityCacheName, businessContext.BusinessKey.KeyType);
        }

        private IEnumerable<ActivityDataModel> LoadX2WorkflowActivitiesFromDatabaseIntoCacheManager(BusinessContext businessContext, string cacheName)
        {
            var sqlStatement = this.GetUiStatementToLoadWorkflowActivities(businessContext);

            try
            {
                using (var db = this.dbFactory.NewDb().InWorkflowContext())
                {
                    var activityDataModels = db.Select<ActivityDataModel>(sqlStatement);
                    if (!activityDataModels.Any()) { return null; }

                    var cacheItems = activityDataModels.ToDictionary<ActivityDataModel, string, dynamic>(dataModel => dataModel.ID.ToString(),
                                                                                                         dataModel => dataModel);
                    this.cacheManager.AddRange(cacheName, cacheItems);

                    return activityDataModels;
                }
            }
            catch (Exception runtimeException)
            {
                var errorMessage = string.Format("WorkflowTileActionProvider.LoadX2WorkflowsFromDatabaseIntoCacheManager failed\n{0}", runtimeException);
                Trace.WriteLine(errorMessage);
            }

            return null;
        }

        private IEnumerable<long> RetrieveInstanceIds(IEnumerable<WorkFlowDataModel> allWorkflows, BusinessContext businessContext)
        {
            var allInstanceIds = new List<long>();

            foreach (var workFlowDataModel in allWorkflows)
            {
                var sqlStatement = this.GetUiStatementToLoadInstanceIds(workFlowDataModel.StorageTable, businessContext);

                try
                {
                    using (var db = this.dbFactory.NewDb().InWorkflowContext())
                    {
                        var dataTableResults = db.Select<long>(sqlStatement);
                        if (!dataTableResults.Any()) { continue; }

                        allInstanceIds.AddRange(dataTableResults);
                    }
                }
                catch (Exception runtimeException)
                {
                    var errorMessage = string.Format("WorkflowTileActionProvider.RetrieveInstanceIds failed\n{0}", runtimeException);
                    Trace.WriteLine(errorMessage);
                }
            }

            return allInstanceIds;
        }

        private IEnumerable<InstanceActivitySecurityDataModel> RetrieveInstanceActivitiesForUserCapabilities(IEnumerable<long> allInstanceIds, string capabilities)
        {
            var allInstanceActivities = new List<InstanceActivitySecurityDataModel>();

            foreach (var instanceId in allInstanceIds)
            {
                var sqlStatement = this.GetUiStatementToLoadInstanceActivities(instanceId, capabilities);

                try
                {
                    using (var db = this.dbFactory.NewDb().InWorkflowContext())
                    {
                        var dataTableResults = db.Select<InstanceActivitySecurityDataModel>(sqlStatement);
                        if (!dataTableResults.Any()) { continue; }

                        allInstanceActivities.AddRange(dataTableResults);
                    }
                }
                catch (Exception runtimeException)
                {
                    var errorMessage = string.Format("WorkflowTileActionProvider.RetrieveInstanceActivitiesForUserCapabilities failed\n{0}", runtimeException);
                    Trace.WriteLine(errorMessage);
                }
            }

            return allInstanceActivities;
        }
    }
}
