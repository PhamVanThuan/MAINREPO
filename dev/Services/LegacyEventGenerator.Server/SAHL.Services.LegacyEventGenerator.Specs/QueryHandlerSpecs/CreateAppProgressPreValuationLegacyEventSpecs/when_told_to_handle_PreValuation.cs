using System;
using System.Linq;
using System.Data;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppProgressPreValuationLegacyEventHandler
{
    public class when_told_to_handle_PreValuation : WithFakes
    {

        private static ISystemMessageCollection messages;
        private static CreateAppProgressPreValuationLegacyEventQuery query;
        private static CreateAppProgressPreValuationLegacyEventQueryHandler handler;
        private static AppProgressPreValuationLegacyEvent expectedResult;
        private static ILegacyEventDataService legacyEventDataService;



        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();

            query = new CreateAppProgressPreValuationLegacyEventQuery();
            handler = new CreateAppProgressPreValuationLegacyEventQueryHandler(legacyEventDataService);

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

