using SAHL.Core.Services;

namespace SAHL.Core.Events.Projections
{
    public interface IServiceProjector<TEvent, TService> : IEventProjector
        where TEvent : IEvent
        where TService : IServiceClient
    {
        void Handle(TEvent @event, IServiceRequestMetadata metadata, TService service);
    }
}