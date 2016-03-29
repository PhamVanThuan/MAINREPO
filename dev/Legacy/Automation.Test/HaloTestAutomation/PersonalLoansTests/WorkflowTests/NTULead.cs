using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class NTULead : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        #region Test Setup/Teardown

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageLead, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.NTULead);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Service<IWatiNService>().KillAllIEProcesses();
        }

        #endregion Test Setup/Teardown

        #region Tests

        /// <summary>
        /// This is a test to verify that when a Personal Loan Consultant/Supervisor NTU's a Lead, a confirm
        /// prompt is displayed and if 'Yes' is selected, the case is archived and removed from the worklist.
        /// </summary>
        [Test, Description(@"This is a test to verify that when a Personal Loan Consultant/Supervisor NTU's a Lead, a confirm prompt is displayed and if 'Yes' is selected, the case is archived and removed from the worklist.")]
        public void VerifyThatNTUdCaseIsArchivedAndRemovedFromWorklistWhenYesIsSelected()
        {
            // Click 'Yes' in the confirm option
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            // Assert that the case is archived and removed from the worklist.
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveNTULead);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_NTULead);
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// This is a test to verify that when a Personal Loan Consultant/Supervisor NTU's a Lead, a confirm
        /// prompt is displayed and if 'No' is selected, the case remains on the worklist at stage 'Manage Lead'.
        /// </summary>
        [Test, Description(@"This is a test to verify that when a Personal Loan Consultant/Supervisor NTU's a Lead, a confirm prompt is displayed and if 'No' is selected, the case remains on the worklist at stage 'Manage Lead'.")]
        public void VerifyThatNTUdCaseRemainsAtStageManagedLeadWhenNoIsSelected()
        {
            // Click 'No' in the confirm option
            base.Browser.Page<WorkflowYesNo>().Confirm(false, false);

            // Assert that the case is remains on the worklist at stage 'Managed Lead'.
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageLead);
            Assert.That(offerExists);
        }

        #endregion Tests
    }
}