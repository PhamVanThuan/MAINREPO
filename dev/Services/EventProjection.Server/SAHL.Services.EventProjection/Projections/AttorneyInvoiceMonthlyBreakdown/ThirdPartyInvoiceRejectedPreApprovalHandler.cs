using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceRejectedPreApprovalHandler : ITableProjector<ThirdPartyInvoiceRejectedPreApprovalEvent, IDataModel>
    {
        private readonly IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;

        public ThirdPartyInvoiceRejectedPreApprovalHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
        }

        public void Handle(ThirdPartyInvoiceRejectedPreApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            if (@event.ThirdPartyId.HasValue && !@event.ThirdPartyId.GetValueOrDefault().Equals(Guid.Empty))
            {
                this.attorneyInvoiceMonthlyBreakDownManager.IncrementRejectedCountForAttorney(@event.ThirdPartyId.Value);
                this.attorneyInvoiceMonthlyBreakDownManager.DecrementUnProcessedCountForAttorney(@event.ThirdPartyId.Value);
            }
        }
    }
}