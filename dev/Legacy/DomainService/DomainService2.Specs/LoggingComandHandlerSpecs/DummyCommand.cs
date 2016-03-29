namespace DomainService2.Specs.LoggingCommandHandlerSpecs
{
    public class DummyCommand : StandardDomainServiceCommand
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