using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Testing;
using StructureMap;

namespace SAHL.Core.Data.Specs.UnitOfWork
{
    public class when_unitofwork_is_created : WithFakes
    {
        private static IDbConfigurationProvider dbConfigurationProvider;
        private static IDbConnectionProviderFactory dbConnectionProviderFactory;
        private static IDbConnectionProvider dbConnectionProvider;
        private static IDbConnectionProviderStorage dbConnectionProviderStorage;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static bool hasUnitOfWorkContexts;

        Establish context = () =>
        {
            var container = Testing.Ioc.TestingIoc.Initialise();

            dbConfigurationProvider = container.GetInstance<IDbConfigurationProvider>();
            dbConnectionProvider = new DefaultDbConnectionProvider(dbConfigurationProvider);
            dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();

            dbConnectionProviderFactory.WhenToldTo(x => x.GetNewConnectionProvider()).Return(dbConnectionProvider);
            DbContextConfiguration.Instance.DbConnectionProviderFactory = dbConnectionProviderFactory;

            unitOfWorkFactory = new UnitOfWorkFactory(dbConnectionProviderStorage, dbConnectionProviderFactory);
        };

        Because of = () =>
        {
            using (unitOfWorkFactory.Build())
            {
                hasUnitOfWorkContexts = dbConnectionProvider.HasUnitOfWorkContexts;
            }
        };

        It should_register_a_new_context_with_connection_provider = () =>
        {
            hasUnitOfWorkContexts.ShouldBeTrue();
        };
    }
}