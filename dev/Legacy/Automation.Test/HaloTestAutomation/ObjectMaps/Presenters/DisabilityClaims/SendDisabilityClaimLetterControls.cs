using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class SendDisabilityClaimLetterControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_chkFax")]
        protected CheckBox FaxCheckBox { get; set; }

        [FindBy(Id = "ctl00_Main_txtFaxCode")]
        protected TextField FaxCode { get; set; }

        [FindBy(Id = "ctl00_Main_txtFax")]
        protected TextField FaxNumber { get; set; }
               

        [FindBy(Id = "ctl00_Main_chkEmail")]
        protected CheckBox EmailCheckBox { get; set; }

        [FindBy(Id = "ctl00_Main_txtEmail")]
        protected TextField EmailAddress { get; set; }

        [FindBy(Id = "ctl00_Main_chkPost")]
        protected CheckBox PostCheckBox { get; set; }

        [FindBy(Id = "ctl00_Main_btnPreview")]
        protected Button Preview { get; set; }

        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Button SendCorrespondence { get; set; }

        [FindBy(Id = "ctl00_Main_btnPreview")]
        protected Button Cancel { get; set; }


    }
}