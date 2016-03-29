using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesNotProcessedThisMonth;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesNotProcessedLastMonth;
using SAHL.Services.Interfaces.Calendar.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesNotProcessedLastMonthSpecs
{
    public class when_it_is_the_first_day_of_the_month : WithFakes
    {
        private static UpdateInvoicesNotProcessedLastMonthHandler handler;
        private static IAttorneyInvoicesNotProcessedLastMonthDataManager invoicesNotProcessedLastMonthDataManager;
        private static IAttorneyInvoicesNotProcessedThisMonthDataManager invoicesNotProcessedThisMonthDataManager;
        private static AttorneyInvoicesNotProcessedThisMonthDataModel attorneyInvoicesPaidThisMonthDataModel;

        private static FirstDayOfTheMonthEvent @event { get; set; }

        private static IServiceRequestMetadata metadata;

        private Establish context = () =>
         {
             metadata = An<IServiceRequestMetadata>();
             attorneyInvoicesPaidThisMonthDataModel = new AttorneyInvoicesNotProcessedThisMonthDataModel(12, 2500);
             invoicesNotProcessedLastMonthDataManager = An<IAttorneyInvoicesNotProcessedLastMonthDataManager>();
             invoicesNotProcessedThisMonthDataManager = An<IAttorneyInvoicesNotProcessedThisMonthDataManager>();
             handler = new UpdateInvoicesNotProcessedLastMonthHandler(invoicesNotProcessedLastMonthDataManager
                 , invoicesNotProcessedThisMonthDataManager);
             @event = new FirstDayOfTheMonthEvent(DateTime.Now);

             invoicesNotProcessedThisMonthDataManager.WhenToldTo(a => a.GetAttorneyInvoicesNotProcessedThisMonth())
                  .Return(attorneyInvoicesPaidThisMonthDataModel);
         };

        private Because of = () =>
        {
            handler.Handle(@event, metadata);
        };

        private It should_get_not_processed_invoices = () =>
        {
            invoicesNotProcessedThisMonthDataManager.WasToldTo(x => x.GetAttorneyInvoicesNotProcessedThisMonth());
        };

        private It should_update_invoices_not_processed_last_mont = () =>
        {
            invoicesNotProcessedLastMonthDataManager.WasToldTo(x => x.UpdateAttorneyInvoicesNotProcessedLastMonth(attorneyInvoicesPaidThisMonthDataModel));
        };
    }
}