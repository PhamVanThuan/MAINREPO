using SAHL.Core.Data;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.Interfaces.Calendar.Events;

namespace SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedLastMonth
{
    public class UpdateInvoicesNotProcessedLastMonthHandler : ITableProjector<FirstDayOfTheMonthEvent, IDataModel>
    {
        private readonly IAttorneyInvoicesNotProcessedLastMonthDataManager attorneyInvoicesNotProcessedLastMonthDataManager;
        private readonly IAttorneyInvoicesNotProcessedThisMonthDataManager attorneyInvoicesNotProcessedThisMonthDataManager;

        public UpdateInvoicesNotProcessedLastMonthHandler(IAttorneyInvoicesNotProcessedLastMonthDataManager attorneyInvoicesNotProcessedLastMonthDataManager
            , IAttorneyInvoicesNotProcessedThisMonthDataManager attorneyInvoicesNotProcessedThisMonthDataManager)
        {
            this.attorneyInvoicesNotProcessedLastMonthDataManager = attorneyInvoicesNotProcessedLastMonthDataManager;
            this.attorneyInvoicesNotProcessedThisMonthDataManager = attorneyInvoicesNotProcessedThisMonthDataManager;
        }

        public void Handle(FirstDayOfTheMonthEvent @event, IServiceRequestMetadata metadata)
        {
            var invoicesNotProcessedThisMonth = attorneyInvoicesNotProcessedThisMonthDataManager.GetAttorneyInvoicesNotProcessedThisMonth();
            if (invoicesNotProcessedThisMonth == null)
            {
                invoicesNotProcessedThisMonth = new AttorneyInvoicesNotProcessedThisMonthDataModel(0, 0);
            }
            attorneyInvoicesNotProcessedLastMonthDataManager.UpdateAttorneyInvoicesNotProcessedLastMonth(invoicesNotProcessedThisMonth);
        }
    }
}