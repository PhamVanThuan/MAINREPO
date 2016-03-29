using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IManualDebitOrderService
    {
        void DeleteManualDebitOrders(int accountKey);

        IEnumerable<Automation.DataModels.ManualDebitOrder> GetManualDebitOrders(int accountKey);

        IEnumerable<Automation.DataModels.ManualDebitOrder> InsertManualDebitOrder(Automation.DataModels.Account account, string userName);
    }
}