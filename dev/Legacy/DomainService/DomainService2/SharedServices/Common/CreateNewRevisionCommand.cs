namespace DomainService2.SharedServices.Common
{
    public class CreateNewRevisionCommand : StandardDomainServiceCommand
    {
        public CreateNewRevisionCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}