using BuildingBlocks.Services.Contracts;
using Common.Enums;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Contact : LifeContactControls
    {
        private readonly ILifeService lifeService;

        public Life_Contact()
        {
            lifeService = ServiceLocator.Instance.GetService<ILifeService>();
        }

        /// <summary>
        /// Click the Next button on the Life_Contact view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void ConfirmDetailsGoNext()
        {
            base.ctl00MainbtnNext.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Click the Add address button on this view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void ClickAddAddress()
        {
            base.ctl00MainbtnAddAddress.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Click the Update address button on this view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void ClickUpdateAddressButton()
        {
            base.ctl00MainbtnUpdateAddress.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Click the Update Contact Details button on this view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void ClickUpdateContactDetailsButton()
        {
            base.ctl00MainbtnUpdateDetails.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// This will select the first legal entity of type Natural Person from the applicant details grid.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        /// <param name="OfferKey">OfferKey for the life lead.</param>
        public void SelectFirstNaturalPersonFromGrid(int OfferKey)
        {
            foreach (var row in lifeService.GetLegalEntitiesFromLifeAccountRoles(OfferKey).RowList)
            {
                if (row.Column("LegalEntityTypeKey").GetValueAs<int>() == (int)LegalEntityTypeEnum.NaturalPerson)
                {
                    TableRow value = base.tAssuredLivesGrid.FindRow(row.Column("IDNumber").Value, 2);
                    value.Click();
                    break;
                }
            }
        }
    }
}