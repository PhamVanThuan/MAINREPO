using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;

namespace SAHL.Core.Data.Specs
{
    public class when_InReadOnlyAppContext_is_disposed : WithFakes
    {
        private static IDbConnectionProviderFactory dbConnectionProviderFactory;
        private static IDbConnectionProviderStorage dbConnectionProviderStorage;
        private static IDbConnectionProvider dbConnectionProvider;
        private static ISqlRepositoryFactory dbSqlRepositoryFactory;
        private static IDbConfigurationProvider dbConfigurationProvider;
        private static IDbFactory dbFactory;

        private Establish context = () =>
        {
            dbSqlRepositoryFactory = An<ISqlRepositoryFactory>();
            dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();
            dbConfigurationProvider = new DefaultDbConfigurationProvider();
            dbConnectionProvider = new DefaultDbConnectionProvider(dbConfigurationProvider);
            dbConnectionProviderFactory.WhenToldTo(x => x.GetNewConnectionProvider()).Return(dbConnectionProvider);

            dbFactory = new DbFactory(dbSqlRepositoryFactory, dbConnectionProviderStorage, dbConnectionProviderFactory);
        };

        private Because of = () =>
        {
            dbFactory.NewDb().InReadOnlyAppContext().Dispose();
        };

        private It should_register_a_new_context = () =>
        {
            dbConnectionProvider.HasRegisteredContexts.ShouldBeFalse();
        };
    }
}