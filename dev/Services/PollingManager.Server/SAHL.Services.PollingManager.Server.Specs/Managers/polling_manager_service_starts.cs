using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ExchangeManager.Server.Specs.Managers
{
    public class polling_manager_service_starts : WithFakes
    {
        private static IEnumerable<IPolledHandler> polledHandlers;
        private static PollingManagerService pollingManagerService;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;

        private Establish context = () =>
        {
            IRawLogger rawLogger = An<IRawLogger>();
            ILoggerSource loggerSource = An<ILoggerSource>();
            ILoggerAppSource loggerAppSource = An<ILoggerAppSource>();
            IPolledHandler polledHandler = An<IPolledHandler>();
            List<IPolledHandler> polledList = new List<IPolledHandler>();
            polledList.Add(polledHandler);
            polledHandlers = polledList;
            pollingManagerService = new PollingManagerService(polledHandlers, rawLogger, loggerSource, loggerAppSource);
        };

        private Because of = () =>
        {
            pollingManagerService.Start();
        };

        private It should_initialise = () =>
        {
            polledHandlers.FirstOrDefault().WasToldTo(x => x.Initialise());
        };

        private It should_call_start_for_the_polledHandler = () =>
        {
            polledHandlers.FirstOrDefault().WasToldTo(x => x.Start());
        };
    }
}