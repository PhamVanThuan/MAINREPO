using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicationWithoutComcorpOfferPropertyDetailsQueryStatement : IServiceQuerySqlStatement<GetApplicationWithoutComcorpOfferPropertyDetailsQuery, OfferDataModel>
    {
        public string GetStatement()
        {
            return @"select top 1 o.*
                    from [2am].dbo.Offer o
                        left join [2am].dbo.ComcorpOfferPropertyDetails p on o.OfferKey = p.OfferKey
						where o.OfferStatusKey = 1
							and o.OfferTypeKey in (6,7,8)
							and p.OfferKey is null
                    order by NEWID()";
        }
    }
}