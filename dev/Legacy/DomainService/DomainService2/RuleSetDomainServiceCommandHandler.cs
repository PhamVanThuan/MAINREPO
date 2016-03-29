namespace DomainService2
{
    public class RuleSetDomainServiceCommandHandler<T> : IHandlesDomainServiceCommand<T> where T : RuleSetDomainServiceCommand
    {
        protected T command;
        private ICommandHandler commandHandler;
        private bool reverseRuleResult;
        private bool clearMessages;

        public RuleSetDomainServiceCommandHandler(ICommandHandler commandHandler)
            : this(commandHandler, false)
        {
        }

        public RuleSetDomainServiceCommandHandler(ICommandHandler commandHandler, bool reverseRuleResult)
            : this(commandHandler, reverseRuleResult, false)
        {
        }

        public RuleSetDomainServiceCommandHandler(ICommandHandler commandHandler, bool reverseRuleResult, bool clearMessages)
        {
            this.commandHandler = commandHandler;
            this.reverseRuleResult = reverseRuleResult;
            this.clearMessages = clearMessages;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command)
        {
            this.command = command;
            this.SetupRule();
            command.Result = this.reverseRuleResult ? !this.commandHandler.CheckRules<T>(messages, command) : this.commandHandler.CheckRules<T>(messages, command);
            if (clearMessages)
            {
                messages.Clear();
            }
        }

        public virtual void SetupRule()
        {
        }
    }
}