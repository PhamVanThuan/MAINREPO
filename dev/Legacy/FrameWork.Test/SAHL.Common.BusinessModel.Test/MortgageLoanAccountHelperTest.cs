using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Data;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class MortgageLoanAccountHelperTest : TestBase
    {
        [Test]
        public void CalcAccruedInterest()
        {
            int financialServiceKey = 0;
            int accKey = 0;

            try
            {
                using (new TransactionScope())
                {
                    string query = @"SELECT TOP 1 fs.FinancialServiceKey,fs.AccountKey,ft.FinancialTransactionKey
                                        FROM [2am].dbo.Account A (NOLOCK)
                                        JOIN [2am].dbo.FinancialService fs (NOLOCK) ON A.AccountKey = fs.AccountKey
                                        JOIN [2am].fin.FinancialTransaction ft ON ft.FinancialServiceKey = fs.FinancialServiceKey
                                        JOIN [2am].fin.Balance b (NOLOCK) ON fs.FinancialServiceKey = b.FinancialServiceKey
                                        WHERE fs.ParentFinancialServiceKey IS NULL
                                        AND b.BalanceTypeKey = 1
                                        AND fs.FinancialServiceTypeKey = 1
                                        AND A.RRR_ProductKey = 2
                                        AND fs.AccountStatusKey = 1
                                        AND A.AccountStatusKey = 1
                                        AND ft.TransactionTypeKey in (920, 940)
                                        AND ft.InsertDate > dateadd(dd, -DatePart(dd, GetDate()), GetDate())";
                    DataTable DT = base.GetQueryResults(query);
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        Assert.That(DT.Rows.Count == 1);
                        financialServiceKey = Convert.ToInt32(DT.Rows[0][0]);
                        accKey = Convert.ToInt32(DT.Rows[0][1]);
                        int financialTransactionKey = Convert.ToInt32(DT.Rows[0][2]);

                        DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                        IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                        AccountVariFixLoan vf = BMTM.GetMappedType<AccountVariFixLoan>(AccountVariFixLoan_DAO.Find(accKey) as AccountVariFixLoan_DAO);
                        double interestMonthToDate;
                        double interestEndofMonth;
                        vf.CalculateInterest(financialServiceKey, out interestMonthToDate, out interestEndofMonth);
                    }
                }
            }
            catch (Exception ex)
            {
                //this randomly fails on the build server, need to know exactly why it fails
                Assert.Fail(String.Format(@"Acc={0}, FSKey={1}, error={2}", accKey, financialServiceKey, ex.ToString()));
            }
        }

        [Test]
        public void CapitalisedLifePropertyCheck()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 pa.AccountKey FROM account PA (NOLOCK)
                                JOIN Account CA (NOLOCK) ON CA.ParentAccountKey = PA.AccountKey
                                JOIN dbo.FinancialService FS (NOLOCK) ON FS.AccountKey = CA.AccountKey
                                JOIN fin.Balance b (NOLOCK) on FS.FinancialServiceKey = b.FinancialServiceKey
                                WHERE CA.RRR_ProductKey = 4
                                AND CA.AccountStatusKey = 1
                                AND fs.FinancialServiceTypeKey = 5";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int accKey = Convert.ToInt32(DT.Rows[0][0]);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IMortgageLoanAccount mLA = BMTM.GetMappedType<IMortgageLoanAccount>(Account_DAO.Find(accKey) as Account_DAO);

                try
                {
                    double d = mLA.CapitalisedLife;
                    Assert.NotNull(d);
                }
                catch (Exception exc)
                {
                    Assert.Fail("Please Check that the Balance Data exists");
                }
            }
        }

        [Test]
        public void GetHOCAccount()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 PA.AccountKey, FSh.AccountKey FROM
                                [2AM].DBO.Account PA (NOLOCK)
                                JOIN [2AM].[dbo].[FinancialService] FSv (nolock) on FSv.AccountKey = PA.AccountKey AND FSv.FinancialServiceTypeKey = 1
                                JOIN [2AM].[dbo].[FinancialService] FSf (nolock) on FSf.AccountKey = PA.AccountKey AND FSf.FinancialServiceTypeKey = 2
                                    and FSf.AccountStatusKey = 1 --The FS must be active
                                JOIN [2AM].[dbo].Account CA (nolock) ON CA.ParentAccountKey = PA.AccountKey
                                JOIN [2AM].[dbo].[FinancialService] FSh (nolock) on FSh.AccountKey = CA.AccountKey AND FSh.FinancialServiceTypeKey = 4";
                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                int hocAccountKey = Convert.ToInt32(DT.Rows[0][1]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                AccountVariFixLoan vf = BMTM.GetMappedType<AccountVariFixLoan>(AccountVariFixLoan_DAO.Find(accountKey));

                Assert.IsNotNull(vf);
                Assert.IsNotNull(vf.HOCAccount, "HOCAccount is null for AccountVariFixLoan_DAO [{0}]", vf.Key);
                Assert.AreEqual(vf.HOCAccount.Key, hocAccountKey);
            }
        }

        [Test]
        public void TotalLoanInstallment()
        {
            using (new SessionScope())
            {
                string query = @"SELECT TOP 1 A.AccountKey FROM [2AM].[dbo].Account A (NOLOCK)
                                JOIN [2AM].[dbo].[FinancialService] FSv (nolock) on FSv.AccountKey = A.AccountKey AND FSv.FinancialServiceTypeKey = 1
                                JOIN [2AM].[dbo].[FinancialService] FSf (nolock) on FSf.AccountKey = A.AccountKey AND FSf.FinancialServiceTypeKey = 2
                                JOIN [2AM].[dbo].Account AR (nolock) ON AR.ParentAccountKey = A.AccountKey
                                JOIN [2AM].[dbo].[FinancialService] FSl (nolock) on FSl.AccountKey = AR.AccountKey AND FSl.FinancialServiceTypeKey = 5
                                WHERE A.RRR_ProductKey = 2";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                AccountVariFixLoan vf = BMTM.GetMappedType<AccountVariFixLoan>(AccountVariFixLoan_DAO.Find(accountKey) as AccountVariFixLoan_DAO);
                Assert.That(vf != null);
                IAccountInstallmentSummary x = vf.InstallmentSummary;
            }
        }

        [Test]
        public void LoanCurrentBalanceVarifFix()
        {
            using (new SessionScope())
            {
                // get the first open loan account with a variable and fixed leg
                string query = "SELECT TOP 1 A.AccountKey FROM [2AM].[dbo].[Account] A (nolock) "
                   + "JOIN [2AM].[dbo].[FinancialService] FSv (nolock) on FSv.AccountKey = A.AccountKey AND FSv.FinancialServiceTypeKey = 1  and FSv.AccountStatusKey = 1 "
                   + "JOIN [2AM].[dbo].[FinancialService] FSf (nolock) on FSf.AccountKey = A.AccountKey AND FSf.FinancialServiceTypeKey = 2  and FSf.AccountStatusKey = 1 "
                   + "WHERE A.AccountStatusKey = 1";

                DataTable DT = base.GetQueryResults(query);

                Assert.That(DT.Rows.Count == 1);

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                AccountVariFixLoan vf = BMTM.GetMappedType<AccountVariFixLoan>(AccountVariFixLoan_DAO.Find(DT.Rows[0][0]) as AccountVariFixLoan_DAO);
                Assert.That(vf != null);

                IMortgageLoanAccount mla = vf as IMortgageLoanAccount;

                Assert.That(mla.LoanCurrentBalance == vf.SecuredMortgageLoan.CurrentBalance + vf.FixedSecuredMortgageLoan.CurrentBalance);
            }
        }

        [Test]
        public void GetSecuredMortgageLoan()
        {
            string sql = @"select top 1 fs.AccountKey
                from [2AM].[fin].MortgageLoan ml
                inner join FinancialService fs on ml.FinancialServiceKey = fs.FinancialServiceKey
                inner join Account a on fs.AccountKey = a.AccountKey
                inner join MortgageLoanPurpose mlp on ml.MortgageLoanPurposeKey = mlp.MortgageLoanPurposeKey
                where mlp.MortgageLoanPurposeGroupKey = 1	-- MortgageLoan
                and fs.FinancialServiceTypeKey = 1			-- VariableLoan
                and a.RRR_ProductKey = 9                    -- NewVariable
                and fs.AccountStatusKey = 1
                ";
            DataTable dt = GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");
            int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                IAccountRepository rep = RepositoryFactory.GetRepository<IAccountRepository>();
                IAccountNewVariableLoan acc = (IAccountNewVariableLoan)rep.GetAccountByKey(accountKey);
                IMortgageLoan ml = acc.SecuredMortgageLoan;
                Assert.IsNotNull(ml);
                Assert.AreEqual(ml.AccountStatus.Key, (int)AccountStatuses.Open);
            }
        }

        [Test]
        public void GetAccountBalanceByDate()
        {
            DateTime dt = DateTime.Now.AddDays(-200);

            string sql = String.Format(@"select top 5 min(AccountKey) as AccountKey
                from Account a
                where a.AccountStatusKey in (1, 5)
                and a.OpenDate < '{0}'
                group by a.RRR_ProductKey", (dt.Year + "-" + dt.Month + "-" + dt.Day));

            DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    IMortgageLoanAccount mla = accRepo.GetAccountByKey(Convert.ToInt32(dr[0].ToString())) as IMortgageLoanAccount;

                    if (mla != null)
                        Assert.IsTrue(mla.GetAccountBalanceByDate(dt) > 0);
                }
            }
        }

        [Test]
        public void TotalShortTermLoanInstallment()
        {
            using (new SessionScope())
            {
                string sql = String.Format(@"SELECT     fs.AccountKey as AccountKey, fs.Payment as Payment
                                            FROM    fin.MortgageLoan AS ml INNER JOIN
                                            FinancialService AS fs ON ml.FinancialServiceKey = fs.FinancialServiceKey INNER JOIN
                                            MortgageLoanPurpose AS mlp ON ml.MortgageLoanPurposeKey = mlp.MortgageLoanPurposeKey INNER JOIN
                                            MortgageLoanPurposeGroup AS mlpg ON mlp.MortgageLoanPurposeGroupKey = mlpg.MortgageLoanPurposeGroupKey
                                            WHERE     (mlpg.MortgageLoanPurposeGroupKey = 2) AND (fs.AccountStatusKey IN (1, 5))");

                DataTable dt = GetQueryResults(sql);

                if (dt.Rows.Count == 0)
                {
                    Assert.Ignore("No data");
                    return;
                }
                int accountKey = Convert.ToInt32(dt.Rows[0]["AccountKey"]);
                double sti = Convert.ToInt32(dt.Rows[0]["Payment"]); ;
                dt.Dispose();
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                IMortgageLoanAccount mla = accRepo.GetAccountByKey(accountKey) as IMortgageLoanAccount;
                IAccountInstallmentSummary ais = new AccountInstallmentSummary(mla);
                if (ais != null)
                    sti = ais.TotalShortTermLoanInstallment;
            }
        }

        [Test]
        public void GetLatestBehaviouralScore()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();

                string query = @"WITH FinS_CTE (AccountKey, BondRegistrationDate)
                                AS
                                (
	                                select top 100 FS.AccountKey, Max(B.BondRegistrationDate) AS BondRegistrationDate
	                                FROM		Bond AS B WITH (nolock)
	                                INNER JOIN BondMortgageLoan AS BML WITH (nolock) ON B.BondKey = BML.BondKey
	                                INNER JOIN FinancialService FS (nolock) ON BML.FinancialServiceKey = FS.FinancialServiceKey
	                                WHERE     B.BondRegistrationDate IS NOT NULL
	                                GROUP BY FS.FinancialServiceKey, B.BondRegistrationDate, FS.AccountKey
                                )

                                --select * from FinS_CTE

                                SELECT top 1 FS.AccountKey, abII.BehaviouralScore,abII.AccountingDate
                                FROM		FinS_CTE FS
                                INNER JOIN AccountBaselII (nolock)  as abII ON FS.AccountKey = abII.AccountKey
                                GROUP BY FS.AccountKey, FS.BondRegistrationDate, abII.BehaviouralScore,abII.AccountingDate
                                ORDER BY FS.BondRegistrationDate DESC, abII.AccountingDate desc";

                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Rows.Count == 1, "No data found.");

                DateTime date = (DateTime)DT.Rows[0][2];
                Double BSScore = (Double)DT.Rows[0][1];

                int accountKey = Convert.ToInt32(DT.Rows[0][0]);
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                IMortgageLoanAccount mortgageLoanAccount = accRepo.GetAccountByKey(accountKey) as IMortgageLoanAccount;

                IAccountBaselII actBasel = mortgageLoanAccount.GetLatestBehaviouralScore();

                Assert.That(actBasel.AccountingDate.ToShortDateString() == date.ToShortDateString());
                Assert.That(actBasel.BehaviouralScore == BSScore);
            }
        }
    }
}