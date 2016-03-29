using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class PaymentDistributionAgentUpdate : PaymentDistributionAgentUpdateControls
    {
        private static readonly IWatiNService watinService;

        static PaymentDistributionAgentUpdate()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ClickSelect()
        {
            base.btnSelect.Click();
        }

        public void SelectAllAccounts(TestBrowser b)
        {
            watinService.GenericCheckAllCheckBoxes(b);
        }

        public void ClickUpdate()
        {
            base.btnUpdate.Click();
        }
    }
}