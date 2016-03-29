using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IPersonalLoanService
    {
        void RemoveCreditLifePolicyFromPersonalLoanOffer(ref Automation.DataModels.PersonalLoanApplication personalLoanApplication);

        void AddExternalLife(int offerKey, Automation.DataModels.ExternalLifePolicy externalLifePolicy);

        void AddSAHLLife(int offerKey);

        IEnumerable<Automation.DataModels.LegalEntity> GetXNumberOfLegalEntitiesWhoQualifyForPersonalLoans(int p);

        IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntityDetailsForCapitecApplication(int offerKey);

        IEnumerable<Automation.DataModels.LegalEntity> GetLegalEntityDetailsForCapitecAccount(int accountKey);

        List<string> WaitUntilAllBatchLeadsHaveBeenCreated(int expectedNumberOfCases);

        bool CheckLegalEntityHasValidPersonalLoanApplication(string idNumber, string workflowState);
    }
}