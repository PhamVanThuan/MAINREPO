using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using System;
using System.Linq;

namespace SAHL.Shared.BusinessModel.Specs.Rules.EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRuleSpecs
{
    public class when_effective_date_is_less_than_first_of_current_month : WithFakes
    {
        private static TransactionRuleModel ruleModel;
        private static EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule rule;
        private static ISystemMessageCollection messages;
        private static string expectedMessage;

        private Establish context = () =>
        {
            var transaction = new PostTransactionModel(121, 12, 1000m, DateTime.Now.AddDays(-40), "ref2", "system");
            ruleModel = new TransactionRuleModel { EffectiveDate = transaction.EffectiveDate };
            rule = new EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule();
            messages = SystemMessageCollection.Empty();
            expectedMessage = "Cannot post a transaction with an effective date prior to the 1st of this month.";
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
