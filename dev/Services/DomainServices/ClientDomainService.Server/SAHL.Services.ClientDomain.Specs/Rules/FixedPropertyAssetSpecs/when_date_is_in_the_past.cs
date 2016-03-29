using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.Specs.Rules.FixedPropertyAssetSpecs
{
    internal class when_date_is_in_the_past : WithFakes
    {
        private static FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<FixedPropertyAssetModel> rule;
        private static FixedPropertyAssetModel model;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            rule = new FixedPropertyAssetAcquiredDateCannotBeInTheFutureRule<FixedPropertyAssetModel>();
            messages = SystemMessageCollection.Empty(); var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            model = new FixedPropertyAssetModel(date.AddSeconds(-1), 1, 1, 1);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_a_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}