using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.FurtherLending
{
    /// <summary>
    /// Contains BuildingBlock methods for the SuperLoOptOut view
    /// </summary>
    public class SuperLoOptOut : OptOutSuperLoControls
    {
        private readonly IWatiNService watinService;

        public SuperLoOptOut()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Clicks the opt out super lo button
        /// </summary>
        public void OptOutSuperLo()
        {
            base.btnOptOutSuperLo.Click();
            watinService.HandleConfirmationPopup(base.btnOptOutSuperLo);
        }
    }
}