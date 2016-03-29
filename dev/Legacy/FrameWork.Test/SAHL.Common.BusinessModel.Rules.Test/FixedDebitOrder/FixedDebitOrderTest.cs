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
using SAHL.Common.BusinessModel.Rules.FixedDebitOrder;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.Test.FixedDebitOrder
{
    [TestFixture]
    public class FixedDebitOrderTest : RuleBase
    {
        IFinancialService _fs = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _fs = _mockery.StrictMock<IFinancialService>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountTest()
        {
            SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccount rule = new SAHL.Common.BusinessModel.Rules.FixedDebitOrder.FinancialServiceBankAccount();
            IEventList<IFinancialServiceBankAccount> bankAccounts = new EventList<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount fsb0 = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServiceBankAccount fsb1 = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IGeneralStatus gs = _mockery.StrictMock<IGeneralStatus>();
            IDomainMessageCollection dmc = new DomainMessageCollection();

            IFinancialServiceType fsType = _mockery.StrictMock<IFinancialServiceType>();
            SetupResult.For(_fs.FinancialServiceType).Return(fsType);
            SetupResult.For(fsType.Key).Return((int)FinancialServiceTypes.VariableLoan);

            // null check on the bank accounts - should fail
            SetupResult.For(_fs.FinancialServiceBankAccounts).IgnoreArguments().Return(null);
            ExecuteRule(rule, 1, _fs);

            SetupResult.For(_fs.FinancialServiceType).Return(fsType);
            SetupResult.For(fsType.Key).Return((int)FinancialServiceTypes.VariableLoan);
            // bank accounts returns an empty list - should fail
            SetupResult.For(_fs.FinancialServiceBankAccounts).IgnoreArguments().Return(bankAccounts);
            ExecuteRule(rule, 1, _fs);

            SetupResult.For(_fs.FinancialServiceType).Return(fsType);
            SetupResult.For(fsType.Key).Return((int)FinancialServiceTypes.VariableLoan);

            // bank accounts returns multiple active bank accounts - should fail
            SetupResult.For(_fs.FinancialServiceBankAccounts).IgnoreArguments().Return(bankAccounts);
            bankAccounts.Add(dmc, fsb0);
            bankAccounts.Add(dmc, fsb1);
            SetupResult.For(fsb0.GeneralStatus).Return(gs);
            SetupResult.For(fsb1.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);
            ExecuteRule(rule, 1, _fs);

            SetupResult.For(_fs.FinancialServiceType).Return(fsType);
            SetupResult.For(fsType.Key).Return((int)FinancialServiceTypes.VariableLoan);

            // just a single, inactive bank account - should fail
            SetupResult.For(_fs.FinancialServiceBankAccounts).IgnoreArguments().Return(bankAccounts);
            bankAccounts.RemoveAt(dmc, 1);
            SetupResult.For(fsb0.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Inactive);
            ExecuteRule(rule, 1, _fs);

            SetupResult.For(_fs.FinancialServiceType).Return(fsType);
            SetupResult.For(fsType.Key).Return((int)FinancialServiceTypes.VariableLoan);

            // just a single, active bank account - should pass
            SetupResult.For(_fs.FinancialServiceBankAccounts).IgnoreArguments().Return(bankAccounts);
            SetupResult.For(fsb0.GeneralStatus).Return(gs);
            SetupResult.For(gs.Key).Return((int)GeneralStatuses.Active);
            ExecuteRule(rule, 0, _fs);
            
        }


        [NUnit.Framework.Test]
        public void FinancialServiceBankAddNewDebitOrderTestFail()
        {
            FinancialServiceBankAccountAddNewDebitOrder rule = new FinancialServiceBankAccountAddNewDebitOrder();
          
            IFinancialServiceBankAccount fsbank = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType paymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fsbank.FinancialServicePaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            IBankAccount bankAcct = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(fsbank.BankAccount).IgnoreArguments().Return(null);

            ExecuteRule(rule, 1, fsbank);

        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAddNewDebitOrderTestPass()
        {
            FinancialServiceBankAccountAddNewDebitOrder rule = new FinancialServiceBankAccountAddNewDebitOrder();

            IFinancialServiceBankAccount fsbank = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType paymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fsbank.FinancialServicePaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)FinancialServicePaymentTypes.DebitOrderPayment);
            IBankAccount bankAcct = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(fsbank.BankAccount).IgnoreArguments().Return(bankAcct);

            ExecuteRule(rule, 0, fsbank);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAddNewNonDebitOrderTestPass()
        {
            FinancialServiceBankAccountAddNewDebitOrder rule = new FinancialServiceBankAccountAddNewDebitOrder();
 
            IFinancialServiceBankAccount fsbank = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType paymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fsbank.FinancialServicePaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);
            IBankAccount bankAcct = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(fsbank.BankAccount).IgnoreArguments().Return(bankAcct);

            ExecuteRule(rule, 0, fsbank);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAddNewNonDebitOrderNullBankAccountTestPass()
        {
            FinancialServiceBankAccountAddNewDebitOrder rule = new FinancialServiceBankAccountAddNewDebitOrder();

            IFinancialServiceBankAccount fsbank = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType paymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fsbank.FinancialServicePaymentType).Return(paymentType);
            SetupResult.For(paymentType.Key).Return((int)FinancialServicePaymentTypes.DirectPayment);
            IBankAccount bankAcct = _mockery.StrictMock<IBankAccount>();
            SetupResult.For(fsbank.BankAccount).IgnoreArguments().Return(null);

            ExecuteRule(rule, 0, fsbank);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankvarifixTestPass()
        {
            FinancialServiceBankvarifix rule = new FinancialServiceBankvarifix();

            IFinancialServiceBankAccount fsbank = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(fsbank.FinancialService).Return(financialService);
            IAccount acc = _mockery.StrictMock<IAccount>();
            SetupResult.For(financialService.Account).Return(acc);
            IReadOnlyEventList<IFinancialService> finServiceFixed = _mockery.StrictMock<IReadOnlyEventList<IFinancialService>>();
            IReadOnlyEventList<IFinancialService> finServiceVar = _mockery.StrictMock<IReadOnlyEventList<IFinancialService>>();

            SetupResult.For(acc.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Open })).IgnoreArguments().Return(finServiceFixed);
            SetupResult.For(acc.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open })).IgnoreArguments().Return(finServiceVar);

            IFinancialService fsFixed = _mockery.StrictMock<IFinancialService>();
            IFinancialService fsVar = _mockery.StrictMock<IFinancialService>();

            SetupResult.For(finServiceFixed[0]).IgnoreArguments().Return(fsFixed);
            SetupResult.For(finServiceVar[0]).IgnoreArguments().Return(fsVar);

            IEventList<IFinancialServiceBankAccount> fsbAcctListFixed = _mockery.StrictMock<IEventList<IFinancialServiceBankAccount>>();
            SetupResult.For(fsFixed.FinancialServiceBankAccounts).IgnoreArguments().Return(fsbAcctListFixed);

            IFinancialServiceBankAccount fsbBankFixed = _mockery.StrictMock<IFinancialServiceBankAccount>();
            SetupResult.For(fsbAcctListFixed[0]).IgnoreArguments().Return(fsbBankFixed);

            IEventList<IFinancialServiceBankAccount> fsbAcctListVar = _mockery.StrictMock<IEventList<IFinancialServiceBankAccount>>();
            SetupResult.For(fsVar.FinancialServiceBankAccounts).IgnoreArguments().Return(fsbAcctListVar);

            IFinancialServiceBankAccount fsbBankVar = _mockery.StrictMock<IFinancialServiceBankAccount>();
            SetupResult.For(fsbAcctListVar[0]).IgnoreArguments().Return(fsbBankVar);

            IBankAccount fsBankAcct = _mockery.StrictMock<IBankAccount>();

            SetupResult.For(fsbBankFixed.BankAccount).Return(fsBankAcct);
            SetupResult.For(fsbBankVar.BankAccount).Return(fsBankAcct);

            SetupResult.For(fsbBankFixed.DebitOrderDay).IgnoreArguments().Return(1);
            SetupResult.For(fsbBankVar.DebitOrderDay).IgnoreArguments().Return(1);

            ExecuteRule(rule, 0, fsbank);
        }
    }
}
