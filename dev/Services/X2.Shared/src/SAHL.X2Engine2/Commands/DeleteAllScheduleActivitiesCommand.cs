using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class DeleteAllScheduleActivitiesCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public DeleteAllScheduleActivitiesCommand(long instanceId)
        {
            this.InstanceId = instanceId;
        }
    }
}