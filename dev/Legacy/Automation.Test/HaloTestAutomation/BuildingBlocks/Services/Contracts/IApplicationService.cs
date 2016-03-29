using Automation.DataAccess;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IApplicationService
    {
        IEnumerable<Automation.DataModels.OfferAttributeType> GetOfferAttributeTypes();

        void DeleteOfferDebitOrder(int offerKey);

        void InsertEmploymentRecords(int offerKey);

        bool UpdateOfferMortgageLoanPropertyKey(int propertyKey, int offerKey);

        QueryResults GetOfferByStateAndfinancialAdjustmentSourceType(int numberOfRecords, string workflowState,
        FinancialAdjustmentTypeSourceEnum financialAdjustmentSourceType, int minLTV, int maxLTV);

        QueryResults GetActiveOfferRolesByOfferRoleType(int offerkey, OfferRoleTypeEnum offerRoleType);

        bool CreateOfferRole(int legalEntityKey, int offerKey, OfferRoleTypeEnum offerRoleType, GeneralStatusEnum generalStatus);

        Automation.DataModels.OfferMailingAddress GetOfferMailingAddress(int offerKey);

        void UpdateCorrespondenceMedium(int offerKey, CorrespondenceMediumEnum correspondenceMedium);

        int GetRandomOfferWithOfferExpense(string workflowName, string workflowState, OfferTypeEnum offerType, double maxFee, ExpenseTypeEnum expenseType);

        int GetOfferAccountKey(int offerKey, bool isReservedAccountKey = false);

        string GetADUserNameOfFirstActiveOfferRole(int offerKey, OfferRoleTypeEnum offerRoleType);

        int GetLegalEntityKeyOfFirstActiveOfferRole(int offerKey, OfferRoleTypeEnum offerRoleType);

        int GetFirstApplicantLegalEntityKeyOnOffer(int offerKey);

        long GetFirstApplicantIDNumberOnOffer(int offerKey);

        Automation.DataModels.Offer GetRandomOfferRecord(ProductEnum product, OfferTypeEnum offerType, OfferStatusEnum status);

        Automation.DataModels.Offer GetRandomLatestOfferInformationMortgageLoanRecord(MortgageLoanPurposeEnum purpose, ProductEnum product, OfferStatusEnum offerStatus);

        QueryResults GetClientOfferRolesNotOnAccount(int offerKey);

        List<int> GetOfferWithoutMailingAddress(int noOffers);

        void CleanupNewBusinessOffer(int offerKey);

        void CleanupOfferForEmpiricaScoreTests(int offerKey);

        string GetADUserByActiveOfferRoles(int offerKey, OfferRoleTypeEnum offerRoleType);

        int GetOfferInformationRecordCount(int offerKey);

        int NewRolesCount(int offerKey, OfferTypeEnum offerType);

        QueryResults GetApplicationDeclarations(int offerKey, int legalEntityKey);

        QueryResults GetExternalApplicationDeclarations(int offerKey, int legalEntityKey);

        QueryResults GetOfferFinancialAdjustmentsByType(int offerKey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource);

        IEnumerable<Automation.DataModels.OfferExpense> GetOfferExpenses(int offerkey);

        QueryResults GetOfferInformationVariableLoanRecordsByOfferKeyOrderedDesc(int offerKey);

        bool OfferRoleExists(int legalEntityKey, int offerKey, GeneralStatusEnum generalStatus);

        QueryResults GetOfferConditions(int offerKey);

        QueryResults GetLatestOfferInformationByOfferKey(int offerKey);

        QueryResults GetOfferData(int offerKey);

        QueryResults GetOfferInformationRecordsByOfferKey(int offerKey);

        IEnumerable<Automation.DataModels.OfferDebitOrder> GetOfferDebitOrder(int offerKey);

        IEnumerable<Automation.DataModels.ITC> GetITCRecordsByOfferKey(int offerKey);

        QueryResults GetOpenApplicationCaptureOffer();

        void UpdateOfferStartDate(DateTime offerStartDate, int offerKey);

        int GetApplicationCaptureOfferWith2Applicants();

        int GetApplicationCaptureOfferWith2ApplicantsWhereLegalEntitiesHaveSalutationAndInitials();

        QueryResults GetOfferMortgageLoanByOfferKey(int offerKey);

        QueryResults GetLastOfferCreatedByADUser(string adUserName);

        QueryResults GetTestOfferByStateAndAdUserForDuplicatePropertyRuleTest(int records, string state, string offerTypeKeys, string adUsername);

        QueryResults GetOfferByStateAndAdUserForDuplicatePropertyRuleTest(int records, string state, string offerTypeKeys, string adUsername);

        QueryResults GetSellerLegalName(int offerKey);

        QueryResults GetOfferInformationPersonalLoanRecordsByOfferKey(int offerkey);

        QueryResults GetOfferExpenseByOfferKey(int offerkey, string expensetype);

        void InsertOfferMailingAddress(int offerKey);

        void CleanUpOfferDebitOrder(int offerKey);

        void DeleteOfferMailingAddress(int offerKey);

        IEnumerable<Automation.DataModels.OfferAttribute> GetOfferAttributes(int offerKey);

        Automation.DataModels.PersonalLoanApplication GetPersonalLoanApplication(int offerKey);

        void UpdateNewPurchaseVariableLoanOffer(int offerkey, float householdIncome, float loanAgreementAmount, float cashDeposit, float propertyValuation, float feesTotal, float instalment, float bondToRegister, float linkRate, float term,
            MarketRateEnum marketRateKey, int rateConfigurationKey, EmploymentTypeEnum employmentType, float purchasePrice);

        void InsertSettlementBanking(int offerKey);

        void DeleteAllOfferClientDomiciliumAddresses(int offerKey);

        void UpdateAllOfferClientDomiciliumAddresses(int offerkey, GeneralStatusEnum domiciliumStatus);

        Automation.DataModels.LegalEntityDomicilium InsertOfferRoleDomicilium(int legalEntityDomiciliumKey, int offerrolekey, int offerKey);

        IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByOfferKey(int offerKey, OfferRoleTypeGroupEnum offerRoleTypeGroup);

        IEnumerable<Automation.DataModels.LegalEntityDomicilium> GetOfferRoleDomiciliums(int offerKey);

        void CleanupAllClientAddressesForOffer(int applicationKey, bool p, GeneralStatusEnum generalStatus);

        IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByLegalEntityKey(int legalEntityKey, OfferRoleTypeGroupEnum offerRoleTypeGroup);

        IEnumerable<Automation.DataModels.ExternalRole> GetExternalRolesByOfferKey(int offerKey);

        void DeleteExternalRoleDeclarations(int offerKey);

        void InsertDeclarations(int offerKey, GenericKeyTypeEnum genericKeyTypeKey, OriginationSourceProductEnum originationSourceProductKey);

        void CleanUpOfferDomicilium(int offerKey);

        int GetOfferByPropertyKey(int propertyKey);

        void InsertExternalRoleDomicilium(int offerKey);

        Automation.DataModels.LegalEntityDomicilium InsertExternalRoleDomicilium(int legalEntityDomiciliumKey, int externalRoleKey, int offerKey);

        void InsertOfferAttribute(int offerKey, OfferAttributeTypeEnum offerAttributeType);

        void DeleteOfferAttribute(int offerKey, OfferAttributeTypeEnum offerAttributeType);

        double GetLoanAgreementAmount(int offerkey);

        IEnumerable<Automation.DataModels.OfferRole> GetOfferRoleAttributes(int offerKey);

        int GetApplicationEmploymentType(int offerKey);

        IEnumerable<Automation.DataModels.Offer> GetOpenFurtherLendingOffersAtStateByAccountKey(int accountKey, string stateName);

        void UpdateAllMainApplicantEmploymentRecords(int offerKey, int employmentTypeKey, float householdIncome, bool GEPFfunded = false);

        int GetMaxOfferKey();

        Automation.DataModels.Offer GetCapitecMortgageLoanApplication();

        IEnumerable<Automation.DataModels.OfferRole> GetActiveOfferRolesByLegalEntityKeys(int[] legalEntityKeys, OfferRoleTypeGroupEnum offerRoleTypeGroup);

        int GetOfferByLegalEntityKey(int legalEntityKey);

        bool OfferKeyExists(int applicationKey);

        int GetLegalEntityKeyFromOfferKey(int offerKey);

        void CleanUpLegalEntityRequiredFields(int offerKey);

        IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppCapWithoutCapitalisedInitiationFeeOfferAttribute();

        IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppCapeWith100PercentLTV();

        IEnumerable<Automation.DataModels.Offer> GetAlphaOffersAtAppMan();

        int GetOfferByWorkflowState(string workflowState);

        int GetOfferByOfferTypeAndWorkflowState(int offerType, string workflowState);

        void InsertAffordabilityAssessment(int affordabilityAssessmentStatusKey, int offerKey);

    }
}