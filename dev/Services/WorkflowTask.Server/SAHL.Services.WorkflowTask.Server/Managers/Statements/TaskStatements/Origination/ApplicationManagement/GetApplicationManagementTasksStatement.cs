using SAHL.Core.DataType;
using System.Collections.Generic;

namespace SAHL.Services.WorkflowTask.Server.Managers.Statements.TaskStatements.Origination.ApplicationManagement
{
    internal class GetApplicationManagementTasksStatement : TaskBaseStatement
    {
        public GetApplicationManagementTasksStatement(string username, List<string> roles, bool includeTasksWithTheRoleOfEveryone,
            string businessProcessName, string workFlowName, List<string> workFlowStateNames, string storageTable)
            : base(username, roles, includeTasksWithTheRoleOfEveryone, businessProcessName, workFlowName, workFlowStateNames, storageTable)
        {
        }

        public override string GetStatement()
        {
            return @"
SELECT WL.InstanceID
    ,TagIds = (" + tagStatementSubQuery + @")
    ,OriginationSource = CASE
        WHEN oac.OfferAttributeTypeKey IS NOT NULL
            THEN 'Capitec'
        ELSE 'SAHL'
        END

    ,[Application No.] = WL.OfferKey
    ,[Account No.] = WL.ReservedAccountKey
    ,WL.Subject
    ,[Type] = CASE 
        WHEN oa.OfferAttributeTypeKey IS NOT NULL
            THEN ot.Description + ' (' + oat.Description + ')'
        ELSE ot.Description
        END
    ,[Loan Amount|" + Formats.Currency.Default + @"] = oivl.LoanAgreementAmount
    ,[Updated|" + Formats.DateTime.Default + @"] = WL.StateChangeDate
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
        INNER JOIN [X2].[X2DATA].Application_Management X2D WITH (NOLOCK) ON X2D.InstanceID = I.ID
        INNER JOIN [X2].[X2].[State] STA WITH (NOLOCK) ON I.StateID = STA.ID
        INNER JOIN [X2].[X2].WorkFlow WKF WITH (NOLOCK) ON I.WorkFlowID = WKF.ID
        INNER JOIN [X2].[X2].Process PRO WITH (NOLOCK) ON WKF.ProcessID = PRO.ID
        WHERE PRO.ViewableOnUserInterfaceVersion LIKE '3%'
            AND STA.Name IN @WorkFlowStateNames
            AND WKF.Name = @WorkFlowName
            AND PRO.Name = @BusinessProcessName
            AND WL.ADUserName IN @UsernameAndRoles
        ) Inst
    INNER JOIN [2am]..[Offer] O WITH (NOLOCK) ON O.OfferKey = Inst.OfferKey
    INNER JOIN [2AM]..OfferInformation oi WITH (NOLOCK) ON oi.OfferKey = o.OfferKey
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
    ) WL
INNER JOIN [2AM]..OfferType ot WITH (NOLOCK) ON ot.OfferTypeKey = WL.OfferTypeKey
INNER JOIN [2AM]..OfferInformationVariableLoan oivl WITH (NOLOCK) ON oivl.OfferInformationKey = WL.OfferInformationKey
LEFT JOIN [2am].dbo.OfferAttribute oa WITH (NOLOCK) ON WL.OfferKey = oa.offerKey
    AND oa.offerAttributeTypeKey IN (5, 27)
LEFT JOIN [2am].dbo.OfferAttributeType oat WITH (NOLOCK) ON oa.offerAttributeTypeKey = oat.offerAttributeTypeKey
LEFT JOIN [2am].dbo.OfferAttribute oac(NOLOCK) ON oac.OfferKey = WL.OfferKey
    AND oac.OfferAttributeTypeKey = 30 -- capitec
";
        }
    }
}
