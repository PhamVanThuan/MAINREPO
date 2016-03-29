using SAHL.Core.Data;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Projections.AccountsPaidForAttorneyInvoicesMonthly
{
    public class ThirdPartyInvoiceMarkedAsPaidAccountsPaidHandler : ITableProjector<ThirdPartyInvoiceMarkedAsPaidEvent, IDataModel>
    {
        private readonly IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakdownManagerDataManager;
        private readonly IAccountsPaidForAttorneyInvoicesDataManager accountsPaidForThirdPartyInvoicesDataManager;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ThirdPartyInvoiceMarkedAsPaidAccountsPaidHandler(IAttorneyInvoiceMonthlyBreakdownDataManager monthlyBreakdownManagerDataManager,
            IAccountsPaidForAttorneyInvoicesDataManager accountsPaidForThirdPartyInvoicesDataManager, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.monthlyBreakdownManagerDataManager = monthlyBreakdownManagerDataManager;
            this.accountsPaidForThirdPartyInvoicesDataManager = accountsPaidForThirdPartyInvoicesDataManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Handle(ThirdPartyInvoiceMarkedAsPaidEvent @event, IServiceRequestMetadata metadata)
        {
            using (var uow = unitOfWorkFactory.Build())
            {
                var invoice = monthlyBreakdownManagerDataManager.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey);
                Guid attorneyId = invoice.ThirdPartyId.GetValueOrDefault();
                this.accountsPaidForThirdPartyInvoicesDataManager.InsertRecord(attorneyId, @event.ThirdPartyInvoiceKey, invoice.AccountKey);
                int countOfAccounts = this.accountsPaidForThirdPartyInvoicesDataManager.GetDistinctCountOfAccountsForAttorney(attorneyId);
                this.monthlyBreakdownManagerDataManager.AdjustAccountsPaidCount(attorneyId, countOfAccounts);
                uow.Complete();
            }
        }
    }
}