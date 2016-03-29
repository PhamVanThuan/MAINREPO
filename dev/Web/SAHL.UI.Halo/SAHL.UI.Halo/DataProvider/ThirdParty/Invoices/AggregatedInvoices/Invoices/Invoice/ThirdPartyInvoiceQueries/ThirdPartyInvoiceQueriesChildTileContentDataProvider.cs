using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries
{
    public class ThirdPartyInvoiceQueriesChildTileContentDataProvider : HaloTileBaseContentDataProvider<ThirdPartyInvoiceQueryChildModel>
    {
        public ThirdPartyInvoiceQueriesChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            var query = string.Format(@"SELECT 
		                                    COUNT(*)[TotalInvoiceQueries]
	                                    FROM 
		                                    [EventProjection].[projection].[Correspondence]
	                                    WHERE
		                                    [GenericKeyTypeKey] = {0}
		                                    AND
		                                    [GenericKey] = {1}"
                                        , (int)GenericKeyType.ThirdPartyInvoice, businessContext.BusinessKey.Key);

            return query;
        }
    }
}
