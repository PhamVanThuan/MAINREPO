using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System.Data;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public enum GeneralStatusKey
    {
        Active = 1,
        Inactive = 2,
        All = 3
    }

    public partial interface IAccount : IEntityValidation
    {
        IApplication GetLatestApplicationByType(OfferTypes OfferType);

        /// <summary>
        /// Returns the household income for the account.  Note that this relies on a computed database column so
        /// ensure the object has been persisted (and if necessary refreshed) before using this value.
        /// </summary>
        double GetHouseholdIncome();

        /// <summary>
        /// Returns the latest BehaviouralScore imported into 2am from the basel import job
        /// </summary>
        IAccountBaselII GetLatestBehaviouralScore();

        /// <summary>
        /// The PTI for the Account
        /// This is always based on the Amortising Instalment
        /// </summary>
        double CalcAccountPTI { get; }

        /// <summary>
        /// Return a Table list of Account LegalEntities with an indication of whether the
        /// the ReasonDefinition provided exists
        /// this is to confirm whether there are any LegalEntities that are not marked as
        /// Dead or sequestrated, and to provide detail for the emails
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        DataTable GetAccountRoleNotificationByTypeAndDecription(ReasonTypes rt, ReasonDescriptions rd);

        IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes);

        IReadOnlyEventList<ILegalEntity> GetLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey);

        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes);

        IReadOnlyEventList<ILegalEntityNaturalPerson> GetNaturalPersonLegalEntitiesByRoleType(IDomainMessageCollection Messages, int[] RoleTypes, GeneralStatusKey StatusKey);

        void RemoveRolesForLegalEntity(IDomainMessageCollection Messages, ILegalEntity LegalEntity, int[] RoleTypes);

        string GetLegalName(LegalNameFormat Format);

        SAHL.Common.Globals.AccountTypes AccountType { get; }

        //IHOC GetHOC();
        IFinancialService GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType);

        //        IReadOnlyEventList<IFinancialService> GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType);
        IReadOnlyEventList<IFinancialService> GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType, AccountStatuses[] Statuses);

        //        IReadOnlyEventList<IFinancialService> GetFinancialServicesByType(SAHL.Common.Globals.FinancialServiceTypes FinancialServiceType, bool Active);
        IAccount GetRelatedAccountByType(SAHL.Common.Globals.AccountTypes AccountType, IEventList<IAccount> RelatedAccounts);

        int GetEmploymentTypeKey();

        IEventList<IAccount> GetNonProspectRelatedAccounts();

        bool HasBeenInArrears(int Months, float Minimum);

        IAccountInstallmentSummary InstallmentSummary { get; }

        bool IsActive { get; }

        /// <summary>
        /// A property that shows if an active Debt Counselling case exists
        /// for this Account;
        /// </summary>
        bool UnderDebtCounselling { get; }

        double CommittedLoanValue { get; }

        bool HasActiveSubsidy { get; }

        bool IsThirtyYearTerm { get; }

        bool HasAccountInformationType(AccountInformationTypes accountInformationType);
    }
}