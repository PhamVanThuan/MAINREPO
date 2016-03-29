using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Transactions
{
    public interface ILoanTransactionsReversal
    {
        ISystemMessageCollection postTransaction(ReversalTransactionModel transactionModel);
    }
}
