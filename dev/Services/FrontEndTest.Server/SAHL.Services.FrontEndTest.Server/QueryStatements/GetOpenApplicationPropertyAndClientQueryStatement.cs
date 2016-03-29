using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetOpenApplicationPropertyAndClientQueryStatement : IServiceQuerySqlStatement<GetOpenApplicationPropertyAndClientQuery, GetOpenApplicationPropertyAndClientQueryResult>
    {
        public string GetStatement()
        {
            var sql = @"select top 1 PropertyKey, IDNumber from [FeTest].dbo.OpenNewBusinessApplications app
                        join FETest.dbo.ActiveNewBusinessApplicants aa on app.OfferKey = aa.OfferKey
                        join FETest.dbo.NaturalPersonClient npc on aa.LegalEntityKey = npc.LegalEntityKey
                        where PropertyKey > 0 order by newid()";
            return sql;
        }
    }
}