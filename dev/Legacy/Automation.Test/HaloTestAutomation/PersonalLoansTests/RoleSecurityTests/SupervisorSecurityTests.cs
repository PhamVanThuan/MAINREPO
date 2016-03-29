using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;

namespace PersonalLoansTests.RoleSecurityTests
{
    [RequiresSTA]
    internal class SupervisorSecurityTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        [Test, Sequential]
        public void check_pl_supervisor_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.CalculateApplication,
                        WorkflowActivities.PersonalLoans.SendOffer,
                        WorkflowActivities.PersonalLoans.ApplicationinOrder,
                        WorkflowActivities.PersonalLoans.SendDocuments,
                        WorkflowActivities.PersonalLoans.DocumentsVerified,
                        WorkflowActivities.PersonalLoans.DisburseFunds,
                        WorkflowActivities.PersonalLoans.NTU,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.RollbackDisbursement,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.ManageLead,
                        WorkflowStates.PersonalLoansWF.ManageApplication,
                        WorkflowStates.PersonalLoansWF.DocumentCheck,
                        WorkflowStates.PersonalLoansWF.LegalAgreements,
                        WorkflowStates.PersonalLoansWF.VerifyDocuments,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.Disbursed,
                        WorkflowStates.PersonalLoansWF.DocumentCheck
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLSupervisor);
        }
    }
}