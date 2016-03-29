using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IBondLoanAgreementService
    {
        Automation.DataModels.Bond GetLatestBondRecordByOfferkey(int offerKey);

        IEnumerable<Automation.DataModels.Bond> GetAccountBonds(int accountKey);
    }
}