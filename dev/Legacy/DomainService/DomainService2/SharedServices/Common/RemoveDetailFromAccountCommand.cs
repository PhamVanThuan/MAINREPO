namespace DomainService2.SharedServices.Common
{
    public class RemoveDetailFromAccountCommand : StandardDomainServiceCommand
    {
        public RemoveDetailFromAccountCommand(int applicationKey, string detailTypeDescription)
        {
            this.ApplicationKey = applicationKey;
            this.DetailTypeDescription = detailTypeDescription;
        }

        public int ApplicationKey { get; set; }

        public string DetailTypeDescription { get; set; }
    }
}