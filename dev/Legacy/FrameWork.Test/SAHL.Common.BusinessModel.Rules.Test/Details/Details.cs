using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate.Criterion;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.Details;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Rules.Test.Details
{
    [TestFixture]
    public class Details : RuleBase
    {
        [NUnit.Framework.SetUp]
        public new void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Details.DetailsMaximumAmount"/> rule.
        /// </summary>
        [Test]
        public void DetailsMaximumAmountTest()
        {
            using (new SessionScope())
            {
                DetailsMaximumAmount rule = new DetailsMaximumAmount();
                IDetail detail = _mockery.StrictMock<IDetail>();

                // null amount - should pass
                SetupResult.For(detail.Amount).Return(null);
                ExecuteRule(rule, 0, detail);

                // 100 amount - should pass
                SetupResult.For(detail.Amount).Return(100D);
                ExecuteRule(rule, 0, detail);

                // 1,000,000,000 amount - should fail
                SetupResult.For(detail.Amount).Return(1000000000D);
                ExecuteRule(rule, 1, detail);
            }
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Details.DetailsMinimumAmount"/> rule.
        /// </summary>
        [Test]
        public void DetailsMinimumAmountTest()
        {
            using (new SessionScope())
            {
                DetailsMinimumAmount rule = new DetailsMinimumAmount();
                IDetail detail = _mockery.StrictMock<IDetail>();

                // null amount - should pass
                SetupResult.For(detail.Amount).Return(null);
                ExecuteRule(rule, 0, detail);

                // 100 amount - should pass
                SetupResult.For(detail.Amount).Return(100D);
                ExecuteRule(rule, 0, detail);

                // 0 amount - should fail
                SetupResult.For(detail.Amount).Return(0D);
                ExecuteRule(rule, 1, detail);
            }
        }

        [NUnit.Framework.Test]
        public void DetailsMandatoryDateTestPass()
        {
            DetailsMandatoryDate rule = new DetailsMandatoryDate();
            IDetail detail = _mockery.StrictMock<IDetail>();
            DateTime datetime = DateTime.Now;
            SetupResult.For(detail.DetailDate).Return(datetime);
            ExecuteRule(rule, 0, detail);
        }

        [NUnit.Framework.Test]
        public void DetailsMandatoryDateTestFail()
        {
            DetailsMandatoryDate rule = new DetailsMandatoryDate();
            /*
            IDetail detail = _mockery.StrictMock<IDetail>();
            DateTime? datetime = null;
            SetupResult.For(detail.DetailDate).Return(datetime);
            ExecuteRule(rule, 1, detail);
            */
            IDetail detail = _mockery.StrictMock<IDetail>();
            DateTime datetime = new DateTime(1800, 1, 1);
            SetupResult.For(detail.DetailDate).Return(datetime);
            ExecuteRule(rule, 1, detail);
        }

        #region DetailsPositiveLoanBalanceHOCTest

        /// <summary>
        /// Rule for prevented adding "Paid up no HOC" or "Paid up HOC" if client has positive loan balance
        /// </summary>
        [NUnit.Framework.Test]
        public void DetailsPositiveLoanBalanceHOCTest()
        {
            int accPass = 0;
            int accFail = 0;

            IDbConnection con = Helper.GetSQLDBConnection();
            try
            {
                //need to get an account key with > 0 Balance
                string sqlFail = String.Format(@"select top 1 AccountKey
                                                from FinancialService fs (nolock)
                                                join fin.balance bal (nolock) on fs.FinancialServiceKey = bal.FinancialServiceKey
                                                where fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey in (1, 2)
                                                group by fs.AccountKey
                                                having sum(isnull(bal.amount, 0)) > 0.5");

                object obj = Helper.ExecuteScalar(con, sqlFail);

                if (obj != null)
                    accFail = (int)obj;

                //need to get an account with <= 0 Balance
                string sqlPass = String.Format(@"select top 1 AccountKey
                                                from FinancialService fs (nolock)
                                                join fin.balance bal (nolock) on fs.FinancialServiceKey = bal.FinancialServiceKey
                                                where fs.AccountStatusKey = 1 and fs.FinancialServiceTypeKey in (1, 2)
                                                group by fs.AccountKey
                                                having sum(isnull(bal.amount, 0)) < 0.5");

                obj = Helper.ExecuteScalar(con, sqlPass);

                if (obj != null)
                    accPass = (int)obj;
            }
            finally
            {
                if (con != null)
                    con.Dispose();
            }

            if (accFail > 0)
            {
                // Positive LoanBalance and DetailTypeKey is either PaidUpWithHOC or PaidUpWithNoHOC - FAIL
                DetailsPositiveLoanBalanceHOCHelper(1, (int)DetailTypes.PaidUpWithHOC, accFail);
                DetailsPositiveLoanBalanceHOCHelper(1, (int)DetailTypes.PaidUpWithNoHOC, accFail);

                // Positive LoanBalance and DetailTypeKey is neither PaidUpWithHOC or PaidUpWithNoHOC - PASS
                DetailsPositiveLoanBalanceHOCHelper(0, (int)DetailTypes.InstructionSent, accFail);
            }

            if (accPass > 0)
            {
                // Zero LoanBalance and DetailTypeKey is neither PaidUpWithHOC or PaidUpWithNoHOC - PASS
                DetailsPositiveLoanBalanceHOCHelper(0, (int)DetailTypes.InstructionSent, accPass);

                // Zero LoanBalance and DetailTypeKey is either PaidUpWithHOC or PaidUpWithNoHOC - PASS
                DetailsPositiveLoanBalanceHOCHelper(0, (int)DetailTypes.PaidUpWithHOC, accPass);
                DetailsPositiveLoanBalanceHOCHelper(0, (int)DetailTypes.PaidUpWithNoHOC, accPass);
            }
        }

        /// <summary>
        /// Helper method to set up the expectations for the DetailsPositiveLoanBalanceHOC test.
        /// </summary>
        /// <param name="gs"></param>
        private void DetailsPositiveLoanBalanceHOCHelper(int expectedMessageCount, int detailTypeKey, int accKey)
        {
            DetailsPositiveLoanBalanceHOC rule = new DetailsPositiveLoanBalanceHOC();

            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();
            IAccount acc = _mockery.StrictMock<IAccount>();

            SetupResult.For(acc.Key).Return(accKey);

            SetupResult.For(detail.Account).Return(acc);
            SetupResult.For(detailType.Key).Return(detailTypeKey);
            SetupResult.For(detail.DetailType).Return(detailType);

            ExecuteRule(rule, expectedMessageCount, detail);
        }

        #endregion DetailsPositiveLoanBalanceHOCTest

        #region DetailNonPerformingLoanLitigationTest

        /// <summary>
        /// restrictionsd for adding or updating loan details when loan is non-performing -
        /// prevents Litigation to close unless suspended interest has been zeroed
        /// </summary>
        [NUnit.Framework.Test]
        public void DetailNonPerformingLoanLitigationTest()
        {
            DetailNonPerformingLoanLitigation rule = new DetailNonPerformingLoanLitigation();

            IFinancialServiceRepository financialServiceRepo = _mockery.StrictMock<IFinancialServiceRepository>();
            MockCache.Add(typeof(IFinancialServiceRepository).ToString(), financialServiceRepo);
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            IAccount acc = _mockery.StrictMock<IAccount>();
            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();

            bool result;

            // PASS - IsNonPerforming = False and SuspendedInterestAmount = 0
            result = false;

            DateTime? dt;
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(financialServiceRepo.GetSuspendedInterest(1, out dt)).IgnoreArguments().Return(0M);
            SetupResult.For(acc.Key).Return(1);

            SetupResult.For(detail.Account).Return(acc);
            SetupResult.For(detailType.Key).Return((int)DetailTypes.LoanClosed);
            SetupResult.For(detail.DetailType).Return(detailType);

            ExecuteRule(rule, 0, detail);

            // PASS - IsNonPerforming = True and SuspendedInterestAmount = 0
            result = true;

            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(financialServiceRepo.GetSuspendedInterest(1, out dt)).IgnoreArguments().Return(0M);
            SetupResult.For(acc.Key).Return(1);

            SetupResult.For(detail.Account).Return(acc);
            SetupResult.For(detailType.Key).Return((int)DetailTypes.LoanClosed);
            SetupResult.For(detail.DetailType).Return(detailType);

            ExecuteRule(rule, 0, detail);

            // FAIL - IsNonPerforming = True and suspended interest <> 0
            SetupResult.For(financialServiceRepo.IsLoanNonPerforming(1)).IgnoreArguments().Return(result);
            SetupResult.For(financialServiceRepo.GetSuspendedInterest(1, out dt)).IgnoreArguments().Return(1M);
            SetupResult.For(acc.Key).Return(1);

            SetupResult.For(detail.Account).Return(acc);
            SetupResult.For(detailType.Key).Return((int)DetailTypes.LoanClosed);
            SetupResult.For(detail.DetailType).Return(detailType);

            ExecuteRule(rule, 1, detail);
        }

        #endregion DetailNonPerformingLoanLitigationTest

        #region DetailCannotBeClosedWithCurrentBalanceNotZeroTest

        /// <summary>
        /// restrictionsd for adding or updating loan details when loan is non-performing -
        /// prevents Litigation to close unless suspended interest has been zeroed
        /// </summary>
        [NUnit.Framework.Test]
        public void DetailCannotBeClosedWithCurrentBalanceNotZeroTest()
        {
            // PASS - CurrentAmount > 0.00
            DetailCannotBeClosedWithCurrentBalanceNotZeroHelper(0, (int)DetailTypes.LoanClosed, 0.00d);
            // PASS - DetailType not Loan Closed
            DetailCannotBeClosedWithCurrentBalanceNotZeroHelper(0, (int)DetailTypes.BankDetailsIncorrect, 1.00d);
            // FAIL - DetailType not Loan Closed
            DetailCannotBeClosedWithCurrentBalanceNotZeroHelper(1, (int)DetailTypes.LoanClosed, 1.00d);
        }

        /// <summary>
        /// Helper method to set up the expectations for the DetailCannotBeClosedWithCurrentBalanceNotZero test.
        /// </summary>
        /// <param name="gs"></param>
        private void DetailCannotBeClosedWithCurrentBalanceNotZeroHelper(int expectedMessageCount, int detailTypesVal, double currentBalance)
        {
            DetailCannotBeClosedWithCurrentBalanceNotZero rule = new DetailCannotBeClosedWithCurrentBalanceNotZero();

            IDetail detail = _mockery.StrictMock<IDetail>();
            IDetailType detailType = _mockery.StrictMock<IDetailType>();
            IAccount account = _mockery.StrictMock<IAccount>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            DataTable DT = new DataTable();
            string query = String.Empty;

            if (currentBalance == 0)
            {
                query = @"SELECT     TOP (1) fs.AccountKey,CONVERT(numeric(18, 3), SUM(bal.Amount)) AS Balance
                        FROM FinancialService AS fs
                        INNER JOIN Detail AS det ON det.AccountKey = fs.AccountKey
                        AND det.DetailTypeKey = " + detailTypesVal + @"
                        INNER JOIN fin.Balance bal ON fs.FinancialServiceKey = bal.FinancialServiceKey
                        AND fs.FinancialServiceKey = bal.FinancialServiceKey
                        GROUP BY fs.AccountKey
                        HAVING      (CONVERT(numeric(18, 3), SUM(bal.Amount)) < 0.01)
                        AND (CONVERT(numeric(18, 3), SUM(bal.Amount)) > - 0.01) ";
            }
            else
            {
                query = @"SELECT     TOP (1) fs.AccountKey,CONVERT(numeric(18, 3), SUM(bal.Amount)) AS Balance
                        FROM FinancialService AS fs
                        INNER JOIN Detail AS det ON det.AccountKey = fs.AccountKey
                        AND det.DetailTypeKey = " + detailTypesVal + @"
                        INNER JOIN fin.Balance bal ON fs.FinancialServiceKey = bal.FinancialServiceKey
                        AND fs.FinancialServiceKey = bal.FinancialServiceKey
                        GROUP BY fs.AccountKey
						HAVING	convert(numeric(18,3),sum(bal.Amount)) > 0.01";
            }
            Helper.FillFromQuery(DT, query, con, parameters);

            int applicationKey = Convert.ToInt32(DT.Rows[0][0]);

            SetupResult.For(account.Key).Return(applicationKey);
            SetupResult.For(detail.Account).Return(account);
            SetupResult.For(detailType.Key).Return(detailTypesVal);
            SetupResult.For(detail.DetailType).Return(detailType);

            ExecuteRule(rule, expectedMessageCount, detail);
        }

        #endregion DetailCannotBeClosedWithCurrentBalanceNotZeroTest
    }
}