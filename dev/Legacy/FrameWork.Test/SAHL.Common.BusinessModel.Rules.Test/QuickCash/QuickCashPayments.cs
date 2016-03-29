using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Rules.QuickCash;


namespace SAHL.Common.BusinessModel.Rules.Test.QuickCash
{
    [TestFixture]
    public class QuickCashPayments : RuleBase
    {
        IApplicationInformationQuickCashDetail appInfoQuickCash;
        IQuickCashPaymentType qcPaymentType;
        IExpenseType expenseType;
        IPaymentType paymentType;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            appInfoQuickCash = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            qcPaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            expenseType = _mockery.StrictMock<IExpenseType>();
            paymentType = _mockery.StrictMock<IPaymentType>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        private void StrictMockedQuickCashPaymentType(IApplicationExpense appExpense)
        {
            SetupResult.For(appInfoQuickCash.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appExpense.ExpenseType).Return(expenseType);
            SetupResult.For(expenseType.Key).Return((int)ExpenseTypes.StoreCard);
            SetupResult.For(expenseType.PaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)PaymentTypes.CashPaymentnointerest);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashApplicationExpenseAllFieldsTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 0;
            double reqamt = 20000;
            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.Key).Return(2);
            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 3, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashApplicationExpenseTwoFieldsBlankTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 0;
            double reqamt = 20000;
            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("Absa");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.Key).Return(2);

            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 2, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashApplicationExpenseOneAmtEqualZeroFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 0;
            double reqamt = 20000;
            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("Absa");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("9005621133");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.Key).Return(2);

            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 1, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashApplicationExpenseTestPass()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 1000;
            double reqamt = 20000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("Loan");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("900127273");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.Key).Return(2);

            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 0, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashDetailsValidateForDisbursedTestPass()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 1000;
            double reqamt = 20000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("Loan");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("900127273");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.Key).Return(20);

            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 0, appInfoQuickCash);
        }
        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashDetailsValidateForDisbursedTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 1000;
            double reqamt = 20000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(true);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(reqamt);
            SetupResult.For(appExpense.ExpenseAccountName).Return("Loan");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("900127273");
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appExpense.Key).Return(0);
            StrictMockedQuickCashPaymentType(appExpense);
            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 1, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashDetailsValidateDisbursedTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 1000;
            double requestedamt = 2000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(true);
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(requestedamt);
            SetupResult.For(appInfoQuickCash.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appExpense.ExpenseType).Return(expenseType);
            SetupResult.For(expenseType.PaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)PaymentTypes.CashPaymentnointerest);
            SetupResult.For(appExpense.Key).Return(0);

            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 1, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashDetailsValidateNonRegularPaymentTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 1000;
            double requestedamt = 2000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(requestedamt);
            SetupResult.For(appInfoQuickCash.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appExpense.ExpenseType).Return(expenseType);
            SetupResult.For(expenseType.PaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)PaymentTypes.CashPaymentnointerest);
            SetupResult.For(appInfoQuickCash.Key).Return(10);

            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 1, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashDetailsValidateAmtExceededTestFail()
        {
            ApplicationInformationQuickCashDetailsValidate rule = new ApplicationInformationQuickCashDetailsValidate();
            IEventList<IApplicationExpense> appExpenseLst = new EventList<IApplicationExpense>();
            IApplicationExpense appExpense = _mockery.StrictMock<IApplicationExpense>();
            double amt = 2200;
            double requestedamt = 2000;

            SetupResult.For(appInfoQuickCash.Disbursed).Return(false);
            SetupResult.For(appExpense.TotalOutstandingAmount).Return(amt);
            SetupResult.For(appInfoQuickCash.RequestedAmount).Return(requestedamt);
            SetupResult.For(appInfoQuickCash.QuickCashPaymentType).Return(qcPaymentType);
            SetupResult.For(qcPaymentType.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appExpense.ExpenseType).Return(expenseType);
            SetupResult.For(expenseType.Key).Return((int)ExpenseTypes.QuickCash);
            SetupResult.For(expenseType.PaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)PaymentTypes.CashPaymentnointerest);

            SetupResult.For(appExpense.ExpenseAccountName).Return("Absa");
            SetupResult.For(appExpense.ExpenseAccountNumber).Return("5454");
            SetupResult.For(appInfoQuickCash.Key).Return(2);

            appExpenseLst.Add(Messages, appExpense);
            SetupResult.For(appInfoQuickCash.ApplicationExpenses).Return(appExpenseLst);

            ExecuteRule(rule, 1, appInfoQuickCash);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateNoDetailsTestPass()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            // Setup ruleItem.Name
            SetupResult.For(ruleItem.Name).Return("QuickCashMinimumApprovedAmount");

            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);

            ExecuteRule(rule, 0, appInfoQC);
        }


        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateUpfrontExceededTestFail()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt = 6000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt);
            SetupResult.For(appDet.Disbursed).Return(false);

            appDetails.Add(Messages, appDet);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);

            ExecuteRule(rule, 1, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateRegularExceededTestFail()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 5000;
            double amt2 = 16000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(true);
            appDetails.Add(Messages, appDet);

            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(false);
            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);

            ExecuteRule(rule, 1, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateTestPass()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 5000;
            double amt2 = 15000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(true);
            appDetails.Add(Messages, appDet);


            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(false);
            SetupResult.For(appDetail.Key).Return(0);

            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);


            ExecuteRule(rule, 0, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateAllUpfrontNotDisbursedTestFail()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 5000;
            double amt2 = 15000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(false);
            appDetails.Add(Messages, appDet);


            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(false);

            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);


            ExecuteRule(rule, 1, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateAllUpfrontDisbursedTestPass()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();
            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 3000;
            double amt2 = 2000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(false);
            appDetails.Add(Messages, appDet);


            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.UpfrontPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(true);

            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);


            ExecuteRule(rule, 0, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateAllRegularNotDisbursedTestFail()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();
            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 5000;
            double amt2 = 15000;

            double minValue = 1000;
            IRuleItem ruleItem = GetParameterValue(minValue);

            // Setup ruleItem.Name
            SetupResult.For(ruleItem.Name).Return("QuickCashMinimumApprovedAmount");

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(false);
            appDetails.Add(Messages, appDet);

            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(false);

            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);


            ExecuteRule(rule, 1, appInfoQC);
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationQuickCashValidateAllRegularDisbursedTestPass()
        {
            ApplicationInformationQuickCashValidate rule = new ApplicationInformationQuickCashValidate();
            IApplicationInformationQuickCash appInfoQC = _mockery.StrictMock<IApplicationInformationQuickCash>();
            IEventList<IApplicationInformationQuickCashDetail> appDetails = new EventList<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDet = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();
            IApplicationInformationQuickCashDetail appDetail = _mockery.StrictMock<IApplicationInformationQuickCashDetail>();

            double totalAmtApproved = 20000;
            double upFrontApproved = 5000;
            double amt1 = 5000;
            double amt2 = 15000;

            IQuickCashPaymentType qcpaymentType = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDet.QuickCashPaymentType).Return(qcpaymentType);
            SetupResult.For(qcpaymentType.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDet.RequestedAmount).Return(amt1);
            SetupResult.For(appDet.Disbursed).Return(true);
            appDetails.Add(Messages, appDet);

            IQuickCashPaymentType qcpaymentType1 = _mockery.StrictMock<IQuickCashPaymentType>();
            SetupResult.For(appDetail.QuickCashPaymentType).Return(qcpaymentType1);
            SetupResult.For(qcpaymentType1.Key).Return((int)QuickCashPaymentTypes.RegularPayment);
            SetupResult.For(appDetail.RequestedAmount).Return(amt2);
            SetupResult.For(appDetail.Disbursed).Return(true);

            appDetails.Add(Messages, appDetail);

            SetupResult.For(appInfoQC.ApplicationInformationQuickCashDetails).Return(appDetails);
            SetupResult.For(appInfoQC.CreditUpfrontApprovedAmount).Return(upFrontApproved);
            SetupResult.For(appInfoQC.CreditApprovedAmount).Return(totalAmtApproved);

            ExecuteRule(rule, 0, appInfoQC);
        }

        private IRuleItem GetParameterValue(double minValue)
        {
            IRuleRepository RuleRepo = _mockery.StrictMock<IRuleRepository>();
            MockCache.Add(typeof(IRuleRepository).ToString(), RuleRepo);
            IRuleItem ruleItem = _mockery.StrictMock<IRuleItem>();
            SetupResult.For(RuleRepo.FindRuleItemByTypeName("")).IgnoreArguments().Return(ruleItem);
            // Setup ruleItem.parameters
            IEventList<IRuleParameter> ruleParameters = new EventList<IRuleParameter>();
            IRuleParameter ruleParameter = _mockery.StrictMock<IRuleParameter>();
            SetupResult.For(ruleParameter.Value).Return(minValue.ToString());
            ruleParameters.Add(Messages, ruleParameter);
            SetupResult.For(ruleItem.RuleParameters).Return(ruleParameters);
            return ruleItem;
        }
    }
}
