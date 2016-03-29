using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateApplicationNTUFinalizedLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_ntu_finalised_query : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateApplicationNTUFinalizedLegacyEventQuery query;
        private static CreateApplicationNTUFinalizedLegacyEventQueryHandler handler;
        private static ILegacyEventDataService legacyEventDataService;

        private static AppProgressInApplicationCaptureModel model;
        private static int applicationKey;

        private Establish context = () =>
        {
            legacyEventDataService = An<ILegacyEventDataService>();
            handler = new CreateApplicationNTUFinalizedLegacyEventQueryHandler(legacyEventDataService);

            applicationKey = 144;
            query = new CreateApplicationNTUFinalizedLegacyEventQuery();
            query.Initialise(110, applicationKey, (int)GenericKeyType.Offer, new DateTime(2015, 01, 29), 13, "BlackCat");

            model = new AppProgressInApplicationCaptureModel { OfferInformationKey = 99 };
            legacyEventDataService.WhenToldTo(x => x.GetAppProgressModelByStageTransitionCompositeKey(query.StageTransitionCompositeKey)).Return(model);
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_return_an_AppProgressDisbursedLegacyEvent = () =>
        {
            string fullName = query.Result.Results.First().GetType().FullName;
            fullName.ShouldEqual(typeof(ApplicationNTUFinalizedLegacyEvent).FullName);
        };

        private It should_contain_the_correct_properties = () =>
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