using System;
using SAHL.Core.Configuration;

namespace SAHL.Core.Data.Configuration
{
    public class TestDbConfigurationProvider : ConfigurationProvider, IDbConfigurationProvider
    {
        private string suffix;
        public TestDbConfigurationProvider(string suffix)
            : base()
        {
            this.suffix = suffix;
        }

        public string ConnectionStringForApplicationRole
        {
            get
            {
                var key = GetFormattedRole(Strings.DBCONTEXT_APP);
                return this.Config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
            }
        }

        public string ConnectionStringForWorkflowRole
        {
            get
            {
                var key = GetFormattedRole(Strings.DBCONTEXT_WORKFLOW);
                return this.Config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
            }
        }

        public string GetConnectionStringForNamedRole(string roleName)
        {
            return this.Config.ConnectionStrings.ConnectionStrings[roleName].ConnectionString;
        }

        private string GetFormattedRole(string roleName)
        {
            return String.Format("{0}_{1}", roleName, this.suffix);
        }
    }
}