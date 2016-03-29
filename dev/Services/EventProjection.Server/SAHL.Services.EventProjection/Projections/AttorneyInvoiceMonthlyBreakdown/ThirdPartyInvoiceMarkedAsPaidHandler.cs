using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceMarkedAsPaidHandler : ITableProjector<ThirdPartyInvoiceMarkedAsPaidEvent, IDataModel>
    {
        private readonly IAttorneyInvoiceMonthlyBreakdownManager monthlyBreakdownManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakdownManagerDataManager;

        public ThirdPartyInvoiceMarkedAsPaidHandler(IAttorneyInvoiceMonthlyBreakdownManager monthlyBreakdownManager,
            IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakdownManagerDataManager)
        {
            this.monthlyBreakdownManager = monthlyBreakdownManager;
            this.monthlyBreakdownManagerDataManager = monthlyBreakdownManagerDataManager;
        }

        public void Handle(ThirdPartyInvoiceMarkedAsPaidEvent @event, IServiceRequestMetadata metadata)
        {
            decimal debtReview = 0.00M;
            decimal capitalised = 0.00M;
            decimal paidBySpv = 0.00M;
            ThirdPartyInvoiceDataModel thirdPartyInvoice = monthlyBreakdownManagerDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            bool invoiceToBeCapitalised = thirdPartyInvoice.CapitaliseInvoice.GetValueOrDefault();
            decimal invoiceTotalValue = thirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();
            bool underDebtCounselling = monthlyBreakdownManager.IsAccountUnderDebtCounselling(thirdPartyInvoice.AccountKey);
            Guid thirdPartyId = thirdPartyInvoice.ThirdPartyId.GetValueOrDefault();

            if (underDebtCounselling)
            {
                debtReview = invoiceTotalValue;
            }
            if (invoiceToBeCapitalised)
            {
                capitalised = invoiceTotalValue;
            }
            else
            {
                paidBySpv = invoiceTotalValue;
            }
            this.monthlyBreakdownManager.DecrementProcessedCountForAttorney(thirdPartyId);
            this.monthlyBreakdownManagerDataManager.IncrementPaidCount(thirdPartyId);
            this.monthlyBreakdownManagerDataManager.UpdatePaymentFieldsForAttorney(thirdPartyId, debtReview, paidBySpv, capitalised);
        }
    }
}