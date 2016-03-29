using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events
{
    public class EventRaiser : IEventRaiser
    {
        public EventRaiser(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
            this.eventStore = this.iocContainer.GetInstance(typeof(IEventStore)) as IEventStore;
        }

        private IEventStore eventStore;
        private IIocContainer iocContainer;

        public void RaiseEvent(DateTime eventOccuranceDate, IEvent @event, int genericKey, int genericKeyTypeKey, IServiceRequestMetadata metadata)
        {
            this.eventStore.StoreEvent(eventOccuranceDate, @event, genericKey, genericKeyTypeKey, metadata);
        }
    }
}