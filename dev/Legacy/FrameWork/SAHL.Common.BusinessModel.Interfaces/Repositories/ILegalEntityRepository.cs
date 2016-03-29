using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    /// The legal entity repository.
    /// </summary>
    public interface ILegalEntityRepository
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        IEventList<ILegalEntityAssetLiability> GetLegalEntityAssetLiabilityList(int LegalEntityKey);

        ILegalEntityMarketingOption GetEmptyLegalEntityMarketingOption();

        IList<IAffordabilityType> GetAffordabilityTypes();

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IExternalRole> GetExternalRoles(GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, int LegalEntityKey);

        /// <summary>
        /// Gets all roles on all accounts that the given legal entity has a role on.  The origination
        /// source of the current user will be taken into account and AccountStatus 6 (Application
        /// prior to Instruct Attorney) is excluded.
        /// </summary>
        IReadOnlyEventList<IRole> GetRelatedRolesByLegalEntity(SAHLPrincipal principal, int LegalEntityKey);

        [Obsolete("This should not be used - use account.Roles collection instead.")]
        IReadOnlyEventList<IRole> GetRelatedRolesByAccount(SAHLPrincipal principal, int AccountKey);

        IRole GetEmptyRole();

        void SaveRole(IRole role);

        void DeactivateRole(IRole role);

        void ActivateRole(IRole role);

        #region Delete methods

        /// <summary>
        /// "Deletes" a legal entity address from the system by setting the GeneralStatus to Inactive.
        /// </summary>
        /// <param name="legalEntityAddress">The legal entity address.</param>
        void DeleteAddress(ILegalEntityAddress legalEntityAddress);

        /// <summary>
        /// "Deletes" a legal entity bank account from the system by setting the GeneralStatus to Inactive.
        /// </summary>
        /// <param name="legalEntityBankAccountKey">The legal entity bank account key.</param>
        /// <param name="user">The user performing the delete operation.</param>
        void DeleteLegalEntityBankAccount(int legalEntityBankAccountKey, SAHLPrincipal user);

        #endregion Delete methods

        #region Retrieval Methods

        /// <summary>
        /// Gets a Legal Entity by the Legal Entity Key.
        /// </summary>
        /// <param name="LegalEntityKey">The integer legal entity key.</param>
        /// <returns>The <see cref="ILegalEntity">Legal Entity found using the supplied legal entity key, returns null if no legal entity is found.</see></returns>
        ILegalEntity GetLegalEntityByKey(int LegalEntityKey);

        /// <summary>
        /// Returns a single LegalEntityNaturalPerson based on ,matching ID Number
        /// </summary>
        /// <param name="IdentityNo"></param>
        /// <returns></returns>
        ILegalEntityNaturalPerson GetNaturalPersonByIDNumber(string IdentityNo);

        /// <summary>
        /// Gets a list of legal entities whose ID Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByIDNumber(string prefix, int maxRowCount);

        /// <summary>
        /// Gets a list of legal entities whose Passport Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByPassportNumber(string prefix, int maxRowCount);

        /// <summary>
        /// Gets a list of companies whose Registration Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityCompany> GetCompaniesByRegistrationNumber(string prefix, int maxRowCount);

        /// <summary>
        /// Gets a list of trusts whose Registration Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityTrust> GetTrustsByRegistrationNumber(string prefix, int maxRowCount);

        /// <summary>
        /// This method is used to remove the current affordability recordset before adding a fresh one for the offer
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <param name="LegalEntityKey"></param>
        void ClearLegalEntityOfferAffordabilityData(int OfferKey, int LegalEntityKey);

        /// <summary>
        /// Gets a list of close corporations whose Registration Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityCloseCorporation> GetCloseCorporationsByRegistrationNumber(string prefix, int maxRowCount);

        /// <summary>
        /// Gets a dictionary of companies/close corporation and/or trusts whos Registration Number starts with <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IDictionary<string, string> GetRegistrationNumbersForCompanies(string prefix, int maxRowCount);

        /// <summary>
        /// Gets an empty LegalEntityAffordability object that can later be saved to the database.
        /// </summary>
        /// <returns></returns>
        ILegalEntityAffordability GetEmptyLegalEntityAffordability();

        ///// <summary>
        ///// Gets an empty legal entity that can later be saved to the database.
        ///// </summary>
        ///// <returns></returns>
        //ILegalEntity GetEmptyLegalEntity();

        /// <summary>
        /// Gets an empty legal entity that can later be saved to the database. Overloaded to accept the <see cref="ILegalEntityType"/>.
        /// </summary>
        /// <param name="legalEntityType">The Legal Entity type that must be returned.</param>
        /// <returns></returns>
        ILegalEntity GetEmptyLegalEntity(LegalEntityTypes legalEntityType);

        /// <summary>
        /// Saves an address against a legal entity.  Note that this will create the link between the address and
        /// the legal entity if it does not exist already.  If you want to alter an existing ILegalEntityAddress
        /// use the SaveLegalEntityAddress method instead.
        /// </summary>
        /// <param name="addressType">The address type (postal/residential)</param>
        /// <param name="legalEntity">The legal entity.</param>
        /// <param name="address">The address to add.  If this is a new address, it will be created in the database.</param>
        /// <param name="effectiveDate">The date that this address will become effective on.</param>
        void SaveAddress(IAddressType addressType, ILegalEntity legalEntity, IAddress address, DateTime effectiveDate);

        /// <summary>
        /// Saves a single legal entity.
        /// </summary>
        /// <param name="legalEntity">The legal entity.</param>
        /// <param name="recalculateApplicationDetail">A boolean used to determine if any common recalculations eg: Household income should be executed.</param>
        void SaveLegalEntity(ILegalEntity legalEntity, bool recalculateApplicationDetail);

        /// <summary>
        /// Saves a single legal entity with the specified exception status
        /// </summary>
        /// <param name="legalEntity">The legal entity.</param>
        /// <param name="legalEntityExceptionStatus">The LegalEntityExceptionStatus enumerator.</param>
        void SaveLegalEntityWithExceptionStatus(ILegalEntity legalEntity, LegalEntityExceptionStatuses legalEntityExceptionStatus);

        /// <summary>
        /// Saves a bank account against a legal entity.
        /// </summary>
        /// <param name="legalEntity">The legal entity.</param>
        /// <param name="bankAccount">The bank account to add.  If the bank account is new, it will be created in the database.</param>
        /// <param name="principal">The user adding the bank account to the legal entity.</param>
        void SaveBankAccount(ILegalEntity legalEntity, IBankAccount bankAccount, SAHLPrincipal principal);

        /// <summary>
        /// Saves a LegalEntityBankAccount object
        /// </summary>
        /// <param name="legalEntityBankAccount"></param>
        void SaveLegalEntityBankAccount(ILegalEntityBankAccount legalEntityBankAccount);

        /// <summary>
        /// Performs a simple search on legal entities with a straight query (does not use full-text technology).
        /// Note that if FirstNames is set, an or comparison is run against FirstNames, PreferredName and Initials.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <param name="maxResults">The maximum number of rows.</param>
        /// <returns></returns>
        /// <remarks>
        /// This will current only search natural person type legal entities, but has been left
        /// as ILegalEntity in case this changes.
        /// </remarks>
        IEventList<ILegalEntity> SearchLegalEntities(IClientSearchCriteria searchCriteria, int maxResults);

        /// <summary>
        /// Searches all legal entities that have an account, filtered by the origination source of the current
        /// user.
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        IEventList<ILegalEntity> SuperSearchForClientLegalEntities(IClientSuperSearchCriteria SearchCriteria);

        /// <summary>
        /// Searches all legal entities (only the legalentities - this does not take accounts or applications into account).
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        IEventList<ILegalEntity> SuperSearchForAllLegalEntities(IClientSuperSearchCriteria SearchCriteria);

        /// <summary>
        /// Saves a legal entity address with a new address.
        /// </summary>
        /// <param name="legalEntityAddress"></param>
        /// <param name="address"></param>
        void SaveLegalEntityAddress(ILegalEntityAddress legalEntityAddress, IAddress address);

        #endregion Retrieval Methods

        ILegalEntityRelationship CreateLegalEntityRelationship();

        void SaveLegalEntityRelationship(ILegalEntityRelationship ler);

        void DeleteLegalEntityRelationship(ILegalEntityRelationship ler);

        IEventList<ILegalEntityAssetLiability> GetLegalEntityAssetLiabilitiesByAssetLiabilityKey(int assetLiabilityKey);

        IAssetLiability GetEmptyAssetLiability(AssetLiabilityTypes assetLiabilityTypes);

        ILegalEntityAssetLiability GetEmptyLegalEntityAssetLiability();

        ILegalEntityBankAccount GetEmptyLegalEntityBankAccount();

        ILegalEntityBankAccount GetLegalEntityBankAccountByKey(int Key);

        IEmploymentType GetLegalEntityEmploymentTypeForApplication(ILegalEntity legalEntity, IApplication application);

        double GetLegalEntityIncomeForApplication(ILegalEntity legalEntity, IApplication application);

        void SaveLegalEntityAssetLiability(ILegalEntityAssetLiability leAssetLiability);

        void DeleteAssetLiability(IAssetLiability ae);

        void UpdateLegalEntityType(int legalEntityKey, int legalEntityTypeKey);

        /// <summary>
        /// Checks to see whether a legal entity has account roles or application roles other than leadmainapplicant or leadsuretor
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        bool HasNonLeadRoles(ILegalEntity legalEntity);

        void SaveLegalEntityEstateAgent(ILegalEntity legalEntity, bool recalculateApplicationDetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="recalculateApplicationDetail"></param>
        void SaveLegalEntityPaymentDistributionAgent(ILegalEntity legalEntity, bool recalculateApplicationDetail);

        /// <summary>
        /// Save Legal Entity Debt Counsellor
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="recalculateApplicationDetail"></param>
        void SaveLegalEntityDebtCounsellor(ILegalEntity legalEntity, bool recalculateApplicationDetail);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntity"></param>
        void NotifyChangeOfIDNumberLinkedToLifePolicy(ILegalEntity legalEntity);

        IReadOnlyEventList<IApplication> GetOpenFurtherLendingApplicationsByLegalEntity(ILegalEntity legalEntity);

        #region Attorney Methods

        IDictionary<int, string> GetAttorneysByDeedsOffice(int deedsOfficeKey);

        IAttorney GetAttorneyByKey(int attorneyKey);

        IAttorney CreateEmptyAttorney();

        void SaveAttorney(IAttorney attorney);

        #endregion Attorney Methods

        #region Legal Entity Role

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="GenericTypeKey"></param>
        ///// <param name="GenericKey"></param>
        ///// <returns></returns>
        //IList<ILegalEntityRole> GetLegalEntityRoles(int GenericTypeKey, int GenericKey);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="GenericTypeKey"></param>
        ///// <param name="GenericKey"></param>
        ///// <param name="GeneralStatusKey"></param>
        ///// <returns></returns>
        //IList<ILegalEntityRole> GetLegalEntityRoles(int GenericTypeKey, int GenericKey, int GeneralStatusKey);

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="legalEntityRole"></param>
        //void SaveLegalEntityRole(ILegalEntityRole legalEntityRole);

        #endregion Legal Entity Role

        #region External Role

        /// <summary>
        /// Save Legal Entity Role
        /// </summary>
        /// <param name="ExternalRole"></param>
        void SaveExternalRole(IExternalRole ExternalRole);

        /// <summary>
        ///
        /// </summary>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IExternalRole> GetExternalRolesByLegalEntity(int LegalEntityKey);

        /// <summary>
        ///
        /// </summary>
        IExternalRole GetEmptyExternalRole();

        /// <summary>
        /// Get External Roles
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="generalStatus"></param>
        /// <param name="LegalEntityKey"></param>
        /// <returns></returns>
        IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes genericKeyType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus, int LegalEntityKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ExternalRoleType"></param>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyType"></param>
        /// <param name="LegalEntityKey"></param>
        /// <param name="DeactivateExisitingRoles"></param>
        /// <returns></returns>
        IExternalRole InsertExternalRole(ExternalRoleTypes ExternalRoleType, int GenericKey, GenericKeyTypes GenericKeyType, int LegalEntityKey, bool DeactivateExisitingRoles);

        /// <summary>
        ///
        /// </summary>
        /// <param name="attorneyKey"></param>
        /// <returns></returns>
        DataTable GetExternalRolesByAttorneyKey(int attorneyKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="externalRoleTypeGroup"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        IReadOnlyEventList<IExternalRole> GetExternalRoles(int genericKey, int genericKeyTypeKey, ExternalRoleTypeGroups externalRoleTypeGroup, GeneralStatuses generalStatus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="externalRoleTypeGroup"></param>
        /// <param name="generalStatus"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByExternalRoleTypeGroup(int genericKey, int genericKeyTypeKey, ExternalRoleTypeGroups externalRoleTypeGroup, GeneralStatuses generalStatus);

        #endregion External Role

        #region Legal Entity Login

        /// <summary>
        /// Get Empty Legal Entity Login
        /// </summary>
        /// <returns></returns>
        ILegalEntityLogin GetEmptyLegalEntityLogin();

        /// <summary>
        /// Get Legal Entity Logins
        /// </summary>
        /// <returns></returns>
        IEventList<ILegalEntityLogin> GetLegalEntityLogins();

        /// <summary>
        /// Get Legal Entity Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        ILegalEntityLogin GetLegalEntityLogin(string username, string password);

        /// <summary>
        /// Get Legal Entity Login
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        ILegalEntityLogin GetLegalEntityLogin(string username);

        /// <summary>
        /// Create Empty Legal Entity Login
        /// </summary>
        /// <returns></returns>
        ILegalEntityLogin CreateEmptyLegalEntityLogin();

        /// <summary>
        /// Save Legal Entity Login
        /// </summary>
        /// <param name="legalEntityLogin"></param>
        void SaveLegalEntityLogin(ILegalEntityLogin legalEntityLogin);

        /// <summary>
        /// Get Legal Entity
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        ILegalEntityNaturalPerson GetWebAccessLegalEntity(string emailAddress);

        /// <summary>
        /// Get Legal Entity
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        ILegalEntityNaturalPerson GetLegalEntityClientByEmail(string emailAddress);

        /// <summary>
        /// GetAllClientAccountsByLegalEntityKey
        /// used via the client secure website
        /// this is regressed to work like the old website and should be amended asap!
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        IList<Int32> GetAllClientAccountsByLegalEntityKey(int legalEntityKey);

        /// <summary>
        /// Get Attorney
        /// </summary>
        /// <param name="externalRole"></param>
        /// <returns></returns>
        IAttorney GetAttorney(IExternalRole externalRole);

        /// <summary>
        /// Get External Role
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        IExternalRole GetExternalRole(ILegalEntityNaturalPerson legalEntity, ExternalRoleTypes externalRoleType);

        #endregion Legal Entity Login

        #region Legal Entity By OfferKey

        IReadOnlyEventList<IExternalRole> GetLegalEntityInformationByOfferKey(int genericKey);

        #endregion Legal Entity By OfferKey

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        ILegalEntityDomicilium CreateEmptyLegalEntityDomicilium();

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDomiciliumKey"></param>
        /// <returns></returns>
        ILegalEntityDomicilium GetLegalEntityDomiciliumByKey(int legalEntityDomiciliumKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDomiciliumToSave"></param>
        void SaveLegalEntityDomicilium(ILegalEntityDomicilium legalEntityDomiciliumToSave);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDomicilium"></param>
        void SaveAndActivateLegalEntityDomiciliumAndDeactivatePrevious(ILegalEntityDomicilium legalEntityDomicilium);

        /// <summary>
        ///
        /// </summary>
        /// <param name="addressKey"></param>
        /// <returns></returns>
        IEventList<ILegalEntityDomicilium> GetLegalEntityDomiciliumsForAddressKey(int addressKey);

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalEntityDomicilium"></param>
        void DeleteLegalEntityDomicilium(ILegalEntityDomicilium legalEntityDomicilium);

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        bool ClientRequestAdditionalFunds(int accountKey, decimal amount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        int? GetSubsidyAccountKey(int legalEntityKey);
    }
}