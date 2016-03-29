using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Origination.FurtherLending
{
    public class ApplicationSummaryFurtherLending : ApplicationSummaryFurtherLendingControls
    {
        /// <summary>
        /// Returns the account number from the further lending application summary
        /// </summary>
        /// <param name="b">IE TestBrowser</param>
        /// <returns></returns>
        public string GetAccountNumber()
        {
            return base.lblAccountNumber.Text;
        }

        /// <summary>
        /// Returns the further lending application processor from the further lending application summary
        /// </summary>
        /// <param name="b">IE TestBrowser</param>
        /// <returns></returns>
        public string GetApplicationProcessor()
        {
            return base.lblApplicationProcessor.Text;
        }
    }
}