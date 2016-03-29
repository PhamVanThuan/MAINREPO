using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AdminPaymentDistributionAgentLegalEntityRemoveControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_SelectAddress")]
        protected Button ctl00MainSelectAddress { get; set; }

        [FindBy(Id = "ctl00_Main_btnSubmitButton")]
        protected Button ctl00MainbtnSubmitButton { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancelButton")]
        protected Button ctl00MainbtnCancelButton { get; set; }
    }
}