namespace DomainService2.Workflow.Cap2
{
    public class IsLANotRequiredCommand : StandardDomainServiceCommand
    {
        public IsLANotRequiredCommand(int capApplicationKey)
        {
            this.CapApplicationKey = capApplicationKey;
        }

        public int CapApplicationKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}