using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Logging.Loggers.Database;
using SAHL.Core.Testing.Fakes;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Specs.LoggingSpecs
{
    public class when_logging_onentermethodexception : WithFakes
    {
        private static IDatabaseLoggerDataManager databaseLoggerDataManager;
        private static DatabaseLoggerUtils databaseLoggerUtils;
        private static IRawLogger databaseLogger;

        private static LogLevel logLevel;
        private static string user;
        private static string source;
        private static string method;
        private static string application;
        private static IDictionary<string, object> parameters;
        private static Exception exception;
        private static IDbFactory dbFactory;

        private Establish context = () =>
        {
            parameters = new Dictionary<string, object>();
            user = "testuser";
            method = "testmethod";
            application = "testapplication";
            source = "testsource";
            logLevel = LogLevel.Debug;

            exception = new Exception("Exception");

            databaseLoggerDataManager = An<IDatabaseLoggerDataManager>();
            dbFactory = new FakeDbFactory();
            databaseLoggerUtils = new DatabaseLoggerUtils(An<IIocContainer>());
            databaseLogger = new DatabaseLogger(databaseLoggerDataManager, (DatabaseLoggerUtils)databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseLogger.LogOnEnterMethod(logLevel, source, application, user, method, parameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseLoggerDataManager.WasToldTo(x => x.StoreLog(Logger.MethodEnter, source, application, user, method, string.Empty, parameters));
        };
    }
}