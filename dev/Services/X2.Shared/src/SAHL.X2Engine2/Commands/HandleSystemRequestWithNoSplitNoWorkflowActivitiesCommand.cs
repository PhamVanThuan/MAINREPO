using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand : HandleSystemRequestBaseCommand, IContinueWithCommands
    {
        public HandleSystemRequestWithNoSplitNoWorkflowActivitiesCommand(long instanceId, Activity activity, string userName)
            : base(instanceId, activity, userName)
        { }
    }
}