using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailChildTileDataProvider : HaloTileBaseChildDataProvider,
                                                     IHaloTileChildDataProvider<ThirdPartyInvoiceDetailChildTileConfiguration>
    {
        public ThirdPartyInvoiceDetailChildTileDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            return string.Format(@"SELECT
                                    tpi.AccountKey As Context,
                                    tpi.ThirdPartyInvoiceKey As BusinessKey,
                                    {0} as BusinessKeyType
                                   FROM [2am].dbo.ThirdPartyInvoice tpi
                                    JOIN [2am].dbo.ThirdParty tp ON tp.Id = tpi.ThirdPartyId
                                    JOIN [2am].dbo.InvoiceStatus st on st.InvoiceStatusKey = tpi.InvoiceStatusKey
                                   WHERE tp.LegalEntityKey = {1}
                                   AND tpi.InvoiceStatusKey = {2}", genericKeyType, businessContext.BusinessKey.Key, businessContext.Context);
        }
    }
}