using Automation.DataAccess.DataHelper;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;

namespace BuildingBlocks.Services
{
    public class DisabilityClaimService : _2AMDataHelper, IDisabilityClaimService
    {
        DisabilityClaim IDisabilityClaimService.GetDisabilityClaim(int disabilityClaimKey)
        {
            DisabilityClaim results = base.GetDisabilityClaim(disabilityClaimKey);
            return results;
        }


        public DisabilityClaim GetDisabilityClaimByLegalEntityAndAccountKey(int legalEntityKey, int accountKey)
        {
            DisabilityClaim results = base.GetDisabilityClaimByLegalEntityAndAccountKey(legalEntityKey, accountKey);
            return results;
        }
    }
}