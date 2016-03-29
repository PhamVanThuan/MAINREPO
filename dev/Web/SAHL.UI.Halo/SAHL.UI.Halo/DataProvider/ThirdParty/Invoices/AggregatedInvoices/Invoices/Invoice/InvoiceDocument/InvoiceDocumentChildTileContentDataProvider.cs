using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument
{
    public class InvoiceDocumentChildTileContentDataProvider : HaloTileBaseContentDataProvider<InvoiceDocumentChildModel>
    {
        public InvoiceDocumentChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"Select TOP 1
                                    tpi.AccountKey,
                                    tpi.InvoiceNumber,
                                    tpi.InvoiceDate,
                                    tpi.InvoiceStatusKey,
                                    tpi.ThirdPartyInvoiceKey,
                                    st.Description as InvoiceStatusDescription,
                                    tpi.ReceivedFromEmailAddress,
                                    doc.GUID as DocumentGuid
                                FROM [2AM].dbo.ThirdPartyInvoice tpi
                                    JOIN [2AM].dbo.InvoiceStatus st on st.InvoiceStatusKey = tpi.InvoiceStatusKey
                                    JOIN ImageIndex.dbo.Data doc on tpi.ThirdPartyInvoiceKey = Convert(int, doc.Key2)
                                WHERE
                                    tpi.ThirdPartyInvoiceKey = {0} AND doc.Stor = 44 AND doc.Key6 = 'invoices'", businessContext.Context);
        }
    }
}