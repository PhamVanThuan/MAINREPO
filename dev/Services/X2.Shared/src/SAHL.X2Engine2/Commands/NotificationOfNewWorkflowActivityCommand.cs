using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class NotificationOfNewWorkflowActivityCommand : IX2BundledNotificationCommand
    {
        public long InstanceId { get; protected set; }

        public int WorkflowActivityId { get; protected set; }

        public NotificationOfNewWorkflowActivityCommand(long instanceId, int workflowActivityId)
        {
            this.InstanceId = instanceId;
            this.WorkflowActivityId = workflowActivityId;
            this.CommandType = X2BundledNotificationCommandType.WorkflowActivity;
        }

        public X2BundledNotificationCommandType CommandType
        {
            get;
            protected set;
        }
    }
}