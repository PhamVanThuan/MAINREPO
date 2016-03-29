using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using SAHL.Shared.BusinessModel.Transactions;
using System;

namespace SAHL.Shared.BusinessModel.Specs.Rules.EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRuleSpecs
{
    public class when_effective_date_is_after_1st_of_current_month : WithFakes
    {
        private static ILoanTransactions postLoanTransaction;
        private static TransactionRuleModel ruleModel;
        private static EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule rule;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            postLoanTransaction = An<ILoanTransactions>();
            var transaction = new PostTransactionModel(21, 232, 100m, new DateTime(year, month, 2), "ref2", "system");
            ruleModel = new TransactionRuleModel { EffectiveDate = transaction.EffectiveDate };
            rule = new EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule();
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_not_return_error_message = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}
