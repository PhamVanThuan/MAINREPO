using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.EventProjection.Managers.AttorneyInvoiceMonthlyBreakdown.Statements
{
    public class GetThirdPartyInvoiceStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyInvoiceStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public string GetStatement()
        {
            return @"SELECT [ThirdPartyInvoiceKey]
                      ,[SahlReference]
                      ,[InvoiceStatusKey]
                      ,[AccountKey]
                      ,[ThirdPartyId]
                      ,[InvoiceNumber]
                      ,[InvoiceDate]
                      ,[ReceivedFromEmailAddress]
                      ,[AmountExcludingVAT]
                      ,[VATAmount]
                      ,[TotalAmountIncludingVAT]
                      ,[CapitaliseInvoice]
                      ,[ReceivedDate]
                      ,[PaymentReference]
                  FROM [dbo].[ThirdPartyInvoice] where thirdPartyInvoiceKey = @thirdPartyInvoiceKey";
        }
    }
}