using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth
{
    public class InvoicesNotProcessedThisMonthQueriedHandler : ITableProjector<ThirdPartyInvoiceQueriedPostApprovalEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;

        public InvoicesNotProcessedThisMonthQueriedHandler(IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager, IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager)
        {
            this.dataManager = dataManager;
            this.breakdownDataManager = breakdownDataManager;
        }

        public void Handle(ThirdPartyInvoiceQueriedPostApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            var thirdPartyInvoice = this.breakdownDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            decimal invoiceTotal = thirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();
            this.dataManager.IncrementCountAndIncreaseMonthlyValue(invoiceTotal);
        }
    }
}