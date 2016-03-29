using SAHL.Core.Data;

namespace SAHL.Core.X2
{
    public interface IX2Process
    {
        IX2Map GetWorkflowMap(string workflowName);

        IUIStatementsProvider GetUIStatementProvider();
    }
}