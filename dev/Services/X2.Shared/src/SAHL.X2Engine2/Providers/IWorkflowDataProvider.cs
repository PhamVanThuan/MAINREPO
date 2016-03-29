using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.X2;
using SAHL.X2Engine2.ViewModels;
using System.Collections.Generic;

namespace SAHL.X2Engine2.Providers
{
    public interface IWorkflowDataProvider
    {
        ActivityDataModel GetActivityByActivatingExternalActivity(int externalActivityId, int currentStateIdForInstance);

        InstanceDataModel GetInstanceDataModel(long instanceID);

        IEnumerable<StateWorkListDataModel> GetStateWorkList(int stateID);

        SecurityGroupDataModel GetSecurityGroup(int securityGroupID);

        IEnumerable<ActivityDataModel> GetSystemActivitiesForState(int stateId);

        IEnumerable<WorkFlowActivityDataModel> GetWorkflowActivitiesForState(int stateId);

        StateDataModel GetStateById(int stateId);

        Activity GetActivityByNameAndWorkflowName(string activityName, string workflowName);

        IEnumerable<ActivityDataModel> GetUserActivitiesForState(int stateId);

        IEnumerable<ActivitySecurityDataModel> GetActivitySecurityForActivity(int activityId);

        WorkFlowDataModel GetWorkflow(InstanceDataModel instance);

        string GetWorkflowName(InstanceDataModel instance);

        string GetProcessName(InstanceDataModel instance);

        IEnumerable<StageActivityDataModel> GetStageActivities(int activityId);

        ADUserDataModel GetADUser(string userName);

        ActivityDataModel GetActivity(int activityId);

        Activity GetActivityById(int activityId);

        Activity GetWorkflowVersionedActivity(long instanceId, string activityName);

        Activity GetActivityForInstanceAndName(long instanceId, string activityName);

        WorkFlowDataModel GetWorkflowById(int workflowId);

        ProcessDataModel GetProcessById(int processId);

        IEnumerable<ScheduledActivityDataModel> GetAllScheduledTimerActivities();

        ScheduledActivityDataModel GetScheduledActivity(long instanceId, int activityId);

        WorkFlowActivityDataModel GetWorkflowActivityDataModelById(int workflowActivityId);

        StateDataModel GetStateDataModel(string autoforwardStateName, string sourceWorkflowName);

        WorkflowActivity GetWorkflowActivityById(int workflowActivityId);

        void RemoveScheduledActivity(long instanceId, int activityId);

        void RemoveScheduledActivity(int scheduledActivityId);

        IEnumerable<ProcessViewModel> GetConfiguredProcesses(IEnumerable<string> configuredProcessNames);

        IEnumerable<ProcessAssemblyDataModel> GetProcessAssemblies(int processId);

        IEnumerable<ProcessAssemblyNugetInfoDataModel> GetProcessAssemblyNuGetInfoByProcessId(int processId);

        IEnumerable<ProcessWorkflowViewModel> GetSupportedWorkflows(IEnumerable<string> supportedProcessNames);

        IEnumerable<ActivatedExternalActivitiesViewModel> GetActivatedExternalActivitiesViewModelByExternalActivityIDandInstanceID(int activatingExternalActivityID, long? instanceId);
    }
}