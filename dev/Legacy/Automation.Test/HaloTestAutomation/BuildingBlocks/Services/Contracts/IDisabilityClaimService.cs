using Automation.DataModels;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IDisabilityClaimService
    {
        DisabilityClaim GetDisabilityClaim(int disabilityClaimKey);

        IEnumerable<Automation.DataModels.DisabilityPaymentSchedule> GetDisabilityPaymentSchedule(int disabilityClaimKey);

        DisabilityClaim GetDisabilityClaimByLegalEntityAndAccountKey(int legalEntityKey, int accountKey);
    }
}