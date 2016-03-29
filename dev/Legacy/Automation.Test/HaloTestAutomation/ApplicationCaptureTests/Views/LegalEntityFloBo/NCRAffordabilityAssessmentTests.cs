using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    [RequiresSTA]
    public class NCRAffordabilityAssessmentTests : TestBase<NCRAffordabilityDetailsUpdate>
    {
        private int OfferKey { get; set; }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();

            OfferKey = base.Service<IApplicationService>().GetApplicationCaptureOfferWith2ApplicantsWhereLegalEntitiesHaveSalutationAndInitials();           
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(OfferKey);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();
        }

        [Test]
        public void _001_WhenCapturingAnAffordabilityAssessmentWithMultipleApplicants()
        {
            DeleteAssessments();
            
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAddAffordabilityAssessment();
            var legalEntities = base.Service<IApplicationService>().GetActiveOfferRolesByOfferKey(OfferKey, OfferRoleTypeGroupEnum.Client).ToList();
            int householdDependantCount = 2;
            string idNumber = IDNumbers.GetNextIDNumber();

            base.Browser.Page<CaptureAffordabilityAssessment>().AddNonApplicantIncomeContributor(legalEntities.FirstOrDefault().LegalEntityKey);
            base.Browser.Page<CaptureAffordabilityAssessment>().ClickAddContributorButton();
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity(
                               false, idNumber, OfferRoleTypes.LeadMainApplicant, false, "Mr", "T", "Test", "AffordabilityContributor", "Test", Gender.Male,
                               MaritalStatus.Single, PopulationGroup.Coloured, Education.Matric, CitizenType.SACitizen, "", "", Language.English, Language.English, "Alive", "031",
                               "3003001", "", "", "", "", "", "", true, true, true, true, true, "", Service<ICommonService>().GetDateOfBirthFromIDNumber(IDNumbers.GetNextIDNumber())
                           );
            base.Browser.Page<LegalEntityRelationshipsAffordabilityAssessment>().SelectRelationshipType(RelationshipType.CloseRelative);
            base.Browser.Page<LegalEntityRelationshipsAffordabilityAssessment>().ClickAddButton();

            foreach (var entity in legalEntities)
            {
                base.Browser.Page<CaptureAffordabilityAssessment>().SelectIncomeAndNonIncomeContributorsByLegalEntityKey(entity.LegalEntityKey);
            }

            base.Browser.Page<CaptureAffordabilityAssessment>().AddHouseholdDependants(householdDependantCount);
            base.Browser.Page<CaptureAffordabilityAssessment>().SelectIncomeAndNonIncomeContributorsByLegalEntityKey(ServiceLocator.Instance.GetService<ILegalEntityService>().GetLegalEntity(idNumber).LegalEntityKey);
            base.Browser.Page<CaptureAffordabilityAssessment>().ClickSaveButton();

            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed);

            Assert.That(Convert.ToInt32(results.FirstOrDefault().NumberOfContributingApplicants).Equals(legalEntities.Count() + 1));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().NumberOfHouseholdDependants).Equals(householdDependantCount));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().AffordabilityAssessmentStatusKey).Equals((int)AffordabilityAssessmentStatusKey.Unconfirmed));
            Assert.That(Convert.ToInt32(results.FirstOrDefault().GeneralStatusKey).Equals((int)GeneralStatusEnum.Active));
            LegalEntityRelationshipAssertions.AssertRelationshipExists(idNumber, RelationshipType.CloseRelative, legalEntities.FirstOrDefault().LegalEntityKey);
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
            DeleteAssessments();
            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed);

            Assert.True(results.Count() == 0);
        }

        private void DeleteAssessments()
        {
            int assessmentCount = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(OfferKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).Count();

            for (int i = 0; i < assessmentCount; i++)
            {
                base.Browser.Navigate<AffordabilityAssessmentNode>().ClickDeleteAffordabilityAssessment();
                base.Browser.Page<CaptureAffordabilityAssessment>().ClickDeleteAssessmentButton();
            }
        }
    }
}