using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear;
using SAHL.Services.Interfaces.Calendar.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidThisYear
{
    public class ClearAttorneyInvoicesPaidThisYearHandler : ITableProjector<FirstDayOfTheYearEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesPaidThisYearDataManager invoicesPaidThisYearDataManager;

        public ClearAttorneyInvoicesPaidThisYearHandler(IAttorneyInvoicesPaidThisYearDataManager invoicesPaidThisYearDataManager)
        {
            this.invoicesPaidThisYearDataManager = invoicesPaidThisYearDataManager;
        }

        public void Handle(FirstDayOfTheYearEvent @event, IServiceRequestMetadata metadata)
        {
            invoicesPaidThisYearDataManager.ClearAttorneyInvoicesPaidThisYear();
        }
    }
}