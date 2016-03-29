using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;

namespace PersonalLoansTests.RoleSecurityTests
{
    [RequiresSTA]
    internal class CreditAnalystSecurityTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        [Test, Sequential]
        public void check_pl_credit_analyst_static_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.Decline,
                        WorkflowActivities.PersonalLoans.Approve,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.Credit,
                        WorkflowStates.PersonalLoansWF.Credit,
                        WorkflowStates.PersonalLoansWF.Credit,
                        WorkflowStates.PersonalLoansWF.DeclinedbyCredit
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLCreditAnalyst);
        }
    }
}