using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.Calendar.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown
{
    public class ClearAttorneyInvoiceMonthlyBreakdownHandler : ITableProjector<FirstDayOfTheMonthEvent, IDataModel>
    {
        private IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;

        public ClearAttorneyInvoiceMonthlyBreakdownHandler(IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager)
        {
            this.attorneyInvoiceMonthlyBreakDownManager = attorneyInvoiceMonthlyBreakDownManager;
        }

        public void Handle(FirstDayOfTheMonthEvent @event, IServiceRequestMetadata metadata)
        {
            attorneyInvoiceMonthlyBreakDownManager.ClearAttorneyInvoiceMonthlyBreakdownManagerTable();
        }
    }
}