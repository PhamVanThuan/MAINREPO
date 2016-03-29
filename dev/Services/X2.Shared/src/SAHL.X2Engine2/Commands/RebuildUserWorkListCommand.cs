using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class RebuildUserWorkListCommand : ServiceCommand
    {
        public long InstanceID { get; protected set; }

        public string ActivityMessage { get; set; }

        public RebuildUserWorkListCommand(long instanceID, string activityMessage)
        {
            this.InstanceID = instanceID;
            this.ActivityMessage = activityMessage;
        }
    }
}