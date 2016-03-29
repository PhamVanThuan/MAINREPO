using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand : HandleSystemRequestBaseCommand, ISplittable, IContinueWithCommands
    {
        public long NewlyCreatedInstanceId { get; set; }

        public string WorkflowProviderName { get; protected set; }

        public HandleSystemRequestWithSplitWithNoWorkflowActivitiesCommand(long InstanceId, Activity activity, string userName, string workflowProviderName, object data = null)
            : base(InstanceId, activity, userName, data)
        {
            this.WorkflowProviderName = workflowProviderName;
            this.IgnoreWarnings = IgnoreWarnings;
        }
    }
}