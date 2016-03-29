using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class ReallocateUserTests : TestBase<DebtCounsellingAssignSupervisor>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
        }

        /// <summary>
        /// This will test that when performing the common reallocate action the
        /// case is assigned to the selected user and a stage transition is written
        /// </summary>
        [Test, Description(@"This will test that when performing the common reallocate action the
		case is assigned to the selected user and a stage transition is written")]
        public void CommonReallocateToUser()
        {
            //search for a case at DecisiononProposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.DecisiononProposal, TestUsers.DebtCounsellingSupervisor);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ReallocateUser);
            base.View.AssignToUser(TestUsers.DebtCounsellingManager, ButtonTypeEnum.Submit);
            //Assert that still at the same state
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.DecisiononProposal);
            //Assert workflow assignment
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, TestUsers.DebtCounsellingManager,
                            WorkflowRoleTypeEnum.RecoveriesManagerD, true, true);
            //Assert StageTransition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ReallocateUser);
        }
    }
}