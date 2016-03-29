using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetRandomOpenApplicationQueryStatement :
        IServiceQuerySqlStatement<GetRandomOpenApplicationQuery, GetRandomOpenApplicationQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 o.OfferKey as ApplicationNumber
                    from [FeTest].dbo.OpenNewBusinessApplications app 
                    join [2am].dbo.Offer o on app.offerKey = o.offerKey
                    where PropertyKey is not null
                    order by newid()";
        }
    }
}