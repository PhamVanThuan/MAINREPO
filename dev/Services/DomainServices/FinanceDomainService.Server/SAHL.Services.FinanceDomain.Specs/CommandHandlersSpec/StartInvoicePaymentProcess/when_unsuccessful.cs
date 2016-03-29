using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.CATS;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Linq;
using SAHL.Core.Testing;
using NSubstitute;
using System;
using SAHL.Core.Events;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.StartInvoicePaymentProcess
{
    public class when_unsuccessful : WithCoreFakes
    {
        private static ProcessThirdPartyInvoicePaymentCommandHandler handler;
        private static ProcessThirdPartyInvoicePaymentCommand command;
        private static IDomainRuleManager<IThirdPartyInvoiceRuleModel> domainRuleManager;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;

        private Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<IThirdPartyInvoiceRuleModel>>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            command = new ProcessThirdPartyInvoicePaymentCommand(thirdPartyInvoiceKey: 43007);
            handler = new ProcessThirdPartyInvoicePaymentCommandHandler(thirdPartyInvoiceDataManager, domainRuleManager, eventRaiser);

            domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command))
                .Callback<ISystemMessageCollection>(y =>
                {
                    y.AddMessage(new SystemMessage("Invoice payment should be processing", SystemMessageSeverityEnum.Error));
                });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command));
        };

        private It should_not_update_the_invoice_status = () =>
        {
            thirdPartyInvoiceDataManager.WasNotToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.ProcessingPayment));
        };

        private It should_return_errors = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().First().Message.ShouldEqual("Invoice payment should be processing");
        };

        private It should_not_raise_the_event = () =>
        {
            eventRaiser.DidNotReceive().RaiseEvent(Arg.Any<DateTime>(), Arg.Any<IEvent>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<IServiceRequestMetadata>());
        };
    }
}