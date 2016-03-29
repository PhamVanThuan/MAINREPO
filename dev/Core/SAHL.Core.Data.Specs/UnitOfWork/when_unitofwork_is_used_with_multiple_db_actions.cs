using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Testing;
using StructureMap;

namespace SAHL.Core.Data.Specs.UnitOfWork
{
    public class when_unitofwork_is_used_with_multiple_db_actions : WithFakes
    {
        private static IDbConnectionProvider dbConnectionProvider;
        private static IDbConnectionProviderStorage dbConnectionProviderStorage;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static ISqlRepositoryFactory dbSqlRepositoryFactory;
        private static IDbConnectionProviderFactory dbConnectionProviderFactory;
        private static IDbFactory dbFactory;

        Establish context = () =>
        {
            dbConnectionProvider = An<IDbConnectionProvider>();
            dbSqlRepositoryFactory = An<ISqlRepositoryFactory>();
            dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            dbConnectionProvider.WhenToldTo(x => x.HasRegisteredContexts).Return(true);
            dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();

            dbConnectionProviderFactory.WhenToldTo(x => x.GetNewConnectionProvider()).Return(dbConnectionProvider);
            dbFactory = new DbFactory(dbSqlRepositoryFactory, dbConnectionProviderStorage, dbConnectionProviderFactory);

            unitOfWorkFactory = new UnitOfWorkFactory(dbConnectionProviderStorage, dbConnectionProviderFactory);
        };

        Because of = () =>
        {
            using (unitOfWorkFactory.Build())
            {
                dbFactory.NewDb().InReadOnlyAppContext().Dispose();
                dbFactory.NewDb().InReadOnlyAppContext().Dispose();
            }
        };

        It use_the_same_db_connection = () =>
        {
            dbConnectionProvider.WasToldTo(x => x.RegisterUnitOfWorkContext(Param.IsAny<string>())).OnlyOnce();
            dbConnectionProvider.WasToldTo(x => x.UnregisterUnitOfWorkContext(Param.IsAny<string>())).OnlyOnce();
        };
    }
}