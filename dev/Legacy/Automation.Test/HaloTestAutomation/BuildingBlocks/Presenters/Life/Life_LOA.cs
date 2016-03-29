using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_LOA : LifeLOAControls
    {
        /// <summary>
        /// Click the Next button on the Life_LOA view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        public void ConfirmLOAGoNext()
        {
            base.ctl00MainbtnNext.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}