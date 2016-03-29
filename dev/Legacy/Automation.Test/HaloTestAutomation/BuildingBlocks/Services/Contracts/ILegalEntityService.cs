using Automation.DataAccess;
using Automation.DataModels;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILegalEntityService
    {
        QueryResultsRow GetFirstLegalEntityOnAccount(int accountKey);

        string GetLegalEntityIDNumberNotLinkedToAccount(int[] accountkeys);

        string GetLegalEntityLegalNameByIDNumber(string idNumber);

        void UpdateDateOfBirth(int legalEntityKey, DateTime dateOfBirth);

        string GetRandomLegalEntityIdNumberOnAccount();

        Int32 CreateNewLegalEntity(string emailAddress, string idnumber);

        Automation.DataModels.LegalEntity GetLegalEntity(string idNumber = "", string registrationNumber = "", string registeredname = "", string legalname = "", int legalentitykey = 0);

        QueryResultsRow GetLegalEntityIDNumberNotLinkedToOffer(int offerKey);

        Automation.DataModels.LegalEntity GetRandomLegalEntityRecord(LegalEntityTypeEnum leType);

        string AddNewLegalEntity(int offerKey, OfferRoleTypeEnum ort, bool hasApplicationMailingAddress, out int newLEKey, out int originalMaAddressKey);

        void CleanUpNewLegalEntity(int leKey, int offerKey, OfferRoleTypeEnum ort, int maAddressKey);

        void AddNewBankAccount(int leKey, int offerKey, out int bankAccountKey, out int legalEntityBankAccountKey, out int offerDebitOrderKey);

        void UpdateCitizenTypeForApplicants(int citizenTypeKey, int offerKey);

        void InsertITC(int offerKey, int empiricaUpperBound, int empiricaLowerBound);

        QueryResults GetLegalEntityLegalNamesForOffer(int offerKey);

        Automation.DataModels.LegalEntityEmployment GetRandomLegalEntityEmploymentRecord(EmploymentStatusEnum employmenstatus, RemunerationTypeEnum remunType, EmploymentTypeEnum employType);

        string GetLegalEntityLegalName(int legalentityKey);

        string GetLegalEntityIDNumber(int legalEntityKey);

        List<Automation.DataModels.LegalEntityRole> GetActiveLegalEntityRoles(int accountkey, RoleTypeEnum roleTypeKey);

        List<Automation.DataModels.LegalEntityRole> GetAllLegalEntityRoles(int accountkey);

        List<int> GetTwoOpenMortgageLoanAccountsForSameLegalEntity();

        QueryResults LegalEntitiesWithMoreThanOneAccount();

        Automation.DataModels.Account GetLegalEntityLoanAccount(int legalentitykey);

        void UpdateLegalEntityIDNumber(string idNumber, int legalEntityKey);

        QueryResultsRow GetUnrelatedLegalEntity(LegalEntityTypeEnum legalEntityType);

        QueryResultsRow GetFirstLegalEntityMainApplicantOnAccount(int accountKey);

        Automation.DataModels.LegalEntity GetRandomLegalEntityRecord(LegalEntityTypeEnum legalEntityType,
            LanguageEnum documentLanguageKey, LegalEntityExceptionStatusEnum legalEntityExceptionStatusKey = LegalEntityExceptionStatusEnum.None,
            MaritalStatusEnum maritalStatusKey = MaritalStatusEnum.Unknown, CitizenTypeEnum citizenShipType = CitizenTypeEnum.SACitizen,
            string registrationNumberExclusion = "", string IdNumberExclusionExpression = "", string firstnamesExclusionExpression = "",
            string surnameExclusionExpression = "", string registeredNameExclusionExpression = "");

        Automation.DataModels.LegalEntityBankAccount InsertLegalEntityBankAccount(int legalEntityKey);

        Automation.DataModels.LegalEntityAssetLiabilities InsertLegalEntityAssetLiability(Automation.DataModels.LegalEntityAssetLiabilities legalEntityAssetLiability);

        Automation.DataModels.LegalEntityAssetLiabilities GetLatestLegalEntityAssetLiabilityRecord(int legalentitykey);

        IEnumerable<Automation.DataModels.LegalEntityAssetLiabilities> GetLegalEntityAssetLiabilities(int legalentitykey);

        void DeleteLegalEntityAssetLiabilities(int legalentitykey, int assetLiabilitykey);

        IEnumerable<Automation.DataModels.AssetLiabilityType> GetAssetLiabilityTypes();

        Automation.DataModels.LegalEntityOrganisationStructure GetEmptyLegalEntityOrganisationStructure();

        List<Automation.DataModels.Account> GetLegalEntityLoanAccountsByIDNumber(string idNumber);

        Automation.DataModels.SubsidyProvider GetSubsidyProvider(string registeredName);

        string GetLegalEntityIDNumberNotPlayRole(RoleTypeEnum roleTypeEnum, bool over18Years = true);

        string GetLegalEntityIDNumberPlaySameRoleTwiceDifferentAccounts(RoleTypeEnum roleTypeEnum, bool over18Years = true, bool inclHOC = true);

        Automation.DataModels.SubsidyProvider InsertSubsidyProvider(Automation.DataModels.SubsidyProvider subsidyProviderToInsert);

        void DeleteSubsidyProvider(int legalentitykey, int addresskey);

        Automation.DataModels.LegalEntity GetNaturalPersonLegalEntity();

        Automation.DataModels.LegalEntity GetCompanyLegalEntity();

        QueryResults GetAssetsAndLiabilitiesByLegalEntityIdNumber(string IdNumber);

        string GetLegalEntityCitizenType(string idNumber);

        IEnumerable<Automation.DataModels.LegalEntityRole> GetLegalEntityRoles(int accountKey, bool isfullLegalName = true);

        QueryResults GetFirstLegalEntityAndFormattedAddressOnOffer(int offerkey, GeneralStatusEnum legalentityAddressStatus);

        IEnumerable<Automation.DataModels.LegalEntityRole> GetLegalEntityRoles(string idnumber);

        QueryResults GetLegalEntityRelationship(int legalentityKey);

        int GetLegalEntityKeyByTradeName(string tradeName);

        int GetLegalEntityKeyByFirstNames(string firstNames);

        string GetActiveEstateAgencyTradingName();

        QueryResults GetLegalEntityLegalNameByLegalEntityKey(int legalEntityKey);

        QueryResults GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(int legalEntityKey, int offerKey);

        QueryResults GetLegalEntityAffordabilityExpensesByLegalEntityKeyAndOfferKey(int legalEntityKey, int offerKey);

        QueryResults GetLegalEntityAffordabilityByLegalEntityKeyAndOfferKey(int legalEntityKey, string offerKey);

        int CountAffordabilityTypeRecords();

        int CountAffordabilityTypeIncomeRecords();

        int CountAffordabilityTypeExpenseRecords();

        string GetIDNumberForRelationship();

        void GetExistingRelationship(int legalEntityKey, string _legalEntityIDNumber, out string existingRelationship, out string idNumber);

        bool LegalEntitiesWithIDNumberExist(string idnumber);

        string GetLegalEntityLegalNameByPassportNumber(string passportNumber);

        QueryResults GetLegalEntityNamesAndRoleByAccountKey(int accountKey);

        string GetLegalEntityLegalNameByRegistrationNumber(string regNumber);

        int GetLegalEntityKeyByIdNumber(string IdNumber);

        QueryResults ClientSuperSearchFirstLegalEntity();

        void UpdateLegalEntityLoginPasswords();

        void UpdateLegalEntityLogin(string emailAddress, GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus);

        Automation.DataModels.LegalEntityLogin GetAttorneyAccessLogin(GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus);

        Automation.DataModels.LegalEntityLogin GetAttorneyAccessLogin(GeneralStatusEnum legalEntityLoginStatus, GeneralStatusEnum externalRoleStatus,
            string emailAddress);

        Automation.DataModels.LegalEntityLogin GetAttorneyAccessLoginLinkedToAttorney(int attorneyKey, GeneralStatusEnum legalEntityLoginStatus,
            GeneralStatusEnum externalRoleStatus);

        Automation.DataModels.LegalEntity GetMainApplicantLegalEntityWithPersonalLoanOffer(int offerkey);

        Automation.DataModels.LegalEntity GetMainApplicantLegalEntityWithoutPersonalLoanOffer(LegalEntityTypeEnum legalEntityTypeKey = LegalEntityTypeEnum.NaturalPerson);

        void DeleteLegalEntityAffordabilities(int legalEntityKey, bool isExpense);

        void CreateLegalEntityAffordabilities(int genericKey);

        void CreateApplicationAffordabilities(int genericKey, int affordabilityAssessmentStatus);

        void DeleteLegalEntitySubsidy(int legalEntityKey);

        void DeleteITC(int legalEntityKey);

        void UpdateITC(int legalEntityKey, DateTime changeDate);

        int GetRelatedMortgageLoanAccountKeyByOfferKey(int offerKey);

        void UpdateLegalEntityInitials(int leKey, string initials);

        void UpdateEmailAddress(int legalEntityKey, string emailAddress);

        void UpdateCellphone(int legalEntityKey, string cellphone);

        LegalEntity GetClientWithExistingPassword();

        int CreateNewLegalEntityAsDebtCounsellor(string debtCounsellorName);

        LegalEntity GetClientWithAccessToSecureWebsite();

        void DeleteLegalEntity(int legalEntityKey);

        LegalEntity GetClientWhoHasNeverRegisteredForSecureWebsite();

        IEnumerable<Automation.DataModels.LegalEntityLogin> GetClientSecureWebsiteLogin(int legalEntityKey);

        IEnumerable<Automation.DataModels.LegalEntityReturningDiscountQualifyingData> GetLegalEntityReturningDiscountQualifyingDataWithValidIDNumber();

        void UpdateLegalEntityContactDetails(int legalEntity, LegalEntityContactDetails newContactDetails);

        IEnumerable<Automation.DataModels.Account> GetLegalEntityLoanAccounts(int legalentitykey, AccountStatusEnum accountStatusKey);

        void UpdateDocumentLanguage(int legalEntityKey, LanguageEnum language);

        void UpdateHomeLanguage(int legalEntityKey, LanguageEnum language);

        void UpdateEducationLevel(int legalEntityKey, EducationEnum educationLevel);

        void UpdatePreferredName(int legalEntityKey, string name);

        void UpdateLegalEntitySalutationToNull(int legalEntityKey);

        void UpdateLegalEntityInitialsToNull(int legalEntityKey);

        void UpdateLegalEntityGenderToNull(int legalEntityKey);

        void UpdateLegalEntityMaritalStatusToNull(int legalEntityKey);

        void UpdateLegalEntityCitizenTypeToEmpty(int legalEntityKey);

        void UpdateLegalEntityDateOfBirthToEmpty(int legalEntityKey);

        IEnumerable<ITC> GetITCs(int expectedLegalEntityKey);

        IEnumerable<LegalEntity> GetDirtyNaturalPeopleOnAccounts(GeneralStatusEnum accountStatus);

        DateTime? GetLegalEntityDateOfBirth(string idNumber);
    }
}