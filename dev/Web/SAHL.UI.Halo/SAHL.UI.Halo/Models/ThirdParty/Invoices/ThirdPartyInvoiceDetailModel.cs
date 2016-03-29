using SAHL.UI.Halo.Shared.Configuration;
using System;

namespace SAHL.UI.Halo.Models.ThirdParty.Invoices
{
    public class ThirdPartyInvoiceDetailModel : IHaloTileModel
    {
        public int AccountKey
        { get; set; }

        public string InvoiceNumber
        { get; set; }

        public DateTime InvoiceDate
        { get; set; }

        public string Reference
        { get; set; }

        public string ReceivedFromEmailAddress
        { get; set; }

        public string LossControlStage
        { get; set; }

        public string ClientName
        { get; set; }
    }
}