using SAHL.Batch.Common;
using SAHL.Batch.Common.MessageForwarding;
using SAHL.Batch.Common.Messages;
using SAHL.Batch.Common.ServiceContracts;
using SAHL.Batch.Service.CapitecService;
using SAHL.Batch.Service.MessageProcessors;
using SAHL.Batch.Service.Repository;
using SAHL.Batch.Service.Services;
using SAHL.Common.Logging;
using SAHL.Communication;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Services.Capitec.Models.Shared;
using SAHL.Services.Capitec.Models.Shared.Capitec;
using SAHL.Shared.Messages.PersonalLoanLead;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace SAHL.Batch.Service.Registries
{
    public class SAHLBatchService : Registry
    {
        public SAHLBatchService()
        {
            ConfigureCommon();
            ConfigurePersonalLoanLead();
            ConfigureMessageForwarding();
            ConfigureCreateCapitecApplication();
        }

        private void ConfigureCommon()
        {
            For<IRepository>().Singleton().Use<BatchServiceRepository>();
            For<IBatchServiceManager>().Singleton().Use<BatchServiceManager>();
            For<ICancellationNotifier>().Singleton().Use<CancellationNotifier>();
            For<IQueueHandlerService>().Singleton().Use<QueueHandlerService>();
            For<IMessageBusConfigurationProvider>().Use<EasyNetQMessageBusConfigurationProvider>();
            For<IDiposableMessageBus>().Singleton().Use<SAHL.Batch.Service.MessageBus.EasyNetQMessageBus>();
            For<IMessageBus>().Use(context => context.GetInstance<IDiposableMessageBus>());
            For<IForwardingMessageBus>().Use(context => context.GetInstance<ForwardingMessageBus>());
            For<ITimer>().Singleton().Use<Timer>();
            For<IBatchServiceConfiguration>().Singleton().Use<BatchServiceConfiguration>();
            For<IMetrics>().Singleton().Use<MessageBusMetrics>();
            For<ILogger>().Singleton().Use<MessageBusLogger>();
        }


        private void ConfigurePersonalLoanLead()
        {
            For<IPersonalLoanLeadCreationService>().Singleton().Use(ObjectFactory.GetInstance<PersonalLoanLeadCreation>());
            For<IMessageProcessor<PersonalLoanLeadMessage>>().Singleton().Use<PersonalLoanLeadProcessor>();
            For<IMessageRetryService<PersonalLoanLeadMessage>>().Singleton().Use<DefaultMessageRetryService<PersonalLoanLeadMessage>>();
            For<IQueuedHandler<PersonalLoanLeadMessage>>().Singleton().Use<MessageQueueHandler<PersonalLoanLeadMessage>>();
            For<IStartableQueueHandler>().Use((context) => context.GetInstance<IQueuedHandler<PersonalLoanLeadMessage>>());
            For<IStoppableQueueHandler>().Use((context) => context.GetInstance<IQueuedHandler<PersonalLoanLeadMessage>>());
        }

        private void ConfigureMessageForwarding()
        {
            For<IForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage>>().Singleton().Use<MessageForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage>>().OnCreation((context, messageforwarder) =>
            {
                messageforwarder.Transform((message) =>
                {
                    return new CapitecApplicationMessage((dynamic)message.CapitecApplication);
                });
            });

            For<IStartableQueueHandler>().Use((context) => context.GetInstance<IForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage>>());
            For<IStoppableQueueHandler>().Use((context) => context.GetInstance<IForwardingQueuedHandler<CreateCapitecApplicationRequest, CapitecApplicationMessage>>());
        }

        private void ConfigureCreateCapitecApplication()
        {
            For<ICapitec>().AlwaysUnique().Use(() => { return new CapitecClient(); });
            For<ICapitecClientService>().Singleton().Use<CapitecWebServiceClient>();
            For<IMessageProcessor<CapitecApplicationMessage>>().Singleton().Use<CreateCapitecApplicationProcessor>();
            For<IMessageRetryService<CapitecApplicationMessage>>().Singleton().Use<MessageRetryService<CapitecApplicationMessage>>();
            For<IQueuedHandler<CapitecApplicationMessage>>().Singleton().Use<MessageQueueHandler<CapitecApplicationMessage>>();
            For<IStartableQueueHandler>().Use((context) => context.GetInstance<IQueuedHandler<CapitecApplicationMessage>>());
            For<IStoppableQueueHandler>().Use((context) => context.GetInstance<IQueuedHandler<CapitecApplicationMessage>>());
        }
    }
}