﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Specs.Fakes;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_formattederror_no_parameters : WithFakes
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

        private Establish context = () =>
        {
            user = "testuser";
            method = "testmethod";
            formattedmessage = "testmessage: {0}";
            formattingArgs = new object[] { "test" };
            rawLogger = An<IRawLogger>();
            rawMetricsLogger = An<IRawMetricsLogger>();
            metricsTimerFactory = An<IMetricTimerFactory>();
            loggerSource = new LoggerSource("test", LogLevel.Info, true);
            loggerAppSource = new FakeLoggerAppSource("testAppName");

            logger = new Logger(rawLogger, rawMetricsLogger, loggerAppSource, metricsTimerFactory);
        };

        private Because of = () =>
        {
            logger.LogFormattedError(loggerSource, user, method, formattedmessage, formattingArgs);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            rawLogger.WasToldTo(x => x.LogFormattedError(loggerSource.LogLevel, loggerSource.Name, loggerAppSource.ApplicationName, user, method, formattedmessage, formattingArgs));
        };
    }
}