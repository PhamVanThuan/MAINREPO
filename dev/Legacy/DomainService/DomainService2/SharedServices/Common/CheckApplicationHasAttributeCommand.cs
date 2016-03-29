namespace DomainService2.SharedServices.Common
{
    public class CheckApplicationHasAttributeCommand : StandardDomainServiceCommand
    {
        public CheckApplicationHasAttributeCommand(int applicationKey, int applicationAttributeTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.ApplicationAttributeTypeKey = applicationAttributeTypeKey;
        }

        public int ApplicationKey { get; set; }

        public int ApplicationAttributeTypeKey { get; set; }

        public bool Result { get; set; }
    }
}