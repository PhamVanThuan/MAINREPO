using SAHL.Core.Data;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetDisabilityClaimInstanceSubjectStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimInstanceSubjectStatement(int disabilityClaimKey)
        {
            DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"select [2am].[dbo].[LegalEntityLegalName]([LegalEntityKey], 0)
from [2am].[dbo].[DisabilityClaim]
where [DisabilityClaimKey] = @DisabilityClaimKey";
        }
    }
}