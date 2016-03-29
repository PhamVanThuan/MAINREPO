using Dapper;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Services;
using SAHL.Services.Cuttlefish.Specs.Fakes;
using SAHL.Services.Cuttlefish.Workers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Services.Cuttlefish.Specs.LogMessageWriterSpecs
{
    public class when_writing_a_log_message_with_no_parameters : WithFakes
    {
        private static ILogMessageWriter writer;
        private static IDataAccessConfigurationProvider dataConfig;
        private static IDbConnectionProvider connectionProvider;
        private static IDbConnection connection;
        private static IDbTransaction transaction;
        private static LogMessageDataModel logMessage;

        private static DateTime logMessageDate;
        private static string logMessageType;
        private static string logMessageUser;
        private static string logMessageMethod;
        private static string logMessageMessage;
        private static string logMessageSource;
        private static string logMessageMachineName;
        private static string logMessageApplication;

        private Establish context = () =>
            {
                logMessageDate = DateTime.Now;
                logMessageType = "Error";
                logMessageUser = "someuser";
                logMessageMethod = "someMethod";
                logMessageSource = "someSource";
                logMessageMachineName = "someMachine";
                logMessageApplication = "someApplication";

                dataConfig = new FakeDataAccessConfigurationProvider("someConnectionString");
                connectionProvider = An<IDbConnectionProvider>();
                connection = An<IDbConnection>();
                transaction = An<IDbTransaction>();
                connectionProvider.WhenToldTo(x => x.GetConnection(Param.IsAny<string>())).Return(connection);
                connection.WhenToldTo(x => x.BeginTransaction()).Return(transaction);

                writer = new LogMessageWriter(dataConfig, connectionProvider);
                logMessage = new LogMessageDataModel(logMessageDate, logMessageType, logMessageMethod, logMessageMessage, logMessageSource, logMessageUser, logMessageMachineName, logMessageApplication);
            };

        private Because of = () =>
            {
                writer.WriteMessage(logMessage, new Dictionary<string, string>());
            };

        private It should_fetch_a_db_connection = () =>
            {
                connectionProvider.WasToldTo(x => x.GetConnection(dataConfig.ConnectionString));
            };

        private It should_open_the_connection = () =>
        {
            connection.WasToldTo(x => x.Open());
        };

        private It should_start_a_transaction = () =>
        {
            connection.WasToldTo(x => x.BeginTransaction());
        };

        private It should_insert_the_log_message = () =>
        {
            connection.WasToldTo(x => x.Query<int>(Arg.Any<string>(), logMessage, transaction, Arg.Any<bool>(), Arg.Any<int?>(), Arg.Any<CommandType?>()));
        };

        private It should_commit_the_transaction = () =>
        {
            transaction.WasToldTo(x => x.Commit());
        };
    }
}