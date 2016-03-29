namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetThirdPartyInvoiceByAccountKeyQueryResult
    {
        public GetThirdPartyInvoiceByAccountKeyQueryResult(int ThirdPartyInvoiceKey, string SahlReference, int InvoiceStatusKey, int AccountKey, string InvoiceNumber)
        {
            this.SahlReference = SahlReference;
            this.InvoiceStatusKey = InvoiceStatusKey;
            this.AccountKey = AccountKey;
            this.ThirdPartyInvoiceKey = ThirdPartyInvoiceKey;
            this.InvoiceNumber = InvoiceNumber;
        }

        public int ThirdPartyInvoiceKey { get; set; }

        public string SahlReference { get; set; }

        public int InvoiceStatusKey { get; set; }

        public int AccountKey { get; set; }

        public string InvoiceNumber { get; set; }
    }
}