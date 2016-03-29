using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.LegacyEventQueryMappingServiceSpecs
{
    public class when_the_service_has_been_stopped_no_queries_can_be_mapped : WithFakes
    {
        private static LegacyEventQueryMappingService service;
        private static ILegacyEventDataService eventDataService;
        private static IIocContainer iocContainer;
        private static IEnumerable<ILegacyEventGeneratorQuery> queries;
        private static dynamic result;

        Establish context = () =>
        {
            queries = new ILegacyEventGeneratorQuery[] { new CreateApplicationDeclinedLegacyEventQuery() };
            eventDataService = An<ILegacyEventDataService>();
            iocContainer = An<IIocContainer>();
            iocContainer.WhenToldTo(x => x.GetAllInstances<ILegacyEventGeneratorQuery>()).Return(queries);
            iocContainer.WhenToldTo(x => x.GetConcreteInstance(typeof(CreateApplicationDeclinedLegacyEventQuery))).Return(typeof(CreateApplicationDeclinedLegacyEventQuery));
            service = new LegacyEventQueryMappingService(eventDataService, iocContainer);
            service.Start();
        };

        Because of = () =>
        {
            service.Stop();
            result = service.GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(queries.First().StageDefinitionStageDefinitionGroupKey);
        };


        private It should_return_null = () =>
        {
            bool isnull = Object.ReferenceEquals(null, result);
            isnull.ShouldBeTrue();
        };
    }
}