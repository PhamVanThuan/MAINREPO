using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Managers.AttorneyInvoicesPaidThisYear;
using SAHL.Services.EventProjection.Projections.AttorneyInvoicesPaidThisYear;
using SAHL.Services.Interfaces.Calendar.Events;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoicesPaidThisYearSpecs
{
    public class when_it_is_the_first_day_of_the_year : WithFakes
    {
        private static ClearAttorneyInvoicesPaidThisYearHandler handler;
        private static IAttorneyInvoicesPaidThisYearDataManager dataManager;

        private static FirstDayOfTheYearEvent @event { get; set; }

        private Establish context = () =>
        {
            dataManager = An<IAttorneyInvoicesPaidThisYearDataManager>();
            handler = new ClearAttorneyInvoicesPaidThisYearHandler(dataManager);
            @event = new FirstDayOfTheYearEvent(DateTime.Now);
        };

        private Because of = () =>
        {
            handler.Handle(@event, null);
        };

        private It should_clear_the_projection_table = () =>
        {
            dataManager.WasToldTo(x => x.ClearAttorneyInvoicesPaidThisYear());
        };
    }
}