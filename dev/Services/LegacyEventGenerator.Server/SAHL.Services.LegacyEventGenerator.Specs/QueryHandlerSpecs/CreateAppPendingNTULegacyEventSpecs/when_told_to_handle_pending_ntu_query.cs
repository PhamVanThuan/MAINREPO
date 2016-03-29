using System;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateAppPendingNTULegacyEventSpecs
{
    public class when_told_to_handle_pending_ntu_query : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateApplicationPendingNTULegacyEventQuery query;
        private static CreateApplicationPendingNTULegacyEventQueryHandler handler;
        private static ILegacyEventDataService legacyEventDataService;

        private static AppProgressInApplicationCaptureModel model;
        private static int applicationKey;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();
            handler = new CreateApplicationPendingNTULegacyEventQueryHandler(legacyEventDataService);

            applicationKey = 144;
            query = new CreateApplicationPendingNTULegacyEventQuery();
            query.Initialise(56, applicationKey, (int)GenericKeyType.Offer, new DateTime(2015, 01, 29), 13, "BlackCat");

            model = new AppProgressInApplicationCaptureModel { OfferInformationKey = 99 };
            legacyEventDataService.WhenToldTo(x => x.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey)).Return(model);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_return_a_application_ntu_pended_legacy_event = () =>
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