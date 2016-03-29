using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.LegacyEventGenerator.Server;
using SAHL.Core.IoC;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using StructureMap;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.LegacyEventQueryMappingServiceSpecs
{
    public class when_configuring_the_ioc_registry : WithFakes
    {
        private static IocRegistry registry;
        private static IEnumerable<ILegacyEventGeneratorQuery> queries;
        private static Container container;

        private Establish context = () =>
        {
            registry = new SAHL.Config.LegacyEventGenerator.Server.IocRegistry();
        };

        private Because of = () =>
        {
            container = new Container(registry);
        };

        [Ignore("Trying to get this test working.")]
        private It should_contain_legacy_event_queries = () =>
        {
            container.GetAllInstances<ILegacyEventGeneratorQuery>().Count().ShouldBeGreaterThan(0);
        };

        [Ignore("Trying to get this test working.")]
        private It should_use_the_mapping_service_as_an_IStoppable = () =>
        {
            container.GetAllInstances<ILegacyEventQueryMappingService>().Count().ShouldBeGreaterThan(0);
            container.GetInstance<IStoppable>().ShouldBeAssignableTo<ILegacyEventQueryMappingService>();
        };
    }
}