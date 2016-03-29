using SAHL.Core.Data;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions.Statements
{
    public class GetInvoiceLineItemsCountStatement : ISqlStatement<int>
    {
        public int ThirdPartyInvoiceKey { get; private set; }

        public GetInvoiceLineItemsCountStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public string GetStatement()
        {
            return @"SELECT COUNT(*) FROM [2AM].[dbo].[InvoiceLineItem] WHERE ThirdPartyInvoiceKey= @ThirdPartyInvoiceKey";
        }
    }
}
