using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using SAHL.Shared.BusinessModel.Transactions;
using System;

namespace SAHL.Shared.BusinessModel.Specs.Transactions.PostLoanTransactionSpecs
{
    public class when_registering_rules : WithFakes
    {
        private static ITransactionDataManager transactionDataManager;
        private static PostTransactionModel transactionModel;
        private static IDomainRuleManager<TransactionRuleModel> domainRuleManager;
        private static TransactionRuleModel ruleModel;
        private static ILoanTransactions postLoanTransaction;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            transactionDataManager = An<ITransactionDataManager>();
            domainRuleManager = An<IDomainRuleManager<TransactionRuleModel>>();
            transactionModel = new PostTransactionModel(123, 480, 1000m, DateTime.Now.AddDays(-2), "ref2", "system");
            ruleModel = new TransactionRuleModel { EffectiveDate = transactionModel.EffectiveDate, TransactionTypeKey = transactionModel.TransactionTypeKey };
            postLoanTransaction = new LoanTransactions(transactionDataManager, domainRuleManager);
            messages = SystemMessageCollection.Empty();
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), ruleModel)).
                Callback<ISystemMessageCollection>(y => y.AllMessages.ShouldBeEmpty());
        };

        private Because of = () =>
        {
            messages = postLoanTransaction.PostTransaction(transactionModel);
        };

        private It should_register_a_rule_to_ensure_the_transaction_type_exists = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<TransactionTypeMustBeValidRule>()));
        };

        private It should_register_a_rule_to_ensure_the_effective_date_is_not_in_the_future = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<EffectiveDateCannotBeInTheFutureRule>()));
        };

        private It should_register_a_rule_to_ensure_the_effective_date_is_not_in_a_previous_month = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<EffectiveDateCannotBePriorToTheFirstOfTheCurrentMonthRule>()));
        };
    }
}