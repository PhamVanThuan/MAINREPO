using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.MetricsSpecs
{
    public class when_logging_throughputmetric_disabled : WithFakes
    {
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static IRawLogger rawLogger;
        private static IRawMetricsLogger rawMetricsLogger;
        private static IMetricTimerFactory metricsTimerFactory;
        private static string user;
        private static string method;
        private static DateTime startTime;
        private static TimeSpan duration;
        private static IDictionary<string, object> parameters;

        private Establish context = () =>
            {
                parameters = new Dictionary<string, object>();
                user = "testuser";
                method = "testmethod";
                startTime = DateTime.Now;
                duration = new TimeSpan(50000);
                rawLogger = An<IRawLogger>();
                rawMetricsLogger = An<IRawMetricsLogger>();
                metricsTimerFactory = An<IMetricTimerFactory>();

                loggerSource = new LoggerSource("test", LogLevel.Error, false);
                loggerAppSource = new FakeLoggerAppSource("testAppName");

                logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
            };

        private Because of = () =>
        {
            logger.LogThroughputMetric(loggerSource, user, method, parameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            rawMetricsLogger.WasNotToldTo(x => x.LogThroughputMetric(loggerSource.Name, loggerAppSource.ApplicationName, user, method, parameters));
        };
    }
}