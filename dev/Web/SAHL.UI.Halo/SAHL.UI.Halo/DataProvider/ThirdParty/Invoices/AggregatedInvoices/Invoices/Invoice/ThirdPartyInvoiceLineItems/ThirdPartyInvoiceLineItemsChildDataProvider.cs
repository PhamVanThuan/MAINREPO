using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceLineItem;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceLineItems
{
    public class ThirdPartyInvoiceLineItemsChildDataProvider : HaloTileBaseChildDataProvider,
                                         IHaloTileChildDataProvider<ThirdPartyInvoiceLineItemChildTileConfiguration>
    {
        public ThirdPartyInvoiceLineItemsChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            return string.Format(@"SELECT [ThirdPartyInvoiceKey] As BusinessKey, {0} As Context,  {1} as BusinessKeyType
                                      FROM [2AM].[dbo].[ThirdPartyInvoice]
                                      WHERE [ThirdPartyInvoiceKey] = {2}
                                      AND [TotalAmountIncludingVAT] > 0"
                , businessContext.Context, genericKeyType, businessContext.BusinessKey.Key);
        }
    }
}