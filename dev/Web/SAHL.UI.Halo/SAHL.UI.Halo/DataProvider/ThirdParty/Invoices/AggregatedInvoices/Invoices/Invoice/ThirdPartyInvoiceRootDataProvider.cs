using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice
{
    public class ThirdPartyInvoiceRootDataProvider : HaloTileBaseChildDataProvider,
                                                       IHaloTileChildDataProvider<ThirdPartyInvoiceRootTileConfiguration>
    {
        public ThirdPartyInvoiceRootDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {

            int genericKeyType = (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice;
            return string.Format(@"SELECT {0} As BusinessKey, {1} As Context,  {2} as BusinessKeyType"
                ,businessContext.BusinessKey.Key, businessContext.Context, genericKeyType);
        }
    }
}