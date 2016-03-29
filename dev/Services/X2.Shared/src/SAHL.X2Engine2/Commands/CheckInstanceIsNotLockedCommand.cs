using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class CheckInstanceIsNotLockedCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public int ActivityId { get; protected set; }

        public string UserName { get; protected set; }

        public bool Result { get; set; }

        public CheckInstanceIsNotLockedCommand(long instanceId, int activityId, string userName)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
            this.UserName = userName;
        }
    }
}