using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetClientsForAccountQueryStatement : IServiceQuerySqlStatement<GetClientsForAccountQuery, GetClientsForAccountQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 a.accountKey as AccountNumber, le.IdNumber from [2am].dbo.Account a 
                    join [2am].dbo.Role r on a.accountKey=r.accountKey
                        and r.GeneralStatusKey = 1
                    join [2am].dbo.LegalEntity le on r.LegalEntityKey=le.LegalEntityKey
                    where a.rrr_productKey = 9
                    and len(le.idnumber) = 13
                    order by newid()";
        }
    }
}