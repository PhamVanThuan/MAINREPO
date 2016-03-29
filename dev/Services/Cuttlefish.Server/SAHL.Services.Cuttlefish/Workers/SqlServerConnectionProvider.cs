using System.Data;
using System.Data.SqlClient;

namespace SAHL.Services.Cuttlefish.Workers
{
    public class SqlServerConnectionProvider : IDbConnectionProvider
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}