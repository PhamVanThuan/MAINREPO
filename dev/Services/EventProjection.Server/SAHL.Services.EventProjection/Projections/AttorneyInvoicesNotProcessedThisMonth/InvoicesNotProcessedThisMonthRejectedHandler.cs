using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth
{
    public class InvoicesNotProcessedThisMonthRejectedHandler : ITableProjector<ThirdPartyInvoiceRejectedPreApprovalEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;

        public InvoicesNotProcessedThisMonthRejectedHandler(IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager, IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager)
        {
            this.dataManager = dataManager;
            this.breakdownDataManager = breakdownDataManager;
        }

        public void Handle(ThirdPartyInvoiceRejectedPreApprovalEvent @event, IServiceRequestMetadata metadata)
        {
            if (@event.ThirdPartyId.HasValue && !@event.ThirdPartyId.GetValueOrDefault().Equals(Guid.Empty))
            {
                var thirdPartyInvoice = this.breakdownDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
                decimal invoiceTotal = thirdPartyInvoice.TotalAmountIncludingVAT.GetValueOrDefault();
                this.dataManager.DecrementCountAndDecreaseMonthlyValue(invoiceTotal);
            }
        }
    }
}