using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceAmendedHandler : ITableProjector<ThirdPartyInvoiceAmendedEvent, IDataModel>
    {
        private IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;

        public ThirdPartyInvoiceAmendedHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
        }

        public void Handle(ThirdPartyInvoiceAmendedEvent @event, IServiceRequestMetadata metadata)
        {
            if (@event.AmmendedThirdPartyInvoice.ThirdPartyId != @event.OriginalThirdPartyInvoice.ThirdPartyId)
            {
                attorneyInvoiceMonthlyBreakDownManager.EnsureProjectionRecordIsCreatedForAttorney(@event.AmmendedThirdPartyInvoice.ThirdPartyId);
                attorneyInvoiceMonthlyBreakDownManager.DecrementUnProcessedCountForAttorney(@event.OriginalThirdPartyInvoice.ThirdPartyId.Value);
                attorneyInvoiceMonthlyBreakDownManager.IncrementUnProcessedCountForAttorney(@event.AmmendedThirdPartyInvoice.ThirdPartyId);
            }
        }
    }
}