using Dapper;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Services.Cuttlefish.Workers
{
    public class LatencyMessageWriter : ILatencyMessageWriter
    {
        private IDataAccessConfigurationProvider dataAccessConfigurationProvider;

        public LatencyMessageWriter(IDataAccessConfigurationProvider dataAccessConfigurationProvider)
        {
            this.dataAccessConfigurationProvider = dataAccessConfigurationProvider;
        }

        public void WriteMessage(LatencyMetricMessageDataModel latencyMessage, Dictionary<string, string> parameters)
        {
            // write the message first then the parameters
            string latencyMessageInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[LatencyMetricMessage] (StartTime, Duration, Source, UserName, MessageDate, MachineName, Application, Metric) VALUES(@StartTime, @Duration, @Source, @UserName, @MessageDate, @MachineName, @Application, @Metric); select cast(scope_identity() as int)";
            string messageParametersInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[LatencyMetricParameters] (LatencyMetricMessage_id, ParameterKey, ParameterValue) VALUES(@LatencyMetricMessage_id, @ParameterKey, @ParameterValue);";

            using (SqlConnection connection = new SqlConnection(this.dataAccessConfigurationProvider.ConnectionString))
            {
                connection.Open();
                using (var txn = connection.BeginTransaction())
                {
                    var identity = connection.Query<int>(latencyMessageInsertSQL, latencyMessage);

                    foreach (var parameter in parameters)
                    {
                        LatencyMetricParametersDataModel messageParam = new LatencyMetricParametersDataModel(identity.First(), parameter.Key, parameter.Value);
                        connection.Execute(messageParametersInsertSQL, messageParam);
                    }

                    txn.Commit();
                }
            }
        }
    }
}