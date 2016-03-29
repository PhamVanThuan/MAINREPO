using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager;
using System.Collections.Generic;

namespace SAHL.Services.ExchangeManager.Server.Specs.Managers
{
    public class polling_manager_with_handler_starts : WithFakes
    {
        private static IPolledHandler lossControlPolledExchangeHandler;
        private static PollingManagerService pollingManagerService;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            IRawLogger rawLogger = An<IRawLogger>();
            ILoggerSource loggerSource = An<ILoggerSource>();
            ILoggerAppSource loggerAppSource = An<ILoggerAppSource>();
            lossControlPolledExchangeHandler = An<IPolledHandler>();

            pollingManagerService = new PollingManagerService(
                  new List<IPolledHandler> { lossControlPolledExchangeHandler }
                , rawLogger
                , loggerSource
                , loggerAppSource
            );
        };

        private Because of = () =>
        {
            pollingManagerService.Start();
        };

        private It should_call_start_for_the_polledHandler = () =>
        {
            lossControlPolledExchangeHandler.WasToldTo(x => x.Start());
        };
    }
}