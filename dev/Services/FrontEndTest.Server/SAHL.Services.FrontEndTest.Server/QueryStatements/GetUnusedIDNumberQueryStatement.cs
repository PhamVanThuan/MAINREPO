using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetUnusedIDNumberQueryStatement :
        IServiceQuerySqlStatement<GetUnusedIDNumberQuery, GetUnusedIDNumberQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 id.IDNumber
                    from [2am].test.IDNumbers id
                        left join [2am].dbo.LegalEntity le on id.IDNumber = le.IDNumber
                    where le.LegalEntityKey is null
                    order by NEWID()";
        }
    }
}