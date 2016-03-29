using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.CommandHandlers.Internal;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.AddInvoiceLineItem
{
    public class when_adding_invoice_line_item_succeed : WithFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static AddThirdPartyInvoiceLineItemsCommand command;
        private static AddThirdPartyInvoiceLineItemsCommandHandler handler;
        private static InvoiceLineItemModel invoiceLineItemModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItemModels;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metada;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            invoiceLineItemModel = new InvoiceLineItemModel(null,1212, 12, 1234.34M, true);
            invoiceLineItemModels = new List<InvoiceLineItemModel>{invoiceLineItemModel};

            command = new AddThirdPartyInvoiceLineItemsCommand(invoiceLineItemModels);
            handler = new AddThirdPartyInvoiceLineItemsCommandHandler(thirdPartyInvoiceDataManager);
            metada = An<IServiceRequestMetadata>();
            messages = SystemMessageCollection.Empty();
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, metada);
        };

        
        private It should_add_disbursement = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AddInvoiceLineItem(Param<InvoiceLineItemModel>.Matches(y => 
                y.ThirdPartyInvoiceKey == invoiceLineItemModel.ThirdPartyInvoiceKey
                && y.InvoiceLineItemDescriptionKey == invoiceLineItemModel.InvoiceLineItemDescriptionKey
                && y.AmountExcludingVAT == invoiceLineItemModel.AmountExcludingVAT
                && y.IsVATItem == invoiceLineItemModel.IsVATItem
                && y.VATAmount == invoiceLineItemModel.VATAmount
                && y.TotalAmountIncludingVAT == invoiceLineItemModel.TotalAmountIncludingVAT
                )));
        };

        private It should_not_return_any_message = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}