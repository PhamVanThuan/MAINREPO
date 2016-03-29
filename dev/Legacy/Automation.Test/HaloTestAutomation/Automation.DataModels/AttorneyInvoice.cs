using System;

namespace Automation.DataModels
{
    public sealed class AttorneyInvoice
    {
        public int AccountKey { get; set; }

        public int AttorneyKey { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime ChangeDate { get; set; }
    }
}