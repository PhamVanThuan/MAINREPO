using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;

namespace PersonalLoansTests.RoleSecurityTests
{
    [RequiresSTA]
    internal class AdminSecurityTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        [Test, Sequential]
        public void check_pl_admin_dynamic_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.DocumentsVerified,
                        WorkflowActivities.PersonalLoans.ReturntoLegalAgreements,
                        WorkflowActivities.PersonalLoans.ApplicationinOrder
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.VerifyDocuments,
                        WorkflowStates.PersonalLoansWF.VerifyDocuments,
                        WorkflowStates.PersonalLoansWF.DocumentCheck
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLAdminD);
        }

        [Test, Sequential]
        public void check_pl_admin_static_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.NTU,
                        WorkflowActivities.PersonalLoans.NTU,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.LegalAgreements,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.LegalAgreements,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.DocumentCheck
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLAdmin);
        }
    }
}