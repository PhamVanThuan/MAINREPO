using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Rules;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec.NotifyBatchPaymentRecipients
{
    public class when_instatiating_the_handler : WithFakes
    {
        private static NotifyCATSPaymentBatchRecipientsCommandHandler handler;
        private static NotifyCATSPaymentBatchRecipientsCommand command;
        private static ICATSDataManager catsDataManager;
        private static ICATSManager catsHelper;
        private static int batchKey;
        private static IServiceRequestMetadata metadata;
        private static IEventRaiser eventRaiser;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IDomainRuleManager<CatsPaymentBatchRuleModel> domainRuleManager;

        private Establish context = () =>
         {
             batchKey = 96;
             catsDataManager = An<ICATSDataManager>();
             catsHelper = An<ICATSManager>();
             eventRaiser = An<IEventRaiser>();
             serviceCommandRouter = An<IServiceCommandRouter>();
             serviceQueryRouter = An<IServiceQueryRouter>();
             metadata = An<IServiceRequestMetadata>();
             domainRuleManager = An<IDomainRuleManager<CatsPaymentBatchRuleModel>>();
             command = new NotifyCATSPaymentBatchRecipientsCommand(batchKey);
         };

        private Because of = () =>
         {
             handler = new NotifyCATSPaymentBatchRecipientsCommandHandler(catsDataManager, catsHelper, serviceCommandRouter, serviceQueryRouter, eventRaiser, domainRuleManager);
         };

        private It should_register_the_rules = () =>
         {
             domainRuleManager.WasToldTo(x => x.RegisterRule(Param.IsAny<BatchShouldBeInProcessedStateRule>()));
         };
    }
}