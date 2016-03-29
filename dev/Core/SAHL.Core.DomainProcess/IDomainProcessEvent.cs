using SAHL.Core.Events;
using SAHL.Core.Services;

namespace SAHL.Core.DomainProcess
{
    public interface IDomainProcessEvent<TEvent> where TEvent : class, IEvent
    {
        void Handle(TEvent @event, IServiceRequestMetadata metadata);
    }
}