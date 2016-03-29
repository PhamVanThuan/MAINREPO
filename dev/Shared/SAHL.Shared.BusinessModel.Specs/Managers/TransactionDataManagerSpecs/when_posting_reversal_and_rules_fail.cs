using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;

namespace SAHL.Shared.BusinessModel.Specs.Transactions.PostLoanTransactionReversalSpecs
{
    public class when_posting_reversal_and_rules_fail : WithFakes
    {
        private static ILoanTransactionsReversal loanTransactionReversal;
        private static IDomainRuleManager<TransactionRuleModel> domainRuleManager;
        private static ITransactionDataManager transactionDataManager;
        private static ReversalTransactionModel transactionModel;
        private static ISystemMessageCollection messages;


        Establish context = () =>
        {
            transactionDataManager = An<ITransactionDataManager>();
            domainRuleManager = An<IDomainRuleManager<TransactionRuleModel>>();
            loanTransactionReversal = new LoanTransactionsReversal(transactionDataManager, domainRuleManager);
            transactionModel = new ReversalTransactionModel(14008, "SystemUser");

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<TransactionRuleModel>())).Callback<ISystemMessageCollection>(y =>
                {
                    y.AddMessage(new SystemMessage("rule failed", SystemMessageSeverityEnum.Error));
                });
        };

        Because of = () =>
        {
            messages = loanTransactionReversal.postTransaction(transactionModel);
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
            transactionDataManager.WasNotToldTo(x => x.PostReversalTransaction(transactionModel));
        };

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}
