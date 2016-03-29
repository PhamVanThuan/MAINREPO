using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class NotificationOfNewScheduledActivityCommand : IX2BundledNotificationCommand
    {
        public long InstanceId { get; protected set; }

        public int ActivityId { get; protected set; }


        public NotificationOfNewScheduledActivityCommand(long instanceId, int activityId)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
            this.CommandType = X2BundledNotificationCommandType.ScheduledActivity;
        }

        public X2BundledNotificationCommandType CommandType
        {
            get;
            protected set;
        }
    }
}