using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown;
using SAHL.Services.EventProjection.Projections.AttorneyInvoiceMonthlyBreakdown;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AttorneyInvoiceMonthlyBreakdownSpecs.MonthlyRollOverHandler
{
    public class when_handling_monthly_rollover_event : WithFakes
    {
        static ClearAttorneyInvoiceMonthlyBreakdownHandler handler;
        static IAttorneyInvoiceMonthlyBreakdownManager attorneyInvoiceMonthlyBreakDownManager;
        static SAHL.Services.Interfaces.Calendar.Events.FirstDayOfTheMonthEvent @event { get; set; }

        Establish context = () =>
        {
            attorneyInvoiceMonthlyBreakDownManager = An<IAttorneyInvoiceMonthlyBreakdownManager>();
            handler = new ClearAttorneyInvoiceMonthlyBreakdownHandler(attorneyInvoiceMonthlyBreakDownManager);
            @event = new Interfaces.Calendar.Events.FirstDayOfTheMonthEvent(DateTime.Now);
        };

        Because of = () =>
        {
            handler.Handle(@event, null);
        };

        It should_clear_the_projection_table = () =>
        {
            attorneyInvoiceMonthlyBreakDownManager.WasToldTo(x => x.ClearAttorneyInvoiceMonthlyBreakdownManagerTable());
        };

    }
}
