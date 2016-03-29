using System.Data;

namespace SAHL.Services.Cuttlefish.Workers
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection(string connectionString);
    }
}