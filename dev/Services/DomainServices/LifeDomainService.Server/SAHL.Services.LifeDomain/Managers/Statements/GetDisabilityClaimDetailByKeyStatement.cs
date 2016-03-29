using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetDisabilityClaimDetailByKeyStatement : ISqlStatement<DisabilityClaimDetailModel>
    {
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimDetailByKeyStatement(int disabilityClaimKey)
        {
            DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"select dc.[DisabilityClaimKey]
                          ,dc.[AccountKey] as LifeAccountKey
                          ,a.[ParentAccountKey] as MortgageLoanAccountKey
                          ,dc.[LegalEntityKey]
	                      ,[2am].[dbo].[LegalEntityLegalName](dc.[LegalEntityKey], 0) ClaimantLegalEntityDisplayName
                          ,dc.[DateClaimReceived]
                          ,dc.[LastDateWorked]
                          ,dc.[DateOfDiagnosis]
                          ,dc.[ClaimantOccupation]
                          ,dc.[DisabilityTypeKey]
	                      ,dt.[Description] DisabilityType
                          ,dc.[OtherDisabilityComments]
                          ,dc.[ExpectedReturnToWorkDate]
                          ,dc.[DisabilityClaimStatusKey]
	                      ,ds.[Description] DisabilityClaimStatus
                          ,dc.[PaymentStartDate]
                          ,dc.[NumberOfInstalmentsAuthorised]
                          ,dc.[PaymentEndDate]
                    from [2am].[dbo].[DisabilityClaim] dc
                    join [2am].[dbo].[DisabilityClaimStatus] ds on ds.DisabilityClaimStatusKey = dc.DisabilityClaimStatusKey
                    join [2am].[dbo].[Account] a on a.AccountKey = dc.AccountKey
                    left join [2am].[dbo].[DisabilityType] dt on dt.DisabilityTypeKey = dc.DisabilityTypeKey
                    where dc.[DisabilityClaimKey] = @DisabilityClaimKey";
        }
    }
}