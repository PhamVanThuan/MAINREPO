using System;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown
{
    public interface IAttorneyInvoiceMonthlyBreakdownManager
    {
        void DecrementProcessedCountForAttorney(Guid attorneyId);

        void IncrementProcessedCountForAttorney(Guid attorneyId);

        void ClearAttorneyInvoiceMonthlyBreakdownManagerTable();

        void DecrementUnProcessedCountForAttorney(Guid attorneyId);

        void IncrementUnProcessedCountForAttorney(Guid attorneyId);

        void IncrementRejectedCountForAttorney(Guid attorneyId);

        void EnsureProjectionRecordIsCreatedForAttorney(Guid attorneyId);
        bool IsAccountUnderDebtCounselling(int accountKey);
    }
}