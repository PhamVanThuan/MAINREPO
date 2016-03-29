using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System.Linq;
using System;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppPendingDeclineLegacyEventSpecs
{
    public class when_told_to_handle_pending_decline_query : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateApplicationPendingDeclineLegacyEventQuery query;
        private static CreateApplicationPendingDeclineLegacyEventQueryHandler handler;    
        private static ILegacyEventDataService legacyEventDataService;

        private static AppProgressInApplicationCaptureModel model;
        private static int applicationKey;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();
            handler = new CreateApplicationPendingDeclineLegacyEventQueryHandler(legacyEventDataService);

            applicationKey = 144;
            query = new CreateApplicationPendingDeclineLegacyEventQuery();
            query.Initialise(56, applicationKey, (int)GenericKeyType.Offer, new DateTime(2015,01,29), 13, "BlackCat");
            
            model = new AppProgressInApplicationCaptureModel { OfferInformationKey = 99 };
            legacyEventDataService.WhenToldTo(x => x.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey)).Return(model);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_return_a_application_decline_pended_legacy_event = () =>
        {
            query.Result.Results.First().ShouldMatch(m =>
                m.AdUserKey == query.ADUserKey && m.AdUserName == query.ADUserName &&
                m.ApplicationInformationKey == model.OfferInformationKey &&
                m.ApplicationKey == applicationKey);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}