using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using SAHL.Services.Interfaces.LegacyEventGenerator.Queries;
using SAHL.Services.LegacyEventGenerator.QueryHandlers;
using SAHL.Services.LegacyEventGenerator.Services.Models.Events;
using System;
using System.Linq;

namespace SAHL.Services.LegacyEventGenerator.Specs.QueryHandlerSpecs.CreateIsComcorpApplicationLegacyEventQueryHandlerSpecs
{
    public class when_told_to_handle_is_comcorp_application_query : WithFakes
    {
        private static ISystemMessageCollection messages;
        private static CreateIsComcorpApplicationLegacyEventQuery query;
        private static CreateIsComcorpApplicationLegacyEventQueryHandler handler;
        private static int applicationKey;

        private Establish context = () =>
        {
            handler = new CreateIsComcorpApplicationLegacyEventQueryHandler();
            applicationKey = 144;
            query = new CreateIsComcorpApplicationLegacyEventQuery();
            query.Initialise(56, applicationKey, (int)GenericKeyType.Offer, new DateTime(2015, 01, 29), 13, "BlackCat");
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
        };

        private It should_return_an_AppProgressDisbursedLegacyEvent = () =>
        {
            string fullName = query.Result.Results.First().GetType().FullName;
            fullName.ShouldEqual(typeof(IsComcorpApplicationLegacyEvent).FullName);
        };

        private It should_contain_the_correct_properties = () =>
        {
            query.Result.Results.First().ShouldMatch(m =>
                m.AdUserKey == query.ADUserKey && m.AdUserName == query.ADUserName &&
                m.ApplicationKey == applicationKey);
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}