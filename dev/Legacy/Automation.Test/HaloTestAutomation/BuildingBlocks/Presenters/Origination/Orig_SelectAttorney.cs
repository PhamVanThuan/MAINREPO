using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;
using System.Threading;

namespace BuildingBlocks.Presenters.Origination
{
    public class Orig_SelectAttorney : Orig_SelectAttorneyControls
    {
        private readonly IAttorneyService attorneyService;

        public Orig_SelectAttorney()
        {
            attorneyService = ServiceLocator.Instance.GetService<IAttorneyService>();
        }

        /// <summary>
        /// This method will select the provided Deeds Office from the dropdown and then find an active
        /// attorney linked to that Deeds Office to select from the subsequent dropdown.
        /// </summary>
        /// <param name="deedsOffice">Deeds Office</param>
        /// <param name="b">TestBrowser Object</param>
        public void SelectAttorneyByDeedsOffice(string deedsOffice)
        {
            //select the deeds office
            base.ddlDeedsOffice.Option(deedsOffice).Select();
            //get the attorney to select
            string attorney = attorneyService.GetActiveAttorneyNameByDeedsOffice(deedsOffice);
            Thread.Sleep(2500);
            //continue
            base.ddlRegistrationAttorney.Option(attorney).Select();
            base.btnUpdateButton.Click();
        }
    }
}