using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace ApplicationCaptureTests.Rules
{
    /// <summary>
    /// Contains rule tests for Empirica Scores.
    /// </summary>
    [TestFixture, RequiresSTA]
    public class EmpiricaScoreRules : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10);
        }

        //empirica limits
        private int _category0EmpiricaMinimumSalaried = 575;

        private int _category0EmpiricaMinimumSalaryDeduction = 575;
        private int _category0EmpiricaMinimumSelfEmployed = 595;

        private int _category1EmpiricaMinimumSalaried = 575;
        private int _category1EmpiricaMinimumSalaryDeduction = 575;
        private int _category1EmpiricaMinimumSelfEmployed = 600;

        private int _category3EmpiricaMinimumSalaried = 600;
        private int _category3EmpiricaMinimumSalaryDeduction = 575;
        private int _category3EmpiricaMinimumSelfEmployed = 610;

        private int _category4EmpiricaMinimumSalaried = 610;
        private int _category4EmpiricaMinimumSalaryDeduction = 575;

        private int _category5EmpiricaMinimumSalaried = 630;
        private int _category5EmpiricaMinimumSalaryDeduction = 575;

        private int _category6EmpiricaMinimumSalaried = 590;
        private int _category6EmpiricaMinimumSalaryDeduction = 575;

        private int _category7EmpiricaMinimumSalaried = 590;
        private int _category7EmpiricaMinimumSalaryDeduction = 575;

        private int _category8EmpiricaMinimumSalaried = 600;
        private int _category8EmpiricaMinimumSalaryDeduction = 575;

        private int _category9EmpiricaMinimumSalaried = 600;
        private int _category9EmpiricaMinimumSalaryDeduction = 575;

        private int _category10EmpiricaMinimumSalaryDeduction = 595;

        private int _category11EmpiricaMinimumSalaried = 640;

        private string message = @"An application with an employment type of {0} in Category {1} should have at least one income contributing legal entity with a minimum empirica score of {2}";

        [Test]
        public void when_max_empirica_less_than_category_0_salaried_minimum()
        {
            TestHelper(_category0EmpiricaMinimumSalaried, 0, (int)EmploymentTypeEnum.Salaried, 70);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 0, _category0EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_0_salary_deduction_minimum()
        {
            TestHelper(_category0EmpiricaMinimumSalaryDeduction, 0, (int)EmploymentTypeEnum.SalariedWithDeductions, 70);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 0, _category0EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_0_self_employed_minimum()
        {
            TestHelper(_category0EmpiricaMinimumSelfEmployed, 0, (int)EmploymentTypeEnum.SelfEmployed, 70);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SelfEmployed, 0, _category0EmpiricaMinimumSelfEmployed));
        }

        [Test]
        public void when_max_empirica_less_than_category_1_salaried_minimum()
        {
            TestHelper(_category1EmpiricaMinimumSalaried, 1, (int)EmploymentTypeEnum.Salaried, 80);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 1, _category1EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_1_salary_deduction_minimum()
        {
            TestHelper(_category1EmpiricaMinimumSalaryDeduction, 1, (int)EmploymentTypeEnum.SalariedWithDeductions, 80);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 1, _category1EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_1_self_employed_minimum()
        {
            TestHelper(_category1EmpiricaMinimumSelfEmployed, 1, (int)EmploymentTypeEnum.SelfEmployed, 80);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SelfEmployed, 1, _category1EmpiricaMinimumSelfEmployed));
        }

        [Test]
        public void when_max_empirica_less_than_category_3_salaried_minimum()
        {
            TestHelper(_category3EmpiricaMinimumSalaried, 3, (int)EmploymentTypeEnum.Salaried, 85);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 3, _category3EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_3_salary_deduction_minimum()
        {
            TestHelper(_category3EmpiricaMinimumSalaryDeduction, 3, (int)EmploymentTypeEnum.SalariedWithDeductions, 85);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 3, _category3EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_3_self_employed_minimum()
        {
            TestHelper(_category3EmpiricaMinimumSelfEmployed, 3, (int)EmploymentTypeEnum.SelfEmployed, 85);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SelfEmployed, 3, _category3EmpiricaMinimumSelfEmployed));
        }

        [Test]
        public void when_max_empirica_less_than_category_4_salaried_minimum()
        {
            TestHelper(_category4EmpiricaMinimumSalaried, 4, (int)EmploymentTypeEnum.Salaried, 90);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 4, _category4EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_4_salary_deduction_minimum()
        {
            TestHelper(_category4EmpiricaMinimumSalaryDeduction, 4, (int)EmploymentTypeEnum.SalariedWithDeductions, 90);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 4, _category4EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_5_salaried_minimum()
        {
            TestHelper(_category5EmpiricaMinimumSalaried, 5, (int)EmploymentTypeEnum.Salaried, 95);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 5, _category5EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_5_salary_deduction_minimum()
        {
            TestHelper(_category5EmpiricaMinimumSalaryDeduction, 5, (int)EmploymentTypeEnum.SalariedWithDeductions, 95);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 5, _category5EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_6_salaried_minimum()
        {
            TestHelper(_category6EmpiricaMinimumSalaried, 6, (int)EmploymentTypeEnum.Salaried, 85);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 6, _category6EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_6_salary_deduction_minimum()
        {
            TestHelper(_category6EmpiricaMinimumSalaryDeduction, 6, (int)EmploymentTypeEnum.SalariedWithDeductions, 85);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 6, _category6EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_7_salaried_minimum()
        {
            TestHelper(_category7EmpiricaMinimumSalaried, 7, (int)EmploymentTypeEnum.Salaried, 92);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 7, _category7EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_7_salary_deduction_minimum()
        {
            TestHelper(_category7EmpiricaMinimumSalaryDeduction, 7, (int)EmploymentTypeEnum.SalariedWithDeductions, 92);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 7, _category7EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_8_salaried_minimum()
        {
            TestHelper(_category8EmpiricaMinimumSalaried, 8, (int)EmploymentTypeEnum.Salaried, 96);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 8, _category8EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_8_salary_deduction_minimum()
        {
            TestHelper(_category8EmpiricaMinimumSalaryDeduction, 8, (int)EmploymentTypeEnum.SalariedWithDeductions, 96);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 8, _category8EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_9_salaried_minimum()
        {
            TestHelper(_category9EmpiricaMinimumSalaried, 9, (int)EmploymentTypeEnum.Salaried, 99);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 9, _category9EmpiricaMinimumSalaried));
        }

        [Test]
        public void when_max_empirica_less_than_category_9_salary_deduction_minimum()
        {
            TestHelper(_category9EmpiricaMinimumSalaryDeduction, 9, (int)EmploymentTypeEnum.SalariedWithDeductions, 99);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 9, _category9EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_10_salary_deduction_minimum()
        {
            TestHelper(_category10EmpiricaMinimumSalaryDeduction, 10, (int)EmploymentTypeEnum.SalariedWithDeductions, 100);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.SalariedWithDeductions, 10, _category10EmpiricaMinimumSalaryDeduction));
        }

        [Test]
        public void when_max_empirica_less_than_category_11_salaried_minimum()
        {
            TestHelper(_category11EmpiricaMinimumSalaried, 11, (int)EmploymentTypeEnum.Salaried, 100);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format(message, EmploymentType.Salaried, 11, _category11EmpiricaMinimumSalaried));
        }

        public void TestHelper(int minEmpScore, int category, int employmentType, int maxLTV = 100)
        {
            var minIncome = 20000D;
            var maxIncome = 500000D;
            switch (category)
            {
                case 0:
                case 1:
                    {
                        minIncome = 13000D;
                        break;
                    }
                case 6:
                case 7:
                case 8:
                case 9:
                    {
                        minIncome = 8000D;
                        maxIncome = 19999D;
                        break;
                    }
                default:
                    break;
            }
            //find a valid case by LTV and only 1 LE
            var results = Service<IX2WorkflowService>().GetApplicationsByStateAndAppType(WorkflowStates.ApplicationCaptureWF.ApplicationCapture, Workflows.ApplicationCapture,
                Exclusions.OrginationAutomation, ((int)OfferTypeEnum.NewPurchase).ToString(), 100, 0, 2, occupancyType: OccupancyTypeEnum.OwnerOccupied, employmentType: employmentType, category: category, maxIncome: maxIncome, minIncome: minIncome);

            var row = (from r in results where !r.Column("ADUserName").Value.Contains(TestUsers.BranchConsultant) select r).FirstOrDefault();
            if (row == null)
                base.FailTest("No application found for test");
            var offerKey = row.Column("offerkey").GetValueAs<int>();
            var cat = row.Column("category").GetValueAs<string>();

            var categoryDesc = Service<ICreditMatrixService>().GetCategoryDescription(category);

            Assert.AreEqual(categoryDesc, cat, "Offer was not in the expected category");

            //update all the required data
            //we cannot use Service<IApplicationService>().CleanupNewBusinessOffer(offerKey); as it manipulates the property and employment data thus changing the LTV and Household Income values
            Service<IApplicationService>().CleanupOfferForEmpiricaScoreTests(offerKey);
            //Insert the ITC
            Service<ILegalEntityService>().InsertITC(offerKey, minEmpScore, minEmpScore - 5);
            //Update Deeds Office details
            Service<IPropertyService>().DBUpdateDeedsOfficeDetails(offerKey);
            //complete the action
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);

            //Recalc and Save
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.UpdateLoanDetails);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().ChangeApplicationProduct(Products.NewVariableLoan);
            base.Browser.Page<ApplicationLoanDetailsUpdate>().RecalcAndSave(false);

            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            // Confirm the Submit Application
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
        }
    }
}