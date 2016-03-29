using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Config.Core;
using SAHL.Core;
using SAHL.Core.Extensions;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs.Mocks;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.X2ServiceCommandRouterSpecs
{
    public class CommandHandlerConvention : IRegistrationConvention
    {
        public void Process(Type commandHandlerType, Registry registry)
        {
            if ((commandHandlerType.IsAssignableToGenericType(typeof(IServiceCommandHandler<>))) &&
                           !commandHandlerType.IsAssignableToGenericType(typeof(IServiceCommandHandlerDecorator<>)) &&
                           !commandHandlerType.IsAbstract)
            {
                Type type = commandHandlerType.GetInterface("IDontDecorateServiceCommand");

                var commandHandlerInterfaces = commandHandlerType.GetInterfaces().Where(x => x.IsAssignableToGenericType(typeof(IServiceCommandHandler<>)));
                foreach (var commandHandlerInterface in commandHandlerInterfaces)
                {
                    var commandType = commandHandlerInterface.GetGenericArguments().First();

                    var enrichWithMethod = typeof(CommandHandlerConvention).GetMethod("Register");
                    var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { commandType, commandHandlerType });
                    if (type == null)
                    {
                        genericEnrichWithMethod.Invoke(null, new object[] { registry });
                    }
                }
            }
        }

        public static void Register<CommandType, CommandHandler>(Registry registry)
            where CommandType : IServiceCommand
            where CommandHandler : IServiceCommandHandler<CommandType>
        {
            var registeredInstance = registry.For<IServiceCommandHandler<CommandType>>().Use<CommandHandler>();
        }
    }

    public class when_checking_valid_rule_command_with_correct_type : WithFakes
    {
        private static IServiceCommandHandlerProvider serviceCommandHandlerProvider;
        private static IX2ServiceCommandRouter commandRouter;
        private static IServiceCommandHandler<Mocks.MockRuleCommand> commandHandler;
        private static IServiceRequestMetadata metadata;
        private static Mocks.MockRuleCommand command;
        private static bool result;

        private Establish context = () =>
        {
            ObjectFactory.Configure(x =>
           {
               x.Scan(scan =>
               {
                   scan.TheCallingAssembly();
                   scan.WithDefaultConventions();

                   scan.Convention<CommandHandlerConvention>();
               });
               //x.For<IIocContainer>().Use<StructureMapIocContainer>();
           });

            var container = ObjectFactory.Container;

            serviceCommandHandlerProvider = NSubstitute.Substitute.For<IServiceCommandHandlerProvider>();

            commandRouter = new X2ServiceCommandRouter(serviceCommandHandlerProvider);
            command = new Mocks.MockRuleCommand();
            metadata = new ServiceRequestMetadata(new Dictionary<string, string>
                            {
                                { ServiceRequestMetadata.HEADER_USERNAME, "userName" }
                            });
        };

        private Because of = () =>
        {
            result = commandRouter.CheckRuleCommand(command, metadata);
        };

        private It should_get_command_handler = () =>
        {
            serviceCommandHandlerProvider.Received(1).GetCommandHandler<MockRuleCommand>();
            serviceCommandHandlerProvider.DidNotReceive().GetCommandHandler<CheckActivityIsValidForStateCommand>();
        };
    }
}