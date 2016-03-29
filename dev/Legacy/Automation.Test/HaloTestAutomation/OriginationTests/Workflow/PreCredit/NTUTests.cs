using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace Origination.Workflow.PreCredit
{
    [RequiresSTA]
    public class NTUTests : Origination.OriginationTestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.NewBusinessProcessor);
        }

        protected override void OnTestStart()
        {
            base.GetOfferKeyAtStateAndSearchForCase(WorkflowStates.ApplicationManagementWF.ManageApplication, Common.Enums.OfferTypeEnum.NewPurchase);
        }

        [Test, Description("Verify that a New Business Processor can move a case from 'Manage Application' to the 'NTU' state by performing the NTU action")]
        public void NTUOfferAtManageApplication()
        {
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTU);
            base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            //Assertions
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.NTU);
        }

        [Test, Description(@"Verify that a New Business Processor can  move a case at 'NTU' state to 'Manage Application' state, by performing
		the 'Reinstate NTU' action")]
        public void ReinstateNTU()
        {
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.NTU, base.TestCase.OfferKey);
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.NTU);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ReinstateNTU);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assertions
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.Open);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, true);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
        }

        [Test, Description(@"Verify that a New Business Processor can  move a case at 'NTU' state to 'Archive NTU' state, by performing the
			'NTU Finalised' action")]
        public void NTUFinalised()
        {
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.ApplicationManagement.NTU, base.TestCase.OfferKey);
            base.SearchForCase(WorkflowStates.ApplicationManagementWF.NTU);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.NTUFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //Assertions
            OfferAssertions.AssertOfferStatus(base.TestCase.OfferKey, OfferStatusEnum.NTU);
            OfferAssertions.AssertOfferEndDate(base.TestCase.OfferKey, DateTime.Now, 0, false);
            X2Assertions.AssertCurrentAppManX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationManagementWF.ArchiveNTU);
        }
    }
}