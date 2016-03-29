namespace DomainService2.SharedServices.Common
{
    public class RefreshLookupItemCommand : StandardDomainServiceCommand
    {
        public RefreshLookupItemCommand(object data)
        {
            this.Data = data;
        }

        public object Data
        {
            get;
            protected set;
        }
    }
}