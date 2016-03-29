namespace SAHL.Core.Data
{
    public interface ISqlRepositoryFactory
    {
        IReadOnlySqlRepository GetNewReadOnlyRepository();

        IReadWriteSqlRepository GetNewReadWriteRepository();

        IDdlRepository GetNewDdlRepository();

        IUIStatementProvider UIStatementProvider { get; }
    }
}