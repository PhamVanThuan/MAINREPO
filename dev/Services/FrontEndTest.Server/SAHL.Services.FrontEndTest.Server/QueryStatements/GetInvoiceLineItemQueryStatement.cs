using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Queries;

namespace SAHL.Services.FrontEndTest.QueryStatements
{
    public class GetInvoiceLineItemQueryStatement : IServiceQuerySqlStatement<GetInvoiceLineItemQuery, InvoiceLineItemDataModel>
    {
        public string GetStatement()
        {
            return @"SELECT [InvoiceLineItemKey]
                        ,[ThirdPartyInvoiceKey]
                        ,[InvoiceLineItemDescriptionKey]
                        ,[Amount]
                        ,[IsVATItem]
                        ,[VATAmount]
                        ,[TotalAmountIncludingVAT]
                    FROM [2AM].[dbo].[InvoiceLineItem] 
                    WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
        }
    }
}
