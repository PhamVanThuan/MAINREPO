using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class LegalEntityDomiciliumAddressControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_gvAddresses_DXMainTable")]
        protected Table DomiciliumAddresses { get; set; }
    }
}