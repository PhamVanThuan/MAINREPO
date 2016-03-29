using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CaptureThirdPartyInvoice
{
    public class when_adding_third_party_invoice_succeed : WithCoreFakes
    {
        private static IServiceRequestMetadata metadata;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static CaptureThirdPartyInvoiceCommand command;
        private static CaptureThirdPartyInvoiceCommandHandler commandHandler;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static ThirdPartyInvoiceCapturedEvent thirdPartyInvoiceCapturedEvent;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IUnitOfWork uow;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;

        private Establish context = () =>
        {
            unitOfWorkFactory = An<IUnitOfWorkFactory>();
            uow = An<IUnitOfWork>();
            serviceCommandRouter = An<IServiceCommandRouter>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            metadata = An<IServiceRequestMetadata>();
            eventRaiser = An<IEventRaiser>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            messages = SystemMessageCollection.Empty();
            invoiceLineItems = new List<InvoiceLineItemModel> { 
                new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) 
            };
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            thirdPartyInvoiceCapturedEvent = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, thirdPartyInvoiceModel);
            command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
            commandHandler = new CaptureThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
                Param.IsAny<AddThirdPartyInvoiceLineItemsCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(uow);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, metadata);
        };

        private It should_execute_the_third_party_invoice_must_have_line_item_rule = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param<ThirdPartyInvoiceModel>.Matches(y =>
                y.LineItems.Count() == thirdPartyInvoiceModel.LineItems.Count()
                && y.LineItems.Count() == thirdPartyInvoiceModel.LineItems.Count()
                && y.ThirdPartyInvoiceKey == thirdPartyInvoiceModel.ThirdPartyInvoiceKey
                && y.ThirdPartyId == thirdPartyInvoiceModel.ThirdPartyId
                 )));
        };

        private It should_add_all_line_items = () =>
        {
            foreach (var invoiceLineItemModel in thirdPartyInvoiceModel.LineItems)
            {
                serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
                  Param<AddThirdPartyInvoiceLineItemsCommand>.Matches(y => y.NewInvoiceLineItems.ToList<InvoiceLineItemModel>().Any(z => z.AmountExcludingVAT == invoiceLineItemModel.AmountExcludingVAT
                      && z.InvoiceLineItemDescriptionKey == invoiceLineItemModel.InvoiceLineItemDescriptionKey
                      && z.InvoiceLineItemKey == invoiceLineItemModel.InvoiceLineItemKey
                      && z.IsVATItem == invoiceLineItemModel.IsVATItem
                      && z.ThirdPartyInvoiceKey == invoiceLineItemModel.ThirdPartyInvoiceKey)

                  ), Param.IsAny<IServiceRequestMetadata>()));
            }
        };

        private It should_create_a_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_update_the_third_party_invoice_record = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendThirdPartyInvoiceHeader(thirdPartyInvoiceModel));
        };

        private It should_set_the_invoice_status_captured = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Captured));
        };

        private It should_raise_a_third_party_invoice_added_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceCapturedEvent>.Matches(
                y => y.ThirdPartyInvoiceModel.InvoiceNumber == thirdPartyInvoiceModel.InvoiceNumber),
                thirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_complete_a_unit_of_work = () =>
        {
            uow.WasToldTo(x => x.Complete());
        };

        private It should_not_return_any_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}