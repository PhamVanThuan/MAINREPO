using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.Interfaces.Calendar.Events;

namespace SAHL.Services.EventProjection.Projections.AccountsPaidForThirdPartyInvoicesMonthly
{
    public class ClearAccountsPaidForThirdPartyInvoicesMonthlyHandler : ITableProjector<FirstDayOfTheMonthEvent, IDataModel>
    {
        private readonly IAccountsPaidForAttorneyInvoicesDataManager accountsPaidForThirdPartyInvoicesDataManager;

        public ClearAccountsPaidForThirdPartyInvoicesMonthlyHandler(IAccountsPaidForAttorneyInvoicesDataManager accountsPaidForThirdPartyInvoicesDataManager)
        {
            this.accountsPaidForThirdPartyInvoicesDataManager = accountsPaidForThirdPartyInvoicesDataManager;
        }

        public void Handle(FirstDayOfTheMonthEvent @event, Core.Services.IServiceRequestMetadata metadata)
        {
            accountsPaidForThirdPartyInvoicesDataManager.ClearAccountsPaidForAttorneyInvoicesMonthly();
        }
    }
}