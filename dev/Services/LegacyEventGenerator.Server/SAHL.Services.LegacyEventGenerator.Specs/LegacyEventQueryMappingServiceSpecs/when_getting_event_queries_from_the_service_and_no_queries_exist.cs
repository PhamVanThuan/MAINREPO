using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;

namespace SAHL.Services.LegacyEventGenerator.Specs.LegacyEventQueryMappingServiceSpecs
{
    public class when_getting_event_queries_from_the_service_and_no_queries_exist : WithFakes
    {
        private static LegacyEventQueryMappingService service;
        private static ILegacyEventDataService eventDataService;
        private static IIocContainer iocContainer;
        private static IEnumerable<ILegacyEventGeneratorQuery> queries;
        private static dynamic result;

        private Establish context = () =>
        {
            queries = new ILegacyEventGeneratorQuery[] { };
            eventDataService = An<ILegacyEventDataService>();
            iocContainer = An<IIocContainer>();
            iocContainer.WhenToldTo(x => x.GetAllInstances<ILegacyEventGeneratorQuery>()).Return(queries);
            iocContainer.WhenToldTo(x => x.GetConcreteInstance(typeof(CreateApplicationDeclinedLegacyEventQuery))).Return(typeof(CreateApplicationDeclinedLegacyEventQuery));
            service = new LegacyEventQueryMappingService(eventDataService, iocContainer);
            service.Start();
        };

        private Because of = () =>
        {
            result = service.GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(110);
        };

        private It should_get_the_concrete_event_instance_the_ioc_container_using_the_correct_event = () =>
        {
            iocContainer.WasNotToldTo(x => x.GetConcreteInstance(Param.IsAny<Type>()));
        };

        private It should_return_null = () =>
        {
            bool isnull = Object.ReferenceEquals(null, result);
            isnull.ShouldBeTrue();
        };
    }
}