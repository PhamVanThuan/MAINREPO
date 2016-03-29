using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AccountMailingAddressUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_ddlCorrespondenceMedium")]
        protected SelectList ddlCorrespondenceMedium { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCorrespondenceMailAddress")]
        protected SelectList ddlCorrespondenceMailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMailingAddress")]
        protected SelectList ddlMailingAddress { get; set; }

        [FindBy(Id = "ctl00_Main_ddlCorrespondenceLanguage")]
        protected SelectList ddlCorrespondenceLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_chkOnlineStatement")]
        protected CheckBox chkOnlineStatement { get; set; }

        [FindBy(Id = "ctl00_Main_ddlOnlineStatementFormat")]
        protected SelectList ddlOnlineStatementFormat { get; set; }

        [FindBy(Id = "ctl00_Main_CancelButton")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_SubmitButton")]
        protected Button btnSubmit { get; set; }
    }
}