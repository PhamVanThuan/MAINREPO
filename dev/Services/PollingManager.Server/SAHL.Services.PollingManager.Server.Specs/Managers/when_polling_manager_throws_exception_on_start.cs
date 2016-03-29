using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Services.Interfaces.PollingManager;

namespace SAHL.Services.PollingManager.Server.Specs.Managers
{
    public class when_polling_manager_throws_exception_on_start : WithFakes
    {
        private static IEnumerable<IPolledHandler> polledHandlers;
        private static PollingManagerService pollingManagerService;
        private static IRawLogger rawLogger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static Exception ex;
        private static IPolledHandler polledHandler;
        private static string exceptionMessage;

        private Establish context = () =>
        {
            exceptionMessage = "Polled Handler threw exception when starting";
            rawLogger = An<IRawLogger>();
            loggerSource = An<ILoggerSource>();
            loggerAppSource = An<ILoggerAppSource>();
            polledHandler = An<IPolledHandler>();
            List<IPolledHandler> polledList = new List<IPolledHandler>();
            polledList.Add(polledHandler);
            polledHandlers = polledList;
            pollingManagerService = new PollingManagerService(polledHandlers, rawLogger, loggerSource, loggerAppSource);
            polledHandler.WhenToldTo(x => x.Start()).Throw(new Exception(exceptionMessage));
        };

        private Because of = () =>
        {
            ex = Catch.Exception(()=>pollingManagerService.Start());
        };

        private It should_initialise = () =>
        {
            polledHandlers.FirstOrDefault().WasToldTo(x => x.Initialise());
        };

        private It should_call_start_for_the_polledHandler = () =>
        {
            polledHandlers.FirstOrDefault().WasToldTo(x => x.Start());
        };

        private It should_log_the_exception = () =>
        {
            rawLogger.WasToldTo(x => x.LogErrorWithException(LogLevel.StartUp, Arg.Any<string>(), Arg.Any<string>(),
                "Polling Manager", Arg.Any<string>(), Arg.Any<string>(), 
                Arg.Is<Exception>(y=>y.Message == exceptionMessage), null));
        };
    }
}
