using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;

namespace ApplicationCaptureTests.Workflow
{
    [RequiresSTA]
    public class EstateAgentConsultantTests : ApplicationCaptureTests.TestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.EstateAgentConsultant1);
        }

        /// <summary>
        /// Verify that an EsateAgent consultant can reassign a Lead
        /// </summary>
        [Test, Description("Verify that an EsateAgent can reassign a Lead")]
        public void _010_ReAssignEstateAgentLead()
        {
            base.GetTestCase("ReAssignEstateAgentLead", true);
            SearchForCase();
            //Step 2: Select the action.
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.ReAssign);
            //Step 3: Select a role and user from the dropdown.
            Browser.Page<WF_ReAssign>().SelectUserRoleAndConsultantFromDropdownAndCommit(base.TestCase.Username, base.TestCase.ReAssignedConsultant);
            //Step 4: Assert that the the case is with consultant.
            string adusernamewithoutdomain = base.TestCase.ReAssignedConsultant.RemoveDomainPrefix();
            OfferAssertions.AssertOfferRoleCreatedAndAssignedToADUser(base.TestCase.OfferKey, adusernamewithoutdomain, OfferRoleTypeEnum.BranchConsultantD, 545);
            //Step 5: Assert that the case have the correct worklfow assignment record.
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
            //Step 6: Assert that the case have an active worklfow assignment record.
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(base.TestCase.OfferKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Verify that an Esate Agency can be assigned to a Lead using the Assign Estate Agent Lead action
        /// </summary>
        [Test, Description("Verify that an EsateAgent can be assigned to a Lead")]
        public void _011_AssignEstateAgentLead()
        {
            base.GetTestCase("AssignEstateAgentLead", true);
            SearchForCase();
            //Step 2: Select the assign estate agent action action.
            Browser.ClickAction(WorkflowActivities.ApplicationCapture.AssignEstateAgentLead);
            //Step 3: Select an estate agency from the dropdown.
            string estateAgencyName = Service<ILegalEntityService>().GetActiveEstateAgencyTradingName();
            Browser.Page<App_AssignEstateAgent>().CaptureEstateAgency(estateAgencyName);
            //Pause for a while so that the HALO app can finish writing evrything to the database
            //Step 4: Assert that the case is in the manage lead state.
            X2Assertions.AssertCurrentAppCapX2State(base.TestCase.OfferKey, WorkflowStates.ApplicationCaptureWF.ManageLead);
            //Step 5: Assert that the case have an estate agency offerrole inserted
            AssignmentAssertions.AssertOfferRoleRecordExists(base.TestCase.OfferKey, OfferRoleTypeEnum.EstateAgentChannel);
        }
    }
}