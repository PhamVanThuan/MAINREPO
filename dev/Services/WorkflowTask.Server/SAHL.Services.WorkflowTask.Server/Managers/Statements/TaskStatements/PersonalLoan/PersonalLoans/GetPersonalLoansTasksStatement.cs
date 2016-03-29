using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.PersonalLoan.PersonalLoans
{
    internal class GetPersonalLoansTasksStatement : TaskBaseStatement
    {
        public GetPersonalLoansTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT WL.InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Application No.] = WL.InstanceName
    ,WL.Subject
    ,[Amount|" + Formats.Currency.Default + @"] = isnull(oipl.LoanAmount, 0)
    ,[Updated|" + Formats.DateTime.Default + @"] = WL.StateChangeDate
    ,[Diary Date] = CONVERT(VARCHAR(10), DiaryDate, 111)
    ,WL.[Message]
    ,[Age In Days|" + Formats.Number.Default + @"] = DATEDIFF(day, WL.CreationDate, GETDATE())
    ,[Days At State|" + Formats.Number.Default + @"] = DATEDIFF(day, WL.StateChangeDate, GETDATE())
FROM (
    SELECT Inst.InstanceID
        ,Inst.StateChangeDate
        ,Inst.Subject
        ,Inst.InstanceName
        ,Inst.Priority
        ,Inst.DeadlineDate
        ,Inst.[Message]
        ,Inst.CreationDate
        ,O.OfferKey
        ,O.ReservedAccountKey
        ,O.OfferTypeKey
        ,max(oi.OfferInformationKey) OfferInformationKey
        ,note.DiaryDate
    FROM (
        SELECT I.ID AS InstanceID
            ,I.StateChangeDate
            ,I.Subject
            ,I.[Name] AS InstanceName
            ,I.Priority
            ,I.DeadlineDate
            ,I.CreationDate
            ,WL.[Message]
            ,X2D.ApplicationKEy AS OfferKey
        FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
        INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
        INNER JOIN [X2].[X2DATA].Personal_Loans X2D WITH (NOLOCK) ON X2D.InstanceID = I.ID
        INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
        INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
        INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
        WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
            AND S.Name IN @WorkFlowStateNames
            AND W.Name = @WorkFlowName
            AND PRO.Name = @BusinessProcessName
            AND WL.ADUserName IN @UsernameAndRoles
        ) Inst
    INNER JOIN [2am]..[Offer] O WITH (NOLOCK) ON O.OfferKey = Inst.OfferKey
    LEFT JOIN [2AM]..OfferInformation oi WITH (NOLOCK) ON oi.OfferKey = o.OfferKey
    LEFT JOIN [2am]..Note note ON note.GenericKey = O.OfferKey
        AND note.GenericKeyTypeKey = 2
    GROUP BY Inst.InstanceID
        ,Inst.StateChangeDate
        ,Inst.Subject
        ,Inst.InstanceName
        ,Inst.Priority
        ,Inst.DeadlineDate
        ,Inst.[Message]
        ,Inst.CreationDate
        ,O.OfferKey
        ,O.ReservedAccountKey
        ,O.OfferTypeKey
        ,note.NoteKey
        ,note.GenericKey
        ,note.GenericKeyTypeKey
        ,note.DiaryDate
    ) WL
INNER JOIN [2AM]..OfferType ot WITH (NOLOCK) ON ot.OfferTypeKey = WL.OfferTypeKey
LEFT JOIN [2AM]..OfferInformationPersonalLoan oipl WITH (NOLOCK) ON oipl.OfferInformationKey = WL.OfferInformationKey
";
        }
    }
}
