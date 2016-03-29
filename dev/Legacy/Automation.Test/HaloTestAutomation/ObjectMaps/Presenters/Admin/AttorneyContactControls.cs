using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AttorneyContactControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtFaxNumber__CODE")]
        protected TextField ctl00MaintxtFaxNumberCODE { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxNumber__NUMB")]
        protected TextField ctl00MaintxtFaxNumberNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstName")]
        protected TextField ctl00MaintxtFirstName { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurname")]
        protected TextField ctl00MaintxtSurname { get; set; }

        [FindBy(Id = "ctl00_Main_txtTelephoneNumber__CODE")]
        protected TextField ctl00MaintxtTelephoneNumberCODE { get; set; }

        [FindBy(Id = "ctl00_Main_txtTelephoneNumber__NUMB")]
        protected TextField ctl00MaintxtTelephoneNumberNUMB { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField ctl00MaintxtEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_lblAttorneyName")]
        protected Span ctl00MainlblAttorneyName { get; set; }

        [FindBy(Id = "ctl00_Main_lblAttorneyNameValue")]
        protected Span ctl00MainlblAttorneyNameValue { get; set; }

        [FindBy(Id = "ctl00_Main_lblFirstName")]
        protected Span ctl00MainlblFirstName { get; set; }

        [FindBy(Id = "ctl00_Main_lblSurname")]
        protected Span ctl00MainlblSurname { get; set; }

        [FindBy(Id = "ctl00_Main_lblTelephone")]
        protected Span ctl00MainlblTelephone { get; set; }

        [FindBy(Id = "ctl00_Main_lblFaxNumber")]
        protected Span ctl00MainlblFaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_lblEmailAddress")]
        protected Span ctl00MainlblEmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_lblRoleType")]
        protected Span ctl00MainlblRoleType { get; set; }

        [FindBy(Id = "ctl00_Main_btnAdd")]
        protected Button ctl00MainbtnAdd { get; set; }

        [FindBy(Id = "ctl00_Main_btnDone")]
        protected Button ctl00MainbtnDone { get; set; }

        [FindBy(Id = "ctl00_Main_litigationAttorneyContactsGrid")]
        protected Table LitigationAttorneyContactsGrid { get; set; }

        [FindBy(Id = "ctl00_Main_cmbRoleType")]
        protected SelectList RoleType { get; set; }
    }
}