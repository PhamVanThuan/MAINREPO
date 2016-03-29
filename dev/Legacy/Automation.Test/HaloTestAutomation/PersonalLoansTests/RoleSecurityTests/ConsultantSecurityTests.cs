using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using NUnit.Framework;

namespace PersonalLoansTests.RoleSecurityTests
{
    [RequiresSTA]
    internal class ConsultantSecurityTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        [Test, Sequential]
        public void check_pl_consultant_dynamic_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.CalculateApplication,
                        WorkflowActivities.PersonalLoans.SendOffer,
                        WorkflowActivities.PersonalLoans.DocumentCheck,
                        WorkflowActivities.PersonalLoans.DeclineFinalised,
                        WorkflowActivities.PersonalLoans.NTUFinalised,
                        WorkflowActivities.PersonalLoans.ReinstateNTU,
                        WorkflowActivities.PersonalLoans.NTULead,
                        WorkflowActivities.PersonalLoans.AdminDecline
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.ManageLead,
                        WorkflowStates.PersonalLoansWF.ManageApplication,
                        WorkflowStates.PersonalLoansWF.ManageApplication,
                        WorkflowStates.PersonalLoansWF.DeclinedbyCredit,
                        WorkflowStates.PersonalLoansWF.NTU,
                        WorkflowStates.PersonalLoansWF.NTU,
                        WorkflowStates.PersonalLoansWF.ManageLead,
                        WorkflowStates.PersonalLoansWF.ManageApplication
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLConsultantD);
        }

        [Test, Sequential]
        public void check_pl_consultant_static_workflow_access(
                [Values(
                        WorkflowActivities.PersonalLoans.ReworkApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.ReturntoManageApplication,
                        WorkflowActivities.PersonalLoans.NTU,
                        WorkflowActivities.PersonalLoans.SendDocuments
                        )] string activity,
                [Values(
                        WorkflowStates.PersonalLoansWF.ManageApplication,
                        WorkflowStates.PersonalLoansWF.Credit,
                        WorkflowStates.PersonalLoansWF.DeclinedbyCredit,
                        WorkflowStates.PersonalLoansWF.Disbursement,
                        WorkflowStates.PersonalLoansWF.LegalAgreements,
                        WorkflowStates.PersonalLoansWF.ManageApplication,
                        WorkflowStates.PersonalLoansWF.LegalAgreements
                        )] string state)
        {
            X2Assertions.AssertUserRoleSecurity(Workflows.PersonalLoans, activity, state, X2SecurityGroups.PLConsultant);
        }
    }
}