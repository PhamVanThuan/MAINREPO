using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_instantaneousvaluemetric_no_parameters : WithFakes
    {
        private static IDatabaseMetricsLoggerDataManager databaseMetricsLoggerDataManager;
        private static IDatabaseLoggerUtils databaseLoggerUtils;
        private static IRawMetricsLogger databaseMetricsLogger;
        private static string source;
        private static string application;
        private static string user;
        private static string metric;
        private static int value;

        private Establish context = () =>
        {
            source = "test source";
            application = "test application";
            user = "test user";
            metric = "test metric";
            value = 0;

            databaseMetricsLoggerDataManager = An<IDatabaseMetricsLoggerDataManager>();
            databaseLoggerUtils = An<IDatabaseLoggerUtils>();
            databaseMetricsLogger = new DatabaseMetricsLogger(databaseMetricsLoggerDataManager, databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseMetricsLogger.LogInstantaneousValueMetric(source, application, user, metric, value);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseMetricsLoggerDataManager.WasToldTo(x => x.StoreInstantaneousValueMetricLog(value, source, user, application, null));
        };
    }
}