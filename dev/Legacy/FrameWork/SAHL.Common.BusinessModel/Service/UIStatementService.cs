using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Service
{
    public class UIStatementService : IUIStatementService
    {
        public string GetStatement(string applicationName, string statementName)
        {
            return UIStatementRepository.GetStatement(applicationName, statementName);
        }

        public void ClearCache()
        {
            UIStatementRepository.ClearCache();
        }
    }
}