namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class Revert30YearTermApplicationToPreviousTermCommand : StandardDomainServiceCommand
    {
        public Revert30YearTermApplicationToPreviousTermCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}