using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AccountsPaidForAttorneyInvoicesMonthlySpecs
{
    public class when_handling_event : WithFakes
    {
        private static ThirdPartyInvoiceMarkedAsPaidEvent @event;
        private static ThirdPartyInvoiceMarkedAsPaidAccountsPaidHandler projector;
        private static IServiceRequestMetadata metadata;
        private static IAttorneyInvoiceMonthlyBreakdownDataManager breakdownDataManager;
        private static IAccountsPaidForAttorneyInvoicesDataManager dataManager;
        private static IUnitOfWorkFactory uowFactory;
        private static IUnitOfWork uow;
        private static ThirdPartyInvoiceDataModel thirdPartyInvoice;
        private static int countOfAccounts;

        private Establish that = () =>
        {
            uowFactory = An<IUnitOfWorkFactory>();
            uow = An<IUnitOfWork>();
            uowFactory.WhenToldTo(x => x.Build()).Return(uow);
            countOfAccounts = 10;
            thirdPartyInvoice = new ThirdPartyInvoiceDataModel(1, "sahl_reference", 1, 1428540, Guid.NewGuid(), "invoice_number", null, "clintons@sahomeloans.com", 5000.00M, 5000.00M, 5000.00M, true,
               DateTime.Now, "payment_reference");
            breakdownDataManager = An<IAttorneyInvoiceMonthlyBreakdownDataManager>();
            metadata = An<IServiceRequestMetadata>();
            dataManager = An<IAccountsPaidForAttorneyInvoicesDataManager>();
            @event = new ThirdPartyInvoiceMarkedAsPaidEvent(DateTime.Now, 12344);
            breakdownDataManager.WhenToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey))
                .Return(thirdPartyInvoice);
            dataManager.WhenToldTo(x => x.GetDistinctCountOfAccountsForAttorney(Arg.Any<Guid>()))
                .Return(countOfAccounts);
            projector = new ThirdPartyInvoiceMarkedAsPaidAccountsPaidHandler(breakdownDataManager, dataManager, uowFactory);
        };

        private Because of = () =>
        {
            projector.Handle(@event, metadata);
        };

        private It should_build_a_unit_of_work = () =>
        {
            uowFactory.WasToldTo(x => x.Build());
        };

        private It should_get_the_third_party_invoice_details = () =>
        {
            breakdownDataManager.WasToldTo(x => x.GetThirdPartyInvoiceByThirdPartyInvoiceKey(@event.ThirdPartyInvoiceKey));
        };

        private It should_insert_a_new_record_into_the_projection = () =>
        {
            dataManager.WasToldTo(x => x.InsertRecord(thirdPartyInvoice.ThirdPartyId.GetValueOrDefault(), @event.ThirdPartyInvoiceKey, thirdPartyInvoice.AccountKey));
        };

        private It should_get_a_distinct_count_of_accounts_for_the_attorney = () =>
        {
            dataManager.WasToldTo(x => x.GetDistinctCountOfAccountsForAttorney(thirdPartyInvoice.ThirdPartyId.GetValueOrDefault()));
        };

        private It should_update_the_accounts_paid_in_the_monthly_breakdown_projection = () =>
        {
            breakdownDataManager.WasToldTo(x => x.AdjustAccountsPaidCount(thirdPartyInvoice.ThirdPartyId.GetValueOrDefault(), countOfAccounts));
        };

        private It should_complete_the_uow = () =>
        {
            uow.WasToldTo(x => x.Complete()).OnlyOnce();
        };
    }
}