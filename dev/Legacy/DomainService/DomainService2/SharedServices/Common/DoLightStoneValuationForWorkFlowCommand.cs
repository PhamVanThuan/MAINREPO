namespace DomainService2.SharedServices.Common
{
    public class DoLightStoneValuationForWorkFlowCommand : StandardDomainServiceCommand
    {
        public DoLightStoneValuationForWorkFlowCommand(int applicationKey, string aduser)
        {
            this.ApplicationKey = applicationKey;
            this.ADUser = aduser;
        }

        public int ApplicationKey { get; set; }

        public string ADUser { get; set; }
    }
}