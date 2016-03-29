namespace SAHL.Core.Data.Context.Builders
{
    public interface IReadOnlyDbContextBuilder
    {
        SAHL.Core.Data.Context.IReadOnlyDbContext InAppContext();

        SAHL.Core.Data.Context.IReadOnlyDbContext InWorkflowContext();
    }
}