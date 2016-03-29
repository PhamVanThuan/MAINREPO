using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using SAHL.Shared.BusinessModel.Transactions;
using System.Linq;

namespace SAHL.Shared.BusinessModel.Specs.Transactions.PostLoanTransactionReversalSpecs
{
    public class when_posting_a_reversal_transaction : WithFakes
    {
        private static ILoanTransactions loanTransactionReversal;
        private static IDomainRuleManager<TransactionRuleModel> domainRuleManager;
        private static ITransactionDataManager transactionDataManager;
        private static ReversalTransactionModel transactionModel;
        private static ISystemMessageCollection messages;


        Establish context = () =>
        {
            transactionDataManager = An<ITransactionDataManager>();
            domainRuleManager = An<IDomainRuleManager<TransactionRuleModel>>();
            loanTransactionReversal = new LoanTransactions(transactionDataManager, domainRuleManager);
            transactionModel = new ReversalTransactionModel(14008, "SystemUser");

            transactionDataManager.WhenToldTo(x => x.PostReversalTransaction(
                Param<ReversalTransactionModel>.Matches(y => y.FinancialTransactionKey == transactionModel.FinancialTransactionKey
                && y.UserId == transactionModel.UserId
                ))).Return(string.Empty);
        };

        Because of = () =>
        {
            messages = loanTransactionReversal.postTransactionReversal(transactionModel);
        };

        It should_register_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<FinancialTransactionServiceKeyMustExistRule>()));
        };

        It should_run_rules = () =>
        {
            domainRuleManager.WasToldTo(x =>
                x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(),
                Param<TransactionRuleModel>.Matches(y => y.TransactionKey == transactionModel.FinancialTransactionKey))
                );
        };

        It should_post_the_reversal_transaction = () =>
        {
            transactionDataManager.WasToldTo(x => x.PostReversalTransaction(transactionModel));
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
