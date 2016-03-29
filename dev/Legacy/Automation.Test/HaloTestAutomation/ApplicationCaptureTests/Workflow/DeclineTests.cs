using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class DeclineTests : ApplicationCaptureTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        [Test, Description("Data setup test for the Decline Timer on an Application")]
        public void _01_DeclineTimeoutApplication()
        {
            base.GetTestCase("DeclineTimeOutApplication");
            base.SearchForCase();
            //Decline the Application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.Decline);
            base.View.SelectReasonAndSubmit(ReasonType.BranchDecline);
            Browser.WaitForComplete();
            //Step 4: Assert that the workflow case is the Decline State, the OfferStatus = Decline and the OfferEndDate is set
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.Decline);
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.OfferKey.ToString(), ScheduledActivities.ApplicationCapture.DeclineTimeout, 30, false);
            //if timer exists fire it
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.FireDeclineTimeoutTimer, base.TestCase.OfferKey);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ArchiveDecline);
            //Assert the end date has been populated
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            //Assert the offer status is correct
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
        }

        /// <summary>
        /// Verify that a Branch Consultant can Decline an Application
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can Decline an Application")]
        public void _02_DeclineApplicationConsultant()
        {
            base.GetTestCase("DeclineApplicationConsultant");
            base.SearchForCase();
            //Perform the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.Decline);
            //Step 3: Decline the case
            base.View.SelectReasonAndSubmit(ReasonType.BranchDecline);
            Browser.WaitForComplete();
            //Step 4: Assert that the workflow case is the Decline State, the OfferStatus = Decline and the OfferEndDate is set
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
            //Assert that the OfferEndDate has been populated
            DateTime actionDate = DateTime.Now;
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, actionDate, 0, false);
            //Assert that the Case is in the Decline state
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.Decline);
        }

        /// <summary>
        /// Verify that a Branch Consultant can finalise a decline on an Application
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can finalise a decline on an Application")]
        public void _03_FinaliseDeclineConsultant()
        {
            base.GetTestCase("FinaliseDeclineConsultant");
            base.SearchForCase();
            //Step 2: Perform the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.Decline);
            //Step 3: Decline the case
            base.View.SelectReasonAndSubmit(ReasonType.BranchDecline);
            //Step 4: Finalise the Decline
            Browser.WaitForComplete();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.DeclineFinalised);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Step 5: Assert that the case is archived
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ArchiveDecline);
            //Assert the offer status is correct
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
            //Assert the end date has been populated
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
        }

        /// <summary>
        /// Verify that a Branch Consultant can reinstate a decline on an Application
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can reinstate a decline on an Application")]
        public void _04_ReinstateDeclineConsultant()
        {
            base.GetTestCase("ReinstateDeclineConsultant");
            base.SearchForCase();
            //Step 2: Perform the action
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.Decline);
            //Step 3: Decline the case
            base.View.SelectReasonAndSubmit(ReasonType.BranchDecline);
            //Step 4: Reinstate the Decline
            Browser.WaitForComplete();
            //fetch the previous state
            var results = Service<IX2WorkflowService>().GetAppCapInstanceDetails(base.TestCase.OfferKey);
            string prevState = results.Rows(0).Column("Last_State").Value;
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReactivateDecline);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Step 5: Assert that the case is at the previous state, offer status is open and the OfferEndDate is NULL
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, prevState);
            //Assert Offer Status = Open
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Open);
            //Assert OfferEndDate is back to NULL
            DateTime date = DateTime.Now;
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, date, 0, true);
        }
    }
}