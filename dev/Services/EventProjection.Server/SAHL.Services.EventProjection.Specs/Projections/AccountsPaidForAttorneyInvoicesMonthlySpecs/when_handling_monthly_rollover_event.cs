using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.EventProjection.Managers.AccountsPaidForAttorneyInvoicesMonthly;
using SAHL.Services.EventProjection.Projections.AccountsPaidForThirdPartyInvoicesMonthly;
using System;

namespace SAHL.Services.EventProjection.Specs.Projections.AccountsPaidForAttorneyInvoicesMonthlySpecs
{
    public class when_handling_monthly_rollover_event : WithFakes
    {
        static ClearAccountsPaidForThirdPartyInvoicesMonthlyHandler handler;
        static IAccountsPaidForAttorneyInvoicesDataManager accountsPaidForThirdPartyInvoicesDataManager;
        static SAHL.Services.Interfaces.Calendar.Events.FirstDayOfTheMonthEvent @event { get; set; }

        Establish context = () =>
        {
            accountsPaidForThirdPartyInvoicesDataManager = An<IAccountsPaidForAttorneyInvoicesDataManager>();
            handler = new ClearAccountsPaidForThirdPartyInvoicesMonthlyHandler(accountsPaidForThirdPartyInvoicesDataManager);
            @event = new Interfaces.Calendar.Events.FirstDayOfTheMonthEvent(DateTime.Now);
        };

        Because of = () =>
        {
            handler.Handle(@event, null);
        };

        It should_clear_the_projection_table = () =>
        {
            accountsPaidForThirdPartyInvoicesDataManager.WasToldTo(x => x.ClearAccountsPaidForAttorneyInvoicesMonthly());
        };

    }
}
