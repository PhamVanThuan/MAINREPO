using SAHL.Core.Data;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Projections.Wrappers
{
    public class TableEventProjectionWrapper<TEvent, TModel>
        : IProjectionHandleWrapper<TEvent, ITableProjector<TEvent, TModel>>
        where TEvent : class,IEvent
        where TModel : IDataModel
    {
        IIocContainer container;
        Action<TEvent, IServiceRequestMetadata> actionToWrap;

        public TableEventProjectionWrapper(IIocContainer container, Action<TEvent, IServiceRequestMetadata> actionToWrap)
        {
            this.container = container;
            this.actionToWrap = actionToWrap;
        }

        public void Handle(WrappedEvent<TEvent> @event)
        {
            actionToWrap(@event.InternalEvent, @event.ServiceRequestMetadata);
        }
    }
}