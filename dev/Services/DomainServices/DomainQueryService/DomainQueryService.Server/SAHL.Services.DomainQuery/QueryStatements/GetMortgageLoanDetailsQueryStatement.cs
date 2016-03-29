using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.DomainQuery.QueryStatements
{
    public class GetMortgageLoanDetailsQueryStatement : IServiceQuerySqlStatement<GetMortgageLoanDetailsQuery, GetMortgageLoanDetailsQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select
	                        fs.AccountKey,
	                        fs.FinancialServiceTypeKey,
	                        fs.AccountStatusKey,
	                        fs.OpenDate,
	                        fs.CloseDate,
	                        ml.CreditMatrixKey,
	                        ml.PropertyKey,
	                        ml.MortgageLoanPurposeKey,
	                        lb.Term,
	                        lb.InitialBalance,
	                        lb.RemainingInstalments,
	                        lb.InterestRate,
	                        lb.RateConfigurationKey,
	                        lb.ResetConfigurationKey,
	                        lb.RateAdjustment,
	                        lb.ActiveMarketRate,
	                        lb.MTDInterest
                        from
	                        [2am].[dbo].FinancialService fs 
                        join
	                        [2am].[fin].MortgageLoan ml on ml.FinancialServiceKey = fs.FinancialServiceKey
                        join
	                        [2am].[fin].LoanBalance lb on lb.FinancialServiceKey = fs.FinancialServiceKey
                        where
	                        fs.AccountKey = @AccountKey";
            return query;
        }
    }
}