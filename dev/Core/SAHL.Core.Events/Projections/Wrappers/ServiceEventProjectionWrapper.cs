using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Projections.Wrappers
{
    public class ServiceEventProjectionWrapper<TEvent, TService>
        : IProjectionHandleWrapper<TEvent, IServiceProjector<TEvent, TService>>
        where TEvent : class,IEvent
        where TService : IServiceClient
    {
        IIocContainer container;
        Action<TEvent, IServiceRequestMetadata, TService> actionToWrap;

        public ServiceEventProjectionWrapper(IIocContainer container, Action<TEvent, IServiceRequestMetadata, TService> actionToWrap)
        {
            this.container = container;
            this.actionToWrap = actionToWrap;
        }

        public void Handle(WrappedEvent<TEvent> @event)
        {
            TService service = container.GetInstance<TService>();
            actionToWrap(@event.InternalEvent, @event.ServiceRequestMetadata, service);
        }
    }
}