using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ClientCommunicationPersonalLoansSMSControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Image Send { get; set; }

        [FindBy(Id = "ctl00_Main_ddlSMSType")]
        protected SelectList SMSType { get; set; }

        [FindBy(Id = "ctl00_Main_txtSMSText")]
        protected TextField SMSText { get; set; }

        [FindBy(Id = "ctl00_Main_gridRecipients")]
        protected Table RecipientsTable { get; set; }
    }
}