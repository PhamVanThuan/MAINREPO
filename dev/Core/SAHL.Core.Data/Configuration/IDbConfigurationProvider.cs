using SAHL.Core.Configuration;

namespace SAHL.Core.Data.Configuration
{
    public interface IDbConfigurationProvider : IConfigurationProvider
    {
        string ConnectionStringForApplicationRole { get; }

        string ConnectionStringForWorkflowRole { get; }

        string GetConnectionStringForNamedRole(string roleName);
    }
}