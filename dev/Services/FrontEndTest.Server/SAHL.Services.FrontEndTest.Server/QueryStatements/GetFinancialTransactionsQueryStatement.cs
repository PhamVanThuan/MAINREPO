using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetFinancialTransactionsQueryStatement : IServiceQuerySqlStatement<GetFinancialTransactionsQuery, GetFinancialTransactionsQueryResult>
    {
        public string GetStatement()
        {
            return @"Select * from [2am].fin.FinancialTransaction Where FinancialServiceKey = @FinancialServiceKey";
        }
    }
}
