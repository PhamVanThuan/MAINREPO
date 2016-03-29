using SAHL.Core.Logging;

namespace SAHL.Core.Data.Dapper
{
    public class DapperSqlRepositoryFactory : ISqlRepositoryFactory
    {
        private ILogger logger;
        private ILoggerSource loggerSource;

        public IUIStatementProvider UIStatementProvider { get; protected set; }

        public DapperSqlRepositoryFactory(IUIStatementProvider uiStatementProvider, ILogger logger, ILoggerSource loggerSource)
        {
            this.UIStatementProvider = uiStatementProvider;
            this.logger = logger;
            this.loggerSource = loggerSource;
        }

        public IReadOnlySqlRepository GetNewReadOnlyRepository()
        {
            var repo = new DapperRepository(this.UIStatementProvider, this.logger, this.loggerSource);
            return repo as IReadOnlySqlRepository;
        }

        public IReadWriteSqlRepository GetNewReadWriteRepository()
        {
            var repo = new DapperRepository(this.UIStatementProvider, this.logger, this.loggerSource);
            return repo as IReadWriteSqlRepository;
        }

        public IDdlRepository GetNewDdlRepository()
        {
            var repository = new DapperDdlRepository();
            return repository as IDdlRepository;
        }
    }
}