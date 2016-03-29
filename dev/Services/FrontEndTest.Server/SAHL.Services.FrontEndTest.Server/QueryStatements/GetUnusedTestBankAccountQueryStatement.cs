using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetUnusedTestBankAccountQueryStatement : IServiceQuerySqlStatement<GetUnusedTestBankAccountQuery, GetUnusedTestBankAccountQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 bad.* from [2am].test.BankAccountDetails bad
                    left join [2am].dbo.BankAccount ba on ba.AccountNumber = bad.AccountNumber
                    where ba.AccountNumber is null";
        }
    }
}