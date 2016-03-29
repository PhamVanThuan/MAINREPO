using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class DeleteScheduledActivityCommand : ServiceCommand
    {
        public long InstanceId { get; set; }

        public int ActivityId { get; set; }

        public DeleteScheduledActivityCommand(long instanceId, int activityId)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
        }
    }
}