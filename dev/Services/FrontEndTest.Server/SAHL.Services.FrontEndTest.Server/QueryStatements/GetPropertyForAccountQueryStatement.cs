using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetPropertyForAccountQueryStatement : IServiceQuerySqlStatement<GetPropertyForAccountQuery,PropertyDataModel>
    {
        public string GetStatement()
        {
            return @"select top 1 p.* from [2am].dbo.account a
                join [2am].dbo.financialservice fs on a.AccountKey = fs.AccountKey
                join [2am].fin.MortgageLoan ml on fs.FinancialServiceKey = ml.FinancialServiceKey
                join [2am].dbo.Property p on ml.PropertyKey = p.PropertyKey
                where a.AccountKey = @AccountKey";
        }
    }
}
