using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice.Actions.Statements;
using SAHL.UI.Halo.Shared.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.UI.Halo.Specs.DynamicActionProvider
{
    public class when_loading_dynamic_action_for_captured_invoice : WithFakes
    {
        private static ThirdPartyInvoiceDynamicActionProvider actionProvider;
        private static IDbFactory dbFactory;
        private static BusinessContext businessContext;
        private static BusinessKey businessKey;
        private static IEnumerable<IHaloTileAction> tileAction;
        private static IDbContext dbContext;
        private static int thirdPartyInvoiceKey;

        Establish context = () =>
        {
            dbFactory = An<IDbFactory>();
            dbContext = An<IDbContext>(new object[0]);
            thirdPartyInvoiceKey = 7;
            businessKey = new BusinessKey { Key = thirdPartyInvoiceKey };
            businessContext = new BusinessContext("", businessKey);
            actionProvider = new ThirdPartyInvoiceDynamicActionProvider(dbFactory);

            dbFactory.WhenToldTo(x => x.NewDb().InAppContext()).Return(dbContext);
            dbContext.WhenToldTo(x => x.SelectOne(Param<GetInvoiceLineItemsCountStatement>.Matches(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)))
                .Return(1);
        };

        Because of = () =>
        {
            tileAction = actionProvider.GetTileActions(businessContext);
        };

        It should_check_the_db_for_captured_line_items = () =>
        {
            dbContext.WasToldTo(x => x.SelectOne(Param<GetInvoiceLineItemsCountStatement>.Matches(y => y.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)));
        };

        It should_return_only_the_capture_invoice_wizard_action = () =>
        {
            tileAction.First().ShouldBeOfExactType(typeof(ThirdPartyInvoiceAmendWizardAction));
            tileAction.Count().ShouldEqual(1);
        };


    }
}
