using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.Test.Helpers
{
    [TestFixture]
    internal class LoanCalculatorHelperTest : TestBase
    {
        IAccountRepository _accRepo;
        IMortgageLoanRepository _mortLoanRepo;

        public LoanCalculatorHelperTest()
        {
            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _mortLoanRepo = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
        }

        [Test, Sequential]
        public void CalculateNewProductInstallmentAtEndOfPeriodWithBothLifeAndHOCTest([Values(
                                                              Products.NewVariableLoan, Products.VariFixLoan
                                                              )] Products product)
        {
            using (new SessionScope())
            {
                int accountKey = GetAccountWithBothLifeAndHOC((int)product);
                IMortgageLoan mortLoan = _mortLoanRepo.GetMortgageloanByAccountKey(accountKey);
                double capBalance = 0;
                if (mortLoan.CAP != null)
                {
                    capBalance = mortLoan.CAP.CAPBalance;
                }
                double installment = LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(mortLoan.InterestRate, mortLoan.ActiveMarketRate, mortLoan.ActiveMarketRate, mortLoan, capBalance, mortLoan.RemainingInstallments, 12);
                Assert.Greater(installment, 0, string.Format(@"Failing using accountkey {0} and instalment {1}", accountKey, installment));
            }
        }

        [Test]
        public void CalculateNewProductInstallmentAtEndOfPeriodUsingAccountWithLifeTest()
        {
            using (new SessionScope())
            {
                int accountKey = GetAccountWithLife();
                IMortgageLoan mortLoan = _mortLoanRepo.GetMortgageloanByAccountKey(accountKey);
                double capBalance = 0;
                if (mortLoan.CAP != null)
                {
                    capBalance = mortLoan.CAP.CAPBalance;
                }
                double installment = LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(mortLoan.InterestRate, mortLoan.ActiveMarketRate, mortLoan.ActiveMarketRate, mortLoan, capBalance, mortLoan.RemainingInstallments, 12);
                Assert.Greater(installment, 0, string.Format(@"Failing using accountkey {0} and instalment {1}", accountKey, installment));
            }
        }

        [Test]
        public void CalculateNewProductInstallmentAtEndOfPeriodWithHOCTest()
        {
            using (new SessionScope())
            {
                int accountKey = GetAccountWithHOC();
                IMortgageLoan mortLoan = _mortLoanRepo.GetMortgageloanByAccountKey(accountKey);
                double capBalance = 0;
                if (mortLoan.CAP != null)
                {
                    capBalance = mortLoan.CAP.CAPBalance;
                }
                double installment = LoanCalculator.CalculateNewProductInstallmentAtEndOfPeriod(mortLoan.InterestRate, mortLoan.ActiveMarketRate, mortLoan.ActiveMarketRate, mortLoan, capBalance, mortLoan.RemainingInstallments, 12);
               
                Assert.Greater(installment, 0, string.Format(@"Failing using accountkey {0} and instalment {1}", accountKey, installment));
            }
        }

        #region helper methods

        private int GetAccountWithBothLifeAndHOC(int mortgageLoanProductKey)
        {
            string query = string.Format(@"SELECT top 1 a.accountKey 
                    FROM [2am].dbo.Account a (nolock)
                    JOIN [2am].dbo.FinancialService fs on a.accountKey=fs.accountKey
                        and fs.parentFinancialServiceKey is null                     
                    JOIN [2am].dbo.Account hca (nolock)
                    ON a.AccountKey = hca.ParentAccountKey 
	                    and hca.RRR_ProductKey = 3
                    Join [2am].dbo.Account lca (nolock)
                    ON a.AccountKey = lca.ParentAccountKey 
	                    and lca.RRR_ProductKey = 4
                    WHERE hca.AccountStatusKey = 1
                    AND lca.AccountStatusKey = 1
                    and a.rrr_productKey= {0} and a.accountStatusKey=1 and fs.payment > 0 and fs.FinancialServiceTypeKey = 1
                    order by newid()", mortgageLoanProductKey);

            return Convert.ToInt32(ExecuteScalar(query));
        }

        private int GetAccountWithLife()
        {
            string query = @"SELECT top 1 a.AccountKey 
                            FROM [2am].dbo.Account a (nolock)
                            join [2am].dbo.financialService fs (nolock) on fs.AccountKey = a.AccountKey
	                            and fs.FinancialServiceTypeKey = 1
                            join [2am].[fin].[Balance] b (nolock) on b.FinancialServiceKey = fs.FinancialServiceKey
	                            and b.BalanceTypeKey = 1
	                            and b.[Amount] > 100
                            Join [2am].dbo.Account lca (nolock) ON a.AccountKey = lca.ParentAccountKey 
	                            and lca.RRR_ProductKey = 4
                            JOIN [2am].dbo.FinancialService lfs (nolock) on lfs.AccountKey = lca.AccountKey
                            WHERE lca.AccountStatusKey = 1";

            return Convert.ToInt32(ExecuteScalar(query));
        }

        private int GetAccountWithHOC()
        {
            string query = @"SELECT top 1 a.loannumber FROM [2am].dbo.vOpenLoanAccounts a (nolock)
                            join [2am].dbo.financialService fs (nolock)
                            on fs.AccountKey = a.loannumber
                            join fin.Balance b
                            on b.FinancialServiceKey= fs.FinancialServiceKey
                            and b.BalanceTypeKey = 1
                            JOIN [2am].dbo.Account hca (nolock)
                            ON a.loannumber = hca.ParentAccountKey and hca.RRR_ProductKey = 3
                            JOIN [2am].dbo.FinancialService hfs (nolock)
                            on hfs.AccountKey = hca.AccountKey
                            WHERE hca.AccountStatusKey = 1
                            and b.Amount > 100";

            return Convert.ToInt32(ExecuteScalar(query));
        }

        private object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, 40);
        }

        private object ExecuteScalar(string query, int timeout)
        {
            object obj = null;
            using (DBHelper dbHelper = new DBHelper(Databases.TwoAM))
            {
                using (IDbCommand cmd = dbHelper.CreateCommand(query))
                {
                    cmd.CommandTimeout = timeout;
                    obj = dbHelper.ExecuteScalar(cmd);
                }
            }

            return obj;
        }

        #endregion helper methods
    }
}