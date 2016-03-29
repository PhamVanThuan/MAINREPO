using SAHL.Core.Data.Context;

namespace SAHL.Core.Data
{
    internal class UnitOfWorkBuilder
    {
        private IDbConnectionProviderFactory _dbConnectionProviderFactory;
        private IDbConnectionProviderStorage _dbConnectionProviderStorage;
        private IDbConnectionProvider _dbConnectionProvider;

        public UnitOfWorkBuilder(IDbConnectionProviderStorage dbConnectionProviderStorage, IDbConnectionProviderFactory dbConnectionProviderFactory)
        {
            this._dbConnectionProviderFactory = dbConnectionProviderFactory;
            this._dbConnectionProviderStorage = dbConnectionProviderStorage;
        }

        internal IUnitOfWork Build()
        {
            if (!_dbConnectionProviderStorage.HasConnectionProvider)
            {
                _dbConnectionProvider = _dbConnectionProviderFactory.GetNewConnectionProvider();
                _dbConnectionProviderStorage.RegisterConnectionProvider(_dbConnectionProvider);
            }
            else
            {
                _dbConnectionProvider = _dbConnectionProviderStorage.ConnectionProvider;
            }

            var unitOfWork = new UnitOfWork(_dbConnectionProviderStorage);
            return unitOfWork;
        }
    }
}