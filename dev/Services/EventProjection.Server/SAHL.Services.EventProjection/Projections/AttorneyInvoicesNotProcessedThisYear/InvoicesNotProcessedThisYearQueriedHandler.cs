using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisYear
{
    public class InvoicesNotProcessedThisYearQueriedHandler : ITableProjector<ThirdPartyInvoiceQueriedPostApprovalEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisYearDataManager dataManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;

        public InvoicesNotProcessedThisYearQueriedHandler(IAttorneyInvoicesNotProcessedThisYearDataManager dataManager, IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager)
        {
            this.dataManager = dataManager;
            this.breakdownDataManager = breakdownDataManager;
        }

        public void Handle(ThirdPartyInvoiceQueriedPostApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            var thirdPartyInvoice = this.breakdownDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            decimal invoiceTotal = thirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();
            this.dataManager.IncrementCountAndIncreaseYearlyValue(invoiceTotal);
        }
    }
}