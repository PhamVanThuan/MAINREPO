using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ISPVTestService
    {
        IEnumerable<Automation.DataModels.SPV> GetActiveChildSPVs();

        void UpdateSPVDescription(int spvKey, string description);
    }
}