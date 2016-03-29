using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.FixedPropertyAssetSpecs
{
    internal class when_date_is_in_the_future : WithFakes
    {
        private static FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<FixedPropertyAssetModel> rule;
        private static FixedPropertyAssetModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            rule = new FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<FixedPropertyAssetModel>();
            messages = SystemMessageCollection.Empty();
            var date = DateTime.Today.AddDays(1);
            model = new FixedPropertyAssetModel(date, 1, 1, 1);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldContain("The acquisition date for a fixed property asset cannot be in the future.");
        };
    }
}
