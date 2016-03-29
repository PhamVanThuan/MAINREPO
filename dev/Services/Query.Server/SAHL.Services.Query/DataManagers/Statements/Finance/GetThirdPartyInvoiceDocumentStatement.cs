using SAHL.Core.Data;
using SAHL.Services.Query.Models.Finance;

namespace SAHL.Services.Query.DataManagers.Statements.Finance
{
    public class GetThirdPartyInvoiceDocumentStatement : ISqlStatement<ThirdPartyInvoiceDocumentDataModel>
    {
        public string GetStatement()
        {
            return @"Select 
	                    cast(stor.DocumentGuid as uniqueidentifier) AS Id,
                        tpi.ThirdPartyInvoiceKey as InvoiceId,
                        stor.AccountKey AS AccountKey,
	                    stor.StorKey as StorKey,
	                    stor.ThirdPartyInvoiceKey AS ThirdPartyInvoiceKey,
	                    stor.EmailSubject AS EmailSubject,
	                    stor.FromEmailAddress AS FromEmailAddress,
	                    stor.InvoiceFileName AS InvoiceFileName,
	                    stor.Category AS Category,
	                    stor.DateReceived AS DateReceived,
	                    stor.DateProcessed AS DateProcessed,
	                    st.InvoiceStatusKey AS InvoiceStatusKey,
	                    st.Description AS InvoiceStatusDescription,
	                    tpi.ThirdPartyId AS ThirdPartyId,
	                    tp.ThirdPartyKey AS ThirdPartyKey             
                    FROM [2AM].[dbo].ThirdPartyInvoice tpi
                    LEFT JOIN [2AM].[dbo].ThirdParty tp 
	                    On tp.Id = tpi.ThirdPartyId
                    JOIN [2AM].[dbo].InvoiceStatus st 
	                    On st.InvoiceStatusKey = tpi.InvoiceStatusKey
                    JOIN [2AM].[datastor].[ThirdPartyInvoicesSTOR] AS stor 
	                    On stor.ThirdPartyInvoiceKey = tpi.ThirdPartyInvoiceKey";
        }
    }
}