using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.Interfaces.Calendar.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidLastMonth
{
    public class UpdateInvoicesPaidLastMonthHandler : ITableProjector<FirstDayOfTheMonthEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesPaidLastMonthDataManager attorneyInvoicesPaidLastMonthDataManager;
        private readonly IAttorneyInvoicesPaidThisMonthDataManager attorneyInvoicesPaidThisMonthDataManager;

        public UpdateInvoicesPaidLastMonthHandler(IAttorneyInvoicesPaidLastMonthDataManager attorneyInvoicesPaidLastMonthDataManager
            , IAttorneyInvoicesPaidThisMonthDataManager attorneyInvoicesPaidThisMonthDataManager)
        {
            this.attorneyInvoicesPaidLastMonthDataManager = attorneyInvoicesPaidLastMonthDataManager;
            this.attorneyInvoicesPaidThisMonthDataManager = attorneyInvoicesPaidThisMonthDataManager;
        }

        public void Handle(FirstDayOfTheMonthEvent @event, IServiceRequestMetadata metadata)
        {
            var invoicesPaidLastMonth = attorneyInvoicesPaidThisMonthDataManager.GetAttorneyInvoicesPaidThisMonthStatement();
            if (invoicesPaidLastMonth == null)
            {
                invoicesPaidLastMonth = new AttorneyInvoicesPaidThisMonthDataModel(0, 0);
            }
            attorneyInvoicesPaidLastMonthDataManager.UpdateAttorneyInvoicesPaidLastMonth(invoicesPaidLastMonth);
            attorneyInvoicesPaidThisMonthDataManager.ClearAttorneyInvoicesPaidThisMonthStatement();
        }
    }
}