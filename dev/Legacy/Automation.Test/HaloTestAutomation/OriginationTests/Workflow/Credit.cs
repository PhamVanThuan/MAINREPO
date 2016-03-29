using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;
using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;
using Description = NUnit.Framework.DescriptionAttribute;
using Navigation = BuildingBlocks.Navigation;
using System.Linq;
using WorkflowAutomation.Harness;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments;

namespace Origination.Workflow
{
    /// <summary>
    /// Contains tests for the Credit portion of Origination
    /// </summary>
    [TestFixture, RequiresSTA]
    public class _04Credit : OriginationTestBase<BasePage>
    {
        private TestBrowser browser;        
        private const string TestIdentifierColumn = @"CreditTestID";
        private IEnumerable<Automation.DataModels.LegalEntityReturningDiscountQualifyingData> legalEntityReturningDiscountQualifyingData;
        private Automation.DataModels.OriginationTestCase testCase;
        
        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (browser != null)
            {
                try
                {
                    browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    browser.Dispose();
                    browser = null;
                }
            }
        }

        #region Tests
        public IEnumerable<Automation.DataModels.OriginationTestCase> GetTestCasesForApplicationInOrder()
        {
            var testCases = Service<ICommonService>().GetOriginationTestCases();
            return (from t in testCases where t.ApplicationManagementTestGroup == "ApplicationInOrder" select t);
        }
        /// <summary>
        /// Approves the applications sent through by our PreCredit test fixture.
        /// </summary>
        [Test, TestCaseSource(typeof(_04Credit), "GetTestCasesForApplicationInOrder"), Description("Approves the applications sent through by our PreCredit test fixture.")]
        public void _000_ApproveApplication(Automation.DataModels.OriginationTestCase testCase)
        {
            int offerKey = Service<ICommonService>().GetOfferKeyFromTestSchemaTable("OffersAtApplicationCapture", "TestIdentifier", testCase.TestIdentifier);
            QueryResults results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
            Assert.True(results.HasResults, "There is no credit instance details for offerkey: {0}", offerKey);

            //Get the Credit state
            string creditState = results.Rows(0).Column("StateName").Value;
            if (creditState != WorkflowStates.CreditWF.Credit)
            {
                Logger.LogAction(string.Format(@"Application {0} is not at the {1} state in the {2} workflow", offerKey, WorkflowStates.CreditWF.Credit,
                    Workflows.Credit));
            }
            else
            {
                ApproveApplication(browser, offerKey, TestUsers.CreditUnderwriter, testCase.LoanType);
                results.Dispose();
                results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);

                if (results.Rows(0).Column("StateName").Value == WorkflowStates.CreditWF.Credit)
                {
                    //our case is still in Credit, login as a supervisor to approve
                    ApproveApplication(browser, offerKey, TestUsers.CreditSupervisor, testCase.LoanType);
                }
                results.Dispose();
                results = Service<IX2WorkflowService>().GetCreditInstanceDetails(offerKey);
                if (results.Rows(0).Column("StateName").Value == WorkflowStates.CreditWF.Credit)
                {
                    //our case is still in Credit, login as a supervisor to approve
                    ApproveApplication(browser, offerKey, TestUsers.CreditExceptions, testCase.LoanType);
                }
            }
            //case should be at LOA
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Username, offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Check that a Credit Underwriter Consultant can perform the Escalate to Supervisor action.  Ensure that the application is assigned to a Credit Supervisor Consultant
        /// </summary>
        [Test, Description(@"Check that a Credit Underwriter Consultant can perform the Escalate to Supervisor action.  Ensure that the application is assigned to a Credit Supervisor Consultant")]
        [Ignore]
        public void _001_ReferSeniorAnalystTest()
        {
            QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.Credit, @"CUUser",
                (int)OfferTypeEnum.NewPurchase, (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance);

            var offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string assignedUser = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(assignedUser, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ReferSeniorAnalyst);
            browser.Page<WF_ReAssignSeniorCreditAnalyst>().SelectConsultantFromDropdownAndCommit(TestUsers.CreditSupervisor,
                WF_ReAssignSeniorCreditAnalyst.btn.Submit);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditSupervisorD, TestUsers.CreditSupervisor);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditUnderwriterD, assignedUser);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
        }

        /// <summary>
        /// Check that a Credit Underwriter Consultant can perform the Escalate to Manager action.  Ensure that the application is assigned to a Credit Manager Consultant
        /// </summary>
        [Test, Description(@"Check that a Credit Underwriter Consultant can perform the Escalate to Manager action.  Ensure that the application is assigned to a Credit Manager Consultant")]
        [Ignore]
        public void _002_EscalateToManagerTest()
        {
            QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.Credit, @"CUUser",
                (int)OfferTypeEnum.NewPurchase, (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance);
            var offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string assignedUser = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(TestUsers.CreditManager, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.EscalatetoMgr);
            browser.Page<WF_ReAssign>().SelectConsultantFromDropdownAndCommit(TestUsers.CreditManager);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditManagerD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditManagerD, TestUsers.CreditManager);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditManagerD);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditUnderwriterD, assignedUser);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
        }

        /// <summary>
        /// Check that a Credit Underwriter Consultant can perform the Escalate to Exceptions Manager action.  Ensure that the application is assigned to a Credit Exceptions Consultant
        /// </summary>
        [Test, Description(@"Check that a Credit Underwriter Consultant can perform the Escalate to Exceptions Manager action.  Ensure that the application is assigned to a Credit Exceptions Consultant")]
        [Ignore]
        public void _003_EscalateToExceptionsManagerTest()
        {
            QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.Credit, @"CUUser", (int)OfferTypeEnum.NewPurchase,
                (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance);
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            string assignedUser = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(TestUsers.CreditExceptions, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.EscalateToExceptionsMgr);
            browser.Page<WF_ReAssign>().SelectConsultantFromDropdownAndCommit(TestUsers.CreditExceptions);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditExceptionsD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditExceptionsD, TestUsers.CreditExceptions);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditExceptionsD);

            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, OfferRoleTypeEnum.CreditUnderwriterD, assignedUser);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsInactive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
        }

        /// <summary>
        /// Check that it is possible to perform the Decline Application action.  Ensure the application moves to the Archive Decline by Credit state
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Decline Application action")]
        [Ignore]
        public void _013_DeclineApplication()
        {
            int offerKey = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.Credit, "cuuser", (int)OfferTypeEnum.NewPurchase,
                (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance).Rows(0).Column("OfferKey").GetValueAs<int>();
            string username = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit).Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DeclineApplication);
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.CreditDecline);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ArchiveDeclinedbyCredit);
        }

        /// <summary>
        /// Check that it is possible to perform the Agree with Decision action.  Ensure the application moves to the LOA state and is assigned
        /// to a Branch Consultant user
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Agree with Decision action")]
        [Ignore]
        public void _014_AgreeWithDecision()
        {
            int offerKey = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.ReviewDecision, "csuser", (int)OfferTypeEnum.NewPurchase,
                (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance).Rows(0).Column("OfferKey").GetValueAs<int>();
            string username = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.ReviewDecision).Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.ReviewDecision);
            browser.ClickAction(WorkflowActivities.Credit.Agreewithdecision);
            browser.Page<WorkflowYesNo>().Confirm(true, false);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ArchiveAprAndOfferApp);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Check that it is possible to perform the Override Decision action.  Ensure the application moves to the Credit state
        /// </summary>
        [Test, Description("Check that it is possible to perform the Override Decision action")]
        [Ignore]
        public void _015_OverrideDecision()
        {
            int offerKey = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.ReviewDecision, "csuser", (int)OfferTypeEnum.NewPurchase,
                (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance).Rows(0).Column("OfferKey").GetValueAs<int>();
            string username = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.ReviewDecision).Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.ReviewDecision);

            browser.ClickAction(WorkflowActivities.Credit.OverrideDecision);
            browser.Page<WorkflowYesNo>().Confirm(true, false);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Description("")]
        [Ignore]
        public void _016_ExceptionsDeclineWithOffer()
        {
            QueryResults results = Service<IX2WorkflowService>().GetWorkflowInstanceForStateADUserAndOfferType(WorkflowStates.CreditWF.Credit, "ceuser", (int)OfferTypeEnum.NewPurchase,
                (int)OfferTypeEnum.SwitchLoan, (int)OfferTypeEnum.Refinance);
            int offerKey = results.Rows(0).Column("OfferKey").GetValueAs<int>();
            int offerTypeKey = results.Rows(0).Column("OfferTypeKey").GetValueAs<int>();
            string username = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit).Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ExceptionsDeclinewithOffer);
            switch (offerTypeKey)
            {
                case (int)OfferTypeEnum.SwitchLoan:
                case (int)OfferTypeEnum.Refinance:
                    if (!browser.Page<ApplicationLoanDetailsApprove>().IsQCDeclined())
                    {
                        browser.Page<ApplicationLoanDetailsApprove>().QCDecline();
                        browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QuickCashDecline);
                    }
                    break;
            }
            base.Browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Assert that when saved, an application with no active valuation in the last 12 months, remains in the Credit stage of the Credit workflow.
        /// Also assert that when saved, the error message "Active valuation not conducted within the last 12 months." is displayed.
        /// </summary>
        [Test, Description("Assert that when saved, an application with no active valuation in the last 12 months, remains in the Credit stage of the Credit workflow. Also assert that when saved, the error message 'Active valuation not conducted within the last 12 months.' is displayed.")]
        public void _021_when_performing_approve_application_action_and_valuation_older_than_12_months()
        {
            // Get Offer Key
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ValuationGreaterThan12MonthsOld_NewPurchase", TestIdentifierColumn);

            // Login as Credit Underwriter
            browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);

            // Perform the application approval
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //perform the Approve Application action
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            // Assert that the error message "Active valuation not conducted within the last 12 months." is displayed
            const string errMsg = "Active valuation not conducted within the last 12 months.";
            browser.Page<BasePageAssertions>().AssertValidationMessageExists(errMsg);
            browser.Page<BasePageAssertions>().AssertValidationIsError();

            // Assert that the application remains in the Credit stage of the workflow
            BuildingBlocks.Assertions.X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.Credit, offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// #16957 - New Purchase application for a property with an existing valuation older than 12 months.
        /// The Valuation request was Archived by the Valuations Manager.
        /// The Application in Order action has been performed without a Valuation being performed
        /// When the Decline with Offer action is performed from credit
        /// Expect an error message "Active valuation not conducted within the last 12 months." to be displayed
        /// </summary>
        [Test, Description("When performing the Decline with Offer action on an application with a valuation older than 12 months expect an error message to display")]
        public void _022_when_performing_decline_with_offer_action_and_valuation_older_than_12_months()
        {
            //get offerkey from test.OffersAtApplicationCapture use TestIdentifier = 'ValuationGreaterThan12MonthsOld_NewVariable_OverrideCheck'
            var offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier("ValuationGreaterThan12MonthsOld_NewPurchase");
            //login with Credit Underwriter (cuuser)
            var browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.DeclinewithOffer);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //assert that the error message "Active valuation not conducted within the last 12 months." is displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Active valuation not conducted within the last 12 months.");
            //i'm not sure if this has been done but try assert that there is no yes/no button on the error message
            browser.Page<BasePageAssertions>().AssertValidationIsError();
            //assert that the case is still at the Credit stage in the Credit workflow
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// #16957 - New Purchase application for a property with an existing valuation older than 12 months.
        /// The Valuation request was Archived by the Valuations Manager.
        /// The Application in Order action has been performed without a Valuation being performed
        /// When the Approve with Pricing Changes action is performed from credit
        /// Expect an error message "Active valuation not conducted within the last 12 months." to be displayed
        /// </summary>
        [Test, Description("When performing the Approve with Pricing Changes action on an application with a valuation older than 12 months expect an error message to display")]
        public void _023_when_performing_approve_with_pricing_changes_action_and_valuation_older_than_12_months()
        {
            //get offerkey from test.OffersAtApplicationCapture use TestIdentifier = 'ValuationGreaterThan12MonthsOld_NewVariable_OverrideCheck'
            var offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier("ValuationGreaterThan12MonthsOld_NewPurchase");
            //login with Credit Underwriter (cuuser)
            var browser = new TestBrowser(TestUsers.CreditUnderwriter, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //perform the Approve Application action
            browser.ClickAction(WorkflowActivities.Credit.ApprovewithPricingChanges);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //assert that the error message "Active valuation not conducted within the last 12 months." is displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Active valuation not conducted within the last 12 months.");
            //i'm not sure if this has been done but try assert that there is no yes/no button on the error message
            browser.Page<BasePageAssertions>().AssertValidationIsError();
            //assert that the case is still at the Credit stage in the Credit workflow
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// #16957 - New Purchase application for a property with an existing valuation older than 12 months.
        /// The Valuation request was Archived by the Valuations Manager.
        /// The Application in Order action has been performed without a Valuation being performed
        /// When the Exceptions Decline with Offer action is performed from credit
        /// Expect an error message "Active valuation not conducted within the last 12 months." to be displayed
        /// </summary>
        [Test, Description("When performing the Exceptions Decline with Offer action on an application with a valuation older than 12 months expect an error message to display")]
        public void _024_when_performing_exceptions_decline_with_offer_action_and_valuation_older_than_12_months()
        {
            //get offerkey from test.OffersAtApplicationCapture use TestIdentifier = 'ValuationGreaterThan12MonthsOld_NewVariable_OverrideCheck'
            var offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier("ValuationGreaterThan12MonthsOld_NewPurchase_Jumbo");
            //login with Credit Underwriter (cuuser)
            var browser = new TestBrowser(TestUsers.CreditExceptions, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ExceptionsDeclinewithOffer);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //assert that the error message "Active valuation not conducted within the last 12 months." is displayed
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Active valuation not conducted within the last 12 months.");
            //i'm not sure if this has been done but try assert that there is no yes/no button on the error message
            browser.Page<BasePageAssertions>().AssertValidationIsError();
            //assert that the case is still at the Credit stage in the Credit workflow
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// When approving application, find a case with all incomes confirmed and expect no error messages
        /// </summary>
        [Test, Description("When approving application, find a case with all incomes confirmed and expect no error messages")]
        public void _025_when_performing_approve_with_all_confirmed_incomes()
        {
            var date = DateTime.Now;
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.CreditWF.Credit, Workflows.Credit, OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            var legalentity = Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(offerKey);
            Service<IEmploymentService>().InsertUnconfirmedSalariedEmployment(legalentity);
            Service<IEmploymentService>().ConfirmAllEmployment(offerKey);
            //login with Credit Underwriter (cuuser)
            var browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //perform the Approve Application action
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey).Rows(0).Column("ID").GetValueAs<int>();

            //need to update the valuation to be less than 12 months old.
            Service<IValuationService>().UpdateValuationDateToDateLessThan12MonthsAgo(offerKey);

            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            browser.Page<BasePage>().DomainWarningClickYes();
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(instanceID, 1, date.ToString(Formats.DateTimeFormatSQL), WorkflowActivities.ApplicationManagement.NewBusiness);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);
        }

        /// <summary>
        /// When approving application, find case with confirmed and unconfirmed income and expect an error message to display
        /// </summary>
        [Test, Description("When approving application, find case with confirmed and unconfirmed income and expect an error message to display")]
        public void _026_when_performing_approve_with_confirmed_and_uconfirmed_income()
        {
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.CreditWF.Credit, Workflows.Credit, OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation);
            Service<IEmploymentService>().ConfirmAllEmployment(offerKey);
            var legalentity = Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(offerKey);
            Service<IEmploymentService>().InsertUnconfirmedSalariedEmployment(legalentity);
            //login with Credit Underwriter (cuuser)
            var browser = new TestBrowser(TestUsers.CreditSupervisor, TestUsers.Password);
            //perform the Approve Application action
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //Add assertion for error message pop up, once dev is done...
            browser.Page<WorkflowYesNo>().AssertMessageDisplayed("Each contributing legal entity must have confirmed basic income.");
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
        }

        /// <summary>
        /// #24418 - Returning Clients get a 50 % discount on Initiation Fee
        /// The case is escalated to Exceptions Manager to apply Quick Pay option and decline with offer. 
        /// We are checking that when the case has Quick Pay selected the initiation fee is 2.5% of the loan amount regardless of whether a discount is applied or not.
        /// </summary>
        [Test, Description("When performing the Exceptions Decline with Offer action on an application with quick pay selected")]
        public void _027_when_performing_exceptions_decline_with_offer_action_and_quick_pay_is_selected()
        {
            //sets the loan amount to filter for cases with more than 1 mil
            int LoanAmount = 1000000;
            var offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByTypeAndLoanAmount(WorkflowStates.CreditWF.Credit, Workflows.Credit, OfferTypeEnum.NewPurchase, Exclusions.OrginationAutomation, LoanAmount);
            var legalentity = Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(offerKey);
            Service<IEmploymentService>().InsertUnconfirmedSalariedEmployment(legalentity);
            Service<IEmploymentService>().ConfirmAllEmployment(offerKey);
            Service<IEmploymentService>().ConfirmSuretorEmployment(offerKey);
            //login with Credit Exceptions (ceuser)
            var browser = new TestBrowser(TestUsers.CreditExceptions, TestUsers.Password);
            //perform the Search 
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //Perform the Exceptions Decline with offer action
            browser.ClickAction(WorkflowActivities.Credit.ExceptionsDeclinewithOffer);
            //Tick the Quick pay option
            browser.Page<ApplicationLoanDetailsApprove>().ApplyQuickPay();
            //Save
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //Handle the warming pop up if it appears
            browser.Page<BasePage>().DomainWarningClickYes();
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey).Rows(0).Column("InstanceID").GetValueAs<long>();
            //assert that the case is still has moved to the LOA state in Application management workflow
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.NewBusiness,instanceID,1);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //Gets the Loan agreement amount so that we can compare the initiation fees.
            double LoanAgreementAmount = Service<IApplicationService>().GetLoanAgreementAmount(offerKey);
            double expectedAmount = LoanAgreementAmount * 0.025;
            OfferAssertions.AssertOfferExpense(offerKey, expectedAmount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        [Test, Description("When performing the Exceptions Decline with Offer action on an application with a valuation older than 12 months expect an error message to display")]
        public void _028_when_performing_exceptions_decline_with_offer_action_and_quick_pay_is_selected_with_discounted_case()
        {
            //Gets all the legalentity data 
            legalEntityReturningDiscountQualifyingData = Service<ILegalEntityService>().GetLegalEntityReturningDiscountQualifyingDataWithValidIDNumber();
            //Set up the application capture data
            testCase = new Automation.DataModels.OriginationTestCase()
            {
                Product = Products.NewVariableLoan,
                MarketValue = "2000000",
                CashDeposit = "500000",
                EmploymentType = EmploymentType.Salaried,
                Term = "240",
                HouseHoldIncome = "50000"
            };

            var returningMainApplicantInitiationFeeDiscount = ControlNumeric.InitiationFee * ControlNumeric.ReturningMainApplicantInitiationFeeDiscount;

            browser = new TestBrowser(TestUsers.BranchConsultant);
            //Enter the info into the calculator and proceed
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().gotoApplicationCalculator(browser);
            browser.Page<Views.LoanCalculator>().LoanCalculatorLead_NewPurchase(testCase.Product, testCase.MarketValue, testCase.CashDeposit, testCase.EmploymentType, testCase.Term,
                              testCase.HouseHoldIncome, ButtonTypeEnum.CreateApplication);
            //Filter out what legalentity you want from the above set. In this case we want someone that qualifies for an initiation fee discount
            var legalEntity = (from le in legalEntityReturningDiscountQualifyingData
                               where le.MainApplicantOpenAccountsCount == 1 && le.ReturningClientDiscountOpenAccountsCount == 0
                               select le).FirstOrDefault();
            //add this legal entity to the case above and get the offerkey associated to it.
            browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(legalEntity.IDNumber);
            int offerKey = browser.Page<ApplicationSummaryBase>().GetOfferKey();
            OfferAssertions.AssertOfferRoleAttributeExists(offerKey, legalEntity.LegalEntityKey, Common.Enums.OfferRoleAttributeTypeEnum.ReturningClient, true);
            //Submit
            //browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationCapture, "SubmitApplication", offerKey);
            Service<IX2WorkflowService>().WaitForAppManCaseCreate(offerKey);
            OfferAssertions.AssertOfferAttributeExists(offerKey, Common.Enums.OfferAttributeTypeEnum.DiscountedInitiationFee_ReturningClient, true);
            OfferAssertions.AssertOfferExpense(offerKey, returningMainApplicantInitiationFeeDiscount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
            System.Threading.Thread.Sleep(2000);
            //Move case from QA to Manage Application
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.ApplicationManagement, "QAToManageApplication", offerKey);
            //Using the helper to move case from Manage Application to Credit.
            Helper.ProcessFromManageApplication(base.scriptEngine, offerKey, OfferTypeEnum.NewPurchase, testCase.MarketValue);
            //Dispose the browser as we are now logging in with new user
            browser.Dispose();

            browser = new TestBrowser(TestUsers.CreditExceptions, TestUsers.Password);
            //perform the Search for case
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //Perform the Exceptions Decline with offer action
            browser.ClickAction(WorkflowActivities.Credit.ExceptionsDeclinewithOffer);
            //Tick the Quick pay option
            browser.Page<ApplicationLoanDetailsApprove>().ApplyQuickPay();
            //Save
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
            //Handle the warming pop up if it appears
            browser.Page<BasePage>().DomainWarningClickYes();
            var instanceID = Service<IX2WorkflowService>().GetAppManInstanceDetails(offerKey).Rows(0).Column("InstanceID").GetValueAs<long>();
            //assert that the case is still has moved to the LOA state in Application management workflow
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.ApplicationManagement.NewBusiness, instanceID, 1);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);
            //Gets the Loan agreement amount so that we can compare the initiation fees.
            double LoanAgreementAmount = Service<IApplicationService>().GetLoanAgreementAmount(offerKey);
            double expectedAmount = LoanAgreementAmount * 0.025;
            OfferAssertions.AssertOfferExpense(offerKey, expectedAmount, false, ExpenseTypeEnum.InitiationFeeBondPreparationFee);
        }

        [Test, Description("Checks that the expected values are written to the database")]
        public void _29_WhenUpdatingAffordabilityAssessmentExpectedValuesAreWrittenToTheDB()
        {
            int offerKey = base.Service<IApplicationService>().GetOfferByOfferTypeAndWorkflowState((int)OfferTypeEnum.NewPurchase, WorkflowStates.CreditWF.Credit);
            base.Service<IApplicationService>().InsertAffordabilityAssessment((int)AffordabilityAssessmentStatusKey.Unconfirmed, offerKey);

            base.Browser = new TestBrowser(TestUsers.CreditSupervisor2);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.Navigate<AffordabilityAssessmentNode>().ClickAffordabilityAssessments();

            var affordabilityAssessmentKey = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(offerKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).FirstOrDefault().AffordabilityAssessmentKey;
            string affordabilityAssessmentContributors = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentContributorsByAffordabilityAssessmentStatus(offerKey, (int)AffordabilityAssessmentStatusKey.Unconfirmed).ToList().FirstOrDefault().Column("AffordabilityAssessmentContributors").Value;
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
        public void _30_WhenConfirmingAnAffordabilityAssessment()
        {
            int offerKey = base.Service<IApplicationService>().GetOfferByOfferTypeAndWorkflowState((int)OfferTypeEnum.NewPurchase, WorkflowStates.CreditWF.Credit);
            base.Service<IApplicationService>().InsertAffordabilityAssessment((int)AffordabilityAssessmentStatusKey.Unconfirmed, offerKey);

            base.Browser = new TestBrowser(TestUsers.CreditSupervisor2);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            base.Browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            base.Browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.CreditWF.Credit);
            base.Browser.ClickAction(WorkflowActivities.Credit.ConfirmAffordabilityAssessment);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            
            var results = base.Service<IAffordabilityAssessmentService>().GetAffordabilityAssessmentByGenericKeyAndAssessmentStatus(offerKey, (int)AffordabilityAssessmentStatusKey.Confirmed);

            Assert.That(Convert.ToInt32(results.FirstOrDefault().AffordabilityAssessmentStatusKey).Equals((int)AffordabilityAssessmentStatusKey.Confirmed));
            StageTransitionAssertions.AssertStageTransitionCreated(offerKey, StageDefinitionStageDefinitionGroupEnum.Origination_AffordabilityAssessment_Confirm_Affordability);
        }
        #endregion Tests

        #region IgnoredTests

        //these tests are being ignored due to a rework of the origination test project that has resulted in not all of the cases these tests use being sent to Credit
        /// <summary>
        /// Check that it is possible to perform the Approve with Pricing Changes action for a New Purchase, Edge application.  Note that because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve with Pricing Changes action for a New Purchase, Edge application")]
        [Ignore]
        public void _004_ApproveWithPricingChangesNewPurchaseEdge()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveWithPricingChangesNewPurchaseEdge", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApprovewithPricingChanges);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Approve with Pricing Changes action for a Refinance, New Variable application with approved Quick Cash.
        /// Because we cannot predict if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed
        /// differently for these users, different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve with Pricing Changes action for a Refinance, New Variable application with approved Quick Cash")]
        [Ignore]
        public void _005_ApproveWithPricingChangesQCApproveRefinanceNewVariable()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveWithPricingChangesQCApproveRefinanceNewVariable", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApprovewithPricingChanges);
            double maxQC = browser.Page<ApplicationLoanDetailsApprove>().GetMaximumQC() * 0.75;
            browser.Page<ApplicationLoanDetailsApprove>().UpdateQuickCashDetails(maxQC, maxQC);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Approve with Pricing Changes action for a Switch, Varifix application with Quick Cash declined.  Because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve with Pricing Changes action for a Switch, Varifix application with Quick Cash declined")]
        [Ignore]
        public void _006_ApproveWithPricingChangesQCDeclineSwitchVarifix()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveWithPricingChangesQCDeclineSwitchVarifix", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;
            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApprovewithPricingChanges);
            browser.Page<ApplicationLoanDetailsApprove>().QCDecline();
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QuickCashDecline);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Decline with Offer action for a Refinance, Edge application with Quick Cash declined.  Because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description("Check that it is possible to perform the Decline with Offer action for a Refinance, Edge application with Quick Cash declined")]
        [Ignore]
        public void _007_DeclineWithOfferQCDeclineRefinanceEdge()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"DeclineWithOfferQCDeclineRefinanceEdge", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DeclinewithOffer);
            browser.Page<ApplicationLoanDetailsApprove>().QCDecline();
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QuickCashDecline);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Decline with Offer action for a Switch, New Variable application with approved Quick Cash.  Because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Decline with Offer action for a Switch, New Variable application with approved Quick Cash")]
        [Ignore]
        public void _008_DeclineWithOfferQCApprovedSwitchNewVariable()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"DeclineWithOfferQCApprovedSwitchNewVariable", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DeclinewithOffer);
            double maxQC = browser.Page<ApplicationLoanDetailsApprove>().GetMaximumQC() * 0.75;
            browser.Page<ApplicationLoanDetailsApprove>().UpdateQuickCashDetails(maxQC, maxQC);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Decline with Offer action for a New Purchase, Varifix application.  Because we cannot predict
        /// if the application will be assigned to a credit underwriter or credit supervisor user and that the application is processed differently for these users,
        /// different assertions will be run according to the assigned user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Decline with Offer action for a New Purchase, Varifix application")]
        [Ignore]
        public void _009_DeclineWithOfferNewPurchaseVarifix()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"DeclineWithOfferNewPurchaseVarifix", TestIdentifierColumn);
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DeclinewithOffer);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Helper.AssertionDeclineWithOfferApproveWithPricingChanges(username, offerKey);
        }

        /// <summary>
        /// Check that it is possible to perform the Approve Application action for a Refinance, Varifix application with Quick Cash declined.  Ensure the application moves to the LOA state and is assigned
        /// to a Branch Consultant user
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve Application action for a Refinance, Varifix application with Quick Cash declined")]
        [Ignore]
        public void _010_ApproveApplicationQCDeclineRefinanceVarifix()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveApplicationQCDeclineRefinanceVarifix", TestIdentifierColumn);

            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            browser.Page<ApplicationLoanDetailsApprove>().QCDecline();
            browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.QuickCashDecline);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            System.Threading.Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Check that it is possible to perform the Approve Application action for a Switch, Edge application with approved Quick Cash.
        /// Ensure the application moves to the LOA state and is assigned to a Branch Consultant user
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve Application action for a Switch, Edge application with approved Quick Cash")]
        [Ignore]
        public void _011_ApproveApplicationQCApproveSwitchEdge()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveApplicationQCApproveSwitchEdge", TestIdentifierColumn);

            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            double maxQC = browser.Page<ApplicationLoanDetailsApprove>().GetMaximumQC() * 0.75;
            browser.Page<ApplicationLoanDetailsApprove>().UpdateQuickCashDetails(maxQC, maxQC);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            System.Threading.Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Check that it is possible to perform the Approve Application action for a New Purchase, New Variable application.
        /// Ensure the application moves to the LOA state and is assigned  to a Branch Consultant user
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Approve Application action for a New Purchase, New Variable application")]
        [Ignore]
        public void _012_ApproveApplicationNewPurchaseNewVariable()
        {
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(@"ApproveApplicationNewPurchaseNewVariable", TestIdentifierColumn);

            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string username = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(username, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DeclinewithOffer);
            browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.LOA);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
        }

        /// <summary>
        /// Check that it is possible to perform the Request Further Info action.  Ensure the application moves to the Further info Request state
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the Request Further Info action.  Ensure the application moves to the Further info Request state")]
        [Ignore]
        public void _017_RequestFurtherInfo()
        {
            string testIdentifier = "RequestFurtherInfo";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier, "CreditTestID");
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);
            string aduserName = results.Rows(0).Column("ADUserName").Value;
            int offerRoleTypeKey = results.Rows(0).Column("OfferRoleTypeKey").GetValueAs<int>();

            browser = new TestBrowser(aduserName, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.RequestFurtherInfo);
            browser.Page<GenericMemoAdd>().AddMemoRecord("Unresolved", "RequestFurtherInfo Test");
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ArchiveFurtherInfo);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.FurtherInfoRequest);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);

            browser.Dispose();
            browser = null;

            browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);

            browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(browser);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.FurtherInfoRequest);

            browser.ClickAction(WorkflowActivities.ApplicationManagement.InfoRequestComplete);
            browser.Page<WorkflowYesNo>().Confirm(true, true);
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
            var offerRoleType = (OfferRoleTypeEnum)Enum.ToObject(typeof(OfferRoleTypeEnum), offerRoleTypeKey);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, offerRoleType, aduserName);
        }

        /// <summary>
        /// Check that it is possible to perform the 'Dispute Indicated' action.  Ensure the application moves to the 'Disputes' state.
        /// On completing the 'Dispute Finalised' action ensure the application moves back to the 'Credit' state and is assigned to the last assigned credit user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the 'Dispute Indicated' action.  Ensure the application moves to the 'Disputes' state.
		On completing the 'Dispute Finalised' action ensure the application moves back to the 'Credit' state and is assigned to the last assigned credit user.")]
        [Ignore]
        public void _018_DisputeIndicated()
        {
            const string testIdentifier = "DisputeIndicated";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier, "CreditTestID");
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);

            string aduserName = results.Rows(0).Column("ADUserName").Value;
            string offerRoleTypeKey = results.Rows(0).Column("OfferRoleTypeKey").Value;

            browser = new TestBrowser(aduserName, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.DisputeIndicated);
            browser.Page<GenericMemoAdd>().AddMemoRecord(MemoStatus.UnResolved, "DisputeIndicated Test");
            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ArchiveDisputes);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.Disputes);
            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.BranchConsultantD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.BranchConsultantD);

            browser.Dispose();
            browser = null;

            browser = new TestBrowser(TestUsers.BranchConsultant, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.Disputes);
            browser.ClickAction(WorkflowActivities.ApplicationManagement.DisputeFinalised);
            browser.Page<WorkflowYesNo>().Confirm(true, true);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
            var offerRoleType = (OfferRoleTypeEnum)Enum.ToObject(typeof(OfferRoleTypeEnum), offerRoleTypeKey);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, offerRoleType, aduserName);
        }

        /// <summary>
        /// Check that it is possible to perform the 'Request Policy Override' action.  Ensure the application moves to the 'Policy Override' state and is assigned to
        /// the selected user. On completing the 'Feedback on Override' action ensure the application moves to the 'Credit' state and is assigned to the selected user.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the 'Request Policy Override' action.  Ensure the application moves to the 'Policy Override' state
		and is assigned to the selected user. On completing the 'Feedback on Override' action ensure the application moves to the 'Credit' state and is assigned to the
		selected user.")]
        [Ignore]
        public void _019_PolicyOverride()
        {
            const string testIdentifier = "PolicyOverride";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier, "CreditTestID");
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);

            string aduserName = results.Rows(0).Column("ADUserName").Value;

            browser = new TestBrowser(aduserName, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.RequestPolicyOverride);

            browser.Page<WF_ReAssign>().SelectRoleAndConsultantFromDropdownAndCommit(TestUsers.CreditSupervisor, OfferRoleTypes.CreditSupervisorD);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.PolicyOverride);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);

            browser.Dispose();
            browser = null;

            results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);

            aduserName = results.Rows(0).Column("ADUserName").Value;
            browser = new TestBrowser(aduserName);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.PolicyOverride);

            browser.ClickAction(WorkflowActivities.Credit.FeedbackonOverride);
            browser.Page<WF_ReAssign>().SelectRoleAndConsultantFromDropdownAndCommit(TestUsers.CreditUnderwriter, OfferRoleTypes.CreditUnderwriterD);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.CreditUnderwriterD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.CreditSupervisorD);
        }

        /// <summary>
        /// Check that it is possible to perform the 'Return to Processor' action.  Ensure the application moves to the 'Manage Application' state.
        /// On completing the 'Application in Order' action ensure the application moves to the 'Credit' state and is assigned to the last active credit user on the case.
        /// </summary>
        [Test, Description(@"Check that it is possible to perform the 'Return to Processor' action.  Ensure the application moves to the 'Manage Application' state.
		On completing the 'Application in Order' action ensure the application moves to the 'Credit' state and is assigned to the last active credit user on the case")]
        [Ignore]
        public void _020_ReturnToProcessor()
        {
            const string testIdentifier = "ReturnToProcessor";
            int offerKey = Service<ICommonService>().GetOfferKeyByTestIdentifier(testIdentifier, "CreditTestID");
            QueryResults results = Service<IAssignmentService>().GetX2WorkFlowAssignment_ByStateName(offerKey, WorkflowStates.CreditWF.Credit);

            string aduserName = results.Rows(0).Column("ADUserName").Value;
            string offerRoleTypeKey = results.Rows(0).Column("OfferRoleTypeKey").Value;

            browser = new TestBrowser(aduserName, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);

            browser.ClickAction(WorkflowActivities.Credit.ReturntoProcessor);
            browser.Page<WorkflowYesNo>().Confirm(true, false);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.ArchiveProcessor);
            X2Assertions.AssertCurrentAppManX2State(offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            AssignmentAssertions.AssertThatAWorkFlowAssignmentRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertThatTheWorkFlowAssignmentRecordIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOfferRoleRecordExists(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);
            AssignmentAssertions.AssertOnlyOneOfferRoleRecordOfThisOfferRoleTypeIsActive(offerKey, OfferRoleTypeEnum.NewBusinessProcessorD);

            browser.Dispose();
            browser = null;

            browser = new TestBrowser(TestUsers.NewBusinessProcessor, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationManagementWF.ManageApplication);

            browser.ClickAction(WorkflowActivities.ApplicationManagement.ApplicationinOrder);
            browser.Page<WorkflowYesNo>().Confirm(true, false);

            Thread.Sleep(5000);
            X2Assertions.AssertCurrentCreditX2State(offerKey, WorkflowStates.CreditWF.Credit);
            var offerRoleType = (OfferRoleTypeEnum)Enum.ToObject(typeof(OfferRoleTypeEnum), offerRoleTypeKey);
            AssignmentAssertions.AssertWhoTheOfferRoleRecordIsAssignedTo(offerKey, offerRoleType, aduserName);
        }

        #endregion IgnoredTests

        #region HelperMethods

        /// <summary>
        /// Approves an application
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="offerKey"></param>
        /// <param name="userName"></param>
        /// <param name="loanType">loanType</param>
        private void ApproveApplication(TestBrowser browser, int offerKey, string userName, string loanType)
        {
            browser = new TestBrowser(userName, TestUsers.Password);

            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //newly added Confirm Application Employment action
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(offerKey, WorkflowStates.CreditWF.Credit, EmploymentType.Salaried);
            //Added twice as after confirming the Employment the Page might be cleared if you are not the specified user.
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            //Approve application
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            //we need confirmed income
            Service<IApplicationService>().InsertEmploymentRecords(offerKey);
            //we need the application type
            switch (loanType)
            {
                case "New purchase":
                    browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
                    break;

                case "Switch loan":
                case "Refinance":
                    double maxQC = browser.Page<ApplicationLoanDetailsApprove>().GetMaximumQC() * 0.75;
                    browser.Page<ApplicationLoanDetailsApprove>().UpdateQuickCashDetails(maxQC, (maxQC * 0.25));
                    browser.Page<ApplicationLoanDetailsApprove>().SaveApplication();
                    browser.Page<BasePage>().DomainWarningClickYes();
                    break;
            }
            if (browser != null)
            {
                try
                {
                    browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    browser.Dispose();
                    browser = null;
                }
            }
            Service<IX2WorkflowService>().WaitForX2State(offerKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.LOA);
        }


        #endregion HelperMethods

    }
}