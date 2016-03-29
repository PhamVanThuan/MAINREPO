using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IManualDebitOrderRepository
    {
        IManualDebitOrder GetEmptyManualDebitOrder();

        void SaveManualDebitOrder(IManualDebitOrder manualDebitOrder);

        void CancelManualDebitOrder(IManualDebitOrder manualDebitOrder);

        IManualDebitOrder GetManualDebitOrderByKey(int Key);

        IEventList<IManualDebitOrder> GetPendingManualDebitOrdersByFinancialServiceKey(int FinancialServiceKey);

        IEventList<IManualDebitOrder> GetCollectedManualDebitOrdersByFinancialServiceKey(int FinancialServiceKey);
    }
}