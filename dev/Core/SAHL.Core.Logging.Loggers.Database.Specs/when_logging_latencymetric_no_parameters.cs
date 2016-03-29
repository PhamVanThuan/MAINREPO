using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using System;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_latencymetric_no_parameters : WithFakes
    {
        private static IDatabaseMetricsLoggerDataManager databaseMetricsLoggerDataManager;
        private static IDatabaseLoggerUtils databaseLoggerUtils;
        private static IRawMetricsLogger databaseMetricsLogger;
        private static string source;
        private static string application;
        private static string user;
        private static string metric;
        private static DateTime startdate;
        private static TimeSpan duration;

        private Establish context = () =>
        {
            source = "test source";
            application = "test application";
            user = "test user";
            metric = "test metric";
            startdate = DateTime.Now;
            duration = new TimeSpan(10);

            databaseMetricsLoggerDataManager = An<IDatabaseMetricsLoggerDataManager>();
            databaseLoggerUtils = An<IDatabaseLoggerUtils>();
            databaseMetricsLogger = new DatabaseMetricsLogger(databaseMetricsLoggerDataManager, databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseMetricsLogger.LogLatencyMetric(source, application, user, metric, startdate, duration);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseMetricsLoggerDataManager.WasToldTo(x => x.StoreLatencyMetricLog(source, application, user, metric, startdate, duration, null));
        };
    }
}