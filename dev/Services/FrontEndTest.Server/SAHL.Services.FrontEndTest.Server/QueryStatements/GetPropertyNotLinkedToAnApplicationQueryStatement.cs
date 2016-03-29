using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetPropertyNotLinkedToAnApplicationQueryStatement : IServiceQuerySqlStatement<GetPropertyNotLinkedToAnApplicationQuery, PropertyDataModel>
    {
        public string GetStatement()
        {
            string query = @"select p.* from [2am].dbo.Property p
                             left join [2am].dbo.OfferMortgageLoan oml on p.propertyKey=oml.PropertyKey
                                where oml.propertyKey is null and PropertyDescription1 is not null
                                and PropertyDescription2 is not null and PropertyDescription3 is not null
                                and ErfPortionNumber is not null and ErfNumber is not null and AddressKey is not null";
            return query;
        }
    }
}