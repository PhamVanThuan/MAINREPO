using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AttorneyDetailControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_addressDetails_txtUnitNumber")]
        protected TextField txtUnitNumber { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtBuildingNumber")]
        protected TextField txtBuildingNumber { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtBuildingName")]
        protected TextField txtBuildingName { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtStreetNumber")]
        protected TextField txtStreetNumber { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtStreetName")]
        protected TextField txtStreetName { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtSuburb")]
        protected TextField txtSuburb { get; set; }

        [FindBy(Id = "SAHLAutoComplete_DefaultItem")]
        protected Div SAHLAutoCompleteSuburb { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtCity")]
        protected TextField txtCity { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_txtPostalCode")]
        protected TextField txtPostalCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtAttorneyName")]
        protected TextField txtAttorneyName { get; set; }

        [FindBy(Id = "ctl00_Main_txtAttorneyContact")]
        protected TextField txtAttorneyContact { get; set; }

        [FindBy(Id = "ctl00_Main_txtAttorneyMandate_txtRands")]
        protected TextField txtAttorneyMandate_txtRands { get; set; }

        [FindBy(Id = "ctl00_Main_txtPhoneNumberCode")]
        protected TextField txtPhoneNumberCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtPhoneNumber")]
        protected TextField txtPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField txtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_dtEffectiveDate")]
        protected TextField EffectiveDate { get; set; }

        [FindBy(Id = "ctl00_Main_SelectAddress")]
        protected Button SelectAddress { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button MainCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button MainSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDeedsOffice")]
        protected SelectList ddlDeedsOffice { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_ddlCountry")]
        protected SelectList ddlCountry { get; set; }

        [FindBy(Id = "ctl00_Main_addressDetails_ddlProvince")]
        protected SelectList ddlProvince { get; set; }

        [FindBy(Id = "ctl00_Main_txtAttorneyMandate")]
        protected TextField txtAttorneyMandate { get; set; }

        [FindBy(Id = "ctl00_Main_tdAttorney")]
        protected Span tdAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_lblAttorneyNumber")]
        protected Span lblAttorneyNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblLitigationAttorney")]
        protected Span lblLitigationAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAttorney")]
        protected SelectList ddlAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRegistrationAttorney")]
        protected SelectList ddlRegistrationAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_ddlDeedsOfficeChange")]
        protected SelectList ddlDeedsOfficeChange { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAddressType")]
        protected SelectList ddlAddressType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlAddressFormat")]
        protected SelectList ddlAddressFormat { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLitigationAttorney")]
        protected SelectList ddlLitigationAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_btnContacts")]
        protected Button btnContacts { get; set; }
    }
}