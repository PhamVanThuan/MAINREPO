using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LifeLegalEntityAddExistingWorkFlowControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtNatAddInitials")]
        protected TextField ctl00MaintxtNatAddInitials { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddDateOfBirth")]
        protected TextField ctl00MaintxtNatAddDateOfBirth { get; set; }

        [FindBy(Id = "ctl00_Main_valNatAddDateOfBirthControl")]
        protected TextField ctl00MainvalNatAddDateOfBirthControl { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddTaxNumber")]
        protected TextField ctl00MaintxtNatAddTaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddPreferredName")]
        protected TextField ctl00MaintxtNatAddPreferredName { get; set; }

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
        protected TextField ctl00MaintxtCellPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField ctl00MaintxtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddFirstNames")]
        protected TextField ctl00MaintxtNatAddFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddSurname")]
        protected TextField ctl00MaintxtNatAddSurname { get; set; }

        [FindBy(Id = "ctl00_Main_lblNatIntroductionDate")]
        protected Span ctl00MainlblNatIntroductionDate { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button ctl00MainbtnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button ctl00MainbtnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_ddlLegalEntityTypes")]
        protected SelectList ctl00MainddlLegalEntityTypes { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddSalutation")]
        protected SelectList ctl00MainddlNatAddSalutation { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddCitizenshipType")]
        protected SelectList ctl00MainddlNatAddCitizenshipType { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddHomeLanguage")]
        protected SelectList ctl00MainddlNatAddHomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddGender")]
        protected SelectList ctl00MainddlNatAddGender { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddDocumentLanguage")]
        protected SelectList ctl00MainddlNatAddDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddMaritalStatus")]
        protected SelectList ctl00MainddlNatAddMaritalStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddStatus")]
        protected SelectList ctl00MainddlNatAddStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddpopulationGroup")]
        protected SelectList ctl00MainddlNatAddpopulationGroup { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddInsurableInterest")]
        protected SelectList ctl00MainddlNatAddInsurableInterest { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatAddEducation")]
        protected SelectList ctl00MainddlNatAddEducation { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionListBox_0")]
        protected CheckBox ctl00MainMarketingOptionListBox0 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionListBox_1")]
        protected CheckBox ctl00MainMarketingOptionListBox1 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionListBox_2")]
        protected CheckBox ctl00MainMarketingOptionListBox2 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionListBox_3")]
        protected CheckBox ctl00MainMarketingOptionListBox3 { get; set; }

        [FindBy(Id = "ctl00_Main_MarketingOptionListBox_4")]
        protected CheckBox ctl00MainMarketingOptionListBox4 { get; set; }

        [FindBy(Id = "tblLegalEntityType")]
        protected Table tblLegalEntityType { get; set; }

        [FindBy(Id = "txtNatAddPassportNumber")]
        protected TextField ctl00MaintxtNatAddPassportNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtNatAddIDNumber")]
        protected TextField ctl00MaintxtNatAddIDNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtInitialsUpdate")]
        protected TextField ctl00MaintxtInitialsUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdDateOfBirth")]
        protected TextField ctl00MaintxtUpdDateOfBirth { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdTaxNumber")]
        protected TextField ctl00MaintxtUpdTaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdPreferredName")]
        protected TextField ctl00MaintxtUpdPreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstNamesUpdate")]
        protected TextField ctl00MaintxtFirstNamesUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurnameUpdate")]
        protected TextField ctl00MaintxtSurnameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_lblType")]
        protected Span ctl00MainlblType { get; set; }

        [FindBy(Id = "ctl00_Main_lblIntroductionDate")]
        protected Span ctl00MainlblIntroductionDate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSalutationUpdate")]
        protected SelectList ctl00MainddlSalutationUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCitizenshipTypeUpdate")]
        protected SelectList ctl00MainddlCitizenshipTypeUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdHomeLanguage")]
        protected SelectList ctl00MainddlUpdHomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlGenderUpdate")]
        protected SelectList ctl00MainddlGenderUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdDocumentLanguage")]
        protected SelectList ctl00MainddlUpdDocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMaritalStatusUpdate")]
        protected SelectList ctl00MainddlMaritalStatusUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdStatus")]
        protected SelectList ctl00MainddlUpdStatus { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdPopulationGroup")]
        protected SelectList ctl00MainddlUpdPopulationGroup { get; set; }

        [FindBy(Id = "ctl00_Main_ddlNatUpdInsurableInterest")]
        protected SelectList ctl00MainddlNatUpdInsurableInterest { get; set; }

        [FindBy(Id = "ctl00_Main_ddlEducation")]
        protected SelectList ctl00MainddlEducation { get; set; }
    }
}