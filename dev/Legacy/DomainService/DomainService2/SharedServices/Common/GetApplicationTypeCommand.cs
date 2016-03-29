namespace DomainService2.SharedServices.Common
{
    public class GetApplicationTypeCommand : StandardDomainServiceCommand
    {
        public GetApplicationTypeCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public int ApplicationTypeKeyResult { get; set; }
    }
}