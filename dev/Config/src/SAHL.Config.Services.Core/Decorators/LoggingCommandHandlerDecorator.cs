using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DebuggerNonUserCode]
    [DecorationOrder(1)]
    public class LoggingCommandHandlerDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private readonly ILogger logger;
        private readonly IServiceCommandHandler<TCommand> innerHandler;
        private readonly ILoggerSource loggerSource;

        public LoggingCommandHandlerDecorator(IServiceCommandHandler<TCommand> innerHandler, ILogger logger, IIocContainer container, ILoggerSource loggerSource)
        {
            this.loggerSource = loggerSource;
            this.innerHandler = innerHandler;
            this.logger = logger;

            var namedLoggerSourceInstance = container.GetInstance<ILoggerSource>("CommandHandlerLogSource");
            if (namedLoggerSourceInstance != null)
            {
                this.loggerSource = namedLoggerSourceInstance;
            }
        }

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return this.innerHandler; }
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();

            try
            {
                messages.Aggregate(innerHandler.HandleCommand(command, metadata));
            }
            catch (Exception ex)
            {
                this.LogException(command, metadata, ex);
                throw;
            }

            return messages;
        }

        private void LogException(TCommand command, IServiceRequestMetadata metadata, Exception exception)
        {
            var parameters = new Dictionary<string, object>();
            var userName = (metadata == null ? "LoggingCommandHandlerDecorator" : metadata.UserName);

            parameters["CommandMsgId"] = command.Id;
            parameters["CommandObject"] = command;

            logger.LogErrorWithException(this.loggerSource, userName,
                                         this.GetInnerCommandHandlerType(innerHandler),
                                         exception.Message, exception, parameters);
        }

        private string GetInnerCommandHandlerType(IServiceCommandHandler<TCommand> handler)
        {
            if (handler is IServiceCommandHandlerDecorator<TCommand>)
            {
                return this.GetInnerCommandHandlerType((handler as IServiceCommandHandlerDecorator<TCommand>).InnerCommandHandler);
            }
            return handler.GetType().FullName;
        }
    }
}