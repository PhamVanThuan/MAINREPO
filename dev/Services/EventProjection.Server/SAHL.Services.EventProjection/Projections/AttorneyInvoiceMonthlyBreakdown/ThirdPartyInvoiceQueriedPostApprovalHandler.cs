using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ThirdPartyInvoiceQueriedPostApprovalHandler : ITableProjector<ThirdPartyInvoiceQueriedPostApprovalEvent, IDataModel>
    {
        private readonly IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakDownDataManager;

        public ThirdPartyInvoiceQueriedPostApprovalHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager,
            IAttorneyInvoiceMonthlyBreakdownDataManager attorneyInvoiceMonthlyBreakDownDataManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
            this.attorneyInvoiceMonthlyBreakDownDataManager = attorneyInvoiceMonthlyBreakDownDataManager;
        }

        public void Handle(ThirdPartyInvoiceQueriedPostApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            Guid thirdPartyId = this.attorneyInvoiceMonthlyBreakDownDataManager.GetThirdPartyIdByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
            this.attorneyInvoiceMonthlyBreakDownManager.DecrementProcessedCountForAttorney(thirdPartyId);
            this.attorneyInvoiceMonthlyBreakDownManager.IncrementUnProcessedCountForAttorney(thirdPartyId);
        }
    }
}