using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AccountAttorneyInvoiceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AccountAttorneyInvoiceDataModel(int accountKey, int attorneyKey, string invoiceNumber, decimal amount, string comment, DateTime invoiceDate, decimal vatAmount, decimal totalAmount, DateTime changeDate)
        {
            this.AccountKey = accountKey;
            this.AttorneyKey = attorneyKey;
            this.InvoiceNumber = invoiceNumber;
            this.Amount = amount;
            this.Comment = comment;
            this.InvoiceDate = invoiceDate;
            this.VatAmount = vatAmount;
            this.TotalAmount = totalAmount;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public AccountAttorneyInvoiceDataModel(int accountAttorneyInvoiceKey, int accountKey, int attorneyKey, string invoiceNumber, decimal amount, string comment, DateTime invoiceDate, decimal vatAmount, decimal totalAmount, DateTime changeDate)
        {
            this.AccountAttorneyInvoiceKey = accountAttorneyInvoiceKey;
            this.AccountKey = accountKey;
            this.AttorneyKey = attorneyKey;
            this.InvoiceNumber = invoiceNumber;
            this.Amount = amount;
            this.Comment = comment;
            this.InvoiceDate = invoiceDate;
            this.VatAmount = vatAmount;
            this.TotalAmount = totalAmount;
            this.ChangeDate = changeDate;
		
        }		

        public int AccountAttorneyInvoiceKey { get; set; }

        public int AccountKey { get; set; }

        public int AttorneyKey { get; set; }

        public string InvoiceNumber { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public DateTime InvoiceDate { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.AccountAttorneyInvoiceKey =  key;
        }
    }
}