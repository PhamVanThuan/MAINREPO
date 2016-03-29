using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisYear;
using SAHL.Services.Interfaces.FinanceDomain.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedThisYear
{
    public class InvoicesNotProcessedThisYearCapturedHandler : ITableProjector<ThirdPartyInvoiceCapturedEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedThisYearDataManager dataManager;

        public InvoicesNotProcessedThisYearCapturedHandler(IAttorneyInvoicesNotProcessedThisYearDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void Handle(ThirdPartyInvoiceCapturedEvent @event, IServiceRequestMetadata metadata)
        {
            decimal invoiceTotal = @event.ThirdPartyInvoiceModel.TotalAmountIncludingVAT;
            this.dataManager.IncrementCountAndIncreaseYearlyValue(invoiceTotal);
        }
    }
}