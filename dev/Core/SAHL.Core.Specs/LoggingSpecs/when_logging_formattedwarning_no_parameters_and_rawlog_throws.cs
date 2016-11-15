﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_formattedwarning_no_parameters_and_rawlog_throws : WithFakes
    {
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ILoggerAppSource loggerAppSource;
        private static IRawLogger rawLogger;
        private static IRawMetricsLogger rawMetricsLogger;
        private static IMetricTimerFactory metricsTimerFactory;
        private static string user;
        private static string method;
        private static string formattedmessage;
        private static object[] formattingArgs;
        private static IDictionary<string, object> parameters;
        private static Exception exception;

        private Establish context = () =>
            {
                parameters = new Dictionary<string, object>();
                user = "testuser";
                method = "testmethod";
                formattedmessage = "testmessage: {0}";
                formattingArgs = new object[] { "test" };
                rawLogger = An<IRawLogger>();
                rawMetricsLogger = An<IRawMetricsLogger>();
                metricsTimerFactory = An<IMetricTimerFactory>();
                loggerSource = new LoggerSource("test", LogLevel.Info, true);
                loggerAppSource = new FakeLoggerAppSource("testAppName");
                rawLogger.WhenToldTo(x => x.LogFormattedWarning(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, formattedmessage, formattingArgs))
                    .Throw(new Exception());

                logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
            };

        private Because of = () =>
        {
            exception = Catch.Exception(() => logger.LogFormattedWarning(loggerSource, user, method, formattedmessage, formattingArgs));
        };

        private It should_call_the_rawlogger_LogOnEnter_method = () =>
        {
            rawLogger.WasToldTo(x => x.LogFormattedWarning(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, formattedmessage, formattingArgs));
        };

        private It should_not_throw_the_exception_out = () =>
        {
            exception.ShouldBeNull();
        };
    }
}