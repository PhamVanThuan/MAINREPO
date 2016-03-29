using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.LoanTransactionManagerSpec
{
    public class when_posting_a_reversal_transaction : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static ILoanTransactions loanTransactionService;
        private static int financialTransactionKey;
        private static string userId;
        private static ReversalTransactionModel transactionModel;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            financialTransactionKey = 1408;
            userId = "SystemUser";
            loanTransactionService = An<ILoanTransactions>();
            loanTransactionManager = new LoanTransactionManager(loanTransactionService);

            transactionModel = new ReversalTransactionModel(financialTransactionKey, userId);
            loanTransactionService.WhenToldTo(x => x.postTransactionReversal(Param<ReversalTransactionModel>.Matches(y =>
                y.FinancialTransactionKey == financialTransactionKey
                && y.UserId == userId))).Return(SystemMessageCollection.Empty());
        };

        Because of = () =>
        {
            messages = loanTransactionManager.PostReversalTransaction(financialTransactionKey, userId);
        };

        It should_post_the_reversal_transaction = () =>
        {
            loanTransactionService.WasToldTo(x => x.postTransactionReversal(Param<ReversalTransactionModel>.Matches(y =>
                y.FinancialTransactionKey == financialTransactionKey
                && y.UserId == userId)));
        };

        It should_return_without_errors = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
