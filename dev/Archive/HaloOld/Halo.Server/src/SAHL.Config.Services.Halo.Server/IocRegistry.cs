using SAHL.Core.Caching;
using SAHL.Core.IoC;
using SAHL.Core.Messaging;
using SAHL.Core.Messaging.MessageDistributors;
using SAHL.Core.Messaging.MessageProcessors;
using SAHL.Core.Messaging.MessageQueues;
using SAHL.Core.Messaging.MessageServices;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.UI.ApplicationState.Managers;
using SAHL.Core.UI.Configuration;
using SAHL.Core.UI.Models;
using SAHL.Core.UI.Modules;
using SAHL.Core.UI.Providers.Tiles;
using SAHL.Core.UI.UserState.Managers;
using SAHL.Core.Web.Caching;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Linq;

namespace SAHL.Config.Services.Halo
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));

                // caching
                x.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));

                // menu config
                x.ConnectImplementationsToTypesClosing(typeof(IMenuItemConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IRibbonItemConfiguration<>));

                x.Convention<ApplicationModuleConvention>();

                // tile config
                x.Convention<TileModelConvention>();
                x.Convention<AlternateTileModelConvention>();
                x.Convention<ParentedEditorPageConfigurationConvention>();

                x.ConnectImplementationsToTypesClosing(typeof(IRootTileConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IMajorTileConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IParentedTileConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IParentedActionTileConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IDrillDownTileConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(ITileDataProvider<>));
                x.ConnectImplementationsToTypesClosing(typeof(ITileContentProvider<>));

                x.ConnectImplementationsToTypesClosing(typeof(IEditorConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IParentedEditorConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IParentedEditorPageConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IEditorPageSelectorConfiguration<>));
                x.ConnectImplementationsToTypesClosing(typeof(IEditorPageModelValidator<>));
                x.Convention<CommandMessageProcessorConvention>();

                x.WithDefaultConventions();
            });

            For<ICache>().Use<ApplicationCache>().Named("ApplicationCache");
            For<ICache>().Singleton().Use<InMemoryCache>().Named("SessionCache");

            For<IApplicationStateManager>().Singleton().Use<ApplicationStateManager>().Ctor<ICache>().Is(x => x.GetInstance<ICache>("ApplicationCache"));
            For<IStartable>().Singleton().Use(x => x.GetInstance<IApplicationStateManager>());

            For<IUserStateManager>().Use<UserStateManager>().Ctor<ICache>().Is(x => x.GetInstance<ICache>("SessionCache"));
            For<ICacheKeyGenerator>().Use<CacheKeyGenerator>();
            For<IHashGenerator>().Use<DefaultHashGenerator>();

            For<IServiceCommandHandlerProvider>().Use<StructureMapServiceHandlerProvider>();

            For<IServiceCommandRouter>().Use<ServiceCommandRouter>();
            For<IMessageProcessor<IServiceCommand>>().Use<CommandMessageProcessor>();

            For<IMessageQueue>().Singleton().Use<MessageQueue>().Named("CommandQueue");
            For<ITrackingMessageDistributor<IServiceCommand>>().Singleton().Use<TrackingMessageDistributor<IServiceCommand>>().Named("CommandDistributor");
            For<IMessageDistributorSettings>().Singleton().Use<AsyncBatchMessageDistributorSettings>();
            For<IMessageProcessorProvider>().Singleton().Use<MessageProcessorProvider>();
            For<IMessageProcessorRouter<QueuedMessage, ISystemMessageCollection>>().Singleton().Use<MessageProcessorRouter>();
            For<ICommandService>().Singleton().Use<CommandService>().Ctor<ITrackingMessageDistributor<IServiceCommand>>().Is(x => x.GetInstance<ITrackingMessageDistributor<IServiceCommand>>("CommandDistributor"));
            For<IStartableService>().Use((context) => context.GetInstance<ICommandService>());
            For<IStoppableService>().Use((context) => context.GetInstance<ICommandService>());
        }
    }

    public class CommandMessageProcessorConvention : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IServiceCommand)) && type.IsAbstract == false)
            {
                Type openGenericMessageProcessorType = typeof(IMessageProcessor<>);
                Type genericMessageProcessorType = openGenericMessageProcessorType.MakeGenericType(type);

                //registry.For<IMessageProcessor<TestCommand>>().Use<CommandMessageProcessor>();

                registry.For(genericMessageProcessorType).Use(typeof(CommandMessageProcessor));
            }
        }
    }

    public class ApplicationModuleConvention : IRegistrationConvention
    {
        public void Process(System.Type type, Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(IApplicationModule)) && type.IsAbstract == false)
            {
                registry.For(typeof(IApplicationModule)).Singleton().Use(type);
            }
        }
    }

    public class TileModelConvention : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(ITileModel)) && type.IsAbstract == false && !type.GetInterfaces().Contains(typeof(IAlternateTileModel)))
            {
                registry.For(typeof(ITileModel)).Use(type);
            }
        }
    }

    public class AlternateTileModelConvention : IRegistrationConvention
    {
        public void Process(System.Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.GetInterfaces().Contains(typeof(ITileModel)) && type.IsAbstract == false && type.GetInterfaces().Contains(typeof(IAlternateTileModel)))
            {
                var genericIAlternateType = type.GetInterfaces().Where(x => x.Name.StartsWith("IAlternateTileModel") && x.IsGenericType == true).SingleOrDefault();
                string fullNamespace = type.Namespace;
                string[] splitNameSpace = fullNamespace.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
                if (splitNameSpace.Length > 2)
                {
                    registry.For(genericIAlternateType).Use(type).Named(string.Format("{0}.{1}", splitNameSpace[splitNameSpace.Length - 2], splitNameSpace[splitNameSpace.Length - 1]));
                }
            }
        }
    }

    public class ParentedEditorPageConfigurationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            Type parentedEditorPageConfig = type.GetInterfaces().Where(x => x.Name.StartsWith("IParentedEditorPageConfiguration")).SingleOrDefault();
            if (parentedEditorPageConfig != null)
            {
                Type genericParam = parentedEditorPageConfig.GenericTypeArguments[0];
                Type editorConfig = genericParam.GetInterfaces().Where(x => x.Name.StartsWith("IEditorConfiguration") && x.IsGenericType == true).SingleOrDefault();
                if (editorConfig != null)
                {
                    Type editorConfigType = editorConfig.GenericTypeArguments[0];
                    Type openGenericEditorConfig = typeof(IEditorConfiguration<>);
                    Type genericEditorConfig = openGenericEditorConfig.MakeGenericType(editorConfigType);
                    Type openGenericParentedConfig = typeof(IParentedEditorPageConfiguration<>);
                    Type genericParentedConfig = openGenericParentedConfig.MakeGenericType(genericEditorConfig);

                    registry.For(genericParentedConfig).Use(type);
                }
            }
        }
    }
}