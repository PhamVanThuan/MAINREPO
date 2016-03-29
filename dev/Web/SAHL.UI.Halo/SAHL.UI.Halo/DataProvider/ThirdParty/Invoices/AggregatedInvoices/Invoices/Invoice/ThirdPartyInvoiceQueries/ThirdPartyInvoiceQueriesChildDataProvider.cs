using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceQueries
{
    public class ThirdPartyInvoiceQueriesChildDataProvider: HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<ThirdPartyInvoiceQueriesChildTileConfiguration>
    {
        public ThirdPartyInvoiceQueriesChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            var query = string.Format(
                                    @"SELECT top 1 c.[GenericKey] As BusinessKey, 
                                        cs.[StateName] As Context, c.[GenericKeyTypeKey] as [BusinessKeyType]
                                      FROM
                                        [EventProjection].[projection].[CurrentStateForInstance] cs
                                        JOIN [EventProjection].[projection].[Correspondence] c
                                             on c.GenericKey = cs.GenericKey and
                                                c.GenericKeyTypeKey = cs.GenericKeyTypeKey
                                      WHERE
                                        c.GenericKey = {0} AND c.GenericKeyTypeKey = {1}
                                        AND
                                        cs.StateName = 'Invoice Query'"
                , businessContext.BusinessKey.Key, genericKeyType);
            
            return query;
        }
    }
}
