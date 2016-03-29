using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class UpdateThirdPartyInvoiceTotalsStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }
        public decimal TotalAmountExcludingVAT { get; protected set; }
        public decimal VATAmount { get; protected set; }
        public decimal TotalAmountIncludingVAT { get; protected set; }
        public UpdateThirdPartyInvoiceTotalsStatement(int thirdPartyInvoiceKey, decimal invoiceTotalVAT, decimal invoiceAmountExcludingVAT, decimal totalAmountIncludingVAT)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.TotalAmountExcludingVAT = invoiceAmountExcludingVAT;
            this.VATAmount = invoiceTotalVAT;
            this.TotalAmountIncludingVAT = totalAmountIncludingVAT;
        }

        public string GetStatement()
        {
            var sqlText = @"UPDATE [2AM].[dbo].[ThirdPartyInvoice]
                           SET [AmountExcludingVAT] = @TotalAmountExcludingVAT
                              ,[VATAmount] = @VATAmount
                              ,[TotalAmountIncludingVAT] = @TotalAmountIncludingVAT
                         WHERE 
                              ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
            return sqlText;
        }
    }
}
