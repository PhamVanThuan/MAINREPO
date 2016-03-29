using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class MemoAssertions
    {
        private static readonly IMemoService memoService;

        static MemoAssertions()
        {
            memoService = ServiceLocator.Instance.GetService<IMemoService>();
        }

        /// <summary>
        /// Assert the latest memo record column captured against the generic key provided.
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="genericKeyType">GenericKeyTypeKey</param>
        /// <param name="column"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static int AssertLatestMemoInformation(int genericKey, GenericKeyTypeEnum genericKeyType, string column, string expectedValue)
        {
            var memo = memoService.GetLatestMemoColumn(genericKey, genericKeyType, column);
            string actualValue = (from m in memo select m.Value).FirstOrDefault();
            int memoKey = (from m in memo select m.Key).FirstOrDefault();
            Logger.LogAction(String.Format("Asserting the expected value for the {0} column is {1}", column, expectedValue));
            StringAssert.AreEqualIgnoringCase(expectedValue, actualValue,
                String.Format("The expected value of {0} for the Memo column {1} was not found. The actual value is: {2}",
                expectedValue, column, actualValue));
            return memoKey;
        }

        /// <summary>
        /// Checks if there is a memo for the Application Received action in the Mortgage Loan Account Memo
        /// </summary>
        /// <param name="AccountKey">Mortgage Loan Account</param>
        /// <param name="OfferKey">Further Lending Offer</param>
        public static void AssertApplicationReceivedMemo(int AccountKey, int OfferKey)
        {
            var results = memoService.GetFLAppReceivedMemo(AccountKey, OfferKey);
            Logger.LogAction("Asserting the Application Received Memo Record Exists");
            Assert.True(results.HasResults, "No Memo Record found for Application Received action.");
            results.Dispose();
        }
    }
}