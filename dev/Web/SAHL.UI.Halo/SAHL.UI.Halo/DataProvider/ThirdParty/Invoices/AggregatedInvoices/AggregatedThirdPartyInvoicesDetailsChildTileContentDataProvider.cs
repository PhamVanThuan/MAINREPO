using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.DataProvider.ThirdParty.Invoices.AggregatedInvoices
{
    public class AggregatedThirdPartyInvoicesDetailsChildTileContentDataProvider : HaloTileBaseContentDataProvider<AggregatedThirdPartyInvoiceGroupedModel>
    {
        public AggregatedThirdPartyInvoicesDetailsChildTileContentDataProvider(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public override string GetSqlStatement(BusinessContext businessContext)
        {
            return string.Format(@"SELECT
                                    invoice.InvoiceStatusKey as InvoiceStatusKey,
                                    st.Description as InvoiceStatusDescription,
                                    count(1) as NumberOfInvoices
                                FROM ThirdPartyInvoice Invoice
                                    INNER JOIN ThirdParty party ON party.Id = Invoice.ThirdPartyId
                                    INNER JOIN InvoiceStatus st on st.InvoiceStatusKey = Invoice.InvoiceStatusKey
                                WHERE
                                    party.LegalEntityKey = {0}
                                    AND
                                    invoice.InvoiceStatusKey = {1}
                                Group By
                                    invoice.InvoiceStatusKey,
                                    st.Description", businessContext.BusinessKey.Key, businessContext.Context);
        }
    }
}