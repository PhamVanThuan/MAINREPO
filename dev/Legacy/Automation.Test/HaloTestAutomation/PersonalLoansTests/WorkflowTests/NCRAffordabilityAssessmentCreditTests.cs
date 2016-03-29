using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class NCRAffordabilityAssessmentCreditTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        private int OfferKey { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
            OfferKey = base.GenericKey;
        }

        [Test, Description("Checks that the expected values are written to the database")]
        public void _01_WhenUpdatingAffordabilityAssessmentExpectedValuesAreWrittenToTheDB()
        {
            base.Service<IApplicationService>().InsertAffordabilityAssessment((int)AffordabilityAssessmentStatusKey.Unconfirmed, OfferKey);

            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(OfferKey);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();

            var affordabilityAssessmentKey = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).FirstOrDefault().AffordabilityAssessmentKey;
            string affordabilityAssessmentContributors = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).ToList().FirstOrDefault().Column("AffordabilityAssessmentContributors").Value;
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessment(affordabilityAssessmentContributors);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickUpdateAffordabilityAssessment();

            int expected_income_field_value = 10000;
            int expected_expense_field_value = 1000;

            // All total values are calculated by multiplying the expected field value by the number of fields
            int expected_income_total = ((expected_income_field_value * 6) - expected_expense_field_value);
            int expected_expenses_total = expected_expense_field_value * 7;
            int expected_obligations_total = expected_expense_field_value * 6;
            int expected_otherExpenses_total = expected_expense_field_value * 6;
            int expected_consolidate_total = expected_expense_field_value * 6;

            int expected_total_expenses = expected_expenses_total + expected_obligations_total + (expected_expense_field_value * 2) + expected_otherExpenses_total - expected_consolidate_total;
            int expected_surplus = (expected_income_total - expected_total_expenses);
            int expected_surplus_to_net_percentage = (Int32)Math.Round((double)(expected_surplus * 100) / expected_income_total);

            base.Browser.Page<UpdateAffordabilityAssessment>().PopulateCreditFields(expected_income_field_value, expected_expense_field_value);
            base.Browser.Page<UpdateAffordabilityAssessment>().SetCommentFields();
            base.Browser.Page<UpdateAffordabilityAssessment>().ClickSave();
            base.Browser.Page<UpdateAffordabilityAssessment>().IgnoreWarningsAndContinue();

            var affordabilityAssessmentItems = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(affordabilityAssessmentKey);

            foreach (var item in affordabilityAssessmentItems)
            {
                if (item.AffordabilityAssessmentItemCategoryKey == 1)
                {
                    Assert.AreEqual(expected_income_field_value, item.CreditValue);
                }
                else
                {
                    Assert.AreEqual(expected_expense_field_value, item.CreditValue);
                }
            }

            Assert.AreEqual(expected_income_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetCreditNetIncome());
            Assert.AreEqual(expected_expenses_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetCreditMonthlyTotalExpenses());
            Assert.AreEqual(expected_obligations_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetPaymentObligationsCreditTotal());
            Assert.AreEqual(expected_otherExpenses_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetOtherExpensesMonthlyCreditTotal());
            Assert.AreEqual(expected_consolidate_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetPaymentObligationDebtToConsolidateTotal());

            Assert.AreEqual(expected_income_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryNetIncome());
            Assert.AreEqual(expected_total_expenses, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryTotalExpenses());
            Assert.AreEqual(expected_surplus, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummarySurplus_Deficit());
            Assert.AreEqual(expected_surplus_to_net_percentage, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryNetHouseholdIncomePercentage());
        }

        [Test, Description("When submitting an application with an unconfirmed assessment out of credit, assessment status whould change to confirmed")]
        public void _02_WhenConfirmingAnAffordabilityAssessment()
        {
            string expectedUser = base.Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);

            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(OfferKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ConfirmAffordabilityAssessment);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);

            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Confirmed);

            Assert.That(Convert.ToInt32(results.FirstOrDefault().AffordabilityAssessmentStatusKey).Equals((int)AffordabilityAssessmentStatusKey.Confirmed));
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_AffordabilityAssessment_Confirm_Affordability);
        }
    }
}