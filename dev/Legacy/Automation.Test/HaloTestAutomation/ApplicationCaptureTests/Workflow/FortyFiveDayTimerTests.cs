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
    public class FortyFiveDayTimerTests : TestBase<BasePage>
    {
        [Test, Description("This test ensures that the scheduled activity has been created and will update the timer")]
        public void _01_FortyFiveDayTimerExists()
        {
            base.GetTestCase("ArchiveTimer", true);
            //assert that the scheduled activity has been set up, 45 days from the offer start date
            var offerData = Service<IApplicationService>().GetOfferData(base.TestCase.OfferKey);
            DateTime offerStartDate = offerData.Rows(0).Column("OfferStartDate").GetValueAs<DateTime>();
            X2Assertions.AssertScheduledActivityTimer(base.TestCase.OfferKey.ToString(), ScheduledActivities.ApplicationCapture._45daytimer, 45, false, offerStartDate);
            //we now need to update the timer so that a later test case can assert it archives the case
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, WorkflowAutomationScripts.ApplicationCapture.Fire45DayTimer, base.TestCase.OfferKey);
        }

        /// <summary>
        /// Ensures that the 45 Day Timer has correctly archived our case
        /// </summary>
        [Test, Description("Ensures that the 45 Day Timer has correctly archived our case")]
        public void _02_FortyFiveDayTimerArchive()
        {
            //see _0003_FortyFiveDayTimer() for preliminary test steps
            base.GetTestCase("ArchiveTimer", true);
            var instanceID = Service<IX2WorkflowService>().GetAppCapInstanceDetails(base.TestCase.OfferKey).Rows(0).Column("InstanceID").GetValueAs<int>();
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationCapture.FortyFiveDayTimer, instanceID, 1);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ApplicationNotAccepted);
        }
    }
}