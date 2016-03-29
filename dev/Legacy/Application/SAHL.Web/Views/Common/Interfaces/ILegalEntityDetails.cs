using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.ReadOnly;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILegalEntityDetails : IViewBase
    {
        #region Declare View Events

        event KeyChangedEventHandler OnReBindLegalEntity;

        /// <summary>
        /// Called when the Cancel button is clicked
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Called when the Submit button is clicked
        /// </summary>
        event EventHandler onSubmitButtonClicked;

        /// <summary>
        /// Called when the LegalEntityType changes.
        /// </summary>
        event KeyChangedEventHandler onLegalEntityTypeChange;

        #endregion Declare View Events

        #region Declare Presenter Actions

        /// <summary>
        /// Populate the list of marketing options.
        /// </summary>
        /// <param name="marketingoptions"></param>
        void BindMarketingOptons(IDictionary<int, string> marketingoptions);

        /// <summary>
        /// Populate the details of a new Legal Entity to be saved.
        /// </summary>
        /// <param name="legalEntity"></param>
        void PopulateLegalEntityDetailsForAdd(ILegalEntity legalEntity);

        /// <summary>
        /// Populate the details of a new Legal Entity to be saved.
        /// </summary>
        /// <param name="legalEntity"></param>
        void PopulateLegalEntityDetailsForUpdate(ILegalEntity legalEntity);

        /// <summary>
        /// Bind a list of <see cref="ILegalEntityType"/> objects.
        /// </summary>
        /// <param name="LegalEntityType"></param>
        /// <param name="DefaultValue"></param>
        void BindLegalEntityTypes(IDictionary<string, string> LegalEntityType, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="IRoleType"/> objects.
        /// </summary>
        /// <param name="RoleType"></param>
        /// <param name="DefaultValue"></param>
        void BindRoleTypes(IDictionary<string, string> RoleType, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ISalutation"/> objects.
        /// </summary>
        /// <param name="Salutation"></param>
        /// <param name="DefaultValue"></param>
        void BindSalutation(IDictionary<string, string> Salutation, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="IGender"/> objects.
        /// </summary>
        /// <param name="Gender"></param>
        /// <param name="DefaultValue"></param>
        void BindGender(IDictionary<string, string> Gender, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="IMaritalStatus"/> objects.
        /// </summary>
        /// <param name="MaritalStatus"></param>
        /// <param name="DefaultValue"></param>
        void BindMaritalStatus(IDictionary<string, string> MaritalStatus, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="IPopulationGroup"/> objects.
        /// </summary>
        /// <param name="PopulationGroup"></param>
        /// <param name="DefaultValue"></param>
        void BindPopulationGroup(IDictionary<string, string> PopulationGroup, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="IEducation"/> objects.
        /// </summary>
        /// <param name="Education"></param>
        /// <param name="DefaultValue"></param>
        void BindEducation(IDictionary<string, string> Education, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ICitizenType"/> objects.
        /// </summary>
        /// <param name="CitizenType"></param>
        /// <param name="DefaultValue"></param>
        void BindCitizenType(IDictionary<string, string> CitizenType, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ILanguage"/> objects.
        /// </summary>
        /// <param name="HomeLanguage"></param>
        /// <param name="DefaultValue"></param>
        void BindHomeLanguage(ICollection<ILanguageReadOnly> HomeLanguage, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ILanguage"/> objects.
        /// </summary>
        /// <param name="DocumentLanguage"></param>
        /// <param name="DefaultValue"></param>
        void BindDocumentLanguage(ICollection<ILanguageReadOnly> DocumentLanguage, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ILegalEntityStatus"/> objects.
        /// </summary>
        /// <param name="LegalEntityStatus"></param>
        /// <param name="DefaultValue"></param>
        void BindLegalEntityStatus(IDictionary<string, string> LegalEntityStatus, string DefaultValue);

        /// <summary>
        /// Bind the list of <see cref="ILegalEntityStatus"/> objects.
        /// </summary>
        /// <param name="ResidenceStatus"></param>
        /// <param name="DefaultValue"></param>
        void BindResidenceStatus(IDictionary<string, string> ResidenceStatus, string DefaultValue);

        /// <summary>
        /// Bind the list of Insurance Types
        /// </summary>
        /// <param name="InsurableInterestType"></param>
        /// <param name="DefaultValue"></param>
        void BindInsurableInterestType(IDictionary<string, string> InsurableInterestType, string DefaultValue);

        /// <summary>
        /// Binds the appropriate registration number depending on the <see cref="ILegalEntityType">LegalEntityType</see>.
        /// </summary>
        /// <param name="RegistrationNumber"></param>
        void BindRegistrationNumberLabel(string RegistrationNumber);

        /// <summary>
        /// Binds a single <see cref="ILegalEntityNaturalPerson">Natural Person legal entity</see> object.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityReadOnlyNaturalPerson(ILegalEntityNaturalPerson LegalEntity);

        /// <summary>
        /// Binds a single <see cref="ILegalEntity">Company LegalEntity</see> object.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityReadOnlyCompany(ILegalEntity LegalEntity);

        /// <summary>
        /// Binds a single <see cref="ILegalEntityNaturalPerson">Natural Person legal entity</see> object.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityUpdatableNaturalPerson(ILegalEntityNaturalPerson LegalEntity);

        /// <summary>
        /// Binds a single <see cref="ILegalEntityCompany">Company LegalEntity</see> object.
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityUpdatableCompany(ILegalEntity LegalEntity);

        /// <summary>
        /// Binds the InsurableInterest label to teh currently selected option.
        /// Primarily applies to life Presenters.
        /// </summary>
        /// <param name="InsurableInterest"></param>
        void BindInsurableInterestReadonly(string InsurableInterest);

        /// <summary>
        /// Sets (normally) the IntroductionDate
        /// </summary>
        /// <param name="IntroductionDate"></param>
        void BindIntroductionDate(DateTime IntroductionDate);

        /// <summary>
        /// Gets a ListItemCollection of MarketingOptions.
        /// </summary>
        /// <returns></returns>
        ListItemCollection MarketingOptionsExcluded { get; }

        #endregion Declare Presenter Actions

        #region Properties

        // <summary>
        // Gets a ListItemCollection of MarketingOptions.
        // </summary>
        //ListItemCollection MarketingOptions { get; }

        /// <summary>
        /// Gets the selected Legal Entity Type
        /// </summary>
        int SelectedLegalEntityType { get; }

        int SelectedCOLegalEntityTypeUpdate { get; }

        /// <summary>
        /// Gets the selected Role Type in Add mode
        /// </summary>
        int SelectedRoleTypAdd { get; }

        /// <summary>
        /// Gets the selected Natural Person Role Type in Update mode
        /// </summary>
        int SelectedRoleTypeUpdateNaturalPerson { get; }

        /// <summary>
        /// Gets the selected Natural Person Role Type in Update mode
        /// </summary>
        int SelectedRoleTypeUpdateCompany { get; }

        /// <summary>
        /// Gets the selected Insurable Interest Type
        /// </summary>
        int SelectedInsurableInterestTypeAdd { get; }

        /// <summary>
        /// Gets the selected Insurable Interest Type
        /// </summary>
        int SelectedInsurableInterestTypeUpdate { get; }

        /// <summary>
        /// Sets the text of the submit button (may be set to Submit or Add)
        /// </summary>
        string SubmitButtonText { set; }

        /// <summary>
        /// Sets whether the Marketing options are selectable
        /// </summary>
        bool MarketingOptionsEnabled { set; }

        /// <summary>
        /// Sets whether the CancelButton is visible
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Sets whether the SubmitButton is visible
        /// </summary>
        bool SubmitButtonVisible { set; }

        /// <summary>
        /// Sets the UpdateMyDetails flag
        /// </summary>
        bool SetUpdateMyDetails { set; }

        /// <summary>
        /// Sets whether the RoleType controls are visible
        /// </summary>
        bool RoleTypeVisible { set; }

        /// <summary>
        /// Sets whether the CitizenType controls are visible
        /// </summary>
        bool CitizenTypeVisible { set; }

        /// <summary>
        /// Sets whether the DateOfBirth controls are visible
        /// </summary>
        bool DateOfBirthVisible { set; }

        /// <summary>
        /// Sets whether the PassportNumber controls are visible
        /// </summary>
        bool PassportNumberVisible { set; }

        /// <summary>
        /// Sets whether the TaxNumber controls are visible
        /// </summary>
        bool TaxNumberVisible { set; }

        /// <summary>
        /// Sets whether the Gender controls are visible
        /// </summary>
        bool GenderVisible { set; }

        /// <summary>
        /// Sets whether the MaritalStatus controls are visible
        /// </summary>
        bool MaritalStatusVisible { set; }

        /// <summary>
        /// Sets whether the Status controls are visible
        /// </summary>
        bool StatusVisible { set; }

        /// <summary>
        /// Sets whether the PopulationGroup controls are visible
        /// </summary>
        bool PopulationGroupVisible { set; }

        /// <summary>
        /// Sets whether the InsurableInterest controls are visible
        /// </summary>
        bool InsurableInterestVisible { set; }

        /// <summary>
        /// Sets whether pnlAdd is visible
        /// </summary>
        bool PanelAddVisible { set; }

        /// <summary>
        /// Sets whether pnlNaturalPersonAdd is visible
        /// </summary>
        bool PanelNaturalPersonAddVisible { set; }

        /// <summary>
        /// Sets whether pnlNaturalPersonDisplay is visible
        /// </summary>
        bool PanelNaturalPersonDisplayVisible { set; }

        /// <summary>
        /// Sets whether pnlCompanyAdd is visible
        /// </summary>
        bool PanelCompanyAddVisible { set; }

        /// <summary>
        /// Sets whether Marketing Option Panel is visible
        /// </summary>
        bool PanelMarketingOptionPanelVisible { set; }

        /// <summary>
        /// Sets whether all marketing options excluded are automatically checked
        /// </summary>
        bool SelectAllMarketingOptionsExcluded { set; }

        /// <summary>
        /// Sets whether pnlCompanyDisplay is visible.
        /// </summary>
        bool PanelCompanyDisplayVisible { set; }

        /// <summary>
        /// Sets whether the InsurableInterest controls are visible (Applies to both the label and dropdown controls)
        /// </summary>
        bool InsurableInterestUpdateVisible { set; }

        /// <summary>
        /// Sets whether the InsurableInterest controls are visible (Applies to both the label and dropdown controls)
        /// </summary>
        bool InsurableInterestDisplayVisible { set; }

        /// <summary>
        /// Sets whether the update control group is visible.
        /// </summary>
        bool UpdateControlsVisible { set; }

        /// <summary>
        /// Sets whether the controls that are normaly locked should be made visible (for update).
        /// Used for legal entities with prospect accounts or dirty ID numbers.
        /// </summary>
        bool LockedUpdateControlsVisible { get; set; }

        /// <summary>
        /// Disables non-contact details (used by the LifeContactUpdate presenter).
        /// </summary>
        bool NonContactDetailsDisabled { set; }

        /// <summary>
        /// Enable update if the LegalEntity has active financial services.
        /// </summary>
        bool ApplicantsUpdateWithActiveFinancialServices { set; }

        ///// <summary>
        ///// Enable update if the LegalEntity has no active financial services.
        ///// </summary>
        //bool ApplicantsUpdateWithoutActiveFinancialServices { set;}

        /// <summary>
        /// Used by the ApplicantAddExisting presenter. Enable updating of the role.
        /// </summary>
        bool ApplicantAddExistingRoleVisible { set; }

        /// <summary>
        /// Locks the DOB.
        /// </summary>
        bool LimitedUpdate { set; }

        /// <summary>
        /// Sets whether or not the RoleType is visible.
        /// </summary>
        bool AddRoleTypeVisible { set; }

        bool UpdateRoleTypeVisible { set; }

        bool DisplayRoleTypeVisible { set; }

        int ApplicantRoleTypeKey { get; set; }

        /// <summary>
        /// Sets whether or not the LegalEntityType is enabled.
        /// </summary>
        bool LegalEntityTypeEnabled { set; }

        /// <summary>
        /// Sets whether Income Contributer checkbox is visible
        /// </summary>
        bool IncomeContributorVisible { set; }

        /// <summary>
        /// Gets/Sets whether the Legal Entity is an income contributor
        /// </summary>
        bool SelectedIncomeContributor { get; set; }

        /// <summary>
        /// Disables Ajax functionality.
        /// </summary>
        bool DisableAjaxFunctionality { set; }

        #endregion Properties
    }
}