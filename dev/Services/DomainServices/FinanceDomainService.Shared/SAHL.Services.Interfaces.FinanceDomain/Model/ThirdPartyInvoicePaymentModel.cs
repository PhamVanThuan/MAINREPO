namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class ThirdPartyInvoicePaymentModel
    {
        public string FirmName { get; set; }

        public string EmailAddress { get; set; }

        public int AccountNumber { get; set; }

        public decimal InvoiceAmountIncludingVat { get; set; }

        public string InvoiceNumber { get; set; }

        public int ThirdPartyKey { get; set; }

        public ThirdPartyInvoicePaymentModel(string firmName, string emailAddress, int accountNumber, decimal invoiceAmountIncludingVat, string invoiceNumber, int thirdPartyKey)
        {
            this.FirmName = firmName;
            this.EmailAddress = emailAddress;
            this.AccountNumber = accountNumber;
            this.InvoiceAmountIncludingVat = invoiceAmountIncludingVat;
            this.InvoiceNumber = invoiceNumber;
            this.ThirdPartyKey = thirdPartyKey;
        }
    }
}