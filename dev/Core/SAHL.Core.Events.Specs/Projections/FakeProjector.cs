using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Events.Specs.EventSerialiserSpecs.Fakes;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Specs.Projections
{
    public class StandAloneClass
    {
    }

    public class FakeProjector : IEventProjector
    {
    }

    public class FakeEvent : IEvent
    {
        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime Date
        {
            get { throw new NotImplementedException(); }
        }

        public string ClassName
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class FakeService : IServiceClient
    {
        public SystemMessages.ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IServiceCommand
        {
            throw new NotImplementedException();
        }

        public SystemMessages.ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            throw new NotImplementedException();
        }
    }

    public class FakeServiceProjector : IServiceProjector<FakeEvent, FakeService>
    {
        bool handled = false;
        FakeService client = null;
        IEvent @event = null;

        public bool Handled
        {
            get
            {
                return this.handled;
            }
        }

        public IServiceClient Client
        {
            get
            {
                return this.client;
            }
        }

        public IEvent Event
        {
            get
            {
                return this.@event;
            }
        }

        public void Handle(FakeEvent @event, IServiceRequestMetadata metadata, FakeService service)
        {
            this.@event = @event;
            this.client = service;
            this.handled = true;
        }
    }

    public class FakeTableProjector : ITableProjector<FakeEvent, IDataModel>
    {
        public bool Handled { get; private set; }
        public FakeEvent Event { get; private set; }
        
        public void Handle(FakeEvent @event, IServiceRequestMetadata metadata)
        {
            this.Event = @event;
            this.Handled = true;
        }
    }

    public class FakeGenericProjector : IEventProjector<FakeEvent>
    {
    }
}