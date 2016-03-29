using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Transactions;
using System;

namespace SAHL.Services.FinanceDomain.Managers.LoanTransaction
{
    public class LoanTransactionManager : ILoanTransactionManager
    {
        private ILoanTransactions loanTransactionService;

        public LoanTransactionManager(ILoanTransactions loanTransactionService)
        {
            this.loanTransactionService = loanTransactionService;
        }

        public ISystemMessageCollection PostTransaction(int financialServiceKey, LoanTransactionTypeEnum transactionTypeKey, decimal amount, DateTime effectiveDate,
            string reference, string userId)
        {
            var transactionModel = new PostTransactionModel(financialServiceKey, (int)transactionTypeKey, amount, effectiveDate, reference, userId);

            return this.loanTransactionService.PostTransaction(transactionModel);
        }

        public ISystemMessageCollection PostReversalTransaction(int financialTransactionKey, string userId)
        {
            var transactionModel = new ReversalTransactionModel(financialTransactionKey, userId);
            return loanTransactionService.postTransactionReversal(transactionModel);
        }
    }
}
