using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;
using System;

namespace SAHL.Shared.BusinessModel.Specs.Transactions.PostLoanTransactionSpecs
{
    public class when_posting_txn_and_returns_messages : WithFakes
    {
        private static ITransactionDataManager transactionDataManager;
        private static PostTransactionModel transactionModel;
        private static IDomainRuleManager<TransactionRuleModel> domainRuleManager;
        private static ILoanTransactions postLoanTransaction;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            transactionDataManager = An<ITransactionDataManager>();
            domainRuleManager = An<IDomainRuleManager<TransactionRuleModel>>();
            transactionModel = new PostTransactionModel(123, 9997, 1000m, DateTime.Now.AddDays(-2), "ref2", "system");
            postLoanTransaction = new LoanTransactions(transactionDataManager, domainRuleManager);
            messages = SystemMessageCollection.Empty();
            transactionDataManager.WhenToldTo(x => x.PostTransaction(transactionModel)).Return("EffectiveDate cannot be prior to OpenDate.");
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param<TransactionRuleModel>.Matches(y =>
                y.EffectiveDate == transactionModel.EffectiveDate
                && y.TransactionTypeKey == transactionModel.TransactionTypeKey
                ))).
                Callback<ISystemMessageCollection>(y => y.AllMessages.ShouldBeEmpty());
        };

        private Because of = () =>
        {
            messages = postLoanTransaction.PostTransaction(transactionModel);
        };

        private It should_run_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(messages, Param<TransactionRuleModel>.Matches(y =>
                y.EffectiveDate == transactionModel.EffectiveDate
                && y.TransactionTypeKey == transactionModel.TransactionTypeKey
                )));
        };

        private It should_post_transaction = () =>
        {
            transactionDataManager.WasToldTo(x => x.PostTransaction(transactionModel));
        };

        private It should_return_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}
