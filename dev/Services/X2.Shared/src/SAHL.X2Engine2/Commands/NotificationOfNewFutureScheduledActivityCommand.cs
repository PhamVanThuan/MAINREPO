using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Commands
{
    public class NotificationOfNewFutureScheduledActivityCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public int ActivityId { get; protected set; }

        public NotificationOfNewFutureScheduledActivityCommand(long instanceId, int activityId)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
        }
    }
}