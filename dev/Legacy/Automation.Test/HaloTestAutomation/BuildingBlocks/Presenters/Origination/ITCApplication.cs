using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination
{
    public class ITCApplication : ITCApplicationControls
    {
        private readonly IWatiNService watinService;

        public ITCApplication()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Clicks on the Do Enquiry button
        /// </summary>
        /// <param name="b">IE TestBrowser Object</param>
        public void DoEnquiry()
        {
            //click the button
            watinService.HandleConfirmationPopup(base.doEnquiry);
        }
    }
}