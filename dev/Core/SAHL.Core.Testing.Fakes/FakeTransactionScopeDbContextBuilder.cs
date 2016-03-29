using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Builders;

namespace SAHL.Core.Testing.Fakes
{
    public class FakeTransactionScopeDbContextBuilder : ITransactionScopeDbContextBuilder
    {
        private IDbContext dbContext;

        public FakeTransactionScopeDbContextBuilder(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Data.Context.IDbContext InAppContext()
        {
            return this.dbContext;
        }

        public Data.Context.IDbContext InWorkflowContext()
        {
            return this.dbContext;
        }
    }
}