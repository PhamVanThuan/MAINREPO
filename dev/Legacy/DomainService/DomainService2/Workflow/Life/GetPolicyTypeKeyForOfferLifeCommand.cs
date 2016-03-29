namespace DomainService2.Workflow.Life
{
    public class GetPolicyTypeKeyForOfferLifeCommand : StandardDomainServiceCommand
    {
        public GetPolicyTypeKeyForOfferLifeCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }

        public int PolicyTypeKeyResult { get; set; }
    }
}