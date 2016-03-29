using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Events;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.StartInvoicePaymentProcess
{
    public class when_successful : WithCoreFakes
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
                     y.Clear();
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

        private It should_update_the_invoice_status = () =>
         {
             thirdPartyInvoiceDataManager.WasToldTo(x => x.UpdateThirdPartyInvoiceStatus(command.ThirdPartyInvoiceKey, InvoiceStatus.ProcessingPayment));
         };

        private It should_not_return_any_errors = () =>
         {
             messages.HasErrors.ShouldBeFalse();
         };

        private It should_raise_the_event = () =>
        {
            eventRaiser.Received().RaiseEvent(Arg.Any<DateTime>(), Arg.Any<ThirdPartyInvoicePaymentProcessedEvent>(), command.ThirdPartyInvoiceKey,
                (int)GenericKeyType.ThirdPartyInvoice, serviceRequestMetaData);
        };
    }
}