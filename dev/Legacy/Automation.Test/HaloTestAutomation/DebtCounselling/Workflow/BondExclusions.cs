using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class BondExclusions : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        /// <summary>
        /// Performs the exclude bond action ensuring that the case correctly moves states.
        /// </summary>
        [Test, Description("Performs the exclude bond action ensuring that the case correctly moves states.")]
        public void ExcludeBond()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ExcludeBond);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //case moves states
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.BondExclusions);
            //check assignment
            DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(base.TestCase.DebtCounsellingKey, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            //transition
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DebtCounsellingBondExclusion);
        }

        /// <summary>
        /// This test will put the account linked to the debt counselling case into arrears and then run the stored procedure that inserts the
        /// external activity to move the x2 case from Bond Exclusions to Bond Exclusions Arrears.
        /// </summary>
        [Test, Description(@"This test will put the account linked to the debt counselling case into arrears and then run the stored procedure that inserts the
        external activity to move the x2 case from Bond Exclusions to Bond Exclusions Arrears.")]
        public void BondExclusionInArrears()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.BondExclusions, TestUsers.DebtCounsellingConsultant);
            Service<IX2WorkflowService>().InsertActiveExternalActivity(Workflows.DebtCounselling, ExternalActivities.DebtCounselling.ExtIntoBondExclArrears, base.TestCase.InstanceID,
                base.TestCase.DebtCounsellingKey);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.BondExclusionsArrears);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD, true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_BondExclusionArrearsIN);
        }

        /// <summary>
        /// This test will put the account linked to the debt counselling case into arrears and then run the stored procedure that inserts the
        /// external activity to move the x2 case from Bond Exclusions to Bond Exclusions Arrears. Once the case has moved to the Bond Exclusions Arrears
        /// the arrears is then removed and the same stored procedure is run again to move the case back to the Bond Exclusions state.
        /// </summary>
        [Test, Description(@"This test will put the account linked to the debt counselling case into arrears and then run the stored procedure that inserts the
        external activity to move the x2 case from Bond Exclusions to Bond Exclusions Arrears. Once the case has moved to the Bond Exclusions Arrears
        the arrears is then removed and the same stored procedure is run again to move the case back to the Bond Exclusions state.")]
        public void BondExclusionOutOfArrears()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.BondExclusionsArrears, TestUsers.DebtCounsellingConsultant);
            Service<IX2WorkflowService>().InsertActiveExternalActivity(Workflows.DebtCounselling, ExternalActivities.DebtCounselling.ExtOutBondExclArrears, base.TestCase.InstanceID,
                base.TestCase.DebtCounsellingKey);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.BondExclusions);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                        true, true);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_BondExclusionArrearsOUT);
        }
    }
}