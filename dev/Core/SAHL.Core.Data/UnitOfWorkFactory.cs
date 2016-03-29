using SAHL.Core.Data.Context;

namespace SAHL.Core.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private IDbConnectionProviderFactory dbConnectionProviderFactory;
        private IDbConnectionProviderStorage dbConnectionProviderStorage;

        public UnitOfWorkFactory(IDbConnectionProviderStorage dbConnectionProviderStorage, IDbConnectionProviderFactory dbConnectionProviderFactory)
        {
            this.dbConnectionProviderFactory = dbConnectionProviderFactory;
            this.dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        public IUnitOfWork Build()
        {
            return new UnitOfWorkBuilder(this.dbConnectionProviderStorage, this.dbConnectionProviderFactory).Build();
        }
    }
}