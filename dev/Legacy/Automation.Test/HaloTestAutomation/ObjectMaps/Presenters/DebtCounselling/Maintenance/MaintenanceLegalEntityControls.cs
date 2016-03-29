using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps
{
    public abstract class MaintenanceLegalEntityControls : BasePageControls
    {
        #region ContactDetail

        [FindBy(IdRegex = "EmailAddress")]
        protected TextField EmailAddress { get; set; }

        [FindBy(IdRegex = "WorkPhoneCode")]
        protected TextField WorkPhoneCode { get; set; }

        [FindBy(IdRegex = "WorkPhoneNumber")]
        protected TextField WorkPhoneNumber { get; set; }

        [FindBy(IdRegex = "FaxCode")]
        protected TextField FaxCode { get; set; }

        [FindBy(IdRegex = "FaxNumber")]
        protected TextField FaxNumber { get; set; }

        [FindBy(IdRegex = "HomePhoneCode")]
        protected TextField HomePhoneCode { get; set; }

        [FindBy(IdRegex = "HomePhoneNumber")]
        protected TextField HomePhoneNumber { get; set; }

        #endregion ContactDetail

        #region Address

        [FindBy(IdRegex = "UnitNumber")]
        protected TextField UnitNumber { get; set; }

        [FindBy(IdRegex = "BuildingNumber")]
        protected TextField BuildingNumber { get; set; }

        [FindBy(IdRegex = "BuildingName")]
        protected TextField BuildingName { get; set; }

        [FindBy(IdRegex = "StreetNumber")]
        protected TextField StreetNumber { get; set; }

        [FindBy(IdRegex = "StreetName")]
        protected TextField StreetName { get; set; }

        [FindBy(IdRegex = "AddressFormat")]
        protected SelectList AddressFormat { get; set; }

        [FindBy(IdRegex = "AddressType")]
        protected SelectList AddressType { get; set; }

        [FindBy(IdRegex = "Suburb")]
        protected TextField Suburb { get; set; }

        [FindBy(IdRegex = "City")]
        protected TextField City { get; set; }

        [FindBy(IdRegex = "Country")]
        protected SelectList Country { get; set; }

        [FindBy(IdRegex = "Province")]
        protected SelectList Province { get; set; }

        [FindBy(IdRegex = "PostOffice")]
        protected TextField PostOffice { get; set; }

        [FindBy(IdRegex = "PostalCode")]
        protected TextField PostalCode { get; set; }

        [FindBy(IdRegex = "CellPhoneNumber")]
        protected TextField CellPhoneNumber { get; set; }

        #endregion Address

        #region LegalEntity

        [FindBy(IdRegex = "LegalEntityTypes")]
        protected SelectList LegalEntityType { get; set; }

        [FindBy(IdRegex = "Initials")]
        protected TextField Initials { get; set; }

        [FindBy(IdRegex = "PreferredName")]
        protected TextField PreferredName { get; set; }

        [FindBy(IdRegex = "Salutation")]
        public SelectList Salutation { get; set; }

        [FindBy(IdRegex = "Gender")]
        public SelectList Gender { get; set; }

        [FindBy(IdRegex = "Status")]
        protected SelectList Status { get; set; }

        [FindBy(IdRegex = "RegisteredName")]
        protected TextField RegisteredName { get; set; }

        [FindBy(IdRegex = "IDNumber")]
        protected TextField IDNumber { get; set; }

        [FindBy(IdRegex = "FirstNames")]
        protected TextField FirstNames { get; set; }

        [FindBy(IdRegex = "Surname")]
        protected TextField Surname { get; set; }

        #endregion LegalEntity

        #region OrganisationStructure

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegisteredNameUpdate")]
        public TextField RegisteredNameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegistrationNumberUpdate")]
        public TextField RegistrationNumberUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlOSDescriptionTypeAdd")]
        protected SelectList Role { get; set; }

        [FindBy(IdRegex = "OrganisationType")]
        protected SelectList OrganisationType { get; set; }

        #endregion OrganisationStructure

        [FindBy(Id = "SAHLValidationSummaryBody")]
        protected Div SAHLValidationSummaryBody { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button SubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button CancelButton { get; set; }

        [FindBy(Text = "Legal Entity Address Details")]
        protected Link linkLegalEntityAddressDetails { get; set; }

        [FindBy(Text = "Legal Entity Details")]
        protected Link linkLegalEntityDetails { get; set; }

        [FindBy(Id = "SAHLAutoComplete_DefaultItem")]
        protected Div SAHLAutoComplete_DefaultItem { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityAddDetails_content_dtEffectiveDate")]
        protected TextField EffectiveDate { get; set; }

        public DivCollection SAHLAutoComplete_DefaultItem_Collection()
        {
            return SAHLAutoComplete.Divs;
        }

        [FindBy(Id = "SAHLAutoCompleteDiv")]
        public Div SAHLAutoComplete;

        #region SummaryView Controls

        [FindBy(Id = "ctl00_Main_SelectAddress")]
        protected Button ctl00MainSelectAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button ctl00MainbtnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblLEType")]
        protected Span lblLEType { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblOrganisationTypeDisplay")]
        protected Span lblOrganisationTypeDisplay { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCORegisteredName")]
        protected Span lblCORegisteredName { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCORegistrationNumber")]
        protected Span lblCORegistrationNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblRole")]
        protected Span lblRole { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblNatIntroductionDate")]
        protected Span lblNatIntroductionDate { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblIDNumber")]
        protected Span lblIDNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblSalutation")]
        protected Span lblSalutation { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblInitials")]
        protected Span lblInitials { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblFirstNames")]
        protected Span lblFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblSurname")]
        protected Span lblSurname { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblPreferredName")]
        protected Span lblPreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblGender")]
        protected Span lblGender { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblStatus")]
        protected Span lblStatus { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblHomePhone")]
        protected Span lblHomePhone { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblWorkPhone")]
        protected Span lblWorkPhone { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblFaxNumber")]
        protected Span lblFaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblCellphoneNumber")]
        protected Span lblCellphoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_lblEmailAddress")]
        protected Span lblEmailAddress { get; set; }

        #endregion SummaryView Controls
    }
}