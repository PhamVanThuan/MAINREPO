using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class ThirdPartyInvoiceRootModel : IHaloTileModel
    {
        public int AccountKey
        { get; set; }

        public string InvoiceNumber
        { get; set; }

        public int InvoiceStatusKey
        { get; set; }

        public int ThirdPartyInvoiceKey
        { get; set; }

        public string InvoiceStatusDescription
        { get; set; }

        public double AmountExcludingVAT
        { get; set; }

        public double VATAmount
        { get; set; }

        public double TotalAmountIncludingVAT
        { get; set; }

        public DateTime InvoiceDate
        { get; set; }

        public string Reference
        { get; set; }

        public string ReceivedFromEmailAddress
        { get; set; }

        public string PaymentReference
        { get; set; }

        public string AttorneyName
        { get; set; }
    }
}