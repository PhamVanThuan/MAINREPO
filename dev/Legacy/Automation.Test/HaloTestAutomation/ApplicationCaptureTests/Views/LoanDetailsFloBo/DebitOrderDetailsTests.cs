using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace ApplicationCaptureTests.Views.LoanDetailsFloBo
{
    [RequiresSTA]
    public sealed class DebitOrderDetailsTests : TestBase<DebitOrderDetailsAppUpdate>
    {
        private int _offerKey;
        private int _accountKey;
        private int _legalEntityKey;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.BranchConsultant);
            var results = Service<IApplicationService>().GetOpenApplicationCaptureOffer();
            var row = results.Rows(0);
            this._offerKey = row.Column("offerkey").GetValueAs<int>();
            this._accountKey = row.Column("reservedAccountKey").GetValueAs<int>();
            this._legalEntityKey = Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(_offerKey);
            //make sure we have a legal entity bank account to use
            base.Service<ILegalEntityService>().InsertLegalEntityBankAccount(_legalEntityKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Navigate<BuildingBlocks.Navigation.WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().Search(_offerKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            //delete any offer debit orders
            Service<IApplicationService>().DeleteOfferDebitOrder(_offerKey);
            Service<IEmploymentService>().DeleteLegalEntityEmployment(_legalEntityKey, EmploymentTypeEnum.SalariedWithDeductions);
            //go to loan details
            GoToDebitOrderUpdate();
        }

        #region Tests

        /// <summary>
        /// Tests that a user can add a Debit Order Payment offer debit order
        /// </summary>
        [Test]
        public void AddDebitOrderPaymentType()
        {
            int debitOrderDay = 3;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.DebitOrderPayment, true);
            OfferAssertions.OfferDebitOrderExists(_offerKey, FinancialServicePaymentTypeEnum.DebitOrderPayment, debitOrderDay);
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void AddSubsidyPaymentTypeNotAllowedWithoutSubsidisedEmployment()
        {
            int debitOrderDay = 7;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.SubsidyPayment, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                "Payment Type can not be subsidy - There are no applicants on this application with Employment Type of Subsidy");
        }

        /// <summary>
        /// Tests that a user can add a direct payment offer debit order
        /// </summary>
        [Test]
        public void AddDirectPaymentType()
        {
            int debitOrderDay = 24;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.DirectPayment, false);
            OfferAssertions.OfferDebitOrderExists(_offerKey, FinancialServicePaymentTypeEnum.DirectPayment, debitOrderDay);
        }

        /// <summary>
        /// A user cannot add a debit order payment without selecting a bank account.
        /// </summary>
        [Test]
        public void AddDebitOrderPaymentWithNoBankAccount()
        {
            int debitOrderDay = 3;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.DebitOrderPayment, false);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Bank Account must be selected.");
        }

        /// <summary>
        /// A user cannot add a debit order payment without selecting a debit order day
        /// </summary>
        [Test]
        public void AddDebitOrderPaymentWithNoDebitOrderDay()
        {
            int debitOrderDay = 0;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.DebitOrderPayment, true);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("Debit Order Day is a mandatory field");
        }

        /// <summary>
        /// The max value in the debit order day dropdown should be 28
        /// </summary>
        [Test]
        public void MaxAllowedDebitOrderDayIs28()
        {
            base.View.AssertMaxAllowedDebitOrderDay();
        }

        /// <summary>
        /// A debit order can be added of type Subsidy Payment if subsidised employment has been added to a legal entity on the application.
        /// This test will add a large stop order amount in order to ensure that the stop order amount is greater than the instalment value.
        /// </summary>
        [Test]
        public void AddSubsidyDebitOrder()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntity_ByLegalEntityKey(_legalEntityKey);
            base.Browser.Navigate<LegalEntityNode>().EmploymentDetails(NodeTypeEnum.Add);
            //we need to add subsidised employment
            var employment = Service<IEmploymentService>().GetSubsidisedEmployment();
            employment.StopOrderAmount = 50000; //make this a large amount to avoid rule stopping the add
            base.Browser.Page<LegalEntityEmploymentDetails>().AddEmploymentDetails(employment, true);
            // SubsidyProvider
            base.Browser.Page<LegalEntityDetailsSubsidyAddLegalEntityEmploymentDetails>
                ().AddSubsidyDetailsForAccount(_accountKey, employment, Common.Enums.ButtonTypeEnum.Save);
            GoToDebitOrderUpdate();
            int debitOrderDay = 7;
            base.View.PopulateView(debitOrderDay, FinancialServicePaymentTypeEnum.SubsidyPayment, false);
            OfferAssertions.OfferDebitOrderExists(_offerKey, FinancialServicePaymentTypeEnum.SubsidyPayment, debitOrderDay);
        }

        #endregion Tests

        #region Helpers

        private void GoToDebitOrderUpdate()
        {
            base.Browser.Navigate<LoanDetailsNode>().ClickLoanDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickDebitOrderDetailsNode();
            base.Browser.Navigate<LoanDetailsNode>().ClickUpdateDebitOrderDetailsNode();
        }

        #endregion Helpers
    }
}