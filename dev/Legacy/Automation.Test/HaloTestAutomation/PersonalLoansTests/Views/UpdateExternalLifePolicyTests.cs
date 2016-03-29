using BuildingBlocks.Navigation.FLOBO.PersonalLoan;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Extensions;
using NUnit.Framework;

namespace PersonalLoansTests.Views
{
    [TestFixture, RequiresSTA]
    public class UpdateExternalLifePolicyTests : PersonalLoansWorkflowTestBase<PersonalLoanMaintainExternalLifePolicy>
    {
        [Test]
        public void when_personal_loan_case_is_still_a_lead_and_the_user_tries_to_view_external_life_policy_add_screen_they_are_redirected_to_a_blank_summary_screen()
        {
            var state = WorkflowStates.PersonalLoansWF.ManageLead;
            var roleToLoginAs = Common.Enums.WorkflowRoleTypeEnum.PLConsultantD;
            base.FindAndLoadPersonalLoanApplication(state, roleToLoginAs, hasSAHLLife: false, hasExternalLife: false);
            NavigateToUpdateExternalLifePolicy();
            base.Browser.Page<PersonalLoanExternalLifePolicySummary>().AssertBlankForm();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Life Policy cannot be captured for a lead.");
        }

        [Test]
        public void when_sahl_life_is_selected_and_the_user_tries_to_view_the_external_life_policy_add_screen_they_are_redirected_to_the_summary_screen_with_a_warning()
        {
            FindOrCreateCaseAtManageApplicationState(hasSAHLLife: true, hasExternalLife: false);
            NavigateToUpdateExternalLifePolicy();
            base.Browser.Page<PersonalLoanExternalLifePolicySummary>().AssertSAHLLifeForm();
        }

        [Test]
        public void when_sahl_life_is_not_selected_the_user_can_view_the_external_life_policy_add_screen()
        {
            FindOrCreateCaseAtManageApplicationState(hasSAHLLife: false, hasExternalLife: false);
            NavigateToUpdateExternalLifePolicy();
            var externalLife = base.CreateDefaultExternalCreditLife();
            base.View.AssertFieldsExist();
        }

        [Test]
        public void when_external_life_policy_is_added_the_user_is_redirected_to_the_summary_screen()
        {
            FindOrCreateCaseAtManageApplicationState(hasSAHLLife: false, hasExternalLife: false);
            NavigateToUpdateExternalLifePolicy();
            var externalLife = base.CreateDefaultExternalCreditLife();
            View.AddPolicy(externalLife);
            base.Browser.Page<PersonalLoanExternalLifePolicySummary>().AssertFieldValues(externalLife);
        }

        public void NavigateToUpdateExternalLifePolicy()
        {
            base.Browser.Navigate<ExternalLifeNode>().ClickLifeNode();
            base.Browser.Navigate<ExternalLifeNode>().ClickUpdateExternalLifePolicyNode();
        }

        private void FindOrCreateCaseAtManageApplicationState(bool hasSAHLLife, bool hasExternalLife)
        {
            var roleToLoginAs = Common.Enums.WorkflowRoleTypeEnum.PLConsultantD;
            base.FindAndLoadPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageApplication, roleToLoginAs, hasSAHLLife, hasExternalLife);
            if (base.GenericKey == 0)
            {
                base.FindPersonalLoanApplication(WorkflowStates.PersonalLoansWF.ManageLead, roleToLoginAs, hasSAHLLife: false, hasExternalLife: false);
                var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "CalculateApplicationWithNoLifeCover", base.GenericKey);
                if (results.LastActivitySucceeded())
                {
                    if (hasExternalLife)
                    {
                        var externalLife = base.CreateDefaultExternalCreditLife();
                        Service<IPersonalLoanService>().AddExternalLife(base.GenericKey, externalLife);
                    }
                    if (hasSAHLLife)
                    {
                        Service<IPersonalLoanService>().AddSAHLLife(base.GenericKey);
                    }
                    base.ReloadCase(WorkflowStates.PersonalLoansWF.ManageApplication, roleToLoginAs);
                }
                else
                    throw new System.Exception("Failed to find a case");
            }
        }
    }
}