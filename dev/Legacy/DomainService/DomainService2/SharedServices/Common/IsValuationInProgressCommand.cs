namespace DomainService2.SharedServices.Common
{
    public class IsValuationInProgressCommand : StandardDomainServiceCommand
    {
        public IsValuationInProgressCommand(long instanceID, int genericKey)
        {
            this.InstanceID = instanceID;
            this.GenericKey = genericKey;
        }

        public long InstanceID
        {
            get;
            protected set;
        }

        public int GenericKey
        {
            get;
            protected set;
        }

        public bool Result { get; set; }
    }
}