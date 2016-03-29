using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetLatestApplicationInformationVariableLoanQueryStatement : IServiceQuerySqlStatement<GetLatestApplicationInformationVariableLoanQuery, 
                                                                             GetLatestApplicationInformationVariableLoanQueryResult>
    {
        public string GetStatement()
        {
            string query = @"select oi.OfferKey, oi.ProductKey, oi.OfferInformationTypeKey, oivl.* from [2AM].dbo.Offer o
                            outer apply (select max(offerInformationKey) as maxoi, OfferKey
                            from [2AM].dbo.OfferInformation oi where oi.OfferKey = o.OfferKey
                            group by OfferKey) as offerInfo
                            join [2AM].dbo.OfferInformation oi on offerInfo.maxoi = oi.OfferInformationKey
                            join [2AM].dbo.OfferInformationVariableLoan oivl on oi.offerInformationKey = oivl.offerInformationKey
                            where o.OfferKey = @ApplicationNumber";
            return query;
        }
    }
}