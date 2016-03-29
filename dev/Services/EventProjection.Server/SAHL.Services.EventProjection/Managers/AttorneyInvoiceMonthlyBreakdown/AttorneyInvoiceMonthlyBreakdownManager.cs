using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown
{
    public class AttorneyInvoiceMonthlyBreakdownManager : IAttorneyInvoiceMonthlyBreakdownManager
    {
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakDownDataManager;

        public AttorneyInvoiceMonthlyBreakdownManager(IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakDownManager)
        {
            this.monthlyBreakDownDataManager = monthlyBreakDownManager;
        }

        public void DecrementProcessedCountForAttorney(Guid attorneyId)
        {
            this.monthlyBreakDownDataManager.AdjustProcessedCount(attorneyId, -1);
        }

        public void ClearAttorneyInvoiceMonthlyBreakdownManagerTable()
        {
            this.monthlyBreakDownDataManager.ClearAttorneyInvoiceMonthlyBreakdownManagerTable();
        }

        public void IncrementProcessedCountForAttorney(Guid attorneyId)
        {
            monthlyBreakDownDataManager.AdjustProcessedCount(attorneyId, +1);
        }

        public void DecrementUnProcessedCountForAttorney(Guid attorneyId)
        {
            this.monthlyBreakDownDataManager.AdjustUnprocessedCount(attorneyId, -1);
        }

        public void IncrementUnProcessedCountForAttorney(Guid attorneyId)
        {
            this.monthlyBreakDownDataManager.AdjustUnprocessedCount(attorneyId, +1);
        }

        public void IncrementRejectedCountForAttorney(Guid attorneyId)
        {
            this.monthlyBreakDownDataManager.IncrementRejectedCount(attorneyId);
        }

        public void EnsureProjectionRecordIsCreatedForAttorney(Guid attorneyId)
        {
            var registeredName = this.monthlyBreakDownDataManager.GetRegisteredNameForAttorney(attorneyId);
            this.monthlyBreakDownDataManager.MergeAttorneyMonthlyBreakdownRecordForAttorney(attorneyId, registeredName);
        }

        public bool IsAccountUnderDebtCounselling(int accountKey)
        {
            DebtCounsellingDataModel debtCounselling = this.monthlyBreakDownDataManager.GetOpenDebtCounsellingByAccountKey(accountKey);
            if (debtCounselling != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}