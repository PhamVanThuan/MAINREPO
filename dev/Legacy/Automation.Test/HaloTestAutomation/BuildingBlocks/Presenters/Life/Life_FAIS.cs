using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_FAIS : LifeFAISControls
    {
        private readonly IWatiNService watinService;

        public Life_FAIS()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void Populate(bool checkSecondLifeInsured, bool checkNOSecondLifeInsured, bool checkAllCheckBoxes, string contactNumber)
        {
            if (checkSecondLifeInsured)
                base.ctl00MainrbnsConfirmation1.Checked = true;

            if (checkNOSecondLifeInsured)
                base.ctl00MainrbnsConfirmation0.Checked = true;

            if (checkAllCheckBoxes)
                watinService.GenericCheckAllCheckBoxes(base.Document.DomContainer);

            if (base.ctl00MaintxtPhone.Exists && !String.IsNullOrEmpty(contactNumber))
                base.ctl00MaintxtPhone.Value = contactNumber;
        }

        /// <summary>
        /// Click the Next button on the Life_FAIS view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void AcceptFAISGoNext()
        {
            base.ctl00MainbtnNext.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}