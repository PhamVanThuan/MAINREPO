using System;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class RebuildInstanceCommand : ServiceCommand
    {
        public long InstanceId { get; protected set; }

        public RebuildInstanceCommand(long instanceId) : base(Guid.NewGuid())
        {
            this.InstanceId = instanceId;
        }
    }
}