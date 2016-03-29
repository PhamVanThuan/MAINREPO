using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions.Statements;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions
{
    public class ThirdPartyInvoiceDynamicActionProvider : IHaloTileDynamicActionProvider
    {
        private IDbFactory dbFactory;
        public ThirdPartyInvoiceDynamicActionProvider(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        private IHaloTileAction GetTileActionToUse(int thirdPartyInvoiceKey)
        {
            IHaloTileAction tileToUse = new ThirdPartyInvoiceCaptureWizardAction();
            var getInvoiceLineItemsCountStatement = new GetInvoiceLineItemsCountStatement(thirdPartyInvoiceKey);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                int capturedLineItemsCount = db.SelectOne(getInvoiceLineItemsCountStatement);
                if (capturedLineItemsCount > 0)
                {
                    tileToUse = new ThirdPartyInvoiceAmendWizardAction();
                }
            }

            return tileToUse;
        }

        public IEnumerable<IHaloTileAction> GetTileActions(BusinessContext businessContext)
        {
            var dynamicActions = new List<IHaloTileAction>();
            var thirdPartyInvoiceKey = Convert.ToInt32(businessContext.BusinessKey.Key);
            dynamicActions.Add(GetTileActionToUse(thirdPartyInvoiceKey));
            return dynamicActions;
        }
    }
}
