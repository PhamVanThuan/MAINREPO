using ObjectMaps.Pages;
using WatiN.Core;

namespace ObjectMaps.Presenters.Admin
{
    public abstract class UpdateMyProfileDetailsControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlSalutationUpdate")]
        protected SelectList Salutation { get; set; }

        [FindBy(Id = "ctl00_Main_ddlEducation")]
        protected SelectList Education { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdHomeLanguage")]
        protected SelectList HomeLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_ddlUpdDocumentLanguage")]
        protected SelectList DocumentLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_txtInitialsUpdate")]
        protected TextField Initials { get; set; }

        [FindBy(Id = "ctl00_Main_txtFirstNamesUpdate")]
        protected TextField FirstName { get; set; }

        [FindBy(Id = "ctl00_Main_txtSurnameUpdate")]
        protected TextField Surname { get; set; }

        [FindBy(Id = "ctl00_Main_txtUpdPreferredName")]
        protected TextField PreferredName { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneCode")]
        protected TextField HomePhoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtHomePhoneNumber")]
        protected TextField HomePhoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneCode")]
        protected TextField WorkphoneCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtWorkPhoneNumber")]
        protected TextField WorkphoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField FaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxNumber")]
        protected TextField FaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtCellPhoneNumber")]
        protected TextField CellphoneNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmailAddress")]
        protected TextField EmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button UpdateButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button CancelButton { get; set; }
    }
}