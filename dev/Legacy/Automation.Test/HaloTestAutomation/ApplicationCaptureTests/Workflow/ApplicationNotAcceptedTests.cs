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
    public class ApplicationNotAcceptedTests : ApplicationCaptureTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
        }

        /// <summary>
        /// Creates an application for the Application Not Accepted Timeout test
        /// </summary>
        [Test, Description("Data setup script for AppNotAcceptedArchiveApplicationLead() test")]
        public void _01_AppNotAcceptedArchiveAppLead()
        {
            base.GetTestCase("AppNotAcceptedArchiveApplicationLead", true);
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ApplicationNotAccepted);
            base.View.SelectReasonAndSubmit(ReasonType.LeadNTU);
            X2Assertions.AssertScheduleActivitySetup(base.TestCase.OfferKey.ToString(), ScheduledActivities.ApplicationCapture.ArchiveApplication, 30, 0, 0);
            //fire the timer
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.FireArchiveApplicationTimer, base.TestCase.OfferKey);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationNotAccepted);
        }

        /// <summary>
        /// Verify that a Branch Consultant can move a case from 'Manage Lead' to the 'Application Not Accepted Hold' state by performing the AppNotAccepted action
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can move a case from 'Manage Lead' to the 'Application Not Accepted Hold' state by performing the AppNotAccepted action")]
        public void _02_AppNotAcceptedLead()
        {
            base.GetTestCase("AppNotAcceptedLead", true);
            base.SearchForCase();
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ApplicationNotAccepted);
            base.View.SelectReasonAndSubmit(ReasonType.LeadNTU);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            AssignmentAssertions.AssertOfferExistsOnWorkList(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.AppNotAcceptedHold, base.TestCase.Username, Workflows.ApplicationCapture);
        }

        /// <summary>
        /// Verify that a Branch Consultant can  move a case at 'Application Not Accepted Hold' state to 'Manage Lead' state, by performing the 'App Not Accepted Finalised' action
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can  move a case at 'Application Not Accepted Hold' state to 'Manage Lead' state, by performing the 'App Not Accepted Finalised' action")]
        public void _03_ReactivateAppNotAcceptedLead()
        {
            base.GetTestCase("ReactivateAppNotAcceptedLead", true);
            base.SearchForCase();
            //NTU Application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ApplicationNotAccepted);
            base.View.SelectReasonAndSubmit(ReasonType.LeadNTU);
            base.SearchForCase();
            //Reactivate NTU'd Application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReactivateAppNotAccepted);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.OfferKey, Workflows.ApplicationCapture, WorkflowStates.ApplicationCaptureWF.ManageLead);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Open);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, true);
            AssignmentAssertions.AssertOfferExistsOnWorkList(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ManageLead, base.TestCase.Username, Workflows.ApplicationCapture);
        }

        /// <summary>
        /// Verify that a Branch Consultant can perform the 'App Not Accepted Finalised' action for a case at 'Application Not Accepted Hold' state, which sets the Offer Status to NTU'd
        /// </summary>
        [Test, Description("Verify that a Branch Consultant can perform the 'App Not Accepted Finalised' action for a case at 'Application Not Accepted Hold' state, which sets the Offer Status to NTU'd")]
        public void _04_AppNotAcceptedFinalisedLead()
        {
            base.GetTestCase("AppNotAcceptedFinalisedLead", true);
            base.SearchForCase();
            //NTU Application
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ApplicationNotAccepted);
            base.View.SelectReasonAndSubmit(ReasonType.LeadNTU);
            base.SearchForCase();
            //NTU Finalise
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.AppNotAcceptedFinalised);
            Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assert that Offer.OfferStatusKey = 4 (NTU)
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationNotAccepted);
        }
    }
}