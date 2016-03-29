using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public class X2Data
    {
        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public DataTable Data { get; set; }
    }

    public interface IX2Repository
    {
        IList<IWorkflowRole> GetWorkflowRoleForGenericKey(string adUserName, int workflowRoleTypeGroupKey, int generalStatusKey);

        bool IsValuationApprovalRequired(long instanceID);

        X2Data GetX2DataForInstance(IInstance instance);

        IState GetStateByName(string workflow, string process, string state);

        IState GetStateByKey(int StateID);

        IInstance GetInstanceByKey(long InstanceID);

        IEventList<IInstance> GetInstanceForSourceInstanceID(Int64 SourceInstanceID);

        IEventList<IInstance> GetInstanceByPrincipal(SAHLPrincipal principal);

        IWorkFlow GetWorkFlowByKey(int WorkFlowID);

        IWorkFlow GetWorkFlowByName(string WorkFlowName, string ProcessName);

        IEventList<IWorkList> GetWorkListByState(SAHLPrincipal principal, long StateID);

        IEventList<IWatchListConfiguration> GetWatchListConfigurationByWorkFlowName(string WorkFlowName);

        IWatchListConfiguration GetWatchListConfiguration(string ProcessName, string WorkFlowName);

        IEventList<IDataGridConfiguration> GetDataGridConfigurationByStatementName(string StatementName);

        IEventList<IDataGridConfiguration> GetDataGridConfigurationByWorkFlowName(string WorkFlowName, string ProcessName);

        IUIStatement GetUIStatement(string StatementName, string ApplicationName);

        /// <summary>
        /// Gets the complete list of UIStatements for the max version.
        /// </summary>
        /// <returns></returns>
        IEventList<IUIStatement> GetAllUIStatement();

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IUIStatement GetUIStatment(int Key);

        /// <summary>
        /// Gets the latest external activity ID from a specified workflow where the name is EXTCreateApplication
        /// </summary>
        /// <param name="WorkFlowName"></param>
        /// <param name="ExternalName"></param>
        /// <returns></returns>
        int GetLatestExternalActivityIDFromWorkFlow(string WorkFlowName, string ExternalName);

        /// <summary>
        /// Returns an empty ActiveExternalActivity.
        /// </summary>
        IActiveExternalActivity GetEmptyActiveExternalActivity();

        /// <summary>
        /// Returns an External Activity object given its Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IExternalActivity GetExternalActivityByKey(int key);

        /// <summary>
        /// Creates an empty IActiveExternalActivity
        /// </summary>
        /// <returns></returns>
        IActiveExternalActivity CreateActiveExternalActivity();

        /// <summary>
        /// saves an IActiveExternalActivity that will be picked up and executed by the engine.
        /// </summary>
        /// <param name="activity"></param>
        void SaveActiveExternalActivity(IActiveExternalActivity activity);

        /// <summary>
        /// Gets an Iexternalactivity for a given name on a given workflow.
        /// </summary>
        /// <param name="ExternalActivityName"></param>
        /// <param name="WorkflowID"></param>
        /// <returns></returns>
        IExternalActivity GetExternalActivityByName(string ExternalActivityName, int WorkflowID);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="WorkflowName"></param>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        IInstance GetInstanceForGenericKey(int GenericKey, string WorkflowName, string ProcessName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="WorkflowName"></param>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        IInstance GetLatestInstanceForGenericKey(int GenericKey, string WorkflowName, string ProcessName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        IList<IInstance> GetInstancesForGenericKey(int GenericKey, string ProcessName);

        IActivity GetActivityForName(string Name);

        IEventList<IActivity> GetActivitiesForName(string Name);

        IWorkFlowHistory GetHistoryForInstanceAndActivity(IInstance ID, IActivity activity);

        /// <summary>
        /// Searches workflows within the specified criteria.
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        IList<IInstance> SuperSearchWorkflow(IWorkflowSearchCriteria SearchCriteria);

        /// <summary>
        /// Gets the list of searchable workflows available to a user by organisation structure.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        IEventList<IWorkFlow> GetSearchableWorkflowsForUser(SAHLPrincipal principal);

        /// <summary>
        /// Updates the name on the specified instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="instanceSubject"></param>
        void UpdateInstanceSubject(IInstance instance, string instanceSubject);

        /// <summary>
        /// Updates the name on the specified applicationKey
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="instanceSubject"></param>
        void UpdateInstanceSubject(int applicationKey, string instanceSubject);

        IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstance(IInstance instance);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstanceAndActivity(long instanceID, string activityName);

        IEventList<IWorkFlowHistory> GetWorkflowHistoryForInstanceAndActivity(IInstance instance, IActivity activity);

        IEventList<IInstance> GetChildInstances(Int64 ParentInstanceID);

        string GetUserWhoPerformedActivity(Int64 InstanceID, string ActivityName);

        /// <summary>
        /// This method checks if an instance can be assigned to a user.
        /// </summary>
        /// <param name="Instance">The instance that might be assigned</param>
        /// <param name="ADUser">The ADUser that might get the instance assigned.</param>
        /// <returns>True if the Instance can be assigned.</returns>
        bool CanInstanceBeAssignedToUser(IInstance Instance, IADUser ADUser);

        /// <summary>
        /// Searches workflows for archived instances within the specified criteria.
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        IList<IInstance> WorkflowArchiveSuperSearch(IWorkflowSearchCriteria SearchCriteria);

        /// <summary>
        /// Gets the list of archive workflows.
        /// </summary>
        /// <param name="archiveSuperSearchWorkflows"></param>
        /// <returns></returns>
        IEventList<IWorkFlow> GetArchiveSearchWorkflows(string archiveSuperSearchWorkflows);

        /// <summary>
        /// Uses the super search but does additional work.
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <param name="AdditionalCriteria"></param>
        /// <returns></returns>
        Dictionary<IInstance, IApplication> WorkflowSearch(IWorkflowSearchCriteria SearchCriteria, Hashtable AdditionalCriteria);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="instance"></param>
        /// <param name="businessKey"></param>
        /// <param name="nodeDesc"></param>
        /// <param name="longDesc"></param>
        void SetInstanceNodeDescription(IWorkFlow workflow, IInstance instance, int businessKey, out string nodeDesc, out string longDesc);

        /// <summary>
        /// Gets an <see cref="IWorkflowRoleType"/> according to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IWorkflowRoleType GetWorkflowRoleTypeByKey(int key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IList<IInstance> GetInstancesForWorkflowRoleTypeAndUser(int workflowRoleTypeKey, string ADUserName);

        /// <summary>
        /// Gets the list of instances on an adusers current worklist.
        /// </summary>
        /// <param name="offerRoleType"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IList<IInstance> GetInstancesOnWorkListForOfferRoleTypeAndUser(int offerRoleType, string ADUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="ADUserName"></param>
        /// <returns></returns>
        IDictionary<int, int> GetInstanceCountForWorkflowRoleTypeAndUser(int workflowRoleTypeKey, string ADUserName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="workflowRoleTypeKey"></param>
        /// <returns></returns>
        IDictionary<int, int> GetInstanceCountForWorkflowRoleTypeAndUser(int workflowRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="adUserName"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="genericKey"></param>
        /// <param name="message"></param>
        void AssignWorkflowRoleForADUser(Int64 instanceID, string adUserName, int workflowRoleTypeKey, int genericKey, string message);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <returns></returns>
        IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <returns></returns>
        IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey, int workflowRoleTypeKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="generalStatusKey"></param>
        /// <returns></returns>
        IList<IWorkflowRole> GetWorkflowRoleForGenericKey(int genericKey, int workflowRoleTypeKey, int generalStatusKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="workflowRoleTypeKey"></param>
        /// <param name="generalStatusKey"></param>
        /// <returns></returns>
        IList<IWorkflowRole> GetWorkflowRoleForLegalEntityKey(int legalEntityKey, int workflowRoleTypeKey, int generalStatusKey);

        /// <summary>
        /// Intentionally not returning business model objects so there is no risk of consumers
        /// walking through the domain
        /// This is X2 data
        /// </summary>
        /// <param name="iList"></param>
        /// <returns></returns>
        DataTable GetScheduledActivities(DataTable iList);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetRelatedInstances(Int64 id);

        /// <summary>
        /// Inserts a ercord into the X2.ActiveExternalActivity Table. This will be picked up by the engine and executed. Used to
        /// create cases in remote workflows (Submit applicaiton in App Man an example) or to pickup a case and move it within
        /// a workflow (App Man. Clone case sits with a 15 day timer. The parent raises an EXT activity to archive the child)
        /// </summary>
        /// <param name="ExtActivityName"></param>
        /// <param name="ActivatingInstanceID"></param>
        /// <param name="workflowName"></param>
        /// <param name="processName"></param>
        /// <param name="XMLFieldInputs"></param>
        void CreateAndSaveActiveExternalActivity(string ExtActivityName, Int64 ActivatingInstanceID, string workflowName, string processName, string XMLFieldInputs);

        /// <summary>
        /// We need to find the parent instance / main instance for the open status Debt Counselling case
        /// of the related the account
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        IInstance GetDebtCousellingInstanceByAccountKey(int accountKey);

        /// <summary>
        /// Get Search Setup
        /// </summary>
        /// <returns></returns>
        IList<ISetup> GetSearchSetups();

        /// <summary>
        /// Get Search Contexts
        /// </summary>
        /// <returns></returns>
        IList<IContext> GetSearchContexts();

        /// <summary>
        /// Get Search Internal Role
        /// </summary>
        /// <returns></returns>
        IList<IInternalRole> GetSearchInternalRoles();

        /// <summary>
        /// Get Search Workflow Contexts
        /// </summary>
        /// <returns></returns>
        IList<IWorkflowContext> GetSearchWorkflowContexts();

        /// <summary>
        /// Get Search Workflow Datas
        /// </summary>
        /// <returns></returns>
        IList<IWorkflowData> GetSearchWorkflowDatas();

        /// <summary>
        /// Get Search Filters
        /// </summary>
        /// <returns></returns>
        IList<BusinessModel.Interfaces.IFilter> GetSearchFilters();

        /// <summary>
        /// Get Search Selects
        /// </summary>
        /// <returns></returns>
        IList<ISearchSelect> GetSearchSelects();

        /// <summary>
        /// Get Search Cleanups
        /// </summary>
        /// <returns></returns>
        IList<ICleanup> GetSearchCleanups();

        /// <summary>
        ///
        /// </summary>
        /// <param name="InstanceID"></param>
        /// <returns></returns>
        DataTable GetCurrentConsultantAndAdmin(Int64 InstanceID);

        string GetCurrentConsultantEmailAddress(Int64 InstanceID);

        string GetEmailAddressForCaseOwner(long instanceID);

        bool HasRelatedSourceInstancesInWorkflow(Int64 instanceID, string workflow);

        string GetPersonalLoansInstanceSubject(int applicationKey);

        string GetDebtCounsellingInstanceSubject(int debtCounsellingKey);

        void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data);

        IDictionary<string, object> GetX2DataRow(long instanceID);

        bool HasInstancePerformedActivity(Int64 instanceID, string activityName);

        string GetPreviousStateName(long instanceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        DataRow GetApplicationManagementDataForApplicationKey(int applicationKey);

        /// <summary>
        /// Rturns a list of processes (latest version) which contain a specified user activity 
        /// </summary>
        /// <param name="userActivityName"></param>
        /// <returns></returns>
        IList<IProcess> GetProcessesByUserActivity(string userActivityName);

        IList<IInstance> GetFLInstancesForAccountAtState(int accountKey, string stateName);
    }
}