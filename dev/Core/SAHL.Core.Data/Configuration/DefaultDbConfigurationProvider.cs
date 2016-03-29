using SAHL.Core.Configuration;

namespace SAHL.Core.Data.Configuration
{
    public class DefaultDbConfigurationProvider : ConfigurationProvider, IDbConfigurationProvider
    {
        public DefaultDbConfigurationProvider()
            : base()
        {
        }

        public string ConnectionStringForApplicationRole
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings[Strings.DBCONTEXT_APP].ConnectionString; }
        }

        public string ConnectionStringForWorkflowRole
        {
            get { return this.Config.ConnectionStrings.ConnectionStrings[Strings.DBCONTEXT_WORKFLOW].ConnectionString; }
        }

        public string GetConnectionStringForNamedRole(string roleName)
        {
            return this.Config.ConnectionStrings.ConnectionStrings[roleName].ConnectionString;
        }
    }
}