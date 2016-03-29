using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown
{
    public interface IAttorneyInvoiceMonthlyBreakdownDataManager
    {
        void AdjustUnprocessedCount(Guid thirdpartyId, int valueToAdd);

        void AdjustProcessedCount(Guid thirdpartyId, int valueToAdd);

        void ClearAttorneyInvoiceMonthlyBreakdownManagerTable();

        void MergeAttorneyMonthlyBreakdownRecordForAttorney(Guid thirdpartyId, string attorneyRegisteredName);

        string GetRegisteredNameForAttorney(Guid thirdpartyId);

        void IncrementRejectedCount(Guid thirdpartyId);

        Guid GetThirdPartyIdByThirdPartyInvoiceKey(int thirdPartyInvoiceKey);

        void IncrementPaidCount(Guid thirdPartyId);

        ThirdPartyInvoiceDataModel GetThirdPartyInvoiceByThirdPartyInvoiceKey(int thirdPartyInvoiceKey);

        DebtCounsellingDataModel GetOpenDebtCounsellingByAccountKey(int accountKey);

        void UpdatePaymentFieldsForAttorney(Guid attorneyId, decimal debtReview, decimal paidBySPV, decimal capitalised);

        void AdjustAccountsPaidCount(Guid guid, int value);
    }
}