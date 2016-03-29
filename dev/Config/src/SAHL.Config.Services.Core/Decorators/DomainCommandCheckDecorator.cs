using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    public class DomainCommandCheckDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private IServiceCommandHandler<TCommand> _innerHandler;
        private IIocContainer _iocContainer;

        public DomainCommandCheckDecorator(IServiceCommandHandler<TCommand> innerHandler, IIocContainer iocContainer)
        {
            _innerHandler = innerHandler;
            _iocContainer = iocContainer;
        }

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return _innerHandler; }
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();

            var allDomainChecksForCommand = this.GetAllDomainChecksForCommand(command);
            foreach (var currentDomainCheck in allDomainChecksForCommand)
            {
                var domainCommandCheckHandler = this.GetDomainCommandCheckHandler(currentDomainCheck);
                if (domainCommandCheckHandler == null) { continue; }

                var domainCommandCheckHandlerMethod = this.GetDomainCommandCheckHandlerMethod(currentDomainCheck, domainCommandCheckHandler);
                if (domainCommandCheckHandlerMethod == null) { continue; }

                try
                {
                    messages.Aggregate(domainCommandCheckHandlerMethod.Invoke(domainCommandCheckHandler, new[] { command }));
                }
                catch (Exception runtimeException)
                {
                    var systemMessage = new SystemMessage(runtimeException.ToString(), SystemMessageSeverityEnum.Exception);
                    messages.AddMessage(systemMessage);
                }
            }

            if (!messages.HasErrors)
            {
                messages.Aggregate(_innerHandler.HandleCommand(command, metadata));
            }

            return messages;
        }

        private MethodInfo GetDomainCommandCheckHandlerMethod(Type currentDomainCheck, dynamic domainCommandCheckHandler)
        {
            var allMethods = domainCommandCheckHandler.GetType().GetMethods();

            foreach (var currentMethod in allMethods)
            {
                if (currentMethod.Name != "HandleCheckCommand") { continue; }

                var allParameters = currentMethod.GetParameters();
                foreach (var currentParameter in allParameters)
                {
                    if (currentParameter.ParameterType.Name != currentDomainCheck.Name) { continue; }
                    return currentMethod;
                }
            }

            throw new Exception(string.Format("Unable to find a Handle method for Domain Service Check [{0}.{1}]",
                                              domainCommandCheckHandler.GetType().Name,
                                              currentDomainCheck.Name));
        }

        private IEnumerable<Type> GetAllDomainChecksForCommand(TCommand command)
        {
            var interfaces = command.GetType().GetInterfaces();
            return interfaces.Where(type => type.GetInterfaces().Any(subType => subType.Name.StartsWith("IDomainCommandCheck")));
        }

        private dynamic GetDomainCommandCheckHandler(Type commandCheckType)
        {
            var checkHandlerType = typeof(IDomainCommandCheckHandler<>);
            var genericCheckHandlerType = checkHandlerType.MakeGenericType(commandCheckType);

            var domainCommandCheckHandler = _iocContainer.GetInstance(genericCheckHandlerType, commandCheckType.Name);
            return domainCommandCheckHandler;
        }
    }
}