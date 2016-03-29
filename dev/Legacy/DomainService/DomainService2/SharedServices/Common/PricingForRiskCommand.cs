namespace DomainService2.SharedServices.Common
{
    public class PricingForRiskCommand : StandardDomainServiceCommand
    {
        public PricingForRiskCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }
    }
}