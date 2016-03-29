using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetAlphaHousingApplicationQueryStatement : IServiceQuerySqlStatement<GetAlphaHousingApplicationQuery, GetAlphaHousingApplicationQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select top 1 o.* from FeTest.dbo.AlphaHousingApplications alpha
                                join FeTest.dbo.OpenNewBusinessApplications apps on alpha.OfferKey = apps.OfferKey
                                    and apps.IsAccepted = @IsAccepted
                                join [2am].dbo.Offer o on alpha.OfferKey = o.OfferKey
                                where
                                --Conditional spv check: If less than 0 then return any spv else return specified spv 
                                'true' = case when @SPVKey < 0 then 'true' else case when SPVKey = @SPVKey then 'true' else 'false' end end
                                and ltv > @LTV
                                order by newid()";
            return query;
        }
    }
}