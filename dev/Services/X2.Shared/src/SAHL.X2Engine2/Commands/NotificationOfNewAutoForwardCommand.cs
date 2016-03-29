using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class NotificationOfNewAutoForwardCommand : IX2BundledNotificationCommand
    {
        public long InstanceId { get; set; }

        public NotificationOfNewAutoForwardCommand(long instanceId)
        {
            this.InstanceId = instanceId;
            this.CommandType = X2BundledNotificationCommandType.AutoForward;
        }

        public X2BundledNotificationCommandType CommandType
        {
            get;
            protected set;
        }
    }
}