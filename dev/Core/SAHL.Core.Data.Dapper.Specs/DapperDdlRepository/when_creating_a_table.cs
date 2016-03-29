using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Context;
using SAHL.Core.Testing;
using System.Data;

namespace SAHL.Core.Data.Dapper.Specs.DapperDdlRepository
{
    public class when_creating_a_table : WithFakes
    {
        private static IDbConnection dbConnection;
        private static IDbConnectionProvider dbConnectionProvider;
        private static ISqlRepositoryFactory dbSqlRepositoryFactory;
        private static IDbConnectionProviderStorage dbConnectionProviderStorage;
        private static IDbConnectionProviderFactory dbConnectionProviderFactory;
        private static IDbFactory dbFactory;
        private static IDdlDbContext ddlDbContext;
        private static int numberOfRowsAffected;

        Establish context = () =>
        {
            SAHL.Core.Testing.Ioc.TestingIoc.Initialise();

            dbConnection = An<IDbConnection>();

            dbConnectionProvider = An<IDbConnectionProvider>();
            dbSqlRepositoryFactory = An<ISqlRepositoryFactory>();
            dbConnectionProviderFactory = An<IDbConnectionProviderFactory>();

            dbConnectionProvider.WhenToldTo(x => x.RegisterContext(Param.IsAny<string>())).Return(dbConnection);

            dbConnectionProviderStorage = new DefaultDbConnectionProviderStorage();
            dbConnectionProviderStorage.RegisterConnectionProvider(dbConnectionProvider);

            dbFactory = new DbFactory(dbSqlRepositoryFactory, dbConnectionProviderStorage, dbConnectionProviderFactory);

            ddlDbContext = dbFactory.NewDb().DDLInAppContext();
        };

        Because of = () =>
        {
            numberOfRowsAffected = ddlDbContext.Create<TestDataModel>();
        };

        It should_execute_the_create_table_table_statement = () =>
        {
            numberOfRowsAffected.ShouldBeGreaterThanOrEqualTo(0);
        };
    }
}