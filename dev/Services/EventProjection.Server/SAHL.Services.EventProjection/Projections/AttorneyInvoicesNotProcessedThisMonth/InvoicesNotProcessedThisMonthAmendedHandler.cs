using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth
{
    public class InvoicesNotProcessedThisMonthAmendedHandler : ITableProjector<ThirdPartyInvoiceAmendedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;

        public InvoicesNotProcessedThisMonthAmendedHandler(IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager)
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
                this.dataManager.AdjustMonthlyValue(adjustmentRequired);
            }
        }
    }
}