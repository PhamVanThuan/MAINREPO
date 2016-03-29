using NUnit.Framework;
using ObjectMaps.Pages;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class DebtCounsellingSummaryReview : DebtCounsellingSummaryReviewControls
    {
        /// <summary>
        /// Returns the value of the Error Label
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetEworkLossControlLabel()
        {
            return base.lblEWorkMessage.Text;
        }

        /// <summary>
        /// Returns the value of the Ework Stage Label
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetEworkStageLabel()
        {
            return base.lblEworkStage.Text;
        }

        /// <summary>
        /// Returns the value of the Assigned User Label
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetEworkAssignedUserLabel()
        {
            return base.lblEworkUser.Text;
        }

        /// <summary>
        /// Opens up the Ework Case Details Accordion Control
        /// </summary>
        /// <param name="b"></param>
        public void ClickEworkCaseDetailsLink()
        {
            base.EworkCaseDetails.Click();
        }

        public bool eWorkLabelExists()
        {
            return base.lblEWorkMessage.Exists;
        }

        /// <summary>
        /// Checks whether or not the remaining term field is highlighted in a different colour by checking which of the fields currently exist on the screen.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="highlighted"></param>
        public void AssertDebtCounsellingSummaryRemainingTermFieldHighlighted(int accountKey, bool highlighted)
        {
            Logger.LogAction(@"Asserting that the backgroundcolour of the Remaining Term field is highlighted or not");
            //The id attribute of the Remaining Term field changes when it is highlighted.
            //Rather than checking the field attributes of a field that might or might not exist (as id is the only way to identify these fields)
            //I am checking which field exists to determine if the field formatting has changed
            Assert.AreEqual(highlighted, base.lblRemainingTermHighlight.Exists,
                string.Format(@"Failed when evaluating if highlighted Remaining Term field exists when highlighted set to {0}. AccountKey: {1}", highlighted, accountKey));
            Assert.AreNotEqual(highlighted, base.lblRemainingTerm.Exists,
                string.Format(@"Failed when evaluating if ordinary Remaining Term field exists when highlighted set to {0}. AccountKey: {1}", highlighted, accountKey));
        }
    }
}