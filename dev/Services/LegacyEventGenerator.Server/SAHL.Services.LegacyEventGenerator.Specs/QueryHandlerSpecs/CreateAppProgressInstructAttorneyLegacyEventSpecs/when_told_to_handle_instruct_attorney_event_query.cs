using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;


namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppProgressInstructAttorneyLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_instruct_attorney_event_query : WithFakes
    {

        private static ISystemMessageCollection messages;
        private static CreateRegistrationsInstructAttorneyLegacyEventQuery eventQuery;
        private static CreateRegistrationInstructAttorneyLegacyEventQueryHandler eventQueryHandler;

        private static ILegacyEventDataService legacyEventDataService;
        
        Establish that = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();

            eventQuery = new CreateRegistrationsInstructAttorneyLegacyEventQuery();
            eventQueryHandler = new CreateRegistrationInstructAttorneyLegacyEventQueryHandler(legacyEventDataService);

            legacyEventDataService.WhenToldTo(x => x.GetAppProgressModelByStageTransitionCompositeKey(eventQuery.StageTransitionCompositeKey)).Return(new AppProgressInApplicationCaptureModel());   

        };

        private Because of = () =>
        {
            messages = eventQueryHandler.HandleQuery(eventQuery);
        };

        private It should = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };


    }
}
