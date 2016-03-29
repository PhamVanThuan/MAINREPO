using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.LoanAdjustments.LoanAdjustments
{
    internal class GetLoanAdjustmentTasksStatement : TaskBaseStatement
    {
        public GetLoanAdjustmentTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
 SELECT DISTINCT I.ID AS InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Account No.] = LA.AccountKey
    ,[Account Name] = (select legalname from [2AM].[dbo].GetAccountLegalNameAndID(LA.AccountKey))
    ,[Creation Date|" + Formats.Date.Default + @"] = I.CreationDate
    ,WL.Message
    ,[Requested By] = LA.RequestUser
FROM [X2].[X2].[WorkList] AS WL WITH (NOLOCK)
INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
INNER JOIN [X2].[X2DATA].[Loan_Adjustments] AS LA WITH (NOLOCK) ON I.ID = LA.InstanceID
INNER JOIN [2AM].[dbo].[Account] AS ACT WITH (NOLOCK) ON LA.AccountKey = ACT.AccountKey
INNER JOIN [2AM].[dbo].[ROLE] as RL with (NOLOCK) ON ACT.Accountkey = RL.Accountkey
INNER JOIN [2AM].[dbo].[LegalEntity] AS LE WITH (NOLOCK) ON RL.LegalEntityKey = LE.LegalEntityKey
WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
    AND RL.RoleTypeKey = 2
    AND S.Name IN @WorkFlowStateNames
    AND W.Name = @WorkFlowName
    AND PRO.Name = @BusinessProcessName
    AND WL.ADUserName IN @UsernameAndRoles
";
        }
    }
}
