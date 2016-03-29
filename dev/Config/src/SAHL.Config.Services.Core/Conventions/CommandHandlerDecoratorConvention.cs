using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Config.Core.Decorators;
using SAHL.Core.Extensions;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SAHL.Config.Services.Core.Conventions
{
    public class CommandHandlerDecoratorConvention : IRegistrationConvention
    {
        private IEnumerable<Type> decorators;

        public CommandHandlerDecoratorConvention()
        {
            decorators = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("SAHL") || x.FullName.StartsWith("Capitec"))
                                      .SelectMany(t => t.GetTypes())
                                      .Where(t => t.IsClass &&
                                             t.IsAssignableToGenericType(typeof(IServiceCommandHandlerDecorator<>)))
                                      .OrderBy(t =>
                                      {
                                          var attribute = t.GetCustomAttributes(typeof(DecorationOrderAttribute), true).FirstOrDefault() as DecorationOrderAttribute;

                                          return attribute != null ? attribute.Index : 0;
                                      });
        }

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
                    var enrichWithMethod = typeof(CommandHandlerDecoratorConvention).GetMethod("EnrichWith");
                    var genericEnrichWithMethod = enrichWithMethod.MakeGenericMethod(new Type[] { commandType, commandHandlerType });
                    if (type == null)
                    {
                        genericEnrichWithMethod.Invoke(null, new object[] { registry, decorators });
                    }
                }
            }
        }

        public static void EnrichWith<CommandType, CommandHandler>(Registry registry, IEnumerable<Type> decorators)
            where CommandType : IServiceCommand
            where CommandHandler : IServiceCommandHandler<CommandType>
        {
            var registeredInstance = registry.For<IServiceCommandHandler<CommandType>>().Use<CommandHandler>();

            object previousDecorator = null;
            foreach (var decorator in decorators)
            {
                var genericDecoratorType = decorator.MakeGenericType(typeof(CommandType));
                if (previousDecorator == null)
                {
                    var method = typeof(StructureMapDecorator).GetMethod("Decorate");
                    var genericMethod = method.MakeGenericMethod(typeof(CommandHandler));
                    object decoratorHelper = genericMethod.Invoke(null, new object[] { registeredInstance });

                    var withMethod = decoratorHelper.GetType().GetMethod("With");
                    var genericWithMethod = withMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericWithMethod.Invoke(decoratorHelper, null);
                }
                else
                {
                    var andThenMethod = previousDecorator.GetType().GetMethod("AndThen");
                    var genericAndThenMethod = andThenMethod.MakeGenericMethod(genericDecoratorType);
                    previousDecorator = genericAndThenMethod.Invoke(previousDecorator, null);
                }
            }
        }
    }
}