using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Specs.Fakes;
using System.Collections.Generic;

namespace SAHL.Core.Logging.Loggers.Database.Specs
{
    public class when_logging_debug : WithFakes
    {
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static IRawLogger rawLogger;
        private static IRawMetricsLogger rawMetricsLogger;
        private static IMetricTimerFactory metricsTimerFactory;
        private static string user;
        private static string method;
        private static string message;
        private static IDictionary<string, object> parameters;

        private Establish context = () =>
            {
                parameters = new Dictionary<string, object>();
                user = "testuser";
                method = "testmethod";
                message = "testmessage";
                rawLogger = An<IRawLogger>();
                rawMetricsLogger = An<IRawMetricsLogger>();
                metricsTimerFactory = An<IMetricTimerFactory>();
                loggerSource = new LoggerSource("test", LogLevel.Info, true);
                loggerAppSource = new FakeLoggerAppSource("testAppName");

                logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
            };

        private Because of = () =>
        {
            logger.LogDebug(loggerSource, user, method, message, parameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            rawLogger.WasToldTo(x => x.LogDebug(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, message, parameters));
        };
    }
}