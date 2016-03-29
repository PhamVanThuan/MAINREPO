using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetApplicantsDetailsQueryStatement : IServiceQuerySqlStatement<GetApplicantsDetailsQuery, LegalEntityDataModel>
    {
        public string GetStatement()
        {
            return @"select le.*
                from [2am].dbo.Offer o
                    join [2am].dbo.OfferRole r on o.OfferKey = r.OfferKey
                    join [2am].dbo.OfferRoleType rt on r.OfferRoleTypeKey = rt.OfferRoleTypeKey and rt.OfferRoleTypeGroupKey = 3
                    join [2am].dbo.LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
                where o.offerkey = @ApplicationNumber";
        }
    }
}