namespace DomainService2.Workflow.Origination.Credit
{
    public class UpdateConditionsCommand : StandardDomainServiceCommand
    {
        public UpdateConditionsCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}