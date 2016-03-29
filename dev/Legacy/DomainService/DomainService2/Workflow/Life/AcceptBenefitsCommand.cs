namespace DomainService2.Workflow.Life
{
    public class AcceptBenefitsCommand : StandardDomainServiceCommand
    {
        public AcceptBenefitsCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}