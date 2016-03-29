using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class AmendInvoiceLineItemStatement : ISqlStatement<InvoiceLineItemDataModel>
    {
        public int? InvoiceLineItemKey { get; protected set; }

        public int InvoiceLineItemDescriptionKey { get; protected set; }

        public decimal Amount { get; protected set; }

        public bool IsVATItem { get; protected set; }

        public decimal VATAmount { get; protected set; }

        public decimal TotalAmountIncludingVAT { get; protected set; }

        public AmendInvoiceLineItemStatement(InvoiceLineItemModel invoiceLineItemModel)
        {
            this.InvoiceLineItemKey = invoiceLineItemModel.InvoiceLineItemKey;
            this.InvoiceLineItemDescriptionKey = invoiceLineItemModel.InvoiceLineItemDescriptionKey;
            this.Amount = invoiceLineItemModel.AmountExcludingVAT;
            this.IsVATItem = invoiceLineItemModel.IsVATItem;
            this.VATAmount = invoiceLineItemModel.VATAmount;
            this.TotalAmountIncludingVAT = invoiceLineItemModel.TotalAmountIncludingVAT;
        }

        public string GetStatement()
        {
            var sql = @"UPDATE [2AM].[dbo].[InvoiceLineItem]
                       SET
                           [InvoiceLineItemDescriptionKey] = @InvoiceLineItemDescriptionKey
                          ,[Amount] = @Amount
                          ,[IsVATItem] = @IsVATItem
                          ,[VATAmount] = @VATAmount
                          ,[TotalAmountIncludingVAT] = @TotalAmountIncludingVAT
                     WHERE [InvoiceLineItemKey] = @InvoiceLineItemKey";

            return sql;
        }
    }
}