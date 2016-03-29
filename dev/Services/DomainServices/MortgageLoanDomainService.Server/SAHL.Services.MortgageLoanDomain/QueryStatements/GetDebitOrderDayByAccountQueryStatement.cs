using SAHL.Core.Services;
using SAHL.Services.Interfaces.MortgageLoanDomain.Model;
using SAHL.Services.Interfaces.MortgageLoanDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.MortgageLoanDomain.QueryStatements
{
    public class GetDebitOrderDayByAccountQueryStatement : IServiceQuerySqlStatement<GetDebitOrderDayByAccountQuery, GetDebitOrderDayByAccountQueryResult>
    {
        public string GetStatement()
        {
            var query = @"select 
                            fsb.DebitOrderDay
                        from
                            [2am].[dbo].[FinancialService] fs
                        join
                            [2am].[fin].[MortgageLoan] ml
                        on
                            fs.FinancialServiceKey = ml.FinancialServiceKey
                        join
                            [2am].[dbo].[MortgageLoanPurpose] mlp
                        on
                            ml.MortgageLoanPurposeKey = mlp.MortgageLoanPurposeKey
                        join
                            [2am].[dbo].[FinancialServiceBankAccount] fsb
                        on
                            fsb.FinancialServiceKey = fs.FinancialServiceKey
                        where
                            fs.AccountKey = @AccountKey
                        and 
                            mlp.MortgageLoanPurposeGroupKey = 1
                        and 
                            fs.FinancialServiceTypeKey = 1
                        and 
                            fsb.GeneralStatusKey = 1
                        order by
                            fsb.FinancialServiceBankAccountKey desc";
            return query;
        }
    }
}
