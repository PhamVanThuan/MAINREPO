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
    public class PerformFurtherValuationTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.NewBusinessProcessor);
        }

        protected override void OnTestStart()
        {
        }

        [Test, Description("Verify that a New Business Processor can perform the 'Perform Further Valuation' action at 'Manage Application' state")]
        public void PerformFurtherValuation()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.FurtherValuationRequest, 
                1, 1, 1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformFurtherValuation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(5000);
            X2Assertions.AssertX2CloneExists(offerKey.ToString(), WorkflowStates.ApplicationManagementWF.FurtherValuationRequired, Workflows.ApplicationManagement);
            X2Assertions.AssertCurrentValuationsX2State(offerKey, WorkflowStates.ValuationsWF.FurtherValuationRequest);
        }

        [Test, Description("Verify that whilst the ValuationRequired flag is set to FALSE the NBPUser cannot perform the Perform Further Valuation action")]
        public void PerformFurtherValuation_ValuationRequiredFlagFalse()
        {
            // Get an offer at "Manage Application" state where the ValuationRequired flag is set to FALSE
            int offerKey = Service<IX2WorkflowService>().GetWorkflowCaseWithoutBusinessEvent(WorkflowStates.ApplicationManagementWF.ManageApplication,
                Workflows.ApplicationManagement, OfferTypeEnum.SwitchLoan, StageDefinitionStageDefinitionGroupEnum.Credit_ValuationApproved);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformFurtherValuation);
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Valuation is not required");
        }

        [Test, Description("Verify that a notification which reads 'Confirm Perform Further Valuation' is returned if the 'Perform Further Valuation' action is performed on an application which already has more than one valuation. This is to verify that multiple valuations can be performed on an application.")]
        public void PerformFurtherValuation2ndFurtherValuation()
        {
            int offerKey = Service<IX2WorkflowService>().GetAppManOffers_FilterByValuationsAndWorkflowHistory(WorkflowStates.ValuationsWF.FurtherValuationRequest, 
                0, 1, -1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformFurtherValuation);
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Confirm Perform Further Valuation");
        }

        [Test, Description("Verify that a notification which reads 'Confirm Perform Further Valuation' is returned if the 'Perform Further Valuation' action is performed on an application which already has more than one valuation. This is to verify that multiple valuations can be performed on an application.")]
        public void PerformFurtherValuationFurtherValuationRequiredCloneExists()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, null, 0,
                WorkflowStates.ApplicationManagementWF.FurtherValuationRequired, 1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformFurtherValuation);
            base.Browser.Page<BasePageAssertions>().AssertFrameContainsText("Confirm Perform Further Valuation");
        }

        [Test, Description("Verify that no error is returned if the 'Perform Further Valuation' action is performed when an open Valuation exists on a cas.e")]
        public void PerformFurtherValuationValuationHoldExists()
        {
            // Get an offer at "Manage Application" state that does not have an Instance clone at "Valuation Hold" State
            int offerKey = Service<IX2WorkflowService>().GetAppManOfferKey_FilterByClone(WorkflowStates.ApplicationManagementWF.ManageApplication, null, 0,
                WorkflowStates.ApplicationManagementWF.ValuationHold, 1, (int)OfferTypeEnum.NewPurchase);
            base.TestCase = new Automation.DataModels.OriginationTestCase() { OfferKey = offerKey };
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.PerformFurtherValuation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageDoesNotExist("Validation failed with errors");
        }
    }
}