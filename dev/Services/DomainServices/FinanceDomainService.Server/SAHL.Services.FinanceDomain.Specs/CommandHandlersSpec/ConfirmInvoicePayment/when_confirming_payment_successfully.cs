using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ConfirmInvoicePayment
{
    public class when_confirming_payment_successfully : WithCoreFakes
    {
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static MarkThirdPartyInvoiceAsPaidCommandHandler commandHandler;
        private static MarkThirdPartyInvoiceAsPaidCommand command;
        private static IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private static int thirdPartyInvoiceKey;

        private Establish context = () =>
        {
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            domainRuleManager = An<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>();
            thirdPartyInvoiceKey = 78342;
            command = new MarkThirdPartyInvoiceAsPaidCommand(thirdPartyInvoiceKey);
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            commandHandler = new MarkThirdPartyInvoiceAsPaidCommandHandler(thirdPartyInvoiceDataManager, domainRuleManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_ensure_invoice_payment_is_processing = () =>
        {
            domainRuleManager.WasToldTo(y => y.ExecuteRules(
                 Param.IsAny<ISystemMessageCollection>()
               , Param<IThirdPartyInvoiceRuleModel>.Matches(m => m.ThirdPartyInvoiceKey == thirdPartyInvoiceKey)
            ));
        };

        private It should_change_invoice_status_to_paid = () =>
        {
            thirdPartyInvoiceDataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.Paid));
        };

        private It should_raise_the_event = () =>
        {
            eventRaiser.Received().RaiseEvent(Arg.Any<DateTime>(), Arg.Any<ThirdPartyInvoiceMarkedAsPaidEvent>(), command.ThirdPartyInvoiceKey,
                (int)GenericKeyType.ThirdPartyInvoice, serviceRequestMetaData);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}