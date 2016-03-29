using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class PaymentReceivedAndReview : DebtCounsellingTests.TestBase<PaymentReceivedUpdate>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        private const string pdaRuleMessage =
                @"Please select a Payment Distribution Agent or add an active Debit Order Payment, Subsidy Payment or Direct Payment before continuing this action.";

        #region PaymentReceivedTests

        /// <summary>
        /// Ensures that the mandatory data is supplied before the user can complete the action
        /// </summary>
        [Test, Description("Ensures that the mandatory data is supplied before the user can complete the action")]
        public void MandatoryData()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.View.EnterBlankPaymentReceivedDate();
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Payment Received Date is Mandatory");
            base.View.EnterPaymentReceivedDateThatMatchesDebitOrderDay(base.TestCase.AccountKey);
            base.View.EnterBlankPaymentReceivedAmount();
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Payment Received Amount is mandatory and must be greater than 0");
            base.View.EnterZeroPaymentReceivedAmount();
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Payment Received Amount is mandatory and must be greater than 0");
        }

        /// <summary>
        /// If the Payment Received Day and the Debit Order Day do not match the user should be warned
        /// </summary>
        [Test, Description("If the Payment Received Day and the Debit Order Day do not match the user should be warned")]
        public void DayAndDebitOrderDayMismatch()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.View.EnterValidReviewDate();
            base.View.EnterValidPaymentReceivedAmount();
            base.View.EnterPaymentReceivedDateThatDoesNotMatchDebitOrderDay(base.TestCase.AccountKey);
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "The Payment Received Date and the Reset Instalment Expectancy Date do not have matching day values. Please ensure that the Debit Order has been adjusted correctly.");
        }

        /// <summary>
        /// The payment received date cannot be in the future. The user should be warned if they capture a date in the future
        /// </summary>
        [Test, Description("The payment received date cannot be in the future. The user should be warned if they capture a date in the future")]
        public void PaymentReceivedDateValidation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.View.EnterPaymentReceivedDateGreaterThanToday();
            base.View.EnterValidReviewDate();
            base.View.EnterValidPaymentReceivedAmount();
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("The Payment Received Date cannot be in the future");
        }

        /// <summary>
        /// The review date cannot be more than 18 months in the future. The user should be warned if they capture a date greater than this.
        /// </summary>
        [Test, Description("The review date cannot be more than 18 months in the future. The user should be warned if they capture a date greater than this.")]
        public void ReviewDateValidation()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.View.EnterPaymentReceivedDateThatMatchesDebitOrderDay(base.TestCase.AccountKey);
            base.View.EnterValidReviewDate();
            base.View.EnterReviewDateGreaterThan18Months();
            base.View.ClickUpdateButton();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format(@"The Review Date cannot be more than 18 months from today. The latest possible date is {0}.",
                DateTime.Now.AddMonths(18).ToString(Formats.DateFormat)));
        }

        /// <summary>
        /// This test will provide the payment received details and ensure that the case is assigned to the correct user and moves states
        /// </summary>
        [Test, Description("This test will provide the payment received details and ensure that the case is assigned to the correct user and moves states")]
        public void CompletePaymentReceived()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            //add to worklist
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.supervisorAssignmentInclusionStates, true, base.TestCase.DebtCounsellingKey);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            PaymentReceived();
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PaymentReview);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, true, true);
        }

        /// <summary>
        /// This test will move a case through the Debt Counselling workflow and assign a Debt Counselling Manager to the case at the Decision on Proposal. This
        /// will ensure that when we assign a supervisor after performing the Payment Received action that we are using the load balance mechanism to assign the
        /// supervisor to the case.
        /// </summary>
        [Test, Description(@"This test will move a case through the Debt Counselling workflow and assign a Debt Counselling Manager to the case at the Decision on Proposal. This
		will ensure that when we assign a supervisor after performing the Payment Received action that we are using the load balance mechanism to assign the
		supervisor to the case.")]
        public void PerformingPaymentReceivedWithNoPreviousSupervisorShouldRoundRobinAssignTheSupervisorRole()
        {
            StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant, searchForCase: false);
            //send for approval to manager
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendProposalForApprovalAssignToManager, base.TestCase.DebtCounsellingKey);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.AcceptProposalAsManager, base.TestCase.DebtCounsellingKey);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.NotificationOfDecision, base.TestCase.DebtCounsellingKey);
            base.LoadCase(WorkflowStates.DebtCounsellingWF.PendPayment);
            string adUserName = Service<IAssignmentService>().GetUserForDebtCounsellingAssignment(WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, Workflows.DebtCounselling,
                    DebtCounsellingLoadBalanceStates.supervisorAssignmentInclusionStates, true, base.TestCase.DebtCounsellingKey);
            WorkflowHelper.AddPDAIfOneDoesNotExist(base.TestCase.DebtCounsellingKey, base.TestCase.AccountKey, base.Browser);
            PaymentReceived();
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PaymentReview);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, adUserName, WorkflowRoleTypeEnum.DebtCounsellingSupervisorD, true, true);
        }

        /// <summary>
        /// Should no PDA or active Debit Order Payment, Direct Payment or Subsidy Payment exist on the case the user will be prompted to add one prior to being able
        /// to complete the Payment Received action. This test will ensure that the validation message is displayed when the requirements are not met.
        /// </summary>
        [Test, Description(@"Should no PDA or active Debit Order Payment, Direct Payment or Subsidy Payment exist on the case the user will be prompted to add one prior to being able
		to complete the Payment Received action. This test will ensure that the validation message is displayed when the requirements are not met.")]
        public void NoPDAActiveDebitOrderPaymentDetailTypes()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            //remove the PDA
            Service<IExternalRoleService>().DeleteExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.PaymentDistributionAgent);
            //add PDA
            Service<IExternalRoleService>().InsertExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, 0, ExternalRoleTypeEnum.PaymentDistributionAgent,
                GeneralStatusEnum.Active);
            //start action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.Browser.Page<BasePageAssertions>().AssertNotificationDoesNotExist();
            //remove the PDA
            Service<IExternalRoleService>().DeleteExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.PaymentDistributionAgent);
            //remove detail types
            Service<IDetailTypeService>().RemoveDetailTypes(base.TestCase.AccountKey);
            //add an active debit order payment
            base.Service<IDebitOrdersService>().InsertFirstLegalEntityBankAccountAsFSBankAccount(base.TestCase.AccountKey, FinancialServicePaymentTypeEnum.DebitOrderPayment);
            //start action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.Browser.Page<BasePageAssertions>().AssertNotificationDoesNotExist();
            //add detail types
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.DebitOrderSuspended, base.TestCase.AccountKey);
            Service<IDetailTypeService>().InsertDetailType(DetailTypeEnum.BankDetailsIncorrect, base.TestCase.AccountKey);
            //start action expect fail
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.Browser.Page<BasePageAssertions>().AssertNotification(pdaRuleMessage);
            //remove detail types
            Service<IDetailTypeService>().RemoveDetailTypes(base.TestCase.AccountKey);
            //delete debit order and insert direct payment
            base.Service<IDebitOrdersService>().InsertFirstLegalEntityBankAccountAsFSBankAccount(base.TestCase.AccountKey, FinancialServicePaymentTypeEnum.DirectPayment);
            //start action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.Browser.Page<BasePageAssertions>().AssertNotificationDoesNotExist();
            //delete direct payment and insert subsidy payment
            base.Service<IDebitOrdersService>().InsertFirstLegalEntityBankAccountAsFSBankAccount(base.TestCase.AccountKey, FinancialServicePaymentTypeEnum.SubsidyPayment);
            //start action
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            base.Browser.Page<BasePageAssertions>().AssertNotificationDoesNotExist();
        }

        /// <summary>
        /// Should no PDA or active Debit Order Payment, Direct Payment or Subsidy Payment exist on the case the user will be prompted to add one prior to being able
        /// to complete the Payment Received action. This test will ensure that the validation message is displayed when the debit order payment is set to inactive.
        /// </summary>
        [Test, Description(@"Should no PDA or active Debit Order Payment, Direct Payment or Subsidy Payment exist on the case the user will be prompted to add one prior to being able
		to complete the Payment Received action. This test will ensure that the validation message is displayed when the debit order payment is set to inactive.")]
        public void NoPDAExistsDebitOrderPaymentInactive()
        {
            try
            {
                base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
                //remove the PDA
                base.Service<IExternalRoleService>().DeleteExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey, ExternalRoleTypeEnum.PaymentDistributionAgent);
                base.Service<IDebitOrdersService>().InsertFirstLegalEntityBankAccountAsFSBankAccount(base.TestCase.AccountKey, FinancialServicePaymentTypeEnum.DebitOrderPayment);
                base.Service<IDebitOrdersService>().UpdateFinancialServiceBankAccountStatus(base.TestCase.AccountKey, GeneralStatusEnum.Inactive);
                base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
                base.Browser.Page<BasePageAssertions>().AssertNotification(pdaRuleMessage);
            }
            finally
            {
                base.Service<IDebitOrdersService>().UpdateFinancialServiceBankAccountStatus(base.TestCase.AccountKey, GeneralStatusEnum.Active);
            }
        }

        #endregion PaymentReceivedTests

        private void PaymentReceived()
        {
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.PaymentReceived);
            string pmtDate = base.Browser.Page<PaymentReceivedUpdate>().EnterPaymentReceivedDateThatMatchesDebitOrderDay(base.TestCase.AccountKey);
            decimal amt = base.Browser.Page<PaymentReceivedUpdate>().EnterValidPaymentReceivedAmount();
            string reviewDate = base.Browser.Page<PaymentReceivedUpdate>().EnterValidReviewDate();
            base.Browser.Page<PaymentReceivedUpdate>().ClickUpdateButton();
            //check the details have been added
            DebtCounsellingAssertions.AssertPaymentReceivedDetails(base.TestCase.DebtCounsellingKey, pmtDate, amt, reviewDate);
        }
    }
}