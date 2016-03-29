using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class ClearWorkListCommand : ServiceCommand
    {
        public long InstanceID { get; protected set; }

        public ClearWorkListCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }
    }
}