using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetThirdPartyInvoiceQueryStatement : IServiceQuerySqlStatement<GetThirdPartyInvoiceQuery, ThirdPartyInvoiceDataModel>
    {
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
                          ,ISNULL([ReceivedDate], 0) AS ReceivedDate, PaymentReference
                     FROM [2AM].[dbo].[ThirdPartyInvoice] WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
        }
    }
}
