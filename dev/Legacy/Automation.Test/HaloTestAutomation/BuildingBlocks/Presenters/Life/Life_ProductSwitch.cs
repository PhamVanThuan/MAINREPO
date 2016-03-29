using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_ProductSwitch : LifeProductSwitchControls
    {
        /// <summary>
        /// Click the Next button on the Life_PolicyWorkFlow view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        /// <param name="PolicyType">Policy Type to switch to</param>
        public void SwitchTo(string PolicyType)
        {
            base.ctl00MainddlPolicyType.Option(PolicyType).Select();
            base.ctl00MainbtnSubmit.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}