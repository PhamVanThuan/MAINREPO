using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateQACompletedLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_createqacompletedlegacyeventquery : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateQACompletedLegacyEventQuery query;
        private static CreateQACompletedLegacyEventQueryHandler handler;

        private static ILegacyEventQueryMappingService legacyEventMappingService;

        private Establish context = () =>
        {
            legacyEventMappingService = An<ILegacyEventQueryMappingService>();

            query = new CreateQACompletedLegacyEventQuery();
            handler = new CreateQACompletedLegacyEventQueryHandler();
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}