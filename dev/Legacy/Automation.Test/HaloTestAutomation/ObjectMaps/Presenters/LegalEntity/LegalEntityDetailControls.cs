using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntityDetailControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtNatAddIDNumber")]
        protected TextField AddIDNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddInitials")]
        protected TextField AddInitials { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddDateOfBirth")]
        protected TextField AddDateOfBirth { get; set; }

        [FindBy(Id = "ctl00_Main_valNatAddDateOfBirthControl")]
        protected TextField AddDateOfBirthControl { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddFirstNames")]
        protected TextField AddFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddPassportNumber")]
        protected TextField AddPassportNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddSurname")]
        protected TextField AddSurname { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddTaxNumber")]
        protected TextField AddTaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddPreferredName")]
        protected TextField AddPreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneCode")]
        protected TextField txtHomePhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneNumber")]
        protected TextField txtHomePhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneCode")]
        protected TextField txtWorkPhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneNumber")]
        protected TextField txtWorkPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField txtFaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxNumber")]
        protected TextField txtFaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField txtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLegalEntityTypes")]
        protected SelectList ddlLegalEntityTypes { get; set; }

        [FindBy(Id = "ctl00_Main_ddlRoleTypeAdd")]
        protected SelectList ddlRoleTypeAdd { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddSalutation")]
        protected SelectList AddSalutation { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddCitizenshipType")]
        protected SelectList AddCitizenshipType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddHomeLanguage")]
        protected SelectList AddHomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddGender")]
        protected SelectList AddGender { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddDocumentLanguage")]
        protected SelectList AddDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddMaritalStatus")]
        protected SelectList AddMaritalStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddStatus")]
        protected SelectList AddStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddpopulationGroup")]
        protected SelectList AddPopulationGroup { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddEducation")]
        protected SelectList AddEducation { get; set; }

        [FindBy(Name = "ctl00$Main$chkAddIncomeContributor")]
        protected CheckBox AddIncomeContributor { get; set; }

        [FindBy(Name = "ctl00$Main$MarketingOptionExcludedListBox$0")]
        protected CheckBox A { get; set; }

        [FindBy(Name = "ctl00$Main$MarketingOptionExcludedListBox$1")]
        protected CheckBox B { get; set; }

        [FindBy(Name = "ctl00$Main$MarketingOptionExcludedListBox$2")]
        protected CheckBox C { get; set; }

        [FindBy(Name = "ctl00$Main$MarketingOptionExcludedListBox$3")]
        protected CheckBox D { get; set; }

        [FindBy(Name = "ctl00$Main$MarketingOptionExcludedListBox$4")]
        protected CheckBox E { get; set; }

        [FindBy(Id = "SAHLAutoCompleteDiv_iframe")]
        protected Element SAHLAutoCompleteDiv_iframe { get; set; }

        protected Div SAHLAutoComplete_DefaultItem(string IDNumber)
        {
            return base.Document.Div("SAHLAutoCompleteDiv").Div(Find.ByText(new Regex("^" + IDNumber + "[\x20-\x7E]*$")));
        }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button btnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button btnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOAddCompanyName")]
        protected TextField txtCOAddCompanyName { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOAddTradingName")]
        protected TextField txtCOAddTradingName { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOAddRegistrationNumber")]
        protected TextField txtCOAddRegistrationNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOAddTaxNumber")]
        protected TextField txtCOAddTaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCOAddDocumentlanguage")]
        protected SelectList ddlCOAddDocumentlanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCOAddStatus")]
        protected SelectList ddlCOAddStatus { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdPreferredName")]
        protected TextField ctl00MaintxtUpdPreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneCode")]
        protected TextField ctl00MaintxtHomePhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneNumber")]
        protected TextField ctl00MaintxtHomePhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneCode")]
        protected TextField ctl00MaintxtWorkPhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneNumber")]
        protected TextField ctl00MaintxtWorkPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField ctl00MaintxtFaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxNumber")]
        protected TextField ctl00MaintxtFaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtCellPhoneNumber")]
        protected TextField CellPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField ctl00MaintxtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button ctl00MainbtnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button ctl00MainbtnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSalutationUpdate")]
        protected SelectList ctl00MainddlSalutationUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdHomeLanguage")]
        protected SelectList ctl00MainddlUpdHomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdDocumentLanguage")]
        protected SelectList ctl00MainddlUpdDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdStatus")]
        protected SelectList ctl00MainddlUpdStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdPopulationGroup")]
        protected SelectList ctl00MainddlUpdPopulationGroup { get; set; }

        [FindBy(Id = "ctl00_Main_ddlEducation")]
        protected SelectList ctl00MainddlEducation { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionExcludedListBox_0")]
        protected CheckBox MainMarketingOption0 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionExcludedListBox_1")]
        protected CheckBox MainMarketingOption1 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionExcludedListBox_2")]
        protected CheckBox MainMarketingOption2 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionExcludedListBox_3")]
        protected CheckBox MainMarketingOption3 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionExcludedListBox_4")]
        protected CheckBox MainMarketingOption4 { get; set; }

        [FindBy(Id = "ctl00_Main_txtInitialsUpdate")]
        protected TextField ctl00MaintxtInitialsUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdDateOfBirth")]
        protected TextField ctl00MaintxtUpdDateOfBirth { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstNamesUpdate")]
        protected TextField ctl00MaintxtFirstNamesUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurnameUpdate")]
        protected TextField ctl00MaintxtSurnameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCitizenshipTypeUpdate")]
        protected SelectList ctl00MainddlCitizenshipTypeUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlGenderUpdate")]
        protected SelectList ctl00MainddlGenderUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMaritalStatusUpdate")]
        protected SelectList ctl00MainddlMaritalStatusUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdPassportNumber")]
        protected TextField ctl00MaintxtUpdPassportNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtIDNumberUpdate")]
        protected TextField ctl00MaintxtIDNumberUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_chkNatUpdIncomeContributor")]
        protected CheckBox ctl00MainchkNatUpdIncomeContributor { get; set; }

        [FindBy(Id = "ctl00_Main_acIDNumberUpdate_acIDNumberUpdate_Hidden")]
        protected Element IDNumberUpdateHidden { get; set; }

        [FindBy(Id = "ctl00_Main_lblMaritalStatus")]
        protected Label MaritalStatusHidden { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdTaxNumber")]
        protected TextField TaxNumberUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOUpdTaxNumber")]
        protected TextField TaxNumberCompanyUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_COLegalEntityTypeUpdate")]
        protected SelectList LegalEntityTypeUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_COIntroductionDateUpdate")]
        protected TextField IntroductionDateUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_CORegisteredNameUpdate")]
        protected TextField RegisteredNameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_CORegistrationNumberUpdate")]
        protected TextField RegistrationNumberUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCOUpdStatus")]
        protected SelectList LegalEntityStatusUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtCOUpdTradingName")]
        protected TextField TradingNameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCOUpdDocumentLanguage")]
        protected SelectList CompanyDocumentLanguageUpdate { get; set; }

        [FindBy(IdRegex = "InsurableInterest")]
        protected SelectList InsurableInterest { get; set; }
    }
}