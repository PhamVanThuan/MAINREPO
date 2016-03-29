using SAHL.Core.Data.Configuration;

namespace SAHL.Core.Data.Specs
{
    public class FakeDbConfigurationProvider : IDbConfigurationProvider
    {
        public string ConnectionStringForApplicationRole
        {
            get { return "AppRole"; }
        }

        public string ConnectionStringForWorkflowRole
        {
            get { return "WorkflowRole"; }
        }

        public string GetConnectionStringForNamedRole(string roleName)
        {
            return "Something";
        }

        public System.Configuration.Configuration Config
        {
            get { return null; }
        }
    }
}