using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Transactions
{
    public interface ILoanTransactions
    {
        ISystemMessageCollection PostTransaction(PostTransactionModel transactionModel);

        ISystemMessageCollection postTransactionReversal(ReversalTransactionModel reversalTransactionModel);
    }
}