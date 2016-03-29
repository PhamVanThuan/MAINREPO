using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_RPAR : LifeRPARControls
    {
        /// <summary>
        /// Click the Next button on the Life_RPAR view.
        /// </summary>
        /// <param name="b">TestBrowser instance that is being used</param>
        /// <param name="ReplaceAnotherPolicy">Indicator if the policy is being used to replace an existing one</param>
        public void AcceptRPARGoNext(bool ReplaceAnotherPolicy)
        {
            if (ReplaceAnotherPolicy)
                //Yes
                base.ctl00MainrblYesNo0.Checked = true;
            else
                //No
                base.ctl00MainrblYesNo1.Checked = true;
            base.ctl00MainbtnNext.Click();
            base.Document.DomContainer.WaitForComplete();
        }
    }
}