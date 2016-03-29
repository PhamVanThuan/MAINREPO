using SAHL.Core.Attributes;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    [InsertConventionExclude]
    public class UpdateThirdPartyInvoiceHeaderStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public Guid ThirdPartyId { get; protected set; }

        public string InvoiceNumber { get; protected set; }

        public DateTime? InvoiceDate { get; protected set; }

        public bool CapitaliseInvoice { get; protected set; }

        public string PaymentReference { get; protected set; }

        public UpdateThirdPartyInvoiceHeaderStatement(int thirdPartyInvoiceKey, Guid thirdPartyId, string invoiceNumber, DateTime? invoiceDate, bool capitaliseInvoice, string paymentReference)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.ThirdPartyId = thirdPartyId;
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceDate = invoiceDate;
            this.CapitaliseInvoice = capitaliseInvoice;
            this.PaymentReference = paymentReference;
        }

        public string GetStatement()
        {
            var sql = @"UPDATE [2AM].[dbo].[ThirdPartyInvoice]
                       SET
                           [ThirdPartyId] = @ThirdPartyId
	                      ,[InvoiceNumber] = @InvoiceNumber
	                      ,[InvoiceDate] = @InvoiceDate
                          ,[CapitaliseInvoice] = @CapitaliseInvoice
                          ,[PaymentReference] = @PaymentReference
                     WHERE [ThirdPartyInvoiceKey] = @ThirdPartyInvoiceKey";

            return sql;
        }
    }
}