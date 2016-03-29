namespace DomainService2.Hosts
{
    public abstract class HostBase : IDomainHost
    {
        private ICommandHandler commandHandler;

        public HostBase(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        protected ICommandHandler CommandHandler
        {
            get
            {
                return this.commandHandler;
            }
        }
    }
}