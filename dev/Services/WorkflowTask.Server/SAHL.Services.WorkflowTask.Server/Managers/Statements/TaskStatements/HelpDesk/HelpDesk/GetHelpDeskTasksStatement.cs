using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.HelpDesk.HelpDesk
{
    internal class GetHelpDeskTasksStatement : TaskBaseStatement
    {
        public GetHelpDeskTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT DISTINCT I.ID AS InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,I.Subject
    ,[Updated|" + Formats.DateTime.Default + @"] = I.StateChangeDate
    ,I.Priority
    ,[Deadline|" + Formats.DateTime.Default + @"] = I.DeadlineDate
    ,WL.Message
FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
INNER JOIN [X2].[X2Data].[Help_Desk] HD WITH (NOLOCK) ON I.ID = HD.InstanceID
INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
    AND S.Name IN @WorkFlowStateNames
    AND W.Name = @WorkFlowName
    AND PRO.Name = @BusinessProcessName
    AND WL.ADUserName IN @UsernameAndRoles
";
        }
    }
}
