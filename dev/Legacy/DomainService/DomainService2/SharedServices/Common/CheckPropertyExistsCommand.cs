namespace DomainService2.SharedServices.Common
{
    public class CheckPropertyExistsCommand : StandardDomainServiceCommand
    {
        public CheckPropertyExistsCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; set; }

        public bool Result { get; set; }
    }
}