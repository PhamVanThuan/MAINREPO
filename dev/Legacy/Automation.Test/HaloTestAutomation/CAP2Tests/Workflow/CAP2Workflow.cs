using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.Cap;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing.CATSDisbursement;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using Navigation = BuildingBlocks.Navigation;

namespace CAP2Tests
{
    /// <summary>
    /// The CAP 2 Workflow tests
    /// </summary>
    [TestFixture, RequiresSTA]
    public class CAP2Workflow : CAP2Tests.TestBase<BasePage>
    {
        /// <summary>
        /// IE TestBrowser Object
        /// </summary>
        private TestBrowser browser;

        /// <summary>
        /// Holds the database connection string
        /// </summary>
        private string Identifier;

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (browser != null)
            {
                if (browser.Page<BasePage>().CheckForErrorMessages())
                    this.browser.Refresh();
            }
        }

        /// <summary>
        /// Runs when the CAP 2 Workflow test fixture starts up
        /// </summary>
        [TestFixtureSetUp]
        public void TestSuiteStartUp()
        {
            browser = new TestBrowser(TestUsers.ClintonS, TestUsers.Password_ClintonS);
            browser.Navigate<Navigation.MenuNode>().CloseLoanNodesCBO();
        }

        /// <summary>
        /// Tests the creation of the callback scheduled activity.
        /// </summary>
        [Test, Description("Create a callback so that the timer can be tested")]
        public void _0001_CreateCallbackForCallbackExpired()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP4;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            string currentState = Service<ICAP2Service>().GetCAP2WorkflowCurrentState(CAPOffer.AccountKey);
            //create the callback
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreateCallback);
            string selectedHour;
            string selectedMin;
            browser.Page<MemoFollowUpAdd>().CreateFollowup(30, out selectedHour, out selectedMin);
            //assert the scheduled activity is set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey, ScheduledActivities.CAP2Offers.WaitforCallback, selectedHour, selectedMin);
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.CallbackHold);
            //Assert the case has moved states
            int instanceID = CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CallbackHold);
            //the previous state should be set up
            CAP2Assertions.AssertCAP2PreviousState(CAPOffer.AccountKey, currentState);
        }

        /// <summary>
        /// A CAp 2 Offer has an expiry timer that is set to the end of the CAP phase. Each CAP case should have this scheduled
        /// activity setup when it is created. This test ensures that the scheduled activity is set up to be date that the CAP
        /// sales phase ends and then update the scheduled activity value.
        /// </summary>
        [Test, Description("Once a CAP 2 offer has been created an expiry scheduled activity is set up for when the case will be auto archived")]
        public void _0002_CAP2ExpiryScheduledActivitySetUp()
        {
            Identifier = Cap2TestIdentifiers.CAPExpiryTest;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            int instanceID = Service<ICAP2Service>().GetCAP2InstanceID(CAPOffer.AccountKey);
            //get the expiry date
            string expDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            DateTime now = DateTime.Now;
            //find the difference between now and the expected expiry date
            TimeSpan diff = Convert.ToDateTime(expDate) - now;
            int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
            //this is how far in the future we are expecting the scheduled activity to be set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.OfferExpired, daysDiff, 0, 0);
        }

        /// <summary>
        /// Using the Correspondence screen a CAP 2 user should be able to send the CAP Letter and Agreement to the client. This test will
        /// ensure that a database record is inserted into the CorrespondenceHistory tables to record that the document has been
        /// sent to the client.
        /// </summary>
        [Test, Description("Ensures the Print CAP Letter action can be completed")]
        public void _001_CAP2PrintLetter()
        {
            Identifier = Cap2TestIdentifiers.CAP2PrintLetter;
            WatiN.Core.Settings.WaitForCompleteTimeOut = 120;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.PrintCapLetter);
            browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            //assert that the Correspondence record has been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(CAPOffer.AccountKey, CorrespondenceReports.CAPLetter, CorrespondenceMedium.Post);
        }

        /// <summary>
        /// A CAP offer should be able to be NTU'd and this will move the case the NTU offer state. A reason should be supplied
        /// for the NTU. This test also ensures that a scheduled activity is setup for the case to expire
        /// when the CAP Sales Phase is complete.
        /// </summary>
        [Test, Description("Ensures that a CAP 2 Offer can be NTU'd")]
        public void _002_CAP2NTU()
        {
            Identifier = Cap2TestIdentifiers.CAP2NTU;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.NTUCAP2Offer);
            //NTU the case
            browser.Page<CapOfferSummary>().DeclineOrNTUCAP2Application("NTU");
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.NotTakenUp);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.NTUOffer);
            //get the expiry date
            string expDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            DateTime now = DateTime.Now;
            //find the difference between now and the expected expiry date
            TimeSpan diff = Convert.ToDateTime(expDate) - now;
            int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
            //this is how far in the future we are expecting the scheduled activity to be set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.NTUOffer, daysDiff, 0, 0);
        }

        /// <summary>
        /// A CAP offer should be able to be declined and this will move the case the Declined offer state. A reason should be supplied
        /// for the decline. This test also ensures that a scheduled activity is setup for the case to expire
        /// when the CAP Sales Phase is complete.
        /// </summary>
        [Test, Description("Ensures that a CAP 2 Offer can be Declined")]
        public void _003_CAP2Decline()
        {
            Identifier = Cap2TestIdentifiers.CAP2Decline;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.DeclineCAP2);
            //Decline the case
            browser.Page<CapOfferSummary>().DeclineOrNTUCAP2Application("Decline");
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.OfferDeclined);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2OfferDeclined);
            //get the expiry date
            string expDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            DateTime now = DateTime.Now;
            //find the difference between now and the expected expiry date
            TimeSpan diff = Convert.ToDateTime(expDate) - now;
            int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
            //this is how far in the future we are expecting the scheduled activity to be set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.Declined, daysDiff, 0, 0);
        }

        /// <summary>
        /// Tests the rule that checks to ensure that an account's product is eligible for a CAP offer to be created against it.
        /// This test tries to create a CAP offer against a VariFix account which does not allow CAP as an option.
        /// </list>
        /// </summary>
        [Test, Description("Ensures that a CAP offer cannot be created on a VariFix account")]
        public void _004_VariFixCAP2()
        {
            Identifier = Cap2TestIdentifiers.VariFixCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Account Product does not permit CAP");
        }

        /// <summary>
        /// Tests the rule that will ensure that if an account has an existing CAP 2 rate override, that will not end prior to the
        /// CAP start date for the current sales phase, then we cannot create a CAP offer for the account.
        /// </summary>
        [Test, Description("Ensure a CAP offer cannot be created on an account with an active CAP")]
        public void _005_ExistingCAP2()
        {
            Identifier = Cap2TestIdentifiers.ExistingCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("The account has an active cap for the same period");
        }

        /// <summary>
        /// Tests the rule that checks to see if there as an existing Open CAP 2 offer for an account. If there is
        /// then we are not allowed to create the CAP 2 offer.
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created if the account has an open CAP 2 offer")]
        public void _006_ExistingCAP2Offer()
        {
            Identifier = Cap2TestIdentifiers.CAP2PrintLetter;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //ensure that you can select the Create CAP 2 Offer action
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CreateCAP2Offer();
            browser.Page<CAPCreateSearch>().CreateCAP2OfferCheckWarnings(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().IgnoreWarningsAndContinue();
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("An open cap offer already exists for this account");
        }

        /// <summary>
        /// Tests the rule that checks to see if the account has an active Interest Only rate override. If it does then we cannot
        /// create a CAP 2 Offer as CAP is not allowed on Interest Only accounts.
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created if the account has an Interest Only Rate Override")]
        public void _007_InterestOnlyCAP2Offer()
        {
            Identifier = Cap2TestIdentifiers.InterestOnlyCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cap is not permitted on Interest only accounts");
        }

        /// <summary>
        /// Tests the rule that will ensure that the Mortgage Loan Account status is set to Open. If it is not set to Open then
        /// we are not allowed to create a CAP 2 offer.
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created if the account is closed")]
        public void _008_ClosedLoanCAP2Offer()
        {
            Identifier = Cap2TestIdentifiers.ClosedAccountCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"Cap is only permitted on Open accounts. Account {0} is currently in Closed status", CAPOffer.AccountKey));
        }

        /// <summary>
        /// RCS accounts have a different reset configuration and there should not be CAP Sales Configuration for their ResetConfig.
        /// This rule ensures that should a user try and create a CAP 2 offer on an RCS account then they will receive a warning
        /// indicating that there is no CAP Sales Configuration for an RCS reset.
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created for an RCS Account")]
        public void _009_RCSAccountCAP2Offer()
        {
            Identifier = Cap2TestIdentifiers.RCSAccountCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists
                ("No Cap Type Configuration found for Reset Configuration - 15th March, June, September, December on Account - " + CAPOffer.AccountKey);
        }

        /// <summary>
        /// Tests the rule that ensures that if the Mortgage Loan Account is under cancellation then the user cannot create a CAP 2
        /// offer against the account. The rule looks for a Detail Type of "Under Cancellation".
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created on an account that is under cancellation")]
        public void _010_UnderCancellationCAP()
        {
            Identifier = Cap2TestIdentifiers.UnderCancellationCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cap2 create not allowed. Account - " + CAPOffer.AccountKey + " is under cancellation");
        }

        /// <summary>
        /// If a Mortgage Loan Account is currently undergoing Debt Counselling then the CAP user cannot create a CAP 2 offer against
        /// the Account. This is determined by counting the number of 3851 composites (into debt counselling) and comparing to the
        /// count of the 3852 composites (out of debt counselling). If IN > OUT then the account is undergoing debt counselling.
        /// </summary>
        [Test, Description("Ensure that a CAP offer cannot be created on an account that is in Debt Counselling")]
        public void _011_DebtCounsellingCAP()
        {
            Identifier = Cap2TestIdentifiers.DebtCounsellingCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("This Account is undergoing Debt Counselling.");
        }

        /// <summary>
        /// The Mortgage Loan Account can only have a CAP 2 offer created against it if the new balance after the CAP advance has
        /// been posted against the account is greater than a set minimum. The current minimum is R75,000. Only the CAP options that do
        /// qualify should be created against the CAP offer. If no options qualify then no offer is created.
        /// </summary>
        [Test, Description("Ensure that a CAP offer can only be created if the Account Balance + CAP Fee > 75000")]
        public void _012_MinBalanceCAP()
        {
            Identifier = Cap2TestIdentifiers.MinBalanceCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            //assert that the warning messages are displayed
            string[] capOptions = new string[] { CapTypes.OnePerc, CapTypes.TwoPerc, CapTypes.ThreePerc };
            const string minBalance = "R 75,000.00";
            browser.Page<CapOfferSummary>().AssertCAP2MinBalanceQualification(capOptions, minBalance);
        }

        /// <summary>
        /// A rule should be running in the workflow that will check to see if a CAP 2 offer requires a Readvance or a Further Advance
        /// in order to be accepted. When performing the Readvance Required action on a case that requires a Further Advance the rule
        /// will force the user to send the case for a Further Advance decision.
        /// </list>
        /// </summary>
        [Test, Description("A user should not be allowed to send a CAP offer down the Readvance Required route if it is a Further Advance app")]
        public void _013_ReadvanceRequiredOnFurtherAdvanceCAP()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.FormsSent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.AwaitingDocuments);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Increase in CLV is greater than 0. Further Advance Decision required. ");
        }

        /// <summary>
        /// If the Current Balance + CAP Advance will result in the Bond Registration Amount being exceeded then the user is
        /// warned that this CAP detail record does not qualify and as such is not created against the CAP Offer. This test creates
        /// a case where only the 1% option does not qualify, so both the 2% and 3% options should be created.
        /// </summary>
        [Test, Description("Ensure that CAP options that the user does not qualify for are not created: 1%")]
        public void _014_OnePercDNQCAP()
        {
            Identifier = Cap2TestIdentifiers.OnePercDNQCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            var capOptions = new int[] { 1 };
            browser.Page<CapOfferSummary>().AssertCAP2Qualification(capOptions);
            var validCapOptions = new int[] { 2, 3 };
            browser.Page<CapOfferSummary>().IgnoreWarningsAndContinue();
            CAP2Assertions.AssertCAPDetailRecordsExist(CAPOffer.AccountKey, validCapOptions);
        }

        /// <summary>
        /// If the Current Balance + CAP Advance will result in the Bond Registration Amount being exceeded then the user is
        /// warned that this CAP detail record does not qualify and as such is not created against the CAP Offer. This test creates
        /// a case where only the 1% and 2% options do not qualify, so both the 3% option should be created.
        /// </summary>
        [Test, Description("Ensure that CAP options that the user does not qualify for are not created: 1% and 2%")]
        public void _015_TwoPercDNQCAP()
        {
            Identifier = Cap2TestIdentifiers.TwoPercDNQCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            var capOptions = new int[] { 1, 2 };
            browser.Page<CapOfferSummary>().AssertCAP2Qualification(capOptions);
            var validCapOptions = new int[] { 3 };
            browser.Page<CapOfferSummary>().IgnoreWarningsAndContinue();
            CAP2Assertions.AssertCAPDetailRecordsExist(CAPOffer.AccountKey, validCapOptions);
        }

        /// <summary>
        /// If the Current Balance + CAP Advance will result in the Bond Registration Amount being exceeded then the user is
        /// warned that this CAP detail record does not qualify and as such is not created against the CAP Offer. This test creates
        /// a case where no options qualify so the CAP offer will not be created.
        /// </summary>
        [Test, Description("Ensure that CAP options that the user does not qualify for are not created: 1%, 2% and 3%")]
        public void _016_ThreePercentDNQCAP()
        {
            Identifier = Cap2TestIdentifiers.ThreePercDNQCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.CreateCaseCheckForWarnings(CAPOffer.AccountKey, browser);
            var capOptions = new int[] { 1, 2, 3 };
            browser.Page<CapOfferSummary>().AssertCAP2Qualification(capOptions);
        }

        /// <summary>
        /// An SPV has a mandated max PTI that a loan in the SPV is allowed to have. This test ensures that the rule correctly picks
        /// up that the effective PTI after the CAP advance has been posted does not exceed the maxiumum allowed for that SPV when
        /// performing the Readvance Required action.
        /// </summary>
        [Test, Description("A CAP offer that will result in the max PTI for an SPV being exceeded should not be allowed to be accepted")]
        public void _017_SPVMaxPTIExceededCAP()
        {
            Identifier = Cap2TestIdentifiers.PTIExceededCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            //select the cap option
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.OnePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //the PTI warning message should exist
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("PTI greater than Max PTI for cap type 1% Above Current Rate");
        }

        /// <summary>
        /// An SPV has a mandated max LTV that a loan in the SPV is allowed to have. This test ensures that the rule correctly picks
        /// up that the effective LTV after the CAP advance has been posted does not exceed the maxiumum allowed for that SPV when
        /// performing the Readvance Required action.
        /// </summary>
        [Ignore] //ignored as there is no test data for this test.
        [Test, Description("A CAP offer that will result in the max LTV for an SPV being exceeded should not be allowed to be accepted")]
        public void _018_SPVMaxLTVExceededCAP()
        {
            Identifier = Cap2TestIdentifiers.LTVExceededCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.OnePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //the PTI warning message should exist
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("LTV greater than Max LTV for cap type 1% Above Current Rate");
        }

        /// <summary>
        /// When a CAP has invoked on the account we give the client a refund. The client has an option of getting the CAP refund
        /// into either their loan account or the bank account linked to their debit order. When the CAP offer is accepted the user
        /// has to select a CAP payment option.
        /// </summary>
        [Test, Description("A payment option has be selected before a CAP offer option can be accepted.")]
        public void _019_PaymentOptionRequired()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Payment Option.");
            //case should still be a the same state
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.AwaitingDocuments);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms);
        }

        /// <summary>
        /// A client can be offered a promotional CAP 2 under certain circumstances. The CAP is free but we only allow the user
        /// to receive a 3% CAP 2 as a promotion. This should be the only option available on the screen when performing the
        /// Promotion Client action.
        /// </summary>
        [Test, Description("Only the 3% cap option should exist when offering a promotional CAP")]
        public void _020_PromotionCAP()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP2;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            int instanceID = Service<ICAP2Service>().GetCAP2InstanceID(CAPOffer.AccountKey);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the promotion client action
            browser.ClickAction(WorkflowActivities.Cap2Offers.PromotionClient);
            //only the 3% option should exist in the grid
            string[] capOptionsDne = new string[] { CapTypes.OnePerc, CapTypes.TwoPerc };
            browser.Page<CapOfferSummary>().AssertCAP2OptionsDoNotExist(capOptionsDne);
            string[] capOptionsExist = new string[] { CapTypes.ThreePerc };
            browser.Page<CapOfferSummary>().AssertCAP2OptionsExist(capOptionsExist);
            //continue
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.TakenUp);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CompletedCAP2Offer);
            //Assert the Cap Option has been taken up
            CAP2Assertions.AssertCAP2OptionTakenUp(3, CAPOffer.AccountKey);
            //there should be a scheduled activity setup for the Completed Expired timer
            string expDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            DateTime now = DateTime.Now;
            //find the difference between now and the expected expiry date
            TimeSpan diff = Convert.ToDateTime(expDate) - now;
            int daysDiff = Convert.ToInt32(Math.Round(diff.TotalDays, 0));
            //this is how far in the future we are expecting the scheduled activity to be set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.CompletedExpired, daysDiff, 0, 0);
        }

        /// <summary>
        /// Prior to moving the CAP 2 offer to the Completed CAP 2 Offer state the CAP ReAdvance should be posted in Loan Servicing
        /// by the CAP manager. If this transaction has not been posted then the workflow case cannot continue.
        /// </summary>
        [Test, Description("The Readvance has to be posted in Loan Servicing before completing the Readvance Done action")]
        public void _021_ReadvanceDoneCAPAdvanceNotPosted()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP3;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            //select the cap option
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.TwoPerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            Thread.Sleep(5000);
            //assert the case has moved states
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.ReadvanceRequired);
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceDone);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //warning should exist
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cannot complete action as Readvance has not been posted");
        }

        /// <summary>
        /// Should the CAP 2 offer require a Readvance to be posted but the effective LTV after the CAP 2 offer has been posted
        /// will be greater than 80% then we require that case to be sent to Credit for approval
        /// </summary>
        [Test, Description("A CAP Readvance with an effective LTV > 80% should go to Credit for approval.")]
        public void _022_ReadvanceRequiredReadvanceOver80()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceGreaterThan80;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            //select the cap option
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.OnePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "Readvances where the LTV is greater than 80% will go automatically to credit for approval");
            browser.Page<CapOfferSummary>().DomainWarningClickYes();
            Thread.Sleep(5000);
            //assert the case has moved states
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.PrepareForCredit);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.PrepareForCredit);
        }

        /// <summary>
        /// A Further Advance CAP case needs to be sent to the Credit user in order to be approved. This test ensures
        /// that the workflow case can be sent into the Credit Approval state.
        /// </summary>
        [Test, Description("Ensures that a CAP Further Advance case can be sent to the Credit Approval state")]
        public void _023_FurtherAdvanceSendToCredit()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //this test case was used in _013_ReadvanceRequiredOnFurtherAdvanceCAP and is already at Awaiting Forms
            //perform the Further Advance Decison action
            browser.ClickAction(WorkflowActivities.Cap2Offers.FurtherAdvanceDecision);
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.PrepareForCredit);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.PrepareForCredit);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.PrepareForCredit, browser);
            //perform the Credit Approval action
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreditApproval);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.CreditApproval);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CreditApproval);
        }

        /// <summary>
        /// This test will login as the CAP Credit user who has been assigned the Further Advance for appoval at the Credit
        /// Approval state. After the case has been approved it moves to the Granted CAP2 Offer state
        /// </summary>
        [Test, Description("Approves a CAP Further Advance case from the Credit Approval state")]
        public void _024_FurtherAdvanceCAPCreditApprove()
        {
            //login again as Credit User
            TestBrowser creditBrowser = new TestBrowser(TestUsers.CreditUnderwriter2, TestUsers.Password);
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //this test case was used in _023_FurtherAdvanceSendToCredit and is already at Credit Approval
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CreditApproval, creditBrowser);
            //approve the case
            creditBrowser.ClickAction(WorkflowActivities.Cap2Offers.GrantCAP2Offer);
            creditBrowser.Page<CapOfferSummary>().CompleteCAP2Action();
            //case should now be at Awaiting LA
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.GrantedCap2Offer);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.GrantedCAP2Offer);
            creditBrowser.Dispose();
            creditBrowser = null;
        }

        /// <summary>
        /// Further Advance cases require that a new Loan Agreement be sent to the client before the further advance can be
        /// performed on the account. After the user has completed the LASent action they can then move the case to the
        /// Readvance Required state.
        /// </summary>
        [Test, Description("The LA needs to be sent for a Further Advance case at the Granted CAP2 Offer")]
        public void _025_FurtherAdvanceAwaitingLA()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //this test case was used in _024_FurtherAdvanceCAPCreditApprove() and is already at Granted CAP2 Offer
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.GrantedCAP2Offer, browser);
            //perform the Awaiting LA Sent
            browser.ClickAction(WorkflowActivities.Cap2Offers.LASent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.AwaitingLA);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingLA);
            //load the case at the next state
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingLA, browser);
            //perform the Awaiting LA Sent
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadyForReadvance);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.ReadvanceRequired);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired);
        }

        /// <summary>
        /// There is a common action called Change Payment Option that allows a CAP Consultant to change the CAP Payment Option
        /// for the CAP offer. This test ensures the user can complete the action and that the CAP Payment Option on the CAP offer
        /// has been updated.
        /// </summary>
        [Test, Description("A Cap Consultant should be allowed to change the CAP Payment Option for the CAP Offer")]
        public void _026_ChangeCAPPaymentOption()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //this test case was used in _025_FurtherAdvanceAwaitingLA() and is already at Readvance Required Offer
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ChangePaymentOption);
            //change the payment option
            string newPaymentOption = browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //assert that it has changed
            CAP2Assertions.AssertCAP2PaymentOption(CAPOffer.AccountKey, newPaymentOption);
            CAP2Assertions.AssertCAP2OptionTakenUp(1, CAPOffer.AccountKey);
        }

        /// <summary>
        /// Creating a callback on a CAP offer will move the case to the Callback Hold until such point as either the
        /// Wait for Callback timer expires or the CAP Consultant chooses to continue with the offer.  This test case ensures the case
        /// moves to the correct hold state and that the scheduled activity is setup correctly.
        /// </summary>
        [Test, Description("A CAP user should be able to create a Callback for a CAP 2 offer")]
        public void _027_CreateCallback()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //this test was last used in _019_PaymentOptionRequired() and is at Awaiting Forms
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //prior to creating lets fetch the current state
            string currentState = Service<ICAP2Service>().GetCAP2WorkflowCurrentState(CAPOffer.AccountKey);
            //create the callback
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreateCallback);
            string selectedHour; string selectedMin;
            browser.Page<MemoFollowUpAdd>().CreateFollowup(30, out selectedHour, out selectedMin);
            //assert the scheduled activity is set up
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey, ScheduledActivities.CAP2Offers.WaitforCallback, selectedHour, selectedMin);
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.CallbackHold);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CallbackHold);
            //the previous state should be set up
            CAP2Assertions.AssertCAP2PreviousState(CAPOffer.AccountKey, currentState);
        }

        /// <summary>
        /// The consultant does not have to wait for the Wait for Callback timer to expire before continuing with the CAP 2 case.
        /// They have the option to perform the Continue Sale action from the Callback Hold state. This test case will retrieve the
        /// case's previous state and status, ensuring the case is reinstated correctly.
        /// </summary>
        [Test, Description("A CAP consultant can choose to continue with the case without waiting for the Callback to expire")]
        public void _028_ContinueSaleFromCallbackHold()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP1;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //this test was last used in _027_CreateCallback() and should be at Callback Hold
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CallbackHold, browser);
            //we need to know where it is getting reinstated to
            string nextState; string nextStatus;
            Service<ICAP2Service>().GetCAP2PreviousStateAndStatus(CAPOffer.AccountKey, out nextState, out nextStatus);
            //Continue with the Sale
            browser.ClickAction(WorkflowActivities.Cap2Offers.ContinueSale);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //the case moves to its previous stae
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, nextStatus);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, nextState);
        }

        /// <summary>
        /// Once the Wait for Callback timer has expired then the case will be moved to the Ready for Callback state. This will allow
        /// the consultant to perform the Continue With Sale action in order to carry on with the CAP 2 offer. This test case will
        /// retrieve the previous CAP 2 state and status, ensuring that the case is reinstated correctly.
        /// </summary>
        [Test, Description("When the Callback Expires the case should be moved to the Ready to Callback state from which it can continue")]
        public void _029_ContinueWithSaleFromReadyToCallback()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP4;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            scriptEngine.ExecuteScript(WorkflowEnum.CAP2Offers, WorkflowAutomationScripts.Cap2Offers.WaitForCallbackTimer, CAPOffer.CapOfferKey);
            //this test was last used in _027_CreateCallback() and should be at Callback Hold
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadytoCallback, browser);
            //Assert the case has changed when the callback expired
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.ReadytoCallback);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadytoCallback);
            //we need to know where it is getting reinstated to
            string nextState; string nextStatus;
            Service<ICAP2Service>().GetCAP2PreviousStateAndStatus(CAPOffer.AccountKey, out nextState, out nextStatus);
            //continue with sale
            browser.ClickAction(WorkflowActivities.Cap2Offers.ContinuewithSale);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, nextStatus);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, nextState);
        }

        /// <summary>
        /// When a user tries to create a CAP 2 offer on a Mortgage Loan Account that has not yet been through a rate reset, an error
        /// message should be displayed to them indicating that this is not allowed.
        /// <list type="table">
        /// <listheader>
        /// <rule>Rule Name</rule>
        /// </listheader>
        /// <item>
        /// <rule>ApplicationCap2AccountResetDateCheck</rule>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("A CAP cannot be sold to a client that has not yet been through a reset")]
        public void _030_CAP2ResetCheck()
        {
            Identifier = Cap2TestIdentifiers.CAP2ResetCheck;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CreateCAP2Offer();
            browser.Page<CAPCreateSearch>().CreateCAP2OfferCheckWarnings(CAPOffer.AccountKey);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Cap is not permitted on an account that has not been through a reset date.");
        }

        /// <summary>
        /// Once a CAP offer has been moved to the Readvance Required state the CAP manager should then be allowed to post the CAP
        /// 2 Advance from the CATS Disbursement screen in Loan Servicing.
        /// </summary>
        [Test, Description("A CAP manager user should be able to post the CAP 2 disbursement against the account")]
        public void _031_PostCAP2DisbursementReadvanceCAP()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP3;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(CAPOffer.AccountKey);

            browser.Navigate<LoanServicingCBO>().LoanAccountNode(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
            //post the CAP 2 Readvance
            browser.Page<CATSDisbursementAdd>().PostCAP2Readvance();
            //assert that a disbursement record exists
            CAP2Assertions.AssertCAP2ReadvanceDisbursementPosted(CAPOffer.AccountKey);
            CAP2Assertions.AssertCAP2AdvanceAmount(CAPOffer.AccountKey);
        }

        /// <summary>
        /// After the case has had the CAP 2 Advance performed against the account, the CAP 2 case can be completed and moved into
        /// the Completed CAP 2 Offer state by performing the Readvance Done action.
        /// </summary>
        [Test, Description("Once the readvance has been posted then the manager can move the case to Completed CAP 2 Offer")]
        public void _032_ReadvanceDoneReadvanceCAP()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP3;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceDone);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.TakenUp);
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CompletedCAP2Offer);
        }

        /// <summary>
        /// When a CAP 2 offer is at the NTU or Decline state the CAP 2 consultant has the option of reworking the CAP 2 offer.
        /// This moves the case back from the NTU or Decline state and will re-open the case. This results in the case being
        /// moved to the Open CAP2 Offer state.
        /// </summary>
        /// <param name="identifier">This is the CAP Test Identifier this test is repeated for. (CAP2NTU and CAP2Decline)</param>
        [Test, Sequential, Description("Reworking an NTU/Decline CAP should re-open the CAP offer")]
        public void _033_ReworkCAP2Offer([Values(Cap2TestIdentifiers.CAP2NTU, Cap2TestIdentifiers.CAP2Decline)] string identifier)
        {
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(identifier);
            //select from the worklist, depending on which test we are reinstating
            switch (identifier)
            {
                case Cap2TestIdentifiers.CAP2NTU:
                    Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.NTUOffer, browser);
                    break;

                case Cap2TestIdentifiers.CAP2Decline:
                    Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2OfferDeclined, browser);
                    break;
            }

            browser.ClickAction(WorkflowActivities.Cap2Offers.ReworkOffer);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //case should now be reopened
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.Open);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer);
        }

        /// <summary>
        /// When a CAP consultant is trying to move a CAP 2 offer that requires a Further Advance to be performed we need to first
        /// check that the SPV still allows further lending. If it does not allow further lending then the user will be given a
        /// a error message indicating that they must first move the account out of the SPV and then continue.
        /// <list type="table">
        /// <listheader>
        /// <rule>Rule Name</rule>
        /// </listheader>
        /// <item>
        /// <rule>ApplicationCap2AllowFurtherLendingSPV</rule>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("A further advance CAP cannot move to the Readvance Required state if the SPV doesnt allow further lending")]
        public void _034_FurtherAdvanceCAPSPVDisallowsFurtherLending()
        {
            Identifier = Cap2TestIdentifiers.CAP2SPVNotAllowed;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.FormsSent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.FurtherAdvanceDecision);
            //select a payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.ThreePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreditApproval);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //we need to change users now
            //login again as Credit User
            var creditBrowser = new TestBrowser(TestUsers.CreditUnderwriter2);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CreditApproval, creditBrowser);
            //approve the case
            creditBrowser.ClickAction(WorkflowActivities.Cap2Offers.GrantCAP2Offer);
            creditBrowser.Page<CapOfferSummary>().CompleteCAP2Action();
            creditBrowser.Dispose();
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.GrantedCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.LASent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadyForReadvance);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //there should now be a warning on the screen
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please arrange for this account to be moved out of this SPV as it is set to no further lending.");
            //the case should not move states or change status
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, CAPStatus.AwaitingLA);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingLA);
        }

        /// <summary>
        /// Once the CAP 2 offer has been completed then the consultant has the option to cancel the CAP 2 offer by performing
        /// the Cancel CAP 2 Offer action. This will move the case to the Cancelled CAP 2 Offer state in the workflow.
        /// </summary>
        [Test, Description("A user should be allowed to cancel a CAP Offer after it has been completed")]
        public void _035_CancelCAP2Offer()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP4;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.FormsSent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.AwaitingDocuments);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms);
            //at this point try and post the CAP 2 Readvance disbursement
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
            browser.Page<CATSDisbursementAdd>().SelectDisbursementType(DisbursementTransactionTypes.CAP2ReAdvance);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("No Cap Offers Awaiting Readvance Found !.");
            browser.Navigate<LoanServicingCBO>().RemoveLegalEntities();
            //load the case
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.ThreePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //post the disbursement
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();

            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(CAPOffer.AccountKey);

            browser.Navigate<LoanServicingCBO>().LoanAccountNode(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
            //post the CAP 2 Readvance
            browser.Page<CATSDisbursementAdd>().PostCAP2Readvance();
            //assert that a disbursement record exists
            CAP2Assertions.AssertCAP2ReadvanceDisbursementPosted(CAPOffer.AccountKey);
            CAP2Assertions.AssertCAP2AdvanceAmount(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().RemoveLegalEntities();
            //load the case and perform readvance done
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceDone);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.CancelCAP2Offer);
            browser.Page<CapOfferSummary>().DeclineOrNTUCAP2Application("NTU");
            //check that the offer has been updated
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CancelledCAP2Offer);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.NotTakenUp);
        }

        /// <summary>
        /// Confirming the cancellation of the CAP 2 offer should only be done once the correction for the loan transaction has been
        /// posted and the CAP 2 rate override deactivated. This action will archive the case.
        /// </summary>
        [Test, Description("Confirming the cancellation will move the case to the CAP 2 Expired state")]
        public void _036_ConfirmCAP2Cancellation()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP4;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CancelledCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.ConfirmCancellation);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //check the offer has been updated
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2Expired);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.NotTakenUp);
        }

        /// <summary>
        /// This test will move a CAP 2 offer into the NTU Offer state. The test ensures first that the scheduled activity has been created
        /// and will then update the timer value for a future test to assert that the timer correctly fires and archives the workflow
        /// case.
        /// </summary>
        [Test, Description("The NTU scheduled activity should archive the cap 2 offer")]
        public void _037_NTUScheduledActivitySetUp()
        {
            Identifier = Cap2TestIdentifiers.CAP2NTU;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            int instanceID = Service<ICAP2Service>().GetCAP2InstanceID(CAPOffer.AccountKey);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.NTUCAP2Offer);
            //NTU the case
            browser.Page<CapOfferSummary>().DeclineOrNTUCAP2Application("NTU");
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.NotTakenUp);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.NTUOffer);
            string expiryDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            //Check that the scheduled activity has been created
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.NTUOffer, 0, 0, 0, expiryDate);
        }

        /// <summary>
        /// This test will move a CAP 2 offer into the CAP2 Offer Declined state. The test ensures first that the scheduled activity has been created
        /// and will then update the timer value for a future test to assert that the timer correctly fires and archives the workflow
        /// case.
        /// </summary>
        [Test, Description("The Declined scheduled activity should archive the cap 2 offer")]
        public void _038_DeclinedScheduledActivitySetUp()
        {
            Identifier = Cap2TestIdentifiers.CAP2Decline;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            int instanceID = Service<ICAP2Service>().GetCAP2InstanceID(CAPOffer.AccountKey);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            //select the action
            browser.ClickAction(WorkflowActivities.Cap2Offers.DeclineCAP2);
            //Decline the case
            browser.Page<CapOfferSummary>().DeclineOrNTUCAP2Application("Decline");
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.OfferDeclined);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2OfferDeclined);
            string expiryDate = Service<ICAP2Service>().GetCapExpiryDate(CAPOffer.AccountKey);
            //Check that the scheduled activity has been created
            X2Assertions.AssertScheduleActivitySetup(CAPOffer.AccountKey.ToString(), ScheduledActivities.CAP2Offers.Declined, 0, 0, 0, expiryDate);
        }

        /// <summary>
        /// The balance to CAP for the CAP offer is stored against the CAP offer and remains static unless the CAP 2 offer gets
        /// recalculated. The reason for this is that the CAP premiums are calculated based on the outstanding balance at the time
        /// of the offer being created. A consultant has the ability to update the Balance to CAP and the premiums by performing
        /// the Recalculate CAP 2 action. This test will post a pre-payment against the account and then recalculate the CAP 2 offer
        /// ensuring that the CAP Balance gets reduced.
        /// </summary>
        [Test, Description("Recalculating a CAP offer after the account balance has changed should recalculate the premiums etc")]
        public void _039_RecalculateCAP2OfferPostTransaction()
        {
            Identifier = Cap2TestIdentifiers.RecalculateCAP2OfferPostTransaction;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //load the client into the CBO
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(CAPOffer.AccountKey);
            //we need the cap offer details prior to reworking
            var results = Service<ICAP2Service>().GetLatestCapOfferByAccountKey(CAPOffer.AccountKey);
            double balanceToCap = results.Rows(0).Column("CurrentBalance").GetValueAs<double>();
            results.Dispose();
            //load up the account and post a prepayment
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().LoanTransactions();
            browser.Navigate<LoanServicingCBO>().PostTransactions();
            const decimal transactionValue = 15000.25M;
            browser.Page<PostTransaction>().PostLoanTransaction(TransactionTypeEnum.PrePayment320, transactionValue, DateTime.Now, "CAP 2 Test");
            //now we need to load up the offer and recalculate it
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.RecalculateCAP2);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.Page<CapOfferSummary>().ClickAddOffer();
            //the balance to cap should reflect the new balance
            results = Service<ICAP2Service>().GetLatestCapOfferByAccountKey(CAPOffer.AccountKey);
            double newBalanceToCap = results.Rows(0).Column("CurrentBalance").GetValueAs<double>();
            results.Dispose();
            double calculatedBalanceToCap = balanceToCap - Convert.ToDouble(transactionValue);
            Assert.AreEqual(newBalanceToCap, calculatedBalanceToCap);
        }

        /// <summary>
        /// This test uses the case setup earlier in the _0002_CAP2ExpiryScheduledActivitySetUp() test. By this point in the test
        /// suite the scheduled activity for the test case will have fired and this test ensures that the case has been correctly archived.
        /// <seealso cref="_0002_CAP2ExpiryScheduledActivitySetUp()"/>
        /// </summary>
        [Test, Description("When the CAP 2 Expiry Date is reached the case should be archived.")]
        public void _040_ExpiredCAP2OfferTimer()
        {
            Identifier = Cap2TestIdentifiers.CAPExpiryTest;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            scriptEngine.ExecuteScript(WorkflowEnum.CAP2Offers, WorkflowAutomationScripts.Cap2Offers.OfferExpiredTimer, CAPOffer.CapOfferKey);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.Expired);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2Expired);
        }

        /// <summary>
        /// When declining a CAP 2 offer a Decline must be provided before the workflow action can be completed.
        /// </summary>
        [Test, Description("When declining or NTU'ing a CAP offer a reason has to be selected")]
        public void _041_CAP2DeclineReasonMandatory()
        {
            Identifier = Cap2TestIdentifiers.OnePercDNQCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.DeclineCAP2);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //there should be a validation message
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Reason.");
        }

        /// <summary>
        /// When NTU'ing a CAP 2 offer a NTU Reason must be provided before the workflow action can be completed.
        /// </summary>
        [Test, Description("When declining or NTU'ing a CAP offer a reason has to be selected")]
        public void _042_CAP2NTUReasonMandatory()
        {
            Identifier = Cap2TestIdentifiers.OnePercDNQCAP;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.NTUCAP2Offer);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //there should be a validation message
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Reason.");
        }

        /// <summary>
        /// When cancelling a CAP 2 offer a Cancellation Reason must be provided before the workflow action can be completed.
        /// </summary>
        [Test, Description("When cancelling a CAP 2 offer a reason has to be selected")]
        public void _043_CAP2CancellationReasonMandatory()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP3;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CompletedCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.CancelCAP2Offer);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //there should be a validation message
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please select a Reason.");
        }

        /// <summary>
        /// Readvance CAP 2 offers can still be sent to Credit for approval. However, a CAP offer that does not require a Further
        /// Advance to be performed does not require a new Loan Agreement to be sent to the client. A system decision is made after
        /// the Grant CAP 2 Offer action has been performed in order to move these cases directly to the Readvance Required state.
        /// </summary>
        [Test, Description("A Readvance offer that is sent to credit for approval does not require a new LA to be sent")]
        public void _044_CAP2ReadvanceSendToCredit()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceGreaterThan80;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.PrepareForCredit, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreditApproval);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            //we need to change users now login again as Credit User
            var creditBrowser = new TestBrowser(TestUsers.CreditUnderwriter2);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CreditApproval, creditBrowser);
            //approve the case
            creditBrowser.ClickAction(WorkflowActivities.Cap2Offers.GrantCAP2Offer);
            creditBrowser.Page<CapOfferSummary>().CompleteCAP2Action();
            //this case should skip the LA Sent action as it is a readvance application and does not require it
            Thread.Sleep(5000); //lets wait for the workflow to run its checks
            //Assert the case has changed status
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.ReadvanceRequired);
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            //Assert the case has moved states
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired);
            creditBrowser.Dispose();
            creditBrowser = null;
        }

        /// <summary>
        /// This test case uses a previous test case (_020_PromotionCAP) where the CAP has been completed and the scheduled activity
        /// for the expiry has been updated. The test ensures that the updated scheduled activity has been fired and that the case
        /// has been correctly archived.
        /// <seealso cref="_020_PromotionCAP"/>
        /// </summary>
        [Test, Description("The Completed Expired timer should archive the CAP 2 workflow case")]
        public void _045_CompletedExpiredTimer()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP2;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            scriptEngine.ExecuteScript(WorkflowEnum.CAP2Offers, WorkflowAutomationScripts.Cap2Offers.CompletedExpiredTimer, CAPOffer.CapOfferKey);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.TakenUp);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2Expired);
        }

        /// <summary>
        /// This test case executes the SP that opts awaiting cases into CAP 2
        /// </summary>
        [Test, Description("Ensures that the rate override records are correctly created from our CAP 2 offer")]
        public void _046_OptInCAP2()
        {
            Identifier = Cap2TestIdentifiers.ReadvanceCAP3;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            //we need to call the proc to create the rate overrides
            Service<ICAP2Service>().OptIntoCAP2();
            //now we can run our assertions
            CAP2Assertions.AssertCAP2FinancialAdjustmentExists(CAPOffer.AccountKey, FinancialAdjustmentStatusEnum.Inactive);
            CAP2Assertions.AssertCap2DataSetUp(CAPOffer.AccountKey, FinancialAdjustmentStatusEnum.Inactive);
        }

        /// <summary>
        /// This test has its scheduled activity updated in _037_NTUScheduledActivitySetUp. This activity should now have fired
        /// and the case archived. This test case ensures that the NTU timer is archiving the case correctly.
        /// <seealso cref="_002_CAP2NTU"/>
        /// <seealso cref="_033_ReworkCAP2Offer"/>
        /// <seealso cref="_037_NTUScheduledActivitySetUp"/>
        /// </summary>
        [Test, Description("When the NTU Offer Timer elapses the case should be archived.")]
        public void _047_NTUCAPOfferTimer()
        {
            Identifier = Cap2TestIdentifiers.CAP2NTU;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            scriptEngine.ExecuteScript(WorkflowEnum.CAP2Offers, WorkflowAutomationScripts.Cap2Offers.NTUOfferTimer, CAPOffer.CapOfferKey);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.NotTakenUp);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2Expired);
        }

        /// <summary>
        /// This test has its scheduled activity updated in _037_DeclinedScheduledActivitySetUp. This activity should now have fired
        /// and the case archived. This test case ensures that the Decline timer is archiving the case correctly.
        /// <seealso cref="_003_CAP2Decline"/>
        /// <seealso cref="_033_ReworkCAP2Offer"/>
        /// <seealso cref="_038_DeclinedScheduledActivitySetUp"/>
        /// </summary>
        [Test, Description("When the Declined timer elapses the case should be archived.")]
        public void _048_DeclineCAPOfferTimer()
        {
            Identifier = Cap2TestIdentifiers.CAP2Decline;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            scriptEngine.ExecuteScript(WorkflowEnum.CAP2Offers, WorkflowAutomationScripts.Cap2Offers.DeclinedTimer, CAPOffer.CapOfferKey);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, CAPStatus.OfferDeclined);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CAP2Expired);
        }

        /// <summary>
        /// There are rules in the CAP 2 workflow that will prevent a user from moving the case to the Readvance Required state if the SPV no longer allows
        /// further lending and the CAP Advance will result in a Further Advance being performed. This test ensures that if a CAP case where the new balance will
        /// not exceed the LAA AND the account is an SPV that does not allow further lending, then the application can still be processed.
        /// </summary>
        [Test, Description(@"There are rules in the CAP 2 workflow that will prevent a user from moving the case to the Readvance Required state if the SPV no longer allows
		further lending and the CAP Advance will result in a Further Advance being performed. This test ensures that if a CAP case where the new balance will
		not exceed the LAA AND the account is an SPV that does not allow further lending, then the application can still be processed.")]
        public void _049_CAP2SPVNotAllowedFAdvUnderLAA()
        {
            Identifier = Cap2TestIdentifiers.CAP2SPVNotAllowedFAdvUnderLAA;
            ProcessCAPUnderLAAViaCredit(Identifier);
        }

        /// <summary>
        /// A user should not be allowed to send a CAP offer down the Readvance Required route if it is a Further Advance app. This test case ensures
        /// that this rule is still fired on further advance CAP where the new balance will be less than the LAA."
        /// </summary>
        [Test, Description(@"A user should not be allowed to send a CAP offer down the Readvance Required route if it is a Further Advance app. This test case ensures
			that this rule is still fired on further advance CAP where the new balance will be less than the LAA.")]
        public void _050_ReadvanceRequiredOnFurtherAdvanceUnderLAA()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAPUnderLAA2;
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(Identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.AwaitingForms, browser);
            //select the Readvance Required action
            browser.ClickAction(WorkflowActivities.Cap2Offers.ReadvanceRequired);
            //select the payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Increase in CLV is greater than 0. Further Advance Decision required. ");
        }

        /// <summary>
        /// The CAP 2 workflow needs to distinguish between Further Advance and Readvance CAP cases. i.e Will the CAP Advance result in the LAA being exceeded or not.
        /// This test will send a Further Advance CAP case that is below the LAA through the credit portion of the CAP 2 workflow and ensure that the case
        /// skips the Granted CAP 2 Offer state, as no new Loan Agreement is required.
        /// </summary>
        [Test, Description(@"The CAP 2 workflow needs to distinguish between Further Advance and Readvance CAP cases. i.e Will the CAP Advance result in the LAA being
		exceeded or not. This test will send a Further Advance CAP case that is below the LAA through the credit portion of the CAP 2 workflow and ensure that the case
		skips the Granted CAP 2 Offer state, as no new Loan Agreement is required.")]
        public void _051_FurtherAdvanceCAPUnderLAA()
        {
            Identifier = Cap2TestIdentifiers.FurtherAdvanceCAPUnderLAA;
            ProcessCAPUnderLAAViaCredit(Identifier);
        }

        /// <summary>
        /// There are rules in the CAP 2 workflow that will prevent a user from moving the case to the Readvance Required state if the SPV no longer allows
        /// further lending and the CAP Advance will result in a Further Advance being performed. This test ensures that if a CAP case where the new balance will
        /// be less than the CLV AND the account is an SPV that does not allow further lending, then the application can still be processed.
        /// </summary>
        [Test, Description(@"There are rules in the CAP 2 workflow that will prevent a user from moving the case to the Readvance Required state if the SPV no longer allows
		further lending and the CAP Advance will result in a Further Advance being performed. This test ensures that if a CAP case where the new balance will
		be less than the CLV AND the account is an SPV that does not allow further lending, then the application can still be processed.")]
        public void _052_CAP2SPVNotAllowedReadvance()
        {
            Identifier = Cap2TestIdentifiers.CAP2SPVNotAllowedReadvance;
            ProcessCAPUnderLAAViaCredit(Identifier);
        }

        /// <summary>
        /// This test will ensure that we can disburse the test cases from tests 49, 51 and 52 above.
        /// </summary>
        [Test, Description("This test will ensure that we can disburse the test cases from tests 49, 51 and 52 above.")]
        public void _053_DisburseCAP2Cases([Values(Cap2TestIdentifiers.CAP2SPVNotAllowedFAdvUnderLAA, Cap2TestIdentifiers.CAP2SPVNotAllowedReadvance, Cap2TestIdentifiers.FurtherAdvanceCAPUnderLAA)] string identifier)
        {
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(identifier);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(CAPOffer.AccountKey);
            browser.Navigate<LoanServicingCBO>().CATSDisbursement(NodeTypeEnum.Add);
            //post the CAP 2 Readvance
            browser.Page<CATSDisbursementAdd>().PostCAP2Readvance();
            //assert that a disbursement record exists
            CAP2Assertions.AssertCAP2ReadvanceDisbursementPosted(CAPOffer.AccountKey);
            CAP2Assertions.AssertCAP2AdvanceAmount(CAPOffer.AccountKey);
        }

        /// <summary>
        /// This test case will ensure that the warning is displayed to the user when trying to create a new CAP case where one of the legal entities playing a role on the
        /// account is under debt counselling on another account.
        /// </summary>
        [Test, Description(@"This test case will ensure that the warning is displayed to the user when trying to create a new CAP case where one of the legal entities playing a role on the
		account is under debt counselling on another account.")]
        public void _054_LegalEntityUnderDebtCounsellingOnRelatedAccount()
        {
            var legalEntities = Service<ICAP2Service>().GetLegalEntitiesWithMoreThanOneCAPTestCase();
            //we need the account of the first case to put it under debt counselling.
            var accountKey = legalEntities.Rows(1).Column("AccountKey").GetValueAs<int>();
            var relatedAccountKey = legalEntities.Rows(0).Column("AccountKey").GetValueAs<int>();
            //insert debt counselling
            List<int> insertedKeys = new List<int>();
            try
            {
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                //we need to try and create CAP on the second account
                Helper.CreateCaseCheckForWarnings(relatedAccountKey, browser);
                DebtCounsellingAssertions.AssertRelatedAccountDebtCounsellingRule(browser, relatedAccountKey, accountKey, null);
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        public void _055_LegalEntityUnderDebtCounsellingWarningOnSummaryScreen()
        {
            var legalEntities = Service<ICAP2Service>().GetLegalEntitiesWithMoreThanOneCAPTestCase();
            //we need the account of the first case to put it under debt counselling.
            var accountKey = legalEntities.Rows(1).Column("AccountKey").GetValueAs<int>();
            var relatedAccountKey = legalEntities.Rows(0).Column("AccountKey").GetValueAs<int>();
            var insertedKeys = new List<int>();
            //check for open cap offer
            var capOffer = Service<ICAP2Service>().GetLatestCapOfferByAccountKey(relatedAccountKey);
            var openCAPOffer = (from co in capOffer
                                where capOffer.Rows(0).Column("CapStatusKey").GetValueAs<int>() == (int)CapStatusEnum.Open
                                select co).FirstOrDefault();
            try
            {
                if (openCAPOffer == null)
                {
                    Helper.CreateCAPCase(browser, relatedAccountKey);
                }
                //put the other one under DC
                insertedKeys = Service<IDebtCounsellingService>().AddAccountUnderDebtCounselling(accountKey);
                //load the case
                Helper.LoadCAP2Offer(relatedAccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
                //Assert warnings
                DebtCounsellingAssertions.AssertRelatedAccountDebtCounsellingRule(browser, relatedAccountKey, accountKey, null);
            }
            finally
            {
                Service<IDebtCounsellingService>().RemoveDebtCounsellingCase(insertedKeys[0], insertedKeys[1]);
            }
        }

        /// <summary>
        /// When a user tries to create a cap offer on an account that has a pending financial adjustment (Status = Inactive with a future dated
        /// start date), a warning should be displayed to the user and no offer created.
        /// </summary>
        [Test]
        public void _056_CannotCreateCAP2OfferWhenFinancialAdjustmentIsPending()
        {
            var results = Service<IFinancialAdjustmentService>().GetAccountWithFutureDatedFinancialAdjustment(
                FinancialAdjustmentTypeSourceEnum.CAP2_FixedRateAdjustment, FinancialAdjustmentStatusEnum.Inactive);
            var account = base.Service<IAccountService>().GetAccountByKey(results.Column("accountKey").GetValueAs<int>());
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().LegalEntityMenu();
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CreateCAP2Offer();
            browser.Page<CAPCreateSearch>().CreateCAP2OfferCheckWarnings(account.AccountKey);
            browser.Page<BasePageAssertions>().AssertValidationMessagesContains("A Financial Adjustment of type CAP is pending");
        }

        #region HelperMethods

        private void ProcessCAPUnderLAAViaCredit(string _identifier)
        {
            var CAPOffer = Service<ICAP2Service>().GetTestCapOffer(_identifier);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.OpenCAP2Offer, browser);
            browser.ClickAction(WorkflowActivities.Cap2Offers.FormsSent);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.FurtherAdvanceDecision);
            //select a payment option
            browser.Page<CapOfferSummary>().SelectCAPPaymentOption(CAPOffer.AccountKey);
            browser.Page<CapOfferSummary>().SelectCAPOptionFromGrid(CapTypes.OnePerc);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            browser.ClickAction(WorkflowActivities.Cap2Offers.CreditApproval);
            browser.Page<CapOfferSummary>().CompleteCAP2Action();
            //we need to change users now
            var creditBrowser = new TestBrowser(TestUsers.CreditUnderwriter2);
            Helper.LoadCAP2Offer(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.CreditApproval, creditBrowser);
            //approve the case
            creditBrowser.ClickAction(WorkflowActivities.Cap2Offers.GrantCAP2Offer);
            creditBrowser.Page<CapOfferSummary>().CompleteCAP2Action();
            //the case should now be in the Readvance Required state
            CAP2Assertions.AssertCAP2OptionTakenUp(2, CAPOffer.AccountKey);
            CAP2Assertions.AssertX2State(CAPOffer.AccountKey, CAPStatus.ReadvanceRequired);
            CAP2Assertions.AssertStatus(CAPOffer.AccountKey, WorkflowStates.CAP2OffersWF.ReadvanceRequired);
            creditBrowser.Dispose();
            creditBrowser = null;
        }

        public class Cap2TestIdentifiers
        {
            public const string ReadvanceGreaterThan80 = "ReadvanceGreaterThan80";
            public const string FurtherAdvanceCAP1 = "FurtherAdvanceCAP1";
            public const string FurtherAdvanceCAP2 = "FurtherAdvanceCAP2";
            public const string FurtherAdvanceCAP3 = "FurtherAdvanceCAP3";
            public const string FurtherAdvanceCAP4 = "FurtherAdvanceCAP4";
            public const string FurtherAdvanceCAP5 = "FurtherAdvanceCAP5";
            public const string ReadvanceCAP1 = "ReadvanceCAP1";
            public const string ReadvanceCAP2 = "ReadvanceCAP2";
            public const string ReadvanceCAP3 = "ReadvanceCAP3";
            public const string ReadvanceCAP4 = "ReadvanceCAP4";
            public const string ReadvanceCAP5 = "ReadvanceCAP5";
            public const string RecalculateCAP2OfferPostTransaction = "RecalculateCAP2OfferPostTransaction";
            public const string CAPExpiryTest = "CAPExpiryTest";
            public const string CAP2PrintLetter = "CAP2PrintLetter";
            public const string CAP2NTU = "CAP2NTU";
            public const string CAP2Decline = "CAP2Decline";
            public const string VariFixCAP = "VariFixCAP";
            public const string ExistingCAP = "ExistingCAP";
            public const string InterestOnlyCAP = "InterestOnlyCAP";
            public const string ClosedAccountCAP = "ClosedAccountCAP";
            public const string RCSAccountCAP = "RCSAccountCAP";
            public const string UnderCancellationCAP = "UnderCancellationCAP";
            public const string DebtCounsellingCAP = "DebtCounsellingCAP";
            public const string MinBalanceCAP = "MinBalanceCAP";
            public const string OnePercDNQCAP = "OnePercDNQCAP";
            public const string TwoPercDNQCAP = "TwoPercDNQCAP";
            public const string ThreePercDNQCAP = "ThreePercDNQCAP";
            public const string PTIExceededCAP = "PTIExceededCAP";
            public const string CAP2ResetCheck = "CAP2ResetCheck";
            public const string CAP2SPVNotAllowed = "CAP2SPVNotAllowed";
            public const string CAP2SPVNotAllowedFAdvUnderLAA = "CAP2SPVNotAllowedFAdvUnderLAA";
            public const string FurtherAdvanceCAPUnderLAA = "FurtherAdvanceCAPUnderLAA";
            public const string CAP2SPVNotAllowedReadvance = "CAP2SPVNotAllowedReadvance";
            public const string FurtherAdvanceCAPUnderLAA2 = "FurtherAdvanceCAPUnderLAA2";
            public const string FurtherAdvanceCAPOverLAA = "FurtherAdvanceCAPOverLAA";
            public const string LTVExceededCAP = "LTVExceededCAP";
        }

        #endregion HelperMethods
    }
}