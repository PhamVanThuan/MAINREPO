using Machine.Fakes;
using Machine.Specifications;
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
    public class when_adding_third_party_invoice_line_item : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IThirdPartyInvoiceManager thirdPartyInvoiceDataFilter;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static AmendThirdPartyInvoiceCommand command;
        private static AmendThirdPartyInvoiceCommandHandler handler;
        private static ThirdPartyInvoiceModel thirdPartyInvoice;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static InvoiceLineItemModel newInvoiceLineItemModel;
        private static decimal totalInvoiceAmountExcludingVATChange;
        private static decimal totalVATAmountChange;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            thirdPartyInvoiceDataFilter = An<IThirdPartyInvoiceManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            thirdPartyInvoiceKey = 1212;
            newInvoiceLineItemModel = new InvoiceLineItemModel(null, thirdPartyInvoiceKey, 12, 1234.34M, true);
            invoiceLineItems = new List<InvoiceLineItemModel> { newInvoiceLineItemModel, new InvoiceLineItemModel(121, thirdPartyInvoiceKey, 1, 21212.34M, true) };
            thirdPartyInvoice = new ThirdPartyInvoiceModel(thirdPartyInvoiceKey, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);

            totalInvoiceAmountExcludingVATChange = newInvoiceLineItemModel.AmountExcludingVAT;
            totalVATAmountChange = newInvoiceLineItemModel.VATAmount;

            command = new AmendThirdPartyInvoiceCommand(thirdPartyInvoice);
            handler = new AmendThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, thirdPartyInvoiceDataFilter, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);

            thirdPartyInvoiceDataFilter.WhenToldTo(y => y.GetUpdatedInvoicedLineItems(command.ThirdPartyInvoiceModel.LineItems))
                .Return(new InvoiceLineItemModel[] { });

            thirdPartyInvoiceDataFilter.WhenToldTo(y => y.GetRemovedInvoiceLineItems(Param.IsAny<IEnumerable<InvoiceLineItemDataModel>>(), Param.IsAny<IEnumerable<InvoiceLineItemModel>>()))
                .Return(new InvoiceLineItemDataModel[] { });
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_an_invoice_line_item = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
               Param<AddThirdPartyInvoiceLineItemsCommand>.Matches(y => y.NewInvoiceLineItems.ToList<InvoiceLineItemModel>().Any(z => z.AmountExcludingVAT == newInvoiceLineItemModel.AmountExcludingVAT
                   && z.InvoiceLineItemDescriptionKey == newInvoiceLineItemModel.InvoiceLineItemDescriptionKey
                   && z.InvoiceLineItemKey == newInvoiceLineItemModel.InvoiceLineItemKey
                   && z.IsVATItem == newInvoiceLineItemModel.IsVATItem
                   && z.ThirdPartyInvoiceKey == newInvoiceLineItemModel.ThirdPartyInvoiceKey)

               ), serviceRequestMetaData));
        };

        private It should_correctly_adjust_the_total_invoice_amount_and_applicable_VAT = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(y => y.AmendInvoiceTotals(
                  Param<int>.Matches(m => m == thirdPartyInvoiceKey)
            ));
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}