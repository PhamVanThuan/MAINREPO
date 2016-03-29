namespace DomainService2.SharedServices.Common
{
    public class GetApplicationStatusCommand : StandardDomainServiceCommand
    {
        public GetApplicationStatusCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public int ApplicationStatusKeyResult { get; set; }
    }
}