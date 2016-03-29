using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace PersonalLoansTests.Views
{
    [RequiresSTA]
    public class NCRAffordabilityAssessmentTests : PersonalLoansWorkflowTestBase<NCRAffordabilityDetailsUpdate>
    {
        private int OfferKey { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD, true);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();
            OfferKey = base.GenericKey;
        }

        [Test]
        public void _001_WhenCapturingAnAffordabilityAssessmentWithMultipleApplicants()
        {
            int assessmentCount = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).Count();

            DeleteAssessments(assessmentCount);

            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAddAffordabilityAssessment();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(OfferKey);
            int householdDependantCount = 2;
            int contributingApplicantsCount = 2;
            string idNumber = IDNumbers.GetNextIDNumber();

            base.Browser.Page<CaptureAffordabilityAssessment>().AddNonApplicantIncomeContributor(legalEntityKey);
            base.Browser.Page<CaptureAffordabilityAssessment>().ClickAddContributorButton();
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity(
                               false, idNumber, OfferRoleTypes.LeadMainApplicant, false, "Mr", "T", "Test", "AffordabilityContributor", "Test", Gender.Male,
                               MaritalStatus.Single, PopulationGroup.Coloured, Education.Matric, CitizenType.SACitizen, "", "", Language.English, Language.English, "Alive", "031",
                               "3003001", "", "", "", "", "", "", true, true, true, true, true, "", Service<ICommonService>().GetDateOfBirthFromIDNumber(IDNumbers.GetNextIDNumber())
                           );
            base.Browser.Page<LegalEntityRelationshipsAffordabilityAssessment>().SelectRelationshipType(RelationshipType.CloseRelative);
            base.Browser.Page<LegalEntityRelationshipsAffordabilityAssessment>().ClickAddButton();

            base.Browser.Page<CaptureAffordabilityAssessment>().SelectIncomeAndNonIncomeContributorsByLegalEntityKey(legalEntityKey);

            base.Browser.Page<CaptureAffordabilityAssessment>().AddHouseholdDependants(householdDependantCount);
            base.Browser.Page<CaptureAffordabilityAssessment>().SelectIncomeAndNonIncomeContributorsByLegalEntityKey(base.Service<ILegalEntityService>().GetLegalEntity(idNumber).LegalEntityKey);
            base.Browser.Page<CaptureAffordabilityAssessment>().ClickSaveButton();

            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed);

            Assert.That(Convert.ToInt32(results.FirstOrDefault().NumberOfContributingApplicants).Equals(contributingApplicantsCount));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().NumberOfHouseholdDependants).Equals(householdDependantCount));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().AffordabilityAssessmentStatusKey).Equals((int)AffordabilityAssessmentStatusKey.Unconfirmed));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().GeneralStatusKey).Equals((int)GeneralStatusEnum.Active));
            LegalEntityRelationshipAssertions.AssertRelationshipExists(idNumber, RelationshipType.CloseRelative, legalEntityKey);
        }

        [Test]
        public void _002_WhenUpdatingAffordabilityAssessmentExpectedValuesAreWrittenToTheDB()
        {
            var affordabilityAssessmentKey = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).FirstOrDefault().AffordabilityAssessmentKey;
            string affordabilityAssessmentContributors = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).ToList().FirstOrDefault().Column("AffordabilityAssessmentContributors").Value;
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessment(affordabilityAssessmentContributors);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickUpdateAffordabilityAssessment();

            int expected_income_field_value = 10000;
            int expected_expense_field_value = 1000;

            // All total values are calculated by multiplying the expected field value by the number of fields
            int expected_income_total = ((expected_income_field_value * 6) - expected_expense_field_value);
            int expected_expenses_total = (expected_expense_field_value * 7);
            int expected_obligations_total = expected_expense_field_value * 6;
            int expected_otherExpenses_total = expected_expense_field_value * 6;

            int expected_total_expenses = expected_expenses_total + expected_obligations_total + (expected_expense_field_value * 2) + expected_otherExpenses_total;
            int expected_surplus = (expected_income_total - expected_total_expenses);
            int expected_surplus_to_net_percentage = (Int32)Math.Round((double)(expected_surplus * 100) / expected_income_total);

            base.Browser.Page<UpdateAffordabilityAssessment>().PopulateClientFields(expected_income_field_value, expected_expense_field_value);
            base.Browser.Page<UpdateAffordabilityAssessment>().ClickSave();

            var affordabilityAssessmentItems = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentItemsByAffordabilityAssessmentKey(affordabilityAssessmentKey);

            foreach (var item in affordabilityAssessmentItems)
            {
                if (item.AffordabilityAssessmentItemCategoryKey == 1)
                {
                    Assert.AreEqual(expected_income_field_value, item.ClientValue);
                }
                else
                {
                    Assert.AreEqual(expected_expense_field_value, item.ClientValue);
                }
            }

            Assert.AreEqual(expected_income_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetClientNetIncome());
            Assert.AreEqual(expected_expenses_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetClientMonthlyTotalExpenses());
            Assert.AreEqual(expected_obligations_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetPaymentObligationsClientTotal());
            Assert.AreEqual(expected_otherExpenses_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetOtherExpensesMonthlyClientTotal());
            Assert.AreEqual(expected_income_total, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryNetIncome());
            Assert.AreEqual(expected_total_expenses, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryTotalExpenses());
            Assert.AreEqual(expected_surplus, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummarySurplus_Deficit());
            Assert.AreEqual(expected_surplus_to_net_percentage, base.Browser.Page<UpdateAffordabilityAssessment>().GetSummaryNetHouseholdIncomePercentage());
        }

        [Test]
        public void _003_WhenDeletingAnAffordabilityAssessmentExpectedAssessmentsAreDeleted()
        {
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();
            int assessmentCount = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).Count();            
            DeleteAssessments(assessmentCount);
            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed);

            Assert.True(results.Count() == 0);
        }

        private void DeleteAssessments(int assessmentCount)
        {
            for (int i = 0; i < assessmentCount; i++)
            {
                base.Browser.Navigate<AffordabilityAssessmentNode>().ClickDeleteAffordabilityAssessment();
                base.Browser.Page<CaptureAffordabilityAssessment>().ClickDeleteAssessmentButton();
            }
        }
    }
}