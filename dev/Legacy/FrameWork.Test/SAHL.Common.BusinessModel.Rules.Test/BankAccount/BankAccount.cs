using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.BankAccount;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.BankAccount
{
    [TestFixture]
    public class BankAccount : RuleBase
    {
        IBankAccount _ba = null;
        IACBBranch _br = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        #region BankAccountCDVValidation

        [NUnit.Framework.Test]
        public void BankAccountCDVValidationTestPass()
        {
            BankAccountCDVValidation rule = new BankAccountCDVValidation();

            // Should not be doing retrieves from db
            //string HQL = "select ba from BankAccount_DAO ba where ba.ACBBranch =?";
            //SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL, "632005");
            //q.SetQueryRange(1);
            //BankAccount_DAO[] res = q.Execute();
            //IBankAccount ba = new SAHL.Common.BusinessModel.BankAccount(res[0]); // there should always be at least one bank account

            IBankAccount ba = _mockery.StrictMock<IBankAccount>();
            IACBBranch branch = _mockery.StrictMock<IACBBranch>();
            IACBType type = _mockery.StrictMock<IACBType>();
            SetupResult.For(branch.Key).Return("632005");
            SetupResult.For(type.Key).Return(1);

            //
            SetupResult.For(ba.ACBBranch).Return(branch);
            SetupResult.For(ba.ACBType).Return(type);
            SetupResult.For(ba.AccountNumber).Return("9056937882");

            //
            ExecuteRule(rule, 0, ba);
        }

        [NUnit.Framework.Test]
        public void BankAccountCDVValidationTestFail()
        {
            BankAccountCDVValidation rule = new BankAccountCDVValidation();

            // Should not be doing retrieves from db
            //string HQL = "select ba from BankAccount_DAO ba where ba.ACBBranch =?";
            //SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL, "632005");
            //q.SetQueryRange(1);
            //BankAccount_DAO[] res = q.Execute();
            //IBankAccount ba = new SAHL.Common.BusinessModel.BankAccount(res[0]); // there should always be at least one bank account
            //ba.AccountNumber = ba.AccountNumber + ba.AccountNumber;

            IBankAccount ba = _mockery.StrictMock<IBankAccount>();
            IACBBranch branch = _mockery.StrictMock<IACBBranch>();
            IACBType type = _mockery.StrictMock<IACBType>();
            SetupResult.For(branch.Key).Return("632005");
            SetupResult.For(type.Key).Return(1);

            //
            SetupResult.For(ba.ACBBranch).Return(branch);
            SetupResult.For(ba.ACBType).Return(type);
            SetupResult.For(ba.AccountNumber).Return("90569378829056937882");

            //
            ExecuteRule(rule, 1, ba);
        }

        #endregion BankAccountCDVValidation

        #region BankAccountUnique

        [NUnit.Framework.Test]
        public void BankAccountUniquePass()
        {
            using (new SessionScope())
            {
                BankAccountUnique rule = new BankAccountUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                // we need to get hold of an account that is under cancellation
                string sql = @"select top 1 ba.*
                from [2am].[dbo].[BankAccount] ba (nolock)
                inner join [2am].[dbo].[ACBBranch] ab (nolock)
                on ba.ACBBranchCode = ab.ACBBranchCode
                where ab.ACBBranchCode > 0";

                SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(QueryLanguage.Sql, sql);
                q.AddSqlReturnDefinition(typeof(BankAccount_DAO), "ba");
                BankAccount_DAO[] res = q.Execute();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IBankAccount ba = BMTM.GetMappedType<IBankAccount>(res[0]); // there should always be at least one bank account

                _ba = _mockery.StrictMock<IBankAccount>();
                _br = _mockery.StrictMock<IACBBranch>();

                SetupResult.For(_br.Key).Return(ba.ACBBranch.Key);
                SetupResult.For(_ba.Key).Return(ba.Key);
                SetupResult.For(_ba.AccountNumber).Return(ba.AccountNumber);
                SetupResult.For(_ba.ACBBranch).Return(_br);

                ExecuteRule(rule, 0, _ba);
            }
        }

        [NUnit.Framework.Test]
        public void BankAccountUniqueFail()
        {
            using (new SessionScope())
            {
                BankAccountUnique rule = new BankAccountUnique(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                // we need to get hold of an account that is under cancellation
                string sql = @"select top 1 ba.*
                from [2am].[dbo].[BankAccount] ba (nolock)
                inner join [2am].[dbo].[ACBBranch] ab (nolock)
                on ba.ACBBranchCode = ab.ACBBranchCode
                where ab.ACBBranchCode > 0";

                SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(QueryLanguage.Sql, sql);
                q.AddSqlReturnDefinition(typeof(BankAccount_DAO), "ba");
                BankAccount_DAO[] res = q.Execute();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IBankAccount ba = BMTM.GetMappedType<IBankAccount>(res[0]); // there should always be at least one bank account

                _ba = _mockery.StrictMock<IBankAccount>();
                _br = _mockery.StrictMock<IACBBranch>();

                SetupResult.For(_br.Key).Return(ba.ACBBranch.Key);
                SetupResult.For(_ba.Key).Return(0);
                SetupResult.For(_ba.AccountNumber).Return(ba.AccountNumber);
                SetupResult.For(_ba.ACBBranch).Return(_br);

                ExecuteRule(rule, 1, _ba);
            }
        }

        //[NUnit.Framework.Test]
        //public void BankAccountUsedByDebitOrderPass()
        //{
        //    BankAccountUsedByDebitOrder rule = new BankAccountUsedByDebitOrder();

        //    string HQL = "select ba from BankAccount_DAO ba where (select count(fsba) from FinancialServiceBankAccount_DAO fsba where fsba.BankAccount.Key = ba.Key) = 0";
        //    SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL);
        //    q.SetQueryRange(1);
        //    BankAccount_DAO[] res = q.Execute();

        //    IBankAccount ba = new SAHL.Common.BusinessModel.BankAccount(res[0]); // there should always be at least one bank account

        //    ExecuteRule(rule, 0, ba);
        //}

        //[NUnit.Framework.Test]
        //public void BankAccountUsedByDebitOrderFail()
        //{
        //    BankAccountUsedByDebitOrder rule = new BankAccountUsedByDebitOrder();

        //    string HQL = "select ba from BankAccount_DAO ba where (select count(fsba) from FinancialServiceBankAccount_DAO fsba where fsba.BankAccount.Key = ba.Key) > 0";
        //    SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL);
        //    q.SetQueryRange(10);
        //    BankAccount_DAO[] res = q.Execute();

        //    IBankAccount ba = new SAHL.Common.BusinessModel.BankAccount(res[0]); // there should always be at least one bank account

        //    ExecuteRule(rule, 1, ba);
        //}

        [NUnit.Framework.Test]
        public void BankAccountUpdateNotUsedTestStopOrderFail()
        {
            using (new SessionScope())
            {
                BankAccountUpdateNotUsed rule = new BankAccountUpdateNotUsed();

                string HQL = "select ba from BankAccount_DAO ba";
                SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL);
                q.SetQueryRange(1);
                BankAccount_DAO[] res = q.Execute();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IBankAccount ba = BMTM.GetMappedType<IBankAccount>(res[0]); // there should always be at least one bank account

                ExecuteRule(rule, 1, ba);
            }
        }

        [NUnit.Framework.Test]
        public void BankAccountUpdateNotUsedTestFail()
        {
            using (new SessionScope())
            {
                BankAccountUpdateNotUsed rule = new BankAccountUpdateNotUsed();

                string HQL = "select ba from BankAccount_DAO ba where ba.AccountName <> '0' "; // Not a Stop Order
                SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL);
                q.SetQueryRange(1);
                BankAccount_DAO[] res = q.Execute();

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IBankAccount ba = BMTM.GetMappedType<IBankAccount>(res[0]); // there should always be at least one bank account

                ExecuteRule(rule, 1, ba);
            }
        }

        [NUnit.Framework.Test]
        public void BankAccountUpdateNotUsedTestPass()
        {
            BankAccountUpdateNotUsed rule = new BankAccountUpdateNotUsed();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IBankAccountRepository bankRepo = _mockery.StrictMock<IBankAccountRepository>();
            IApplicationRepository appRepo = _mockery.StrictMock<IApplicationRepository>();

            MockCache.Add(typeof(IBankAccountRepository).ToString(), bankRepo);
            MockCache.Add(typeof(IApplicationRepository).ToString(), appRepo);

            IBankAccount bank = _mockery.StrictMock<IBankAccount>();
            IACBBranch acbbranch = _mockery.StrictMock<IACBBranch>();
            IACBType acbType = _mockery.StrictMock<IACBType>();

            SetupResult.For(bank.ACBBranch).Return(acbbranch);
            SetupResult.For(acbbranch.Key).Return("0");
            SetupResult.For(bank.ACBType).Return(acbType);
            SetupResult.For(acbType.Key).Return(0);
            SetupResult.For(bank.AccountNumber).Return("192992929");
            SetupResult.For(bank.AccountName).Return("test");
            IBankAccountSearchCriteria bas = _mockery.StrictMock<IBankAccountSearchCriteria>();

            SetupResult.For(bas.ACBBranchKey).Return("9999");
            SetupResult.For(bas.AccountNumber).Return("123456789");
            SetupResult.For(bas.ACBTypeKey).Return(1);

            SetupResult.For(bankRepo.SearchLegalEntityBankAccounts(bas, 0)).IgnoreArguments().Return(null);
            SetupResult.For(bank.GetFinancialServiceBankAccounts()).IgnoreArguments().Return(null);
            SetupResult.For(appRepo.GetApplicationExpenseByBankAccountNameAndBankAccountNumber(null, null)).IgnoreArguments().Return(null);
            _mockery.ReplayAll();
            ExecuteRule(rule, 0, bank);
        }

        #endregion BankAccountUnique
    }
}