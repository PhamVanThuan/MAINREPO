using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Exclusions : LifeExclusionsControls
    {
        private readonly IWatiNService watinService;

        public Life_Exclusions()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Click the Next button on the Life_Exclusions view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void AcceptExclusionsGoNext()
        {
            watinService.GenericCheckAllCheckBoxes(base.Document.DomContainer);
            base.ctl00MainbtnNext.Click();
        }

        /// <summary>
        /// This will test the "All exclusions must be accepted before you can continue." rule fires when a checkbox is not checked.
        /// </summary>
        /// <param name="b">TestBrowser Instance</param>
        public void AssertExclusionsMustBeAccepted()
        {
            base.ctl00MainbtnNext.Click();
            Assert.True(base.divValidationSummaryBody.Exists, "Validation messages are not diplsayed.");
            Assert.True(base.divValidationSummaryBody.Text.Contains("All exclusions must be accepted before you can continue.")
                                                        , "Expected message: \"All exclusions must be accepted before you can continue.\"");
        }

        /// <summary>
        /// Select "Death Only" on the Exclusions screen
        /// </summary>
        /// <param name="b">TestBrowser Instance</param>
        public void SelectDeathOnly()
        {
            base.ctl00_Main_DeathOnly.Click();
            base.Document.DomContainer.WaitForComplete();
        }

        /// <summary>
        /// Select "Death WIth IPB" on the Exclusions screen
        /// </summary>
        /// <param name="b">TestBrowser Instance</param>
        public void SelectDeathWithIPB()
        {
            base.DeathAndIPB.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}