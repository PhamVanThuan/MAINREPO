using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AccountMailingAddressControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_lblCorrespondenceMedium")]
        protected Span lblCorrespondenceMedium { get; set; }

        [FindBy(Id = "ctl00_Main_AddressLineDisp")]
        protected Span AddressLineDisp { get; set; }

        [FindBy(Id = "ctl00_Main_lblCorrespondenceLanguage")]
        protected Span lblCorrespondenceLanguage { get; set; }

        [FindBy(Id = "ctl00_Main_chkOnlineStatement")]
        protected CheckBox chkOnlineStatement { get; set; }

        [FindBy(Id = "ctl00_Main_OnlineStatementFormat")]
        protected Span OnlineStatementFormat { get; set; }

        [FindBy(Id = "ctl00_Main_lblCorrespondenceMailAddress")]
        protected Span lblCorrespondenceMailAddress { get; set; }
    }
}