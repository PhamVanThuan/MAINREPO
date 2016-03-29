using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class DeleteInstanceCommand : ServiceCommand
    {
        public long InstanceID { get; protected set; }

        public DeleteInstanceCommand(long instanceID)
        {
            this.InstanceID = instanceID;
        }
    }
}