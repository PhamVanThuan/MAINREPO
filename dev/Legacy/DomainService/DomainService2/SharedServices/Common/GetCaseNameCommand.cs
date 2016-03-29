namespace DomainService2.SharedServices.Common
{
    public class GetCaseNameCommand : StandardDomainServiceCommand
    {
        public GetCaseNameCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }

        public string CaseNameResult
        {
            get;
            set;
        }
    }
}