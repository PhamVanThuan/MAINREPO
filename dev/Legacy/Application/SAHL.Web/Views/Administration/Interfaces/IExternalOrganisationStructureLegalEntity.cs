using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration.Interfaces
{
	/// <summary>
    /// ExternalOrganisationStructureLegalEntity Interface
	/// </summary>
    public interface IExternalOrganisationStructureLegalEntity : IViewBase
	{

        #region Declare View Events

        event KeyChangedEventHandler OnAddressTypeSelectedIndexChanged;

        event KeyChangedEventHandler OnAddressFormatSelectedIndexChanged;

        event KeyChangedEventHandler OnOrganisationTypeSelectedIndexChanged;

        event KeyChangedEventHandler OnReBindLegalEntity;

        event KeyChangedEventHandler SelectAddressButtonClicked;

        /// <summary>
        /// Called when the Cancel button is clicked
        /// </summary>
        event EventHandler onCancelButtonClicked;

        /// <summary>
        /// Called when the Submit button is clicked
        /// </summary>
        event EventHandler onSubmitButtonClicked;

        #endregion

        #region Declare Presenter Actions

        void SetUpAddressReadOnly();

        bool AddressFormatVisible { set;}

        bool AddressTypeVisible { set;}

        bool AddressCaptureEnabled { set; get; }

        bool AddressHidden { set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReadOnly"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void SetupDisplay(bool ReadOnly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address"></param>
        void BindAddressDetails(IAddress Address);

        /// <summary>
        /// Clears address details
        /// </summary>
        void ClearAddress();

        /// <summary>
        /// Populate the Address details of a new Legal Entity to be saved.
        /// </summary>
        /// <param name="legalEntity"></param>
        void BindLegalEntityAddress(ILegalEntity legalEntity);

        /// <summary>
        /// Populate the details of a new Legal Entity to be saved.
        /// </summary>
        /// <param name="legalEntity"></param>
        void PopulateLegalEntityDetails(ILegalEntity legalEntity);

        /// <summary>
        /// Bind a list of <see cref="ILegalEntityType"/> objects.
        /// </summary>
        /// <param name="LegalEntityType"></param>
        /// <param name="DefaultValue"></param>
        void BindLegalEntityTypes(IDictionary<string, string> LegalEntityType, string DefaultValue);

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
        /// Bind the list of <see cref="ILegalEntityStatus"/> objects.
        /// </summary>
        /// <param name="LegalEntityStatus"></param>
        /// <param name="DefaultValue"></param>
        void BindLegalEntityStatus(IDictionary<string, string> LegalEntityStatus, string DefaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrgStructureDescriptions"></param>
        /// <param name="DefaultValue"></param>
        void BindOrgStructureDesctiption(IDictionary<string, string> OrgStructureDescriptions, string DefaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrganisationTypes"></param>
        /// <param name="DefaultValue"></param>
        void BindOrganisationType(IDictionary<string, string> OrganisationTypes, string DefaultValue);

        /// <summary>
        /// Binds a single <see cref="ILegalEntity">LegalEntity</see> object.
        /// </summary>
        /// <param name="le"></param>
        /// <param name="OrgStructureDescription"></param>
        /// <param name="OrganisationTypeDescription"></param>
        void BindLegalEntity(ILegalEntity le, string OrgStructureDescription, string OrganisationTypeDescription);

        /// <summary>
        /// Sets (normally) the IntroductionDate
        /// </summary>
        /// <param name="IntroductionDate"></param>
        void BindIntroductionDate(DateTime IntroductionDate);

        void BindAddressTypeDropDown(IDictionary<int, string> addressTypes);

        void BindAddressFormatDropDown(IDictionary<int, string> addressFormatList);

        #endregion

        #region Properties

        bool DisableAjaxFunctionality { set; }

        bool LegalEntityTypeReadOnly { set;}

        bool OSDescriptionTypeAddReadOnly { set;}

        bool OrganisationTypeReadOnly { set;}

        bool SubmitButtonVisible { set;}

        int AddressFormatSelectedIndex { set;}

        int GetAjaxAddressKey { get;}

        AddressFormats GetSetAddressFormat { get;set;}

        DateTime GetSetEffectiveDate { get;set;}

        int GetAddressFormatSelectedValue { get;set;}

        bool SetAddressDetailsVisibility { set;}

        int GetAddressTypeSelectedValue { get;}

        IAddress GetCapturedAddress { get;}

        /// <summary>
        /// Gets the selected Legal Entity Type
        /// </summary>
        int SelectedLegalEntityType { get; }

        /// <summary>
        /// The OrganisationType fro the LE selected
        /// </summary>
        IOrganisationType OrganisationType { get; }

        /// <summary>
        /// The Description for the OrgStructure table
        /// </summary>
        string OrganisationStructureDescription { get; }      

        /// <summary>
        /// Sets the text of the submit button (may be set to Submit or Add)
        /// </summary>
        string SubmitButtonText { set;}

        /// <summary>
        /// Sets whether pnlNaturalPersonAdd is visible
        /// </summary>
        bool PanelNaturalPersonAddVisible { set;}

        /// <summary>
        /// Sets whether pnlCompanyAdd is visible
        /// </summary>
        bool PanelCompanyAddVisible { set;}

        /// <summary>
        /// Sets whether Company IntroductionDate is Disabled
        /// </summary>
        bool DisableCompanyIntroductionDate { set; }

        /// <summary>
        /// Sets whether Company TradingName is Disabled
        /// </summary>
        bool DisableCompanyTradingName { set; }

        /// <summary>
        /// Sets whether Company RegistrationNumber is Disabled
        /// </summary>
        bool DisableCompanyRegistrationNumber { set; }

        /// <summary>
        /// Sets whether Company Status is Disabled
        /// </summary>
        bool DisableCompanyStatus { set; }

        /// <summary>
        /// Sets whether Contact Home Phone is Disabled
        /// </summary>
        bool DisableContactHomePhone { set; }

        /// <summary>
        /// Sets whether Contact Work Phone is Disabled
        /// </summary>
        bool DisableContactWorkPhone { set; }

        /// <summary>
        /// Sets whether Contact Fax Number is Disabled
        /// </summary>
        bool DisableContactFaxNumber { set; }

        /// <summary>
        /// Sets whether Contact Cell Phone is Disabled
        /// </summary>
        bool DisableContactCellPhone { set; }

        /// <summary>
        /// Sets whether Contact Email Address is Disabled
        /// </summary>
        bool DisableContactEmailAddress { set; }

		/// <summary>
		/// Sets whether the NCR DCR Registration Number
		/// </summary>
		bool DisableNCRDCRegNumber { set; }

        /// <summary>
        /// Sets the text on the CompanyName label
        /// </summary>
        string CompanyNameLabelText { set; }

        /// <summary>
        /// Sets the text on the label showing the Parent OrgStructure Type description
        /// </summary>
        string ParentOrgstructureTypeLabelText { set; }

        /// <summary>
        /// Sets the text on the label showing the Parent OrgStructure description
        /// </summary>
        string ParentOrgstructureDescLabelText { set; }

        /// <summary>
        /// Get Debt Counselling Details
        /// </summary>
        /// <param name="legalEntity"></param>
        void PopulateDebtCounsellorDetail(ILegalEntity legalEntity);

        /// <summary>
        /// Get Debt Counselling Details
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="debtCounsellorDetail"></param>
        void PopulateDebtCounsellorDetail(ILegalEntity legalEntity, IDebtCounsellorDetail debtCounsellorDetail);

        #endregion
	}
}
