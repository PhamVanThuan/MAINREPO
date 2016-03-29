using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.DebtCounselling.DebtCounselling
{
    internal class GetDebtCounsellingTasksStatement : TaskBaseStatement
    {
        public GetDebtCounsellingTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone,
            string businessProcessName, string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT I.ID AS InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Account] = I.NAME
    ,I.Subject
    ,WL.Message
    ,[Updated|" + Formats.Date.Default + @"] = CASE 
        WHEN LatestNote.NoteDate > I.StateChangeDate
            THEN LatestNote.NoteDate
        ELSE I.StateChangeDate
        END
    ,[Days At State|" + Formats.Number.Default + @"] = [2am].dbo.fBusinessDayDiff(I.StateChangeDate, getdate())
    ,[Diary Date|" + Formats.Date.Default + @"] = DC.DiaryDate
    ,[Months In Arrears|" + Formats.Number.Default + @"] = ISNULL(FAA.MonthsInArrears, 0)
FROM [X2].[X2].Instance AS I WITH (NOLOCK)
INNER JOIN [X2].[X2DATA].Debt_Counselling AS DCD WITH (NOLOCK) ON DCD.InstanceId = I.ID
INNER JOIN [X2].[X2].WorkList AS WL WITH (NOLOCK) ON WL.InstanceID = I.ID
INNER JOIN [2AM].[DebtCounselling].[DebtCounselling] AS DC WITH (NOLOCK) ON DC.DebtCounsellingKey = DCD.DebtCounsellingKey
LEFT JOIN [dw].[dwwarehousepre].securitisation.FactAccountAttribute AS FAA WITH (NOLOCK) ON DC.AccountKey = FAA.AccountKey
LEFT JOIN (
    SELECT max(InsertedDate) AS NoteDate
        ,n.GenericKey
    FROM [2am].dbo.Note n
    INNER JOIN [2am].dbo.NoteDetail nd ON n.noteKey = nd.noteKey
    WHERE genericKeyTypeKey = 27
    GROUP BY genericKey
    ) AS LatestNote ON 1 = 1
    AND dc.DebtCounsellingKey = LatestNote.GenericKey
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
    }
}
