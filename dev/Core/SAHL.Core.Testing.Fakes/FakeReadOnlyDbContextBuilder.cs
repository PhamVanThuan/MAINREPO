using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Builders;

namespace SAHL.Core.Testing.Fakes
{
    public class FakeReadOnlyDbContextBuilder : IReadOnlyDbContextBuilder
    {
        private IReadOnlyDbContext dbContext;

        public FakeReadOnlyDbContextBuilder(IReadOnlyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Data.Context.IReadOnlyDbContext InAppContext()
        {
            return this.dbContext;
        }

        public Data.Context.IReadOnlyDbContext InWorkflowContext()
        {
            return this.dbContext;
        }
    }
}