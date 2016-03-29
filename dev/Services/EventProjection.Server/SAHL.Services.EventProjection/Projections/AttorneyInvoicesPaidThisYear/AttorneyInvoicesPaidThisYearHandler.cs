using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidThisYear
{
    public class AttorneyInvoicesPaidThisYearHandler : ITableProjector<ThirdPartyInvoiceMarkedAsPaidEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesPaidThisYearDataManager invoicesPaidThisYearDataManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;

        public AttorneyInvoicesPaidThisYearHandler(IAttorneyInvoicesPaidThisYearDataManager invoicesPaidThisYearDataManager, IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager)
        {
            this.invoicesPaidThisYearDataManager = invoicesPaidThisYearDataManager;
            this.breakdownDataManager = breakdownDataManager;
        }

        public void Handle(ThirdPartyInvoiceMarkedAsPaidEvent @event, IServiceRequestMetadata metadata)
        {
            ThirdPartyInvoiceDataModel invoice = this.breakdownDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            decimal invoiceTotalValue = invoice.TotalAmountIncludingVAT.GetValueOrDefault();
            this.invoicesPaidThisYearDataManager.IncrementCountAndAddInvoiceToValueColumn(invoiceTotalValue);
        }
    }
}