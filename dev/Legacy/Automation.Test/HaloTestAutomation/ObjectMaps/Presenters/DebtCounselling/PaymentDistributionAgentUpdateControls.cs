using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class PaymentDistributionAgentUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnSelect")]
        protected Button btnSelect { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate { get; set; }
    }
}