﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_onmethodexception_and_rawlog_throws : WithFakes
    {
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static IRawLogger rawLogger;
        private static IRawMetricsLogger rawMetricsLogger;
        private static IMetricTimerFactory metricsTimerFactory;
        private static string user;
        private static string method;
        private static IDictionary<string, object> parameters;
        private static Exception exception;
        private static Exception exception2;

        private Establish context = () =>
            {
                exception = new Exception("Test Exception");
                parameters = new Dictionary<string, object>();
                user = "testuser";
                method = "testmethod";
                rawLogger = An<IRawLogger>();
                rawMetricsLogger = An<IRawMetricsLogger>();
                metricsTimerFactory = An<IMetricTimerFactory>();
                loggerSource = new LoggerSource("test", LogLevel.Info, true);
                loggerAppSource = new FakeLoggerAppSource("testAppName");
                rawLogger.WhenToldTo(x => x.LogOnMethodException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, exception, parameters))
                    .Throw(new Exception());

                logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
            };

        private Because of = () =>
        {
            exception2 = Catch.Exception(() => logger.LogOnMethodException(loggerSource, user, method, exception, parameters));
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            rawLogger.WasToldTo(x => x.LogOnMethodException(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, exception, parameters));
        };

        private It should_not_throw_the_exception_out = () =>
        {
            exception2.ShouldBeNull();
        };
    }
}