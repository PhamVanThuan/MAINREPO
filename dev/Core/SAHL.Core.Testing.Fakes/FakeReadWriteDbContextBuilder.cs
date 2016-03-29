using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Builders;

namespace SAHL.Core.Testing.Fakes
{
    public class FakeReadWriteDbContextBuilder : IReadWriteDbContextBuilder
    {
        private IDbContext dbContext;

        public FakeReadWriteDbContextBuilder(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }

        public ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }

        public ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope()
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }

        public ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel)
        {
            return new FakeTransactionScopeDbContextBuilder(this.dbContext);
        }
    }
}