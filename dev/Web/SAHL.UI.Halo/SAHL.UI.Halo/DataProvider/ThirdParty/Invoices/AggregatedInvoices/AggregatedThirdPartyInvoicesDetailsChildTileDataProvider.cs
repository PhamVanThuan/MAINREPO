using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Details;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices
{
    public class AggregatedThirdPartyInvoicesChildDataProvider : HaloTileBaseChildDataProvider,
                                                                 IHaloTileChildDataProvider<AggregatedThirdPartyInvoicesDetailsChildTileConfiguration>
    {
        public AggregatedThirdPartyInvoicesChildDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            int genericKeyType = (int) SAHL.Core.BusinessModel.Enums.GenericKeyType.LegalEntity;
            return string.Format(@"SELECT
                                        invoice.InvoiceStatusKey As Context,
                                        party.LegalEntityKey As BusinessKey,
                                        {0} as BusinessKeyType
                                    FROM ThirdPartyInvoice Invoice
                                        INNER JOIN ThirdParty party ON party.Id = Invoice.ThirdPartyId 
                                        INNER JOIN InvoiceStatus st on st.InvoiceStatusKey = Invoice.InvoiceStatusKey
                                    WHERE 
                                        party.LegalEntityKey = {1}	
                                    Group By
                                        invoice.InvoiceStatusKey, party.LegalEntityKey", genericKeyType, businessContext.BusinessKey.Key);
        }

    }
}