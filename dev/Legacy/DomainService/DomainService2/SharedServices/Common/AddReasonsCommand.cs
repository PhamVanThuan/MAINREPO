namespace DomainService2.SharedServices.Common
{
    public class AddReasonsCommand : StandardDomainServiceCommand
    {
        public AddReasonsCommand(int genericKey, int reasonDescriptionKey, int reasonTypeKey)
        {
            this.GenericKey = genericKey;
            this.ReasonDescriptionKey = reasonDescriptionKey;
            this.ReasonTypeKey = reasonTypeKey;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public int ReasonDescriptionKey
        {
            get;
            protected set;
        }

        public int ReasonTypeKey
        {
            get;
            protected set;
        }
    }
}