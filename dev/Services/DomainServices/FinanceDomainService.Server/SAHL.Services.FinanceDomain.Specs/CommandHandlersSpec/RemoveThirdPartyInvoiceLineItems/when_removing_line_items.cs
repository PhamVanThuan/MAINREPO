using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.RemoveThirdPartyInvoiceLineItems
{
    public class when_removing_line_items : WithCoreFakes
    {
        private static RemoveThirdPartyInvoiceLineItemsCommandHandler handler;
        private static RemoveThirdPartyInvoiceLineItemsCommand command;
        private static IEnumerable<InvoiceLineItemDataModel> lineItemsToRemove;
        private static IThirdPartyInvoiceDataManager dataManager;

        private Establish context = () =>
            {
                dataManager = An<IThirdPartyInvoiceDataManager>();
                lineItemsToRemove = new List<InvoiceLineItemDataModel>()
                {
                    new InvoiceLineItemDataModel(1, 2222, 1, 100.50M, false, 0.00M, 100.50M ),
                    new InvoiceLineItemDataModel(2, 2222, 1, 100.50M, false, 0.00M, 100.50M ),
                };
                command = new RemoveThirdPartyInvoiceLineItemsCommand(lineItemsToRemove);
                handler = new RemoveThirdPartyInvoiceLineItemsCommandHandler(dataManager);
            };

        private Because of = () =>
            {
                messages = handler.HandleCommand(command, serviceRequestMetaData);
            };

        private It should_remove_the_first_line_item = () =>
            {
                dataManager.Received().RemoveInvoiceLineItem(lineItemsToRemove.First().InvoiceLineItemKey);
            };

        private It should_remove_the_second_line_item = () =>
            {
                dataManager.Received().RemoveInvoiceLineItem(lineItemsToRemove.Last().InvoiceLineItemKey);
            };

        private It should_return_no_error_messages = () =>
            {
                messages.AllMessages.ShouldBeEmpty();
            };
    }
}