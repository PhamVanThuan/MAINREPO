using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Correspondence;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceCorrespondences
{
    public class ThirdPartyInvoiceCorrespondencesChildDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<ThirdPartyInvoiceCorrespondencesChildTileConfiguration>
    {
        public ThirdPartyInvoiceCorrespondencesChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            var query = string.Format(@"SELECT TOP 1  {0} as BusinessKeyType, {1} As BusinessKey, {2} As Context
                                                FROM [EventProjection].[projection].CurrentStateForInstance cs
                                                     JOIN [EventProjection].[projection].[Correspondence] c on 
													 cs.GenericKey = c.GenericKey and 
                                                     cs.GenericKeyTypeKey = c.GenericKeyTypeKey
        	                                    WHERE
        		                                      cs.[GenericKeyTypeKey] = {0}
        		                                    AND
        		                                      cs.[GenericKey] = {1}
        											And 
        											  StateName <> 'Invoice Query'"
                                        , genericKeyType, businessContext.BusinessKey.Key, businessContext.Context);
            return query;
        }
    }
}