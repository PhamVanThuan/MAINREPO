using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers.LoanTransaction;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.LoanTransactionManagerSpec
{
    public class when_posting_a_transaction_with_errors : WithFakes
    {
        private static ILoanTransactionManager loanTransactionManager;
        private static ISystemMessageCollection messages;
        private static ILoanTransactions loanTransactionService;
        private static int financialServiceKey;
        private static LoanTransactionTypeEnum transactionTypeKey;
        private static int amount;
        private static DateTime effectiveDate;
        private static string reference;
        private static string userId;

        Establish context = () =>
        {
            loanTransactionService = An<ILoanTransactions>();
            loanTransactionManager = new LoanTransactionManager(loanTransactionService);

            financialServiceKey = 56;
            transactionTypeKey = LoanTransactionTypeEnum.CapitalisedLegalFeeTransaction;
            amount = 500;
            effectiveDate = DateTime.Now;
            reference = "SPVReferene";
            userId = "System";
            var errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(new SystemMessage("Transaction Failed", SystemMessageSeverityEnum.Error));
            loanTransactionService.WhenToldTo(x => x.PostTransaction(Param.IsAny<PostTransactionModel>())).Return(errorMessages);
        };

        Because of = () =>
        {
            messages = loanTransactionManager.PostTransaction(financialServiceKey, transactionTypeKey, amount, effectiveDate, reference, userId);
        };

        It should_post_a_loan_transaction = () =>
        {
            loanTransactionService.WasToldTo(x => x.PostTransaction(Param<PostTransactionModel>
                .Matches(
                    y => y.Amount == amount
                    && y.FinancialServiceKey == financialServiceKey
                    && y.TransactionTypeKey == (int)transactionTypeKey
                    && y.EffectiveDate == effectiveDate
                    && y.Reference == reference
                    && y.UserId == userId)
                 ));
        };

        It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.AllMessages.First().Message.ShouldEqual("Transaction Failed");
        };
    }
}
