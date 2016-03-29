using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetComcorpApplicationsWithMultipleApplicantsQueryStatement : IServiceQuerySqlStatement<GetComcorpApplicationsWithMultipleApplicantsQuery,
                                                                              GetComcorpApplicationsWithMultipleApplicantsQueryResult>
    {
        public string GetStatement()
        {
            return @"select o.OfferKey as ApplicationNumber, o.Reference
                    from [2am].dbo.Offer o
                        join [2am].dbo.OfferAttribute oa on o.OfferKey = oa.OfferKey 
                            and oa.OfferAttributeTypeKey = 31
                        join [2am].dbo.OfferRole r on o.OfferKey = r.OfferKey
                        join [2am].dbo.OfferRoleType rt on r.OfferRoleTypeKey = rt.OfferRoleTypeKey and rt.OfferRoleTypeGroupKey = 3
                    where o.OfferStatusKey = 1
                        and o.OfferTypeKey in (6,7,8)
                        and o.Reference is not null
                    group by o.OfferKey, Reference
                    having count(r.LegalEntityKey) > 1";
        }
    }
}