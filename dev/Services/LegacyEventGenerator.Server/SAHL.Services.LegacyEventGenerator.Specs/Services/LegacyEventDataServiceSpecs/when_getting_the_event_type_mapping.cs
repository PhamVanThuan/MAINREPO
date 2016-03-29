using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models;
using SAHL.Services.LegacyEventGenerator.Services.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.Services.LegacyEventDataServiceSpecs
{
    public class when_getting_the_event_type_mapping : WithFakes
    {
        private static IDictionary<string, int> result;
        private static LegacyEventDataService service;
        private static FakeDbFactory fakeDbFactory;
        private static int stageTransitionCompositeKey;
        private static IEnumerable<EventTypeMappingModel> eventModels;

        private Establish context = () =>
            {
                eventModels = new EventTypeMappingModel[] { new EventTypeMappingModel(1, "EventName1"), new EventTypeMappingModel(2, "EventName2") };
                fakeDbFactory = new FakeDbFactory();
                stageTransitionCompositeKey = 5656541;
                service = new LegacyEventDataService(fakeDbFactory);
                fakeDbFactory.FakedDb.InReadOnlyAppContext().WhenToldTo(x => x.Select(Arg.Any<GetEventTypeMappingStatement>()))
                    .Return(eventModels);
            };

        private Because of = () =>
            {
                result = service.GetEventTypeMapping();
            };

        private It should_return_a_dictionary_containing_both_of_event_details = () =>
        {
            result.Count.ShouldEqual(2);
        };

        private It should_contain_event_one = () =>
        {
            result.Where(x => x.Value == 1).Select(y => y.Key).FirstOrDefault().ShouldEqual("EventName1");
        };

        private It should_contain_event_two = () =>
        {
            result.Where(x => x.Value == 2).Select(y => y.Key).FirstOrDefault().ShouldEqual("EventName2");
        };

        private It should_use_the_correct_statement_with_the_key_provided = () =>
        {
            fakeDbFactory.FakedDb.InReadOnlyAppContext().WasToldTo(x => x.Select(Arg.Any<GetEventTypeMappingStatement>()));
        };
    }
}