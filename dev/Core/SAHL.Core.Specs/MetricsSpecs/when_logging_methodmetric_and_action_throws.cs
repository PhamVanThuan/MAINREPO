using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.MetricsSpecs
{
    public class when_logging_methodmetric_and_action_throws : WithFakes
    {
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static IRawLogger rawLogger;
        private static IRawMetricsLogger rawMetricsLogger;
        private static IMetricTimerFactory metricsTimerFactory;
        private static IMetricTimer timer;
        private static IMetricTimerResult timerResult;
        private static string user;
        private static string method;
        private static DateTime startTime;
        private static TimeSpan duration;
        private static IDictionary<string, object> parameters;
        private static Exception exception;

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
                timer = An<IMetricTimer>();
                timerResult = new MetricTimerResult(startTime, duration);

                metricsTimerFactory.WhenToldTo(x => x.NewTimer())
                    .Return(timer);
                timer.WhenToldTo(x => x.Stop())
                    .Return(timerResult);

                loggerSource = new LoggerSource("test", LogLevel.Error, true);
                loggerAppSource = new FakeLoggerAppSource("testAppName");

                logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
            };

        private Because of = () =>
        {
            exception = Catch.Exception(() => logger.LogMethodMetric(loggerSource, user, method, () => { throw new Exception("Test Exception"); }, parameters));
        };

        private It should_not_call_the_corresponding_rawlogger_method = () =>
        {
            rawMetricsLogger.WasNotToldTo(x => x.LogLatencyMetric(loggerSource.Name, loggerAppSource.ApplicationName, user, method, startTime, duration, parameters));
            rawMetricsLogger.WasNotToldTo(x => x.LogThroughputMetric(loggerSource.Name, loggerAppSource.ApplicationName, user, method, parameters));
        };

        private It should_throw_the_exception_out = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}