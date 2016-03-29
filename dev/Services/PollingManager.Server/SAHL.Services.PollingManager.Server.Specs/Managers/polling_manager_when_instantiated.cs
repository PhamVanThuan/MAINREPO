using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager;

namespace SAHL.Services.ExchangeManager.Server.Specs.Managers
{
    public class polling_manager_when_instantiated : WithFakes
    {
        private static IPolledHandler lossControlPolledExchangeHandler;
        private static IPolledHandler anotherHandler;

        private Establish context = () =>
        {
            lossControlPolledExchangeHandler = An<IPolledHandler>();
            anotherHandler = An<IPolledHandler>();
        };

        private Because of = () =>
        {
            new PollingManagerService(
                new List<IPolledHandler> { lossControlPolledExchangeHandler, anotherHandler },
                null,
                null,
                null);
        };

        private It should_intialise_all_handlers = () =>
        {
            lossControlPolledExchangeHandler.WasToldTo(y => y.Initialise());
            anotherHandler.WasToldTo(y => y.Initialise());
        };
    }
}
