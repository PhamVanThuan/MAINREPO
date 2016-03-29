namespace DomainService2.SharedServices.Common
{
    public class IsComcorpApplicationCommand : StandardDomainServiceCommand
    {
        public int ApplicationKey { get; protected set; }

        public bool Result { get; set; }

        public IsComcorpApplicationCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }
    }
}