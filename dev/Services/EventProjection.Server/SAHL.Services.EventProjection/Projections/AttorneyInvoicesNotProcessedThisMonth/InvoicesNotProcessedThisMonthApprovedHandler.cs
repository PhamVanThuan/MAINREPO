using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth
{
    public class InvoicesNotProcessedThisMonthApprovedHandler : ITableProjector<ThirdPartyInvoiceApprovedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;

        public InvoicesNotProcessedThisMonthApprovedHandler(IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Handle(ThirdPartyInvoiceApprovedEvent @event, IServiceRequestMetadata metadata)
        {
            decimal invoiceTotal = @event.ApprovedThirdPartyInvoice.TotalAmountIncludingVAT;
            this.dataManager.DecrementCountAndDecreaseMonthlyValue(invoiceTotal);
        }
    }
}