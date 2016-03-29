using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AccountsPaidForAttorneyInvoicesMonthly
{
    public class AttorneyInvoicesPaidThisMonthHandler : ITableProjector<ThirdPartyInvoiceMarkedAsPaidEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesPaidThisMonthDataManager invoicesPaidThisMonthDataManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;

        public AttorneyInvoicesPaidThisMonthHandler(IAttorneyInvoicesPaidThisMonthDataManager invoicesPaidThisMonthDataManager, IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager)
        {
            this.invoicesPaidThisMonthDataManager = invoicesPaidThisMonthDataManager;
            this.breakdownDataManager = breakdownDataManager;
        }

        public void Handle(ThirdPartyInvoiceMarkedAsPaidEvent @event, IServiceRequestMetadata metadata)
        {
            ThirdPartyInvoiceDataModel invoice = this.breakdownDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            decimal invoiceTotalValue = invoice.TotalAmountIncludingVAT.GetValueOrDefault();
            this.invoicesPaidThisMonthDataManager.IncrementCountAndAddInvoiceToValueColumn(invoiceTotalValue);
        }
    }
}