using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisMonth
{
    public class InvoicesNotProcessedThisMonthCapturedHandler : ITableProjector<ThirdPartyInvoiceCapturedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager;

        public InvoicesNotProcessedThisMonthCapturedHandler(IAttorneyInvoicesNotProcessedThisMonthDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Handle(ThirdPartyInvoiceCapturedEvent @event, IServiceRequestMetadata metadata)
        {
            decimal invoiceTotal = @event.ThirdPartyInvoiceModel.TotalAmountIncludingVAT;
            this.dataManager.IncrementCountAndIncreaseMonthlyValue(invoiceTotal);
        }
    }
}