using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisYear
{
    public class InvoicesNotProcessedThisYearAmendedHandler : ITableProjector<ThirdPartyInvoiceAmendedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisYearDataManager dataManager;

        public InvoicesNotProcessedThisYearAmendedHandler(IAttorneyInvoicesNotProcessedThisYearDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Handle(ThirdPartyInvoiceAmendedEvent @event, IServiceRequestMetadata metadata)
        {
            decimal newInvoiceTotal = @event.AmmendedThirdPartyInvoice.TotalAmountIncludingVAT;
            decimal oldInvoiceTotal = @event.OriginalThirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();
            if (newInvoiceTotal != oldInvoiceTotal)
            {
                decimal adjustmentRequired = newInvoiceTotal - oldInvoiceTotal;
                this.dataManager.AdjustYearlyValue(adjustmentRequired);
            }
        }
    }
}