using System.Diagnostics;
using System.Transactions;
using SAHL.Core.Services;
using SAHL.Core.Services.Attributes;
using SAHL.Core.SystemMessages;

namespace SAHL.Config.Services.Core.Decorators
{
    [DebuggerNonUserCode]
    [DecorationOrder(0)]
    public class TransactionCommandHandlerDecorator<TCommand> : IServiceCommandHandlerDecorator<TCommand> where TCommand : IServiceCommand
    {
        private IServiceCommandHandler<TCommand> innerHandler;

        public IServiceCommandHandler<TCommand> InnerCommandHandler
        {
            get { return this.innerHandler; }
        }

        public TransactionCommandHandlerDecorator(IServiceCommandHandler<TCommand> innerHandler)
        {
            this.innerHandler = innerHandler;
        }

        public ISystemMessageCollection HandleCommand(TCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                messages.Aggregate(innerHandler.HandleCommand(command, metadata));
                transactionScope.Complete();
            }
            return messages;
        }
    }
}