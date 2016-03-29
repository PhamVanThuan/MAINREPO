namespace DomainService2
{
    public abstract class DomainServiceCommand : IDomainServiceCommand
    {
        public DomainServiceCommand()
            : this(false)
        {
        }

        public DomainServiceCommand(bool ignoreWarnings)
        {
            this.IgnoreWarnings = ignoreWarnings;
        }

        public bool IgnoreWarnings
        {
            get;
            protected set;
        }
    }
}