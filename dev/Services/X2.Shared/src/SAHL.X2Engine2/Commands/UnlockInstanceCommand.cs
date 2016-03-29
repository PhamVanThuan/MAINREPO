using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class UnlockInstanceCommand : ServiceCommand
    {
        public long InstanceID { get; protected set; }

        public UnlockInstanceCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }
    }
}