using System;
using System.Collections.Generic;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using Navigation = BuildingBlocks.Navigation;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    /// Contains tests for the creation of debt counselling cases and for Proposals/Counter Proposals
    /// </summary>
    [RequiresSTA]
    public class WorkflowTests : TestBase<BasePage>
    {
        #region PrivateVariables

        private TestBrowser browser;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestStart()
        {
            Settings.WaitForCompleteTimeOut = 120;
            Service<IWatiNService>().KillAllIEProcesses();
            if (browser != null)
            {
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            }
        }

        protected override void OnTestTearDown()
        {
            if (browser == null)
            {
                return;
            }
            else
            {
                browser.Page<BasePage>().CheckForErrorMessages();
                browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
                browser.Dispose();
                browser = null;
            }
        }

        #endregion SetupTearDown

        #region Tests

        /// <summary>
        /// Creates the test debt counselling cases from the configuration in the database tables
        /// </summary>
        /// <param name="testIdentifier">List of Test Cases</param>
        [Test, Sequential]
        public void _000_CreateDebtCounsellingCases([ValueSource(typeof(DebtCounsellingCreateSequentialData), "TestIdentifier")] string testIdentifier)
        {
            string userName = Service<IDebtCounsellingService>().GetCaseOwnerName(testIdentifier);
            browser = new TestBrowser(userName, TestUsers.Password);
            CreateDebtCounsellingCase(testIdentifier, browser);
            AssertDebtCounsellingCaseCreation(testIdentifier);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchTestIdentifier"></param>
        [Test, Sequential]
        public void _001_DebtCounsellingSearchTests([ValueSource(typeof(DebtCousellingSearchSequentialData), "TestIdentifier")] string testIdentifier)
        {
            DebtCounsellingSearchTest(testIdentifier);
        }

        /// <summary>
        /// Will test that a duplicate case can not be created.
        /// </summary>
        [Test, Description(@"")]
        public void _002_DebtCounsellingDuplicateCaseCreateTests()
        {
            try
            {
                browser = new TestBrowser(TestUsers.DebtCounsellingAdmin);
                CreateDebtCounsellingCase("DC_DuplicateCaseCreate_Original", browser);
                CreateDuplicateDebtCounsellingCase("DC_DuplicateCaseCreate_Second", browser);
            }
            catch
            {
                Service<IDebtCounsellingService>().UpdateDebtCounsellingLegalEntity("DC_DuplicateCaseCreate_Second", true);
                throw;
            }
        }

        /// <summary>
        /// This test will check that checkboxes that we are expecting to be disabled are correctly disabled when the search tree builds up
        /// the list of results. This should only be used on test cases in the debt counselling configuration tables where the UnderDebtCounselling value
        /// is set to ZERO in order to indicate that this checkbox should be disabled for selection.
        /// </summary>
        [Test, Description(@"This test will ensure that the checkboxes in the case creation screen are not available to be selected when the legal entity
			is of type Company, Trust or Close Corporation. This test will also ensure that the surety roles cannot be selected.")]
        public void _003_ValidateCheckboxesNonNaturalPersons([Values(
                                                          "DC_CloseCorpTest",
                                                          "DC_CompanyTest",
                                                          "DC_TrustTest"
                                                          )] string testIdentifier)
        {
            browser = new TestBrowser(TestUsers.DebtCounsellingAdmin);
            //navigate
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            //step one
            string ncrNumber = Service<IDebtCounsellingService>().GetNCRNumber(testIdentifier: testIdentifier);
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(ncrNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);

            //step three
            string idnumber = Service<IDebtCounsellingService>().GetIDNumber(testIdentifier);
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idnumber);
            //validate that the checkboxes cannot be selected
            browser.Page<DebtCounsellingCreateCase>().AssertCheckBoxesDisabled(testIdentifier);
        }

        #endregion Tests

        #region HelperMethods

        /// <summary>
        ///
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="browser"></param>
        public void CreateDebtCounsellingCase(
                string testIdentifier,
                TestBrowser browser,
                string comment = ""
            )
        {
            //navigate
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            //step one
            string ncrNumber = Service<IDebtCounsellingService>().GetNCRNumber(testIdentifier: testIdentifier);
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(ncrNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);

            //step two
            browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now);

            //step three
            string idnumber = Service<IDebtCounsellingService>().GetIDNumber(testIdentifier);
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idnumber);
            browser.Page<DebtCounsellingCreateCase>().SelectLegalEntitiesFromTree(testIdentifier, browser);
            browser.Page<DebtCounsellingCreateCase>().ClickCreateCase();
        }

        /// <summary>
        /// runs the assertions for the creation of a debt counselling case from the configuration data
        /// </summary>
        /// <param name="testIdentifier"></param>
        public void AssertDebtCounsellingCaseCreation(string testIdentifier)
        {
            List<int> accounts = Service<IDebtCounsellingService>().GetAccountListByDCTestIdentifier(testIdentifier);
            DebtCounsellingAssertions.AssertDebtCounselingCase(accounts.ToArray());
            //check the group key
            DebtCounsellingAssertions.AssertDebtCounsellingGroup(accounts);
            //save the debt counselling key for our test cases
            Service<IDebtCounsellingService>().SaveDebtCounsellingkeys(testIdentifier);
            //once we have the debt counselling keys we can check the x2 state
            List<int> dckeys = Service<IDebtCounsellingService>().GetDCKeyListByDCTestIdentifier(testIdentifier);
            foreach (int dckey in dckeys)
            {
                string userName = Service<IDebtCounsellingService>().GetCaseOwnerName(testIdentifier);
                //run the assertion for the x2 state
                DebtCounsellingAssertions.AssertX2State(dckey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
                //run the assertion for workflow role assignment
                DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(dckey, userName, WorkflowRoleTypeEnum.DebtCounsellingAdminD, false, true);
                //check that the workflow role exists
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(dckey,
                    WorkflowRoleTypeEnum.DebtCounsellingAdminD, userName);
                //we need to check that the Debt Counsellor role exists
                int dcLegalEntityKey = Service<IDebtCounsellingService>().GetDebtCounsellorLegalEntityKey(Service<IDebtCounsellingService>().GetNCRNumber(testIdentifier: testIdentifier));
                ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(dckey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.DebtCounsellor, dcLegalEntityKey);
            }
            DebtCounsellingAssertions.AssertClientRolesExist(testIdentifier);
            //check if we need to run the assertions to check for roles that should not have been created
            if (Service<IDebtCounsellingService>().MinDebtCounselling(testIdentifier) == 0)
            {
                DebtCounsellingAssertions.AssertClientRolesDoNotExist(testIdentifier);
            }
        }

        /// <summary>
        /// Runs the test steps for the validation of the search results
        /// </summary>
        /// <param name="testIdentifier"></param>
        private void DebtCounsellingSearchTest(string testIdentifier, string comment = "")
        {
            browser = new TestBrowser(TestUsers.DebtCounsellingAdmin, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            //Step One
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(Service<IDebtCounsellingService>().GetNCRNumber());
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);

            //step two
            browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now);

            //Step Three
            string idnumber = Service<IDebtCounsellingService>().GetIDNumber(testIdentifier);
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idnumber);
            //Step Four, Assert that there are only open accounts of importance of origination type RCS
            List<string> accountsOfImportance
                = browser.Page<DebtCounsellingCreateCase>().GetAccountsOfImportance();
            DebtCounsellingAssertions.AssertCreateCaseSearch(testIdentifier, idnumber, accountsOfImportance);
        }

        /// <summary>
        ///  This will test the a duplicate case can not be created.
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="browser"></param>
        public void CreateDuplicateDebtCounsellingCase(string testIdentifier, TestBrowser browser, string comment = "")
        {
            Service<IDebtCounsellingService>().UpdateDebtCounsellingLegalEntity("DC_DuplicateCaseCreate_Second", false);
            //navigate
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<Navigation.MenuNode>().LossControlNode();
            browser.Navigate<Navigation.MenuNode>().CreateCase();
            //step one
            string ncrNumber = Service<IDebtCounsellingService>().GetNCRNumber(testIdentifier: testIdentifier);
            browser.Page<DebtCounsellorSelect>().SearchByNCRNumber(ncrNumber);
            browser.Page<DebtCounsellorSelect>().ClickButton(ButtonTypeEnum.Select);

            //step two
            browser.Page<DebtCounsellingCreateCase>().PopulateView(DateTime.Now);

            //step three
            string idnumber = Service<IDebtCounsellingService>().GetIDNumber(testIdentifier);
            browser.Page<DebtCounsellingCreateCase>().SearchAndAddPeopleofImportance(idnumber);
            //validate that the checkboxes cannot be selected
            browser.Page<DebtCounsellingCreateCase>().AssertCheckBoxesDisabled(testIdentifier);
        }

        /// <summary>
        /// runs the assertions for the creation of a debt counselling case from the configuration data
        /// </summary>
        /// <param name="testIdentifier"></param>
        public void AssertDebtCounsellingCaseAtX2State(string testIdentifier)
        {
            Service<IDebtCounsellingService>().SaveDebtCounsellingkeys(testIdentifier);
            //once we have the debt counselling keys we can check the x2 state
            List<int> dckeys = Service<IDebtCounsellingService>().GetDCKeyListByDCTestIdentifier(testIdentifier);
            foreach (int dckey in dckeys)
            {
                string caseOwner = Service<IDebtCounsellingService>().GetCaseOwnerName(testIdentifier);
                //run the assertion for the x2 state
                DebtCounsellingAssertions.AssertX2State(dckey, WorkflowStates.DebtCounsellingWF.ReviewNotification);
                //run the assertion for workflow role assignment
                DebtCounsellingAssertions.AssertLatestDebtCounsellingWorkflowRoleAssignment(dckey, caseOwner, WorkflowRoleTypeEnum.DebtCounsellingAdminD,
                    false, true);
                //check that the workflow role exists
                WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(dckey, WorkflowRoleTypeEnum.DebtCounsellingAdminD, caseOwner);
                //we need to check that the Debt Counsellor role exists
                int dcLegalEntityKey = Service<IDebtCounsellingService>().GetDebtCounsellorLegalEntityKey(Service<IDebtCounsellingService>().GetNCRNumber(testIdentifier: testIdentifier));
                ExternalRoleAssertions.AssertActiveExternalRoleExistsForLegalEntity(dckey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                    ExternalRoleTypeEnum.DebtCounsellor, dcLegalEntityKey);
            }
            DebtCounsellingAssertions.AssertClientRolesExist(testIdentifier);
            //check if we need to run the assertions to check for roles that should not have been created
            if (Service<IDebtCounsellingService>().MinDebtCounselling(testIdentifier) == 0)
            {
                DebtCounsellingAssertions.AssertClientRolesDoNotExist(testIdentifier);
            }
        }

        /// <summary>
        /// Create a dynamic date based on the 1st day of the current month.  Inputing 0 for all variables will return the 1st of the current month.
        /// In order to return the last day of the desired month, set the month variable 1 month past the desired month and the date variable to -1
        /// </summary>
        /// <param name="addDays">Days to add</param>
        /// <param name="addMonths">Months to Add</param>
        /// <param name="addYears">Years to Add</param>
        /// <returns>DateTime</returns>
        public DateTime CreateDateFromToday(int addDays, int addMonths, int addYears)
        {
            DateTime today = DateTime.Today;
            return new DateTime(today.AddMonths(addMonths).AddYears(addYears).Year, today.AddMonths(addMonths).Month, 1).AddDays(addDays);
        }

        #endregion HelperMethods
    }
}