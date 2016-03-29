using System;

using SAHL.Core.IoC;
using SAHL.Core.DomainProcess;
using SAHL.Core.Messaging.Shared;

namespace SAHL.Services.DomainProcessManager.Services
{
    public interface IDomainProcessCoordinatorService : IStartable, IStoppable
    {
        void AddDomainProcess(IDomainProcess domainProcess);
        IDomainProcess FindDomainProcess(Guid domainProcessId);
        void HandleEvent<T>(T domainProcessEvent) where T : class, IMessage;
    }
}
