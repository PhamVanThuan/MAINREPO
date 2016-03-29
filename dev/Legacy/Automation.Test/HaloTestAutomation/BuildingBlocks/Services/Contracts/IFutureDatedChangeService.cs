using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IFutureDatedChangeService
    {
        IEnumerable<Automation.DataModels.FutureDatedChange> GetFutureDatedChangeByIdentifierReference(int identifierReference);

        void DeleteFutureDatedChangeByIdentifierReference(int identifierReference);
    }
}