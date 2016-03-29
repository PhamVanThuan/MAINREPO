namespace SAHL.Core.Data
{
    public interface IUIStatementProvider
    {
        string Get(string statementContext, string uiStatementName);

        void Add(IUIStatementsProvider uiStatementsProvider);
    }
}