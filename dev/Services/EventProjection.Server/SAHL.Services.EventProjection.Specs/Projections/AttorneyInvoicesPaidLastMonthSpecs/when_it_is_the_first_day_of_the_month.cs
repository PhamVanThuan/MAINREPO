using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidLastMonth;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisMonth;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidLastMonth;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesPaidLastMonthSpecs
{
    public class when_it_is_the_first_day_of_the_month : WithFakes
    {
        private static UpdateInvoicesPaidLastMonthHandler handler;
        private static IAttorneyInvoicesPaidLastMonthDataManager attorneyInvoicesPaidLastMonthDataManager;
        private static IAttorneyInvoicesPaidThisMonthDataManager attorneyInvoicesPaidThisMonthDataManager;
        private static AttorneyInvoicesPaidThisMonthDataModel attorneyInvoicesPaidThisMonthDataModel;

        private static SAHL.Services.Interfaces.Calendar.Events.FirstDayOfTheMonthEvent @event { get; set; }

        private Establish context = () =>
        {
            attorneyInvoicesPaidThisMonthDataModel = new AttorneyInvoicesPaidThisMonthDataModel(11, 5500);
            attorneyInvoicesPaidLastMonthDataManager = An<IAttorneyInvoicesPaidLastMonthDataManager>();
            attorneyInvoicesPaidThisMonthDataManager = An<IAttorneyInvoicesPaidThisMonthDataManager>();
            handler = new UpdateInvoicesPaidLastMonthHandler(attorneyInvoicesPaidLastMonthDataManager, attorneyInvoicesPaidThisMonthDataManager);
            @event = new Interfaces.Calendar.Events.FirstDayOfTheMonthEvent(DateTime.Now);

            attorneyInvoicesPaidThisMonthDataManager.WhenToldTo(a => a.GetAttorneyInvoicesPaidThisMonthStatement())
                 .Return(attorneyInvoicesPaidThisMonthDataModel);
        };

        private Because of = () =>
        {
            handler.Handle(@event, null);
        };

        private It should_paid_invoices = () =>
        {
            attorneyInvoicesPaidThisMonthDataManager.WasToldTo(x => x.GetAttorneyInvoicesPaidThisMonthStatement());
        };

        private It should_update_invoices_paid_last_month = () =>
        {
            attorneyInvoicesPaidLastMonthDataManager.WasToldTo(x => x.UpdateAttorneyInvoicesPaidLastMonth(attorneyInvoicesPaidThisMonthDataModel));
        };
    }
}