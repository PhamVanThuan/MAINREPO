using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.InvoiceDocument
{
    public class InvoiceDocumentChildDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<InvoiceDocumentChildTileConfiguration>
    {
        public InvoiceDocumentChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            return string.Format(@"SELECT 
                                    tpi.ThirdPartyInvoiceKey As Context, 
                                    tpi.ThirdPartyInvoiceKey As BusinessKey,
                                    {0} as BusinessKeyType
                                FROM [2am].dbo.ThirdPartyInvoice tpi
                                JOIN [2am].dbo.InvoiceStatus st on st.InvoiceStatusKey = tpi.InvoiceStatusKey  
                                JOIN ImageIndex.dbo.Data doc on tpi.ThirdPartyInvoiceKey = Convert(int, doc.Key2)
                                WHERE 
                                    tpi.ThirdPartyInvoiceKey = {1} 
                                    AND doc.Stor = 44 
                                    AND doc.Key6 = 'invoices'", genericKeyType, businessContext.BusinessKey.Key);
        }
    }
}