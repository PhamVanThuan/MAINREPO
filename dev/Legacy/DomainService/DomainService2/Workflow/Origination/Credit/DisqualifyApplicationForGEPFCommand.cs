namespace DomainService2.Workflow.Origination.Credit
{
    public class DisqualifyApplicationForGEPFCommand : StandardDomainServiceCommand
    {
        public DisqualifyApplicationForGEPFCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}