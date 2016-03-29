using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AdminPaymentDistributionAgentLegalEntityUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtWorkPhoneCode")]
        protected TextField txtWorkPhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtWorkPhoneNumber")]
        protected TextField txtWorkPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtFaxCode")]
        protected TextField txtFaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtFaxNumber")]
        protected TextField txtFaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_SelectAddress")]
        protected Button btnSelectAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button nbtnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button btnCancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlLegalEntityTypes")]
        protected SelectList ddlLegalEntityTypes { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlOrganisationType")]
        protected SelectList ddlOrganisationType { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddInitials")]
        protected TextField Initials { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddPreferredName")]
        protected TextField PreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtHomePhoneCode")]
        protected TextField txtHomePhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtHomePhoneNumber")]
        protected TextField txtHomePhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlOSDescriptionTypeAdd")]
        protected SelectList ctl00MainapLegalEntityDetailscontentddlOSDescriptionTypeAdd { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddSalutation")]
        protected SelectList ddlSalutation { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddGender")]
        protected SelectList ddlGender { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_ddlNatAddStatus")]
        protected SelectList ddlStatus { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtEmailAddress")]
        protected TextField txtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegisteredNameUpdate")]
        protected TextField RegisteredNameUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddIDNumber")]
        protected TextField txtIDNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddFirstNames")]
        protected TextField txtFirstNames { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtNatAddSurname")]
        protected TextField txtSurname { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_txtCellPhoneNumber")]
        protected TextField txtCellPhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_apLegalEntityDetails_content_CORegistrationNumberUpdate")]
        protected TextField txtRegistrationNumber { get; set; }
    }
}