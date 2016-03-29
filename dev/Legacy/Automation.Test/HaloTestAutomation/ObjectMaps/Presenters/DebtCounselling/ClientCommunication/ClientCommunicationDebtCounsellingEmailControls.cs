using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ClientCommunicationDebtCounsellingEmailControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_txtEmailSubject")]
        protected TextField EmailSubject { get; set; }

        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Image Send { get; set; }

        [FindBy(Id = "ctl00_Main_gridRecipients")]
        protected Table RecipientsTable { get; set; }
    }
}