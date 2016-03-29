using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice
{
    public class ThirdPartyInvoiceRootTileContentDataProvider : HaloTileBaseContentDataProvider<ThirdPartyInvoiceRootModel>
    {
        public ThirdPartyInvoiceRootTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"Select
                                        tpi.AccountKey,
                                        tpi.InvoiceNumber,
                                        tpi.InvoiceDate,
                                        tpi.InvoiceStatusKey,
                                        tpi.ThirdPartyInvoiceKey,
                                        tpi.AmountExcludingVAT,
                                        tpi.VATAmount,
                                        tpi.TotalAmountIncludingVAT,
                                        st.Description as InvoiceStatusDescription,
                                        tpi.SahlReference as Reference,
                                        tpi.ReceivedFromEmailAddress,
                                        tpi.PaymentReference,
										isnull(le.RegisteredName, '') as AttorneyName
                                    FROM [2am].dbo.ThirdPartyInvoice tpi
                                        JOIN [2am].dbo.InvoiceStatus st on st.InvoiceStatusKey = tpi.InvoiceStatusKey
										left join [2AM].[dbo].[ThirdParty] TP 	on TP.Id = tpi.ThirdPartyId
										left join [2AM].[dbo].[LegalEntity] LE  on le.LegalEntityKey = TP.LegalEntityKey 
                                    WHERE 
                                        tpi.ThirdPartyInvoiceKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}