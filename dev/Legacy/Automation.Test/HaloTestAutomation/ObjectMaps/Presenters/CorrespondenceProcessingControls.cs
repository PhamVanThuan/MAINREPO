using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class CorrespondenceProcessingControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnPreview")]
        protected Button PreviewButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Button SendButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_Main_chkFax")]
        protected CheckBox chkFax { get; set; }

        [FindBy(Id = "ctl00_Main_chkEmail")]
        protected CheckBox chkEmail { get; set; }

        [FindBy(Id = "ctl00_Main_chkSMS")]
        protected CheckBox chkSMS { get; set; }

        [FindBy(Id = "ctl00_Main_chkPost")]
        protected CheckBox chkPost { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField FaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFax")]
        protected TextField FaxNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmail")]
        protected TextField EmailAddress { get; set; }

        [FindBy(Id = "ctl00_valSummary_Body")]
        protected Div divValidationSummaryBody { get; set; }

        [FindBy(Id = "ctl00_Main_gridRecipients")]
        protected Table Recipients { get; set; }

        //[FindBy(Name = "ctl00$valSummary$ctl05")]
        //protected Button divContinueButton { get; set; }
    }
}