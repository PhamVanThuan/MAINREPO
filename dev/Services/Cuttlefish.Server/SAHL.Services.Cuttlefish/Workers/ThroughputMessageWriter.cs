using Dapper;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Services.Cuttlefish.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Services.Cuttlefish.Workers
{
    public class ThroughputMessageWriter : SAHL.Services.Cuttlefish.Workers.IThroughputMessageWriter
    {
        private IDataAccessConfigurationProvider dataAccessConfigurationProvider;

        public ThroughputMessageWriter(IDataAccessConfigurationProvider dataAccessConfigurationProvider)
        {
            this.dataAccessConfigurationProvider = dataAccessConfigurationProvider;
        }

        public void WriteMessage(ThroughputMetricMessageDataModel throughputMessage, Dictionary<string, string> parameters)
        {
            // write the message first then the parameters
            string throughputMessageInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[ThroughputMetricMessage] (Source, UserName, MessageDate, MachineName, Application, Metric) VALUES(@Source, @UserName, @MessageDate, @MachineName, @Application, @Metric); select cast(scope_identity() as int)";
            string messageParametersInsertSQL = "INSERT INTO [Cuttlefish].[dbo].[ThroughputMetricParameters] (ThroughputMetricMessage_id, ParameterKey, ParameterValue) VALUES(@ThroughputMetricMessage_id, @ParameterKey, @ParameterValue);";

            using (SqlConnection connection = new SqlConnection(this.dataAccessConfigurationProvider.ConnectionString))
            {
                connection.Open();
                using (var txn = connection.BeginTransaction())
                {
                    var identity = connection.Query<int>(throughputMessageInsertSQL, throughputMessage);

                    foreach (var parameter in parameters)
                    {
                        ThroughputMetricParametersDataModel messageParam = new ThroughputMetricParametersDataModel(identity.First(), parameter.Key, parameter.Value);
                        connection.Execute(messageParametersInsertSQL, messageParam);
                    }

                    txn.Commit();
                }
            }
        }
    }
}