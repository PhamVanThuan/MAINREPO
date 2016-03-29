namespace DomainService2.Workflow.Origination.FurtherLending
{
    public class HighestPriorityCommand : StandardDomainServiceCommand
    {
        public HighestPriorityCommand(int applicationkey)
        {
            this.ApplicationKey = applicationkey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}