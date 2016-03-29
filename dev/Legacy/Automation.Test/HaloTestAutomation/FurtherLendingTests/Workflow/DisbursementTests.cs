using System;
using System.Linq;
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LoanServicing;
using BuildingBlocks.Presenters.LoanServicing.CATSDisbursement;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace FurtherLendingTests.Workflow
{
    [RequiresSTA]
    public class DisbursementTests : TestBase<BasePage>
    {
        private int _disbursementOfferKey;
        private string _disbursementReference;
        private bool _ruleUpdated;

        #region setupTearDown

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
            base.Browser = new TestBrowser(TestUsers.FLProcessor3);
        }

        protected override void OnTestFixtureTearDown()
        {
            base.OnTestFixtureTearDown();
            RevertRuleUpdates();
        }

        #endregion setupTearDown

        /// <summary>
        /// In certain cases a valuation may need to be done on a further lending application. In these instances the disbursement can be split between the
        /// client and the SAHL Valuation Recovery account. This test ensures that the FL Processor can setup a CATS Disbursement record where the payment is
        /// split and send the case to be reviewed by a FL Supervisor. It will check that there are two disbursements reocrds saved and that the records match
        /// the amounts entered for the valuation amount and the clients amount.
        /// </summary>
        [Test]
        public void _001_SplitReadvanceDisbursementToValuations()
        {
            double readvanceValue; double valuationValue;
            _disbursementOfferKey = Helper.GetApplicationForFurtherLendingTest(WorkflowStates.ReadvancePaymentsWF.SetupPayment, Workflows.ReadvancePayments);
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, _disbursementOfferKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            int accountKey = base.Service<IApplicationService>().GetOfferAccountKey(_disbursementOfferKey);
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            base.Browser.Page<CATSDisbursementAdd>().PostReadvance(_disbursementOfferKey, true, out valuationValue, out readvanceValue, false, out _disbursementReference);
            //base.Browser.ClickWorkflowLoanNode(WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            base.Browser.ClickWorkflowLoanNode(Workflows.ReadvancePayments);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.PaymentPrepared);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(_disbursementOfferKey, OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisorDisburseFunds);
            X2Assertions.AssertCurrentReadvPaymentsX2State(_disbursementOfferKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            DisbursementAssertions.AssertDisbursementExistsAtStatus(accountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.ReAdvance);
            DisbursementAssertions.AssertDisbursementAmount(accountKey, DisbursementStatusEnum.Pending, DisbursementTransactionTypeEnum.ReAdvance,
                readvanceValue, valuationValue);
        }

        /// <summary>
        /// This test ensures that once the CATS Disbursement record has been reviewed the FL Supervisor has the ability to post the split disbursement
        /// record. It checks the disbursement and loan transaction records are posted correctly and that the instalment is recalculated after the ReAdvance
        /// is performed.
        /// </summary>
        [Test]
        public void _002_SplitReadvanceDisbursementToValuations()
        {
            //search for the case
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, _disbursementOfferKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            int accountKey = base.Service<IApplicationService>().GetOfferAccountKey(_disbursementOfferKey);
            double readvanceValue = Service<IDisbursementService>().GetDisbursementRecords(accountKey, DisbursementStatusEnum.Pending,
                DisbursementTransactionTypeEnum.ReAdvance);
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickManageDisbursementsNode();
            base.FLSupervisorBrowser.Page<CATSDisbursementAdd>().Post(_disbursementOfferKey, out _disbursementReference);
            DisbursementAssertions.AssertDisbursementExistsAtStatus(accountKey, DisbursementStatusEnum.ReadyForDisbursement, DisbursementTransactionTypeEnum.ReAdvance);
            DisbursementAssertions.AssertReadvanceDisbursementFinancialTransaction(accountKey, DisbursementStatusEnum.ReadyForDisbursement,
                DisbursementTransactionTypeEnum.ReAdvance, readvanceValue, TransactionTypeEnum.Readvance);
            TransactionAssertions.AssertLoanTransactionExists(accountKey, TransactionTypeEnum.InstallmentChange);
        }

        /// <summary>
        /// This test will ensure that a user can rollback a readvance disbursement via the Rollback Disbursement screen. This however
        /// can only be done prior to 12:30pm in production. This test needs to check the current time, if it is later than 12:30pm then
        /// will have to update the rule parameter to a later time and refresh the rule parameters.
        /// </summary>
        [Test,
        Description(@"This test case will ensure that the user can rollback a disbursement at the ready for disbursement
                     status through the use of the Rollback Disbursement screen."
            )]
        public void _003_RollbackDisbursementAtReadyForDisbursement()
        {
            if (Service<ICommonService>().IsTimeOver(new TimeSpan(12, 30, 00)))
            {
                var browser = new TestBrowser(TestUsers.ClintonS, TestUsers.Password_ClintonS);
                var newCutOffTime = new TimeSpan(23, 00, 00);
                browser.Page<AdminFlushCache>().UpdateRuleParameterAndRefreshCache(browser, "CATSDisbursementRollback", "@CutOffTime", newCutOffTime.ToString());
                _ruleUpdated = true;
                browser.Dispose();
            }
            //search for the case
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor);
            base.FLSupervisorBrowser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.FLSupervisorBrowser);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, _disbursementOfferKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            int accountKey = base.Service<IApplicationService>().GetOfferAccountKey(_disbursementOfferKey);
            ////we need to post the disbursement record
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickCATSDisbursementNode();
            base.FLSupervisorBrowser.Navigate<LoanDetailsNode>().ClickRollbackDisbursementsNode();
            //we need the loan transaction number
            string financialTransactionKey =
                (from r in Service<ILoanTransactionService>().GetLoanTransactionByTypeDateAndAccountKey(accountKey, DateTime.Now.AddMinutes(-1), (int)TransactionTypeEnum.Readvance)
                 select r.Column("FinancialTransactionKey").GetValueAs<string>()).FirstOrDefault();
            base.FLSupervisorBrowser.Page<DisbursementRollback>().RollbackDisbursement(financialTransactionKey);
            //we need to check that the loantransactions have been reversed
            financialTransactionKey = Service<ILoanTransactionService>().GetLoanTransactionColumn("FinancialTransactionKey", "Reference", String.Format(@"Reversal Transaction - {0}", financialTransactionKey));
            Assert.True(financialTransactionKey.Length > 0, "No reversal transaction found");
            DisbursementAssertions.AssertDisbursementExistsAtStatus(accountKey, DisbursementStatusEnum.RolledBack, DisbursementTransactionTypeEnum.ReAdvance);
        }

        /// <summary>
        /// This test ensures that the FL Supervisor has the ability to perform the Disbursement Incorrect action at the Disbursed Funds state if the
        /// CATS Disbursement record was incorrectly setup. The case is returned to the Setup Payment state where the FL Processor has the ability to
        /// capture the correct record.
        /// </summary>
        [Test]
        public void _004_PerformDisbursementIncorrectAsSupervisor()
        {
            int offerKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.ReadvancePaymentsWF.SetupPayment, Workflows.ReadvancePayments,
                OfferTypeEnum.FurtherAdvance, "FLAutomation");
            base.Browser.Page<WorkflowSuperSearch>().Search(base.Browser, offerKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            base.Browser.ClickAction(WorkflowActivities.ReadvancePayments.PaymentPrepared);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(offerKey, OfferRoleTypeEnum.FLSupervisorD,
                RoundRobinPointerEnum.FLSupervisorDisburseFunds);
            base.FLSupervisorBrowser = new TestBrowser(TestUsers.FLSupervisor, TestUsers.Password);
            base.FLSupervisorBrowser.Page<WorkflowSuperSearch>().Search(base.FLSupervisorBrowser, offerKey, WorkflowStates.ReadvancePaymentsWF.DisburseFunds);
            base.FLSupervisorBrowser.ClickAction(WorkflowActivities.ReadvancePayments.DisbursementIncorrect);
            base.FLSupervisorBrowser.Page<WorkflowYesNo>().Confirm(true, false);
            X2Assertions.AssertCurrentReadvPaymentsX2State(offerKey, WorkflowStates.ReadvancePaymentsWF.SetupPayment);
            WorkflowRoleAssignmentAssertions.AssertUserReactivatedOrRoundRobinAssigned(offerKey, OfferRoleTypeEnum.FLProcessorD,
                RoundRobinPointerEnum.FLProcessor);
        }

        /// <summary>
        /// This method will revert the rule update made to the CATS Disbursement Rollback rule and set the cut off time for
        /// disbursements back to 12:30 only if the rule had its parameter updated.
        /// </summary>
        private void RevertRuleUpdates()
        {
            if (_ruleUpdated)
            {
                //revert the rule back to 12:30
                var oldCutOffTime = new TimeSpan(12, 30, 00);
                Service<ICommonService>().UpdateRuleParameter("CATSDisbursementRollback", "@CutOffTime", oldCutOffTime.ToString());
            }
        }
    }
}