namespace DomainService2
{
    public class RuleDomainServiceCommandHandler<T> : IHandlesDomainServiceCommand<T> where T : RuleDomainServiceCommand
    {
        protected T command;
        private ICommandHandler commandHandler;
        private bool reverseRuleResult;
        private bool clearMessages;

        public RuleDomainServiceCommandHandler(ICommandHandler commandHandler)
            : this(commandHandler, false)
        {
        }

        public RuleDomainServiceCommandHandler(ICommandHandler commandHandler, bool reverseRuleResult)
            : this(commandHandler, reverseRuleResult, false)
        {
        }

        public RuleDomainServiceCommandHandler(ICommandHandler commandHandler, bool reverseRuleResult, bool clearMessages)
        {
            this.commandHandler = commandHandler;
            this.reverseRuleResult = reverseRuleResult;
            this.clearMessages = clearMessages;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, T command)
        {
            this.command = command;
            this.SetupRule();
            command.Result = this.reverseRuleResult ? !this.commandHandler.CheckRule<T>(messages, command) : this.commandHandler.CheckRule<T>(messages, command);
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