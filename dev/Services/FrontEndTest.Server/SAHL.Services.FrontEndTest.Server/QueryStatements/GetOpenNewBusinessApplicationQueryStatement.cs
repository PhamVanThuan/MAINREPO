using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetOpenNewBusinessApplicationQueryStatement : IServiceQuerySqlStatement<GetOpenNewBusinessApplicationQuery, GetOpenNewBusinessApplicationQueryResult>
    {
        public string GetStatement()
        {
            return @"select top 1 o.* from FETest.dbo.OpenNewBusinessApplications app
                    join [2am].dbo.Offer o on app.offerKey = o.offerKey
                    where HasDebitOrder = @HasDebitOrder
                    and HasMailingAddress = @HasMailingAddress
                    and HasProperty = @HasProperty
                    and IsAccepted = @IsAccepted
                    and HouseholdIncome > @HouseholdIncome
                    order by newid()";
        }
    }
}