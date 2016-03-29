using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetThirdPartyInvoiceByKeyStatement : ISqlStatement<ThirdPartyInvoiceDataModel>
    {
        public int ThirdPartyInvoiceKey { get; protected set; }

        public GetThirdPartyInvoiceByKeyStatement(int thirdPartyInvoiceKey)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
        }

        public string GetStatement()
        {
            return @"SELECT  [ThirdPartyInvoiceKey]
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
                            ,ISNULL([ReceivedDate], 0) AS ReceivedDate
                            ,[PaymentReference]                     
                      FROM
                            [2AM].[dbo].[ThirdPartyInvoice]
                      Where
                            [ThirdPartyInvoiceKey] = @ThirdPartyInvoiceKey";
        }
    }
}