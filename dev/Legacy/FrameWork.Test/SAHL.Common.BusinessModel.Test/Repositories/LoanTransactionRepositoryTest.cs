using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.DataSets;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    /// <summary>
    /// Due to the complexity of certain of the methods
    /// just the executing of these are checked to make sure they dont fall over.
    /// The actual functionality of the SQL, SP...etc is not being tested.
    /// </summary>
    [TestFixture]
    public class LoanTransactionRepositoryTest : TestBase
    {
        private ILoanTransactionRepository _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();

        [Test]
        public void GenerateAmortiseDataTest()
        {
            using (new SessionScope())
            {
                LoanCalculations.AmortisationScheduleDataTable dt = _ltRepo.GenerateAmortiseData(234000, 3921, 0.073);
                Assert.Greater(dt.Rows.Count, 0);
            }
        }

        [Test]
        public void GetCATSDisbursementLoanTransactions()
        {
            using (new SessionScope())
            {
                int fsKey = 10074099;
                int ttNumber = 160;
                DateTime insertDate = new DateTime(2009, 4, 14, 10, 55, 19, 720);
                DateTime effDate = new DateTime(2009, 4, 14);
                string reference = "absa.338160356";
                string userID = @"SAHL\DuncanNg";

                IList<IFinancialTransaction> ltList = _ltRepo.GetCATSDisbursementLoanTransactions(fsKey, ttNumber, insertDate, effDate, reference, userID);
            }
        }

        /// <summary>
        /// Calling reverse sql to generated data for the test.
        /// </summary>
        [Test]
        public void GetAdvancesDisbursedThisYearByAccountKeyTest()
        {
            using (new SessionScope())
            {
                string testQuery = @"
                DECLARE @NewFromDate datetime;

                SET @NewFromDate = (CONVERT(CHAR(4), getdate(), 121) + '-01-01 00:00:00.000');

                SELECT TOP 1 FS.AccountKey
                FROM
                   [2AM].[fin].[FinancialTransaction] FT WITH (NOLOCK)
                JOIN
                   [2AM].[fin].[TransactionType] TT WITH (NOLOCK)
                ON
                   FT.TransactionTypeKey = TT.TransactionTypeKey
                JOIN
                   [2AM].[dbo].[FinancialService] FS WITH (NOLOCK)
                ON
                    FT.FinancialServiceKey = FS.FinancialServiceKey
                WHERE
                   FS.AccountStatusKey IN (1,5)
                AND
                   FT.TransactionTypeKey IN (140)
                AND
	                FT.IsRolledBack IS NULL
                AND
                   FT.InsertDate >= @NewFromDate order by FT.InsertDate desc;";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testQuery, typeof(FinancialService_DAO), new ParameterCollection());
                if (o != null)
                {
                    int AccountKey = Convert.ToInt32(o);
                    _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                    int validate = _ltRepo.GetAdvancesDisbursedThisYearByAccountKey(AccountKey);

                    if (validate > 0)
                        return;

                    Assert.Fail();
                }
            }
        }

        /// <summary>
        /// using test setup as per method [GetTransactionsAndTypes] in base presenter
        /// [SAHL.Web.Views.Common.Presenters.Transactions.Base] which is called from inheriting presenter
        /// [SAHL.Web.Views.Common.Presenters.Transactions.View].
        /// Not testing the SQL just testing that the call does not fail.
        /// Should this fail follow lead from presenter first.
        /// </summary>
        [Test]
        public void GetTransactionsTest()
        {
            int AccountKey = -1;
            object res = null;
            List<SqlParameter> Parameters = null;
            string StatementName = "GetFinancialTransactionsByAccountKey";
            string userGroups = string.Empty;
            int FetchRows = 1;

            using (new SessionScope())
            {
                string testAccQuery = @" SELECT TOP 1 FS.AccountKey
                FROM
                   [2AM].[fin].FinancialTransaction LT WITH (NOLOCK)
                JOIN
                   [2AM].[fin].[TransactionType] TT WITH (NOLOCK)
                ON
                   LT.TransactionTypeKey = TT.TransactionTypeKey
                JOIN
                   [2AM].[dbo].[FinancialService] FS WITH (NOLOCK)
                ON
                   LT.FinancialServiceKey = FS.FinancialServiceKey
                WHERE
                   FS.AccountStatusKey IN (1,5)
                AND
	                LT.IsRolledBack = 0 ";

                // Test Data
                res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testAccQuery, typeof(FinancialService_DAO), new ParameterCollection());

                if (res != null)
                {
                    AccountKey = Convert.ToInt32(res);

                    if (res != null)
                    {
                        Parameters = new List<SqlParameter>();
                        Parameters.Add(new SqlParameter("@AccountKey", AccountKey));

                        //
                        _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                        userGroups = (this.CurrentPrincipalCache.GetCachedRolesAsStringForQuery(true, true, false));

                        //
                        Parameters.Add(new SqlParameter("@Roles", userGroups));

                        DataTable trans = _ltRepo.GetTransactions(StatementName, "COMMON", Parameters, FetchRows);
                    }
                    else
                        Assert.Fail("No test data retrieved hence method has not been tested.");
                }
                else
                    Assert.Fail("No test data retrieved hence method has not been tested.");
            }
        }

        /// <summary>
        /// using test setup as per method [_view_OnPostButtonClicked] in presenter
        /// [SAHL.Web.Views.Common.Presenters.PostTransaction].
        /// Not testing the SQL just testing that the call does not fail.
        /// Should this fail follow lead from presenter first.
        /// </summary>
        [Test]
        public void PostTransactionTest()
        {
            string adUserName = string.Empty;
            int TransactionTypeNumber = -1;
            int AccountKey = -1;
            DateTime TransactionEffectiveDate = DateTime.Now;
            double TransactionAmount = 1000.00;
            string TransactionReference = "test";
            object res = null;

            using (new SessionScope())
            {
                // using method [SAHL.Common.BusinessModel.Repositories.BulkBatchRepository.GetLoanTransactionTypes]
                // to generate reverse sql to find an applicable aduser
                string testADUserQuery = @" select top 1 ttda.ADCredentials
                from [2am].[fin].TransactionType tt
                join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey
                join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
                join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey
                join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey
                where tte.TransactionEffectkey < 3 -- Dr & Cr
                and tte.BalanceTypeKey = 1 -- Loan
                and tte.ParentTransactionTypeBalanceEffectKey is null
                and tt1.ScreenBatch = 1
                and ttg.TransactionGroupKey = 1 -- Financial
                and ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3) -- Correction
                and ttda.ADCredentials like 'SAHL\%' ";

                res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testADUserQuery, typeof(FinancialService_DAO), new ParameterCollection());

                if (res != null)
                {
                    adUserName = Convert.ToString(res);

                    // using method [SAHL.Common.BusinessModel.Repositories.BulkBatchRepository.GetLoanTransactionTypes]
                    // to generate reverse sql to find an applicable TransactionType
                    string testTranTypeQuery = string.Format(@"select top 1 tt.TransactionTypeKey
                        from [2am].[fin].TransactionType tt
                        join [2am].[dbo].[TransactionTypeDataAccess] ttda (nolock) ON tt.TransactionTypeKey = ttda.TransactionTypeKey
                        join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
                        join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                        join [2am].dbo.TransactionTypeUI tt1 (nolock) on tt.TransactionTypeKey = tt1.TransactionTypeKey
                        join [2am].[fin].TransactionTypeGroup ttg (nolock) on tt.TransactionTypekey = ttg.TransactionTypekey
                        where tte.TransactionEffectkey < 3 -- Dr & Cr
                        and tte.BalanceTypeKey = 1 -- Loan
                        and tte.ParentTransactionTypeBalanceEffectKey is null
                        and tt1.ScreenBatch = 1
                        and ttg.TransactionGroupKey = 1 -- Financial
                        and ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3) -- Correction
	                    and tt.TransactionTypeKey not in (-1,236,966,967)
                        and ttda.ADCredentials  = '{0}'", adUserName); ;

                    res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testTranTypeQuery, typeof(FinancialService_DAO), new ParameterCollection());

                    // Find an Applicable Account against which a Transaction can be posted
                    if (res != null)
                    {
                        TransactionTypeNumber = Convert.ToInt32(res);

                        string testAccQuery = @"SELECT TOP 1 FS.AccountKey
                        FROM
                           [2AM].[fin].[FinancialTransaction] FT WITH (NOLOCK)
                        JOIN
                           [2AM].[fin].[TransactionType] TT WITH (NOLOCK)
                        ON
                           FT.TransactionTypeKey = TT.TransactionTypeKey
                        JOIN
                           [2AM].[dbo].[FinancialService] FS WITH (NOLOCK)
                        ON
                            FT.FinancialServiceKey = FS.FinancialServiceKey
                        WHERE
                           FS.AccountStatusKey IN (1,5)
                        AND
	                        FT.IsRolledBack = 0";

                        res = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(testAccQuery, typeof(FinancialService_DAO), new ParameterCollection());
                        if (res != null)
                        {
                            AccountKey = Convert.ToInt32(res);

                            // Perform actual test
                            TransactionScope tx = new TransactionScope();
                            try
                            {
                                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                                _ltRepo.PostTransactionByAccountKey(AccountKey, (short)TransactionTypeNumber, TransactionEffectiveDate, TransactionAmount, TransactionReference, adUserName);
                            }
                            catch (DomainValidationException)
                            {
                                // bypassing rules running for the transaction types (-1,236,966,967) these are excluded from the test query
                                // Rule has fired, which means the test has executed without exception but the business rule is preventing it from posting.
                            }
                            catch (Exception e)
                            {
                                Assert.Fail(e.Message);
                            }
                            finally
                            {
                                tx.VoteRollBack();
                                tx.Dispose();
                            }
                        }
                        else
                            Assert.Fail("No test data retrieved hence method has not been tested.");
                    }
                    else
                        Assert.Fail("No test data retrieved hence method has not been tested.");
                }
                else
                    Assert.Fail("No test data retrieved hence method has not been tested.");
            }
        }

        [Test]
        public void FindLoanProcessTran()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 D.AccountKey, FT.TransactionTypeKey, FT.Reference, FT.UserID
                FROM [2AM].[dbo].[Disbursement] D (NOLOCK)
                Join [2AM].[dbo].[DisbursementTransactionType] DTT (NOLOCK) ON DTT.DisbursementTransactionTypeKey = D.DisbursementTransactionTypeKey
                Join [2AM].[fin].[TransactionType] TT (nolock) ON TT.TransactionTypeKey = DTT.TransactionTypeNumber
                Join [2AM].[fin].[FinancialTransaction] FT (nolock) ON FT.TransactionTypeKey = DTT.TransactionTypeNumber
                Join [2AM].[fin].[TransactionTypeGroup] TTG (nolock) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                Join [2AM].[dbo].[TransactionTypeDataAccess] TTDA (nolock) ON TTDA.TransactionTypeKey = TT.TransactionTypeKey
                WHERE TTG.TransactionGroupKey = 1
                AND ttg.TransactionTypeKey not in (Select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3);";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                int typeKey = Convert.ToInt32(DT.Rows[0][1]);
                string reference = Convert.ToString(DT.Rows[0][2]);
                string userid = Convert.ToString(DT.Rows[0][3]);

                SAHLPrincipal principal = base.TestPrincipal;

                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                DataTable resultDT = _ltRepo.FindLoanProcessTran(accountKey, typeKey, reference, principal);
            }
        }

        //TODO: BackEnd Revamp - http://sahls31:8181/trac/SAHL.db/ticket/18881

        [Test]
        public void pLoanProcessRollbackTran()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = @"
                        with RBT_CTE (TransactionTypeKey) AS
                                    (
                                            SELECT  DISTINCT TT.TransactionTypeKey
                                            FROM [2am].[fin].[TransactionType] TT (nolock)
                                            join [2am].[fin].TransactionTypeBalanceEffect tte (nolock) on tt.TransactionTypekey = tte.TransactionTypekey
									        join [2am].[fin].TransactionEffect tte1 (nolock) on tte.TransactionEffectkey = tte1.TransactionEffectkey
                                            join [2am].[fin].[TransactionTypeGroup] TTG (nolock) ON TT.TransactionTypeKey = TTG.TransactionTypeKey
                                            join [2am].[dbo].[TransactionTypeDataAccess] TTDA (nolock) ON TTDA.TransactionTypeKey = TT.TransactionTypeKey
                                            join [2am].[dbo].TransactionTypeUI TT1 (nolock) on TT.TransactionTypeKey = TT1.TransactionTypeKey
                                            WHERE tte.TransactionEffectkey < 3 -- Dr & Cr
                                            AND TT1.ScreenBatch = 1
                                            AND TTG.TransactionGroupKey = 1
                                            AND TT.TransactionTypeKey not in (select TransactionTypeKey from [2am].fin.TransactionTypeGroup where TransactionGroupKey = 3)
                                    )

                            SELECT TOP 1 FT.FinancialTransactionKey
                                FROM [2am].[fin].[FinancialTransaction] FT (nolock)
                                join [2am].dbo.FinancialService fs (nolock) on fs.FinancialServiceKey = FT.FinancialServiceKey
									and fs.AccountStatusKey = 1
                                WHERE (FT.IsRolledBack = 'false' OR FT.IsRolledBack IS NULL)
                                and FT.TransactionTypeKey in
                                (
									select TransactionTypeKey from RBT_CTE
                                )
                                ORDER BY FT.FinancialTransactionKey DESC

                ";

                DataTable DT = base.GetQueryResults(query);

                int loanTranNumber = Convert.ToInt32(DT.Rows[0][0]);
                SAHLPrincipal principal = base.TestPrincipal;

                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                _ltRepo.pLoanProcessRollbackTran(loanTranNumber, base.TestPrincipal);
            }
        }

        [Test]
        public void GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey()
        {
            using (new SessionScope())
            {
                string query = @"select top 1 FinancialServiceKey from FinancialService";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iFinancialServiceKey = Convert.ToInt32(DT.Rows[0][0]);

                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                IFinancialTransaction LoanTrans = _ltRepo.GetLastLoanTransactionByTransactionTypeAndFinancialServiceKey(SAHL.Common.Globals.TransactionTypes.ManualDebitOrderPayment, iFinancialServiceKey, true);
            }
        }

        [Test]
        public void PostTransactionByAccountKey()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = @"select top 1  accountkey  from account where accountstatuskey = 1 and parentaccountkey is null and rrr_productkey = 2 order by 1 desc";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int iAccountKey = Convert.ToInt32(DT.Rows[0][0]);

                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                _ltRepo.PostTransactionByAccountKey(iAccountKey, 967, DateTime.Now, 100, "Test", "Test");
            }
        }

        [Test]
        public void PostTransactionByFinancialServiceKey()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string query = @"SELECT     TOP (1) FinancialService.FinancialServiceKey
                                    FROM         Account INNER JOIN
                                    FinancialService ON Account.AccountKey = FinancialService.AccountKey
                                    WHERE     (Account.AccountStatusKey = 1) AND (Account.ParentAccountKey IS NULL) AND (Account.RRR_ProductKey = 1)
                                    ORDER BY Account.AccountKey DESC";
                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                    Assert.Ignore("No data available");

                int ifinKey = Convert.ToInt32(DT.Rows[0][0]);

                _ltRepo = RepositoryFactory.GetRepository<ILoanTransactionRepository>();
                _ltRepo.PostTransactionByFinancialServiceKey(ifinKey, 967, DateTime.Now, 100, "Test", "Test");
            }
        }
    }
}