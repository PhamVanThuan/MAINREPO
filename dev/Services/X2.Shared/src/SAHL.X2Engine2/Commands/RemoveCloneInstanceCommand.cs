using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class RemoveCloneInstanceCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public RemoveCloneInstanceCommand(long instanceId)
        {
            this.InstanceId = instanceId;
        }
    }
}