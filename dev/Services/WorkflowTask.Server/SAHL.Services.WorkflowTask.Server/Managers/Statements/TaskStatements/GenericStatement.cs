using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements
{
    /// <summary>
    /// The statement to execute when no matching business process is found
    /// </summary>
    class GenericStatement : TaskBaseStatement, IGenericStatement
    {
        public GenericStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName, string workFlowName,
                                List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            if (string.IsNullOrEmpty(StorageTable))
            {
                return @"
                    SELECT InstanceId = I.ID
                        ,TagIds = (" + tagStatementSubQuery + @")

                        ,I.Subject
                        ,[Updated] = I.StateChangeDate
                        ,I.Priority
                        ,[Deadline] = I.DeadlineDate
                        ,WL.[Message]
                        ,ProcessName = PRO.Name
                    FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
                    INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
                    INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
                    INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
                    INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
                    WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
                        AND S.Name IN @WorkFlowStateNames
                        AND W.Name = @WorkFlowName
                        AND PRO.Name = @BusinessProcessName
                        AND WL.ADUserName IN @UsernameAndRoles
                    ";
            }
            else
            {
                return @"
                    SELECT InstanceId = I.ID
                        ,TagIds = (" + tagStatementSubQuery + @")

                        ,I.Subject
                        ,[Updated] = I.StateChangeDate
                        ,I.Priority
                        ,[Deadline] = I.DeadlineDate
                        ,WL.[Message]
                        ,ST.GenericKey
                        ,W.GenericKeyTypeKey
                        ,ProcessName = PRO.Name 
                        ,WorkflowName = W.Name
                    FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
                    INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
                    INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
                    INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
                    INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
                    INNER JOIN [X2].[X2DATA].[" + StorageTable + @"] ST ON ST.InstanceID = I.ID
                    WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
                        AND S.Name IN @WorkFlowStateNames
                        AND W.Name = @WorkFlowName
                        AND PRO.Name = @BusinessProcessName
                        AND WL.ADUserName IN @UsernameAndRoles
                    ";
            }
        }
    }
}
