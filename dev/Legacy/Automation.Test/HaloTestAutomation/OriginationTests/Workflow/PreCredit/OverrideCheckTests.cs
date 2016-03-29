using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace Origination.Workflow.PreCredit
{
    [RequiresSTA]
    public class OverrideCheckTests : OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.NewBusinessManager);
        }
        protected override void OnTestStart()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication, Common.Enums.OfferTypeEnum.SwitchLoan);
        }

        [Test, Description(@"Verify that a New Business Manager can perform the 'Override Check' action which will move the application
		to the 'Credit' state")]
        public void OverrideCheck_Credit()
        {
            PerformOverrideCheck(true);
        }

        [Test, Description(@"Verify that a New Business Manager can perform the 'Override Check' action which will move the application
		to the 'Credit' state")]
        public void OverrideCheck_ValuationRequired()
        {
            PerformOverrideCheck(false);
        }

        private void PerformOverrideCheck(bool valuationRequired)
        {
            string expectedEndState = !valuationRequired ? WorkflowStates.CreditWF.ValuationApprovalRequired : WorkflowStates.CreditWF.Credit;
            Service<IPropertyService>().DBUpdateDeedsOfficeDetails(base.TestCase.OfferKey);
            Service<IX2WorkflowService>().SetIsValuationRequiredIndicator(valuationRequired, base.TestCase.InstanceID);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.OverrideCheck);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForCreditCaseCreate(base.TestCase.InstanceID, base.TestCase.OfferKey, expectedEndState);
            X2Assertions.AssertCurrentCreditX2State(base.TestCase.OfferKey, expectedEndState);
        }
    }
}