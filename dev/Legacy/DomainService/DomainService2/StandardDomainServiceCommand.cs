namespace DomainService2
{
    public abstract class StandardDomainServiceCommand : DomainServiceCommand, IStandardDomainServiceCommand
    {
        public StandardDomainServiceCommand()
            : base(false)
        {
        }

        public StandardDomainServiceCommand(bool ignoreWarnings)
            : base(ignoreWarnings)
        {
        }
    }
}