namespace SAHL.X2Engine2.Commands
{
    public class CheckInstanceIsLockedForUserCommand : RuleCommand
    {
        public long InstanceId { get; protected set; }

        public int ActivityId { get; protected set; }

        public string UserName { get; protected set; }

        public CheckInstanceIsLockedForUserCommand(long instanceId, int activityId, string userName)
        {
            this.InstanceId = instanceId;
            this.ActivityId = activityId;
            this.UserName = userName;
        }
    }
}