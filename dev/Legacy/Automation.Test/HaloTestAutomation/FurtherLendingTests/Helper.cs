using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.CBO;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WorkflowAutomation.Harness;

namespace FurtherLendingTests
{
    /// <summary>
    /// Helper Class for Application Management tests containing common methods used among the tests in the test suite
    /// </summary>
    public static class Helper
    {
        private static IApplicationService applicationService;
        private static IAccountService accountService;
        private static IADUserService aduserService;
        private static IFurtherLendingService flService;
        private static IAssignmentService assignmentService;
        private static IX2WorkflowService x2Service;
        private static ICommonService commonService;
        private static IX2ScriptEngine scriptEngine;

        static Helper()
        {
            applicationService = ServiceLocator.Instance.GetService<IApplicationService>();
            accountService = ServiceLocator.Instance.GetService<IAccountService>();
            aduserService = ServiceLocator.Instance.GetService<IADUserService>();
            flService = ServiceLocator.Instance.GetService<IFurtherLendingService>();
            assignmentService = ServiceLocator.Instance.GetService<IAssignmentService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
            scriptEngine = new X2ScriptEngine();
        }

        /// <summary>
        /// Holds the details of the further lending test case.
        /// </summary>
        public class FLTestCase
        {
            public int OfferKey { get; set; }

            public string Processor { get; set; }

            public string Supervisor { get; set; }

            public int AccountKey { get; set; }

            public string CurrentState { get; set; }

            public int ReadvanceOfferKey { get; set; }

            public int FurtherAdvanceOfferKey { get; set; }

            public int FurtherLoanOfferKey { get; set; }

            public int ProcessViaWorkflowAutomation { get; set; }
        }

        /// <summary>
        /// Loads up a further lending case from the FL automation table and adds it onto the FloBO
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <param name="browser"></param>
        public static FLTestCase SearchForFurtherLendingApp(string testIdentifier, TestBrowser browser)
        {
            var testCase = GetTestCase(testIdentifier);
            Search(testCase.OfferKey, browser);
            return testCase;
        }

        /// <summary>
        /// Gets the details of a test case from the test table and returns a FLTestCase class.
        /// </summary>
        /// <param name="testIdentifier"></param>
        /// <returns></returns>
        public static FLTestCase GetTestCase(string testIdentifier)
        {
            var results = flService.GetFLAutomationRowByTestIdentifier(testIdentifier);

            var offerKey = flService.ReturnFurtherLendingOfferKeyByTestGroup(results);
            var testCase = new FLTestCase
            {
                OfferKey = offerKey,
                Processor = results.Rows(0).Column("AssignedFLAppProcUser").Value,
                Supervisor = results.Rows(0).Column("AssignedFLSupervisor").Value,
                AccountKey = results.Rows(0).Column("AccountKey").GetValueAs<int>(),
                CurrentState = results.Rows(0).Column("CurrentState").Value,
                ReadvanceOfferKey = results.Rows(0).Column("ReadvOfferKey").GetValueAs<int>(),
                FurtherAdvanceOfferKey = results.Rows(0).Column("FAdvOfferKey").GetValueAs<int>(),
                FurtherLoanOfferKey = results.Rows(0).Column("FLOfferKey").GetValueAs<int>(),
                ProcessViaWorkflowAutomation = results.Rows(0).Column("ProcessViaWorkflowAutomation").GetValueAs<int>()
            };
            return testCase;
        }

        /// <summary>
        /// Searches for an offer
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="browser"></param>
        public static void Search(int offerKey, TestBrowser browser)
        {
            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
        }

        /// <summary>
        /// This method will get the currently assigned FL Processor by looking on the Further Lending application summary
        /// and picking it up from the label. It will then check if that user is currently marked as active. If they are
        /// not active and we are moving the case to a state that reassigns the FL Processor role application then we need
        /// to find the next FL Processor due for RR assignment, as this will become our expected user.
        /// </summary>
        /// <param name="offerKey">Application Number</param>
        /// <returns>FL Processor we expect to be assigned the case</returns>
        public static string GetFLProcessorForApplicationCheckIsActive(int offerKey)
        {
            var flAppProcUser = applicationService.GetADUserNameOfFirstActiveOfferRole(offerKey, OfferRoleTypeEnum.FLProcessorD);
            bool isActive = aduserService.IsADUserActive(flAppProcUser);
            //if the user is inactive they will not be assigned the case when the case is sent to Decline
            if (!isActive)
            {
                //so we find the next FL Processor
                flAppProcUser = assignmentService.GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD, RoundRobinPointerEnum.FLProcessor);
            }
            return flAppProcUser;
        }

        #region TestHelpers

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public static void NavigateToFurtherLendingCalculator(int accountKey, TestBrowser browser)
        {
            //navigate to the client search
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            browser.Page<FurtherLendingPreCheck>().Next();
        }

        /// <summary>
        /// This helper will navigate to the further lending calculator and then check if a validation message should or should not exist. In the case
        /// where you are looking for a specific validation message the string needs to be passed to this method.
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="accountKey">accountKey</param>
        /// <param name="browser">TestBrowser</param>
        /// <param name="msg">Validation Message, pass NULL if you are checking if the validation summary should not exist.</param>
        public static void GoToFurtherLendingCalculatorAndCheckIfMessageExists(string identifier, int accountKey, TestBrowser browser, string msg, bool exists)
        {
            //get the row
            QueryResults results = flService.GetFLAutomationRowByTestIdentifier(identifier);
            //update the timeout to a minute
            Settings.WaitForCompleteTimeOut = 120;
            //navigate to the client search
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            browser.Page<FurtherLendingPreCheck>().Next();
            List<string> messages = browser.Page<BasePage>().GetValidationMessages();
            var message = (from m in messages where m.Contains(msg) select m).FirstOrDefault();
            Assert.That(string.IsNullOrEmpty(message) == !exists);
        }

        /// <summary>
        /// Runs the test steps required to Create FL Applications
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="browser">TestBrowser Object</param>
        public static void CreateFurtherLendingApplications(string identifier, int accountKey, TestBrowser browser)
        {
            //get the row
            QueryResults results = flService.GetFLAutomationRowByTestIdentifier(identifier);
            //update the timeout to a minute
            Settings.WaitForCompleteTimeOut = 120;
            //navigate to the client search
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(browser);
            browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            //load the client into the CBO
            browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAccountNode(accountKey);
            browser.Navigate<LoanServicingCBO>().LoanAdjustments();
            browser.Navigate<LoanServicingCBO>().FurtherLendingCalculator();
            browser.Page<FurtherLendingPreCheck>().Next();
            //create multiple applications
            switch (identifier)
            {
                default:
                    browser.Page<FurtherLendingCalculator>().CreateMultipleApplications(results.Rows(0).Column("ContactOption").Value, results, 0, 0);
                    break;

                case "SuperLoSPVChange1":
                    browser.Page<FurtherLendingCalculator>().CreateMultipleApplications(results.Rows(0).Column("ContactOption").Value, results, 81, 85);
                    break;
            }

            browser.Navigate<LoanServicingCBO>().RemoveLegalEntities();
            flService.InsertFLOfferKeys(accountKey);
            results.Dispose();
            results = flService.GetFLAutomationRowByTestIdentifier(identifier);
            FurtherLendingAssertions.AssertFLOfferCreated(results);
            //we need to double check any invalid ID numbers
            accountService.CorrectAccountRolesWithInvalidIDNumbers(accountKey);
            results.Dispose();
        }

        /// <summary>
        /// Runs the test steps required to perform the Application Received action and store the assertions
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="browser">TestBrowser Object</param>
        public static void ApplicationReceived(string identifier, TestBrowser browser)
        {
            var testCase = GetTestCase(identifier);
            //round robin check
            string flAppProcUser = assignmentService.GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD, RoundRobinPointerEnum.FLProcessor);
            var instanceid = x2Service.GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.AwaitingApplication, testCase.OfferKey);
            if (testCase.ProcessViaWorkflowAutomation == 1)
            {
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.FurtherLending.ApplicationReceived, testCase.OfferKey);
            }
            else
            {
                Search(testCase.OfferKey, browser);
                //perform the application received action
                browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationReceived);
                browser.Page<WorkflowYesNo>().Confirm(true, false);
            }
            x2Service.WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.HighestPriority, instanceid, 1);
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.QA);
            //assert the memo record exists
            MemoAssertions.AssertApplicationReceivedMemo(testCase.AccountKey, testCase.OfferKey);
            //check if Highest Priority has happened
            //assert workflow assignment to expected user
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
            //if all is okay then we can save the FLAppProcUser
            flService.UpdateFLAutomation("AssignedFLAppProcUser", flAppProcUser, identifier);
        }

        /// <summary>
        /// Runs the test steps required to QA Complete a Further Lending application
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="browser">TestBrowser Object</param>
        public static void QAComplete(string identifier, TestBrowser browser)
        {
            var testCase = GetTestCase(identifier);
            var instanceid = x2Service.GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.QA, testCase.OfferKey);
            string selectedReason = string.Empty;
            if (testCase.ProcessViaWorkflowAutomation == 1)
            {
                scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, WorkflowAutomationScripts.FurtherLending.QACompleteFL, testCase.OfferKey);
            }
            else
            {
                Search(testCase.OfferKey, browser);
                browser.ClickAction(WorkflowActivities.ApplicationManagement.QAComplete);
                selectedReason = browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QAComplete);
                x2Service.WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.AssignProcessor, instanceid, 1);
                //REASON ASSERTION
                ReasonAssertions.AssertReason(selectedReason, ReasonType.QAComplete, testCase.OfferKey, GenericKeyTypeEnum.Offer_OfferKey);
            }
            x2Service.WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.AssignProcessor, instanceid, 1);
            //wait for case to move
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ManageApplication);
            //THE CASE SHOULD STILL BELONG TO THE SAME USER
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Runs the assertions to ensure the further lending case has been assigned in Credit
        /// </summary>
        /// <param name="testMethod">TestMethod</param>
        /// <param name="identifier">Test Identifier</param>
        public static void AssertFurtherLendingCreditCases(string testMethod, string identifier)
        {
            int offerKey = commonService.GetTestMethodParameters<int>(testMethod, identifier, ParameterTypeEnum.OfferKey);
            int instanceID = X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.CreditHold);
            //wait for credit case creation
            x2Service.WaitForCreditCaseCreate(instanceID, offerKey, WorkflowStates.CreditWF.Credit);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
            flService.UpdateFLAutomation("Credit", "1", identifier);
            string creditAdUser;
            OfferRoleTypeEnum creditOfferRoleType;
            flService.UpdateAssignedCreditUser(offerKey, identifier, out creditAdUser, out creditOfferRoleType);
            flService.UpdateFLAutomation("AssignedCreditUser", creditAdUser, identifier);
            AssignmentAssertions.AssertWorkflowAssignment(creditAdUser, offerKey, creditOfferRoleType);
            //remove the assertion records from the db
            commonService.DeleteTestMethodData(testMethod, identifier);
        }

        /// <summary>
        /// This helper is used to submit a further advance application into the Credit workflow. It also stores the required
        /// assertions values in the TestMethod and TestMethodParameter tables for the AssertFurtherLendingCreditCases method
        /// to use.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="callingMethod">Test Method using the Helper method</param>
        /// <param name="browser">TestBrowser Object</param>
        public static void ApplicationInOrderFurtherAdvance(string identifier, string callingMethod, TestBrowser browser)
        {
            var testCase = SearchForFurtherLendingApp(identifier, browser);
            //update the loan conditions
            Helper.SaveLoanConditions(browser, testCase.OfferKey);
            //we need to clean up the further lending offer data
            flService.CleanUpOfferData(testCase.OfferKey);
            //complete the LE CBO node requirements
            LegalEntityCBONode.CompleteLegalEntityNode(browser, testCase.OfferKey, true, true, false, false, false, false, false, false);
            //navigate to the doc checklist
            browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().DocumentChecklist();
            browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //perform the action
            browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            browser.Page<WorkflowYesNo>().Confirm(true, true);
            commonService.InsertTestMethod(callingMethod, identifier, "FurtherLendingTests");
            //then store our params
            commonService.SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static int GetApplicationForFurtherLendingTest(string workflowState, string workflow)
        {
            int offerKey = 0;
            var offerTypes = new List<OfferTypeEnum> { OfferTypeEnum.Readvance, OfferTypeEnum.FurtherAdvance, OfferTypeEnum.FurtherLoan };
            //loop through the list.
            foreach (var type in offerTypes)
            {
                offerKey = x2Service.GetOfferKeyAtStateByType(workflowState, workflow, type, "FLAutomation");
                if (offerKey > 0)
                    break;
            }
            if (offerKey == 0)
                Assert.Fail(string.Format(@"No further lending case found at {0}.", workflowState));

            return offerKey;
        }

        /// <summary>
        /// This is a grouping of Building Blocks methods that can be called when a test needs to add data to an application using the
        /// Loan Details node.
        /// </summary>
        /// <param name="browser">IE TestBrowser Object</param>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="loanConditions">True=add, False=do not add</param>
        /// <param name="settlementBank">True=add, False=do not add</param>
        /// <param name="doDetails">True=add, False=do not add</param>
        /// <param name="mailingAddress">True=add, False=do not add</param>
        public static void SaveLoanConditions(TestBrowser browser, int offerKey)
        {
            //first select the Loan Details node
            browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            browser.Navigate<LoanDetailsNode>().ClickManageLoanConditionsNode();
            browser.Page<ConditionAddSet>().SaveConditionSet();
        }

        #endregion TestHelpers
    }
}