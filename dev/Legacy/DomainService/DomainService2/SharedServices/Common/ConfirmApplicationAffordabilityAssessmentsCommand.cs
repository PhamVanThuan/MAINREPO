namespace DomainService2.SharedServices.Common
{
    public class ConfirmApplicationAffordabilityAssessmentsCommand : StandardDomainServiceCommand
    {
        public ConfirmApplicationAffordabilityAssessmentsCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}