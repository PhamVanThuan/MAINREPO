using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class CAP2Assertions
    {
        private static ICAP2Service cap2Service;
        private static IDisbursementService disbursementService;

        static CAP2Assertions()
        {
            cap2Service = ServiceLocator.Instance.GetService<ICAP2Service>();
            disbursementService = ServiceLocator.Instance.GetService<IDisbursementService>();
        }

        /// <summary>
        /// This assertion will check that a CAPOffer has been created for an Account
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account the offer was created on</param>
        public static void AssertCAP2OfferCreated(int accountKey)
        {
            var results = cap2Service.GetLatestCapOfferByAccountKey(accountKey);
            Logger.LogAction(String.Format(@"Asserting that CAP offer for account {0} is created", accountKey));
            Assert.True(results.HasResults, "No CAP Offer record found for account " + accountKey);
            results.Dispose();
        }

        /// <summary>
        /// This assertion will assert the current state of the instance in the X2 workflow against an expected state
        /// supplied by a test.
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account the offer was created on</param>
        /// <param name="expectedState">Expected X2 state in the CAP 2 Offers workflow</param>
        public static int AssertX2State(int accountKey, string expectedState)
        {
            string capOfferKey = (from r in cap2Service.GetLatestCapOfferByAccountKey(accountKey)
                                  select r.Column("CapOfferKey").Value).FirstOrDefault();
            var x2Data = (from r in cap2Service.GetLatestCap2X2DataByAccountKeyAndCapOfferKey(accountKey, capOfferKey)
                          select r).FirstOrDefault();
            string currentState = x2Data.Column("StateName").GetValueAs<string>();
            int instanceID = x2Data.Column("InstanceID").GetValueAs<int>();
            Logger.LogAction(String.Format(@"Asserting that the CAP offer is in {0} workflow state.", expectedState));
            StringAssert.AreEqualIgnoringCase(expectedState, currentState, "Cap Offer is not in the expected state: " + expectedState);
            return instanceID;
        }

        /// <summary>
        /// This assertion will assert the current CAP status from the CapOffer table against an expected status
        /// supplied by a test.
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account the offer was created on</param>
        /// <param name="expectedStatus">The expected CAP Status (CAPStatus.Description)</param>
        public static void AssertStatus(int accountKey, string expectedStatus)
        {
            var results = cap2Service.GetLatestCapOfferByAccountKey(accountKey);
            string currentStatus = results.Rows(0).Column("CapStatus").Value;
            results.Dispose();
            Logger.LogAction(String.Format(@"Asserting that the CAP status is set to {0}", expectedStatus));
            StringAssert.AreEqualIgnoringCase(expectedStatus, currentStatus, "The Cap Offer Status is not the expected: " + expectedStatus);
        }

        /// <summary>
        /// This assertion ensures that there is an X2 instance linked to a CapOffer
        /// </summary>
        /// <param name="accountKey">Mortgage Loan Account the offer was created on</param>
        public static void AssertX2InstanceCreated(int accountKey)
        {
            var results = cap2Service.GetLatestCapOfferByAccountKey(accountKey);
            string capOfferKey = results.Rows(0).Column("CapOfferKey").Value;
            results.Dispose();
            results = cap2Service.GetLatestCap2X2DataByAccountKeyAndCapOfferKey(accountKey, capOfferKey);
            Logger.LogAction(String.Format(@"Asserting that the X2 instance is created for {0}", accountKey));
            Assert.True(results.HasResults, "No X2 Data Found for a CapOffer");
            results.Dispose();
        }

        /// <summary>
        /// Asserts that the expected CapOfferDetail records exist for a CapOffer. It will loop through the SQL
        /// result set and assert that a record for a particular cap type exists. The test should provide an array
        /// of Cap Types that should exist.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="capOptions">Cap Types expected for the CapOffer</param>
        public static void AssertCAPDetailRecordsExist(int accountKey, int[] capOptions)
        {
            var results = cap2Service.GetCapOfferDetailByAccountKey(accountKey);
            foreach (var s in capOptions) //loop through the array of cap types provided
            {
                bool exists = false;

                foreach (QueryResultsRow row in results.RowList) //then check to see if it is in the result set
                {
                    if (s == row.Column("CapTypeDescription").GetValueAs<int>())
                    {
                        exists = true;
                    }
                }
                Logger.LogAction(String.Format(@"Asserting that the CAP detail records exist."));
                Assert.True(exists, "Cap Type does not exist for the CapOffer");
            }

            results.Dispose();
        }

        /// <summary>
        /// Ensures that the correct CapOfferDetail record has been marked as Taken Up after a CAP offer has been accepted
        /// </summary>
        /// <param name="capOptionTakenUp">The CAP option taken up by the client i.e. "1","2" or "3"</param>
        /// <param name="accountKey">AccountKey</param>
        public static void AssertCAP2OptionTakenUp(int capOptionTakenUp, int accountKey)
        {
            int i = 0;
            var results = cap2Service.GetCapOfferDetailByAccountKey(accountKey);
            //this should only ever return a single row
            foreach (QueryResultsRow row in results.RowList) //then check to see if it is in the result set
            {
                if (row.Column("CapStatusKey").GetValueAs<int>() == (int)CapStatusEnum.TakenUp)
                {
                    Assert.AreEqual(capOptionTakenUp, row.Column("CapTypeDescription").GetValueAs<int>());
                    i++;
                    //there should only be one cap option marked as taken up, we check the number of times we enter the loop and fail if more than once.
                    if (i > 1)
                        Assert.Fail("Too many cap options have been marked as taken up.");
                }
            }
            results.Dispose();
        }

        /// <summary>
        /// Retrieves the current CAP 2 payment option for a CAP 2 offer and compares it to an expected value
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="expPaymentOption">Expected CAP Payment Option</param>
        public static void AssertCAP2PaymentOption(int accountKey, string expPaymentOption)
        {
            string actPaymentOption = (from r in cap2Service.GetCapPaymentOptionByAccountKey(accountKey)
                                       select r.Column("CapTypeDescription").Value).FirstOrDefault();
            //assert they are equal
            Logger.LogAction("Asserting that the payment option is " + expPaymentOption);
            StringAssert.AreEqualIgnoringCase(expPaymentOption, actPaymentOption, "The expected payment option was not found.");
        }

        /// <summary>
        /// Retrieves the previous state of a CAP offer that is populated when creating a Callback. This is then compared to an expected
        /// previous state i.e. the state from which the Callback was created.
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="expPrevState">The expected previous state</param>
        public static void AssertCAP2PreviousState(int accountKey, string expPrevState)
        {
            string lastState = (from r in cap2Service.GetCap2X2Data(accountKey) select r.Column("Last_State").Value).FirstOrDefault();
            //assert they are equal
            Logger.LogAction(String.Format(@"Asserting that the previous state has been set to {0}", expPrevState));
            StringAssert.AreEqualIgnoringCase(expPrevState, lastState);
        }

        /// <summary>
        /// This assertion checks that a CAP 2 Readvance disbursement exists for our Account
        /// </summary>
        /// <param name="accountKey"></param>
        public static void AssertCAP2ReadvanceDisbursementPosted(int accountKey)
        {
            var results = disbursementService.GetDisbursementFinancialTransactionByAccountKeyAndDisbursementType(accountKey, DisbursementTransactionTypes.CAP2ReAdvance, "1");
            Logger.LogAction(String.Format(@"Asserting that the CAP 2 ReAdvance has been posted for {0}", accountKey));
            Assert.True(results.HasResults, "No CAP ReAdvance found for this account");
            results.Dispose();
        }

        /// <summary>
        /// Used in order to check that the CAP fee is correctly posted as the CAP Advance value in the disbursement process
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public static void AssertCAP2AdvanceAmount(int accountKey)
        {
            //first we need to fetch the CAP detail record that is set to Readvance Required (6)
            double takenUpCAPPremium = (from r in cap2Service.GetCapOfferDetailByAccountKey(accountKey)
                                        where r.Column("CapStatusKey").GetValueAs<int>() == (int)CapStatusEnum.ReadvanceRequired
                                        select r.Column("Fee").GetValueAs<double>()).FirstOrDefault();
            takenUpCAPPremium = Math.Round(takenUpCAPPremium, 2);
            //we now need to get the disbursed amount
            var results = disbursementService.GetDisbursementFinancialTransactionByAccountKeyAndDisbursementType(accountKey, DisbursementTransactionTypes.CAP2ReAdvance, "1");
            double disbursedAmount = results.Rows(0).Column("TransactionTypeNumber").GetValueAs<int>() == 141 ?
                disbursedAmount = results.Rows(0).Column("Amount").GetValueAs<double>() : 0.00;
            disbursedAmount = Math.Round(disbursedAmount, 2);
            //now check if they are equal
            Logger.LogAction("Asserting that the CAP 2 ReAdvance been posted");
            Assert.That(takenUpCAPPremium == disbursedAmount, string.Format(@"The amount disbursed for {0} does not match the taken up CAP option: Expected: {1} was {2}",
                accountKey, takenUpCAPPremium, disbursedAmount));
        }

        /// <summary>
        /// Used in order to check that an active CAP 2 rate override exists for an Account
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public static void AssertCAP2FinancialAdjustmentExists(int accountKey, FinancialAdjustmentStatusEnum fadjStatus)
        {
            var results = cap2Service.GetCap2FinancialAdjustmentDetailByAccountKey(accountKey, fadjStatus);
            Logger.LogAction("Asserting that the CAP 2 rate override has been created.");
            Assert.True(results.HasResults, "No CAP 2 Rate Override was found");
        }

        /// <summary>
        /// Calls a set of assertions in order to check that the CAP rate override is correctly created. Passes sets of
        /// QueryResults to those assertions
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        public static void AssertCap2DataSetUp(int accountKey, FinancialAdjustmentStatusEnum fadjStatus)
        {
            var capOffer = cap2Service.GetLatestCapOfferByAccountKey(accountKey);
            var financialAdjustment = cap2Service.GetCap2FinancialAdjustmentDetailByAccountKey(accountKey, fadjStatus);
            var capOfferDetail = cap2Service.GetCapOfferDetailByAccountAndStatus(accountKey, (int)CapStatusEnum.TakenUp);
            AssertCapPaymentOption(capOffer, financialAdjustment);
            AssertCapBalance(capOffer, financialAdjustment);
            AssertCAPFinancialAdjustmentTerm(financialAdjustment);
            AssertCapOfferCapitalisationDate(capOffer);
            AssertFinancialAdjustmentFromDate(financialAdjustment, accountKey);
            capOffer.Dispose();
            capOfferDetail.Dispose();
            financialAdjustment.Dispose();
        }

        /// <summary>
        /// Used in order to check that CAP Payment Option from the CAP Offer is reflected on the Rate Override
        /// </summary>
        /// <param name="capOffer">CAP Offer data set</param>
        /// <param name="financialAdjustment">Rate Override data set</param>
        public static void AssertCapPaymentOption(QueryResults capOffer, QueryResults financialAdjustment)
        {
            int capOfferPaymentOption = capOffer.Rows(0).Column("CAPPaymentOptionKey").GetValueAs<int>();
            int PaymentOption = financialAdjustment.Rows(0).Column("CapPaymentOptionKey").GetValueAs<int>();
            Logger.LogAction("Asserting that the CAP Payment options is correct on the Rate Override.");
            Assert.That(capOfferPaymentOption == PaymentOption, "CAP Payment Options are not equal");
        }

        /// <summary>
        /// Used in order to check that the CAP Offer CAP Balance reflects correctly on the Rate Override.
        /// </summary>
        /// <param name="capOffer">CAP Offer data set</param>
        /// <param name="financialAdjustment">Rate Override data set</param>
        public static void AssertCapBalance(QueryResults capOffer, QueryResults financialAdjustment)
        {
            float capOfferBalance = capOffer.Rows(0).Column("CurrentBalance").GetValueAs<float>();
            float balance = financialAdjustment.Rows(0).Column("CapBalance").GetValueAs<float>();
            Logger.LogAction("Asserting that the CAP Balance reflects correctly on the Rate Override.");
            Assert.That(capOfferBalance - balance <= 0.01);
        }

        /// <summary>
        /// Used in order to check that the CTC.CapEffectiveDate equals the FinancialAdjustment.FromDate
        /// </summary>
        /// <param name="financialAdjustment">Financial Adjustment record</param>
        /// <param name="accountKey">AccountKey</param>
        public static void AssertFinancialAdjustmentFromDate(QueryResults financialAdjustment, int accountKey)
        {
            string expiryDate = (from r in cap2Service.GetCapTypeConfigurationEndDateForCapOffer(accountKey)
                                 select r.Column("capeffectivedate").GetValueAs<string>()).FirstOrDefault();
            //now we need the rate override from date
            string fromDate = financialAdjustment.Rows(0).Column("FromDate").Value;
            Logger.LogAction("Asserting that the CTC.CapEffectiveDate equals the FinancialAdjustment.FromDate");
            StringAssert.AreEqualIgnoringCase(expiryDate, fromDate);
        }

        /// <summary>
        /// Used in order to check that the Rate Override term is set to 24 months
        /// </summary>
        /// <param name="financialAdjustment">Financial Adjustment record</param>
        public static void AssertCAPFinancialAdjustmentTerm(QueryResults financialAdjustment)
        {
            int rateOverrideTerm = financialAdjustment.Rows(0).Column("Term").GetValueAs<int>();
            Logger.LogAction("Asserting that the Rate Override term is set to 24 months");
            Assert.That(rateOverrideTerm == 24, "Rate Override term is not set to 24 months");
        }

        /// <summary>
        /// Used in order to check that the Capitalisation Date is set on the Cap Offer after the rate override is created
        /// </summary>
        /// <param name="capOffer">CapOffer data set</param>
        public static void AssertCapOfferCapitalisationDate(QueryResults capOffer)
        {
            DateTime dateActual = capOffer.Rows(0).Column("CapitalisationDate").GetValueAs<DateTime>();
            //get the number of days between the two dates
            TimeSpan ts = dateActual - DateTime.Now;
            int daysDiff = ts.Days;
            //compare the expected difference versus actual
            Logger.LogAction("Asserting that the capitalisation date is set on the rate override");
            Assert.AreEqual(daysDiff, 0);
        }
    }
}