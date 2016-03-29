using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using StructureMap;

namespace SAHL.Core.Events.Specs.EventRaiserSpecs
{
    public class when_raising_an_event_given_that_EventStore_is_internal : WithFakes
    {
        private static IEventStore eventStore;

        private Establish context = () =>
        {
            ObjectFactory.Configure(x =>
            {
                x.For<IEventSerialiser>().Use<EventSerialiser>();
                x.For<IEventStore>().Use<EventStore>();
                x.For<IDbFactory>().Use<DbFactory>();
                x.For<ISqlRepositoryFactory>().Use(An<ISqlRepositoryFactory>());
                x.For<IDbConnectionProviderFactory>().Use(An<IDbConnectionProviderFactory>());
                x.For<IDbConnectionProviderStorage>().Use(An<IDbConnectionProviderStorage>());
                x.For<IEventStore>().Use((y) => new EventStore(y.TryGetInstance<IEventSerialiser>(), y.TryGetInstance<IDbFactory>()));
            });
        };

        private Because of = () =>
        {
            eventStore = ObjectFactory.GetInstance<IEventStore>();
        };

        private It should_have_concrete_implementation_EventStore = () =>
        {
            eventStore.ShouldNotBeNull();
            eventStore.ShouldBeOfExactType<EventStore>();
        };
    }
}