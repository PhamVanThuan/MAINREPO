using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AmmendThirdPartyInvoice
{
    public class when_an_invoice_has_updated_line_items : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceManager thirdPartyInvoiceDataFilter;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static AmendThirdPartyInvoiceCommand command;
        private static AmendThirdPartyInvoiceCommandHandler handler;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static InvoiceLineItemModel newInvoiceLineItemModel;
        private static InvoiceLineItemDataModel oldInvoiceLineItemDataModel;
        private static int thirdPartyInvoiceKey;
        private static decimal totalVATAmountChange;
        private static decimal totalInvoiceAmountExcludingVATChange;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataFilter = An<IThirdPartyInvoiceManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            messages = SystemMessageCollection.Empty();
            thirdPartyInvoiceKey = 1212;
            newInvoiceLineItemModel = new InvoiceLineItemModel(121, thirdPartyInvoiceKey, 1, 110.00M, true);
            oldInvoiceLineItemDataModel = new InvoiceLineItemDataModel(121, thirdPartyInvoiceKey, 1, 100.00M, true, 14.00M, 114.00M);
            invoiceLineItems = new List<InvoiceLineItemModel> { newInvoiceLineItemModel };
            thirdPartyInvoice = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);

            totalInvoiceAmountExcludingVATChange = newInvoiceLineItemModel.AmountExcludingVAT - oldInvoiceLineItemDataModel.Amount;
            totalVATAmountChange = newInvoiceLineItemModel.VATAmount - oldInvoiceLineItemDataModel.VATAmount.Value;

            command = new AmendThirdPartyInvoiceCommand(thirdPartyInvoice);
            handler = new AmendThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, thirdPartyInvoiceDataFilter, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);

            thirdPartyInvoiceDataFilter.WhenToldTo(x => x.GetUpdatedInvoicedLineItems(Arg.Any<IEnumerable<InvoiceLineItemModel>>()))
                .Return(new List<InvoiceLineItemModel>() { newInvoiceLineItemModel });

            thirdPartyInvoiceDataFilter.WhenToldTo(x => x.GetRemovedInvoiceLineItems(Arg.Any<IEnumerable<InvoiceLineItemDataModel>>(), Arg.Any<IEnumerable<InvoiceLineItemModel>>()))
               .Return(
                     new InvoiceLineItemDataModel[] { }
               );
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_invoice_line_item_has_been_updated = () =>
        {
            thirdPartyInvoiceDataFilter.WasToldTo(x => x.GetUpdatedInvoicedLineItems(command.ThirdPartyInvoiceModel.LineItems));
        };

        private It should_update_an_invoice_line_item = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AmendThirdPartyInvoiceLineItemsCommand>(
               Param<AmendThirdPartyInvoiceLineItemsCommand>.Matches(y =>
                   y.lineItemsToUpdate.First().AmountExcludingVAT == newInvoiceLineItemModel.AmountExcludingVAT
                   && y.lineItemsToUpdate.First().InvoiceLineItemDescriptionKey == newInvoiceLineItemModel.InvoiceLineItemDescriptionKey
                   && y.lineItemsToUpdate.First().InvoiceLineItemKey == newInvoiceLineItemModel.InvoiceLineItemKey
                   && y.lineItemsToUpdate.First().IsVATItem == newInvoiceLineItemModel.IsVATItem
                   && y.lineItemsToUpdate.First().ThirdPartyInvoiceKey == newInvoiceLineItemModel.ThirdPartyInvoiceKey
               ), serviceRequestMetaData));
        };

        private It should_correctly_adjust_the_invoice_totals = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(y => y.AmendInvoiceTotals(thirdPartyInvoiceKey));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}