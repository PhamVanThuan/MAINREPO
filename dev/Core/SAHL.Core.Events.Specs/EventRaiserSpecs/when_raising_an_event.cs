using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Specs.EventSerialiserSpecs.Fakes;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Events.Specs.EventRaiserSpecs
{
    public class when_raising_an_event : WithFakes
    {
        private static IServiceRequestMetadata metadata = null;
        private static IEventRaiser eventRaiser;
        private static IEventStore eventStore;
        private static IEvent @event;
        private static IIocContainer iocContainer;
        private static int genericKeyTypeKey = 1;
        private static int genericKey = 1;
        private static DateTime date;

        private Establish context = () =>
        {
            eventStore                = An<IEventStore>();
            iocContainer              = An<IIocContainer>();
            iocContainer.WhenToldTo(x => x.GetInstance(typeof(IEventStore))).Return(eventStore);
            eventRaiser               = new EventRaiser(iocContainer);
            date                      = DateTime.Now.AddDays(-1);
            @event                    = new FakeEvent(CombGuid.Instance.Generate(), DateTime.Now, "john", 1, FakeEnum.def, 1, 2000, 
                                                      new List<string>() { "hello", "world" }, 
                                                      new string[] { "hello", "world" }, null, null);
        };

        private Because of = () =>
        {
            eventRaiser.RaiseEvent(date, @event, genericKey, genericKeyTypeKey, metadata);
        };

        private It should_be_stored_with_metadata = () =>
        {
            eventStore.WasToldTo(x => x.StoreEvent(date, @event, genericKey, genericKeyTypeKey, metadata));
        };
    }
}