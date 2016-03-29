using Automation.DataAccess;
using System.Linq;
using System.Reflection;
using System.Threading;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using System;
using BuildingBlocks.CBO;

namespace FurtherLendingTests
{
    [TestFixture, RequiresSTA]
    public class SuperLoOptOutTests : TestBase<BasePage>
    {
        #region PrivateVariables

        #endregion PrivateVariables

        #region Setup/Teardown

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Service<ICommonService>().DeleteTestMethodDataForFixture("ReadvancePaymentTests");
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        #endregion Setup/Teardown

        #region Tests

        /// <summary>
        /// This test will create a number of Further Lending Applications using the FurtherLendingSuperLoSequentialData data class
        /// populated from the test.AutomationFLTestCases table.
        /// </summary>
        /// <param name="identifier">The TestIdentifier of the Test Case</param>
        /// <param name="accountKey">Mortgage Loan Account Key</param>
        /// <param name="testGroup">The Test Group i.e. Readvances, Further Advances or Further Loans</param>
        [Test, Sequential, Description("Ensures that a FL Processor User can create a readvance application."), Category("Super Lo Opt Out")]
        public void _001_CreateSuperLoOptOutFLApplications(
                                        [ValueSource(typeof(FurtherLendingSuperLoSequentialData), "Identifier")] string identifier,
                                        [ValueSource(typeof(FurtherLendingSuperLoSequentialData), "AccountKey")] int accountKey,
                                        [ValueSource(typeof(FurtherLendingSuperLoSequentialData), "TestGroup")] string testGroup
                                                        )
        {
            Helper.CreateFurtherLendingApplications(identifier, accountKey, base.Browser);
        }

        /// <summary>
        /// Performs the application received actions for the the Super Lo Opt test cases
        /// </summary>
        /// <param name="identifier"></param>
        [Test, Sequential, Description("Performs the application received actions for the the Super Lo Opt test cases"), Category("Super Lo Opt Out")]
        public void _002_SuperLoOptOptApplicationReceived([Values(
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent2,
                                                              FurtherLendingTestCases.SuperLoSPVChange1,
                                                              FurtherLendingTestCases.SuperLoNoOptOut1
                                                              )] string identifier)
        {
            Helper.ApplicationReceived(identifier, base.Browser);
        }

        /// <summary>
        /// Performs the QA Complete action for the Super Lo Opt Out cases
        /// </summary>
        /// <param name="identifier"></param>
        [Test, Sequential, Description("Ensures that a FL Processor can QA Complete an application, moving it into Manage Application"), Category("Super Lo Opt Out")]
        public void _004_SuperLoQAComplete([Values(
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent2,
                                                              FurtherLendingTestCases.SuperLoSPVChange1,
                                                              FurtherLendingTestCases.SuperLoNoOptOut1
                                               )] string identifier)
        {
            Console.WriteLine(String.Format(@"--********{0}********--: {1}", MethodBase.GetCurrentMethod(), identifier));
            Helper.QAComplete(identifier, base.Browser);
        }

        /// <summary>
        /// These further advances are all on Super Lo accounts and will need to be sent into Credit in order to be
        /// approved. After approval certain of the cases will be sent to the Require Super Lo Opt Out state in order
        /// opted out of the Super Lo product.
        /// </summary>
        /// <param name="identifier"></param>
        [Test, Sequential, Description(@"These further advances are all on Super Lo accounts and will need to be sent into Credit in order to be
						approved. After approval certain of the cases will be sent to the Require Super Lo Opt Out state in order
						opted out of the Super Lo product."), Category("Super Lo Opt Out")]
        public void _005_ApplicationInOrderSuperLoOptOut([Values(
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent2,
                                                              FurtherLendingTestCases.SuperLoSPVChange1,
                                                              FurtherLendingTestCases.SuperLoNoOptOut1
                                                                 )] string identifier)
        {
            var testCase = Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            //update the loan conditions
            Helper.SaveLoanConditions(base.Browser, testCase.OfferKey);
            //complete the LE CBO node requirements
            LegalEntityCBONode.CompleteLegalEntityNode(base.Browser, testCase.OfferKey, true, false, false, false, false, false, false, false);
            //we need to clean up the further lending offer data
            Service<IFurtherLendingService>().CleanUpOfferData(testCase.OfferKey);
            //navigate to the doc checklist
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().DocumentChecklist();
            base.Browser.Navigate<BuildingBlocks.Navigation.DocumentCheckListNode>().ViewDocumentChecklist(NodeTypeEnum.Update);
            base.Browser.Page<DocumentCheckListUpdate>().UpdateDocumentChecklist();
            //perform the action
            base.Browser.ClickWorkflowLoanNode(Workflows.ApplicationManagement);
            base.Browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<ICommonService>().InsertTestMethod("_005_ApplicationInOrderSuperLoOptOut()", identifier, "FurtherLendingTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_005_ApplicationInOrderSuperLoOptOut()", identifier, ParameterTypeEnum.OfferKey, testCase.OfferKey.ToString());
        }

        /// <summary>
        /// Runs the test assertions and data table updates for the Further Lending Test Cases that have been sent to credit
        /// </summary>
        /// <param name="testMethod"></param>
        /// <param name="identifier"></param>
        [Test, Sequential, Description("Ensures that the further lending cases sent to credit have had their cases created correctly in Credit"),
                                         Category("Super Lo Opt Out")]
        public void _006_AssertFurtherLendingSuperLoCases([Values(
                                                                  "_005_ApplicationInOrderSuperLoOptOut()",
                                                                  "_005_ApplicationInOrderSuperLoOptOut()",
                                                                  "_005_ApplicationInOrderSuperLoOptOut()",
                                                                  "_005_ApplicationInOrderSuperLoOptOut()"
                                                                  )] string testMethod,
                                                          [Values(
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                              FurtherLendingTestCases.SuperLoGreaterThan85Percent2,
                                                              FurtherLendingTestCases.SuperLoSPVChange1,
                                                              FurtherLendingTestCases.SuperLoNoOptOut1
                                                                  )] string identifier)
        {
            Helper.AssertFurtherLendingCreditCases(testMethod, identifier);
        }

        /// <summary>
        /// Once the credit user has approved a super lo further advance case that has an effective LTV greater
        ///than 85% it should be moved to the Require Super Lo Opt Out state in the Readvance Payments workflow
        /// </summary>
        [Test, Sequential, Description(@"Once the credit user has approved a super lo further advance case that has an effective LTV greater
        than 85% it should be moved to the Require Super Lo Opt Out state in the Readvance Payments workflow"),
                                         Category("Super Lo Opt Out")]
        public void _007_ApproveSuperLoOptOut([Values(
                                                  FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                  FurtherLendingTestCases.SuperLoGreaterThan85Percent2
                                                  )] string identifier)
        {
            //search for the case
            var browser = new TestBrowser(TestUsers.CreditSupervisor);
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = r.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            int employmentTypeKey = Service<IApplicationService>().GetApplicationEmploymentType(offerKey);
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(employmentTypeKey);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);

            string FLSupervisor = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisor);
            browser.Page<FurtherLendingApprove>().Approve();
            //we need to save the test method details
            Service<ICommonService>().InsertTestMethod("_007_ApproveSuperLoOptOut", identifier, "ReadvancePaymentTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_007_ApproveSuperLoOptOut", identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
            Service<ICommonService>().SaveTestMethodParameters("_007_ApproveSuperLoOptOut", identifier, ParameterTypeEnum.State,
                WorkflowStates.ReadvancePaymentsWF.RequireSuperLoOptOut);
            Service<ICommonService>().SaveTestMethodParameters("_007_ApproveSuperLoOptOut", identifier, ParameterTypeEnum.ADUserName,
                FLSupervisor);
            browser.Dispose();
        }

        /// <summary>
        /// Once the credit user has approved a super lo opt further advance case that will result in the
        /// account being moved to a Blue Banner SPV, it should be moved to the Require Super Lo Opt Out state in the Readvance Payments workflow
        /// </summary>
        [Test, Sequential, Description(@"Once the credit user has approved a super lo opt further advance case that will result in the
        account being moved to a Blue Banner SPV, it should be moved to the Require Super Lo Opt Out state in the Readvance Payments workflow"),
                                         Category("Super Lo Opt Out")]
        public void _008_ApproveSuperLoOptOutSPVChange([Values(
                                                           FurtherLendingTestCases.SuperLoSPVChange1
                                                           )] string identifier)
        {
            //search for the case
            var browser = new TestBrowser(TestUsers.CreditSupervisor);
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = r.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            int employmentTypeKey = Service<IApplicationService>().GetApplicationEmploymentType(offerKey);
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(employmentTypeKey);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            string FLSupervisor = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisor);
            browser.Page<FurtherLendingApprove>().Approve();
            //we need to save the test method details
            Service<ICommonService>().InsertTestMethod("_008_ApproveSuperLoOptOutSPVChange", identifier, "ReadvancePaymentTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_008_ApproveSuperLoOptOutSPVChange", identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
            Service<ICommonService>().SaveTestMethodParameters("_008_ApproveSuperLoOptOutSPVChange", identifier, ParameterTypeEnum.State,
                WorkflowStates.ReadvancePaymentsWF.RequireSuperLoOptOut);
            Service<ICommonService>().SaveTestMethodParameters("_008_ApproveSuperLoOptOutSPVChange", identifier, ParameterTypeEnum.ADUserName,
                FLSupervisor);
            browser.Dispose();
        }

        /// <summary>
        /// "Once the credit user has approved a super lo further advance case that has an effective LTV less than 85%
        /// and will not result in the client being moved to Blue Banner then the case will be sent to the Contact Client state
        /// </summary>
        [Test, Sequential, Description(@"Once the credit user has approved a super lo further advance case that has an effective LTV less than 85%
        and will not result in the client being moved to Blue Banner then the case will be sent to the Contact Client state "),
                                         Category("Super Lo Opt Out")]
        public void _009_ApproveSuperLoOptOutNotRequired([Values(
                                                             FurtherLendingTestCases.SuperLoNoOptOut1
                                                             )] string identifier)
        {
            Console.WriteLine(String.Format(@"--********{0}********--", MethodBase.GetCurrentMethod()));
            //search for the case
            var browser = new TestBrowser(TestUsers.CreditSupervisor);
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = r.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            string flProcessor = r.Rows(0).Column("AssignedFLAppProcUser").Value;
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            int employmentTypeKey = Service<IApplicationService>().GetApplicationEmploymentType(offerKey);
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(employmentTypeKey);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            browser.Page<FurtherLendingApprove>().Approve();
            //we need to save the test method details
            Service<ICommonService>().InsertTestMethod("_009_ApproveSuperLoOptOutNotRequired", identifier, "ReadvancePaymentTests");
            //then store our params
            Service<ICommonService>().SaveTestMethodParameters("_009_ApproveSuperLoOptOutNotRequired", identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
            Service<ICommonService>().SaveTestMethodParameters("_009_ApproveSuperLoOptOutNotRequired", identifier, ParameterTypeEnum.State,
                                            WorkflowStates.ReadvancePaymentsWF.ContactClient);
            Service<ICommonService>().SaveTestMethodParameters("_009_ApproveSuperLoOptOutNotRequired", identifier, ParameterTypeEnum.ADUserName, flProcessor);
            browser.Dispose();
        }

        /// <summary>
        /// This test runs the assertions for the super lo opt out cases once they have been approved from credit. It ensures that:
        /// The greater than 85% LTV and SPV changes to Blue Banner cases have been moved correctly to the Require Super Lo Opt Out
        /// state and are assigned to a FL Supervisor. The super lo applications that do not meet the above criteria should have been
        /// moved to the Contact Client state and are assigned to a FL Processor.
        /// </summary>
        /// <param name="testMethod">Test Method</param>
        /// <param name="identifier">Test Identifier</param>
        [Test, Sequential, Description("Asserts the case movement has correctly occurred for the Super Lo Opt Out test cases"),
                                         Category("Super Lo Opt Out")]
        public void _010_AssertSuperLoOptOut([Values(
                                                 "_007_ApproveSuperLoOptOut",
                                                 "_007_ApproveSuperLoOptOut",
                                                 "_008_ApproveSuperLoOptOutSPVChange",
                                                 "_009_ApproveSuperLoOptOutNotRequired"
                                                 )] string testMethod,
                                             [Values(
                                                 FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                 FurtherLendingTestCases.SuperLoGreaterThan85Percent2,
                                                 FurtherLendingTestCases.SuperLoSPVChange1,
                                                 FurtherLendingTestCases.SuperLoNoOptOut1
                                                 )] string identifier
         )
        {
            Console.WriteLine(String.Format(@"--********{0}********--{1}", MethodBase.GetCurrentMethod(), testMethod));
            int offerKey = Service<ICommonService>().GetTestMethodParameters<int>(testMethod, identifier, ParameterTypeEnum.OfferKey);
            string state = Service<ICommonService>().GetTestMethodParameters<string>(testMethod, identifier, ParameterTypeEnum.State);
            string user = Service<ICommonService>().GetTestMethodParameters<string>(testMethod, identifier, ParameterTypeEnum.ADUserName);
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, state);
            //case is no longer in credit
            Service<IFurtherLendingService>().UpdateFLAutomation("Credit", "0", identifier);
            Service<IFurtherLendingService>().UpdateFLAutomation("ReadvancePayments", "1", identifier);
            //assert the user
            switch (testMethod)
            {
                case "_009_ApproveSuperLoOptOutNotRequired":
                    AssignmentAssertions.AssertWorkflowAssignment(user, offerKey, OfferRoleTypeEnum.FLProcessorD);
                    break;

                case "_007_ApproveSuperLoOptOut":
                case "_008_ApproveSuperLoOptOutSPVChange":
                    AssignmentAssertions.AssertWorkflowAssignment(user, offerKey, OfferRoleTypeEnum.FLSupervisorD);
                    Service<IFurtherLendingService>().UpdateFLAutomation("AssignedFLSupervisor", user, identifier);
                    break;
            }
            //remove the assertion records from the db
            Service<ICommonService>().DeleteTestMethodData(testMethod, identifier);
        }

        /// <summary>
        /// Attempting to cancel the super lo opt for a further advance application that requires the opt
        /// should not be allowed and the reason(s) for the opt out being required should be displayed.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        [Test, Sequential, Description(@"Attempting to cancel the super lo opt for a further advance application that requires the opt
                           should not be allowed and the reason(s) for the opt out being required should be displayed."),
                                         Category("Super Lo Opt Out")]
        public void _011_CancelSuperLoOptOutRequired([Values(
                                                         FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                         FurtherLendingTestCases.SuperLoSPVChange1
                                                         )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            //these assertions are to ensure that the warning message is displayed when entering the state
            switch (identifier)
            {
                case FurtherLendingTestCases.SuperLoGreaterThan85Percent1:
                    base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        "The effective LTV of this case is greater than 85%. Please opt this client out of Super Lo.");
                    break;

                case FurtherLendingTestCases.SuperLoSPVChange1:
                    base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        "This application would move the Super Lo account to Blue Banner. Please opt the account out of Super Lo.");
                    break;
            }
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.CancelOptOutRequest);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false); ;
            //these assertions ensure that the case is not allowed to have the cancel request completed if it is required
            switch (identifier)
            {
                case FurtherLendingTestCases.SuperLoGreaterThan85Percent1:
                    base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        "The effective LTV of this case is greater than 85%. Please opt this client out of Super Lo.");
                    break;

                case FurtherLendingTestCases.SuperLoSPVChange1:
                    base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(
                        "This application would move the Super Lo account to Blue Banner. Please opt the account out of Super Lo.");
                    break;
            }
        }

        /// <summary>
        /// Should a user try and perform the Require Super Lo Opt Out action on an application whose product is not
        /// currently Super Lo a warning message will be displayed to the user indicating that this is not allowed.
        /// </summary>
        [Test, Description(@"Should a user try and perform the Require Super Lo Opt Out action on an application whose product is not
                currently Super Lo a warning message will be displayed to the user indicating that this is not allowed."),
                                         Category("Super Lo Opt Out")]
        public void _012_SuperLoOptRequiredVariableLoan()
        {
            //we need a case at contact client that is not a super lo case
            int offerKey = 0;
            QueryResults results = Service<IX2WorkflowService>().GetOfferKeysAtStateByType(WorkflowStates.ReadvancePaymentsWF.ContactClient, Workflows.ReadvancePayments,
                                                  OfferTypeEnum.FurtherAdvance, "FLAutomation");

            offerKey = (from r in results
                        where r.Column("Product").Value != "Super Lo"
                        select r.Column("offerkey").GetValueAs<int>()).FirstOrDefault();
            if (offerKey == 0)
                Assert.Ignore("No application found for test");

            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            //ntu the case
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.NTU);
            base.FLSupervisorBrowser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            //reload the case
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            //reinstate
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.ReinstateNTU);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            //reload the case
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            //opt out
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.OptOutSuperLo);
            base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists("The current product on the account must be Super Lo to perform this action.");
        }

        /// <summary>
        /// Processing the super lo opt out should result in the following: The account product is changed to become
        /// a variable loan, the rate override for super lo is set to inactive and the application is reworked in order
        /// for the latest revision to have a variable loan product
        /// </summary>
        [Test, Description(@"Processing the super lo opt out should result in the following: The account product is changed to become
                            a new variable loan, the rate override for super lo is set to inactive and the application is reworked in order
                            for the latest revision to have a new variable loan product"),
                                         Category("Super Lo Opt Out")]
        public void _013_OptOutSuperLoOptOutRequired([Values(
                                                         FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                         FurtherLendingTestCases.SuperLoSPVChange1
                                                         )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.OptOutSuperLo);
            base.FLSupervisorBrowser.Page<SuperLoOptOut>().OptOutSuperLo();
            Thread.Sleep(5000);
            //check that the account product has changed
            AccountAssertions.AssertAccountInformation(testCase.AccountKey, AccountTable.RRR_ProductKey, ((int)ProductEnum.NewVariableLoan).ToString());
            //check that the rate override has been disabled
            FinancialAdjustmentAssertions.AssertFinancialAdjustment(testCase.AccountKey, FinancialAdjustmentTypeSourceEnum.SuperLo_DifferentialProvision,
                FinancialAdjustmentStatusEnum.Canceled, true);
            //ensure the offer has been revised to be a variable loan product
            OfferAssertions.AssertLatestOfferInformationProduct(testCase.OfferKey, Products.NewVariableLoan);
        }

        /// <summary>
        /// This test asserts that the test cases used for the Super Lo opt out have been correctly assigned into the Credit workflow
        /// once the opt out has been processed. The test will then update the Credit and Readvance Payment indicators in the master table
        /// so that the test framework knows at where these cases are. The assigned credit user is also updated for future reference.
        /// </summary>
        /// <param name="identifier"></param>
        [Test, Description(@"Once the application's account has been opted out of Super Lo the case should be back in the
                        Credit Workflow"), Category("Super Lo Opt Out")]
        public void _014_OptOutSuperLoCreditAssignment([Values(
                                                           FurtherLendingTestCases.SuperLoGreaterThan85Percent1,
                                                           FurtherLendingTestCases.SuperLoSPVChange1
                                                           )] string identifier)
        {
            Console.WriteLine(String.Format(@"--********{0}********--{1}", MethodBase.GetCurrentMethod(), identifier));
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = r.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
            //case is no longer in readvance payments
            Service<IFurtherLendingService>().UpdateFLAutomation("Credit", "1", identifier);
            Service<IFurtherLendingService>().UpdateFLAutomation("ReadvancePayments", "0", identifier);
            string creditAdUser;
            OfferRoleTypeEnum creditOfferRoleType;
            Service<IFurtherLendingService>().UpdateAssignedCreditUser(offerKey, identifier, out creditAdUser, out creditOfferRoleType);
            Service<IFurtherLendingService>().UpdateFLAutomation("AssignedCreditUser", creditAdUser, identifier);
            AssignmentAssertions.AssertWorkflowAssignment(creditAdUser, offerKey, creditOfferRoleType);
            r.Dispose();
        }

        #endregion Tests
    }
}