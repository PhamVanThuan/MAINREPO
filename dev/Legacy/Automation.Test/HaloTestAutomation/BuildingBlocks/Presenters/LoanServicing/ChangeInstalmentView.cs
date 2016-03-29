using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class ChangeInstalmentView : ChangeInstalmentControls
    {
        private readonly IWatiNService watinService;

        public ChangeInstalmentView()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ChangeInstalment()
        {
            watinService.HandleConfirmationPopup(base.ChangeInstalment);
            base.Document.Page<BasePage>().DomainWarningClickYesHandlingPopUp();
        }
    }
}