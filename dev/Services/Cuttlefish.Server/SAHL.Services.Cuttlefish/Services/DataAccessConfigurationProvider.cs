using SAHL.Core.Configuration;

namespace SAHL.Services.Cuttlefish.Services
{
    public class DataAccessConfigurationProvider : ConfigurationProvider, IDataAccessConfigurationProvider
    {
        public string ConnectionString
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings["DBCONNECTION_ServiceArchitect"].ConnectionString; }
        }
    }
}