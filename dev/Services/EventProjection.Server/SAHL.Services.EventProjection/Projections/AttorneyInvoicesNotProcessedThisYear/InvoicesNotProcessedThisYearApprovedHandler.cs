using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisYear
{
    public class InvoicesNotProcessedThisYearApprovedHandler : ITableProjector<ThirdPartyInvoiceApprovedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisYearDataManager dataManager;

        public InvoicesNotProcessedThisYearApprovedHandler(IAttorneyInvoicesNotProcessedThisYearDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Handle(ThirdPartyInvoiceApprovedEvent @event, IServiceRequestMetadata metadata)
        {
            decimal invoiceTotal = @event.ApprovedThirdPartyInvoice.TotalAmountIncludingVAT;
            this.dataManager.DecrementCountAndDecreaseYearlyValue(invoiceTotal);
        }
    }
}