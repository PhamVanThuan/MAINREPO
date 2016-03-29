using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Threading;

namespace Origination.Workflow.PreCredit
{
    [RequiresSTA]
    public class ReviewValuationRequiredTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.NewBusinessProcessor);
        }

        protected override void OnTestStart()
        {
        }
        [Test, Description("Verify that a New Business Processor can perform the 'Review Valuation Required' action at 'Manage Application' state")]
        public void ReviewValuationRequired()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.ValuationReviewRequest, 1, 1, 1,
                (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewValuationRequired);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(5000);
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.ValuationReviewRequired, Workflows.ApplicationManagement);
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.ValuationReviewRequest);
        }

        [Test, Description("Verify that a Notification is returned if the 'Review Valuation Required' action is performed on a case that has already had a Valuation Review in progress")]
        public void ReviewValuationRequired_ValuationReviewRequiredCloneExists()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, null, 0,
                WorkflowStates.ApplicationManagementWF.ValuationReviewRequired, 1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewValuationRequired);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                new System.Text.RegularExpressions.Regex("Application:[0-9]* ReservedAccountKey:[0-9]* in Valuations is not complete. Is at State:"));
        }

        [Test, Description("Verify that an error is returned if the 'Review Valuation Required' action is performed when an open Valuation exists on a case")]
        public void ReviewValuationRequired_ValuationHoldExists()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, null, 0,
                WorkflowStates.ApplicationManagementWF.ValuationHold, 1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewValuationRequired);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                new System.Text.RegularExpressions.Regex("Application:[0-9]* ReservedAccountKey:[0-9]* in Valuations is not complete. Is at State:"));
        }
    }
}