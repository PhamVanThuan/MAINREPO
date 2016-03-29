using Automation.DataAccess;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class FurtherLendingAssertions
    {
        /// <summary>
        /// Asserts that a further lending offer has been created. This is called after the columns that store the
        /// Readvance/Further Advance/Further Loan offer keys have been updated via a stored proc. It then uses the
        /// Test Group in order to determine how many offers we expected to be created. i.e. Further Advances will look
        /// at the FAdvOfferKey and check that it is > 0.
        /// </summary>
        /// <param name="results">QueryResults row</param>
        public static void AssertFLOfferCreated(QueryResults results)
        {
            string TestGroup = results.Rows(0).Column("TestGroup").Value;
            int ReadvOfferKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
            int FAdvOfferKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            int FLOfferKey = results.Rows(0).Column("FLOfferKey").GetValueAs<int>();
            switch (TestGroup)
            {
                case "Readvances":
                    Logger.LogAction("Asserting Readvance Offer Exists");
                    Assert.True(ReadvOfferKey > 0, "No Readvance Offer Found");
                    break;

                case "FurtherAdvances":
                    Logger.LogAction("Asserting Further Advance Offer Exists");
                    Assert.True(FAdvOfferKey > 0, "No Further Advance Offer Found");
                    break;

                case "FurtherLoans":
                    Logger.LogAction("Asserting Further Loan Offer Exists");
                    Assert.True(FLOfferKey > 0, "No Further Loan Offer Found");
                    break;

                case "ReadvAndFAdv":
                    Logger.LogAction("Asserting Readvance & Further Advance Offers Exist");
                    Assert.True(ReadvOfferKey > 0 && FAdvOfferKey > 0, "Not all Offers Created");
                    break;

                case "Existing - Further Advance":
                    Logger.LogAction("Asserting Further Advance Offer Exist");
                    Assert.True(FAdvOfferKey > 0, "No Further Loan Offer Found");
                    break;

                case "Existing - Further Loan":
                    Logger.LogAction("Asserting Further Loan Offer Exist");
                    Assert.True(FLOfferKey > 0, "No Further Loan Offer Found");
                    break;
            }
        }
    }
}