using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceCapturedHandler : ITableProjector<ThirdPartyInvoiceCapturedEvent, IDataModel>
    {
        private IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;

        public ThirdPartyInvoiceCapturedHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
        }

        public void Handle(ThirdPartyInvoiceCapturedEvent @event, IServiceRequestMetadata metadata)
        {
            attorneyInvoiceMonthlyBreakDownManager.EnsureProjectionRecordIsCreatedForAttorney(@event.ThirdPartyInvoiceModel.ThirdPartyId);
            attorneyInvoiceMonthlyBreakDownManager.IncrementUnProcessedCountForAttorney(@event.ThirdPartyInvoiceModel.ThirdPartyId);
        }
    }
}