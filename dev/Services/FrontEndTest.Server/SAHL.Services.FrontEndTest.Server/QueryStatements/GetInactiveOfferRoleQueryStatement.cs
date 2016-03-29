using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetInactiveOfferRoleQueryStatement : IServiceQuerySqlStatement<GetInactiveOfferRoleQuery, int>
    {
        public string GetStatement()
        {
            return @"select top 1 OfferRoleKey from [2am].dbo.OfferRole where GeneralStatusKey = 2
                    order by newid()";
        }
    }
}