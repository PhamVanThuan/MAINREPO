using SAHL.Core.IoC;
using SAHL.Core.Messaging.Shared;

namespace SAHL.Services.DomainProcessManager.Services
{
    public interface IDomainProcessEventHandlerService : IStartable, IStoppable
    {
        void Handle<T>(T domainProcessEvent) where T : class, IMessage;
    }
}