using SAHL.Core.Data.Configuration;

namespace SAHL.Core.Data.Context.Configuration
{
    public class DbContextConfiguration : IDbContextConfiguration
    {
        private static object lockObject = new object();

        private static DbContextConfiguration instance;

        public static DbContextConfiguration Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new DbContextConfiguration();
                    }
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        public DbContextConfiguration()
        {
            // default storage is thread local
            this.DbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();

            // default configuration provider
            this.DbConfigurationProvider = new DefaultDbConfigurationProvider();

            // default connection factory
            this.DbConnectionProviderFactory = new DefaultDbConnectionProviderFactory(this.DbConfigurationProvider);
        }

        public IDbConfigurationProvider DbConfigurationProvider { get; set; }

        public IDbConnectionProviderFactory DbConnectionProviderFactory { get; set; }

        public IDbConnectionProviderStorage DbConnectionProviderStorage { get; set; }

        public ISqlRepositoryFactory RepositoryFactory { get; set; }
    }
}