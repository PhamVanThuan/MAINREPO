using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_onmethodsuccess : WithFakes
    {
        private static IDatabaseLoggerDataManager databaseLoggerDataManager;
        private static IDatabaseLoggerUtils databaseLoggerUtils;
        private static IRawLogger databaseLogger;

        private static LogLevel logLevel;
        private static string user;
        private static string source;
        private static string method;
        private static string application;
        private static IDictionary<string, object> parameters;

        private Establish context = () =>
        {
            parameters = new Dictionary<string, object>();
            user = "testuser";
            method = "testmethod";
            application = "testapplication";
            source = "testsource";
            logLevel = LogLevel.Debug;

            databaseLoggerDataManager = An<IDatabaseLoggerDataManager>();
            databaseLoggerUtils = An<IDatabaseLoggerUtils>();
            databaseLogger = new DatabaseLogger(databaseLoggerDataManager, databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseLogger.LogOnMethodSuccess(logLevel, source, application, user, method, parameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseLoggerDataManager.WasToldTo(x => x.StoreLog(Logger.MethodSuccess, source, application, user, method, string.Empty, parameters));
        };
    }
}