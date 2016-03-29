using Automation.DataAccess;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace Origination.Workflow.PreCredit
{
    [RequiresSTA]
    public class DeclineTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.NewBusinessProcessor);
        }
        protected override void OnTestStart()
        {
        }
        [Test, Description("Verify that a New Business Processor can Decline an Application at 'Manage Application' state")]
        public void DeclineApplicationConsultant()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.Decline);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
            DateTime actionDate = DateTime.Now;
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, actionDate, 0, false);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.Decline);
        }

        [Test, Description("Verify that a New Business Processor can finalise a decline on an Application")]
        public void FinaliseDeclineConsultant()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.Decline, base.TestCase.OfferKey);
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.Decline);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.DeclineFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.ArchiveDecline);
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Declined);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
        }

        [Test, Description("Verify that a New Business Processor can reinstate a decline on an Application")]
        public void ReinstateDecline()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication, OfferTypeEnum.NewPurchase);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.Decline, base.TestCase.OfferKey);
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.Decline);
            //fetch the previous state
            QueryResults results = Service<IX2WorkflowService>().GetAppManInstanceDetails(base.TestCase.OfferKey);
            string prevState = results.Rows(0).Column("PreviousState").Value;
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateDecline);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, prevState);
            //Assert Offer Status = Open
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Open);
            //Assert OfferEndDate is back to NULL
            DateTime date = DateTime.Now;
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, date, 0, true);
        }
    }
}