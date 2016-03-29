using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.NotifyBatchPaymentRecipients
{
    public class when_rules_fail : WithFakes
    {
        private static NotifyCATSPaymentBatchRecipientsCommandHandler handler;
        private static NotifyCATSPaymentBatchRecipientsCommand command;
        private static ICATSDataManager catsDataManager;
        private static ICATSManager catsManager;
        private static int batchKey;
        private static ISystemMessageCollection messages;
        private static IServiceRequestMetadata metadata;
        private static IEventRaiser eventRaiser;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IDomainRuleManager<CatsPaymentBatchRuleModel> domainRuleManager;

        private Establish context = () =>
         {
             batchKey = 96;
             catsDataManager = An<ICATSDataManager>();
             catsManager = An<ICATSManager>();
             eventRaiser = An<IEventRaiser>();
             serviceCommandRouter = An<IServiceCommandRouter>();
             serviceQueryRouter = An<IServiceQueryRouter>();
             metadata = An<IServiceRequestMetadata>();
             domainRuleManager = An<IDomainRuleManager<CatsPaymentBatchRuleModel>>();
             command = new NotifyCATSPaymentBatchRecipientsCommand(batchKey);
             handler = new NotifyCATSPaymentBatchRecipientsCommandHandler(catsDataManager, catsManager, serviceCommandRouter, serviceQueryRouter, eventRaiser, domainRuleManager);

             var errorMessages = SystemMessageCollection.Empty();
             errorMessages.AddMessage(new SystemMessage("An error occured", SystemMessageSeverityEnum.Error));
             domainRuleManager.WhenToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<CatsPaymentBatchRuleModel>()))
                 .Callback<ISystemMessageCollection>(m =>
                 {
                     m.AddMessages(errorMessages.AllMessages);
                 });
         };

        private Because of = () =>
         {
             messages = handler.HandleCommand(command, metadata);
         };

        private It should_register_the_rules = () =>
         {
             domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<CatsPaymentBatchRuleModel>()));
         };

        private It should_return_error_messages = () =>
         {
             messages.HasErrors.ShouldBeTrue();
         };
    }
}