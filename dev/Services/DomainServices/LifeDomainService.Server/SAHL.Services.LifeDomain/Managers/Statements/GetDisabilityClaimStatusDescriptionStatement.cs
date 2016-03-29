using SAHL.Core.Data;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class GetDisabilityClaimStatusDescriptionStatement : ISqlStatement<string>
    {
        public int DisabilityClaimKey { get; protected set; }

        public GetDisabilityClaimStatusDescriptionStatement(int disabilityClaimKey)
        {
            DisabilityClaimKey = disabilityClaimKey;
        }

        public string GetStatement()
        {
            return @"select dcs.Description
from [2am].[dbo].[DisabilityClaim] dc 
join [2am].[dbo].[DisabilityClaimStatus] dcs on dcs.DisabilityClaimStatusKey = dc.DisabilityClaimStatusKey
where dc.DisabilityClaimKey = @DisabilityClaimKey";
        }
    }
}