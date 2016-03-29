using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppProgressReturnToManageAppFromValuationLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_returntomanageappfromvaluationlegacyeventquery : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateAppProgressReturnToManageAppFromValuationLegacyEventQuery query;
        private static CreateAppProgressReturnToManageAppFromValuationLegacyEventQueryHandler handler;

        private static ILegacyEventDataService legacyEventDataService;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();

            query = new CreateAppProgressReturnToManageAppFromValuationLegacyEventQuery();
            handler = new CreateAppProgressReturnToManageAppFromValuationLegacyEventQueryHandler(legacyEventDataService);

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