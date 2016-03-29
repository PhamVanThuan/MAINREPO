namespace DomainService2.Specs.TransactionalCommandHandlerSpecs
{
    public class DummyCommand : IDomainServiceCommand
    {
        private bool ignoreWarnings;

        public DummyCommand(bool ignoreWarnings)
        {
            this.ignoreWarnings = ignoreWarnings;
        }

        public bool IgnoreWarnings
        {
            get { return this.ignoreWarnings; }
        }
    }
}