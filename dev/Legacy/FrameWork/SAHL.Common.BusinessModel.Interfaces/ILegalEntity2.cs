using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public enum LegalNameFormat
    {
        Full,
        FullNoSalutation,
        InitialsOnly,
        InitialsOnlyNoSalutation,
        SurnamesOnly
    }

    public partial interface ILegalEntity : IEntityValidation
    {
        /// <summary>
        ///
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Format"></param>
        /// <returns></returns>
        string GetLegalName(LegalNameFormat Format);

        /// <summary>
        ///
        /// </summary>
        bool IsUpdatable { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        ILifeInsurableInterest GetInsurableInterest(int AccountKey);

        /// <summary>
        /// Gets the legal number for the legal entity.  This will differ depending on the underlying legal entity type e.g. for
        /// a natural person the ID number will be returned.
        /// </summary>
        /// <remarks>This is for display purposes only.</remarks>
        string LegalNumber { get; }

        /// <summary>
        /// Gets the IRole for the legal entity on the specified Account.
        /// </summary>
        /// <remarks></remarks>
        IRole GetRole(int AccountKey);

        /// <summary>
        /// Gets the IApplicationRole of group Client for the legal entity on the specified Application.
        /// </summary>
        /// <remarks></remarks>
        IApplicationRole GetApplicationRoleClient(int ApplicationKey);

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a supplied list of role type groups.
        /// </summary>
        /// <param name="roleTypeGroups"></param>
        /// <returns></returns>
        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByRoleTypeGroups(params OfferRoleTypeGroups[] roleTypeGroups);

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a supplied list of role types.
        /// </summary>
        /// <param name="roleTypes">A list of role types.</param>
        /// <returns></returns>
        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByRoleTypes(params OfferRoleTypes[] roleTypes);

        /// <summary>
        /// Gets a list of all application roles for the legal entity for a particular application.
        /// </summary>
        /// <param name="applicationKey">The application key</param>
        /// <returns></returns>
        IReadOnlyEventList<IApplicationRole> GetApplicationRolesByApplicationKey(int applicationKey);

        /// <summary>
        /// Gets the type of the legal entity.   This will always correspond to the discriminated type of the object.
        /// </summary>
        ILegalEntityType LegalEntityType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="InterfaceType"></typeparam>
        /// <typeparam name="DAOType"></typeparam>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        object GetPreviousValue<InterfaceType, DAOType>(string PropertyName);

        /// <summary>
        /// A property that shows if an active Debt Counselling case exists
        /// for this LegalEntity;
        /// </summary>
        bool UnderDebtCounselling { get; }

        /// <summary>
        /// Legal Entity Login
        /// </summary>
        ILegalEntityLogin LegalEntityLogin { get; set; }

        /// <summary>
        /// Get active Debt Counselling cases that the Legal Entity plays an active role in
        /// </summary>
        IEventList<IDebtCounselling> DebtCounsellingCases { get; }

        IApplicationUnsecuredLending PersonalLoanApplication { get; }

        IDebtCounsellorDetail DebtCounsellorDetail { get;  set; }

        ILegalEntityAddress ActiveDomiciliumAddress { get; }

        ILegalEntityDomicilium ActiveDomicilium { get; }

        /// <summary>
        /// Gets the active external role of group Client for the legal entity on the specified Application.
        /// </summary>
        IExternalRole GetActiveClientExternalRoleForOffer(int applicationKey);
    }
}