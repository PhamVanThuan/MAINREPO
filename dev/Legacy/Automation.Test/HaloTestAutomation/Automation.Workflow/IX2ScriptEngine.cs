using System.Collections.Generic;

namespace WorkflowAutomation.Harness
{
    public interface IX2ScriptEngine
    {
        Dictionary<int, Common.Models.WorkflowReturnData> ExecuteScript(Common.Enums.WorkflowEnum workflow, string scriptToRun, int keyValue, string identity = "");

        bool ClearCacheFor(string processName, string workflowName, Common.Enums.CacheTypes cacheToClear);

        bool ClearDomainServiceCacheForAllWorkflows();
    }
}