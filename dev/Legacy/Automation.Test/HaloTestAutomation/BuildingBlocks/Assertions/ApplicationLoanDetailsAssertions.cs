using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class ApplicationLoanDetailsAssertions
    {
        private static readonly IApplicationService applicationService;

        static ApplicationLoanDetailsAssertions()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
        }

        /// <summary>
        /// This assertion will fetch the OfferInformationFinancialAdjustment record of a particular type linked to the latest OfferInformation
        /// record. It will then assert that the record exists and that the Discount field is correct if required.
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="financialAdjustmentTypeSource">RateOverrideTypeKey to fetch</param>
        /// <param name="assertValue">True = Assert Discount, False = do not assert discount</param>
        /// <param name="expectedValue">Expected Discount value.</param>
        public static void AssertOfferInformationFinancialAdjustmentExists(int offerKey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource, bool assertValue,
            double expectedValue)
        {
            QueryResults results = applicationService.GetOfferFinancialAdjustmentsByType(offerKey, financialAdjustmentTypeSource);
            //assert a record is found linked to the latest Offer Information
            Logger.LogAction("Asserting the OfferInformationRateOverride for type " + financialAdjustmentTypeSource + " exists against application " + offerKey);
            Assert.True(results.HasResults, "No OfferInformationRateOverrideRecord exists for {0}");
            //assert the OfferInformationRateOverride.Discount if required
            if (assertValue)
            {
                double discountValue = results.Rows(0).Column("Discount").GetValueAs<double>(); ;
                Logger.LogAction("Asserting the value in the OfferInformationRateOverride table is correct");
                Assert.AreEqual(expectedValue / 100, discountValue, "The Rate Override value does not match the expected value");
            }
            results.Dispose();
        }
    }
}