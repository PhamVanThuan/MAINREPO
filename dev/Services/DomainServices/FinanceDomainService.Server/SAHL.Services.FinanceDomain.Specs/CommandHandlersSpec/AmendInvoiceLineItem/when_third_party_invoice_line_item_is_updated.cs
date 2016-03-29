using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AmmendInvoiceLineItem
{
    public class when_third_party_invoice_line_item_is_updated : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static AmendThirdPartyInvoiceLineItemsCommand command;
        private static AmendThirdPartyInvoiceLineItemsCommandHandler handler;
        private static List<InvoiceLineItemModel> lineItemsToUpdate;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            lineItemsToUpdate = new List<InvoiceLineItemModel>() {
                new InvoiceLineItemModel(131, 1212, 12, 1234.34M, true),
                new InvoiceLineItemModel(131, 1212, 12, 1234.34M, true)
            };
            command = new AmendThirdPartyInvoiceLineItemsCommand(lineItemsToUpdate);
            handler = new AmendThirdPartyInvoiceLineItemsCommandHandler(thirdPartyInvoiceDataManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_amend_the_first_line_item = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendInvoiceLineItem(lineItemsToUpdate.First()));
        };

        private It should_amend_the_last_line_item = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendInvoiceLineItem(lineItemsToUpdate.Last()));
        };

        private It should_only_amend_two_line_items = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendInvoiceLineItem(Arg.Any<InvoiceLineItemModel>())).Twice();
        };

        private It should_not_return_any_message = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}