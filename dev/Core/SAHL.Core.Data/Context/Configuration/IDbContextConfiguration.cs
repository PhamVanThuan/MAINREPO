using SAHL.Core.Data.Configuration;

namespace SAHL.Core.Data.Context.Configuration
{
    public interface IDbContextConfiguration
    {
        IDbConfigurationProvider DbConfigurationProvider { get; set; }

        IDbConnectionProviderFactory DbConnectionProviderFactory { get; set; }

        IDbConnectionProviderStorage DbConnectionProviderStorage { get; set; }

        ISqlRepositoryFactory RepositoryFactory { get; set; }
    }
}