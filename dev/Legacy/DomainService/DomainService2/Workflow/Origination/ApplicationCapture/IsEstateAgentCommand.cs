namespace DomainService2.Workflow.Origination.ApplicationCapture
{
    public class IsEstateAgentCommand : StandardDomainServiceCommand
    {
        public string CreatorADUserName { get; set; }

        public IsEstateAgentCommand(string creatorADUserName)
        {
            this.CreatorADUserName = creatorADUserName;
        }

        public bool Result { get; set; }
    }
}