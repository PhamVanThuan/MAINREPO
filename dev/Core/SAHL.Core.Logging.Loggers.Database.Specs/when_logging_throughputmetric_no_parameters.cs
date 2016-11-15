﻿using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using System;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_throughputmetric_no_parameters : WithFakes
    {
        private static IDatabaseMetricsLoggerDataManager databaseMetricsLoggerDataManager;
        private static IDatabaseLoggerUtils databaseLoggerUtils;
        private static IRawMetricsLogger databaseMetricsLogger;
        private static string source;
        private static string application;
        private static string user;
        private static string metric;

        private Establish context = () =>
        {
            source = "test source";
            application = "test application";
            user = "test user";
            metric = "test metric";

            databaseMetricsLoggerDataManager = An<IDatabaseMetricsLoggerDataManager>();
            databaseLoggerUtils = An<IDatabaseLoggerUtils>();
            databaseMetricsLogger = new DatabaseMetricsLogger(databaseMetricsLoggerDataManager, databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseMetricsLogger.LogThroughputMetric(source, application, user, metric);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseMetricsLoggerDataManager.WasToldTo(x => x.StoreThroughputMetricLog(source, user, Arg.Any<DateTime>(), application, metric, null));
        };
    }
}