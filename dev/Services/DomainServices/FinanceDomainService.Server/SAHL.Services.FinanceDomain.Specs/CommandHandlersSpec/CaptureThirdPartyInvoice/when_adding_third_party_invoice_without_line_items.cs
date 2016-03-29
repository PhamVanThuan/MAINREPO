using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CaptureThirdPartyInvoice
{
    public class when_adding_third_party_invoice_without_line_items : WithCoreFakes
    {
        private static IServiceRequestMetadata metadata;
        private static ISystemMessage errorMessage;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static CaptureThirdPartyInvoiceCommand command;
        private static CaptureThirdPartyInvoiceCommandHandler commandHandler;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static ThirdPartyInvoiceCapturedEvent thirdPartyInvoiceCapturedEvent;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;

        private Establish context = () =>
        {
            errorMessage = new SystemMessage("Invalid invoice. A third party invoice should contain at least one line item."
                        , SystemMessageSeverityEnum.Error);
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            metadata = An<IServiceRequestMetadata>();
            eventRaiser = An<IEventRaiser>();
            messages = SystemMessageCollection.Empty();
            invoiceLineItems = new List<InvoiceLineItemModel> { };
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            thirdPartyInvoiceCapturedEvent = new ThirdPartyInvoiceCapturedEvent(DateTime.Now, thirdPartyInvoiceModel);
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
            commandHandler = new CaptureThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);
            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ThirdPartyInvoiceModel>()))
             .Callback<ISystemMessageCollection>(y =>
             {
                 y.AddMessage(errorMessage);
             });
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

        private It should_not_update_the_third_party_invoice_record = () =>
        {
            thirdPartyInvoiceDataManager.WasNotToldTo(x => x.AmendThirdPartyInvoiceHeader(Arg.Any<ThirdPartyInvoiceModel>()));
        };

        private It should_not_raise_a_account_third_party_invoice_added_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param<ThirdPartyInvoiceCapturedEvent>.Matches(
                y => y.ThirdPartyInvoiceModel.InvoiceNumber == thirdPartyInvoiceModel.InvoiceNumber),
                thirdPartyInvoiceModel.ThirdPartyInvoiceKey, (int)SAHL.Core.BusinessModel.Enums.GenericKeyType.ThirdPartyInvoice, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_a_no_line_item_found_error_messages = () =>
        {
            messages.AllMessages.Any(x => x.Message.Equals(errorMessage.Message));
        };
    }
}