using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.Cap2Offers.Cap2Offers
{
    internal class GetCap2OffersTasksStatement : TaskBaseStatement
    {
        public GetCap2OffersTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone, string businessProcessName,
            string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT DISTINCT WL.InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")

    ,[Loan No.] = CO.AccountKey
    ,WL.Subject
    ,[Updated|" + Formats.DateTime.Default + @"] = WL.StateChangeDate
    ,Status = WL.STATUS
    ,[Offer Expires|" + Formats.DateTime.Default + @"] = CTC.OfferEndDate
FROM (
    SELECT I.ID AS InstanceID
        ,I.StateChangeDate
        ,I.Subject
        ,S.NAME AS STATUS
        ,X2D.CapOfferKey
    FROM [X2].[X2].[Worklist] WL WITH (NOLOCK)
    INNER JOIN [X2].[X2].[Instance] I WITH (NOLOCK) ON WL.InstanceID = I.ID
    INNER JOIN [X2].[X2].[State] S WITH (NOLOCK) ON I.StateID = S.ID
    INNER JOIN [X2].[X2DATA].CAP2_Offers X2D ON X2D.InstanceID = I.ID
    INNER JOIN [X2].[X2].[Workflow] W WITH (NOLOCK) ON S.WorkflowID = W.ID
    INNER JOIN [X2].[X2].[Process] PRO WITH (NOLOCK) ON w.ProcessID = PRO.ID
    WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
        AND S.Name IN @WorkFlowStateNames
        AND W.Name = @WorkFlowName
        AND PRO.Name = @BusinessProcessName
        AND WL.ADUserName IN @UsernameAndRoles
    ) WL
INNER JOIN [2am]..[CapOffer] CO WITH (NOLOCK) ON WL.CapOfferKey = CO.CapOfferKey
INNER JOIN [2am]..[CapTypeConfiguration] CTC WITH (NOLOCK) ON CO.CapTypeConfigurationKey = CTC.CapTypeConfigurationKey
";
        }
    }
}
