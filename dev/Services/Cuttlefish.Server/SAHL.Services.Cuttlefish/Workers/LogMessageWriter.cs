using Dapper;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Services;
using SAHL.Services.Cuttlefish.Workers;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAHL.Services.Cuttlefish
{
    public class LogMessageWriter : ILogMessageWriter
    {
        private IDataAccessConfigurationProvider dataAccessConfigurationProvider;
        private IDbConnectionProvider dbConnectionProvider;

        public LogMessageWriter(IDataAccessConfigurationProvider dataAccessConfigurationProvider, IDbConnectionProvider dbConnectionProvider)
        {
            this.dataAccessConfigurationProvider = dataAccessConfigurationProvider;
            this.dbConnectionProvider = dbConnectionProvider;
        }

        public void WriteMessage(LogMessageDataModel logMessage, Dictionary<string, string> parameters)
        {
            // write the message first then the parameters
            string logMessageInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[LogMessage] (MessageDate, LogMessageType, MethodName, Message, Source, UserName, MachineName, Application) VALUES(@MessageDate, @LogMessageType, @MethodName, @Message, @Source, @UserName, @MachineName, @Application); select cast(scope_identity() as int)";
            string messageParametersInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[MessageParameters] (LogMessage_id, ParameterKey, ParameterValue) VALUES(@LogMessage_id, @ParameterKey, @ParameterValue);";

            using (IDbConnection connection = this.dbConnectionProvider.GetConnection(this.dataAccessConfigurationProvider.ConnectionString))
            {
                connection.Open();
                using (var txn = connection.BeginTransaction())
                {
                    var identity = connection.Query<int>(logMessageInsertSQL, logMessage, txn);

                    foreach (var parameter in parameters)
                    {
                        MessageParametersDataModel messageParam = new MessageParametersDataModel(identity.First(), parameter.Key, parameter.Value);
                        connection.Execute(messageParametersInsertSQL, messageParam, txn);
                    }

                    txn.Commit();
                }
            }
        }
    }
}