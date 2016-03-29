namespace SAHL.Core.Data.Context.Builders
{
    public interface IReadWriteDbContextBuilder
    {
        ITransactionScopeDbContextBuilder WithInheritedTransactionScope();

        ITransactionScopeDbContextBuilder WithInheritedTransactionScope(System.Transactions.IsolationLevel isolationLevel);

        ITransactionScopeDbContextBuilder WithNewTransactionScope();

        ITransactionScopeDbContextBuilder WithNewTransactionScope(System.Transactions.IsolationLevel isolationLevel);

        ITransactionScopeDbContextBuilder WithNoTransactionScope();

        ITransactionScopeDbContextBuilder WithNoTransactionScope(System.Transactions.IsolationLevel isolationLevel);
    }
}