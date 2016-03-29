using System;

namespace SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly
{
    public interface IAccountsPaidForAttorneyInvoicesDataManager
    {
        void InsertRecord(Guid thirdPartyId, int thirdPartyInvoiceKey, int accountKey);
        int GetDistinctCountOfAccountsForAttorney(Guid guid);
        void ClearAccountsPaidForAttorneyInvoicesMonthly();
    }
}
