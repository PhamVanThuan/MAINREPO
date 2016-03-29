using ObjectMaps.Presenters.LoanServicing;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class CancelCapView : CapCancelControls
    {
        /// <summary>
        ///
        /// </summary>
        public void CancelCap()
        {
            base.CancellationReason.Options[1].Select();
            base.SubmitButton.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void CancelCapWithoutReason()
        {
            base.CancellationReason.Options[0].Select();
            base.SubmitButton.Click();
        }
    }
}