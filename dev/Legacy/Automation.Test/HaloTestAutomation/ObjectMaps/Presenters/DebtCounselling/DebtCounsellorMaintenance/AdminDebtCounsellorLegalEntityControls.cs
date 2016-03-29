using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AdminDebtCounsellorLegalEntityControls : BasePageControls
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

        [FindBy(IdRegex = "PhoneNumber")]
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
    }
}