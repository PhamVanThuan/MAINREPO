using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class FinancialAdjustmentAssertions
    {
        private static IAccountService accountService;
        private static IFinancialAdjustmentService fadjService;

        static FinancialAdjustmentAssertions()
        {
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            fadjService = ServiceLocator.Instance.GetService<IFinancialAdjustmentService>();
        }

        /// <summary>
        /// This assertion will check that a given financial adjustment with a given status exists against the account number provided.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="finAdjTypeSource">financial adjustment Type</param>
        /// <param name="finAdjStatus">financial adjustment Status</param>
        /// <param name="exists">Exists or Not</param>
        /// <param name="financialAdjustmentKey">Passing a financial adjustment key will make the assertion look for a specific financial adjustment</param>
        public static int AssertFinancialAdjustment(int accountKey, FinancialAdjustmentTypeSourceEnum finAdjTypeSource, FinancialAdjustmentStatusEnum finAdjStatus,
            bool exists, int financialAdjustmentKey = 0)
        {
            Logger.LogAction(string.Format(@"Asserting financial adjustment of Type {0} for Account {1} is set to {2}", finAdjTypeSource.ToString(), accountKey,
                finAdjStatus.ToString()));
            var results = fadjService.GetFinAdjustmentByAccountFinAdjustmentTypeAndStatus(accountKey, finAdjTypeSource, finAdjStatus);
            if (exists)
            {
                if (financialAdjustmentKey > 0 && results.HasResults)
                {
                    var fadjKey = (from r in results
                                   where r.Column("financialAdjustmentKey").GetValueAs<int>() == financialAdjustmentKey
                                   select r).FirstOrDefault();
                    Assert.That(fadjKey != null);
                }
                else
                {
                    Assert.True(results.HasResults, "No financial adjustment Found. AccountKey: {0}, FinancialAdjustmentTypeSourcekey: {1}, FinancialAdjustmentStatusKey: {2}",
                        accountKey,
                        finAdjTypeSource,
                        finAdjStatus);
                }
            }
            else
            {
                Assert.False(results.HasResults, string.Format(@"financial adjustment {0} Found", finAdjTypeSource));
            }
            return (from r in results select r.Column("FinancialAdjustmentKey").GetValueAs<int>()).FirstOrDefault();
        }

        /// <summary>
        /// Checks that a particular type of Financial Adjustment Type Source has been saved in the OfferInformationFinancialAdjustment table for an offer
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="finAdjustmentTypeSource">Financial Adjustment Type Source</param>
        public static bool OfferInfoFinancialAdjustmentExists(int offerKey, FinancialAdjustmentTypeSourceEnum finAdjustmentTypeSource)
        {
            var results = fadjService.GetOfferInformationFinancialAdjustmentByOfferAndType(offerKey, finAdjustmentTypeSource);
            Logger.LogAction(string.Format(@"Asserting OfferInformationFinancialAdjustment of Type {0} exists against Offer: {1}", finAdjustmentTypeSource,
                offerKey));
            return results.HasResults;
        }

        /// <summary>
        /// Asserts that the required changes have occurred when updating a financial adjustment using the update financial adjustment screen in loan servicing
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="mlDiscount"></param>
        /// <param name="mlInteresRate"></param>
        /// <param name="mlPayment"></param>
        /// <param name="date"></param>
        /// <param name="financialAdjustmentKey"></param>
        public static void AssertFinancialAdjustmentUpdate(int accountKey, string mlDiscount, string mlInteresRate, string mlPayment, int financialAdjustmentKey)
        {
            // Check LTs Written : Post loan transaction 920 (Rate Change) and 940 (Installment Change)
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.InterestRateChange);
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.InstallmentChange);
            // Check these values are not the same : Installment, Rate and Discount recalc
            var account = accountService.GetAccountByKey(accountKey);
            var mortgageLoan = (from ml in account.FinancialServices
                                where ml.FinancialServiceTypeKey == (int)FinancialServiceTypeEnum.VariableLoan
                                select ml).FirstOrDefault();
            Assert.That(mortgageLoan.RateAdjustment.ToString() != mlDiscount);
            Assert.That(mortgageLoan.InterestRate.ToString() != mlInteresRate);
            Assert.That(mortgageLoan.Payment.ToString() != mlPayment);
        }

        public static void AssertOfferAdjustments(int offerkey, double expectedAdjustmentRate, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource)
        {
            //assert the adjustment is applied
            bool exists = FinancialAdjustmentAssertions.OfferInfoFinancialAdjustmentExists(offerkey, financialAdjustmentTypeSource);
            Assert.IsTrue(exists, string.Format(@"Expected Adjustment {0} against application: {1}", financialAdjustmentTypeSource, offerkey));
            var results = fadjService.GetOfferInformationFinancialAdjustmentByOfferAndType(offerkey, financialAdjustmentTypeSource);
            //Should only be one... for AlphaHousing
            var savedRateAdjustment = results.Sum(x => x.Column("Discount").GetValueAs<double>()); //results.FirstOrDefault().Column("Discount").GetValueAs<double>();
            Assert.AreEqual(expectedAdjustmentRate, savedRateAdjustment);
        }

        public static void AssertNoOfferAdjustments(int offerkey, FinancialAdjustmentTypeSourceEnum financialAdjustmentTypeSource)
        {
            //assert the adjustment is applied
            bool exists = FinancialAdjustmentAssertions.OfferInfoFinancialAdjustmentExists(offerkey, financialAdjustmentTypeSource);
            Assert.That(!exists, string.Format(@"Financial adjustment of {0} found against application {1}, expected no adjustment", financialAdjustmentTypeSource, offerkey));
        }

        public static void AssertFinancialAdjustmentCreatedByCorrectRateAdjustmentElement(int offerKey, double expectedAdjustment, string expectedRateAdjustmentElement)
        {
            var results = fadjService.GetAppliedOfferInformationRateAdjustment(offerKey, expectedAdjustment, expectedRateAdjustmentElement);
            Assert.That(results.HasResults, string.Format(@"Adjustment not added by the correct element for application {0}: Expected: {1} discount, using element {2}", offerKey, expectedAdjustment, expectedRateAdjustmentElement));
        }
    }
}