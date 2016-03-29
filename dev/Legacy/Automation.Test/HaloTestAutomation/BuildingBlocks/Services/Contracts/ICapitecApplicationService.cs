using SAHL.Services.Capitec.Models.Shared;
using System.Collections.Generic;
namespace BuildingBlocks.Services.Contracts
{
    public interface ICapitecApplicationService
    {
        void CreateCapitecApplication(CapitecApplication capitecApplication);
        int GetUnusedOfferKey();
    }
}
