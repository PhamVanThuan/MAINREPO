using SAHL.Core.Data.Context;
using SAHL.Core.Data.Context.Builders;

namespace SAHL.Core.Data
{
    public interface IDb
    {
        IDdlDbContext DDLInAppContext();

        IDdlDbContext DDLInWorkflowContext();

        IReadOnlyDbContextBuilder ForReadOnly();

        IReadWriteDbContextBuilder ForReadWrite();

        IDbContext InAppContext();

        IReadOnlyDbContext InReadOnlyAppContext();

        IReadOnlyDbContext InReadOnlyWorkflowContext();

        IDbContext InWorkflowContext();

        ITransactionScopeDbContextBuilder WithInheritedTransactionScope();

        ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel);

        ITransactionScopeDbContextBuilder WithNewTransactionScope();

        ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel);

        ITransactionScopeDbContextBuilder WithNoTransactionScope();

        ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel);
    }
}