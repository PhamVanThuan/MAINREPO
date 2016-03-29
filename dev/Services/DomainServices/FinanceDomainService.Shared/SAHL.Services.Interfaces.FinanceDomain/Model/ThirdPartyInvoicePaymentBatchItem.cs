
namespace SAHL.Services.Interfaces.FinanceDomain.Model
{
    public class ThirdPartyInvoicePaymentBatchItem
    {
        public ThirdPartyInvoicePaymentBatchItem(int legalEntityKey, int genericKey, int genericTypeKey, int accountKey, decimal invoiceTotal, int sourceBankAccountKey,
            int targetBankAccountKey, string sahlReferenceNumber, string sourceReference, int thirdPartyPaymentBatchKey, string targetName, string invoiceNumber, string emailAddress
            , string paymentReference)
        {
            LegalEntityKey = legalEntityKey;
            GenericKey = genericKey;
            GenericTypeKey = genericTypeKey;
            AccountKey = accountKey;
            InvoiceTotal = invoiceTotal;
            SourceBankAccountKey = sourceBankAccountKey;
            TargetBankAccountKey = targetBankAccountKey;
            SahlReferenceNumber = sahlReferenceNumber;
            PaymentReference = paymentReference;
            SourceReference = sourceReference;
            ThirdPartyPaymentBatchKey = thirdPartyPaymentBatchKey;
            TargetName = targetName;
            InvoiceNumber = invoiceNumber;
            EmailAddress = emailAddress;
        }

        public int LegalEntityKey { get; set; }
        public int GenericKey { get; set; }
        public int GenericTypeKey { get; set; }
        public int AccountKey { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int SourceBankAccountKey { get; set; }
        public int TargetBankAccountKey { get; set; }
        public string SahlReferenceNumber { get; set; }
        public string PaymentReference { get; set; }
        public string SourceReference { get; set; }
        public int ThirdPartyPaymentBatchKey { get; set; }
        public string TargetName { get; set; }
        public string InvoiceNumber { get; set; }
        public string EmailAddress { get; set; }
    }
}
