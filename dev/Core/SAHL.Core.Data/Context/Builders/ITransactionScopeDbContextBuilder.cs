namespace SAHL.Core.Data.Context.Builders
{
    public interface ITransactionScopeDbContextBuilder
    {
        SAHL.Core.Data.Context.IDbContext InAppContext();

        SAHL.Core.Data.Context.IDbContext InWorkflowContext();
    }
}