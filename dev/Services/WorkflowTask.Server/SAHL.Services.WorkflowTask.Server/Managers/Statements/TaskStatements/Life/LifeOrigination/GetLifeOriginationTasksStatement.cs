using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.Life.LifeOrigination
{
    internal class GetLifeOriginationTasksStatement : TaskBaseStatement
    {
        public GetLifeOriginationTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone,
            string businessProcessName, string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT DISTINCT WL.InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Policy No.] = WL.[Name]
    ,[Loan No.] = A1.ParentAccountKey
    ,[Subject] = CASE 
        WHEN MLP.Description IS NOT NULL
            THEN MLP.Description + ' : ' + WL.Subject
        ELSE WL.Subject
        END
    ,[Updated" + Formats.DateTime.Default + @"] = WL.StateChangeDate
    ,WL.[Message]
FROM (
    SELECT I.[Name]
        ,InstanceId = I.ID
        ,I.StateChangeDate
        ,I.Subject
        ,WL.[Message]
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
    ) WL
INNER JOIN [2am]..[Account] A1 WITH (NOLOCK) ON CAST(A1.AccountKey AS VARCHAR(100)) = WL.[Name]
INNER JOIN [2am]..[FinancialService] FSL WITH (NOLOCK) ON FSL.AccountKey = A1.ParentAccountKey
INNER JOIN [2am].[fin].[MortgageLoan] ML WITH (NOLOCK) ON ML.FinancialServiceKey = FSL.FinancialServiceKey
INNER JOIN [2am]..[MortgageLoanPurpose] MLP WITH (NOLOCK) ON MLP.MortgageLoanPurposeKey = ML.MortgageLoanPurposeKey
WHERE FSL.AccountStatusKey <> 2 -- only get active accounts
";
        }
    }
}
