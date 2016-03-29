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
    public class when_logging_formatted_error_with_exception : WithFakes
    {
        private static IDatabaseLoggerDataManager databaseLoggerDataManager;
        private static DatabaseLoggerUtils databaseLoggerUtils;
        private static IRawLogger databaseLogger;

        private static LogLevel logLevel;
        private static string user;
        private static string source;
        private static string method;
        private static string application;
        private static string formattedmessage;
        private static string message;
        private static object[] formattingArgs;
        private static Exception exception;
        private static IDictionary<string, object> parameters;
        private static IDictionary<string, object> expectedParameters;

        private Establish context = () =>
        {
            databaseLoggerUtils = new DatabaseLoggerUtils(An<IIocContainer>());
            expectedParameters = new Dictionary<string, object>();
            databaseLoggerUtils.EnsureParametersCreated(ref expectedParameters);
            exception = new Exception("Exception");
            var exceptionData = databaseLoggerUtils.SerializeObject(exception);
            expectedParameters.Add(Logger.Exception, exceptionData);

            parameters = new Dictionary<string, object>();

            user = "testuser";
            method = "testmethod";
            application = "testapplication";
            source = "testsource";
            message = "testmessage: {0}";
            formattingArgs = new object[] { "test" };
            formattedmessage = string.Format(message, formattingArgs);

            logLevel = LogLevel.Error;

            databaseLoggerDataManager = An<IDatabaseLoggerDataManager>();
            databaseLogger = new DatabaseLogger(databaseLoggerDataManager, databaseLoggerUtils);
        };

        private Because of = () =>
        {
            databaseLogger.LogFormattedErrorWithException(logLevel, source, application, user, method, message, exception, parameters, formattingArgs);
        };

        private It should_contain_the_exception_as_a_parameter = () =>
        {
            parameters.ShouldContainOnly(expectedParameters);
        };

        private It should_call_the_corresponding_rawlogger_method = () =>
        {
            databaseLoggerDataManager.WasToldTo(x => x.StoreLog(logLevel.ToString(), source, application, user, method, formattedmessage, parameters));
        };
    }
}