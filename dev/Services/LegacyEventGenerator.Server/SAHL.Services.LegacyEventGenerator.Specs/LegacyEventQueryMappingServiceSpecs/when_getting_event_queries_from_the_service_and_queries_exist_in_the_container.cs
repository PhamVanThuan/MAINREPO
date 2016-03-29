using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.LegacyEventGenerator.Specs.LegacyEventQueryMappingServiceSpecs
{
    public class when_getting_event_queries_from_the_service_and_queries_exist_in_the_container : WithFakes
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
            iocContainer.WhenToldTo(x=>x.GetAllInstances<ILegacyEventGeneratorQuery>()).Return(queries);
            iocContainer.WhenToldTo(x => x.GetConcreteInstance(typeof(CreateApplicationDeclinedLegacyEventQuery))).Return(typeof(CreateApplicationDeclinedLegacyEventQuery));
            service = new LegacyEventQueryMappingService(eventDataService, iocContainer);
            service.Start();
        };

        Because of = () =>
        {
            result = service.GetLegacyEventQueryByStageDefinitionStageDefinitionGroupKey(queries.First().StageDefinitionStageDefinitionGroupKey);
        };

        It should_get_the_concrete_event_instance_the_ioc_container_using_the_correct_event = () =>
        {
            iocContainer.WasToldTo(x => x.GetConcreteInstance(typeof(CreateApplicationDeclinedLegacyEventQuery)));
        };

        It should_return_the_event = () =>
        {
            string fullName = result.FullName;
            fullName.ShouldEqual(typeof(CreateApplicationDeclinedLegacyEventQuery).FullName);
        };
    }
}
