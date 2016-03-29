using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceApprovedHandler : ITableProjector<ThirdPartyInvoiceApprovedEvent, IDataModel>
    {
        private IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;

        public ThirdPartyInvoiceApprovedHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
        }

        public void Handle(ThirdPartyInvoiceApprovedEvent @event, IServiceRequestMetadata metadata)
        {
            attorneyInvoiceMonthlyBreakDownManager.DecrementUnProcessedCountForAttorney(@event.ApprovedThirdPartyInvoice.ThirdPartyId);
            attorneyInvoiceMonthlyBreakDownManager.IncrementProcessedCountForAttorney(@event.ApprovedThirdPartyInvoice.ThirdPartyId);
        }
    }
}