using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.Origination.QuickCash
{
    internal class GetQuickCashTasksStatement : TaskBaseStatement
    {
        public GetQuickCashTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT DISTINCT I.ID AS InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Applicaiton No.] = I.[Name]
    ,I.Subject
    ,[Type] = OT.Description
    ,[Loan Amount|" + Formats.Currency.Default + @"] = (
        SELECT TOP 1 OIL.LoanAgreementAmount
        FROM [2am]..OfferInformationVariableLoan OIL WITH (NOLOCK)
        INNER JOIN [2am]..[OfferInformation] OI WITH (NOLOCK) ON OI.OfferInformationKey = OIL.OfferInformationKey
        WHERE convert(VARCHAR, OI.OfferKey) = I.[Name]
        ORDER BY OIL.OfferInformationKey DESC
    )
    ,[Updated|" + Formats.DateTime.Default + @"] = I.StateChangeDate
    ,WL.[Message]
FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
INNER JOIN [2am]..[Offer] O WITH (NOLOCK) ON convert(VARCHAR, O.OfferKey) = I.[Name]
INNER JOIN [2am]..[OfferType] OT WITH (NOLOCK) ON OT.OfferTypeKey = O.OfferTypeKey
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
