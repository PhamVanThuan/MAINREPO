using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.FixedDebitOrder;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.FixedDebitOrder
{
    [TestFixture]
    public class FutureDatedChangeTest : RuleBase
    {
        private IFutureDatedChange _fdc;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _fdc = _mockery.StrictMock<IFutureDatedChange>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void FinancialServiceDOEffectiveDateBusinessDayTestFail()
        {
            FinancialServiceDOEffectiveDateBusinessDay rule = new FinancialServiceDOEffectiveDateBusinessDay();

            DateTime effectiveDate = Convert.ToDateTime("2008/02/09");  // Saturday
            SetupResult.For(_fdc.EffectiveDate).IgnoreArguments().Return(effectiveDate);

            ExecuteRule(rule, 1, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceDOEffectiveDateBusinessDayTestPass()
        {
            FinancialServiceDOEffectiveDateBusinessDay rule = new FinancialServiceDOEffectiveDateBusinessDay();

            DateTime effectiveDate = Convert.ToDateTime("2008/02/08");  // Friday
            SetupResult.For(_fdc.EffectiveDate).IgnoreArguments().Return(effectiveDate);

            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceDOEffectiveDatePreviousDayBusinessDayTestPass()
        {
            FinancialServiceDOEffectiveDatePreviousDayBusinessDay rule = new FinancialServiceDOEffectiveDatePreviousDayBusinessDay();

            DateTime effectiveDate = Convert.ToDateTime("2008/02/12");  // Tuesday
            SetupResult.For(_fdc.EffectiveDate).IgnoreArguments().Return(effectiveDate);

            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceDOEffectiveDatePreviousDayBusinessDayTestFail()
        {
            FinancialServiceDOEffectiveDatePreviousDayBusinessDay rule = new FinancialServiceDOEffectiveDatePreviousDayBusinessDay();

            DateTime effectiveDate = Convert.ToDateTime("2008/02/10");  // Sunday
            SetupResult.For(_fdc.EffectiveDate).IgnoreArguments().Return(effectiveDate);

            ExecuteRule(rule, 1, _fdc);
        }

        [NUnit.Framework.Test]

        // Test where there are no other FutureDatedTrans for the account
        public void FinancialServiceDebitOrderUpdateDayMonthDoneTestPass()
        {
            FinancialServiceDebitOrderUpdateDayMonthDone rule = new FinancialServiceDebitOrderUpdateDayMonthDone();

            FinancialService_DAO fs = FinancialService_DAO.FindFirst();
            int key = fs.Key;
            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(key);

            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Now);  // Giving DateTime.Now to make the dates the same
            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceDebitOrderUpdateDayMonthDoneTestFail()
        {
            FinancialServiceDebitOrderUpdateDayMonthDone rule = new FinancialServiceDebitOrderUpdateDayMonthDone();

            IList<IFutureDatedChange> fdcList = new List<IFutureDatedChange>();
            DateTime effectiveDate = DateTime.Now.AddDays(9);
            SetupResult.For(_fdc.EffectiveDate).Return(effectiveDate);
            string tableName = "FinancialServiceBankAccount";

            string hql = "SELECT f FROM FutureDatedChange_DAO f JOIN f.FutureDatedChangeDetails fd WHERE f.FutureDatedChangeType.Key = ? and fd.TableName = ?";
            SimpleQuery<FutureDatedChange_DAO> q = new SimpleQuery<FutureDatedChange_DAO>(hql, (int)FutureDatedChangeTypes.NormalDebitOrder, tableName);
            FutureDatedChange_DAO[] fdcRec = q.Execute();

            if (fdcRec == null || fdcRec.Length == 0)
            {
                Assert.Ignore("No data.");
                return;
            }

            int IdentifierReferenceKey = 1;

            for (int i = 0; i < fdcRec.Length; i++)
            {
                if (fdcRec[i].EffectiveDate.Month == effectiveDate.Month)
                {
                    IdentifierReferenceKey = fdcRec[i].IdentifierReferenceKey;
                    break;
                }
            }

            if (IdentifierReferenceKey > 1)
            {
                SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(IdentifierReferenceKey);

                ExecuteRule(rule, 1, _fdc);
            }
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountAddNewFixedDebitChangeReferenceTestRuleExistsOnDB()
        {
            FinancialServiceBankAccountAddNewFixedDebitChangeReference rule = new FinancialServiceBankAccountAddNewFixedDebitChangeReference();

            Account_DAO acc = Account_DAO.FindFirst();
            int key = acc.Key;

            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(key);
            IFutureDatedChangeType fdcType = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(_fdc.FutureDatedChangeType).Return(fdcType);
            SetupResult.For(fdcType.Key).IgnoreArguments().Return((int)FutureDatedChangeTypes.NormalDebitOrder);

            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountAddNewFixedDebitChangeReferenceTestFail()
        {
            FinancialServiceBankAccountAddNewFixedDebitChangeReference rule = new FinancialServiceBankAccountAddNewFixedDebitChangeReference();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IAccountRepository accountRepo = _mockery.StrictMock<IAccountRepository>();
            MockCache.Add((typeof(IAccountRepository)).ToString(), accountRepo);
            IAccount acc;

            acc = null;

            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(0);
            IFutureDatedChangeType fdcType = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(_fdc.FutureDatedChangeType).Return(fdcType);
            SetupResult.For(fdcType.Key).Return((int)FutureDatedChangeTypes.FixedDebitOrder);

            SetupResult.For(accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(null);

            ExecuteRule(rule, 1, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountAddNewDebitOrderChangeReferenceTestFail()
        {
            FinancialServiceBankAccountAddNewDebitOrderChangeReference rule = new FinancialServiceBankAccountAddNewDebitOrderChangeReference();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IFinancialServiceRepository fsRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), fsRepo);

            IFinancialService fs = null;

            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(0);
            IFutureDatedChangeType fdcType = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(_fdc.FutureDatedChangeType).Return(fdcType);
            SetupResult.For(fdcType.Key).Return((int)FutureDatedChangeTypes.NormalDebitOrder);

            SetupResult.For(fsRepo.GetFinancialServiceByKey(0)).IgnoreArguments().Return(null);

            ExecuteRule(rule, 1, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountAddNewFixedDebitChangeReferenceTestPass()
        {
            FinancialServiceBankAccountAddNewFixedDebitChangeReference rule = new FinancialServiceBankAccountAddNewFixedDebitChangeReference();

            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), fsRepo);

            Account_DAO acc = Account_DAO.FindFirst();
            int key = acc.Key;
            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(key);

            IFutureDatedChangeType fdcType = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(_fdc.FutureDatedChangeType).Return(fdcType);
            SetupResult.For(fdcType.Key).Return((int)FutureDatedChangeTypes.FixedDebitOrder);

            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FinancialServiceBankAccountAddNewDebitOrderChangeReferenceTestPass()
        {
            FinancialServiceBankAccountAddNewDebitOrderChangeReference rule = new FinancialServiceBankAccountAddNewDebitOrderChangeReference();

            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), fsRepo);

            IFutureDatedChangeType fdcType = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(_fdc.FutureDatedChangeType).Return(fdcType);
            SetupResult.For(fdcType.Key).Return((int)FutureDatedChangeTypes.NormalDebitOrder);

            FinancialService_DAO fs = FinancialService_DAO.FindFirst();
            int key = fs.Key;
            SetupResult.For(_fdc.IdentifierReferenceKey).IgnoreArguments().Return(key);

            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FutureDatedChangeEffectiveDateMinimumTest()
        {
            FutureDatedChangeEffectiveDateMinimum rule = new FutureDatedChangeEffectiveDateMinimum();

            // yesterday - should fail
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Today.AddDays(-1));
            ExecuteRule(rule, 1, _fdc);

            // today - should fail
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Today);
            ExecuteRule(rule, 0, _fdc);

            // yesterday - should pass
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Today.AddDays(1));
            ExecuteRule(rule, 0, _fdc);
        }

        [NUnit.Framework.Test]
        public void FutureDatedChangeEffectiveDateMaximumTest()
        {
            FutureDatedChangeEffectiveDateMaximum rule = new FutureDatedChangeEffectiveDateMaximum();

            // 1 day less than 6 months - should pass
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Now.AddMonths(6).AddDays(-1));
            ExecuteRule(rule, 0, _fdc);

            // 6 months - should pass
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Now.AddMonths(6));
            ExecuteRule(rule, 0, _fdc);

            // 1 day over 6 months - should fail
            SetupResult.For(_fdc.EffectiveDate).Return(DateTime.Now.AddMonths(6).AddDays(1));
            ExecuteRule(rule, 1, _fdc);
        }

        [NUnit.Framework.Test]
        public void FutureDatedChangeEffectiveDateCheckTest()
        {
            IFinancialServiceBankAccount bankAccount = _mockery.StrictMock<IFinancialServiceBankAccount>();

            // future dated change type not Manual Debit Order - pass
            FutureDatedChangeEffectiveDateCheckTestHelper(bankAccount, FutureDatedChangeTypes.FixedDebitOrder, 2, 2, 0);

            // bank account null - pass
            FutureDatedChangeEffectiveDateCheckTestHelper(null, FutureDatedChangeTypes.NormalDebitOrder, 2, 2, 0);

            // effective day before current debit order day - pass
            FutureDatedChangeEffectiveDateCheckTestHelper(bankAccount, FutureDatedChangeTypes.NormalDebitOrder, 1, 2, 0);

            // effective day on current debit order day - fail
            FutureDatedChangeEffectiveDateCheckTestHelper(bankAccount, FutureDatedChangeTypes.NormalDebitOrder, 2, 2, 1);

            // effective day after current debit order day - pass
            FutureDatedChangeEffectiveDateCheckTestHelper(bankAccount, FutureDatedChangeTypes.NormalDebitOrder, 3, 2, 0);
        }

        private void FutureDatedChangeEffectiveDateCheckTestHelper(IFinancialServiceBankAccount bankAccount,
            FutureDatedChangeTypes fdcType, int effectiveDay, int currentDebitOrderDay, int expectedMessageCount)
        {
            FutureDatedChangeEffectiveDateCheck rule = new FutureDatedChangeEffectiveDateCheck();

            IFutureDatedChange fdc = _mockery.StrictMock<IFutureDatedChange>();
            IFutureDatedChangeType futureDatedChangeType = _mockery.StrictMock<IFutureDatedChangeType>();
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();

            SetupResult.For(fdc.FutureDatedChangeType).Return(futureDatedChangeType);
            SetupResult.For(fdc.IdentifierReferenceKey).Return(1).IgnoreArguments();
            SetupResult.For(futureDatedChangeType.Key).Return((int)fdcType);

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IFinancialServiceRepository fsRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), fsRepo);
            SetupResult.For(fsRepo.GetFinancialServiceByKey(0)).IgnoreArguments().Return(financialService);

            SetupResult.For(financialService.CurrentBankAccount).Return(bankAccount);
            if (bankAccount != null)
            {
                DateTime dt = DateTime.Now.AddMonths(1);
                DateTime effectiveDate = new DateTime(dt.Year, dt.Month, effectiveDay);
                SetupResult.For(fdc.EffectiveDate).Return(effectiveDate);
                SetupResult.For(bankAccount.DebitOrderDay).Return(currentDebitOrderDay);
            }

            ExecuteRule(rule, expectedMessageCount, fdc);
        }

        /// <summary>
        /// Tests the FutureDatedChangeSinglePaymentCheck.  This uses a <see cref="MonthCheckObject"/> to
        /// run multiple tests.  This works by starting with an unpopulated object and slowly populating
        /// it so the rule gets futher and further through the stages.  It has to run in the order it is
        /// written for all the code to be tested.
        /// </summary>
        [Test]
        public void FutureDatedChangeSinglePaymentCheckTest()
        {
            IFutureDatedChangeType futureDatedChangeType = _mockery.StrictMock<IFutureDatedChangeType>();
            MonthCheckObject smco = new MonthCheckObject();
            IFutureDatedChange fdc = _mockery.StrictMock<IFutureDatedChange>();
            IFutureDatedChangeDetail fdcDetail = _mockery.StrictMock<IFutureDatedChangeDetail>();
            IFinancialServiceBankAccount fsBankAccountCurrent = _mockery.StrictMock<IFinancialServiceBankAccount>();
            IFinancialServicePaymentType fsPaymentType = _mockery.StrictMock<IFinancialServicePaymentType>();
            SetupResult.For(fsPaymentType.Key).Return(2);

            // no future dated change type set - pass
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // FutureDatedChangeType of FixedDebitOrder - pass
            smco.FutureDatedChangeType = futureDatedChangeType;
            smco.FutureDatedChangeTypeKey = (int)FutureDatedChangeTypes.FixedDebitOrder;
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // normal debit order but no attached bank accounts (collection still empty) - pass
            smco.FutureDatedChangeTypeKey = (int)FutureDatedChangeTypes.NormalDebitOrder;
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // no current bank account - pass
            smco.FinancialServiceBankAccountCount = 2;
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // current bank account key = new bank account key - pass
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return(smco.CurrentDebitOrderDay.ToString());
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ChangeDate).Return(DateTime.Now);
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            SetupResult.For(fdc.EffectiveDate).Return(DateTime.Now);
            SetupResult.For(fdc.ChangeDate).Return(DateTime.Now);

            smco.FutureDatedChange = fdc;
            smco.CurrentBankAccount = fsBankAccountCurrent;
            smco.CurrentBankAccountKey = 2;
            smco.NewBankAccountKey = 2;

            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            smco.CurrentBankAccountKey = 2;
            smco.NewBankAccountKey = 11;

            // test the various possible combinations of NewDebitOrderDay(N), CurrentDebitOrderDay,
            // EffectiveDate (E) (future dated change date is always 7 in the test)
            //      C | E | N
            //      E | C | N
            //      E | N | C
            //      C | N | E
            //      N | E | C
            //      N | C | E

            // C/E/N - fail
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("7"); //N
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            smco.CurrentDebitOrderDay = 2; //C

            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 4)); //E
            FutureDatedChangeSinglePaymentCheckTestHelper(1, smco);

            // E/C/N - pass
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("7"); //N
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            smco.CurrentDebitOrderDay = 5; //C
            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 2)); //E
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // E/N/C - pass
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("5"); //N
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            smco.CurrentDebitOrderDay = 7; //C
            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 2)); //E
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // C/N/E - pass
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("5"); //N
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            smco.CurrentDebitOrderDay = 2; //C
            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 7)); //E
            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // N/E/C - fail
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("2"); //N
            SetupResult.For(fdcDetail.Action).Return('u').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            smco.CurrentDebitOrderDay = 7; //C
            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 5)); //E
            FutureDatedChangeSinglePaymentCheckTestHelper(1, smco);

            // test one pass for a future dated change
            smco.CurrentDebitOrderDay = 5;
            smco.EffectiveDate = new DateTime?(new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(1).Month, 10));

            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return(smco.CurrentDebitOrderDay.ToString());
            SetupResult.For(fdcDetail.Action).Return('a').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            FutureDatedChangeSinglePaymentCheckTestHelper(0, smco);

            // test one fail for a future dated change
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("10");
            SetupResult.For(fdcDetail.Action).Return('a').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            FutureDatedChangeSinglePaymentCheckTestHelper(1, smco);

            // test fail for a future dated change - checks for skipped payment
            //Current debit order day is set in the future

            smco.CurrentDebitOrderDay = 16;
            smco.EffectiveDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);

            //Future Debit order day is set to the 1st of the month which should result in a skipped payment error
            SetupResult.For(fdcDetail.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcDetail.Value).Return("1");
            SetupResult.For(fdcDetail.Action).Return('a').IgnoreArguments();
            SetupResult.For(fdcDetail.ReferenceKey).Return(1);

            smco.FutureDatedChangeDetail = fdcDetail;

            SetupResult.For(fdc.Key).Return(1);
            smco.FutureDatedChange = fdc;

            //Changed test as skipped payment returns a 0
            FutureDatedChangeSinglePaymentCheckTestHelper(1, smco);
        }

        private class MonthCheckObject
        {
            public IFutureDatedChangeType FutureDatedChangeType;
            public int FutureDatedChangeTypeKey;
            public int FinancialServiceBankAccountCount;
            public IFinancialServiceBankAccount CurrentBankAccount;
            public int CurrentBankAccountKey;
            public int CurrentDebitOrderDay;
            public int NewBankAccountKey;
            public DateTime? EffectiveDate = new DateTime?();
            public IFutureDatedChange FutureDatedChange;
            public IFutureDatedChangeDetail FutureDatedChangeDetail;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expectedMessageCount">Expected message count after running the rule.</param>
        private void FutureDatedChangeSinglePaymentCheckTestHelper(int expectedMessageCount,
            MonthCheckObject smco)
        {
            FutureDatedChangeSinglePaymentCheck rule = new FutureDatedChangeSinglePaymentCheck();

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IFinancialServiceRepository fsRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add((typeof(IFinancialServiceRepository)).ToString(), fsRepo);
            IFutureDatedChangeRepository fdcRepo = _mockery.StrictMock<IFutureDatedChangeRepository>();
            MockCache.Add((typeof(IFutureDatedChangeRepository)).ToString(), fdcRepo);

            // set up the mocked objects
            IFinancialService financialService = _mockery.StrictMock<IFinancialService>();
            SetupResult.For(financialService.Key).Return(1);
            IEventList<IFinancialServiceBankAccount> fsBankAccounts = new EventList<IFinancialServiceBankAccount>();

            // create a few financial service bank account objects
            for (int i = 1; i <= smco.FinancialServiceBankAccountCount; i++)
            {
                IFinancialServiceBankAccount fsba = _mockery.StrictMock<IFinancialServiceBankAccount>();
                SetupResult.For(fsba.Key).Return(i);
                SetupResult.For(fsba.DebitOrderDay).Return(7);
                fsBankAccounts.Add(null, fsba);
            }

            SetupResult.For(_fdc.FutureDatedChangeType).Return(smco.FutureDatedChangeType);
            if (smco.FutureDatedChangeType != null)
                SetupResult.For(smco.FutureDatedChangeType.Key).Return(smco.FutureDatedChangeTypeKey);
            SetupResult.For(_fdc.IdentifierReferenceKey).Return(1);
            SetupResult.For(fsRepo.GetFinancialServiceByKey(0)).IgnoreArguments().Return(financialService);
            SetupResult.For(financialService.FinancialServiceBankAccounts).Return(fsBankAccounts);
            SetupResult.For(financialService.CurrentBankAccount).Return(smco.CurrentBankAccount);
            if (smco.CurrentBankAccount != null)
            {
                SetupResult.For(smco.CurrentBankAccount.Key).Return(smco.CurrentBankAccountKey);
                SetupResult.For(smco.CurrentBankAccount.DebitOrderDay).Return(smco.CurrentDebitOrderDay);
            }
            if (smco.EffectiveDate.HasValue)
                SetupResult.For(_fdc.EffectiveDate).Return(smco.EffectiveDate.Value);

            // future dated change setup
            IList<IFutureDatedChange> lstFutureDatedChange = new List<IFutureDatedChange>();
            SetupResult.For(fdcRepo.GetFutureDatedChangesByGenericKey(1, 1)).IgnoreArguments().Return(lstFutureDatedChange);
            if (smco.FutureDatedChange != null)
            {
                if (smco.CurrentBankAccountKey != smco.NewBankAccountKey)
                {
                    SetupResult.For(_fdc.Key).Return(1);

                    IFutureDatedChange FutureDatedChange2 = _mockery.StrictMock<IFutureDatedChange>();
                    SetupResult.For(FutureDatedChange2.Key).Return(int.MaxValue);

                    SetupResult.For(FutureDatedChange2.FutureDatedChangeType).Return(smco.FutureDatedChangeType);
                    SetupResult.For(FutureDatedChange2.IdentifierReferenceKey).Return(1);
                    SetupResult.For(FutureDatedChange2.EffectiveDate).Return(smco.EffectiveDate.HasValue ? smco.EffectiveDate.Value : DateTime.Now);

                    if (smco.FutureDatedChangeDetail != null)
                    {
                        IEventList<IFutureDatedChangeDetail> lstFutureDatedChangeDetails = _mockery.StrictMock<EventList<IFutureDatedChangeDetail>>();
                        lstFutureDatedChangeDetails = new EventList<IFutureDatedChangeDetail>();
                        lstFutureDatedChangeDetails.Add(null, smco.FutureDatedChangeDetail);
                        SetupResult.For(FutureDatedChange2.FutureDatedChangeDetails).Return(lstFutureDatedChangeDetails);
                    }
                    lstFutureDatedChange.Add(FutureDatedChange2);
                }
                else
                    lstFutureDatedChange.Add(smco.FutureDatedChange);
            }

            if (smco.FutureDatedChangeDetail != null)
            {
                IEventList<IFutureDatedChangeDetail> lstFutureDatedChangeDetails = _mockery.StrictMock<EventList<IFutureDatedChangeDetail>>();
                lstFutureDatedChangeDetails = new EventList<IFutureDatedChangeDetail>();
                lstFutureDatedChangeDetails.Add(null, smco.FutureDatedChangeDetail);

                SetupResult.For(_fdc.FutureDatedChangeDetails).Return(lstFutureDatedChangeDetails);
            }

            IFinancialServiceBankAccount fsbaNew = _mockery.StrictMock<IFinancialServiceBankAccount>();
            SetupResult.For(fsbaNew.Key).Return(smco.NewBankAccountKey);

            SetupResult.For(fsbaNew.DebitOrderDay).Return(smco.CurrentDebitOrderDay);
            SetupResult.For(fsRepo.GetFinancialServiceBankAccountByKey(0)).IgnoreArguments().Return(fsbaNew);

            ExecuteRule(rule, expectedMessageCount, _fdc);
        }

        #region DebitOrderFinancialServiceCheck Test

        [Test]
        public void DebitOrderFinancialServiceCheckTest()
        {
            using (new SessionScope())
            {
                DebitOrderFinancialServiceCheck rule = new DebitOrderFinancialServiceCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string testAccountQuery = @"select top 1 fs.AccountKey
                from FinancialService fs (nolock)
                where
	                fs.AccountStatusKey in (1,5)
                and
	                fs.FinancialServiceTypeKey in (1,2)";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testAccountQuery, typeof(FinancialService_DAO), new ParameterCollection());

                if (o != null)
                {
                    int AccountKey = Convert.ToInt32(o);
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IAccount account = accRepo.GetAccountByKey(AccountKey);
                    ExecuteRule(rule, 0, account);
                }
                else
                    Assert.Fail("No test data");
            }
        }

        [Test]
        public void DebitOrderFinancialServiceCheckFail()
        {
            using (new SessionScope())
            {
                DebitOrderFinancialServiceCheck rule = new DebitOrderFinancialServiceCheck(RepositoryFactory.GetRepository<ICastleTransactionsService>());

                string testAccountQuery = @"select top 1 fs.AccountKey
                from FinancialService fs (nolock)
                where
	                fs.AccountStatusKey in (3)
                and
	                fs.FinancialServiceTypeKey in (1,2)";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testAccountQuery, typeof(FinancialService_DAO), new ParameterCollection());

                if (o != null)
                {
                    int AccountKey = Convert.ToInt32(o);
                    IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                    IAccount account = accRepo.GetAccountByKey(AccountKey);
                    ExecuteRule(rule, 1, account);
                }
                else
                    Assert.Inconclusive("No test data");
            }
        }

        #endregion DebitOrderFinancialServiceCheck Test

        [Test]
        public void FutureDatedChangeEffectiveDateDODayCheckTest()
        {
            FutureDatedChangeEffectiveDateDODayCheckHelper(1, "1", new DateTime(2010, 4, 1));
            FutureDatedChangeEffectiveDateDODayCheckHelper(0, "1", new DateTime(2010, 4, 2));
            FutureDatedChangeEffectiveDateDODayCheckHelper(1, "1", new DateTime(2010, 5, 1));
            FutureDatedChangeEffectiveDateDODayCheckHelper(0, "1", new DateTime(2010, 5, 2));
        }

        private void FutureDatedChangeEffectiveDateDODayCheckHelper(int msgCount, string DODay, DateTime date)
        {
            IFutureDatedChangeDetail fdcD = _mockery.StrictMock<IFutureDatedChangeDetail>();
            SetupResult.For(fdcD.Key).Return(1);
            SetupResult.For(fdcD.ColumnName).Return("DebitOrderDay");
            SetupResult.For(fdcD.TableName).Return("FinancialServiceBankAccount");
            SetupResult.For(fdcD.Value).Return(DODay);

            IEventList<IFutureDatedChangeDetail> details = new EventList<IFutureDatedChangeDetail>();
            details.Add(null, fdcD);

            IFutureDatedChangeType fdcT = _mockery.StrictMock<IFutureDatedChangeType>();
            SetupResult.For(fdcT.Key).Return((int)FutureDatedChangeTypes.NormalDebitOrder);

            IFutureDatedChange fdc = _mockery.StrictMock<IFutureDatedChange>();
            SetupResult.For(fdc.Key).Return(1);
            SetupResult.For(fdc.FutureDatedChangeType).Return(fdcT);
            SetupResult.For(fdc.FutureDatedChangeDetails).Return(details);
            SetupResult.For(fdc.EffectiveDate).Return(date);

            FutureDatedChangeEffectiveDateDODayCheck rule = new FutureDatedChangeEffectiveDateDODayCheck();

            ExecuteRule(rule, msgCount, fdc);
        }
    }
}