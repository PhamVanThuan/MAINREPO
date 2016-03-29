using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Managers
{
    public interface ITransactionDataManager
    {
        bool DoesTransactionTypeExist(int transactionTypeKey);

        bool DoesFinancialTransactionKeyExist(int financialTransactionKey);

        string PostTransaction(PostTransactionModel transactionModel);

        string PostReversalTransaction(ReversalTransactionModel transactionModel);
    }
}
