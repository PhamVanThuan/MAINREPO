using Machine.Fakes;
using Machine.Specifications;
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
    public class when_adding_a_line_item_fails : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static CaptureThirdPartyInvoiceCommand command;
        private static CaptureThirdPartyInvoiceCommandHandler commandHandler;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static ThirdPartyInvoiceCapturedEvent thirdPartyInvoiceCapturedEvent;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static ISystemMessageCollection errorMessages;
        private static ISystemMessage errorMessage;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;

        private Establish context = () =>
        {
            errorMessage = new SystemMessage("An internal error occured."
                        , SystemMessageSeverityEnum.Error);
            errorMessages = SystemMessageCollection.Empty();
            errorMessages.AddMessage(errorMessage);
            serviceCommandRouter = An<IServiceCommandRouter>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            messages = SystemMessageCollection.Empty();
            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) };
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            thirdPartyInvoiceCapturedEvent = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, thirdPartyInvoiceModel);
            command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
            commandHandler = new CaptureThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
                Param.IsAny<AddThirdPartyInvoiceLineItemsCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(errorMessages);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
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

        private It should_create_a_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_update_the_third_party_invoice_record = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.AmendThirdPartyInvoiceHeader(thirdPartyInvoiceModel));
        };

        private It should_try_to_add_line_items = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
                Param.IsAny<AddThirdPartyInvoiceLineItemsCommand>(), Param.IsAny<IServiceRequestMetadata>())).Times(1);
        };

        private It should_not_raise_a_third_party_invoice_added_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceCapturedEvent>.Matches(
                y => y.ThirdPartyInvoiceModel.InvoiceNumber == thirdPartyInvoiceModel.InvoiceNumber),
                thirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_complete_a_unit_of_work = () =>
        {
            unitOfWork.WasNotToldTo(x => x.Complete());
        };

        private It should_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };
    }
}