using System;

using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    public class CommandRuleHandlerDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private readonly IServiceCommandHandler<TCommand> innerHandler;
        private readonly IIocContainer iocContainer;

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return this.innerHandler; }
        }

        public CommandRuleHandlerDecorator(IServiceCommandHandler<TCommand> innerHandler, IIocContainer iocContainer)
        {
            this.innerHandler = innerHandler;
            this.iocContainer = iocContainer;
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();

            // get any rules for the query
            var rules = this.iocContainer.GetAllInstances<IServiceCommandRule<TCommand>>();
            foreach (var rule in rules)
            {
                try
                {
                    messages.Aggregate(rule.ExecuteRule(command));
                }
                catch (Exception)
                {
                    messages.AddMessage(new SystemMessage(string.Format("There was a system error running the a rule [{0}], processing has been halted",
                                        rule.GetType().FullName), SystemMessageSeverityEnum.Error));
                }
            }

            if (!messages.HasErrors)
            {
                messages.Aggregate(innerHandler.HandleCommand(command, metadata));
            }

            return messages;
        }
    }
}