using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration.TileContent;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.ThirdPartyInvoiceLineItems
{
    public class ThirdPartyInvoiceLineItemsChildTileContentDataProvider : HaloTileBaseContentMultipleDataProvider<ThirdPartyInvoiceLineItemModel>
    {
        public ThirdPartyInvoiceLineItemsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"Select TOP 5 
		                                    Coalesce(INVLItemCat.InvoiceLineItemCategory,'') as LineItemType,
		                                    Coalesce(INVLItemDesc.InvoiceLineItemDescription, '') as LineItemDesc,
		                                    Coalesce(INVLItem.Amount,0) as LineItemAmount,
		                                    Coalesce(INVLItem.VATAmount, 0) as LineItemVatAmount,
		                                    Coalesce(INVLItem.TotalAmountIncludingVAT, 0) as LineItemTotalAmtInclVAT
                                    From [2AM].[dbo].[InvoiceLineItem] INVLItem
                                    Join [2AM].[dbo].[InvoiceLineItemDescription] INVLItemDesc
                                                on INVLItemDesc.InvoiceLineItemDescriptionKey = INVLItem.InvoiceLineItemDescriptionKey
                                    Join [2AM].[dbo].[InvoiceLineItemCategory] INVLItemCat
                                                on INVLItemDesc.InvoiceLineItemCategoryKey = INVLItemCat.InvoiceLineItemCategoryKey
                                    WHERE ThirdPartyInvoiceKey ={0} ", businessContext.BusinessKey.Key);
        }
    }
}