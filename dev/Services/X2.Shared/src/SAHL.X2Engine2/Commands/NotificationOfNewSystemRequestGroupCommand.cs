using System.Collections.Generic;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class NotificationOfNewSystemRequestGroupCommand : IX2BundledNotificationCommand
    {
        public List<string> ActivityNames { get; protected set; }

        public long InstanceId { get; protected set; }

        public NotificationOfNewSystemRequestGroupCommand(List<string> activityNames, long instanceId)
        {
            this.ActivityNames = activityNames;
            this.InstanceId = instanceId;
            this.CommandType = X2BundledNotificationCommandType.SystemRequestGroup;
        }

        public X2BundledNotificationCommandType CommandType
        {
            get;
            protected set;
        }
    }
}