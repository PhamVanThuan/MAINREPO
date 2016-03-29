using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using System;

namespace SAHL.Services.FinanceDomain.Managers.LoanTransaction
{
    public interface ILoanTransactionManager
    {
        ISystemMessageCollection PostTransaction(int financialServiceKey, LoanTransactionTypeEnum transactionTypeKey, decimal amount, DateTime effectiveDate,
            string reference, string userId);

        ISystemMessageCollection PostReversalTransaction(int financialTransactionKey, string userId);
    }
}
