using SAHL.Core.Services;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using SAHL.Services.Interfaces.FinanceDomain.Queries;

namespace SAHL.Services.FinanceDomain.QueryStatements
{
    public class GetThirdPartyInvoiceQueryStatement : IServiceQuerySqlStatement<GetThirdPartyInvoiceQuery, GetThirdPartyInvoiceQueryResult>
    {
        public string GetStatement()
        {
            return @"SELECT  ti.[ThirdPartyInvoiceKey]
                            ,ti.[InvoiceNumber]
                            ,ti.[InvoiceStatusKey]
                            ,ti.[AccountKey]
                            ,ti.[SahlReference]
                            ,ti.[InvoiceDate]
                            ,ISNULL(ti.[ReceivedDate], 0) AS ReceivedDate
                            ,ti.[ReceivedFromEmailAddress]
                            ,ti.[ThirdPartyId]
                            ,le.[RegisteredName] AS ThirdPartyRegisteredName
                            ,ti.[CapitaliseInvoice]
                            ,ti.[AmountExcludingVAT]
                            ,ti.[VATAmount]
                            ,ti.[TotalAmountIncludingVAT]  
                     FROM [2AM].[dbo].[ThirdPartyInvoice] ti 
                    LEFT JOIN [2AM].[dbo].[ThirdParty] t ON t.Id = ti.ThirdPartyId
                    LEFT JOIN [2AM].[dbo].[LegalEntity] le ON le.LegalEntityKey = t.LegalEntityKey
                    WHERE ThirdPartyInvoiceKey = @ThirdPartyInvoiceKey";
        }
    }
}