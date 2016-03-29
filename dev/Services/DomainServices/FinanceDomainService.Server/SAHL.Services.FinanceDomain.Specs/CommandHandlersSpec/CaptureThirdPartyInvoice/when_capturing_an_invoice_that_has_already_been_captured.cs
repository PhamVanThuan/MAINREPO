using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
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

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.CaptureThirdPartyInvoice
{
    public class when_capturing_an_invoice_that_has_already_been_captured : WithCoreFakes
    {
        private static ISystemMessage errorMessage;
        private static IServiceRequestMetadata metadata;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static CaptureThirdPartyInvoiceCommand command;
        private static CaptureThirdPartyInvoiceCommandHandler commandHandler;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;

        private Establish context = () =>
        {
            errorMessage = new SystemMessage("Third party invoice has already been captured", SystemMessageSeverityEnum.Error);
            serviceCommandRouter = An<IServiceCommandRouter>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            metadata = An<IServiceRequestMetadata>();
            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            messages = SystemMessageCollection.Empty();
            Guid thirdPartyId = CombGuid.Instance.Generate();
            DateTime date = DateTime.Now;
            invoiceLineItems = new List<InvoiceLineItemModel> {
                new InvoiceLineItemModel(null, 1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true)
            };
            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, thirdPartyId, "DD0011", date, invoiceLineItems, true, string.Empty);
            command = new CaptureThirdPartyInvoiceCommand(thirdPartyInvoiceModel);
            commandHandler = new CaptureThirdPartyInvoiceCommandHandler(thirdPartyInvoiceDataManager, eventRaiser, serviceCommandRouter, unitOfWorkFactory, domainRuleManager);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand<AddThirdPartyInvoiceLineItemsCommand>(
                Param.IsAny<AddThirdPartyInvoiceLineItemsCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(SystemMessageCollection.Empty());
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

        private It should_return_a_third_party_invoice_has_already_been_captured_error_messages = () =>
        {
            messages.AllMessages.Any(x => x.Message.Equals(errorMessage.Message));
        };
    }
}