using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_Benefits : LifeBenefitsControls
    {
        public void CheckAcceptBenefits()
        {
            base.ctl00Mainchk00.Checked = true;
        }
        public void CheckNotAcceptBenefits()
        {
            base.ctl00Mainchk01.Checked = true;
        }
        public void ClickNext()
        {
            base.ctl00MainbtnNext.Click();
        }
        /// <summary>
        /// This will test the "All benefits must be explained before you can continue." rule fires when the checkbox is not checked.
        /// </summary>
        public void BenefitsMustBeExplainedRule()
        {
            base.ctl00MainbtnNext.Click();
        }
    }
}