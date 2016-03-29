using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppProgressInApplicationCaptureLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_inapplicationcapturelegacyeventquery : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateAppProgressInApplicationCaptureLegacyEventQuery query;
        private static CreateAppProgressInApplicationCaptureLegacyEventQueryHandler handler;

        private static ILegacyEventDataService legacyEventDataService;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();

            query = new CreateAppProgressInApplicationCaptureLegacyEventQuery();
            handler = new CreateAppProgressInApplicationCaptureLegacyEventQueryHandler(legacyEventDataService);

            legacyEventDataService.WhenToldTo(x => x.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey)).Return(new AppProgressInApplicationCaptureModel());
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