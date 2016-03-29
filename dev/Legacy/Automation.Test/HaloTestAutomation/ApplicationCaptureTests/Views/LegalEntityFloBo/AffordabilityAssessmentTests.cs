using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    /// <summary>
    ///
    /// </summary>
    [RequiresSTA]
    public class AffordabilityAssessmentTests : TestBase<AffordabilityDetailsUpdate>
    {
        private string offerTypeDescription;
        private int legalEntityKey1, legalEntityKey2, offerKey;
        private QueryResults results;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant10);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //check to see if there are any Application Capture cases at Application Capture state that have 2 client offer roles

            int oKey = Service<IApplicationService>().GetApplicationCaptureOfferWith2Applicants();
            if (oKey > 0)
            {
                offerKey = oKey;
            }
            else
            {
                offerKey = 0;
                // remove any nodes from task list
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
                // navigate to the application calculator
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
                base.Browser.Navigate<BuildingBlocks.Navigation.CalculatorsNode>().Calculators(base.Browser, CalculatorNodeTypeEnum.ApplicationCalculator);
                // complete the calculator
                base.Browser.Page<BuildingBlocks.Views.LoanCalculator>().LoanCalculatorLead_Refinance(
                    Products.NewVariableLoan, "978500", "450000", EmploymentType.Salaried, null, false, "28500", ButtonTypeEnum.CreateApplication);
                // add the 1st LE
                base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Miss", "auto", "First", "Applicant", null,
                   Gender.Female, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null,
                   null, null, null, null, "0761948851", null, false, false, false, true, false, ButtonTypeEnum.Next);
                // navigate to add legal entity node
                base.Browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Add);
                // add the 2nd LE
                base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddLegalEntity(OfferRoleTypes.LeadMainApplicant, true, null, "Miss", "auto", "Second", "Applicant", null,
                    Gender.Female, MaritalStatus.Single, "Unknown", "Unknown", CitizenType.SACitizen, null, null, null, "Unknown", Language.English, null, null, null,
                    null, null, null, null, "0761948851",
                    null, false, false, false, true, false, ButtonTypeEnum.Next);
                // get the offerkey
                offerKey = base.Browser.Page<ApplicationSummaryBase>().GetOfferKey();
                Assert.That(offerKey > 0, "Offer not created");
            }

            // get the offertype data
            results = Service<IApplicationService>().GetOfferData(offerKey);
            offerTypeDescription = results.Rows(0).Column("OfferType").Value;
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);

            // get the LEKeys for the Offer
            results = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey);
            Assert.Greater(results.RowList.Count, 1, "This case not have 2 applicants and cannot be used.");

            legalEntityKey1 = results.Rows(0).Column("LegalEntityKey").GetValueAs<int>();
            legalEntityKey2 = results.Rows(1).Column("LegalEntityKey").GetValueAs<int>();
        }

        /// <summary>
        /// Check that on the Update Affordability Details screen,
        /// all affordability and expenses entries with values greater than zero are written to the db on Update.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures the all affordability and expenses entries with values greater than zero are written to the db on Update")]
        public void _001_AffordabilityNonZeroValuesWrittenToDB()
        {
            // navigate to first applicant
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);

            // navigate to 'update affordability and expenses' contextmenu node
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in income values
            const int income = 3000;
            const string desc = "desc";
            base.View.PopulateIncomeFields(income, income, income, income, desc, income, desc, income, desc);
            // fill in expense values
            const int expense = 500;
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, desc, expense, desc, expense, expense, expense, desc, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            // fill in dependant values
            const int dependantsInHousehold = 1;
            const int contributingDependants = 2;
            base.View.PopulateDependantFields(dependantsInHousehold, contributingDependants);
            // press "update" button
            base.View.ClickUpdateButton();

            // check income records
            int expectedIncomeRecords = Service<ILegalEntityService>().CountAffordabilityTypeIncomeRecords();
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            int rowCount = results.RowList.Where((t, i) => results.Rows(i).Column("Amount").GetValueAs<int>() > 0).Count();

            Assert.AreEqual(rowCount, expectedIncomeRecords);

            // check expense records
            int expectedExpenseRecords = Service<ILegalEntityService>().CountAffordabilityTypeExpenseRecords();
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityExpensesByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            rowCount = results.RowList.Where((t, i) => results.Rows(i).Column("Amount").GetValueAs<int>() > 0).Count();
            Assert.AreEqual(rowCount, expectedExpenseRecords);

            // check dependant records
            results = Service<IApplicationService>().GetOfferMortgageLoanByOfferKey(offerKey);
            Assert.AreEqual(results.Rows(0).Column("DependentsPerHousehold").GetValueAs<int>(), dependantsInHousehold);
            Assert.AreEqual(results.Rows(0).Column("ContributingDependents").GetValueAs<int>(), contributingDependants);
        }

        /// <summary>
        /// Check that on the Update Affordability Details screen,
        /// all affordability and expenses entries with zero values are not written to the db
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures all affordability and expenses entries with zero values are not written to the db")]
        public void _002_AffordabilityZeroValuesNotWrittenToDB()
        {
            // navigate to first applicant
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);
            // navigate to 'update affordability and expenses' contextmenu node
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);
            // fill in income values
            const int income = 0;
            const string desc = "";
            base.View.PopulateIncomeFields(1000, income, income, income, desc, income, desc, income, desc);
            // fill in expense values
            const int expense = 0;
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, desc, expense, desc, expense, expense, expense, desc, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            // fill in dependant values
            const int dependantsInHousehold = 0;
            const int contributingDependants = 0;
            base.View.PopulateDependantFields(dependantsInHousehold, contributingDependants);
            // press "update" button
            base.View.ClickUpdateButton();

            // read the database and confirm that all values are written for this legalentity and offer

            // check income records
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            Assert.AreEqual(results.RowList.Count, 1);

            // check expense records
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityExpensesByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            Assert.AreEqual(results.RowList.Count, 0);

            // check dependant records
            results = Service<IApplicationService>().GetOfferMortgageLoanByOfferKey(offerKey);
            Assert.AreEqual(results.Rows(0).Column("DependentsPerHousehold").GetValueAs<int>(), 0);
            Assert.AreEqual(results.Rows(0).Column("ContributingDependents").GetValueAs<int>(), 0);
        }

        /// <summary>
        /// Check that on the Update Affordability Details screen,
        /// validation exists to ensure at least one income amount is entered when an expense amount is entered.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures at least one income amount is entered when an expense amount is entered")]
        public void _003_AffordabilityValidateIncomeIfExpenseEntered()
        {
            // navigate to first applicant
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);
            // navigate to 'update affordability and expenses' contextmenu node
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);
            // fill in income values
            const int income = 0;
            const string desc = "desc";
            base.View.PopulateIncomeFields(income, income, income, income, desc, income, desc, income, desc);
            // fill in expense values
            const int expense = 500;
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, desc, expense, desc, expense, expense, expense, desc, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            // press "update" button
            base.View.ClickUpdateButton();
            //assert that the error message is displayed
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("At least one income amount is required.");
        }

        /// <summary>
        /// Check that on the Update Affordability Details screen,
        /// validation exists to ensure a description is captured where a value is entered in an Amount field with a corresponding Description field.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures a description is captured where a value is entered in an Amount field with a corresponding Description field")]
        public void _004_AffordabilityValidateAmountDescription()
        {
            // navigate to first applicant
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);

            // navigate to 'update affordability and expenses' contextmenu node
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in income and expense values
            const int income = 3000;
            const string desc = "";
            const string descwithvalue = "toys";
            const int expense = 1000;
            base.View.PopulateIncomeFields(income, income, income, income, descwithvalue, income, descwithvalue, income, descwithvalue);
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, desc, expense, descwithvalue, expense, expense, expense, descwithvalue, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            // press "update" button and check that the assertion works
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Other Instalments.");
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, descwithvalue, expense, desc, expense, expense, expense, descwithvalue, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Other.");
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, descwithvalue, expense, descwithvalue, expense, expense, expense, desc, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Other debt repayment.");
            base.View.PopulateIncomeFields(income, income, income, income, desc, income, descwithvalue, income, descwithvalue);
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, descwithvalue, expense, descwithvalue, expense, expense, expense, descwithvalue, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Income from Investments.");
            base.View.PopulateIncomeFields(income, income, income, income, descwithvalue, income, desc, income, descwithvalue);
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Other Income 1.");
            base.View.PopulateIncomeFields(income, income, income, income, descwithvalue, income, descwithvalue, income, desc);
            base.View.ClickUpdateButton();

            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Enter description for Other Income 2.");
            base.View.PopulateIncomeFields(income, income, income, income, descwithvalue, income, descwithvalue, income, descwithvalue);
            base.View.ClickUpdateButton();
        }

        /// <summary>
        /// Check that it is possible to Cancel an Update Affordability Details action and that no updates are committed on Cancelling.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures that it is possible to Cancel an Update Affordability Details action and that no updates are committed on Cancelling")]
        public void _005_AffordabilityCancellation()
        {
            // navigate to first applicant
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);

            // read the database and get the current total of income & expense for this legalentity and offer
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            int expectedRecordCount = results.RowList.Count;
            double expectedRecordTotal = 0;
            for (int i = 0; i < results.RowList.Count; i++)
            {
                expectedRecordTotal += results.Rows(i).Column("Amount").GetValueAs<double>();
            }

            // navigate to 'update affordability and expenses' contextmenu node
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in income values
            const int income = 3000;
            const string desc = "desc";
            base.View.PopulateIncomeFields(income, income, income, income, desc, income, desc, income, desc);
            // fill in expense values
            const int expense = 850;
            base.View.PopulateExpenseFields(expense, expense, expense, expense, expense, expense, expense, expense, expense, expense, desc, expense, desc, expense, expense, expense, desc, expense, expense, expense, expense, expense, expense, expense, expense, expense, expense);

            // press "cancel" button
            base.View.ClickCancelButton();

            // read the database and get the current total of income & expense for this legalentity and offer
            results = Service<ILegalEntityService>().GetLegalEntityAffordabilityIncomeByLegalEntityKeyAndOfferKey(legalEntityKey1, offerKey);
            int newRecordCount = results.RowList.Count;
            double newRecordTotal = results.RowList.Select((t, i) => results.Rows(i).Column("Amount").GetValueAs<double>()).Sum();

            Assert.AreEqual(expectedRecordCount, newRecordCount);
            Assert.AreEqual(expectedRecordTotal, newRecordTotal);
        }

        /// <summary>
        /// Check that the Affordability view on the Loan node displays the sum of all Affordability and Expense details for all legal entities
        /// (multiple main applicant and suretors) attached to the application.
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures affordability view on the Loan node displays the combined affordability and Expense details for all legal entities")]
        public void _006_AffordabilityViewCombined()
        {
            // navigate to first applicant and update
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in basic income & rental income
            const int expectedBasicIncome1 = 5000;
            const int expectedRentalIncome1 = 3000;
            base.View.PopulateIncomeFields(expectedBasicIncome1, 0, expectedRentalIncome1, 0, string.Empty, 0, string.Empty, 0,
                string.Empty);
            // fill in living expenses
            const int expectedLivingExpenses1 = 500;
            const int expectedRentalRepayment1 = 100;
            base.View.PopulateExpenseFields(0, expectedLivingExpenses1, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, expectedRentalRepayment1, 0, 0, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // press "update" button
            base.View.ClickUpdateButton();

            // navigate to second applicant and update
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey2);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in basic income & rental income
            const int expectedBasicIncome2 = 4000;
            const int expectedRentalIncome2 = 2000;
            base.View.PopulateIncomeFields(expectedBasicIncome2, 0, expectedRentalIncome2, 0, string.Empty, 0, string.Empty, 0,
                string.Empty);
            // fill in living expenses
            const int expectedLivingExpenses2 = 300;
            const int expectedRentalRepayment2 = 200;
            base.View.PopulateExpenseFields(0, expectedLivingExpenses2, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, expectedRentalRepayment2, 0, 0, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // press "update" button
            base.View.ClickUpdateButton();

            // set expectancies
            int expectedBasicIncomeTotal = expectedBasicIncome1 + expectedBasicIncome2;
            int expectedRentalIncomeTotal = expectedRentalIncome1 + expectedRentalIncome2;
            int expectedTotalIncome = expectedBasicIncomeTotal + expectedRentalIncomeTotal;
            int expectedMonthlyExpensesTotal = expectedLivingExpenses1 + expectedLivingExpenses2;
            int expectedDebtRepaymentTotal = expectedRentalRepayment1 + expectedRentalRepayment2;
            int expectedTotalExpenses = expectedMonthlyExpensesTotal + expectedDebtRepaymentTotal;
            int expectedAffordability = expectedTotalIncome - expectedTotalExpenses;

            int incomeTotal, monthlyExpenseTotal, debtRepaymentTotal, totalIncome, TotalExpenses, affordability;

            // navigate to loan node
            base.Browser.Navigate<ApplicationOverviewNode>().Select(offerKey, offerTypeDescription);
            // navigate to affordability node
            base.Browser.Navigate<ApplicationOverviewNode>().AffordabilityNode();

            // get the values from the screen
            base.View.GetAffordabilityTotals(out incomeTotal, out monthlyExpenseTotal, out debtRepaymentTotal, out totalIncome, out TotalExpenses, out affordability);

            // do the assertions
            Assert.AreEqual(incomeTotal, expectedTotalIncome);
            Assert.AreEqual(monthlyExpenseTotal, expectedMonthlyExpensesTotal);
            Assert.AreEqual(debtRepaymentTotal, expectedDebtRepaymentTotal);
            Assert.AreEqual(totalIncome, expectedTotalIncome);
            Assert.AreEqual(TotalExpenses, expectedTotalExpenses);
            Assert.AreEqual(affordability, expectedAffordability);
        }

        /// <summary>
        /// Check that the Affordability view on any legal entity node displays only that legal entities individual affordability and expense details
        /// <list type="table">
        /// <listheader>
        /// <workflow>Workflow</workflow>
        /// </listheader>
        /// <item>
        /// <workflow>Application Capture</workflow>
        /// </item>
        /// </list>
        /// </summary>
        [Test, Description("Ensures affordability view on the legal entity node displays only the individual affordability and expense details")]
        public void _007_AffordabilityViewSingle()
        {
            // navigate to first applicant and update
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in basic income & rental income
            const int expectedBasicIncome1 = 5000;
            const int expectedRentalIncome1 = 3000;
            base.View.PopulateIncomeFields(expectedBasicIncome1, 0, expectedRentalIncome1, 0, string.Empty, 0, string.Empty, 0,
                string.Empty);
            // fill in living expenses
            const int expectedLivingExpenses1 = 500;
            const int expectedRentalRepayment1 = 200;
            base.View.PopulateExpenseFields(0, expectedLivingExpenses1, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, expectedRentalRepayment1, 0, 0, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // press "update" button
            base.View.ClickUpdateButton();

            // navigate to second applicant and update
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey2);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.Update);

            // fill in basic income & rental income
            const int expectedBasicIncome2 = 4000;
            const int expectedRentalIncome2 = 2000;
            base.View.PopulateIncomeFields(expectedBasicIncome2, 0, expectedRentalIncome2, 0, string.Empty, 0, string.Empty, 0,
                string.Empty);
            // fill in living expenses
            const int expectedLivingExpenses2 = 300;
            const int expectedRentalRepayment2 = 200;
            base.View.PopulateExpenseFields(0, expectedLivingExpenses2, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, string.Empty, expectedRentalRepayment2, 0, 0, string.Empty, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // press "update" button
            base.View.ClickUpdateButton();

            int incomeTotal, monthlyExpenseTotal, debtRepaymentTotal, totalIncome, TotalExpenses, affordability;

            int expectedTotalIncome = expectedBasicIncome1 + expectedRentalIncome1;
            int expectedTotalExpenses = expectedLivingExpenses1 + expectedRentalRepayment1;
            int expectedAffordability = expectedTotalIncome - expectedTotalExpenses;

            // navigate to first applicant and assert field values
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey1);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.View);

            // get the values from the screen
            base.View.GetAffordabilityTotals(out incomeTotal, out monthlyExpenseTotal, out debtRepaymentTotal, out totalIncome, out TotalExpenses, out affordability);

            // do the assertions
            Assert.AreEqual(incomeTotal, expectedTotalIncome);
            Assert.AreEqual(monthlyExpenseTotal, expectedLivingExpenses1);
            Assert.AreEqual(debtRepaymentTotal, expectedRentalRepayment1);
            Assert.AreEqual(totalIncome, expectedTotalIncome);
            Assert.AreEqual(TotalExpenses, expectedTotalExpenses);
            Assert.AreEqual(affordability, expectedAffordability);

            expectedTotalIncome = expectedBasicIncome2 + expectedRentalIncome2;
            expectedTotalExpenses = expectedLivingExpenses2 + expectedRentalRepayment2;
            expectedAffordability = expectedTotalIncome - expectedTotalExpenses;

            // navigate to second applicant and assert field values
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey2);
            base.Browser.Navigate<LegalEntityNode>().AffordabilityAndExpenses(NodeTypeEnum.View);

            // get the values from the screen
            base.View.GetAffordabilityTotals(out incomeTotal, out monthlyExpenseTotal, out debtRepaymentTotal, out totalIncome, out TotalExpenses, out affordability);

            // do the assertions
            Assert.AreEqual(incomeTotal, expectedTotalIncome);
            Assert.AreEqual(monthlyExpenseTotal, expectedLivingExpenses2);
            Assert.AreEqual(debtRepaymentTotal, expectedRentalRepayment2);
            Assert.AreEqual(totalIncome, expectedTotalIncome);
            Assert.AreEqual(TotalExpenses, expectedTotalExpenses);
            Assert.AreEqual(affordability, expectedAffordability);
        }
    }
}