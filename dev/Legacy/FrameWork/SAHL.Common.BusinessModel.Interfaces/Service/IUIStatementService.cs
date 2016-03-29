namespace SAHL.Common.BusinessModel.Interfaces.Service
{
    public interface IUIStatementService
    {
        string GetStatement(string applicationName, string statementName);
        void ClearCache();
    }
}