using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events
{
    public class WrappedEvent<T> : IWrappedEvent<T> where T : class, IEvent
    {
        public WrappedEvent(Guid id, T internalEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            if (id == Guid.Empty) { throw new ArgumentNullException("eventId"); }
            if (internalEvent == null) { throw new ArgumentNullException("internalEvent"); }

            this.Id = id;
            this.InternalEvent = internalEvent;
            ServiceRequestMetadata = serviceRequestMetadata;
        }

        public Guid Id { get; private set; }

        public T InternalEvent { get; private set; }

        public IServiceRequestMetadata ServiceRequestMetadata { get; private set; }

        public string Name
        {
            get { return this.InternalEvent.Name; }
        }
    }
}