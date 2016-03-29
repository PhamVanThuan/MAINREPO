using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class EscalateToExceptionsManager : PersonalLoansWorkflowTestBase<AssignCreditExceptionsManager>
    {
        protected override void OnTestStart()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD, true);
            base.OnTestStart();
        }

        [Test]
        public void when_escalate_to_exceptions_manager_action_performed_should_load_assign_credit_exceptions_manager_view()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.EscalateToExceptionsManager);
            base.View.AssertViewDisplayed();
        }

        [Test]
        public void when_escalate_to_exceptions_manager_action_performed_should_list_credit_exceptions_managers_to_assign()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.EscalateToExceptionsManager);
            List<string> plCreditExceptionsManagers = new List<string>() { @"SAHL\GaneshS", @"SAHL\KeithS" };
            base.View.AssertUsersListOptions(plCreditExceptionsManagers, false);
        }

        [Test]
        public void pl_credit_analyst_should_be_able_to_perform_escalate_to_exceptions_manager_action_at_credit()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.EscalateToExceptionsManager);
            base.View.SelectCreditExceptionsManager(TestUsers.PersonalLoansCreditExceptionsManager1);
            base.View.ClickSubmit();
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.personalLoanApplication.OfferKey, WorkflowRoleTypeEnum.PLCreditExceptionsManagerD, TestUsers.PersonalLoansCreditExceptionsManager1, WorkflowStates.PersonalLoansWF.Credit, Workflows.PersonalLoans);
        }

        [Test]
        public void when_a_case_that_has_been_returned_to_manage_application_by_credit_exceptions_manager_is_returned_back_to_Credit_the_case_is_assigned_to_the_previously_assigned_PL_Credit_Analyst()
        {
            string expectedUser = base.CaseOwner;
            string date = DateTime.Now.ToString(Formats.DateTimeFormatSQL);
            int offerKey = base.personalLoanApplication.OfferKey;
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.EscalateToExceptionsManager);
            base.View.SelectCreditExceptionsManager(TestUsers.PersonalLoansCreditExceptionsManager1);
            base.View.ClickSubmit();
            base.Service<X2WorkflowService>().WaitForX2State(base.personalLoanApplication.OfferKey, Workflows.PersonalLoans, WorkflowStates.PersonalLoansWF.Credit);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditExceptionsManagerD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReturntoManageApplication);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            base.Service<X2WorkflowService>().WaitForX2State(base.personalLoanApplication.OfferKey, Workflows.PersonalLoans, WorkflowStates.PersonalLoansWF.ManageApplication);
            //login as consultant
            base.ReloadCase(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            base.UpdateDeclarations();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentCheck);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            base.Service<X2WorkflowService>().WaitForX2State(base.personalLoanApplication.OfferKey, Workflows.PersonalLoans, WorkflowStates.PersonalLoansWF.DocumentCheck);
            //login as admin
            base.ReloadCase(WorkflowStates.PersonalLoansWF.DocumentCheck, WorkflowRoleTypeEnum.PLAdminD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            base.Service<X2WorkflowService>().WaitForX2State(base.personalLoanApplication.OfferKey, Workflows.PersonalLoans, WorkflowStates.PersonalLoansWF.Credit);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.personalLoanApplication.OfferKey, WorkflowRoleTypeEnum.PLCreditAnalystD, expectedUser, WorkflowStates.PersonalLoansWF.Credit, Workflows.PersonalLoans);
        }
    }
}