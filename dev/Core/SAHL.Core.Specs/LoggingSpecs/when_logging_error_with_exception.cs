using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_error_with_exception : WithFakes
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
        private static Exception exception;

        private Establish context = () =>
            {
                exception = new Exception("TestException");
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
            logger.LogErrorWithException(loggerSource, user, method, message, exception, parameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            rawLogger.WasToldTo(x => x.LogErrorWithException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, message, exception, parameters));
        };
    }
}