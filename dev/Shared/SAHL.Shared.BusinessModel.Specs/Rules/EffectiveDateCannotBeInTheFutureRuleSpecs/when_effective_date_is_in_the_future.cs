using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using System;
using System.Linq;

namespace SAHL.Shared.BusinessModel.Specs.Rules.EffectiveDateCannotBeInTheFutureRuleSpecs
{
    public class when_effective_date_is_in_the_future : WithFakes
    {
        private static TransactionRuleModel ruleModel;
        private static EffectiveDateCannotBeInTheFutureRule rule;
        private static ISystemMessageCollection messages;
        private static string expectedMessage;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            var transaction = new PostTransactionModel(123, 12, 1000m, DateTime.Now.AddDays(3), "ref1", "system");
            ruleModel = new TransactionRuleModel { EffectiveDate = transaction.EffectiveDate };
            rule = new EffectiveDateCannotBeInTheFutureRule();
            expectedMessage = "The Effective Date cannot be in the future";
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_return_error_message = () =>
        {
            messages.AllMessages.First().Message.ShouldContain(expectedMessage);
        };
    }
}
