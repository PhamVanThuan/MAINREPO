using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
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
    public class when_an_invoice_has_removed_line_items : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceManager thirdPartyInvoiceDataFilter;
        private static AmendThirdPartyInvoiceCommand command;
        private static AmendThirdPartyInvoiceCommandHandler handler;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static InvoiceLineItemModel oldInvoiceLineItemModel;
        private static InvoiceLineItemDataModel oldInvoiceLineItemDataModel;
        private static InvoiceLineItemDataModel removedInvoiceLineItemModel;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static int thirdPartyInvoiceKey;
        private static decimal totalVATAmountChange;
        private static decimal totalInvoiceAmountExcludingVATChange;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataFilter = An<IThirdPartyInvoiceManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            thirdPartyInvoiceKey = 1212;
            int removedInvoiceLineItemKey = 100;
            oldInvoiceLineItemModel = new InvoiceLineItemModel(121, thirdPartyInvoiceKey, 1, 100.00M, true);
            oldInvoiceLineItemDataModel = new InvoiceLineItemDataModel(121, thirdPartyInvoiceKey, 1, 100.00M, true, 14.00M, 114.00M);
            removedInvoiceLineItemModel = new InvoiceLineItemDataModel(removedInvoiceLineItemKey, thirdPartyInvoiceKey, 1, 100.00M, true, 14.00M, 114.00M);

            thirdPartyInvoiceDataManager.WhenToldTo(x => x.GetInvoiceLineItems(thirdPartyInvoiceKey)).
                Return(new List<InvoiceLineItemDataModel> { oldInvoiceLineItemDataModel, removedInvoiceLineItemModel });

            invoiceLineItems = new List<InvoiceLineItemModel> { oldInvoiceLineItemModel };
            thirdPartyInvoice = new ThirdPartyInvoiceModel(thirdPartyInvoiceKey, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);

            totalInvoiceAmountExcludingVATChange = oldInvoiceLineItemModel.AmountExcludingVAT - (oldInvoiceLineItemDataModel.Amount + removedInvoiceLineItemModel.Amount);
            totalVATAmountChange = oldInvoiceLineItemModel.VATAmount - (oldInvoiceLineItemDataModel.VATAmount.Value + removedInvoiceLineItemModel.VATAmount.Value);

            command = new AmendThirdPartyInvoiceCommand(thirdPartyInvoice);
            handler = new AmendThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, thirdPartyInvoiceDataFilter, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);

            thirdPartyInvoiceDataFilter.WhenToldTo(x => x.GetRemovedInvoiceLineItems(Arg.Any<IEnumerable<InvoiceLineItemDataModel>>(), Arg.Any<IEnumerable<InvoiceLineItemModel>>()))
                .Return(new InvoiceLineItemDataModel[] { removedInvoiceLineItemModel });

            thirdPartyInvoiceDataFilter.WhenToldTo(y => y.GetUpdatedInvoicedLineItems(command.ThirdPartyInvoiceModel.LineItems))
                .Return(new List<InvoiceLineItemModel>());
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_remove_the_removed_line_items = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand(Arg.Is<RemoveThirdPartyInvoiceLineItemsCommand>(
                y => y.lineItemsToRemove.First().Equals(removedInvoiceLineItemModel)
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