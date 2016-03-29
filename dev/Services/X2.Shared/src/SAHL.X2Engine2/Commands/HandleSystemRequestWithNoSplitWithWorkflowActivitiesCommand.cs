using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand : HandleSystemRequestBaseCommand, IContinueWithCommands
    {
        public HandleSystemRequestWithNoSplitWithWorkflowActivitiesCommand(long instanceId, WorkflowActivity workFlowActivity, string userName, string workflowProviderName)
            : base(instanceId, null, userName)
        {
            this.WorkflowProviderName = workflowProviderName;
            this.WorkFlowActivity = workFlowActivity;
        }

        public WorkflowActivity WorkFlowActivity { get; set; }

        public string WorkflowProviderName { get; set; }
    }
}