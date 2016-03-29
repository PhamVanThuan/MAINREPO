using SAHL.Core.Identity;
using System;

namespace SAHL.Core.Services
{
    public abstract class ServiceCommand : IServiceCommand
    {
        public ServiceCommand()
        {
            this.Id = CombGuid.Instance.Generate();
        }

        public ServiceCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; protected set; }
    }
}