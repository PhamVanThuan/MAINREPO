using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules.FinancialTransaction;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.FinancialTransaction
{
    [TestFixture]
    public class FinancialTransaction : RuleBase
    {
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

        #region PostTransactionNonPerformingLoan967

        /// <summary>
        /// The 967 transaction can only be posted against a loan that is currently marked as non-performing AND that has had a 966 transaction posted against it.
        /// </summary>
        [NUnit.Framework.Test]
        public void PostTransactionNonPerformingLoan967Test()
        {
            PostTransactionNonPerformingLoan967 rule = new PostTransactionNonPerformingLoan967();

            IFinancialServiceRepository financialServiceRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add(typeof(IFinancialServiceRepository).ToString(), financialServiceRepo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IAccount acc = _mockery.StrictMock<IAccount>();
            IFinancialService fs = _mockery.StrictMock<IFinancialService>();
            IEventList<IFinancialService> fss = new EventList<IFinancialService>(); ;
            IFinancialTransaction lt = _mockery.StrictMock<IFinancialTransaction>();
            IEventList<IFinancialTransaction> lts = new EventList<IFinancialTransaction>();
            ITransactionType tt = _mockery.StrictMock<ITransactionType>();
            IDomainMessageCollection dmc = new DomainMessageCollection();
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            IFinancialServiceType ft = _mockery.StrictMock<IFinancialServiceType>();

            // Child FS
            IFinancialService childFS = _mockery.StrictMock<IFinancialService>();
            IEventList<IFinancialService> childFSList = new EventList<IFinancialService>();

            // Setup Parent FS
            SetupResult.For(ft.Key).Return((int)FinancialServiceTypes.VariableLoan);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(fs.FinancialServiceType).Return(ft);
            SetupResult.For(fs.AccountStatus).Return(accountStatus);

            // Setup Child FS
            SetupResult.For(childFS.FinancialServiceType).Return(ft);
            SetupResult.For(childFS.AccountStatus).Return(accountStatus);
            childFSList.Add(dmc, childFS);
            SetupResult.For(fs.FinancialServices).Return(childFSList);

            // PASS - IsNonPerforming = True and has 966 transactions
            bool result = true;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(true);
            SetupResult.For(tt.Key).Return((Int16)TransactionTypes.NonPerformingInterest);
            SetupResult.For(lt.TransactionType).Return(tt);
            lts.Add(dmc, lt);

            //Robin --
            SetupResult.For(childFS.FinancialTransactions).Return(lts);
            fss.Add(dmc, fs);
            SetupResult.For(acc.FinancialServices).Return(fss);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 0, acc);

            // FAIL - IsNonPerforming = True and has no 966 transactions

            childFSList = new EventList<IFinancialService>();
            lts = new EventList<IFinancialTransaction>();

            // Setup Parent FS
            SetupResult.For(ft.Key).Return((int)FinancialServiceTypes.VariableLoan);
            SetupResult.For(accountStatus.Key).Return((int)AccountStatuses.Open);
            SetupResult.For(fs.FinancialServiceType).Return(ft);
            SetupResult.For(fs.AccountStatus).Return(accountStatus);

            // Setup Child FS
            SetupResult.For(childFS.FinancialServiceType).Return(ft);
            SetupResult.For(childFS.AccountStatus).Return(accountStatus);
            childFSList.Add(dmc, childFS);
            SetupResult.For(fs.FinancialServices).Return(childFSList);

            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(true);
            SetupResult.For(tt.Key).Return((Int16)1);
            SetupResult.For(lt.TransactionType).Return(tt);
            lts.Add(dmc, lt);
            SetupResult.For(childFS.FinancialTransactions).Return(lts);
            fss.Add(dmc, fs);
            SetupResult.For(acc.FinancialServices).Return(fss);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 1, acc);

            // FAIL - IsNonPerforming = False
            result = false;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 1, acc);
        }

        #endregion PostTransactionNonPerformingLoan967

        #region PostTransactionNonPerformingLoan236_966Test

        /// <summary>
        /// The 236 and 966 transactions can only be posted against a loan that is currently marked as non-performing.
        /// </summary>
        [NUnit.Framework.Test]
        public void PostTransactionNonPerformingLoan236_966Test()
        {
            PostTransactionNonPerformingLoan236_966 rule = new PostTransactionNonPerformingLoan236_966();

            IFinancialServiceRepository financialServiceRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add(typeof(IFinancialServiceRepository).ToString(), financialServiceRepo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IAccount acc = _mockery.StrictMock<IAccount>();
            bool result;

            // PASS - IsNonPerforming = True
            result = true;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 0, acc);

            // FAIL - IsNonPerforming = False
            result = false;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 1, acc);
        }

        #endregion PostTransactionNonPerformingLoan236_966Test

        #region RollbackTransactionNonPerformingLoan236

        /// <summary>
        /// The 1236 rollback transaction can only be confirmed against a loan that is currently marked as non-performing.
        /// </summary>
        [NUnit.Framework.Test]
        public void RollbackTransactionNonPerformingLoan236()
        {
            RollbackTransactionNonPerformingLoan236 rule = new RollbackTransactionNonPerformingLoan236();

            IFinancialServiceRepository financialServiceRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add(typeof(IFinancialServiceRepository).ToString(), financialServiceRepo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            IAccount acc = _mockery.StrictMock<IAccount>();
            bool result;

            // PASS - IsNonPerforming = True
            result = true;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 0, acc);

            // FAIL - IsNonPerforming = False
            result = false;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(acc.Key).Return(1);
            ExecuteRule(rule, 1, acc);
        }

        #endregion RollbackTransactionNonPerformingLoan236

        #region PostTransactionCheckDateLessThanFirstOfCurrentMonth

        [Test]
        public void PostTransactionCheckDateLessThanFirstOfCurrentMonth_WhenDatePriorToTheFirstOfCurrentMonth_Should_Fail()
        {
            PostTransactionCheckDateLessThanFirstOfCurrentMonth rule = new PostTransactionCheckDateLessThanFirstOfCurrentMonth();
            DateTime previousMonth = DateTime.Now.AddMonths(-1);
            ExecuteRule(rule, 1, previousMonth);
        }

        [Test]
        public void PostTransactionCheckDateLessThanFirstOfCurrentMonth_WhenDateOnTheFirstOfCurrentMonth_Should_Pass()
        {
            PostTransactionCheckDateLessThanFirstOfCurrentMonth rule = new PostTransactionCheckDateLessThanFirstOfCurrentMonth();
            DateTime FirstOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ExecuteRule(rule, 0, FirstOfCurrentMonth);
        }

        [Test]
        public void PostTransactionCheckDateLessThanFirstOfCurrentMonth_WhenDateAfterTheFirstOfCurrentMonth_Should_Pass()
        {
            PostTransactionCheckDateLessThanFirstOfCurrentMonth rule = new PostTransactionCheckDateLessThanFirstOfCurrentMonth();
            DateTime AfterFirstOfCurrent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5);
            ExecuteRule(rule, 0, AfterFirstOfCurrent);
        }

        [Test]
        public void PostTransactionCheckDateLessThanFirstOfCurrentMonth_WhenDateIsNull_Should_Fail()
        {
            PostTransactionCheckDateLessThanFirstOfCurrentMonth rule = new PostTransactionCheckDateLessThanFirstOfCurrentMonth();
            object obj = null;
            ExecuteRule(rule, 1, obj);
        }

        [Test]
        public void PostTransactionCheckDateLessThanFirstOfCurrentMonth_WhenDateIsDefault_Should_Fail()
        {
            PostTransactionCheckDateLessThanFirstOfCurrentMonth rule = new PostTransactionCheckDateLessThanFirstOfCurrentMonth();
            ExecuteRule(rule, 1, new DateTime());
        }

        #endregion PostTransactionCheckDateLessThanFirstOfCurrentMonth

        #region PostTransactionCheckEffectiveDate

        [Test, TestCaseSource("GetPaymentTransactions")]
        public void PostTransactionCheckEffectiveDate_BackDatedPayment_ThisMonth_Pass(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 0, DateTime.Now, txnKey);
        }

        [Test, TestCaseSource("GetPaymentTransactions")]
        public void PostTransactionCheckEffectiveDate_BackDatedPayment_LastMonth_Pass(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 0, DateTime.Now.AddMonths(-1), txnKey);
        }

        [Test, TestCaseSource("GetPaymentTransactions")]
        public void PostTransactionCheckEffectiveDate_BackDatedPayment_Previous2Month_Fail(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 1, DateTime.Now.AddMonths(-2), txnKey);
        }

        [Test, TestCaseSource("GetPaymentTransactions")]
        public void PostTransactionCheckEffectiveDate_BackDatedPayment_Future_Fail(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 1, DateTime.Now.AddDays(1), txnKey);
        }

        [Test, TestCaseSource("GetTransactionsNonPayment")]
        public void PostTransactionCheckEffectiveDate_NonPayment_ThisMonth_Pass(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 0, DateTime.Now, txnKey);
        }

        [Test, TestCaseSource("GetTransactionsNonPayment")]
        public void PostTransactionCheckEffectiveDate_NonPayment_Future_Fail(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 1, DateTime.Now.AddDays(1), txnKey);
        }

        [Test, TestCaseSource("GetTransactionsNonPayment")]
        public void PostTransactionCheckEffectiveDate_NonPayment_LastMonth_Fail(int txnKey)
        {
            PostTransactionCheckEffectiveDate rule = new PostTransactionCheckEffectiveDate(RepositoryFactory.GetRepository<ICastleTransactionsService>());
            ExecuteRule(rule, 1, DateTime.Now.AddMonths(-1), txnKey);
        }

        static List<int> GetPaymentTransactions()
        {
            return new List<int> { 310, 1310, 311, 1311, 312, 1312, 320, 1320, 600, 1600, 620, 1620, 630, 1630, 710, 1710, 621, 1621, 161, 1161 };
        }

        static List<int> GetTransactionsNonPayment()
        {
            return new List<int> {110, 120, 130, 140, 141, 150, 160, 165, 170, 180, 190, 191, 195, 196, 210, 211, 220, 221, 225, 230, 231, 235, 236, 237, 250,
                                    255, 261, 265, 270, 275, 313, 321, 330, 340, 350, 360, 370, 399, 400, 410, 415, 420, 425, 426, 427, 430, 440, 441, 445, 446,
                                    450, 451, 452, 453, 460, 465, 466, 470, 471, 472, 475, 480, 481, 485, 486, 490, 495, 510, 550, 560, 561, 562, 570, 571, 610,
                                    720, 721, 722, 723, 724, 725, 726, 730, 731, 732, 733, 734, 735, 736, 737, 738, 739, 760, 761, 765, 820, 830, 831, 832, 833,
                                    834, 835, 836, 840, 841, 850, 900, 901, 902, 903, 904, 905, 906, 907, 908, 909, 910, 911, 912, 913, 915, 916, 920, 921, 922,
                                    923, 925, 930, 931, 932, 933, 934, 935, 936, 937, 940, 950, 960, 961, 965, 966, 967, 968, 970, 971, 972, 973, 974, 975, 1110,
                                    1120, 1130, 1140, 1141, 1150, 1160, 1165, 1170, 1180, 1190, 1191, 1195, 1196, 1210, 1211, 1220, 1221, 1225, 1230, 1235, 1236,
                                    1237, 1240, 1250, 1255, 1261, 1265, 1270, 1275, 1300, 1313, 1321, 1330, 1340, 1350, 1360, 1370, 1399, 1400, 1410, 1415, 1420,
                                    1425, 1426, 1427, 1430, 1440, 1441, 1445, 1446, 1450, 1451, 1452, 1453, 1460, 1465, 1466, 1470, 1471, 1472, 1475, 1480, 1481,
                                    1485, 1486, 1490, 1495, 1510, 1550, 1560, 1561, 1570, 1571, 1610, 1720, 1722, 1730, 1732, 1737, 1738, 1739, 1760, 1761, 1765,
                                    1820, 1830, 1831, 1832, 1833, 1834, 1835, 1836, 1840, 1841, 1850, 1925, 1968, 1969, 1970, 1971, 1972, 1973, 1974, 1975, 1976,
                                    1977, 1978, 1979, 1980, 1981, 1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997,
                                    1998, 1999, 2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018,
                                    2019, 2020};

            //            string sql = @"select TransactionTypeKey from fin.TransactionType tt
            //				where  tt.transactiontypekey
            //				not in (310, 1310, 311, 1311, 312, 1312, 320, 1320, 600, 1600, 620, 1620, 630, 1630, 710, 1710, 621, 1621, 161, 1161)";

            //            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(SAHL.Common.BusinessModel.DAO.GeneralStatus_DAO), null);

            //            var txnList = new List<int>();

            //            foreach (DataRow r in ds.Tables[0].Rows)
            //            {
            //                txnList.Add(Int32.Parse(r[0].ToString()));
            //            }

            //            return txnList;
        }

        #endregion PostTransactionCheckEffectiveDate
    }
}