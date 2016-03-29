using SAHL.Core.Attributes;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.DataModels;
using SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements
{
    [NolockConventionExclude]
    public class GetWorkFlowStatesForWorkListItemsForUsernameStatement
        : TaskBaseStatement<WorkFlowStateItem> //not strictly a TaskStatement, but we can re-use the functionality therein
    {
        public GetWorkFlowStatesForWorkListItemsForUsernameStatement(string username, List<string> capabilities = null)
            : base(username, capabilities, true, null, null, null, null)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT DISTINCT BusinessProcessName = pro.Name
    ,WorkFlowName = wkf.Name
    ,StateName = sta.Name
    ,wkf.StorageTable
FROM [X2].[X2].[WorkList] wkl WITH (NOLOCK) 
INNER JOIN [X2].[X2].[Instance] ins WITH (NOLOCK) ON wkl.InstanceID = ins.ID
INNER JOIN [X2].[X2].[Workflow] wkf WITH (NOLOCK) ON ins.WorkFlowID = wkf.ID
INNER JOIN [X2].[X2].[State] sta WITH (NOLOCK) ON ins.StateId = sta.ID
INNER JOIN [X2].[X2].[Process] pro WITH (NOLOCK) ON wkf.ProcessID = pro.ID
WHERE pro.ViewableOnUserInterfaceVersion LIKE '3%'
    AND wkl.ADUsername IN @UsernameAndRoles
";
        }
    }
}