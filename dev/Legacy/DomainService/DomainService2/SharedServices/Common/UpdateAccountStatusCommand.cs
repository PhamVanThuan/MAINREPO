namespace DomainService2.SharedServices.Common
{
    public class UpdateAccountStatusCommand : StandardDomainServiceCommand
    {
        public UpdateAccountStatusCommand(int applicationKey, int accountStatusKey)
        {
            this.ApplicationKey = applicationKey;
            this.AccountStatusKey = accountStatusKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public int AccountStatusKey
        {
            get;
            protected set;
        }
    }
}