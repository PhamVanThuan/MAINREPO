using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.LifeDomain.Models;

namespace SAHL.Services.LifeDomain.Managers.Statements
{
    public class UpdateDisabilityClaimStatusStatement :  ISqlStatement<int>
    {
        public int DisabilityClaimKey { get; protected set; }
        public int DisabilityClaimStatusKey { get; protected set; }

        public UpdateDisabilityClaimStatusStatement(int disabilityClaimKey, DisabilityClaimStatus disabilityClaimStatus)
        {
            DisabilityClaimKey = disabilityClaimKey;
            DisabilityClaimStatusKey = (int)disabilityClaimStatus;
        }

        public string GetStatement()
        {
            return @"update [2am].[dbo].DisabilityClaim set DisabilityClaimStatusKey = @DisabilityClaimStatusKey where DisabilityClaimKey = @DisabilityClaimKey";            
        }
    }
}