using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using System;

namespace SAHL.Shared.BusinessModel.Specs.Rules.EffectiveDateCannotBeInTheFutureRuleSpecs
{
    public class when_valid_effective_date_is_not_in_future : WithFakes
    {
        private static TransactionRuleModel ruleModel;
        private static EffectiveDateCannotBeInTheFutureRule rule;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            var tranaction = new PostTransactionModel(123, 12, 1000m, DateTime.Now.AddDays(-1), "ref1", "system");
            ruleModel = new TransactionRuleModel { EffectiveDate = tranaction.EffectiveDate };
            rule = new EffectiveDateCannotBeInTheFutureRule();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_return_error_message = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}
