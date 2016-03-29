using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class HandleSystemRequestThatIsAnAutoForwardCommand : ServiceCommand, IContinueWithCommands
    {
        public long InstanceId { get; protected set; }

        public string UserName { get; protected set; }

        public bool Result { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public HandleSystemRequestThatIsAnAutoForwardCommand(long InstanceId, string userName)
        {
            this.InstanceId = InstanceId;
            this.UserName = userName;
            this.IgnoreWarnings = false;
        }
    }
}