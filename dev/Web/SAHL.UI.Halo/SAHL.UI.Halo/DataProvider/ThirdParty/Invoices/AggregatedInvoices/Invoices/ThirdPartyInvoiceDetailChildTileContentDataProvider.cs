using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices
{
    public class ThirdPartyInvoiceDetailChildTileContentDataProvider : HaloTileBaseContentDataProvider<ThirdPartyInvoiceDetailModel>
    {
        public ThirdPartyInvoiceDetailChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"Select
                                    tpi.AccountKey,
                                    tpi.InvoiceNumber,
                                    tpi.InvoiceDate,
                                    tpi.SahlReference as Reference,
                                    tpi.ReceivedFromEmailAddress,
                                    lc.Stage as LossControlStage,
                                    [2am].dbo.AccountLegalName({0}, 0) AS ClientName
                                FROM [2am].dbo.ThirdPartyInvoice tpi
                                    JOIN [2am].dbo.ThirdParty tp ON tp.Id = tpi.ThirdPartyId
                                    JOIN [2am].dbo.InvoiceStatus st on st.InvoiceStatusKey = tpi.InvoiceStatusKey
                                    LEFT JOIN (
                                    select top 1 f.eMapName as Process, f.eStageName as Stage, cast(f.eFolderName as int) AccountKey
                                    from [e-work].dbo.eFolder f 
                                    where f.eFolderName ='{0}'
                                    and f.eMapName = 'LossControl'
                                    order by f.ecreationtime desc
                                    ) lc ON lc.AccountKey = tpi.AccountKey
                                WHERE 
                                    tpi.ThirdPartyInvoiceKey = {1}", businessContext.Context, businessContext.BusinessKey.Key);
        }
    }
}