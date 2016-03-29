using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public partial interface IApplication : IEntityValidation
    {
        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType);

        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType, GeneralStatuses GeneralStatus);

        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup);

        IApplicationRole GetLatestApplicationRoleByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType);

        IApplicationRole GetLatestApplicationRoleByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup);

        IApplicationRole GetFirstApplicationRoleByGroup(SAHL.Common.Globals.OfferRoleTypeGroups OfferRoleTypeGroup);

        IApplicationRole GetFirstApplicationRoleByType(SAHL.Common.Globals.OfferRoleTypes OfferRoleType);

        IReadOnlyEventList<IApplicationRole> ApplicationRoles { get; }

        void AddLeadRole(Lead_MortgageLoan_Role roletype, ILegalEntity LE);

        void AddApplicationRole(Application_MortgageLoan_Role roletype, ILegalEntity LE);

        IApplicationRole AddRole(int applicationRoleTypeKey, ILegalEntity LE);

        void RemoveRolesForLegalEntity(IDomainMessageCollection Messages, ILegalEntity LegalEntity, int[] ApplicationRoleTypes);

        IApplicationInformation GetLatestApplicationInformation();

        IApplicationProduct CurrentProduct { get; }

        bool IsOpen { get; }

        /// <summary>
        /// Gets the application status when the object is first loaded.  This will not change during
        /// the lifetime of the object.  If you wish to change the status of the application and
        /// persist, use <see cref="ApplicationStatus"/>.  For newly created applications, this will
        /// always be null.
        /// </summary>
        IApplicationStatus ApplicationStatusPrevious { get; }

        IApplicationProduct[] ProductHistory { get; }

        void CalculateApplicationDetail(bool IsBondExceptionAction, bool keepMarketRate);

        void CreateRevision();

        //Replaced by method below
        //IList<ILegalEntity> GetLegalEntitiesByRoleType(RoleTypes[] RoleTypes);

        IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(OfferRoleTypes[] OfferRoleTypes);

        IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(OfferRoleTypes[] OfferRoleTypes, GeneralStatusKey StatusKey);

        /// <summary>
        /// Get External Roles for the Application
        /// NB: this is for non mortgage loan applications
        ///     i.e.: Personal Loans
        /// </summary>
        /// <param name="externalRoleType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonsByExternalRoleType(SAHL.Common.Globals.ExternalRoleTypes externalRoleType, GeneralStatuses status);

        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] OfferRoleTypes);

        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] OfferRoleTypes, GeneralStatusKey StatusKey);

        /// <summary>
        /// Returns the household income stored as part of the
        /// </summary>
        /// <returns></returns>
        double GetHouseHoldIncome();

        /// <summary>
        /// Returns the sum of confirmed income, taking into account all Legal Entities that have Income Contributor Roles
        /// </summary>
        /// <returns></returns>
        double DetermineConfirmedHouseHoldIncome();

        /// <summary>
        /// Gets the highest and second highest income contributors on an offer
        /// </summary>
        /// <param name="primaryApplicantLEKey">LegalEntityKey of the highest income earner or -1 if none</param>
        /// <param name="secondaryApplicantLEKey">LegalEntityKey of the 2nd highest income earner or -1 if none</param>
        void GetPrimaryAndSecondaryApplicants(out int primaryApplicantLEKey, out int secondaryApplicantLEKey);

        #region RoddersCalcHouseHoldIncomeStuff

        ///// <summary>
        ///// Calculates the HouseIncome, taking into account all Legal Entities that have Income Contributor Roles.
        ///// Both confirmed and unconfirmed income is considered for this calculation.
        ///// </summary>
        //void RecalculateHouseHoldIncome();

        ///// <summary>
        ///// Calculates the HouseIncome, taking into account all Legal Entities that have Income Contributor Roles.
        ///// Only confirmed income is considered for this calculation.
        ///// </summary>
        //void CalculateHouseHoldIncomeConfirmed();

        #endregion RoddersCalcHouseHoldIncomeStuff

        /// <summary>
        /// Calculates the HouseIncome, taking into account all Legal Entities that have Income Contributor Roles.
        /// This common method checks the Offer/Application Information Status to determine whether the calculation
        /// should use unconfirmed income or not.
        /// </summary>
        void CalculateHouseHoldIncome();

        /// <summary>
        /// Groups all monthly income from contributing LegalEntities by employment type.
        /// Uses the greatest income to determine and set the Application income type.
        /// This method determines whether confirmed income only should be used.
        /// </summary>
        void SetEmploymentType();

        /// <summary>
        /// Manually determined employment type which the system will never touch unless when
        /// replacing it with another manually determined employment type.
        /// </summary>
        /// <param name="employmentTypeKey"></param>
        void SetManuallySelectedEmploymentType(int employmentTypeKey);

        string GetLegalName(LegalNameFormat Format);

        /// <summary>
        /// Returns whether or not an application has a specified OfferAttributeType
        /// </summary>
        /// <param name="OfferAttributeType"></param>
        /// <returns></returns>
        bool HasAttribute(OfferAttributeTypes OfferAttributeType);

        /// <summary>
        /// Determine what the Applicant Type for the Application should be and set it on the application object
        /// If the main Applicant is CC, Company or Trust, Select appropriate applicant type,
        /// if company is 1 main applicant natural person, select ‘Single’, if there is
        /// more than 1 main applicant as natural person, select ‘Joint’
        /// </summary>
        void SetApplicantType();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        void PricingForRisk();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        double GetRateAdjustments();

        /// <summary>
        /// Check for existing Financial Adjustments
        /// </summary>
        /// <param name="fats"></param>
        /// <returns></returns>
        bool HasFinancialAdjustment(FinancialAdjustmentTypeSources fats);

        /// <summary>
        /// Determins if a loan is an alpha housing loan
        /// </summary>
        /// <returns></returns>
        bool IsAlphaHousing();

        /// <summary>
        /// Determines if the application is a Capitec loan, by checking if it has the 'Capitec Loan' OfferAttribute
        /// </summary>
        /// <returns></returns>
        bool IsCapitec();

        /// <summary>
        /// Determines if the application is a Comcorp loan, by checking if it has the 'Comcorp Loan' OfferAttribute
        /// </summary>
        /// <returns></returns>
        bool IsComcorp();

        IReadOnlyEventList<ILegalEntity> ActiveClients { get; }

        IVendor GetComcorpVendor();

        bool HasCondition(string ConditionName);

        bool IsGEPF();
    }
}