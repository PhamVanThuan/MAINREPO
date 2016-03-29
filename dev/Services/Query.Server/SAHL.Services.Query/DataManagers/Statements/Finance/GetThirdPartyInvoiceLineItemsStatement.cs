using SAHL.Core.Data;
using SAHL.Services.Query.Models.Finance;

namespace SAHL.Services.Query.DataManagers.Statements.Finance
{
    public class GetThirdPartyInvoiceLineItemsStatement : ISqlStatement<ThirdPartyInvoiceLineItemsDataModel>
    {
        public string GetStatement()
        {
            return @"Select
	                	INVLItem.InvoiceLineItemKey as Id,
	                    TPI.ThirdPartyInvoiceKey As InvoiceId,
	                    TP.Id As ThirdPartyId,		       
	                    INVStatus.Description As InvoiceStatus,	
	                    Coalesce(INVLItemCat.InvoiceLineItemCategory,'') as LineItemType,
	                    Coalesce(INVLItemDesc.InvoiceLineItemDescription, '') as LineItemDesc,
	                    INVLItemDesc.InvoiceLineItemDescriptionKey as InvoiceLineItemDescriptionKey,
	                    INVLItemCat.InvoiceLineItemCategoryKey as InvoiceLineItemCategoryKey,
	                    Coalesce(INVLItem.Amount,0) as LineItemAmount,
	                    INVLItem.IsVATItem as IsVatable,
	                    Coalesce(INVLItem.VATAmount, 0) as LineItemVatAmount,
	                    Coalesce(INVLItem.TotalAmountIncludingVAT, 0) as LineItemTotalAmtInclVAT,
                        TPI.CapitaliseInvoice
                From [2AM].[dbo].[ThirdParty] TP
                Join [2AM].[dbo].[ThirdPartyInvoice] TPI
	                On TP.Id = TPI.ThirdPartyId
                Join [2AM].[dbo].[InvoiceStatus] As INVStatus
	                on	INVStatus.InvoiceStatusKey = TPI.InvoiceStatusKey
                Join [2AM].[dbo].[InvoiceLineItem] INVLItem 
	                on INVLItem.ThirdPartyInvoiceKey = TPI.ThirdPartyInvoiceKey
                Join [2AM].[dbo].[InvoiceLineItemDescription] INVLItemDesc 
	                on INVLItemDesc.InvoiceLineItemDescriptionKey = INVLItem.InvoiceLineItemDescriptionKey
                Join [2AM].[dbo].[InvoiceLineItemCategory] INVLItemCat
	                on INVLItemDesc.InvoiceLineItemCategoryKey = INVLItemCat.InvoiceLineItemCategoryKey";
        }
    }
}
