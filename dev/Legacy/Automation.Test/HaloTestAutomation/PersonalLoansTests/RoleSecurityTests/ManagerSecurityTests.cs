using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;

namespace PersonalLoansTests.RoleSecurityTests
{
    [RequiresSTA]
    internal class ManagerSecurityTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        [Test, Sequential]
        public void check_pl_manager_static_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.DisburseFunds,
                        WorkflowActivities.PersonalLoans.NTUFinalised,
                        WorkflowActivities.PersonalLoans.ReinstateNTU,
                        WorkflowActivities.PersonalLoans.RollbackDisbursement
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.NTU,
                        WorkflowStates.PersonalLoansWF.NTU,
                        WorkflowStates.PersonalLoansWF.Disbursed
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLManager);
        }
    }
}