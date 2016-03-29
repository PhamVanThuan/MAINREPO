using Automation.DataAccess;
using System.Reflection;
using System.Threading;
using Automation.DataAccess;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing.CATSDisbursement;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Presenters.Origination.FurtherLending;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;
using System;

namespace FurtherLendingTests
{
    [TestFixture, RequiresSTA]
    public class ReadvancePaymentsWorkflow : TestBase<BasePage>
    {
        #region Setup/Teardown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            Service<ICommonService>().DeleteTestMethodDataForFixture("ReadvancePaymentsWorkflow");
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            if (base.Browser != null)
            {
                base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            }
        }

        #endregion Setup/Teardown

        /// <summary>
        /// This test will login as the assigned further lending supervisor and perform the Refer to Credit action on a readvance application at the Rapid Decision
        /// state in the Readvance Payments workflow. This will send the case into the Credit workflow, at which point we will need to find the assigned credit user
        /// and then update the test.AutomationFLTestCases table with the credit indicator and the assigned credit user.
        /// </summary>
        [Test, Description("A further lending supervisor can refer a readvance application to Credit in order for a credit decision to be made."), Category("Readvances")]
        public void _001_ReferReadvanceToCredit([Values(FurtherLendingTestCases.ReadvanceCreate3)] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            //our app man case should be at rapid hold
            int instanceid = X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.RapidHold);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.RefertoCredit);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, true);
            Service<IX2WorkflowService>().WaitForCreditCaseCreate(instanceid, testCase.OfferKey, WorkflowStates.CreditWF.Credit);
            //Assert the case has moved
            X2Assertions.AssertCurrentCreditX2State(testCase.OfferKey, WorkflowStates.CreditWF.Credit);
            Service<IFurtherLendingService>().UpdateFLAutomation("Credit", "1", identifier);
            Service<IFurtherLendingService>().UpdateFLAutomation("ReadvancePayments", "0", identifier);
            //lets get the Credit user and update the table
            string creditAdUser;
            OfferRoleTypeEnum creditOfferRoleType;
            Service<IFurtherLendingService>().UpdateAssignedCreditUser(testCase.OfferKey, identifier, out creditAdUser, out creditOfferRoleType);
            AssignmentAssertions.AssertWorkflowAssignment(creditAdUser, testCase.OfferKey, creditOfferRoleType);
            //app man case should be at credit hold
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.CreditHold);
            //readvance payments case archived
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.Archive);
        }

        /// <summary>
        /// A further lending supervisor user has the ability to approve Rapid ReAdvance applications that are sent to the Rapid Decision state.
        /// In this test case the FL Supervisor approves the application and the case is sent to the Setup Payment state in the Readvance
        /// Payments workflow. After the FL Supervisor has completed the action the case should be on both the supervisor's and the FL application
        /// processor's worklists.
        /// </summary>
        [Test, Description("A further lending supervisor can approve a readvance application"), Category("Readvances")]
        public void _002_ApproveReadvance([Values(
                                                    FurtherLendingTestCases.ReadvanceCreate5,
                                                    FurtherLendingTestCases.ReadvanceLessThanLAA
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.ApproveRapid);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, true);
            //case should now be at Setup Payment
            Thread.Sleep(2500);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            //should be assigned to our FLAppProcUser
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
            //should also be in the supervisor's list
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Supervisor, testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD);
        }

        /// <summary>
        /// This test ensures that the FL Processor has the ability to setup the CATS Disbursement record at the Setup Payment state. After the CATS
        /// Disburement record is setup, the FL Processor performs the Payment Prepared action to send the case through to a FL Supervisor at the
        /// Disburse Funds state in order for the record to be reviewed before it is posted.
        /// </summary>
        [Test, Sequential]
        public void _003a_ReadvancePaymentPrepared([Values(FurtherLendingTestCases.ReadvanceCreate5,
                                                           FurtherLendingTestCases.ReadvanceLessThanLAA
                                                        )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLProcessorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            double readvanceValue;
            double valuationValue;
            base.FLProcessorBrowser.Page<CATSDisbursementAdd>().PostReadvance(testCase.OfferKey, false, out valuationValue, out readvanceValue, false);
            //base.FLProcessorBrowser.ClickWorkflowLoanNode(WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            base.FLProcessorBrowser.ClickWorkflowLoanNode(Workflows.ReadvancePayments);
            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.PaymentPrepared);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisorDisburseFunds);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            DisbursementAssertions.AssertDisbursementExistsAtStatus(testCase.AccountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.ReAdvance);
            DisbursementAssertions.AssertDisbursementAmount(testCase.AccountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.ReAdvance, Convert.ToDecimal(readvanceValue));
        }

        /// <summary>
        /// This test ensures that the FL Supervisor has the ability to review and post the CATS Disbursement record at the Disburse Funds state.
        /// It checks the disbursement and loan transaction records are posted correctly and that the instalment is recalculated after the ReAdvance
        /// is performed.
        /// </summary>
        [Test, Sequential]
        public void _003b_SupervisorPostDisbursement([Values(
                                                    FurtherLendingTestCases.ReadvanceCreate5,
                                                    FurtherLendingTestCases.ReadvanceLessThanLAA
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);
            double readvanceValue = Service<IDisbursementService>().GetDisbursementRecords(testCase.AccountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.ReAdvance);
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            string reference;
            base.FLSupervisorBrowser.Page<CATSDisbursementAdd>().Post(testCase.OfferKey, out reference);
            DisbursementAssertions.AssertDisbursementExistsAtStatus(testCase.AccountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.ReAdvance);
            DisbursementAssertions.AssertReadvanceDisbursementFinancialTransaction(testCase.AccountKey, DisbursementStatusEnum.ReadyForDisbursement,
                DisbursementTransactionTypeEnum.ReAdvance, readvanceValue, TransactionTypeEnum.Readvance);
            TransactionAssertions.AssertLoanTransactionExists(testCase.AccountKey, TransactionTypeEnum.InstallmentChange);
        }

        /// <summary>
        /// This test will ensure that a FL Supervisor cannot post a duplicate disbursement within 2 days of each other. Because there is currently no
        /// concrete way of linking a disbursement to an offer, we have to run a date check with the same disbursement value. This test case will try
        /// and repost the disbursement from an earlier test. A warning message should be displayed to the user indicating that a duplicate disbursement
        /// has been found and that they cannnot continue.
        /// </summary>
        [Test, Description(@"If a 140 transaction has been posted for the same value against the account in past 2 days then the user cannot post another
        disbursement.")]
        public void _005_PostDuplicateDisbursement([Values(
                                                    FurtherLendingTestCases.ReadvanceCreate5
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            double readvanceValue;
            double valuationValue;
            base.FLSupervisorBrowser.Page<CATSDisbursementAdd>().PostReadvance(testCase.OfferKey, false, out valuationValue, out readvanceValue, false);
            base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(String.Format(
                "An advance transaction for {0} exists in the past 2 days. This has already been disbursed.", readvanceValue.ToString(Formats.CurrencyFormat2)));
        }

        /// <summary>
        /// In order to proceed to the Disbursed state a FL Supervisor must have posted a Readvance disbursement. This test ensures that if a user
        /// tries to complete the Disbursement Complete action without having posted a readvance disbursement, they are warned that a disbursement has not
        /// yet been posted and they are not allowed to continue without having posted the disbursement.
        /// </summary>
        [Test, Description(@"If a FL Supervisor tries to perform the Disbursment Complete action without posting the readvance then a warning message
        will be displayed.")]
        public void _006_DisbursementCompleteReadvanceNotPosted()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.SetupPayment, Workflows.ReadvancePayments,
                OfferTypeEnum.Readvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.PaymentPrepared);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);

            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DisbursementComplete);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists("Transaction 140 has not been added in the past 24 hours.");
        }

        /// <summary>
        /// A FL Supervisor should not be able to perform the rollback action if a rollback transaction has not been posted
        /// </summary>
        [Test, Description("A FL Supervisor should not be able to perform the rollback action if a rollback transaction has not been posted.")]
        public void _007_RollbackDisbursementNoReversalTran([Values(
                                                    FurtherLendingTestCases.ReadvanceCreate5
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DisbursementComplete);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisorDisburseFunds);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.RollbackDisbursement);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists("Transaction type 1140 not found in the last 24 hours");
            //need this in order to unlock the instance as we have started the activity above.
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(false, false);
        }

        /// <summary>
        /// Setups the 12 hour scheduled activity on our disbursed case so that the workflow case archives
        /// </summary>
        [Test, Description(@"This test case will ensure that the 12 hours disbursement timer is correctly setup. It will then update the timer
        in order to a later test to assert that the case is correctly archived.")]
        public void _008_12HourTimerArchive([Values(
                                                    FurtherLendingTestCases.ReadvanceCreate5
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            int instanceID = X2Assertions.AssertScheduleActivitySetup(testCase.OfferKey.ToString(), ScheduledActivities.ReadvancePayments._12hrs, 0, 12, 0);
            //fire the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.TwelveHourOverride, testCase.OfferKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ReadvancePayments._12hrs, instanceID, 1);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.FolderArchive);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Accepted);
        }

        /// <summary>
        /// Declining an application at the Rapid Decision state should result in the case being assigned to the FL Processor at the Decline state.
        /// If the currently assigned FL Processor is marked as inactive then a new FL Processor should be round robin assigned to the application
        /// at the Decline state.
        /// </summary>
        [Test,
         Description(
             "Declining at the Rapid Decision by a FL Supervisor should send the case the Decline state in a FL Processors worklist"
             )]
        public void _009_DeclineRapidAtRapidDecision()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate6;
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            //we need to check if the further lending processor is active
            string flAppProcUser = GetFLProcessorForApplicationCheckIsActive(base.FLSupervisorBrowser);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.Decline);
            string selectedReason = base.FLSupervisorBrowser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            //check the reason exists against the offer information key
            ReasonAssertions.AssertReason(selectedReason, ReasonType.AdministrativeDecline, testCase.OfferKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //the case should now be at the Decline state
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.Decline);
            //the offer end data should be populated
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            //the offer status is set to declined
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Declined);
            //the case is assigned to the FL Processor
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Performing an NTU on an application at the Rapid Decision state should result in the case being assigned to the FL Processor at the NTU state.
        /// If the currently assigned FL Processor is marked as inactive then a new FL Processor should be round robin assigned to the application
        /// at the NTU state.
        /// </summary>
        [Test, Description("Performing an NTU at the Rapid Decision by a FL Supervisor should send the case the NTU state in a FL Processors worklist")]
        public void _010_NTURapidAtRapidDecision()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate7;
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            string flAppProcUser = GetFLProcessorForApplicationCheckIsActive(base.FLSupervisorBrowser);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.NTU);
            string selectedReason = base.FLSupervisorBrowser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ApplicationNTU, testCase.OfferKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.NTU);
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// A FL Processor can finalise an NTU at the NTU state in the Readvance Payments workflow. This test ensures that the case is correctly
        /// moved to the Archive NTU state, the Offer End Date is populated and that the Offer Status is set to NTU.
        /// </summary>
        [Test, Description("A FL Processor can Finalise an NTU at the NTU state in the Readvance Payments Workflow")]
        public void _011_NTUFinal()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate7;
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.Page<WorkflowSuperSearch>().Search(base.FLProcessorBrowser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.NTUFinal);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(3000);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.ArchiveNTU);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// A FL Processor can finalise an decline at the Decline state in the Readvance Payments workflow. This test ensures that the case is correctly
        /// moved to the Archive Decline state, the Offer End Date is populated and that the Offer Status is set to Declined.
        /// </summary>
        [Test,
         Description("A FL Processor can Finalise a Decline at the Decline state in the Readvance Payments Workflow")]
        public void _012_DeclineFinal()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate6;

            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DeclineFinal);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(3000);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.ArchiveDecline);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Declined);
        }

        /// <summary>
        /// A FL Processor can reinstate an NTU'd application at the NTU state in the Readvance Payments workflow. This test ensures that the case is
        /// correctly moved back to its previous state, the Offer End Date is set back to NULL and that the offer status is set back to OPEN.
        /// </summary>
        [Test, Description("A FL Processor can reinstate an NTU'd application at the NTU state")]
        public void _013_ReinstateNTU()
        {
            //search for the case
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.ContactClient, Workflows.ReadvancePayments,
                                                              OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            string flAppProcUser = GetFLProcessorForApplicationCheckIsActive(base.Browser);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.NTU);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ApplicationNTU, offerKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, offerKey, OfferRoleTypeEnum.FLProcessorD);
            //reinstate the NTU
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.ReinstateNTU);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check the status
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, true);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.Open);
        }

        /// <summary>
        /// A FL Processor can reinstate an declined application at the Decline state in the Readvance Payments workflow back to the Rapid Decision.
        /// This test will perform the decline on an application at the Rapid Decision and then reinstate the decline. It then ensures that the case is
        /// correctly moved back to its previous state, the Offer End Date is set back to NULL and that the offer status is set back to OPEN. The case is
        /// also reassigned a FL Supervisor back at the Rapid Decision state.
        /// </summary>
        [Test,
         Description(
             "A FL Processor can reinstate an declined application at the Declined state back to the Rapid Decision state"
             )]
        public void _014_ReinstateDeclineToRapidDecision()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate8;
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            string flAppProcUser = GetFLProcessorForApplicationCheckIsActive(base.FLSupervisorBrowser);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.Decline);
            string selectedReason = base.FLSupervisorBrowser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.AdministrativeDecline, testCase.OfferKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.Decline);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Declined);
            AssignmentAssertions.AssertWorkflowAssignment(flAppProcUser, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);

            //reinstate the NTU
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.Decline);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.ReinstateDecline);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);

            //check the status
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.RapidDecision);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, true);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Open);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD, RoundRobinPointerEnum.FLSupervisor);
        }

        /// <summary>
        /// This test case will setup the NTU Timer in order for a later test to assert that the timer correctly archives the application
        /// </summary>
        [Test, Description("Setup of the scheduled activity for the NTU timer")]
        public void _015_NTUTimerArchivesCase()
        {
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.ContactClient, Workflows.ReadvancePayments,
                                                                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            int instanceID = Service<IX2WorkflowService>().GetReadvancePaymentsInstanceIDByState(offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.NTU);
            string selectedReason = base.Browser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.ApplicationNTU);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.ApplicationNTU, offerKey,
                                          GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.NTU);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
            //check if the scheduled activity is setup
            X2Assertions.AssertScheduleActivitySetup(offerKey.ToString(), ScheduledActivities.ReadvancePayments.NTUTimeout, 30, 0, 0);
            //update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.FireNTUTimeoutTimer, offerKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ReadvancePayments.NTUTimeout, instanceID, 1);
            //assert
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey,
                                 WorkflowStates.ReadvancePaymentsWF.ArchiveNTU);
            OfferAssertions.AssertOfferEndDate(offerKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(offerKey, OfferStatusEnum.NTU);
        }

        /// <summary>
        /// This test case will setup the Decline Timer in order for a later test to assert that the timer correctly archives the application
        /// </summary>
        [Test, Description("Setup of the scheduled activity for the Decline timer")]
        public void _016_DeclineTimerArchivesCase()
        {
            const string identifier = FurtherLendingTestCases.ReadvanceCreate9;
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            int instanceid = Service<IX2WorkflowService>().GetReadvancePaymentsInstanceIDByState(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.RapidDecision);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.RapidDecision);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.Decline);
            string selectedReason = base.FLSupervisorBrowser.Page<CommonReasonCommonDecline>().SelectReasonAndSubmit(ReasonType.AdministrativeDecline);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.AdministrativeDecline, testCase.OfferKey,
                                          GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey,
                                                         WorkflowStates.ReadvancePaymentsWF.Decline);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Declined);
            //check if the scheduled activity is setup
            X2Assertions.AssertScheduleActivitySetup(testCase.OfferKey.ToString(), ScheduledActivities.ReadvancePayments.DeclineTimeout, 30, 0, 0);
            //update the scheduled activity
            scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.FireDeclineTimeoutTimer, testCase.OfferKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.ReadvancePayments.DeclineTimeout, instanceid, 1);
            //assert
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.ArchiveDecline);
            OfferAssertions.AssertOfferEndDate(testCase.OfferKey, DateTime.Now, 0, false);
            OfferAssertions.AssertOfferStatus(testCase.OfferKey, OfferStatusEnum.Declined);
        }

        /// <summary>
        /// A further advance application cannot be disbursed if the new loan agreement has not been added. This test ensures that
        /// trying to post the further advance disbursement that results in the Loan Agreement Amount being exceeded results in the
        /// correct validation warning being presented to the user.
        /// </summary>
        [Test, Description("Posting the disbursement for a further advance with out increasing a LAA results in a error message being received.")]
        public void _017_PostFurtherAdvanceDisbursementLoanAgreementAmtExceeded([Values(
                                                                                    FurtherLendingTestCases.LAAExceeded
                                                                                    )] string identifier)
        {
            //search for the case
            double readvanceValue;
            double valuationValue;

            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            switch (testCase.CurrentState)
            {
                case WorkflowStates.ReadvancePaymentsWF.SendSchedule:
                    base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.SendSchedule);
                    base.FLSupervisorBrowser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
                    Thread.Sleep(5000);
                    base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule);
                    base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.ReceiveSchedule);
                    base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
                    base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
                    break;

                case WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule:
                    base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.ReceiveSchedule);
                    base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
                    base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
                    break;
            }
            //we need to post the disbursement record
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            base.FLSupervisorBrowser.Page<CATSDisbursementAdd>().PostReadvance(testCase.OfferKey, false, out valuationValue, out readvanceValue, false);
            //calculate the amount by which the new balance would exceed the LAA
            double diffValue = Service<IFurtherLendingService>().GetLAAExceededAmountForFurtherAdvance(testCase.OfferKey);
            base.FLSupervisorBrowser.Page<BasePageAssertions>().AssertValidationMessageExists(String.Format(@"The total amounts entered for this disbursement will exceed the Loan Agreement Amount by {0}", diffValue.ToString(Formats.CurrencyFormat2)));
        }

        /// <summary>
        /// A further advance application requires a new loan agreement to be added prior to the application being disbursed. This test will
        /// add a Loan Agreement and then ensure that the new Bond Registration Amount has been updated after the loan agreement has been added.
        /// </summary>
        [Test, Description("A user should be able to add an additional Loan Agreement to the Mortgage Loan in order to post a further advance.")]
        public void _018_AddLoanAgreement([Values(
                                            FurtherLendingTestCases.LAAExceeded
                                            )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().AddLoanAgreement();
            var bond = base.Service<IBondLoanAgreementService>().GetLatestBondRecordByOfferkey(testCase.OfferKey);
            string valueToIncrease = bond.BondRegistrationAmount.ToString();
            base.FLSupervisorBrowser.Page<BondLoanAgreement>().AddLoanAgreement(valueToIncrease);
            //we need to get the latest bond record again and compare
            bond = base.Service<IBondLoanAgreementService>().GetLatestBondRecordByOfferkey(testCase.OfferKey);
            string newBondValue = bond.BondRegistrationAmount.ToString();
            StringAssert.AreEqualIgnoringCase(valueToIncrease, newBondValue, "The latest Bond record was not updated to the new value");
        }

        /// <summary>
        /// This test case will pickup the further lending applications that have had the additional sureties added to the
        /// application and approve them out of Credit and back into the Readvance Payments workflow for processing. It will also pick up
        /// the other further advance and further loan applications that were sent into Credit and approve those applications.
        /// </summary>
        [Test, Sequential,
        Description(@"This test case will pickup the further lending applications that have had the additional sureties added to the
        application and approve them out of Credit and back into the Readvance Payments workflow for processing. It will also pick up
        the other further advance and further loan applications that were sent into Credit and approve those applications."
            )]
        public void _023_ApproveFLApplications([Values(
                                                   FurtherLendingTestCases.ReadvanceCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                   FurtherLendingTestCases.FurtherLoanCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                   FurtherLendingTestCases.FurtherLoanCreate3
                                                   )] string identifier)
        {
            //we need to login as the Credit Supervisor
            ApproveFLApplications(identifier, "_023_ApproveFLApplications", TestUsers.CreditSupervisor);
        }

        /// <summary>
        ///
        /// </summary>
        [Test, Sequential, Description(@"")]
        public void _023a_CheckFLApplicationApproval([Values(
                                                   FurtherLendingTestCases.ReadvanceCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                   FurtherLendingTestCases.FurtherLoanCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                   FurtherLendingTestCases.FurtherLoanCreate3
                                                   )] string identifier)
        {
            //we need to make sure the case didnt fail to leave credit
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = Service<IFurtherLendingService>().ReturnFurtherLendingOfferKeyByTestGroup(r);
            //check if the transition exists
            if (Service<IStageTransitionService>().CheckIfTransitionExists(offerKey, 2004))
                ApproveFLApplications(identifier, "_023a_CheckFLApplicationApproval", TestUsers.CreditUnderwriter);
        }

        /// <summary>
        /// Runs the assertions to ensure that the further lending applications that have been approved and are no longer in the
        /// Credit workflow and have been moved to the next state in the Readvance Payments workflow. This test includes the further lending applications
        /// that have had additional sureties added the applications.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        [Test, Sequential, Description(@"Runs the assertions to ensure that the further lending applications that have been approved and are no longer in the
        Credit workflow and have been moved to the next state in the Readvance Payments workflow.")]
        public void _024_AssertFLCreditApproval([Values(
                                                  FurtherLendingTestCases.ReadvanceCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate2,
                                                   FurtherLendingTestCases.FurtherLoanCreate2,
                                                   FurtherLendingTestCases.FurtherAdvanceCreate3,
                                                   FurtherLendingTestCases.FurtherLoanCreate3
                                                   )] string identifier)
        {
            AssertFLCreditApproval(identifier, "_023_ApproveFLApplications");
        }

        /// <summary>
        /// We need to carry out the Inform Client action on our Further Advance/Further Loan cases with an additional surety added
        /// to the application.
        /// </summary>
        [Test, Sequential, Description(@"We need to carry out the Inform Client action on our Further Advance/Further Loan cases with an additional surety added
                            to the application.")]
        public void _039_AdditionalSuretyInformClient([Values(
                                                          FurtherLendingTestCases.FurtherLoanCreate2,
                                                          FurtherLendingTestCases.FurtherAdvanceCreate2
                                                          )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            switch (identifier)
            {
                case FurtherLendingTestCases.FurtherAdvanceCreate2:
                    //we need the next R&V Admin user
                    string rvAdminUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.RVAdminD,
                        RoundRobinPointerEnum.RVAdmin);
                    base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.InformClient);
                    base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
                    Thread.Sleep(5000);
                    //case should now be at Deed of Surety Instruction
                    X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.DeedofSuretyInstruction);
                    //wfa check
                    AssignmentAssertions.AssertWorkflowAssignment(rvAdminUser, testCase.OfferKey, OfferRoleTypeEnum.RVAdminD);
                    break;

                case FurtherLendingTestCases.FurtherLoanCreate2:
                    //case should now be at Application Check
                    base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.InformClient);
                    base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
                    Thread.Sleep(5000);
                    X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Once the further lending case is at the Deed of Surety Instruction the Releases and Variations user can
        /// indicate that the updated suretyship agreement has been signed. Readvance applications will be moved
        /// to the Setup Payment state and the Further Advance application to the Contact Client state
        /// </summary>
        [Test, Description(@"Once the further lending case is at the Deed of Surety Instruction the Releases and Variations user can
                            indicate that the updated suretyship agreement has been signed. Readvance applications will be moved
                            to the Setup Payment state and the Further Advance application to the Contact Client state")]
        public void _040_SuretyshipSigned([Values(
                                              FurtherLendingTestCases.ReadvanceCreate2,
                                              FurtherLendingTestCases.FurtherAdvanceCreate2
                                              )] string identifier)
        {
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = 0;
            switch (identifier)
            {
                case FurtherLendingTestCases.ReadvanceCreate2:
                    offerKey = r.Rows(0).Column("ReadvOfferKey").GetValueAs<int>();
                    break;

                case FurtherLendingTestCases.FurtherAdvanceCreate2:
                    offerKey = r.Rows(0).Column("FAdvOfferKey").GetValueAs<int>();
                    break;

                default:
                    break;
            }
            string flProcessor = r.Rows(0).Column("AssignedFLAppProcUser").Value;
            string rvAdminUser = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(offerKey, OfferRoleTypeEnum.RVAdminD);
            //start the browser as the rv admin user
            var browser = new TestBrowser(rvAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ReadvancePaymentsWF.DeedofSuretyInstruction);
            //perform the action
            browser.ClickAction(WorkflowActivities.ReadvancePayments.SuretyshipSigned);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(5000);
            switch (identifier)
            {
                case FurtherLendingTestCases.ReadvanceCreate2:
                    //case should now be at
                    X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
                    //wfa check
                    AssignmentAssertions.AssertWorkflowAssignment(flProcessor, offerKey, OfferRoleTypeEnum.FLProcessorD);
                    break;

                case FurtherLendingTestCases.FurtherAdvanceCreate2:
                    //case should now be at
                    X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.SendSchedule);
                    AssignmentAssertions.AssertWorkflowAssignment(flProcessor, offerKey, OfferRoleTypeEnum.FLProcessorD);
                    break;

                default:
                    break;
            }
            //all of the application roles should now be on the account too, so our query should be an empty result set
            QueryResults le = base.Service<IApplicationService>().GetClientOfferRolesNotOnAccount(offerKey);
            Assert.False(le.HasResults, "There are still offer roles that exist only on the account");
            le.Dispose();
            r.Dispose();
            browser.Dispose();
        }

        /// <summary>
        /// This test will carry out the inform client action on our further advance and further loan applications
        /// that were approved out of the Credit workflow.
        /// </summary>
        /// <param name="identifier">Test Identifier to repeat the test for</param>
        [Test, Sequential, Description(@"This test will carry out the inform client action on our further advance and further loan applications
            that were approved out of the Credit workflow.")]
        public void _042_InformClient([Values(
                                          FurtherLendingTestCases.FurtherAdvanceCreate3,
                                          FurtherLendingTestCases.FurtherLoanCreate3
                                          )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.InformClient);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            switch (identifier)
            {
                case FurtherLendingTestCases.FurtherAdvanceCreate3:
                    //case should now be at Send Schedule
                    Service<IX2WorkflowService>().WaitForX2State(testCase.OfferKey, Workflows.ReadvancePayments, WorkflowStates.ReadvancePaymentsWF.SendSchedule);
                    X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SendSchedule);
                    break;

                case FurtherLendingTestCases.FurtherLoanCreate3:
                    //case should now be at Application Check
                    Service<IX2WorkflowService>().WaitForX2State(testCase.OfferKey, Workflows.ApplicationManagement, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
                    X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Performs the Send Schedule activity on a Further Advance application.
        /// </summary>
        [Test, Description("Performs the Send Schedule activity on a Further Advance application.")]
        public void _043_FurtherAdvanceSendSchedule([Values(FurtherLendingTestCases.FurtherAdvanceCreate3)] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.SendSchedule);
            base.FLProcessorBrowser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            Thread.Sleep(7500);
            //case should now be at the Awaiting Schedule state
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.AwaitingSchedule);
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Performs the Receive Schedule activity on a Further Advance application.
        /// </summary>
        [Test, Description("Performs the Receive Schedule activity on a Further Advance application.")]
        public void _044_FurtherAdvanceReceiveSchedule([Values(FurtherLendingTestCases.FurtherAdvanceCreate3)] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.ReceiveSchedule);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            //case should now be at the Awaiting Schedule state
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// Performs the Payment Prepared activity on a Further Advance application.
        /// </summary>
        [Test, Description("Performs the Payment Prepared activity on a Further Advance application.")]
        public void _045_FurtherAdvancePaymentPrepared([Values(FurtherLendingTestCases.FurtherAdvanceCreate3)] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLProcessorBrowser = new TestBrowser(testCase.Processor);
            Helper.Search(testCase.OfferKey, base.FLProcessorBrowser);

            base.FLProcessorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.PaymentPrepared);
            base.FLProcessorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            //case should now be at the Disburse Funds state
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisorDisburseFunds);
            string flSupervisor = base.Service<IApplicationService>().GetADUserByActiveOfferRoles(testCase.OfferKey, OfferRoleTypeEnum.FLSupervisorD);
            //save the supervisor
            Service<IFurtherLendingService>().UpdateFLAutomation("AssignedFLSupervisor", flSupervisor, identifier);
        }

        /// <summary>
        /// Disburses a further advance application. This test will need to add a new loan agreement prior to disbursing the
        /// application.
        /// </summary>
        [Test, Description("This test will disburse a further advance application.")]
        public void _046_FurtherAdvancePostDisbursement_DisbursementComplete([Values(FurtherLendingTestCases.FurtherAdvanceCreate3)] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);

            //we need to add a new loan agreement
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().AddLoanAgreement();
            var bond = base.Service<IBondLoanAgreementService>().GetLatestBondRecordByOfferkey(testCase.OfferKey);
            string valueToIncrease = bond.BondRegistrationAmount.ToString();
            base.FLSupervisorBrowser.Page<BondLoanAgreement>().AddLoanAgreement(valueToIncrease);
            //base.FLSupervisorBrowser.ClickWorkflowLoanNode(WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            base.FLProcessorBrowser.ClickWorkflowLoanNode(Workflows.ReadvancePayments);
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            int accountKey = base.Service<IApplicationService>().GetOfferAccountKey(testCase.OfferKey);
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            double valAmt;
            double readvAmt;
            base.FLSupervisorBrowser.Page<CATSDisbursementAdd>().PostReadvance(testCase.OfferKey, false, out valAmt, out readvAmt, true);
            //check the disbursement has been posted
            DisbursementAssertions.AssertDisbursementExistsAtStatus(accountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.ReAdvance);
            //check the disbursement records
            DisbursementAssertions.AssertDisbursementAmount(accountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.ReAdvance,
                (decimal)readvAmt);
            DisbursementAssertions.AssertReadvanceDisbursementFinancialTransaction(accountKey, DisbursementStatusEnum.ReadyForDisbursement,
                DisbursementTransactionTypeEnum.ReAdvance, readvAmt, TransactionTypeEnum.Readvance);
            //check that the instalment recalc has been posted
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.InstallmentChange);
            //base.FLSupervisorBrowser.ClickWorkflowLoanNode(WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            base.FLProcessorBrowser.ClickWorkflowLoanNode(Workflows.ReadvancePayments);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DisbursementComplete);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            //add an assertion to ensure that the scheduled activity is set up
            X2Assertions.AssertScheduleActivitySetup(testCase.OfferKey.ToString(), ScheduledActivities.ReadvancePayments._12hrs, 0, 12, 0);
        }

        /// <summary>
        /// This test ensures that a correspondence record exists for the sending of the new schedule to loan agreement via
        /// the Correspondence screen.
        /// </summary>
        [Test, Description(@"This test ensures that a correspondence record exists for the sending of the new schedule to loan agreement via
        the Correspondence screen.")]
        public void _047_FurtherAdvanceScheduleCorrespondence([Values(
                                                    FurtherLendingTestCases.FurtherAdvanceCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            //check the correspondence record
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(testCase.OfferKey, CorrespondenceReports.LegalAgreementStandardVariable, CorrespondenceMedium.Post);
        }

        /// <summary>
        /// This test case will archive the readvance application for the Further Advance under the Loan Agreement amount so that the subsequent further
        /// advance can be processed.
        /// </summary>
        [Test, Description(@"This test case will archive the readvance application for the Further Advance under the Loan Agreement amount so that
        the subsequent further advance can be processed.")]
        public void _048_ArchiveReadvanceUnderLAA([Values(
                                                    FurtherLendingTestCases.ReadvanceLessThanLAA
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            base.FLSupervisorBrowser = new TestBrowser(testCase.Supervisor);
            Helper.Search(testCase.OfferKey, base.FLSupervisorBrowser);
            //perform the disbursement complete action
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DisbursementComplete);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            //case should be at the disbursement state
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.Disbursed);
            base.scriptEngine.ExecuteScript(WorkflowEnum.ReadvancePayments, WorkflowAutomationScripts.ReadvancePayments.OverrideTimer, testCase.OfferKey, TestUsers.ClintonS);
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.FolderArchive);
        }

        /// <summary>
        /// This test will create the Further Advance application that is under the LAA which means that the application does
        /// not require signing of new legal agreements. After performing the Inform Client action the case will then skip
        /// the sending of the new schedules and end up at the Setup Payment state.
        /// </summary>
        [Test, Description(@"This test will create the Further Advance application that is under the LAA which means that the application does
        not require signing of new legal agreements. After performing the Inform Client action the case will then skip
        the sending of the new schedules and end up at the Setup Payment state.")]
        public void _049_CreateFurtherAdvanceLessThanLAA([Values(
                                                    FurtherLendingTestCases.FurtherAdvanceLessThanLAA
                                                    )] string identifier)
        {
            string testMethod = MethodBase.GetCurrentMethod().ToString();
            var testCase = Helper.GetTestCase(identifier);
            //create the application
            Helper.CreateFurtherLendingApplications(identifier, testCase.AccountKey, base.Browser);
            //perform application received
            Helper.ApplicationReceived(identifier, base.Browser);
            //perform QA Complete
            Helper.QAComplete(identifier, base.Browser);
            //we need to submit this case into credit
            Helper.ApplicationInOrderFurtherAdvance(identifier, testMethod, base.Browser);
            //check that it is in Credit
            Thread.Sleep(7500);
            Helper.AssertFurtherLendingCreditCases(testMethod, identifier);
            //approve the case out of credit
            ApproveFLApplications(identifier, testMethod, TestUsers.CreditSupervisor);
            //assert the approval
            Thread.Sleep(5000);
            testCase = Helper.GetTestCase(identifier);
            //check if the CHECK FAILED transition exists
            if (Service<IStageTransitionService>().CheckIfTransitionExists(testCase.OfferKey, 2004))
            {
                ApproveFLApplications(identifier, testMethod, TestUsers.CreditUnderwriter);
            }
            Thread.Sleep(5000);
            AssertFLCreditApproval(identifier, testMethod);
            //now we can perform the Inform Client action
            testCase = Helper.GetTestCase(identifier);
            var instanceID = X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
            Helper.SearchForFurtherLendingApp(identifier, base.Browser);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.InformClient);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ExternalActivities.ReadvancePayments.FurtherAdvanceBelowLAA, instanceID, 1);
            //this case should not require legal agreements to be sent and should end up at Setup Payment
            X2Assertions.AssertCurrentReadvPaymentsX2State(testCase.OfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            //still assigned to our FL Processor
            AssignmentAssertions.AssertWorkflowAssignment(testCase.Processor, testCase.OfferKey, OfferRoleTypeEnum.FLProcessorD);
        }

        /// <summary>
        /// This test will pickup our further lending application and instruct the attorney. By instructing the attorney we are creating a
        /// case in the Registration Pipeline e-work workflow map.
        /// </summary>
        [Test, Description(@"This test will pickup our further lending application and instruct the attorney. By instructing the attorney we are creating a
        case in the Registration Pipeline e-work workflow map.")]
        public void _050_InstructAttorneyFurtherLoan([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            //we need to login as a registrations admin user
            //start the browser as the rv admin user
            var browser = new TestBrowser(TestUsers.RegAdminUser, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
            var instanceID = X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.ApplicationCheck);
            //we need to select an attorney
            browser.ClickAction(WorkflowActivities.ApplicationManagement.SelectAttorney);
            browser.Page<Orig_SelectAttorney>().SelectAttorneyByDeedsOffice(DeedsOffice.Pietermaritzburg);
            //now we can instruct
            browser.ClickAction(WorkflowActivities.ApplicationManagement.InstructAttorney);
            browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ConditionalActivities.ApplicationManagement.CreateEWorkPipelineCase,
                    instanceID, 1);
            browser.Dispose();
        }

        /// <summary>
        /// This test asserts that the case from the previous test has been correctly instructed into the Pipeline eWork map.
        /// It ensures that a correspondence record has been added for the Further Loan Attorney Instruction, an ework workflow case has been
        /// created in the Assign state and that the X2 case has been moved to the Registration Pipeline state.
        /// </summary>
        [Test, Description(@"This test asserts that the case from the previous test has been correctly instructed into the Pipeline eWork map.
            It ensures that a correspondence record has been added for the Further Loan Attorney Instruction, an ework workflow case has been
            created in the Assign state and that the X2 case has been moved to the Registration Pipeline state.")]
        public void _051_InstructAttorneyAssertions([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            //wait for pipeline case create
            Service<IEWorkService>().WaitForeWorkCaseToCreate(testCase.AccountKey, EworkStages.Assign, EworkMaps.Pipeline);
            //we need to check for the case being created in the registration pipeline state in X2
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.RegistrationPipeline);
            //we need to check that the ework case has been created
            eWorkAssertions.AssertEworkCaseExists(testCase.AccountKey.ToString(), EworkStages.Assign, EworkMaps.Pipeline);
            //a correspondence record should have been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(testCase.OfferKey, CorrespondenceReports.AttorneyFurtherLoanInstruction, CorrespondenceMedium.Email);
        }

        /// <summary>
        /// This test ensures that the firing of the Up For Fees flag that Ework calls correctly moves our case out of the Registration Pipeline
        /// state and into the Ready for Disbursement state. This test will update the case directly in the eworks database to be at the stage
        /// directly prior to the Up For Fees action being performed.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Up For Fees flag that Ework calls correctly moves our case out of the Registration Pipeline
        state and into the Disbursement state. This test will update the case directly in the ework database to be at the Up For Fees stage.")]
        public void _052_ExtUpForFees([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            Int64 instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.RegistrationPipeline, testCase.OfferKey);
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(EworkStages.UpForFees, EworkStages.Assign, EworkMaps.Pipeline, testCase.AccountKey.ToString());
            //call the x2 procedure to insert the flag
            Service<IX2WorkflowService>().PipelineUpForFees(testCase.OfferKey);
            //update again to move the case on
            Service<IEWorkService>().UpdateEworkStage(EworkStages.Allocation, EworkStages.UpForFees, EworkMaps.Pipeline, testCase.AccountKey.ToString());
            //check that the case has been moved to the correct state in X2
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ExternalActivities.ApplicationManagement.Pipeline_UpForFees, instanceid, 1);
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
        }

        /// <summary>
        /// This test ensures that the firing of the Complete flag that Ework calls correctly moves our case out of the Disbursement
        /// state and into the Disbursement Review state. This test will update the case directly in the ework database to be at the Prep File stage.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Complete flag that Ework calls correctly moves our case out of the Disbursement
        state and into the Disbursement Review state. This test will update the case directly in the ework database to be at the Prep File stage.")]
        public void _053_ExtComplete([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            Int64 instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.Disbursement, testCase.OfferKey);
            //update the e-work database
            Service<IEWorkService>().UpdateEworkStage(EworkStages.PrepFile, EworkStages.Allocation, EworkMaps.Pipeline, testCase.AccountKey.ToString());
            //call the x2 procedure to insert the flag
            Service<IX2WorkflowService>().PipelineComplete(testCase.OfferKey);
            //update again to move the case on
            Service<IEWorkService>().UpdateEworkStage(EworkStages.PermissionToRegister, EworkStages.PrepFile, EworkMaps.Pipeline, testCase.AccountKey.ToString());
            //check that the external activity fired
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ExternalActivities.ApplicationManagement.ExtComplete, instanceid, 1);
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
        }

        /// <summary>
        /// This test ensures that the firing of the Held Over flag that Ework calls correctly moves our case out of the Disbursement
        /// Review state and into the Disbursement state.
        /// </summary>
        [Test, Description(@"This test ensures that the firing of the Held Over flag that Ework calls correctly moves our case out of the Disbursement
        Review state and into the Disbursement state.")]
        public void _054_ExtHeldOver([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            Int64 instanceid = Service<IX2WorkflowService>().GetAppManInstanceIDByState(WorkflowStates.ApplicationManagementWF.DisbursementReview, testCase.OfferKey);
            //insert the flag
            Service<IX2WorkflowService>().PipeLineHeldOver(testCase.OfferKey);
            //check that the external activity fired
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ExternalActivities.ApplicationManagement.ExtHeldOver, instanceid, 1);
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
        }

        /// <summary>
        /// A registrations user can move a case to the Disbursement Review state by performing the Review Disbursement Setup action
        /// that is applied at the Disbursement state
        /// </summary>
        [Test, Description(@"A registrations user can move a case to the Disbursement Review state by performing the Review Disbursement Setup action
        that is applied at the Disbursement state")]
        public void _055_ReviewDisbursementSetup([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            //we need to login as a registrations admin user
            var browser = new TestBrowser(TestUsers.RegAdminUser);
            browser.Page<WorkflowSuperSearch>().Search(browser, testCase.OfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.ReviewDisbursementSetup);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check if it has moved states
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
            browser.Dispose();
        }

        /// <summary>
        /// Performing the Disbursment Incorrect action will push the case back to the Disbursement state by performing the
        /// Disbursement Incorrect action.
        /// </summary>
        [Test, Description(@"Performing the Disbursement Incorrect action will push the case back to the Disbursement state by performing the
        Disbursement Incorrect action.")]
        public void _056_DisbursementIncorrect([Values(
                                                    FurtherLendingTestCases.FurtherLoanCreate3
                                                    )] string identifier)
        {
            var testCase = Helper.GetTestCase(identifier);
            //we need to login as a registrations admin user
            var browser = new TestBrowser(TestUsers.RegistrationsSupervisor, TestUsers.Password);
            browser.Page<WorkflowSuperSearch>().Search(browser, testCase.OfferKey, WorkflowStates.ApplicationManagementWF.DisbursementReview);
            //perform the action
            browser.ClickAction(WorkflowActivities.ApplicationManagement.DisbursementIncorrect);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            //check if it has moved states
            X2Assertions.AssertCurrentAppManX2State(testCase.OfferKey, WorkflowStates.ApplicationManagementWF.Disbursement);
            browser.Dispose();
        }

        /// <summary>
        /// A further lending processor should be allowed to perform Request Lightstone Valuation action that will perform a Lightstone
        /// Valuation on the property associated to the Further Lending application.
        /// </summary>
        [Test, Description(@"A further lending processor should be allowed to perform Request Lightstone Valuation action that will perform a Lightstone
        Valuation on the property associated to the Further Lending application.")]
        public void _058_RequestLightstoneValuation()
        {
            int offerKey = Service<IFurtherLendingService>().GetFurtherLendingOfferWithLightstonePropertyID(true);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.RequestLightstoneValuation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            Thread.Sleep(5000);
            //check that a lightstone valuation exists
            PropertyValuationAssertions.AssertLatestLightstoneValuationRecord(offerKey);
        }

        /// <summary>
        /// A Further Lending Processor cannot perform the Request Lightstone Valuation action for an
        /// application where the related property has an existing Lightstone valuation that is less than 2 months in age.
        /// </summary>
        [Test, Description(@"A Further Lending Processor cannot perform the Request Lightstone Valuation action for an
            application where the related property has an existing Lightstone valuation that is less than 2 months in age.")]
        public void _059_RequestLightstoneValuationExistingValuation()
        {
            int offerKey = Service<IFurtherLendingService>().GetFurtherLendingOfferWithLightstonePropertyID(false);
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(offerKey);
            //navigate to the action
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.RequestLightstoneValuation);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("A LightStone valuation for this property exists less than 2 months old.");
        }

        #region TestHelpers

        /// <summary>
        /// This method will get the currently assigned FL Processor by looking on the Further Lending application summary
        /// and picking it up from the label. It will then check if that user is currently marked as active. If they are
        /// not active and we are moving the case to a state that reassigns the FL Processor role application then we need
        /// to find the next FL Processor due for RR assignment, as this will become our expected user.
        /// </summary>
        /// <param name="browser">IE TestBrowser Object</param>
        /// <returns>FL Processor we expect to be assigned the case</returns>
        private string GetFLProcessorForApplicationCheckIsActive(TestBrowser browser)
        {
            string flAppProcUser = browser.Page<ApplicationSummaryFurtherLending>().GetApplicationProcessor();
            bool isActive = base.Service<IADUserService>().IsADUserActive(flAppProcUser);
            //if the user is inactive they will not be assigned the case when the case is sent to Decline
            if (!isActive)
            {
                //so we find the next FL Processor
                flAppProcUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.FLProcessorD,
                    RoundRobinPointerEnum.FLProcessor);
            }
            return flAppProcUser;
        }

        /// <summary>
        /// This helper method will approve a further lending application. Depending on the identifier it will perform different
        /// steps. ie. A Further Loan Application will require a decision on the Quick Cash prior to it being able to leave the
        /// Credit workflow.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="callingMethod">Test Method calling this Helper Method</param>
        /// <param name="creditUser">The Credit user to login as</param>
        private void ApproveFLApplications(string identifier, string callingMethod, string creditUser)
        {
            var browser = new TestBrowser(creditUser, TestUsers.Password);
            QueryResults r = Service<IFurtherLendingService>().GetFLAutomationRowByTestIdentifier(identifier);
            int offerKey = Service<IFurtherLendingService>().ReturnFurtherLendingOfferKeyByTestGroup(r);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            int employmentTypeKey = Service<IApplicationService>().GetApplicationEmploymentType(offerKey);
            browser.ClickAction(WorkflowActivities.Credit.ConfirmApplicationEmployment);
            browser.Page<ConfirmApplicationEmployment>().SelectEmploymentType(employmentTypeKey);
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.CreditWF.Credit);
            browser.ClickAction(WorkflowActivities.Credit.ApproveApplication);
            //we need to approve the application
            switch (identifier)
            {
                case FurtherLendingTestCases.ReadvanceCreate2:
                    //we can approve without caring about QC
                    browser.Page<FurtherLendingApprove>().Approve();
                    //get the next RV Admin user
                    string rvAdminUser = Service<IAssignmentService>().GetNextRoundRobinAssignmentByOfferRoleTypeKey(OfferRoleTypeEnum.RVAdminD,
                        RoundRobinPointerEnum.RVAdmin);
                    //insert the data for the assertions
                    Service<ICommonService>().InsertTestMethod(callingMethod, identifier, "ReadvancePaymentsWorkflow");
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.ADUserName,
                                                    rvAdminUser);
                    break;

                case FurtherLendingTestCases.FurtherAdvanceCreate2:
                case FurtherLendingTestCases.FurtherAdvanceCreate3:
                case FurtherLendingTestCases.FurtherAdvanceLessThanLAA:
                    //we can approve without caring about QC
                    browser.Page<FurtherLendingApprove>().Approve();
                    //insert the data for the assertions
                    Service<ICommonService>().InsertTestMethod(callingMethod, identifier, "ReadvancePaymentsWorkflow");
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.ADUserName,
                                                    r.Rows(0).Column("AssignedFLAppProcUser").Value);
                    break;

                case FurtherLendingTestCases.FurtherLoanCreate2:
                case FurtherLendingTestCases.FurtherLoanCreate3:
                    //we need to approve QC
                    browser.Page<FurtherLendingApprove>().ApproveWithQC();
                    //insert the data for the assertions
                    Service<ICommonService>().InsertTestMethod(callingMethod, identifier, "ReadvancePaymentsWorkflow");
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.OfferKey, offerKey.ToString());
                    Service<ICommonService>().SaveTestMethodParameters(callingMethod, identifier, ParameterTypeEnum.ADUserName,
                                                    r.Rows(0).Column("AssignedFLAppProcUser").Value);
                    break;

                default:
                    break;
            }
            r.Dispose();
            browser.Dispose();
            browser = null;
        }

        /// <summary>
        /// This method is used to assert the Credit Approval of the further lending applications that form part of our automated tests.
        /// Depending on the test case provided the method will run a different assertion as different applications will be sent to
        /// different states post the credit approval.
        /// </summary>
        /// <param name="identifier">Test Identifier</param>
        /// <param name="callingMethod">The test method that is calling this helper.</param>
        private void AssertFLCreditApproval(string identifier, string callingMethod)
        {
            int offerKey = Service<ICommonService>().GetTestMethodParameters<int>(callingMethod, identifier, ParameterTypeEnum.OfferKey);
            string adUserName = Service<ICommonService>().GetTestMethodParameters<string>(callingMethod, identifier, ParameterTypeEnum.ADUserName);
            switch (identifier)
            {
                //this case had an additional surety added to the application and will be at the Deed of Surety Instruction state.
                case FurtherLendingTestCases.ReadvanceCreate2:
                    X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.DeedofSuretyInstruction);
                    //add a rv admin RR assertion for the assignment
                    AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.RVAdminD);
                    break;

                case FurtherLendingTestCases.FurtherLoanCreate2:
                case FurtherLendingTestCases.FurtherLoanCreate3:
                case FurtherLendingTestCases.FurtherAdvanceLessThanLAA:
                    X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
                    AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.FLProcessorD);
                    //add quick cash approval assertions
                    break;

                case "-":
                case FurtherLendingTestCases.FurtherAdvanceCreate3:
                    X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.ContactClient);
                    AssignmentAssertions.AssertWorkflowAssignment(adUserName, offerKey, OfferRoleTypeEnum.FLProcessorD);
                    break;

                default:
                    break;
            }
            //update the flag
            Service<IFurtherLendingService>().UpdateFLAutomation("Credit", "0", identifier);
            Service<IFurtherLendingService>().UpdateFLAutomation("ReadvancePayments", "1", identifier);
            //delete the test method data
            Service<ICommonService>().DeleteTestMethodData(callingMethod, identifier);
        }

        #endregion TestHelpers
    }
}