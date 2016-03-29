using SAHL.Core.Data.Configuration;

namespace SAHL.Core.Data.Context
{
    public class DefaultDbConnectionProviderFactory : IDbConnectionProviderFactory
    {
        private IDbConfigurationProvider configurationProvider;

        public DefaultDbConnectionProviderFactory(IDbConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public IDbConnectionProvider GetNewConnectionProvider()
        {
            return new DefaultDbConnectionProvider(this.configurationProvider);
        }
    }
}