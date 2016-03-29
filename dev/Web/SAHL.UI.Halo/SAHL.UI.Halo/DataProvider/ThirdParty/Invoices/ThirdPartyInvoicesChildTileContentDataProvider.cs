using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices
{
    public class ThirdPartyInvoicesChildTileContentDataProvider : HaloTileBaseContentDataProvider<ThirdPartyInvoiceChildModel>
    {
        public ThirdPartyInvoicesChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT
                                    COUNT(ti.ThirdPartyInvoiceKey) as NumberOfInvoices
                                FROM
                                    [2am].dbo.ThirdPartyInvoice ti
                                    JOIN [2am].dbo.ThirdParty tp ON tp.Id = ti.ThirdPartyId
                                WHERE tp.LegalEntityKey = {0}", businessContext.BusinessKey.Key);
        }
    }
}