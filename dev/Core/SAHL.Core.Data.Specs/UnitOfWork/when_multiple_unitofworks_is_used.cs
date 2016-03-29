using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context;
using SAHL.Core.Testing;
using StructureMap;
using System.Data;

namespace SAHL.Core.Data.Specs.UnitOfWork
{
    public class when_multiple_unitofworks_is_used : WithFakes
    {
        private static IDbConfigurationProvider dbConfigurationProvider;
        private static IDbConnectionProviderFactory dbConnectionProviderFactory;
        private static FakeDbConnectionProvider dbConnectionProvider;
        private static IDbConnectionProviderStorage dbConnectionProviderStorage;
        private static ISqlRepositoryFactory dbSqlRepositoryFactory;
        private static IUnitOfWorkFactory unitOfWorkFactory;
        private static IDbFactory dbFactory;

        private static IDbConnection connection1;
        private static IDbConnection connection2;
        private static IDbConnection connection3;

        Establish context = () =>
        {
            var container =  Testing.Ioc.TestingIoc.Initialise();

            dbConfigurationProvider = container.GetInstance<IDbConfigurationProvider>();
            dbConnectionProvider = new FakeDbConnectionProvider(dbConfigurationProvider);
            dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();
            dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();
            dbSqlRepositoryFactory = An<ISqlRepositoryFactory>();
            dbConnectionProviderFactory.WhenToldTo(x => x.GetNewConnectionProvider()).Return(dbConnectionProvider);

            unitOfWorkFactory = new UnitOfWorkFactory(dbConnectionProviderStorage, dbConnectionProviderFactory);
            dbFactory = new DbFactory(dbSqlRepositoryFactory, dbConnectionProviderStorage, dbConnectionProviderFactory);
        };

        Because of = () =>
        {
            using (unitOfWorkFactory.Build())
            {
                using (dbFactory.NewDb().InReadOnlyAppContext())
                {
                    connection1 = dbConnectionProvider.GetConnectionForContext(Strings.DBCONTEXT_APP);

                    using (dbFactory.NewDb().InAppContext())
                    {
                        connection2 = dbConnectionProvider.GetConnectionForContext(Strings.DBCONTEXT_APP);
                    }
                }

                using (unitOfWorkFactory.Build())
                {
                    using (dbFactory.NewDb().InReadOnlyAppContext())
                    {
                        connection3 = dbConnectionProvider.GetConnectionForContext(Strings.DBCONTEXT_APP);
                    }
                }
            }
        };

        It should = () =>
        {
            connection1.ShouldNotBeNull();
            connection2.ShouldNotBeNull();
            connection3.ShouldNotBeNull();

            connection1.ShouldBeTheSameAs(connection2);
            connection1.ShouldBeTheSameAs(connection3);
        };

        private class FakeDbConnectionProvider : DefaultDbConnectionProvider
        {
            public FakeDbConnectionProvider(IDbConfigurationProvider dbConfigurationProvider)
                : base(dbConfigurationProvider)
            {
            }

            public new int NoUnitOfWorkContexts
            {
                get { return base.NoUnitOfWorkContexts; }
            }

            public new int NoRegisteredContexts
            {
                get { return base.NoRegisteredContexts; }
            }

            public new bool IsUnitOfWorkContextInQueue(string uowContextName)
            {
                return base.IsUnitOfWorkContextInQueue(uowContextName);
            }
        }
    }
}