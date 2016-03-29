using Automation.DataAccess;
using Automation.DataModels;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILifeService
    {
        QueryResults GetLifePolicyType(int offerKey);

        QueryResults GetLifeLegalEntityAddresses(int offerKey);

        QueryResults GetLegalEntitiesFromLifeAccountRoles(int offerKey);

        QueryResults GetLifePolicyStatus(int lifeAccountKey);

        QueryResults GetTestData(string tableName);

        QueryResults GetRandomAddresses(int offerKey, int noResults, string addressFormat);

        IEnumerable<LifeLead> GetLifeLeads(int mortgageAccountKey);
        IEnumerable<LifeLead> GetLifeLeads(string workflowState, int mortgageAccountKey, LifePolicyTypeEnum lifePolicyTypeEnum);
        IEnumerable<LifeLead> GetLifeLeads(string workflowState, string consultantName, LifePolicyTypeEnum lifePolicyTypeEnum);

        LifePolicyClaim InsertCreditLifePolicyClaim(Automation.DataModels.LifePolicyClaim newClaim);

        FinancialService GetCreditLifePolicyFinancialService(int personalLoanAccountKey);

        void ClearCreditLifePolicyClaims(int creditLifePolicyFinancialServiceKey);

        IEnumerable<Automation.DataModels.LifePolicyClaim> GetCreditLifePolicyClaims(int creditLifePolicyFinancialService);

        IEnumerable<Automation.DataModels.ExternalLifePolicy> GetExternalLifePolicyByAccount(int accountKey);

        void ReAssignLeadToConsultant(int mortgageLoanAccountKey,string consultantName);

        void CreateInstance(int mortgageLoanNumber);

        IEnumerable<QualifyingAccount> GetQualifyingMortgageAccountsForLife(int count);

        IEnumerable<LifeOfferAssignment> GetLifeOfferAssignments(int mortgageLoanAccountKey);

        IEnumerable<string> GetLifeWorkflowStateNames();

        IEnumerable<LifeLead> GetCreatedLifeLeads();
    }
}